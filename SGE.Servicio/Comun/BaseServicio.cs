using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using SGE.Model.Comun;
using SGE.Model.Entidades.Interfaces;
using SGE.Model.Repositorio;
using SGE.Repositorio;

namespace SGE.Servicio.Comun
{
    public class BaseServicio
    {

        private readonly static string CookieName = ConfigurationManager.AppSettings.Get("CookieName");
        private readonly IUsuarioRepositorio _usuariorepositorio;
        private readonly IList<IError> _errors = new List<IError>();
        private readonly IRolFormularioAccionRepositorio _rolformularioaccionrepositorio;

        public BaseServicio()
        {
            _usuariorepositorio = new UsuarioRepositorio();
            _rolformularioaccionrepositorio = new RolFormularioAccionRepositorio();
        }

        public IUsuario GetUsuarioLoguer()
        {

            var httpCookie = HttpContext.Current.Request.Cookies[CookieName];

            if (httpCookie == null || String.IsNullOrEmpty(httpCookie.Value))
            {
                return null;
            }

            var ticket = FormsAuthentication.Decrypt(httpCookie.Value);
            if (ticket == null || ticket.Expired)
            {
                return null;
            }
            var identity = new FormsIdentity(ticket);

            return _usuariorepositorio.GetUsuario(identity.Name);
        }

        public IList<IError> Errors
        {
            get { return _errors; }
        }

        public void AddError(TypeError type, string message)
        {
            _errors.Add(new Error { Message = message, TypeError = type });
        }

        public void AddError(string message)
        {
            _errors.Add(new Error { Message = message, TypeError = TypeError.None });
        }

        public bool AccesoPrograma(Enums.Formulario formulario)
        {
            var accesoprograma =
              _rolformularioaccionrepositorio.GetFormulariosDelUsuario(GetUsuarioLoguer().Id_Usuario).Where(
                  c =>
                  c.Id_Accion == (int)Enums.Acciones.VerEfectoresSociales &&
                  c.Id_Formulario == (int)formulario).Count() > 0
                  ? true
                  : false;

            return accesoprograma;
        }
    }
}
