using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Repositorio
{
    public interface IUsuarioRepositorio
    {
        IList<IUsuario> GetUsuarios();
        IList<IUsuario> GetUsuarios(int skip, int make);
        IList<IUsuario> GetUsuarios(string login, string nombre, string apellido);
        IList<IUsuario> GetUsuarios(string login, string nombre, string apellido, int skip, int take);
        IUsuario GetUsuario(string login);
        IUsuario GetUsuario(int id);
        int AddUsuario(IUsuario usr);
        void UpdateUsuario(IUsuario usr);
        void DeleteUsuario(IUsuario usr);
        int GetUsuariosCount();
    }
}
