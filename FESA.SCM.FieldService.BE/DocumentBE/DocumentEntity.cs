using FESA.SCM.Common.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FESA.SCM.FieldService.BE.DocumentBE {

	public class DocumentEntity : Entity {

		public string Id { get; set; }
		public string Code { get; set; }
		public string Name { get; set; }
		public int Position { get; set; }
		public bool Popup { get; set; }

	}


}
