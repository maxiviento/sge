using SGE.Servicio.VistaInterfaces;
using SGE.Servicio.Vista.Shared;

namespace SGE.Servicio.ServicioInterfaces
{
    public interface ICausaRechazoServicio
    {
        ICausasRechazoVista GetCausasRechazo(ICausasRechazoVista vista);
        ICausasRechazoVista GetIndex();
        ICausasRechazoVista GetCausasRechazo(string descripcion);
        ICausaRechazoVista GetCausaRechazo(int id, string accion);
        ICausasRechazoVista GetCausasRechazo(Pager pager, int id, string descripcion);
        int AddCausaRechazo(ICausaRechazoVista causaRechazo);
        ICausaRechazoVista AddCausaRechazoLast(string accion);
        int UpdateCausaRechazo(ICausaRechazoVista causaRechazo);
        int DeleteCausaRechazo(ICausaRechazoVista causaRechazo);

        bool CausaRechazoInUse(int idCausaRechazo);
    }
}
