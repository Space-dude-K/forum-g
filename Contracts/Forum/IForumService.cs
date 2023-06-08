using Entities.DTO.ForumDto;
using Entities.DTO.ForumDto.ForumView;

namespace Interfaces.Forum
{
    public interface IForumService
    {
        Task<List<ForumViewCategoryDto>> GetForumCategories();
    }
}
