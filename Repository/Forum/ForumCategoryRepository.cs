using Entities;
using Contracts.Forum;
using Entities.Models.Forum;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Linq;

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
            
            if(category.ForumBases.Any())
            {
                category.ForumBases.Select(f => f.ForumUserId = 1).ToList();
            }

            Create(category);
        }

        public IEnumerable<ForumCategory> GetAllCategories(bool trackChanges)
        {
            return FindAll(trackChanges)
            .OrderBy(c => c.Name)
             .ToList();
        }

        public IEnumerable<ForumCategory> GetCategoriesByIds(IEnumerable<int> ids, bool trackChanges)
        {
            return FindByCondition(x => ids.Contains(x.Id), trackChanges).ToList();
        }

        public ForumCategory GetCategory(int categoryId, bool trackChanges)
        {
            return FindByCondition(c => c.Id.Equals(categoryId), trackChanges)
            .SingleOrDefault();
        }
    }
}