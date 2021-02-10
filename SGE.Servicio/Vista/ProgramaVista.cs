using System.ComponentModel.DataAnnotations;
using SGE.Servicio.VistaInterfaces;

namespace SGE.Servicio.Vista
{
    public class ProgramaVista : IProgramaVista
    {
        [Key]
        [Display(Name = "*ID")]
        [Required(ErrorMessage = "Debes ingresar el ID.")]
        [Range( 0,99,ErrorMessage = "Valor fuera de rango")]
        public int Id { get; set; }

        [Display(Name = "*Descripción")]
        [Required(ErrorMessage = "Debes ingresar la descripción.")]
        [StringLength(200, ErrorMessage = "Descripción supera los 200 caracteres.")]
        public string Descripcion { get; set; }

        [Display(Name = "Monto del Programa")]
        [Range( 0,99999999.99,ErrorMessage = "Valor fuera de rango.")]
        public decimal MontoPrograma { get; set; }

        [Display(Name = "Convenio")]
        [StringLength(20, ErrorMessage = "Convenio supera los 20 caracteres.")]
        public string ConvenioBcoCba { get; set; }

        public string Accion { get; set; }
        public string BuscarDescripcion {get; set; }
        public string BusquedaAnterior { get; set; }
    }
}
