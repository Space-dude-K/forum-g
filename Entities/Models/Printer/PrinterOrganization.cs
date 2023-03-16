using System.ComponentModel.DataAnnotations;

namespace Entities.Models.Printer
{
    public class PrinterOrganization
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Printer organization is a required field.")]
        [MaxLength(60, ErrorMessage = "Maximum length for the printer organization is 60 characters.")]
        public string Name { get; set; }
        public virtual PrinterCity PrinterCity { get; set; }
        public int PrinterCityId { get; set; }
        public virtual ICollection<PrinterRoom> Rooms { get; set; }
    }
}