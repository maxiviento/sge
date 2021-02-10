using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SGE.Servicio.VistaInterfaces;
using SGE.Servicio.Vista.Shared;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Servicio.ServicioInterfaces
{
    public interface IBarrioServicio
    {
        IBarriosVista GetBarrios(string nombre);
        //ICursoVista GetBarrios(string nombre);
        IBarriosVista GetIndex();
        IBarriosVista GetBarrios(IPager pPager, string nombre);
        IBarrioVista GetBarrio(int idBarrio, string accion);
        int AddBarrio(IBarrioVista vista);
        int UpdateBarrio(IBarrioVista vista);
        IBarrioVista GetBarrioReload(IBarrioVista vista);
        IList<IBarrio> GetBarriosLocalidad(int idLocalidad);
    }
}
