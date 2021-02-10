using System;
using System.Configuration;
using System.Web;
using System.Web.Security;
using SGE.Model.Entidades.Interfaces;
using SGE.Model.Repositorio;

namespace SGE.Repositorio
{
    public class BaseRepositorio
    {
        private readonly static string CookieName = ConfigurationManager.AppSettings.Get("CookieName");
        private readonly IUsuarioRepositorio _usuariorepositorio;

        public BaseRepositorio()
        {
            _usuariorepositorio = new UsuarioRepositorio();
        }

        public void AgregarDatos(IComunDatos datos)
        {
            datos.FechaSistema = DateTime.Now;
            datos.IdUsuarioSistema = GetUsuarioLoguer().Id_Usuario;
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

    }
}
