using System;
using System.ComponentModel.DataAnnotations;
using SGE.Servicio.VistaInterfaces;

namespace SGE.Servicio.Vista
{
    public class FichaObservacionVista : IFichaObservacionVista
    {
        public int IdFichaObservacion { get; set; }

        public int IdFicha { get; set; }

        [Display(Name = "*Observación")]
        [Required(ErrorMessage = "Debes ingresar una observación.")]
        [StringLength(1000, ErrorMessage = "La observación supera los 1000 caracteres.")]
        public string Descripcion { get; set; }

        public string Accion { get; set; }

        public string AccionFicha { get; set; }
    }
}
