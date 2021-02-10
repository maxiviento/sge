using System.ComponentModel.DataAnnotations;
using SGE.Servicio.VistaInterfaces;

namespace SGE.Servicio.Vista
{
    public class RolVista : IRolVista
    {
        [Key]
        [Display(Name = "Id:")]
        public int Id { get; set; }

        [Display(Name = "*Rol")]
        [Required(ErrorMessage = "Debes ingresar el nombre del Rol.")]
        [StringLength(50, ErrorMessage = "Nombre del Rol supera los 50 caracteres.")]
        public string Nombre { get; set; }

        public int IdEstado { get; set; }

        public string Accion { get; set; }
    }
}
