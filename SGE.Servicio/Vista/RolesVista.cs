using System.Collections.Generic;
using SGE.Servicio.VistaInterfaces;
using SGE.Servicio.Vista.Shared;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Servicio.Vista
{
    public class RolesVista : IRolesVista
    {
        public IList<IRol> Roles { get; set; }
        public IPager Pager { get; set; }
        public string Nombre { get; set; }
        public string NombreBusqueda { get; set; }
    }
}
