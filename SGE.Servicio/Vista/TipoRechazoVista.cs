using System.ComponentModel.DataAnnotations;
using SGE.Servicio.VistaInterfaces;

namespace SGE.Servicio.Vista
{
    public class TipoRechazoVista : ITipoRechazoVista
    {
        [Key]
        [Display(Name = "*ID")]
        [Range(0,9999999999,ErrorMessage = "Valor fuera de rango")] 
        [Required(ErrorMessage = "Debes ingresar el ID.")]
        public int Id { get; set; }
        public int IdOld { get; set; }

        [Display(Name = "*Descripción")]
        [Required(ErrorMessage = "Debes ingresar la Descripción.")]
        [StringLength(250, ErrorMessage = "Descripción supera los 250 caracteres.")]
        public string Descripcion { get; set; }

        public string Accion { get; set; }

        public bool Selected { get; set; }
    }
}
