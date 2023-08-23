using Entities.DTO.ForumDto.Create;
using Entities.DTO.ForumDto.ForumView;
using Entities.DTO.UserDto;
using Entities.ViewModels.Forum;

namespace Interfaces.Forum
{
    public interface IForumService
    {
        Task<bool> CreateForumBase(int categoryId, ForumBaseForCreationDto forum);
        Task<bool> CreateForumCategory(ForumCategoryForCreationDto category);
        Task<bool> CreateForumPost(int categoryId, int forumId, int topicId, ForumPostForCreationDto post);
        Task<bool> CreateForumTopic(int categoryId, int forumId, ForumTopicForCreationDto topic);
        Task<bool> DeleteForumPost(int categoryId, int forumId, int topicId, int postId);
        Task<List<ForumViewCategoryDto>> GetForumCategories();
        Task<ForumHomeViewModel> GetForumCategoriesAndForumBasesForModel();
        Task<ForumBaseViewModel> GetForumTopicsForModel(int categoryId, int forumId);
        Task<int> GetTopicCount(int categoryId);
        Task<int> GetTopicPostCount(int topicId);
        Task<ForumTopicViewModel> GetTopicPostsForModel(int categoryId, int forumId, int topicId, int pageNumber, int pageSize);
        Task<bool> UpdatePostCounter(int categoryId, bool incresase);
        Task<bool> IncreaseViewCounterForForumBase(int categoryId, int forumId);
        Task<bool> IncreaseViewCounterForTopic(int categoryId, int forumId, int topicId);
        Task<bool> UpdatePost(int categoryId, int forumId, int topicId, int postId, string newText);
        Task<bool> UpdateTopicCounter(int categoryId, bool incresase);
        Task<bool> UpdatePostCounterForUser(int userId, bool incresase);
        //Task<int> GetPostCounterForUser(int userId);
        //Task<ForumUserDto> GetForumUser(int userId);
    }
}