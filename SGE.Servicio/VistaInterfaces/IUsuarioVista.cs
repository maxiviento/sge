using System.Collections.Generic;
using SGE.Servicio.Vista;

namespace SGE.Servicio.VistaInterfaces
{
    public interface IUsuarioVista
    {
        int Id { get; set; }
        string Login { get; set; }
        string Nombre { get; set; }
        string Apellido { get; set; }
        string Password { get; set; }
        string ConfirmPassword { get; set; }
        int IdEstado { get; set; }
        string Accion { get; set; }
        IList<UsuarioRolVista> Roles { get; set; }
    }
}
