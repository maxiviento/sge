using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Repositorio
{
    public interface IProyectoRepositorio
    {
        IProyecto GetProyecto(int id);
        int AddProyecto(IProyecto proyecto);
        void UpdateProyecto(IProyecto proyecto);
        void DeleteProyecto(IProyecto proyecto);
        IList<IProyecto> GetProyectos(string NombreProyecto);
    }
}
