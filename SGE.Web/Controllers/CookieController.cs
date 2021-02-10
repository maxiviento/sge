using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SGE.Web.Infrastucture;

namespace SGE.Web.Controllers
{
    public class CookieController : Controller
    {
        //
        // GET: /Cookie/

        [HttpPost]
        public ActionResult UltimaActividad()
        {
            return Json(DateTime.Now.ToString());
        }

        [HttpPost]
        public ActionResult ControlSeccion(string date)
        {
            DateTime ultimaactividad = Convert.ToDateTime(date);
            string dialog = "";

            TimeSpan fecha = DateTime.Now - ultimaactividad;
            var minutosinactivos = fecha.Minutes;

            int maximainactividad = Convert.ToInt32(ConfigurationManager.AppSettings.Get("MaximoInactividad"));

            if (minutosinactivos == (maximainactividad - 1) && (maximainactividad - 1) > 0)
            {
                dialog = "60s";
            }

            if (minutosinactivos >= maximainactividad)
            {
                Authentication.UnAuthenticate(Response);
                dialog = "Expiro";
            }

            return Json(dialog);
        }

        [HttpPost]
        public ActionResult CerrarSeccion(string date)
        {
            Authentication.UnAuthenticate(Response);

            return Json("Ok");
        }


    }
}
