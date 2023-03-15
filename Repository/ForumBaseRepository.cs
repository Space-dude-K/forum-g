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
    public class ForumBaseRepository : RepositoryBase<ForumUser>, IForumBaseRepository
    {
        public ForumBaseRepository(ForumContext forumContext) : base(forumContext)
        {
        }
    }
}
