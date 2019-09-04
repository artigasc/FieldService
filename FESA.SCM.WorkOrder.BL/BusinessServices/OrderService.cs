using System;
using System.Collections.Generic;
using FESA.SCM.WorkOrder.BE.MachineryBE;
using FESA.SCM.WorkOrder.BE.OrderBE;
using FESA.SCM.WorkOrder.BE.TechnicalContactBE;
using FESA.SCM.WorkOrder.BL.BusinessInterfaces;
using System.Linq;

namespace FESA.SCM.WorkOrder.BL.BusinessServices {
    public class OrderService : IOrderService {
        #region Members
        private readonly IOrderRepository _orderRepository;
        private readonly ITechnicalContactRepository _technicalContactRepository;
        private readonly IMachineryRepository _machineryRepository;
        #endregion
        #region Constructor
        public OrderService(IOrderRepository orderRepository, ITechnicalContactRepository technicalContactRepository,
            IMachineryRepository machineryRepository) {
            if (orderRepository == null)
                throw new ArgumentNullException(nameof(orderRepository));

            _orderRepository = orderRepository;

            if (technicalContactRepository == null)
                throw new ArgumentNullException(nameof(technicalContactRepository));

            _technicalContactRepository = technicalContactRepository;

            if (machineryRepository == null)
                throw new ArgumentNullException(nameof(machineryRepository));

            _machineryRepository = machineryRepository;
        }
        #endregion
        #region Public Methods
        public IList<Order> GetOrdersPaginatedList(Order order, int pageIndex, int pageSize, out int totalRows) {
            return _orderRepository.GetPaginated(order, pageIndex, pageSize, out totalRows);
        }

        public IList<Order> GetOrderByCustomer(string customerid, string supervisorid, DateTime dateIni, DateTime dateFin, string idsOffice, string idsCostCenter)
        {
            return _orderRepository.GetOrderByCustomer(customerid, supervisorid, dateIni, dateFin, idsOffice, idsCostCenter);
        }

   
        public Order GetOrderById(string id) {
            var order = _orderRepository.GetById(id);
            if (order != null) {
                var listTechnical = _technicalContactRepository.GetContactByIdOrder(order.Id);
                order.TechnicalContacts = new List<TechnicalContact>(listTechnical);
            }
            return order;
        }

        public Order AddOrder(Order order) {
            var orderId = _orderRepository.GetIdByCode(order.Code);
            if (string.IsNullOrEmpty(orderId)) {
                order.Machinery.Id = Guid.NewGuid().ToString();
                order.Machinery.CreationDate = DateTime.Now;
                _machineryRepository.Add(order.Machinery);
                order.Id = Guid.NewGuid().ToString();
                _orderRepository.Add(order);
                order.Active = false;
            } else {
                order.Id = orderId;
                order.Active = true;
            }

            if (order.TechnicalContacts != null) {
                var contacts = _technicalContactRepository.GetContactByIdOrder(order.Id);
                if (contacts != null && contacts.Count > 0) {
                    foreach (var item in order.TechnicalContacts) {
                        var exist = contacts.Where(x => x.ContactId == item.ContactId && x.CustomerId == order.Id).FirstOrDefault();
                        if (exist != null) {
                            item.Id = exist.Id;
                            item.CustomerId = exist.CustomerId;
                            item.Active = true;
                        }
                    }
                }

                foreach (var item in contacts) {
                    var noExist = order.TechnicalContacts.Where(x => x.ContactId == item.ContactId && x.CustomerId == item.CustomerId).FirstOrDefault();
                    if (noExist == null) {
                        _technicalContactRepository.DeleteByOrderAndContactId(item.CustomerId, item.ContactId);
                    }
                }

                foreach (var item in order.TechnicalContacts) {
                    if (!item.Active) {
                        item.Id = Guid.NewGuid().ToString();
                        item.CreationDate = DateTime.Now;
                        item.CustomerId = order.Id;
                        _technicalContactRepository.Add(item);
                    } else {
                        _technicalContactRepository.Update(item);
                    }
                }

            }
            return order;
        }

        public void UpdateOrder(Order order) {
            _orderRepository.Update(order);
            //_technicalContactRepository.Update(order.TechnicalContacts);
            _machineryRepository.Update(order.Machinery);
        }

        public void DeleteOrder(string id, string modifiedBy, DateTime lastModification) {
            _orderRepository.Delete(id, modifiedBy, lastModification);
        }

        public IList<TechnicalContact> GetTechnicalByOrderId(Order orderid) {
            throw new NotImplementedException();
        }
        #endregion
    }
}