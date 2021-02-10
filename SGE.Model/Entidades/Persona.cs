using SGE.Model.Entidades.Interfaces;
using System;

namespace SGE.Model.Entidades
{
    public class Persona : IPersona
    {
        public int id_persona { get; set; }
        public string apellido { get; set; }
        public string nombre { get; set; }
        public string dni { get; set; }
        public string cuil { get; set; }
        public string celular { get; set; }
        public string telefono { get; set; }
        public string mail { get; set; }
        public int? id_barrio { get; set; }
        public int? id_localidad { get; set; }
        public string empleado { get; set; }
        public int? id_usr_sist { get; set; }
        public DateTime? fec_sist { get; set; }
        public int? id_usr_modif { get; set; }
        public DateTime? fec_modif { get; set; }
    }
}
