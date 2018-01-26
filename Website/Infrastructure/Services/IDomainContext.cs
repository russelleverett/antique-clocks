﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Website.Infrastructure.Data.Entities;

namespace Website.Infrastructure.Services {
    public interface IDomainContext {
        IQueryable<User> Users { get; }
        IQueryable<Clock> Clocks { get; }
        IQueryable<Resource> Resources { get; }

        void Add<T>(T entity) where T : class, IEntity;
        void Delete<T>(int id) where T : class, IEntity;
        void Reload<T>(T entity) where T : class, IEntity;
        void SaveChanges();
    }

    public class DomainContext : DbContext, IDomainContext {
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Clock> Clocks { get; set; }
        public virtual DbSet<Resource> Resources { get; set; }

        public DomainContext(DbContextOptions options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            if (!optionsBuilder.IsConfigured) {

            }
        }

        public new void Add<T>(T entity) where T : class, IEntity {
            Set<T>().Add(entity);
        }
        public void Delete<T>(int id) where T : class, IEntity {
            var item = Set<T>().Find(id);
            if (item == null)
                throw new ArgumentOutOfRangeException("id");
            Set<T>().Remove(item);
        }
        public void Reload<T>(T entity) where T : class, IEntity {
            Entry<T>(entity).Reload();
        }
        void IDomainContext.SaveChanges() {
            SaveChanges();
        }

        IQueryable<User> IDomainContext.Users {
            get { return Users.AsQueryable(); }
        }

        IQueryable<Clock> IDomainContext.Clocks {
            get { return Clocks.AsQueryable(); }
        }

        IQueryable<Resource> IDomainContext.Resources {
            get { return Resources.AsQueryable(); }
        }
    }
}