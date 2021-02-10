using System.Web.Mvc;
using SGE.Model.Comun;
using SGE.Servicio.Servicio;
using SGE.Servicio.ServicioInterfaces;
using SGE.Servicio.Vista;
using SGE.Web.Controllers.Shared;
using SGE.Web.Infrastucture;

namespace SGE.Web.Controllers
{
    [CustomAuthorizeAttribute(Formulario = Enums.Formulario.CambiarComntraseña)]
    public class PasswordController : BaseController
    {
        private readonly IUsuarioServicio _usuarioServicio;

        public PasswordController(): base((int)Formularios.CambiarPassword)
        {
            _usuarioServicio = new UsuarioServicio();
        }
        
        public ViewResult Modificar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Modificar(UsuarioPasswordVista model)
        {
            model.Login = Authentication.GetUserIdentity(Request);

            if (!string.IsNullOrEmpty(model.Login))
            {
                switch (_usuarioServicio.UpdatePassword(model))
                {
                    case 0:
                        TempData["MensajeSuccess"] = "La contraseña fue modificada con exito";
                        return RedirectToAction("Modificar", "Password");
                    case 1:
                        ModelState.AddModelError(string.Empty, "La contraseña actual no es válida");
                        return View("Modificar", model);
                    default:
                        //Mustra listado de error;
                        return View("Modificar", model);
                }
            }
           
            return RedirectToAction("Modificar", "Password");
        }
    }
}