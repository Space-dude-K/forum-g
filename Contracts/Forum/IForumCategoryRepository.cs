using Entities.Models.Forum;

namespace Contracts.Forum
{
    public interface IForumCategoryRepository
    {
        IEnumerable<ForumCategory> GetAllCategories(bool trackChanges);
        ForumCategory GetCategory(int categoryId, bool trackChanges);
        void CreateCategory(ForumCategory category);
        IEnumerable<ForumCategory> GetCategoriesByIds(IEnumerable<int> ids, bool trackChanges);
        void DeleteCategory(ForumCategory category);
    }
}