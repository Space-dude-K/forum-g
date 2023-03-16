using Contracts.Forum;
using Entities;
using Entities.Models.Forum;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Repository.Forum
{
    public class ForumBaseRepository : RepositoryBase<ForumBase, ForumContext>, IForumBaseRepository
    {
        public ForumBaseRepository(ForumContext forumContext) : base(forumContext)
        {
            
        }
        public IEnumerable<ForumBase> GetAllForums(bool trackChanges)
        {
            return FindAll(trackChanges)
             .OrderBy(c => c.ForumTitle)
             .ToList();
        }
    }
}
