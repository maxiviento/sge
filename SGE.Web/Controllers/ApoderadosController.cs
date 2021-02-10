using System;
using System.Web.Mvc;
using System.Web.Routing;
using SGE.Model.Comun;
using SGE.Servicio.Servicio;
using SGE.Servicio.ServicioInterfaces;
using SGE.Servicio.Vista;
using SGE.Servicio.VistaInterfaces;
using SGE.Web.Controllers.Shared;
using SGE.Web.Infrastucture;

namespace SGE.Web.Controllers
{
    [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmBeneficiario)]
    public class ApoderadosController : BaseController
    {
        private readonly IApoderadoServicio _apoderadoServicio;
        private readonly ILocalidadServicio _localidadservicio;

        public ApoderadosController() : base((int)Formularios.Apoderado)
        {
            _apoderadoServicio = new ApoderadoServicio();
            _localidadservicio = new LocalidadServicio();
        }

        public ActionResult Index()
        {
            var vista = _apoderadoServicio.GetApoderadosByBeneficiario(0);
            
            return View(vista);
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmBeneficiario, Accion = Enums.Acciones.ConsultarApoderado)]
        public ActionResult Ver(int idApoderado, string accion)
        {
            IApoderadoVista vista = _apoderadoServicio.GetApoderado(idApoderado, "Ver");
            vista.AccionLlamada = accion;
            
            return View("Formulario", vista);
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmBeneficiario, Accion = Enums.Acciones.EliminarApoderado)]
        public ActionResult Eliminar(int idApoderado, string accion)
        {
            IApoderadoVista vista = _apoderadoServicio.GetApoderado(idApoderado, "Eliminar");
            vista.AccionLlamada = accion;
            
            return View("Formulario", vista);
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmBeneficiario, Accion = Enums.Acciones.AgregarApoderado)]
        public ActionResult Agregar(int? idBeneficiario)
        {
            IApoderadoVista vista = _apoderadoServicio.GetApoderado(0, "Agregar");
            if (idBeneficiario != null) vista.IdBeneficiario = idBeneficiario.Value;
            vista.ApoderadoDesde = DateTime.Now;
            vista.Monedas.Enabled = false;
            
            return View("Formulario", vista);
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmBeneficiario, Accion = Enums.Acciones.ModificarApoderado)]
        public ActionResult Modificar(int idApoderado, int idBeneficiario)
        {
            IApoderadoVista vista = _apoderadoServicio.GetApoderado(idApoderado, "Modificar");
            vista.IdBeneficiario = idBeneficiario;
            vista.Monedas.Enabled = false;
            
            return View("Formulario", vista);
        }


        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmBeneficiario, Accion = Enums.Acciones.ModificarApoderado)]
        public ActionResult CambiarEstado(int idApoderado, int idBeneficiario)
        {
            IApoderadoVista vista = _apoderadoServicio.GetApoderado(idApoderado, "CambiarEstado");
            vista.IdBeneficiario = idBeneficiario;
            vista.Monedas.Enabled = false;
            
            return View("Formulario", vista);
        }

        [HttpPost]
        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmBeneficiario, Accion = Enums.Acciones.AgregarApoderado)]
        public ActionResult AgregarApoderado(ApoderadoVista model)
        {
            switch (_apoderadoServicio.AddApoderado(model))
            {
                case 0:
                    //model.Accion = "Agregar";
                    return RedirectToAction("Modificar", new RouteValueDictionary(
                                 new { controller = "Beneficiarios", action = "Modificar", idBeneficiario = model.IdBeneficiario }));
                case 1:
                    TempData["MensajeError"] = "Este Apoderado ya corresponde a otro Beneficiario. ¿Desea suspender el que tiene y asignarlo a este Beneficiario?";
                    return View("Formulario", model);
                default:
                    return View("Formulario", model);
            }
        }

        [HttpPost]
        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmBeneficiario, Accion = Enums.Acciones.ModificarApoderado)]
        public ActionResult ModificarApoderado(ApoderadoVista model)
        {
            switch (_apoderadoServicio.UpdateApoderado(model))
            {
                case 0:
                    return RedirectToAction("Modificar", new RouteValueDictionary(
                                 new { controller = "Beneficiarios", action = "Modificar", idBeneficiario = model.IdBeneficiario }));
                case 1:
                    TempData["MensajeError"] = "Ya existe Beneficiario para este Apoderado.";
                    return View("Formulario", model);
                default:
                    return View("Formulario", model);
            }
        }

        [HttpPost]
        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmBeneficiario, Accion = Enums.Acciones.ModificarApoderado)]
        public ActionResult CambiarEstado(ApoderadoVista model)
        {
            if (_apoderadoServicio.CambiarEstado(model))
            {
                return RedirectToAction("Modificar", new RouteValueDictionary(
                                 new { controller = "Beneficiarios", action = "Modificar", idBeneficiario = model.IdBeneficiario }));
            }
            
            return View("Formulario", model);
        }

        [HttpPost]
        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmBeneficiario, Accion = Enums.Acciones.EliminarApoderado)]
        public ActionResult EliminarApoderado(ApoderadoVista model)
        {
            if (_apoderadoServicio.DeleteApoderado(model))
            {
                return RedirectToAction("Modificar", new RouteValueDictionary(
                                  new { controller = "Beneficiarios", action = "Modificar", idBeneficiario = model.IdBeneficiario }));
            }
            
            return View("Formulario", model);
        }

        [HttpPost]
        public ActionResult ExisteApoderado(string nroDocumento, string idBeneficiario)
        {
            //verifica si el nro de documento ya está como apoderado

            // 15/05/2014 - para quitar la validación retornamos siempre false y comentamos la llamada al metodo            
            //bool mreturn = _apoderadoServicio.ExistsApoderado(nroDocumento, Convert.ToInt32(idBeneficiario));
            bool mreturn = false;
            return Json(mreturn);
        }

        [HttpPost]
        public ActionResult ExisteApoderadoActivo(string nroDocumento)
        {
            //bool mreturn = _apoderadoServicio.EsApoderadoActivo(nroDocumento);
            bool mreturn = false;
            return Json(mreturn);
        }

        [HttpPost]
        public ActionResult VolverFormularioBeneficiario(ApoderadoVista model)
        {
            if (model.AccionLlamada != "Ver")
            {
                return RedirectToAction("Modificar", new RouteValueDictionary(
                             new { controller = "Beneficiarios", action = "Modificar", idBeneficiario = model.IdBeneficiario }));
            }
            
            return RedirectToAction("Ver", new RouteValueDictionary(
                                               new { controller = "Beneficiarios", action = "Ver", idBeneficiario = model.IdBeneficiario }));
        }


        [HttpPost]
        public ActionResult BuscarSucursal(string idlocalidad)
        {
            ILocalidadVista localidad = _localidadservicio.GetSucursalCoberturaByLocalidad(Convert.ToInt32(idlocalidad));

            return Json(localidad);
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmBeneficiario, Accion = Enums.Acciones.ModificarApoderado)]
        public ActionResult LimpiarFecha(ApoderadoVista model)
        {

            model.RealizoElCambio = _apoderadoServicio.LimpiarFechaSolicitud(model);

            return RedirectToAction("RetornoLimpiarFecha", new RouteValueDictionary(
                                                     new
                                                         {
                                                             controller = "Apoderados",
                                                             action = "RetornoLimpiarFecha",
                                                             idApoderado = model.IdApoderado,
                                                             idBeneficiario = model.IdBeneficiario,
                                                             cambiofecha = model.RealizoElCambio
                                                         }));


        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.AbmBeneficiario, Accion = Enums.Acciones.ModificarApoderado)]
        public ActionResult RetornoLimpiarFecha(int idApoderado, int idBeneficiario, bool cambiofecha)
        {
            IApoderadoVista vista = _apoderadoServicio.GetApoderado(idApoderado, "Modificar");
            vista.IdBeneficiario = idBeneficiario;
            vista.Monedas.Enabled = false;
            vista.RealizoElCambio = cambiofecha;
            
            return View("Formulario", vista);
        }

    }
}
