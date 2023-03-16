using Contracts.Forum;
using Entities;
using Entities.Models;
using Entities.Models.Forum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Repository.Forum
{
    public class ForumUserRepository : RepositoryBase<ForumUser, ForumContext>, IForumUserRepository
    {
        public ForumUserRepository(ForumContext forumContext) : base(forumContext)
        {
        }

    }
}
