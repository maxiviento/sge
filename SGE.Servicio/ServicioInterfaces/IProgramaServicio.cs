using SGE.Servicio.VistaInterfaces;
using SGE.Servicio.Vista.Shared;
using SGE.Model.Entidades.Interfaces;
using System.Collections.Generic;

namespace SGE.Servicio.ServicioInterfaces
{
    public interface IProgramaServicio
    {
        IProgramasVista GetProgramas();
        IProgramasVista GetIndex();
        IProgramasVista GetProgramas(string descripcion);
        IProgramaVista GetPrograma(int id, string accion);
        IProgramasVista GetProgramas(Pager pager, int id, string descripcion);
        int AddPrograma(IProgramaVista programa);
        IProgramaVista AddProgramaLast(string accion);
        int UpdatePrograma(IProgramaVista programa);
        IList<IPrograma> GetPrograma(string programa);
    }
}
