using Contracts;
using Entities.Models;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class ForumPostRepository : RepositoryBase<ForumUser>, IForumPostRepository
    {
        public ForumPostRepository(ForumContext forumContext) : base(forumContext)
        {
        }
    }
}
