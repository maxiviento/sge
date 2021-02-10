using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces;

namespace SGE.Servicio.Vista
{
    public class PermisosVista : IPermisosVista
    {
        public IList<IRol> Roles { get; set; }
        public IPager Pager { get; set; }
        public string Nombre { get; set; }
    }
}
