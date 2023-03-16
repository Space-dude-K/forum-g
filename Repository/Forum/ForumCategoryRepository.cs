using Entities;
using Contracts.Forum;
using Entities.Models.Forum;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Repository.Forum
{
    public class ForumCategoryRepository : RepositoryBase<ForumCategory, ForumContext>, IForumCategoryRepository
    {
        public ForumCategoryRepository(ForumContext forumContext) : base(forumContext)
        {
        }

        public void CreateCategory(ForumCategory category)
        {
            Create(category);
        }

        public IEnumerable<ForumCategory> GetAllCategories(bool trackChanges)
        {
            return FindAll(trackChanges)
            .OrderBy(c => c.Name)
             .ToList();
        }
        public ForumCategory GetCategory(int categoryId, bool trackChanges)
        {
            return FindByCondition(c => c.Id.Equals(categoryId), trackChanges)
            .SingleOrDefault();
        }
    }
}