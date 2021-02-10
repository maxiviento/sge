using System;
using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces.Shared;

namespace SGE.Servicio.VistaInterfaces
{
    public interface IPersonaVista
    {
        int id_persona { get; set; }
        string apellido { get; set; }
        string nombre { get; set; }
        string dni { get; set; }
        string cuil { get; set; }
        string celular { get; set; }
        string telefono { get; set; }
        string mail { get; set; }
        int? id_barrio { get; set; }
        int? id_localidad { get; set; }
        string empleado { get; set; }
        int? id_usr_sist { get; set; }
        DateTime? fec_sist { get; set; }
        int? id_usr_modif { get; set; }
        DateTime? fec_modif { get; set; }

        string Accion { get; set; }
        string AccionLlamada { get; set; }
        IComboBox Localidades { get; set; }
        IComboBox barrios { get; set; }
        IComboBox ActorRoles { get; set; }
        IComboBox departamentos { get; set; }
        IList<IActorVista> listActorRol { get; set; }
        IActorVista newActor { get; set; }


    }
}
