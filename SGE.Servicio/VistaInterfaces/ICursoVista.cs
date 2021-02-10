using System.Collections.Generic;
using SGE.Servicio.VistaInterfaces.Shared;
using SGE.Model.Entidades.Interfaces;
using System;
using SGE.Servicio.Vista.Shared;
using SGE.Model.Entidades;

namespace SGE.Servicio.VistaInterfaces
{
    public interface ICursoVista
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
        IList<ICurso> cursos { get; set; }
        string Accion { get; set; }
        string AccionLlamada { get; set; }
        string DURACION { get; set; }
        int? NRO_COND_CURSO { get; set; }

        //Datos de ONG
        string ONG_N_NOMBRE { get; set; }
        string ONG_CUIT { get; set; }
        string ONG_CELULAR { get; set; }
        string ONG_TELEFONO { get; set; }
        string ONG_MAIL { get; set; }
        int? ONG_ID_RESPONSABLE { get; set; }
        int? ONG_ID_CONTACTO { get; set; }
        string ONG_FACTURACION { get; set; }
        string RES_APELLIDO { get; set; }
        string RES_NOMBRE { get; set; }
        string RES_DNI { get; set; }
        string RES_CUIL { get; set; }
        string RES_CELULAR { get; set; }
        string RES_TELEFONO { get; set; }
        string RES_MAIL { get; set; }
        int? RES_ID_LOCALIDAD { get; set; }
        string RES_N_LOCALIDAD { get; set; }
        string CON_APELLIDO { get; set; }
        string CON_NOMBRE { get; set; }
        string CON_DNI { get; set; }
        string CON_CUIL { get; set; }
        string CON_CELULAR { get; set; }
        string CON_TELEFONO { get; set; }
        string CON_MAIL { get; set; }
        int? CON_ID_LOCALIDAD { get; set; }
        string CON_N_LOCALIDAD { get; set; }

        //IEscuela escuela { get; set; }

        string N_ESCUELA { get; set; }
        string Cue { get; set; }
        int? Anexo { get; set; }
        string Barrio { get; set; }
        string CALLE { get; set; }
        string NUMERO { get; set; }
        int? CUPO_CONFIAMOS { get; set; }
        string TELEFONO { get; set; }
        string COOR_APELLIDO { get; set; }
        string COOR_NOMBRE { get; set; }
        string COOR_DNI { get; set; }
        string COOR_CUIL { get; set; }
        string COOR_CELULAR { get; set; }
        string COOR_TELEFONO { get; set; }
        string COOR_MAIL { get; set; }
        int? COOR_ID_LOCALIDAD { get; set; }
        string COOR_N_LOCALIDAD { get; set; }
        string DIR_APELLIDO { get; set; }
        string DIR_NOMBRE { get; set; }
        string DIR_DNI { get; set; }
        string DIR_CUIL { get; set; }
        string DIR_CELULAR { get; set; }
        string DIR_TELEFONO { get; set; }
        string DIR_MAIL { get; set; }
        string ENC_APELLIDO { get; set; }
        string ENC_NOMBRE { get; set; }
        string ENC_DNI { get; set; }
        string ENC_CUIL { get; set; }
        string ENC_CELULAR { get; set; }
        string ENC_TELEFONO { get; set; }
        string ENC_MAIL { get; set; }
        int? ENC_ID_LOCALIDAD { get; set; }
        string ENC_N_LOCALIDAD { get; set; }
        IList<IAsistencia> asistencias { get; set; }
        IList<IPeriodo> periodos { get; set; }
        IList<IFichasCurso> fichas { get; set; }
        IComboBox cboperiodos { get; set; }
        int totalFichas { get; set; }
        string MESES { get; set; }
    }
}
