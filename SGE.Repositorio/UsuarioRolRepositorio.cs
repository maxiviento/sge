using System.Collections.Generic;
using System.Linq;
using SGE.Model.Entidades;
using SGE.Model.Entidades.Interfaces;
using SGE.Model.Repositorio;
using SGE.Repositorio.Modelo;

namespace SGE.Repositorio
{
    public class UsuarioRolRepositorio : BaseRepositorio, IUsuarioRolRepositorio
    {
        private readonly DataSGE _mdb;

        public UsuarioRolRepositorio()
        {
            _mdb = new DataSGE();
        }

        public IList<IUsuarioRol> GetRolesDelUsuario(int idUsr)
        {
            var usuarioroles =
                _mdb.T_USUARIO_ROL.Where(c => c.ID_USUARIO == idUsr).Select(
                    item =>
                    new UsuarioRol
                        {
                            IdUsuario = idUsr,
                            IdRol = item.ID_ROL,
                            IdUsuarioSistema = item.ID_USR_SIST,
                            FechaSistema = item.FEC_SIST,
                            UsuarioSistema = item.T_USUARIOS.LOGIN
                        }).ToList().Cast<IUsuarioRol>().ToList();

            return usuarioroles;
        }

        public IList<IUsuarioRol> GetUsuariosDelRol(int idRol)
        {
            return
                _mdb.T_USUARIO_ROL.Where(c => c.ID_ROL == idRol).Select(
                    item => new UsuarioRol { IdUsuario = item.ID_USUARIO, IdRol = idRol }).Cast<IUsuarioRol>().ToList();
        }

        public void AddUsuarioRol(IUsuarioRol usrRol)
        {
            AgregarDatos(usrRol);

            var objusurol = new T_USUARIO_ROL
                                {
                                    ID_ROL = usrRol.IdRol,
                                    ID_USUARIO = usrRol.IdUsuario,
                                    ID_USR_SIST = usrRol.IdUsuarioSistema
                                };

            _mdb.T_USUARIO_ROL.AddObject(objusurol);

            _mdb.SaveChanges();
        }

        public void DeleteUsuarioRol(IUsuarioRol usrRol)
        {

            AgregarDatos(usrRol);

            var usuariorol =
                _mdb.T_USUARIO_ROL.FirstOrDefault(c => c.ID_USUARIO == usrRol.IdUsuario && c.ID_ROL == usrRol.IdRol);

            if (usuariorol == null) return;

            usuariorol.ID_USR_SIST = usrRol.IdUsuarioSistema;

            _mdb.SaveChanges();

            _mdb.DeleteObject(usuariorol);

            _mdb.SaveChanges();
        }
    }
}
