using SGE.Servicio.VistaInterfaces;
using SGE.Servicio.Vista.Shared;

namespace SGE.Servicio.ServicioInterfaces
{
    public interface ISedeServicio
    {
        ISedesVista GetSedes();
        ISedeVista GetSede(int id, string accion);
        ISedesVista GetSedes(string descripcion, int? idEmpresa);        
        ISedesVista GetSedes(Pager pager, string descripcion, int? idEmpresa);
        IEmpresaVista GetSedesByEmpresa(IEmpresaVista model);   
        int AddSede(ISedeVista sede);
        int UpdateSede(ISedeVista sede);
        int DeleteSede(ISedeVista sede);
        bool ExistsSede(int idSede,string descripcion);
        bool SedeTieneFichas(int idSede);
        ISedesVista GetSedesByEmpresa(int idEmpresa);
        int AddSedeFromInscripcion(ISedeVista sede);
    }
}
