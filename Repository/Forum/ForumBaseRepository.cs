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
            forum.ForumCategoryId = categoryId;
            Create(forum);
        }

        public IEnumerable<ForumBase> GetAllForums(int? categoryId, bool trackChanges)
        {
            return categoryId is null ? FindAll(trackChanges)
            .OrderBy(c => c.ForumTitle)
             .ToList() : FindByCondition(f => f.ForumCategoryId.Equals(categoryId), trackChanges)
                .OrderBy(c => c.ForumTitle).ToList();
        }
    }
}
