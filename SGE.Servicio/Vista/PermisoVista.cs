using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SGE.Servicio.VistaInterfaces;

namespace SGE.Servicio.Vista
{
    public class PermisoVista : IPermisoVista
    {
        [Key]
        [Display(Name = "Id:")]
        public int Id { get; set; }

        [Display(Name = "Rol")]
        public string Rol { get; set; }

        [Display(Name = "Accion")]
        public string Accion { get; set; }

        public IList<FormularioVista> Formularios { get; set; }
    }
}
