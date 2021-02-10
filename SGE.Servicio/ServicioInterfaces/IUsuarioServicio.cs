using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces;

namespace SGE.Servicio.ServicioInterfaces
{
    public interface IUsuarioServicio
    {
        IUsuariosVista GetUsuarios();
        IUsuariosVista GetIndex();
        IUsuariosVista GetUsuarios(string login, string nombre, string apellido);
        IUsuariosVista GetUsuarios(IPager pager, string login, string nombre, string apellido);
        IUsuarioVista GetUsuario(int idUsuario, string accion);
        int AddUsuario(IUsuarioVista vista);
        bool UpdateUsuario(IUsuarioVista vista);
        bool DeleteUsuario(IUsuarioVista vista);
        int Login(IUsuarioLoginVista vista, ref string userData);
        int UpdatePassword(IUsuarioPasswordVista vista);
    }
}
