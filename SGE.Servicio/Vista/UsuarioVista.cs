using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using SGE.Servicio.VistaInterfaces;

namespace SGE.Servicio.Vista
{
    public class UsuarioVista : IUsuarioVista
    {
        [Key]
        [Display(Name = "Id:")]
        public int Id { get; set; }

        [Display(Name = "*Login")]
        [Required(ErrorMessage = "Debes ingresar el login.")]
        [StringLength(20, ErrorMessage = "Login supera los 20 caracteres.")]
        public string Login { get; set; }

        [Display(Name = "*Nombre")]
        [Required(ErrorMessage = "Debes ingresar el nombre.")]
        [StringLength(100, ErrorMessage = "Nombre supera los 100 caracteres.")]
        public string Nombre { get; set; }

        [Display(Name = "*Apellido")]
        [Required(ErrorMessage = "Debes ingresar el apellido.")]
        [StringLength(100, ErrorMessage = "Apellido supera los 30 caracteres.")]
        public string Apellido { get; set; }

        [Display(Name = "*Clave")]
        [Required(ErrorMessage = "Debes ingresar la clave.")]
        [StringLength(30, ErrorMessage = "Mínimo {2} caracteres - Máximo {1} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "*Confirmación")]
        [Required(ErrorMessage = "Debes ingresar la confirmación de la clave.")]
        [StringLength(30, ErrorMessage = "Mínimo {2} caracteres - Máximo {1} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "La clave y la confirmación de la clave no coinciden.")]
        public string ConfirmPassword { get; set; }

        public int IdEstado { get; set; }
        public string Accion { get; set; }

        public IList<UsuarioRolVista> Roles { get; set; }
    }
}
