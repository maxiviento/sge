using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces;

namespace SGE.Servicio.ServicioInterfaces
{
    public interface IRolServicio
    {
        IRolesVista GetRoles();
        IRolesVista GetIndex();
        IRolesVista GetRoles(string nombre);
        IRolesVista GetRoles(IPager pager, string nombre);
        IRolVista GetRol(int idRol, string accion);
        int AddRol(IRolVista vista);
        int UpdateRol(IRolVista vista);
        int DeleteRol(IRolVista vista);
    }
}
