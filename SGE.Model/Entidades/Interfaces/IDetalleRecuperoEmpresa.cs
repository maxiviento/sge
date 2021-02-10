using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGE.Model.Entidades.Interfaces
{
    public interface IDetalleRecuperoEmpresa
    {
        int ID_DETALLE_REC_DEB { get; set; }
        int? ID_RECUPERO_DEB { get; set; }
        int? ID_BENEFICIARIO_CONCEP_LIQ { get; set; }
        decimal MONTO_PARCIAL { get; set; }
        int? ID_USR_SIST { get; set; }
        DateTime? FEC_SIST { get; set; }
        int? ID_USR_MODIF { get; set; }
        DateTime? FEC_MODIF { get; set; }

    }
}
