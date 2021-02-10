using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Entidades
{
    public class AporteDebito :IAporteDebito
    {
        public int ID_COFINANCIMIENTO { get; set; }
        public decimal? MONTO { get; set; }
        public int? CANT_EMPLEADOS { get; set; }
    }
}
