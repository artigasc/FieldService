using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Solution.Ferreyros.Models
{
    public class OTDetail
    {
        public string Client { get; set; }
        public string Detail { get; set; }
        public string DateApplicationStart { get; set; }
        public DateTime EstimatedDateStart { get; set; }
        public DateTime EstimatedDateEnd { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public List<Client> FullName { get; set; }
        public List<Client> Email { get; set; }
        public List<Client> Mobile { get; set; }
    }
}
