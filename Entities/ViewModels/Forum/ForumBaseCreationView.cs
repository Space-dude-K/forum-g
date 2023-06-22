using Entities.DTO.ForumDto.ForumView;
using Entities.ModelAttributes;

namespace Entities.ViewModels.Forum
{
    public class ForumBaseCreationView
    {
        //public List<ForumViewCategoryDto> Categories { get; set; }
        [EnsureMinimumElements(min: 1, ErrorMessage = "Select at least one item")]
        public List<string>? Categories { get; set; }
        public string SelectedCategory { get; set; }
        public string ForumTitle { get; set; }
        public string ForumSubtitle { get; set;}
    }
}