using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models.Forum
{
    public class ForumTopic
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Topic title is a required field.")]
        [MaxLength(60, ErrorMessage = "Maximum length for the topic title is 60 characters.")]
        public string Name { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int TotalPosts { get; set; }
        public int TopicViewCounter { get; set; }
        /// <summary>
        /// Navigation property.
        /// </summary>
        public virtual ForumUser ForumUser { get; set; }
        public int ForumUserId { get; set; }
        /// <summary>
        /// Navigation property.
        /// </summary>
        public virtual ForumBase ForumBase { get; set; }
        public int ForumBaseId { get; set; }
        /// <summary>
        /// Navigation property.
        /// </summary>
        public virtual ICollection<ForumPost> ForumPosts { get; set; }
    }
}
