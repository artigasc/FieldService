using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Web;
using FESA.SCM.Common;
using FESA.SCM.WorkOrder.BE.OrderBE;
using FESA.SCM.WorkOrder.BL.BusinessInterfaces;
using FESA.SCM.WorkOrder.InstanceProviders;
using FESA.SCM.WorkOrder.BE.TechnicalContactBE;
using FESA.SCM.WorkOrder.BE.OrderPerClient;

namespace FESA.SCM.WorkOrder {
	[UnityInstanceProviderBehaviour]
	[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple)]
	public class WorkOrderApi : IWorkOrderApi {
		#region Members
		private readonly IOrderService _orderService;
		private readonly IOrderPerClientService _orderPerClientService;
		#endregion
		#region Constructor
		public WorkOrderApi(IOrderService orderService,
			IOrderPerClientService orderPerClientService) {
			if (orderService == null)
				throw new ArgumentNullException(nameof(orderService));
			_orderService = orderService;

			if (orderPerClientService == null)
				throw new ArgumentNullException(nameof(orderPerClientService));
			_orderPerClientService = orderPerClientService;
		}
		#endregion
		#region Methods
		public IList<Order> GetOrdersPaginatedList(Order order, int pageIndex, int pageSize, out int totalRows) {
			try {
				return _orderService.GetOrdersPaginatedList(order, pageIndex, pageSize, out totalRows);
			} catch (Exception ex) {
				Log.Write(ex);
				throw;
			}
		}

		public IList<OrderPerClient> GetNumberOrders(string supervisorid, DateTime dateIni, DateTime dateFin, string idsOffice, string idsCostCenter) {
			try {
				return _orderPerClientService.GetNumberOrders(supervisorid, dateIni, dateFin, idsOffice, idsCostCenter);
			} catch (Exception ex) {
				Log.Write(ex);
				throw;
			}
		}

		public IList<Order> GetOrderByCustomer(string customerid, string supervisorid, DateTime dateIni, DateTime dateFin, string idsOffice, string idsCostCenter) {
			try {
				return _orderService.GetOrderByCustomer(customerid, supervisorid, dateIni, dateFin, idsOffice, idsCostCenter);
			} catch (Exception ex) {
				Log.Write(ex);
				throw;
			}
		}

		public Order GetOrderById(string id) {
			try {
				return _orderService.GetOrderById(id);
			} catch (Exception ex) {
				Log.Write(ex);
				throw;
			}
		}

		public Order AddOrder(Order order) {
			try {
				return _orderService.AddOrder(order);
			} catch (Exception ex) {
				Log.Write(ex);
				throw;
			}
		}

		public void UpdateOrder(Order order) {
			try {
				_orderService.UpdateOrder(order);
			} catch (Exception ex) {
				Log.Write(ex);
				throw;
			}
		}

		public void DeleteOrder(string id, string modifiedBy, DateTime lastModification) {
			try {
				_orderService.DeleteOrder(id, modifiedBy, lastModification);
			} catch (Exception ex) {
				Log.Write(ex);
				throw;
			}
		}

		public IList<TechnicalContact> GetTechnicalByOrderId(Order orderid) {
			try {
				return _orderService.GetTechnicalByOrderId(orderid);
			} catch (Exception ex) {
				Log.Write(ex);
				throw;
			}
		}
		#endregion

	}
}
