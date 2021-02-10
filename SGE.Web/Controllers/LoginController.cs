using System;
using System.Web.Mvc;
using SGE.Model.Comun;
using SGE.Servicio.Vista;
using SGE.Servicio.Servicio;
using SGE.Servicio.ServicioInterfaces;
using SGE.Web.Controllers.Shared;
using SGE.Web.Infrastucture;

namespace SGE.Web.Controllers
{
    public class LoginController : BaseController
    {
        private readonly IUsuarioServicio _usuarioServicio;

        public LoginController()
            : base(0)
        {
            _usuarioServicio = new UsuarioServicio();
        }

        public ViewResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UsuarioLoginVista model)
        {
            if (ModelState.IsValid)
            {
                var usrdatos = String.Empty;
                switch (_usuarioServicio.Login(model, ref usrdatos))
                {
                    case (int)Enums.Autentificacion.Autenticado:
                        // Autentica, Genera la cookie
                        Authentication.Authenticate(Response, model.Login, model.Persistir, usrdatos);
                        return RedirectToAction("Index", "Home");
                    case (int)Enums.Autentificacion.UsuarioInexistente:
                        ModelState.AddModelError(string.Empty, "El usuario ingresado no existe.");
                        return View("Login", model);
                    case (int)Enums.Autentificacion.ContraseñaInvalida:
                        ModelState.AddModelError(string.Empty, "La contraseña ingresada no es válida.");
                        return View("Login", model);
                    default:
                        //Mustra listado de error;
                        return View("Login", model);
                }
            }

            return View("Login", model);
        }


        public ActionResult LogOff()
        {
            // Elimina la cookie
            Authentication.UnAuthenticate(Response);

            return RedirectToAction("Login", "Login");
        }
    }
}