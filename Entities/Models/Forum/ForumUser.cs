using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models.Forum
{
    public class ForumUser
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "User name is a required field.")]
        [MaxLength(60, ErrorMessage = "Maximum length for the Name is 60 characters.")]
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Lastname { get; set; }
        public int Karma { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public virtual ForumAccount ForumAccount { get; set; }
    }
}
