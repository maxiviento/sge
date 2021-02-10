using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Repositorio
{
    public interface IUniversidadRepositorio
    {
        IList<IUniversidad> GetUniversidades();
        IList<IUniversidad> GetUniversidades(int skip, int make);
        IList<IUniversidad> GetUniversidades(string descripcion);
        IList<IUniversidad> GetUniversidades(string descripcion, int skip, int take);
        IUniversidad GetUniversidad(int id);
        int AddUniversidad(IUniversidad universidad);
        void UpdateUniversidad(IUniversidad universidad);
        int GetUniversidadMaxId();
        int GetUniversidadesCount();
    }
}
