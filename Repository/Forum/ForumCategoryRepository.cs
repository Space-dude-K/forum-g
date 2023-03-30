using Entities;
using Contracts.Forum;
using Entities.Models.Forum;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Repository.Forum
{
    public class ForumCategoryRepository : RepositoryBase<ForumCategory, ForumContext>, IForumCategoryRepository
    {
        public ForumCategoryRepository(ForumContext forumContext) : base(forumContext)
        {
        }

        public void CreateCategory(ForumCategory category)
        {
            category.ForumUserId = 1;
            
            /*if(category.ForumBases.Any())
            {
                category.ForumBases.Select(f => f.ForumUserId = 1).ToList();
            }*/

            Create(category);
        }
        public void DeleteCategory(ForumCategory category)
        {
            Delete(category);
        }
        public async Task<IEnumerable<ForumCategory>> GetAllCategoriesAsync(bool trackChanges)
        {
            return await FindAll(trackChanges)
            .OrderBy(c => c.Name)
             .ToListAsync();
        }
        public async Task<ForumCategory> GetCategoryAsync(int categoryId, bool trackChanges)
        {
            return await FindByCondition(c => c.Id.Equals(categoryId), trackChanges)
            .SingleOrDefaultAsync();
        }
        public async Task<IEnumerable<ForumCategory>> GetCategoriesByIdsAsync(IEnumerable<int> ids, bool trackChanges)
        {
            return await FindByCondition(x => ids.Contains(x.Id), trackChanges).ToListAsync();
        }
    }
}