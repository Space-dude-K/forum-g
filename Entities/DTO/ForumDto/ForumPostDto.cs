namespace Entities.DTO.ForumDto
{
    public class ForumPostDto
    {
        public int Id { get; set; }
        public string PostName { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int ForumTopicId { get; set; }
        public int ForumUserId { get; set; }
    }
}