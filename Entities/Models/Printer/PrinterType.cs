using System.ComponentModel.DataAnnotations;

namespace Entities.Models.Printer
{
    public class PrinterType
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Printer name is a required field.")]
        [MaxLength(60, ErrorMessage = "Maximum length for the printer name is 60 characters.")]
        public string Name { get; set; }
        public virtual PrinterDevice PrinterDevice { get; set; }
    }
}