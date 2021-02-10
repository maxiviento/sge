using System;
using System.IO;
using System.Web.Mvc;

namespace SGE.Servicio.Comun
{


    public class FakeView : IView
    {
        #region IView Members

        public void Render(ViewContext viewContext, TextWriter writer)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
