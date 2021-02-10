using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SGE.Servicio.VistaInterfaces;

namespace SGE.Servicio.ServicioInterfaces
{
    public interface IActorServicio
    {
        IActorVista GetActorDNI(string dni, int rolActor);
        IActorVista GetActor(int idActor);
        IActorVista GetFacilitadorMonitorDNI(string dni);
    }
}
