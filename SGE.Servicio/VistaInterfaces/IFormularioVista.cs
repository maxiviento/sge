using System.Collections.Generic;
using SGE.Servicio.Vista;

namespace SGE.Servicio.VistaInterfaces
{
    public interface IFormularioVista
    {
        int Id { get; set; }
        string Formulario { get; set; }
        IList<AccionVista> Acciones { get; set; }
    }
}
