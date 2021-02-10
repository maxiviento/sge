using System.Collections.Generic;
using SGE.Servicio.VistaInterfaces;
using SGE.Servicio.Vista.Shared;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Servicio.Vista
{
    public class UsuariosVista : IUsuariosVista
    {
        public IList<IUsuario> Usuarios { get; set; }
        public IPager Pager { get; set; }
        public string Login { get; set; }
        public string Apellido { get; set; }
        public string Nombre { get; set; }
        public string LoginBusqueda { get; set; }
        public string ApellidoBusqueda { get; set; }
        public string NombreBusqueda { get; set; }
    }
}
