using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces;
using SGE.Servicio.VistaInterfaces.Shared;

namespace SGE.Servicio.Vista
{
    public class SucursalesCoberturaVista : ISucursalesCoberturaVista
    {
        public SucursalesCoberturaVista()
        {
            Asignadas = "T";
            BusquedaLocalidades = new ComboBox();
            BusquedaSucursales = new ComboBox();
            BusquedaDepartamentos = new ComboBox();
        }

        //public IList<ISucursalCobertura> ListaSucursalesCobertura { get; set; }
        public IList<ILocalidad > ListaSucursalesCobertura { get; set; }
        public IPager Pager { get; set; }
        public string Asignadas { get; set; }
        public IComboBox BusquedaLocalidades { get; set; }
        public IComboBox BusquedaSucursales { get; set; }
        public IComboBox BusquedaDepartamentos { get; set; }
    }
}
