﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTO.ForumDto
{
    public abstract class ForumBaseForManipulationDto
    {
        [Required(ErrorMessage = "Forum title is a required field.")]
        [MaxLength(30, ErrorMessage = "Maximum length for the forum title is 30 characters.")]
        public string ForumTitle { get; set; }
        [Required(ErrorMessage = "Forum sub title is a required field.")]
        [MaxLength(30, ErrorMessage = "Maximum length for the forum sub title is 30 characters.")]
        public string ForumSubTitle { get; set; }
    }
}