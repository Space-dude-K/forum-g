using Entities.DTO.ForumDto;
using Entities.DTO.ForumDto.ForumView;

namespace Entities.ViewModels.Forum
{
    public class ForumTopicViewModel
    {
        public List<ForumViewPostDto> Posts { get; set; }
    }
}