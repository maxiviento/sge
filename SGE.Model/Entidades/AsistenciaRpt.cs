using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Entidades
{
    public class AsistenciaRpt : IAsistenciaRpt
    {
        public int ID_FICHA { get; set; }
        public string APELLIDO { get; set; }
        public string NOMBRE { get; set; }
        public string CUIL { get; set; }
        public string NUMERO_DOCUMENTO { get; set; }
        public int ID_EST_FIC { get; set; }
        public string N_ESTADO_FICHA { get; set; }
        public int? ID_BENEFICIARIO { get; set; }
        public int? BEN_ID_ESTADO { get; set; }
        public string BEN_N_ESTADO { get; set; }
        public DateTime? BEN_FEC_INICIO { get; set; }
        public DateTime? BEN_FEC_BAJA { get; set; }
        public int? ESC_ID_ESCUELA { get; set; }
        public string N_ESCUELA { get; set; }
        public string ESC_CALLE { get; set; }
        public string ESC_NUMERO { get; set; }
        public string ESC_BARRIO { get; set; }
        public int ESC_ID_LOCALIDAD { get; set; }
        public string ESC_N_LOCALIDAD { get; set; }
        public string COO_APE { get; set; }
        public string COO_NOM { get; set; }
        public string COO_DNI { get; set; }
        public string ENC_APE { get; set; }
        public string ENC_NOM { get; set; }
        public string ENC_DNI { get; set; }
        public int? CUR_ID_CURSO { get; set; }
        public string N_CURSO { get; set; }
        public string PERIODO { get; set; }
        public int PORC_ASISTENCIA { get; set; }
        public int ID_ETAPA { get; set; }
        public int NRO_COND_CURSO { get; set; }
    }
}
