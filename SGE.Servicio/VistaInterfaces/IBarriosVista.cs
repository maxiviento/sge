using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;
using SGE.Servicio.Vista.Shared;
using System;

namespace SGE.Servicio.VistaInterfaces
{
    public interface IBarriosVista
    {

        IList<IBarrio> barrios { get; set; }
        IPager Pager { get; set; }
        int Id { get; set; }
        string BusquedaDescripcion { get; set; }
    }
}
