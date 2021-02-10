using System.ComponentModel.DataAnnotations;
using SGE.Servicio.VistaInterfaces;

namespace SGE.Servicio.Vista
{
    public class UsuarioRolVista : IUsuarioRolVista
    {
        [Key]
        [Display(Name = "Id:")]
        public int IdRol { get; set; }

        [Display(Name = "Rol")]
        public string NombreRol { get; set; }

        [Display(Name = "Asignado")]
        public bool Selected { get; set; }
    }
}
