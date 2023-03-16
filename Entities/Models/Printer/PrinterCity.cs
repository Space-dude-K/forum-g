using System.ComponentModel.DataAnnotations;

namespace Entities.Models.Printer
{
    public class PrinterCity
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Printer city is a required field.")]
        [MaxLength(60, ErrorMessage = "Maximum length for the printer city is 60 characters.")]
        public string Name { get; set; }
        public virtual ICollection<PrinterOrganization> Organizations { get; set; }
    }
}