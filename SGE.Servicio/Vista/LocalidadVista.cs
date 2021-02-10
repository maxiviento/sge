using System.ComponentModel.DataAnnotations;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces;
using SGE.Servicio.VistaInterfaces.Shared;

namespace SGE.Servicio.Vista
{
    public class LocalidadVista : ILocalidadVista
    {
        public LocalidadVista()
        {
            NombreDepartamentoCombo = new ComboBox();
        }

        [Key]
        [Display(Name = "*ID")]
        [Required(ErrorMessage = "Debes ingresar el ID.")]
        [Range( 0,9999999999, ErrorMessage = "Valor fuera de rango")]
        public int Id { get; set; }

        [Display(Name = "*Nombre")]
        [Required(ErrorMessage = "Debes ingresar Nombre.")]
        [StringLength(100, ErrorMessage = "Nombre supera los 100 caracteres.")]
        public string Nombre{ get; set; }

        public int IdDepartamento { get; set; }

        [Display(Name = "*Departamento")]
        [Required(ErrorMessage = "Debes ingresar Nombre del Departamento.")]
        [StringLength(50, ErrorMessage = "Nombre del Departamento supera los 50 caracteres.")]
        public string NombreDepartamento { get; set; }

        [Display(Name = "*Departamento")]
        [Required(ErrorMessage = "Debes ingresar Nombre del Departamento.")]
        public IComboBox NombreDepartamentoCombo { get; set; }

        public string Accion { get; set; }
        public string BuscarDescripcion {get; set; }
        public string BusquedaAnterior { get; set; }
        public int IdSucursal { get; set; }
        public string NombreSucursal { get; set; }
    }
}
