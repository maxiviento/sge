using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Repositorio
{
    public interface IEstadoFichaRepositorio
    {
        IList<IEstadoFicha> GetEstadosFicha();
    }
}
