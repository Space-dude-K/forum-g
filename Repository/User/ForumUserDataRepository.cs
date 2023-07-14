using Entities;
using Entities.Models.Forum;
using Interfaces.User;

namespace Repository.User
{
    public class ForumUserDataRepository : RepositoryBase<ForumUser, ForumContext>, IForumUserDataRepository
    {
        public ForumUserDataRepository(ForumContext forumContext) : base(forumContext)
        {
        }
        public void CreateForumUser(int appUserId)
        {
            ForumUser forumUser = new() { Id = appUserId, AppUserId = appUserId, CreatedAt = DateTime.Now };

            Create(forumUser);
        }
    }
}