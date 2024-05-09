using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;
using MusicLove.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace MusicLove.Data.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext dbContext;
        internal DbSet<T> dbSet;

        public Repository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
            this.dbSet = dbContext.Set<T>();
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IQueryable<T>> include = null, bool isTracked = false)
        {
            return GetQuery(filter, include, isTracked).ToList();
        }

        public T Get(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IQueryable<T>> include = null, bool isTracked = false)
        {
            return GetQuery(filter, include, isTracked).FirstOrDefault();
        }

        private IQueryable<T> GetQuery(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IQueryable<T>> include, bool isTracked)
        {
            IQueryable<T> query = isTracked == true ? dbSet : dbSet.AsNoTracking();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (include != null)
            {
                query = include(query);
            }
            
            return query;
        }

        public void Add(T entity)
        {
            dbSet.Add(entity);
        }
        public void Delete(T entity)
        {
            dbSet.Remove(entity);
        }
        public void DeleteRange(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
        }

        public bool Exists(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = dbSet;
            return query.Where(filter).Any() == true;
        }
    }
}