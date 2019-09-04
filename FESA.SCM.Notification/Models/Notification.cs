using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FESA.SCM.Notification.Models
{
    public class Notification
    {
        public string pns { get; set; }
        public string message { get; set; }
        public string toTag { get; set; }
    }
}