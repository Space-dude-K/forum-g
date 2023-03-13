using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class ForumCategory
    {
        private int totalPost;

        public int Id { get; set; }
        public string Name { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
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
