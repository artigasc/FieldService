using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Http;
using FESA.SCM.Common;
using FESA.SCM.ServiceGateway.DTO;
using FESA.SCM.ServiceGateway.Models;
using Newtonsoft.Json;
using RestSharp;
using FESA.SCM.ServiceGateway.Helper;
using Newtonsoft.Json.Linq;

namespace FESA.SCM.ServiceGateway.Controllers {
    public class MailsController: ApiController {
        private readonly string _assignmentApi;
        private readonly string _workorderApi;
        private readonly string _customerApi;
        private readonly string _identityApi;
        public MailsController() {
            _assignmentApi = ConfigurationManager.AppSettings["assignment-api"];
            _workorderApi = ConfigurationManager.AppSettings["workorder-api"];
            _customerApi = ConfigurationManager.AppSettings["customer-api"];
            _identityApi = ConfigurationManager.AppSettings["identity-api"];
            ;
        }

        [HttpPost]
        public IHttpActionResult AssignmentEnd([FromBody]MailNotification mailNotification) {
            try {
                var client = new RestClient(_assignmentApi);
                var request = new RestRequest("getbyid");
                request.AddParameter("id", mailNotification.AssignmentId);
                var response = client.ExecuteGetTaskAsync(request).GetAwaiter().GetResult();
                var assignment = Deserialize(response.Content);
                SendMail(assignment, null, mailNotification.userId);
                return Ok();
            } catch (Exception ex) {
                Log.Write(ex);
                return InternalServerError();
            }
        }

        [HttpPost]
        public IHttpActionResult ActivityNotification([FromBody]MailRequest mailRequest) {
            try {
                var client = new RestClient(_assignmentApi);
                var request = new RestRequest("getbyid");
                request.AddParameter("id", mailRequest.AssignmentId);
                var response = client.ExecuteGetTaskAsync(request).GetAwaiter().GetResult();
                var assignment = Deserialize(response.Content);
                SendMail(assignment, mailRequest.ActivityType, mailRequest.userId);
                return Ok(response);
            } catch (Exception ex) {
                Log.Write(ex);
                return InternalServerError();
            }
        }

        private dynamic Deserialize(string json) {
            return JsonConvert.DeserializeObject<IDictionary<string, dynamic>>(json).FirstOrDefault().Value;
        }

        private void SendMail(dynamic assignment, ActivityType? type = null, string userId = null) {

            #region Workorder

            var client = new RestClient(_workorderApi);
            var request = new RestRequest("getorderbyid");
            request.AddParameter("Id", assignment.WorkOrderId);
            var response = client.ExecuteGetTaskAsync(request).GetAwaiter().GetResult();
            var workOrder = Deserialize(response.Content);

            #endregion

            #region Customer

            var customer = workOrder.Machinery;

            #endregion

            #region Machine

            var machine = new Machine() {
                Id = customer.Id,
                AssignmentId = assignment.Id,
                Brand = customer.Brand,
                Model = customer.Model,
                SerialNumber = customer.SerialNumber,
                TotalHoursFunction = customer.LifeHours
            };

            #endregion

            #region Actividades

            client = new RestClient(_assignmentApi);
            request = new RestRequest("GetActivityByIdAndUserId");
            request.AddParameter("Id", assignment.Id);
            request.AddParameter("userId", userId);
            response = client.ExecuteGetTaskAsync(request).GetAwaiter().GetResult();
            var activity = JsonConvert.DeserializeObject<IDictionary<string, List<Activity>>>(response.Content)
                    .FirstOrDefault()
                    .Value;

			#endregion

			#region ValidateActivity

			#region New Version

			var notify = false;
			int countExists = activity.Where(x => x.ActivityType == ActivityType.FieldReport
							|| x.ActivityType == ActivityType.FieldService
							|| x.ActivityType == ActivityType.StandByClient
							|| x.ActivityType == ActivityType.StandByFesa).Count();

			int OnlyTime = 0;

			if (countExists == 0) {
				switch (type) {
					case ActivityType.Traveling:
						OnlyTime = activity.Where(x => x.ActivityType == type).Count();
						break;
					case ActivityType.Driving:
						OnlyTime = activity.Where(x => x.ActivityType == type).Count();
						break;
				}
				if (OnlyTime == 1) {
					notify = true;
				}
			} else {
				if (type == ActivityType.Traveling || type == ActivityType.Driving) {
					notify = false;
				} else {
					notify = true;
				}
			}

			#endregion

			#region LastVersion

			//int travel = 0, driving = 0;
			//var notify = false;

			//foreach (var item in activity) {

			//    var a = item.ActivityType.ToString();

			//    if (item.ActivityType == ActivityType.Traveling) {
			//        travel++;
			//    }
			//    if (item.ActivityType == ActivityType.Driving) {
			//        driving++;
			//    }

			//}

			//if (type == ActivityType.Traveling || type == ActivityType.Driving) {
			//    switch (type) {
			//        case ActivityType.Traveling:
			//            if (travel <= 1) {
			//                notify = true;
			//            }
			//            break;
			//        case ActivityType.Driving:
			//            if (driving <= 1) {
			//                notify = true;
			//            }
			//            break;
			//    }
			//} else {
			//    notify = true;
			//}

			#endregion

			#endregion

			#region Contact Ferreyros

			string id = assignment.Id;
            client = new RestClient(_identityApi);
            request = new RestRequest("getusersbyidassignment");
            request.AddParameter("id", id);
            response = client.ExecuteGetTaskAsync(request).GetAwaiter().GetResult();
            var contactsFerreyros =
                JsonConvert.DeserializeObject<IDictionary<string, dynamic>>(response.Content)
                    .FirstOrDefault()
                    .Value;

            dynamic lead = null;
            dynamic supervisor = null;

            foreach (var item in contactsFerreyros) {
                if (item.UserType == 2) {
                    lead = item;
                } else if (item.UserType == 1) {
                    supervisor = item;
                }
            }

			#endregion

			#region Correo Destinatario

			#region PRODUCCION

			//var userNames = new List<string>();
			//foreach (var item in contactsFerreyros) {
			//	if (item.UserType == 1) {
			//		userNames.Add(item.Email.ToString());
			//	}
			//}
			//foreach (var item in workOrder.TechnicalContacts) {
			//	userNames.Add(item.Email.ToString());
			//}

			#endregion

			#region TEST

			var userNames = new List<string>();
			userNames = new List<string>();
			userNames.Add("antonio.zarate@cmscloud.pe");
			userNames.Add("calidaddesistemas@ferreyros.com.pe");

			#endregion

			string logo = "http://imgur.com/XAjtjmj.jpg";

            #endregion

            #region Formato Fecha

            var timezone = TimeZoneInfo.FindSystemTimeZoneById("Hawaiian Standard Time");
            var fecha = DateTime.Now.AddHours(5);
            var now = TimeZoneInfo.ConvertTime(fecha, timezone);

            #endregion

            SendEmailClient mail = new SendEmailClient();
            if (type == null) {
                var subject = string.Format(Constants.Mailsubject.FieldReport, workOrder.Code);

                var body = string.Format(Constants.MailBodies.FieldReport
                                           , now
                                           , assignment?.Description
                                           , machine?.Model
                                           , machine?.SerialNumber
                                           , ($"{assignment.Location.Department} {assignment.Location.Province} {assignment.Location.District}")
                                           , lead?.Name
                                           , lead?.Celullar
                                           , lead?.Email
                                           , supervisor?.Name
                                           , supervisor?.Email
                                           , logo);
                if (userId == lead.Id.Value)
                    mail.PostSendEmail(userNames, subject, body);
                return;
            }

            if (notify) {

                switch (type) {
                    case ActivityType.Traveling:
                        var subject1 = string.Format(Constants.Mailsubject.Traveling, workOrder.Code);
                        string assignedTec1 = string.Empty;
                        foreach (var item in contactsFerreyros) {
                            var isSupervisor = item.UserType == 1 ? "( Supervisor )" : "";
                            assignedTec1 = assignedTec1 + $"<li> {item.Name} {item.LastName} {isSupervisor}</li>";
                        }

                        var body1 = string.Format(Constants.MailBodies.Traveling
                                                , now
                                                , ($"{assignment.Location.Department} {assignment.Location.Province} {assignment.Location.District}")
                                                , assignment?.Description
                                                , machine?.Model
                                                , machine?.SerialNumber
                                                , assignedTec1
                                                , lead?.Name
                                                , lead?.Celullar
                                                , lead?.Email
                                                , supervisor?.Name
                                                , supervisor?.Email
                                                , logo);

                        if (userId == lead.Id.Value)
                            mail.PostSendEmail(userNames, subject1, body1);

                        break;
                    case ActivityType.Driving:

                        var subject2 = string.Format(Constants.Mailsubject.Driving, workOrder.Code);
                        string assignedTec2 = string.Empty;
                        foreach (var item in contactsFerreyros) {
                            var isSupervisor = item.UserType == 1 ? "( Supervisor )" : "";
                            assignedTec2 = assignedTec2 + $"<li> {item.Name} {item.LastName} {isSupervisor}</li>";
                        }

                        var body2 = string.Format(Constants.MailBodies.Driving
                                                , now
                                                , ($"{assignment.Location.Department} {assignment.Location.Province} {assignment.Location.District}")
                                                , assignment?.Description
                                                , machine?.Model
                                                , machine?.SerialNumber
                                                , assignedTec2
                                                , lead?.Name
                                                , lead?.Celullar
                                                , lead?.Rpm
                                                , lead?.Email
                                                , supervisor?.Name
                                                , supervisor?.Celullar
                                                , supervisor?.Email
                                                , logo);
                        if (userId == lead.Id.Value)
                            mail.PostSendEmail(userNames, subject2, body2);

                        break;
                    case ActivityType.FieldService:
                        var subject3 = string.Format(Constants.Mailsubject.FieldService, workOrder.Code);

                        var body3 = string.Format(Constants.MailBodies.FieldService
                                                , now
                                                , ($"{assignment.Location.Department} {assignment.Location.Province} {assignment.Location.District}")
                                                , assignment?.Description
                                                , machine?.Model
                                                , machine?.SerialNumber
                                                , lead == null ? "" : lead.Name
                                                , lead == null ? "" : lead.Celullar
                                                , lead == null ? "" : lead.Rpm
                                                , lead == null ? "" : lead.Email
                                                , supervisor == null ? "" : supervisor.Name
                                                , supervisor == null ? "" : supervisor.Celullar
                                                , supervisor == null ? "" : supervisor.Email
                                                , logo);
                        if (userId == lead.Id.Value)
                            mail.PostSendEmail(userNames, subject3, body3);
                        break;
                    case ActivityType.StandByClient:
                        var subject4 = string.Format(Constants.Mailsubject.StandByClient, workOrder.Code);

                        var body4 = string.Format(Constants.MailBodies.StandByClient
                                                , now
                                                , assignment?.Description
                                                , machine?.Model
                                                , machine?.SerialNumber
                                                , supervisor?.Name
                                                , supervisor?.Email
                                                , lead?.Name
                                                , lead?.Celullar
                                                , lead?.Email
                                                , logo);
                        if (userId == lead.Id.Value)
                            mail.PostSendEmail(userNames, subject4, body4);
                        break;
                    case ActivityType.StandByFesa:
                        var subject5 = string.Format(Constants.Mailsubject.StandByFesa, workOrder.Code);

                        var body5 = string.Format(Constants.MailBodies.StandByFesa
                                                , now
                                                , assignment?.Description
                                                , machine?.Model
                                                , machine?.SerialNumber
                                                , supervisor?.Name
                                                , supervisor?.Celullar
                                                , supervisor?.Email
                                                , lead?.Name
                                                , lead?.Celullar
                                                , lead?.Email
                                                , logo);
                        if (userId == lead.Id.Value)
                            mail.PostSendEmail(userNames, subject5, body5);
                        break;
                }
            }
        }



    }
}
