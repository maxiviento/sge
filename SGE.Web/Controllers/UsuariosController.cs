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
    [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmUsuario)]
    public class UsuariosController : BaseController
    {
        private readonly IUsuarioServicio _usuarioServicio;

        public UsuariosController(): base((int)Formularios.Usuario)
        {
            _usuarioServicio = new UsuarioServicio();
        }

        public ViewResult Index()
        {
            return View(_usuarioServicio.GetIndex());
        }

        [HttpPost]
        public ActionResult BuscarUsuarios(UsuariosVista model)
        {
            return View("Index", _usuarioServicio.GetUsuarios(model.LoginBusqueda, model.NombreBusqueda, model.ApellidoBusqueda));
        }

        public ActionResult IndexPager(Pager pager, UsuariosVista model)
        {
            IUsuariosVista vista = _usuarioServicio.GetUsuarios(pager, model.LoginBusqueda, model.NombreBusqueda, model.ApellidoBusqueda);

            TempData["Model"] = vista;
            return RedirectToAction("PageRedirect");
        }

        public ActionResult PageRedirect()
        {
            return View("Index", (UsuariosVista)TempData["Model"]);
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmUsuario, Accion = Enums.Acciones.Agregar)]
        public ActionResult Agregar()
        {
            return View("Formulario", _usuarioServicio.GetUsuario(0, "Agregar"));
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmUsuario, Accion = Enums.Acciones.Consultar)]
        public ActionResult Ver(int idUsuario)
        {
            return View("Formulario", _usuarioServicio.GetUsuario(idUsuario, "Ver"));
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmUsuario, Accion = Enums.Acciones.Modificar)]
        public ActionResult Modificar(int idUsuario)
        {
            return View("Formulario", _usuarioServicio.GetUsuario(idUsuario, "Modificar"));
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmUsuario, Accion = Enums.Acciones.Eliminar)]
        public ActionResult Eliminar(int idUsuario)
        {
            return View("Formulario", _usuarioServicio.GetUsuario(idUsuario, "Eliminar"));
        }

        [HttpPost]
        public ActionResult AgregarUsuario(UsuarioVista model)
        {
            switch (_usuarioServicio.AddUsuario(model))
            {
                case 0:
                    return View("Index", _usuarioServicio.GetUsuarios());
                case 1:
                    ModelState.AddModelError(string.Empty, "El Loguin ya existe");
                    return View("Formulario", _usuarioServicio.GetUsuario(0, "Agregar"));
                default:
                    //Muestra listado de error;
                    return View("Formulario", _usuarioServicio.GetUsuario(0, "Agregar"));
            }
        }

        [HttpPost]
        public ActionResult ModificarUsuario(UsuarioVista model)
        {
            ModelState.Clear();

            return _usuarioServicio.UpdateUsuario(model) ? View("Index", _usuarioServicio.GetUsuarios()) 
                : View("Formulario", _usuarioServicio.GetUsuario(model.Id, "Modificar"));
        }

        [HttpPost]
        public ActionResult EliminarUsuario(UsuarioVista model)
        {
            ModelState.Clear();

            return _usuarioServicio.DeleteUsuario(model) ? View("Index", _usuarioServicio.GetUsuarios()) 
                : View("Formulario", _usuarioServicio.GetUsuario(model.Id, "Eliminar"));
        }
    }
}