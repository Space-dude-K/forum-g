namespace Entities.Models.Printer
{
    public class PrinterRoomHistory
    {
        public int Id { get; set; }
        public string Reason { get; set; }
        public DateTime? InstalledAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public virtual PrinterRoom PrinterRoom { get; set; }
        public int PrinterDeviceId { get; set; }
    }
}