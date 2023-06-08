using Entities.DTO.ForumDto;
using Entities.DTO.ForumDto.ForumView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.ViewModels.Forum
{
    public class ForumHomeViewModel
    {
        public List<ForumViewCategoryDto> Categories { get; set; }
    }
}
