using System;
using System.Web.Mvc;
using SGE.Model.Comun;

namespace SGE.Web.Infrastucture
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        public Enums.Formulario Formulario { get; set; }
        public Enums.Acciones Accion { get; set; }

        /// <summary>
        /// OnAuthorization
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            // Comprobar si el usuario esta autenticado (OK lo hace principal)
            var principal = filterContext.HttpContext.User as CustomPrincipal;
            if (principal == null)
            {
                return;
            }

            var idFormulario = (byte)Formulario;
            var idAccion = (byte)Accion;

            // Si la accion = 0 la validacion se hace por formularios (Controller)
            if (idAccion == 0)
            {
                // Validar en la DB si tiene los derechos establecidos en los atributos solicitados
                if (!principal.IsInForms(idFormulario.ToString()))
                {
                    Authentication.UnAuthenticate(filterContext.HttpContext.Response);
                    filterContext.Result = new HttpUnauthorizedResult();
                }
            }
            else
            {
                // Validar en la DB si tiene los derechos establecidos en los atributos solicitados
                if (!principal.IsInPermisos(idFormulario.ToString(), idAccion.ToString()))
                {
                    Authentication.UnAuthenticate(filterContext.HttpContext.Response);
                    filterContext.Result = new HttpUnauthorizedResult();
                }
            }
        }
    }

    [Flags]
    public enum Formularios
    {
        Usuario = 1,
        Beneficiario = 2,
        Ficha = 3,
        Liquidacion = 4,
        CausaRechazo = 5,
        CambiarPassword = 6,
        AsignarPermisos = 7,
        Conceptos = 8,
        Roles = 9,
        TipoRechazo = 10,
        Programa = 11,
        Institucion = 12,
        Departamento = 13,
        Empresa = 14,
        Universidad = 15,
        Localidad = 16,
        Sede = 17,
        Carrera = 18,
        SucursalCobertura = 19,
        Apoderado = 20,
        InicializarTablas=21,
        FichaPpp=22,
        FichaObservacion=23,
        SubProgramas=24,
        Etapa = 128,
        Persona = 131,
        Cursos = 132,
        Barrios = 133,
        Escuelas = 134,
        Ongs = 135,
    }

    [Flags]
    public enum Acciones
    {
        Agregar = 1,
        Modificar = 2,
        Eliminar = 3,
        Consultar = 4,
        CambiarEstado = 5,
    }
}