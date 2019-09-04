using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using FESA.SCM.Common;
using FESA.SCM.ServiceGateway.DTO;
using FESA.SCM.ServiceGateway.Helper;
using Newtonsoft.Json;
using RestSharp;
using FESA.SCM.ServiceGateway.Models;
using Newtonsoft.Json.Converters;
using System.Dynamic;

namespace FESA.SCM.ServiceGateway.Controllers {
	public class AssignmentsController : ApiController {


		private readonly string _assignmentApi;
		private readonly string _workorderApi;
		private readonly string _customerApi;
		private readonly string _identityApi;
		private readonly string _notificationApi;

		public AssignmentsController() {
			_assignmentApi = ConfigurationManager.AppSettings["assignment-api"];
			_workorderApi = ConfigurationManager.AppSettings["workorder-api"];
			_customerApi = ConfigurationManager.AppSettings["customer-api"];
			_identityApi = ConfigurationManager.AppSettings["identity-api"];
			_notificationApi = ConfigurationManager.AppSettings["notification-api"];
		}

		public IHttpActionResult Get() {
			return Ok(new {
				message = "this is working; "
			});
		}

		public async Task<IHttpActionResult> Get(string userId) {
			try {
				if (string.IsNullOrEmpty(userId))
					return BadRequest();

				var restClient = new RestClient(_assignmentApi);
				var assignmentRequest = new RestRequest("getbyuserid");
				assignmentRequest.AddParameter(nameof(userId), userId);
				var response = await restClient.ExecuteGetTaskAsync(assignmentRequest);

				if (response.StatusCode != HttpStatusCode.OK)
					return Ok(new {
						ErrorCode = response.StatusCode,
						response.ErrorMessage
					});

				var result = JsonConvert.DeserializeObject<IDictionary<string, ResponseAll>>(response.Content)
					.FirstOrDefault()
					.Value;

				if (result.Assignments.Count == 0) {
					return Ok(new ResponseAll() {
						Message = "No se encontraron OTs asignadas"
					});
				}

				var assignments = result.Assignments.Select(raw => ConvertToAssignment(raw, userId)).Cast<Assignment>().ToList();
				if (assignments != null && assignments.Count > 0) {
					result.Assignments = new List<Assignment>(assignments);
				}
				return Ok(result);
			} catch (Exception ex) {
				Log.Write(ex);
				return InternalServerError();
			}
		}

		[HttpPost]
		public async Task<IHttpActionResult> Sync([FromBody] List<Assignment> assignments) {
			try {
				var stringFormat = "dd/MM/yyyy H:mm:ss";
				var TimeZone = TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time");
				foreach (var item in assignments) {
					//item.StartDate = DateTime.ParseExact(item.StartDate.ToString(stringFormat), stringFormat, CultureInfo.InvariantCulture);
					if (item.EndDate <= System.Data.SqlTypes.SqlDateTime.MinValue.Value) {
						item.EndDate = item.EstimatedEndDate;
					}
					//item.EndDate = DateTime.ParseExact(item.EndDate.ToString(stringFormat), stringFormat, CultureInfo.InvariantCulture);
					item.EstimatedStartDate = DateTime.ParseExact(item.EstimatedStartDate.ToString(stringFormat), stringFormat, CultureInfo.InvariantCulture);
					item.EstimatedEndDate = DateTime.ParseExact(item.EstimatedEndDate.ToString(stringFormat), stringFormat, CultureInfo.InvariantCulture);
					item.RegisterDate = DateTime.ParseExact(item.RegisterDate.ToString(stringFormat), stringFormat, CultureInfo.InvariantCulture);
				}

				var restclient = new RestClient(_assignmentApi);
				var request = new RestRequest("syncassignments") { JsonSerializer = new DynamicJsonSerializer() };

				request.AddJsonBody(new {
					assignments
				});
				var response = await restclient.ExecutePostTaskAsync(request);

				var intSuccess = JsonConvert.DeserializeObject<IDictionary<string, int>>(response.Content)
					.FirstOrDefault().Value;

				return Ok(intSuccess);
			} catch (Exception ex) {
				Log.Write(ex);
				return InternalServerError();
			}
		}

		[HttpPost]
		public async Task<IHttpActionResult> AddFullAssignment([FromBody]dynamic assignment) {
			try {

				//string userFesaId = "F422";
				//var useraa = await GetUser(userFesaId);



				if (assignment == null)
					return Ok(new {
						AssignmentId = "",
						ErrorCode = "400",
						ErrorMessage = "Parameter cannot be null"
					});

				var customer = assignment.Customer;

				var client = new RestClient(_customerApi);
				var request = new RestRequest("getcustomerbyruc");
				request.AddParameter("ruc", customer.Ruc);

				var response = await client.ExecuteGetTaskAsync(request);

				if (response.StatusCode != HttpStatusCode.OK)
					return Ok(new {
						AssignmentId = "",
						ErrorCode = response.StatusCode,
						response.ErrorMessage
					});

				var customerId = Deserliaze(response.Content)?.Id ?? await AddCustomer(customer);
				var order = assignment.WorkOrder;

				client = new RestClient(_workorderApi);
				request = new RestRequest("addOrder") { JsonSerializer = new DynamicJsonSerializer() };

				//
				var TechnicalContacts = new List<dynamic>();
				foreach (var technicalC in assignment.Customer.Contacts) {
					TechnicalContacts.Add(new {
						technicalC.Id,
						ContactId = technicalC.Id,
						technicalC.Name,
						technicalC.LastName,
						technicalC.Charge,
						technicalC.Phone,
						technicalC.Email,
						CustomerId = ""
					});
				}
				//

				request.AddJsonBody(new {
					order = new {
						Code = order.Code,
						CreationDate = DateTime.ParseExact(order.CreationDate.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
						Description = assignment.Description,
						CustomerId = customerId,
						TechnicalContacts = TechnicalContacts,
						Machinery = assignment.Machinery,
						CostCenter = order.CostCenter,
						Office = order.Office
					}
				});

				response = await client.ExecutePostTaskAsync(request);
				if (response.StatusCode != HttpStatusCode.OK)
					return Ok(new {
						AssignmentId = "",
						ErrorCode = response.StatusCode,
						response.ErrorMessage
					});

				var _order = Deserliaze(response.Content);
				var orderId = _order.Id;

				var assignedPersonnel = new List<dynamic>();

				foreach (var personnel in assignment.Personnel) {
					var user = await GetUser(personnel.Code);
					var type = GetPersonelType(personnel);
					if (user == null)
						return Ok(new {
							AssignmentId = "",
							ErrorCode = response.StatusCode,
							ErrorMessage = "El usuario con código " + personnel.Code + " no está registrado en el directorio"
						});
					assignedPersonnel.Add(new {
						user.Id,
						PersonnelType = type,
						user.Name,
						user.Pns
					});
				}

				client = new RestClient(_assignmentApi);
				request = new RestRequest("addassignment") { JsonSerializer = new DynamicJsonSerializer() };
				request.AddJsonBody(new {
					assignment = new {
						assignment.CiaId,
						assignment.RequestId,
						assignment.CorpId,
						assignment.AssignmentType,
						WorkOrderId = orderId,
						Active = _order.Active,
						assignment.Description,
						assignment.Priority,
						RequestDate = DateTime.ParseExact(assignment.RequestDate.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
						EstimatedEndDate = DateTime.ParseExact(assignment.EstimatedEndDate.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
						EstimatedStartDate = DateTime.ParseExact(assignment.EstimatedStartDate.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
						assignment.Location,
						AssignedPersonnel = assignedPersonnel
					}
				});

				response = await client.ExecutePostTaskAsync(request);

				if (response.StatusCode != HttpStatusCode.OK)
					return Ok(new {
						AssignmentId = "",
						ErrorCode = response.StatusCode,
						response.ErrorMessage
					});

				foreach (var o in assignedPersonnel) {
					client = new RestClient(_notificationApi);
					request = new RestRequest("Notifications") { JsonSerializer = new DynamicJsonSerializer() };
					var message = $"Estimado {o.Name}, tiene una nueva asignación";
					request.AddJsonBody(new {
						pns = o.Pns,
						message = message,
						toTag = o.Id
					});

					//request.AddParameter("pns", o.Pns);
					//request.AddParameter("toTag", o.Id);
					//request.AddJsonBody(message);

					var alertResponse = await client.ExecutePostTaskAsync(request);
				}

				return Ok(new {
					AssignmentId = Deserliaze(response.Content),
					ErrorCode = "",
					ErrorMessage = ""
				});
				//return Ok();

			} catch (Exception ex) {
				Log.Write(ex);
				return Ok(new {
					AssignmentId = "",
					ErrorCode = "500",
					ErrorMessage = ex.Message
				});
			}
		}

		[HttpPost]
		public async Task<IHttpActionResult> SyncActivities([FromBody]List<Activity> activities) {
			try {
				var restclient = new RestClient(_assignmentApi);
				var request = new RestRequest("syncactivities") { JsonSerializer = new DynamicJsonSerializer() };

				request.AddJsonBody(new {
					activities
				});

				var response = await restclient.ExecutePostTaskAsync(request);

				var intSuccess = JsonConvert.DeserializeObject<IDictionary<string, int>>(response.Content)
					.FirstOrDefault().Value;

				return Ok(intSuccess);

			} catch (Exception ex) {
				Log.Write(ex);
				return InternalServerError();
			}
		}

		[HttpPost]
		public async Task<IHttpActionResult> SyncActivitiesDocuments([FromBody]Request requestBody) {
			try {
				var restclient = new RestClient(_assignmentApi);
				var request = new RestRequest("syncactivitiesdocuments") { JsonSerializer = new DynamicJsonSerializer() };
				request.AddJsonBody(new {
					request = requestBody
				});
				var response = await restclient.ExecutePostTaskAsync(request);
				var intSuccess = JsonConvert.DeserializeObject<IDictionary<string, int>>(response.Content)
					.FirstOrDefault().Value;
				return Ok(intSuccess);
			} catch (Exception ex) {
				Log.Write(ex);
				return InternalServerError();
			}
		}

		[HttpPost]
		public async Task<IHttpActionResult> SyncReport([FromBody]Report report) {
			try {
				var restclient = new RestClient(_assignmentApi);
				var request = new RestRequest("syncreport") { JsonSerializer = new DynamicJsonSerializer() };

				request.AddJsonBody(new {
					report
				});

				var response = await restclient.ExecutePostTaskAsync(request);

				var intSuccess = JsonConvert.DeserializeObject<IDictionary<string, int>>(response.Content)
					.FirstOrDefault().Value;

				return Ok(intSuccess);

			} catch (Exception ex) {
				Log.Write(ex);
				return InternalServerError();
			}
		}

		[HttpPost]
		public async Task<IHttpActionResult> SyncDocuments([FromBody] List<Document> documents) {
			try {
				var restclient = new RestClient(_assignmentApi);
				var request = new RestRequest("syncdocuments") { JsonSerializer = new DynamicJsonSerializer() };

				request.AddJsonBody(new {
					documents
				});
				var response = await restclient.ExecutePostTaskAsync(request);
				return Ok();
			} catch (Exception ex) {
				Log.Write(ex);
				return InternalServerError();
			}
		}


		private Assignment ConvertToAssignment(Assignment raw, string userId) {

			try {
				#region Workorder

				var client = new RestClient(_workorderApi);
				var request = new RestRequest("getorderbyid");
				request.AddParameter("Id", raw.WorkOrderId);

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
				#region Machine

				var machine = new Machine {
					Id = wo.Machinery.Id,
					AssignmentId = raw.Id,
					Brand = wo.Machinery.Brand,
					Model = wo.Machinery.Model,
					SerialNumber = wo.Machinery.SerialNumber,
					TotalHoursFunction = wo.Machinery.LifeHours
				};

				#endregion
				#region Contact Ferreyros

				string id = raw.Id;
				client = new RestClient(_identityApi);
				request = new RestRequest("getusersbyidassignment");
				//request = new RestRequest("getuserbyid");
				request.AddParameter("id", id);
				response = client.ExecuteGetTaskAsync(request).GetAwaiter().GetResult();
				var contactsFerreyros =
					JsonConvert.DeserializeObject<IDictionary<string, dynamic>>(response.Content)
						.FirstOrDefault()
						.Value;

				#endregion

				#region Activities

				var activities = new List<Activity>();
				foreach (var itemDoc in raw.Activities) {
					activities.Add(new Activity {
						Id = itemDoc.Id,
						UserId = itemDoc.UserId,
						AssignmentId = itemDoc.AssignmentId,
						Group = itemDoc.Group,
						Name = itemDoc.Name,
						Description = itemDoc.Description,
						Duration = itemDoc.Duration,
						//StartDate = DateTime.SpecifyKind(itemDoc.StartDate.Value, DateTimeKind.Unspecified),
						//EndDate = DateTime.SpecifyKind(itemDoc.EndDate.Value, DateTimeKind.Unspecified),
						//ANTONIODATE
						StartDate = DateTime.SpecifyKind(itemDoc.StartDate, DateTimeKind.Unspecified),
						EndDate = DateTime.SpecifyKind(itemDoc.EndDate.Value, DateTimeKind.Unspecified),
						ActivityType = itemDoc.ActivityType,
						ActivityState = itemDoc.ActivityState,
						Online = true,
						Active = itemDoc.Active
					});
				}

				#endregion

				#region Documents

				var documents = new List<Document>();
				foreach (var itemDoc in raw.Documents) {
					documents.Add(new Document {
						Id = itemDoc.Id,
						Name = itemDoc.Name,
						Position = itemDoc.Position,
						DocumentId = itemDoc.DocumentId,
						ActivityId = itemDoc.ActivityId,
						ActivityValue = itemDoc.ActivityValue,
						Text = itemDoc.Text,
						Date = itemDoc.Date,
						Check = itemDoc.Check,
						UserId = itemDoc.UserId,
						AssignmentId = itemDoc.AssignmentId,
						Active = itemDoc.Active
					});
				}

				#endregion

				#region BuildAssignment

				var TechnicalContacts = new List<Contact>();
				if (wo.TechnicalContacts != null || wo.TechnicalContacts != "") {
					foreach (var item in wo.TechnicalContacts) {
						TechnicalContacts.Add(new Contact() {
							AssignmentId = raw.Id,
							Id = item.Id,
							Name = item.Name,
							LastName = item.LastName,
							Charge = item.Charge,
							Phone = item.Phone,
							Email = item.Email
						});
					}
				}

				var FerreyrosContacts = new List<Contact>();
				if (contactsFerreyros != null || contactsFerreyros != "") {
					foreach (var item in contactsFerreyros) {
						FerreyrosContacts.Add(new Contact() {
							AssignmentId = raw.Id,
							Id = Guid.NewGuid().ToString(), // item.Id,
							Name = item.Name,
							Charge = item?.Role.Name,
							Phone = item.Phone,
							Email = item.Email,
							IsFerreyrosContact = true
						});
					}
				}

				var stringFormat = "dd/MM/yyyy H:mm:ss";

				var assignment = new Assignment {
					Id = raw.Id,
					AssignmentType = raw.AssignmentType,
					Status = raw.Status,
					WorkOrderNumber = wo.Code,
					WorkOrderId = raw?.WorkOrderId,
					Location = new Location {
						AssignmentId = raw.Id,
						Department = raw.Location.Department,
						Id = raw.Location.Id,
						Ubicacion = raw.Location.Ubicacion,
						Province = raw.Location.Province,
						District = raw.Location.District
					},
					Description = raw.Description,
					Priority = raw.Priority,
					CompanyName = customer?.BusinessName,
					EstimatedEndDate = DateTime.ParseExact(raw.EstimatedEndDate.ToString(stringFormat), stringFormat, CultureInfo.InvariantCulture), // EstimatedEndDate = raw.EstimatedEndDate,
					StartDate = DateTime.ParseExact(raw.StartDate.ToString(stringFormat), stringFormat, CultureInfo.InvariantCulture), // raw.StartDate,
					EstimatedStartDate = DateTime.ParseExact(raw.EstimatedStartDate.ToString(stringFormat), stringFormat, CultureInfo.InvariantCulture), // raw.EstimatedStartDate,
					Machine = machine,
					//RegisterDate = DateTime.ParseExact(raw.RequestDate.ToString(stringFormat), stringFormat, CultureInfo.InvariantCulture), // raw.RequestDate,                
					RegisterDate = DateTime.ParseExact(raw.RequestDate.ToString(stringFormat), stringFormat, CultureInfo.InvariantCulture), // raw.RequestDate,                
					RequestDate = DateTime.ParseExact(raw.RequestDate.ToString(stringFormat), stringFormat, CultureInfo.InvariantCulture), // raw.RequestDate,
					TechnicalContacts = new List<Contact>(TechnicalContacts),
					FerreyrosContacts = new List<Contact>(FerreyrosContacts),
					Activities = new List<Activity>(activities),
					Documents = new List<Document>(documents),
					Active = raw.Active,
				};

				if (raw.EndDate.HasValue) {
					assignment.EndDate = DateTime.ParseExact(raw.EndDate.Value.ToString(stringFormat), stringFormat, CultureInfo.InvariantCulture);
				}


				#endregion

				return assignment;
			} catch (Exception ex) {
				Log.Write(ex);
				return new Assignment();
			}

		}

		private async Task<dynamic> AddCustomer(dynamic customer) {
			var client = new RestClient(_customerApi);
			var request = new RestRequest("addcustomer") { JsonSerializer = new DynamicJsonSerializer() };
			request.AddJsonBody(new {
				customer = new {
					//ContactId = customer.Id,
					customer.Name,
					customer.BusinessName,
					customer.Ruc,
					customer.Contacts
				}
			});

			var response = await client.ExecutePostTaskAsync(request);

			return response.StatusCode != HttpStatusCode.OK ? null : Deserliaze(response.Content);
		}

		private dynamic GetPersonelType(dynamic personnel) {
			if (personnel.LeadFlag == 1) {
				return (int)UserType.Lead;
			}
			//return personnel.SupervisorFlag == 1 ? (int)UserType.Technician : (int)UserType.Supervisor;
			return personnel.SupervisorFlag == 1 ? (int)UserType.Supervisor : (int)UserType.Technician;
		}

		private async Task<dynamic> GetUser(dynamic fesaUserId) {
			var client = new RestClient(_identityApi);
			var request = new RestRequest("getbyfesauserid");
			request.AddParameter("fesauserid", fesaUserId);
			var response = await client.ExecuteGetTaskAsync(request);
			return response.StatusCode != HttpStatusCode.OK ? null : Deserliaze(response.Content);
		}

		private dynamic Deserliaze(string json) {
			return JsonConvert.DeserializeObject<IDictionary<string, dynamic>>(json).FirstOrDefault().Value;
		}


		//SUPERVISOR
		[HttpPost]
		public async Task<IHttpActionResult> GetOts([FromBody]Request requestBody) {
			try {
				//if (string.IsNullOrEmpty(item.UserId))
				//	return BadRequest();

				//VALIDAR SI ES SUPERVISOR 

				//FALTA FUNCIONALIDAD

				//OBTENER OTS DE ACUERDO A LOS FILTROS 
				var restClient = new RestClient(_assignmentApi);
				var request = new RestRequest("getbyots") {
					JsonSerializer = new DynamicJsonSerializer()
				};
				request.AddJsonBody(new {
					request = requestBody
				});
				//assignmentRequest.AddParameter("assignment", request.Assignment);
				//assignmentRequest.AddParameter(nameof(request.PageIndex), request.PageIndex);
				//assignmentRequest.AddParameter(nameof(request.PageSize), request.PageSize);
				var response = await restClient.ExecutePostTaskAsync(request);
				if (response.StatusCode != HttpStatusCode.OK)
					return Ok(new {
						ErrorCode = response.StatusCode,
						response.ErrorMessage
					});

				var result = JsonConvert.DeserializeObject<IDictionary<string, ResponseAll>>(response.Content)
					.FirstOrDefault()
					.Value;

				if (result.Assignments.Count == 0) {
					return Ok(new ResponseAll() {
						Message = "No se encontraron OTs asignadas"
					});
				}

				var assignments = result.Assignments.Select(raw => ConvertToAssignmentWEB(raw, requestBody.Assignment.UserId)).Cast<Assignment>().ToList();
				if (assignments != null && assignments.Count > 0) {
					result.Assignments = new List<Assignment>(assignments);
				}

				return Ok(result);
			} catch (Exception ex) {
				Log.Write(ex);
				return InternalServerError();
			}
		}

		[HttpPost]
		public async Task<IHttpActionResult> GetDetailOT([FromBody]Assignment item) {
			try {
				if (string.IsNullOrEmpty(item.UserId) || string.IsNullOrEmpty(item.Id))
					return BadRequest();

				//VALIDAR SI ES SUPERVISOR 
				//FALTA FUNCIONALIDAD

				//OBTENER OTS DE ACUERDO A LOS FILTROS 
				var restClient = new RestClient(_assignmentApi);
				var assignmentRequest = new RestRequest("getbyid");
				assignmentRequest.AddParameter("id", item.Id);
				var response = await restClient.ExecuteGetTaskAsync(assignmentRequest);
				if (response.StatusCode != HttpStatusCode.OK)
					return Ok(new {
						ErrorCode = response.StatusCode,
						response.ErrorMessage
					});

				var result = JsonConvert.DeserializeObject<IDictionary<string, Assignment>>(response.Content)
					.FirstOrDefault()
					.Value;

				if (result == null && string.IsNullOrEmpty(result.Id)) {
					return Ok(new ResponseAll() {
						Message = "No se encontró el detalle de la OT"
					});
				}

				result = ConvertToAssignmentWEB(result, item.UserId);
				if (result == null && !string.IsNullOrEmpty(result.Id)) {
					return Ok(new {
						Assignment = result,
						Message = "No se encontró el detalle de la OT"
					});
				}

				return Ok(new {
					Assignment = result,
					Message = "OK"
				});

			} catch (Exception ex) {
				Log.Write(ex);
				return InternalServerError();
			}
		}

		[HttpPost]
		public async Task<IHttpActionResult> GetDetailOTAD([FromBody]Assignment item) {
			try {
				if (string.IsNullOrEmpty(item.UserId) || string.IsNullOrEmpty(item.Id))
					return BadRequest();

				//VALIDAR SI ES SUPERVISOR 
				//FALTA FUNCIONALIDAD

				//OBTENER OTS DE ACUERDO A LOS FILTROS 
				var restClient = new RestClient(_assignmentApi);
				var assignmentRequest = new RestRequest("getdetailtechnician");
				assignmentRequest.AddParameter("assignmentId", item.Id);
				assignmentRequest.AddParameter(nameof(item.UserId), item.UserId);
				var response = await restClient.ExecuteGetTaskAsync(assignmentRequest);

				if (response.StatusCode != HttpStatusCode.OK)
					return Ok(new {
						ErrorCode = response.StatusCode,
						response.ErrorMessage
					});

				var result = JsonConvert.DeserializeObject<IDictionary<string, ResponseAll>>(response.Content)
					.FirstOrDefault()
					.Value;

				if (result == null) {
					return Ok(new ResponseAll() {
						Message = "No se encontró el detalle del técnico"
					});
				}

				return Ok(result);

			} catch (Exception ex) {
				Log.Write(ex);
				return InternalServerError();
			}
		}

		[HttpGet]
		public async Task<IHttpActionResult> GetReportById(string userId, string assignmentId, int rol) {
			try {
				if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(userId))
					return BadRequest();

				//VALIDAR SI ES SUPERVISOR 
				//FALTA FUNCIONALIDAD

				//OBTENER OTS DE ACUERDO A LOS FILTROS 
				var restClient = new RestClient(_assignmentApi);
				var assignmentRequest = new RestRequest("getreportbyid");
				assignmentRequest.AddParameter(nameof(assignmentId), assignmentId);
				assignmentRequest.AddParameter(nameof(rol), rol);
				var response = await restClient.ExecuteGetTaskAsync(assignmentRequest);

				if (response.StatusCode != HttpStatusCode.OK)
					return Ok(new {
						ErrorCode = response.StatusCode,
						response.ErrorMessage
					});

				var result = JsonConvert.DeserializeObject<IDictionary<string, Assignment>>(response.Content)
					.FirstOrDefault().Value;

				if (result == null) {
					return Ok(new ResponseAll() {
						Message = "No se encontró el informe"
					});
				}
				result = GetContactFerreyros(result);

				return Ok(new ResponseAll {
					Message = "OK",
					Assignment = result
				});

			} catch (Exception ex) {
				Log.Write(ex);
				return InternalServerError();
			}
		}

		[HttpPost]
		public async Task<IHttpActionResult> UpdateExecutiveReport([FromBody]Report report) {
			if (report == null)
				return Ok(new {
					Content = "",
					Status = HttpStatusCode.BadRequest,
					ErrorMessage = "Parameter cannot be null"
				});

			var client = new RestClient(_assignmentApi);
			var request = new RestRequest("updateexecutivereport") { JsonSerializer = new DynamicJsonSerializer() };
			request.AddJsonBody(new {
				report
			});
			var response = await client.ExecutePostTaskAsync(request);

			return Ok(new {
				Content = Deserliaze(response.Content),
				Status = HttpStatusCode.OK,
				ErrorMessage = "Successfully Saved!"
			});
		}

		private Assignment ConvertToAssignmentWEB(Assignment raw, string userId) {

			try {
				#region Workorder

				var client = new RestClient(_workorderApi);
				var request = new RestRequest("getorderbyid");
				request.AddParameter("Id", raw.WorkOrderId);

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
				#region Machine

				var machine = new Machine {
					Id = wo.Machinery.Id,
					AssignmentId = raw.Id,
					Brand = wo.Machinery.Brand,
					Model = wo.Machinery.Model,
					SerialNumber = wo.Machinery.SerialNumber,
					TotalHoursFunction = wo.Machinery.LifeHours
				};

				#endregion
				#region Contact Ferreyros

				string id = raw.Id;
				client = new RestClient(_identityApi);
				request = new RestRequest("getusersbyidassignment");
				//request = new RestRequest("getuserbyid");
				request.AddParameter("id", id);
				response = client.ExecuteGetTaskAsync(request).GetAwaiter().GetResult();
				var contactsFerreyros =
					JsonConvert.DeserializeObject<IDictionary<string, dynamic>>(response.Content)
						.FirstOrDefault()
						.Value;

				#endregion


				#region BuildAssignment

				var TechnicalContacts = new List<Contact>();
				if (wo.TechnicalContacts != null || wo.TechnicalContacts != "") {
					foreach (var item in wo.TechnicalContacts) {
						TechnicalContacts.Add(new Contact() {
							AssignmentId = raw.Id,
							Id = item.Id,
							Name = item.Name,
							LastName = item.LastName,
							Charge = item.Charge,
							Phone = item.Phone,
							Email = item.Email,
						});
					}
				}

				var FerreyrosContacts = new List<Contact>();
				if (contactsFerreyros != null || contactsFerreyros != "") {
					foreach (var item in contactsFerreyros) {
						FerreyrosContacts.Add(new Contact() {
							AssignmentId = raw.Id,
							Id = item.Id, // item.Id,
							Name = item.Name,
							Charge = item?.Role.Name,
							Phone = item.Phone,
							Email = item.Email,
							IsFerreyrosContact = true,
							AssignmentStatus = item.UserStatus
						});
					}
				}

				var stringFormat = "dd/MM/yyyy H:mm:ss";

				var assignment = new Assignment {
					Id = raw.Id,
					AssignmentType = raw.AssignmentType,
					Status = raw.Status,
					WorkOrderNumber = wo.Code,
					WorkOrderId = raw.WorkOrderId,
					Description = raw?.Description,
					//Priority = raw.Priority,
					CompanyName = customer?.BusinessName,
					RequestDate = DateTime.ParseExact(raw.RequestDate.ToString(stringFormat), stringFormat, CultureInfo.InvariantCulture),
					StartDate = DateTime.ParseExact(raw.StartDate.ToString(stringFormat), stringFormat, CultureInfo.InvariantCulture),
					//EndDate = DateTime.ParseExact(raw.EndDate.ToString(stringFormat), stringFormat, CultureInfo.InvariantCulture),
					EstimatedStartDate = DateTime.ParseExact(raw.EstimatedStartDate.ToString(stringFormat), stringFormat, CultureInfo.InvariantCulture),
					EstimatedEndDate = DateTime.ParseExact(raw.EstimatedEndDate.ToString(stringFormat), stringFormat, CultureInfo.InvariantCulture),
					Machine = machine,
					TechnicalContacts = new List<Contact>(TechnicalContacts),
					FerreyrosContacts = new List<Contact>(FerreyrosContacts),
					Active = raw.Active,
					Rating = raw?.Rating
				};
				if (raw.EndDate.HasValue) {
					assignment.EndDate = DateTime.ParseExact(raw.EndDate.Value.ToString(stringFormat), stringFormat, CultureInfo.InvariantCulture);
				}

				if (raw.Location != null) {
					assignment.Location = new Location {
						AssignmentId = raw.Id,
						Department = raw?.Location.Department,
						Id = raw?.Location.Id,
						Ubicacion = raw?.Location.Ubicacion,
						Province = raw?.Location.Province,
						District = raw?.Location.District
					};
				}

				if (raw.Report != null) {
					assignment.Report = raw.Report;
				}



				#endregion

				return assignment;
			} catch (Exception ex) {
				Log.Write(ex);
				return new Assignment();
			}

		}

		private Assignment GetContactFerreyros(Assignment raw) {

			try {

				#region Contact Ferreyros

				string id = raw.Id;
				var client = new RestClient(_identityApi);
				var request = new RestRequest("getusersbyidassignment");
				//request = new RestRequest("getuserbyid");
				request.AddParameter("id", id);
				var response = client.ExecuteGetTaskAsync(request).GetAwaiter().GetResult();
				var contactsFerreyros =
					JsonConvert.DeserializeObject<IDictionary<string, dynamic>>(response.Content)
						.FirstOrDefault()
						.Value;

				#endregion


				#region BuildAssignment

				var FerreyrosContacts = new List<Contact>();
				if (contactsFerreyros != null || contactsFerreyros != "") {
					foreach (var item in contactsFerreyros) {
						FerreyrosContacts.Add(new Contact() {
							AssignmentId = raw.Id,
							Id = item.Id, // item.Id,
							Name = item.Name,
							Charge = item?.Role.Name,
							Phone = item.Phone,
							Email = item.Email,
							IsFerreyrosContact = true,
							AssignmentStatus = item.UserStatus
						});
					}
				};

				raw.FerreyrosContacts = new List<Contact>(FerreyrosContacts);

				#endregion

				return raw;
			} catch (Exception ex) {
				Log.Write(ex);
				return new Assignment();
			}

		}


		[HttpPost]
		public async Task<IHttpActionResult> UpdateArchiveInformFinal([FromBody]Report report) {
			try {
				if (report == null)
					return Ok(new {
						Content = "",
						Status = HttpStatusCode.BadRequest,
						ErrorMessage = "Parameter cannot be null"
					});

                var client = new RestClient(_assignmentApi);
                var request = new RestRequest("updatearchiveinformfinal") { JsonSerializer = new DynamicJsonSerializer() };
                if (report.NameFile != "1") {

                    string container = "report";

                    dynamic dynamicAttachment = new ExpandoObject();
                    dynamicAttachment.File = report.FileData;
                    dynamicAttachment.Container = container;
                    dynamicAttachment.Name = report.NameFile;
                    dynamicAttachment.FileUrl = "";

                    var urlFile = await StorageHelperAzure.UploadFileToAzure(dynamicAttachment
                   , "DefaultEndpointsProtocol=https;AccountName=serviciodecampo;AccountKey=Ev0LRkUoub7rzwDGR8N1xF6dgALm+O8Pzv767eXNUfXYRtyNXuFX5HilPj9+fpcMUxMCVSkfXGo484Y7xvvTmA==;EndpointSuffix=core.windows.net");
                    report.UrlFile = urlFile.FileUrl;
                    report.FileData = new byte[0];
                    
                }
                request.AddJsonBody(new {
                    report
                });
                var response = await client.ExecutePostTaskAsync(request);

				return Ok(new {
					Report = Deserliaze(response.Content),
					Status = HttpStatusCode.OK,
					ErrorMessage = "Successfully Saved!"
				});
			} catch (Exception ex) {
				Log.Write(ex);
				return Ok(new {
					Content = "",
					Status = HttpStatusCode.BadRequest,
					ErrorMessage = "Server Error"
				});
			}

		}

		[HttpPost]
		public async Task<IHttpActionResult> AddOrUpdateSendFileToReportSupervisor([FromBody]Report reportdata) {
			try {
				if (reportdata == null)
					return Ok(new {
						Content = "",
						Status = HttpStatusCode.BadRequest,
						ErrorMessage = "Parameter cannot be null"
					});
				var client = new RestClient(_assignmentApi);

				string container = "report";

				Report reportlocalURL = new Report() {
					Id = reportdata.Id,
					AssignmentId = reportdata.AssignmentId,
					UrlFile = reportdata.UrlFile,
					FileData = reportdata.FileData,
					NameFile = reportdata.NameFile,
					ModifiedBy = reportdata.ModifiedBy
				};
				Report reportlocal = new Report() {
					Id = reportdata.Id,
					AssignmentId = reportdata.AssignmentId,
					UrlFile = reportdata.UrlFile,
					Comment2 = reportdata.Comment2,
					Check = reportdata.Check,
					ActionType = reportdata.ActionType,
					ModifiedBy = reportdata.ModifiedBy
				};

				//If has changed technical final inform

				if (!string.IsNullOrEmpty(reportlocalURL.NameFile)) {
					dynamic dynamicAttachment = new ExpandoObject();
					dynamicAttachment.File = reportlocalURL.FileData;
					dynamicAttachment.Container = container;
					dynamicAttachment.Name = reportlocalURL.NameFile;
					dynamicAttachment.FileUrl = "";
					var urlFile = await StorageHelperAzure.UploadFileToAzure(dynamicAttachment
					, "DefaultEndpointsProtocol=https;AccountName=serviciodecampo;AccountKey=Ev0LRkUoub7rzwDGR8N1xF6dgALm+O8Pzv767eXNUfXYRtyNXuFX5HilPj9+fpcMUxMCVSkfXGo484Y7xvvTmA==;EndpointSuffix=core.windows.net");
					reportlocalURL.UrlFile = urlFile.FileUrl;
					reportlocalURL.FileData = new byte[0];
					var requestUrlUnformeFinal = new RestRequest("updateurlinformfinalsupervisor") { JsonSerializer = new DynamicJsonSerializer() };
					requestUrlUnformeFinal.AddJsonBody(new {
						report = reportlocalURL
					});

                    var responseFilesTechnical = await client.ExecutePostTaskAsync(requestUrlUnformeFinal);
                }
                    var requestfiles = new RestRequest("updatesendfiletoreportsupervisor") { JsonSerializer = new DynamicJsonSerializer() };

                    List<File> filesUpdated = new List<File>();
                    foreach (var item in reportdata.Files) {
                        File newFile = new File();
                        newFile = item;
                        
                        if (item.status ==0) { 
                            dynamic dynamicAttachmentFile = new ExpandoObject();
                            dynamicAttachmentFile.File = item.FileData;
                            dynamicAttachmentFile.Container = container;
                            dynamicAttachmentFile.Name = item.Name;
                            dynamicAttachmentFile.FileUrl = "";

                            var urlFileServ = await StorageHelperAzure.UploadFileToAzure(dynamicAttachmentFile
                           , "DefaultEndpointsProtocol=https;AccountName=serviciodecampo;AccountKey=Ev0LRkUoub7rzwDGR8N1xF6dgALm+O8Pzv767eXNUfXYRtyNXuFX5HilPj9+fpcMUxMCVSkfXGo484Y7xvvTmA==;EndpointSuffix=core.windows.net");
                            newFile.URL = urlFileServ.FileUrl;
                            newFile.FileData = new byte[0];
                        }  
                     filesUpdated.Add(newFile);
                    }
                    reportdata.Files = filesUpdated;

                    reportdata.FileData = new byte[] { };
                    requestfiles.AddJsonBody(new {
                        report = reportdata
                    });
                    var responseFiles = await client.ExecutePostTaskAsync(requestfiles);
                var request = new RestRequest("updateinformfinaldatasupervisor") { JsonSerializer = new DynamicJsonSerializer() };
                request.AddJsonBody(new {
                    report = reportlocal
                });
                var response = await client.ExecutePostTaskAsync(request);


				return Ok(new {
					Report = Deserliaze(response.Content),
					Status = HttpStatusCode.OK,
					ErrorMessage = "Successfully Saved!"
				});
			} catch (Exception ex) {
				Log.Write(ex);
				return Ok(new {
					Content = "",
					Status = HttpStatusCode.BadRequest,
					ErrorMessage = "Server Error"
				});
			}

		}

        [HttpPost]
        public async Task<IHttpActionResult> AddFileToReportSupervisor([FromBody]Report report) {
            try {
                if (report == null)
                    return Ok(new {
                        Content = "",
                        Status = HttpStatusCode.BadRequest,
                        ErrorMessage = "Parameter cannot be null"
                    });

                var client = new RestClient(_assignmentApi);
                string container = "report";
                if (report.Files.Count() > 0) {
                    var requestfiles = new RestRequest("updatesendfiletoreportsupervisor") { JsonSerializer = new DynamicJsonSerializer() };

                    List<File> filesUpdated = new List<File>();
                    foreach (var item in report.Files) {
                        File newFile = new File();
                        dynamic dynamicAttachmentFile = new ExpandoObject();
                        dynamicAttachmentFile.File = item.FileData;
                        dynamicAttachmentFile.Container = container;
                        dynamicAttachmentFile.Name = item.Name;
                        dynamicAttachmentFile.FileUrl = "";

                        var urlFileServ = await StorageHelperAzure.UploadFileToAzure(dynamicAttachmentFile
                       , "DefaultEndpointsProtocol=https;AccountName=serviciodecampo;AccountKey=Ev0LRkUoub7rzwDGR8N1xF6dgALm+O8Pzv767eXNUfXYRtyNXuFX5HilPj9+fpcMUxMCVSkfXGo484Y7xvvTmA==;EndpointSuffix=core.windows.net");
                        newFile = item;
                        newFile.URL = urlFileServ.FileUrl;
                        newFile.FileData = new byte[0];
                        filesUpdated.Add(newFile);
                    }
                    report.Files = filesUpdated;

                    report.FileData = new byte[] { };
                    requestfiles.AddJsonBody(new {
                        report = report
                    });
                    var responseFiles = await client.ExecutePostTaskAsync(requestfiles);
                    return Ok(new {
                        Report = Deserliaze(responseFiles.Content),
                        Status = HttpStatusCode.OK,
                        ErrorMessage = "Successfully Saved!"
                    });
                } 
                return Ok(new {
                    Report = "",
                    Status = HttpStatusCode.OK,
                    ErrorMessage = "Server Error"
                });
            } catch (Exception ex) {
                Log.Write(ex);
                return Ok(new {
                    Content = "",
                    Status = HttpStatusCode.BadRequest,
                    ErrorMessage = "Server Error"
                });
            }

		}

		[HttpPost]
		public async Task<IHttpActionResult> DeleteArchiveInformFinal([FromBody]Report report) {
			try {
				if (report == null)
					return Ok(new {
						Content = "",
						Status = HttpStatusCode.BadRequest,
						ErrorMessage = "Parameter cannot be null"
					});

				var client = new RestClient(_assignmentApi);
				var request = new RestRequest("deletearchiveinformfinal") { JsonSerializer = new DynamicJsonSerializer() };
				report.FileData = new byte[0];
				request.AddJsonBody(new {
					report
				});
				var response = await client.ExecutePostTaskAsync(request);
				return Ok(new {
					Report = Deserliaze(response.Content),
					Status = HttpStatusCode.OK,
					ErrorMessage = "Successfully Saved!"
				});
			} catch (Exception ex) {
				Log.Write(ex);
				return Ok(new {
					Content = "",
					Status = HttpStatusCode.BadRequest,
					ErrorMessage = "Server Error"
				});
			}

		}

		[HttpPost]
		public async Task<IHttpActionResult> DeleteFileReportSupervisor([FromBody]File file) {
			try {
				if (file == null)
					return Ok(new {
						Content = "",
						Status = HttpStatusCode.BadRequest,
						ErrorMessage = "Parameter cannot be null"
					});

				var client = new RestClient(_assignmentApi);
				var request = new RestRequest("deletefilereportsupervisor") { JsonSerializer = new DynamicJsonSerializer() };
				request.AddJsonBody(new {
					file
				});
				var response = await client.ExecutePostTaskAsync(request);
				return Ok(new {
					Report = Deserliaze(response.Content),
					Status = HttpStatusCode.OK,
					ErrorMessage = "Successfully Saved!"
				});
			} catch (Exception ex) {
				Log.Write(ex);
				return Ok(new {
					Content = "",
					Status = HttpStatusCode.BadRequest,
					ErrorMessage = "Server Error"
				});
			}

		}



	}
}
