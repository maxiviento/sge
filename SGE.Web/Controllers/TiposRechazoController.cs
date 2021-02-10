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
    [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmTipoRechazo)]
    public class TiposRechazoController : BaseController
    {
        private readonly ITipoRechazoServicio _tipoRechazoServicio;

        public TiposRechazoController(): base((int)Formularios.TipoRechazo)
        {
            _tipoRechazoServicio = new TipoRechazoServicio();
        }

        public ViewResult Index()
        {
            var vista = _tipoRechazoServicio.GetIndex();
            return View(vista);
        }

        [HttpPost]
        public ActionResult BuscarTipoRechazo(TiposRechazoVista model)
        {
            return View("Index", _tipoRechazoServicio.GetTiposRechazo(model.DescripcionBusca));
        }

        public ActionResult IndexPager(Pager pager, TiposRechazoVista model)
        {
            ITiposRechazoVista vista = _tipoRechazoServicio.GetTiposRechazo(pager, model.Id, model.DescripcionBusca);

            TempData["Model"] = vista;
            return RedirectToAction("PageRedirect");
        }

        public ActionResult PageRedirect()
        {
            return View("Index", (TiposRechazoVista)TempData["Model"]);
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmTipoRechazo, Accion = Enums.Acciones.Agregar)]
        public ActionResult Agregar()
        {
            ITipoRechazoVista vista = _tipoRechazoServicio.AddTipoRechazoLast("Agregar");
            return View("Formulario", vista);
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmTipoRechazo, Accion = Enums.Acciones.Consultar)]
        public ActionResult Ver(int idTipoRechazo)
        {
            ITipoRechazoVista vista = _tipoRechazoServicio.GetTipoRechazo(idTipoRechazo, "Ver");
            return View("Formulario", vista);
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmTipoRechazo, Accion = Enums.Acciones.Modificar)]
        public ActionResult Modificar(int idTipoRechazo)
        {
            ITipoRechazoVista vista = _tipoRechazoServicio.GetTipoRechazo(idTipoRechazo, "Modificar");
            vista.IdOld = idTipoRechazo;
            return View("Formulario", vista);
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmTipoRechazo, Accion = Enums.Acciones.Eliminar)]
        public ActionResult Eliminar(int idTipoRechazo)
        {
            ITipoRechazoVista vista = _tipoRechazoServicio.GetTipoRechazo(idTipoRechazo, "Eliminar");
            return View("Formulario", vista);
        }

        [HttpPost]
        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmTipoRechazo, Accion = Enums.Acciones.Agregar)]
        public ActionResult AgregarTipoRechazo(TipoRechazoVista model)
        {
            switch (_tipoRechazoServicio.AddTipoRechazo(model))
            {
                case 0:
                    var vista = _tipoRechazoServicio.GetTiposRechazo();
                    return View("Index", vista);
                case 1:
                    TempData["MensajeError"] = "Ya existe un Tipo de Rechazo con ese nombre o Id";
                    return View("Formulario", model);
                default:
                    //Muestra listado de error;
                    return View("Formulario", model);
            }
        }

        [HttpPost]
        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmTipoRechazo, Accion = Enums.Acciones.Modificar)]
        public ActionResult ModificarTipoRechazo(TipoRechazoVista model)
        {
            switch (_tipoRechazoServicio.UpdateTipoRechazo(model))
            {
                case 0:
                    var vista = _tipoRechazoServicio.GetTiposRechazo();
                    return View("Index", vista);
                case 1:
                    TempData["MensajeError"] = "Ya existe una Tipo de Rechazo con ese nombre o Id";
                    return View("Formulario", model);
                default:
                    //Muestra listado de error;
                    return View("Formulario", model);
            }
        }

        [HttpPost]
        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmTipoRechazo, Accion = Enums.Acciones.Eliminar)]
        public ActionResult EliminarTipoRechazo(TipoRechazoVista model)
        {
            switch (_tipoRechazoServicio.DeleteTipoRechazo(model))
            {
                case 0:
                    var vista = _tipoRechazoServicio.GetTiposRechazo();
                    return View("Index", vista);
                case 1:
                    TempData["MensajeError"] = "Excepción";
                    return View("Formulario", model);
                default:
                    //Muestra listado de error;
                    return View("Formulario", model);
            }
        }

        public ActionResult ValidarEliminar(int idTipoRechazo)
        {
            bool mreturn = _tipoRechazoServicio.TipoRechazoInUse(idTipoRechazo);

            return Json(mreturn);
        }
    }
}
