using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Repositorio
{
    public interface ILocalidadRepositorio
    {
        IList<ILocalidad> GetLocalidades();
        IList<ILocalidad> GetLocalidadesByDepto(int idDepartamento);
        IList<ILocalidad> GetLocalidades(int skip, int make);
        IList<ILocalidad> GetLocalidades(string descripcion);
        IList<ILocalidad> GetLocalidades(string descripcion, int skip, int take);
        IList<ILocalidad> GetSucursalesforLocalidades();
        ILocalidad GetLocalidad(int id);
        int AddLocalidad(ILocalidad localidad);
        void UpdateLocalidad(ILocalidad localidad);
        int GetLocalidadMaxId();
        int GetLocalidadesCount();


        // Para SucursalCobertura
        IList<ILocalidad> GetSucursalesforLocalidad(int skip, int take);
        IList<ILocalidad> GetSucursalesforLocalidad(int? idSucursal, int? idLocalidad, int? idDepartamento,
                                                    string asignadas, int skip, int take);

        int GetSucursalesCoberturaCount(int? idSucursal, int? idLocalidad, int? idDepartamento,
                                                      string asignadas);

        ILocalidad GetSucursalCoberturaByLocalidad(int? idLocalidad);
        ILocalidad GetSucursalAsignadaForLocalidad(int idLocalidad, int idSucursal);
        int GetSucursalesforLocalidadCount();
        IList<IBarrio> GetBarrios();
    }
}
