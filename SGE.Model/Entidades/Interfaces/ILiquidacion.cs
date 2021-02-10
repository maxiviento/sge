using System;
using System.Collections.Generic;

namespace SGE.Model.Entidades.Interfaces
{
    public interface ILiquidacion : IComunDatos
    {
        int IdPrograma { get; set; }
        int IdLiquidacion { get; set; }
        int IdEstadoLiquidacion { get; set; }
        DateTime FechaLiquidacion { get; set; }
        string Observacion { get; set; }
        int? NroResolucion { get; set; }
        string TxtNombre { get; set; }
        bool Generado { get; set; }
        string Estado { get; set; }
        IList<IBeneficiario> ListaBeneficiarios { get; set; }
        IEnumerable<IBeneficiario> Beneficiarios { get; set; }
        string NombreEstado { get; set; }
        string Programa { get; set; }
        int CantBenef { get; set; }
        string Usuario { get; set; }
        string Convenio { get; set; }
    }
}
