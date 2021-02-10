<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<SGE.Servicio.Vista.Shared.ComboBox>" %>
<% if (Model.Enabled)
   { %>
<%=Html.DropDownList("Selected", new SelectList(Model.Combo, "id", "description", Model.Selected) , new {@Class ="Accion"}) %>
<%}
   else
   {%>
<%=Html.DropDownList("Selected", new SelectList(Model.Combo, "id", "description", Model.Selected), new { disabled = "disabled" , @Class ="Accion" })%>
<%} %>
