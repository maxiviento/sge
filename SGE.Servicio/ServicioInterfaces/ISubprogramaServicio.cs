using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces;
using SGE.Model.Entidades;
using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;
using SGE.Servicio.Vista;
using System.Web;

namespace SGE.Servicio.ServicioInterfaces
{
    public interface ISubprogramaServicio
    {
        ISubprogramasVista GetSubprogramas();
        ISubprogramasVista GetIndex();
        ISubprogramasVista GetSubprogramas(string descripcion);
        ISubprogramaVista GetSubprograma(int id, string accion);
        ISubprogramasVista GetSubprogramas(Pager pager, int id, string descripcion);
        int AddSubprograma(ISubprogramaVista subprograma);
        int UpdateSubprograma(ISubprogramaVista subprograma);
        List<Subprograma> GetSubprogramasRol(int idPrograma);
        IList<ISubprograma> GetSubprogramasN(string nombre);
        List<Subprograma> GetSubprogramas(int idPrograma);
        SubprogramaVista SubirArchivoFichas(HttpPostedFileBase archivo, SubprogramaVista model);
        IFichasVista CargarFichasSubprogXLS(string ExcelUrl, int idprograma);
        int udpSubprogramaFichas(SubprogramaVista model);
        SubprogramaVista getFichas(SubprogramaVista model);
    }
}
