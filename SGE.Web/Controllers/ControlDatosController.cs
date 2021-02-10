using System.Web.Mvc;
using SGE.Model.Comun;
using SGE.Servicio.Servicio;
using SGE.Servicio.ServicioInterfaces;
using SGE.Servicio.Vista.Shared;
using SGE.Web.Controllers.Shared;
using SGE.Web.Infrastucture;
using SGE.Servicio.Vista;
using SGE.Servicio.VistaInterfaces;
using System;

namespace SGE.Web.Controllers
{
    [CustomAuthorizeAttribute(Formulario = Enums.Formulario.ControlDatos)]
    public class ControlDatosController : BaseController
    {
        //
        // GET: /ControlDatos/

        private readonly IControlDatosServicio _controldatosservicio;
        private readonly EtapaServicio _etapaServicio;


        public ControlDatosController()
            : base((int)Enums.Formulario.ControlDatos)
        {
            _controldatosservicio = new ControlDatosServicio();
            _etapaServicio = new EtapaServicio();
        }

        public ActionResult Index()
        {
            return View(_controldatosservicio.GetControlDatosIndex());
            //return View();
        }

        public ActionResult IndexPager(Pager pager, ControlDatosVista model)
        {
            IControlDatosVista vista;
            //return View("Index",_controldatosservicio.GetControlDatos(pager));
            vista = _controldatosservicio.GetControlDatos(pager, Convert.ToInt32(model.Programas.Selected), Convert.ToInt32(
                                                                              model.etapas.Selected));
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
            vista.Programas.Selected = model.Programas.Selected;


            TempData["VistaRetorno"] = vista;



            return RedirectToAction("PageRedirect");
        }

        public ActionResult PageRedirect()
        {
            return View("Index", (ControlDatosVista)TempData["VistaRetorno"]);
        }

        [HttpPost]
        public ActionResult IndexTraerEtapas(ControlDatosVista model)
        {
            IControlDatosVista vista;
            //EtapasVista etapa = new EtapasVista();
            vista = _controldatosservicio.GetControlDatosIndex();
            vista.programaSel = model.programaSel;
            vista.filtroEjerc = model.filtroEjerc;
            model = (ControlDatosVista)vista;
            model.BuscarPorEtapa = "S";
            model.etapas = _etapaServicio.getEtapasCombo(model.programaSel, model.filtroEjerc);

            return View("Index",
                        model);
        }


        [HttpPost]
        public ActionResult BuscarControlDatos(ControlDatosVista model)
        {
            IControlDatosVista vista;
            vista = _controldatosservicio.GetControlDatos(Convert.ToInt32(model.Programas.Selected), Convert.ToInt32(
                                                                              model.etapas.Selected));
            vista.Programas.Selected = model.Programas.Selected;
            


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

            return View("Index", vista);
        }


        [HttpPost]
        public ActionResult ExportarControlDatos(ControlDatosVista model)
        {
            model = (ControlDatosVista)TempData["VistaCD"];
            TempData["VistaCD"] = model;
            
            return File(_controldatosservicio.ExportControlDatos(model),
                        "application/vnd.ms-excel", "ControlDatos.xls");
        }


    }
}
