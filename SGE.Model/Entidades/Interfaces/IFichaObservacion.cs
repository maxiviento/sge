using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGE.Model.Entidades.Interfaces
{
    public interface IFichaObservacion : IComunDatos
    {
        int IdFichaObservacion { get; set; }
        int IdFicha { get; set; }
        string Observacion { get; set; }
    }
}
