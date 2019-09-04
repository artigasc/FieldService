using System;
using System.Collections.Generic;
using FESA.SCM.FieldService.BE.AssignmentBE;
using FESA.SCM.FieldService.BE.ActivityBE;
using FESA.SCM.FieldService.BE.DocumentBE;
using FESA.SCM.FieldService.BE;
using System.Threading.Tasks;
using FESA.SCM.FieldService.BE.ReportBE;

namespace FESA.SCM.FieldService.BL.BusinessInterfaces {
	public interface IAssignmentService {
		IList<Assignment> GetAll();
		IList<Assignment> GetPaginated(Assignment assignment, int pageIndex, int pageSize, out int totalRows);
		Assignment GetById(string id);
		string AddAssignment(Assignment assignment);
		void UpdateAssignment(Assignment assignment);
		void DeleteAssignment(string id, string modifiedBy, DateTime lastModification);
		ResponseAll GetByUserId(string userId);
		Task<int> SyncAssignments(IList<Assignment> assignments);

		int SyncActivities(IList<Activity> activities);
		void SyncDocuments(IList<Document> documents);

		IList<Assignment> GetAssignmentByOrder(string orderid);
		IList<Activity> GetActivityByIdAndUserId(string id, string userId);


		//WEB SUPERVISOR
		ResponseAll GetOTs(Assignment item, int pageIndex, int pageSize);
		ResponseAll GetDetailTechnician(string assignmentId, string userId);
		Assignment GetReportByProfile(string assignmentId, int rol);
		Task<int> UpdateExecutiveReport(BE.ReportBE.Report report);
        Task<int> UpdateArchiveInformFinal(BE.ReportBE.Report report);
        int UpdateSendFiletoReportSupervisor(Report report);
        Task<int> UpdateInformFinalDataSupervisor(Report report);
        int UpdateUrlInformFinalSupervisor(Report report);
        //TÉCNICO
        Task<int> SyncActivitiesDocuments(Request request);
		int SyncReport(BE.ReportBE.Report item);
        int AddFileReportSupervisor(File file);
        int DeleteFileReportSupervisor(File file);
        int DeleteArchiveInformFinal(Report report);
       

    }
}