using SGE.Servicio.VistaInterfaces;
using System.ComponentModel.DataAnnotations;

namespace SGE.Servicio.Vista
{
    public class DepartamentoVista : IDepartamentoVista
    {
        [Key]
        [Display(Name = "Id")]
        public int Id { get; set;}

        [Display(Name = "*Descripción")]
        [Required(ErrorMessage = "Debe ingresar la descripción.")]
        [StringLength(50, ErrorMessage = "Descripción supera los 50 caracteres.")]
        public string Descripcion { get; set;}

        public string Accion { get; set;}
    }
}
