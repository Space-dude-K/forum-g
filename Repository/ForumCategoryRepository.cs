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
    public class ForumCategoryRepository : RepositoryBase<ForumUser>, IForumCategoryRepository
    {
        public ForumCategoryRepository(ForumContext forumContext) : base(forumContext)
        {
        }
    }
}