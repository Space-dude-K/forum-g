using Entities.DTO.ForumDto;
using Entities.DTO.ForumDto.Create;
using Entities.DTO.ForumDto.ForumView;
using Entities.ViewModels.Forum;

namespace Interfaces.Forum
{
    public interface IForumService
    {
        Task<bool> CreateForumBase(int categoryId, ForumBaseForCreationDto forum);
        Task<bool> CreateForumCategory(ForumCategoryForCreationDto category);
        Task<bool> CreateForumPost(int categoryId, int forumId, int topicId, ForumPostForCreationDto post);
        Task<bool> CreateForumTopic(int categoryId, int forumId, ForumTopicForCreationDto topic);
        Task<List<ForumViewCategoryDto>> GetForumCategories();
        Task<ForumHomeViewModel> GetForumCategoriesAndForumBasesForModel();
        Task<ForumBaseViewModel> GetForumTopicsForModel(int categoryId, int forumId);
        Task<int> GetTopicPostCount(int categoryId, int forumId, int topicId);
        Task<ForumTopicViewModel> GetTopicPostsForModel(int categoryId, int forumId, int topicId, int pageNumber, int pageSize);
        Task<bool> IncreaseViewCounterForForumBase(int categoryId, int forumId);
        Task<bool> IncreaseViewCounterForTopic(int categoryId, int forumId, int topicId);
    }
}
