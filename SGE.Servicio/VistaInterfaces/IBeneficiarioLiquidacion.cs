using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;
using SGE.Servicio.VistaInterfaces.Shared;

namespace SGE.Servicio.VistaInterfaces
{
    public interface IBeneficiarioLiquidacion :IBeneficiario
    {
        IList<IConcepto> Conceptos { get; set; }
        IComboBox ComboConceptos { get; set; }
        bool Apto { get; set; }
        bool DniEncontrado { get; set; }
        decimal Test { get; set; }
    }
}
