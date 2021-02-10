using System.Collections.Generic;
using System.Linq;
using SGE.Model.Entidades;
using SGE.Model.Entidades.Interfaces;
using SGE.Model.Repositorio;
using SGE.Repositorio.Modelo;

namespace SGE.Repositorio
{
    public class RolFormularioAccionRepositorio : BaseRepositorio, IRolFormularioAccionRepositorio
    {
        private readonly DataSGE _mdb;

        public RolFormularioAccionRepositorio()
        {
            _mdb = new DataSGE();
        }

        private IQueryable<IRolFormularioAccion> QRolFormularioAccion()
        {
            var a = (from c in _mdb.T_ROL_FORMULARIO_ACCION
                     select
                         new RolFormularioAccion
                             {
                                 Id_Rol = c.ID_ROL,
                                 Id_Formulario = c.ID_FORMULARIO,
                                 Id_Accion = c.ID_ACCION,
                                 IdUsuarioSistema = c.ID_USR_SIST,
                                 FechaSistema = c.FEC_SIST,
                                 UsuarioSistema = c.T_USUARIOS.LOGIN
                             });
            return a;
        }

        public IList<IRolFormularioAccion> GetFormulariosDelRol(int idRol)
        {
            return QRolFormularioAccion().Where(c => (c.Id_Rol.Equals(idRol))).OrderBy(c => c.Id_Formulario).ToList();
        }

        public IList<IRolFormularioAccion> GetFormulariosDelUsuario(int idUsuario)
        {
            var a = (from rfa in _mdb.T_ROL_FORMULARIO_ACCION
                    join ur in _mdb.T_USUARIO_ROL on rfa.ID_ROL equals ur.ID_ROL
                    where ur.ID_USUARIO == idUsuario
                    select new RolFormularioAccion
                               {
                                   Id_Rol = rfa.ID_ROL,
                                   Id_Formulario = rfa.ID_FORMULARIO,
                                   Id_Accion = rfa.ID_ACCION,
                                   IdUsuarioSistema = rfa.ID_USR_SIST,
                                   FechaSistema = rfa.FEC_SIST,
                                   UsuarioSistema = rfa.T_USUARIOS.LOGIN
                               }).ToList().Cast<IRolFormularioAccion>().ToList();
            
            return a;
        }

        public IRolFormularioAccion GetRolFormularioAccion(int idRol, int idFormulario, int idAccion)
        {
            return QRolFormularioAccion().Where(c => (c.Id_Rol.Equals(idRol) && c.Id_Formulario.Equals(idFormulario) && c.Id_Accion.Equals(idAccion))).SingleOrDefault();
        }

        public void AddRolFormularioAccion(IRolFormularioAccion rolFormularioAccion)
        {
            AgregarDatos(rolFormularioAccion);

            var existe =
                _mdb.T_ROL_FORMULARIO_ACCION.FirstOrDefault(
                    c =>
                    c.ID_ROL == rolFormularioAccion.Id_Rol && c.ID_ACCION == rolFormularioAccion.Id_Accion &&
                    c.ID_FORMULARIO == rolFormularioAccion.Id_Formulario);

            if (existe != null) return;

            var obj = new T_ROL_FORMULARIO_ACCION
                               {
                                   ID_ROL = rolFormularioAccion.Id_Rol,
                                   ID_FORMULARIO = (short)rolFormularioAccion.Id_Formulario,
                                   ID_ACCION = (short)rolFormularioAccion.Id_Accion,
                                   ID_USR_SIST = rolFormularioAccion.IdUsuarioSistema,
                                   FEC_SIST = rolFormularioAccion.FechaSistema
                               };

            _mdb.T_ROL_FORMULARIO_ACCION.AddObject(obj);

            _mdb.SaveChanges();
        }

        public void DeleteRolFormulariosAcciones(IRolFormularioAccion rolFormularioAccion)
        {
            AgregarDatos(rolFormularioAccion);

            var rolformularioaccion =
                _mdb.T_ROL_FORMULARIO_ACCION.Where(
                    c => c.ID_ROL == rolFormularioAccion.Id_Rol).ToList();

            foreach (var formularioAccion in rolformularioaccion)
            {
                formularioAccion.ID_USR_SIST = rolFormularioAccion.IdUsuarioSistema;

                _mdb.SaveChanges();

                _mdb.DeleteObject(formularioAccion);

                _mdb.SaveChanges();
            }
        }
    }
}
