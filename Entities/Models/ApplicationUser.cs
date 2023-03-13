using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// Navigation property.
        /// </summary>
        public ForumUser ForumUser { get; set; }
    }
}
