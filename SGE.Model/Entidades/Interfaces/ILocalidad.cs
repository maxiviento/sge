using System.Collections.Generic;

namespace SGE.Model.Entidades.Interfaces
{
    public interface ILocalidad
    {
        int IdLocalidad { get; set; }
        string NombreLocalidad { get; set; }
        int? IdDepartamento { get; set; }
        string NombreDepartamento { get; set; }
        IEnumerable<ISucursal> Sucursales { get; set; }
        bool TieneSucursalAsignada { get; set; }
        IDepartamento Departamento { get; set; }
    }
}
