using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Repositorio
{
    public interface IAsistenciasRepositorio
    {
        IList<IAsistencia> GetAsistencias(int idCurso);
        int AddAsistencias(IList<IAsistencia> asistencias);
        int UpdateAsistencias(IList<IAsistencia> asistencias);
        IList<IAsistencia> GetAsistenciasPeriodo(int idCurso, string mesPeriodo);
        IList<IAsistencia> GetAsistenciasFicha(int idCurso, int idFicha);
        IList<IAsistenciaRpt> GetAsistenciasRpt(string mesPeriodo, int idCurso, int idEscuela, int idEstadoBenf, int idEtapa);
    }
}
