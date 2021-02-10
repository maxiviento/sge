using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGE.Model.Entidades.Interfaces
{
    public interface IEstadoBeneficiario
    {
        int Id_Estado_Beneficiario { get; set; }
        string Estado_Beneficiario { get; set; }
    }
}
