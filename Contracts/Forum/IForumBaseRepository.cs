﻿using Entities.Models.Forum;

namespace Contracts.Forum
{
    public interface IForumBaseRepository
    {
        IEnumerable<ForumBase> GetAllForums(int? categoryId, bool trackChanges);
        void CreateForumForCategory(int categoryId, ForumBase forum);
    }
}