using Entities.DTO.ForumDto;
using Entities.DTO.ForumDto.ForumView;

namespace Entities.ViewModels.Forum
{
    public class ForumTopicViewModel
    {
        public int TotalPosts { get { return Posts.Count; } }
        public string SubTopicAuthor { get; set; }
        public string SubTopicCreatedAt { get; set; }
        public List<ForumViewPostDto> Posts { get; set; }
    }
}