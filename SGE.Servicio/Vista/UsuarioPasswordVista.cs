using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using SGE.Servicio.VistaInterfaces;

namespace SGE.Servicio.Vista
{
    public class UsuarioPasswordVista : IUsuarioPasswordVista
    {
        public string Login { get; set; }

        [Display(Name = "*Clave Actual")]
        [Required(ErrorMessage = "Debes ingresar la clave actual.")]
        [StringLength(30, ErrorMessage = "Mínimo {2} caracteres - Máximo {1} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Display(Name = "*Nueva Clave")]
        [Required(ErrorMessage = "Debes ingresar la nueva clave.")]
        [StringLength(30, ErrorMessage = "Mínimo {2} caracteres - Máximo {1} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Display(Name = "*Nueva Clave Confirmación")]
        [Required(ErrorMessage = "Debes ingresar la confirmación de la clave.")]
        [StringLength(30, ErrorMessage = "Mínimo {2} caracteres - Máximo {1} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "La nueva clave y la confirmación de la nueva clave no coinciden.")]
        public string NewPasswordConfirm { get; set; }
       
    }
}
