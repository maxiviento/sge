using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Repositorio
{
    public interface IPersonaRepositorio
    {

        IList<IPersona> GetPersonas();

        int AddPersona(IPersona persona);
        IPersona GetPersona(int id);
        IList<IPersona> GetPersonas(int skip, int take);
        IList<IPersona> GetPersonas(string descripcion);
        IList<IPersona> GetPersonas(string descripcion, int skip, int take);
        void UpdatePersona(IPersona persona);
        int GetPersonasCount();
        IList<IPersona> GetPersonasDNI(string dni);
        IList<IActoresRol> GetActoresRol();
        IPersona GetPersonaDni(string dni);


    }
}
