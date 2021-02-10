using System;
using System.Collections.Generic;
using System.Web;
using SGE.Servicio.Comun;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces;
using SGE.Model.Entidades.Interfaces;
using SGE.Servicio.Vista;

namespace SGE.Servicio.ServicioInterfaces
{
    public interface IPersonaServicio
    {
        IPersonasVista GetPersonas();
        IPersonasVista GetIndex();
        IPersonasVista GetPersonas(string descripcion);
        IPersonaVista GetPersona(int idPersona, string accion);
        IPersonasVista GetPersonas(Pager pPager, int id, string descripcion);
        int AddPersona(IPersonaVista vista);
        int UpdatePersona(IPersonaVista vista);
        List<ILocalidad> cboCargarLocalidad(int idDep);
        List<IBarrio> cboCargarBarrio(int idLocalidad);
        IPersonaVista GetPersonaReload(PersonaVista vista);
        IPersonaVista GetPersonaActor(int idPersona);
        IPersonaVista GetPersonaActorReload(PersonaVista vista);
        int AddPersonaActor(IPersonaVista vista);
        IGrupoFamiliarVista GetGrupoFamiliar(IGrupoFamiliarVista vista);
        IPersona GetPersonaDni(string dni);
        int AddFamiliar(IGrupoFamiliarVista model);
        int DelFamiliar(int idficha, int idvinculo);


    }
}
