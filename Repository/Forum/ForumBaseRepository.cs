using Contracts.Forum;
using Entities;
using Entities.Models.Forum;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Repository.Forum
{
    public class ForumBaseRepository : RepositoryBase<ForumBase, ForumContext>, IForumBaseRepository
    {
        public ForumBaseRepository(ForumContext forumContext) : base(forumContext)
        {
            
        }

        public void CreateForumForCategory(int categoryId, ForumBase forum)
        {
            forum.ForumUserId = 1;
            forum.ForumCategoryId = categoryId;
            Create(forum);
        }

        public void DeleteForum(ForumBase forum)
        {
            Delete(forum);
        }

        public async Task<IEnumerable<ForumBase>> GetAllForumsAsync(int? categoryId, bool trackChanges)
        {
            return await FindByCondition(f => f.ForumCategoryId.Equals(categoryId), trackChanges)
                .OrderBy(c => c.ForumTitle).ToListAsync();
        }
        public async Task<ForumBase> GetForumAsync(int categoryId, int forumId, bool trackChanges)
        {
            return await FindByCondition(c => c.ForumCategoryId.Equals(categoryId) && c.Id.Equals(forumId), trackChanges)
                .SingleOrDefaultAsync();
        }
    }
}
