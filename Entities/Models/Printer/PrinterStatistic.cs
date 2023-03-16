using System.ComponentModel.DataAnnotations;

namespace Entities.Models.Printer
{
    public class PrinterStatistic
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Toner level is a required field.")]
        [MaxLength(60, ErrorMessage = "Maximum length for the toner level is 60 characters.")]
        public int TonerLevel { get; set; }
        [Required(ErrorMessage = "Drum level is a required field.")]
        [MaxLength(60, ErrorMessage = "Maximum length for the drum level is 60 characters.")]
        public int DrumLevel { get; set; }
        [Required(ErrorMessage = "Total pages printed is a required field.")]
        [MaxLength(60, ErrorMessage = "Maximum length for the total pages printed is 60 characters.")]
        public int TotalPagesPrinted { get; set; }
        public virtual PrinterDevice PrinterDevice { get; set; }
    }
}