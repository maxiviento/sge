using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Repositorio
{
    public interface IDepartamentoRepositorio
    {
        IList<IDepartamento> GetDepartamentos();
        IList<IDepartamento> GetDepartamentos(int skip, int make);
        IList<IDepartamento> GetDepartamentos(string descripcion);
        IList<IDepartamento> GetDepartamentos(string descripcion, int skip, int take);
        IDepartamento GetDepartamento(int id);
        int AddDepartamento(IDepartamento depto);
        void UpdateDepartamento(IDepartamento depto);
        void DeleteDepartamento(IDepartamento depto);
        bool ExistsDepartamento(string depto);
        int GetDepartamentosCount();
        int GetDepartamentoMaxId();
    }
}
