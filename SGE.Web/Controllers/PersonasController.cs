using System;
using System.Web.Mvc;
using System.Web.Routing;
using SGE.Model.Comun;
using SGE.Servicio.Servicio;
using SGE.Servicio.ServicioInterfaces;
using SGE.Servicio.Vista;
using SGE.Servicio.VistaInterfaces;
using SGE.Web.Controllers.Shared;
using SGE.Web.Infrastucture;
using SGE.Servicio.Vista.Shared;
using SGE.Model.Entidades;
using SGE.Model.Entidades.Interfaces;
namespace SGE.Web.Controllers
{
    [CustomAuthorizeAttribute(Formulario = Enums.Formulario.ABMPersonas)]
    public class PersonasController : BaseController
    {
        private readonly IPersonaServicio _personaServicio;
        private readonly ILocalidadServicio _localidadservicio;

        public PersonasController(): base((int)Formularios.Persona)
        {
            _personaServicio = new PersonaServicio();
            _localidadservicio = new LocalidadServicio();

        }
        //
        // GET: /Persona/

        public ActionResult Index()
        {
            var vista = _personaServicio.GetIndex();
            vista.DescripcionBusca = "";
            return View(vista);
        }

        [HttpPost]
        public ActionResult BuscarPersona(PersonasVista model)
        {
            return View("Index", _personaServicio.GetPersonas(model.DescripcionBusca));
        }


        [HttpPost]
        public ActionResult CargarLocalidades(int idDep)
        {
            var localidad = _personaServicio.cboCargarLocalidad(Convert.ToInt32(idDep))
                            ;

            return Json(localidad);
        }

        [HttpPost]
        public ActionResult BuscarBarrios(int idlocalidad)
        {
            var barrios = _personaServicio.cboCargarBarrio(Convert.ToInt32(idlocalidad))
                            ;

            return Json(barrios);
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.ABMPersonas, Accion = Enums.Acciones.Consultar)]
        public ActionResult Ver(int idPersona)
        {
            IPersonaVista vista = _personaServicio.GetPersona(idPersona, "Ver");
            return View("Formulario", vista);
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.ABMPersonas, Accion = Enums.Acciones.Agregar)]
        public ActionResult Agregar()
        {
            IPersonaVista vista = _personaServicio.GetPersona(0, "Agregar");
            return View("Formulario", vista);
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.ABMPersonas, Accion = Enums.Acciones.Modificar)]
        public ActionResult Modificar(int idPersona)
        {
            IPersonaVista vista = _personaServicio.GetPersona(idPersona, "Modificar");
            return View("Formulario", vista);
        }

        public ActionResult IndexPager(Pager pager, PersonasVista model)
        {
            
            IPersonasVista vista = _personaServicio.GetPersonas(pager, model.Id, model.DescripcionBusca);
            vista.DescripcionBusca = model.DescripcionBusca;
            TempData["Model"] = vista;
            return RedirectToAction("PageRedirect");
        }

        public ActionResult PageRedirect()
        {
            return View("Index", TempData["Model"]);
        }

        [HttpPost]
        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.ABMPersonas, Accion = Enums.Acciones.Modificar)]
        public ActionResult ModificarPersona(PersonaVista model)
        {
            switch (_personaServicio.UpdatePersona(model))
            {
                case 0:
                    var vis = _personaServicio.GetPersonas();
                    vis.DescripcionBusca = "";
                    return View("Index", vis);
                case 1:
                    TempData["MensajeError"] = "Ya existe una persona con ese DNI";
                    return View("Formulario", model);
                case 2:
                    TempData["MensajeError"] = "Seleccione una opción";
                    return View("Formulario", model);
                default:
                    //Muestra listado de error;
                    return View("Formulario", model);
            }
        }


        [HttpPost]
        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.ABMPersonas, Accion = Enums.Acciones.Agregar)]
        public ActionResult AgregarPersona(PersonaVista model)
        {
            PersonaVista modelo = model;
            switch (_personaServicio.AddPersona(model))
            {
                case 0:
                    return View("Index", _personaServicio.GetPersonas());
                case 1:
                    TempData["MensajeError"] = "Ya existe una persona con ese DNI";
                    return View("Formulario", _personaServicio.GetPersonaReload(model));
                case 2:
                    TempData["MensajeError"] = "Seleccione una opción";
                    return View("Formulario", model);
                default:
                    //Muestra listado de error;
                    return View("Formulario", model);
            }
        }

        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.ABMPersonas, Accion = Enums.Acciones.Modificar)]
        public ActionResult ActorRol(int idPersona)
        {
            IPersonaVista vista = _personaServicio.GetPersonaActor(idPersona);
            return View("FormularioActor", vista);
        }


        [HttpPost]
        [CustomAuthorizeAttribute(Formulario = Enums.Formulario.ABMPersonas, Accion = Enums.Acciones.Agregar)]
        public ActionResult AgregarActorRol(PersonaVista model)
        {
            PersonaVista modelo = model;
            switch (_personaServicio.AddPersonaActor(model))
            {
                case 0:
                    return View("FormularioActor", _personaServicio.GetPersonaActorReload(model));
                case 1:
                    TempData["MensajeError"] = "Debe seleccionar un rol para cargar el actor";
                   // return RedirectToAction("ActorRol", _personaServicio.GetPersonaActorReload(model));
                    
                    return View("FormularioActor", _personaServicio.GetPersonaActorReload(model));
                case 2:
                    TempData["MensajeError"] = "Ya existe el rol de actor para esta persona";
                    return View("FormularioActor", _personaServicio.GetPersonaActorReload(model));
                case 3:
                    TempData["MensajeError"] = "Seleccione una opción";
                    return View("FormularioActor", model);
                default:
                    //Muestra listado de error;
                    return View("FormularioActor", model);
            }
        }


        //public ActionResult ActorRol(PersonaVista model)
        //{
        //    //PersonaVista vista = _personaServicio.GetPersonaActor(idPersona);
        //    return View("FormularioActor", model);
        //}

        public ActionResult Familia(int idFicha)
        {
            IGrupoFamiliarVista vista = new GrupoFamiliarVista();
            //GrupoFamiliar = _personaServicio.
            vista.idficha = idFicha;
            vista = _personaServicio.GetGrupoFamiliar(vista);

            return View("Familia", vista);
        }

        [HttpPost]
        public ActionResult GetDNI(string dni)
        {
            var persona = _personaServicio.GetPersonaDni(dni);
                            

            return Json(persona);
        }

        public ActionResult AddFamiliar(GrupoFamiliarVista model)
        {
            model.idVinculo = Convert.ToInt32(model.vinculos.Selected);
            _personaServicio.AddFamiliar(model);
            return RedirectToAction("Familia","Personas", new RouteValueDictionary(
                          new { idFicha = model.idficha }));
        }

        public ActionResult DeleteRelFam(int idFicha ,int idPersona)
        {

            _personaServicio.DelFamiliar(idFicha, idPersona);
            IGrupoFamiliarVista vista = new GrupoFamiliarVista();
            //GrupoFamiliar = _personaServicio.
            //vista.idficha = idFicha;
            //vista = _personaServicio.GetGrupoFamiliar(vista);
            return RedirectToAction("Familia", "Personas", new RouteValueDictionary(
                          new { idFicha = idFicha }));
        }

        [HttpPost]
        public ActionResult addPersona(string dni, string apellido, string nombre)
        {
            PersonaVista modelo = new PersonaVista();
            modelo.dni = dni;
            modelo.apellido = apellido;
            modelo.nombre = nombre;

            IPersona persona;
            persona = new Persona();
            switch (_personaServicio.AddPersona(modelo))
            {
                case 0:
                    persona = _personaServicio.GetPersonaDni(dni);
                    break;
                case 1:

                    persona.id_persona = 0;
                    break;
                case 2:

                    persona.id_persona = -2;
                    break;

            }

            return Json(persona.id_persona);


        }





    }
}
