﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models.Forum
{
    public class ForumCategory
    {
        private int totalPost;

        public int Id { get; set; }
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
