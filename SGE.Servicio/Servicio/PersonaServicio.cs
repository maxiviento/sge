using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
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
    public class PersonaServicio : IPersonaServicio
    {
        private readonly IPersonaRepositorio _personaRepositorio;
        private readonly IAutenticacionServicio _autenticacion;
        private readonly ILocalidadRepositorio _localidadRepositorio;
        private readonly IDepartamentoRepositorio _departamentoRepositorio;
        private readonly IActorRepositorio _actorRepositorio;
        private readonly IRelFamiliarRepositorio _relfamiliaresRepositorio;


        public PersonaServicio()
        {
            _personaRepositorio = new PersonaRepositorio();
            _autenticacion = new AutenticacionServicio();
            _localidadRepositorio = new LocalidadRepositorio();
            _departamentoRepositorio = new DepartamentoRepositorio();
            _actorRepositorio = new ActorRepositorio();
            _relfamiliaresRepositorio = new RelFamiliarRepositorio();
        }

        public IPersonasVista GetPersonas()
        {
            IPersonasVista vista = new PersonasVista();

            var pager = new Pager(_personaRepositorio.GetPersonasCount(),
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexPersona", _autenticacion.GetUrl("IndexPager", "Personas"));
            
            
            vista.Pager = pager;
            vista.personas = _personaRepositorio.GetPersonas(pager.Skip, pager.PageSize);

            return vista;
        }

        public IPersonasVista GetIndex()
        {
            IPersonasVista vista = new PersonasVista();

            var pager = new Pager(0,
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexPersona", _autenticacion.GetUrl("IndexPager", "Personas"));

            vista.Pager = pager;
            

            return vista;
        }

        public IPersonasVista GetPersonas(string descripcion)
        {
            IPersonasVista vista = new PersonasVista();

            var pager = new Pager(_personaRepositorio.GetPersonas(descripcion).Count(),
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexPersona", _autenticacion.GetUrl("IndexPager", "Personas"));

            vista.Pager = pager;
            vista.personas = _personaRepositorio.GetPersonas(descripcion, pager.Skip, pager.PageSize);

            return vista;
        }

        public IPersonaVista GetPersona(int idPersona, string accion)
        {
            IPersonaVista vista = new PersonaVista();
            IPersona persona;
            vista.Accion = accion;
            CargarDepartamento(vista, _departamentoRepositorio.GetDepartamentos());



            if (accion != "Agregar")
            {
                persona = _personaRepositorio.GetPersona(idPersona);

                                vista.id_persona = persona.id_persona;
                                vista.nombre = persona.nombre;
                                vista.apellido = persona.apellido;
                                vista.dni =persona.dni;
                                vista.cuil =persona.cuil;
                                vista.celular =persona.celular;
                                vista.telefono =persona.telefono;
                                vista.mail =persona.mail;
                                vista.id_barrio =persona.id_barrio;
                                vista.id_localidad =persona.id_localidad;
                                vista.empleado =persona.empleado;
                                vista.id_usr_sist =persona.id_usr_sist;
                                vista.fec_sist =persona.fec_sist;
                                vista.id_usr_modif =persona.id_usr_modif;
                                vista.fec_modif = persona.fec_modif;
                //cargar localidad
                //cargar departamento
                //
                                
                                int idlocalidad = Convert.ToInt32(persona.id_localidad ?? 0);
                                var d = _localidadRepositorio.GetLocalidad(idlocalidad);
                                string idDep = d.IdDepartamento == null ? "0" : d.IdDepartamento.ToString();
                                vista.departamentos.Selected = idDep;
                                CargarLocalidad(vista, _localidadRepositorio.GetLocalidadesByDepto(idDep == null ? 0 : Convert.ToInt32(idDep)));
                                vista.Localidades.Selected = persona.id_localidad.ToString();
                                CargarBarrio(vista, (from b in _localidadRepositorio.GetBarrios().Where(x => x.ID_LOCALIDAD == idlocalidad).DefaultIfEmpty()
                                                     select b).ToList());                
                vista.barrios.Selected = persona.id_barrio.ToString();

                var actorPersona = (from a in _actorRepositorio.GetActorPersona(vista.id_persona)
                                    select new ActorVista
                                    {
                                        ID_ACTOR = a.ID_ACTOR,
                                        ID_ACTOR_ROL = a.ID_ACTOR_ROL,
                                        N_ACTOR_ROL = a.N_ACTOR_ROL,

                                    }).ToList();

                cargarActorRol(vista, actorPersona);

                //if (accion != "ModificarActorRol")
                //{
                // CargarActoresRol(vista, _personaRepositorio.GetActoresRol());
                //}
            }
            else
            {
                persona = new Persona();

                vista.nombre = persona.nombre;
                vista.id_localidad = 0;
                vista.id_barrio = 0;
                vista.departamentos.Selected = "0";
                
                CargarLocalidad(vista, _localidadRepositorio.GetLocalidades());
                CargarBarrio(vista, _localidadRepositorio.GetBarrios());
                
            }

            

            if (accion == "Ver" || accion=="Eliminar")
            {
                vista.Localidades.Enabled = false;
                vista.barrios.Enabled = false;
                vista.departamentos.Enabled = false;
            }

            return vista;
        }

        public IPersonasVista GetPersonas(Pager pPager, int id, string descripcion)
        {
            IPersonasVista vista = new PersonasVista();

            var pager = new Pager(_personaRepositorio.GetPersonas(descripcion).Count(),
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexPersona", _autenticacion.GetUrl("IndexPager", "Personas"));

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

            vista.personas = _personaRepositorio.GetPersonas(descripcion, vista.Pager.Skip, vista.Pager.PageSize);
            vista.DescripcionBusca = descripcion;

            return vista;
        }

        public int AddPersona(IPersonaVista vista)
        {

            // ya existe
            if (_personaRepositorio.GetPersonas(vista.dni).Count() > 0)
            {
                return 1;
            }

            vista.id_persona = _personaRepositorio.AddPersona(VistaToDatos(vista));

            return 0;
        }

        private static IPersona VistaToDatos(IPersonaVista vista)
        {
            IPersona persona = new Persona
                {
                    id_persona = vista.id_persona,
                    nombre = vista.nombre,
                    apellido = vista.apellido,
                    dni = vista.dni,
                    cuil = vista.cuil,
                    celular = vista.celular,
                    telefono = vista.telefono,
                    mail = vista.mail,
                    id_barrio = vista.barrios.Selected == "0" ? 0 : Convert.ToInt32(vista.barrios.Selected),
                    id_localidad = vista.Localidades.Selected == "0" ? 0 : Convert.ToInt32(vista.Localidades.Selected),
                    empleado = vista.empleado,

                };

            return persona;
        }

        public int UpdatePersona(IPersonaVista vista)
        {
            //// Combos sin seleccionar
            //if (vista.NombreInstitucion.Selected == "0" || vista.NombreNivel.Selected == "0" || vista.NombreSector.Selected == "0")
            //{
            //    CargarInstituciones(vista, _institucionRepositorio.GetInstituciones());
            //    CargarNiveles(vista, _nivelRepositorio.GetNiveles());
            //    CargarSectores(vista, _sectorInstitucionRepositorio.GetSectoresInstitucionByInstitucion(Convert.ToInt32(vista.NombreInstitucion.Selected)));
            //    return 2;
            //}
            //if (_personaRepositorio.GetPersonas(vista.dni).Count() > 0)
            //{
                //CargarInstituciones(vista, _institucionRepositorio.GetInstituciones());
                //CargarNiveles(vista, _nivelRepositorio.GetNiveles());
                //CargarSectores(vista, _sectorInstitucionRepositorio.GetSectoresInstitucionByInstitucion(Convert.ToInt32(vista.NombreInstitucion.Selected)));
            //    return 1;
            //}
            
            //    // Actualizar Carrera
                _personaRepositorio.UpdatePersona(VistaToDatos(vista));

                return 0;
           
        }


        private static void CargarDepartamento(IPersonaVista vista, IEnumerable<IDepartamento> listadepartamentos)
        {
            foreach (var listadepartamento in listadepartamentos)
            {
                vista.departamentos.Combo.Add(new ComboItem { Id = listadepartamento.IdDepartamento, Description = listadepartamento.NombreDepartamento });

            }
        }

        private static void CargarLocalidad(IPersonaVista vista, IList<ILocalidad> listalocalidades)
        {
            vista.Localidades.Combo.Add(new ComboItem { Id = 0, Description = "Seleccione Localidad..." });
            foreach (var listalocalidad in listalocalidades)
            {
                vista.Localidades.Combo.Add(new ComboItem { Id = listalocalidad.IdLocalidad, Description = listalocalidad.NombreLocalidad });

            }
        }

        private static void CargarBarrio(IPersonaVista vista, IList<IBarrio> listaBarrios)
        {
            vista.barrios.Combo.Add(new ComboItem { Id = 0, Description = "Seleccione Barrio..." });
            if (listaBarrios[0] != null)
            {
                foreach (var barrio in listaBarrios)
                {
                    vista.barrios.Combo.Add(new ComboItem { Id = barrio.ID_BARRIO, Description = barrio.N_BARRIO });

                }
            }
        }

        public List<ILocalidad> cboCargarLocalidad(int idDep)
        {

            return _localidadRepositorio.GetLocalidadesByDepto(idDep).ToList();
        }

        public List<IBarrio> cboCargarBarrio(int idLocalidad)
        {

            return (from b in _localidadRepositorio.GetBarrios().Where(x=>x.ID_LOCALIDAD == idLocalidad).DefaultIfEmpty()
                        select b).ToList();
        }

        public IPersonaVista GetPersonaReload(PersonaVista vista)
        {
            
            //IPersona persona;
            string accion = vista.Accion;
            string loc = vista.Localidades.Selected;
            string dd = vista.departamentos.Selected;
            string bb = vista.barrios.Selected;
            



            if (accion != "Agregar")
            {
                //persona = _personaRepositorio.GetPersona(vista.id_persona);

                //vista.id_persona = persona.id_persona;
                //vista.nombre = persona.nombre;
                //vista.apellido = persona.apellido;
                //vista.dni = persona.dni;
                //vista.cuil = persona.cuil;
                //vista.celular = persona.celular;
                //vista.telefono = persona.telefono;
                //vista.mail = persona.mail;
                //vista.id_barrio = persona.id_barrio;
                //vista.id_localidad = persona.id_localidad;
                //vista.empleado = persona.empleado;
                //vista.id_usr_sist = persona.id_usr_sist;
                //vista.fec_sist = persona.fec_sist;
                //vista.id_usr_modif = persona.id_usr_modif;
                //vista.fec_modif = persona.fec_modif;
                //cargar localidad
                //cargar departamento
                //
                CargarDepartamento(vista, _departamentoRepositorio.GetDepartamentos());
                int idlocalidad = loc==null ? 0 : Convert.ToInt32(loc);
                var d = _localidadRepositorio.GetLocalidad(idlocalidad);
                string idDep = d.IdDepartamento == null ? "0" : d.IdDepartamento.ToString();
                vista.departamentos.Selected = idDep;
                CargarLocalidad(vista, _localidadRepositorio.GetLocalidadesByDepto(idDep == null ? 0 : Convert.ToInt32(idDep)));
                vista.Localidades.Selected = loc;
                CargarBarrio(vista, (from b in _localidadRepositorio.GetBarrios().Where(x => x.ID_LOCALIDAD == idlocalidad).DefaultIfEmpty()
                                     select b).ToList());
                vista.barrios.Selected = bb;

                var actorPersona = (from a in _actorRepositorio.GetActorPersona(vista.id_persona)
                                    select new ActorVista
                                    {
                                        ID_ACTOR = a.ID_ACTOR,
                                        ID_ACTOR_ROL = a.ID_ACTOR_ROL,
                                        N_ACTOR_ROL = a.N_ACTOR_ROL,

                                    }).ToList();

                cargarActorRol(vista, actorPersona);
            }
            else
            {
                //persona = new Persona();
                CargarDepartamento(vista, _departamentoRepositorio.GetDepartamentos());
                //vista.nombre = persona.nombre;
                //vista.id_localidad = 0;
                //vista.id_barrio = 0;
                //vista.departamentos.Selected = "0";

                if (vista.departamentos.Selected != "0" && vista.departamentos.Selected !=null)
                {
                    CargarLocalidad(vista, _localidadRepositorio.GetLocalidadesByDepto(Convert.ToInt32(vista.departamentos.Selected)));
                    vista.Localidades.Selected = loc;
                }
                else
                { CargarLocalidad(vista, _localidadRepositorio.GetLocalidades()); }

                if (vista.Localidades.Selected != "0" && vista.Localidades.Selected != null)
                {
                    CargarBarrio(vista, (from b in _localidadRepositorio.GetBarrios().Where(x => x.ID_LOCALIDAD == Convert.ToInt32(loc)).DefaultIfEmpty()
                                         select b).ToList());
                }
                else 
                { 
                    CargarBarrio(vista, _localidadRepositorio.GetBarrios());
                    vista.barrios.Selected = bb;
                }
                

            }



            if (accion == "Ver" || accion == "Eliminar")
            {
                vista.Localidades.Enabled = false;
                vista.barrios.Enabled = false;
                vista.departamentos.Enabled = false;

            }

            return vista;
        }

        private static void CargarActoresRol(IPersonaVista vista, IEnumerable<IActoresRol> listaActoresRol)
        {
            vista.ActorRoles.Combo.Add(new ComboItem { Id = 0, Description = "Seleccione un Rol..." });
            foreach (var actorRol in listaActoresRol)
            {
                vista.ActorRoles.Combo.Add(new ComboItem { Id = actorRol.ID_ACTOR_ROL, Description = actorRol.N_ACTOR_ROL });

            }
        }

        public IPersonaVista GetPersonaActor(int idPersona)
        {
            IPersonaVista vista = new PersonaVista();
            IPersona persona;
                persona = _personaRepositorio.GetPersona(idPersona);
                vista.id_persona = persona.id_persona;
                vista.nombre = persona.nombre;
                vista.apellido = persona.apellido;
                vista.dni = persona.dni;
                vista.cuil = persona.cuil;
                vista.celular = persona.celular;
                vista.telefono = persona.telefono;
                vista.mail = persona.mail;
                vista.id_barrio = persona.id_barrio;
                vista.id_localidad = persona.id_localidad;
                vista.empleado = persona.empleado;
                vista.id_usr_sist = persona.id_usr_sist;
                vista.fec_sist = persona.fec_sist;
                vista.id_usr_modif = persona.id_usr_modif;
                vista.fec_modif = persona.fec_modif;
                
                //buscar los actores de la persona
                //var actorPersona = _actorRepositorio.GetActorPersona(idPersona).ToList();
                var actorPersona = (from a in _actorRepositorio.GetActorPersona(idPersona)
                                  select new ActorVista
                                  {
                                      ID_ACTOR = a.ID_ACTOR,
                                      ID_ACTOR_ROL = a.ID_ACTOR_ROL,
                                      N_ACTOR_ROL = a.N_ACTOR_ROL,

                                  }).ToList();
                cargarActorRol(vista, actorPersona);
                //vista.listActorRol = actorPersona;//(IList<IActorVista>)actPersona.ToList();

                CargarActoresRol(vista, _personaRepositorio.GetActoresRol());
                
            

            return vista;
        }

        public IPersonaVista GetPersonaActorReload(PersonaVista vista)
        {
            string actRol = vista.ActorRoles.Selected == null ? "0" : vista.ActorRoles.Selected;
            
            IPersona persona;
            persona = _personaRepositorio.GetPersona(vista.id_persona);
            vista.id_persona = persona.id_persona;
            vista.nombre = persona.nombre;
            vista.apellido = persona.apellido;
            vista.dni = persona.dni;
            vista.cuil = persona.cuil;
            vista.celular = persona.celular;
            vista.telefono = persona.telefono;
            vista.mail = persona.mail;
            vista.id_barrio = persona.id_barrio;
            vista.id_localidad = persona.id_localidad;
            vista.empleado = persona.empleado;
            vista.id_usr_sist = persona.id_usr_sist;
            vista.fec_sist = persona.fec_sist;
            vista.id_usr_modif = persona.id_usr_modif;
            vista.fec_modif = persona.fec_modif;



            //buscar los actores de la persona
            //IList<IActor> actorPersona = _actorRepositorio.GetActorPersona(idPersona).ToList();
            var actorPersona = (from a in _actorRepositorio.GetActorPersona(vista.id_persona)
                              select new ActorVista
                              {
                                  ID_ACTOR = a.ID_ACTOR,
                                  ID_ACTOR_ROL = a.ID_ACTOR_ROL,
                                  N_ACTOR_ROL = a.N_ACTOR_ROL,

                              }).ToList();

            cargarActorRol(vista, actorPersona);//vista.listActorRol = (IList<IActorVista>)actPersona.ToList();

            CargarActoresRol(vista, _personaRepositorio.GetActoresRol());
            vista.ActorRoles.Selected = actRol;



            return vista;
        }

        private void cargarActorRol(IPersonaVista vista, IEnumerable<IActorVista> listaActores)

        {
         //vista.listActorRol = new List<IActorVista>();   
            foreach (var actorRol in listaActores)
            {
                vista.listActorRol.Add(new ActorVista
                {
                    ID_ACTOR = actorRol.ID_ACTOR,
                    ID_ACTOR_ROL = actorRol.ID_ACTOR_ROL,
                    N_ACTOR_ROL = actorRol.N_ACTOR_ROL
                });

            }

        }

        public int AddPersonaActor(IPersonaVista vista)
        {
            // verificar que se seleccione el rol
            if (vista.ActorRoles.Selected == "" || vista.ActorRoles.Selected == "0")
            {
                return 1;
            }
            // ya existe
            if (_actorRepositorio.GetActorPersona(vista.id_persona, Convert.ToInt32(vista.ActorRoles.Selected)).Count() > 0)
            {
                return 2;
            }

            _actorRepositorio.AddPersonaActor(VistaToDatosActor(vista));

            return 0;
        }

        private static IActor VistaToDatosActor(IPersonaVista vista)
        {
            IActor persona = new Actor
            {
                ID_PERSONA = vista.id_persona,
                ID_ACTOR_ROL = Convert.ToInt32(vista.ActorRoles.Selected),

            };

            return persona;
        }

        public IGrupoFamiliarVista GetGrupoFamiliar(IGrupoFamiliarVista vista)
        {
            vista.Familiares = _relfamiliaresRepositorio.GetFamiliares(vista.idficha);
            cargarVinculos(vista,_relfamiliaresRepositorio.getVinculos());
            return vista;
        }

        private void cargarVinculos(IGrupoFamiliarVista vista, IEnumerable<IVinculo> listaVinculos)
        {
            foreach (var vinculo in listaVinculos)
            {
                vista.vinculos.Combo.Add(new ComboItem { Id = vinculo.ID_VINCULO, Description = vinculo.N_VINCULO });

            }

        }
        public IPersona GetPersonaDni(string dni)
        {
            return _personaRepositorio.GetPersonaDni(dni);
        }

        public int AddFamiliar(IGrupoFamiliarVista model)
        {

            IRelacionFam familiar;

            familiar = new RelacionFam
            {
                ID_FICHA = model.idficha,
                ID_PERSONA = model.idPersona,
                ID_VINCULO = model.idVinculo,
            };
            _relfamiliaresRepositorio.AddFamiliar(familiar);
            return 0;
        }

        public int DelFamiliar(int idficha, int idvinculo)
        {
            _relfamiliaresRepositorio.DelFamiliar(idficha, idvinculo);
            return 0;
        }
    }
}
