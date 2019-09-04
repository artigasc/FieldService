using System.Collections.Generic;

namespace FESA.SCM.FieldService.BE.DocumentBE {
	public interface IDocumentRepository {
		void InsertDocuments(IList<Document> documents);
		IList<Document> GetDocumentsByAssignmentByUserId(string assignmentId, string userId);
		IList<Document> GetDocumentsByAssignmentId(string assignmentid);


		//NEW 
		IList<DocumentEntity> GetAllEntityDocument();
		IList<File> GetAllFileByRef(string idRef);

	}
}