using System.Net;

namespace FESA.SCM.Core.Models
{
    public class ClientResponse
    {
        public dynamic Content { get; set; }
        public HttpStatusCode Status { get; set; }
        public string ErrorMessage { get; set; }
        public string StatusDescription { get; set; }
    }
}