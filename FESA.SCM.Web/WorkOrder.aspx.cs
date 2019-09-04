using System;
using System.Collections.Generic;
using System.Web.Security;
using System.Web;
using FESA.SCM.Web.DataAccess;
using System.Linq;
using System.Web.UI.WebControls;
using static FESA.SCM.Web.Models.Enumerations;
using FESA.SCM.Web.Models;

namespace FESA.SCM.Web {
    public partial class WorkOrder: System.Web.UI.Page {
        private string _userId;

        protected void Page_Load(object sender, EventArgs e) {
            System.Web.HttpCookie authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie == null)
                return;

            var encTicket = authCookie.Value;
            var ticket = FormsAuthentication.Decrypt(encTicket);
            if (ticket != null)
                _userId = ticket.UserData;


            if (Page.IsPostBack)
                return;
            ListarOrdenPaginadas();

            var fesaDataSource = new FesaDataSourceDataContext();
            OfficeList.DataSource = fesaDataSource.GET_ALL_OFFICE();
            OfficeList.DataBind();
            CostCenterList.DataSource = fesaDataSource.GET_ALL_COSTCENTER();
            CostCenterList.DataBind();
        }
        protected void Page_Changed(object sender, EventArgs e) {

            int pageIndex = int.Parse((sender as LinkButton).CommandArgument);
            if (TxtBuscar.Text == "" && OfficeList.SelectedItem.Text == "Seleccione" && CostCenterList.SelectedItem.Text == "Seleccione") {
                this.ListarOrdenPaginadas(pageIndex);
            } else if (TxtBuscar.Text != "" && OfficeList.SelectedItem.Text == "Seleccione" && CostCenterList.SelectedItem.Text == "Seleccione") {
                this.ListarTodaOrdenes(pageIndex);
            }

            if (OfficeList.SelectedItem.Text != "Seleccione" && CostCenterList.Text != "Seleccione") {
                this.ListarTodaOrdenes(pageIndex);
            }

        }
        protected void GVOrder_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e) {
            GVOrder.PageIndex = e.NewPageIndex;
            this.DataBind();
        }

        protected void BtnBuscar_Click(object sender, EventArgs e) {
            ListarTodaOrdenes();
        }

        protected void BtnBuscartodo_Click(object sender, EventArgs e) {
            TxtBuscar.Text = "";
            CostCenterList.SelectedIndex = -1;
            OfficeList.SelectedIndex = -1;
            ListarOrdenPaginadas();
        }

        public void ListarOrdenPaginadas(int currentPage = 1) {
            using (var connection = new FesaDataSourceDataContext()) {

                GVOrder.DataSource = null;
                GVOrder.DataBind();
                int pageSize = 10;
                int? totalRowCount = 0;
                //ANTONIO
                List<ORDERTEMP> orders = connection.SP_GET_ALL_ORDER_PAGINATE_P(currentPage, pageSize, ref totalRowCount).ToList();
                //List<OrderNew> Orders = connection.SP_GET_ORDER_PAGINATE(TxtBuscar.Text,CostCenterList.SelectedIndex,OfficeList.SelectedIndex, currentPage, pageSize, ref _TotalRowCount).ToList();
                totalRowCount = Convert.ToInt32(totalRowCount.Value);
                GVOrder.DataSource = orders;
                GVOrder.DataBind();
                //GeneratePager(totalRowCount, pageSize, currentPage);
                this.PopulatePager(totalRowCount, currentPage);
            }
        }

        private void PopulatePagers(int? totalRowCount, int currentPage) {


            int pageSize = 10;
            double dblPageCount = (double)((decimal)totalRowCount / Convert.ToDecimal(pageSize));
            int pageCount = (int)Math.Ceiling(dblPageCount);
            List<ListItem> pages = new List<ListItem>();
            if (pageCount > 0) {
                pages.Add(new ListItem("Primer Registro", "1", currentPage > 1));
                int pagesToShow = 4;
                int minPage = Math.Max(1, currentPage - (pagesToShow / 2));
                int maxPage = Math.Min(pageCount, minPage + pagesToShow);
                if (minPage > 1)
                    pages.Add(new ListItem("...", (minPage - 1).ToString(), false));
                for (int i = minPage; i <= maxPage; i++) {
                    pages.Add(new ListItem(i.ToString(), i.ToString(), i != currentPage));
                }
                if (maxPage < pageCount)
                    pages.Add(new ListItem("...", (maxPage + 1).ToString(), false));
                pages.Add(new ListItem("Siguiente", (currentPage + 1).ToString(), currentPage < pageCount));
            }
            rptPager.DataSource = pages;
            rptPager.DataBind();

        }


        private void PopulatePager(int? totalRowCount, int currentPage) {

            int pageSize = 10;
            double dblPageCount = (double)((decimal)totalRowCount / Convert.ToDecimal(pageSize));
            int pageCount = (int)Math.Ceiling(dblPageCount);
            List<ListItem> pages = new List<ListItem>();
            if (pageCount > 0) {
                pages.Add(new ListItem("Primer Registro", "1", currentPage > 1));
                int pagesToShow = 4;
                int minPage = Math.Max(1, currentPage - (pagesToShow / 2));
                int maxPage = Math.Min(pageCount, minPage + pagesToShow);
                if (minPage > 1)
                    pages.Add(new ListItem("...", (minPage - 1).ToString(), false));
                for (int i = minPage; i <= maxPage; i++) {
                    pages.Add(new ListItem(i.ToString(), i.ToString(), i != currentPage));
                }
                if (maxPage < pageCount)
                    pages.Add(new ListItem("...", (maxPage + 1).ToString(), false));
                pages.Add(new ListItem("Siguiente", (currentPage + 1).ToString(), currentPage < pageCount));
            }
            rptPager.DataSource = pages;
            rptPager.DataBind();



        }
        public void ListarTodaOrdenes(int currentPage = 1) {
            using (var connection = new FesaDataSourceDataContext()) {
                GVOrder.DataSource = null;
                GVOrder.DataBind();
                int pageSize = 10;
                int? totalRowCount = 0;
                //ANTONIO
                List<ORDERTEMP> orders = connection.SP_GET_ORDER_PAGINATE(TxtBuscar.Text, Convert.ToInt32(CostCenterList.SelectedValue), Convert.ToInt32(OfficeList.SelectedValue), currentPage, pageSize, ref totalRowCount).ToList();

                totalRowCount = Convert.ToInt32(totalRowCount);

                GVOrder.DataSource = orders;
                GVOrder.DataBind();
                //GeneratePager(totalRowCount, pageSize, currentPage);
                PopulatePagers(totalRowCount, currentPage);

            }
        }

        ///*FILTRADO*/
        //public void GeneratePager(int? totalRowCount, int pageSize, int currentPage)
        //{


        //    int totalPageCount = (int)Math.Ceiling((decimal)totalRowCount / pageSize);
        //    int totalLinkInPage = (int)Math.Ceiling((decimal)totalRowCount / 10) / 5;
        //    int startPageLink = Math.Max(currentPage - (int)Math.Floor((decimal)totalLinkInPage / 2), 1);
        //    int lastPageLink = Math.Min(startPageLink + totalLinkInPage - 1, totalPageCount);

        //    if ((startPageLink + totalLinkInPage - 1) > totalPageCount)
        //    {
        //        lastPageLink = Math.Min(currentPage + (int)Math.Floor((decimal)totalLinkInPage / 2), totalPageCount);
        //        startPageLink = Math.Max(lastPageLink - totalLinkInPage + 1, 1);
        //    }

        //    List<ListItem> pageLinkContainer = new List<ListItem>();

        //    if (startPageLink != 1)
        //        pageLinkContainer.Add(new ListItem("First", "1", currentPage != 1));
        //    for (int i = startPageLink; i <= lastPageLink; i++)
        //    {
        //        pageLinkContainer.Add(new ListItem(i.ToString(), i.ToString(), currentPage != i));
        //    }
        //    if (lastPageLink != totalPageCount)
        //        pageLinkContainer.Add(new ListItem("Last", totalPageCount.ToString(), currentPage != totalPageCount));

        //    dlPager.DataSource = pageLinkContainer;
        //    dlPager.DataBind();

        //}

        //protected void dlPager_ItemCommand(object source, DataListCommandEventArgs e)
        //{
        //    if (e.CommandName.Equals("PageNo"))
        //    {
        //        ListarOrdenPaginadas(Convert.ToInt32(e.CommandArgument));
        //    }
        //}
        /*FIN FILTRADO*/

        protected void GVOrder_RowDataBound(object sender, GridViewRowEventArgs e) {
            GridView tg;
            if (e.Row.RowType == DataControlRowType.DataRow) {
                tg = e.Row.FindControl("TecniGV") as GridView;
                List<Tecnico> list = new List<Tecnico>();
                using (var connection = new FesaDataSourceDataContext()) {

                    var row = e.Row.DataItem;
                    System.Reflection.PropertyInfo pi = row.GetType().GetProperty("ASSIGNMENTID");
                    String assignmentId = (String)(pi.GetValue(row, null));


                    if (row != null) {
                        var personnel = connection.GET_PERSONNEL_BY_ASSIGNMENT_SP(assignmentId).ToList();

                        foreach (var item in personnel) {
                            int? userType = item.USERTYPE;
                            int? userStatus = item.USERSTATUS;

                            if (userType != null && userStatus != null) {
                                string userTypeDescription;
                                switch (userType) {
                                    case (int)UserType.Supervisor:
                                        userTypeDescription = "Supervisor";
                                        break;
                                    case (int)UserType.Technician:
                                        userTypeDescription = "Técnico";
                                        break;
                                    default:
                                        userTypeDescription = "Error";
                                        break;
                                }
                                string userStatusDescription = "";

                                if (userType == 1) {                                    
                                } else {

                                    //ASSIGMENT X USER (TABLA PERSONNEL)
                                    //switch (userStatus) {
                                    //    case (int)Models.AssignmentStatus.Hold:
                                    //        userStatusDescription = "Asignado";
                                    //        break;
                                    //    case (int)Models.AssignmentStatus.Active:
                                    //        userStatusDescription = "En Proceso";
                                    //        break;
                                    //    case (int)Models.AssignmentStatus.Complete:
                                    //        userStatusDescription = "Finalizado";
                                    //        break;
                                    //    default:
                                    //        userStatusDescription = "Error";
                                    //        break;
                                    //}

                                    switch (userStatus) {
                                        case (int)UserStatus.Assigned:
                                            userStatusDescription = "Asignado";
                                            break;
                                        case (int)UserStatus.Available:
                                            userStatusDescription = "Disponible";
                                            break;
                                        case (int)UserStatus.OnWork:
                                            userStatusDescription = "En proceso";
                                            break;
										case 3:
											userStatusDescription = "Completado";
											break;
										default:
                                            userStatusDescription = "Error";
                                            break;
                                    }
                                }
                                list.Add(new Tecnico {
                                    Name = item.NAMEPERSONNEL,
                                    Usertype = Convert.ToInt32(item.USERTYPE),
                                    Personnelid = item.PERSONNELID,
                                    Usertypedescription = userTypeDescription,
                                    Userstatusdescription = userStatusDescription,
                                    Assignmentid = assignmentId,
                                    DescriptionIcon = userTypeDescription == "Técnico" ? ResolveUrl("~/Images/Tecnico.png") : ResolveUrl("~/Images/Supervisor.png")
                                });
                            }


                        }
                        if (tg != null) {
                            tg.DataSource = list.ToList();
                            tg.DataBind();
                            if (tg.HeaderRow != null)
                                tg.HeaderRow.Visible = false;
                        }
                    }

                }
                tg = e.Row.FindControl("TecniStatus") as GridView;
                if (tg != null) {
                    tg.DataSource = list.ToList();
                    tg.DataBind();
                    if (tg.HeaderRow != null)
                        tg.HeaderRow.Visible = false;
                }
                list.Clear();
            }
        }

        private class Tecnico {
            public string Name { get; set; }
            public int Usertype { get; set; }
            public string Personnelid { get; set; }
            public string Usertypedescription { get; set; }
            public string Userstatusdescription { get; set; }
            public string Assignmentid { get; set; }
            public string DescriptionIcon { get; set; }
        }

        protected void TecniGV_RowDataBound(object sender, GridViewRowEventArgs e) {

        }
    }
}