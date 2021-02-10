using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGE.Model.Entidades.Interfaces
{
    public interface ISucursalCobertura
    {
        int IdTablaBcoCba { get; set; }
        int IdLocalidad { get; set; }
        int IdDepartamento { get; set; }
        string NombreSucusal { get; set; }
        string NombreLocalidad { get; set; }
        string NombreDepartamento { get; set; }
        string Departamentos { get; set; }
        string TieneSucursalAsignada { get; set; }

        IEnumerable<ISucursal> ListaSucursales { get; set; }
        IEnumerable<IDepartamento> ListaDepartamentos { get; set; }
        IEnumerable<ILocalidad> ListaLocalidades { get; set; }
        IEnumerable<ILocalidad> Localidad { get; set; }
    }
}
