using System;

namespace SGE.Model.Entidades.Interfaces
{
    public interface IApoderado : IComunDatos
    {
        int IdApoderado { get; set; }
        int? IdBeneficiario { get; set; }
        string Apellido { get; set; }
        string Nombre { get; set; }
        int? TipoDocumento { get; set; }
        string NumeroDocumento { get; set; }
        string Cuil { get; set; }
        string Sexo { get; set; }
        short? IdSistema { get; set; }
        int? NumeroCuentaBco { get; set; }
        string Cbu { get; set; }
        string UsuarioBanco { get; set; }
        short? IdSucursal { get; set; }
        short? IdMoneda { get; set; }
        DateTime? ApoderadoDesde { get; set; }
        DateTime? ApoderadoHasta { get; set; }
        short? IdEstadoApoderado { get; set; }
        string NombreEstadoApoderado { get; set; }
        DateTime? FechaNacimiento { get; set; }
        string Calle { get; set; }
        string Numero { get; set; }
        string Piso { get; set; }
        string Dpto { get; set; }
        string Monoblock { get; set; }
        string Parcela { get; set; }
        string Manzana { get; set; }
        string EntreCalles { get; set; }
        string Barrio { get; set; }
        string CodigoPostal { get; set; }
        int? IdLocalidad { get; set; }
        string Localidad { get; set; }
        string TelefonoFijo { get; set; }
        string TelefonoCelular { get; set; }
        string Mail { get; set; }
        DateTime? FechaSolicitudCuenta { get; set; }
        string CodigoSucursalBanco { get; set; }
        Localidad LocalidadApoderado { get; set; }
        int? IdApoderadoNullable { get; set; }
        ISucursal SucursalApoderado { get; set; }
        string CodigoMoneda { get; set; }

    }
}
