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
    [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmCarreras)]
    public class CarrerasController : BaseController
    {
        private readonly ICarreraServicio _carreraServicio;

        public CarrerasController(): base((int)Formularios.Carrera)
        {
            _carreraServicio=new CarreraServicio();
        }

        public ViewResult Index()
        {
            var vista=_carreraServicio.GetIndex();
            vista.DescripcionBusca = "";
            return View(vista);
        }

        [HttpPost]
        public ActionResult BuscarCarrera(CarrerasVista model)
        {
            return View("Index", _carreraServicio.GetCarreras(model.DescripcionBusca));
        }

        public ActionResult IndexPager(Pager pager, CarrerasVista model)
        {
            ICarrerasVista vista = _carreraServicio.GetCarreras(pager, model.Id, model.DescripcionBusca);

            TempData["Model"] = vista;
            return RedirectToAction("PageRedirect");
        }

       public ActionResult PageRedirect()
       {
           return View("Index", TempData["Model"]);
       }

       [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmCarreras, Accion = Enums.Acciones.Agregar)]
       public ActionResult Agregar()
        {
            ICarreraVista vista = _carreraServicio.GetCarrera(0,"Agregar");
            return View("Formulario", vista);
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmCarreras, Accion = Enums.Acciones.Consultar)]
        public ActionResult Ver(int idCarrera)
        {
            ICarreraVista vista = _carreraServicio.GetCarrera(idCarrera, "Ver");
            return View("Formulario", vista);
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmCarreras, Accion = Enums.Acciones.Modificar)]
        public ActionResult Modificar(int idCarrera)
        {
            ICarreraVista vista = _carreraServicio.GetCarrera(idCarrera, "Modificar");
            return View("Formulario", vista);
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmCarreras, Accion = Enums.Acciones.Eliminar)]
        public ActionResult Eliminar(int idCarrera)
        {
            ICarreraVista vista = _carreraServicio.GetCarrera(idCarrera, "Eliminar");
            return View("Formulario", vista);
        }

        [HttpPost]
        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmCarreras, Accion = Enums.Acciones.Agregar)]
        public ActionResult AgregarCarrera(CarreraVista model)
        {
            switch (_carreraServicio.AddCarrera(model))
            {
                case 0:
                    return View("Index",  _carreraServicio.GetCarreras());
                case 1:
                    TempData["MensajeError"] = "Ya existe una Carrera con ese nombre";
                    return View("Formulario", model);
                case 2:
                    TempData["MensajeError"] = "Seleccione una opción";
                    return View("Formulario", model);
                default:
                    //Muestra listado de error;
                    return View("Formulario", model);
            }
        }

        [HttpPost]
        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmCarreras, Accion = Enums.Acciones.Modificar)]
        public ActionResult ModificarCarrera(CarreraVista model)
        {
            switch (_carreraServicio.UpdateCarrera(model))
            {
                case 0:
                    var vis= _carreraServicio.GetCarreras();
                    vis.DescripcionBusca = "";
                    return View("Index",vis);
                case 1:
                    TempData["MensajeError"] = "Ya existe una Carrera con ese nombre";
                    return View("Formulario", model);
                case 2:
                    TempData["MensajeError"] = "Seleccione una opción";
                    return View("Formulario", model);
                default:
                    //Muestra listado de error;
                    return View("Formulario", model);
            }
        }

        [HttpPost]
        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmCarreras, Accion = Enums.Acciones.Eliminar)]
        public ActionResult EliminarCarrera(CarreraVista model)
        {
            switch (_carreraServicio.DeleteCarrera(model))
            {
                case 0:
                    var vista= _carreraServicio.GetCarreras();
                    vista.DescripcionBusca = "";
                    return View("Index",vista);
                case 1:
                    TempData["MensajeError"] = "Excepción!";
                    var vistaAux = _carreraServicio.GetCarrera(model.Id, "Eliminar");
                    return View("Formulario", vistaAux);
                default:
                    //Muestra listado de error;
                    return View("Formulario", model);
            }
        }

        [HttpPost]
        public ActionResult ValidarEliminar(int idCarrera)
        {
            bool mreturn = _carreraServicio.CarreraInUse(idCarrera);

            return Json(mreturn);
        }

        [HttpPost]
        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmCarreras, Accion = Enums.Acciones.Agregar)]     //Es usado por las 2 acciones
        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmCarreras, Accion = Enums.Acciones.Modificar)]
        public ActionResult FormularioCarreraCambiaInstitucion(CarreraVista model)
        {
            _carreraServicio.FormCarreraChangeInstitucion(model);

            return View("Formulario", model);
        }


        // 13/03/2013 - DI CAMPLI LEANDRO - LISTADO SECTOR INSITUCIÓN CARRERA

       // [HttpPost]
        public ActionResult ReporteSectorInstCarrera(CarrerasVista model)
        {
           // return View("Index", _carreraServicio.GetCarreras(model.DescripcionBusca));
            int idnivel = model.idNivel;
            model = (CarrerasVista)_carreraServicio.GetCarrerasSectorInstitucion(model.idNivel);
            model.idNivel = idnivel;

            return View("SectorInstCarrera", model);
        }

        public ActionResult PrintCarerras(int idnivel)
        {
            CarrerasVista vista;
            //CustomerList customerList = CreateCustomerList();
            //FillImageUrl(customerList, "report.jpg");
            //return this.ViewPdf("Customer report", "PrintDemo", customerList);
            vista = (CarrerasVista)_carreraServicio.GetCarrerasSectorInstitucion(idnivel);
            vista.idNivel = idnivel;
            string filename = string.Empty;
            if (idnivel == 1)
            { filename = "CarrerasTerciarias.pdf"; }
            else if (idnivel == 2) { filename = "CarrerasUniversitarias.pdf"; }
            else { filename = "CarrerasTodas.pdf"; }

            return this.ViewPdf("Reporte Sector Institución Carreras",filename, "ImpCarreras", vista);
        }

        [HttpPost]
        public ActionResult GetCarreras(int idInstitucion)
        {

            var carreras = _carreraServicio.GetCarreras(idInstitucion);

            return Json(carreras);
        }

    }
}
