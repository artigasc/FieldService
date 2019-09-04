<%@ Page Title="" Language="C#" Async="true" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="FESA.SCM.Web._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <table class="nav-justified">
        <tr>
            <td style="width: 339px">
                &nbsp;</td>
            <td style="width: 272px">
                <asp:Login ID="LoginForm" runat="server"
                    LoginButtonText="Ingresar" UserNameRequiredErrorMessage="Nombre de usuario requerido."
                    PasswordRequiredErrorMessage="Contraseña requerida."
                     PasswordLabelText="Clave:" RememberMeText="Recordar Contraseña." FailureAction="Refresh"
                     TitleText="Ingreso al Sistema" UserNameLabelText="Usuario:" Width="404px" OnAuthenticate="LoginForm_Authenticate" DisplayRememberMe="False" FailureText="Contraseña incorrecta" Height="186px">
                    <LabelStyle CssClass="lbl-login" HorizontalAlign="Left" />
                    <LoginButtonStyle CssClass="botonI" BorderStyle="None" Height="30px" Width="100px" />
                    <TextBoxStyle Width="280px" CssClass="form-control" />
                    <TitleTextStyle Font-Bold="True" HorizontalAlign="Left" CssClass="title-login" Font-Size="20px" />
                </asp:Login>
            </td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td style="width: 339px">&nbsp;</td>
            <td style="width: 272px">&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
    </table>
</asp:Content>
