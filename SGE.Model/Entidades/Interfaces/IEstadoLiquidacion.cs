using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGE.Model.Entidades.Interfaces
{
    public interface IEstadoLiquidacion
    {
        int IdEstadoLiquidacion {get;set;}
        string NombreEstado { get; set; }
    }
}
