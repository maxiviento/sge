using System;
using System.Collections.Generic;
using System.Linq;
using SGE.Model.Comun;
using SGE.Model.Entidades;
using SGE.Model.Entidades.Interfaces;
using SGE.Model.Repositorio;
using SGE.Repositorio.Modelo;

namespace SGE.Repositorio
{
    public class RolRepositorio :BaseRepositorio, IRolRepositorio
    {
         private readonly DataSGE _mdb;

         public RolRepositorio()
        {
            _mdb = new DataSGE();
        }

        private IQueryable<IRol> QRol()
        {
            var a = (from c in _mdb.T_ROLES
                     where c.ID_ROL > 1 && c.ID_ESTADO == (int)Enums.EstadoRegistro.Activo
                     select
                         new Rol 
                         {
                             Id_Rol = c.ID_ROL,
                             Nombre_Rol = c.N_ROL,
                             IdUsuarioSistema = c.ID_USR_SIST,
                             FechaSistema = c.FEC_SIST,
                             UsuarioSistema = c.T_USUARIOS.LOGIN
                         });
            return a;
        }

        private IQueryable<IRol> QRolAll()
        {
            var a = (from c in _mdb.T_ROLES
                     where c.ID_ESTADO == (int)Enums.EstadoRegistro.Activo
                     select
                         new Rol
                         {
                             Id_Rol = c.ID_ROL,
                             Nombre_Rol = c.N_ROL,
                             IdUsuarioSistema = c.ID_USR_SIST,
                             FechaSistema = c.FEC_SIST,
                             UsuarioSistema = c.T_USUARIOS.LOGIN
                         });
            return a;
        }

        public IList<IRol> GetRoles()
        {
            return QRol().ToList();
        }

        public IList<IRol> GetRoles(int skip, int make)
        {
            return QRol().OrderBy(c => c.Nombre_Rol.ToLower()).Skip(skip).Take(make).ToList();
        }

        public IList<IRol> GetRoles(string nombre)
        {
            nombre = nombre ?? String.Empty;
            
            return
                QRol().Where(c => (c.Nombre_Rol.ToLower().Contains(nombre.ToLower().Trim()) || String.IsNullOrEmpty(nombre.Trim()))).
                OrderBy(c => c.Nombre_Rol.ToLower()).ToList();
        }

        public IList<IRol> GetRoles(string nombre, int skip, int take)
        {
            nombre = nombre ?? String.Empty;
            
            return
                QRol().Where(
                    c =>
                    (c.Nombre_Rol.ToLower().Contains(nombre.ToLower().Trim()) || String.IsNullOrEmpty(nombre.Trim()))).OrderBy(c => c.Nombre_Rol.ToLower())
                    .Skip(skip).Take(take)
                    .ToList();
        }

        public IRol GetRol(int id)
        {
            return QRol().Where(c => c.Id_Rol == id).SingleOrDefault();
        }

        public IRol GetRol(string nombre)
        {
            return QRolAll().Where(c => (c.Nombre_Rol.ToLower().Equals(nombre.ToLower()))).SingleOrDefault();
        }

        public int AddRol(IRol rol)
        {
            AgregarDatos(rol);

            var rolModel = new T_ROLES
                               {
                                   ID_ROL = SecuenciaRepositorio.GetId(),
                                   N_ROL = rol.Nombre_Rol.Trim(),
                                   ID_ESTADO = (short)rol.Id_Estado_Rol,
                                   ID_USR_SIST = rol.IdUsuarioSistema,
                                   FEC_SIST = rol.FechaSistema
                               };

            _mdb.T_ROLES.AddObject(rolModel);
            _mdb.SaveChanges();

            return rolModel.ID_ROL;
        }

        public void UpdateRol(IRol rol)
        {
            AgregarDatos(rol);
            if (rol.Id_Rol != 0)
            {
                var obj = _mdb.T_ROLES.SingleOrDefault(c => c.ID_ROL == rol.Id_Rol);

                if (obj != null)
                {
                    obj.N_ROL = rol.Nombre_Rol.Trim();
                    obj.ID_USR_SIST = rol.IdUsuarioSistema;
                    _mdb.SaveChanges();
                }
            }
        }

        void IRolRepositorio.DeleteRol(IRol rol)
        {
            AgregarDatos(rol);
            if (rol.Id_Rol != 0)
            {
                var obj = _mdb.T_ROLES.SingleOrDefault(c => c.ID_ROL == rol.Id_Rol);

                if (obj != null)
                {
                    obj.ID_ESTADO = (int)Enums.EstadoRegistro.Eliminado;
                    obj.ID_USR_SIST = rol.IdUsuarioSistema;
                    _mdb.SaveChanges();
                }
            }
        }

        public int GetRolesCount()
        {
            return QRol().Count();
        }
    }
}
