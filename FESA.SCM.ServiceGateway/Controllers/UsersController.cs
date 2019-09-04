using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using FESA.SCM.Common;
using FESA.SCM.ServiceGateway.DTO;
using FESA.SCM.ServiceGateway.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RestSharp;
using FESA.SCM.ServiceGateway.Helper;
using System.Globalization;
using System.Net;

namespace FESA.SCM.ServiceGateway.Controllers {
	public class UsersController : ApiController {
		private readonly string _api;
		public UsersController() {
			_api = ConfigurationManager.AppSettings["identity-api"];
		}

		[HttpPost]
		public async Task<IHttpActionResult> Login([FromBody]Credential credential) {
			try {
				var restclient = new RestClient(_api);
				var request = new RestRequest("loginuser");
				request.AddJsonBody(new {
					userName = credential.UserName,
					userPassword = credential.UserPassword
				});
				var response = await restclient.ExecutePostTaskAsync(request);
				var user = JsonConvert.DeserializeObject<IDictionary<string, User>>(response.Content,
					new IsoDateTimeConverter { DateTimeFormat = "dd/mm/yyyy:hh:mm:ss" }).FirstOrDefault().Value;

				return Ok(user);
			} catch (Exception ex) {
				Log.Write(ex);
				return InternalServerError();
			}
		}

		[HttpPost]
		public async Task<IHttpActionResult> ChangePassword([FromBody]Credential credential) {
			try {
				var restclient = new RestClient(_api);
				var request = new RestRequest("changeuserpassword");
				request.AddJsonBody(new {
					userId = credential.UserId,
					userPassword = credential.UserPassword
				});
				var response = await restclient.ExecutePostTaskAsync(request);
				return Ok(response);
			} catch (Exception ex) {
				Log.Write(ex);
				return InternalServerError();
			}
		}

		[HttpGet]
		public async Task<IHttpActionResult> LogOff(string userId) {
			try {
				var restclient = new RestClient(_api);
				var request = new RestRequest("logoff");
				request.AddParameter("userId", userId);
				var response = await restclient.ExecuteGetTaskAsync(request);

				var intSuccess = JsonConvert.DeserializeObject<IDictionary<string, int>>(response.Content)
					.FirstOrDefault().Value;

				return Ok(intSuccess);
			} catch (Exception ex) {
				Log.Write(ex);
				return InternalServerError();
			}
		}

		[HttpGet]
		public async Task<IHttpActionResult> InsertDetails(string userId, int userStatus, DateTime date) {
			try {
				var restclient = new RestClient(_api);
				var request = new RestRequest("InsertDetails");
				request.AddParameter("userId", userId);
				request.AddParameter("userStatus", userStatus);
				request.AddParameter("date", date.ToString("dd/MM/yyyy HH:mm:ss.fff", CultureInfo.InvariantCulture));
				var response = await restclient.ExecuteGetTaskAsync(request);
				return Ok(response);
			} catch (Exception ex) {
				Log.Write(ex);
				return InternalServerError();
			}
		}

		[HttpPost]
		public async Task<IHttpActionResult> ResetPassword([FromBody]Credential credential) {
			try {
				var restclient = new RestClient(_api);
				var request = new RestRequest("ResetPassword");
				request.AddJsonBody(new {
					userName = credential.UserName
				});
				//request.AddParameter("userName", credential.UserName);
				var response = await restclient.ExecutePostTaskAsync(request);
				//if (JsonConvert.DeserializeObject<IDictionary<string, dynamic>>(response.Content).FirstOrDefault().Value)
				//    return NotFound();

				var user = Deserialize(response.Content);
				var password = user["Password"];

				string body = "<html>" + "<head></head>" + "<body>Estimado Cliente,<br/>" +
						$"Su nueva contraseña es : {password} " +
						"</body>" + "</html>";

				SendEmailClient send = new SendEmailClient();
				var userNames = new List<string>() { credential.UserName };
				send.PostSendEmail(userNames, "Recuperación de Contraseña App Ferreyros", body);
				return Ok();
			} catch (Exception ex) {
				Log.Write(ex);
				return InternalServerError();
			}
		}

		[HttpGet]
		public async Task<IHttpActionResult> GetOfficeCostCenter() {
			try {
				var restClient = new RestClient(_api);
				var officeRequest = new RestRequest("getalloffice");
				var response = await restClient.ExecuteGetTaskAsync(officeRequest);

				if (response.StatusCode != HttpStatusCode.OK)
					return Ok(new {
						ErrorCode = response.StatusCode,
						response.ErrorMessage
					});

				var allOffice =
					 JsonConvert.DeserializeObject<IDictionary<string, List<dynamic>>>(response.Content)
						.FirstOrDefault()
						.Value;
				var offices = allOffice.Select(raw => ConvertToOffice(raw)).Cast<Office>().ToList();

				var costCenterRequest = new RestRequest("getallcostcenter");
				response = await restClient.ExecuteGetTaskAsync(costCenterRequest);

				var allCostCenter =
					 JsonConvert.DeserializeObject<IDictionary<string, List<dynamic>>>(response.Content)
						.FirstOrDefault()
						.Value;
				var costCenters = allCostCenter.Select(raw => ConvertToCostCenter(raw)).Cast<CostCenter>().ToList();

				return Ok(new {
					offices = offices,
					costcenters = costCenters
				});

			} catch (Exception ex) {
				Log.Write(ex);
				throw;
			}

		}

		[HttpGet]
		public async Task<IHttpActionResult> GetAllFiltersWEB() {
			try {
				var restClient = new RestClient(_api);
				var officeRequest = new RestRequest("getallfilters");
				var response = await restClient.ExecuteGetTaskAsync(officeRequest);

				if (response.StatusCode != HttpStatusCode.OK)
					return Ok(new {
						ErrorCode = response.StatusCode,
						response.ErrorMessage
					});

				var all =
					 JsonConvert.DeserializeObject<IDictionary<string, ResponseFilter>>(response.Content)
						.FirstOrDefault()
						.Value;

				return Ok(all);

			} catch (Exception ex) {
				Log.Write(ex);
				throw;
			}
		}


		private Office ConvertToOffice(dynamic raw) {
			return new Office { Id = raw.Id, Description = raw.Description };
		}

		private CostCenter ConvertToCostCenter(dynamic raw) {
			return new CostCenter { Id = raw.Id, Description = raw.Description };
		}

		private dynamic Deserialize(string json) {
			return JsonConvert.DeserializeObject<IDictionary<string, dynamic>>(json).FirstOrDefault().Value;
		}

		[HttpPost]
#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
		public async Task<IHttpActionResult> UpdateUser([FromBody] User user)
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
		{
			try {
				//var restclient = new RestClient(_api);
				//var request = new RestRequest("UpdateUser") { JsonSerializer = new DynamicJsonSerializer() };
				//request.AddJsonBody(new 
				//{ user });
				//var response = await restclient.ExecutePostTaskAsync(request);
				return Ok();
			} catch (Exception ex) {
				Log.Write(ex);
				return InternalServerError();
			}
		}
	}
}
