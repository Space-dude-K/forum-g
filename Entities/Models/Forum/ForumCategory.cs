using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models.Forum
{
    public class ForumCategory
    {
        private int totalPost;

        public int Id { get; set; }
        [Required(ErrorMessage = "Category title is a required field.")]
        [MaxLength(60, ErrorMessage = "Maximum length for the category title is 60 characters.")]
        public string Name { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int TotalPosts
        {
            get
            {
                return ForumBases.Sum(f => f.ForumTopics.Sum(t => t.ForumPosts.Count));
            }
        }
        /// <summary>
        /// Navigation property.
        /// </summary>
        public virtual ForumUser ForumUser { get; set; }
        public int ForumUserId { get; set; }
        /// <summary>
        /// Navigation property.
        /// </summary>
        public virtual ICollection<ForumBase> ForumBases { get; set; }
    }
}
