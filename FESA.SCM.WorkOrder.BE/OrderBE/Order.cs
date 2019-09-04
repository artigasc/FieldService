using FESA.SCM.Common.Base;
using FESA.SCM.WorkOrder.BE.MachineryBE;
using FESA.SCM.WorkOrder.BE.TechnicalContactBE;
using System.Collections.Generic;

namespace FESA.SCM.WorkOrder.BE.OrderBE
{
    public class Order : Entity
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string CustomerId { get; set; }
        public string Description { get; set; }
        public Machinery Machinery { get; set; }
        public List<TechnicalContact> TechnicalContacts { get; set; }
        public string CostCenter { get; set; }
        public string Office { get; set; }
    }
}