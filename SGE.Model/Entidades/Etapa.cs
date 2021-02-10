using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Entidades
{
    public class Etapa : IEtapa
    {
        public int ID_ETAPA {get;set;}
        public int ID_PROGRAMA { get; set; }
        public string N_ETAPA {get;set;}
        public decimal? MONTO_ETAPA { get; set; }
        public string EJERCICIO { get; set; }
        public DateTime FEC_INCIO { get; set; }
        public DateTime? FEC_FIN { get; set; }
        public int? ID_USR_SIST { get; set; }
        public DateTime? FEC_SIST { get; set; }
        public int? ID_USR_MODIF { get; set; }
        public DateTime? FEC_MODIF { get; set; }

        public string N_PROGRAMA { get; set; }
    }
}
