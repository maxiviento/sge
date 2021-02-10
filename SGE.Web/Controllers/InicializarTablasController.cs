using System.Web.Mvc;
using SGE.Servicio.Servicio;
using SGE.Servicio.ServicioInterfaces;
using SGE.Servicio.Vista;
using SGE.Web.Controllers.Shared;
using SGE.Web.Infrastucture;

namespace SGE.Web.Controllers
{
    //[CustomAuthorizeAttribute(Formulario = Formularios.InicializarTablas)]
    public class InicializarTablasController : BaseController
    {
        private readonly IInicializarTablasServicio _inicializarTablasServicio;

        public InicializarTablasController(): base((int)Formularios.InicializarTablas)
        {
            _inicializarTablasServicio=new InicializarTablasServicio();
        }

        public ViewResult Index()
        {
            var vista = new InicializarTablasVista();
            return View(vista);
        }

        [HttpPost]
        //[CustomAuthorizeAttribute(Formulario = Formularios.InicializarTablas, Accion = Acciones.Agregar)]
        public ActionResult Insertar(InicializarTablasVista model)
        {
            _inicializarTablasServicio.Insertar(model);
            return View("Index", model);
        }
    }
}
