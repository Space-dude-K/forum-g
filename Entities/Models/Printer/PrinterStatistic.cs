namespace Entities.Models.Printer
{
    public class PrinterStatistic
    {
        public int Id { get; set; }
        public int TonerLevel { get; set; }
        public int DrumLevel { get; set; }
        public int TotalPagesPrinted { get; set; }
        public virtual PrinterDevice PrinterDevice { get; set; }
    }
}