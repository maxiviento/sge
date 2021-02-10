using System.Collections.Generic;
using SGE.Servicio.VistaInterfaces.Shared;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Servicio.VistaInterfaces
{
    public interface IFichaObservacionVista
    {
        int IdFichaObservacion { get; set; }
        int IdFicha { get; set; }
        string Descripcion { get; set; }
        string Accion { get; set; }
        string AccionFicha { get; set; }
    }
}
