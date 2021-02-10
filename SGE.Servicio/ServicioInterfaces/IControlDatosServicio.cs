using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces;

namespace SGE.Servicio.ServicioInterfaces
{
    public interface IControlDatosServicio
    {
        IControlDatosVista GetControlDatos(int idPrograma, int idEtapa);
        IControlDatosVista GetControlDatos(IPager pager, int idPrograma, int idEtapa);
        IControlDatosVista GetControlDatosIndex();
        byte[] ExportControlDatos(IControlDatosVista model);
       
    }
}
