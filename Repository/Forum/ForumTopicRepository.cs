﻿using Entities;
using Contracts.Forum;
using Entities.Models.Forum;
using Microsoft.EntityFrameworkCore;
using Entities.RequestFeatures.Forum;
using Entities.RequestFeatures;

namespace Repository.Forum
{
    public class ForumTopicRepository : RepositoryBase<ForumTopic, ForumContext>, IForumTopicRepository
    {
        public ForumTopicRepository(ForumContext forumContext) : base(forumContext)
        {
        }

        public void CreateTopicForForum(int forumBaseId, ForumTopic topic)
        {
            topic.ForumUserId = 1;
            topic.ForumBaseId = forumBaseId;
            Create(topic);
        }

        public void DeleteTopic(ForumTopic topic)
        {
            Delete(topic);
        }

        public async Task<PagedList<ForumTopic>> GetAllTopicsFromForumAsync(
            int? forumBaseId, ForumTopicParameters forumTopicParameters, bool trackChanges)
        {
            var topics = await FindByCondition(f => f.ForumBaseId.Equals(forumBaseId), trackChanges)
                .OrderBy(c => c.Name)
                .ToListAsync();

            return PagedList<ForumTopic>.ToPagedList(topics, forumTopicParameters.PageNumber, forumTopicParameters.PageSize);
        }

        public async Task<ForumTopic> GetTopicAsync(int forumBaseId, int topicId, bool trackChanges)
        {
            return await FindByCondition(c => c.ForumBaseId.Equals(forumBaseId) && c.Id.Equals(topicId), trackChanges)
                .SingleOrDefaultAsync();
        }
    }
}