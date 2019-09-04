using FESA.SCM.Common.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FESA.SCM.WorkOrder.BE.OrderPerClient
{
    public class OrderPerClient : Entity
    {
        public string OrderId { get; set; }
        public int Quantity { get; set; }
        public string CompanyName { get; set; }
        public AssignmentType AssignmentType { get; set; }
    }
}
