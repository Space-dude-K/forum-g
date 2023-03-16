using Entities.Models;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts.Forum;
using Entities.Models.Forum;

namespace Repository.Forum
{
    public class ForumPostRepository : RepositoryBase<ForumPost, ForumContext>, IForumPostRepository
    {
        public ForumPostRepository(ForumContext forumContext) : base(forumContext)
        {
        }
    }
}
