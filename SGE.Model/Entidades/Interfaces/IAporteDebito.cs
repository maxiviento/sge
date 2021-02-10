using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGE.Model.Entidades.Interfaces
{
    public interface IAporteDebito
    {
        int ID_COFINANCIMIENTO { get; set; }
        decimal? MONTO{ get; set; }
        int? CANT_EMPLEADOS { get; set; }

    }
}
