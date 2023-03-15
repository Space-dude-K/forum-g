using Contracts;
using Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Repository
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected ForumContext _forumContext;
        public RepositoryBase(ForumContext repositoryContext)
        {
            _forumContext = repositoryContext;
        }

        public IQueryable<T> FindAll(bool trackChanges)
        {
            return !trackChanges ? _forumContext.Set<T>()
                .AsNoTracking() : _forumContext.Set<T>();
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges)
        {
            return !trackChanges ? _forumContext.Set<T>()
            .Where(expression).AsNoTracking() : _forumContext.Set<T>().Where(expression);
        }

        public void Create(T entity) => _forumContext.Set<T>().Add(entity);
        public void Update(T entity) => _forumContext.Set<T>().Update(entity);
        public void Delete(T entity) => _forumContext.Set<T>().Remove(entity);
    }
}