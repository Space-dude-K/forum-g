using Entities.Models.Forum;
using Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Forum
{
    public interface IForumTopicCounterRepository
    {
        Task<PagedList<ForumTopicCounter>> GetPostCounterAsync(int? forumTopicId, bool trackChanges);
        Task<PagedList<ForumTopicCounter>> GetPostCountersAsync(bool trackChanges);
    }
}
