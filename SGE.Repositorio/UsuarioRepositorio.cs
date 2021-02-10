using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using SGE.Model.Comun;
using SGE.Model.Entidades;
using SGE.Model.Entidades.Interfaces;
using SGE.Model.Repositorio;
using SGE.Repositorio.Modelo;

namespace SGE.Repositorio
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly DataSGE _mdb;

        public UsuarioRepositorio()
        {
            _mdb = new DataSGE();
        }

        private IQueryable<IUsuario> QUsuario()
        {
            var a = (from c in _mdb.T_USUARIOS
                     where c.ID_USUARIO > 1 && c.ID_ESTADO == (int)Enums.EstadoRegistro.Activo
                     select
                         new Usuario
                         {
                             Id_Usuario = c.ID_USUARIO,
                             Nombre_Usuario = c.NOMBRE,
                             Apellido_Usuario = c.APELLIDO,
                             Login_Usuario = c.LOGIN,
                             Pasword_Usuario = c.HASHED_PASS,
                             Id_Estado_Usuario = c.ID_ESTADO,
                            IdUsuarioSistema = c.ID_USR_SIST,
                            FechaSistema = c.FEC_SIST,
                            UsuarioSistema = c.T_USUARIOS2.LOGIN
                         });
            return a;
        }

        private IQueryable<IUsuario> QUsuarioAll()
        {
            var a = (from c in _mdb.T_USUARIOS
                     where c.ID_ESTADO == (int)Enums.EstadoRegistro.Activo
                     select
                         new Usuario
                         {
                             Id_Usuario = c.ID_USUARIO,
                             Nombre_Usuario = c.NOMBRE,
                             Apellido_Usuario = c.APELLIDO,
                             Login_Usuario = c.LOGIN,
                             Pasword_Usuario = c.HASHED_PASS,
                             Id_Estado_Usuario = c.ID_ESTADO,
                             IdUsuarioSistema = c.ID_USR_SIST,
                             FechaSistema = c.FEC_SIST,
                             UsuarioSistema = c.T_USUARIOS2.LOGIN
                             
                         });
            return a;
        }

        public IList<IUsuario> GetUsuarios()
        {
            return QUsuario().ToList();
        }

        public IList<IUsuario> GetUsuarios(int skip, int make)
        {
            return QUsuario().OrderBy(c => c.Apellido_Usuario.ToLower()).Skip(skip).Take(make).ToList();
        }

        public IList<IUsuario> GetUsuarios(string login, string nombre, string apellido)
        {
            login = login ?? String.Empty;
            nombre = nombre ?? String.Empty;
            apellido = apellido ?? String.Empty;

            return
                QUsuario().Where(
                    c =>
                    (c.Login_Usuario.ToLower().Contains(login.ToLower().Trim()) || String.IsNullOrEmpty(login)) &&
                    (c.Nombre_Usuario.ToLower().Contains(nombre.ToLower().Trim()) || String.IsNullOrEmpty(nombre)) &&
                    (c.Apellido_Usuario.ToLower().Contains(apellido.ToLower().Trim()) || String.IsNullOrEmpty(apellido))).OrderBy(c => c.Apellido_Usuario.ToLower())
                    .ToList();

        }

        public IList<IUsuario> GetUsuarios(string login, string nombre, string apellido, int skip, int take)
        {
            login = login ?? String.Empty;
            nombre = nombre ?? String.Empty;
            apellido = apellido ?? String.Empty;


            return
                QUsuario().Where(
                    c =>
                    (c.Login_Usuario.ToLower().Contains(login.ToLower()) || String.IsNullOrEmpty(login)) &&
                    (c.Nombre_Usuario.ToLower().Contains(nombre.ToLower()) || String.IsNullOrEmpty(nombre)) &&
                    (c.Apellido_Usuario.ToLower().Contains(apellido.ToLower()) || String.IsNullOrEmpty(apellido))).OrderBy(c => c.Apellido_Usuario.ToLower())
                    .Skip(skip).Take(take)
                    .ToList();
        }

        public IUsuario GetUsuario(string login)
        {
            return QUsuarioAll().Where(c => (c.Login_Usuario.ToLower().Equals(login.ToLower()))).SingleOrDefault();
        }

        public IUsuario GetUsuario(int id)
        {
            return QUsuario().Where(c => c.Id_Usuario == id).SingleOrDefault();
        }

        public int AddUsuario(IUsuario usr)
        {
            usr.IdUsuarioSistema = GetUsuarioLoguer().Id_Usuario;
            
            var usrModel = new T_USUARIOS
                               {
                                   ID_USUARIO = SecuenciaRepositorio.GetId(),
                                   LOGIN = usr.Login_Usuario,
                                   APELLIDO = usr.Apellido_Usuario,
                                   NOMBRE = usr.Nombre_Usuario,
                                   HASHED_PASS = usr.Pasword_Usuario,
                                   ID_ESTADO = (short)usr.Id_Estado_Usuario,
                                   ID_USR_SIST = usr.IdUsuarioSistema
                               };

            _mdb.T_USUARIOS.AddObject(usrModel);
            _mdb.SaveChanges();

            return usrModel.ID_USUARIO;
        }

        public void UpdateUsuario(IUsuario usr)
        {
            
            usr.IdUsuarioSistema = GetUsuarioLoguer().Id_Usuario;
            
            if (usr.Id_Usuario == 0) return;
            
            var obj = _mdb.T_USUARIOS.SingleOrDefault(c => c.ID_USUARIO == usr.Id_Usuario);

            if (obj == null) return;
            
            obj.LOGIN = usr.Login_Usuario;
            obj.NOMBRE = usr.Nombre_Usuario;
            obj.APELLIDO = usr.Apellido_Usuario;
            obj.ID_USR_SIST = usr.IdUsuarioSistema;

            if (!String.IsNullOrEmpty(usr.Pasword_Usuario))
            {
                obj.HASHED_PASS = usr.Pasword_Usuario;
            }

            _mdb.SaveChanges();
        }

        public void DeleteUsuario(IUsuario usr)
        {
            usr.IdUsuarioSistema = GetUsuarioLoguer().Id_Usuario;
            
            if (usr.Id_Usuario == 0) return;
            
            var obj = _mdb.T_USUARIOS.SingleOrDefault(c => c.ID_USUARIO == usr.Id_Usuario);

            if (obj == null) return;
            
            obj.ID_ESTADO = (int)Enums.EstadoRegistro.Eliminado;
            obj.ID_USR_SIST = usr.IdUsuarioSistema;

            _mdb.SaveChanges();
        }

        public int GetUsuariosCount()
        {
            return QUsuario().Count();
        }

        public IUsuario GetUsuarioLoguer()
        {
            string cookieName = ConfigurationManager.AppSettings.Get("CookieName");
            var httpCookie = HttpContext.Current.Request.Cookies[cookieName];

            if (httpCookie == null || String.IsNullOrEmpty(httpCookie.Value))
            {
                return null;
            }

            var ticket = FormsAuthentication.Decrypt(httpCookie.Value);
            if (ticket == null || ticket.Expired)
            {
                return null;
            }
            var identity = new FormsIdentity(ticket);

            IUsuario usuario =
                QUsuarioAll().Where(c => (c.Login_Usuario.ToLower().Equals(identity.Name.ToLower()))).SingleOrDefault();

            return usuario;
        }
    }
}
