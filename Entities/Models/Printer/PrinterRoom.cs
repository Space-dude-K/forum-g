namespace Entities.Models.Printer
{
    public class PrinterRoom
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual PrinterOrganization PrinterOrganization { get; set; }
        public int PrinterOrganizationId { get; set; }
        public virtual PrinterRoomHistory? RoomHistory { get; set; }
        public int? PrinterRoomHistoryId { get; set; }
        public virtual ICollection<PrinterDevice> Printers { get; set; }
    }
}