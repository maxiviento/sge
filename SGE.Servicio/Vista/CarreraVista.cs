using System.ComponentModel.DataAnnotations;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces;
using SGE.Servicio.VistaInterfaces.Shared;

namespace SGE.Servicio.Vista
{
    public class CarreraVista : ICarreraVista
    {
        public CarreraVista()
        {
            NombreNivel= new ComboBox();
            NombreInstitucion = new ComboBox();
            NombreSector = new ComboBox();
        }

        [Key]
        [Display(Name = "*ID")]
        [Required(ErrorMessage = "Debes ingresar el ID.")]
        [Range( 0,9999999999, ErrorMessage = "Valor fuera de rango")]
        public int Id { get; set; }

        [Display(Name = "*Nombre")]
        [Required(ErrorMessage = "Debes ingresar Nombre.")]
        [StringLength(200, ErrorMessage = "Nombre supera los 200 caracteres.")]
        public string Nombre{ get; set; }

        public int? IdNivel { get; set; }
        public int? IdInstitucion { get; set; }
        public int? IdSector { get; set; }

        [Display(Name = "*Nivel")]
        [Required(ErrorMessage = "Debes seleccionar el Nivel.")]
        public IComboBox NombreNivel { get; set; }

        [Display(Name = "*Institución")]
        [Required(ErrorMessage = "Debes seleccionar la Institucion.")]
        public IComboBox NombreInstitucion { get; set; }

        [Display(Name = "*Sector")]
        [Required(ErrorMessage = "Debes seleccionar el Sector.")]
        public IComboBox NombreSector { get; set; }

        public string Accion { get; set; }
        public string BuscarDescripcion {get; set; }
        public string BusquedaAnterior { get; set; }
    }
}
