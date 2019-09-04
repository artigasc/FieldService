using FESA.SCM.Common.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FESA.SCM.FieldService.BE.DocumentBE {
	public class File : Entity {

		public string Id { get; set; }
		public string IdRef { get; set; }
		public string Name { get; set; }
		public string Ext { get; set; }
		public string URL { get; set; }
        public byte[] FileData { get; set; }
        public int status { get; set; }
    }
}
