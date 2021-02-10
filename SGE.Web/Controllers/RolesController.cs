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
    [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmRoles)]
    public class RolesController : BaseController
    {
        private readonly IRolServicio _rolServicio;

        public RolesController(): base((int)Formularios.Roles)
        {
            _rolServicio = new RolServicio();
        }

        public ActionResult Index()
        {
            return View(_rolServicio.GetIndex());
        }

        [HttpPost]
        public ActionResult BuscarRoles(RolesVista model)
        {
            return View("Index", _rolServicio.GetRoles(model.NombreBusqueda));
        }

        public ActionResult IndexPager(Pager pager, RolesVista model)
        {
            IRolesVista vista = _rolServicio.GetRoles(pager, model.NombreBusqueda);

            TempData["Model"] = vista;
            
            return RedirectToAction("PageRedirect");
        }

        public ActionResult PageRedirect()
        {
            return View("Index", (RolesVista)TempData["Model"]);
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmRoles, Accion = Enums.Acciones.Agregar)]
        public ActionResult Agregar()
        {
            return View("Formulario", _rolServicio.GetRol(0, "Agregar"));
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmRoles, Accion = Enums.Acciones.Consultar)]
        public ActionResult Ver(int Id_Rol)
        {
            return View("Formulario", _rolServicio.GetRol(Id_Rol, "Ver"));
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmRoles, Accion = Enums.Acciones.Modificar)]
        public ActionResult Modificar(int Id_Rol)
        {
            return View("Formulario", _rolServicio.GetRol(Id_Rol, "Modificar"));
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmRoles, Accion = Enums.Acciones.Eliminar)]
        public ActionResult Eliminar(int Id_Rol)
        {
            return View("Formulario", _rolServicio.GetRol(Id_Rol, "Eliminar"));
        }

        [HttpPost]
        public ActionResult AgregarRol(RolVista model)
        {
            switch (_rolServicio.AddRol(model))
            {
                case 0:
                    return View("Index", _rolServicio.GetRoles());
                case 1:
                    ModelState.AddModelError(string.Empty, "Ya existe un rol con ese nombre");
                    return View("Formulario", _rolServicio.GetRol(0, "Agregar"));
                default:
                    //Mustra listado de error;
                    return View("Formulario", _rolServicio.GetRol(0, "Agregar"));
            }
        }

        [HttpPost]
        public ActionResult ModificarRol(RolVista model)
        {
            switch (_rolServicio.UpdateRol(model))
            {
                case 0:
                    return View("Index", _rolServicio.GetRoles());
                case 1:
                    ModelState.AddModelError(string.Empty, "Ya existe un rol con ese nombre. Verifique.");
                    return View("Formulario", _rolServicio.GetRol(model.Id, "Modificar"));
                default:
                    //Mustra listado de error;
                    return View("Formulario", _rolServicio.GetRol(model.Id, "Modificar"));
            }
        }

        [HttpPost]
        public ActionResult EliminarRol(RolVista model)
        {
            ModelState.Clear();

            switch (_rolServicio.DeleteRol(model))
            {
                case 0:
                    return View("Index", _rolServicio.GetRoles());
                case 1:
                    ModelState.AddModelError(string.Empty, "El rol ya tiene permisos asignados. Verifique");
                    return View("Formulario", _rolServicio.GetRol(model.Id, "Eliminar"));
                case 2:
                    ModelState.AddModelError(string.Empty, "El rol tiene usuarios asignados. No se puede Eliminar.");
                    return View("Formulario", _rolServicio.GetRol(model.Id, "Eliminar"));
                default:
                    return View("Formulario", _rolServicio.GetRol(model.Id, "Eliminar"));
            }
        }
    }
}
