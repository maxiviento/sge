using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SGE.Servicio.VistaInterfaces;

namespace SGE.Servicio.ServicioInterfaces
{
    public interface IProyectoServicio
    {
  //      IProyectoVista GetProyectos();
        IProyectoVista GetProyectos(string NombreProyecto);
    }
}
