using System.Collections.Generic;

namespace SGE.Model.Entidades.Interfaces
{
    public interface ISede : IComunDatos
    {
        int IdSede { get; set; }
        int? IdEmpresa { get; set; }
        string NombreEmpresa { get; set; }
        string NombreSede { get; set; }
        string Calle { get; set; }
        string Numero { get; set; }
        string Piso { get; set; }
        string Dpto { get; set; }
        string CodigoPostal { get; set; }
        int? IdLocalidad { get; set; }
        string ApellidoContacto { get; set; }
        string NombreContacto { get; set; }
        string Telefono { get; set; }
        string Fax { get; set; }
        string Email { get; set; }
        string NombreLocalidad { get; set; }

        IList<ISede> ListaSedes { get; set; }
    }
}
