using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FESA.SCM.ServiceGateway.DTO
{
    public class Order
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string CustomerId { get; set; }
        public string Description { get; set; }
        public Machinery Machinery { get; set; }
        public TechnicalContact TechnicalContact { get; set; }
        public List<TechnicalContact> TechnicalContacts { get; set; }
        public string CostCenter { get; set; }
        public string Office { get; set; }
    }
}
