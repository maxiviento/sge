using System.ComponentModel.DataAnnotations;
using SGE.Servicio.Comun;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces;
using SGE.Servicio.VistaInterfaces.Shared;

namespace SGE.Servicio.Vista
{
    public class SedeVista: ISedeVista
    {
        public SedeVista()
        {
            Empresas = new ComboBox();
            Localidades = new ComboBox();
        }

        [Key]
        [Display(Name = "Id:")]
        public int Id { get; set; }

        [Display(Name = "*Descripción")]
        [Required(ErrorMessage = "Debes ingresar la descripción.")]
        [StringLength(200, ErrorMessage = "Descripción supera los 200 caracteres.")]
        public string Descripcion { get; set; }

        [Display(Name = "*Empresas")]
        public IComboBox Empresas { get; set; }

        [Display(Name = "*Localidades")]
        public ComboBox Localidades { get; set; }

        [Display(Name = "*Calle")]
        [StringLength(100, ErrorMessage = "Calle supera los 100 caracteres.")]
        public string Calle { get; set; }

        [Display(Name = "*Número")]
        [StringLength(10, ErrorMessage = "Número supera los 10 caracteres.")]
        public string Numero { get; set; }

        [Display(Name = "*Piso")]
        [StringLength(10, ErrorMessage = "Piso supera los 10 caracteres.")]
        public string Piso { get; set; }

        [Display(Name = "*Departamento")]
        [StringLength(10, ErrorMessage = "Departamento supera los 10 caracteres.")]
        public string Dpto { get; set; }

        [Display(Name = "*Código Postal")]
        [StringLength(10, ErrorMessage = "Código Postal supera los 10 caracteres.")]
        public string CodigoPostal { get; set; }

        [Display(Name = "*Apellido del Contacto")]
        [StringLength(150, ErrorMessage = "Apellido del contacto supera los 150 caracteres.")]
        public string ApellidoContacto { get; set; }

        [Display(Name = "*Nombre del Contacto")]
        [StringLength(150, ErrorMessage = "Nombre del contacto supera los 150 caracteres.")]
        public string NombreContacto { get; set; }

        [Display(Name = "*Teléfono")]
        [StringLength(50, ErrorMessage = "Teléfono supera los 50 caracteres.")]
        [Required(ErrorMessage = "Debes ingresar el Teléfono.")]
        public string Telefono { get; set; }

        [Display(Name = "*Fax")]
        [StringLength(50, ErrorMessage = "Fax supera los 50 caracteres.")]
        public string Fax { get; set; }

        [ValidateEmail]
        [StringLength(75, ErrorMessage = "Email supera los 75 caracteres.")]
        [Required(ErrorMessage = "Debes ingresar el Email.")]
        public string Email { get; set; }

        public string Accion { get; set; }

        public int IdEmpresa { get; set; }
    }
}
