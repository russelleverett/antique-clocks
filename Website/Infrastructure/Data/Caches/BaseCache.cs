using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Website.Infrastructure.Data.Entities;
using Website.Infrastructure.Services;

namespace Website.Infrastructure.Data.Caches {
    public interface ICache<T> where T : class, IEntity {
        bool Initialized { get; }
        string LastError { get; }

        T GetById(int id);

        void Clear();
    }

    public interface IDatabaseCache<T> where T : class, IEntity {
        T Add(T item);
        T Update(int id, Action<T> updateAction);
        T UpdateItem(T item, Action<T> updateAction);
        void Remove(T item);
    }

    public abstract class BaseCache<T> : ICache<T> where T : class, IEntity {
        protected DateTime _lastUpdateTime = DateTime.Parse("03/01/2018");

        protected Dictionary<int, T> _cache;
        protected object _cacheLock = new object();
        protected string _lastError;
        protected int _threadSleep = 300000;
        protected bool _initialized;
        protected Random _random;
        protected const int ONE_MINUTE = 60000;
        protected const int FIVE_MINUTES = 300000;
        protected const int TEN_MINUTES = 600000;
        protected const int ALL_DAY = 86400000;

        public string LastError {
            get {
                try {
                    return _lastError;
                }
                finally { _lastError = null; }
            }
        }
        public bool Initialized { get { return _initialized; } }

        public BaseCache() {
            _cache = new Dictionary<int, T>();
        }

        protected virtual void Initialize() {
            _random = new Random(DateTime.UtcNow.Millisecond);
            Thread _updateThread = new Thread(async () => {
                do {
                    try {
                        await UpdateCache();
                        _lastUpdateTime = DateTime.Now;
                        _initialized = true;
                    }
                    catch (Exception exc) {
                        _lastError = exc.Message;
                    }
                    finally { Thread.Sleep(_threadSleep); }
                }
                while (true);
            });
            _updateThread.IsBackground = true;
            _updateThread.Start();
        }

        public T GetById(int id) {
            if (_cache.ContainsKey(id))
                return _cache[id];
            return GetFromSource(p => p.Id == id);
        }

        public virtual void Clear() {
            lock (_cacheLock) {
                _cache.Clear();
                _lastUpdateTime = DateTime.Parse("03/01/2018");
            }
        }

        protected abstract Task UpdateCache();

        protected virtual void IndexItem(T item) { }

        protected virtual string LoadFromFile() {
            return null;
        }

        protected abstract T GetFromSource(Func<T, bool> expression);
    }

    public abstract class DatabaseCache<T> : BaseCache<T>, IDatabaseCache<T> where T : class, IDatabaseEntity {
        protected readonly IDomainContext _context;

        public DatabaseCache(IDomainContext context) : base() {
            _context = context;
            Initialize();
        }

        public T Add(T item) {
            if (item == null)
                return null;

            lock (_cacheLock) {
                item.CreateDate = DateTime.UtcNow;
                _context.Add(item);
                _context.SaveChanges();

                _cache.Add(item.Id, item);
            }

            return item;
        }

        public T Update(int id, Action<T> updateAction) {
            var item = GetById(id);
            if (item != null) {
                updateAction(item);
                item.UpdateDate = DateTime.UtcNow;
                _context.SaveChanges();

                lock (_cacheLock) {
                    _cache[id] = item;
                }
            }
            return item;
        }

        public T UpdateItem(T item, Action<T> updateAction) {
            if (item == null)
                return null;

            lock (_cacheLock) {
                updateAction(item);
                item.UpdateDate = DateTime.UtcNow;
                _context.SaveChanges();

                _cache[item.Id] = item;
            }

            return item;
        }

        public void Remove(T item) {
            if (item == null)
                return;

            lock (_cacheLock) {
                _context.Delete<T>(item.Id);
                _context.SaveChanges();

                _cache.Remove(item.Id);
            }
        }

        protected override T GetFromSource(Func<T, bool> expression) {
            lock (_cacheLock) {
                var item = _context.Queriable<T>().FirstOrDefault(expression);
                if (item != null) {
                    if (!_cache.ContainsKey(item.Id))
                        _cache.Add(item.Id, item);
                    return item;
                }
                return null;
            }
        }

        protected override async Task UpdateCache() {
            await Task.Run(() => {
                lock (_cacheLock) {
                    var entities = _context.Queriable<T>().Where(p => (p.UpdateDate ?? DateTime.Now) > _lastUpdateTime).ToList();

                    foreach (var entity in entities) {
                        if (!_cache.ContainsKey(entity.Id)) {
                            _cache.Add(entity.Id, entity);
                        }
                        else _cache[entity.Id] = entity;
                        IndexItem(entity);
                    }
                }
            });
        }
    }
}
