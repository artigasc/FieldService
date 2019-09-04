using System;
using System.Collections.Generic;
using FESA.SCM.WorkOrder.BE.OrderBE;
using FESA.SCM.WorkOrder.BE.TechnicalContactBE;
using FESA.SCM.WorkOrder.BE.OrderPerClient;

namespace FESA.SCM.WorkOrder.BL.BusinessInterfaces
{
    public interface IOrderService
    {
        IList<Order> GetOrdersPaginatedList(Order order, int pageIndex, int pageSize, out int totalRows);
        Order GetOrderById(string id);
        Order AddOrder(Order order);
        void UpdateOrder(Order order);
        void DeleteOrder(string id, string modifiedBy, DateTime lastModification);
        IList<TechnicalContact> GetTechnicalByOrderId(Order orderid);
        IList<Order> GetOrderByCustomer(string customerid, string supervisorid, DateTime dateIni, DateTime dateFin, string idsOffice, string idsCostCenter);
        
    }
}