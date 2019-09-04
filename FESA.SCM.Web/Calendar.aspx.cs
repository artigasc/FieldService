using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using FESA.SCM.Web.Logic;
using Microsoft.Ajax.Utilities;
using FESA.SCM.Web.Models;
using FESA.SCM.Web.DataAccess;
using System.Web;
using System.Web.Security;
using System.Drawing;

namespace FESA.SCM.Web
{
    public partial class Calendar : System.Web.UI.Page
    {
        private string userId_;
        private DateTime _fechaInicio;
        private DateTime _fechaFin;
        private int _office;
        private int _costCenter;

        protected void Page_Load(object sender, EventArgs e)
        {
            System.Web.HttpCookie authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie == null)
                return;

            var encTicket = authCookie.Value;
            var ticket = FormsAuthentication.Decrypt(encTicket);
            userId_ = ticket.UserData;

            if (!Page.IsPostBack)
            {
                var fesaDataSource = new FesaDataSourceDataContext();
                OfficeList.DataSource = fesaDataSource.GET_ALL_OFFICE();
                OfficeList.DataBind();
                CostCenterList.DataSource = fesaDataSource.GET_ALL_COSTCENTER();
                CostCenterList.DataBind();
            }
        }

        protected void ButtonClick(object sender, EventArgs e)
        {
                var Fecha = FechaFin;
                _fechaInicio = DateTime.Parse($"{FechaInicio.Text.ToString(CultureInfo.InvariantCulture)} 00:00:00.000");
                _fechaFin = DateTime.Parse($"{FechaFin.Text.ToString(CultureInfo.InvariantCulture)} 00:00:00.000");
                DateTime a = _fechaFin.AddDays(1);
                _office = Int32.Parse(OfficeList.SelectedItem.Value);
                _costCenter = Int32.Parse(CostCenterList.SelectedItem.Value);
                Scheduler.StartDate = new DateTime(_fechaInicio.Year, _fechaInicio.Month, _fechaInicio.Day);
                Scheduler.Days = (a - _fechaInicio).Days;
                Scheduler.DataSource = LoadActivities();
                Scheduler.DataResourceField = "resource_id";
                DataBind();
        }

        private DataTable LoadActivities()
        {
            var activities = CalendarProvider.GetItems(_office, _costCenter, _fechaInicio, _fechaFin);

            DataTable dt = new DataTable();

            List<dynamic> rows = new List<dynamic>();

            foreach (var item in activities)
            {
                rows.Add(item);
            }

            rows = rows.DistinctBy(o => o.USERNAME).ToList();

            // Añadir cabeceras de filas (Colaboradores)
            Scheduler.Resources.Clear();
            foreach (var item in rows)
            {
                Scheduler.Resources.Add(item.USERNAME, item.ID_USER);
            }

            // Añadir cabeceras de columnas
            dt.Columns.Add("Id", typeof(string));
            dt.Columns.Add("Nombre", typeof(string));
            dt.Columns.Add("StartDate", typeof(DateTime));
            dt.Columns.Add("EndDate", typeof(DateTime));
            dt.Columns.Add("Status", typeof(string));
            dt.Columns.Add("resource_id", typeof(string));

           
            // Desplegar Filas
            foreach (var activity in activities)
            {
                DateTime estimatedStartDate = DateTime.ParseExact(activity.ESTIMATEDSTARTDATE.ToString("dd/MM/yyyy HH:mm:ss"), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                DateTime estimatedEndDate = DateTime.ParseExact(activity.ESTIMATEDENDDATE.ToString("dd/MM/yyyy HH:mm:ss"), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                if (estimatedStartDate == estimatedEndDate)
                    estimatedEndDate = estimatedEndDate.AddDays(1);
                dt.Rows.Add(activity.ID_ACTIVITY,
                    ($"{activity.CODE}<br>{activity.NAME}<br>{(Enumerations.ActivityType) Enum.ToObject(typeof(Enumerations.ActivityType), activity.ACTIVITYTYPE)}"),
                    estimatedStartDate,
                    estimatedEndDate,
                    (Enumerations.ActivityType)Enum.ToObject(typeof(Enumerations.ActivityType), activity.ACTIVITYTYPE),
                    activity.ID_USER);
            }
            return dt;           
        }

        protected void Scheduler_BeforeEventRender(object sender, DayPilot.Web.Ui.Events.Scheduler.BeforeEventRenderEventArgs e)
        {
            string color = "";
            switch ((string)e.DataItem["Status"])
            {
                case "None":
                    color = "#000000";
                    break;
                case "PreparingTrip":
                    color = "#000000";
                    break;
                case "Traveling":
                    color = "#40bfff";
                    break;
                case "Driving":
                    color = "#ffb4a2";
                    break;
                case "FieldService":
                    color = "#3fdb59";
                    break;
                case "StandByClient":
                    color = "#40bfff";
                    break;
                case "FieldReport":
                    color = "#40bfff";
                    break;
                case "StandByFesa":
                    color = "#40bfff";
                    break;
                default:
                    break;
            }
            e.DurationBarColor = color;

        }

    }
}