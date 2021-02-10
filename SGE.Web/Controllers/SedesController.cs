using System;
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
    [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmSedes)]
    public class SedesController : BaseController
    {
        //
        // GET: /Sede/
        private readonly ISedeServicio _sedeServicio;
        private readonly IEmpresaServicio _empresaServicio;

        public SedesController(): base((int)Formularios.Sede)
        {
            _sedeServicio = new SedeServicio();
            _empresaServicio = new EmpresaServicio();
        }

        public ActionResult Index(SedeVista model)
        {
            var vis = _sedeServicio.GetSedes();
            return View(vis);
        }

        [HttpPost]
        public ActionResult BuscarSede(SedesVista model)
        {
            return View("Index", _sedeServicio.GetSedes(model.BusquedaDescripcion,
                                                        Convert.ToInt32(model.BusquedaEmpresas.Selected)));
        }

        public ActionResult IndexPager(Pager pager, SedesVista model)
        {
            ISedesVista vista = _sedeServicio.GetSedes(pager,
                                                       model.BusquedaDescripcion,
                                                       Convert.ToInt32(model.BusquedaEmpresas.Selected));

            TempData["Model"] = vista;
            return RedirectToAction("PageRedirect");
        }

        public ActionResult PageRedirect()
        {
            return View("Index", (SedesVista)TempData["Model"]);
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmSedes, Accion = Enums.Acciones.Consultar)]
        public ActionResult Ver(int idSede)
        {
            ISedeVista vista = _sedeServicio.GetSede(idSede, "Ver");
            return View("Formulario", vista);
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmSedes, Accion = Enums.Acciones.Agregar)]
        public ActionResult Agregar(SedeVista model)
        {
            ISedeVista vista = _sedeServicio.GetSede(0, "Agregar");
            vista.Id = model.Id;
            vista.Descripcion = "";
            vista.IdEmpresa = model.IdEmpresa;
            ModelState.Clear();
            return View("Formulario", vista);
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmSedes, Accion = Enums.Acciones.Modificar)]
        public ActionResult Modificar(int idSede)
        {
            ISedeVista vista = _sedeServicio.GetSede(idSede, "Modificar");
            return View("Formulario", vista);
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmSedes, Accion = Enums.Acciones.Eliminar)]
        public ActionResult Eliminar(int idSede)
        {
            ISedeVista vista = _sedeServicio.GetSede(idSede, "Eliminar");
            return View("Formulario", vista);
        }

        [HttpPost]
        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmSedes, Accion = Enums.Acciones.Agregar)]
        public ActionResult AgregarSede(SedeVista model)
        {
            IEmpresaVista empresaVista = new EmpresaVista();

            if (model.IdEmpresa==0)
            {
                _empresaServicio.AddEmpresa(empresaVista);
                model.IdEmpresa = empresaVista.Id;

                switch (_sedeServicio.AddSede(model))
                {
                    case 0:
                        return RedirectToAction("Modificar", new RouteValueDictionary(
                                new { controller = "Empresas", action = "Modificar", idEmpresa = model.IdEmpresa }));
                    case 1:
                        TempData["MensajeError"] = "Ya existe una sede con ese nombre.";
                        return View("Formulario", model);
                    default:
                        return View("Formulario", model);
                }
            }
            else
            {
                switch (_sedeServicio.AddSede(model))
                {
                    case 0:
                        //var vis = _empresaServicio.GetEmpresa(model.IdEmpresa,model.Accion);
                        return RedirectToAction("Modificar", new RouteValueDictionary(
                                new { controller = "Empresas", action = "Modificar", idEmpresa = model.IdEmpresa }));
                    case 1:
                        TempData["MensajeError"] = "Ya existe una sede con ese nombre";
                        return View("Formulario", model);
                    default:
                        return View("Formulario", model);
                }
            }
        }

        [HttpPost]
        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmSedes, Accion = Enums.Acciones.Modificar)]
        public ActionResult ModificarSede(SedeVista model)
        {
            switch (_sedeServicio.UpdateSede(model))
            {
                case 0:
                    return RedirectToAction("Modificar", new RouteValueDictionary(
                            new { controller = "Empresas", action = "Modificar", idEmpresa = model.IdEmpresa }));
                case 1:
                    TempData["MensajeError"] = "Ya existe una sede con ese nombre";
                    return View("Formulario", model);
                default:
                    return View("Formulario", model);
            }
        }

        [HttpPost]
        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmSedes, Accion = Enums.Acciones.Eliminar)]
        public ActionResult EliminarSede(SedeVista model)
        {
            switch (_sedeServicio.DeleteSede(model))
            {
                case 0:
                    return RedirectToAction("Modificar", new RouteValueDictionary(
                            new { controller = "Empresas", action = "Modificar", idEmpresa = model.IdEmpresa }));
                case 1:
                    TempData["MensajeError"] = "La sede está en uso";
                    return View("Formulario", model);
                default:
                    //Muestra listado de error;
                    return View("Formulario", model);
            }
        }

        [HttpPost]
        public ActionResult Validar(int idSede)
        {
            bool mreturn = _sedeServicio.SedeTieneFichas(idSede);

            return Json(mreturn);
        }

        [HttpPost]
        public ActionResult ValidarNombre(int id, string descripcion)
        {
            bool mreturn = _sedeServicio.ExistsSede(id, descripcion);

            return Json(mreturn);
        }

        [HttpPost]
        public ActionResult VolverFormularioEmpresa(SedeVista model)
        {
            return RedirectToAction("Modificar", new RouteValueDictionary(
                          new { controller = "Empresas", action = "Modificar", idEmpresa = model.IdEmpresa }));
        }
     
    }
}
