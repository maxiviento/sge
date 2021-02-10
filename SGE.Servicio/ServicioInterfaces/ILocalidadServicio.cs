using SGE.Servicio.VistaInterfaces;
using SGE.Servicio.Vista.Shared;

namespace SGE.Servicio.ServicioInterfaces
{
    public interface ILocalidadServicio
    {
        ILocalidadesVista GetLocalidades();
        ILocalidadesVista GetIndex();
        ILocalidadesVista GetLocalidades(string descripcion);
        ILocalidadVista GetLocalidad(int id, string accion);
        ILocalidadesVista GetLocalidades(Pager pager, int id, string descripcion);
        int AddLocalidad(ILocalidadVista localidad);
        ILocalidadVista AddLocalidadLast(string accion);
        int UpdateLocalidad(ILocalidadVista localidad);
        ILocalidadVista GetSucursalCoberturaByLocalidad(int idLocalidad);
    }
}
