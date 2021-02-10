using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;
using SGE.Servicio.Vista.Shared;

namespace SGE.Servicio.VistaInterfaces
{
    public interface IRolesVista
    {
        IList<IRol> Roles { get; set; }
        IPager Pager { get; set; }
        string Nombre { get; set; }
        string NombreBusqueda { get; set; }
    }
}
