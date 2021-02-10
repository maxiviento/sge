using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Transactions;
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
    public class PermisoServicio : IPermisoServicio
    {
        private readonly IRolRepositorio _rolRepositorio;
        private readonly IFormularioRepositorio _formularioRepositorio;
        private readonly IFormularioAccionRepositorio _formularioAccionRepositorio;
        private readonly IRolFormularioAccionRepositorio _rolFormularioAccionRepositorio;
        private readonly IAutenticacionServicio _aut;
        
        public PermisoServicio()
        {
            _rolRepositorio = new RolRepositorio();
            _formularioRepositorio = new FormularioRepositorio();
            _formularioAccionRepositorio = new FormularioAccionRepositorio();
            _rolFormularioAccionRepositorio = new RolFormularioAccionRepositorio();
            _aut = new AutenticacionServicio();
        }

        public IPermisosVista GetRoles()
        {
            IPermisosVista vista = new PermisosVista();

            var pager = new Pager(_rolRepositorio.GetRolesCount(),
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexPermisos", _aut.GetUrl("IndexPager", "Permisos"));

            vista.Pager = pager;

            vista.Roles = _rolRepositorio.GetRoles(pager.Skip, pager.PageSize);

            return vista;
        }

        public IPermisosVista GetIndex()
        {
            IPermisosVista vista = new PermisosVista();

            var pager = new Pager(0,
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexPermisos", _aut.GetUrl("IndexPager", "Permisos"));

            vista.Pager = pager;

            return vista;
        }

        public IPermisosVista GetRoles(string nombre)
        {
            IPermisosVista vista = new PermisosVista();

            var pager = new Pager(_rolRepositorio.GetRoles(nombre).Count(),
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexPermisos", _aut.GetUrl("IndexPager", "Permisos"));

            vista.Pager = pager;

            vista.Roles = _rolRepositorio.GetRoles(nombre, pager.Skip, pager.PageSize);

            return vista;
        }

        public IPermisosVista GetRoles(IPager pPager, string nombre)
        {
            IPermisosVista vista = new PermisosVista();

            var pager = new Pager(_rolRepositorio.GetRoles(nombre).Count(),
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexPermisos", _aut.GetUrl("IndexPager", "Permisos"));

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

            return vista;
        }

        public IPermisoVista GetPermiso(int idRol, string accion)
        {
            IPermisoVista vista = new PermisoVista {Id = idRol};
            
            // Obtiene los datos del rol
            var objreturn = _rolRepositorio.GetRol(idRol);
            vista.Rol = objreturn.Nombre_Rol;

            // Obtiene el resto de los datos
            vista.Accion = accion;
            vista.Formularios = new List<FormularioVista>();

            // Obtiene los formulario y las acciones
            IList<IFormulario> objFormularios = _formularioRepositorio.GetFormularios();
            foreach (var objFormulario in objFormularios)
            {
                var iFormularioVista = new FormularioVista
                                           {
                                               Id = objFormulario.Id_Formulario,
                                               Formulario = objFormulario.Nombre_Formulario,
                                               Acciones = new List<AccionVista>()
                                           };

                var objAcciones = _formularioAccionRepositorio.GetAccionesDelFormulario(objFormulario.Id_Formulario);
                foreach (var formularioAccion in objAcciones)
                {
                    var iAccionVista = new AccionVista
                                           {
                                               Id = formularioAccion.Id_Accion,
                                               Accion = formularioAccion.Nombre_Accion,
                                               Selected = false
                                           };

                    if (_rolFormularioAccionRepositorio.GetRolFormularioAccion(idRol, objFormulario.Id_Formulario, formularioAccion.Id_Accion) != null)
                    {
                        iAccionVista.Selected = true;
                    }
                    
                    iFormularioVista.Acciones.Add(iAccionVista);
                }

                if (iFormularioVista.Acciones.Count == 0) continue;

                vista.Formularios.Add(iFormularioVista);
            }

            return vista;
        }

        public bool UpdatePermiso(IPermisoVista vista)
        {
            using (var transaction = new TransactionScope())
            {
                _rolFormularioAccionRepositorio.DeleteRolFormulariosAcciones(VistaToDatos(vista.Id, 0, 0));

                foreach (var formulario in vista.Formularios)
                {
                    foreach (var accion in formulario.Acciones.Where(accion => accion.Selected))
                    {
                        _rolFormularioAccionRepositorio.AddRolFormularioAccion(VistaToDatos(vista.Id, formulario.Id, accion.Id));
                    }
                }

                transaction.Complete();

                return true;
            }
        }

        #region Privates

        private static IRolFormularioAccion VistaToDatos(int idRol, int idFormulario, int idAccion)
        {
            IRolFormularioAccion rolFormAccion = new RolFormularioAccion
            {
                Id_Rol = idRol,
                Id_Formulario = idFormulario,
                Id_Accion = idAccion
            };

            return rolFormAccion;
        }

        #endregion
    }
}
