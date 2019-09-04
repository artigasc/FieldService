using System.Collections.Generic;
using FESA.SCM.Common.Base;
using FESA.SCM.FieldService.BE.DocumentBE;
using FESA.SCM.FieldService.BE.ReportBE;

namespace FESA.SCM.FieldService.BE.AssignmentBE {
    public interface IAssignmentRepository : IBaseRepository<Assignment> {

        IList<dynamic> GetByPersonnelIdAssignment(string id);
        IList<dynamic> GetContactByAssignemtnId(string id);
        void SyncAssignments(IList<Assignment> assignments);
        IList<Assignment> GetByUserId(string userId);
        Assignment GetByWorkOrder(string workOrderId);
        void AddPersonnel(IList<Personnel> personnel);
        IList<Personnel> GetByAssignmentId(string assignmentId);
        void AddLocation(Location entity);
        IList<Assignment> GetAssignmentByOrder(string orderid);

        //PRUEBAANTONIO
        void UpdateStatusUser(string id, int userStatus);

        int GetAssignmentCountByUser(string userId);

        int UpdatePersonnelStatus(string idUser, string idAssignment, AssignmentStatus assignmentStatus);

        //WEB SUPERVISOR
        IList<Assignment> GetByOTs(Assignment item, int pageSize, int pageTotal);
        int GetByOTsCount(Assignment item, int pageSize, int pageTotal);
        IList<Assignment> GetByOTsReport(Assignment item, int pageSize, int pageTotal);
        int GetByOTsReportCount(Assignment item, int pageSize, int pageTotal);
        Assignment GetReportTechnician(string assignmentId);
        Assignment GetReportSupervisor(string assignmentId);

        Assignment GetDetailMail(string assignmentId);

        int UpdateExecutiveReport(BE.ReportBE.Report report);

        int UpdateArchiveInformFinal(BE.ReportBE.Report report);

        //TECNICO
        int InsertReport(Report item);
        int UpdateStatusByLider(Assignment entity);
        int UpdateDateStart(Assignment assignment);



        //CA
        IList<File> GetFilesByReference(string fileId);
        //int UpdateSendFiletoReportSupervisor(Report report);
        int AddFileReportSupervisor(File file);
        int DeleteFileReportSupervisor(File file);
        int DeleteArchiveInformFinal(Report report);
        int UpdateInformFinalDataSupervisor(BE.ReportBE.Report report);
        int UpdateUrlInformFinalSupervisor(BE.ReportBE.Report report);

    }
}