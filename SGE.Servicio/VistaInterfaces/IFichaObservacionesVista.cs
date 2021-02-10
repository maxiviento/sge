using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;
using SGE.Servicio.Vista.Shared;

namespace SGE.Servicio.VistaInterfaces
{
    public interface IFichaObservacionesVista
    {
        IList<IFichaObservacion> Observaciones { get; set; }
        IPager Pager { get; set; }
        int Id { get; set; }
        int IdFicha { get; set; }
        string AccionFicha { get; set; }
    }
}
