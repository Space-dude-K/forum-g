namespace Entities.Models.Printer
{
    public class PrinterRoomHistory
    {
        public int Id { get; set; }
        public string Reason { get; set; }
        public string InstalledAt { get; set; }
        public string DeletedAt { get; set; }
        public virtual PrinterRoom PrinterRoom { get; set; }
        public int PrinterDeviceId { get; set; }
    }
}