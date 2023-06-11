using Entities.DTO.ForumDto.ForumView;
using Entities.Models.Forum;
using Microsoft.EntityFrameworkCore.Query;

namespace Entities.ViewModels.Forum
{
    public class ForumHomeViewModel
    {
        public List<ForumViewCategoryDto> Categories { get; set; }
    }
}