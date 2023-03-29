using Entities.DTO.ForumDto.Create;

namespace Entities.DTO.ForumDto.Update
{
    public class ForumCategoryForUpdateDto
    {
        public string Name { get; set; }
        public IEnumerable<ForumBaseForCreationDto>? ForumBases { get; set; }
    }
}