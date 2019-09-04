using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FESA.SCM.Common;
using FESA.SCM.ServiceGateway.DTO;
using RestSharp;
using Newtonsoft.Json;
using System.Configuration;
using System.Threading.Tasks;
using FESA.SCM.WorkOrder.BE.OrderPerClient;

namespace FESA.SCM.ServiceGateway.Controllers {
	public class ReportsController : ApiController {

		private readonly string _assignmentApi;
		private readonly string _workorderApi;
		private readonly string _customerApi;
		private readonly string _identityApi;

		public ReportsController() {
			_assignmentApi = ConfigurationManager.AppSettings["assignment-api"];
			_workorderApi = ConfigurationManager.AppSettings["workorder-api"];
			_customerApi = ConfigurationManager.AppSettings["customer-api"];
			_identityApi = ConfigurationManager.AppSettings["identity-api"];
		}

		public async Task<IHttpActionResult> GetOcupabilityLevel(string supervisorId, string idsOffice, string idsCostCenter) {
			if (string.IsNullOrEmpty(supervisorId))
				return BadRequest();

			var restClient = new RestClient(_assignmentApi);
			var assignmentRequest = new RestRequest("GetOcupabilityLevel");
			assignmentRequest.AddParameter(nameof(supervisorId), supervisorId);
			assignmentRequest.AddParameter(nameof(idsOffice), idsOffice);
			assignmentRequest.AddParameter(nameof(idsCostCenter), idsCostCenter);
			var response = await restClient.ExecuteGetTaskAsync(assignmentRequest);

			if (response.StatusCode != HttpStatusCode.OK)
				return Ok(new { ErrorCode = response.StatusCode, response.ErrorMessage });

			var ocupability =
				JsonConvert.DeserializeObject<IDictionary<string, List<Ocupability>>>(response.Content)
					.FirstOrDefault()
					.Value;

			if (ocupability.Count == 0)
				return Ok(new List<Ocupability>());

			return Ok(ocupability);
		}
		public async Task<IHttpActionResult> GetUsersForState(UserStatus status, string supervisorid, string idsOffice, string idsCostCenter) {
			var restClient = new RestClient(_identityApi);
			var identityRequest = new RestRequest("GetUserByStatus");
			identityRequest.AddParameter("userstatus", status);
			identityRequest.AddParameter("supervisorid", supervisorid);
			identityRequest.AddParameter(nameof(idsOffice), idsOffice);
			identityRequest.AddParameter(nameof(idsCostCenter), idsCostCenter);
			var response = await restClient.ExecuteGetTaskAsync(identityRequest);

			if (response.StatusCode != HttpStatusCode.OK)
				return Ok(new { ErrorCode = response.StatusCode, response.ErrorMessage });

			var ocupability =
				JsonConvert.DeserializeObject<IDictionary<string, List<User>>>(response.Content)
					.FirstOrDefault()
					.Value;

			if (ocupability.Count == 0)
				return Ok(new List<User>());

			return Ok(ocupability);
		}
		[HttpPost]
		public IHttpActionResult GetOcupabilityLevel_(string supervisorId) {
			try {
				if (string.IsNullOrEmpty(supervisorId))
					return BadRequest();

				var mylist = new List<Ocupability>
				{
					new Ocupability
					{
						Percentage = 15.8f,
						UserStatus = UserStatus.Available
					},
					new Ocupability
					{
						Percentage = 44.2f,
						UserStatus = UserStatus.Assigned
					},
					new Ocupability
					{
						Percentage = 50,
						UserStatus = UserStatus.OnWork
					}
				};

				return Ok(mylist);
			} catch (Exception ex) {
				Log.Write(ex);
				return InternalServerError();
			}
		}
		public async Task<IHttpActionResult> GetOcupabilityThroughTimePerUser_(string supervisorid, int status) {
			var restClient = new RestClient(_assignmentApi);
			var identityRequest = new RestRequest("GetOcupabilityThroughTime");
			identityRequest.AddParameter(nameof(supervisorid), supervisorid);
			identityRequest.AddParameter(nameof(status), status);
			var response = await restClient.ExecuteGetTaskAsync(identityRequest);

			if (response.StatusCode != HttpStatusCode.OK)
				return Ok(new { ErrorCode = response.StatusCode, response.ErrorMessage });

			var ocupability =
				JsonConvert.DeserializeObject<IDictionary<string, List<OcupabilityThroughTime>>>(response.Content)
					.FirstOrDefault()
					.Value;

			foreach (var item in ocupability) {
				item.ShowedDate = $"{item.AssignmentDate.Day}/{item.AssignmentDate.Month}";
			}

			if (ocupability.Count == 0)
				return Ok(new List<OcupabilityThroughTime>());

			return Ok(ocupability);
		}
		public IHttpActionResult GetOcupabilityThroughTimePerUser(string technicianId) {
			try {
				if (string.IsNullOrEmpty(technicianId))
					return BadRequest();

				var mylist = new List<OcupabilityThroughTime>
				{
					new OcupabilityThroughTime
					{
						Quantity = 5,
						AssignmentDate = DateTime.Parse("01/15/2016"),
						UserId = "1",
						status = UserStatus.Assigned
					},
					new OcupabilityThroughTime
					{
						Quantity = 15,
						AssignmentDate = DateTime.Parse("02/15/2016"),
						UserId = "1"
					},
					new OcupabilityThroughTime
					{
						Quantity = 25,
						AssignmentDate = DateTime.Parse("03/15/2016"),
						UserId = "1"
					},
					new OcupabilityThroughTime
					{
						Quantity = 16,
						AssignmentDate = DateTime.Parse("04/15/2016"),
						UserId = "1"
					},
					new OcupabilityThroughTime
					{
						Quantity = 20,
						AssignmentDate = DateTime.Parse("05/15/2016"),
						UserId = "1"
					},
					new OcupabilityThroughTime
					{
						Quantity = 36,
						AssignmentDate = DateTime.Parse("06/15/2016"),
						UserId = "1"
					},
					new OcupabilityThroughTime
					{
						Quantity = 10,
						AssignmentDate = DateTime.Parse("07/2016"),
						UserId = "1"
					},
			};
				return Ok(mylist);
			} catch (Exception ex) {
				Log.Write(ex);
				return InternalServerError();
			}
		}
		public IHttpActionResult GetUsersForState(UserStatus status) {
			try {
				if (string.IsNullOrEmpty(Convert.ToString(status)))
					return BadRequest();

				var mylist = new List<User>
				{
					new User
					{
						Id = Guid.NewGuid().ToString(),
						Name = "Fernando Ramirez",
						UserStatus = status
					},
					new User
					{
						Id = Guid.NewGuid().ToString(),
						Name = "Juan Carlos Benitez",
						UserStatus = status
					},
					new User
					{
						Id = Guid.NewGuid().ToString(),
						Name = "Julian Villaran",
						UserStatus = status
					},
				};

				return Ok(mylist);
			} catch (Exception ex) {
				Log.Write(ex);
				return InternalServerError();
			}
		}
		public async Task<IHttpActionResult> GetNumberOrders_(string supervisorid, DateTime dateIni, DateTime dateFin, string idsOffice, string idsCostCenter) {

			var restClient = new RestClient(_workorderApi);
			var i = DateTime.Now.Date;
			var identityRequest = new RestRequest("GetNumberOrders");
			identityRequest.AddParameter(nameof(supervisorid), supervisorid);
			identityRequest.AddParameter(nameof(dateIni), dateIni.ToString("MM/dd/yyyy"));
			identityRequest.AddParameter(nameof(dateFin), dateFin.ToString("MM/dd/yyyy"));
			identityRequest.AddParameter("idsOffice", idsOffice);
			identityRequest.AddParameter("idsCostCenter", idsCostCenter);
			var response = await restClient.ExecuteGetTaskAsync(identityRequest);

			if (response.StatusCode != HttpStatusCode.OK)
				return Ok(new { ErrorCode = response.StatusCode, response.ErrorMessage });

			var ocupability =
				JsonConvert.DeserializeObject<IDictionary<string, List<OrderPerClient>>>(response.Content)
					.FirstOrDefault()
					.Value;

			if (ocupability.Count == 0)
				return Ok(new List<OrderPerClient>());

			return Ok(ocupability);
		}
		public IHttpActionResult GetOrdersPerClient(string supervisorId) {
			try {

				#region Workorder

				var client = new RestClient(_assignmentApi);
				var request = new RestRequest("getorderbyid");
				request.AddParameter("supervisorId", supervisorId);
				var response = client.ExecuteGetTaskAsync(request).GetAwaiter().GetResult();
				var wo =
					JsonConvert.DeserializeObject<IDictionary<string, dynamic>>(response.Content)
						.FirstOrDefault()
						.Value;

				#endregion

				#region Customer

				client = new RestClient(_customerApi);
				request = new RestRequest("getbycustomerid");
				request.AddParameter("Id", wo.CustomerId);
				response = client.ExecuteGetTaskAsync(request).GetAwaiter().GetResult();
				var customer =
					JsonConvert.DeserializeObject<IDictionary<string, dynamic>>(response.Content)
						.FirstOrDefault()
						.Value;

				#endregion


				var mylist = new List<OrdersPerClient>
				{
					new OrdersPerClient
					{
						AssignmentType = AssignmentType.FieldService,
						CompanyName = "ISA REP",
						Quantity = 40
					},
					new OrdersPerClient
					{
						AssignmentType = AssignmentType.Scheduled,
						CompanyName = "ISA REP",
						Quantity = 10
					},
					new OrdersPerClient
					{
						AssignmentType = AssignmentType.FieldService,
						CompanyName = "RSM FAESA",
						Quantity = 30
					},
					new OrdersPerClient
					{
						AssignmentType = AssignmentType.Scheduled,
						CompanyName = "RSM FAESA",
						Quantity = 8
					},
					new OrdersPerClient
					{
						AssignmentType = AssignmentType.FieldService,
						CompanyName = "Skynet",
						Quantity = 45
					},
					new OrdersPerClient
					{
						AssignmentType = AssignmentType.Scheduled,
						CompanyName = "Skynet",
						Quantity = 16
					}
				};

				return Ok(mylist);
			} catch (Exception ex) {
				Log.Write(ex);
				return InternalServerError();
			}
		}
		public async Task<IHttpActionResult> GetOrderByCustomer(string customerid, string supervisorid, DateTime dateIni, DateTime dateFin, string idsOffice, string idsCostCenter) {
			var restClient = new RestClient(_workorderApi);
			var identityRequest = new RestRequest("GetOrderByCustomer");
			identityRequest.AddParameter(nameof(customerid), customerid);
			identityRequest.AddParameter(nameof(supervisorid), supervisorid);
			identityRequest.AddParameter(nameof(dateIni), dateIni.ToString("MM/dd/yyyy"));
			identityRequest.AddParameter(nameof(dateFin), dateFin.ToString("MM/dd/yyyy"));
			identityRequest.AddParameter(nameof(idsOffice), idsOffice);
			identityRequest.AddParameter(nameof(idsCostCenter), idsCostCenter);
			var response = await restClient.ExecuteGetTaskAsync(identityRequest);

			if (response.StatusCode != HttpStatusCode.OK)
				return Ok(new { ErrorCode = response.StatusCode, response.ErrorMessage });

			var ocupability =
				JsonConvert.DeserializeObject<IDictionary<string, List<Order>>>(response.Content)
					.FirstOrDefault()
					.Value;

			if (ocupability.Count == 0)
				return Ok(new List<Order>());

			return Ok(ocupability);
		}
		public async Task<IHttpActionResult> GetAssignmentByOrder(string orderid) {
			var restClientAssignment = new RestClient(_assignmentApi);
			var assignmentRequest = new RestRequest("GetAssignmentByOrder");

			assignmentRequest.AddParameter(nameof(orderid), orderid);

			var restClientIdentity = new RestClient(_identityApi);
			var identityRequest = new RestRequest("GetUsersByIdAssignment");

			var restClientOrder = new RestClient(_workorderApi);
			var orderRequest = new RestRequest("GetOrderById");

			var restClientCustomer = new RestClient(_customerApi);
			var customerRequest = new RestRequest("GetByCustomerId");

			var responseAssignment = await restClientAssignment.ExecuteGetTaskAsync(assignmentRequest);

			if (responseAssignment.StatusCode != HttpStatusCode.OK)
				return Ok(new { ErrorCode = responseAssignment.StatusCode, responseAssignment.ErrorMessage });

			var ocupability =
				JsonConvert.DeserializeObject<IDictionary<string, List<Assignment>>>(responseAssignment.Content)
					.FirstOrDefault()
					.Value;

			foreach (var itemOcupability in ocupability) {

				foreach (var item in itemOcupability.Activities) {
					//ANTONIODATE
					item.StartDate = DateTime.SpecifyKind(item.StartDate, DateTimeKind.Unspecified);
					item.EndDate = DateTime.SpecifyKind(item.EndDate.Value, DateTimeKind.Unspecified);
				}

				identityRequest.AddParameter("id", itemOcupability.Id);

				var responseIdentity = await restClientIdentity.ExecuteGetTaskAsync(identityRequest);
				if (responseIdentity.StatusCode != HttpStatusCode.OK)
					return Ok(new { ErrorCode = responseIdentity.StatusCode, responseIdentity.ErrorMessage });
				var identities =
				//JsonConvert.DeserializeObject<IDictionary<string, List<User>>>(responseIdentity.Content)
				JsonConvert.DeserializeObject<IDictionary<string, dynamic>>(responseIdentity.Content)
					.FirstOrDefault()
					.Value;

				orderRequest.AddParameter("id", orderid);

				var responseOrder = await restClientOrder.ExecuteGetTaskAsync(orderRequest);
				if (responseOrder.StatusCode != HttpStatusCode.OK)
					return Ok(new { ErrorCode = responseOrder.StatusCode, responseOrder.ErrorMessage });
				var orders =
				JsonConvert.DeserializeObject<IDictionary<string, Order>>(responseOrder.Content)
					.FirstOrDefault()
					.Value;

				itemOcupability.TechnicalContacts = orders.TechnicalContacts.Select(x => new Contact() { Id = x.Id, Name = x.Name, Email = x.Email, Phone = x.Phone, CustomerId = orders.CustomerId, IsFerreyrosContact = false, LastName = x.LastName, Charge = x.Charge }).ToList();
				//itemOcupability.FerreyrosContacts = identities.Select(x => new Contact() { Id = x.Id, Name = x.Name, Email = x.Email, Phone = x.Phone, Charge =  "",  CustomerId = orders.CustomerId,  IsFerreyrosContact = true }).ToList();

				itemOcupability.FerreyrosContacts = new List<Contact>();
				foreach (var item in identities) {
					itemOcupability.FerreyrosContacts.Add(new Contact {
						Id = item.Id,
						Name = item.Name,
						LastName = item.LastName,
						Email = item.Email,
						Phone = item.Phone,
						Charge = item?.Role.Name,
						CustomerId = orders.CustomerId,
						IsFerreyrosContact = true
					});
				}


				itemOcupability.Machine = new Machine() {
					Id = orders.Machinery.Id,
					Brand = orders.Machinery.Brand,
					Model = orders.Machinery.Model,
					LifeHours = orders.Machinery.LifeHours,
					SerialNumber = orders.Machinery.SerialNumber,
					TotalHoursFunction = (int)orders.Machinery.LifeHours
				};
				itemOcupability.WorkOrderNumber = orders.Code;
				itemOcupability.RegisterDate = itemOcupability.CreationDate;

				customerRequest.AddParameter("id", orders.CustomerId);

				var responseCustomer = await restClientCustomer.ExecuteGetTaskAsync(customerRequest);
				if (responseCustomer.StatusCode != HttpStatusCode.OK)
					return Ok(new { ErrorCode = responseCustomer.StatusCode, responseCustomer.ErrorMessage });
				var customers =
				JsonConvert.DeserializeObject<IDictionary<string, CustomerCompany>>(responseCustomer.Content)
					.FirstOrDefault()
					.Value;

				itemOcupability.CompanyName = customers.BusinessName;
			}

			if (ocupability.Count == 0)
				return Ok(new List<Assignment>());

			//var item = ocupability[0];
			return Ok(ocupability);
		}

	}
}
