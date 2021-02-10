using SGE.Servicio.VistaInterfaces;
using System;
using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;
using SGE.Servicio.Vista.Shared;
using System.ComponentModel.DataAnnotations;
using SGE.Model.Entidades;
using SGE.Servicio.VistaInterfaces.Shared;

namespace SGE.Servicio.Vista
{
    public class CursoVista : ICursoVista
    {
        public CursoVista()
        {
            //escuela = new Escuela();   
            asistencias = new List<IAsistencia>();
            periodos = new List<IPeriodo>();
            fichas = new List<IFichasCurso>();
            cboperiodos = new ComboBox();
        }

        public int ID_CURSO { get; set; }
        [StringLength(100, ErrorMessage = "Nombre supera los 100 caracteres.")]
        [Required(ErrorMessage = "Ingrese el Nombre del curso")]
        public string N_CURSO { get; set; }
        public int? ID_DOCENTE { get; set; }
        public int? ID_TUTOR { get; set; }
        public int? ID_ONG { get; set; }
        public int? ID_ESCUELA { get; set; }
        public DateTime? F_INICIO { get; set; }
        public DateTime? F_FIN { get; set; }
        [StringLength(100, ErrorMessage = "Nombre supera los 30 caracteres.")]
        [Required(ErrorMessage = "Ingrese el Expediente")]
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
        public IList<ICurso> cursos { get; set; }

        public string Accion { get; set; }
        public string AccionLlamada { get; set; }

        public string DURACION { get; set; }
        public int? NRO_COND_CURSO { get; set; }

        public string ONG_N_NOMBRE { get; set; }
        public string ONG_CUIT { get; set; }
        public string ONG_CELULAR { get; set; }
        public string ONG_TELEFONO { get; set; }
        public string ONG_MAIL { get; set; }
        public int? ONG_ID_RESPONSABLE { get; set; }
        public int? ONG_ID_CONTACTO { get; set; }
        public string ONG_FACTURACION { get; set; }
        public string RES_APELLIDO { get; set; }
        public string RES_NOMBRE { get; set; }
        public string RES_DNI { get; set; }
        public string RES_CUIL { get; set; }
        public string RES_CELULAR { get; set; }
        public string RES_TELEFONO { get; set; }
        public string RES_MAIL { get; set; }
        public int? RES_ID_LOCALIDAD { get; set; }
        public string RES_N_LOCALIDAD { get; set; }
        public string CON_APELLIDO { get; set; }
        public string CON_NOMBRE { get; set; }
        public string CON_DNI { get; set; }
        public string CON_CUIL { get; set; }
        public string CON_CELULAR { get; set; }
        public string CON_TELEFONO { get; set; }
        public string CON_MAIL { get; set; }
        public int? CON_ID_LOCALIDAD { get; set; }
        public string CON_N_LOCALIDAD { get; set; }

        //public IEscuela escuela { get; set; }

        // 22/05/2015 - DATOS ESCUELA 
        public string N_ESCUELA { get; set; }
        public string Cue { get; set; }
        public int? Anexo { get; set; }
        public string Barrio { get; set; }
        public string CALLE { get; set; }
        public string NUMERO { get; set; }
        public int? CUPO_CONFIAMOS { get; set; }
        public string TELEFONO { get; set; }
        public string COOR_APELLIDO { get; set; }
        public string COOR_NOMBRE { get; set; }
        public string COOR_DNI { get; set; }
        public string COOR_CUIL { get; set; }
        public string COOR_CELULAR { get; set; }
        public string COOR_TELEFONO { get; set; }
        public string COOR_MAIL { get; set; }
        public int? COOR_ID_LOCALIDAD { get; set; }
        public string COOR_N_LOCALIDAD { get; set; }
        public string DIR_APELLIDO { get; set; }
        public string DIR_NOMBRE { get; set; }
        public string DIR_DNI { get; set; }
        public string DIR_CUIL { get; set; }
        public string DIR_CELULAR { get; set; }
        public string DIR_TELEFONO { get; set; }
        public string DIR_MAIL { get; set; }
        public string ENC_APELLIDO { get; set; }
        public string ENC_NOMBRE { get; set; }
        public string ENC_DNI { get; set; }
        public string ENC_CUIL { get; set; }
        public string ENC_CELULAR { get; set; }
        public string ENC_TELEFONO { get; set; }
        public string ENC_MAIL { get; set; }
        public int? ENC_ID_LOCALIDAD { get; set; }
        public string ENC_N_LOCALIDAD { get; set; }

        public IList<IAsistencia> asistencias { get; set; }
        public IList<IPeriodo> periodos { get; set; }
        public IList<IFichasCurso> fichas { get; set; }
        public IComboBox cboperiodos { get; set; }
        public int totalFichas { get; set; }
        public string MESES { get; set; }
    }
}
