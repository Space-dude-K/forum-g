using Entities.Models.Forum;
using Entities.RequestFeatures;
using Entities.RequestFeatures.Forum;

namespace Contracts.Forum
{
    public interface IForumBaseRepository
    {
        Task<PagedList<ForumBase>> GetAllForumsFromCategoryAsync(int? categoryId, ForumBaseParameters forumBaseParameters, bool trackChanges);
        void CreateForumForCategory(int categoryId, ForumBase forum);
        void DeleteForum(ForumBase forum);
        Task<ForumBase> GetForumFromCategoryAsync(int categoryId, int forumId, bool trackChanges);
        Task<IEnumerable<ForumBase>> GetForumsFromCategoryByIdsAsync(int categoryId, IEnumerable<int> ids, bool trackChanges);
    }
}