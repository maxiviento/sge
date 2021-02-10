using System;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Entidades
{
    public class Curso: ICurso
    {
        public int ID_CURSO { get; set; }
        public string N_CURSO { get; set; }
        public int? ID_DOCENTE { get; set; }
        public int? ID_TUTOR { get; set; }
        public int? ID_ONG { get; set; }
        public int? ID_ESCUELA { get; set; }
        public DateTime? F_INICIO { get; set; }
        public DateTime? F_FIN { get; set; }
        public string DURACION { get; set; }
        public string EXPEDIENTE_LEG { get; set; }
        public int ID_USR_SIST { get; set; }
        public DateTime? FEC_SIST { get; set; }
        public int ID_USR_MODIF { get; set; }
        public DateTime? FEC_MODIF { get; set; }

        public string DOC_APELLIDO { get; set; }
        public string DOC_NOMBRE { get; set; }
        public string DOC_DNI { get; set; }
        public string DOC_CUIL { get; set; }
        public string DOC_CELULAR { get; set; }
        public string DOC_TELEFONO { get; set; }
        public string DOC_MAIL { get; set; }
        public int? DOC_ID_LOCALIDAD { get; set; }
        public string DOC_N_LOCALIDAD { get; set; }

        public string TUT_APELLIDO { get; set; }
        public string TUT_NOMBRE { get; set; }
        public string TUT_DNI { get; set; }
        public string TUT_CUIL { get; set; }
        public string TUT_CELULAR { get; set; }
        public string TUT_TELEFONO { get; set; }
        public string TUT_MAIL { get; set; }
        public int? TUT_ID_LOCALIDAD { get; set; }
        public string TUT_N_LOCALIDAD { get; set; }

        public int? NRO_COND_CURSO { get; set; }
        public string MESES { get; set; }


        // 25/09/2016
        //public int IdEscuelaCurso { get; set; }
        public int? IdLocalidadCurso { get; set; }
        public string NEscuelaCurso { get; set; }
        public string CueCurso { get; set; }
        public int? AnexoCurso { get; set; }
        public string BarrioCurso { get; set; }
        public string CALLECurso { get; set; }
        public string NUMEROCurso { get; set; }
        public string TELEFONOCurso { get; set; }
        public string regiseCurso { get; set; }
        //public ILocalidad LocalidadEscuela { get; set; }
        //public IBarrio barrio { get; set; }


    }
}
