using System.Web.Mvc;
using SGE.Web.Controllers.Shared;

namespace SGE.Web.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        public HomeController(): base(0)
        {

        }

        public ActionResult Index()
        {
            return View();
        }
    }
}
