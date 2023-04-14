using Entities.Models.Forum;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces.Printer;
using Entities.Models.Printer;

namespace Repository.Printer
{
    public class PrinterDeviceRepository : RepositoryBase<PrinterDevice, PrinterContext>, IPrinterDeviceRepository
    {
        public PrinterDeviceRepository(PrinterContext printerContext) : base(printerContext)
        {
        }

        public IEnumerable<PrinterDevice> GetAllPrinterDevices(bool trackChanges)
        {
            return FindAll(trackChanges)
             .OrderBy(p => p.Id)
             .ToList();
        }
    }
}
