using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using FESA.SCM.Common;
using FESA.SCM.FieldService.BE.AssignmentBE;
using FESA.SCM.FieldService.BE.DocumentBE;
using FESA.SCM.FieldService.DA.TableTypes;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace FESA.SCM.FieldService.DA {
	public class AssignmentRepository : IAssignmentRepository {
		public IList<Assignment> GetAll() {
			throw new NotImplementedException();
		}

		public IList<Assignment> GetPaginated(Assignment filters, int pageIndex, int pageSize, out int totalRows) {
			throw new NotImplementedException();
		}


		public IList<dynamic> GetByPersonnelIdAssignment(string id) {
			List<dynamic> users;
			var database = DatabaseFactory.CreateDatabase();
			using (var cmd = database.GetStoredProcCommand("GET_USERS_BY_ASSIGNMENT_SP", id)) {
				using (var reader = database.ExecuteReader(cmd)) {
					users = new List<dynamic>();
					while (reader.Read()) {
						dynamic item = new ExpandoObject();
						item.Id = DataConvert.ToString(reader["ID"]);
						item.Name = DataConvert.ToString(reader["NAME"]);
						item.Email = DataConvert.ToString(reader["EMAIL"]);
						item.Phone = DataConvert.ToString(reader["PHONE"]);
						item.Photo = DataConvert.ToString(reader["PHOTO"]);
						item.UserType = DataConvert.ToInt32(reader["USERTYPE"]);
						item.UserStatus = DataConvert.ToInt32(reader["USERSTATUS"]);
						item.RoleName = DataConvert.ToString(reader["ROLENAME"]);
						item.Celullar = DataConvert.ToString(reader["CELULLAR"]);
						item.Rpm = DataConvert.ToString(reader["RPM"]);
						users.Add(item);
					}
				}
			}
			return users;
		}

		public IList<dynamic> GetContactByAssignemtnId(string id) {
			List<dynamic> contacts;
			var database = DatabaseFactory.CreateDatabase();
			using (var cmd = database.GetStoredProcCommand("TSP_ASSIGNMENT_GETCONTACTSBYID", id)) {
				using (var reader = database.ExecuteReader(cmd)) {
					contacts = new List<dynamic>();
					while (reader.Read()) {
						dynamic item = new ExpandoObject();
						item.Id = DataConvert.ToString(reader["ID"]);
						item.ContactId = DataConvert.ToString(reader["CONTACTID"]);
						item.CustomerId = DataConvert.ToString(reader["CUSTOMERID"]);
						item.Name = DataConvert.ToString(reader["NAME"]);
						item.LastName = DataConvert.ToString(reader["LASTNAME"]);
						item.Email = DataConvert.ToString(reader["EMAIL"]);
						item.Charge = DataConvert.ToString(reader["CHARGE"]);
						item.Phone = DataConvert.ToString(reader["PHONE"]);
						item.CompanyName = "";
						contacts.Add(item);
					}
				}
			}
			return contacts;
		}

		public Assignment GetById(string id) {
			Assignment assignment = null;
			var database = DatabaseFactory.CreateDatabase();
			using (var cmd = database.GetStoredProcCommand("GET_ASSIGNMENT_BYID_SP", id)) {
				using (var reader = database.ExecuteReader(cmd)) {
					while (reader.Read()) {
						assignment = new Assignment {
							Id = DataConvert.ToString(reader["ID"]),
							WorkOrderId = DataConvert.ToString(reader["WORKORDERID"]),
							Location = new Location {
								Id = DataConvert.ToString(reader["LOCATIONID"]),
								District = DataConvert.ToString(reader["UBICACION"]),
								Province = DataConvert.ToString(reader["PROVINCE"]),
								Department = DataConvert.ToString(reader["DEPARTMENT"])
							},
							AssignmentType = (AssignmentType)DataConvert.ToInt32(reader["ASSIGNMENTTYPE"]),
							Status = (AssignmentStatus)DataConvert.ToInt32(reader["ASSIGNMENTSTATUS"]),
							Description = DataConvert.ToString(reader["DESCRIPTION"]),
							RequestDate = DataConvert.ToDateTime(reader["REQUESTDATE"]),
							EstimatedStartDate = DataConvert.ToDateTime(reader["ESTIMATEDSTARTDATE"]),
							EstimatedEndDate = DataConvert.ToDateTime(reader["ESTIMATEDENDDATE"]),
							StartDate = DataConvert.ToDateTime(reader["STARTDATE"]),
							EndDate = DataConvert.ToDateTime(reader["ENDDATE"])
						};
					}
				}
			}
			return assignment;
		}

		public Assignment GetByWorkOrder(string workOrderId) {
			Assignment assignment = null;
			var database = DatabaseFactory.CreateDatabase();
			using (var cmd = database.GetStoredProcCommand("GET_ASSIGNMENT_BYWORKORDER_SP", workOrderId)) {
				using (var reader = database.ExecuteReader(cmd)) {
					while (reader.Read()) {
						assignment = new Assignment {
							Id = DataConvert.ToString(reader["ID"]),
							WorkOrderId = DataConvert.ToString(reader["WORKORDERID"]),
							//AssignmentType = (AssignmentType)DataConvert.ToInt32(reader["ASSIGNMENTTYPE"]),
							//AssignmentStatus = (AssignmentStatus)DataConvert.ToInt32(reader["ASSIGNMENTSTATUS"]),
							//Priority = DataConvert.ToInt32(reader["PRIORITY"]),
							//Description = DataConvert.ToString(reader["DESCRIPTION"]),
							//RequestDate = DataConvert.ToDateTime(reader["REQUESTDATE"]),
							//EstimatedStartDate = DataConvert.ToDateTime(reader["ESTIMATEDSTARTDATE"]),
							//EstimatedEndDate = DataConvert.ToDateTime(reader["ESTIMATEDENDDATE"]),
							//StartDate = DataConvert.ToDateTime(reader["STARTDATE"]),
							//EndDate = DataConvert.ToDateTime(reader["ENDDATE"])
						};
					}
				}
			}
			return assignment;

		}

		public void Add(Assignment entity) {
			DatabaseFactory.CreateDatabase()
				.ExecuteNonQuery("INSERT_ASSIGNMENT_SP", entity.Id, entity.CiaId, entity.RequestId, entity.WorkOrderId,
					entity.Location.Id, entity.EstimatedStartDate, entity.EstimatedEndDate, entity.RequestDate,
					entity.Description, entity.StartDate, entity.Priority, (int)entity.Status,
					(int)entity.AssignmentType, entity.CreatedBy, entity.CreationDate);
		}

		public void Update(Assignment entity) {
			DatabaseFactory.CreateDatabase()
				.ExecuteNonQuery("UPDATE_ASSIGNMENT_SP", entity.Id, entity.CiaId, entity.RequestId, entity.WorkOrderId,
					entity.Location.Id, entity.EstimatedStartDate, entity.EstimatedEndDate, entity.RequestDate,
					entity.Description, entity.StartDate, entity.Priority, (int)entity.Status,
					(int)entity.AssignmentType, entity.CreatedBy, entity.CreationDate);
		}

		public int UpdateStatusByLider(Assignment entity) {
			return DatabaseFactory.CreateDatabase()
				.ExecuteNonQuery("TSP_ASSIGNMENT_UPDATESTATUST_LIDER", entity.Id, entity.UserId, entity.StartDate, entity.EndDate
							, entity.NumberPlate, (int)entity.Status, entity.ModifiedBy);
		}

		public int UpdateDateStart(Assignment entity) {
			return DatabaseFactory.CreateDatabase()
				.ExecuteNonQuery("TSP_ASSIGNMENT_UPDATEDATESTART", entity.Id, entity.UserId, entity.StartDate
							, entity.ModifiedBy);
		}



		public void Delete(string id, string modifiedBy, DateTime lastModification) {
			DatabaseFactory.CreateDatabase().ExecuteNonQuery("DELETE_ASSINGMENT_SP", id, modifiedBy, lastModification);
		}


		public int GetAssignmentCountByUser(string userId) {
			int count = 0;
			var database = DatabaseFactory.CreateDatabase();
			using (var cmd = database.GetStoredProcCommand("GET_ASSIGNMENTSTATUS_BYUSER", userId)) {
				using (var reader = database.ExecuteReader(cmd)) {
					while (reader.Read()) {
						count = DataConvert.ToInt(reader["TOTAL"]);
					}
				}
			}
			return count;
		}

		public void SyncAssignments(IList<Assignment> assignments) {
			var usuarios = new AssignmentTableType();
			usuarios.AddRange(assignments);
			if (usuarios.Count == 0)
				return;
			var database = DatabaseFactory.CreateDatabase();
			var cmd = database.GetStoredProcCommand("BULK_INSERT_ASSIGNMENT_SP");
			cmd.Parameters.Add(new SqlParameter {
				ParameterName = "@ASSIGNMENTS",
				Value = usuarios,
				DbType = DbType.Object,
				SqlDbType = SqlDbType.Structured,
				TypeName = "ASSIGNMENT_TYPE"
			});
			database.ExecuteNonQuery(cmd);
		}

		public IList<Assignment> GetByUserId(string userId) {
			IList<Assignment> assignments;
			var database = DatabaseFactory.CreateDatabase();
			using (var cmd = database.GetStoredProcCommand("GET_ASSIGNMENTS_BY_USERID_SP", userId)) {
				using (var reader = database.ExecuteReader(cmd)) {
					assignments = new List<Assignment>();
					while (reader.Read()) {
						assignments.Add(new Assignment {
							Id = DataConvert.ToString(reader["ID"]),
							WorkOrderId = DataConvert.ToString(reader["WORKORDERID"]),
							Location = new Location {
								Id = DataConvert.ToString(reader["LOCATIONID"]),
								AssignmentId = DataConvert.ToString(reader["ID"]),
								District = DataConvert.ToString(reader["DISTRICT"]),
								Province = DataConvert.ToString(reader["PROVINCE"]),
								Department = DataConvert.ToString(reader["DEPARTMENT"]),
								Ubicacion = DataConvert.ToString(reader["UBICACION"]),
							},
							AssignmentType = (AssignmentType)DataConvert.ToInt32(reader["ASSIGNMENTTYPE"]),
							Status = (AssignmentStatus)DataConvert.ToInt32(reader["ASSIGNMENTSTATUS"]),
							Priority = DataConvert.ToInt32(reader["PRIORITY"]),
							Description = DataConvert.ToString(reader["DESCRIPTION"]),
							RequestDate = DataConvert.ToDateTime(reader["REQUESTDATE"]),
							EstimatedStartDate = DataConvert.ToDateTime(reader["ESTIMATEDSTARTDATE"]),
							EstimatedEndDate = DataConvert.ToDateTime(reader["ESTIMATEDENDDATE"]),
							StartDate = DataConvert.ToDateTime(reader["STARTDATE"]),
							EndDate = DataConvert.ToDateTime(reader["ENDDATE"]),
							Active = DataConvert.ToBool(reader["ACTIVE"])
						});
					}
				}
			}
			return assignments;
		}

		public void AddPersonnel(IList<Personnel> personnel) {
			var usuarios = new PersonnelTableType();
			usuarios.AddRange(personnel);
			if (usuarios.Count == 0)
				return;
			var database = DatabaseFactory.CreateDatabase();
			var cmd = database.GetStoredProcCommand("BULK_INSERT_PERSONNEL_SP");
			cmd.Parameters.Add(new SqlParameter {
				ParameterName = "@PERSONNEL",
				Value = usuarios,
				DbType = DbType.Object,
				SqlDbType = SqlDbType.Structured,
				TypeName = "PERSONNEL_TYPE"
			});
			database.ExecuteNonQuery(cmd);
		}

		public IList<Personnel> GetByAssignmentId(string assignmentId) {
			IList<Personnel> assignnedPersonnel;
			var database = DatabaseFactory.CreateDatabase();
			using (var cmd = database.GetStoredProcCommand("GET_PERSONNEL_BY_ASSIGNMENTID_SP", assignmentId)) {
				using (var reader = database.ExecuteReader(cmd)) {
					assignnedPersonnel = new List<Personnel>();
					while (reader.Read()) {
						assignnedPersonnel.Add(new Personnel {
							Id = DataConvert.ToString(reader["ID"]),
							AssignmentId = assignmentId,
							PersonnelType = (PersonnelType)DataConvert.ToInt32(reader["PERSONNELTYPE"])
						});
					}
				}
			}
			return assignnedPersonnel;
		}

		public void AddLocation(Location entity) {
			DatabaseFactory.CreateDatabase()
				.ExecuteNonQuery("INSERT_LOCATION_SP", entity.Id
				, entity.AssignmentId
				, entity.Department
				, entity.Province
				, entity.District
				, entity.Ubicacion);
			//entity.CiaId, entity.RequestId, entity.WorkOrderId,
			//    entity.Location.Id, entity.EstimatedStartDate, entity.EstimatedEndDate, entity.RequestDate,
			//    entity.Description, entity.StartDate, entity.Priority, (int)entity.AssignmentStatus,
			//    (int)entity.AssignmentType, entity.CreatedBy, entity.CreationDate);
		}

		public IList<Assignment> GetAssignmentByOrder(string orderid) {
			IList<Assignment> assignments;
			var database = DatabaseFactory.CreateDatabase();
			using (var cmd = database.GetStoredProcCommand("GET_ASSIGNMENTS_BY_ORDERID_SP", orderid)) {
				using (var reader = database.ExecuteReader(cmd)) {
					assignments = new List<Assignment>();
					while (reader.Read()) {
						assignments.Add(new Assignment {
							Id = DataConvert.ToString(reader["ID"]),
							WorkOrderId = DataConvert.ToString(reader["WORKORDERID"]),
							Location = new Location {
								Id = DataConvert.ToString(reader["LOCATIONID"]),
								AssignmentId = DataConvert.ToString(reader["ID"]),
								District = DataConvert.ToString(reader["DISTRICT"]),
								Province = DataConvert.ToString(reader["PROVINCE"]),
								Department = DataConvert.ToString(reader["DEPARTMENT"]),
								Ubicacion = DataConvert.ToString(reader["UBICACION"]),
							},
							AssignmentType = (AssignmentType)DataConvert.ToInt32(reader["ASSIGNMENTTYPE"]),
							Status = (AssignmentStatus)DataConvert.ToInt32(reader["ASSIGNMENTSTATUS"]),
							Priority = DataConvert.ToInt32(reader["PRIORITY"]),
							Description = DataConvert.ToString(reader["DESCRIPTION"]),
							RequestDate = DataConvert.ToDateTime(reader["REQUESTDATE"]),
							CreationDate = DataConvert.ToDateTime(reader["CREATIONDATE"]),
							EstimatedStartDate = DataConvert.ToDateTime(reader["ESTIMATEDSTARTDATE"]),
							EstimatedEndDate = DataConvert.ToDateTime(reader["ESTIMATEDENDDATE"]),
							StartDate = DataConvert.ToDateTime(reader["STARTDATE"]),
							EndDate = DataConvert.ToDateTime(reader["ENDDATE"])
						});
					}
				}
			}
			return assignments;
		}

		public void UpdateStatusUser(string id, int userStatus) {
			//PRUEBAANTONIO
			DatabaseFactory.CreateDatabase()
				.ExecuteNonQuery("UPDATESTATUS_USER_SP", id, userStatus);
		}

		public int UpdatePersonnelStatus(string idUser, string idAssignment, AssignmentStatus assignmentStatus) {
			return DatabaseFactory.CreateDatabase()
			   .ExecuteNonQuery("UPDATE_PERSONNEL_STATUS", idUser, idAssignment, (int)assignmentStatus);
		}


		//WEB SUPERVISOR
		public IList<Assignment> GetByOTs(Assignment item, int pageIndex, int pageSize) {
			IList<Assignment> assignments;
			var database = DatabaseFactory.CreateDatabase();
			using (var cmd = database.GetStoredProcCommand("GET_ASSIGNMENTS_WEB", item.WorkOrderNumber, item.CompanyName,
															item.StartDate, item.EndDate, item.Office, item.CostCenter, pageIndex, pageSize)) {
				using (var reader = database.ExecuteReader(cmd)) {
					assignments = new List<Assignment>();
					while (reader.Read()) {
						assignments.Add(new Assignment {
							Id = DataConvert.ToString(reader["ID"]),
							WorkOrderId = DataConvert.ToString(reader["WORKORDERID"]),
							Description = DataConvert.ToString(reader["DESCRIPTION"]),
							StartDate = DataConvert.ToDateTime(reader["STARTDATE"]),
							EndDate = DataConvert.ToDateTime(reader["ENDDATE"]),
							AssignmentType = (AssignmentType)DataConvert.ToInt32(reader["ASSIGNMENTTYPE"]),
							Status = (AssignmentStatus)DataConvert.ToInt32(reader["ASSIGNMENTSTATUS"]),
							//RequestDate = DataConvert.ToDateTime(reader["REQUESTDATE"]),
							//EstimatedStartDate = DataConvert.ToDateTime(reader["ESTIMATEDSTARTDATE"]),
							//EstimatedEndDate = DataConvert.ToDateTime(reader["ESTIMATEDENDDATE"]),
							Location = new Location {
								Id = DataConvert.ToString(reader["LOCATIONID"]),
								AssignmentId = DataConvert.ToString(reader["ID"]),
								District = DataConvert.ToString(reader["DISTRICT"]),
								Province = DataConvert.ToString(reader["PROVINCE"]),
								Department = DataConvert.ToString(reader["DEPARTMENT"]),
								Ubicacion = DataConvert.ToString(reader["UBICACION"]),
							},
							Active = DataConvert.ToBool(reader["ACTIVE"]),
							Rating = DataConvert.ToString(reader["RATING"])
						});
					}
				}
			}
			return assignments;
		}

		public int GetByOTsCount(Assignment item, int pageIndex, int pageSize) {
			int count = 0;
			var database = DatabaseFactory.CreateDatabase();
			using (var cmd = database.GetStoredProcCommand("GET_ASSIGNMENTSCOUNT_WEB", item.WorkOrderNumber, item.CompanyName,
															item.StartDate, item.EndDate, item.Office, item.CostCenter, pageIndex, pageSize)) {
				using (var reader = database.ExecuteReader(cmd)) {
					while (reader.Read()) {
						count = DataConvert.ToInt32(reader["COUNT"]);
					}
				}
				return count;
			}
		}


		public IList<Assignment> GetByOTsReport(Assignment item, int pageIndex, int pageSize) {
			IList<Assignment> assignments;
			var database = DatabaseFactory.CreateDatabase();
			using (var cmd = database.GetStoredProcCommand("GET_ASSIGNMENTS_REPORT_WEB", item.WorkOrderNumber, item.CompanyName,
															item.StartDate, item.EndDate, item.Office, item.CostCenter, pageIndex, pageSize)) {
				using (var reader = database.ExecuteReader(cmd)) {
					assignments = new List<Assignment>();
					while (reader.Read()) {
						assignments.Add(new Assignment {
							Id = DataConvert.ToString(reader["ID"]),
							WorkOrderId = DataConvert.ToString(reader["WORKORDERID"]),
							WorkOrderNumber = DataConvert.ToString(reader["CODE"]),
							StartDate = DataConvert.ToDateTime(reader["STARTDATE"]),
							EndDate = DataConvert.ToDateTime(reader["ENDDATE"]),
							//REPORT 
							Report = new BE.ReportBE.Report {
								UrlAct = DataConvert.ToString(reader["STRURLACT"]),
								UrlExe = DataConvert.ToString(reader["STRURLEXE"]),
								UrlFile = DataConvert.ToString(reader["STRURLFILE"]),
							},
							Active = DataConvert.ToBool(reader["ACTIVE"]),
						});
					}
				}
			}
			return assignments;
		}

		public int GetByOTsReportCount(Assignment item, int pageIndex, int pageSize) {
			int count = 0;
			var database = DatabaseFactory.CreateDatabase();
			using (var cmd = database.GetStoredProcCommand("GET_ASSIGNMENTS_REPORTCOUNT_WEB", item.WorkOrderNumber, item.CompanyName,
															item.StartDate, item.EndDate, item.Office, item.CostCenter, pageIndex, pageSize)) {
				using (var reader = database.ExecuteReader(cmd)) {
					while (reader.Read()) {
						count = DataConvert.ToInt32(reader["COUNT"]);
					}
				}
				return count;
			}
		}

		public Assignment GetReportTechnician(string assignmentId) {
			Assignment item = null;
			var database = DatabaseFactory.CreateDatabase();
			using (var cmd = database.GetStoredProcCommand("GET_REPORT_TECHNICIAN", assignmentId)) {
				using (var reader = database.ExecuteReader(cmd)) {
					while (reader.Read()) {
						item = new Assignment {
							Id = DataConvert.ToString(reader["ID"]),
							WorkOrderNumber = DataConvert.ToString(reader["CODE"]),
							CompanyName = DataConvert.ToString(reader["STRCOMPANY"]),
							RUC = DataConvert.ToString(reader["RUC"]),
							Description = DataConvert.ToString(reader["DESCRIPTION"]),
							Machine = new Machine {
								AssignmentId = assignmentId,
								Model = DataConvert.ToString(reader["MODEL"]),
								SerialNumber = DataConvert.ToString(reader["SERIALNUMBER"]),
								TotalHoursFunction = DataConvert.ToInt32(reader["LIFEHOURS"]),
								Brand = DataConvert.ToString(reader["BRAND"])
							},
							Report = new BE.ReportBE.Report {
								Id = DataConvert.ToString(reader["STRREPORTID"]),
								Antecedent = DataConvert.ToString(reader["STRANTECEDENT"]),
								Work = DataConvert.ToString(reader["STRWORK"]),
								Observation = DataConvert.ToString(reader["STROBSERVATION"]),
								Replacement = DataConvert.ToString(reader["STRREPLACEMENT"]),
								Comment1 = DataConvert.ToString(reader["STRCOMMENT1"]),
								Obs1 = DataConvert.ToBool(reader["BITOBS1"]),
								Date = DataConvert.ToDateTime(reader["DTTDATE"]),
								Date1 = DataConvert.ToDateTimeNull(reader["DTTDATE1"]),
								Sent1 = DataConvert.ToBool(reader["BITSENT1"]),
								TotalMinute = DataConvert.ToInt32(reader["INTMINUTE"]),
								UrlFile = DataConvert.ToString(reader["STRURLFILE"]),
								UrlExe = DataConvert.ToString(reader["STRURLEXE"]),
								CreationDate = DataConvert.ToDateTime(reader["DTTCREATIONDATE"]),
								Check = DataConvert.ToBool(reader["BITCHECK"]),
								TotalMinuteStandard1 = DataConvert.ToString(reader["INTMINSTAND1"]),
							}
						};
					}
				}
			}
			return item;
		}

		public Assignment GetReportSupervisor(string assignmentId) {
			Assignment item = null;
			var database = DatabaseFactory.CreateDatabase();
			using (var cmd = database.GetStoredProcCommand("GET_REPORT_SUPERVISOR", assignmentId)) {
				using (var reader = database.ExecuteReader(cmd)) {
					while (reader.Read()) {
						item = new Assignment {
							Id = DataConvert.ToString(reader["ID"]),
							WorkOrderNumber = DataConvert.ToString(reader["CODE"]),
							CompanyName = DataConvert.ToString(reader["STRCOMPANY"]),
							RUC = DataConvert.ToString(reader["RUC"]),
							Description = DataConvert.ToString(reader["DESCRIPTION"]),
							RequestDate = DataConvert.ToDateTime(reader["REQUESTDATE"]),
							CorpId = DataConvert.ToString(reader["SUCURSAL"]),
							Machine = new Machine {
								AssignmentId = assignmentId,
								Model = DataConvert.ToString(reader["MODEL"]),
								SerialNumber = DataConvert.ToString(reader["SERIALNUMBER"]),
								TotalHoursFunction = DataConvert.ToInt32(reader["LIFEHOURS"]),
								Brand = DataConvert.ToString(reader["BRAND"])
							},
							Report = new BE.ReportBE.Report {
								Id = DataConvert.ToString(reader["STRREPORTID"]),
								Antecedent = DataConvert.ToString(reader["STRANTECEDENT"]),
								Work = DataConvert.ToString(reader["STRWORK"]),
								Observation = DataConvert.ToString(reader["STROBSERVATION"]),
								Replacement = DataConvert.ToString(reader["STRREPLACEMENT"]),
								Comment1 = DataConvert.ToString(reader["STRCOMMENT1"]),
								Comment2 = DataConvert.ToString(reader["STRCOMMENT2"]),
								Obs1 = DataConvert.ToBool(reader["BITOBS1"]),
								Obs2 = DataConvert.ToBool(reader["BITOBS2"]),
								Date = DataConvert.ToDateTime(reader["DTTDATE"]),
								Date1 = DataConvert.ToDateTimeNull(reader["DTTDATE1"]),
								Date2 = DataConvert.ToDateTimeNull(reader["DTTDATE2"]),
								Sent1 = DataConvert.ToBool(reader["BITSENT1"]),
								Sent2 = DataConvert.ToBool(reader["BITSENT2"]),
								TotalMinute = DataConvert.ToInt32(reader["INTMINUTE"]),
								UrlFile = DataConvert.ToString(reader["STRURLFILE"]),
								UrlExe = DataConvert.ToString(reader["STRURLEXE"]),
								CreationDate = DataConvert.ToDateTime(reader["DTTCREATIONDATE"]),
								Check = DataConvert.ToBool(reader["BITCHECK"]),
								TotalMinuteStandard1 = DataConvert.ToString(reader["INTMINSTAND1"]),
								TotalMinuteStandard2 = DataConvert.ToString(reader["INTMINSTAND2"]),

							}
						};
					}
				}
			}
			return item;

		}

		public Assignment GetDetailMail(string assignmentId) {
			Assignment item = null;
			var database = DatabaseFactory.CreateDatabase();
			using (var cmd = database.GetStoredProcCommand("TSP_ASSIGNMENT_GETDETAILMAIL", assignmentId)) {
				using (var reader = database.ExecuteReader(cmd)) {
					while (reader.Read()) {
						item = new Assignment {
							Id = DataConvert.ToString(reader["ID"]),
							WorkOrderNumber = DataConvert.ToString(reader["CODE"]),
							CompanyName = DataConvert.ToString(reader["STRCOMPANY"]),
							RUC = DataConvert.ToString(reader["RUC"]),
							Description = DataConvert.ToString(reader["DESCRIPTION"]),
							RequestDate = DataConvert.ToDateTime(reader["REQUESTDATE"]),
							Machine = new Machine {
								AssignmentId = assignmentId,
								Model = DataConvert.ToString(reader["MODEL"]),
								SerialNumber = DataConvert.ToString(reader["SERIALNUMBER"]),
								TotalHoursFunction = DataConvert.ToInt32(reader["LIFEHOURS"]),
								Brand = DataConvert.ToString(reader["BRAND"])
							},
							Location = new Location {
								Ubicacion = DataConvert.ToStringNull(reader["UBICACION"]),
							},
						};
					}
				}
			}
			return item;
		}


		//TÉCNICO 
		public int InsertReport(BE.ReportBE.Report entity) {
			return DatabaseFactory.CreateDatabase()
				.ExecuteNonQuery("TSP_REPORT_INSERT", entity.Id, entity.AssignmentId, entity.Antecedent, entity.Work,
					entity.Observation, entity.Replacement, entity.Date, entity.UrlAct, entity.UrlSign, entity.ContactId, entity.Value, entity.Online, entity.CreatedBy);
		}

		public int UpdateExecutiveReport(BE.ReportBE.Report report) {
			return DatabaseFactory.CreateDatabase()
			   .ExecuteNonQuery("TSP_REPORT_UPDATE_EXECUTIVE", report.Id, report.AssignmentId, report.Antecedent, report.Work,
								report.Observation, report.Replacement, report.UrlExe, report.ModifiedBy);
		}

        public int UpdateArchiveInformFinal(BE.ReportBE.Report report) {
            return DatabaseFactory.CreateDatabase()
                .ExecuteNonQuery("TSP_REPORT_UPDATE_FINAL", report.Id, report.AssignmentId, report.TotalMinute, report.Comment1,
                                report.UrlFile, report.Sent1, report.ModifiedBy);
        }

        public int AddFileReportSupervisor(BE.DocumentBE.File file) {
			int i = 0;
			DatabaseFactory.CreateDatabase()
				.ExecuteNonQuery("TSP_FILE_INSERT", file.Id, file.IdRef, file.Name,
								file.Ext, file.URL, file.CreatedBy);
			i = 1;
			return i;

		}

		public int DeleteFileReportSupervisor(BE.DocumentBE.File file) {
			return DatabaseFactory.CreateDatabase()
				.ExecuteNonQuery("TSP_FILE_DELETE", file.Id, file.IdRef);
		}

		public int DeleteArchiveInformFinal(BE.ReportBE.Report report) {
			int i = 0;
			DatabaseFactory.CreateDatabase()
			.ExecuteNonQuery("TSP_REPORT_DELETE_INFORMFINAL", report.Id, report.AssignmentId, report.UrlFile, report.ModifiedBy);
			i = 1;
			return i;
		}

        public IList<File> GetFilesByReference(string refId) {
            IList<File> returnData = new List<File>();
            var database = DatabaseFactory.CreateDatabase();
            using (var cmd = database.GetStoredProcCommand("TSP_FILE_GET_REFERENCE", refId)) {
                using (var reader = database.ExecuteReader(cmd)) {
                    while (reader.Read()) {
                        returnData.Add(
                            new File {
                                Id = DataConvert.ToString(reader["ID"]),
                                Name = DataConvert.ToString(reader["CODE"]),
                                URL = DataConvert.ToString(reader["STRCOMPANY"]),
                            }
                            );
                    }
                }
            }
            return returnData;
        }

        public int UpdateInformFinalDataSupervisor(BE.ReportBE.Report report) {
            return DatabaseFactory.CreateDatabase()
            .ExecuteNonQuery("TSP_REPORT_UPDATE_FINAL_SUPERVISOR", report.Id, report.AssignmentId, report.Comment2, report.Check,
                            report.ModifiedBy);
        }

        public int UpdateUrlInformFinalSupervisor(BE.ReportBE.Report report) {
            return DatabaseFactory.CreateDatabase()
                .ExecuteNonQuery("TSP_REPORT_UPDATE_FINAL_URL", report.Id, report.AssignmentId,report.UrlFile, report.ModifiedBy);
        }
    }
}
