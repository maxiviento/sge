using System;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Entidades
{
    public class Empresa: IEmpresa
    {
        public Empresa()
        {
            Usuario = new UsuarioEmpresa();
            LocalidadEmpresa = new Localidad();
        }

        public int IdEmpresa { get; set; }
        public int? IdLocalidad { get; set; }
        public string NombreEmpresa { get; set; }
        public string Cuit { get; set; }
        public string CodigoActividad { get; set; }
        public string DomicilioLaboralIdem { get; set; }
        public int? CantidadEmpleados { get; set; }
        public string Calle { get; set; }
        public string Numero { get; set; }
        public string Piso { get; set; }
        public string Dpto { get; set; }
        public string CodigoPostal { get; set; }

        public int? IdUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public string ApellidoUsuario { get; set; }
        public string Cuil { get; set; }
        public string Telefono { get; set; }
        public string Mail { get; set; }
        public string LoginUsuario { get; set; }
        public string PasswordUsuario { get; set; }

        public string BusquedaSede { get; set; }
        public string BusquedaFicha { get; set; }

        public string NombreLocalidad { get; set; }
        public IUsuarioEmpresa Usuario { get; set; }
        public ILocalidad LocalidadEmpresa { get; set; }

        public DateTime? FechaSistema { get; set; }
        public int? IdUsuarioSistema { get; set; }
        public string UsuarioSistema { get; set; }
        public string celular { get; set; }
        public string verificada { get; set; }
        public string mailEmp { get; set; }

        public string EsCooperativa { get; set; }

        public string CBU { get; set; }
        public string CUENTA { get; set; }
        public int? TIPOCUENTA { get; set; }
        public string CUENTA_BANCOR { get; set; }
        public DateTime? FEC_ADHESION { get; set; }
    }
}
