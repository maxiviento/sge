using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces;
using SGE.Model.Entidades;

namespace SGE.Servicio.Vista
{
    public class ActorVista : IActorVista
    {

        public int ID_ACTOR { get; set; }
        public string APELLIDO { get; set; }
        public string NOMBRE { get; set; }
        public string DNI { get; set; }
        public string CUIL { get; set; }
        public string CELULAR { get; set; }
        public string TELEFONO { get; set; }
        public string MAIL { get; set; }
        public int? ID_LOCALIDAD { get; set; }
        public string N_LOCALIDAD { get; set; }
        public int? ID_ACTOR_ROL { get; set; }
        public string N_ACTOR_ROL { get; set; }
        public int? ID_BARRIO { get; set; }
        public string N_BARRIO { get; set; }
        public IList<IActor> Actores { get; set; }
        public IPager Pager { get; set; }
        public int Id { get; set; }
        public string BusquedaDescripcion { get; set; }

        
    }
}
