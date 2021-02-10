using System.Web.Mvc;
using SGE.Model.Comun;
using SGE.Servicio.Servicio;
using SGE.Servicio.ServicioInterfaces;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces;
using SGE.Web.Controllers.Shared;
using SGE.Web.Infrastucture;
using SGE.Servicio.Vista;
using System.Text;

namespace SGE.Web.Controllers
{
    [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmInstituciones)]
    public class InstitucionesController : BaseController
    {
        private readonly IInstitucionServicio _institucionServicio;

        public InstitucionesController(): base((int)Formularios.Institucion)
        {
            _institucionServicio = new InstitucionServicio();
        }

        public ViewResult Index()
        {
            var vista = _institucionServicio.GetIndex();
            return View(vista);
        }

        [HttpPost]
        public ActionResult BuscarInstitucion(InstitucionesVista model)
        {
            return View("Index", _institucionServicio.GetInstituciones(model.Descripcion));
        }

        public ActionResult IndexPager(Pager pager, InstitucionesVista model)
        {
            IInstitucionesVista vista = _institucionServicio.GetInstituciones(pager, model.Id, model.Descripcion);

            TempData["Model"] = vista;
            return RedirectToAction("PageRedirect");
        }

        public ActionResult PageRedirect()
        {
            return View("Index", (InstitucionesVista)TempData["Model"]);
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmInstituciones, Accion = Enums.Acciones.Agregar)]
        public ActionResult Agregar(int? idInstitucion)
        {
            IInstitucionVista vista = _institucionServicio.GetInstitucion(-1, "Agregar");
            if (idInstitucion != null) vista.Id = idInstitucion.Value;

            return View("Formulario", vista);
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmInstituciones, Accion = Enums.Acciones.Consultar)]
        public ActionResult Ver(int idInstitucion)
        {
            IInstitucionVista vista = _institucionServicio.GetInstitucion(idInstitucion, "Ver");
            return View("Formulario", vista);
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmInstituciones, Accion = Enums.Acciones.Modificar)]
        public ActionResult Modificar(int idInstitucion)
        {
            IInstitucionVista vista = _institucionServicio.GetInstitucion(idInstitucion, "Modificar");
            return View("Formulario", vista);
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmInstituciones, Accion = Enums.Acciones.Eliminar)]
        public ActionResult Eliminar(int idInstitucion)
        {
            IInstitucionVista vista = _institucionServicio.GetInstitucion(idInstitucion, "Eliminar");
            return View("Formulario", vista);
        }

        [HttpPost]
        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmInstituciones, Accion = Enums.Acciones.Agregar)]
        public ActionResult AgregarInstitucion(InstitucionVista model)
        {
            ModelState.Clear();
            switch (_institucionServicio.AddInstitucion(model))
            {
                case 0:
                    //var vista = _institucionServicio.GetInstituciones();
                    //return View("Index", vista);
                    model.Accion = "Modificar";
                    return View("Formulario", model);
                case 1:
                    TempData["MensajeError"] = "Ya existe una Institución con ese nombre";
                    return View("Formulario", model);
                default:
                    return View("Formulario", model);
            }
        }

        [HttpPost]
        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmInstituciones, Accion = Enums.Acciones.Modificar)]
        public ActionResult ModificarInstitucion(InstitucionVista model)
        {
            ModelState.Clear();
            switch (_institucionServicio.UpdateInstitucion(model))
            {
                case 0:
                    var vista = _institucionServicio.GetInstituciones();
                    return View("Index", vista);
                case 1:
                    TempData["MensajeError"] = "Ya existe una Institución con ese nombre";
                    return View("Formulario", model);
                default:
                    return View("Formulario", model);
            }
        }

        [HttpPost]
        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmInstituciones, Accion = Enums.Acciones.Eliminar)]
        public ActionResult EliminarInstitucion(InstitucionVista model)
        {
            ModelState.Clear();
            switch (_institucionServicio.DeleteInstitucion(model))
            {
                case 0:
                    var vista = _institucionServicio.GetInstituciones();
                    return View("Index", vista);
                case 1:
                    TempData["MensajeError"] = "La Institución está en uso";
                    return View("Formulario", model);
                default:
                    return View("Formulario", model);
            }
        }

        // 07/03/2013 - DI CAMPLI LEANDRO

        [HttpPost]
        public ActionResult AsociarSector(int idInstitucion, string accion)
        {

             ISectoresVista vista = _institucionServicio.GetSectorNoAsignado(idInstitucion);

            var tablaretorno = new StringBuilder();

            tablaretorno.Append(
                "<thead><tr><th style='height: 30px; width: 500px;'><div align='center'>Nombre Sede</div></th>");
        tablaretorno.Append("<th style='width: 80px;'><div align='center'>Acción</div></th></tr></thead><tbody>");

        if (vista.Sectores.Count > 0)
        {
            foreach (var sector in vista.Sectores)
            {
                tablaretorno.Append("<tr><td style='height: 28px;  text-align: left;'>" + sector.NombreSector + "</td>");

                tablaretorno.Append(
                    "<td style='height: 28px;  text-align: center;'><img  src='../../Content/images/icon_derecha.gif' style='cursor: pointer;' width='18'  height='18' alt='' onclick='SeleccionarSector(" +
                    idInstitucion + "," + sector.IdSector + ",$" + accion + "$);'  /></td></tr>");
            }
        }
        else { tablaretorno.Append("<tr><b>No se encontraron datos</b><td></td><td></td></tr>"); }

            tablaretorno.Append("</tbody>");

            return Json(tablaretorno.ToString().Replace('$', '"'));
        }

        [HttpPost]
        public ActionResult AddSectorInstitucion(int idInstitucion, int idSector, string acc)
        {

            _institucionServicio.AddAsociarSector(idInstitucion, idSector);
            IInstitucionVista vista = _institucionServicio.GetInstitucion(idInstitucion, acc);

            if (acc == "Modificar")
            { return RedirectToAction("Modificar", "Instituciones", new { idInstitucion = vista.Id }); }
            else { return View("Ver", "Instituciones", new { idInstitucion = vista.Id }); }

        }

        
        public ActionResult delSectorInstitucion(int idInstitucion, int idSector, string acc)
        {

            string msg = _institucionServicio.DeleteSectorInstitucion(idInstitucion, idSector);
            IInstitucionVista vista = _institucionServicio.GetInstitucion(idInstitucion, acc);

            //vista.restultados = msg;
            TempData["MensajeError"] = msg;
            if (acc == "Modificar")
            { return RedirectToAction("Modificar", "Instituciones", new { idInstitucion = vista.Id }); }
            else { return View("Ver", "Instituciones", new { idInstitucion = vista.Id }); }
        }

    }
}
