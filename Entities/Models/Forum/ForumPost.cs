using System.ComponentModel.DataAnnotations;

namespace Entities.Models.Forum
{
    public class ForumPost
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Post title is a required field.")]
        [MaxLength(60, ErrorMessage = "Maximum length for the post title is 60 characters.")]
        public string PostText { get; set; }
        public int? Likes { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public virtual ForumTopic ForumTopic { get; set; }
        public int ForumTopicId { get; set; }
        public virtual ForumUser ForumUser { get; set; }
        public int ForumUserId { get; set; }
    }
}