using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces;
using System.ComponentModel.DataAnnotations;
using System;

namespace SGE.Servicio.Vista
{
    public class EscuelasVista : IEscuelasVista
    {

        public EscuelasVista()
        {
            escuelas = new List<IEscuela>();
        
        }

        public IList<IEscuela> escuelas { get; set; }
        public IPager Pager { get; set; }
        public int Id { get; set; }
        public string BusquedaDescripcion { get; set; }
    }
}
