using System;
using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Entidades
{
    public class Barrio : IBarrio
    {
        public int ID_BARRIO {get; set;}
        public string N_BARRIO { get; set; }
        public int? ID_SECCIONAL { get; set; }
        public int? ID_LOCALIDAD { get; set; }
        public int? ID_USR_SIST { get; set; }
        public DateTime? FEC_SIST { get; set; }
        public int? ID_USR_MODIF { get; set; }
        public DateTime? FEC_MODIF { get; set; }

        public string N_LOCALIDAD { get; set; }
        public string N_SECCIONAL { get; set; }
    }  
}
