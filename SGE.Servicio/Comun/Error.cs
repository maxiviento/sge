using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGE.Servicio.Comun
{
    public class Error : IError
    {
        public TypeError TypeError { get; set; }
        public string Message { get; set; }
    }
}
