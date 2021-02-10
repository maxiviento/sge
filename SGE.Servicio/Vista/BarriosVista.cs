using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces;
using System.ComponentModel.DataAnnotations;
using System;

namespace SGE.Servicio.Vista
{
    public class BarriosVista : IBarriosVista
    {
        public BarriosVista()
        {
            barrios = new List<IBarrio>();
        
        }
        public IList<IBarrio> barrios { get; set; }
        public IPager Pager { get; set; }
        public int Id { get; set; }
        public string BusquedaDescripcion { get; set; }
    }
}
