using Contracts.Forum;
using Entities.Models.Forum;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Forum
{
    // public abstract class RepositoryBase<T, TContext> : IRepositoryBase<T> where T : class where TContext : DbContext
    // RepositoryBase<ForumCategory, ForumContext>, IForumCategoryRepository
    public abstract class ForumCoreRepository<T> : RepositoryBase<T, ForumContext>, IForumCore<T> where T : class
    {
        protected ForumCoreRepository(ForumContext repositoryContext) : base(repositoryContext)
        {
        }

        public void Create(T category)
        {
            throw new NotImplementedException();
        }

        public void Delete(T category)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetAllAsync(bool trackChanges)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetAsync(int categoryId, bool trackChanges)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetByIdsAsync(IEnumerable<int> ids, bool trackChanges)
        {
            throw new NotImplementedException();
        }
    }
}
