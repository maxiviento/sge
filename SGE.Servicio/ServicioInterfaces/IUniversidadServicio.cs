using SGE.Servicio.VistaInterfaces;
using SGE.Servicio.Vista.Shared;

namespace SGE.Servicio.ServicioInterfaces
{
    public interface IUniversidadServicio
    {
        IUniversidadesVista GetUniversidades();
        IUniversidadesVista GetIndex();
        IUniversidadesVista GetUniversidades(string descripcion);
        IUniversidadVista GetUniversidad(int id, string accion);
        IUniversidadesVista GetUniversidades(Pager pager, int id, string descripcion);
        int AddUniversidad(IUniversidadVista vista);
        IUniversidadVista AddUniversidadLast(string accion);
        int UpdateUniversidad(IUniversidadVista vista);
    }
}
