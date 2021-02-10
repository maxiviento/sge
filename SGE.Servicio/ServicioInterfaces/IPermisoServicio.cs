using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces;

namespace SGE.Servicio.ServicioInterfaces
{
    public interface IPermisoServicio
    {
        IPermisosVista GetRoles();
        IPermisosVista GetIndex();
        IPermisosVista GetRoles(string nombre);
        IPermisosVista GetRoles(IPager pager, string nombre);
        IPermisoVista GetPermiso(int idRol, string accion);
        bool UpdatePermiso(IPermisoVista vista);
    }
}
