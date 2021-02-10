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
    [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmConceptos)]
    public class ConceptosController : BaseController
    {
        private readonly IConceptoServicio _conceptoservicio;

        public ConceptosController() : base((int)Formularios.Conceptos)
        {
            _conceptoservicio = new ConceptoServicio();
        }

        public ActionResult Index()
        {
            return View("Index", _conceptoservicio.GetIndex());
        }

        public ActionResult IndexPager(Pager pager, ConceptosVista model)
        {
            IConceptosVista vista = _conceptoservicio.GetConceptos(pager, model.AñoBusqueda, 
                model.MesBusqueda, model.ObservacionBusqueda);

            TempData["Model"] = vista;
            return RedirectToAction("PageRedirect");
        }

        public ActionResult PageRedirect()
        {
            return View("Index", (ConceptosVista)TempData["Model"]);
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmConceptos, Accion = Enums.Acciones.Agregar)]
        public ActionResult Formulario()
        {
            IConceptoVista vista = _conceptoservicio.GetProximoConcepto();
            vista.Accion = "Nuevo";

            return View("Formulario", vista);
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmConceptos, Accion = Enums.Acciones.Agregar)]
        public ActionResult Agregar(ConceptoVista modelo)
        {
            int mconc = _conceptoservicio.AddConcepto(modelo);
            
            return View("Index", _conceptoservicio.GetConceptos());
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmConceptos, Accion = Enums.Acciones.Consultar)]
        public ActionResult Ver(int id_concepto)
        {
            IConceptoVista vista = _conceptoservicio.GetConcepto(id_concepto, "Ver");
            
            return View("Formulario", vista);
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmConceptos, Accion = Enums.Acciones.Modificar)]
        public ActionResult Modificar(int id_concepto)
        {
            IConceptoVista vista = _conceptoservicio.GetConcepto(id_concepto, "Modificar");
            
            return View("Formulario", vista);
        }

        [HttpPost]
        public ActionResult ModificarConcepto(ConceptoVista modelo)
        {
            bool mret = _conceptoservicio.UpdateConcepto(modelo);

            return View("Index", _conceptoservicio.GetConceptos());
        }
    }
}
