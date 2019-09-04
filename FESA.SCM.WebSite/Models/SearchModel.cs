using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FESA.SCM.WebSite.Models
{
    public class SearchModel
    {
        public string Client { get; set; }
        public string WorkOrderNumber { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Supervisor { get; set; }
        public string IdBrand { get; set; }
        public string IdOffice { get; set; }
        public string IdCostCenter { get; set; }

    }

    public class SendFileModel {
        public Microsoft.AspNetCore.Http.Internal.FormFile FileContent { get; set; }
        public string AssignmentId { get; set; }
        public string action { get; set; }
    }
}
