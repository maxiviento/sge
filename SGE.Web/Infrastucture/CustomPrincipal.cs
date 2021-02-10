using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

namespace SGE.Web.Infrastucture
{
    public class CustomPrincipal : IPrincipal
    {
        private readonly IIdentity _identity;
        private readonly string[] _roles;
        private readonly string[] _formularios;
        private readonly string[] _formulariosAcciones;
        private readonly string _nombreCompleto;
        private readonly int _idUsuario;

        public CustomPrincipal(IIdentity identity, IEnumerable<string> roles, IEnumerable<string> formularios, IEnumerable<string> formulariosAcciones, string nombreCompleto, int idUsuario)
        {
            _identity = identity;

            _roles = roles.ToArray();
            Array.Sort(_roles);

            _formularios = formularios.ToArray();
            Array.Sort(_formularios);

            _formulariosAcciones = formulariosAcciones.ToArray();
            Array.Sort(_formulariosAcciones);

            _nombreCompleto = nombreCompleto;

            _idUsuario = idUsuario;
        }

        public IIdentity Identity
        {
            get
            {
                return _identity;
            }
        }

        // IPrincipal Implementation
        public bool IsInRole(string role)
        {
            return Array.BinarySearch(_roles, role) > -1 ? true : false;
        }

        // Verifica si el existe el formulario en la lista de formularios
        public bool IsInForms(string idform)
        {
            return Array.BinarySearch(_formularios, idform) > -1 ? true : false;
        }

        // Verifica si existe el permiso para ese formulario, accion
        public bool IsInPermisos(string idform, string idaccion)
        {
            string fa = idform + "," + idaccion;

            if (Array.BinarySearch(_formulariosAcciones, fa) > -1)
            {
                return true;
            }

            return false;
        }

        public int GetIdUsuario()
        {
            return _idUsuario;
        }

        public string GetNombreCompleto()
        {
            return _nombreCompleto;
        }

        // Obtiene las acciones del formulario
        public string[] GetAcciones(string idform)
        {
            IList<string> actionList = new List<string>();

            foreach (string rolFormularioAccion in _formulariosAcciones)
            {
                var rfa = rolFormularioAccion.Split(',');
                if (rfa[0].Equals(idform))
                {
                    actionList.Add(rfa[1]);
                }
            }

            string[] actions = actionList.ToArray();
            Array.Sort(actions);

            return actions;
        }

        // retorna el array de Formularios
        public string[] GetForms()
        {
            return _formularios;
        }

        // Checks whether a principal is in all of the specified set of roles
        public bool IsInAllRoles(params string[] roles)
        {
            foreach (string searchrole in roles)
            {
                if (Array.BinarySearch(_roles, searchrole) < 0)
                    return false;
            }
            return true;
        }

        // Checks whether a principal is in any of the specified set of roles
        public bool IsInAnyRoles(params string[] roles)
        {
            foreach (string searchrole in roles)
            {
                if (Array.BinarySearch(_roles, searchrole) > 0)
                    return true;
            }

            return false;
        }


        public string[] GetRoles()
        {
            return _roles;
        }
    }
}