namespace Entities.Models.Printer
{
    public class PrinterCity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<PrinterOrganization> Organizations { get; set; }
    }
}