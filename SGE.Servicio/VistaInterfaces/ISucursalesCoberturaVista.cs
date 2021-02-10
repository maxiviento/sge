using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces.Shared;

namespace SGE.Servicio.VistaInterfaces
{
    public interface ISucursalesCoberturaVista
    {
        //IList<ISucursalCobertura> ListaSucursalesCobertura { get; set; }
        IList<ILocalidad> ListaSucursalesCobertura { get; set; }
        IPager Pager { get; set; }
        string Asignadas { get; set; }
        IComboBox BusquedaLocalidades { get; set; }
        IComboBox BusquedaSucursales { get; set; }
        IComboBox BusquedaDepartamentos { get; set; }
    }
}
