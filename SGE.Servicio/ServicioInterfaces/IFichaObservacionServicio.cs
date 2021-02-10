using SGE.Servicio.VistaInterfaces;
using SGE.Servicio.Vista.Shared;
namespace SGE.Servicio.ServicioInterfaces
{
    public interface IFichaObservacionServicio
    {
        IFichaObservacionesVista GetIndex();
        //para delete y update
        IFichaObservacionVista GetObservacion(int idFicha, int idFichaObservacion, string accion);
        //listado
        IFichaObservacionesVista GetObservacionesByFicha(int idFicha);
        //listado-paginado
        IFichaObservacionesVista GetObservacionesByFicha(Pager pager, int idFicha);

        int AddObservacion(IFichaObservacionVista obs);
        int UpdateObservacion(IFichaObservacionVista obs);
        int DeleteObservacion(IFichaObservacionVista obs);
    }
}
