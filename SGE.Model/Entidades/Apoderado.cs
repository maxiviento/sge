using System;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Entidades
{
    public class Apoderado : IApoderado
    {
        public Apoderado()
        {
            LocalidadApoderado = new Localidad();
            SucursalApoderado = new Sucursal();
        }

        public int IdApoderado { get; set; }
        public int? IdBeneficiario { get; set; }
        public string Apellido { get; set; }
        public string Nombre { get; set; }
        public int? TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public string Cuil { get; set; }
        public string Sexo { get; set; }
        public short? IdSistema { get; set; }
        public int? NumeroCuentaBco { get; set; }
        public string Cbu { get; set; }
        public string UsuarioBanco { get; set; }
        public short? IdSucursal { get; set; }
        public short? IdMoneda { get; set; }
        public DateTime? ApoderadoDesde { get; set; }
        public DateTime? ApoderadoHasta { get; set; }
        public short? IdEstadoApoderado { get; set; }
        public string NombreEstadoApoderado { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string Calle { get; set; }
        public string Numero { get; set; }
        public string Piso { get; set; }
        public string Dpto { get; set; }
        public string Monoblock { get; set; }
        public string Parcela { get; set; }
        public string Manzana { get; set; }
        public string EntreCalles { get; set; }
        public string Barrio { get; set; }
        public string CodigoPostal { get; set; }
        public int? IdLocalidad { get; set; }
        public string Localidad { get; set; }
        public string TelefonoFijo { get; set; }
        public string TelefonoCelular { get; set; }
        public string Mail { get; set; }
        public DateTime? FechaSolicitudCuenta { get; set; }
        public string CodigoSucursalBanco { get; set; }
        public Localidad LocalidadApoderado { get; set; }
        public int? IdApoderadoNullable { get; set; }
        public ISucursal SucursalApoderado { get; set; }
        public string CodigoMoneda { get; set; }
        public DateTime? FechaSistema { get; set; }
        public int? IdUsuarioSistema { get; set; }
        public string UsuarioSistema { get; set; }
    }
}
