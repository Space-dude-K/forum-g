using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTO.ForumDto.ForumView
{
    public class ForumViewCategoryDto : ForumCategoryDto
    {
        public int TotalPosts { get; set; }
    }
}
