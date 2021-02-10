using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;
using SGE.Servicio.Vista.Shared;

namespace SGE.Servicio.VistaInterfaces
{
    public interface ISectoresVista
    {

        IList<ISector> Sectores { get; set; }
        IPager Pager { get; set; }
        int Id { get; set; }
        string BusquedaDescripcion { get; set; }        
        ComboBox BusquedaSectores { get; set; }

    }
}
