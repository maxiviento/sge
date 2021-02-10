using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGE.Model.Entidades.Interfaces
{
    public interface IConyuge:IComunDatos
    {
        int IdConyugue { get; set; }
        int IdBeneficiario { get; set; }
        string Cuil { get; set; }
        string Apellido { get; set; }
        string Nombre { get; set; }
        DateTime FechaNacimiento { get; set; }
        int? TipoDocumento { get; set; }
        string NumeroDocumento { get; set; }
        string Sexo { get; set; }
        int? Nacionalidad { get; set; }
    }
}
