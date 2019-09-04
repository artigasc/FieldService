using FESA.SCM.Web.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FESA.SCM.Web.DataAccess;
using System.Web.Security;
using System.Data.SqlClient;
using System.Globalization;
using static FESA.SCM.Web.Models.Enumerations;

namespace FESA.SCM.Web
{
    public partial class DetailsWorkOrder : System.Web.UI.Page
    {
        
        private  string userId_;
   
        protected void Page_Load(object sender, EventArgs e)
        {
            System.Web.HttpCookie authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie == null)
                return;

            var encTicket = authCookie.Value;
            var ticket = FormsAuthentication.Decrypt(encTicket);
            userId_ = ticket.UserData;
            var orderid = Request.Params["ORDERID"];
            if (orderid != null)
            {              
                ListaDetallesOrden();
                ListarEquipo();
                ListaContacto();
             


            }
        }

        public void ListaDetallesOrden()
        {
           
            var id = Request.Params["ORDERID"];
            using (var connection = new FesaDataSourceDataContext())
            {
                var customer = connection.GET_ASSIGNMENTDETAILS_SP(id).ToList();
                var contacto = connection.GET_CONTACTDETAILS_SP(id).ToList();
                var user = connection.GET_PERSONNELDETAILS_SP(id).ToList();

             
                foreach (var c in customer)
                {
                    Code.Text = c.CODE;
                    Empresa.Text = c.NAME;
                    Description.Text = c.DESCRIPTION;
                    FechSolicitud.Text = c.REQUESTDATE.Value.ToString("dd/MM/yyyy");
                    FechEstimada.Text = c.ESTIMATEDSTARTDATE.Value.ToString("dd/MM/yyyy");
                    FechFinEstimada.Text = c.ESTIMATEDENDDATE.Value.ToString("dd/MM/yyyy");
                    District.Text = c.DISTRICT;
                    Department.Text = c.DEPARTMENT;
                    Province.Text = c.PROVINCE;
                    Serial.Text = c.SERIALNUMBER;
                    Brand.Text = c.BRAND;
                    Model.Text = c.MODEL;
                    LifeHours.Text = Convert.ToString(c.LIFEHOURS);
                }
               
            }

        }

        public void ListarEquipo()
        {
            using (var connection = new FesaDataSourceDataContext())
            {
                var id = Request.Params["ORDERID"];
                List<Equipo> list = new List<Equipo>();
                var connec = connection.GET_PERSONNELDETAILS_SP(id).ToList();

                foreach (var u in connec)
                {
                    list.Add(new Equipo
                    {
                       NamePersonnel = u.NAME,
                       EmailPersonnel = u.EMAIL,
                       PhonePersonnel = u.PHONE
                    });
                }
                GVDetails.DataSource = list.ToList();
                GVDetails.DataBind();
              

            }
        }

        public void ListaContacto()
        {
            using (var connection = new FesaDataSourceDataContext())
            {
                var id = Request.Params["ORDERID"];
                List<Contacto> list = new List<Contacto>();
                var connec = connection.GET_CONTACTDETAILS_SP(id).ToList();

                foreach (var u in connec)
                {
                    list.Add(new Contacto
                    {
                        NameContact = u.NAME,
                        EmailContact = u.EMAIL,
                        PhoneContact = u.PHONE
                    });
                }
                GVContact.DataSource = list.ToList();
                GVContact.DataBind();
                
            }
        }
        
        protected void BtnOrden_Click(object sender, EventArgs e)
        {
           Response.Redirect("WorkOrder.aspx"); 
        }

        private class Equipo 
        {
            public string NamePersonnel { get; set; }
            public string EmailPersonnel { get; set; }
            public string PhonePersonnel { get; set; }
           
        }
        private class Contacto
        {
            public string NameContact { get; set; }
            public string EmailContact { get; set; }
            public string PhoneContact { get; set; }
        }

    }
}