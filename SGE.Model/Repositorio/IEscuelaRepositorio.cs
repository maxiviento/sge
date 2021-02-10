using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Repositorio
{
    public interface IEscuelaRepositorio
    {
        IList<IEscuela> GetEscuelas();
        IList<IEscuela> GetEscuelasByLocalidad(int? idLocalidad);
        IList<IEscuela> GetEscuelasCompleto();
        IList<IEscuela> GetEscuelas(string nombre);
        IEscuela GetEscuelaCompleto(int idEscuela);
        int AddEscuela(IEscuela escuela);
        void UpdateEscuela(IEscuela escuela);
    }
}
