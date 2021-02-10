using System.Collections.Generic;
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
using System.Web;
using System;

namespace SGE.Web.Controllers
{
    [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmEmpresas)]
    public class EmpresasController : BaseController
    {
        private readonly IEmpresaServicio _empresaServicio;
        private readonly ISedeServicio _sedeServicio;

        public EmpresasController()
            : base((int)Formularios.Empresa)
        {
            _empresaServicio = new EmpresaServicio();
            _sedeServicio = new SedeServicio();
        }

        public ActionResult Index()
        {
            var vista = _empresaServicio.GetIndex();
            return View(vista);
        }

        [HttpPost]
        public ActionResult BuscarEmpresa(EmpresasVista model)
        {
            return View("Index", _empresaServicio.GetEmpresas(model.DescripcionBusqueda, model.CuitBusqueda));
        }

        public ActionResult IndexPager(Pager pager, EmpresasVista model)
        {
            IEmpresasVista vista = _empresaServicio.GetEmpresas(pager, model.Id, model.DescripcionBusqueda, model.CuitBusqueda);

            TempData["Model"] = vista;
            return RedirectToAction("PageRedirect");
        }

        public ActionResult PageRedirect()
        {
            return View("Index", (EmpresasVista)TempData["Model"]);
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmEmpresas, Accion = Enums.Acciones.Consultar)]
        public ActionResult Ver(int idEmpresa)
        {
            IEmpresaVista vista = _empresaServicio.GetEmpresa(idEmpresa, "Ver");
            return View("Formulario", vista);
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmEmpresas, Accion = Enums.Acciones.Agregar)]
        public ActionResult Agregar(int? idEmpresa, string origenForm, string cuit)
        {
            IEmpresaVista vista = _empresaServicio.GetEmpresa(-1, "Agregar");
            if (idEmpresa != null) vista.Id = idEmpresa.Value;
            vista.IdUsuario = null;
            vista.OrigenLlamada = origenForm;
            vista.Cuit = cuit;

            return View("Formulario", vista);
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmEmpresas, Accion = Enums.Acciones.Modificar)]
        public ActionResult Modificar(int idEmpresa)
        {
            IEmpresaVista vista = _empresaServicio.GetEmpresa(idEmpresa, "Modificar");
            return View("Formulario", vista);
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmEmpresas, Accion = Enums.Acciones.Eliminar)]
        public ActionResult Eliminar(int idEmpresa)
        {
            IEmpresaVista vista = _empresaServicio.GetEmpresa(idEmpresa, "Eliminar");
            return View("Formulario", vista);
        }

        [HttpPost]
        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmEmpresas, Accion = Enums.Acciones.Agregar)]
        public ActionResult AgregarEmpresa(EmpresaVista model)
        {
            if (_empresaServicio.AddEmpresa(model) == 0)
            {
                ModelState.Clear();
                AddError(_empresaServicio.GetErrores());
                return View("Formulario", model);
            }
            
            var vista = _empresaServicio.GetEmpresas();
            return View("Index", vista);
        }

        [HttpPost]
        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmEmpresas, Accion = Enums.Acciones.Modificar)]
        public ActionResult ModificarEmpresa(EmpresaVista model)
        {
            if (_empresaServicio.UpdateEmpresa(model) == 0)
            {
                AddError(_empresaServicio.GetErrores());
                return View("Formulario", model);
            }
            
            //var vista = _empresaServicio.GetEmpresas();
            
            //return View("Index", vista);
            IEmpresaVista vista = _empresaServicio.GetEmpresa(model.Id, "Modificar");
            return View("Formulario", vista);
        }

        [HttpPost]
        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmEmpresas, Accion = Enums.Acciones.Eliminar)]
        public ActionResult EliminarEmpresa(EmpresaVista model)
        {
            switch (_empresaServicio.DeleteEmpresa(model))
            {
                case 0:
                    var vista = _empresaServicio.GetEmpresas();
                    
                    return View("Index", vista);
                case 1:
                    TempData["MensajeError"] = "La empresa está en uso";
                    
                    return View("Formulario", model);
                default:
                    return View("Formulario", model);
            }
        }

        [HttpPost]
        public ActionResult Validar(int idEmpresa)
        {
            bool mreturn = _empresaServicio.EmpresaEnUso(idEmpresa);

            return Json(mreturn);
        }

        [HttpPost]
        public ActionResult ValidarCuit(string cuit, string descripcion)
        {
            bool mreturn = _empresaServicio.ExistsCuit(cuit);

            return Json(mreturn);
        }

        public ActionResult AgregarUsuario(EmpresaVista model)
        {
            model.Password =  "123456";
            switch (_empresaServicio.AddUsuarioEmpresa(model))
            {
                case 0:
                    var vista = _empresaServicio.GetEmpresa(model.Id, "Modificar");
                    
                    return View("Formulario", vista);
                case 1:
                    TempData["MensajeError"] = "Ya existe un usuario con el mismo CUIL/CUIT.";
                    
                    return View("Formulario", model);
                default:
                    return View("Formulario", model);
            }
        }

        public ActionResult ModificarUsuario(EmpresaVista model)
        {
            model.IdUsuario = _empresaServicio.GetEmpresa(model.Id, "Modificar").IdUsuario;

            switch (_empresaServicio.UpdateUsuarioEmpresa(model))
            {
                case 0:
                    var vista = _empresaServicio.GetEmpresa(model.Id, "Modificar");
                    
                    return View("Formulario", vista);
                case 1:
                    TempData["MensajeError"] = "Ya existe un usuario con el mismo CUIL.";

                    return View("Formulario", model);
                default:
                    return View("Formulario", model);
            }
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmEmpresas, Accion = Enums.Acciones.Modificar)]
        public ActionResult DesvincularFichas(int? idEmpresa)
        {
            IEmpresaVista empresa = _empresaServicio.GetEmpresa(idEmpresa.GetValueOrDefault(), "Ver");

            var vista = _empresaServicio.CargarListaEmpresasDestino();

            vista.IdEmpresaOrigen = empresa.Id;
            vista.EmpresaOrigen = empresa.Descripcion;
            vista.CuitOrigen = empresa.Cuit;
            vista.ObjEmpresaOrigen = empresa;

            return View("DesvinculacionFichas", vista);
        }

        [HttpPost]
        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmEmpresas, Accion = Enums.Acciones.Modificar)]
        public ActionResult BuscarEmpresaAvincular(EmpresasVista model)
        {
            return View("DesvinculacionFichas", _empresaServicio.CargarListaEmpresasDestino(model.Descripcion, model.Cuit, model.IdEmpresaOrigen));
        }

        public ActionResult IndexPagerSelecccionarEmpresa(Pager pager, EmpresasVista model)
        {
            IEmpresasVista vista = _empresaServicio.CargarListaEmpresasDestino(pager, model.Id, model.Descripcion, model.Cuit);

            IEmpresaVista empresa = _empresaServicio.GetEmpresa(model.IdEmpresaOrigen, "Ver");
            vista.IdEmpresaOrigen = empresa.Id;
            vista.EmpresaOrigen = empresa.Descripcion;
            vista.CuitOrigen = empresa.Cuit;
            vista.ObjEmpresaOrigen = empresa;


            TempData["Model"] = vista;
            return RedirectToAction("PageRedirect2");
        }

        public ActionResult PageRedirect2()
        {
            return View("DesvinculacionFichas", (EmpresasVista)TempData["Model"]);
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmEmpresas, Accion = Enums.Acciones.Modificar)]
        public ActionResult SeleccionarDestino(int idEmpresaDestino, int idOrigen)
        {
            IEmpresasVista vista = new EmpresasVista();

            IList<EmpresaVista> listaempresas = new List<EmpresaVista>();

            var empresaOrigen = (EmpresaVista)_empresaServicio.GetEmpresa(idOrigen, "Ver");
            empresaOrigen.Legenda = "Origen";

            var empresaDestino = (EmpresaVista)_empresaServicio.GetEmpresa(idEmpresaDestino, "Ver");
            empresaDestino.Legenda = "Destino";

            listaempresas.Add(empresaOrigen);
            listaempresas.Add(empresaDestino);

            TempData["EmpresaIdOrigen"] = empresaOrigen.Id;
            TempData["EmpresaIdDestino"] = empresaDestino.Id;
            TempData["IdOrigen"] = empresaOrigen.Id;

            vista.EmpresaOrigen = empresaOrigen.Descripcion;
            vista.EmpresaDestino = empresaDestino.Descripcion;
            vista.IdEmpresaDestino = empresaDestino.Id;
            vista.IdEmpresaOrigen = empresaOrigen.Id;
            TempData["model"] = listaempresas;

            return View("ConfirmacionTraspasoFichas", listaempresas);
        }

        [HttpPost]
        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmEmpresas, Accion = Enums.Acciones.Modificar)]
        public ActionResult ConfirmarTraspasoFichas(IList<EmpresaVista> model)
        {
            if (_empresaServicio.DesvincularFichas((int)TempData["EmpresaIdOrigen"], (int)TempData["EmpresaIdDestino"]))
            {
                return RedirectToAction("Modificar", new RouteValueDictionary(
                       new { controller = "Empresas", action = "Modificar", idEmpresa = (int)TempData["EmpresaIdOrigen"] }));
            }
            
            AddError(_empresaServicio.GetErrores());
            TempData["IdOrigen"] = (int)TempData["EmpresaIdOrigen"];
            
            return View("ConfirmacionTraspasoFichas", (IList<EmpresaVista>)TempData["model"]);
        }

        [HttpPost]
        public ActionResult VolverFormularioEmpresa(EmpresasVista model)
        {
            return RedirectToAction("Modificar", new RouteValueDictionary(
                          new { controller = "Empresas", action = "Modificar", idEmpresa = model.IdEmpresaOrigen }));
        }

        [HttpPost]
        public ActionResult VolverCancelar(EmpresaVista model)
        {
            return RedirectToAction(null, new RouteValueDictionary(
                     new { controller = "Fichas", action = "RedirectFicha", origenLlamada = "Empresa" }));
        }

        [HttpPost]
        public ActionResult VolverFormularioSeleccionarDestino(EmpresasVista model)
        {
            return RedirectToAction("Modificar", new RouteValueDictionary(
                          new { controller = "Empresas", action = "SeleccionarDestino", idEmpresa = model.IdEmpresaOrigen }));
        }

        [HttpPost]
        public ActionResult BuscarSede(EmpresaVista model)
        {
            IEmpresaVista vista = new EmpresaVista();
            vista = _empresaServicio.GetEmpresa(model.Id, model.Accion);
            vista.BusquedaSede = model.BusquedaSede;
            ModelState.Clear();
            
            return View("Formulario", _sedeServicio.GetSedesByEmpresa(model));
        }

        [HttpPost]
        public ActionResult BuscarFicha(EmpresaVista model)
        {
            IEmpresaVista vista = new EmpresaVista();
            vista = _empresaServicio.GetEmpresa(model.Id, model.Accion);
            vista.BusquedaFicha = model.BusquedaFicha;
            vista.TabSeleccionado = "1";
            ModelState.Clear();
            
            return View("Formulario", _empresaServicio.BuscarFicha(vista));
        }

        public ActionResult BlanquearCuentaUsr(EmpresaVista model)
        {
            model.IdUsuario = _empresaServicio.GetEmpresa(model.Id, "Modificar").IdUsuario;
            _empresaServicio.BlanquearCuentaUsr(model);

                    var vista = _empresaServicio.GetEmpresa(model.Id, "Modificar");

                    return View("Formulario", vista);
             
            
        }


        // [HttpPost]
        public ActionResult ExportarFichasEmpresa(EmpresaVista model)
        {
            model = (EmpresaVista)TempData["Vista"];
            TempData["Vista"] = model; //Se rea asigna porque el TempData al ser llamado pierde vigencia
            return File(_empresaServicio.ExportFichasEmpresa(model),
                        "application/vnd.ms-excel", "FichasEmpresa.xls");
        }

        //02/06/2019 - RECUPERO CUPONES DEL BANCO

        public ActionResult Recupero()
        {

            return View(_empresaServicio.indexRecupero());
        }

        [HttpPost]
        public ActionResult SubirArchivoRecupero(HttpPostedFileBase archivoRecupero, RecuperoVista model)
        {
            //IRecuperoVista vistaPivot = _empresaServicio.indexRecupero();
            IRecuperoVista vista = _empresaServicio.SubirArchivoRecupero(archivoRecupero, model);

            return View("Recupero", vista);
        }

        public ActionResult ProcesarArchivo(string ArchivoNombre)
        {
            IRecuperoVista vistaPivot = _empresaServicio.indexRecupero();
            vistaPivot.ArchivoNombre = ArchivoNombre;
            vistaPivot.NombreArchivo = ArchivoNombre;
            IRecuperoVista vista = _empresaServicio.ProcesarArchivo(vistaPivot);
            //
            vista.ProcesarArchivo = true;


            return View("Recupero", vista);
        }

        [HttpPost]
        public ActionResult ExportarCupones(RecuperoVista model)
        {
            model = (RecuperoVista)TempData["RecuperoVista"];
            TempData["RecuperoVista"] = model;
            //string result1 = this.RenderActionResultToString(this.PartialView("FormularioBeneficiarios", model));
            return File(_empresaServicio.ExportRecupero(model),
                        "application/vnd.ms-excel", "Recupero.xls");
        }

        
        public ActionResult RecuperoDebito()
        {
            return View(_empresaServicio.indexRecuperoDebito());
        }
        [HttpPost]
        public ActionResult GenerarRecuperoDebito(RecuperoDebitoVista model)
        {
            string concepto = model.Conceptos.Selected ?? "";
            DateTime fecha = model.fechaDebito;
            //model = (RecuperoDebitoVista)TempData["RecuperoDebitoVista"];
            IRecuperoDebitoVista vista;
            vista = _empresaServicio.indexRecuperoDebito();
            //TempData["RecuperoDebitoVista"] = model;
            vista.Conceptos.Selected = concepto;
            vista.fechaDebito = fecha;
            _empresaServicio.addLiqEmpresaDebito(vista);
            return View("RecuperoDebito",vista);
        }

        public ActionResult BuscarRecuperos(RecuperoDebitoVista model)
        {
            string concepto = model.Conceptos.Selected ?? "";
            DateTime fecha = model.fechaDebito;
            //model = (RecuperoDebitoVista)TempData["RecuperoDebitoVista"];
            IRecuperoDebitoVista vista;
            vista = _empresaServicio.indexRecuperoDebito();
            //TempData["RecuperoDebitoVista"] = model;
            vista.Conceptos.Selected = concepto;
            vista.fechaDebito = fecha;
            _empresaServicio.getRecuperosConcepto(vista);
            return View("RecuperoDebito", vista);
        }

        public ActionResult GenerarArchivo(int idrecupero, string nomfile, int idConcepto)
        {
            //string concepto = model.Conceptos.Selected ?? "";
            IRecuperoDebitoVista model;
            model = _empresaServicio.indexRecuperoDebito();
            model.Conceptos.Selected = Convert.ToString(idConcepto);
            //model = (RecuperoDebitoVista)TempData["RecuperoDebitoVista"];
            model.ArchivoNombre = _empresaServicio.GenerarArchivo(idrecupero,nomfile);
            // return View("RecuperoDebito", model);

            return File("../Archivos/" + model.ArchivoNombre, "text/plain", model.ArchivoNombre);
            //return RedirectToAction("MostrarErrorDatos");
        }

         

        //public ActionResult MostrarErrorDatos()
        //{
        //    return View("RecuperoDebito", (RecuperoDebitoVista)TempData["RecuperoDebitoVista"]);
        //}


        public ActionResult ProcesarRecuperoDeb(int idrecupero, string nomfile, int idConcepto)
        {
            IRecuperoDebitoVista model;
            model = _empresaServicio.indexRecuperoDebito();
            model.Conceptos.Selected = Convert.ToString(idConcepto);
            //model = (RecuperoDebitoVista)TempData["RecuperoDebitoVista"];
            model.ArchivoNombre = nomfile;
            model.idconcepto = idConcepto;
            model.idrecuperodeb = idrecupero;
            return View(model);

        }

        [HttpPost]
        public ActionResult SubirArchRecuperoDeb(HttpPostedFileBase archivoRecuperoDeb, RecuperoDebitoVista model)
        {
            //IRecuperoVista vistaPivot = _empresaServicio.indexRecupero();
            IRecuperoDebitoVista vista = _empresaServicio.SubirArchivoRecuperoDebito(archivoRecuperoDeb, model);


            return View("ProcesarRecuperoDeb", vista);
        }



        public ActionResult ProcesarArchivoDebito(string ArchivoNombre ,int idRecupero,int idConcpeto)
        {
            IRecuperoDebitoVista vistaPivot = _empresaServicio.indexRecuperoDebito();
            vistaPivot.ArchivoNombre = ArchivoNombre;
            vistaPivot.idrecuperodeb = idRecupero;
            vistaPivot.idconcepto = idConcpeto;
            IRecuperoDebitoVista vista = _empresaServicio.ProcesarArchivoDeb(vistaPivot);
            //
            vista.ProcesarArchivo = true;


            return View("ProcesarRecuperoDeb", vista);
        }
    }
}
