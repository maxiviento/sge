using System;
using System.Collections.Generic;
using SGE.Model.Comun;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Repositorio
{
    public interface IActorRepositorio
    {
        IList<IActor> GetActorDNI(string dni, int rolActor);
        IList<IActor> GetActor(int idActor);
        IList<IActor> GetActorPersona(int idPersona);
        int AddPersonaActor(IActor actor);
        void UpdatePersona(IActor actor);
        IList<IActor> GetActorPersona(int idPersona, int rolActor);
    }
}
