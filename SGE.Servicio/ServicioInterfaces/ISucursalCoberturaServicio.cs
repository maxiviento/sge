using System.Web;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces;
namespace SGE.Servicio.ServicioInterfaces
{
    public interface ISucursalCoberturaServicio
    {
        ISucursalesCoberturaVista GetSucursalesCobertura();
        ISucursalesCoberturaVista GetIndex();
        ISucursalesCoberturaVista GetSucursalesCobertura(int? idSucursal, int? idLocalidad, int? idDepartamento, string asignadas);
        ISucursalesCoberturaVista GetSucursalesCobertura(Pager pager, int? idSucursal, int? idLocalidad, int? idDepartamento, string asignadas);
        ISucursalCoberturaVista GetSucursalCoberturaByLocalidad(int idLocalidad, string accion);
        int UpdateSucursalCobertura(ISucursalCoberturaVista sucursalCobertura);

        void AddLocalidadSucursal(ISucursalCoberturaVista sucursalCobertura);
        void DeleteLocalidadSucursal(ISucursalCoberturaVista sucursalCobertura);

        ISucursalesCoberturaVista GetSucursalesCobertura(Vista.SucursalesCoberturaVista model);
    }
}
