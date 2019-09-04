using System;
using System.Collections.Generic;
using FESA.SCM.Core.Models;
using FESA.SCM.Core.ViewModels;
using Foundation;
using Microsoft.Practices.ServiceLocation;
using UIKit;

namespace FESA.SCM.iPhone.Controls
{
    public class DocumentTableSource : UITableViewSource
    {
        private readonly List<Document> _documents;
        private readonly AssignmentVm _assignmentVm;
        public DocumentTableSource(List<Document> documents)
        {
            _assignmentVm = ServiceLocator.Current.GetInstance<AssignmentVm>();
            _documents = documents;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var document = _documents[indexPath.Row];
            var cell = tableView.DequeueReusableCell("DocumentCell") as DocumentCell ?? new DocumentCell();
            cell.SetDocument(document);
            return cell;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return _documents.Count;
        }
        
        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            var document = _documents[indexPath.Row];

            if (string.IsNullOrEmpty(document.Id))
            {
                document.AssignmentId = _assignmentVm.SelectedAssignment.Id;
                document.Id = Guid.NewGuid().ToString();
                if (_assignmentVm.SelectedAssignment.Documents == null)
                    _assignmentVm.SelectedAssignment.Documents = new List<Document>();
                _assignmentVm.SelectedAssignment.Documents.Add(document);
            }
            else
            {
                _assignmentVm.SelectedAssignment.Documents.Remove(document);
                document.Id = null;
                document.AssignmentId = null;
            }
            
            tableView.BeginUpdates();
            tableView.ReloadRows(new [] { indexPath }, UITableViewRowAnimation.Fade);
            tableView.EndUpdates();
            tableView.DeselectRow(indexPath, true);
        }
    }
}