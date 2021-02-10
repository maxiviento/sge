using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SGE.Servicio.Comun;
using SGE.Web.Infrastucture;

namespace SGE.Web.Controllers.Shared
{
    public class BaseController : Controller
    {
        private int IdForm { get; set; }

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);

            var principal = requestContext.HttpContext.User as CustomPrincipal;
            if (principal == null)
            {
                return;
            }

            ViewData["IdUsuario"] = principal.GetIdUsuario();

            ViewData["NombreCompleto"] = principal.GetNombreCompleto();

            // Arma el menu
            ViewData["Menu"] = principal.GetForms().ToList();

            // Activa las acciones del formulario activo
            if (IdForm > 0)
            {
                ViewData["Acciones"] = principal.GetAcciones(IdForm.ToString());
            }
            else
            {
                ViewData["Acciones"] = new string['0'];
            }

            ViewData["Roles"] = principal.GetRoles();

        }

        public BaseController(int idFormulario)
        {
            IdForm = idFormulario;
        }

        public void AddError(IList<IError> errors)
        {
            foreach (var item in errors)
            {
                ModelState.AddModelError(string.Empty, item.Message);
            }
        }

        //
        public ActionResult ViewPdf(string pageTitle, string filename, string viewName, object model)
        {
          htmlViewRenderer HtmlViewRenderer;
          StandardPdfRenderer standardPdfRenderer;
          HtmlViewRenderer = new htmlViewRenderer();
          standardPdfRenderer = new StandardPdfRenderer();

            //object model = new object();
            // Render the view html to a string.
            string htmlText = HtmlViewRenderer.RenderViewToString(this, viewName, model);

            // Let the html be rendered into a PDF document through iTextSharp.
            byte[] buffer = standardPdfRenderer.Render(htmlText, pageTitle);

            // Return the PDF as a binary stream to the client.
            //return new BinaryContentResult(buffer, "application/pdf");

            return File(buffer, "application/pdf", filename);
        }

        //
    }
}
