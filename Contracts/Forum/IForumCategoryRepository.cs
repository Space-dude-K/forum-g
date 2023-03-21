using Entities.Models.Forum;

namespace Contracts.Forum
{
    public interface IForumCategoryRepository
    {
        Task<IEnumerable<ForumCategory>> GetAllCategoriesAsync(bool trackChanges);
        Task<ForumCategory> GetCategoryAsync(int categoryId, bool trackChanges);
        void CreateCategory(ForumCategory category);
        Task<IEnumerable<ForumCategory>> GetCategoriesByIdsAsync(IEnumerable<int> ids, bool trackChanges);
        void DeleteCategory(ForumCategory category);
    }
}