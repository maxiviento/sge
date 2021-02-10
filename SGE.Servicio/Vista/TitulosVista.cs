using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces;
using SGE.Servicio.VistaInterfaces.Shared;
namespace SGE.Servicio.Vista
{
    public class TitulosVista :ITitulosVista
    {
        public IList<ITitulo> Titulos { get; set; }
        public IPager Pager { get; set; }
        public int Id { get; set; }
        public string BusquedaPorDescripcion { get; set; }
        //public IComboBox BusquedaPorUniversidad { get; set; }
    }
}
