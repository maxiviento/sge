using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SGE.Servicio.VistaInterfaces;
using SGE.Servicio.Comun;
using SGE.Servicio.Vista.Shared;

namespace SGE.Servicio.ServicioInterfaces
{
    public interface IReporteServicio
    {
        IReporteDepLocProgEsta IndexRepDepLocProgEst();

        IReporteDepLocProgEsta GetReporteDepLocProgEstaChangeDep(IReporteDepLocProgEsta vista);

        IReporteDepLocProgEsta GetReporteDepLocProgEsta(IReporteDepLocProgEsta vista);

        byte[] ExportReporteCantidades(IReporteDepLocProgEsta vista);

        byte[] ExportReporteDetalle(int idDepartamento, int idPrograma, int idLocalidad, string estado, int estadoBenef, int tipoprograma);
        IAsistenciaRptVista GetIndexAsistenciasRpt();
        IAsistenciaRptVista GetAsistenciasRpt(string mesPeriodo, int idCurso, int idEscuela, int idEstadoBenf, int idEtapa);
        IAsistenciaRptVista GetAsistenciasRpt(IPager pPager, string mesPeriodo, int idCurso, int idEscuela, int idEstadoBenf, int idEtapa);
        byte[] ExportarAsistencias(string mesPeriodo, int idCurso, int idEscuela, int idEstadoBenf, int idEtapa);
    }
}
