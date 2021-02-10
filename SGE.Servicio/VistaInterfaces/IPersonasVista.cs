using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;
using SGE.Servicio.Vista.Shared;

namespace SGE.Servicio.VistaInterfaces
{
    public interface IPersonasVista
    {
        IList<IPersona> personas { get; set; }
        IPager Pager { get; set; }
        int Id { get; set; }
        string DescripcionBusca { get; set; }
        int idNivel { get; set; }
    }
}
