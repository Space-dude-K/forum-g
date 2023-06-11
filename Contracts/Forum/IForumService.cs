using Entities.DTO.ForumDto;
using Entities.DTO.ForumDto.ForumView;
using Entities.ViewModels.Forum;

namespace Interfaces.Forum
{
    public interface IForumService
    {
        Task<List<ForumViewCategoryDto>> GetForumCategories();
        Task<ForumHomeViewModel> GetForumCategoriesAndForumBases();
    }
}
