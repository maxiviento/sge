using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Repositorio
{
    public interface IUsuarioRolRepositorio
    {
        IList<IUsuarioRol> GetRolesDelUsuario(int idUsr);
        IList<IUsuarioRol> GetUsuariosDelRol(int idRol);
        void AddUsuarioRol(IUsuarioRol usrRol);
        void DeleteUsuarioRol(IUsuarioRol usrRol);
    }
}
