using System.Collections.Generic;
using SGE.Servicio.VistaInterfaces;
using SGE.Servicio.Vista.Shared;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Servicio.Vista
{
    class SectoresVista : ISectoresVista
    {

        public SectoresVista()
        {
            BusquedaSectores = new ComboBox();
        }

        public IList<ISector> Sectores { get; set; }
        public IPager Pager { get; set; }
        public int Id { get; set; }
        public string BusquedaDescripcion { get; set; }
        public ComboBox BusquedaSectores { get; set; }
    }
}
