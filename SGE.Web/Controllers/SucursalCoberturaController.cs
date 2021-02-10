using System;
using System.Web.Mvc;
using SGE.Servicio.Servicio;
using SGE.Servicio.ServicioInterfaces;
using SGE.Servicio.Vista;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces;
using SGE.Web.Controllers.Shared;
using SGE.Web.Infrastucture;
using SGE.Model.Comun;

namespace SGE.Web.Controllers
{
    [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmSucursalesCobertura)]
    public class SucursalCoberturaController : BaseController
    {
        private readonly ISucursalCoberturaServicio _sucursalCoberturaServicio;

        public SucursalCoberturaController()
            : base((int)Formularios.SucursalCobertura)
        {
            _sucursalCoberturaServicio = new SucursalCoberturaServicio();
        }

        public ActionResult Index(SucursalCoberturaVista model)
        {
            var vis = _sucursalCoberturaServicio.GetIndex();
            return View(vis);
        }

        [HttpPost]
        public ActionResult BuscarSucursalCobertura(SucursalesCoberturaVista model)
        {
            return View("Index", _sucursalCoberturaServicio.GetSucursalesCobertura(Convert.ToInt32(model.BusquedaSucursales.Selected),
                                                                                   Convert.ToInt32(model.BusquedaLocalidades.Selected), 
                                                                                   Convert.ToInt32(model.BusquedaDepartamentos.Selected),
                                                                                   model.Asignadas));
        }

        public ActionResult IndexPager(Pager pager, SucursalesCoberturaVista model)
        {
            ISucursalesCoberturaVista vista = _sucursalCoberturaServicio.GetSucursalesCobertura(pager,
                                                       Convert.ToInt32(model.BusquedaSucursales.Selected),
                                                                                   Convert.ToInt32(model.BusquedaLocalidades.Selected), 
                                                                                   Convert.ToInt32(model.BusquedaDepartamentos.Selected),
                                                                                   model.Asignadas);

            TempData["Model"] = vista;
            return RedirectToAction("PageRedirect");
        }

        public ActionResult PageRedirect()
        {
            return View("Index", (SucursalesCoberturaVista)TempData["Model"]);
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmSucursalesCobertura, Accion = Enums.Acciones.Consultar)]
        public ActionResult Ver(int idLocalidad)
        {
            ISucursalCoberturaVista vista = _sucursalCoberturaServicio.GetSucursalCoberturaByLocalidad(idLocalidad, "Ver");
            return View("Formulario", vista);
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmSucursalesCobertura, Accion = Enums.Acciones.Modificar)]
        public ActionResult Modificar(int idLocalidad)
        {
            ISucursalCoberturaVista vista = _sucursalCoberturaServicio.GetSucursalCoberturaByLocalidad(idLocalidad, "Modificar");
            return View("Formulario", vista);
        }

        [HttpPost]
        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmSucursalesCobertura, Accion = Enums.Acciones.Modificar)]
        public ActionResult ModificarSucursalCobertura(SucursalCoberturaVista model)
        {
            switch (_sucursalCoberturaServicio.UpdateSucursalCobertura(model))
            {
                case 0:
                    return View("Index", _sucursalCoberturaServicio.GetSucursalesCobertura());
                case 1:
                    TempData["MensajeError"] = "No se pudo Modificar la Sucursal de la Localidad";
                    return View("Formulario", model);
                default:
                    return View("Formulario", model);
            }
        }

        public ActionResult CambiarCombo(SucursalesCoberturaVista model)
        {
            ISucursalesCoberturaVista vista = _sucursalCoberturaServicio.GetSucursalesCobertura(model);
            return View("Index", vista);
        }
    }
}
