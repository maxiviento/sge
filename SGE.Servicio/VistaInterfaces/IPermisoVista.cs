using System.Collections.Generic;
using SGE.Servicio.Vista;

namespace SGE.Servicio.VistaInterfaces
{
    public interface IPermisoVista
    {
        int Id { get; set; }
        string Rol { get; set; }
        string Accion { get; set; }
        IList<FormularioVista> Formularios { get; set; }
    }
}
