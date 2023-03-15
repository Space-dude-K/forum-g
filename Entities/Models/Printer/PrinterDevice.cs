namespace Entities.Models.Printer
{
    public class PrinterDevice
    {
        public int Id { get; set; }
        public virtual PrinterType PrinterType { get; set; }
        public int? PrinterTypeId { get; set; }
        public virtual PrinterStatistic PrinterStatistic { get; set; }
        public int? PrinterStatisticId { get; set; }
        public virtual PrinterRoom PrinterRoom { get; set; }
        public int? PrinterRoomId { get; set; }
    }
}