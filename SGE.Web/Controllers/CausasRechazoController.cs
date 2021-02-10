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
    [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmCausaRechazo)]
    public class CausasRechazoController : BaseController
    {
        private readonly ICausaRechazoServicio _causaRechazoServicio;

        public CausasRechazoController(): base((int)Formularios.CausaRechazo)
        {
            _causaRechazoServicio=new CausaRechazoServicio();
        }

        public ViewResult Index()
        {
            var vista=_causaRechazoServicio.GetIndex();
            
            return View(vista);
        }

        [HttpPost]
        public ActionResult BuscarCausaRechazo(CausasRechazoVista model)
        {
            return View("Index", _causaRechazoServicio.GetCausasRechazo(model.BuscarDescripcion));
        }

        public ActionResult IndexPager(Pager pager, CausasRechazoVista model)
        {
            ICausasRechazoVista vista = _causaRechazoServicio.GetCausasRechazo(pager, model.Id, model.BuscarDescripcion);

            TempData["Model"] = vista;
            return RedirectToAction("PageRedirect");
        }

       public ActionResult PageRedirect()
       {
           return View("Index", (CausasRechazoVista)TempData["Model"]);
       }

       [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmCausaRechazo, Accion = Enums.Acciones.Agregar)]
       public ActionResult Agregar()
        {
            ICausaRechazoVista vista = _causaRechazoServicio.AddCausaRechazoLast("Agregar");
            return View("Formulario", vista);
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmCausaRechazo, Accion = Enums.Acciones.Consultar)]
        public ActionResult Ver(int idCausaRechazo, string busquedaAnterior)
        {
            ICausaRechazoVista vista = _causaRechazoServicio.GetCausaRechazo(idCausaRechazo, "Ver");
            vista.BusquedaAnterior = busquedaAnterior;
            return View("Formulario", vista);
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmCausaRechazo, Accion = Enums.Acciones.Modificar)]
        public ActionResult Modificar(int idCausaRechazo, string busquedaAnterior)
        {
            ICausaRechazoVista vista = _causaRechazoServicio.GetCausaRechazo(idCausaRechazo, "Modificar");
            vista.IdOld = idCausaRechazo;
            vista.BusquedaAnterior = busquedaAnterior;
            
            return View("Formulario", vista);
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmCausaRechazo, Accion = Enums.Acciones.Eliminar)]
        public ActionResult Eliminar(int idCausaRechazo, string busquedaAnterior )
        {
            ICausaRechazoVista vista = _causaRechazoServicio.GetCausaRechazo(idCausaRechazo, "Eliminar");
            return View("Formulario", vista);
        }

        [HttpPost]
        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmCausaRechazo, Accion = Enums.Acciones.Agregar)]
        public ActionResult AgregarCausaRechazo(CausaRechazoVista model)
        {
            switch (_causaRechazoServicio.AddCausaRechazo(model))
            {
                case 0:

                    var vista = new CausasRechazoVista();
                    vista.BuscarDescripcion = model.BusquedaAnterior;
                  return View("Index", _causaRechazoServicio.GetCausasRechazo(vista));
                case 1:
                    TempData["MensajeError"] = "Ya existe una Causa de Rechazo con ese nombre o Id";
                    return View("Formulario", model);
                default:
                    //Muestra listado de error;
                    return View("Formulario", model);
            }
        }

        [HttpPost]
        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmCausaRechazo, Accion = Enums.Acciones.Modificar)]
        public ActionResult ModificarCausaRechazo(CausaRechazoVista model)
        {
            switch (_causaRechazoServicio.UpdateCausaRechazo(model))
            {
                case 0:
                    var vista = new CausasRechazoVista();
                    vista.BuscarDescripcion = model.BusquedaAnterior;
                    return View("Index",_causaRechazoServicio.GetCausasRechazo(vista));
                case 1:
                    TempData["MensajeError"] = "Ya existe una Causa de Rechazo con ese nombre o Id";
                    return View("Formulario", model);
                default:
                    //Muestra listado de error;
                    return View("Formulario", model);
            }
        }

        [HttpPost]
        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmCausaRechazo, Accion = Enums.Acciones.Eliminar)]
        public ActionResult EliminarCausaRechazo(CausaRechazoVista model)
        {
            switch (_causaRechazoServicio.DeleteCausaRechazo(model))
            {
                case 0:
                    var vista = new CausasRechazoVista();
                    vista.BuscarDescripcion = model.BusquedaAnterior;
                    return View("Index",_causaRechazoServicio.GetCausasRechazo(vista));
                case 1:
                    TempData["MensajeError"] = "Excepción!";
                    return View("Formulario", model);
               default:
                    //Muestra listado de error;
                    return View("Formulario", model);
            }
        }

        public ActionResult ValidarEliminar(int idCausaRechazo)
        {
            bool mreturn = _causaRechazoServicio.CausaRechazoInUse(idCausaRechazo);

            return Json(mreturn);
        }

    }
}
