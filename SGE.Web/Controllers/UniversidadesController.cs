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
    [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmUniversidades)]
    public class UniversidadesController : BaseController
    {
        private readonly IUniversidadServicio _universidadServicio;

        public UniversidadesController(): base((int)Formularios.Universidad)
        {
            _universidadServicio=new UniversidadServicio();
        }

        public ViewResult Index()
        {
            var vista=_universidadServicio.GetIndex();
            vista.DescripcionBusca = "";
            return View(vista);
        }

        [HttpPost]
        public ActionResult BuscarUniversidad(UniversidadesVista model)
        {
            return View("Index", _universidadServicio.GetUniversidades(model.DescripcionBusca));
        }

        public ActionResult IndexPager(Pager pager, UniversidadesVista model)
        {
            IUniversidadesVista vista = _universidadServicio.GetUniversidades(pager, model.Id, model.DescripcionBusca);

            TempData["Model"] = vista;
            return RedirectToAction("PageRedirect");
        }

       public ActionResult PageRedirect()
       {
           return View("Index", TempData["Model"]);
       }

       [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmUniversidades, Accion = Enums.Acciones.Agregar)]
       public ActionResult Agregar()
        {
            IUniversidadVista vista = _universidadServicio.AddUniversidadLast("Agregar");
            return View("Formulario", vista);
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmUniversidades, Accion = Enums.Acciones.Consultar)]
        public ActionResult Ver(int idUniversidad)
        {
            IUniversidadVista vista = _universidadServicio.GetUniversidad(idUniversidad, "Ver");
            return View("Formulario", vista);
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmUniversidades, Accion = Enums.Acciones.Modificar)]
        public ActionResult Modificar(int idUniversidad)
        {
            IUniversidadVista vista = _universidadServicio.GetUniversidad(idUniversidad, "Modificar");
            return View("Formulario", vista);
        }

        [HttpPost]
        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmUniversidades, Accion = Enums.Acciones.Agregar)]
        public ActionResult AgregarUniversidad(UniversidadVista model)
        {
            switch (_universidadServicio.AddUniversidad(model))
            {
                case 0:
                    return View("Index",  _universidadServicio.GetUniversidades());
                case 1:
                    TempData["MensajeError"] = "Ya existe una Universidad con ese nombre";
                    return View("Formulario", model);
                default:
                    //Muestra listado de error;
                    return View("Formulario", model);
            }
        }

        [HttpPost]
        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmUniversidades, Accion = Enums.Acciones.Modificar)]
        public ActionResult ModificarUniversidad(UniversidadVista model)
        {
            switch (_universidadServicio.UpdateUniversidad(model))
            {
                case 0:
                    var vista= _universidadServicio.GetUniversidades();
                    vista.DescripcionBusca = "";
                    return View("Index",vista);
                case 1:
                    TempData["MensajeError"] = "Ya existe una Universidad con ese nombre";
                    return View("Formulario", model);
                default:
                    //Muestra listado de error;
                    return View("Formulario", model);
            }
        }
    }
}
