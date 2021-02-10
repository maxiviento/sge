using System.ComponentModel.DataAnnotations;
using SGE.Servicio.VistaInterfaces;

namespace SGE.Servicio.Vista
{
    public class UsuarioLoginVista : IUsuarioLoginVista
    {
        [Display(Name = "Usuario")]
        [Required(ErrorMessage = "El Usuario es requerido.")]
        [StringLength(20, ErrorMessage = "Nombre supera los 20 caracteres.")]
        public string Login { get; set; }

        [Display(Name = "Contraseña")]
        [Required(ErrorMessage = "La Contraseña es requerida.")]
        [StringLength(30, ErrorMessage = "Mínimo {2} caracteres - Máximo {1} caracteres", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Recuérdame ?")]
        public bool Persistir { get; set; }
    }
}
