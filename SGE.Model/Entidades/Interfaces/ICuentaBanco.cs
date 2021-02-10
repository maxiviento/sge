using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGE.Model.Entidades.Interfaces
{
    public interface ICuentaBanco : IComunDatos
    {
        int IdCuentaBanco { get; set; }
        int IdBeneficiario { get; set; }
        short? IdSistema { get; set; }
        int? NroCta { get; set; }
        string Cbu { get; set; }
        string UsuarioBanco { get; set; }
        short? IdSucursal { get; set; }
        short? IdMoneda { get; set; }
        DateTime? FechaSolicitudCuenta { get; set; }
    }
}
