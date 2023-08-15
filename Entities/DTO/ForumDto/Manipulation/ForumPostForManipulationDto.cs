namespace Entities.DTO.ForumDto.Manipulation
{
    public abstract class ForumPostForManipulationDto
    {
        public string PostName { get; set; } = "TestName";
        public string PostText { get; set; }
        public int Likes { get; set; } = 0;
    }
}