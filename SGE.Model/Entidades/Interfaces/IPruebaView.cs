using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGE.Model.Entidades.Interfaces
{
    public interface IPruebaView
    {
        int IdApoderado { get; set; }
        int? IdBeneficiario { get; set; }
    }
}
