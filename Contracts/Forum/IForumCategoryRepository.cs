using Entities.Models.Forum;
using Entities.RequestFeatures;
using Entities.RequestFeatures.Forum;

namespace Interfaces.Forum
{
    public interface IForumCategoryRepository
    {
        Task<PagedList<ForumCategory>> GetAllCategoriesAsync(ForumCategoryParameters forumCategoryParameters, bool trackChanges);
        Task<ForumCategory> GetCategoryAsync(int categoryId, bool trackChanges);
        void CreateCategory(ForumCategory category);
        Task<IEnumerable<ForumCategory>> GetCategoriesByIdsAsync(IEnumerable<int> ids, bool trackChanges);
        void DeleteCategory(ForumCategory category);
    }
}