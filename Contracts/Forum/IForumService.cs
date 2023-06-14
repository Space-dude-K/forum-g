using Entities.DTO.ForumDto;
using Entities.DTO.ForumDto.ForumView;
using Entities.ViewModels.Forum;

namespace Interfaces.Forum
{
    public interface IForumService
    {
        Task<List<ForumViewCategoryDto>> GetForumCategories();
        Task<ForumHomeViewModel> GetForumCategoriesAndForumBasesForModel();
        Task<ForumBaseViewModel> GetForumTopicsForModel(int categoryId, int forumId);
        Task<ForumTopicViewModel> GetTopicPostsForModel(int categoryId, int forumId, int topicId);
        Task<bool> IncreaseViewCounterForForumBase(int categoryId, int forumId);
    }
}
