using FESA.SCM.ServiceGateway.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using FESA.SCM.Common;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using System.Net;
using FESA.SCM.ServiceGateway.Helper;
using System.Configuration;

namespace FESA.SCM.ServiceGateway.Controllers {
	public class ClientController : ApiController {
		// GET: Client
		private readonly string _contactApi;

		public ClientController() {
			_contactApi = ConfigurationManager.AppSettings["customer-api"];
		}
		[System.Web.Http.HttpPost]
		public async Task<IHttpActionResult> AddContact([FromBody]Contact contact) {
			try {
				if (contact == null)
					return Ok(new {
						Id = "",
						ErrorCode = "400",
						ErrorMessage = "Parameter cannot be null"
					});

				var restclient = new RestClient(_contactApi);
				var request = new RestRequest("addcontact") { JsonSerializer = new DynamicJsonSerializer() };

				request.AddJsonBody(new {
					contact
				});
				var response = await restclient.ExecutePostTaskAsync(request);
				return Ok(new {
					Id = Deserliaze(response.Content),
					ErrorCode = "",
					ErrorMessage = ""
				});
			} catch (Exception ex) {
				Log.Write(ex);
				return InternalServerError();
				//return Ok(new {
				//    Id = "",
				//    ErrorCode = "500",
				//    ErrorMessage = ex.Message
				//});
			}
		}
		private dynamic Deserliaze(string json) {
			return JsonConvert.DeserializeObject<IDictionary<string, dynamic>>(json).FirstOrDefault().Value;
		}
	}
}