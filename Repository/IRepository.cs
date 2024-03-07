using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MusicLove.Data.Repository
{
    public interface IRepository<T> where T : class
    {
        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null, string include = "", bool isTraked = false);
        public T Get(Expression<Func<T, bool>> filter = null, string include = "", bool isTraked = false);
        public void Add(T entity);
        public void Delete(T entity);
        public void DeleteRange(IEnumerable<T> entities);
        public bool Exists(Expression<Func<T, bool>> filter);
    }
}