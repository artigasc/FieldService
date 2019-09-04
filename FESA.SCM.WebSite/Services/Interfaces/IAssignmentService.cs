using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FESA.SCM.WebSite.Models;
namespace FESA.SCM.WebSite.Services.Interfaces {
    public interface IAssignmentService {
        Task<ResponseAssignment> GetOtsAsync(Request request);
        Task<AssignmentModel> GetOtAsync(string idOT, string idUser);
        Task<ResponseAssignment> GetOtDetailAsync(string idOT, string idUser);
        Task<ResponseFilter> GetDataForFilterAsync();
        Task<AssignmentModel> GetReportByIdAsync(string idRol, string idAssignment, string idUser);
        Task<string> SaveContact(ContactModel model);
        Task<ClientResponse> UpdateExecutiveInform(ReportModel model);
        Task<ClientResponse> UpdateArchiveInform(ReportModel model);
        Task<ClientResponse> AddOrUpdateSendFileToReportSupervisor(ReportModel model);
        Task<ClientResponse> DeleteArchiveInform(ReportModel model);
        Task<ClientResponse> AddFileToReportSupervisor(ReportModel model);
        Task<ClientResponse> DeleteFileReportSupervisor(FileModel model);
    }
}
