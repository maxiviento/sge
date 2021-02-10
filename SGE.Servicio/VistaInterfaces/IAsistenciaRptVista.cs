using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SGE.Model.Entidades.Interfaces;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces.Shared;

namespace SGE.Servicio.VistaInterfaces
{
    public interface IAsistenciaRptVista
    {
        IList<IAsistenciaRpt> ListaAsistencias { get; set; }
        IPager Pager { get; set; }
        string FileDownload { get; set; }
        string FileNombre { get; set; }

        bool ErrorDatos { get; set; }
        int[] idFichas { get; set; }

        //Para buscar por curso
        int ID_CURSO { get; set; }
        string N_CURSO { get; set; }
        int? NRO_COND_CURSO { get; set; }
        int? ID_DOCENTE { get; set; }
        int? ID_TUTOR { get; set; }
        int? ID_ONG { get; set; }
        int? ID_ESCUELA { get; set; }
        DateTime? F_INICIO { get; set; }
        DateTime? F_FIN { get; set; }
        string DURACION { get; set; }
        string EXPEDIENTE_LEG { get; set; }
        //int ID_USR_SIST { get; set; }
        //DateTime? FEC_SIST { get; set; }
        //int ID_USR_MODIF { get; set; }
        //DateTime? FEC_MODIF { get; set; }

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
        string mesPeriodo { get; set; }
        int idCurso { get; set; }
        int idEscuela { get; set; }
        int idEstadoBenf { get; set; }
        IComboBox cboperiodos { get; set; }
        IComboBox EstadosBeneficiarios { get; set; }
        string BuscarPorEtapa { get; set; }
        IComboBox EtapaProgramas { get; set; }
        string ejerc { get; set; }
        IComboBox etapas { get; set; }
        int filtroEtapa { get; set; }
        int programaSel { get; set; }
        string filtroEjerc { get; set; }
        string filtroNombre { get; set; }
    }
}
