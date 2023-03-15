using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models.Forum
{
    public class ForumTopic
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
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
