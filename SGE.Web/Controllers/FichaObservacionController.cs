using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
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
    [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmFichaObservacion)]
    public class FichaObservacionController : BaseController
    {
        private readonly IFichaObservacionServicio _fichaObservacionServicio;

        public FichaObservacionController()
            : base((int)Formularios.FichaObservacion)
        {
            _fichaObservacionServicio = new FichaObservacionServicio();
        }

        public ActionResult Index(int idFicha, string accionFicha)
        {
            var vista = _fichaObservacionServicio.GetObservacionesByFicha(idFicha);
            vista.AccionFicha = accionFicha;
            TempData["Model"] = vista;
            return View(vista);
        }

        public ActionResult IndexPager(Pager pager, FichaObservacionesVista model)
        {
            IFichaObservacionesVista vista = _fichaObservacionServicio.GetObservacionesByFicha(pager, model.IdFicha);
            vista.AccionFicha = model.AccionFicha;
            TempData["Model"] = vista;
            return RedirectToAction("PageRedirect");
        }

        public ActionResult PageRedirect()
        {
            return View("Index", (FichaObservacionesVista)TempData["Model"]);
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmFichaObservacion, Accion = Enums.Acciones.Consultar)]
        public ActionResult Ver(int idFicha, int idFichaObservacion, string accionFicha)
        {
            IFichaObservacionVista vista = _fichaObservacionServicio.GetObservacion(idFicha,idFichaObservacion, "Ver");
            vista.AccionFicha = accionFicha;
            return View("Formulario", vista);
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmFichaObservacion, Accion = Enums.Acciones.Agregar)]
        public ActionResult Agregar(int idFicha, int? idFichaObservacion, string accionFicha)
        {
            IFichaObservacionVista vista = _fichaObservacionServicio.GetObservacion(idFicha, -1, "Agregar");
            if (idFichaObservacion != null)
            {
                vista.IdFichaObservacion = idFichaObservacion.Value;
                vista.IdFicha = idFicha;
                vista.AccionFicha = accionFicha;
            }
          
            return View("Formulario", vista);
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmFichaObservacion, Accion = Enums.Acciones.Modificar)]
        public ActionResult Modificar(int idFicha, int idFichaObservacion,string accionFicha)
        {
            IFichaObservacionVista vista = _fichaObservacionServicio.GetObservacion(idFicha, idFichaObservacion, "Modificar");
            vista.AccionFicha = accionFicha;
            return View("Formulario", vista);
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmFichaObservacion, Accion = Enums.Acciones.Eliminar)]
        public ActionResult Eliminar(int idFicha, int idFichaObservacion, string accionFicha)
        {
            IFichaObservacionVista vista = _fichaObservacionServicio.GetObservacion(idFicha,idFichaObservacion, "Eliminar");
            vista.AccionFicha = accionFicha;
            return View("Formulario", vista);
        }

        [HttpPost]
        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmFichaObservacion, Accion = Enums.Acciones.Agregar)]
        public ActionResult AgregarObservacion(FichaObservacionVista model)
        {
            switch (_fichaObservacionServicio.AddObservacion(model))
            {
                case 0:
                    var vis = _fichaObservacionServicio.GetObservacionesByFicha(model.IdFicha);
                    vis.AccionFicha = model.AccionFicha;
                    return View("Index", vis);
                default:
                    return View("Formulario", model);
            }
        }

        [HttpPost]
        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmFichaObservacion, Accion = Enums.Acciones.Modificar)]
        public ActionResult ModificarObservacion(FichaObservacionVista model)
        {
            ModelState.Clear();
            switch (_fichaObservacionServicio.UpdateObservacion(model))
            {
                case 0:
                    var vis = _fichaObservacionServicio.GetObservacionesByFicha(model.IdFicha);
                    vis.AccionFicha = model.AccionFicha;
                    return View("Index", vis);
                default:
                    return View("Formulario", model);
            }
        }

        [HttpPost]
        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmFichaObservacion, Accion = Enums.Acciones.Eliminar)]
        public ActionResult EliminarObservacion(FichaObservacionVista model)
        {
            ModelState.Clear();
            switch (_fichaObservacionServicio.DeleteObservacion(model))
            {
                case 0:
                    var vis = _fichaObservacionServicio.GetObservacionesByFicha(model.IdFicha);
                    vis.AccionFicha = model.AccionFicha;
                    return View("Index", vis);
                case 1:
                    TempData["MensajeError"] = "No se pudo eliminar la observación.";
                    return View("Formulario", model);
                default:
                    return View("Formulario", model);
            }
        }

        [HttpPost]
        public ActionResult VolverListadoObservaciones(FichaObservacionVista model)
        {
            return RedirectToAction("Index", new RouteValueDictionary(
                          new { controller = "FichaObservacion", action = "Index", idFicha = model.IdFicha, accionFicha=model.AccionFicha}));
        }

        [HttpPost]
        public ActionResult VolverFormFicha(FichaObservacionesVista model)
        {
            return RedirectToAction(model.AccionFicha, "Fichas", new { idFicha = model.IdFicha });
        }

    }
}
