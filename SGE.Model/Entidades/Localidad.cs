using System;
using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Entidades
{
    public class Localidad : ILocalidad
    {
        public Localidad()
        {
            Departamento = new Departamento();
        }

        public int IdLocalidad { get; set; }
        public string NombreLocalidad { get; set; }
        public int? IdDepartamento { get; set; }
        public string NombreDepartamento { get; set; }
        public IEnumerable<ISucursal> Sucursales { get; set; }
        public bool TieneSucursalAsignada { get; set; }
        public IDepartamento Departamento { get; set; }
    }
}
