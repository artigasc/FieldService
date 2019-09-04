<%@ Page Title="" Language="C#" MasterPageFile="~/navegation.Master" AutoEventWireup="true" CodeBehind="DetailsWorkOrder.aspx.cs" Inherits="FESA.SCM.Web.DetailsWorkOrder" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   
        <table class="nav-justified">
            <tr>
                <td colspan="3"><h3>Órdenes de Trabajo</h3><hr /></td>
            </tr>
           
            <tr>
                <td style="width: 105px"><asp:Label ID="Label1" runat="server" Text="Nro. OT:" Font-Bold="True"></asp:Label></td>
                <td>
                    <asp:Label ID="Code" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width: 105px">&nbsp;<asp:Label ID="Label3" runat="server" Text="Empresa:" Font-Bold="True"></asp:Label></td>
                <td>
                    <asp:Label ID="Empresa" runat="server" Text=""></asp:Label>
                </td>
            </tr>
        </table>

        <div class="tab-espacio">
               <ul class="nav nav-tabs">
                <li class="active"><a data-toggle="tab" href="#home">Datos del servicio</a></li>
                <li><a data-toggle="tab" href="#menu1">Lugar de la operación</a></li>
                <li><a data-toggle="tab" href="#menu2">Datos de la máquina</a></li>
                <li><a data-toggle="tab" href="#menu3">Contactos técnicos</a></li>
                <li><a data-toggle="tab" href="#menu4">Equipo Ferreyros</a></li>
              </ul>
              <div class="tab-content">
                <div id="home" class="tab-pane fade in active">
                  
                    <table class="nav-justified">
                        <tr>
                            <td style="width: 211px"><asp:Label  runat="server" Text="Detalle:" Font-Bold="True"></asp:Label></td>
                            <td><asp:Label ID="Description" runat="server" Text=""></asp:Label></td>
                        </tr>
                        <tr>
                            <td style="width: 211px"><asp:Label  runat="server" Text="Fecha de solicitud cliente:" Font-Bold="True"></asp:Label></td>
                            <td><asp:Label ID="FechSolicitud" runat="server" Text=""></asp:Label></td>
                        </tr>
                        <tr>
                            <td style="width: 211px"><asp:Label  runat="server" Text="Fecha prometida de inicio:" Font-Bold="True"></asp:Label></td>
                            <td><asp:Label ID="FechEstimada" runat="server" Text=""></asp:Label></td>
                        </tr>
                        <tr>
                            <td style="width: 211px"><asp:Label  runat="server" Text="Fecha final estimada:" Font-Bold="True"></asp:Label></td>
                            <td><asp:Label ID="FechFinEstimada" runat="server" Text=""></asp:Label></td>
                        </tr>
                        <tr><td style="width: 211px"></td></tr>
                        <tr><td style="width: 211px"></td></tr>
                        <tr>
                            <td colspan="2" class="text-right" ><asp:Button ID="BtnOrden" runat="server" class="btn btn-warning" Text="Regresar a la vista OTs"  Width="168px" Height="37px" BorderStyle="None" OnClick="BtnOrden_Click" /></td>

                        </tr>
                    </table>
                     
                </div>

                <div id="menu1" class="tab-pane fade">
                   <table class="nav-justified">
                        <tr>
                            <td style="width: 211px"><asp:Label  runat="server" Text="Ubicación:" Font-Bold="True"></asp:Label></td>
                            <td><asp:Label ID="District" runat="server" Text=""></asp:Label></td>
                        </tr>
                        <tr>
                            <td style="width: 211px"><asp:Label  runat="server" Text="Departamento:" Font-Bold="True"></asp:Label></td>
                            <td><asp:Label ID="Department" runat="server" Text=""></asp:Label></td>
                        </tr>
                        <tr>
                            <td style="width: 211px"><asp:Label  runat="server" Text="Provincia:" Font-Bold="True"></asp:Label></td>
                            <td><asp:Label ID="Province" runat="server" Text=""></asp:Label></td>
                        </tr>
                        
                        <tr><td></td></tr>
                        <tr><td></td></tr>
                       <tr>
                            <td colspan="2" class="text-right" ><asp:Button ID="BtnOrden1" runat="server" class="btn btn-warning" Text="Regresar a la vista OTs"  Width="168px" Height="37px" BorderStyle="None" OnClick="BtnOrden_Click" /></td>

                        </tr>
                    </table>


                   
                </div>
                <div id="menu2" class="tab-pane fade">
                  <table class="nav-justified">
                        <tr>
                            <td style="width: 211px"><asp:Label  runat="server" Text="Numero serial:" Font-Bold="True"></asp:Label></td>
                            <td><asp:Label ID="Serial" runat="server" Text=""></asp:Label></td>
                        </tr>
                        <tr>
                            <td style="width: 211px"><asp:Label  runat="server" Text="Marca:" Font-Bold="True"></asp:Label></td>
                            <td><asp:Label ID="Brand" runat="server" Text=""></asp:Label></td>
                        </tr>
                        <tr>
                            <td style="width: 211px"><asp:Label  runat="server" Text="Modelo:" Font-Bold="True"></asp:Label></td>
                            <td><asp:Label ID="Model" runat="server" Text=""></asp:Label></td>
                        </tr>
                        <tr>
                            <td style="width: 211px"><asp:Label  runat="server" Text="Hora de vida util:" Font-Bold="True"></asp:Label></td>
                            <td><asp:Label ID="LifeHours" runat="server" Text=""></asp:Label></td>
                        </tr>
                        <tr><td></td></tr>
                        <tr><td></td></tr>
                        <tr>
                            <td colspan="2" class="text-right" ><asp:Button ID="BtnOrden2" runat="server" class="btn btn-warning" Text="Regresar a la vista OTs"  Width="168px" Height="37px" BorderStyle="None" OnClick="BtnOrden_Click" /></td>

                        </tr>
                    </table>
                </div>
                <div id="menu3" class="tab-pane fade">

                     <asp:GridView ID="GVContact" CssClass="GVContact" runat="server" Width="1145px" AutoGenerateColumns="False" GridLines="None"   style="margin-left: 9px" >
                         <Columns>
                            <asp:BoundField DataField="NameContact" HeaderText="Nombre del Equipo"  >
                            </asp:BoundField>
                             <asp:BoundField DataField="EmailContact" HeaderText="Email del Equipo"  >
                            </asp:BoundField>
                            <asp:BoundField DataField="PhoneContact" HeaderText="Teléfono del Equipo">
                            </asp:BoundField>                                    
                        </Columns>
                    </asp:GridView>
                     <asp:Button ID="Button1" runat="server" class="btn btn-warning btn_orden"  Text="Regresar a la vista OTs"  Width="168px" Height="37px" BorderStyle="None" OnClick="BtnOrden_Click" />

                      
                </div>
                <div id="menu4" class="tab-pane fade">
                  
                    <asp:GridView ID="GVDetails" CssClass="GVDetails" runat="server" Width="1145px" AutoGenerateColumns="False" GridLines="None"   style="margin-left: 9px" >
                     <Columns>
                        <asp:BoundField DataField="NamePersonnel" HeaderText="Nombre del Equipo" ItemStyle-Width="100"  >
                        <ItemStyle Width="100px"></ItemStyle>
                        </asp:BoundField>
                         <asp:BoundField DataField="EmailPersonnel" HeaderText="Email del Equipo" ItemStyle-Width="100" >
                        <ItemStyle Width="100px"></ItemStyle>
                        </asp:BoundField>
                       <asp:BoundField DataField="PhonePersonnel" HeaderText="Teléfono del Equipo" ItemStyle-Width="100">
                        <ItemStyle Width="100px"></ItemStyle>
                        </asp:BoundField>                                    
                    </Columns>
                 </asp:GridView>
                      <asp:Button ID="Button2" runat="server" class="btn btn-warning btn_orden" Text="Regresar a la vista OTs"  Width="168px" Height="37px" BorderStyle="None" OnClick="BtnOrden_Click" />

                </div>
   
              </div>
        </div>
       
    

</asp:Content>
