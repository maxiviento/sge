using System;
using System.ComponentModel.DataAnnotations;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces;
using SGE.Servicio.VistaInterfaces.Shared;

namespace SGE.Servicio.Vista
{
    public class ApoderadoVista: IApoderadoVista
    {
        public ApoderadoVista()
        {
            Localidades = new ComboBox();
            EstadosApoderados = new ComboBox();
            Sexo = new ComboBox();
            TipoDocumento = new ComboBox();
            Monedas = new ComboBox();
            Sucursales = new ComboBox();
            Sistemas = new ComboBox();
        }

        [Key]
        [Display(Name = "Id:")]
        public int IdApoderado { get; set; }
        
        public int? IdBeneficiario { get; set; }

        [Display(Name = "*Apellido")]
        [StringLength(100, ErrorMessage = "Apellido supera los 100 caracteres.")]
        [Required(ErrorMessage="Ingrese el Apellido")]
        public string Apellido { get; set; }

        [Display(Name = "*Nombre")]
        [StringLength(100, ErrorMessage = "Nombre supera los 100 caracteres.")]
        [Required(ErrorMessage = "Ingrese el Nombre")]
        public string Nombre { get; set; }

        public DateTime? FechaNacimiento { get; set; }
        public DateTime? FechaSolicitudCuenta { get; set; }
        public bool RealizoElCambio { get; set; }
        public IComboBox Sexo { get; set; }
        public IComboBox TipoDocumento { get; set; }

        [Range(0, 9999999999999999999, ErrorMessage = "Valor incorrecto para el Número de Documento.")]
        [Required(ErrorMessage = "Ingrese el Número de Documento.")]
        public string NumeroDocumento { get; set; }

        [Display(Name = "*CUIL")]
        [StringLength(13, ErrorMessage = "CUIL supera los 13 caracteres.")]
        [Required(ErrorMessage = "Ingrese el CUIL.")]
        public string Cuil { get; set; }
        
        public short? IdSistema { get; set; }
        public IComboBox Sistemas { get; set; }
        public int? NumeroCuentaBco { get; set; }
        public string Cbu { get; set; }
        public string UsuarioBanco { get; set; }


        public short? IdSucursal { get; set; }
        public IComboBox Sucursales { get; set; }
        public short? IdMoneda { get; set; }
        public IComboBox Monedas { get; set; }

        public DateTime? ApoderadoDesde { get; set; }
        public DateTime? ApoderadoHasta { get; set; }
        public short? IdEstadoApoderado { get; set; }

        public IComboBox EstadosApoderados { get; set; }

        public string TelefonoFijo { get; set; }
        public string TelefonoCelular { get; set; }

        public string Mail { get; set; }

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


        [Display(Name = "*Localidades")]
        public IComboBox Localidades { get; set; }
        public int IdLocalidad { get; set; }

        public string Accion { get; set; }
        public string AccionLlamada { get; set; }
    }
}
