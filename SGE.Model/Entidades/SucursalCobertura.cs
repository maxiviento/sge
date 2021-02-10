using System;
using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Entidades
{
    public class SucursalCobertura : ISucursalCobertura
    {
        public int IdTablaBcoCba { get; set; } //idSucursal 
        public int IdLocalidad { get; set; }
        public int IdDepartamento { get; set; }
        public string NombreSucusal { get; set; }
        public string NombreLocalidad { get; set; }
        public string NombreDepartamento { get; set; }
        public string Departamentos { get; set; }
        public string TieneSucursalAsignada { get; set; }

        public IEnumerable<ISucursal> ListaSucursales { get; set; }
        public IEnumerable<IDepartamento> ListaDepartamentos { get; set; }
        public IEnumerable<ILocalidad> ListaLocalidades { get; set; }

        public IEnumerable<ILocalidad> Localidad { get; set; }
    }
}
