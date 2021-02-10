using System.Web.Mvc;
using SGE.Model.Comun;
using SGE.Servicio.Servicio;
using SGE.Servicio.ServicioInterfaces;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces;
using SGE.Web.Controllers.Shared;
using SGE.Web.Infrastucture;
using SGE.Servicio.Vista;

namespace SGE.Web.Controllers
{
    [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmDepartamentos)]
    public class DepartamentosController : BaseController
    {
        private readonly IDepartamentoServicio _departamentoServicio;

        public DepartamentosController(): base((int)Formularios.Departamento)
        {
            _departamentoServicio = new DepartamentoServicio();
        }

        public ViewResult Index()
        {
            var vis = _departamentoServicio.GetIndex();
            return View(vis);
        }

        [HttpPost]
        public ActionResult BuscarDepartamento(DepartamentosVista model)
        {
            return View("Index", _departamentoServicio.GetDepartamentos(model.Descripcion));
        }

        public ActionResult IndexPager(Pager pager, DepartamentosVista model)
        {
            IDepartamentosVista vista = _departamentoServicio.GetDepartamentos(pager, model.Id, model.Descripcion);

            TempData["Model"] = vista;
            return RedirectToAction("PageRedirect");
        }

        public ActionResult PageRedirect()
        {
            return View("Index", (DepartamentosVista)TempData["Model"]);
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmDepartamentos, Accion = Enums.Acciones.Consultar)]
        public ActionResult Ver(int idDepartamento)
        {
            IDepartamentoVista vista = _departamentoServicio.GetDepartamento(idDepartamento, "Ver");
            return View("Formulario", vista);
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmDepartamentos, Accion = Enums.Acciones.Agregar)]
        public ActionResult Agregar(int? idDepto)
        {
            IDepartamentoVista vista = _departamentoServicio.GetDepartamento(-1, "Agregar");
            if (idDepto != null) vista.Id = idDepto.Value;

            return View("Formulario", vista);
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmDepartamentos, Accion = Enums.Acciones.Modificar)]
        public ActionResult Modificar(int idDepartamento)
        {

            IDepartamentoVista vista = _departamentoServicio.GetDepartamento(idDepartamento, "Modificar");

            return View("Formulario", vista);
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmDepartamentos, Accion = Enums.Acciones.Eliminar)]
        public ActionResult Eliminar(int idDepartamento)
        {
            IDepartamentoVista vista = _departamentoServicio.GetDepartamento(idDepartamento, "Eliminar");
            return View("Formulario", vista);
        }


        [HttpPost]
        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmDepartamentos, Accion = Enums.Acciones.Agregar)]
        public ActionResult AgregarDepartamento(DepartamentoVista model)
        {
            switch (_departamentoServicio.AddDepartamento(model))
            {
                case 0:
                    var vis = _departamentoServicio.GetDepartamentos();
                    return View("Index", vis);
                case 1:
                    TempData["MensajeError"] = "Ya existe un departamento con ese nombre";
                    return View("Formulario", model);
                default:
                    return View("Formulario", model);
            }
        }
        
        [HttpPost]
        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmDepartamentos, Accion = Enums.Acciones.Modificar)]
        public ActionResult ModificarDepartamento(DepartamentoVista model)
        {
            ModelState.Clear();
            switch (_departamentoServicio.UpdateDepartamento(model))
            {
                case 0:
                    var vis = _departamentoServicio.GetDepartamentos();
                    return View("Index", vis);
                case 1:
                    TempData["MensajeError"] = "Ya existe un departamento con ese nombre";
                    return View("Formulario", model);
                default:
                    return View("Formulario", model);
            }
        }

        [HttpPost]
        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmDepartamentos, Accion = Enums.Acciones.Eliminar)]
        public ActionResult EliminarDepartamento(DepartamentoVista model)
        {
            ModelState.Clear();
            switch (_departamentoServicio.DeleteDepartamento(model))
            {
                case 0:
                    var vis = _departamentoServicio.GetDepartamentos();
                    return View("Index", vis);
                case 1:
                    TempData["MensajeError"] = "El departamento está en uso";
                    return View("Formulario", model);
                default:
                    return View("Formulario", model);
            }
        }
    }
}
