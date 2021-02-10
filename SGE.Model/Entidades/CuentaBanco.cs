using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Entidades
{
    public class CuentaBanco: ICuentaBanco
    {
        public int IdCuentaBanco { get; set; }
        public int IdBeneficiario { get; set; }
        public short? IdSistema { get; set; }
        public int? NroCta { get; set; }
        public string Cbu { get; set; }
        public string UsuarioBanco { get; set; }
        public short? IdSucursal { get; set; }
        public short? IdMoneda { get; set; }
        public DateTime? FechaSolicitudCuenta { get; set; }

        public DateTime? FechaSistema { get; set; }
        public int? IdUsuarioSistema { get; set; }
        public string UsuarioSistema { get; set; }
    }
}
