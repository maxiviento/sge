using System.Collections.Generic;
using SGE.Servicio.VistaInterfaces.Shared;

namespace SGE.Servicio.VistaInterfaces
{
    public interface IInicializarTablasVista
    {
        IList<string> Inserciones{ get; set; }

        string Accion { get; set; }
    }
}
