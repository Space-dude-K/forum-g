namespace Entities.DTO.ForumDto
{
    public class ForumCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int ForumUserId { get; set; }
    }
}