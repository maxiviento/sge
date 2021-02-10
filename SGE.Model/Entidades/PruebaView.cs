using System;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Entidades
{
    public class PruebaView : IPruebaView
    {

        public PruebaView()
        { }

        public int IdApoderado { get; set; }
        public int? IdBeneficiario { get; set; }

    
    
    }
}
