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

namespace SGE.Web.Controllers
{
    [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmBeneficiario)]
    public class BeneficiariosController : BaseController
    {
        private readonly IBeneficiarioServicio _beneficiarioServicio;
        private readonly EtapaServicio _etapaServicio;

        public BeneficiariosController()
            : base((int)Formularios.Beneficiario)
        {
            _beneficiarioServicio = new BeneficiarioServicio();
            _etapaServicio = new EtapaServicio();

        }

        public ActionResult Index()
        {

            return View("Cuentas",
                        _beneficiarioServicio.GetBeneficiariosIndex());
        }

        public ActionResult IndexPager(Pager pager, BeneficiariosVista model)
        {
            int idprograma = 0;
            idprograma = Convert.ToInt32(model.Programas.Selected);
            if (model.Programas.Selected == "0")
            {
                if (model.EtapaProgramas.Selected == "3")
                {
                    idprograma = Convert.ToInt32(model.EtapaProgramas.Selected);
                }
            }

            IBeneficiariosVista vista;
            IBeneficiariosVista vistaTemp;
            vistaTemp = (IBeneficiariosVista)TempData["BeneficiariosVista"];
            
            switch (model.ConCuenta)
            {
                case "S":
                    vista = _beneficiarioServicio.GetBeneficiarioConCuenta(pager, model.FileDownload, model.FileNombre,
                                                                          model.NombreBusqueda, model.ApellidoBusqueda,
                                                                          model.CuilBusqueda,
                                                                          model.NumeroDocumentoBusqueda,
                                                                          idprograma,
                                                                          model.ConApoderado, model.ConModalidad,
                                                                          model.ConDiscapacidad, model.ConAltaTemprana,
                                                                          Convert.ToInt32(
                                                                              model.EstadosBeneficiarios.Selected), model.ApellidoNombreApoderadoBusqueda, Convert.ToInt32(
                                                                              model.etapas.Selected), Convert.ToInt32(model.idSubprograma), Convert.ToInt32(model.ComboTiposPpp.Selected), model.nrotramite_busqueda);

                    break;
                case "N":
                    vista =
                        _beneficiarioServicio.GetBeneficiarioSinCuenta(pager, model.FileDownload, model.FileNombre,
                                                                      model.NombreBusqueda, model.ApellidoBusqueda,
                                                                      model.CuilBusqueda,
                                                                      model.NumeroDocumentoBusqueda,
                                                                      idprograma,
                                                                      model.ConApoderado, model.ConModalidad,
                                                                      model.ConDiscapacidad, model.ConAltaTemprana,
                                                                      Convert.ToInt32(
                                                                          model.EstadosBeneficiarios.Selected),
                                                                      model.ApellidoNombreApoderadoBusqueda, Convert.ToInt32(model.etapas.Selected), Convert.ToInt32(model.idSubprograma), Convert.ToInt32(model.ComboTiposPpp.Selected), model.nrotramite_busqueda);
                    break;
                default:

                    vista = _beneficiarioServicio.GetBeneficiarios(pager,
                                                                  model.NombreBusqueda, model.ApellidoBusqueda,
                                                                  model.CuilBusqueda,
                                                                  model.NumeroDocumentoBusqueda,
                                                                  idprograma,
                                                                  model.ConApoderado, model.ConModalidad,
                                                                  model.ConDiscapacidad, model.ConAltaTemprana,
                                                                  Convert.ToInt32(model.EstadosBeneficiarios.Selected),
                                                                  model.ApellidoNombreApoderadoBusqueda, Convert.ToInt32(model.etapas.Selected), Convert.ToInt32(model.idSubprograma), Convert.ToInt32(model.ComboTiposPpp.Selected), model.nrotramite_busqueda);
                    break;
            }
            vista.idBeneficiarios = vistaTemp.idBeneficiarios;

            // 23/05/2013 - DI CAMPLI LEANDRO - RECARGAR NUEVOS FILTROS DE BUSQUEDA

            //vista.EtapaProgramas = vista.Programas;
            vista.programaSel =  Convert.ToInt32(model.EtapaProgramas.Selected);// model.programaSel;
            vista.ejerc = model.ejerc;
            model.filtroEjerc = model.ejerc;
            vista.filtroEjerc = model.ejerc;
            vista.BuscarPorEtapa = model.BuscarPorEtapa;
            if (model.BuscarPorEtapa == "S")
            {
                vista.etapas = _etapaServicio.getEtapasCombo(model.programaSel, model.filtroEjerc);
                vista.etapas.Selected = model.etapas.Selected;
            }
            vista.EtapaProgramas.Selected = model.programaSel.ToString();
            vista.etapas.Selected = model.etapas.Selected;
            vista.idSubprograma = model.idSubprograma;
            vista.idFormulario = "FormIndexBeneficiarios";
            //vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = 0, Description = "Todos" });
            //vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = 1, Description = "PRIMER PASO" });
            //vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = 2, Description = "APRENDIZ" });
            //vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = 3, Description = "XMÍ" });
            //vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = 4, Description = "PILA" });
            //vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = 5, Description = "PIP" });
            //vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = 6, Description = "CLIP" });
            
            vista.ComboTiposPpp.Selected = model.ComboTiposPpp.Selected;

            // 13/03/2018+++++++++++
            vista.CuilBusqueda = model.CuilBusqueda;
            vista.NumeroDocumentoBusqueda = model.NumeroDocumentoBusqueda;
            vista.NombreBusqueda = model.NombreBusqueda;
            vista.ApellidoBusqueda = model.ApellidoBusqueda;
            vista.nrotramite_busqueda = model.nrotramite_busqueda;

            //+++++++++++++++

            TempData["VistaRetorno"] = vista;



            return RedirectToAction("PageRedirect");
        }

        public ActionResult PageRedirect()
        {
            return View("Cuentas", (BeneficiariosVista)TempData["VistaRetorno"]);
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmBeneficiario, Accion = Enums.Acciones.Modificar)]
        public ActionResult Modificar(int idBeneficiario)
        {
            ViewBag.Title = "Modificar Beneficiario";
            IBeneficiarioVista model = _beneficiarioServicio.GetBeneficiario(idBeneficiario);
            model.Accion = "Modificar";
            model.Monedas.Enabled = false;
            
            return View("Formulario", model);
        }


        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmBeneficiario, Accion = Enums.Acciones.Consultar)]
        public ActionResult Ver(int idBeneficiario)
        {
            ViewBag.Title = "Ver Beneficiario";
            IBeneficiarioVista model = _beneficiarioServicio.GetBeneficiario(idBeneficiario);
            model.Sucursales.Enabled = false;
            
            return View("Formulario", model);
        }

        [HttpPost]
        public ActionResult BuscarBeneficiario(BeneficiariosVista model)
        {
            int idprograma = 0;
            idprograma = Convert.ToInt32(model.Programas.Selected);
            if (model.Programas.Selected == "0")
            {
                if (model.EtapaProgramas.Selected == "3")
                {
                    idprograma = Convert.ToInt32(model.EtapaProgramas.Selected);
                }
            }

            IBeneficiariosVista vista;
            switch (model.ConCuenta)
            {
                case "S": //26/02/20013 - LEANDRO DI CAMPLI - GetBeneficiarioConCuenta POR GetBeneficiarioConCuentaBis
                    vista = _beneficiarioServicio.GetBeneficiarioConCuenta(model.NombreBusqueda, model.ApellidoBusqueda,
                                                                          model.CuilBusqueda,
                                                                          model.NumeroDocumentoBusqueda,
                                                                          idprograma,
                                                                          model.ConApoderado, model.ConModalidad,
                                                                          model.ConDiscapacidad, model.ConAltaTemprana,
                                                                          Convert.ToInt32(
                                                                              model.EstadosBeneficiarios.Selected),
                                                                          model.ApellidoNombreApoderadoBusqueda, Convert.ToInt32(model.etapas.Selected), Convert.ToInt32(model.idSubprograma), Convert.ToInt32(model.ComboTiposPpp.Selected), model.nrotramite_busqueda);
                    break;
                case "N":
                    vista = _beneficiarioServicio.GetBeneficiarioSinCuenta(model.NombreBusqueda, model.ApellidoBusqueda,
                                                                          model.CuilBusqueda,
                                                                          model.NumeroDocumentoBusqueda,
                                                                          idprograma,
                                                                          model.ConApoderado, model.ConModalidad,
                                                                          model.ConDiscapacidad, model.ConAltaTemprana,
                                                                          Convert.ToInt32(
                                                                              model.EstadosBeneficiarios.Selected),
                                                                          model.ApellidoNombreApoderadoBusqueda, Convert.ToInt32(model.etapas.Selected), Convert.ToInt32(model.idSubprograma), Convert.ToInt32(model.ComboTiposPpp.Selected), model.nrotramite_busqueda);
                    break;
                default:

                    vista = _beneficiarioServicio.GetBeneficiarios(model.NombreBusqueda, model.ApellidoBusqueda,
                                                                  model.CuilBusqueda,
                                                                  model.NumeroDocumentoBusqueda,
                                                                  idprograma,
                                                                  model.ConApoderado, model.ConModalidad,
                                                                  model.ConDiscapacidad, model.ConAltaTemprana,
                                                                  Convert.ToInt32(model.EstadosBeneficiarios.Selected),
                                                                  model.ApellidoNombreApoderadoBusqueda, Convert.ToInt32(model.etapas.Selected), Convert.ToInt32(model.idSubprograma), Convert.ToInt32(model.ComboTiposPpp.Selected), model.nrotramite_busqueda);
                    break;
            }
            vista.Programas.Selected = model.Programas.Selected;
            vista.ErrorDatos = false;


            // 22/05/2013 - DI CAMPLI LEANDRO - RECARGAR NUEVOS FILTROS DE BUSQUEDA

            //vista.EtapaProgramas =
            vista.programaSel = Convert.ToInt32(model.EtapaProgramas.Selected);//model.programaSel;
            vista.BuscarPorEtapa = model.BuscarPorEtapa;
                        if (model.BuscarPorEtapa == "S")
                        {
                            vista.etapas = _etapaServicio.getEtapasCombo(model.programaSel, model.filtroEjerc);
                            vista.etapas.Selected = model.etapas.Selected;
                        }
                vista.EtapaProgramas.Selected = model.programaSel.ToString();
            
            vista.ejerc = model.ejerc;
            vista.idFormulario = "FormIndexBeneficiarios";
            //vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = 0, Description = "Todos" });
            //vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = 1, Description = "PRIMER PASO" });
            //vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = 2, Description = "APRENDIZ" });
            //vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = 3, Description = "XMÍ" });
            //vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = 4, Description = "PILA" });
            //vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = 5, Description = "PIP" });
            //vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = 6, Description = "CLIP" });
            vista.ComboTiposPpp.Selected = model.ComboTiposPpp.Selected;

            // 13/03/2018+++++++++++
            vista.CuilBusqueda = model.CuilBusqueda;
            vista.NumeroDocumentoBusqueda = model.NumeroDocumentoBusqueda;
            vista.NombreBusqueda = model.NombreBusqueda;
            vista.ApellidoBusqueda = model.ApellidoBusqueda;
            vista.nrotramite_busqueda = model.nrotramite_busqueda;

            //+++++++++++++++

            return View("Cuentas", vista);
        }

        
        //public ActionResult GenerarFile(string NombreBusqueda, string ApellidoBusqueda, string CuilBusqueda,
        //                                             string NumeroDocumentoBusqueda, int Programas, string ConApoderado, string ConModalidad, string ConDiscapacidad, string ConAltaTemprana, int EstadosBeneficiarios)
        [HttpPost]
        public ActionResult GenerarFile(BeneficiariosVista model)
        {
            //BeneficiariosVista model = new BeneficiariosVista();

            //model.NombreBusqueda=NombreBusqueda;
            //model.ApellidoBusqueda = ApellidoBusqueda;
            //model.CuilBusqueda=CuilBusqueda;
            //model.NumeroDocumentoBusqueda=NumeroDocumentoBusqueda;
            //model.Programas.Selected=Programas.ToString();
            //model.ConApoderado=ConApoderado;
            //model.ConModalidad=ConModalidad;
            //model.ConDiscapacidad=ConDiscapacidad;
            //model.ConAltaTemprana=ConAltaTemprana;
            //model.EstadosBeneficiarios.Selected=EstadosBeneficiarios.ToString();

            IBeneficiariosVista vista = _beneficiarioServicio.GetBeneficiarioSinCuentaFile(model.NombreBusqueda,
                                                                                          model.ApellidoBusqueda,
                                                                                          model.CuilBusqueda,
                                                                                          model.NumeroDocumentoBusqueda,
                                                                                          Convert.ToInt32(
                                                                                              model.Programas.Selected),
                                                                                          model.ConApoderado,
                                                                                          model.ConModalidad,
                                                                                          model.ConDiscapacidad,
                                                                                          model.ConAltaTemprana,
                                                                                          Convert.ToInt32(
                                                                                              model.EstadosBeneficiarios
                                                                                                  .Selected),Convert.ToInt32(
                                                                                              model.idSubprograma), Convert.ToInt32(
                                                                                              model.ComboTiposPpp.Selected), model.nrotramite_busqueda);

            if (vista.ErrorDatos == false)
            {
                if (vista.FileNombre!="")
                {
                    return File(vista.FileDownload, "text/plain", vista.FileNombre);
                }
            }
            
            vista = _beneficiarioServicio.GetBeneficiarioSinCuenta(model.NombreBusqueda, model.ApellidoBusqueda,
                                                                   model.CuilBusqueda,
                                                                   model.NumeroDocumentoBusqueda,
                                                                   Convert.ToInt32(model.Programas.Selected),
                                                                   model.ConApoderado, model.ConModalidad,
                                                                   model.ConDiscapacidad, model.ConAltaTemprana,
                                                                   Convert.ToInt32(
                                                                       model.EstadosBeneficiarios.Selected),
                                                                   model.ApellidoNombreApoderadoBusqueda, Convert.ToInt32(model.etapas.Selected), Convert.ToInt32(model.idSubprograma), Convert.ToInt32(model.ComboTiposPpp.Selected), model.nrotramite_busqueda);

            vista.ErrorDatos = true;
            TempData["DatoVista"] = vista;
            
            return RedirectToAction("MostrarErrorDatos");
        }

        public ActionResult MostrarErrorDatos()
        {
            return View("Cuentas", (BeneficiariosVista)TempData["DatoVista"]);
        }

        [HttpPost]
        public ActionResult ModificarBeneficiario(BeneficiarioVista model)
        {
            if (!ModelState.IsValid)
            {
                return View("Formulario", _beneficiarioServicio.GetBeneficiarioForError(model));
            }

            if (_beneficiarioServicio.UpdateBeneficiario(model))
            {
                ViewBag.Title = "Modificar Beneficiario";
                IBeneficiarioVista vista = _beneficiarioServicio.GetBeneficiario(model.IdBeneficiario);
                vista.SeGuardo = true;
                vista.Accion = "Modificar";
                TempData["Beneficiario"] = vista;
                
                return RedirectToAction("MostrarCambio");
            }
            
            ModelState.AddModelError(string.Empty, "Surgió un error verifique los Datos.");
            TempData["Beneficiario"] = model;
            
            return RedirectToAction("MostrarCambio");
        }

        public ActionResult MostrarCambio()
        {
            return View("Formulario", (BeneficiarioVista)TempData["Beneficiario"]);
        }

        public ActionResult Importar()
        {
            IImportarCuentasVista vista = new ImportarCuentasVista();
            
            return View(vista);
        }

        [HttpPost]
        public ActionResult ImportarCuentas(HttpPostedFileBase archivo)
        {
            return View("Importar", _beneficiarioServicio.ImportarCuentas(archivo));
        }

        [HttpPost]
        public ActionResult CargarCuentas(ImportarCuentasVista model)
        {
            return View("ResultadoImportacion", _beneficiarioServicio.CargarCuentas(model.Archivo));
        }

        [HttpPost]
        public ActionResult LimpiarFechaSolucitud(BeneficiarioVista model)
        {
            ViewBag.Title = "Modificar Beneficiario";
            IBeneficiarioVista vista = _beneficiarioServicio.ClearFechaSolicitud(model.IdBeneficiario);
            vista.Accion = "Modificar";
            vista.Sucursales.Selected = model.Sucursales.Selected;
            ModelState.Clear();
            
            return View("Formulario", vista);
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmBeneficiario, Accion = Enums.Acciones.VerArchivo)]
        public ActionResult ArchivosGenerados()
        {
            return View(_beneficiarioServicio.GetArchivos());
        }

        public ActionResult LimpiarFecha(ArchivosVista model)
        {

            if (_beneficiarioServicio.ClearFechaSolicitudArchivobyFecha(model.FechaLimpieza))
            {
                return View("ArchivosGenerados", _beneficiarioServicio.GetArchivos());
            }

            ModelState.Clear();
            base.AddError(_beneficiarioServicio.GetErrores());
            
            return View("ArchivosGenerados", _beneficiarioServicio.GetArchivos());
        }

        public ActionResult VolveraGenerarArchivo(DateTime fecha)
        {
            IArchivosVista vista = _beneficiarioServicio.VolverGenerarArchivo(fecha);

            return File(vista.Url, "text/plain", vista.Nombre);
        }

        public ActionResult ReporteArt()
        {
            return File(_beneficiarioServicio.ReporteBeneficiarioArt(), "application/pdf",
                        "NomimaBeneficiarioART_" + DateTime.Now.ToShortDateString() + ".pdf");
        }

        public ActionResult ReporteNomina()
        {
            return File(_beneficiarioServicio.ReporteBeneficiarioNomina(), "application/pdf",
                        "NomimaBeneficiario_" + DateTime.Now.ToShortDateString() + ".pdf");
        }

        public ActionResult ReporteArtPpp(BeneficiariosVista model)
        {
            string nombrearchivo = "HorariosEntrenamientoART-Ppp_" + DateTime.Now.ToShortDateString() + ".xls"; 

            return File(_beneficiarioServicio.ExportReporteArtHorariosEntrenamientoPpp(model),
                        "application/vnd.ms-excel", nombrearchivo);
        }

        public ActionResult ReporteArtReconversion(BeneficiariosVista model)
        {
            string nombrearchivo = "HorariosEntrenamientoART-ReconversionProductiva_" + DateTime.Now.ToShortDateString() + ".xls";

            return File(_beneficiarioServicio.ExportReporteArtHorariosEntrenamientoReconversion(model),
                        "application/vnd.ms-excel", nombrearchivo);
        }

        public ActionResult ReporteArtEfectores(BeneficiariosVista model)
        {
            string nombrearchivo = "HorariosEntrenamientoART-EfectoresSociales_" + DateTime.Now.ToShortDateString() + ".xls";

            return File(_beneficiarioServicio.ExportReporteArtHorariosEntrenamientoEfectores(model),
                        "application/vnd.ms-excel", nombrearchivo);
        }

        public ActionResult ReporteArtVat(BeneficiariosVista model)
        {
            string nombrearchivo = "HorariosEntrenamientoART-Vat_" + DateTime.Now.ToShortDateString() + ".xls";

            return File(_beneficiarioServicio.ExportReporteArtHorariosEntrenamientoVat(model),
                        "application/vnd.ms-excel", nombrearchivo);
        }

        public ActionResult ReporteArtPppp(BeneficiariosVista model)
        {
            string nombrearchivo = "HorariosEntrenamientoART-PppProf_" + DateTime.Now.ToShortDateString() + ".xls";

            return File(_beneficiarioServicio.ExportReporteArtHorariosEntrenamientoPppp(model),
                        "application/vnd.ms-excel", nombrearchivo);
        }

        public ActionResult ReporteArtConfVos(BeneficiariosVista model)
        {
            string nombrearchivo = "HorariosEntrenamientoART-ConfVos_" + DateTime.Now.ToShortDateString() + ".xls";

            return File(_beneficiarioServicio.ExportReporteArtHorariosEntrenamientoConfVos(model),
                        "application/vnd.ms-excel", nombrearchivo);
        }

        public ActionResult ExportBeneficiario(BeneficiariosVista model)
        {
            string nombrearchivo;

            switch (Convert.ToInt32(model.Programas.Selected))
            {
                case (int)Enums.TipoFicha.Terciaria:
                    nombrearchivo = "BeneficiariosTerciarios_" + DateTime.Now.ToShortDateString() + ".xls";
                    break;
                case (int)Enums.TipoFicha.Universitaria:
                    nombrearchivo = "BeneficiariosUniversitarios_" + DateTime.Now.ToShortDateString() + ".xls";
                    break;
                case (int)Enums.TipoFicha.Ppp:
                    nombrearchivo = "BeneficiariosPPP_" + DateTime.Now.ToShortDateString() + ".xls";
                    break;
                case (int)Enums.TipoFicha.PppProf:
                    nombrearchivo = "BeneficiariosPppProf_" + DateTime.Now.ToShortDateString() + ".xls";
                    break;
                case (int)Enums.TipoFicha.Vat:
                    nombrearchivo = "BeneficiariosVAT_" + DateTime.Now.ToShortDateString() + ".xls";
                    break;
                case (int)Enums.TipoFicha.ConfiamosEnVos:
                    nombrearchivo = "BeneficiariosConfVos_" + DateTime.Now.ToShortDateString() + ".xls";
                    break;
                default:
                    nombrearchivo = "Beneficiario_" + DateTime.Now.ToShortDateString() + ".xls";
                    break;

            }

            return File(_beneficiarioServicio.ExportBeneficiario(model),
                        "application/vnd.ms-excel", nombrearchivo);
        }

        public ActionResult prueba()
        { 
            //Reportes.ClienteModel m = new Reportes.ClienteModel();
            //var pepe = m.GetAllCustomer();

            return View("Cuentas");
        }


        // 26/02/2013 - LEANDRO DI CAMPLI - MEJORA DE RENDIMIENTO PARA ARMAR EXCEL
        [HttpPost]
        public ActionResult BeneficiariosXls(BeneficiariosVista model)
        {
            string nombrearchivo;

            //model = (BeneficiariosVista)TempData["BeneficiariosVista"];

            switch (Convert.ToInt32(model.Programas.Selected))
            {
                case (int)Enums.TipoFicha.Terciaria:
                    nombrearchivo = "BeneficiariosTerciarios_" + DateTime.Now.ToShortDateString() + ".xls";
                    return File(_beneficiarioServicio.ExportTerciario(model), "application/vnd.ms-excel", nombrearchivo);
                    
                case (int)Enums.TipoFicha.Universitaria:
                    nombrearchivo = "BeneficiariosUniversitarios_" + DateTime.Now.ToShortDateString() + ".xls";
                    return File(_beneficiarioServicio.ExportUniversitario(model), "application/vnd.ms-excel", nombrearchivo);
                    
                case (int)Enums.TipoFicha.Ppp:
                    nombrearchivo = "BeneficiariosPPP_" + DateTime.Now.ToShortDateString() + ".xls";

                    return File(_beneficiarioServicio.ExportPPP(model),"application/vnd.ms-excel", nombrearchivo);

                    //break;
                case (int)Enums.TipoFicha.PppProf:
                    nombrearchivo = "BeneficiariosPppProf_" + DateTime.Now.ToShortDateString() + ".xls";
                    return File(_beneficiarioServicio.ExportPPPprof(model), "application/vnd.ms-excel", nombrearchivo);

                case (int)Enums.TipoFicha.Vat:
                    nombrearchivo = "BeneficiariosVAT_" + DateTime.Now.ToShortDateString() + ".xls";
                    return File(_beneficiarioServicio.ExportVat(model), "application/vnd.ms-excel", nombrearchivo);

                case (int)Enums.TipoFicha.ReconversionProductiva:
                    nombrearchivo = "BeneficiariosRec_Prod_" + DateTime.Now.ToShortDateString() + ".xls";
                    return File(_beneficiarioServicio.ExportRec_Prod(model), "application/vnd.ms-excel", nombrearchivo);

                case (int)Enums.TipoFicha.EfectoresSociales:
                    nombrearchivo = "BeneficiariosEfe_Soc_" + DateTime.Now.ToShortDateString() + ".xls";
                    return File(_beneficiarioServicio.ExportEfec_Soc(model), "application/vnd.ms-excel", nombrearchivo);

                case (int)Enums.TipoFicha.ConfiamosEnVos:
                    nombrearchivo = "BeneficiariosConf_Vos_" + DateTime.Now.ToShortDateString() + ".xls";
                    return File(_beneficiarioServicio.ExportConf_Vos(model), "application/vnd.ms-excel", nombrearchivo);

                default:
                    nombrearchivo = "Beneficiario_" + DateTime.Now.ToShortDateString() + ".xls";
                    break;

            }

            return File(_beneficiarioServicio.ExportBeneficiario(model),
                        "application/vnd.ms-excel", nombrearchivo);
        }

        [HttpPost]
        public ActionResult IndexTraerEtapas(BeneficiariosVista model)
        {
            IBeneficiariosVista vista;
            //EtapasVista etapa = new EtapasVista();
            vista = _beneficiarioServicio.GetBeneficiariosIndex();
            vista.programaSel = model.programaSel;
            vista.filtroEjerc = model.filtroEjerc;
            model = (BeneficiariosVista)vista;
            model.BuscarPorEtapa = "S";
            model.etapas = _etapaServicio.getEtapasCombo(model.programaSel, model.filtroEjerc);
            model.idFormulario = "FormIndexBeneficiarios";

            return View("Cuentas",
                        model);
        }


        public ActionResult RptBeneficiarioHorarios(BeneficiariosVista model)
        {
            string nombrearchivo = "BeneficiariosHorarios_" + DateTime.Now.ToShortDateString() + ".xls";

            return File(_beneficiarioServicio.ExportBeneficiarioHorarios(model),
                        "application/vnd.ms-excel", nombrearchivo);
        }



    }
}
