using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models.Forum
{
    public class ForumPost
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Post title is a required field.")]
        [MaxLength(60, ErrorMessage = "Maximum length for the post title is 60 characters.")]
        public string PostName { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        /// <summary>
        /// Navigation property.
        /// </summary>
        public virtual ForumTopic ForumTopic { get; set; }
        public int ForumTopicId { get; set; }
        /// <summary>
        /// Navigation property.
        /// </summary>
        public virtual ForumUser ForumUser { get; set; }
        public int ForumUserId { get; set; }
    }
}
