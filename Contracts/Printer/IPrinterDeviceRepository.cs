using Entities.Models.Forum;
using Entities.Models.Printer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Printer
{
    public interface IPrinterDeviceRepository
    {
        IEnumerable<PrinterDevice> GetAllPrinterDevices(bool trackChanges);
    }
}
