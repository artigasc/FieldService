using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FESA.SCM.WebSite.Models
{
    public class Response
    {

        public List<AssignmentModel> Assignments { get; set; }

    }

    public class ResponseAssignment {

        public List<AssignmentModel> Assignments { get; set; }

        public AssignmentModel Assignment { get; set; }
        public List<DocumentModel> DocumentsEntity { get; set; }

        public List<dynamic> ActivitiesEntity { get; set; }
        public string Message { get; set; }

        public int TotalRows { get; set; }
        
    }

    public class ResponseFilter {
        public List<CostCenterModel> CostsCenter { get; set; }
        public List<OfficeModel> Offices { get; set; }
        public List<BrandModel> Brands { get; set; }
    }

    public class ResponseContact {
        public string Id { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }

    }
}
