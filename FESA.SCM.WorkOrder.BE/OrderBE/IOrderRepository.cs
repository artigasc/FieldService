using FESA.SCM.Common.Base;
using FESA.SCM.WorkOrder.BE.OrderPerClient;
using FESA.SCM.WorkOrder.BE.TechnicalContactBE;
using System;
using System.Collections.Generic;

namespace FESA.SCM.WorkOrder.BE.OrderBE {
	public interface IOrderRepository : IBaseRepository<Order> {
		IList<Order> GetOrderByCustomer(string customerid, string supervisorid, DateTime dateIni, DateTime dateFin, string idsOffice, string idsCostCenter);
		string GetIdByCode(string code);
	}
}