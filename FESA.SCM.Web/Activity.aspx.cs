using FESA.SCM.Web.DataAccess;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using static FESA.SCM.Web.Models.Enumerations;

namespace FESA.SCM.Web {
    public partial class Activity: System.Web.UI.Page {
        private string userId_;
        private string PERSONNELID;
        private string ASSIGNMENTID;
        protected void Page_Load(object sender, EventArgs e) {
            if (!Page.IsPostBack) {
                System.Web.HttpCookie authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
                if (authCookie == null)
                    return;

                var encTicket = authCookie.Value;
                var ticket = FormsAuthentication.Decrypt(encTicket);
                userId_ = ticket.UserData;

                if (Request.Params["PERSONNELID"] != null) {
                    ASSIGNMENTID = Request.Params["ASSIGNMENTID"];
                    PERSONNELID = Request.Params["PERSONNELID"];

                    ListarActividad(ASSIGNMENTID, PERSONNELID);
                    cbx_informe.Enabled = false;
                    cbx_actaC.Enabled = false;
                    cbx_sims.Enabled = false;
                    cbx_documentS.Enabled = false;
                    cbx_guia.Enabled = false;
                    cbx_checCa.Enabled = false;
                    CheckedDocument();
                }

            }
        }

        public void CheckedDocument() {
            using (var connection = new FesaDataSourceDataContext()) {
                var connec = connection.GET_DOCUMENT_BY_NAME_SP(ASSIGNMENTID).ToList();

                foreach (var u in connec) {
                    string Name = u.NAME;
                    for (int i = 0; i < 7; i++) {
                        switch (Name) {
                            case "INFORME TÉCNICO":

                                cbx_informe.Checked = true;
                                break;
                            case "DETALLE DE GASTO":
                                cbx_detalleG.Enabled = false;
                                cbx_detalleG.Checked = true;
                                break;
                            case "ACTA DE CONFORMIDAD":

                                cbx_actaC.Checked = true;
                                break;
                            case "SIMS":

                                cbx_sims.Checked = true;
                                break;
                            case "DOCUMENTO DE SEGURIDAD":

                                cbx_documentS.Checked = true;
                                break;
                            case "GUÍA DE REPUESTOS FIRMADA POR CLIENTE":

                                cbx_guia.Checked = true;
                                break;
                            case "CHECKING DE CAMIONETA":

                                cbx_checCa.Checked = true;
                                break;
                        }
                    }
                }
            }
        }

        public void ListarActividad(string ASSIGNMENTID, string PERSONNELID) {

            using (var connection = new FesaDataSourceDataContext()) {
                var customer = connection.GET_ACTIVITY_BY_PERSONNELID_SP(ASSIGNMENTID, PERSONNELID).ToList();
                foreach (var s in customer) {

                    var x = s.ACTIVITYID;
                    int? userType = s.ISLEAD;

                    string ISLEADDescription = string.Empty;
                    if (userType != null) {
                        switch (userType) {
                            case (int)ISLEAD.Technician:
                                ISLEADDescription = "(Técnico)";
                                break;
                            case (int)ISLEAD.Leader:
                                ISLEADDescription = "(Lider)";
                                break;
                            case (int)ISLEAD.Supervisor:
                                ISLEADDescription = "(Supervisor)";
                                break;
                        }
                        Label4.Text = s.CODE;
                        Label5.Text = s.NAMECLIENT;
                        Label6.Text = s.NAMEPERSONNEL;
                        Label7.Text = ISLEADDescription;
                    }
                }
                GVACTIVITY.DataSource = customer;
                GVACTIVITY.DataBind();
            }
        }

        protected void GVACTIVITY_RowDataBound(object sender, GridViewRowEventArgs e) {
            GridView tg;
            GridView tg1;
            if (e.Row.RowType == DataControlRowType.DataRow) {
                tg = e.Row.FindControl("GVLO") as GridView;
                tg1 = e.Row.FindControl("GVLOINI") as GridView;
                List<Actividad> list = new List<Actividad>();
                using (var connection = new FesaDataSourceDataContext()) {

                    var row = e.Row.DataItem;
                    System.Reflection.PropertyInfo pi = row.GetType().GetProperty("IDACTIVITY");
                    String activityId = (String)(pi.GetValue(row, null));
                    if (row != null) {
                        var personnel = connection.GET_ACTIVITY_BY_PERSONNELID_ACTIVITYSTATE_END_SP(activityId).ToList();

                        foreach (var item in personnel) {
                            //ANTONIO
                            //Agrego la validación de ubicacion
                            var activity = new Actividad();
                            activity.UBICATIONINI = item.UBICATIONINI?.ToString();
                            activity.UBICATIONEND = item.UBICATIONEND?.ToString();
                            if (string.IsNullOrEmpty(activity.UBICATIONINI) || string.IsNullOrWhiteSpace(activity.UBICATIONINI)) {
                                activity.UBICATIONINI = item.UBICATIONEND?.ToString();
                            }
                            list.Add(activity);
                            break;
                        }
                        tg.DataSource = list.ToList();
                        tg.DataBind();

                        tg1.DataSource = list.ToList();
                        tg1.DataBind();

                        if (tg.HeaderRow != null)
                            tg.HeaderRow.Visible = false;
                        if (tg1.HeaderRow != null)
                            tg1.HeaderRow.Visible = false;
                    }
                }
            }
        }

        private class Actividad {
            public string UBICATION { get; set; }
            public string UBICATIONINI { get; set; }
            public string UBICATIONEND { get; set; }
            public string ACTIVITYID { get; set; }
        }

        protected void HyplUbicationEnd_OnClick(object sender, EventArgs e) {
            LinkButton btn = sender as LinkButton;
            string geolocation = btn.Text;
            string url = "http://maps.google.com/?q=" + geolocation;
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "onclick", "javascript: window.open('" + url + "', '_blank', 'width=700,height=600,position=fixed,top= 200,right= 200,bottom= 200,left= 200');", true);
        }

        protected void HyplUbication_OnClick(object sender, EventArgs e) {
            LinkButton btn = sender as LinkButton;
            string geolocation = btn.Text;
            string url = "http://maps.google.com/?q=" + geolocation;
            //btn.Attributes.Add("onclick", "javascript: window.open('" + url + "', '_blank', 'width=700,height=600,position=fixed,top= 200,right= 200,bottom= 200,left= 200');");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "onclick", "javascript: window.open('" + url + "', '_blank', 'width=700,height=600,position=fixed,top= 200,right= 200,bottom= 200,left= 200');", true);
            // upModal.Update();
        }
    }
}