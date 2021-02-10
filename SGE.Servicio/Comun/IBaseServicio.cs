using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGE.Servicio.Comun
{
    public interface IBaseServicio
    {
        IList<IError> GetErrores();
    }
}
