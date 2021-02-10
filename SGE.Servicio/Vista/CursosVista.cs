using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces;
using System.ComponentModel.DataAnnotations;
using System;

namespace SGE.Servicio.Vista
{
    public class CursosVista : ICursosVista
    {
        public CursosVista()
        {
            cursos = new List<ICurso>();
        
        }
        public int ID_CURSO { get; set; }
        public string N_CURSO { get; set; }
        public int? ID_DOCENTE { get; set; }
        public int? ID_TUTOR { get; set; }
        public int? ID_ONG { get; set; }
        public int? ID_ESCUELA { get; set; }
        public DateTime? F_INICIO { get; set; }
        public DateTime? F_FIN { get; set; }
        public string EXPEDIENTE_LEG { get; set; }
        public int ID_USR_SIST { get; set; }
        public DateTime? FEC_SIST { get; set; }
        public int ID_USR_MODIF { get; set; }
        public DateTime? FEC_MODIF { get; set; }

        //public string DOC_APELLIDO { get; set; }
        //public string DOC_NOMBRE { get; set; }
        //public string DOC_DNI { get; set; }
        //public string DOC_CUIL { get; set; }
        //public string DOC_CELULAR { get; set; }
        //public string DOC_TELEFONO { get; set; }
        //public string DOC_MAIL { get; set; }
        //public int? DOC_ID_LOCALIDAD { get; set; }
        //public string DOC_N_LOCALIDAD { get; set; }

        //public string TUT_APELLIDO { get; set; }
        //public string TUT_NOMBRE { get; set; }
        //public string TUT_DNI { get; set; }
        //public string TUT_CUIL { get; set; }
        //public string TUT_CELULAR { get; set; }
        //public string TUT_TELEFONO { get; set; }
        //public string TUT_MAIL { get; set; }
        //public int? TUT_ID_LOCALIDAD { get; set; }
        //public string TUT_N_LOCALIDAD { get; set; }

        public IList<ICurso> cursos { get; set; }
        public IPager Pager { get; set; }
        public int Id { get; set; }
        public string BusquedaDescripcion { get; set; }
        public int? BusquedaNroCurso { get; set; }

    }
}
