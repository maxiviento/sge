using SGE.Servicio.VistaInterfaces;
using SGE.Servicio.Vista.Shared;

namespace SGE.Servicio.ServicioInterfaces
{
    public interface ITipoRechazoServicio
    {
        ITiposRechazoVista GetTiposRechazo();
        ITiposRechazoVista GetIndex();
        ITiposRechazoVista GetTiposRechazo(string descripcion);
        ITipoRechazoVista GetTipoRechazo(int id, string accion);
        ITiposRechazoVista GetTiposRechazo(Pager pager, int id, string descripcion);
        int AddTipoRechazo(ITipoRechazoVista tipoRechazo);
        ITipoRechazoVista AddTipoRechazoLast(string accion);
        int UpdateTipoRechazo(ITipoRechazoVista tipoRechazo);
        int DeleteTipoRechazo(ITipoRechazoVista tipoRechazo);
        bool TipoRechazoInUse(int idTipoRechazo);
    }
}
