//using System;
//using System.Collections.Generic;
//using Website.Infrastructure.Data.Entities;
//using Website.Infrastructure.Services;

//namespace Website.Infrastructure.Data.Caches {
//    public interface IClockCache : ICache<Clock>, IDatabaseCache<Clock> {
//        IEnumerable<Clock> GetAll();
//        IEnumerable<Clock> GetActiveClocks();
//        IEnumerable<Clock> GetFeaturedClocks();
//        Clock GetByNumber(string number);

//    }

//    public class ClockCache : DatabaseCache<Clock>, IClockCache {
//        public ClockCache(IDomainContext context) : base(context) {
//        }

//        public IEnumerable<Clock> GetAll() {
//            return new List<Clock>(_cache.Values);
//        }

//        public IEnumerable<Clock> GetActiveClocks() {
//            List<Clock> clocks = new List<Clock>();

//            foreach (var clock in _cache.Values) {
//                if (clock.Active) {
//                    clocks.Add(clock);
//                }
//            }

//            return clocks;
//        }

//        public IEnumerable<Clock> GetFeaturedClocks() {
//            List<Clock> clocks = new List<Clock>();

//            foreach (var clock in _cache.Values) {
//                if (clock.Featured) {
//                    clocks.Add(clock);
//                }
//            }

//            return clocks;
//        }

//        public Clock GetByNumber(string number) {
//            foreach (var clock in _cache.Values) {
//                if (clock.Number.Equals(number)) {
//                    return clock;
//                }
//            }
//            return null;
//        }
//    }
//}
