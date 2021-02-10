using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Repositorio
{
    public interface ICursoRepositorio
    {
        ICurso getCurso(int idCurso);
        IList<ICurso> GetCursos(string nombre);
        IList<ICurso> GetCursos(int nroCurso);
        IList<ICurso> getCursos(int idCurso, string nombre);
        int AddCurso(ICurso curso);
        void UpdateCurso(ICurso curso);
        IList<IFichasCurso> GetFichasCurso(int idcurso);
        IList<ICurso> GetCursosEscuela(string nombre);
    }
}
