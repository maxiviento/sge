using System;
namespace SGE.Model.Entidades.Interfaces
{
    public interface IEmpresa : IComunDatos
    {
        int IdEmpresa { get; set; }
        int? IdLocalidad { get; set; }        
        string NombreEmpresa { get; set; }
        string Cuit { get; set; }
        string CodigoActividad { get; set; }
        string DomicilioLaboralIdem { get; set; }
        int? CantidadEmpleados { get; set; }
        string Calle { get; set; }
        string Numero { get; set; }
        string Piso { get; set; }
        string Dpto { get; set; }
        string CodigoPostal { get; set; }

        int? IdUsuario { get; set; }
        string NombreUsuario { get; set; }
        string ApellidoUsuario { get; set; }
        string Cuil { get; set; }
        string Telefono { get; set; }
        string Mail { get; set; }
        string LoginUsuario { get; set; }
        string PasswordUsuario { get; set; }

        string BusquedaSede { get; set; }
        string BusquedaFicha { get; set; }
        string NombreLocalidad { get; set; }

        IUsuarioEmpresa Usuario { get; set; }
        ILocalidad LocalidadEmpresa { get; set; }
        string celular { get; set; }
        string verificada { get; set; }
        string mailEmp { get; set; }
        string EsCooperativa { get; set; }
        string CBU { get; set; }
        string CUENTA { get; set; }
        int? TIPOCUENTA { get; set; }
        string CUENTA_BANCOR { get; set; }
        DateTime? FEC_ADHESION { get; set; }
    }
}
