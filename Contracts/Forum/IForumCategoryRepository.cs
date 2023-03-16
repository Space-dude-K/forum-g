using Entities.Models.Forum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Forum
{
    public interface IForumCategoryRepository
    {
        IEnumerable<ForumCategory> GetAllCategories(bool trackChanges);
        ForumCategory GetCategory(int categoryId, bool trackChanges);
        void CreateCategory(ForumCategory category);
    }
}
