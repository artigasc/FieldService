<%@ Page Language="C#" MasterPageFile="~/navegation.Master" AutoEventWireup ="true" CodeBehind="WorkOrder.aspx.cs" Inherits="FESA.SCM.Web.WorkOrder" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <table class="" style="width: 96%">
        <tr>

            <td colspan="8" style="height: 59px"><h3>Órdenes de Trabajo</h3> </td>
               
              <%--  <hr style="height: -42px" /></td>--%>
        </tr>
    </table>

     <table class="table table-bottom" style="width: 100%; margin-left: 11px;">
        
        <tr>
            <td style="width: 1076px"  class="td-orden">
                <asp:Label ID="Label1" for="kwd_search" runat="server" Text="Orden de trabajo:"  Font-Overline="False"></asp:Label>
            </td>
            <td style="width: 166px">
                <%--<input id="kwd_search" type="text" style="width: 235px" /></td>--%>
                <asp:TextBox ID="TxtBuscar" runat="server" Width="126px" CssClass="form-control" Height="29px" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
            </td>
            <td style="width: 958px" class="td-orden">
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Label ID="Label3" for="kwd_search" runat="server" Text="Oficina:"></asp:Label>
            </td>

            <td style="width: 448px">
                 <asp:DropDownList ID="OfficeList" runat="server" DataTextField="DESCRIPTION" DataValueField="ID" AppendDataBoundItems="true" Height="31px" Width="119px" CssClass="btn-oficina">
                      <asp:ListItem Value="0" Text="Seleccione"></asp:ListItem>

                 </asp:DropDownList>
            </td>
            <td style="width: 1048px" class="td-orden">
                <asp:Label ID="Label2" for="kwd_search" runat="server" Text="Centro de costo:"></asp:Label>
            </td>
            <td style="width: 561px">
                <asp:DropDownList ID="CostCenterList" runat="server" DataTextField="DESCRIPTIONC" DataValueField="ID" AppendDataBoundItems="true" Height="31px" Width="129px" CssClass="btn-oficina">
                    <asp:ListItem Value="0" Text="Seleccione"></asp:ListItem>

                </asp:DropDownList>
            </td>
            
            <td style="width: 222px"> <asp:Button ID="BtnBuscar" runat="server" Text="Buscar"  OnClick="BtnBuscar_Click" CssClass="btn btn-warning"/></td>
            <td style="width: 119px">  
                <asp:Button ID="Button1" runat="server" Text="Ver todas las órdenes"  OnClick="BtnBuscartodo_Click" CssClass="btn btn-warning" Width="169px"/> 
            </td>
        </tr> 
    </table>
    
    <asp:GridView ID="GVOrder" CssClass="GVOrder" runat="server"  AutoGenerateColumns="False"  OnPageIndexChanging="GVOrder_PageIndexChanging" OnRowDataBound="GVOrder_RowDataBound" style="margin-left: 9px">
        <HeaderStyle CssClass="Cabecera" Wrap="True" />
         
         <Columns>
             <asp:HyperLinkField
                 DataNavigateUrlFields="ORDERID"
                 DataNavigateUrlFormatString="DetailsWorkOrder.aspx?ORDERID={0}"
                 DataTextField="NUMBERORDER"
                 HeaderText="Nro. OT" ItemStyle-HorizontalAlign="Center"> 
                <ControlStyle CssClass="buscar" />
                </asp:HyperLinkField>
                <asp:BoundField DataField="NAME" HeaderText="Empresa" ItemStyle-Width="150" ItemStyle-HorizontalAlign="Center">
                <ItemStyle Width="150px"></ItemStyle>
                </asp:BoundField>
                <asp:TemplateField HeaderText="Máquina (Marca-Modelo-N/S)" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <%#Eval("BRAND") + " " + "/" + " " %><%#Eval("MODEL") + " " + "/" + " " %><%#Eval("SERIALNUMBER") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="ESTIMATEDSTARTDATE" HeaderText="Fecha Inicio" ItemStyle-Width="100" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:dd/M/yyyy}" >
                <ItemStyle Width="100px"></ItemStyle>
                </asp:BoundField>
                 <asp:BoundField DataField="ESTIMATEDENDDATE" HeaderText="Fecha Fin" ItemStyle-Width="100" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:dd/M/yyyy}" >
                <ItemStyle Width="100px"></ItemStyle>
                </asp:BoundField>
               

                 <asp:TemplateField HeaderText="Técnicos asignados">
                     <ItemTemplate>
                         <asp:GridView id="TecniGV" CssClass="TecniGV" runat="server" GridLines="None" AutoGenerateColumns="false" >
                             <Columns>
                                 <asp:ImageField DataImageUrlField="DescriptionIcon" ControlStyle-Width="16" ControlStyle-Height = "16" ItemStyle-VerticalAlign="Middle">
                                 </asp:ImageField>
                                 <%--<asp:BoundField DataField="USERTYPEDESCRIPTION" ItemStyle-Width="100" >                               
                                <ItemStyle Width="100px"></ItemStyle>
                                </asp:BoundField>--%>
                                 <asp:HyperLinkField
                                 DataNavigateUrlFields="PERSONNELID, ASSIGNMENTID"
                                  DataNavigateUrlFormatString="Activity.aspx?PERSONNELID={0}&ASSIGNMENTID={1}" 
                                 DataTextField="Name"
                                 HeaderText="Técnicos asignados" 
                                 NavigateUrl="~/Activity.aspx"/>
                             </Columns>
                         </asp:GridView>
                     </ItemTemplate>
                     <ControlStyle Width="350px" />
                 </asp:TemplateField>
              
                <asp:TemplateField HeaderText="Estado del servicio" ControlStyle-Width="150px">
                     <ItemTemplate>
                         <asp:GridView id="TecniStatus" runat="server" GridLines="None" AutoGenerateColumns="false" Width="150">
                             <Columns>
                                 <asp:BoundField DataField="USERSTATUSDESCRIPTION" ItemStyle-Width="150px" ItemStyle-HorizontalAlign="Center"/>
                             </Columns>
                         </asp:GridView>
                     </ItemTemplate>
                 </asp:TemplateField>

            </Columns>
    </asp:GridView>
   
    
   
    <%--<asp:DataList CellPadding="5" RepeatDirection="Horizontal" runat="server" ID="dlPager"
        onitemcommand="dlPager_ItemCommand" style="margin-left: 6px">
        <ItemTemplate>
           <asp:LinkButton Enabled='<%#Eval("Enabled") %>' runat="server" ID="lnkPageNo" Text='<%#Eval("Text") %>' CommandArgument='<%#Eval("Value") %>' CommandName="PageNo"></asp:LinkButton>
        </ItemTemplate>
    </asp:DataList> --%>
    <asp:Repeater ID="rptPager" runat="server">
    <ItemTemplate>
        <asp:LinkButton Enabled='<%#Eval("Enabled") %>' ID="lnkPage" runat="server" Text='<%#Eval("Text") %>' CommandArgument='<%# Eval("Value") %>'
            CssClass='<%# Convert.ToBoolean(Eval("Enabled")) ? "page_enabled" : "page_disabled" %>'
            OnClick="Page_Changed" OnClientClick='<%# !Convert.ToBoolean(Eval("Enabled")) ? "return false;" : "" %>'></asp:LinkButton>
   </ItemTemplate>
</asp:Repeater>
    </asp:Content>
