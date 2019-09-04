using FESA.SCM.Common.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FESA.SCM.WorkOrder.BE.OrderPerClient
{
    public interface IOrderPerClientRepository 
    {
        IList<OrderPerClient> GetNumberOrders(string supervisorid, DateTime dateIni, DateTime dateFin, string idsOffice, string idsCostCenter);
    }
}
