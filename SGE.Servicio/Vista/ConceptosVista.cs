using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SGE.Model.Entidades.Interfaces;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces;

namespace SGE.Servicio.Vista
{
    public class ConceptosVista : IConceptosVista
    {
        public IList<IConcepto> Conceptos { get; set; }
        public IPager Pager { get; set; }

        [Range(2012, 2100)]
        public short? AñoBusqueda { get; set; }
        
        [Range(1, 12)]
        public short? MesBusqueda { get; set; }
        
        public string ObservacionBusqueda { get; set; }
    }
}
