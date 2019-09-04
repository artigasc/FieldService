using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FESA.SCM.WebSite.Models {
    public class FileModel : Entity {
        public string Id { get; set; }
        public string IdRef { get; set; }
        public string Name { get; set; }
        public string Ext { get; set; }
        public string URL { get; set; }
        public int Status { get; set; }
        public byte[] FileData { get; set; }
    }
}
