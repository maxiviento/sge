using System;
using System.Web;
using System.Web.Mvc;
using SGE.Model.Comun;
using SGE.Servicio.Servicio;
using SGE.Servicio.ServicioInterfaces;
using SGE.Servicio.Vista;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces;
using SGE.Web.Controllers.Shared;
using SGE.Web.Infrastucture;
using System.Text;
using System.IO;

namespace SGE.Web.Controllers
{
    [CustomAuthorizeAttribute(Formulario = Enums.Formulario.Liquidaciones)]
    public class LiquidacionController : BaseController
    {
        private readonly ILiquidacionServicio _liquidacionServicio;
        private readonly IProgramaServicio _programaServicio;


        public LiquidacionController()
            : base((int)Formularios.Liquidacion)
        {
            _liquidacionServicio = new LiquidacionServicio();
            _programaServicio = new ProgramaServicio();
        }

        public ActionResult Index()
        {
            return View(_liquidacionServicio.GetLiquidacionesIndex());
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.Liquidaciones, Accion = Enums.Acciones.Consultar)]
        public ActionResult Ver(int idLiquidacion)
        {
            return View("FormularioBeneficiarios", _liquidacionServicio.GetLiquidacion(idLiquidacion, "Ver"));
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.Liquidaciones, Accion = Enums.Acciones.Modificar)]
        public ActionResult CambiarEstado(int idLiquidacion)
        {
            return View("FormularioBeneficiarios", _liquidacionServicio.GetLiquidacion(idLiquidacion, "CambiarEstado"));
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.Liquidaciones, Accion = Enums.Acciones.Modificar)]
        public ActionResult Modificar(int idLiquidacion)
        {
            return View("FormularioBeneficiarios", _liquidacionServicio.GetLiquidacion(idLiquidacion, "Modificar"));
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.Liquidaciones, Accion = Enums.Acciones.Agregar)]
        public ActionResult Agregar()
        {
            return View("Formulario", _liquidacionServicio.GetLiquidacion());
        }

        [HttpPost]
        public ActionResult BuscarLiquidaciones(LiquidacionesVista model)
        {
            return View("Index",
                        _liquidacionServicio.GetLiquidaciones(model.From, model.To, model.NroResolución ?? 0,
                                                              Convert.ToInt32(model.Estados.Selected),
                                                              Convert.ToInt32(model.Programas.Selected)));
        }

        public ActionResult ReporteAnexo1(LiquidacionVista model, int idPrograma)
        {
            ILiquidacionVista vista = (LiquidacionVista)TempData["Vista"];

            model.Liquidacion.IdPrograma = idPrograma;
        
            return File(_liquidacionServicio.ReporteAnexo1Resolucion(model), "application/pdf",
                        "Anexo1Resolucion_" + DateTime.Now.ToShortDateString() + ".pdf");
        }

        public ActionResult IndexPager(Pager pager, LiquidacionesVista model)
        {
            ILiquidacionesVista vista = _liquidacionServicio.GetLiquidaciones(pager, model.From, model.To,
                                                                              model.NroResolución ?? 0,
                                                                              Convert.ToInt32(model.Estados.Selected),
                                                                              Convert.ToInt32(model.Programas.Selected));

            TempData["VistaRetorno"] = vista;

            return RedirectToAction("PageRedirect");

        }

        public ActionResult PageRedirect()
        {
            if (TempData["VistaRetorno"] != null)
            {
                return View("Index", (LiquidacionesVista)TempData["VistaRetorno"]);
            }
            else
            {
                return View("Index",
                            _liquidacionServicio.GetLiquidaciones(DateTime.Now, DateTime.Now.AddDays(-15),
                                                                  0, 0, 0));
            }
        }

        [HttpPost]
        public ActionResult BuscarBeneficiario(LiquidacionVista model)
        {
            ILiquidacionVista vista = (LiquidacionVista)TempData["Vista"];
            vista.NumeroDocumento = model.NumeroDocumento;

            return View("FormularioBeneficiarios", _liquidacionServicio.BuscarBeneficiarioEnNomina(vista));
        }

        public ActionResult QuitarBeneficiario(int idBeneficiario)
        {
            return View("FormularioBeneficiarios",
                        _liquidacionServicio.QuitarBeneficiarioDeNomina((LiquidacionVista)TempData["Vista"],
                                                                       idBeneficiario));
        }

        public ActionResult AgregarBeneficiario(int idBeneficiario)
        {
            return View("FormularioBeneficiarios",
                        _liquidacionServicio.AgregarBeneficiarioDeNomina((LiquidacionVista)TempData["Vista"],
                                                                       idBeneficiario));
        }

        [HttpPost]
        public ActionResult CargarConceptos(LiquidacionVista model)
        {
            ILiquidacionVista vista = (LiquidacionVista)TempData["Vista"];

            //ILiquidacionVista vista = _liquidacionServicio.GetLiquidacion();
            string concepto = model.Conceptos.Selected;
            vista.Conceptos.Selected = concepto;//model.Conceptos.Selected;

            vista.Programas.Selected = model.Programas.Selected;
            vista.FechaLiquidacion = model.FechaLiquidacion;
            vista.Observacion = model.Observacion;
            vista.NroResolucion = model.NroResolucion;
            vista.Convenios.Selected = model.Convenios.Selected;

            return View("FormularioBeneficiarios", _liquidacionServicio.CargarConceptoBeneficiarios(vista));


        }

        public ActionResult VerConceptosBeneficiario(int idBeneficiario)
        {
            var model = (ILiquidacionVista)TempData["Vista"];

            return View("FormularioConceptosBeneficiario",
                        _liquidacionServicio.VerConceptosBeneficiario(model, idBeneficiario));

        }

        public ActionResult VerMontoBeneficiario(int idBeneficiario, int importeAliquidar)
        {
            var model = (ILiquidacionVista)TempData["Vista"];

            return View("FormularioModificacionMontoLiquidar",
                        _liquidacionServicio.VerMontoLiquidarBeneficiario(model, idBeneficiario, importeAliquidar));

        }
          [HttpPost]
        public ActionResult EditarMontoLiquidarBeneficiario(LiquidacionVista vista)
        {
            decimal importeModificado = 0;
            int conceptoseleccionado = Convert.ToInt32(vista.SeleccionadoBeneficiario.ComboConceptos.Selected);

            if (vista.ImportePlanEfe == 0)
                importeModificado =_programaServicio.GetPrograma((int)Enums.Programas.EfectoresSociales,"Ver").MontoPrograma;
            else
                importeModificado= vista.ImportePlanEfe;

            vista.SeleccionadoBeneficiario.MontoPrograma = importeModificado;
            var model = (ILiquidacionVista)TempData["Vista"];
            model.SeleccionadoBeneficiario.MontoPrograma = importeModificado;
            vista.SeleccionadoBeneficiario.ComboConceptos.Selected = conceptoseleccionado.ToString();

            return View("FormularioBeneficiarios",
                                    _liquidacionServicio.VolverFormBeneficiarioDesdeImporte((LiquidacionVista)TempData["Vista"]));
            //return View("FormularioModificacionMontoLiquidar", model);//, _liquidacionServicio.EditarMontoLiquidar(model));
        }

        public ActionResult QuitarConceptoBeneficiario(int idBeneficiario, int idConcepto)
        {
            var model = (ILiquidacionVista)TempData["Vista"];

            return View("FormularioConceptosBeneficiario",
                        _liquidacionServicio.QuitarConceptoBeneficiario(model, idBeneficiario, idConcepto));

        }

        public ActionResult AgregarConcepto(LiquidacionVista model)
        {

            int conceptoseleccionado = Convert.ToInt32(model.SeleccionadoBeneficiario.ComboConceptos.Selected);

            ILiquidacionVista vista = (LiquidacionVista)TempData["Vista"];

            vista.SeleccionadoBeneficiario.ComboConceptos.Selected = conceptoseleccionado.ToString();

            return View("FormularioConceptosBeneficiario", _liquidacionServicio.AgregarConceptoBeneficiario(vista));
        }

        [HttpPost]
        public ActionResult VolverFormBeneficiario(LiquidacionVista model)
        {

            return View("FormularioBeneficiarios",
                        _liquidacionServicio.VolverFormBeneficiario((LiquidacionVista)TempData["Vista"]));
        }

        [HttpPost]
        public ActionResult VolverFormBeneficiarioDesdeImporte(LiquidacionVista model)
        {

            return View("FormularioBeneficiarios",
                        _liquidacionServicio.VolverFormBeneficiarioDesdeImporte((LiquidacionVista)TempData["Vista"]));
        }

        [HttpPost]
        public ActionResult VolverFormulario(LiquidacionVista model)
        {
            var vista = (LiquidacionVista)TempData["Vista"];
            vista.SeleccionadoBeneficiario = null;
            vista.Conceptos.Enabled = true;
            return View("Formulario", vista);
        }

        public ActionResult GuardaLiquidacion(LiquidacionVista model)
        {
            int idliquidacion = model.IdLiquidacion;
            string obs = model.Observacion;
            int nroResolucion = model.NroResolucion ?? 0;
            DateTime fecha = model.FechaLiquidacion;
            model = (LiquidacionVista)TempData["Vista"];

            if (model.Accion != "Alta")
            {
                model.IdLiquidacion = idliquidacion;
                model.Observacion = obs;
                model.NroResolucion = nroResolucion;
                model.FechaLiquidacion = fecha;
            }

            if (model.Accion == "Alta")
            {
               model.IdLiquidacion =  _liquidacionServicio.AgregarLiquidacion(model);
            }
            else
            {
                _liquidacionServicio.UpdateLiquidacion(model);
            }

            if (model.NroResolucion == null || model.NroResolucion == 0)
            {
                return View("Index",
                           _liquidacionServicio.GetLiquidaciones(DateTime.Now.AddDays(30), DateTime.Now.AddDays(-30), 0, 0, 0));
            }
            else
            {
                model.GenerarFile = "Y";
                return View("FormularioBeneficiarios",
                            model);
            }
        }

        public ActionResult GenerarArchivo(LiquidacionVista model)
        {
            var vista = (LiquidacionVista)TempData["Vista"];
            string[] mreturn = _liquidacionServicio.GenerarArchivo(vista).Split(',');
            return File(mreturn[0], "text/plain", mreturn[1]);
        }

        [HttpPost]
        public ActionResult CambiarEstado(LiquidacionVista model)
        {
            _liquidacionServicio.ChangeEstadoLiquidacion(model);

            return View("Index",
                           _liquidacionServicio.GetLiquidaciones(DateTime.Now.AddDays(30), DateTime.Now.AddDays(-60), 0, 0, 0));
        }

        public ActionResult ImportarLiquidacion(int idLiquidacion)
        {
            return View("ImportarLiquidacion", _liquidacionServicio.GetLiquidacionImportacion(idLiquidacion));
        }

        [HttpPost]
        public ActionResult SubirArchivo(HttpPostedFileBase archivoliquidacion, ImportarLiquidacionVista model)
        {
            IImportarLiquidacionVista vista = _liquidacionServicio.SubirArchivo(archivoliquidacion, model.IdLiquidacion);
            
            TempData["Vista"] = vista;
            
            return RedirectToAction("ReturnSubirArchivo");
        }

        public ActionResult ReturnSubirArchivo()
        {
            return View("ImportarLiquidacion", (ImportarLiquidacionVista) TempData["Vista"]);
        }

        [HttpPost]
        public ActionResult ProcesarLiquidacion(ImportarLiquidacionVista model)
        {
            
            return View("ResultadoImportacion", _liquidacionServicio.ProcesarArchivo(model));
        }

        [HttpPost]
        public ActionResult ExportarLiquidacion(LiquidacionVista model)
        {
            model = (LiquidacionVista)TempData["Vista"];
            TempData["Vista"] = model;
            //string result1 = this.RenderActionResultToString(this.PartialView("FormularioBeneficiarios", model));
            return File(_liquidacionServicio.ExportLiquidacion(model),
                        "application/vnd.ms-excel", "Liquidacion.xls");
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.Liquidaciones, Accion = Enums.Acciones.Agregar)]
        public ActionResult RepagoLiquidacion()
        {
            return View("Formulario", _liquidacionServicio.GetLiquidacionRepagos());
        }

        [HttpPost]
        public ActionResult PantallaImporatdoraBeneficiarioRepago(LiquidacionVista model)
        {

            return View("ImportarBeneficiarioRepago", model);
        }

        [HttpPost]
        public ActionResult SubirArchivoRepago(HttpPostedFileBase archivorepago)
        {
            ILiquidacionVista vista = _liquidacionServicio.SubirArchivoRepago(archivorepago, (LiquidacionVista)TempData["Vista"]);

            TempData["VistaSubida"] = vista;

            return RedirectToAction("ReturnSubirArchivoRepago");
        }

        public ActionResult ReturnSubirArchivoRepago()
        {
            return View("ImportarBeneficiarioRepago", (LiquidacionVista)TempData["VistaSubida"]);
        }

        [HttpPost]
        public ActionResult CargarConceptosRepago(LiquidacionVista model)
        {
            ILiquidacionVista vista = (LiquidacionVista)TempData["Vista"];
            vista.Conceptos.Selected = model.Conceptos.Selected;
            vista.Programas.Selected = model.Programas.Selected;
            vista.FechaLiquidacion = model.FechaLiquidacion;
            vista.Observacion = model.Observacion;
            vista.NroResolucion = model.NroResolucion;
            vista.Convenios.Selected = model.Convenios.Selected;

            return View("FormularioBeneficiarios", _liquidacionServicio.CargarConceptoBeneficiariosRepago(vista));
        }



        /// <summary>
        /// Renders an action result to a string. This is done by creating a fake http context
        /// and response objects and have that response send the data to a string builder
        /// instead of the browser.
        /// </summary>
        /// <param name="result">The action result to be rendered to string.</param>
        /// <returns>The data rendered by the given action result.</returns>
        protected string RenderActionResultToString(ActionResult result)
        {
            // Create memory writer.
            var sb = new StringBuilder();
            var memWriter = new StringWriter(sb);

            // Create fake http context to render the view.
            var fakeResponse = new HttpResponse(memWriter);
            var fakeContext = new HttpContext(System.Web.HttpContext.Current.Request, fakeResponse);
            var fakeControllerContext = new ControllerContext(
                new HttpContextWrapper(fakeContext),
                this.ControllerContext.RouteData,
                this.ControllerContext.Controller);
            var oldContext = System.Web.HttpContext.Current;
            System.Web.HttpContext.Current = fakeContext;

            // Render the view.
            result.ExecuteResult(fakeControllerContext);

            // Restore data.
            System.Web.HttpContext.Current = oldContext;

            // Flush memory and return output.
            memWriter.Flush();
            return sb.ToString();
        }
    }
}
