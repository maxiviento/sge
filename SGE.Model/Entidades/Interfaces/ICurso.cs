using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGE.Model.Entidades.Interfaces
{
    public interface ICurso
    {

        int ID_CURSO { get; set; }
        string N_CURSO { get; set; }
        int? ID_DOCENTE { get; set; }
        int? ID_TUTOR { get; set; }
        int? ID_ONG { get; set; }
        int? ID_ESCUELA { get; set; }
        DateTime? F_INICIO  {get;set;}
        DateTime? F_FIN  {get;set;}
        string DURACION { get; set; }
        string EXPEDIENTE_LEG { get; set; }
        int ID_USR_SIST { get; set; }
        DateTime? FEC_SIST  {get;set;}
        int ID_USR_MODIF { get; set; }
        DateTime? FEC_MODIF { get; set; }

        string DOC_APELLIDO { get; set; }
        string DOC_NOMBRE { get; set; }
        string DOC_DNI { get; set; }
        string DOC_CUIL { get; set; }
        string DOC_CELULAR { get; set; }
        string DOC_TELEFONO { get; set; }
        string DOC_MAIL { get; set; }
        int? DOC_ID_LOCALIDAD { get; set; }
        string DOC_N_LOCALIDAD { get; set; }

        string TUT_APELLIDO { get; set; }
        string TUT_NOMBRE { get; set; }
        string TUT_DNI { get; set; }
        string TUT_CUIL { get; set; }
        string TUT_CELULAR { get; set; }
        string TUT_TELEFONO { get; set; }
        string TUT_MAIL { get; set; }
        int? TUT_ID_LOCALIDAD { get; set; }
        string TUT_N_LOCALIDAD { get; set; }

        int? NRO_COND_CURSO { get; set; }
        string MESES { get; set; }

        // 25/09/2016
        //int IdEscuelaCurso { get; set; }
        int? IdLocalidadCurso { get; set; }
        string NEscuelaCurso { get; set; }
        string CueCurso { get; set; }
        int? AnexoCurso { get; set; }
        string BarrioCurso { get; set; }
        string CALLECurso { get; set; }
        string NUMEROCurso { get; set; }
        string TELEFONOCurso { get; set; }
        string regiseCurso { get; set; }
        //ILocalidad LocalidadEscuela { get; set; }
        //IBarrio barrio { get; set; }
    }
}
