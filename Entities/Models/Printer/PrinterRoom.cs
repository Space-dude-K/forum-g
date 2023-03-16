using System.ComponentModel.DataAnnotations;

namespace Entities.Models.Printer
{
    public class PrinterRoom
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Printer room name is a required field.")]
        [MaxLength(60, ErrorMessage = "Maximum length for the printer room name is 60 characters.")]
        public string Name { get; set; }
        public virtual PrinterOrganization PrinterOrganization { get; set; }
        public int PrinterOrganizationId { get; set; }
        public virtual PrinterRoomHistory? RoomHistory { get; set; }
        public int? PrinterRoomHistoryId { get; set; }
        public virtual ICollection<PrinterDevice> Printers { get; set; }
    }
}