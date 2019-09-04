using System.Collections.Generic;
using System.Linq;
using CoreGraphics;
using FESA.SCM.Core.Models;
using FESA.SCM.Core.ViewModels;
using FESA.SCM.iPhone.Controls;
using FESA.SCM.iPhone.Helpers;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using UIKit;

namespace FESA.SCM.iPhone.Controllers
{
    public class DocumentsViewController : BaseController
    {
        private readonly AssignmentVm _assignmentVm;
        private readonly INavigationService _navigationService;
        public DocumentsViewController()
        {
            _assignmentVm = ServiceLocator.Current.GetInstance<AssignmentVm>();
            _navigationService = ServiceLocator.Current.GetInstance<INavigationService>();
            BuildInterface();
        }

        public async void BuildInterface()
        {
            await _assignmentVm.LoadDocumentsByAssignmentAsync();

            var userDocuments = GetDocumentList()
                .GroupJoin(_assignmentVm.SelectedAssignment.Documents, d => d.DocumentType, sd => sd.DocumentType,
                    (doc, docs) => new {Current = doc, Currents = docs.DefaultIfEmpty()})
                .SelectMany(
                    x => x.Currents.Select(a => new Document
                    {
                        AssignmentId = a?.AssignmentId,
                        Name = x.Current.Name,
                        Id = a?.Id,
                        DocumentType = x.Current.DocumentType
                    })).ToList();

            View.BackgroundColor = new UIImage("Images/fondoPrincipal.jpg").GetScaledImageBackground(View);
            var header = new Header(View.Frame.Width)
            {
                LocationTitle = "DOCUMENTOS",
                LeftButtonImage = "Images/btn-atras.png",
                ButtonTouch = async (sender, args) =>
                {
                    await _assignmentVm.SaveDocumentsAsync();
                    _navigationService.GoBack();
                }
            };

            var documentList = new UITableView
            {
                Frame =
                    new CGRect(0, header.Frame.Height, header.Frame.Width, View.Frame.Height - header.Frame.Height - 30),
                Source = new DocumentTableSource(userDocuments),
                SeparatorColor = UIColor.LightGray,
                BackgroundColor = UIColor.Clear,
                ScrollEnabled = false,
                RowHeight = 60,
                PagingEnabled = true
            };

            var copyRight = new UILabel
            {
                Text = "2016 Ferreyros | Derechos Reservados",
                TextAlignment = UITextAlignment.Center,
                TextColor = UIColor.White,
                Font = UIFont.FromName("Helvetica-Light", 10f),
                Frame = new CGRect(0, View.Frame.Height - 30, View.Frame.Width, 20)
            };

            View.AddSubviews(header, documentList ,copyRight);
        }

        private List<Document> GetDocumentList()
        {
            return new List<Document>
            {
                new Document
                {
                    DocumentType = Enumerations.DocumentType.TechnicalReport,
                    Name = "INFORME TÉCNICO"
                },
                new Document
                {
                    DocumentType = Enumerations.DocumentType.TripExpense,
                    Name = "DETALLE DE GASTO"
                },
                new Document
                {
                    DocumentType = Enumerations.DocumentType.Agreement,
                    Name = "ACTA DE CONFORMIDAD"
                },
                new Document
                {
                    DocumentType = Enumerations.DocumentType.Sims,
                    Name = "SIMS"
                },
                new Document
                {
                    DocumentType = Enumerations.DocumentType.SecurityDocument,
                    Name = "DOCUMENTO DE SEGURIDAD"
                },
                new Document
                {
                    DocumentType = Enumerations.DocumentType.PartsGuideSigned,
                    Name = "GUÍA DE REPUESTOS FIRMADA POR CLIENTE"
                },
                new Document
                {
                    DocumentType = Enumerations.DocumentType.VanChecking,
                    Name = "CHECKING DE CAMIONETA"
                }
            };
        }
    }
}