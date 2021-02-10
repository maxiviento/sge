using System;
using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;
using SGE.Servicio.VistaInterfaces;

namespace SGE.Servicio.Vista
{
    public class ApoderadosVista: IApoderadosVista
    {
        public IList<IApoderado> Apoderados { get; set; }
        public string Apellido { get; set; }
        public string Nombre { get; set; }
        public string NumeroDocumento { get; set; }
        public DateTime ApoderadoDesde { get; set; }
        public DateTime ApoderadoHasta { get; set; }
        public short? IdEstadoApoderado { get; set; }
        public string DescripcionEstadoApoderado { get; set; }
    }
}
