using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Entidades
{
    public class Convenio : IConvenio
    {

        public int IdConvenio { get; set; }
        public string NConvenio { get; set; }
    }
}
