using System.Collections.Generic;
using SGE.Servicio.VistaInterfaces.Shared;
using SGE.Model.Entidades.Interfaces;
using System;
using SGE.Servicio.Vista.Shared;

namespace SGE.Servicio.VistaInterfaces
{
    public interface IActorVista
    {
        int ID_ACTOR { get; set; }
        string APELLIDO { get; set; }
        string NOMBRE { get; set; }
        string DNI { get; set; }
        string CUIL { get; set; }
        string CELULAR { get; set; }
        string TELEFONO { get; set; }
        string MAIL { get; set; }
        int? ID_LOCALIDAD { get; set; }
        string N_LOCALIDAD { get; set; }
        int? ID_ACTOR_ROL { get; set; }
        string N_ACTOR_ROL { get; set; }
        int? ID_BARRIO { get; set; }
        string N_BARRIO { get; set; }
        IList<IActor> Actores { get; set; }
        IPager Pager { get; set; }
        int Id { get; set; }
        string BusquedaDescripcion { get; set; }
    }
}
