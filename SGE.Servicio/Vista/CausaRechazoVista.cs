using System.ComponentModel.DataAnnotations;
using SGE.Servicio.VistaInterfaces;

namespace SGE.Servicio.Vista
{
    public class CausaRechazoVista : ICausaRechazoVista
    {
        [Key]
        [Display(Name = "*Id")]
        [Range(0, 9999999999, ErrorMessage = "Valor fuera de rango")]
        [Required(ErrorMessage = "Debes ingresar el Id.")]
        public int Id { get; set; }

        public int IdOld { get; set; }

        [Display(Name = "*Descripción")]
        [Required(ErrorMessage = "Debe ingresar la descripción.")]
        [StringLength(50, ErrorMessage = "Descripción supera los 50 caracteres.")]
        public string Descripcion { get; set; }

        public string Accion { get; set; }
        public string BusquedaAnterior  { get; set; }
    }
}
