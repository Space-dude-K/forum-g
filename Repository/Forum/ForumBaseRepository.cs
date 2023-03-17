using Contracts.Forum;
using Entities;
using Entities.Models.Forum;
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

        public IEnumerable<ForumBase> GetAllForums(int? categoryId, bool trackChanges)
        {
            return FindByCondition(f => f.ForumCategoryId.Equals(categoryId), trackChanges)
                .OrderBy(c => c.ForumTitle).ToList();
        }
        public ForumBase GetForum(int categoryId, int forumId, bool trackChanges)
        {
            return FindByCondition(c => c.ForumCategoryId.Equals(categoryId) && c.Id.Equals(forumId), trackChanges)
                .SingleOrDefault();
        }
    }
}
