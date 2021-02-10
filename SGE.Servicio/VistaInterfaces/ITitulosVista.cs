using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces.Shared;

namespace SGE.Servicio.VistaInterfaces
{
    public interface ITitulosVista
    {
        IList<ITitulo> Titulos { get; set; }
        IPager Pager { get; set; }
        int Id { get; set; }
        string BusquedaPorDescripcion { get; set; }
        //IComboBox BusquedaPorUniversidad { get; set; }
    }
}
