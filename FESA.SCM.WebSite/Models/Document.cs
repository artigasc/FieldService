using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FESA.SCM.WebSite.Models {
    public class DocumentModel {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string AssignmentId { get; set; }
        public string Name { get; set; }
        public string ActivityId { get; set; }
        public int ActivityValue { get; set; }
        public string Text { get; set; }
        public int Position { get; set; }
        public bool Popup { get; set; }

        public string DocumentId { get; set; }
        public DateTime Date { get; set; }

    }
}
