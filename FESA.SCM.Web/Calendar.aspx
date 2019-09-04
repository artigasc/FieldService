<%@ Page Language="C#" MasterPageFile="~/navegation.Master" AutoEventWireup="true" CodeBehind="Calendar.aspx.cs" Inherits="FESA.SCM.Web.Calendar" %>

<%@ Register assembly="DayPilot" namespace="DayPilot.Web.Ui" tagprefix="DayPilot" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
       
        <table class="" style="width: 98%">          
             <tr>
                 <td colspan="8" style="height: 59px"><h3>Calendario</h3></td>
             </tr>
        </table>
        <table class="table table-bottom" style="width: 98%">
          
                <tr>
                     <td style="width: 79px" class="td-orden">
                         <asp:Label ID="Label2" runat="server" Text="Fecha - Inicio"></asp:Label>
                    </td>
                    <td style="width: 126px" >
                        <asp:TextBox ID="FechaInicio" runat="server" cssclass="datepicker-field form-control" width="170px"></asp:TextBox>
                    </td>
                     <td style="width: 79px" class="td-orden">
                         <asp:Label ID="Label3" runat="server" Text="Fecha - Fin"></asp:Label>
                    </td>
                    <td style="width: 144px" >
                        <asp:TextBox ID="FechaFin" runat="server" cssclass="datepicker-field form-control" width="170px" ></asp:TextBox>
                    </td>
                    <td style="width: 147px">
                        <asp:Label ID="Label5" runat="server" Text="Oficina:" ></asp:Label>
                        &nbsp;
                        <asp:DropDownList ID="OfficeList" runat="server" DataTextField="DESCRIPTION" DataValueField="ID" CssClass="btn-oficina">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 143px">
                        <asp:Label ID="Label6" runat="server" Text="Costos:" ></asp:Label>
                        &nbsp;<asp:DropDownList ID="CostCenterList" runat="server" DataTextField="DESCRIPTIONC" DataValueField="ID" CssClass="btn-oficina">
                        </asp:DropDownList>
                    &nbsp;</td>
                    <td style="width: 84px">
                         <asp:Button ID="Button1" runat="server" Text="Buscar" OnClick="ButtonClick" CssClass="btn btn-warning" />
                    </td>
                </tr>   
        </table>
              
        <br />  
        <DayPilot:DayPilotScheduler 
            runat="server" 
            ID="Scheduler"
            ClientIDMode="Static"
            DataStartField="StartDate" 
            DataEndField="EndDate"
            DataTextField="Nombre" 
            DataValueField="Id"
            Scale="Day"  
            CellWidth="120" 
            EventHeight="45"
            CssOnly="true"
            CssClassPrefix="dev"
            DurationBarVisible="true"
            OnBeforeEventRender="Scheduler_BeforeEventRender">
            <TimeHeaders>
                <DayPilot:TimeHeader GroupBy="Month" Format="MMMM yyyy"/>
                <DayPilot:TimeHeader GroupBy="Week" />
                <DayPilot:TimeHeader GroupBy="Day" />
            </TimeHeaders>
        </DayPilot:DayPilotScheduler>
           <br />
    <br />
     <table class="nav-justified">
            <tr>
                <td style="width: 299px">&nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>&nbsp;</td>
            </tr>
        </table>
      
</asp:Content>