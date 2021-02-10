using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SGE.Servicio.VistaInterfaces;
using SGE.Servicio.Vista.Shared;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Servicio.ServicioInterfaces
{
    public interface ICursoServicio
    {
        ICursoVista GetCursos(string NombreCurso);
        ICursoVista GetCursos(int NroCurso);
        ICursosVista GetCursos(int NroCurso, string NombreCurso);
        ICursosVista GetIndex();
        ICursosVista GetCursos(IPager pPager, int NroCurso, string NombreCurso);
        ICursoVista GetCurso(int idCurso, string accion);
        int AddCurso(ICursoVista vista);
        int UpdateCurso(ICursoVista vista);
        ICursoVista GetCursoReload(ICursoVista vista);
        IList<IEscuela> TraerEscuelas(string nombre);
        IEscuela TraerEscuela(int idEscuela);
        ICursosVista GetCursosAdm(int NroCurso, string NombreCurso);
        ICursosVista GetCursosAdm();
        ICursosVista GetCursosAdm(IPager pPager, int NroCurso, string NombreCurso);
        ICursoVista GetCurso(int idCurso);
        IList<IFichasCurso> GetFichasCurso(int idcurso);
        int addAsistencias(List<IFichasCurso> fichasCurso);
        IList<IFichasCurso> TraerAsistenciasCurso(int idCurso, string mesPeriodo);
        int UpdateAsistencias(List<IFichasCurso> fichasCurso);
        IList<IFichasCurso>  TraerAsistenciasCurso(List<IFichasCurso> model, int idCurso, string mesPeriodo);
        IList<IFichasCurso> GetFichasSinAsistencia(int idCurso, string mesPeriodo);
        ICursoVista GetCursosEscuela(string NombreCurso);
    }
}
