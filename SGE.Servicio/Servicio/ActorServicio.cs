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
using SGE.Model.Comun;

namespace SGE.Servicio.Servicio
{
    public class ActorServicio : IActorServicio
    {
        private readonly IActorRepositorio _ActorRepositorio;

        public ActorServicio()
        {
            _ActorRepositorio = new ActorRepositorio();
        
        }

        public IActorVista GetActorDNI(string dni, int rolActor)
        {
            IActorVista vista = new ActorVista();


            vista.Actores = _ActorRepositorio.GetActorDNI(dni, rolActor);

            return vista;
        }

        public IActorVista GetActor(int idActor)
        {
            IActorVista vista = new ActorVista();


            vista.Actores = _ActorRepositorio.GetActor(idActor);

            return vista;
        }

        public IActorVista GetFacilitadorMonitorDNI(string dni)
        {
            IActorVista vista = new ActorVista();

            IList<IActor> actores;
            actores = _ActorRepositorio.GetActorDNI(dni, 0).Where(x => x.ID_ACTOR_ROL == (int)Enums.Actor_Rol.FACILITADOR || x.ID_ACTOR_ROL == (int)Enums.Actor_Rol.MONITOR).ToList();
            vista.Actores = actores;

            return vista;
        }
    }
}
