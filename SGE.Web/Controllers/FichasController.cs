using System;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
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
    [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmFicha)]
    public class FichasController : BaseController
    {
        private readonly IFichaServicio _fichaservicio;
        private readonly IEmpresaServicio _empresaServicio;
        private readonly ISedeServicio _sesdesservicios;
        private readonly IFichaObservacionServicio _fichaObservacionServicio;
        private readonly EtapaServicio _etapaServicio;
        private readonly IProyectoServicio _proyectoServicio;
        private readonly ICursoServicio _CursoServicio;
        private readonly IActorServicio _actorServicio;
        private readonly IOngServicio _ongServicio;
        private readonly ISubprogramaServicio _subprograma;
        private readonly IEscuelaServicio _escuela;

        public FichasController()
            : base((int)Formularios.Ficha)
        {
            _fichaservicio = new FichaServicio();
            _empresaServicio = new EmpresaServicio();
            _sesdesservicios = new SedeServicio();
            _fichaObservacionServicio = new FichaObservacionServicio();
            _etapaServicio = new EtapaServicio();
            _proyectoServicio = new ProyectoServicio();
            _CursoServicio = new CursoServicio();
            _actorServicio = new ActorServicio();
            _ongServicio = new OngServicio();
            _subprograma = new SubprogramaServicio();
            _escuela = new EscuelaServicio();
        }

        public ActionResult Index()
        {
            return View(_fichaservicio.GetIndex());
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmFicha, Accion = Enums.Acciones.Consultar)]
        public ActionResult Ver(int idFicha)
        {
            IFichaVista vista = _fichaservicio.GetFicha(idFicha, "Ver");

            return View("Formulario", vista);
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmFicha, Accion = Enums.Acciones.Consultar)]
        public ActionResult VerDesdeBeneficiario(int idFicha, int idbeneficiario)
        {
            IFichaVista vista = _fichaservicio.GetFicha(idFicha, "Ver");
            vista.IdLlamada = idbeneficiario;
            vista.LugarLlamada = "Beneficiario";
            return View("Formulario", vista);
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmFicha, Accion = Enums.Acciones.Modificar)]
        public ActionResult Modificar(int idFicha)
        {
            IFichaVista vista = _fichaservicio.GetFicha(idFicha, "Modificar");
            vista.LugarLlamada = "FichaModificar";
            return View("Formulario", vista);
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmFicha, Accion = Enums.Acciones.Modificar)]
        public ActionResult ModificarDesdeBeneficiario(int idFicha, int idbeneficiario)
        {
            IFichaVista vista = _fichaservicio.GetFicha(idFicha, "Modificar");
            vista.LugarLlamada = "Beneficiario";
            vista.IdLlamada = idbeneficiario;
            return View("Formulario", vista);
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmFicha, Accion = Enums.Acciones.Agregar)]
        public ActionResult Agregar()
        {
            IFichaVista vista = new FichaVista
                                    {
                                        Estado = { Selected = ((int)Enums.EstadoFicha.Apta).ToString() },
                                        IdFicha = 0,
                                        ComboEstadoFicha = { Selected = ((int)Enums.EstadoFicha.Apta).ToString() }
                                    };

            vista = _fichaservicio.GetFicha(vista, "Agregar");
            return View("Formulario", vista);
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmFicha, Accion = Enums.Acciones.Eliminar)]
        public ActionResult Eliminar(int idFicha)
        {
            IFichaVista vista = _fichaservicio.GetFicha(idFicha, "Eliminar");
            return View("Formulario", vista);
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmFicha, Accion = Enums.Acciones.CambiarEstado)]
        public ActionResult CambiarEstado(int idFicha)
        {
            IFichaVista vista = _fichaservicio.GetFicha(idFicha, "CambiarEstado");
            vista.LugarLlamada = "FichaCambiarEstado";
            return View("Formulario", vista);
        }

        public ActionResult IndexPager(Pager pager, FichasVista model)
        {
            int idprograma = 0;
            idprograma = Convert.ToInt32(model.ComboTipoFicha.Selected);
            if (model.ComboTipoFicha.Selected == "0")
            {
                if (model.EtapaProgramas.Selected == "3")
                {
                    idprograma = Convert.ToInt32(model.EtapaProgramas.Selected);
                }
            }

            IFichasVista vista = _fichaservicio.GetFichas(pager, model.cuil_busqueda, model.nro_doc_busqueda,
                                          model.nombre_busqueda,
                                          model.apellido_busqueda,
                                          idprograma,
                                          Convert.ToInt32(model.ComboEstadoFicha.Selected), Convert.ToInt32(model.etapas.Selected), model.idSubprograma, Convert.ToInt32(model.ComboTiposPpp.Selected), model.nrotramite_busqueda);

            vista.ComboTipoFicha.Selected = model.ComboTipoFicha.Selected;
            
            // 23/05/2013 - DI CAMPLI LEANDRO - RECARGAR NUEVOS FILTROS DE BUSQUEDA

            //vista.EtapaProgramas = vista.Programas;
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

            vista.idSubprograma = model.idSubprograma;
            vista.idFormulario = "FormIndexFichas";
            vista.ComboTiposPpp.Selected = model.ComboTiposPpp.Selected;
            // 13/03/2018+++++++++++
            vista.cuil_busqueda = model.cuil_busqueda;
            vista.nro_doc_busqueda = model.nro_doc_busqueda;
            vista.nombre_busqueda = model.nombre_busqueda;
            vista.apellido_busqueda = model.apellido_busqueda;
            vista.nrotramite_busqueda = model.nrotramite_busqueda;

            //+++++++++++++++

            TempData["Model"] = vista;
            return RedirectToAction("PageRedirect");
        }

        public ActionResult PageRedirect()
        {
            return View("Index", (FichasVista)TempData["Model"]);
        }

        [HttpPost]
        public ActionResult BuscarFichas(FichasVista model)
        {
            int idprograma = 0;
            idprograma = Convert.ToInt32(model.ComboTipoFicha.Selected);
            if (model.ComboTipoFicha.Selected == "0")
            {
                if (model.EtapaProgramas.Selected == "3")
                {
                    idprograma = Convert.ToInt32(model.EtapaProgramas.Selected);
                }
            }

            IFichasVista vista = _fichaservicio.GetFichas(model.cuil_busqueda, (model.nro_doc_busqueda != null ? model.nro_doc_busqueda.Replace(".", "").ToString() : null), model.nombre_busqueda,
                        model.apellido_busqueda, idprograma, Convert.ToInt32(model.ComboEstadoFicha.Selected), Convert.ToInt32(model.etapas.Selected), model.idSubprograma, Convert.ToInt32(model.ComboTiposPpp.Selected), model.nrotramite_busqueda);

            vista.ComboTipoFicha.Selected = model.ComboTipoFicha.Selected;
            
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
            vista.idFormulario = "FormIndexFichas";
            // 13/03/2018+++++++++++
            vista.cuil_busqueda = model.cuil_busqueda;
            vista.nro_doc_busqueda = model.nro_doc_busqueda;
            vista.nombre_busqueda = model.nombre_busqueda;
            vista.apellido_busqueda = model.apellido_busqueda;
            vista.nrotramite_busqueda = model.nrotramite_busqueda;

            //+++++++++++++++
            TempData["Model"] = vista;


             
            return View("Index",vista);
        }

        [HttpPost]
        public ActionResult ModificarFicha(FichaVista modelo)
        {

            if (modelo.LugarLlamada == "FichaCambiarEstado")
            {
                FichaVista m = (FichaVista)TempData["TempFichaModelo"];
                m.Estado = modelo.Estado;
                m.LugarLlamada = modelo.LugarLlamada;
                _fichaservicio.UpdateFicha(m);
            }
            else
            {
                _fichaservicio.UpdateFicha(modelo);
            }

            TempData["mensajeUdp"] = modelo.strMessageUpdate;
            switch (modelo.LugarLlamada)
            {
                case "Beneficiario":
                    return RedirectToAction(modelo.Accion, "Beneficiarios", new { idBeneficiario = modelo.IdLlamada });
                case "FichaModificar":
                    
                    return RedirectToAction("ModificarResultado", "Fichas", new { idFicha = modelo.IdFicha});
                case "FichaCambiarEstado":

                    return RedirectToAction("ModificarResultado", "Fichas", new { idFicha = modelo.IdFicha});
                default:
                    return RedirectToAction("Index", "Fichas");
            }
        }


        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmFicha, Accion = Enums.Acciones.Modificar)]
        public ActionResult ModificarResultado(int idFicha)
        {
            string strMessageUpdate = "";
            strMessageUpdate = (string)TempData["mensajeUdp"]; ;
            IFichaVista vista = _fichaservicio.GetFicha(idFicha, "Modificar");
            vista.LugarLlamada = "FichaModificar";
            vista.strMessageUpdate = strMessageUpdate;
            return View("Formulario", vista);
        }

        [HttpPost]
        public ActionResult AgregarFicha(FichaVista modelo)
        {
            _fichaservicio.AddFicha(modelo);

            
            IFichasVista vista = _fichaservicio.GetFichas(modelo.Cuil, (modelo.NumeroDocumento != null ? modelo.NumeroDocumento.Replace(".", "").ToString() : null), "",
                        "", Convert.ToInt32(modelo.ComboTipoFicha.Selected), Convert.ToInt32(modelo.ComboEstadoFicha.Selected), 0, 0, 0,"");
            vista.cuil_busqueda = modelo.Cuil;
            vista.nro_doc_busqueda = modelo.NumeroDocumento;
            vista.ComboTipoFicha.Selected = modelo.ComboTipoFicha.Selected;
            vista.ComboEstadoFicha.Selected = modelo.ComboEstadoFicha.Selected;

            TempData["Model"] = vista;
            //return View("Index", _fichaservicio.GetFichas());
            return View("Index", vista);
        }

        public ActionResult CambiarCombo(FichaVista model)
        {
            IFichaVista vista = _fichaservicio.GetFicha(model, model.Accion);
            //bool f = false;
            //vista.CambioTipo = f;

            return View("Formulario", vista);
        }

        [HttpPost]
        public ActionResult ImportarExcel(HttpPostedFileBase archivo)
        {
            IImportarExcelVista vista = new ImportarExcelVista();

            if (archivo != null)
            {
                vista = _fichaservicio.ImportarExcel(archivo);
            }
            else
            {
                vista.Importado = false;
                vista.Mensaje = "Elija el archivo a importar.";
            }

            return View("ImportarExcel", vista);
        }

        [HttpPost]
        public ActionResult ProcesarExcel(ImportarExcelVista model)
        {
            _fichaservicio.ProcesarExcel(model);

            IFichasVista vista = _fichaservicio.GetFichas();

            vista.ProcesarExcel = true;

            return View("Index", vista);
        }

        public ActionResult ConvertirFicha(int idFicha, int idEtapa = 0)
        {
            bool exito = _fichaservicio.ConvertirFicha(idFicha);
            IFichaVista vista = _fichaservicio.GetFicha(idFicha, "Modificar");
            vista.CambioTipo = exito;

            if (exito == true)
            { 
                if(idEtapa != 0)
                {
                    _fichaservicio.UpdateFichaEtapa(idFicha, idEtapa);
                }
  
            }

            return View("Formulario", vista);
        }

        [HttpPost]
        public ActionResult ValidarCuit(string cuitEmpresa)
        {
            IEmpresaVista empresa = _empresaServicio.GetEmpresa(cuitEmpresa, "");

            return Json(empresa);
        }

        [HttpPost]
        public ActionResult CargarDatosEmpresa(FichaVista fichaVista)
        {
            return View("Formulario", _fichaservicio.GetEmpresa(fichaVista));
        }

        [HttpPost]
        public ActionResult VerObservaciones(FichaVista fichaVista)
        {
            //TempData["IdFicha"] = (int)TempData["IdFicha"];

            return RedirectToAction("Index", new RouteValueDictionary(
                                                 new
                                                     {
                                                         controller = "FichaObservacion",
                                                         action = "Index",
                                                         idFicha = fichaVista.IdFicha, accionFicha= fichaVista.Accion
                                                     }));
           
        }

        [HttpPost]
        public ActionResult AgregarNuevaEmpresa(string cuitEmpresa, string nombreempresa, string idlocalidad, string iddomiciliolaboral, string cantempleados, string codigoactividad, string calle, string numero, string piso, string dpto, string codigopostal)
        {
            IEmpresaVista empresa = new EmpresaVista
                                        {
                                            Calle = calle,
                                            CantidadEmpleados = Convert.ToInt16((cantempleados == "" ? "0" : cantempleados)),
                                            Cuit = cuitEmpresa,
                                            Descripcion = nombreempresa,
                                            Dpto = dpto,
                                            CodigoActividad = codigoactividad,
                                            CodigoPostal = codigopostal,
                                            IdDomicilioLaboral = Convert.ToInt32(iddomiciliolaboral == "" ? "0" : iddomiciliolaboral),
                                            IdLocalidad = Convert.ToInt32(idlocalidad == "" ? "0" : idlocalidad),
                                            Numero = numero,
                                            Piso = piso
                                        };

            empresa.Localidades.Selected = idlocalidad;
            empresa.DomicilioLaboralIdem.Selected = iddomiciliolaboral;

            int mret = _empresaServicio.AddEmpresa(empresa);

            if (mret == 0)
            {
                empresa = null;
            }
            else
            {
                empresa = _empresaServicio.GetEmpresa(empresa.Cuit, "Ver");
                empresa.Id = mret;
            }

            return Json(empresa);
        }

        public ActionResult RedirectFicha(string origenLlamada)
        {
            IFichaVista vistaFicha = new FichaVista();
            vistaFicha = (IFichaVista)ViewData["DatosAnteriores"];

            return View("Formulario", vistaFicha);
        }

        public ActionResult Cancelar(FichaVista model)
        {
            switch (model.LugarLlamada)
            {
                case "Beneficiario":
                    return RedirectToAction(model.Accion, "Beneficiarios", new { idBeneficiario = model.IdLlamada });
                default:
                    return RedirectToAction("Index", "Fichas");
            }
        }

        public ActionResult ExportFichas(FichasVista model)
        {
            string nombrearchivo;

            switch (Convert.ToInt32(model.ComboTipoFicha.Selected))
            {
                case (int)Enums.TipoFicha.Ppp:
                    nombrearchivo = "FichasPPP_" + DateTime.Now.ToShortDateString() + ".xls";
                    return File(_fichaservicio.ExportarFichasPPP(model.cuil_busqueda,
                                                       (model.nro_doc_busqueda != null
                                                            ? model.nro_doc_busqueda.Replace(".", "").ToString()
                                                            : null), model.nombre_busqueda,
                                                       model.apellido_busqueda,
                                                       Convert.ToInt32(model.ComboTipoFicha.Selected),
                                                       Convert.ToInt32(model.ComboEstadoFicha.Selected), Convert.ToInt32(model.etapas.Selected), Convert.ToInt32(model.ComboTiposPpp.Selected), Convert.ToInt32(model.ComboSubprogramas.Selected), model.nrotramite_busqueda), "application/vnd.ms-excel", nombrearchivo);
                case (int)Enums.TipoFicha.Terciaria:
                    nombrearchivo = "FichasTerciarios_" + DateTime.Now.ToShortDateString() + ".xls";
                    return File(_fichaservicio.ExportarFichasTer(model.cuil_busqueda,
                                               (model.nro_doc_busqueda != null
                                                    ? model.nro_doc_busqueda.Replace(".", "").ToString()
                                                    : null), model.nombre_busqueda,
                                               model.apellido_busqueda,
                                               Convert.ToInt32(model.ComboTipoFicha.Selected),
                                               Convert.ToInt32(model.ComboEstadoFicha.Selected), Convert.ToInt32(model.etapas.Selected), Convert.ToInt32(model.ComboSubprogramas.Selected)), "application/vnd.ms-excel", nombrearchivo);
                    
                case (int)Enums.TipoFicha.Universitaria:
                    nombrearchivo = "FichasUniversitarios_" + DateTime.Now.ToShortDateString() + ".xls";
                    return File(_fichaservicio.ExportarFichasUni(model.cuil_busqueda,
                                               (model.nro_doc_busqueda != null
                                                    ? model.nro_doc_busqueda.Replace(".", "").ToString()
                                                    : null), model.nombre_busqueda,
                                               model.apellido_busqueda,
                                               Convert.ToInt32(model.ComboTipoFicha.Selected),
                                               Convert.ToInt32(model.ComboEstadoFicha.Selected), Convert.ToInt32(model.etapas.Selected), Convert.ToInt32(model.ComboSubprogramas.Selected)), "application/vnd.ms-excel", nombrearchivo);

                    
                case (int)Enums.TipoFicha.PppProf:
                    nombrearchivo = "FichasPppProf_" + DateTime.Now.ToShortDateString() + ".xls";
                    return File(_fichaservicio.ExportarFichasProf(model.cuil_busqueda,
                                               (model.nro_doc_busqueda != null
                                                    ? model.nro_doc_busqueda.Replace(".", "").ToString()
                                                    : null), model.nombre_busqueda,
                                               model.apellido_busqueda,
                                               Convert.ToInt32(model.ComboTipoFicha.Selected),
                                               Convert.ToInt32(model.ComboEstadoFicha.Selected), Convert.ToInt32(model.etapas.Selected)), "application/vnd.ms-excel", nombrearchivo);

                case (int)Enums.TipoFicha.Vat:
                    nombrearchivo = "FichasVat_" + DateTime.Now.ToShortDateString() + ".xls";
                    return File(_fichaservicio.ExportarFichasVat(model.cuil_busqueda,
                                               (model.nro_doc_busqueda != null
                                                    ? model.nro_doc_busqueda.Replace(".", "").ToString()
                                                    : null), model.nombre_busqueda,
                                               model.apellido_busqueda,
                                               Convert.ToInt32(model.ComboTipoFicha.Selected),
                                               Convert.ToInt32(model.ComboEstadoFicha.Selected), Convert.ToInt32(model.etapas.Selected)), "application/vnd.ms-excel", nombrearchivo);

                default:
                    nombrearchivo = "Fichas_" + DateTime.Now.ToShortDateString() + ".xls";
                    
                    return File(new byte[]{}, "application/vnd.ms-excel", nombrearchivo);

                case (int)Enums.TipoFicha.ReconversionProductiva:
                    nombrearchivo = "FichasReconversionProductiva_" + DateTime.Now.ToShortDateString() + ".xls";
                    return File(_fichaservicio.ExportarFichasReco_Prod(model.cuil_busqueda,
                                               (model.nro_doc_busqueda != null
                                                    ? model.nro_doc_busqueda.Replace(".", "").ToString()
                                                    : null), model.nombre_busqueda,
                                               model.apellido_busqueda,
                                               Convert.ToInt32(model.ComboTipoFicha.Selected),
                                               Convert.ToInt32(model.ComboEstadoFicha.Selected), Convert.ToInt32(model.etapas.Selected),model), "application/vnd.ms-excel", nombrearchivo);

                case (int)Enums.TipoFicha.EfectoresSociales:
                    nombrearchivo = "FichasEfectoresSociales_" + DateTime.Now.ToShortDateString() + ".xls";

                    return File(_fichaservicio.ExportarFichasEfec_Soc(model.cuil_busqueda,
                                               (model.nro_doc_busqueda != null
                                                    ? model.nro_doc_busqueda.Replace(".", "").ToString()
                                                    : null), model.nombre_busqueda,
                                               model.apellido_busqueda,
                                               Convert.ToInt32(model.ComboTipoFicha.Selected),
                                               Convert.ToInt32(model.ComboEstadoFicha.Selected), Convert.ToInt32(model.etapas.Selected), model), "application/vnd.ms-excel", nombrearchivo);
                case (int)Enums.TipoFicha.ConfiamosEnVos:
                    nombrearchivo = "FichasConfiamosEnVos_" + DateTime.Now.ToShortDateString() + ".xls";
                    return File(_fichaservicio.ExportarFichasConfVos(model.cuil_busqueda,
                                               (model.nro_doc_busqueda != null
                                                    ? model.nro_doc_busqueda.Replace(".", "").ToString()
                                                    : null), model.nombre_busqueda,
                                               model.apellido_busqueda,
                                               Convert.ToInt32(model.ComboTipoFicha.Selected),
                                               Convert.ToInt32(model.ComboEstadoFicha.Selected), Convert.ToInt32(model.etapas.Selected), model), "application/vnd.ms-excel", nombrearchivo);

            }

            //byte[] ret = _fichaservicio.ExportarFichas(model.cuil_busqueda,
            //                                           (model.nro_doc_busqueda != null
            //                                                ? model.nro_doc_busqueda.Replace(".", "").ToString()
            //                                                : null), model.nombre_busqueda,
            //                                           model.apellido_busqueda,
            //                                           Convert.ToInt32(model.ComboTipoFicha.Selected),
            //                                           Convert.ToInt32(model.ComboEstadoFicha.Selected));

            return File(_fichaservicio.ExportarFichas(model.cuil_busqueda,
                                                       (model.nro_doc_busqueda != null
                                                            ? model.nro_doc_busqueda.Replace(".", "").ToString()
                                                            : null), model.nombre_busqueda,
                                                       model.apellido_busqueda,
                                                       Convert.ToInt32(model.ComboTipoFicha.Selected),
                                                       Convert.ToInt32(model.ComboEstadoFicha.Selected)), "application/vnd.ms-excel", nombrearchivo);
        }

        [HttpPost]
        public ActionResult CargarSedesEmpresa(int idempresa)
        {
            ISedesVista sedes = _sesdesservicios.GetSedesByEmpresa(idempresa);

            var tablaretorno = new StringBuilder();

            tablaretorno.Append(
                "<thead><tr><th style='height: 30px; width: 500px;'><div align='center'>Nombre Sede</div></th>");
            tablaretorno.Append("<th style='width: 200px;'><div align='center'>Ap. y Nomb. Cont.</div></th>");
            tablaretorno.Append("<th style='width: 70px;'><div align='center'>Teléfono</div></th>");
            tablaretorno.Append("<th style='width: 100px;'><div align='center'>Calle</div></th>");
            tablaretorno.Append("<th style='width: 50px;'><div align='center'>Nro.</div></th>");
            tablaretorno.Append("<th style='width: 50px;'><div align='center'>Piso</div></th>");
            tablaretorno.Append("<th style='width: 50px;'><div align='center'>Dpto.</div></th>");
            tablaretorno.Append("<th style='width: 100px;'><div align='center'>Loc.</div></th>");
            tablaretorno.Append("<th style='width: 80px;'><div align='center'>Acción</div></th></tr></thead><tbody>");


            foreach (var sede in sedes.Sedes)
            {
                tablaretorno.Append("<tr><td style='height: 28px;  text-align: left;'>" + sede.NombreSede + "</td>");
                tablaretorno.Append("<td style='height: 28px;  text-align: left;'>" + sede.ApellidoContacto + ", " + sede.NombreContacto + "</td>");
                tablaretorno.Append("<td style='height: 28px;  text-align: left;'>" + sede.Telefono + "</td>");
                tablaretorno.Append("<td style='height: 28px;  text-align: left;'>" + sede.Calle + "</td>");
                tablaretorno.Append("<td style='height: 28px;  text-align: right;'>" + sede.Numero + "</td>");
                tablaretorno.Append("<td style='height: 28px;  text-align: right;'> " + sede.Piso + "</td>");
                tablaretorno.Append("<td style='height: 28px;  text-align: left;'>" + sede.Dpto + "</td>");
                tablaretorno.Append("<td style='height: 28px;  text-align: left;'>" + sede.NombreLocalidad + "</td>");
                tablaretorno.Append(
                    "<td style='height: 28px;  text-align: center;'><img  src='../../Content/images/icon_derecha.gif' style='cursor: pointer;' width='18'  height='18' alt='' onclick='SeleccionarSede(" +
                    sede.IdSede + ",$" + sede.NombreSede + "$, $" + sede.ApellidoContacto + "$, $" +
                    sede.NombreContacto + "$,$" + sede.Telefono + "$, $" + sede.NombreLocalidad + "$,$" + sede.Calle +
                    "$,$" + sede.Numero + "$, $" + sede.Piso + "$, $" + sede.Dpto + "$);'  /></td>");
            }

            tablaretorno.Append("</tr></tbody>");

            return Json(tablaretorno.ToString().Replace('$', '"'));
        }

        [HttpPost]
        public ActionResult AgregarSedeEmpresa(int idempresa, string nombre, string apecon, string nombrecon, string localidad, string telefono, string calle, string nro, string piso, string dpto)
        {
            if (idempresa != 0)
            {
                ISedeVista sedevista = new SedeVista()
                                      {
                                          Descripcion = nombre,
                                          ApellidoContacto = apecon,
                                          NombreContacto = nombrecon,
                                          IdEmpresa = idempresa,
                                          Numero = nro,
                                          Calle = calle,
                                          Dpto = dpto,
                                          Piso = piso,
                                          Telefono = telefono,
                                          Localidades = new ComboBox() { Selected = localidad }
                                      };

                _sesdesservicios.AddSedeFromInscripcion(sedevista);
            }

            ISedesVista sedes = _sesdesservicios.GetSedesByEmpresa(idempresa);

            var tablaretorno = new StringBuilder();

            tablaretorno.Append(
                "<thead><tr><th style='height: 30px; width: 500px;'><div align='center'>Nombre Sede</div></th>");
            tablaretorno.Append("<th style='width: 200px;'><div align='center'>Ap. y Nomb. Cont.</div></th>");
            tablaretorno.Append("<th style='width: 70px;'><div align='center'>Teléfono</div></th>");
            tablaretorno.Append("<th style='width: 100px;'><div align='center'>Calle</div></th>");
            tablaretorno.Append("<th style='width: 50px;'><div align='center'>Nro.</div></th>");
            tablaretorno.Append("<th style='width: 50px;'><div align='center'>Piso</div></th>");
            tablaretorno.Append("<th style='width: 50px;'><div align='center'>Dpto.</div></th>");
            tablaretorno.Append("<th style='width: 100px;'><div align='center'>Loc.</div></th>");
            tablaretorno.Append("<th style='width: 80px;'><div align='center'>Acción</div></th></tr></thead><tbody>");

            foreach (var sede in sedes.Sedes)
            {
                tablaretorno.Append("<tr><td style='height: 28px;  text-align: left;'>" + sede.NombreSede + "</td>");
                tablaretorno.Append("<td style='height: 28px;  text-align: left;'>" + sede.ApellidoContacto + ", " + sede.NombreContacto + "</td>");
                tablaretorno.Append("<td style='height: 28px;  text-align: left;'>" + sede.Telefono + "</td>");
                tablaretorno.Append("<td style='height: 28px;  text-align: left;'>" + sede.Calle + "</td>");
                tablaretorno.Append("<td style='height: 28px;  text-align: right;'>" + sede.Numero + "</td>");
                tablaretorno.Append("<td style='height: 28px;  text-align: right;'> " + sede.Piso + "</td>");
                tablaretorno.Append("<td style='height: 28px;  text-align: left;'>" + sede.Dpto + "</td>");
                tablaretorno.Append("<td style='height: 28px;  text-align: left;'>" + sede.NombreLocalidad + "</td>");
                tablaretorno.Append(
                    "<td style='height: 28px;  text-align: center;'><img  src='../../Content/images/icon_derecha.gif' style='cursor: pointer;' width='18'  height='18' alt='' onclick='SeleccionarSede(" +
                    sede.IdSede + ",$" + sede.NombreSede + "$, $" + sede.ApellidoContacto + "$, $" +
                    sede.NombreContacto + "$,$" + sede.Telefono + "$, $" + sede.NombreLocalidad + "$,$" + sede.Calle +
                    "$,$" + sede.Numero + "$, $" + sede.Piso + "$, $" + sede.Dpto + "$);'  /></td>");
            }

            tablaretorno.Append("</tr></tbody>");

            return Json(tablaretorno.ToString().Replace('$', '"'));
        }

        [HttpPost]
        public ActionResult AgregarHorarioEmpresa(FichaVista modelo)
        {
            _fichaservicio.AgregarHorarioEmpresa(modelo);

            IFichaVista vista = _fichaservicio.GetFicha(modelo.IdFicha, "Modificar");
            return View("Formulario", vista);
        }

        [HttpPost]
        public ActionResult AgregarCicloEmpresa(FichaVista modelo)
        {
            IFichaVista vista = _fichaservicio.AgregarCicloEmpresa(modelo);
            return View("Formulario", vista);
        }

        //18/02/2013 - DI CAMPLI LEANDRO - ALTA MASIVA PARA BENEFICIARIOS
        public ActionResult ALtaMasivaBeneficiarios(/*FichasVista modelo*/)
        {

            //IFichaVista vista = _fichaservicio.GetFichas();

            return View(_fichaservicio.indexAltaMasiviaBenef());
        
        }

        [HttpPost]
        public ActionResult SubirArchivoFichas(HttpPostedFileBase/*string*/ archivoFichas, FichasVista model)
        {
            
            IFichasVista vista = _fichaservicio.SubirArchivoFichas(archivoFichas, model);

            //TempData["VistaSubida"] = vista;

            return View("ALtaMasivaBeneficiarios",  vista);
            //return RedirectToAction("ALtaMasivaBenef");
        }

        //[HttpPost]
        public ActionResult CargarMasivaFichasXLS(string ExcelNombre)
        {
            IFichasVista vista = _fichaservicio.CargarMasivaFichasXLS(ExcelNombre);

            return View("ALtaMasivaBeneficiarios", vista);
        }


        [HttpPost]
        public ActionResult ALtaMasivaBenef(FichasVista modelo)
        {

            //IFichaVista vista = _fichaservicio.GetFichas();

            return View("ALtaMasivaBeneficiarios",modelo);

        }

        // 21/02/2013 - DI CAMPLI LEANDRO - CARGA MASIVA A BENEFICIARIOS
        [HttpPost]
        public ActionResult UpdateCargaMasiva (FichasVista modelo)
        {
            DateTime fechaBenef = modelo.FechaInicioBeneficiario;
            modelo = (FichasVista)TempData["FichasVista"];
            modelo.FechaInicioBeneficiario = fechaBenef;
            int cant = 0;
            cant=_fichaservicio.UpdateCargaMasiva(modelo);
            modelo = (FichasVista)_fichaservicio.GetFichas(modelo);

            modelo.cantProcesados = cant;
            return View("ALtaMasivaBeneficiarios", modelo); 
        }

        // 10/06/2013 - DI CAMPLI LEANDRO
        [HttpPost]
        public ActionResult IndexTraerEtapas(FichasVista model)
        {
            IFichasVista vista;
            //EtapasVista etapa = new EtapasVista();
            vista = _fichaservicio.GetIndex();//_beneficiarioServicio.GetBeneficiariosIndex();
            vista.programaSel = model.programaSel;
            vista.filtroEjerc = model.filtroEjerc;
            model = (FichasVista)vista;
            model.BuscarPorEtapa = "S";
            model.etapas = _etapaServicio.getEtapasCombo(model.programaSel, model.filtroEjerc);
            vista.ComboTiposPpp.Selected = model.ComboTiposPpp.Selected;

            return View("Index",
                        model);
        }
        //[HttpPost]
        //public ActionResult IndexTraerEtapas(int programaSel, string filtroEjerc)
        //{
        //    ComboBox etapas;
        //    etapas = _etapaServicio.getEtapasCombo(programaSel, filtroEjerc);

        //    return Json(etapas);
        //}

        [HttpPost]
        public ActionResult CargarcboLocalidad(int idDepartamento)
        {
            ComboBox Localidades = new ComboBox();
            _fichaservicio.CargarLocalidadEscuela(Localidades, idDepartamento);

            return Json(Localidades);
        }

        [HttpPost]
        public ActionResult CargarcboEscuela(int idLocalidad)
        {
            ComboBox escuelas = new ComboBox();
            _fichaservicio.CargarEscuela(escuelas, idLocalidad);

            return Json(escuelas);
        }

        // 11/06/2014 - DI CAMPLI LEANDRO - LISTAR ETAPAS POR PROGRAMA
        [HttpPost]
        public ActionResult TraerEtapas(int idPrograma)
        {
            if (idPrograma == (int)Enums.TipoFicha.Universitaria)
            {
                idPrograma = (int)Enums.TipoFicha.Terciaria;
            }
            else
            {
                idPrograma = (int)Enums.TipoFicha.Universitaria;
            }

            var etapas = _etapaServicio.getEtapasPrograma(idPrograma);

            var tablaretorno = new StringBuilder();


            tablaretorno.Append("<select id='SelEtapa'>");
            foreach (var etapa in etapas)
            {

                tablaretorno.Append("<option value=" + etapa.ID_ETAPA + ">" + etapa.N_ETAPA +  "</option>");
                
            }

            tablaretorno.Append("</select>");

            return Json(tablaretorno.ToString().Replace('$', '"'));
        }

        [HttpPost]
        public ActionResult ValidarProyecto(string NombreProyecto)
        {
            IProyectoVista proyecto = _proyectoServicio.GetProyectos(NombreProyecto);

            var tablaretorno = new StringBuilder();


            tablaretorno.Append(
                "<thead><tr><th style='height: 30px; width: 350px;'><div align='center'>Nombre Proyecto</div></th>");
            tablaretorno.Append("<th style='width: 100px;'><div align='center'>Cant. Capacitar</div></th>");
            tablaretorno.Append("<th style='width: 100px;'><div align='center'>Inicio</div></th>");
            tablaretorno.Append("<th style='width: 100px;'><div align='center'>Fin</div></th>");
            tablaretorno.Append("<th style='width: 80px;'><div align='center'>Duración (meses)</div></th>");
            tablaretorno.Append("<th style='width: 80px;'><div align='center'>Acción</div></th></tr></thead><tbody>");

            foreach (var proy in proyecto.Proyectos)
            {
                tablaretorno.Append("<tr><td style='height: 28px;  text-align: left;'>" + proy.NombreProyecto + "</td>");
                tablaretorno.Append("<td style='height: 28px;  text-align: left;'>" + proy.CantidadAcapacitar + "</td>");
                tablaretorno.Append("<td style='height: 28px;  text-align: left;'>" + proy.FechaInicioProyecto + "</td>");
                tablaretorno.Append("<td style='height: 28px;  text-align: left;'>" + proy.FechaFinProyecto + "</td>");
                tablaretorno.Append("<td style='height: 28px;  text-align: right;'>" + proy.MesesDuracionProyecto + "</td>");

                tablaretorno.Append(
                    "<td style='height: 28px;  text-align: center;'><img  src='../../Content/images/icon_derecha.gif' style='cursor: pointer;' width='18'  height='18' alt='' onclick='SeleccionarProyecto(" +
                    proy.IdProyecto + ",$" + proy.NombreProyecto + "$, $" + proy.CantidadAcapacitar + "$, $" +
                    proy.FechaInicioProyecto + "$,$" + proy.FechaFinProyecto + "$, $" + proy.MesesDuracionProyecto + "$);'  /></td>");
            }

            tablaretorno.Append("</tr></tbody>");

            return Json(tablaretorno.ToString().Replace('$', '"'));
        }

        // 15/04/2015
        [HttpPost]
        public ActionResult TraerEscuela(int idEscuela)
        {
            
            var escuela = _fichaservicio.TraerEscuela(idEscuela);

            return Json(escuela);
        }

        [HttpPost]
        public ActionResult TraerCursos(string NombreCurso)
        {
            ICursoVista curso = _CursoServicio.GetCursos(NombreCurso);

            var tablaretorno = new StringBuilder();


            tablaretorno.Append(
                "<thead><tr><th style='height: 30px; width: 80px;'><div align='left'>Nro Curso</div></th>");
            tablaretorno.Append("<th style='width: 270px;'><div align='left'>Nombre Curso</div></th>");
            tablaretorno.Append("<th style='width: 100px;'><div align='left'>Inicio</div></th>");
            tablaretorno.Append("<th style='width: 100px;'><div align='left'>Fin</div></th>");
            tablaretorno.Append("<th style='width: 80px;'><div align='left'>Expediente Leg</div></th>");
            tablaretorno.Append("<th style='width: 80px;'><div align='left'>Acción</div></th></tr></thead><tbody>");

            foreach (var cur in curso.cursos)
            {
                tablaretorno.Append("<tr><td style='height: 28px;  text-align: left;'>" + Convert.ToString(cur.NRO_COND_CURSO == null ? "" : cur.NRO_COND_CURSO.ToString()) + "</td>");
                tablaretorno.Append("<td style='height: 28px;  text-align: left;'>" + cur.N_CURSO + "</td>");
                tablaretorno.Append("<td style='height: 28px;  text-align: left;'>" + cur.F_INICIO + "</td>");
                tablaretorno.Append("<td style='height: 28px;  text-align: left;'>" + cur.F_FIN + "</td>");
                tablaretorno.Append("<td style='height: 28px;  text-align: left;'>" + cur.EXPEDIENTE_LEG + "</td>");
                tablaretorno.Append(
                    "<td style='height: 28px;  text-align: center;'><img  src='../../Content/images/icon_derecha.gif' style='cursor: pointer;' width='18'  height='18' alt='' onclick='SeleccionarCurso(" +
                    cur.ID_CURSO + ",$" + Convert.ToString(cur.NRO_COND_CURSO == null ? "" : cur.NRO_COND_CURSO.ToString()) + "$,$" + cur.N_CURSO + "$, $" + cur.F_INICIO + "$, $" +
                    cur.F_FIN + "$,$" + cur.DURACION + "$, $" + cur.EXPEDIENTE_LEG + "$, $" + cur.DOC_APELLIDO + "$, $" +
                    cur.DOC_NOMBRE + "$,$" + cur.DOC_CELULAR + "$, $" + cur.DOC_N_LOCALIDAD + "$, $" +
                    cur.DOC_MAIL + "$,$" + cur.TUT_APELLIDO + "$, $" + cur.TUT_NOMBRE + "$, $" +
                    cur.TUT_CELULAR + "$,$" + cur.TUT_N_LOCALIDAD + "$, $" +
                    cur.TUT_MAIL + "$);'  /></td>");
            }

            tablaretorno.Append("</tr></tbody>");

            return Json(tablaretorno.ToString().Replace('$', '"'));
        }


        [HttpPost]
        public ActionResult CursosPorNro(int NroCurso)
        {
            ICursoVista curso = _CursoServicio.GetCursos(NroCurso);

            var tablaretorno = new StringBuilder();


            tablaretorno.Append(
                "<thead><tr><th style='height: 30px; width: 80px;'><div align='left'>Nro Curso</div></th>");
            tablaretorno.Append("<th style='width: 270px;'><div align='left'>Nombre Curso</div></th>");
            tablaretorno.Append("<th style='width: 100px;'><div align='left'>Inicio</div></th>");
            tablaretorno.Append("<th style='width: 100px;'><div align='left'>Fin</div></th>");
            tablaretorno.Append("<th style='width: 80px;'><div align='left'>Expediente Leg</div></th>");
            tablaretorno.Append("<th style='width: 80px;'><div align='left'>Acción</div></th></tr></thead><tbody>");

            foreach (var cur in curso.cursos)
            {
                tablaretorno.Append("<tr><td style='height: 28px;  text-align: left;'>" + Convert.ToString(cur.NRO_COND_CURSO == null ? "" : cur.NRO_COND_CURSO.ToString())  + "</td>");
                tablaretorno.Append("<td style='height: 28px;  text-align: left;'>" + cur.N_CURSO + "</td>");
                tablaretorno.Append("<td style='height: 28px;  text-align: left;'>" + cur.F_INICIO + "</td>");
                tablaretorno.Append("<td style='height: 28px;  text-align: left;'>" + cur.F_FIN + "</td>");
                tablaretorno.Append("<td style='height: 28px;  text-align: left;'>" + cur.EXPEDIENTE_LEG + "</td>");
                tablaretorno.Append(
                    "<td style='height: 28px;  text-align: center;'><img  src='../../Content/images/icon_derecha.gif' style='cursor: pointer;' width='18'  height='18' alt='' onclick='SeleccionarCurso(" +
                    cur.ID_CURSO + ",$" + Convert.ToString(cur.NRO_COND_CURSO == null ? "" : cur.NRO_COND_CURSO.ToString()) + "$,$" + cur.N_CURSO + "$, $" + cur.F_INICIO + "$, $" +
                    cur.F_FIN + "$,$" + cur.DURACION + "$, $" + cur.EXPEDIENTE_LEG + "$, $" + cur.DOC_APELLIDO + "$, $" +
                    cur.DOC_NOMBRE + "$,$" + cur.DOC_CELULAR + "$, $" + cur.DOC_N_LOCALIDAD + "$, $" +
                    cur.DOC_MAIL + "$,$" + cur.TUT_APELLIDO + "$, $" + cur.TUT_NOMBRE + "$, $" +
                    cur.TUT_CELULAR + "$,$" + cur.TUT_N_LOCALIDAD + "$, $" +
                    cur.TUT_MAIL + "$);'  /></td>");
            }

            tablaretorno.Append("</tr></tbody>");

            return Json(tablaretorno.ToString().Replace('$', '"'));
        }


        [HttpPost]
        public ActionResult obtenerFacilitadorMonitor(string dni)
        {
            IActorVista actor = _actorServicio.GetFacilitadorMonitorDNI(dni);

            var tablaretorno = new StringBuilder();


            tablaretorno.Append(
                "<thead><tr><th style='height: 80px; width: 80px;'><div align='left'>DNI</div></th>");
            tablaretorno.Append("<th style='width: 200px;'><div align='left'>Apellido</div></th>");
            tablaretorno.Append("<th style='width: 200px;'><div align='left'>Nombre</div></th>");
            tablaretorno.Append("<th style='width: 150px;'><div align='left'>Rol</div></th>");
            tablaretorno.Append("<th style='width: 80px;'><div align='left'>Celular</div></th>");
            tablaretorno.Append("<th style='width: 150px;'><div align='left'>Mail</div></th>");
            tablaretorno.Append("<th style='width: 80px;'><div align='left'>Acción</div></th></tr></thead><tbody>");

            foreach (var A in actor.Actores)
            {
                tablaretorno.Append("<tr><td style='height: 28px;  text-align: left;'>" + A.DNI + "</td>");
                tablaretorno.Append("<td style='height: 28px;  text-align: left;'>" + A.APELLIDO + "</td>");
                tablaretorno.Append("<td style='height: 28px;  text-align: left;'>" + A.NOMBRE + "</td>");
                tablaretorno.Append("<td style='height: 28px;  text-align: left;'>" + A.N_ACTOR_ROL + "</td>");
                tablaretorno.Append("<td style='height: 28px;  text-align: left;'>" + A.CELULAR + "</td>");
                tablaretorno.Append("<td style='height: 28px;  text-align: left;'>" + A.MAIL + "</td>");

                
                tablaretorno.Append(
                    "<td style='height: 28px;  text-align: center;'><img  src='../../Content/images/icon_derecha.gif' style='cursor: pointer;' width='18'  height='18' alt='' onclick='SeleccionarActor(" +
                    A.ID_ACTOR + ",$" + A.DNI + "$,$" + A.APELLIDO + "$, $" + A.NOMBRE + "$, $" +
                    A.CELULAR + "$,$" + A.MAIL + "$, $" + A.N_BARRIO + "$, $" + A.N_LOCALIDAD + "$," + A.ID_ACTOR_ROL + ");'  /></td>");
            }

            tablaretorno.Append("</tr></tbody>");

            return Json(tablaretorno.ToString().Replace('$', '"'));
        }

        [HttpPost]
        public ActionResult obtenerActor(string dni, int idRolActor)
        {
            IActorVista actor = _actorServicio.GetActorDNI(dni,idRolActor);

            var tablaretorno = new StringBuilder();
            string rolActor = string.Empty;

            switch (idRolActor)
            { 
                case  (int)Enums.Actor_Rol.GESTOR:
                    rolActor = "GESTOR";
                    break;
            
            }


            tablaretorno.Append(
                "<thead><tr><th style='height: 80px; width: 80px;'><div align='left'>DNI</div></th>");
            tablaretorno.Append("<th style='width: 200px;'><div align='left'>Apellido</div></th>");
            tablaretorno.Append("<th style='width: 200px;'><div align='left'>Nombre</div></th>");
            tablaretorno.Append("<th style='width: 150px;'><div align='left'>Rol</div></th>");
            tablaretorno.Append("<th style='width: 80px;'><div align='left'>Celular</div></th>");
            tablaretorno.Append("<th style='width: 150px;'><div align='left'>Mail</div></th>");
            tablaretorno.Append("<th style='width: 80px;'><div align='left'>Acción</div></th></tr></thead><tbody>");

            foreach (var A in actor.Actores)
            {
                tablaretorno.Append("<tr><td style='height: 28px;  text-align: left;'>" + A.DNI + "</td>");
                tablaretorno.Append("<td style='height: 28px;  text-align: left;'>" + A.APELLIDO + "</td>");
                tablaretorno.Append("<td style='height: 28px;  text-align: left;'>" + A.NOMBRE + "</td>");
                tablaretorno.Append("<td style='height: 28px;  text-align: left;'>" + A.N_ACTOR_ROL + "</td>");
                tablaretorno.Append("<td style='height: 28px;  text-align: left;'>" + A.CELULAR + "</td>");
                tablaretorno.Append("<td style='height: 28px;  text-align: left;'>" + A.MAIL + "</td>");


                tablaretorno.Append(
                    "<td style='height: 28px;  text-align: center;'><img  src='../../Content/images/icon_derecha.gif' style='cursor: pointer;' width='18'  height='18' alt='' onclick='SeleccionarActor(" +
                    A.ID_ACTOR + ",$" + A.DNI + "$,$" + A.APELLIDO + "$, $" + A.NOMBRE + "$, $" +
                    A.CELULAR + "$,$" + A.MAIL + "$, $" + A.N_BARRIO + "$, $" + A.N_LOCALIDAD + "$," + A.ID_ACTOR_ROL + ");'  /></td>");
            }

            tablaretorno.Append("</tr></tbody>");

            return Json(tablaretorno.ToString().Replace('$', '"'));
        }

        [HttpPost]
        public ActionResult obtenerONG(string cuit)
        {
            IOngsVista ong = new OngsVista();
            ong.ongs = _ongServicio.getOngs(cuit);

            var tablaretorno = new StringBuilder();
            string rolActor = string.Empty;


            tablaretorno.Append(
                "<thead><tr><th style='height: 30px; width: 100px;'><div align='left'>Cuit</div></th>");
            tablaretorno.Append("<th style='width: 280px;'><div align='left'>Nombre</div></th>");
            tablaretorno.Append("<th style='width: 80px;'><div align='left'>Teléfono</div></th>");
            tablaretorno.Append("<th style='width: 150px;'><div align='left'>Mail</div></th>");
            tablaretorno.Append("<th style='width: 80px;'><div align='left'>Acción</div></th></tr></thead><tbody>");

            foreach (var row in ong.ongs)
            {
                tablaretorno.Append("<tr><td style='height: 28px;  text-align: left;'>" + row.CUIT + "</td>");
                tablaretorno.Append("<td style='height: 28px;  text-align: left;'>" + row.N_NOMBRE + "</td>");
                tablaretorno.Append("<td style='height: 28px;  text-align: left;'>" + row.TELEFONO + "</td>");
                tablaretorno.Append("<td style='height: 28px;  text-align: left;'>" + row.MAIL_ONG + "</td>");
                tablaretorno.Append(
                    "<td style='height: 28px;  text-align: center;'><img  src='../../Content/images/icon_derecha.gif' style='cursor: pointer;' width='18'  height='18' alt='' onclick='SeleccionaONG(" +
                    row.ID_ONG + ",$" + row.CUIT + "$,$" + row.N_NOMBRE + "$, $" + row.CELULAR + "$, $" +
                    row.TELEFONO + "$,$" + row.MAIL_ONG + "$, $" + row.RES_APELLIDO + "$, $" + row.RES_NOMBRE + "$, $"
                    + row.RES_CELULAR + "$,$" + row.RES_N_LOCALIDAD + "$, $" +
                    row.RES_MAIL + "$, $" + row.CON_APELLIDO + "$, $" + row.CON_NOMBRE + "$, $" +
                    row.CON_CELULAR + "$,$" + row.CON_N_LOCALIDAD + "$, $" +
                    row.CON_MAIL + "$);'  /></td>");
            }

            tablaretorno.Append("</tr></tbody>");

            return Json(tablaretorno.ToString().Replace('$', '"'));
        }

        [HttpPost]
        public ActionResult obtenerONGNom(string nombre)
        {
            IOngsVista ong = new OngsVista();
            ong.ongs = _ongServicio.getOngsNom(nombre);

            var tablaretorno = new StringBuilder();
            string rolActor = string.Empty;


            tablaretorno.Append(
                "<thead><tr><th style='height: 30px; width: 100px;'><div align='left'>Cuit</div></th>");
            tablaretorno.Append("<th style='width: 280px;'><div align='left'>Nombre</div></th>");
            tablaretorno.Append("<th style='width: 80px;'><div align='left'>Teléfono</div></th>");
            tablaretorno.Append("<th style='width: 150px;'><div align='left'>Mail</div></th>");
            tablaretorno.Append("<th style='width: 80px;'><div align='left'>Acción</div></th></tr></thead><tbody>");

            foreach (var row in ong.ongs)
            {
                tablaretorno.Append("<tr><td style='height: 28px;  text-align: left;'>" + row.CUIT + "</td>");
                tablaretorno.Append("<td style='height: 28px;  text-align: left;'>" + row.N_NOMBRE + "</td>");
                tablaretorno.Append("<td style='height: 28px;  text-align: left;'>" + row.TELEFONO + "</td>");
                tablaretorno.Append("<td style='height: 28px;  text-align: left;'>" + row.MAIL_ONG + "</td>");
                tablaretorno.Append(
                    "<td style='height: 28px;  text-align: center;'><img  src='../../Content/images/icon_derecha.gif' style='cursor: pointer;' width='18'  height='18' alt='' onclick='SeleccionaONG(" +
                    row.ID_ONG + ",$" + row.CUIT + "$,$" + row.N_NOMBRE + "$, $" + row.CELULAR + "$, $" +
                    row.TELEFONO + "$,$" + row.MAIL_ONG + "$, $" + row.RES_APELLIDO + "$, $" + row.RES_NOMBRE + "$, $"
                    + row.RES_CELULAR + "$,$" + row.RES_N_LOCALIDAD + "$, $" +
                    row.RES_MAIL + "$, $" + row.CON_APELLIDO + "$, $" + row.CON_NOMBRE + "$, $" +
                    row.CON_CELULAR + "$,$" + row.CON_N_LOCALIDAD + "$, $" +
                    row.CON_MAIL + "$);'  /></td>");
            }

            tablaretorno.Append("</tr></tbody>");

            return Json(tablaretorno.ToString().Replace('$', '"'));
        }

        [HttpPost]
        public ActionResult AddOngFicha(string nombre, string cuit, string celular, string telefono, string mail)
        {
            int idong = 0;
                IOngVista ongvista = new OngVista()
                {
                    N_NOMBRE = nombre,
                    CUIT = cuit,
                    CELULAR = celular,
                    TELEFONO = telefono,
                    MAIL_ONG = mail
                };

               // _sesdesservicios.AddSedeFromInscripcion(sedevista);
                idong = _ongServicio.AddOng(ongvista);

                IOngsVista ong = new OngsVista();
                ong.ongs = _ongServicio.GetOngsId(idong);

            var tablaretorno = new StringBuilder();

            tablaretorno.Append(
                 "<thead><tr><th style='height: 30px; width: 100px;'><div align='left'>Cuit</div></th>");
            tablaretorno.Append("<th style='width: 280px;'><div align='left'>Nombre</div></th>");
            tablaretorno.Append("<th style='width: 80px;'><div align='left'>Teléfono</div></th>");
            tablaretorno.Append("<th style='width: 150px;'><div align='left'>Mail</div></th>");
            tablaretorno.Append("<th style='width: 80px;'><div align='left'>Acción</div></th></tr></thead><tbody>");

            foreach (var row in ong.ongs)
            {
                tablaretorno.Append("<tr><td style='height: 28px;  text-align: left;'>" + row.CUIT + "</td>");
                tablaretorno.Append("<td style='height: 28px;  text-align: left;'>" + row.N_NOMBRE + "</td>");
                tablaretorno.Append("<td style='height: 28px;  text-align: left;'>" + row.TELEFONO + "</td>");
                tablaretorno.Append("<td style='height: 28px;  text-align: left;'>" + row.MAIL_ONG + "</td>");
                tablaretorno.Append(
                    "<td style='height: 28px;  text-align: center;'><img  src='../../Content/images/icon_derecha.gif' style='cursor: pointer;' width='18'  height='18' alt='' onclick='SeleccionaONG(" +
                    row.ID_ONG + ",$" + row.CUIT + "$,$" + row.N_NOMBRE + "$, $" + row.CELULAR + "$, $" +
                    row.TELEFONO + "$,$" + row.MAIL_ONG + "$, $" + row.RES_APELLIDO + "$, $" + row.RES_NOMBRE + "$, $"
                    + row.RES_CELULAR + "$,$" + row.RES_N_LOCALIDAD + "$, $" +
                    row.RES_MAIL + "$, $" + row.CON_APELLIDO + "$, $" + row.CON_NOMBRE + "$, $" +
                    row.CON_CELULAR + "$,$" + row.CON_N_LOCALIDAD + "$, $" +
                    row.CON_MAIL + "$);'  /></td>");
            }

            tablaretorno.Append("</tr></tbody>");

            return Json(tablaretorno.ToString().Replace('$', '"'));
        }

        [HttpPost]
        public ActionResult obtenerSubProg(string nombre)
        {
            ISubprogramasVista subP = new SubprogramasVista();
            subP.Subprogramas = _subprograma.GetSubprogramasN(nombre);

            var tablaretorno = new StringBuilder();
           


            tablaretorno.Append(
                "<thead><tr><th style='height: 30px; width: 100px;'><div align='left'>ID</div></th>");
            tablaretorno.Append("<th style='width: 280px;'><div align='left'>Nombre</div></th>");
            tablaretorno.Append("<th style='width: 80px;'><div align='left'>Descripción</div></th>");
            tablaretorno.Append("<th style='width: 150px;'><div align='left'>Monto</div></th>");
            tablaretorno.Append("<th style='width: 80px;'><div align='left'>Acción</div></th></tr></thead><tbody>");

            foreach (var row in subP.Subprogramas)
            {
                tablaretorno.Append("<tr><td style='height: 28px;  text-align: left;'>" + row.IdSubprograma + "</td>");
                tablaretorno.Append("<td style='height: 28px;  text-align: left;'>" + row.NombreSubprograma + "</td>");
                tablaretorno.Append("<td style='height: 28px;  text-align: left;'>" + row.Descripcion + "</td>");
                tablaretorno.Append("<td style='height: 28px;  text-align: left;'>" + row.monto + "</td>");
                tablaretorno.Append(
                    "<td style='height: 28px;  text-align: center;'><img  src='../../Content/images/icon_derecha.gif' style='cursor: pointer;' width='18' height='18' alt='' onclick='SeleccionaSubP(" +
                    row.IdSubprograma + ",$" + row.NombreSubprograma + "$,$" + row.Descripcion + "$, $" + row.monto + "$);'  /></td></tr>");
            }

            tablaretorno.Append("</tbody>");

            return Json(tablaretorno.ToString().Replace('$', '"'));
        }

        [HttpPost]
        public ActionResult obtenerEscuela(string nombre)
        {
            IEscuelasVista esc = new EscuelasVista();
            esc.escuelas = _escuela.GetEscuelasLst(nombre);

            var tablaretorno = new StringBuilder();



            tablaretorno.Append(
                "<thead><tr><th style='height: 30px; width: 300px;'><div align='left'>Nombre</div></th>");
            //tablaretorno.Append("<th style='width: 280px;'><div align='left'>Nombre</div></th>");
            //tablaretorno.Append("<th style='width: 80px;'><div align='left'>Calle</div></th>");
            //tablaretorno.Append("<th style='width: 150px;'><div align='left'>Número</div></th>");
            //tablaretorno.Append("<th style='width: 150px;'><div align='left'>Teléfono</div></th>");
            tablaretorno.Append("<th style='width: 100px;'><div align='left'>Cue</div></th>");
            tablaretorno.Append("<th style='width: 80px;'><div align='left'>Acción</div></th></tr></thead><tbody>");

            foreach (var row in esc.escuelas)
            {
                tablaretorno.Append("<tr><td style='height: 28px;  text-align: left;'>" + row.Nombre_Escuela + "</td>");
                //tablaretorno.Append("<td style='height: 28px;  text-align: left;'>" + row.Nombre_Escuela + "</td>");
                //tablaretorno.Append("<td style='height: 28px;  text-align: left;'>" + row.CALLE + "</td>");
                //tablaretorno.Append("<td style='height: 28px;  text-align: left;'>" + row.NUMERO + "</td>");
                //tablaretorno.Append("<td style='height: 28px;  text-align: left;'>" + row.TELEFONO + "</td>");
                tablaretorno.Append("<td style='height: 28px;  text-align: left;'>" + row.Cue + "</td>");
                tablaretorno.Append("<td style='height: 28px;  text-align: center;'><img  src='../../Content/images/icon_derecha.gif' style='cursor: pointer;' width='18'  height='18' alt='' onclick='SeleccionaEscuela(" +
                    row.Id_Escuela + ",$" + row.Nombre_Escuela + "$,$" + row.CALLE + "$,$" + row.NUMERO + "$,$" + row.TELEFONO + "$,$" + row.Cue + "$,$" + row.regise + "$);'  /></td></tr>");
               
            }

            tablaretorno.Append("</tbody>");

            return Json(tablaretorno.ToString().Replace('$', '"'));
        }

        [HttpPost]
        public ActionResult CursosEscuela(string NombreCurso)
        {
            ICursoVista curso = _CursoServicio.GetCursosEscuela(NombreCurso);

            var tablaretorno = new StringBuilder();


            tablaretorno.Append(
                "<thead><tr><th style='height: 30px; width: 200px;'><div align='left'>Nombre Curso</div></th>");
            tablaretorno.Append("<th style='width: 80px;'><div align='left'>Inicio</div></th>");
            tablaretorno.Append("<th style='width: 80px;'><div align='left'>Fin</div></th>");
            tablaretorno.Append("<th style='width: 200px;'><div align='left'>Escuela</div></th>");
            tablaretorno.Append("<th style='width: 80px;'><div align='left'>CUE</div></th>");
            tablaretorno.Append("<th style='width: 80px;'><div align='left'>REGISE</div></th>");
            tablaretorno.Append("<th style='width: 80px;'><div align='left'>Acción</div></th></tr></thead><tbody>");

            foreach (var cur in curso.cursos)
            {

                tablaretorno.Append("<td style='height: 28px;  text-align: left;'>" + cur.N_CURSO + "</td>");
                tablaretorno.Append("<td style='height: 28px;  text-align: left;'>" + cur.F_INICIO + "</td>");
                tablaretorno.Append("<td style='height: 28px;  text-align: left;'>" + cur.F_FIN + "</td>");
                tablaretorno.Append("<td style='height: 28px;  text-align: left;'>" + cur.NEscuelaCurso + "</td>");
                tablaretorno.Append("<td style='height: 28px;  text-align: left;'>" + cur.CueCurso + "</td>");
                tablaretorno.Append("<td style='height: 28px;  text-align: left;'>" + cur.regiseCurso + "</td>");
                tablaretorno.Append(
                    "<td style='height: 28px;  text-align: center;'><img  src='../../Content/images/icon_derecha.gif' style='cursor: pointer;' width='18'  height='18' alt='' onclick='SeleCursoEsc(" +
                    cur.ID_CURSO + ",$" + cur.N_CURSO + "$, $" + cur.F_INICIO + "$, $" +
                    cur.F_FIN + "$,$" + cur.DURACION  + "$,$" + cur.MESES  + "$,$" + cur.NEscuelaCurso + "$, $" + cur.CueCurso + "$, $" +
                    cur.regiseCurso + "$);'  /></td></tr>");
            }

            tablaretorno.Append("</tbody>");

            return Json(tablaretorno.ToString().Replace('$', '"'));
        }



        [HttpPost]
        public ActionResult AgregarEsc(string nombre, string calle, string nro, string telefono, string cue, string Regise)
        {

            IEscuelaVista escuelavista = new EscuelaVista()
                {
                    Nombre_Escuela = nombre,
                    NUMERO = nro,
                    CALLE = calle,
                    TELEFONO = telefono,
                    Cue = cue,
                    regise = Regise,
                };

            _escuela.AddEscuela(escuelavista);


            IEscuelasVista esc = new EscuelasVista();
            esc.escuelas = _escuela.GetEscuelasLst(nombre);

            var tablaretorno = new StringBuilder();



            tablaretorno.Append(
                "<thead><tr><th style='height: 30px; width: 300px;'><div align='left'>Nombre</div></th>");
            //tablaretorno.Append("<th style='width: 280px;'><div align='left'>Nombre</div></th>");
            //tablaretorno.Append("<th style='width: 80px;'><div align='left'>Calle</div></th>");
            //tablaretorno.Append("<th style='width: 150px;'><div align='left'>Número</div></th>");
            //tablaretorno.Append("<th style='width: 150px;'><div align='left'>Teléfono</div></th>");
            tablaretorno.Append("<th style='width: 100px;'><div align='left'>Cue</div></th>");
            tablaretorno.Append("<th style='width: 80px;'><div align='left'>Acción</div></th></tr></thead><tbody>");

            foreach (var row in esc.escuelas)
            {
                tablaretorno.Append("<tr><td style='height: 28px;  text-align: left;'>" + row.Nombre_Escuela + "</td>");
                //tablaretorno.Append("<td style='height: 28px;  text-align: left;'>" + row.Nombre_Escuela + "</td>");
                //tablaretorno.Append("<td style='height: 28px;  text-align: left;'>" + row.CALLE + "</td>");
                //tablaretorno.Append("<td style='height: 28px;  text-align: left;'>" + row.NUMERO + "</td>");
                //tablaretorno.Append("<td style='height: 28px;  text-align: left;'>" + row.TELEFONO + "</td>");
                tablaretorno.Append("<td style='height: 28px;  text-align: left;'>" + row.Cue + "</td>");
                tablaretorno.Append("<td style='height: 28px;  text-align: center;'><img  src='../../Content/images/icon_derecha.gif' style='cursor: pointer;' width='18'  height='18' alt='' onclick='SeleccionaEscuela(" +
                    row.Id_Escuela + ",$" + row.Nombre_Escuela + "$,$" + row.CALLE + "$,$" + row.NUMERO + "$,$" + row.TELEFONO + "$,$" + row.Cue + "$,$" + row.regise + "$);'  /></td></tr>");

            }

            tablaretorno.Append("</tr></tbody>");

            return Json(tablaretorno.ToString().Replace('$', '"'));
        }


        [HttpPost]
        public ActionResult getEscuelaCurso(string nombre)
        {
            IEscuelasVista esc = new EscuelasVista();
            esc.escuelas = _escuela.GetEscuelasLst(nombre);

            var tablaretorno = new StringBuilder();



            tablaretorno.Append(
                "<thead><tr><th style='height: 30px; width: 300px;'><div align='left'>Nombre</div></th>");
            tablaretorno.Append("<th style='width: 100px;'><div align='left'>Cue</div></th>");
            tablaretorno.Append("<th style='width: 80px;'><div align='left'>Acción</div></th></tr></thead><tbody>");

            foreach (var row in esc.escuelas)
            {
                tablaretorno.Append("<tr><td style='height: 28px;  text-align: left;'>" + row.Nombre_Escuela + "</td>");
                tablaretorno.Append("<td style='height: 28px;  text-align: left;'>" + row.Cue + "</td>");
                tablaretorno.Append("<td style='height: 28px;  text-align: center;'><img  src='../../Content/images/icon_derecha.gif' style='cursor: pointer;' width='18'  height='18' alt='' onclick='SeleEscuelaCurso(" +
                    row.Id_Escuela + ",$" + row.Nombre_Escuela + "$,$" + row.CALLE + "$,$" + row.NUMERO + "$,$" + row.TELEFONO + "$,$" + row.Cue + "$,$" + row.regise + "$);'  /></td></tr>");

            }

            tablaretorno.Append("</tbody>");

            return Json(tablaretorno.ToString().Replace('$', '"'));
        }


        [HttpPost]
        public ActionResult AgregarCur(string nombre, string duracion, string meses, DateTime? inicio, DateTime? fin, int? idEscuela)
        {

            ICursoVista cursovista = new CursoVista()
            {
                N_CURSO = nombre,
                DURACION = duracion,
                MESES = meses,
                F_INICIO = inicio,
                F_FIN = fin,
                ID_ESCUELA = idEscuela,
            };

            _CursoServicio.AddCurso(cursovista);

            ICursoVista curso = _CursoServicio.GetCursosEscuela(nombre);

            var tablaretorno = new StringBuilder();


            tablaretorno.Append(
                "<thead><tr><th style='height: 30px; width: 200px;'><div align='left'>Nombre Curso</div></th>");
            tablaretorno.Append("<th style='width: 80px;'><div align='left'>Inicio</div></th>");
            tablaretorno.Append("<th style='width: 80px;'><div align='left'>Fin</div></th>");
            tablaretorno.Append("<th style='width: 200px;'><div align='left'>Escuela</div></th>");
            tablaretorno.Append("<th style='width: 80px;'><div align='left'>CUE</div></th>");
            tablaretorno.Append("<th style='width: 80px;'><div align='left'>REGISE</div></th>");
            tablaretorno.Append("<th style='width: 80px;'><div align='left'>Acción</div></th></tr></thead><tbody>");

            foreach (var cur in curso.cursos)
            {

                tablaretorno.Append("<td style='height: 28px;  text-align: left;'>" + cur.N_CURSO + "</td>");
                tablaretorno.Append("<td style='height: 28px;  text-align: left;'>" + cur.F_INICIO + "</td>");
                tablaretorno.Append("<td style='height: 28px;  text-align: left;'>" + cur.F_FIN + "</td>");
                tablaretorno.Append("<td style='height: 28px;  text-align: left;'>" + cur.NEscuelaCurso + "</td>");
                tablaretorno.Append("<td style='height: 28px;  text-align: left;'>" + cur.CueCurso + "</td>");
                tablaretorno.Append("<td style='height: 28px;  text-align: left;'>" + cur.regiseCurso + "</td>");
                tablaretorno.Append(
                    "<td style='height: 28px;  text-align: center;'><img  src='../../Content/images/icon_derecha.gif' style='cursor: pointer;' width='18'  height='18' alt='' onclick='SeleCursoEsc(" +
                    cur.ID_CURSO + ",$" + cur.N_CURSO + "$, $" + cur.F_INICIO + "$, $" +
                    cur.F_FIN + "$,$" + cur.DURACION + "$,$" + cur.MESES + "$,$" + cur.NEscuelaCurso + "$, $" + cur.CueCurso + "$, $" +
                    cur.regiseCurso + "$);'  /></td></tr>");
            }

            tablaretorno.Append("</tbody>");

            return Json(tablaretorno.ToString().Replace('$', '"'));
        }

        #region "Re Empadronar Fichas"
        //02/01/2018 - DI CAMPLI LEANDRO - Re Empadronar Fichas
        public ActionResult ReEmpadronarFichas(/*FichasVista modelo*/)
        {

            //IFichaVista vista = _fichaservicio.GetFichas();

            return View(_fichaservicio.indexReempadronarFichas());

        }

        [HttpPost]
        public ActionResult ArchEmpadronar(HttpPostedFileBase archivoEmpadronar, FichasVista model)
        {

            IFichasVista vista = _fichaservicio.SubirArchivoFichas(archivoEmpadronar, model);

            //TempData["VistaSubida"] = vista;

            return View("ReEmpadronarFichas", vista);

        }

        //[HttpPost]
        public ActionResult EmpadronarFichasXLS(string ExcelNombre)
        {
            IFichasVista vista = _fichaservicio.CargarMasivaFichasXLS(ExcelNombre);

            return View("ReEmpadronarFichas", vista);
        }

        [HttpPost]
        public ActionResult ReEmpadronarFichas(FichasVista modelo)
        {
            modelo = (FichasVista)TempData["FichasEmpadronar"];
            int cant = 0;
            _fichaservicio.ReEmpadronarFichas(modelo);
            

            modelo.cantProcesados = modelo.Empadronados;
            return View("ReEmpadronarFichas", modelo);
        }

        [HttpPost]
        public ActionResult ExportEmpadronados(FichasVista model)
        {
            model = (FichasVista)TempData["FichasEmpadronar"];
            TempData["FichasEmpadronar"] = model;
            return File(_fichaservicio.ExportEmpadronados(model),
                        "application/vnd.ms-excel", "FichasEmpadronadas.xls");
        }


        #endregion

        #region "Gestion Estados Fichas"
        //18/02/2013 - DI CAMPLI LEANDRO - GESTOR ESTADOS FICHAS
        public ActionResult GestorEstadosFichas()
        {

            //IFichaVista vista = _fichaservicio.GetFichas();

            return View(_fichaservicio.indexGestorEstadosFichas());

        }

        [HttpPost]
        public ActionResult SubirArchivoGestor(HttpPostedFileBase/*string*/ archivoFichas, FichasVista model)
        {
            IFichasVista vistaPivot = _fichaservicio.indexGestorEstadosFichas();
            IFichasVista vista = _fichaservicio.SubirArchivoFichas(archivoFichas, model);
            vista.ComboEstadoFicha = vistaPivot.ComboEstadoFicha;
            vista.ComboTipoFicha = vistaPivot.ComboTipoFicha;
            vista.ComboEstadoFicha.Selected = Convert.ToString(model.idEstadoFicha);
            vista.idEstadoFicha = model.idEstadoFicha;
            //TempData["VistaSubida"] = vista;

            return View("GestorEstadosFichas", vista);
        }

        public ActionResult ProcesarXlsGestor(FichasVista model)
        {
            IFichasVista vista = _fichaservicio.CargarMasivaFichasXLS(model.ExcelNombre);
            IFichasVista vistaPivot = _fichaservicio.indexGestorEstadosFichas();
            
            vista.ComboEstadoFicha = vistaPivot.ComboEstadoFicha;
            vista.ComboTipoFicha = vistaPivot.ComboTipoFicha;
            vista.idEstadoFicha = model.idEstadoFicha;
            vista.ComboEstadoFicha.Selected = Convert.ToString(model.idEstadoFicha);
            

            return View("GestorEstadosFichas", vista);
        }



        #endregion

    }
}
