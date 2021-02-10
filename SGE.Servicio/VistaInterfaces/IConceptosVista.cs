using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;
using SGE.Servicio.Vista.Shared;

namespace SGE.Servicio.VistaInterfaces
{
    public interface IConceptosVista
    {
        IList<IConcepto> Conceptos { get; set; }
        IPager Pager { get; set; }
        short? AñoBusqueda { get; set; }
        short? MesBusqueda { get; set; }
        string ObservacionBusqueda { get; set; }
    }
}
