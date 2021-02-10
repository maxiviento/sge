using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGE.Servicio.Comun
{
    public enum TypeError
    {
        NotExist,
        None
    }

    public interface IError
    {
        TypeError TypeError { get; set; }
        string Message { get; set; }
    }
}
