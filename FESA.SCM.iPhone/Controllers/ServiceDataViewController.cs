using System.Collections.Generic;
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
    public class ServiceDataViewController : BaseController
    {
        private readonly INavigationService _navigationService;
        private readonly AssignmentVm _assignmentVm;
        private readonly List<KeyValuePair<string, string>> _serviceData;

        public ServiceDataViewController()
        {
            _navigationService = ServiceLocator.Current.GetInstance<INavigationService>();
            _assignmentVm = ServiceLocator.Current.GetInstance<AssignmentVm>();
            _serviceData = BuildServiceData();
            BuildInterface();
        }

        private void BuildInterface()
        {
            View.BackgroundColor = new UIImage("Images/fondoInicio.jpg").GetScaledImageBackground(View);
            
            var header = new Header(View.Frame.Width)
            {
                LocationTitle = GetTitle(),
                LeftButtonImage = "Images/btn-atras.png",
                ButtonTouch = (sender, args) => { _navigationService.GoBack(); }
            };

            var background = new UIView
            {
                Frame =
                    new CGRect(0, header.Frame.Height, View.Frame.Width,
                        View.Frame.Height - header.Frame.Height - 30),
                BackgroundColor = UIColor.White
            };

            var serviceData = new UITableView
            {
                Frame = new CGRect(0,0, background.Frame.Width, background.Frame.Height),
                BackgroundColor = UIColor.Clear,
                SeparatorColor = UIColor.LightGray,
                UserInteractionEnabled = false,
                ScrollEnabled = false,
                Source = new DetailsTableSource(_serviceData),
                RowHeight = UITableView.AutomaticDimension,
                EstimatedRowHeight = 60
            };

            background.AddSubviews(serviceData);

            var copyRight = new UILabel
            {
                Text = "2016 Ferreyros | Derechos Reservados",
                TextAlignment = UITextAlignment.Center,
                TextColor = UIColor.White,
                Font = UIFont.FromName("Helvetica-Light", 10f),
                Frame = new CGRect(0, View.Frame.Height - 30, View.Frame.Width, 20)
            };

            View.AddSubviews(header, background, copyRight);
        }

        private string GetTitle()
        {
            string title;
            switch (_assignmentVm.SelectedServiceDataType)
            {
                case Enumerations.ServiceDataType.ServiceData:
                    title = "Datos de Servicio";
                    break;
                case Enumerations.ServiceDataType.MachineData:
                    title = "Datos de Máquina";
                    break;
                case Enumerations.ServiceDataType.TechnicalContact:
                    title = "Contácto Técnico";
                    break;
                case Enumerations.ServiceDataType.FesaTeam:
                    title = "Equipo de Ferreyros";
                    break;
                default:
                    title = string.Empty;
                    break;
            }
            return title;
        }

        private List<KeyValuePair<string, string>> BuildServiceData()
        {
            List<KeyValuePair<string, string>> serviceData;
            switch (_assignmentVm.SelectedServiceDataType)
            {
                case Enumerations.ServiceDataType.ServiceData:
                    serviceData = new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("Detalle", _assignmentVm.SelectedAssignment.Description),
                        new KeyValuePair<string, string>("Fecha de solicitud del cliente", _assignmentVm.SelectedAssignment.RegisterDate.ToString("dd/MM/yyyy")),
                        new KeyValuePair<string, string>("Fecha estimada de inicio", _assignmentVm.SelectedAssignment.EstimatedStartDate.ToString("dd/MM/yyyy")),
                        new KeyValuePair<string, string>("Fecha estimada de fin", _assignmentVm.SelectedAssignment.EstimatedEndDate.ToString("dd/MM/yyyy")),
                        new KeyValuePair<string, string>("Ubicacion", _assignmentVm.SelectedAssignment.Location?.District),
                        new KeyValuePair<string, string>("Departamento", _assignmentVm.SelectedAssignment.Location?.Department)
                    };
                    break;
                case Enumerations.ServiceDataType.MachineData:
                    serviceData = new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("Marca", _assignmentVm.SelectedAssignment.Machine?.Brand),
                        new KeyValuePair<string, string>("Modelo", _assignmentVm.SelectedAssignment.Machine?.Model),
                        new KeyValuePair<string, string>("N/S", _assignmentVm.SelectedAssignment.Machine?.SerialNumber),
                        new KeyValuePair<string, string>("Horometro", _assignmentVm.SelectedAssignment.Machine?.TotalHoursFunction.ToString())
                    };
                    break;
                case Enumerations.ServiceDataType.TechnicalContact:
                    serviceData = new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("Nombre", _assignmentVm.SelectedAssignment.TechnicalContact?.Name),
                        new KeyValuePair<string, string>("Cargo", _assignmentVm.SelectedAssignment.TechnicalContact?.Charge),
                        new KeyValuePair<string, string>("Teléfono", _assignmentVm.SelectedAssignment.TechnicalContact?.Phone),
                        new KeyValuePair<string, string>("Celular", _assignmentVm.SelectedAssignment.TechnicalContact?.CellPhone),
                        new KeyValuePair<string, string>("Email", _assignmentVm.SelectedAssignment.TechnicalContact?.Mail)
                    };
                    break;
                case Enumerations.ServiceDataType.FesaTeam:
                    serviceData = new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("Nombre", _assignmentVm.SelectedAssignment.FerreyrosContact?.Name),
                        new KeyValuePair<string, string>("Cargo", _assignmentVm.SelectedAssignment.FerreyrosContact?.Charge),
                        new KeyValuePair<string, string>("Teléfono", _assignmentVm.SelectedAssignment.FerreyrosContact?.Phone),
                        new KeyValuePair<string, string>("Celular", _assignmentVm.SelectedAssignment.FerreyrosContact?.CellPhone),
                        new KeyValuePair<string, string>("Email", _assignmentVm.SelectedAssignment.FerreyrosContact?.Mail)
                    };
                    break;
                default:
                    serviceData = new List<KeyValuePair<string, string>>();
                    break;
            }
            return serviceData;
        }
    }
}