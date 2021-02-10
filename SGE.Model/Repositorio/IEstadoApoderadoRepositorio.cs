using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Repositorio
{
    public interface IEstadoApoderadoRepositorio
    {
        IList<IEstadoApoderado> GetEstadosApoderado();
    }
}
