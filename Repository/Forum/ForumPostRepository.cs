using Entities;
using Contracts.Forum;
using Entities.Models.Forum;
using Microsoft.EntityFrameworkCore;
using Entities.RequestFeatures.Forum;
using Entities.RequestFeatures;
using Repository.Extensions;

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

        public async Task<PagedList<ForumPost>> GetAllPostsFromTopicAsync(
            int? forumTopicId, ForumPostParameters forumPostParameters, bool trackChanges)
        {
            var posts = await FindByCondition(f => f.ForumTopicId.Equals(forumTopicId), trackChanges)
                .FilterPosts(forumPostParameters.MinLikes, forumPostParameters.MaxLikes)
                .Search(forumPostParameters.SearchTerm)
                .Sort(forumPostParameters.OrderBy)
                .ToListAsync();

            return PagedList<ForumPost>.ToPagedList(posts, forumPostParameters.PageNumber, forumPostParameters.PageSize);
        }
        public async Task<PagedList<ForumPost>> GetAllPostsFromCategoryAsyncForBiggerData(
            int? forumTopicId, ForumPostParameters forumPostParameters, bool trackChanges)
        {
            var posts = await FindByCondition(f => f.ForumTopicId.Equals(forumTopicId), trackChanges)
                .OrderBy(c => c.PostName)
                .Skip((forumPostParameters.PageNumber - 1) * forumPostParameters.PageSize)
                .Take(forumPostParameters.PageSize)
                .ToListAsync();

            var count = await FindByCondition(e => e.ForumTopicId.Equals(forumTopicId), trackChanges).CountAsync();

            return new PagedList<ForumPost>(posts, forumPostParameters.PageNumber, forumPostParameters.PageSize, count);
        }
        public async Task<ForumPost> GetPostAsync(int forumTopicId, int postId, bool trackChanges)
        {
            return await FindByCondition(c => c.ForumTopicId.Equals(forumTopicId) && c.Id.Equals(postId), trackChanges)
                .SingleOrDefaultAsync();
        }
    }
}