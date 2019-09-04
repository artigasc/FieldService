using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FESA.SCM.ServiceGateway.Models {

	public class OfficeCostCenterSetting {
		public string GUID { get; set; }
		public int Index { get; set; }
		public int IdOffice { get; set; }
		public string DescriptionOffice { get; set; }
		public int IdCostCenter { get; set; }
		public string DescriptionCostCenter { get; set; }

	}

}