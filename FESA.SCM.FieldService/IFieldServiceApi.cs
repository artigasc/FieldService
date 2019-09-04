using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using FESA.SCM.FieldService.BE.AssignmentBE;
using FESA.SCM.FieldService.BE.ReportBE;
using FESA.SCM.FieldService.BE.ActivityBE;
using FESA.SCM.FieldService.BE.DocumentBE;
using FESA.SCM.FieldService.BE;
using System.Threading.Tasks;

namespace FESA.SCM.FieldService {
	[ServiceContract]
	public interface IFieldServiceApi {

		[OperationContract]
		[WebInvoke(Method = "GET", UriTemplate = "/GetAll", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
			BodyStyle = WebMessageBodyStyle.Wrapped)]
		[return: MessageParameter(Name = "Assignments")]
		IList<Assignment> GetAll();

		[OperationContract]
		[WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
			BodyStyle = WebMessageBodyStyle.Wrapped)]
		[return: MessageParameter(Name = "Assignments")]
		IList<Assignment> GetPaginated(Assignment assignment, int pageIndex, int pageSize, out int totalRows);

		[OperationContract]
		[WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
			BodyStyle = WebMessageBodyStyle.Wrapped)]
		Assignment GetById(string id);

		[OperationContract]
		[WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
			BodyStyle = WebMessageBodyStyle.Wrapped)]
		string AddAssignment(Assignment assignment);

		[OperationContract]
		[WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
			BodyStyle = WebMessageBodyStyle.Wrapped)]
		void UpdateAssignment(Assignment assignment);

		[OperationContract]
		[WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
			BodyStyle = WebMessageBodyStyle.Wrapped)]
		void DeleteAssignment(string id, string modifiedBy, DateTime lastModification);

		[OperationContract]
		[WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
			BodyStyle = WebMessageBodyStyle.Wrapped)]
		//[return: MessageParameter(Name = "Assignments")]
		[return: MessageParameter(Name = "ResponseAll")]
		ResponseAll GetByUserId(string userId);

		[OperationContract]
		[WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
			BodyStyle = WebMessageBodyStyle.Wrapped)]
		Task<int> SyncAssignments(List<Assignment> assignments);


		[OperationContract]
		[WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
			BodyStyle = WebMessageBodyStyle.Wrapped)]
		int SyncActivities(List<Activity> activities);


		[OperationContract]
		[WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
			BodyStyle = WebMessageBodyStyle.Wrapped)]
		void SyncDocuments(List<Document> documents);

		[OperationContract]
		[WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
			BodyStyle = WebMessageBodyStyle.Wrapped)]
		[return: MessageParameter(Name = "Ocupability")]
		IList<Ocupability> GetOcupabilityLevel(string supervisorId, string idsOffice, string idsCostCenter);

		[OperationContract]
		[WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
			BodyStyle = WebMessageBodyStyle.Wrapped)]
		[return: MessageParameter(Name = "OcupabilityThroughTime")]
		IList<OcupabilityThroughTime> GetOcupabilityThroughTime(string supervisorId, int status);

		[OperationContract]
		[WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
			BodyStyle = WebMessageBodyStyle.Wrapped)]
		IList<Assignment> GetAssignmentByOrder(string orderid);

		[OperationContract]
		[WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
			BodyStyle = WebMessageBodyStyle.Wrapped)]
		[return: MessageParameter(Name = "Activity")]
		IList<Activity> GetActivityByIdAndUserId(string Id, string userId);


		//WEB SUPERVISOR 
		[OperationContract]
		[WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
			BodyStyle = WebMessageBodyStyle.Wrapped)]
		//[return: MessageParameter(Name = "Response")]
		ResponseAll GetByOTs(Request request);

		[OperationContract]
		[WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
			BodyStyle = WebMessageBodyStyle.Wrapped)]
		[return: MessageParameter(Name = "Assignment")]
		ResponseAll GetDetailTechnician(string assignmentId, string userId);

		[OperationContract]
		[WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
			BodyStyle = WebMessageBodyStyle.Wrapped)]
		[return: MessageParameter(Name = "Assignment")]
		Assignment GetReportById(string assignmentId, int rol);

		[OperationContract]
		[WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
		   BodyStyle = WebMessageBodyStyle.Wrapped)]
		Task<int> UpdateExecutiveReport(Report report);

		[OperationContract]
		[WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
		  BodyStyle = WebMessageBodyStyle.Wrapped)]
        Task<int> UpdateArchiveInformFinal (Report report);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
          BodyStyle = WebMessageBodyStyle.Wrapped)]
        int UpdateUrlInformFinalSupervisor(Report report);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.Wrapped)]
        Task<int> UpdateInformFinalDataSupervisor(Report report); 

         //TÉCNICO 
         [OperationContract]
		[WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
			BodyStyle = WebMessageBodyStyle.Wrapped)]
		Task<int> SyncActivitiesDocuments(Request request);

		[OperationContract]
		[WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
			BodyStyle = WebMessageBodyStyle.Wrapped)]
		int SyncReport(Report report);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.Wrapped)]
        int UpdateSendFiletoReportSupervisor(Report report);



        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        int AddFileReportSupervisor(File file);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
           BodyStyle = WebMessageBodyStyle.Wrapped)]
        int DeleteFileReportSupervisor(File file);

        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
           BodyStyle = WebMessageBodyStyle.Wrapped)]
        int DeleteArchiveInformFinal(Report report);



    }
}
