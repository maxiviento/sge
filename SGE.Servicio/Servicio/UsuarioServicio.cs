using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using EfficientlyLazy.Crypto;
using SGE.Model.Comun;
using SGE.Model.Entidades;
using SGE.Model.Entidades.Interfaces;
using SGE.Model.Repositorio;
using SGE.Repositorio;
using SGE.Servicio.ServicioInterfaces;
using SGE.Servicio.Vista;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces;
using System.Transactions;

namespace SGE.Servicio.Servicio
{
    public class UsuarioServicio : IUsuarioServicio
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly IUsuarioRolRepositorio _usuarioRolRepositorio;
        private readonly IAutenticacionServicio _aut;

        public UsuarioServicio()
        {
            _usuarioRepositorio = new UsuarioRepositorio();
            _aut = new AutenticacionServicio();
            _usuarioRolRepositorio = new UsuarioRolRepositorio();
        }

        public IUsuariosVista GetUsuarios()
        {
            IUsuariosVista vista = new UsuariosVista();

            var pager = new Pager(_usuarioRepositorio.GetUsuariosCount(),
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexUsuarios", _aut.GetUrl("IndexPager", "Usuarios"));

            vista.Pager = pager;

            vista.Usuarios = _usuarioRepositorio.GetUsuarios(pager.Skip, pager.PageSize);

            return vista;
        }

        public IUsuariosVista GetIndex()
        {
            IUsuariosVista vista = new UsuariosVista();

            var pager = new Pager(0,
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexUsuarios", _aut.GetUrl("IndexPager", "Usuarios"));

            vista.Pager = pager;

            return vista;
        }

        public IUsuariosVista GetUsuarios(string login, string nombre, string apellido)
        {
            IUsuariosVista vista = new UsuariosVista();

            var pager = new Pager(_usuarioRepositorio.GetUsuarios(login, nombre, apellido).Count(),
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexUsuarios", _aut.GetUrl("IndexPager", "Usuarios"));

            vista.Pager = pager;

            vista.Usuarios = _usuarioRepositorio.GetUsuarios(login, nombre, apellido, pager.Skip, pager.PageSize);

            return vista;
        }

        public IUsuariosVista GetUsuarios(IPager pPager, string login, string nombre, string apellido)
        {
            IUsuariosVista vista = new UsuariosVista();

            var pager = new Pager(_usuarioRepositorio.GetUsuarios(login, nombre, apellido).Count(), 
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexUsuarios", _aut.GetUrl("IndexPager", "Usuarios"));

            if (pager.TotalCount != pPager.TotalCount)
            {
                pager.PageNumber = 1;
                pager.Skip = 0;
                vista.Pager = pager;
            }
            else
            {
                vista.Pager = pPager;
            }

            vista.Usuarios = _usuarioRepositorio.GetUsuarios(login, nombre, apellido, vista.Pager.Skip, vista.Pager.PageSize);

            return vista;
        }

        public IUsuarioVista GetUsuario(int idUsuario, string accion)
        {
            IUsuarioVista vista = new UsuarioVista {Accion = accion};

            // Obtener usuario
            // ---------------------------------------------------------------
            if (idUsuario > 0)
            {
                var objreturn = _usuarioRepositorio.GetUsuario(idUsuario);

                vista.Id = objreturn.Id_Usuario;
                vista.Login = objreturn.Login_Usuario;
                vista.Apellido = objreturn.Apellido_Usuario;
                vista.Nombre = objreturn.Nombre_Usuario;
                vista.Password = String.Empty;
                vista.ConfirmPassword = String.Empty;
                vista.IdEstado = objreturn.Id_Estado_Usuario;
            }
            else
            {
                vista.IdEstado = (int)Enums.EstadoRegistro.Activo;
            }

            // Obtener los roles
            // ---------------------------------------------------------------
            IRolRepositorio rolRepositorio = new RolRepositorio();
            var objRoles = rolRepositorio.GetRoles();

            vista.Roles = new List<UsuarioRolVista>();

            foreach (var objRole in objRoles)
            {
                var usrRolVista = new UsuarioRolVista
                                      {
                                          IdRol = objRole.Id_Rol,
                                          NombreRol = objRole.Nombre_Rol,
                                          Selected = false
                                      };

                vista.Roles.Add(usrRolVista);
            }

            // Obtener los roles asignados
            // ---------------------------------------------------------------
            if (idUsuario > 0)
            {
                var objRolesAsignados = _usuarioRolRepositorio.GetRolesDelUsuario(idUsuario);

                foreach (var usuarioRolVista in from objRolesAsignado in objRolesAsignados
                                                from usuarioRolVista in vista.Roles
                                                where usuarioRolVista.IdRol == objRolesAsignado.IdRol
                                                select usuarioRolVista)
                {
                    usuarioRolVista.Selected = true;
                }
            }

            return vista;
        }

        public int AddUsuario(IUsuarioVista vista)
        {
            // Define a transaction scope for the operations.
            using (var transaction = new TransactionScope())
            {
                // Encriptar pass
                vista.Password = DataHashing.Compute(Algorithm.SHA1, vista.Password);

                // Validar login
                // ---------------------------------------------------
                var usr = _usuarioRepositorio.GetUsuario(vista.Login);
                if (usr != null)
                {
                    return 1;
                }

                // Insertar el Usuario
                // ---------------------------------------------------
                vista.Id = _usuarioRepositorio.AddUsuario(VistaToDatos(vista));

                // Insertar Roles del Usuario
                // ---------------------------------------------------
                AddUsuarioRol(vista);

                // Completa la transaccion
                transaction.Complete();

                return 0;
            }
        }

        public bool UpdateUsuario(IUsuarioVista vista)
        {
             // Define a transaction scope for the operations.
            using (var transaction = new TransactionScope())
            {
                // Encriptar pass
                if (!String.IsNullOrEmpty(vista.Password))
                {
                    vista.Password = DataHashing.Compute(Algorithm.SHA1, vista.Password);
                }

                // Actualizar Usuario
                // ---------------------------------------------------
                _usuarioRepositorio.UpdateUsuario(VistaToDatos(vista));

                // Actualizar Roles del Usuario
                // ---------------------------------------------------
                DeleteUsuarioRol(vista);
                AddUsuarioRol(vista);

                // Completa la transaccion
                transaction.Complete();

                return true;
            }
        }

        public bool DeleteUsuario(IUsuarioVista vista)
        {
            _usuarioRepositorio.DeleteUsuario(VistaToDatos(vista));

            return true;
        }

        public int Login(IUsuarioLoginVista vista, ref string userData)
        {
            IRolFormularioAccionRepositorio rolFormularioAccionRepositorio = new RolFormularioAccionRepositorio();

            // Valida si existe el nombre de usuario
            var usuario = _usuarioRepositorio.GetUsuario(vista.Login);
            if (usuario == null)
            {
                return (int)Enums.Autentificacion.UsuarioInexistente;
            }

            // Valida si la contraseña es valida para ese usuario
            vista.Password = DataHashing.Compute(Algorithm.SHA1, vista.Password);
            if (!usuario.Pasword_Usuario.Equals(vista.Password))
            {
                return (int)Enums.Autentificacion.ContraseñaInvalida;
            }

            userData = usuario.Id_Usuario + "|";

            var apellido = usuario.Apellido_Usuario.Length > 8
                               ? usuario.Apellido_Usuario.Substring(0, 8)
                               : usuario.Apellido_Usuario;

            var nombre = usuario.Nombre_Usuario.Length > 8
                             ? usuario.Nombre_Usuario.Substring(0, 8)
                             : usuario.Nombre_Usuario;

            userData += nombre + " " + apellido + "|";

            // Obtiene los permisos
            var usuarioRoles = _usuarioRolRepositorio.GetRolesDelUsuario(usuario.Id_Usuario);
            var usrFormularios = rolFormularioAccionRepositorio.GetFormulariosDelUsuario(usuario.Id_Usuario);
            IList<string> ocurreciasFormAcc = new List<string>();

            var urc = 0;

            foreach (var usuarioRol in usuarioRoles)
            {
                urc++;

                userData += usuarioRol.IdRol + ((!urc.Equals(usuarioRoles.Count)) ? "," : "");
            }

            userData += "|";

            var rfac = 0;

            foreach (var rolFormularioAccion in usrFormularios)
            {
                rfac++;
                
                if (ocurreciasFormAcc.Contains(rolFormularioAccion.Id_Formulario + "," + rolFormularioAccion.Id_Accion))
                {
                    continue;
                }

                userData += rolFormularioAccion.Id_Formulario + "," + rolFormularioAccion.Id_Accion + ((!rfac.Equals(usrFormularios.Count)) ? "#" : "");

                ocurreciasFormAcc.Add(rolFormularioAccion.Id_Formulario + "," + rolFormularioAccion.Id_Accion);
            }

            // Retorna ok
            return (int)Enums.Autentificacion.Autenticado;
        }

        public int UpdatePassword(IUsuarioPasswordVista vista)
        {
            // Obtiene el usuario
            var usuario = _usuarioRepositorio.GetUsuario(vista.Login);

            vista.OldPassword = DataHashing.Compute(Algorithm.SHA1, vista.OldPassword);
            if (!usuario.Pasword_Usuario.Equals(vista.OldPassword))
            {
                return 1;
            }

            vista.NewPassword = DataHashing.Compute(Algorithm.SHA1, vista.NewPassword);
            usuario.Pasword_Usuario = vista.NewPassword;
                
            _usuarioRepositorio.UpdateUsuario(usuario);
              
            return 0;
        }

        #region Privates
        private static IUsuario VistaToDatos(IUsuarioVista vista)
        {
            IUsuario usr = new Usuario
                               {
                                   Id_Usuario = vista.Id,
                                   Login_Usuario = vista.Login,
                                   Apellido_Usuario = vista.Apellido,
                                   Nombre_Usuario = vista.Nombre,
                                   Pasword_Usuario = vista.Password,
                                   Id_Estado_Usuario = vista.IdEstado
                               };
            return usr;
        }

        private void AddUsuarioRol(IUsuarioVista vista)
        {
            foreach (var usrRol in from usuarioRolVista in vista.Roles
                                   where usuarioRolVista.Selected
                                   select new UsuarioRol {IdUsuario = vista.Id, IdRol = usuarioRolVista.IdRol})
            {
                _usuarioRolRepositorio.AddUsuarioRol(usrRol);
            }
        }

        private void DeleteUsuarioRol(IUsuarioVista vista)
        {
            var objRolesAsignados = _usuarioRolRepositorio.GetRolesDelUsuario(vista.Id);

            foreach (var usrRol in objRolesAsignados)
            {
                _usuarioRolRepositorio.DeleteUsuarioRol(usrRol);
            }
        }
        #endregion
    }
}
