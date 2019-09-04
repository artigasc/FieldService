using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Activation;
using FESA.SCM.Common;
using FESA.SCM.FieldService.BE.AssignmentBE;
using FESA.SCM.FieldService.BL.BusinessInterfaces;
using FESA.SCM.FieldService.InstanceProviders;
using FESA.SCM.FieldService.BE.ReportBE;
using FESA.SCM.FieldService.BE.ActivityBE;
using FESA.SCM.FieldService.BE.DocumentBE;
using FESA.SCM.FieldService.BE;
using System.Threading.Tasks;

namespace FESA.SCM.FieldService {
    [Serializable]
    [UnityInstanceProviderBehaviour]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class FieldServiceApi : IFieldServiceApi {
        #region Members
        private readonly IAssignmentService _assignmentService;
        private readonly IOcupabilityService _ocupabilityService;
        private readonly IOcupabilityThroughTimeService _ocupabilityThroughTimeService;
        #endregion
        #region Constructor
        public FieldServiceApi(IAssignmentService assignmentService,
                                IOcupabilityService ocupabilityService,
                                IOcupabilityThroughTimeService ocupabilityThroughTimeService) {
            if (assignmentService == null)
                throw new ArgumentNullException(nameof(assignmentService));

            _assignmentService = assignmentService;

            if (ocupabilityService == null)
                throw new ArgumentNullException(nameof(ocupabilityService));

            _ocupabilityService = ocupabilityService;

            if (ocupabilityThroughTimeService == null)
                throw new ArgumentNullException(nameof(ocupabilityThroughTimeService));

            _ocupabilityThroughTimeService = ocupabilityThroughTimeService;
        }
        #endregion
        #region Methods
        public IList<Assignment> GetAll() {
            try {
                return _assignmentService.GetAll();
            } catch (Exception ex) {
                Log.Write(ex);
                throw;
            }
        }

        public IList<Assignment> GetPaginated(Assignment assignment, int pageIndex, int pageSize, out int totalRows) {
            try {
                return _assignmentService.GetPaginated(assignment, pageIndex, pageSize, out totalRows);
            } catch (Exception ex) {
                Log.Write(ex);
                throw;
            }
        }

        public Assignment GetById(string id) {
            try {
                return _assignmentService.GetById(id);
            } catch (Exception ex) {
                Log.Write(ex);
                throw;
            }
        }

        public string AddAssignment(Assignment assignment) {
            try {
                return _assignmentService.AddAssignment(assignment);
            } catch (Exception ex) {
                Log.Write(ex);
                throw;
            }
        }

        public void UpdateAssignment(Assignment assignment) {
            try {
                _assignmentService.UpdateAssignment(assignment);
            } catch (Exception ex) {
                Log.Write(ex);
                throw;
            }
        }

        public void DeleteAssignment(string id, string modifiedBy, DateTime lastModification) {
            try {
                _assignmentService.DeleteAssignment(id, modifiedBy, lastModification);
            } catch (Exception ex) {
                Log.Write(ex);
                throw;
            }
        }

        public ResponseAll GetByUserId(string userId) {
            try {
                return _assignmentService.GetByUserId(userId);
            } catch (Exception ex) {
                Log.Write(ex);
                throw;
            }
        }

		public async Task<int> SyncAssignments(List<Assignment> assignments) {
			try {
				return await _assignmentService.SyncAssignments(assignments);
			} catch (Exception ex) {
				Log.Write(ex);
				throw;
			}
		}

        public int SyncActivities(List<Activity> activities) {
            try {
                return _assignmentService.SyncActivities(activities);
            } catch (Exception ex) {
                Log.Write(ex);
                throw;
            }
        }

        public void SyncDocuments(List<Document> documents) {
            try {
                _assignmentService.SyncDocuments(documents);
            } catch (Exception ex) {
                Log.Write(ex);
                throw;
            }
        }

        public IList<Ocupability> GetOcupabilityLevel(string supervisorId, string idsOffice, string idsCostCenter) {
            try {
                return _ocupabilityService.GetOcupabilityLevel(supervisorId, idsOffice, idsCostCenter);
            } catch (Exception ex) {
                Log.Write(ex);
                throw;
            }
        }

        public IList<OcupabilityThroughTime> GetOcupabilityThroughTime(string supervisorId, int status) {
            try {
                return _ocupabilityThroughTimeService.GetOcupabilityThroughTime(supervisorId, status);
            } catch (Exception ex) {
                Log.Write(ex);
                throw;
            }
        }

        public IList<Assignment> GetAssignmentByOrder(string orderid) {
            try {
                return _assignmentService.GetAssignmentByOrder(orderid);
            } catch (Exception ex) {
                Log.Write(ex);
                throw;
            }
        }

        public IList<Activity> GetActivityByIdAndUserId(string Id, string userId) {
            try {
                return _assignmentService.GetActivityByIdAndUserId(Id, userId);
            } catch (Exception) {

                throw;
            }
        }

        //SUPERVISOR 
        public ResponseAll GetByOTs(Request request) {
            try {
                return _assignmentService.GetOTs(request.Assignment, request.PageIndex, request.PageSize);
            } catch (Exception ex) {
                Log.Write(ex);
                throw;
            }
        }

        public ResponseAll GetDetailTechnician(string assignmentId, string userId) {
            try {
                return _assignmentService.GetDetailTechnician(assignmentId, userId);
            } catch (Exception ex) {
                Log.Write(ex);
                throw;
            }
        }

        public Assignment GetReportById(string assignmentId, int rol) {
            try {
                return _assignmentService.GetReportByProfile(assignmentId, rol);
            } catch (Exception ex) {
                Log.Write(ex);
                throw;
            }
        }

        public async Task<int> UpdateExecutiveReport(Report report) {
            try {
                return await _assignmentService.UpdateExecutiveReport(report);
            } catch (Exception ex) {
                Log.Write(ex);
                throw;
            }
        }

        public async Task<int> UpdateArchiveInformFinal(Report report) {
            try {
                return await _assignmentService.UpdateArchiveInformFinal(report);
            } catch (Exception ex) {
                Log.Write(ex);
                throw;
            }
        }

        public async Task<int> UpdateInformFinalDataSupervisor(Report reportlocal) {
            try {
                return await  _assignmentService.UpdateInformFinalDataSupervisor(reportlocal);
            } catch (Exception ex) {
                Log.Write(ex);
                throw;
            }
        }
        public int UpdateUrlInformFinalSupervisor(Report report) {
            try {
                return _assignmentService.UpdateUrlInformFinalSupervisor(report);
            } catch (Exception ex) {
                Log.Write(ex);
                 throw;
            }
        }

        //TÉCNICO
        public async Task<int> SyncActivitiesDocuments(Request request) {
			try {
				return await _assignmentService.SyncActivitiesDocuments(request);
			} catch (Exception ex) {
				Log.Write(ex);
				throw;
			}
		}

		public int SyncReport(Report item) {
			try {
				return _assignmentService.SyncReport(item);
			} catch (Exception ex) {
				Log.Write(ex);
				throw;
			}
		}

        public int AddFileReportSupervisor(File file) {
            try {
                return  _assignmentService.AddFileReportSupervisor(file);
            } catch (Exception ex) {
                Log.Write(ex);
                throw;
            }
        }

        public int DeleteFileReportSupervisor(File file) {
            try {
                return _assignmentService.DeleteFileReportSupervisor(file);
            } catch (Exception ex) {
                Log.Write(ex);
                throw;
            }
        }

        public int DeleteArchiveInformFinal(Report report) {
            try {
                return _assignmentService.DeleteArchiveInformFinal(report);
            } catch (Exception ex) {
                Log.Write(ex);
                throw;
            }
        }

        public int UpdateSendFiletoReportSupervisor(Report report) {
            try {
                return _assignmentService.UpdateSendFiletoReportSupervisor(report);
            } catch (Exception ex) {
                Log.Write(ex);
                throw;
            }
        }
        #endregion
    }


}
