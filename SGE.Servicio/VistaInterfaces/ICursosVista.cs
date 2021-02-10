using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;
using SGE.Servicio.Vista.Shared;
using System;

namespace SGE.Servicio.VistaInterfaces
{
    public interface ICursosVista
    {

        int ID_CURSO { get; set; }
        string N_CURSO { get; set; }
        int? ID_DOCENTE { get; set; }
        int? ID_TUTOR { get; set; }
        int? ID_ONG { get; set; }
        int? ID_ESCUELA { get; set; }
        DateTime? F_INICIO { get; set; }
        DateTime? F_FIN { get; set; }
        string EXPEDIENTE_LEG { get; set; }
        int ID_USR_SIST { get; set; }
        DateTime? FEC_SIST { get; set; }
        int ID_USR_MODIF { get; set; }
        DateTime? FEC_MODIF { get; set; }

        //string DOC_APELLIDO { get; set; }
        //string DOC_NOMBRE { get; set; }
        //string DOC_DNI { get; set; }
        //string DOC_CUIL { get; set; }
        //string DOC_CELULAR { get; set; }
        //string DOC_TELEFONO { get; set; }
        //string DOC_MAIL { get; set; }
        //int? DOC_ID_LOCALIDAD { get; set; }
        //string DOC_N_LOCALIDAD { get; set; }

        //string TUT_APELLIDO { get; set; }
        //string TUT_NOMBRE { get; set; }
        //string TUT_DNI { get; set; }
        //string TUT_CUIL { get; set; }
        //string TUT_CELULAR { get; set; }
        //string TUT_TELEFONO { get; set; }
        //string TUT_MAIL { get; set; }
        //int? TUT_ID_LOCALIDAD { get; set; }
        //string TUT_N_LOCALIDAD { get; set; }

        IList<ICurso> cursos { get; set; }
        IPager Pager { get; set; }
        int Id { get; set; }
        string BusquedaDescripcion { get; set; }
        int? BusquedaNroCurso { get; set; }
    }
}
