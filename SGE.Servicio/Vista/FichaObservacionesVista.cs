using System;
using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces;

namespace SGE.Servicio.Vista
{
    public class FichaObservacionesVista : IFichaObservacionesVista
    {
        public IList<IFichaObservacion> Observaciones { get; set; }
        public IPager Pager { get; set; }
        public int Id { get; set; }
        public int IdFicha { get; set; }

        public string AccionFicha { get; set; }
        
    }
}
