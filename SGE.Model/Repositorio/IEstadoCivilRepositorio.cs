using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Repositorio
{
    public interface IEstadoCivilRepositorio
    {
        IList<IEstadoCivil> GetEstadosCivil();
    }
}
