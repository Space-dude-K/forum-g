namespace Entities.DTO.ForumDto
{
    public class ForumCategoryForCreationDto
    {
        public string Name { get; set; }
        public IEnumerable<ForumBaseForCreationDto> ForumBases { get; set; }
    }
}