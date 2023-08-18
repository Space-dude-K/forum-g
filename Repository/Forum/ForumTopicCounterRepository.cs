using Entities;
using Entities.Models.Forum;
using Entities.RequestFeatures;
using Entities.RequestFeatures.Forum;
using Interfaces.Forum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Forum
{
    public class ForumTopicCounterRepository : RepositoryBase<ForumTopicCounter, ForumContext>, IForumTopicCounterRepository
    {
        public ForumTopicCounterRepository(ForumContext forumContext) : base(forumContext)
        {
        }
        public async Task<PagedList<ForumTopicCounter>> GetPostCountersAsync(bool trackChanges)
        {
            var topicCounters = await FindAll(trackChanges)
                .ToListAsync();

            return PagedList<ForumTopicCounter>.ToPagedList(topicCounters, 0, 0, true);
        }
        public async Task<PagedList<ForumTopicCounter>> GetPostCounterAsync(int? forumTopicId, bool trackChanges)
        {
            var topicCounters = await FindByCondition(f => f.ForumTopicId.Equals(forumTopicId), trackChanges)
                .ToListAsync();

            return PagedList<ForumTopicCounter>.ToPagedList(topicCounters, 0, 0, true);
        }
    }
}