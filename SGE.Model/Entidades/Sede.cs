using System;
using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Entidades
{
    public class Sede : ISede
    {
        public Sede()
        {
            ListaSedes = new List<ISede>();
        }

        public int IdSede { get; set; }
        public int? IdEmpresa { get; set; }
        public string NombreEmpresa { get; set; }
        public string NombreSede { get; set; }
        public string Calle { get; set; }
        public string Numero { get; set; }
        public string Piso { get; set; }
        public string Dpto { get; set; }
        public string CodigoPostal { get; set; }
        public int? IdLocalidad { get; set; }
        public string ApellidoContacto { get; set; }
        public string NombreContacto { get; set; }
        public string Telefono { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string NombreLocalidad { get; set; }

        public IList<ISede> ListaSedes { get; set; }

        public DateTime? FechaSistema { get; set; }
        public int? IdUsuarioSistema { get; set; }
        public string UsuarioSistema { get; set; }
    }
}
