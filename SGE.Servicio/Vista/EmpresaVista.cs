using System.Collections.Generic;
using SGE.Model.Entidades;
using SGE.Model.Entidades.Interfaces;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces;
using System.ComponentModel.DataAnnotations;
using SGE.Servicio.VistaInterfaces.Shared;
using System.Web.Mvc;
using SGE.Servicio.Comun;
using System;

namespace SGE.Servicio.Vista
{
    public class EmpresaVista : IEmpresaVista
    {

        public EmpresaVista()
        {
            Localidades = new ComboBox();
            Usuarios = new ComboBox();
            DomicilioLaboralIdem = new ComboBox();
            Sedes = new List<ISede>();
            ListadoFichasPpp = new List<IFicha>();
            FichaEncontrada = "N";
            EmpresaOrigen = new Empresa();
            TabSeleccionado = "0";
        }

        [Key]
        [Display(Name = "Id:")]
        public int Id { get; set; }

        [Display(Name = "*Localidades")]
        public IComboBox Localidades { get; set; }

        [Display(Name = "*Usuarios")]
        public IComboBox Usuarios { get; set; }

        [Display(Name = "*Descripción")]
        [Required(ErrorMessage = "Debe ingresar la descripción.")]
        [StringLength(200, ErrorMessage = "Descripción supera los 200 caracteres.")]
        [ValidationGroup("Empresa")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "Ingrese el CUIT.")]
        [Display(Name = "*CUIT")]
        [StringLength(13, ErrorMessage = "CUIT supera los 13 caracteres.")]
        [ValidationGroup("Empresa")]
        public string Cuit { get; set; }

        public string CuitInicio { get; set; }

        [Display(Name = "*Código de Actividad")]
        [StringLength(8, ErrorMessage = "Código de Actividad supera los 8 caracteres.")]
        public string CodigoActividad { get; set; }

        [Display(Name = "*Domicilio Laboral Idem")]
        public IComboBox DomicilioLaboralIdem { get; set; }

        [Display(Name = "*Cantidad de Empleados")]
        public short? CantidadEmpleados { get; set; }

        [Display(Name = "*Calle")]
        [StringLength(200, ErrorMessage = "Calle supera los 200 caracteres.")]
        public string Calle { get; set; }

        [Display(Name = "*Número")]
        [StringLength(10, ErrorMessage = "Número supera los 10 caracteres.")]
        public string Numero { get; set; }


        [Display(Name = "*Piso")]
        [StringLength(10, ErrorMessage = "Piso supera los 10 caracteres.")]
        public string Piso { get; set; }

        [Display(Name = "*Departamento")]
        [StringLength(10, ErrorMessage = "Departamento supera los 10 caracteres.")]
        public string Dpto { get; set; }

        [Display(Name = "*Código Postal")]
        [StringLength(10, ErrorMessage = "Código Postal supera los 10 caracteres.")]
        public string CodigoPostal { get; set; }

        public string Accion { get; set; }

        public int? IdUsuario { get; set; }

        [Required(ErrorMessage = "Ingrese el Nombre del Usuario")]
        [ValidationGroup("AgregarUsuario")]
        public string NombreUsuario { get; set; }
        [Required(ErrorMessage = "Ingrese el Apellido del Usuario")]
        [ValidationGroup("AgregarUsuario")]
        public string ApellidoUsuario { get; set; }

        [Display(Name = "*CUIL")]
        [StringLength(13, ErrorMessage = "CUIL supera los 13 caracteres.")]
        [Required(ErrorMessage = "Ingrese el CUIL.")]
        [ValidationGroup("AgregarUsuario")]
        public string Cuil { get; set; }

        public string Telefono { get; set; }

        [ValidateEmail]
        public string Mail { get; set; }

        [Display(Name = "Clave")]
        [StringLength(30, ErrorMessage = "Mínimo {2} caracteres - Máximo {1} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Clave Confirmación")]
        [StringLength(30, ErrorMessage = "Mínimo {2} caracteres - Máximo {1} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "La clave y la confirmación de la clave no coinciden.")]
        public string ConfirmPassword { get; set; }

        public IList<ISede> Sedes { get; set; }
        public IList<IFicha> ListadoFichasPpp { get; set; }
        public string BusquedaSede { get; set; }

        public string BusquedaFicha { get; set; }

        public string FichaEncontrada { get; set; }

        public string OrigenLlamada { get; set; }
        public IEmpresa EmpresaOrigen { get; set; }
        public string Legenda { get; set; }
        public string TabSeleccionado { get; set; }
        public int IdLocalidad { get; set; }
        public int IdDomicilioLaboral { get; set; }
        public string NombreLocalidad { get; set; }
        public string NombreDomicilioLaboralSeleccionado { get; set; }
        [Display(Name = "Login de Usuario")]
        public string LoginUsuario { get; set; }

        public string celular { get; set; }
        public string verificada { get; set; }
        [ValidateEmail]
        public string mailEmp { get; set; }

        public int cantBenef { get; set; }
        public int cantBenDisc { get; set; }

        public string EsCooperativa { get; set; }

        public string CBU { get; set; }
        public string CUENTA { get; set; }
        public int? TIPOCUENTA { get; set; }
        public DateTime? FEC_ADHESION { get; set; }
    }
}
