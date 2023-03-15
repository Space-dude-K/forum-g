using Contracts;
using Entities;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Repository
{
    public class ForumUserRepository : RepositoryBase<ForumUser>, IForumUserRepository
    {
        public ForumUserRepository(ForumContext forumContext) : base(forumContext)
        {
        }

    }
}
