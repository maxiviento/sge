using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Repositorio
{
    public interface INivelRepositorio
    {
        IList<INivel> GetNiveles();
        INivel GetNivel(int id);
        int GetNivelesCount();
    }
}
