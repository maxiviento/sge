using System;
using System.Collections.Generic;
using System.Linq;
using SGE.Model.Entidades;
using SGE.Model.Entidades.Interfaces;
using SGE.Model.Repositorio;
using SGE.Repositorio.Modelo;

namespace SGE.Repositorio
{
    public class PersonaRepositorio :BaseRepositorio, IPersonaRepositorio
    {
        private readonly DataSGE _mdb;

        public PersonaRepositorio()
        {
            _mdb = new DataSGE();
        }

        private IQueryable<IPersona> QPersonas()
        {
            var a = (from i in _mdb.T_PERSONAS
                     select
                         new Persona
                             {
                                id_persona = i.ID_PERSONA,
                                nombre = i.NOMBRE,
                                apellido = i.APELLIDO,
                                dni =i.DNI,
                                cuil =i.CUIL,
                                celular =i.CELULAR,
                                telefono =i.TELEFONO,
                                mail =i.MAIL,
                                id_barrio =i.ID_BARRIO,
                                id_localidad =i.ID_LOCALIDAD,
                                empleado =i.EMPLEADO ?? "N",
                                id_usr_sist =i.ID_USR_SIST,
                                fec_sist = DateTime.Now,
                                id_usr_modif =i.ID_USR_MODIF,
                                fec_modif =i.FEC_MODIF
                             });

            return a;
        }
        public IList<IPersona> GetPersonas()
        {
            return QPersonas().ToList();
        }

        public int AddPersona(IPersona persona)
        {
            var a = (from c in _mdb.T_PERSONAS
                     select c.ID_PERSONA);
            //int cont = 0;
            //if(a.Max()!=null )
            //{ cont = a.Max(); }
            //AgregarDatos(persona);
            var model = new T_PERSONAS
                              {
                                  ID_PERSONA = a.Max() + 1,
                                  NOMBRE = persona.nombre,
                                  APELLIDO = persona.apellido,
                                  DNI = persona.dni,
                                  CUIL = persona.cuil,
                                  CELULAR = persona.celular,
                                  TELEFONO = persona.telefono,
                                  MAIL = persona.mail,
                                  ID_BARRIO = persona.id_barrio == 0 ? null : persona.id_barrio,
                                  ID_LOCALIDAD = persona.id_localidad == 0 ? null : persona.id_localidad,
                                  EMPLEADO = persona.empleado,
                                  ID_USR_SIST = GetUsuarioLoguer().Id_Usuario,
                                  FEC_SIST = DateTime.Now,
                                  //ID_USR_MODIF = persona.id_usr_modif == 0 ? null : persona.id_usr_modif,
                                  //FEC_MODIF = persona.fec_modif
                              };

            _mdb.T_PERSONAS.AddObject(model);
            _mdb.SaveChanges();
            return model.ID_PERSONA;
        }

        public IPersona GetPersona(int id)
        {
            return QPersonas().Where(c => c.id_persona == id).SingleOrDefault();
        }

        public IList<IPersona> GetPersonas(int skip, int take)
        {
            return QPersonas().OrderBy(c => c.apellido.Trim().ToUpper()).ThenBy(o => o.nombre.Trim().ToUpper()).Skip(skip).Take(take).ToList();
        }

        private IQueryable<IPersona> QGetPersonas(string descripcion)
        {
            descripcion = descripcion ?? String.Empty;

            return
                QPersonas().Where(
                    c =>
                    (c.dni.ToLower().Contains(descripcion.ToLower()) || string.IsNullOrEmpty(descripcion))).OrderBy(c => c.apellido.Trim().ToUpper()).ThenBy(o => o.nombre.Trim().ToUpper());
        }

        public IList<IPersona> GetPersonas(string descripcion)
        {
            return QGetPersonas(descripcion).OrderBy(c => c.apellido.Trim().ToUpper()).ThenBy(o => o.nombre.Trim().ToUpper()).ToList();
        }

        public IList<IPersona> GetPersonas(string descripcion, int skip, int take)
        {
            return QGetPersonas(descripcion).OrderBy(c => c.apellido.Trim().ToUpper()).ThenBy(o => o.nombre.Trim().ToUpper()).Skip(skip).Take(take).ToList();
        }

        public void UpdatePersona(IPersona persona)
        {
            if (persona.id_persona != 0)
            {
                var row = _mdb.T_PERSONAS.SingleOrDefault(c => c.ID_PERSONA == persona.id_persona);

                if (row != null)
                {
                                  
                                  row.NOMBRE = persona.nombre;
                                  row.APELLIDO = persona.apellido;
                                  row.DNI = persona.dni;
                                  row.CUIL = persona.cuil;
                                  row.CELULAR = persona.celular;
                                  row.TELEFONO = persona.telefono;
                                  row.MAIL = persona.mail;
                                  row.ID_BARRIO = persona.id_barrio == 0 ? null : persona.id_barrio;
                                  row.ID_LOCALIDAD = persona.id_localidad == 0 ? null : persona.id_localidad;
                                  row.EMPLEADO = persona.empleado;
                                  //row.ID_USR_SIST = GetUsuarioLoguer().Id_Usuario,
                                  //row.FEC_SIST = DateTime.Now,
                                  row.ID_USR_MODIF = GetUsuarioLoguer().Id_Usuario;
                                  row.FEC_MODIF = DateTime.Now;

                    _mdb.SaveChanges();
                }
            }
        }

        //public void DeleteCarrera(ICarrera carrera)
        //{
        //    var row = _mdb.T_CARRERAS.SingleOrDefault(c => c.ID_CARRERA == carrera.IdCarrera);

        //    if (row.ID_CARRERA == 0) return;
            
        //    _mdb.DeleteObject(row);
        //    _mdb.SaveChanges();
        //}

        public int GetPersonasCount()
        {
            return QPersonas().Count();
        }

        public IList<IPersona> GetPersonasDNI(string dni)
        {
            return QPersonas().Where(x=>x.dni==dni).ToList();
        }


        private IQueryable<IActoresRol> QActoresRol()
        {
            var a = from r in _mdb.T_ACTORES_ROL
                    select new ActoresRol
                    {
                        ID_ACTOR_ROL = r.ID_ACTOR_ROL,
                        N_ACTOR_ROL = r.N_ACTOR_ROL,

                    };
        
            return a;
        }

        public IList<IActoresRol> GetActoresRol()
        {
            return QActoresRol().OrderBy(x=>x.N_ACTOR_ROL).ToList();
        }

        public IPersona GetPersonaDni(string dni)
        {

            return QPersonas().Where(x=>x.dni == dni ).SingleOrDefault();
        
        }

    }
}
