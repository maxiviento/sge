using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;
using SGE.Servicio.Vista.Shared;

namespace SGE.Servicio.VistaInterfaces
{
    public interface IProgramasVista
    {
        IList<IPrograma> Programas{ get; set; }
        IPager Pager { get; set; }
        int Id { get; set; }
        string DescripcionBusca { get; set; }
    }
}
