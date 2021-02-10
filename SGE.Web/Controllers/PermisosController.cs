using System.Web.Mvc;
using SGE.Model.Comun;
using SGE.Servicio.Servicio;
using SGE.Servicio.ServicioInterfaces;
using SGE.Servicio.Vista;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces;
using SGE.Web.Controllers.Shared;
using SGE.Web.Infrastucture;

namespace SGE.Web.Controllers
{
    [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AsignarPermisos)]
    public class PermisosController : BaseController
    {
        private readonly IPermisoServicio _permisoServicio;

        public PermisosController(): base((int)Formularios.AsignarPermisos)
        {
            _permisoServicio = new PermisoServicio();
        }

        public ActionResult Index()
        {
            return View(_permisoServicio.GetIndex());
        }

        [HttpPost]
        public ActionResult BuscarRoles(PermisosVista model)
        {
            return View("Index", _permisoServicio.GetRoles(model.Nombre));
        }

        public ActionResult IndexPager(Pager pager, PermisosVista model)
        {
            IPermisosVista vista = _permisoServicio.GetRoles(pager, model.Nombre);

            TempData["Model"] = vista;
            return RedirectToAction("PageRedirect");
        }

        public ActionResult PageRedirect()
        {
            return View("Index", (PermisosVista)TempData["Model"]);
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AsignarPermisos, Accion = Enums.Acciones.Consultar)]
        public ActionResult Ver(int Id_Rol)
        {
            IPermisoVista vista = _permisoServicio.GetPermiso(Id_Rol, "Ver");
            return View("Formulario", vista);
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AsignarPermisos, Accion = Enums.Acciones.Modificar)]
        public ActionResult Modificar(int Id_Rol)
        {
            IPermisoVista vista = _permisoServicio.GetPermiso(Id_Rol, "Modificar");
            return View("Formulario", vista);
        }

        [HttpPost]
        public ActionResult ModificarPermiso(PermisoVista model)
        {
            if (_permisoServicio.UpdatePermiso(model))
            {
                return View("Index", _permisoServicio.GetRoles());
            }
            
            ModelState.AddModelError(string.Empty, "Se ha producido un error en la modificación de permisos. Consulte con el Administrador del Sistema.");
            return View("Formulario", _permisoServicio.GetPermiso(model.Id, "Modificar"));
        }
    }
}