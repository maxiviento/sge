using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SGE.Servicio.VistaInterfaces;

namespace SGE.Servicio.Vista
{
    public class FormularioVista : IFormularioVista
    {
        [Key]
        [Display(Name = "Id:")]
        public int Id { get; set; }

        [Display(Name = "Formulario")]
        public string Formulario { get; set; }

        public IList<AccionVista> Acciones { get; set; }
    }
}
