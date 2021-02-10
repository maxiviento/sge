using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SGE.Model.Comun;
using SGE.Servicio.Servicio;
using SGE.Servicio.Vista;
using SGE.Servicio.VistaInterfaces;
using SGE.Web.Controllers.Shared;
using SGE.Web.Infrastucture;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.ServicioInterfaces;

namespace SGE.Web.Controllers
{

    [CustomAuthorize(Formulario = Enums.Formulario.RepDepLocProEst)]
    public class ReportesController : BaseController
    {
        private readonly ReporteServicio _reporteServicio;
        private readonly EtapaServicio _etapaServicio;
        private readonly IBeneficiarioServicio _beneficiarioServicio;
        public ReportesController()
            : base((int)Formularios.FichaObservacion)
        {
            _reporteServicio = new ReporteServicio();
            _etapaServicio = new EtapaServicio();
            _beneficiarioServicio = new BeneficiarioServicio();
        }

        public ActionResult Index()
        {

            return View();
        }

        public ActionResult IndexRDepLocProgEst()
        {
            return View("IndexRDepLocProEst", _reporteServicio.IndexRepDepLocProgEst());
        }

        public ActionResult CambiarComboRDepLocProgEst(ReporteDepLocProgEsta model)
        {
            IReporteDepLocProgEsta vista = _reporteServicio.GetReporteDepLocProgEstaChangeDep(model);
            return View("IndexRDepLocProEst", vista);
        }

        [HttpPost]
        public ActionResult GenerarRDepLocProgEst(ReporteDepLocProgEsta model)
        {
            IReporteDepLocProgEsta vista = _reporteServicio.GetReporteDepLocProgEsta(model);
            return View("IndexRDepLocProEst", vista);
        }

        [HttpPost]
        public ActionResult ExportarRDepLocProgEst(ReporteDepLocProgEsta model)
        {
            string nombrearchivo = "Beneficiarios_" + DateTime.Now.ToShortDateString() + ".xls"; ;

            return File(_reporteServicio.ExportReporteCantidades(model),
                        "application/vnd.ms-excel", nombrearchivo);
        }

        public ActionResult ExportarRDepLocProgEst(string departamento, string localidad, string programa, int idDepartamento, int idPrograma, int idLocalidad, string estado, int estadoBenef, int tipoprograma)
        {
            string dpto = departamento==null ? String.Empty :departamento.ToUpper();
            string loc = localidad == null ? String.Empty : localidad.ToUpper();
            string prog = programa == null ? String.Empty : programa.ToUpper();

            string nombrearchivo = "Beneficiarios_" + "_" + dpto + "_" + loc+ "_" + prog + "_" + DateTime.Now.ToShortDateString() + ".xls"; ;

            return File(_reporteServicio.ExportReporteDetalle(idDepartamento, idPrograma, idLocalidad, estado, estadoBenef, tipoprograma),
                        "application/vnd.ms-excel", nombrearchivo);
        }


        // 19/06/2013 - DI CAMPLI LEANDRO - AGREGAR FILTROS A REPORTES ART
        public ActionResult ReportesArt()
        {
            TempData["VerEfectores"] = _reporteServicio.AccesoPrograma(Enums.Formulario.AbmBeneficiario);
            return View("ReportesArt");
        }

        //***************** 03/08/2015 Reporte de asistencias******************
        public ActionResult AsistenciasRpt()
        {
            return View(_reporteServicio.GetIndexAsistenciasRpt());
        }


        public ActionResult IndexPagerAsistenciaRpt(Pager pager, AsistenciaRptVista model)
        {

            IAsistenciaRptVista vista = _reporteServicio.GetAsistenciasRpt(pager, model.cboperiodos.Selected, model.idCurso, model.idEscuela, Convert.ToInt32(model.EstadosBeneficiarios.Selected ?? "0"), Convert.ToInt32(model.etapas.Selected ?? "0")); ;

            vista.programaSel = Convert.ToInt32(model.EtapaProgramas.Selected);// model.programaSel;
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



            TempData["AsistenciaRpt"] = vista;
            return RedirectToAction("PageRedirect");
        }

        public ActionResult PageRedirect()
        {
            return View("AsistenciasRpt", (AsistenciaRptVista)TempData["AsistenciaRpt"]);
        }

        [HttpPost]
        public ActionResult BuscarAsistencias(AsistenciaRptVista model)
        {

            IAsistenciaRptVista vista = _reporteServicio.GetAsistenciasRpt(model.cboperiodos.Selected, model.idCurso, model.idEscuela, Convert.ToInt32(model.EstadosBeneficiarios.Selected ?? "0"), Convert.ToInt32(model.etapas.Selected ?? "0"));

            vista.programaSel = Convert.ToInt32(model.EtapaProgramas.Selected);// model.programaSel;
            vista.ejerc = model.ejerc;
            model.filtroEjerc = model.ejerc;
            vista.filtroEjerc = model.ejerc;
            if (model.BuscarPorEtapa == "S")
            {
                vista.etapas = _etapaServicio.getEtapasCombo(model.programaSel, model.filtroEjerc);
                vista.etapas.Selected = model.etapas.Selected;
            }
            vista.EtapaProgramas.Selected = model.programaSel.ToString();
            vista.BuscarPorEtapa = model.BuscarPorEtapa;

            TempData["AsistenciaRpt"] = vista;


            return View("AsistenciasRpt", vista);
        }


        public ActionResult AsistenciasXls(AsistenciaRptVista model)
        {
            string nombrearchivo;

            nombrearchivo = "Asistencias_" + DateTime.Now.ToShortDateString() + ".xls";

            return File(_reporteServicio.ExportarAsistencias(model.cboperiodos.Selected, model.idCurso, model.idEscuela, Convert.ToInt32(model.EstadosBeneficiarios.Selected ?? "0"), Convert.ToInt32(model.etapas.Selected ?? "0")), "application/vnd.ms-excel", nombrearchivo);
        }

        [HttpPost]
        public ActionResult IndexTraerEtapas(AsistenciaRptVista model)
        {
            IAsistenciaRptVista vista;
            //EtapasVista etapa = new EtapasVista();
            vista = _reporteServicio.GetIndexAsistenciasRpt();//_beneficiarioServicio.GetBeneficiariosIndex();
            vista.programaSel = model.programaSel;
            vista.filtroEjerc = model.filtroEjerc;
            model = (AsistenciaRptVista)vista;
            model.BuscarPorEtapa = "S";
            model.etapas = _etapaServicio.getEtapasCombo(model.programaSel, model.filtroEjerc);

            return View("AsistenciasRpt",
                        model);
        }
        //***********03/08/2015 Reporte de asistencias ***********************




        public ActionResult BeneficiarioHorario()
        {
            IBeneficiariosVista vista;
            //EtapasVista etapa = new EtapasVista();
            vista = _beneficiarioServicio.GetBeneficiarioHorarios();
            vista.ComboTiposPpp.Selected = vista.ComboTiposPpp.Selected;

            return View("RptBeneficiarioHorarios",
                        vista);
        }


        public ActionResult RptBeneficiarioHorarios(BeneficiariosVista model)
        {
            IBeneficiariosVista vista;
            //EtapasVista etapa = new EtapasVista();
            vista = _beneficiarioServicio.GetBeneficiarioHorarios();
            vista.ComboTiposPpp.Selected = model.ComboTiposPpp.Selected;

            return View("RptBeneficiarioHorarios",
                        model);
        }

    }
}
