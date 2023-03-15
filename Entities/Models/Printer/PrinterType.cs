namespace Entities.Models.Printer
{
    public class PrinterType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual PrinterDevice PrinterDevice { get; set; }
    }
}