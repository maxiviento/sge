using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGE.Model.Entidades.Interfaces
{
    public interface IProyecto : IComunDatos
    {
        int? IdProyecto { get; set; }
        string NombreProyecto { get; set; }
        Int16? CantidadAcapacitar { get; set; }
        DateTime? FechaInicioProyecto { get; set; }
        DateTime? FechaFinProyecto { get; set; }
        Int16? MesesDuracionProyecto { get; set; }
    }
}
