<%@ Page Title="" Language="C#" MasterPageFile="~/navegation.Master" AutoEventWireup="true" CodeBehind="Activity.aspx.cs" Inherits="FESA.SCM.Web.Activity" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     
     <table class="nav-justified" style="width: 96%">
       
        <tr>
            <td colspan="8" style="height: 59px"><h3>Órdenes de Trabajo</h3><hr /></td>
        </tr>
        <tr>
            <td style="width: 161px" >
                Nro. OT:</td>
            <td style="width: 286px">
                <asp:Label ID="Label4" runat="server" Text=""></asp:Label>
           </td>
            <td></td>
        </tr>
         <tr>
            <td style="width: 161px" > 
                Empresa:</td>
            <td style="width: 286px">
                <asp:Label ID="Label5" runat="server" Text=""></asp:Label>
           </td>
             <td></td>
        </tr>
         <tr>
            <td style="width: 161px" >
                Técnico asignado:</td>
            <td style="width: 327px">
                <asp:Label ID="Label6" runat="server" Text=""></asp:Label> &nbsp; <asp:Label ID="Label7" runat="server" Text=""></asp:Label>
            </td>
            <td></td>
        </tr>
         
    </table>
    <asp:GridView ID="GVACTIVITY" CssClass="GVACTIVITY" runat="server" Width="1149px" AutoGenerateColumns="False" style="margin-right: 1px" OnRowDataBound="GVACTIVITY_RowDataBound"  >
        <Columns>
            <asp:BoundField DataField="ACTIVITYID" HeaderText="Actividad"/>
            <asp:BoundField DataField="DESCRIPTION" HeaderText="Descripción de actividad"/>            
            <asp:TemplateField HeaderText="Geolocalización Inicio" ItemStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:GridView id="GVLOINI" runat="server" GridLines="None" AutoGenerateColumns="false" >
                             <Columns>
                                <asp:TemplateField HeaderText="Geolocalización Inicio">                                    
                                    <ItemTemplate>                                        
                                        <asp:LinkButton runat="server"
                                            ID="HyplUbication"
                                            OnClick="HyplUbication_OnClick"                                                                              
                                            Text='<%# Eval("UBICATIONINI")%>' CssClass="geo"></asp:LinkButton>
                                    </ItemTemplate>                                       
                                </asp:TemplateField>
                             </Columns>
                         </asp:GridView>
                           
                            
                        <%--<asp:LinkButton runat="server"
                                            ID="HyplUbication" 
                                            OnClick="HyplUbication_OnClick"                                      
                                            Text='<%# Eval("UBICATION")%>' CssClass="geo"></asp:LinkButton>--%>
                    </ItemTemplate>
                </asp:TemplateField>

            <asp:BoundField DataField="STARTDATE" HeaderText="Fecha Inicio" DataFormatString="{0:dd/M/yyyy}"/>
            <asp:BoundField DataField="STARTDATE" HeaderText="Hora Inicio" DataFormatString="{0:t}"/>            
            
             <asp:TemplateField HeaderText="Geolocalización Fin">
                     <ItemTemplate>
                         <asp:GridView id="GVLO" runat="server" GridLines="None" AutoGenerateColumns="false" >

                             <Columns>

                                <asp:TemplateField HeaderText="Geolocalización Fin">
                                    
                                    <ItemTemplate>
                                        
                                        <asp:LinkButton runat="server"
                                            ID="HyplUbicationEnd"
                                            OnClick="HyplUbicationEnd_OnClick"
                                                                              
                                            Text='<%# Eval("UBICATIONEND")%>'></asp:LinkButton>
                                             
                                    </ItemTemplate>
                                       
                                </asp:TemplateField>
                             </Columns>
                         </asp:GridView>
                     </ItemTemplate>
                 </asp:TemplateField>
      
            <asp:BoundField DataField="ENDDATE" HeaderText="Fecha Fin" DataFormatString="{0:dd/M/yyyy}"/>
            <asp:BoundField DataField="ENDDATE" HeaderText="Hora Fin" DataFormatString="{0:t}"/>
            <asp:BoundField DataField="DURATIONSTR" HeaderText="Duración total"/>
            <asp:BoundField DataField="ESTADO" HeaderText="Estado"/>
        </Columns>
        <HeaderStyle CssClass="Cabecera" />
    </asp:GridView>

    
    <div class="DocumentosRequeridos">
        <br />
           <h4><strong>Documentos Requeridos</strong></h4>
           <table class="nav-justified" id="table-document">
                <tr>
                      <th><asp:CheckBox ID="cbx_informe" runat="server" Text="Informe Técnico" Enabled="false"/></th>
                      <th><asp:CheckBox ID="cbx_sims" runat="server" Text="SIMS" Enabled="false"/></th>
                      <th><asp:CheckBox ID="cbx_checCa" runat="server" Text="CHECKING DE CAMIONETA" Enabled="false"/></th>
                </tr>
               <tr>
                      <th><asp:CheckBox ID="cbx_detalleG" runat="server" Text="Detalle de Gastos" Enabled="false"/></th>
                      <th><asp:CheckBox ID="cbx_documentS" runat="server" Text="Documento de Seguridad" Enabled="false"/></th>
               </tr>
               <tr>
                      <th><asp:CheckBox ID="cbx_actaC" runat="server" Text="Acta de Conformidad" Enabled="false"/></th>
                      <th><asp:CheckBox ID="cbx_guia" runat="server" Text="Guía de Respuestos firmada por Cliente" Enabled="false"/></th>
               </tr>
         </table>
       </div>
</asp:Content>
