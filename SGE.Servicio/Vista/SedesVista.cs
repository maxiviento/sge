using System.Collections.Generic;
using SGE.Servicio.VistaInterfaces;
using SGE.Servicio.Vista.Shared;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Servicio.Vista
{
    public class SedesVista: ISedesVista
    {
        public SedesVista()
        {
            BusquedaEmpresas = new ComboBox();
        }
        
        public IList<ISede> Sedes { get; set; }
        public IPager Pager { get; set; }
        public int Id { get; set; }
        public string BusquedaDescripcion { get; set; }
        public ComboBox BusquedaEmpresas { get; set; }

    }
}
