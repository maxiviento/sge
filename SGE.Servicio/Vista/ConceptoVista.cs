using System.ComponentModel.DataAnnotations;
using SGE.Servicio.VistaInterfaces;

namespace SGE.Servicio.Vista
{
    public class ConceptoVista : IConceptoVista
    {
        public ConceptoVista()
        {
            Accion = "Ver";
        }

        public int IdConcepto { get; set; }

        [Required(ErrorMessage = "El Año es requerido.")]
        [Range(2012, 2100, ErrorMessage = "Año debe ser entre {1} y {2}.")]
        public short? Año { get; set; }

        [Required(ErrorMessage = "El Mes es requerido.")]
        [Range(1, 12, ErrorMessage = "Mes debe ser entre {1} y {2}.")]
        public short? Mes { get; set; }

        public string Observacion { get; set; }
        public string Accion { get; set; }
    }
}
