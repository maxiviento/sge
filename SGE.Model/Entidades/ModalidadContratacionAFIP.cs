using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Entidades
{
    public class ModalidadContratacionAFIP : IModalidadContratacionAFIP
    {
        public int IdModalidadAFIP { get; set; }
        public string ModalidadAFIP { get; set; }
    }
}
