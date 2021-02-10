using System.Collections.Generic;
using SGE.Model.Comun;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Repositorio
{
    public interface ITituloRepositorio
    {
        IList<ITitulo> GetTitulos();
        IList<ITitulo> GetTitulos(int skip, int take);
        IList<ITitulo> GetTitulos(string descripcion);
        IList<ITitulo> GetTitulos(string descripcion, int skip, int take);
        ITitulo GetTitulo (int id);
        int AddTitulo(ITitulo titulo);
        void UpdateTitulo(ITitulo titulo);
        void DeleteTitulo(ITitulo titulo);
        int GetTituloCount();
        bool ExistsTitulo(string titulo);
        IList<ITitulo> GetTitulosByUniversidad(int iduniversidad);
    }
}
