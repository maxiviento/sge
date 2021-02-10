using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Repositorio
{
    public interface IRolRepositorio
    {
        IList<IRol> GetRoles();
        IList<IRol> GetRoles(int skip, int make);
        IList<IRol> GetRoles(string nombre);
        IList<IRol> GetRoles(string nombre, int skip, int take);
        IRol GetRol(int id);
        IRol GetRol(string nombre);
        int AddRol(IRol rol);
        void UpdateRol(IRol rol);
        void DeleteRol(IRol rol);
        int GetRolesCount();
    }
}
