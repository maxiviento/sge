using System;
using System.Web.Mvc;
using System.Web.Routing;
using SGE.Model.Comun;
using SGE.Servicio.Servicio;
using SGE.Servicio.ServicioInterfaces;
using SGE.Servicio.Vista;
using SGE.Servicio.VistaInterfaces;
using SGE.Web.Controllers.Shared;
using SGE.Web.Infrastucture;
using SGE.Servicio.Vista.Shared;
using System.Text;


namespace SGE.Web.Controllers
{
    [CustomAuthorizeAttribute(Formulario = Enums.Formulario.ABMBarrios)]
    public class BarriosController : BaseController
    {
        //
        // GET: /Barrios/
        private readonly IBarrioServicio _barrioServicio;

        public BarriosController() : base((int)Formularios.Barrios)
        {
            _barrioServicio = new BarrioServicio();
        }

        public ActionResult Index()
        {
            var vista = _barrioServicio.GetIndex();
            vista.BusquedaDescripcion = "";
            return View(vista);
        }

        [HttpPost]
        public ActionResult BuscarBarrio(BarriosVista model)
        {

            return View("Index", _barrioServicio.GetBarrios(model.BusquedaDescripcion));
        }

        public ActionResult IndexPager(Pager pager, BarriosVista model)
        {
            IBarriosVista vista = _barrioServicio.GetBarrios(model.BusquedaDescripcion);
            vista.BusquedaDescripcion = model.BusquedaDescripcion;
            TempData["Model"] = vista;
            return RedirectToAction("PageRedirect");
        }

        public ActionResult PageRedirect()
        {
            return View("Index", TempData["Model"]);
        }



        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.ABMBarrios, Accion = Enums.Acciones.Consultar)]
        public ActionResult Ver(int IdBarrio)
        {
            IBarrioVista vista = _barrioServicio.GetBarrio(IdBarrio, "Ver");
            return View("Formulario", vista);
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.ABMBarrios, Accion = Enums.Acciones.Agregar)]
        public ActionResult Agregar()
        {
            IBarrioVista vista = _barrioServicio.GetBarrio(0, "Agregar");
            return View("Formulario", vista);
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.ABMBarrios, Accion = Enums.Acciones.Modificar)]
        public ActionResult Modificar(int IdBarrio)
        {
            IBarrioVista vista = _barrioServicio.GetBarrio(IdBarrio, "Modificar");
            return View("Formulario", vista);
        }


    
        [HttpPost]
        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.ABMBarrios, Accion = Enums.Acciones.Modificar)]
        public ActionResult ModificarBarrio(BarrioVista model)
        {
            switch (_barrioServicio.UpdateBarrio(model))
            {
                case 0:
                    var vis = _barrioServicio.GetIndex();
                    vis.BusquedaDescripcion = "";
                    return View("Index", vis);
                case 1:
                    TempData["MensajeError"] = "----------------";
                    return View("Formulario", model);
                case 2:
                    TempData["MensajeError"] = "Seleccione una opción";
                    return View("Formulario", model);
                default:
                    //Muestra listado de error;
                    return View("Formulario", model);
            }
        }


        [HttpPost]
        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.ABMBarrios, Accion = Enums.Acciones.Agregar)]
        public ActionResult AgregarBarrio(BarrioVista model)
        {
            //CursoVista modelo = model;
            switch (_barrioServicio.AddBarrio(model))
            {
                case 0:
                    return View("Index", _barrioServicio.GetIndex());
                case 1:
                    TempData["MensajeError"] = "------------------------------";
                    return View("Formulario", _barrioServicio.GetBarrioReload(model));
                case 2:
                    TempData["MensajeError"] = "Seleccione una opción";
                    return View("Formulario", model);
                default:
                    //Muestra listado de error;
                    return View("Formulario", model);
            }
        }


        [HttpPost]
        public ActionResult BarrioLocalidad(int idLocalidad)
        {

            var barrios = _barrioServicio.GetBarriosLocalidad(idLocalidad);

            return Json(barrios);
        }

    }
}
