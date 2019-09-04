using FESA.SCM.WebSite.Helpers;
using FESA.SCM.WebSite.Models;
using FESA.SCM.WebSite.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FESA.SCM.WebSite.Services.Implementation {
    public class AssignmentService : IAssignmentService {
        private ApiClient _apiClient;

        public AssignmentService() {
            _apiClient = new ApiClient(Constants.UrlBase);
        }

        public async Task<ResponseAssignment> GetOtsAsync(Request request) {
            ResponseAssignment item = null;
            try {
                var response = await _apiClient.ExecutePost<ResponseAssignment>("Assignments/getots", request);
                item = (ResponseAssignment)response.Content;
            } catch (Exception e) {
                string m = e.Message;
            }
            return item;
        }

        public async Task<AssignmentModel> GetOtAsync(string idOT, string idUser) {
            AssignmentModel Result = null;
            try {
                _apiClient.AddParameter("Id", idOT);
                _apiClient.AddParameter("userId", idUser);
                var response = await _apiClient.ExecutePost<ResponseAssignment>("Assignments/GetDetailOT");
                Result = response.Content.Assignment;
            } catch (Exception e) {
                string m = e.Message;
            }
            return Result;
        }

        public async Task<ResponseAssignment> GetOtDetailAsync(string idOT, string idUser) {
            ResponseAssignment Result = null;
            try {
                _apiClient.AddParameter("Id", idOT);
                _apiClient.AddParameter("userId", idUser);
                var response = await _apiClient.ExecutePost<ResponseAssignment>("Assignments/GetDetailOTAD");
                Result = response.Content;
            } catch (Exception e) {
                string m = e.Message;
            }
            return Result;
        }

        public async Task<ResponseFilter> GetDataForFilterAsync() {
            ResponseFilter Result = null;
            try {
                var response = await _apiClient.ExecuteGet<ResponseFilter>("users/GetAllFiltersWEB");
                Result = response.Content;
            } catch (Exception e) {
                string m = e.Message;
            }
            return Result;
        }

        public async Task<AssignmentModel> GetReportByIdAsync(string idRol, string idAssignment, string idUser) {
            AssignmentModel Result = null;
            try {
                _apiClient.AddParameter("assignmentId", idAssignment);
                _apiClient.AddParameter("userId", idUser);
                _apiClient.AddParameter("rol", idRol);
                var response = await _apiClient.ExecuteGet<ResponseAssignment>("Assignments/getreportbyid");
                Result = response.Content.Assignment;
            } catch (Exception e) {
                string m = e.Message;
            }
            return Result;
        }

        public async Task<string> SaveContact(ContactModel model) {
            string result = null;
            try {
                var response = await _apiClient.ExecutePost<ResponseContact>("client/addcontact", model);
                if (response == null || response.Status != System.Net.HttpStatusCode.OK) {
                    return null;
                }
                result = response.Content.Id;
            } catch (Exception e) {
                string m = e.Message;
            }
            return result;
        }


        public async Task<ClientResponse> UpdateExecutiveInform(ReportModel model) {
            ClientResponse result = null;
            try {
                result = await _apiClient.ExecutePost<ClientResponse>("Assignments/UpdateExecutiveReport", model);
                if (result == null || result.Status != System.Net.HttpStatusCode.OK) {
                    return null;
                }
            } catch (Exception e) {
                string m = e.Message;
            }
            return result;
        }
        
        public async Task<ClientResponse> UpdateArchiveInform(ReportModel model) {
            ClientResponse result = null;
            try {
                result = await _apiClient.ExecutePost<ClientResponse>("Assignments/UpdateArchiveInformFinal", model);
                if (result == null || result.Status != System.Net.HttpStatusCode.OK) {
                    return null;
                }
            } catch (Exception e) {
                string m = e.Message;
            }
            return result;
        }

        public async Task<ClientResponse> AddOrUpdateSendFileToReportSupervisor(ReportModel model) {
            ClientResponse result = null;
            try {
                result = await _apiClient.ExecutePost<ClientResponse>("Assignments/AddOrUpdateSendFileToReportSupervisor", model);
                if (result == null || result.Status != System.Net.HttpStatusCode.OK) {
                    return null;
                }
            } catch (Exception e) {
                string m = e.Message;
            }
            return result;
        }

        public async Task<ClientResponse> DeleteArchiveInform(ReportModel model) {
            ClientResponse result = null;
            try {
                result = await _apiClient.ExecutePost<ClientResponse>("Assignments/DeleteArchiveInformFinal", model);
                if (result == null || result.Status != System.Net.HttpStatusCode.OK) {
                    return null;
                }
            } catch (Exception e) {
                string m = e.Message;
            }
            return result;
        }

        public async Task<ClientResponse> AddFileToReportSupervisor(ReportModel model) {
            ClientResponse result = null;
            try {
                result = await _apiClient.ExecutePost<ClientResponse>("Assignments/AddFileToReportSupervisor", model);
                if (result == null || result.Status != System.Net.HttpStatusCode.OK) {
                    return null;
                }
            } catch (Exception e) {
                string m = e.Message;
            }
            return result;
        }

        public async Task<ClientResponse> DeleteFileReportSupervisor(FileModel model) {
            ClientResponse result = null;
            try {
                result = await _apiClient.ExecutePost<ClientResponse>("Assignments/DeleteFileReportSupervisor", model);
                if (result == null || result.Status != System.Net.HttpStatusCode.OK) {
                    return null;
                }
            } catch (Exception e) {
                string m = e.Message;
            }
            return result;
        }
    }
}


