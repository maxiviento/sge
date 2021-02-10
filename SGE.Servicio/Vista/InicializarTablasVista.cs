using System.Collections.Generic;
using SGE.Servicio.VistaInterfaces;

namespace SGE.Servicio.Vista
{
    public class InicializarTablasVista : IInicializarTablasVista
    {
        public InicializarTablasVista()
        {
            Inserciones=new List<string>();
        }

        public IList<string> Inserciones{ get; set; }

        public string Accion { get; set; }
    }
}
