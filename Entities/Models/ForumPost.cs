using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class ForumPost
    {
        public int Id { get; set; }
        public string PostName { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
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
