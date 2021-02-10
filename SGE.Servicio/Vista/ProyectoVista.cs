using SGE.Servicio.VistaInterfaces;
using System;
using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;
using SGE.Servicio.Vista.Shared;

namespace SGE.Servicio.Vista
{
    public class ProyectoVista : IProyectoVista
    {

        public int? IdProyecto { get; set; }
        public string NombreProyecto { get; set; }
        public int? CantidadAcapacitar { get; set; }
        public DateTime? FechaInicioProyecto { get; set; }
        public DateTime? FechaFinProyecto { get; set; }
        public int? MesesDuracionProyecto { get; set; }

        public IList<IProyecto> Proyectos { get; set; }
        public IPager Pager { get; set; }
        public int Id { get; set; }
        public string BusquedaDescripcion { get; set; }
    }
}
