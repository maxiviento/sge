using System;
using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;
using SGE.Servicio.VistaInterfaces;

namespace SGE.Servicio.Vista
{
    public class ArchivosVista : IArchivosVista
    {
        public IList<IArchivo> ListaArchivos { get; set; }
        public DateTime FechaLimpieza { get; set; }
        public string Url { get; set; }
        public string Nombre { get; set; }
    }
}
