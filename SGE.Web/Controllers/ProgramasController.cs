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
    [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmProgramas)]
    public class ProgramasController : BaseController
    {
        private readonly IProgramaServicio _programaServicio;

        public ProgramasController(): base((int)Formularios.Programa)
        {
            _programaServicio=new ProgramaServicio();
        }

        public ViewResult Index()
        {
            var vista=_programaServicio.GetIndex();
            vista.DescripcionBusca = "";
            return View(vista);
        }

        [HttpPost]
        public ActionResult BuscarPrograma(ProgramasVista model)
        {
            return View("Index", _programaServicio.GetProgramas(model.DescripcionBusca));
        }

        public ActionResult IndexPager(Pager pager, ProgramasVista model)
        {
            IProgramasVista vista = _programaServicio.GetProgramas(pager, model.Id, model.DescripcionBusca);

            TempData["Model"] = vista;
            return RedirectToAction("PageRedirect");
        }

       public ActionResult PageRedirect()
       {
           return View("Index",TempData["Model"]);
       }

       [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmProgramas, Accion = Enums.Acciones.Agregar)]
       public ActionResult Agregar()
        {
            IProgramaVista vista = _programaServicio.AddProgramaLast("Agregar");
            return View("Formulario", vista);
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmProgramas, Accion = Enums.Acciones.Consultar)]
       public ActionResult Ver(int idPrograma)
        {
            IProgramaVista vista = _programaServicio.GetPrograma(idPrograma, "Ver");
            return View("Formulario", vista);
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmProgramas, Accion = Enums.Acciones.Modificar)]
        public ActionResult Modificar(int idPrograma)
        {
            IProgramaVista vista = _programaServicio.GetPrograma(idPrograma, "Modificar");
            return View("Formulario", vista);
        }

        [HttpPost]
        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmProgramas, Accion = Enums.Acciones.Agregar)]
        public ActionResult AgregarPrograma(ProgramaVista model)
        {
            switch (_programaServicio.AddPrograma(model))
            {
                case 0:
                    return View("Index",  _programaServicio.GetProgramas());
                case 1:
                    TempData["MensajeError"] = "Ya existe un Programa con ese nombre";
                    return View("Formulario", model);
                default:
                    //Muestra listado de error;
                    return View("Formulario", model);
            }
        }

        [HttpPost]
        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmProgramas, Accion = Enums.Acciones.Modificar)]
        public ActionResult ModificarPrograma(ProgramaVista model)
        {
            switch (_programaServicio.UpdatePrograma(model))
            {
                case 0:
                    var vista= _programaServicio.GetProgramas();
                    vista.DescripcionBusca = "";
                    return View("Index",vista);
                case 1:
                    TempData["MensajeError"] = "Ya existe un Programa con ese nombre";
                    return View("Formulario", model);
                default:
                    //Muestra listado de error;
                    return View("Formulario", model);
            }
        }
    }
}
