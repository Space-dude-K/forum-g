using Entities.DTO.ForumDto.Create;
using Entities.DTO.ForumDto.ForumView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Forum.ApiServices
{
    public interface IForumBaseService
    {
        Task<bool> CreateForumBase(int categoryId, ForumBaseForCreationDto forum);
        Task<List<ForumViewBaseDto>> GetForumBases(int categoryId);
        Task<bool> IncreaseViewCounterForForumBase(int categoryId, int forumId);
    }
}
