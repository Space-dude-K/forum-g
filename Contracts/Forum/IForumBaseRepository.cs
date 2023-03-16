using Entities.Models.Forum;

namespace Contracts.Forum
{
    public interface IForumBaseRepository
    {
        IEnumerable<ForumBase> GetAllForums(bool trackChanges);
    }
}