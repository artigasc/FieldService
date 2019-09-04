using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using FESA.SCM.Common;
using FESA.SCM.FieldService.BE;
using FESA.SCM.FieldService.BE.ActivityBE;
using FESA.SCM.FieldService.BE.AssignmentBE;
using FESA.SCM.FieldService.BE.DocumentBE;
using FESA.SCM.FieldService.BL.BusinessInterfaces;
using FESA.SCM.FieldService.BL.Helpers;
using NReco.PdfGenerator;

namespace FESA.SCM.FieldService.BL.BusinessServices {
	public class AssignmentService : IAssignmentService {
		#region Members
		private readonly IAssignmentRepository _assignmentRepository;
		private readonly IActivityRepository _activityRepository;
		private readonly IDocumentRepository _documentRepository;
		private readonly List<Attachment> listAttachments;
		private readonly List<string> listUsers;
		private readonly List<string> listUsersCC;
		private readonly EmailHelper email;

		#endregion
		#region Constructor

		public AssignmentService(IAssignmentRepository assignmentRepository, IActivityRepository activityRepository,
			IDocumentRepository documentRepository) {
			if (assignmentRepository == null)
				throw new ArgumentNullException(nameof(assignmentRepository));

			_assignmentRepository = assignmentRepository;

			if (activityRepository == null)
				throw new ArgumentNullException(nameof(activityRepository));

			_activityRepository = activityRepository;

			if (documentRepository == null)
				throw new ArgumentNullException(nameof(documentRepository));

			_documentRepository = documentRepository;

			listAttachments = new List<System.Net.Mail.Attachment>();
			listUsers = new List<string>();
			listUsersCC = new List<string>();
			email = new EmailHelper("", "", "", "");
		}

		#endregion
		#region Methods
		public IList<Assignment> GetAll() {
			return _assignmentRepository.GetAll();
		}

		public IList<Assignment> GetPaginated(Assignment assignment, int pageIndex, int pageSize, out int totalRows) {
			return _assignmentRepository.GetPaginated(assignment, pageIndex, pageSize, out totalRows);
		}

		public Assignment GetById(string id) {
			var assignment = _assignmentRepository.GetById(id);
			//assignment.Documents = _documentRepository.GetDocumentsByAssignment(id).ToList();
			//assignment.Activities = _activityRepository.GetActivitiesByAssignment(id).ToList();
			assignment.AssignedPersonnel = _assignmentRepository.GetByAssignmentId(id).ToList();
			return assignment;
		}

		public string AddAssignment(Assignment assignment) {
			string id = "";
			var _assignment = _assignmentRepository.GetByWorkOrder(assignment.WorkOrderId);
			if (_assignment == null) {
				id = Guid.NewGuid().ToString();
				assignment.Id = id;
				assignment.CreationDate = DateTime.Now;
				assignment.StartDate = assignment.EstimatedStartDate;
				_assignmentRepository.Add(assignment);
				assignment.Active = false;
			} else {
				id = _assignment.Id;
				assignment.Active = true;
			}

			assignment.AssignedPersonnel.ForEach(p => {
				p.AssignmentId = id; //assignment.Id;
			});
			if (!assignment.Active) {
				assignment.Location.AssignmentId = id; // assignment.Id;
				_assignmentRepository.AddLocation(assignment.Location);
			}
			_assignmentRepository.AddPersonnel(assignment.AssignedPersonnel);
			return id;
			//return assignment.Id;
		}

		public void UpdateAssignment(Assignment assignment) {
			_assignmentRepository.Update(assignment);
		}

		public void DeleteAssignment(string id, string modifiedBy, DateTime lastModification) {
			_assignmentRepository.Delete(id, modifiedBy, lastModification);
		}

		//MODIFY
		public ResponseAll GetByUserId(string userId) {
			var result = new ResponseAll();
			var assignments = _assignmentRepository.GetByUserId(userId);
			if (assignments == null) {
				result.Message = "No se encontraron OT asignadas";
				return result;
			}
			result.Assignments = new List<Assignment>(assignments);
			var activitiesEntity = _activityRepository.GetAllEntityActivity();
			if (activitiesEntity != null) {
				result.ActivitiesEntity = new List<ActivityEntity>(activitiesEntity);
			}

			var documentsEntity = _documentRepository.GetAllEntityDocument();
			if (documentsEntity != null) {
				result.DocumentsEntity = new List<DocumentEntity>(documentsEntity);
			}

			foreach (var item in result.Assignments) {
				var activities = _activityRepository.GetActivitiesByAssignmentByUserId(item.Id, userId);
				if (activities != null) {
					item.Activities = new List<Activity>(activities);
				}
				var documents = _documentRepository.GetDocumentsByAssignmentByUserId(item.Id, userId);
				if (documents != null) {
					item.Documents = new List<Document>(documents);
				}
			}
			return result;
		}

		public IList<Activity> GetActivityByIdAndUserId(string Id, string userId) {
			var activities = _activityRepository.GetActivitiesByAssignmentByUserId(Id, userId);
			return activities;
		}

		public async Task<int> SyncAssignments(IList<Assignment> assignments) {
			int i = 0;
			Activity activityOnly = new Activity();
			if (assignments != null && assignments.Count > 0) {
				var userId = assignments.Select(x => x.UserId).FirstOrDefault();
				//_assignmentRepository.SyncAssignments(assignments);
				foreach (var assignment in assignments) {
					i = _assignmentRepository.UpdateStatusByLider(assignment);
					_assignmentRepository.UpdatePersonnelStatus(userId, assignment.Id, assignment.Status);
					if (assignment.Activities != null && assignment.Activities.Count > 0) {
						_assignmentRepository.UpdateDateStart(assignment);
						string messageOfflineTemplate = "<li> {0}: {1} </li>";
						string messageOffline = "";
						if (assignment.Activities.Count == 1) {
							var activity = assignment.Activities[0];
							if (activity.ActivityState == ActivityState.Completed) {
								activity.Online = true;
								_activityRepository.InsertActivities(new List<Activity>() { activity });
							}
							await SendEmailWithActivity(activity);
						} else if (assignment.Activities.Count > 1) {
							CultureInfo ci = new CultureInfo("es-PE");
							foreach (var item in assignment.Activities) {
								if (item.ActivityState == ActivityState.Completed) {
									item.Online = true;
								}
								if (item.ActivityType == ActivityType.Traveling) {
									messageOffline = messageOffline + string.Format(messageOfflineTemplate, "Inicio de viaje", item.StartDate.ToString("dd-MM-yyyy", ci));
								} else if (item.ActivityType == ActivityType.Driving) {
									messageOffline = messageOffline + string.Format(messageOfflineTemplate, "Inicio de ruta", item.StartDate.ToString("dd-MM-yyyy", ci));
								} else if (item.ActivityType == ActivityType.FieldService) {
									messageOffline = messageOffline + string.Format(messageOfflineTemplate, "Inicio de servicio", item.StartDate.ToString("dd-MM-yyyy", ci));
								} else if (item.ActivityType == ActivityType.StandByClient) {
									messageOffline = messageOffline + string.Format(messageOfflineTemplate, "Inicio de espera", item.StartDate.ToString("dd-MM-yyyy", ci));
								} else if (item.ActivityType == ActivityType.FieldReport) {
									messageOffline = messageOffline + string.Format(messageOfflineTemplate, "Fin de servicio", item.EndDate.Value.ToString("dd-MM-yyyy", ci));
								}
							}
							i = _activityRepository.InsertActivities(assignment.Activities);
							await SendEmailOffline(assignment, messageOffline);
						}

						//var activities = assignment.Activities.Where(x => !x.Active).OrderByDescending(x => x.StartDate).ToList();
						//foreach (var item in activities) {
						//	if (item.ActivityState == ActivityState.Completed) {
						//		item.Active = true;
						//	} else if (item.ActivityState == ActivityState.Active) {
						//		activityOnly = item;
						//	}
						//}
						//if (activities.Count != 0) {
						//	activities.Remove(activityOnly);
						//	_activityRepository.InsertActivities(activities);
						//	_activityRepository.InsertActivities(new List<Activity>() { activityOnly });
						//}
						foreach (var activity in assignment.Activities) {
							if (activity.Traces != null && activity.Traces.Count > 0) {
								_activityRepository.InsertTraces(activity.Traces);
							}
						}
					}

					if (assignment.Reports != null && assignment.Reports.Count > 0) {
						foreach (var item in assignment.Reports) {
							if (!string.IsNullOrEmpty(item.UrlSign)) {
								var file = Convert.FromBase64String(item.UrlSign);
								var urlSign = await InsertFile(assignment.WorkOrderNumber + "-Sign", "act", file);
								item.UrlSign = urlSign;
							}
							var dynamicFile = await GenerateAct(item, 1);
							item.UrlAct = dynamicFile.FileUrl;
							item.Online = true;
							_assignmentRepository.InsertReport(item);
						}
					}

					if (assignment.Documents != null && assignment.Documents.Count > 0) {
						_documentRepository.InsertDocuments(assignment.Documents);
					}
				}
				//ACTUALIZAR ESTADO DEL TÉCNICO : 0 = DISPONIBLE, 1 = ASIGNADO, 2 = EN CURSO 
				var intCount = _assignmentRepository.GetAssignmentCountByUser(userId);
				var userStatus = intCount > 0 ? 1 : 0;
				_assignmentRepository.UpdateStatusUser(userId, userStatus);
			}
			return i;
		}

		async Task<string> InsertFile(string name, string container, byte[] file) {
			dynamic dynamicAttachment = new ExpandoObject();
			dynamicAttachment.File = file;
			dynamicAttachment.Container = container;
			dynamicAttachment.Name = name;
			dynamicAttachment.Route = "";
			dynamicAttachment.FileUrl = "";
			var result = await StorageHelperAzure.UploadFileToAzure(dynamicAttachment
				, Constants.AzureStorage.DefaultConnectionString);
			dynamicAttachment.FileUrl = result.FileUrl;
			return dynamicAttachment.FileUrl;
		}


		public int SyncActivities(IList<Activity> activities) {
			var i = _activityRepository.InsertActivities(activities);
			foreach (var activity in activities) {
				//_assignmentRepository.UpdateStatusUser(activity.UserId, 2);
				//var listActivity = _activityRepository.GetActivitiesByAssignmentByUserId(activity.AssignmentId, activity.UserId);
				//if (listActivity.Count == 0) {
				//}
				if (activity.Traces != null && activity.Traces.Count > 0) {
					_activityRepository.InsertTraces(activity.Traces);
				}
			}
			return i;
		}

		public void SyncDocuments(IList<Document> documents) {
			if (documents != null && documents.Count > 0) {
				_documentRepository.InsertDocuments(documents);
			}
		}

		public IList<Assignment> GetAssignmentByOrder(string orderid) {
			var assignments = _assignmentRepository.GetAssignmentByOrder(orderid);

			foreach (var item in assignments) {
				var activities = _activityRepository.GetActivitiesByAssignmentId(item.Id);
				item.Activities = new List<Activity>(activities);
				item.Documents = new List<Document>();
			}
			return assignments;
		}


		private async Task SendEmailWithActivity(Activity item) {
			string message = "";
			var assignment = _assignmentRepository.GetDetailMail(item.AssignmentId);
			var users = _assignmentRepository.GetByPersonnelIdAssignment(item.AssignmentId).ToList();
			var usersClient = _assignmentRepository.GetContactByAssignemtnId(item.AssignmentId).ToList();

			var technician = users.Where(x => x.UserType == 2 && x.Id == item.UserId).FirstOrDefault();
			if (technician == null) {
				return;
			}

			message = GetMessageClient(item, users, usersClient, assignment);
			if (!string.IsNullOrEmpty(message)) {
				listUsers.Clear();
				dynamic userHeader = null;
				if (usersClient != null && usersClient.Count > 0) {
					userHeader = usersClient[0];
					userHeader.CompanyName = assignment.CompanyName;
					foreach (var client in usersClient) {
						listUsers.Add(client.Email);
					}
				}
				var bodyHtml = GetMessageBody(assignment, message, 2, userHeader, users);
				if (listUsers != null && listUsers.Count > 0) {
					await email.PostSendEmail(listUsers, new List<string>(), "Ferreyros", bodyHtml, true, new List<Attachment>());
				}
			}
			message = GetMessageSupervisor(item, users, usersClient, assignment);
			if (!string.IsNullOrEmpty(message)) {
				var userHeader = users.Where(x => x.UserType == 1).FirstOrDefault();
				var bodyHtml = GetMessageBody(assignment, message, 1, userHeader, users);
				listUsers.Clear();
				listUsers.Add(userHeader.Email);
				if (listUsers != null && listUsers.Count > 0) {
					await email.PostSendEmail(listUsers, new List<string>(), "Ferreyros", bodyHtml, true, new List<Attachment>());
				}
			}
			listUsers.Clear();
		}

		private async Task SendEmailOffline(Assignment item, string message) {
			var assignment = _assignmentRepository.GetDetailMail(item.Id);
			var users = _assignmentRepository.GetByPersonnelIdAssignment(item.Id).ToList();
			var usersClient = _assignmentRepository.GetContactByAssignemtnId(item.Id).ToList();

			var technician = users.Where(x => x.UserType == 2 && x.Id == item.UserId).FirstOrDefault();
			if (technician == null) {
				return;
			}

			dynamic userHeader = null;
			if (usersClient != null && usersClient.Count > 0) {
				userHeader = usersClient[0];
				userHeader.CompanyName = assignment.CompanyName;
				foreach (var client in usersClient) {
					listUsers.Add(client.Email);
				}
			}

			var messageBitacora = message;
			message = string.Format(Constants.MessageBodyActivities.ActivitiesOffile, userHeader.Name + " " + userHeader.LastName, assignment.WorkOrderNumber, messageBitacora);

			var bodyHtml = GetMessageBody(assignment, message, 3, userHeader, users);
			if (listUsers != null && listUsers.Count > 0) {
				await email.PostSendEmail(listUsers, new List<string>(), "Ferreyros", bodyHtml, true, new List<Attachment>());
			}
			listUsers.Clear();
		}


		private string GetMessageSupervisor(Activity item, IList<dynamic> users, IList<dynamic> usersClient, Assignment assignment) {
			var supervisor = users.Where(x => x.UserType == 1).FirstOrDefault();
			var technician = users.Where(x => x.UserType == 2 && x.Id == item.UserId).FirstOrDefault();

			dynamic userContact = null;
			if (usersClient != null && usersClient.Count > 0) {
				userContact = usersClient[0];
			}

			if (item.ActivityState == ActivityState.Active && item.ActivityType == ActivityType.PreparingTrip) {
				return string.Format(Constants.MessageBodyActivities.PreparingTripStart.ToString(), supervisor?.Name, technician?.Name
												, assignment.Location?.Ubicacion, userContact?.Name + " " + userContact?.LastName);

			} else if (item.ActivityState == ActivityState.Completed && item.ActivityType == ActivityType.PreparingTrip) {
				return string.Format(Constants.MessageBodyActivities.PreparingTripEnd.ToString(), supervisor?.Name, technician?.Name
												, assignment.Location?.Ubicacion, userContact?.Name + " " + userContact?.LastName);

			} else if (item.ActivityState == ActivityState.Active && item.ActivityType == ActivityType.Traveling) {
				return string.Format(Constants.MessageBodyActivities.TravelingSupervisorStart.ToString(), supervisor?.Name, technician?.Name
												, assignment.Location?.Ubicacion, userContact?.Name + " " + userContact?.LastName); ;

			} else if (item.ActivityState == ActivityState.Completed && item.ActivityType == ActivityType.Traveling) {
				return string.Format(Constants.MessageBodyActivities.TravelingSupervisorEnd.ToString(), supervisor?.Name, technician?.Name
												, assignment.Location?.Ubicacion, userContact?.Name + " " + userContact?.LastName);

			} else if (item.ActivityState == ActivityState.Active && item.ActivityType == ActivityType.Delay) {
				return string.Format(Constants.MessageBodyActivities.DelaySupervisorStart.ToString(), supervisor?.Name, technician?.Name);

			} else if (item.ActivityState == ActivityState.Completed && item.ActivityType == ActivityType.Delay) {
				return string.Format(Constants.MessageBodyActivities.DelaySupervisorEnd.ToString(), supervisor?.Name, technician?.Name);

			} else if (item.ActivityState == ActivityState.Active && item.ActivityType == ActivityType.Driving) {
				return string.Format(Constants.MessageBodyActivities.DrivingSupervisorStart.ToString(), supervisor?.Name, technician?.Name
												, assignment.Location?.Ubicacion, userContact?.Name + " " + userContact?.LastName);

			} else if (item.ActivityState == ActivityState.Completed && item.ActivityType == ActivityType.Driving) {
				return string.Format(Constants.MessageBodyActivities.DrivingSupervisorEnd.ToString(), supervisor?.Name, technician?.Name
												, assignment.Location?.Ubicacion, userContact?.Name + " " + userContact?.LastName);

			} else if (item.ActivityState == ActivityState.Active && item.ActivityType == ActivityType.FieldService) {
				return string.Format(Constants.MessageBodyActivities.FieldServiceSupervisorStart.ToString(), supervisor?.Name, technician?.Name);

			} else if (item.ActivityState == ActivityState.Active && item.ActivityType == ActivityType.StandByClient) {
				return string.Format(Constants.MessageBodyActivities.StandByClientSupervisorStart.ToString(), supervisor?.Name, technician?.Name
												, assignment.Location?.Ubicacion, assignment.WorkOrderNumber);

			} else if (item.ActivityState == ActivityState.Completed && item.ActivityType == ActivityType.StandByClient) {
				return string.Format(Constants.MessageBodyActivities.StandByClientSupervisorEnd.ToString(), supervisor?.Name);

			} else if (item.ActivityState == ActivityState.Active && item.ActivityType == ActivityType.StandByFesa) {
				return string.Format(Constants.MessageBodyActivities.StandByFesaSupervisorStart.ToString(), supervisor?.Name, technician?.Name
												, assignment.Location?.Ubicacion, assignment.WorkOrderNumber);

			} else if (item.ActivityState == ActivityState.Completed && item.ActivityType == ActivityType.FieldReport) {
				return string.Format(Constants.MessageBodyActivities.FieldReportSupervisorEnd.ToString(), supervisor?.Name, technician?.Name);

			} else if (item.ActivityState == ActivityState.Active && item.ActivityType == ActivityType.DrivingReturn) {
				return string.Format(Constants.MessageBodyActivities.DrivingReturnSupervisorStart.ToString(), supervisor?.Name, technician?.Name);

			} else if (item.ActivityState == ActivityState.Completed && item.ActivityType == ActivityType.DrivingReturn) {
				return string.Format(Constants.MessageBodyActivities.DrivingReturnSupervisorEnd.ToString(), supervisor?.Name, technician?.Name);

			} else if (item.ActivityState == ActivityState.Active && item.ActivityType == ActivityType.TravelingReturn) {
				return string.Format(Constants.MessageBodyActivities.TravelingReturnStart.ToString(), supervisor?.Name, technician?.Name);

			} else if (item.ActivityState == ActivityState.Completed && item.ActivityType == ActivityType.TravelingReturn) {
				return string.Format(Constants.MessageBodyActivities.TravelingReturnEnd.ToString(), supervisor?.Name, technician?.Name
												, assignment.Location?.Ubicacion, userContact?.Name + " " + userContact?.LastName);
			}

			// NO VA PARA EL INFORME FINAL

			return "";
		}

		private string GetMessageClient(Activity item, IList<dynamic> users, IList<dynamic> usersClient, Assignment assignment) {
			var supervisor = users.Where(x => x.UserType == 1).FirstOrDefault();
			var technician = users.Where(x => x.UserType == 2 && x.Id == item.UserId).FirstOrDefault();

			dynamic userContact = null;
			if (usersClient != null && usersClient.Count > 0) {
				userContact = usersClient[0];
			}

			if (item.ActivityState == ActivityState.Active && item.ActivityType == ActivityType.Traveling) {
				return string.Format(Constants.MessageBodyActivities.TravelingClienteStart.ToString(), userContact?.Name + " " + userContact?.LastName
											, assignment.Location?.Ubicacion);

			} else if (item.ActivityState == ActivityState.Active && item.ActivityType == ActivityType.Driving) {
				return string.Format(Constants.MessageBodyActivities.DrivingClienteStart.ToString(), userContact?.Name + " " + userContact?.LastName
											, assignment.Location?.Ubicacion);

			} else if (item.ActivityState == ActivityState.Active && item.ActivityType == ActivityType.FieldService) {
				return string.Format(Constants.MessageBodyActivities.FieldServiceClienteStart.ToString(), userContact?.Name + " " + userContact?.LastName);

			} else if (item.ActivityState == ActivityState.Active && item.ActivityType == ActivityType.StandByClient) {
				return string.Format(Constants.MessageBodyActivities.StandByClientClienteStart.ToString(), userContact?.Name + " " + userContact?.LastName);

			} else if (item.ActivityState == ActivityState.Completed && item.ActivityType == ActivityType.FieldReport) {
				return string.Format(Constants.MessageBodyActivities.FieldReportClienteEnd.ToString(), userContact?.Name + " " + userContact?.LastName);
			}
			return "";
		}


		private string GetMessageBody(Assignment item, string message, int type, dynamic user, List<dynamic> users) {
			string htmlBody = "";
			string name = "";
			string nameCompany = "";

			var supervisor = users.Where(x => x.UserType == 1).FirstOrDefault();
			var technician = users.Where(x => x.UserType == 2).FirstOrDefault();

			string nameSupervisor = supervisor.Name;
			string mailSupervisor = supervisor.Email;
			string phoneSupervisor = supervisor.Celullar;

			string nameTechnician = technician?.Name == null ? "" : technician.Name;
			string mailTechnician = technician?.Email == null ? "" : technician.Email;
			string phoneTechnician = technician?.Celullar == null ? "" : technician.Celullar;


			if (type == 1) {
				htmlBody = Resources.Template.ServiceGeneral.ToString();
				name = user.Name;
				nameCompany = "Ferreyros";
			} else if (type == 2) {
				htmlBody = Resources.Template.ServiceFull.ToString();
				name = user.Name + " " + user.LastName;
				nameCompany = user.CompanyName;
			} else if (type == 3) {
				htmlBody = Resources.Template.ServiceOffline.ToString();
			}

			htmlBody = htmlBody.Replace("@Description", item.Description)
								.Replace("@FullName", name)
								.Replace("@SocialName", nameCompany)
								.Replace("@Message", message)
								.Replace("@OT", item.WorkOrderNumber)
								.Replace("@Model", item.Machine.Model)
								.Replace("@Brand", item.Machine.Brand)
								.Replace("@NumberSerie", item.Machine.SerialNumber)
								.Replace("@Supervisor", nameSupervisor)
								.Replace("@SMail", mailSupervisor.Replace("@", "@</br>"))
								.Replace("@SPhone", phoneSupervisor)//.Replace("-", "").Replace("(", "").Replace(")", "").Trim())
								.Replace("@Technical", nameTechnician)
								.Replace("@TMail", mailTechnician.Replace("@", "@</br>"))
								.Replace("@TPhone", phoneTechnician)
								.Replace("@Number", users.Count().ToString());
			return htmlBody;
		}




		//WEB SUPERVISOR
		public ResponseAll GetOTs(Assignment item, int pageIndex, int pageSize) {
			List<Assignment> assignments = null;
			int count = 0;
			if (item.TypeConsult == 2) {
				assignments = new List<Assignment>(_assignmentRepository.GetByOTsReport(item, pageIndex, pageSize));
				count = _assignmentRepository.GetByOTsReportCount(item, pageIndex, pageSize);
			} else {
				assignments = new List<Assignment>(_assignmentRepository.GetByOTs(item, pageIndex, pageSize));
				count = _assignmentRepository.GetByOTsCount(item, pageIndex, pageSize);
			}

			var response = new ResponseAll {
				Assignments = new List<Assignment>(assignments),
				TotalRows = count
			};
			return response;
		}

		public ResponseAll GetDetailTechnician(string assignmentId, string userId) {
			var result = new ResponseAll();
			result.Assignment = new Assignment();
			var activities = _activityRepository.GetActivitiesByAssignmentByUserId(assignmentId, userId);
			if (activities != null) {
				foreach (var activity in activities) {
					var traces = _activityRepository.GetTracesByActivityId(activity.Id);
					if (traces != null) {
						activity.Traces = new List<Trace>(traces);
					}
				}
				result.Assignment.Activities = new List<Activity>(activities);
			}
			var documents = _documentRepository.GetDocumentsByAssignmentByUserId(assignmentId, userId);
			if (documents != null) {
				result.Assignment.Documents = new List<Document>(documents);
			}

			var documentsEntity = _documentRepository.GetAllEntityDocument();
			if (documentsEntity != null) {
				result.DocumentsEntity = new List<DocumentEntity>(documentsEntity);
			}

			return result;
		}

		public Assignment GetReportByProfile(string assignmentId, int rol) {
			Assignment result = null;
			if (rol == 2) {
				result = _assignmentRepository.GetReportTechnician(assignmentId);
			} else {
				result = _assignmentRepository.GetReportSupervisor(assignmentId);
				if (result != null) {
					var files = _documentRepository.GetAllFileByRef(result.Report.Id);
					if (files != null && files.Count > 0) {
						result.Report.Files = new List<File>(files);
					}
				}
			}
			return result;
		}

		public int UpdateUrlInformFinalSupervisor(BE.ReportBE.Report report) {
			int i = 0;
			i = _assignmentRepository.UpdateUrlInformFinalSupervisor(report);
			return i;
		}

		//TÉCNICO 
		public async Task<int> SyncActivitiesDocuments(Request request) {
			var i = _activityRepository.InsertActivities(request.Activities);
			foreach (var activity in request.Activities) {
				if (activity.Traces != null && activity.Traces.Count > 0) {
					_activityRepository.InsertTraces(activity.Traces);
				}
			}
			if (request.Documents != null && request.Documents.Count > 0) {
				_documentRepository.InsertDocuments(request.Documents);
			}

			if (request.Reports != null && request.Reports.Count > 0) {
				foreach (var item in request.Reports) {
					var dynamicFile = await GenerateAct(item, 1);
					item.UrlAct = dynamicFile.FileUrl;
					_assignmentRepository.InsertReport(item);
					listUsers.Add("antonio.zarate@cmscloud.pe");
					listUsersCC.Add("brihan.bocanegra@cmscloud.pe");
					listUsersCC.Add("cristian.artigas@cmscloud.pe");
					await email.PostSendEmail(listUsers, listUsersCC, "FERREYROS", "<p>Este mensaje es para el supervisor. El informe preliminar de servicio ya se puede revisar desde la web</p>", true, listAttachments);
				}
			}
			return i;
		}

		public int SyncReport(BE.ReportBE.Report item) {
			var i = _assignmentRepository.InsertReport(item);
			return i;
		}

		public async Task<int> UpdateExecutiveReport(BE.ReportBE.Report report) {
			int i = 0;
			if (report.ActionType == 1) {
				i = _assignmentRepository.UpdateExecutiveReport(report);
			} else if (report.ActionType == 2) {
				i = _assignmentRepository.UpdateExecutiveReport(report);
				if (i >= 1) {
					var dynamicFile = await GenerateAct(report, 2);
					report.UrlExe = dynamicFile.FileUrl;
					i = _assignmentRepository.UpdateExecutiveReport(report);
					listAttachments.Add(new Attachment(dynamicFile.Route));
					listUsers.Add("antonio.zarate@cmscloud.pe");
					listUsersCC.Add("brihan.bocanegra@cmscloud.pe");
					listUsersCC.Add("cristian.artigas@cmscloud.pe");
					await email.PostSendEmail(listUsers, listUsersCC, "FERREYROS", "<p>Este mensaje es para el cliente. Le estamos enviando un informe preliminar del servicio</p>", true, listAttachments);
				}
			};
			return i;
		}

		public async Task<int> UpdateArchiveInformFinal(BE.ReportBE.Report report) {
            int i = 0;
            if (report.ActionType == 1) {
                i = _assignmentRepository.UpdateArchiveInformFinal(report);
            } else if (report.ActionType == 2) {
                report.Sent1 = true;
                i = _assignmentRepository.UpdateArchiveInformFinal(report);
                if (i >= 1) {
                    if (report.WhoUpdate == 1) {
                        return i;
                    }
                    listUsersCC.Add("cristian.artigas@cmscloud.pe");
                   // listUsersCC.Add("brihan.bocanegra@cmscloud.pe");
                   // listUsersCC.Add("cristian.artigas@cmscloud.pe");
                    await email.PostSendEmail(listUsers, listUsersCC, "FERREYROS", "<p>El tecnico ha generado su informe Final de la OT-XXXX</p>", true, new List<Attachment>());
                }
            }
            return i;
		}

		public async Task<dynamic> GenerateAct(BE.ReportBE.Report report, int type) {
			var assignment = _assignmentRepository.GetReportSupervisor(report.AssignmentId);
			var users = _assignmentRepository.GetByPersonnelIdAssignment(report.AssignmentId).ToList();
			var usersClient = _assignmentRepository.GetContactByAssignemtnId(report.AssignmentId).ToList();

			var supervisor = users.Where(x => x.UserType == 1).FirstOrDefault();
			var technician = users.Where(x => x.UserType == 2).FirstOrDefault();
			var client = usersClient.Where(x => x.Id == report.ContactId).FirstOrDefault();

			var sValue = ConvertNumberToString(report.Value) + "\"";
			var pdfContent = string.Empty;
			if (type == 1) {
				pdfContent = Resources.Template.ActString.ToString();
			} else if (type == 2) {
				pdfContent = Resources.Template.ExeString.ToString();
			}

			CultureInfo ci = new CultureInfo("es-PE");

			var colorStyle = sValue + " " + strFill.Replace("@@strcolor", strColor);

			pdfContent = pdfContent
				.Replace("@SocialName", assignment.CompanyName)
				.Replace("@RUC", assignment.RUC)
				.Replace("@Branch", assignment.CorpId)
				.Replace("@Description", assignment.Description)

				.Replace("@OT", assignment.WorkOrderNumber)
				.Replace("@Brand", assignment.Machine.Brand)
				.Replace("@Model", assignment.Machine.Model)
				.Replace("@Technical", technician?.Name)
				.Replace("@Horometro", assignment.Machine.TotalHoursFunction.ToString())
				.Replace("@NumberSerie", assignment.Machine.SerialNumber)
				.Replace("@DateDelivery", report.Date.ToString("dd-MM-yyyy", ci))
				.Replace("@DateService", assignment.RequestDate.ToString("dd-MM-yyyy", ci))

				.Replace("@Antecedent", report.Antecedent)
				.Replace("@Work", report.Work)
				.Replace("@Observations", report.Observation)
				.Replace("@Recommendations", report.Replacement)
				.Replace("@Client", client?.Name + " " + client?.LastName)
				.Replace("@SignAct", report.UrlSign)
				.Replace(sValue.ToString(), colorStyle)
				//EXE
				.Replace("@Supervisor", supervisor?.Name)
				.Replace("@SPhone", supervisor?.Phone)
				.Replace("@SMail", supervisor?.Email);

			var converter = new HtmlToPdfConverter();
			var pdfFile = converter.GeneratePdf(pdfContent);
			var path = System.IO.Path.GetTempPath();

			dynamic dynamicAttachment = new ExpandoObject();

			string pdfString = ""; string nameFile = "";
			nameFile = report.Id + ".pdf";
			if (type == 1) {
				pdfString = nameFile;
			} else if (type == 2) {
				pdfString = $"{path}\\" + nameFile;
				dynamicAttachment.Route = pdfString;
				System.IO.File.WriteAllBytes(pdfString, pdfFile);
			}

			string container = "act";
			dynamicAttachment.File = pdfFile;
			dynamicAttachment.Container = container;
			dynamicAttachment.Name = nameFile;
			dynamicAttachment.Route = pdfString;
			dynamicAttachment.FileUrl = "";

			var result = await StorageHelperAzure.UploadFileToAzure(dynamicAttachment
				, Constants.AzureStorage.DefaultConnectionString);
			dynamicAttachment.FileUrl = result.FileUrl;
			return dynamicAttachment;
		}

		string strFill = "style=\"background-color:@@strcolor!important\"";
		string strColor = "";
		public string ConvertNumberToString(int value) {
			switch (value) {
				case 1:
					strColor = "#E34030";
					return "one";
				case 2:
					strColor = "#E5503E";
					return "two";
				case 3:
					strColor = "#E65F3A";
					return "three";
				case 4:
					strColor = "#EA7237";
					return "four";
				case 5:
					strColor = "#EE8825";
					return "five";
				case 6:
					strColor = "#F5AC16";
					return "six";
				case 7:
					strColor = "#F5AD00";
					return "seven";
				case 8:
					strColor = "#F4AC16";
					return "eight";
				case 9:
					strColor = "#31A537";
					return "nine";
				case 10:
					strColor = "#00953B";
					return "ten";
			}
			return "zero";
		}


		public int AddFileReportSupervisor(File file) {
			int i = 0;
			i = _assignmentRepository.AddFileReportSupervisor(file);
			return i;
		}

		public int DeleteFileReportSupervisor(File file) {
			int i = 0;
			//i = _assignmentRepository.DeleteFileReportSupervisor(file);
			return i;
		}

		public int DeleteArchiveInformFinal(BE.ReportBE.Report report) {
			int i = 0;
			i = _assignmentRepository.DeleteArchiveInformFinal(report);
			return i;
		}

        public int UpdateSendFiletoReportSupervisor(BE.ReportBE.Report report) {
            int i = 0;
            try {
                foreach (var item in report.Files) {
                    if (item.status == 0) {
                        i = _assignmentRepository.AddFileReportSupervisor(item);
                    } else if (item.status == 1) {
                        i = _assignmentRepository.DeleteFileReportSupervisor(item);
                    }
                }                
            } catch (Exception ex) {
                Log.Write(ex);
                throw;
            }
            return i;

		}

		public async Task<int> UpdateInformFinalDataSupervisor(BE.ReportBE.Report report) {
			int i = 0;
			try {
				if (report.ActionType == 1) {
					i = _assignmentRepository.UpdateInformFinalDataSupervisor(report);
				} else if (report.ActionType == 2) {
					i = _assignmentRepository.UpdateInformFinalDataSupervisor(report);
					List<Attachment> listAttachments = new List<Attachment>();
					if (!string.IsNullOrEmpty(report.UrlFile)) {
						string[] arrName = report.UrlFile.Split('/');
						var stream = new WebClient().OpenRead(new Uri(report.UrlFile));
						Attachment attachement = new Attachment(stream, arrName[arrName.Length - 1]);
						listAttachments.Add(attachement);
						//listUsers.Add("antonio.zarate@cmscloud.pe");
						//listUsersCC.Add("brihan.bocanegra@cmscloud.pe");
						listUsersCC.Add("cristian.artigas@cmscloud.pe");
					} else {

					}
					listUsersCC.Add("cristian.artigas@cmscloud.pe");
					await email.PostSendEmail(listUsers, listUsersCC, "FERREYROS", "<p>Comentario del Supervisor:</p><p>" + report.Comment2 + "</p>", true, listAttachments);


				}
			} catch (Exception ex) {
				Log.Write(ex);
				throw;
			}
			return i;
		}
		#endregion

	}
}