using FESA.SCM.WorkOrder.BE.OrderPerClient;
using FESA.SCM.WorkOrder.BL.BusinessInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FESA.SCM.WorkOrder.BL.BusinessServices
{
    public class OrderPerClientService : IOrderPerClientService
    {
        #region Members
        private readonly IOrderPerClientRepository _orderPerClientRepository;

        #endregion

        #region Constructor
        public OrderPerClientService(IOrderPerClientRepository orderPerClientRepository)
        {
            if (orderPerClientRepository == null)
                throw new ArgumentNullException(nameof(orderPerClientRepository));
            _orderPerClientRepository = orderPerClientRepository;
        }
        #endregion

        #region Methods
        public IList<OrderPerClient> GetNumberOrders(string supervisorid, DateTime dateIni, DateTime dateFin, string idsOffice, string idsCostCenter)
        {
            return _orderPerClientRepository.GetNumberOrders(supervisorid, dateIni, dateFin, idsOffice, idsCostCenter);
        }
        #endregion
    }
}
