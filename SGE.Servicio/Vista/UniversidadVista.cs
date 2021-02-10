using System.ComponentModel.DataAnnotations;
using SGE.Servicio.VistaInterfaces;

namespace SGE.Servicio.Vista
{
    public class UniversidadVista : IUniversidadVista
    {
        [Key]
        [Display(Name = "*ID")]
        [Required(ErrorMessage = "Debes ingresar el Número.")]
        [Range( 0,9999,ErrorMessage = "Valor fuera de rango")]
        public int Id { get; set; }

        [Display(Name = "*Descripción")]
        [Required(ErrorMessage = "Debes ingresar la descripción.")]
        [StringLength(100, ErrorMessage = "Descripción supera los 100 caracteres.")]
        public string Descripcion { get; set; }

        public string Accion { get; set; }
        public string BuscarDescripcion {get; set; }
        public string BusquedaAnterior { get; set; }
    }
}
