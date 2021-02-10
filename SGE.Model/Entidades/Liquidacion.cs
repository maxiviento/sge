using System;
using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Entidades
{
    public class Liquidacion : ILiquidacion
    {
        public Liquidacion()
        {
            ListaBeneficiarios = new List<IBeneficiario>();
        }

        public int IdPrograma { get; set; }
        public int IdLiquidacion { get; set; }
        public int IdEstadoLiquidacion { get; set; }
        public DateTime FechaLiquidacion { get; set; }
        public string Observacion { get; set; }
        public int? NroResolucion { get; set; }
        public string TxtNombre { get; set; }
        public bool Generado { get; set; }
        public string Estado { get; set; }
        public IList<IBeneficiario> ListaBeneficiarios { get; set; }
        public IEnumerable<IBeneficiario> Beneficiarios { get; set; }
        public string NombreEstado { get; set; }
        public string Programa { get; set; }
        public int CantBenef { get; set; }
        public string Usuario { get; set; }
        public string Convenio { get; set; }
        public DateTime? FechaSistema { get; set; }
        public int? IdUsuarioSistema { get; set; }
        public string UsuarioSistema { get; set; }
    }
}
