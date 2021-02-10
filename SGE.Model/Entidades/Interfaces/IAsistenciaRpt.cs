using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGE.Model.Entidades.Interfaces
{
    public interface IAsistenciaRpt
    {
        int ID_FICHA { get; set; }
        string APELLIDO { get; set; }
        string NOMBRE { get; set; }
        string CUIL { get; set; }
        string NUMERO_DOCUMENTO { get; set; }
        int ID_EST_FIC { get; set; }
        string N_ESTADO_FICHA { get; set; }
        int? ID_BENEFICIARIO { get; set; }
        int? BEN_ID_ESTADO { get; set; }
        string BEN_N_ESTADO { get; set; }
        DateTime? BEN_FEC_INICIO { get; set; }
        DateTime? BEN_FEC_BAJA { get; set; }
        int? ESC_ID_ESCUELA { get; set; }
        string N_ESCUELA { get; set; }
        string ESC_CALLE { get; set; }
        string ESC_NUMERO { get; set; }
        string ESC_BARRIO { get; set; }
        int ESC_ID_LOCALIDAD { get; set; }
        string ESC_N_LOCALIDAD { get; set; }
        string COO_APE { get; set; }
        string COO_NOM { get; set; }
        string COO_DNI { get; set; }
        string ENC_APE { get; set; }
        string ENC_NOM { get; set; }
        string ENC_DNI { get; set; }
        int? CUR_ID_CURSO { get; set; }
        string N_CURSO { get; set; }
        string PERIODO { get; set; }
        int PORC_ASISTENCIA { get; set; }
        int ID_ETAPA { get; set; }
        int NRO_COND_CURSO { get; set; }

    }
}
