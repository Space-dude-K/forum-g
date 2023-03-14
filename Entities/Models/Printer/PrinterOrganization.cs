namespace Entities.Models.Printer
{
    public class PrinterOrganization
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual PrinterCity PrinterCity { get; set; }
        public int PrinterCityId { get; set; }
        public virtual ICollection<PrinterRoom> Rooms { get; set; }
    }
}