using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Servicio.VistaInterfaces
{
    public interface IImportarLiquidacionVista
    {
        IList<IBeneficiario> ListaBeneficiarios { get; set; }
        IList<IApoderado> ListaApoderados { get; set; }
        ILiquidacion Liquidacion { get; set; }
        bool Importado { get; set; }
        string Mensaje { get; set; }
        string Archivo { get; set; }
        bool Proceso { get; set; }
        int IdLiquidacion { get; set; }
    }
}
