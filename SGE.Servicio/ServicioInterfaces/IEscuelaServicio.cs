using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SGE.Servicio.VistaInterfaces;
using SGE.Servicio.Vista.Shared;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Servicio.ServicioInterfaces
{
    public interface IEscuelaServicio
    {
        IEscuelasVista GetEscuelas(string NombreEscuela);
        IEscuelasVista GetIndex();
        IEscuelasVista GetEscuelas(IPager pPager, string NombreEscuela);
        IEscuelaVista GetEscuela(int idEscuela, string accion);
        int AddEscuela(IEscuelaVista vista);
        int UpdateEscuela(IEscuelaVista vista);
        IEscuelaVista GetEscuelaReload(IEscuelaVista vista);
        List<ILocalidad> cboCargarLocalidad(int idDep);
        List<IBarrio> cboCargarBarrio(int idLocalidad);
        List<IEscuela> GetEscuelasLst(string NombreEscuela);
    }
}
