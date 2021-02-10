using System.Collections.Generic;
using SGE.Servicio.VistaInterfaces.Shared;
using SGE.Model.Entidades.Interfaces;
using System;
using SGE.Servicio.Vista.Shared;

namespace SGE.Servicio.VistaInterfaces
{
    public interface IProyectoVista
    {
        //Proyecto seleccionado
        int? IdProyecto { get; set; }
        string NombreProyecto { get; set; }
        int? CantidadAcapacitar { get; set; }
        DateTime? FechaInicioProyecto { get; set; }
        DateTime? FechaFinProyecto { get; set; }
        int? MesesDuracionProyecto { get; set; }

        //Proyectos

        IList<IProyecto> Proyectos { get; set; }
        IPager Pager { get; set; }
        int Id { get; set; }
        string BusquedaDescripcion { get; set; }
        //ComboBox BusquedaEmpresas { get; set; }


    }
}
