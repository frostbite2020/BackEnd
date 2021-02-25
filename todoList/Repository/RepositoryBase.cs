using Contracts;
using System;
using System.Linq;
using System.Linq.Expressions;
using Entities.Context;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected ApplicationDBContext ApplicationDBContext { get; set; }

        public RepositoryBase(ApplicationDBContext applicationDBContext)
        {
            this.ApplicationDBContext = applicationDBContext;
        }
        public IQueryable<T> FindAll()
        {
            return this.ApplicationDBContext.Set<T>().AsNoTracking();
        }
        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return this.ApplicationDBContext.Set<T>().Where(expression).AsNoTracking();
        }

        public void Create(T entity)
        {
            this.ApplicationDBContext.Set<T>().Add(entity);
        }
        public void Update(T entity)
        {
            this.ApplicationDBContext.Set<T>().Update(entity);
        }
        public void Delete(T entity)
        {
            this.ApplicationDBContext.Set<T>().Remove(entity);
        }
    }
}
