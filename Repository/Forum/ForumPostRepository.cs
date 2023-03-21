using Entities;
using Contracts.Forum;
using Entities.Models.Forum;
using Microsoft.EntityFrameworkCore;

namespace Repository.Forum
{
    public class ForumPostRepository : RepositoryBase<ForumPost, ForumContext>, IForumPostRepository
    {
        public ForumPostRepository(ForumContext forumContext) : base(forumContext)
        {
        }

        public void CreatePostForTopic(int forumTopicId, ForumPost post)
        {
            post.ForumUserId = 1;
            post.ForumTopicId = forumTopicId;
            Create(post);
        }

        public void DeletePost(ForumPost post)
        {
            Delete(post);
        }

        public async Task<IEnumerable<ForumPost>> GetAllPostsFromTopicAsync(int? forumTopicId, bool trackChanges)
        {
            return await FindByCondition(f => f.ForumTopicId.Equals(forumTopicId), trackChanges)
                .OrderBy(c => c.PostName).ToListAsync();
        }

        public async Task<ForumPost> GetPostAsync(int forumTopicId, int postId, bool trackChanges)
        {
            return await FindByCondition(c => c.ForumTopicId.Equals(forumTopicId) && c.Id.Equals(postId), trackChanges)
                .SingleOrDefaultAsync();
        }
    }
}