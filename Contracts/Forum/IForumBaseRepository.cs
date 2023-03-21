using Entities.Models.Forum;

namespace Contracts.Forum
{
    public interface IForumBaseRepository
    {
        Task<IEnumerable<ForumBase>> GetAllForumsAsync(int? categoryId, bool trackChanges);
        void CreateForumForCategory(int categoryId, ForumBase forum);
        void DeleteForum(ForumBase forum);
        Task<ForumBase> GetForumAsync(int categoryId, int forumId, bool trackChanges);
    }
}