using System;
using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Entidades
{
    public class FichasCurso : IFichasCurso
    {
        public int IdFicha { get; set; }
        public int? tipoFicha { get; set; }
        public string Cuil { get; set; }
        public string Apellido { get; set; }
        public string Nombre { get; set; }
        public int? TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        //public IFichaConfVos FichaConfVos { get; set; }
        //public IBeneficiario BeneficiarioFicha { get; set; }
        public string MES { get; set; }
        public int? PORC_ASISTENCIA { get; set; }
        public int? id_curso { get; set; }

        public int idAsistencia { get; set; }
        public bool registrar { get; set; }


    }
}
