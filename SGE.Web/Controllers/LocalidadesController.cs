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
    [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmLocalidades)]
    public class LocalidadesController : BaseController
    {
        private readonly ILocalidadServicio _localidadServicio;

        public LocalidadesController(): base((int)Formularios.Localidad)
        {
            _localidadServicio=new LocalidadServicio();
        }

        public ViewResult Index()
        {
            var vista=_localidadServicio.GetIndex();
            vista.DescripcionBusca = "";
            return View(vista);
        }

        [HttpPost]
        public ActionResult BuscarLocalidad(LocalidadesVista model)
        {
            return View("Index", _localidadServicio.GetLocalidades(model.DescripcionBusca));
        }

        public ActionResult IndexPager(Pager pager, LocalidadesVista model)
        {
            ILocalidadesVista vista = _localidadServicio.GetLocalidades(pager, model.Id, model.DescripcionBusca);

            TempData["Model"] = vista;
            return RedirectToAction("PageRedirect");
        }

       public ActionResult PageRedirect()
       {
           return View("Index", TempData["Model"]);
       }

       [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmLocalidades, Accion = Enums.Acciones.Agregar)]
       public ActionResult Agregar()
        {
            ILocalidadVista vista = _localidadServicio.AddLocalidadLast("Agregar");
            return View("Formulario", vista);
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmLocalidades, Accion = Enums.Acciones.Consultar)]
       public ActionResult Ver(int idLocalidad)
        {
            ILocalidadVista vista = _localidadServicio.GetLocalidad(idLocalidad, "Ver");
            return View("Formulario", vista);
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmLocalidades, Accion = Enums.Acciones.Modificar)]
        public ActionResult Modificar(int idLocalidad)
        {
            ILocalidadVista vista = _localidadServicio.GetLocalidad(idLocalidad, "Modificar");
            return View("Formulario", vista);
        }

        [HttpPost]
        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmLocalidades, Accion = Enums.Acciones.Agregar)]
        public ActionResult AgregarLocalidad(LocalidadVista model)
        {
            switch (_localidadServicio.AddLocalidad(model))
            {
                case 0:
                    return View("Index",  _localidadServicio.GetLocalidades());
                case 1:
                    TempData["MensajeError"] = "Ya existe una Localidad con ese nombre";
                    return View("Formulario", model);
                default:
                    //Muestra listado de error;
                    return View("Formulario", model);
            }
        }

        [HttpPost]
        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmLocalidades, Accion = Enums.Acciones.Modificar)]
        public ActionResult ModificarLocalidad(LocalidadVista model)
        {
            switch (_localidadServicio.UpdateLocalidad(model))
            {
                case 0:
                    var vista= _localidadServicio.GetLocalidades();
                    vista.DescripcionBusca = "";
                    return View("Index",vista);
                case 1:
                    TempData["MensajeError"] = "Ya existe una Localidad con ese nombre";
                    return View("Formulario", model);
                default:
                    //Muestra listado de error;
                    return View("Formulario", model);
            }
        }
    }
}
