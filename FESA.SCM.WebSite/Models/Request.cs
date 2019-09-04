using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FESA.SCM.WebSite.Models
{
    public class Request {
        public AssignmentModel Assignment { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
