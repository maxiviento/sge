using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iTextSharp;
using SGE.Servicio.Comun;

namespace SGE.Web.Controllers
{
    public class PdfViewController : Controller
    {
        private readonly htmlViewRenderer HtmlViewRenderer;
        private readonly StandardPdfRenderer standardPdfRenderer;

        public PdfViewController()
        {
            this.HtmlViewRenderer = new htmlViewRenderer();
            this.standardPdfRenderer = new StandardPdfRenderer();
        }

        //[HttpPost]
        public ActionResult ViewPdf(string pageTitle, string viewName, object model)
        {
            //object model = new object();
            // Render the view html to a string.
            string htmlText = HtmlViewRenderer.RenderViewToString(this, viewName, model);

            // Let the html be rendered into a PDF document through iTextSharp.
           byte[] buffer = standardPdfRenderer.Render(htmlText, pageTitle);

            // Return the PDF as a binary stream to the client.
            //return new BinaryContentResult(buffer, "application/pdf");

           return File(buffer, "application/pdf");
        }


    }
}
