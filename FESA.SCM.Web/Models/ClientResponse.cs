using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace FESA.SCM.Web.Models
{
    public class ClientResponse
    {
        public dynamic Content { get; set; }
        public HttpStatusCode Status { get; set; }
        public string ErrorMessage { get; set; }
        public string StatusDescription { get; set; }

    }
}