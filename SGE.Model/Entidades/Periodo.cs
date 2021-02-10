using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Entidades
{
    public class Periodo : IPeriodo
    {
        public int idperiodo { get; set; }
        public string N_Periodo { get; set; }
    }
}
