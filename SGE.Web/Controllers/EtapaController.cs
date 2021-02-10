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
using System.Collections;
using SGE.Model.Entidades.Interfaces;
using System.Collections.Generic;

namespace SGE.Web.Controllers
{
    [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmEtapas)]
    public class EtapaController : BaseController
    {
        //
        // GET: /Etapa/

        private readonly EtapaServicio _etapaServicio;

        public EtapaController()
            : base((int)Formularios.Etapa)
        {
            _etapaServicio = new EtapaServicio();

        }



        public ActionResult Index()
        {
            return View("Etapas",
                        _etapaServicio.getEtapasIndex());

        }

        public ActionResult GetEtapas(EtapaVista model)
        {
            return View("Etapas",
                        _etapaServicio.getEtapas(model.ejerc, Convert.ToInt32( model.Programas.Selected) ));

        }



        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmEtapas, Accion = Enums.Acciones.Agregar)]
        public ActionResult Agregar()
        {
            EtapaVista vista = _etapaServicio.GetEtapa(0, "Agregar");
            return View("Formulario", vista);
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmEtapas, Accion = Enums.Acciones.Consultar)]
        public ActionResult Ver(int idEtapa)
        {
            EtapaVista vista = _etapaServicio.GetEtapa(idEtapa, "Ver");
            return View("Formulario", vista);
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmEtapas, Accion = Enums.Acciones.Modificar)]
        public ActionResult Modificar(int idEtapa)
        {
            EtapaVista vista = _etapaServicio.GetEtapa(idEtapa, "Modificar");
            return View("Formulario", vista);
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmEtapas, Accion = Enums.Acciones.Eliminar)]
        public ActionResult Eliminar(int idEtapa)
        {
            EtapaVista vista = _etapaServicio.GetEtapa(idEtapa, "Eliminar");
            return View("Formulario", vista);
        }


        [HttpPost]
        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmEtapas, Accion = Enums.Acciones.Agregar)]
        public ActionResult AgregarEtapa(EtapaVista model)
        {
            EtapaVista vista = model;
            switch (_etapaServicio.AddEtapa(model))
            {

                case 0:
                    return View("Etapas", _etapaServicio.getEtapas("", 0));
                case 1:
                    model =  _etapaServicio.GetEtapa(0, "Agregar");
                    model.Programas.Selected = vista.Programas.Selected;
                    model.N_ETAPA = vista.N_ETAPA;
                    model.EJERCICIO = vista.EJERCICIO;
                    model.FEC_INCIO = vista.FEC_INCIO;
                    model.FEC_FIN = vista.FEC_FIN;
                    model.MONTO_ETAPA = vista.MONTO_ETAPA;

                    TempData["MensajeError"] = "Ya existe una Carrera con ese nombre";
                    return View("Formulario", model);
                case 2:
                    model =  _etapaServicio.GetEtapa(0, "Agregar");
                    model.Programas.Selected = vista.Programas.Selected;
                    model.N_ETAPA = vista.N_ETAPA;
                    model.EJERCICIO = vista.EJERCICIO;
                    model.FEC_INCIO = vista.FEC_INCIO;
                    model.FEC_FIN = vista.FEC_FIN;
                    model.MONTO_ETAPA = vista.MONTO_ETAPA;
                    TempData["MensajeError"] = "Seleccione una opción";
                    return View("Formulario", model);
                case -1:
                    model =  _etapaServicio.GetEtapa(0, "Agregar");
                    model.Programas.Selected = vista.Programas.Selected;
                    model.N_ETAPA = vista.N_ETAPA;
                    model.EJERCICIO = vista.EJERCICIO;
                    model.FEC_INCIO = vista.FEC_INCIO;
                    model.FEC_FIN = vista.FEC_FIN;
                    model.MONTO_ETAPA = vista.MONTO_ETAPA;
                    TempData["MensajeError"] = "Se produjo un error al registrar la Etapa";
                    return View("Formulario", model);
                default:
                    //Muestra listado de error;
                    model = _etapaServicio.GetEtapa(0, "Agregar");
                    return View("Formulario", model);
            }
        }


        public ActionResult modificarEtapa(EtapaVista model)
        {


            int ret = 0;
            model.ID_PROGRAMA = Convert.ToInt32(model.Programas.Selected);
            ret=_etapaServicio.udpEtapa(model);

            model = _etapaServicio.GetEtapa(model.ID_ETAPA, "Modificar");
            if (ret == 0)
            {
                TempData["MensajeError"] = "Los datos se modificaron correctamente";
            }
            else { TempData["MensajeError"] = "Se produjo un error al actualizar la Etapa"; }
            return View("Formulario",model);
            
        }


        //18/02/2013 - DI CAMPLI LEANDRO - ALTA MASIVA PARA BENEFICIARIOS
        public ActionResult AsociarEtapa(/*FichasVista modelo*/)
        {

            //IFichaVista vista = _fichaservicio.GetFichas();
            EtapasVista model = _etapaServicio.getEtapasAsociar();

            model.paso = "PASO 1";
            return View(model);

        }


        public ActionResult GetEtapasAsignar(EtapasVista model)
        {
            EtapasVista vista = new EtapasVista();
            vista = _etapaServicio.getEtapas(model);

            
            
            model.Programas = vista.Programas;
            //model.Programas.Selected = model.programaSel.ToString();
            model.Fichas = vista.Fichas;//(IList<IFicha>)ViewBag.fichasEtapa;

            if (vista.listEtapas.Count == 0)
            {
                model.paso = "PASO 1";
                model.mensaje = "No se encontraron etapas. Modifique los filtros y vuelva a intentarlo.";

                return View("AsociarEtapa",
                            model);
            }
            else
            {
                
                model.etapas = vista.etapas;


                model = _etapaServicio.GetEtapasAsignar(model);
                model.mensaje = "";
                model.ejerc =  model.ejerc ??  "";
                model.programaSel = Convert.ToInt32(model.Programas.Selected);
                model.filtroEjerc = model.ejerc;
                model.Programas.Selected = model.ID_PROGRAMA.ToString();
                model.Programas.Enabled = false;
                
                model.paso = "PASO 2";
                return View("AsociarEtapa",
                            model);
            }

        }


        /*            EtapaVista vista = _etapaServicio.GetEtapa(idEtapa, "Ver");
            return View("Formulario", vista);*/

        public ActionResult GetEtapaAsignar(EtapasVista model)
        {

            EtapasVista vista = new EtapasVista();
            model.programaSel = Convert.ToInt32(model.programaSel);
            model.ejerc = model.ejerc ?? model.filtroEjerc;
            //vista = _etapaServicio.getEtapas(model.ejerc, Convert.ToInt32(model.Programas.Selected));
            EtapasVista etapa = _etapaServicio.GetEtapa(model);
            model.filtroEtapa = model.ID_ETAPA;
            model = etapa;
            model.Programas.Enabled = false;
            model.paso = "PASO 2";
            return View("AsociarEtapa",
                        model);

        }

        public ActionResult EtapaAsignarPaso3(EtapasVista model)
        {

            EtapasVista vista = new EtapasVista();
            model.filtroEtapa = Convert.ToInt32(model.etapas.Selected);
            model.programaSel = Convert.ToInt32(model.programaSel);
            model.ejerc = model.ejerc ?? model.filtroEjerc;
            //vista = _etapaServicio.getEtapas(model.ejerc, Convert.ToInt32(model.Programas.Selected));
            EtapasVista etapa = _etapaServicio.GetEtapa(model);

            model = etapa;
            model.Programas.Enabled = false;
            //model.etapas.Selected = etapa.ToString();
            model.etapas.Enabled = false;
            model.paso = "PASO 3";
            return View("AsociarEtapa",
                        model);

        }


        [HttpPost]
        public ActionResult SubirArchivoFichas(HttpPostedFileBase/*string*/ archivoFichas, EtapasVista model)
        {

            EtapasVista vista = _etapaServicio.SubirArchivoFichas(archivoFichas, model);

            model.ID_ETAPA = model.filtroEtapa;
            model.programaSel = Convert.ToInt32(model.programaSel);
            model.ejerc = model.ejerc ?? model.filtroEjerc;
            //vista = _etapaServicio.getEtapas(model.ejerc, Convert.ToInt32(model.Programas.Selected));
            EtapasVista etapa = _etapaServicio.GetEtapa(model);

            model = etapa;
            model.Programas.Enabled = false;
            //model.etapas.Selected = etapa.ToString();
            model.etapas.Enabled = false;
            model.paso = "PASO 3";
            model.ProcesarExcel = vista.ProcesarExcel;
            model.ExcelNombre = vista.ExcelNombre;
            
            //TempData["VistaSubida"] = vista;

            return View("AsociarEtapa", model);
            //return RedirectToAction("ALtaMasivaBenef");
        }

        //[HttpPost]
        //public ActionResult CargarFichasEtapaXLS(EtapasVista model)
        //ExcelNombre = Model.ExcelNombre, idEtapa = Model.ID_ETAPA, ejercicio = Model.filtroEjerc, idprograma = Model.programaSel
        public ActionResult CargarFichasEtapaXLS(string ExcelNombre, int idEtapa, string ejercicio, int idprograma)
        {
            EtapasVista model = new EtapasVista();
            model.ID_ETAPA = idEtapa;
            model.programaSel = Convert.ToInt32(idprograma);
            model.ejerc = ejercicio;
            model.filtroEjerc = ejercicio;
            model.ExcelNombre = ExcelNombre;
            //vista = _etapaServicio.getEtapas(model.ejerc, Convert.ToInt32(model.Programas.Selected));
            EtapasVista etapa = _etapaServicio.GetEtapa(model);

            model = etapa;
            model.Programas.Enabled = false;
            //model.etapas.Selected = etapa.ToString();
            model.etapas.Enabled = false;
            model.paso = "PASO 3";


            IFichasVista vista = _etapaServicio.CargarFichasEtapaXLS(model.ExcelNombre);

            model.Fichas = vista.Fichas;
            model.ProcesarExcel = vista.ProcesarExcel;

            return View("AsociarEtapa", model);
        }



        public ActionResult udpEtapaFichas(EtapasVista model)
        {
            model = (EtapasVista)TempData["EtapasVista"];
            int cant = 0;
            //EtapasVista etapa = _etapaServicio.GetEtapa(model);
            cant = _etapaServicio.udpEtapaFichas(model);
            model.cantProcesados = cant;
            //modelo = (FichasVista)_fichaservicio.GetFichas(modelo);
            model = _etapaServicio.getFichas(model);
            
            model.Programas.Enabled = false;
            //model.etapas.Selected = etapa.ToString();
            model.etapas.Enabled = false;
            model.paso = "PASO 3";
            
            return  View("AsociarEtapa", model);
        }


    }
}
