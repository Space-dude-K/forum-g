using Entities.DTO.ForumDto.Create;
using System.ComponentModel.DataAnnotations;

namespace Entities.DTO.ForumDto
{
    public abstract class ForumCategoryForManipulationDto
    {
        [Required(ErrorMessage = "Category title is a required field.")]
        [MaxLength(30, ErrorMessage = "Maximum length for the category name is 30 characters.")]
        public string Name { get; set; }
    }
}