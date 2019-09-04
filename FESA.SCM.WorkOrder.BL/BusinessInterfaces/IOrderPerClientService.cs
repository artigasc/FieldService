using FESA.SCM.WorkOrder.BE.OrderPerClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FESA.SCM.WorkOrder.BL.BusinessInterfaces
{
    public interface IOrderPerClientService
    {
        IList<OrderPerClient> GetNumberOrders(string supervisorid, DateTime dateIni, DateTime dateFin, string idsOffice, string idsCostCenter);
    }
}
