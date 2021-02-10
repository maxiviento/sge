using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Entidades
{
    public class Asistencia : IAsistencia
    {
        public int ID_ASISTENCIA { get; set; }
        public int? ID_CURSO { get; set; }
        public int? ID_ESCUELA { get; set; }
        public int? ID_FICHA { get; set; }
        public string MES { get; set; }
        public string PERIODO { get; set; }
        public int? PORC_ASISTENCIA { get; set; }
        public int? ID_USR_SIST { get; set; }
        public DateTime? FEC_SIST { get; set; }
        public int? ID_USR_MODIF { get; set; }
        public DateTime? FEC_MODIF { get; set; }


        public int? tipoFicha { get; set; }
        public string Cuil { get; set; }
        public string Apellido { get; set; }
        public string Nombre { get; set; }
        public int? TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }



    }
}
