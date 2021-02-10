using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;
using SGE.Servicio.Vista.Shared;

namespace SGE.Servicio.VistaInterfaces
{
    public interface ISedesVista
    {
        IList<ISede> Sedes { get; set; }
        IPager Pager { get; set; }
        int Id { get; set; }
        string BusquedaDescripcion { get; set; }        
        ComboBox BusquedaEmpresas { get; set; }
    }
}
