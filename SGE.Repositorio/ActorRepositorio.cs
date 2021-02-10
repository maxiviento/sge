using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using SGE.Model.Comun;
using SGE.Model.Entidades;
using SGE.Model.Entidades.Interfaces;
using SGE.Model.Repositorio;
using SGE.Repositorio.Modelo;

namespace SGE.Repositorio
{
    public class ActorRepositorio :BaseRepositorio, IActorRepositorio
    {
        private readonly DataSGE _mdb;

        public ActorRepositorio()
        {
            _mdb = new DataSGE();
        }
        private IQueryable<IActor> QActorAll()
        {

            var a = (from act in _mdb.T_ACTORES
                     join r in _mdb.T_ACTORES_ROL on act.ID_ACTOR_ROL equals r.ID_ACTOR_ROL
                     from p in _mdb.T_PERSONAS.Where(p => p.ID_PERSONA == (int)act.ID_PERSONA).DefaultIfEmpty()
                     from l in _mdb.T_LOCALIDADES.Where(l => l.ID_LOCALIDAD == p.ID_LOCALIDAD).DefaultIfEmpty()
                     select new Actor
                     {
                         ID_ACTOR = act.ID_ACTOR,
                         APELLIDO = p.APELLIDO,
                         NOMBRE = p.NOMBRE,
                         DNI = p.DNI,
                         CUIL = p.CUIL,
                         CELULAR = p.CELULAR,
                         TELEFONO = p.TELEFONO,
                         MAIL = p.MAIL,
                         ID_LOCALIDAD = l.ID_LOCALIDAD,
                         N_LOCALIDAD = l.N_LOCALIDAD,
                         ID_ACTOR_ROL = r.ID_ACTOR_ROL,
                         N_ACTOR_ROL = r.N_ACTOR_ROL,
                         ID_PERSONA = act.ID_PERSONA,
                     });

            return a;


        }


        public IList<IActor> GetActorDNI(string dni, int rolActor)
        {
            switch(rolActor)
            {
                case 0:
                    return QActorAll().Where(x=>x.DNI ==dni).ToList();
                

                default:
                    return QActorAll().Where(x => x.DNI == dni && x.ID_ACTOR_ROL == rolActor).ToList();
                
            }        
        }

        public IList<IActor> GetActor(int idActor)
        {

                    return QActorAll().Where(x => x.ID_ACTOR == idActor).ToList();

        }

        public IList<IActor> GetActorPersona(int idPersona)
        {

            return QActorAll().Where(x => x.ID_PERSONA == idPersona).ToList();

        }

        public int AddPersonaActor(IActor actor)
        {
            var a = (from c in _mdb.T_ACTORES
                     select c.ID_ACTOR);
            //AgregarDatos(persona);
            var model = new T_ACTORES
            {
                ID_ACTOR = (int)a.Max() + 1,
                ID_PERSONA = actor.ID_PERSONA,
                ID_ACTOR_ROL = actor.ID_ACTOR_ROL,
                ID_USR_SIST = GetUsuarioLoguer().Id_Usuario,
                FEC_SIST = DateTime.Now,
                //ID_USR_MODIF = persona.id_usr_modif == 0 ? null : persona.id_usr_modif,
                //FEC_MODIF = persona.fec_modif
            };

            _mdb.T_ACTORES.AddObject(model);
            _mdb.SaveChanges();
            return model.ID_ACTOR;
        }

        public void UpdatePersona(IActor actor)
        {
            if (actor.ID_ACTOR != 0)
            {
                var row = _mdb.T_ACTORES.SingleOrDefault(c => c.ID_RESPONSABLE == actor.ID_ACTOR);

                if (row != null)
                {

                    row.ID_PERSONA = actor.ID_PERSONA;
                    row.ID_ACTOR_ROL = actor.ID_ACTOR_ROL;
                    row.ID_USR_MODIF = GetUsuarioLoguer().Id_Usuario;
                    row.FEC_MODIF = DateTime.Now;

                    _mdb.SaveChanges();
                }
            }
        }

        public IList<IActor> GetActorPersona(int idPersona, int rolActor)
        {
            switch (rolActor)
            {
                case 0:
                    return QActorAll().Where(x => x.ID_PERSONA == idPersona).ToList();


                default:
                    return QActorAll().Where(x => x.ID_PERSONA == idPersona && x.ID_ACTOR_ROL == rolActor).ToList();

            }
        }
        
    }
}
