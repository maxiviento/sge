using System.Web;
using System.Web.Mvc;
using SGE.Servicio.ServicioInterfaces;


namespace SGE.Servicio.Servicio
{
    public class AutenticacionServicio : IAutenticacionServicio
    {
        public string GetUrl(string action, string controller)
        {
            var url = new UrlHelper(HttpContext.Current.Request.RequestContext);
            var result = url.Action(action, controller);
            return result;
        }
    }
}
