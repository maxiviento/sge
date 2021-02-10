using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Configuration;

namespace SGE.Web.Infrastucture
{
    public class Authentication
    {
        private readonly static string CookieName = ConfigurationManager.AppSettings.Get("CookieName");

        /// <summary>
        /// AuthenticateRequest
        /// </summary>
        /// <param name="context"></param>
        public static void AuthenticateRequest(HttpContext context)
        {
            var httpCookie = context.Request.Cookies[CookieName];

            // Valida el value de la cookie
            if (httpCookie == null || String.IsNullOrEmpty(httpCookie.Value))
            {
                return;
            }

            // valida el Ticket
            var ticket = FormsAuthentication.Decrypt(httpCookie.Value);
            if (ticket == null || ticket.Expired)
            {
                return;
            }

            // Forms Identity
            var identity = new FormsIdentity(ticket);

            // Obtiene los permisos
            IList<string> roles = new List<string>();
            IList<string> forms = new List<string>();
            IList<string> formsAccion = new List<string>();
            var nombreCompleto = String.Empty;
            var idUsuario = 0;

            if (!string.IsNullOrEmpty(ticket.UserData))
            {
                // Obtiene User data
                var row = ticket.UserData.Split('|');

                idUsuario = Convert.ToInt32(row[0]);

                nombreCompleto = row[1];

                roles = row[2].Split(',');

                formsAccion = row[3].Split('#');

                foreach (var s in formsAccion)
                {
                    if (forms.Contains(s.Split(',')[0]))
                        continue;

                    forms.Add(s.Split(',')[0]);
                }
            }

            var principal = new CustomPrincipal(identity, roles, forms, formsAccion, nombreCompleto, idUsuario);
            context.User = principal;
        }

        /// <summary>
        /// Authenticate
        /// </summary>
        /// <param name="response"></param>
        /// <param name="userName"></param>
        /// <param name="createPersistentCookie"></param>
        /// <param name="userData"></param>
        public static void Authenticate(HttpResponseBase response, string userName, bool createPersistentCookie, string userData)
        {
            double cant = double.Parse(ConfigurationManager.AppSettings.Get("CookieExpiration"));
            DateTime now = DateTime.Now;
            DateTime expiration = now.AddMinutes(cant);
            var authTicket = new FormsAuthenticationTicket(1,
                userName,
                now,
                expiration,
                createPersistentCookie, 
                userData);

            string encrTiker = FormsAuthentication.Encrypt(authTicket);

            var cookie = new HttpCookie(CookieName, encrTiker);
            response.Cookies.Add(cookie);
        }

        /// <summary>
        /// UnAuthenticate
        /// </summary>
        /// <param name="response"></param>
        public static void UnAuthenticate(HttpResponseBase response)
        {
            HttpCookie cookie = new HttpCookie(CookieName, String.Empty);
            response.Cookies.Add(cookie);
        }

        /// <summary>
        /// GetUserIdentity
        /// </summary>
        /// <param name="request"></param>
        public static string GetUserIdentity(HttpRequestBase request)
        {
            HttpCookie httpCookie = request.Cookies[CookieName];

            if (httpCookie == null || String.IsNullOrEmpty(httpCookie.Value))
            {
                return string.Empty;
            }

            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(httpCookie.Value);
            if (ticket == null || ticket.Expired)
            {
                return string.Empty;
            }

            FormsIdentity identity = new FormsIdentity(ticket);

            return identity.Name;
        }
    }
}