using System.ComponentModel.DataAnnotations;
using SGE.Servicio.VistaInterfaces;
using SGE.Model.Entidades.Interfaces;
using System.Collections.Generic;

namespace SGE.Servicio.Vista
{
    public class InstitucionVista: IInstitucionVista
    {

        [Key]
        [Display(Name = "*Id")]
        [Required(ErrorMessage = "Debes ingresar el Id.")]
        public int Id { get; set; }

        [Display(Name = "*Descripción")]
        [Required(ErrorMessage = "Debes ingresar la descripción.")]
        [StringLength(200, ErrorMessage = "Descripción supera los 200 caracteres.")]
        public string Descripcion { get; set; }

        public string Accion { get; set; }

        public IList<ISector> Sectores { get; set; } // 07/03/2013 - DI CAMPLI LEANDRO - SE AÑADE ASIGNACION DE SECTORES A INSTITUCIONES
        public string restultados { get; set; } // 08/03/2013 - DI CAMPLI LEANDRO 
    }
}
