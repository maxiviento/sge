using System;
using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Servicio.VistaInterfaces
{
    public interface IApoderadosVista
    {
        IList<IApoderado> Apoderados { get; set; }
        string Apellido { get; set; }
        string Nombre { get; set; }
        string NumeroDocumento { get; set; }
        DateTime ApoderadoDesde { get; set; }
        DateTime ApoderadoHasta { get; set; }
        short? IdEstadoApoderado { get; set; }
        string DescripcionEstadoApoderado { get; set; }
    }
}
