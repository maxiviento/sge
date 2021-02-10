using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces;

namespace SGE.Servicio.Vista
{
    public class UniversidadesVista : IUniversidadesVista
    {
        public IList<IUniversidad> Universidades { get; set; }
        public IPager Pager { get; set; }
        public int Id { get; set; }
        public string DescripcionBusca { get; set; }
    }
}
