using System;
using SGE.Servicio.VistaInterfaces.Shared;

namespace SGE.Servicio.VistaInterfaces
{
    public interface IApoderadoVista
    {
        int IdApoderado { get; set; }
        int? IdBeneficiario { get; set; }
        string Apellido { get; set; }
        string Nombre { get; set; }
        IComboBox TipoDocumento { get; set; }
        string NumeroDocumento { get; set; }
        string Cuil { get; set; }
        IComboBox Sexo { get; set; }

        short? IdSistema { get; set; }
        IComboBox Sistemas { get; set; }

        int? NumeroCuentaBco { get; set; }
        string Cbu { get; set; }
        string UsuarioBanco { get; set; }

        short? IdSucursal { get; set; }
        IComboBox Sucursales { get; set; }

        short? IdMoneda { get; set; }
        IComboBox Monedas { get; set; }

        DateTime? ApoderadoDesde { get; set; }
        DateTime? ApoderadoHasta { get; set; }

        short? IdEstadoApoderado { get; set; }
        IComboBox EstadosApoderados { get; set; }

        DateTime? FechaNacimiento { get; set; }
        DateTime? FechaSolicitudCuenta { get; set; }
        bool RealizoElCambio { get; set; }

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
        int IdLocalidad { get; set; }
        IComboBox Localidades { get; set; }
        string TelefonoFijo { get; set; }
        string TelefonoCelular { get; set; }
        string Mail { get; set; }

        string Accion { get; set; }
        string AccionLlamada { get; set; }
    }
}
