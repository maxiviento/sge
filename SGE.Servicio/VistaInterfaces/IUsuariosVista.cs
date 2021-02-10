using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;
using SGE.Servicio.Vista.Shared;

namespace SGE.Servicio.VistaInterfaces
{
    public interface IUsuariosVista
    {
        IList<IUsuario> Usuarios { get; set; }
        IPager Pager { get; set; }
        string Login { get; set; }
        string Apellido { get; set; }
        string Nombre { get; set; }
        string LoginBusqueda { get; set; }
        string ApellidoBusqueda { get; set; }
        string NombreBusqueda { get; set; }
    }
}
