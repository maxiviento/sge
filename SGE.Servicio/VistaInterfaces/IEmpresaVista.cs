using System.Collections.Generic;
using SGE.Servicio.VistaInterfaces.Shared;
using SGE.Model.Entidades.Interfaces;
using System;

namespace SGE.Servicio.VistaInterfaces
{
    public interface IEmpresaVista
    {
        int Id { get; set; }
        IComboBox Localidades { get; set; }
        IComboBox Usuarios { get; set; }
        string Descripcion { get; set; }
        string Cuit { get; set; }
        string CuitInicio { get; set; }
        string CodigoActividad { get; set; }
        IComboBox DomicilioLaboralIdem { get; set; }
        short? CantidadEmpleados { get; set; }
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
        string Password { get; set; }
        string ConfirmPassword { get; set; }

        IList<ISede> Sedes { get; set; }
        string BusquedaSede { get; set; }

        IList<IFicha> ListadoFichasPpp { get; set; }
        string BusquedaFicha { get; set; }
        string FichaEncontrada { get; set; }

        string Accion { get; set; }

        string OrigenLlamada { get; set; }

        IEmpresa EmpresaOrigen { get; set; }
        string Legenda { get; set; }
        string TabSeleccionado { get; set; }
        int IdLocalidad { get; set; }
        int IdDomicilioLaboral { get; set; }

        string NombreLocalidad { get; set; }
        string NombreDomicilioLaboralSeleccionado { get; set; }

        string LoginUsuario { get; set; }
        string celular { get; set; }
        string verificada { get; set; }
        string mailEmp { get; set; }
        int cantBenef { get; set; }
        int cantBenDisc { get; set; }

        string EsCooperativa { get; set; }
        string CBU { get; set; }
        string CUENTA { get; set; }
        int? TIPOCUENTA { get; set; }
        DateTime? FEC_ADHESION { get; set; }

    }
}
