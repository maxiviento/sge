using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SGE.Model.Entidades.Interfaces;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces.Shared;
using SGE.Servicio.VistaInterfaces;

namespace SGE.Servicio.Vista
{
    public class AsistenciaRptVista: IAsistenciaRptVista
    {
        public AsistenciaRptVista()
        {
            cboperiodos = new ComboBox();
            EstadosBeneficiarios = new ComboBox();
            etapas = new ComboBox();
            EtapaProgramas = new ComboBox();
        }
        public IList<IAsistenciaRpt> ListaAsistencias { get; set; }
        public IPager Pager { get; set; }
        public string FileDownload { get; set; }
        public string FileNombre { get; set; }

        public bool ErrorDatos { get; set; }
        public int[] idFichas { get; set; }

        //Para buscar por curso
        public int ID_CURSO { get; set; }
        public string N_CURSO { get; set; }
        public int? NRO_COND_CURSO { get; set; }
        public int? ID_DOCENTE { get; set; }
        public int? ID_TUTOR { get; set; }
        public int? ID_ONG { get; set; }
        public int? ID_ESCUELA { get; set; }
        public DateTime? F_INICIO { get; set; }
        public DateTime? F_FIN { get; set; }
        public string DURACION { get; set; }
        public string EXPEDIENTE_LEG { get; set; }
        //public int ID_USR_SIST { get; set; }
        //public DateTime? FEC_SIST { get; set; }
        //public int ID_USR_MODIF { get; set; }
        //public DateTime? FEC_MODIF { get; set; }
           
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

        public string mesPeriodo { get; set; }
        public int idCurso { get; set; }
        public int idEscuela { get; set; }
        public int idEstadoBenf { get; set; }
        public IComboBox cboperiodos { get; set; }
        public IComboBox EstadosBeneficiarios { get; set; }
        public string N_ESCUELA { get; set; }
        public IComboBox EtapaProgramas { get; set; }
        public string ejerc { get; set; }
        public string BuscarPorEtapa { get; set; }
        public IComboBox etapas { get; set; }
        public int filtroEtapa { get; set; }
        public int programaSel { get; set; }
        public string filtroEjerc { get; set; }
        public string filtroNombre { get; set; }


    }
}
