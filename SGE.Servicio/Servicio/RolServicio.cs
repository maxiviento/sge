using System;
using System.Linq;
using System.Configuration;
using SGE.Model.Comun;
using SGE.Model.Entidades;
using SGE.Model.Entidades.Interfaces;
using SGE.Model.Repositorio;
using SGE.Repositorio;
using SGE.Servicio.ServicioInterfaces;
using SGE.Servicio.Vista;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces;

namespace SGE.Servicio.Servicio
{
    public class RolServicio : IRolServicio
    {
        private readonly IRolRepositorio _rolRepositorio;
        private readonly IAutenticacionServicio _aut;

        public RolServicio()
        {
            _rolRepositorio = new RolRepositorio();
            _aut = new AutenticacionServicio();
        }

        public IRolesVista GetRoles()
        {
            IRolesVista vista = new RolesVista();

            var pager = new Pager(_rolRepositorio.GetRolesCount(),
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexRoles", _aut.GetUrl("IndexPager", "Roles"));

            vista.Pager = pager;

            vista.Roles = _rolRepositorio.GetRoles(pager.Skip, pager.PageSize);

            return vista;
        }

        public IRolesVista GetIndex()
        {
            IRolesVista vista = new RolesVista();

            var pager = new Pager(0,
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexRoles", _aut.GetUrl("IndexPager", "Roles"));

            vista.Pager = pager;


            return vista;
        }

        public IRolesVista GetRoles(string nombre)
        {
            IRolesVista vista = new RolesVista();

            var pager = new Pager(_rolRepositorio.GetRoles(nombre).Count(),
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexRoles", _aut.GetUrl("IndexPager", "Roles"));

            vista.Pager = pager;

            vista.Roles = _rolRepositorio.GetRoles(nombre, pager.Skip, pager.PageSize);

            return vista;
        }

        public IRolesVista GetRoles(IPager pPager, string nombre)
        {
            IRolesVista vista = new RolesVista();

            var pager = new Pager(_rolRepositorio.GetRoles(nombre).Count(),
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexRoles", _aut.GetUrl("IndexPager", "Roles"));

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

            vista.Roles = _rolRepositorio.GetRoles(nombre, vista.Pager.Skip, vista.Pager.PageSize);
            vista.NombreBusqueda = nombre ?? String.Empty;

            return vista;
        }

        public IRolVista GetRol(int idRol, string accion)
        {
            IRolVista vista = new RolVista {Accion = accion};

            if (idRol > 0)
            {
                IRol objreturn = _rolRepositorio.GetRol(idRol);
                vista.Id = objreturn.Id_Rol;
                vista.Nombre = objreturn.Nombre_Rol;
                vista.IdEstado = objreturn.Id_Estado_Rol;
            }
            else
            {
                vista.IdEstado = (int)Enums.EstadoRegistro.Activo;
            }

            return vista;
        }

        public int AddRol(IRolVista vista)
        {
           
                // Validar que no exita un rol con el mismo nombre
                // ---------------------------------------------------
                IRol rol = _rolRepositorio.GetRol(vista.Nombre);
                if (rol != null)
                {
                    return 1;
                }

                // Insertar el rol
                // ---------------------------------------------------
                vista.Id = _rolRepositorio.AddRol(VistaToDatos(vista));

                return 0;
           
        }

        public int UpdateRol(IRolVista vista)
        {
            
                // Validar que no exita un rol con el mismo nombre
                // ---------------------------------------------------
                IRol rol = _rolRepositorio.GetRol(vista.Nombre);
                if (rol != null && rol.Id_Rol != vista.Id)
                {
                    return 1;
                }

                // Actualizar Usuario
                // ---------------------------------------------------
                _rolRepositorio.UpdateRol(VistaToDatos(vista));

                return 0;
           
        }

        public int DeleteRol(IRolVista vista)
        {
          
                IRolFormularioAccionRepositorio rfaRepositorio = new RolFormularioAccionRepositorio();
                if (rfaRepositorio.GetFormulariosDelRol(vista.Id).Count > 0)
                {
                    return 1;
                }

                IUsuarioRolRepositorio usrRolRepositorio = new UsuarioRolRepositorio();
                if (usrRolRepositorio.GetUsuariosDelRol(vista.Id).Count > 0)
                {
                    return 2;
                }

                _rolRepositorio.DeleteRol(VistaToDatos(vista));

                return 0;
           
        }

        #region Privates

        private static IRol VistaToDatos(IRolVista vista)
        {
            IRol rol = new Rol
            {
                Id_Rol = vista.Id,
                Nombre_Rol = vista.Nombre,
                Id_Estado_Rol = vista.IdEstado
            };
            return rol;
        }

        #endregion
    }
}
