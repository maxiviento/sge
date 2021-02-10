using SGE.Servicio.VistaInterfaces;
using SGE.Servicio.Vista.Shared;

namespace SGE.Servicio.ServicioInterfaces
{
    public interface IDepartamentoServicio
    {
        IDepartamentosVista GetDepartamentos();
        IDepartamentosVista GetIndex();
        IDepartamentosVista GetDepartamentos(string descripcion);
        IDepartamentoVista GetDepartamento(int id, string accion);
        IDepartamentosVista GetDepartamentos(Pager pager, int id, string descripcion);
        int AddDepartamento(IDepartamentoVista depto);
        int UpdateDepartamento(IDepartamentoVista depto);
        int DeleteDepartamento(IDepartamentoVista depto);

    }
}
