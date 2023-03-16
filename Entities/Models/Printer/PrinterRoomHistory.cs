using System.ComponentModel.DataAnnotations;

namespace Entities.Models.Printer
{
    public class PrinterRoomHistory
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Reason is a required field.")]
        [MaxLength(60, ErrorMessage = "Maximum length for the reason is 60 characters.")]
        public string Reason { get; set; }
        public DateTime? InstalledAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public virtual PrinterRoom PrinterRoom { get; set; }
        public int PrinterDeviceId { get; set; }
    }
}