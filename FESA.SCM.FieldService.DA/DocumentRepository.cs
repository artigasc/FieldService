using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using FESA.SCM.Common;
using FESA.SCM.FieldService.BE.DocumentBE;
using FESA.SCM.FieldService.DA.TableTypes;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace FESA.SCM.FieldService.DA {
	public class DocumentRepository : IDocumentRepository {

		public void InsertDocuments(IList<Document> documents) {
			var usuarios = new DocumentTableType();
			usuarios.AddRange(documents);
			if (usuarios.Count == 0) return;
			var database = DatabaseFactory.CreateDatabase();
			var cmd = database.GetStoredProcCommand("BULK_INSERT_DOCUMENTS_SP");
			cmd.Parameters.Add(new SqlParameter {
				ParameterName = "@DOCUMENTS",
				Value = usuarios,
				DbType = DbType.Object,
				SqlDbType = SqlDbType.Structured,
				TypeName = "DOCUMENT"
			});
			database.ExecuteNonQuery(cmd);
		}

		//MODIFY
		public IList<Document> GetDocumentsByAssignmentByUserId(string assignmentId, string userId) {
			IList<Document> documents;
			var database = DatabaseFactory.CreateDatabase();
			using (var cmd = database.GetStoredProcCommand("GET_DOCUMENTS_BY_ASSIGNMENTID_SP", assignmentId, userId)) {
				using (var reader = database.ExecuteReader(cmd)) {
					documents = new List<Document>();
					while (reader.Read()) {
						documents.Add(new Document {
							Id = DataConvert.ToString(reader["STRID"]),
							Name = DataConvert.ToString(reader["STRNAME"]),
							ActivityId = DataConvert.ToString(reader["STRIDACTIVITY"]),
							DocumentId = DataConvert.ToString(reader["STRIDDOCUMENT"]),
							Position = DataConvert.ToInt32(reader["INTPOSITION"]),
							ActivityValue = DataConvert.ToInt32(reader["INTVALUE"]),
							Text = DataConvert.ToString(reader["STRTEXT"]),
							Check = DataConvert.ToBool(reader["BITCHECK"]),
							Date = DataConvert.ToDateTime(reader["DTTDATE"]),
							AssignmentId = assignmentId,
							UserId = userId,
							//Active = DataConvert.ToBool(reader["ACTIVE"])
						});
					}
				}
			}
			return documents;
		}

		public IList<Document> GetDocumentsByAssignmentId(string assignmentId) {
			IList<Document> documents;
			var database = DatabaseFactory.CreateDatabase();
			using (var cmd = database.GetStoredProcCommand("GET_DOCUMENTS_BY_ONLYASSIGNMENTID_SP", assignmentId)) {
				using (var reader = database.ExecuteReader(cmd)) {
					documents = new List<Document>();
					while (reader.Read()) {
						documents.Add(new Document {
							Id = DataConvert.ToString(reader["ID"]),
							AssignmentId = assignmentId,
							UserId = "",
							Name = DataConvert.ToString(reader["NAME"]),
						});
					}
				}
			}
			return documents;
		}


		//NEW 
		public IList<DocumentEntity> GetAllEntityDocument() {
			IList<DocumentEntity> items = null;
			var database = DatabaseFactory.CreateDatabase();
			using (var cmd = database.GetStoredProcCommand("TSP_DOCUMENTENTITY_GETALL")) {
				using (var reader = database.ExecuteReader(cmd)) {
					items = new List<DocumentEntity>();
					while (reader.Read()) {
						items.Add(new DocumentEntity {
							Id = DataConvert.ToString(reader["STRID"]),
							Code = DataConvert.ToString(reader["STRCODE"]),
							Name = DataConvert.ToString(reader["STRNAME"]),
							Position = DataConvert.ToInt32(reader["INTPOSITION"]),
							Popup = DataConvert.ToBool(reader["BITCHECK"])
						});
					}
				}
			}
			return items;
		}

		public IList<File> GetAllFileByRef(string idRef) {
			IList<File> items = null;
			var database = DatabaseFactory.CreateDatabase();
			using (var cmd = database.GetStoredProcCommand("TSP_FILE_GETALLBYREF", idRef)) {
				using (var reader = database.ExecuteReader(cmd)) {
					items = new List<File>();
					while (reader.Read()) {
						items.Add(new File {
							Id = DataConvert.ToString(reader["STRID"]),
							IdRef = DataConvert.ToString(reader["STRIDREF"]),
							Name = DataConvert.ToString(reader["STRNAME"]),
							Ext = DataConvert.ToString(reader["STREXT"]),
							URL = DataConvert.ToString(reader["STRURL"]),
							Active = DataConvert.ToBool(reader["BITACTIVE"]),
							CreationDate = DataConvert.ToDateTime(reader["DTTCREATIONDATE"])
						});
					}
				}
			}
			return items;
		}




	}
}