namespace Entities.DTO.ForumDto
{
    public class ForumTopicDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int ForumUserId { get; set; }
        public int ForumBaseId { get; set; }
    }
}