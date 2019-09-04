using FESA.SCM.Common;
using FESA.SCM.ServiceGateway.DTO;
using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;

namespace FESA.SCM.ServiceGateway.Helper
{
    public static class MailHelper
    {
        public static void SendMail(ActivityType activityType, Assignment assignment = null, Contact contact = null, Machine machine = null, Order workOrder = null, User supervisor = null)
        {
            try
            {
                #region Obtener variables de configuración
                var mailAccount = ConfigurationManager.AppSettings["mailAccount"];
                var mailPassword = ConfigurationManager.AppSettings["mailPassword"];
                var smtp = ConfigurationManager.AppSettings["smtp"];
                var mailDisplayName = ConfigurationManager.AppSettings["mailDisplayName"];
                #endregion
                #region Configurar SMTP
                var smtpClient = new SmtpClient
                {
                    Host = smtp,
                    Port = 587,
                    UseDefaultCredentials = false,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential(mailAccount, mailPassword),
                    EnableSsl = true
                };
                #endregion
                #region Configurar Mail
                var mail = new MailMessage
                {
                    From = new MailAddress(mailAccount, mailDisplayName)
                };
                //mail.To.Add(new MailAddress(workOrder.TechnicalContact.Email));
                mail.To.Add(new MailAddress("manuel.ruiz@cmscloud.pe"));
                mail.IsBodyHtml = true;
                #endregion
                #region Crear cuerpo de correo
                #region Variables de cuerpo de correo
                string htmlBody;
                string subject;
                switch(activityType)
                {
                    case ActivityType.Traveling:
                        string assignedTec0 = null;
                        foreach (Contact item in assignment.TechnicalContacts)
                        {
                            assignedTec0 = assignedTec0 + $"<li> {item.Name} {item.LastName} </li>";
                            
                        }
                        subject = string.Format(Constants.Mailsubject.Traveling, workOrder.Code);
                        htmlBody = string.Format(Constants.MailBodies.Traveling, 
                            DateTime.Now, 
                            ($"{assignment.Location.Ubicacion} {assignment.Location.Province} {assignment.Location.Department}"),
                            assignment.Description,
                            assignment.Machine.Model,
                            assignment.Machine.SerialNumber,
                            assignedTec0

                            );
                        break;
                    case ActivityType.Driving:
                        string assignedTec1 = null;
                        foreach (Contact item in assignment.TechnicalContacts)
                        {
                            assignedTec1 = assignedTec1 + $"<li> {item.Name} {item.LastName} </li>";
                        }
                        subject = string.Format(Constants.Mailsubject.Driving, workOrder.Code);
                        htmlBody = string.Format(Constants.MailBodies.Driving,
                            DateTime.Now,
                            ($"{assignment.Location.Ubicacion} {assignment.Location.Province} {assignment.Location.Department}"),
                            assignment.Description,
                            assignment.Machine.Model,
                            assignment.Machine.SerialNumber,
                            assignedTec1
                            );
                        break;
                    case ActivityType.FieldService:
                        subject = string.Format(Constants.Mailsubject.FieldService, workOrder.Code);
                        htmlBody = string.Format(Constants.MailBodies.FieldService,
                            DateTime.Now,
                            ($"{assignment.Location.Ubicacion} {assignment.Location.Province} {assignment.Location.Department}"),
                            assignment.Description,
                            assignment.Machine.Model,
                            assignment.Machine.SerialNumber
                            );
                        break;
                    case ActivityType.StandByClient:
                        subject = string.Format(Constants.Mailsubject.StandByClient, workOrder.Code);
                        htmlBody = string.Format(Constants.MailBodies.StandByClient,
                            DateTime.Now,
                            assignment.Description,
                            assignment.Machine.Model,
                            assignment.Machine.SerialNumber);
                        break;
                    case ActivityType.StandByFesa:
                        subject = string.Format(Constants.Mailsubject.StandByFesa, workOrder.Code);
                        htmlBody = string.Format(Constants.MailBodies.StandByFesa,
                            DateTime.Now,
                            assignment.Description,
                            assignment.Machine.Model,
                            assignment.Machine.SerialNumber);
                        break;
                    case ActivityType.FieldReport:
                        subject = string.Format(Constants.Mailsubject.FieldReport, workOrder.Code);
                        htmlBody = string.Format(Constants.MailBodies.FieldReport, 
                            assignment.Description, 
                            machine.Model, 
                            machine.SerialNumber, 
                            assignment.Location.Department, 
                            contact.Name, 
                            contact.Phone, 
                            contact.Email, 
                            supervisor.Name, 
                            supervisor.Phone, 
                            supervisor.Email);
                        break;
                    case ActivityType.TravelingReturn:
                        subject = string.Format(Constants.Mailsubject.TravelEnd, workOrder.Code);
                        htmlBody = string.Format(Constants.MailBodies.TravelEnd);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(activityType), activityType, null);
                }
                #endregion
                #endregion
            }
            catch (System.Exception)
            {

                throw;
            }
            
        }
    }
}
