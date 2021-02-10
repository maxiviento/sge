using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Servicio.VistaInterfaces
{
    public interface IArchivosVista
    {
        IList<IArchivo> ListaArchivos { get; set; }
        DateTime FechaLimpieza { get; set; }
        string Url { get; set; }
        string Nombre { get; set; }
    }
}
