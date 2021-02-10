using System;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Entidades
{
    public class Concepto : IConcepto
    {
        public int Id_Concepto { get; set; }
        public short Año { get; set; }
        public short Mes { get; set; }
        public string Observacion { get; set; }

        //Campos solo Utilizados para ver las Liquidaciones de los Beneficiarios
        public string Estado { get; set; }
        public int? NroCuenta { get; set; }
        public string Sucursal { get; set; }
        public int? NroResolucion { get; set; }
        public string SePagoaApoderado { get; set; }
        public DateTime FechaLiquidacion { get; set; }

        public int MontoPagado { get; set; }

        public DateTime? FechaSistema { get; set; }
        public int? IdUsuarioSistema { get; set; }
        public string UsuarioSistema { get; set; }
    }
}
