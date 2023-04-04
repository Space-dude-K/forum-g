﻿using Contracts.Forum;
using Entities;
using Entities.Models.Forum;
using Entities.RequestFeatures;
using Entities.RequestFeatures.Forum;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;

namespace Repository.Forum
{
    public class ForumBaseRepository : RepositoryBase<ForumBase, ForumContext>, IForumBaseRepository
    {
        public ForumBaseRepository(ForumContext forumContext) : base(forumContext)
        {
            
        }

        public void CreateForumForCategory(int categoryId, ForumBase forum)
        {
            forum.ForumUserId = 1;
            forum.ForumCategoryId = categoryId;
            Create(forum);
        }

        public void DeleteForum(ForumBase forum)
        {
            Delete(forum);
        }
        public async Task<PagedList<ForumBase>> GetAllForumsFromCategoryAsync(
            int? categoryId, ForumBaseParameters forumBaseParameters, bool trackChanges)
        {
            var forums = await FindByCondition(f => f.ForumCategoryId.Equals(categoryId), trackChanges)
                .OrderBy(c => c.ForumTitle)
                .ToListAsync();

            return PagedList<ForumBase>.ToPagedList(forums, forumBaseParameters.PageNumber, forumBaseParameters.PageSize);
        }
        public async Task<ForumBase> GetForumFromCategoryAsync(int categoryId, int forumId, bool trackChanges)
        {
            return await FindByCondition(c => c.ForumCategoryId.Equals(categoryId) && c.Id.Equals(forumId), trackChanges)
                .SingleOrDefaultAsync();
        }
    }
}