using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;
using SGE.Servicio.Vista.Shared;

namespace SGE.Servicio.VistaInterfaces
{
    public interface IEmpresasVista
    {
        IList<IEmpresa> Empresas { get; set; }
        IPager Pager { get; set; }
        int Id { get; set; }
        string Descripcion { get; set; }
        string DescripcionBusqueda { get; set; }
        string Cuit { get; set; }
        string CuitBusqueda { get; set; }
        string CodigoActividad { get; set; }
        string DomicilioLaboralIdem { get; set; }
        short? CantidadEmpleados { get; set; }
        string Calle { get; set; }
        string Numero { get; set; }
        string Piso { get; set; }
        string Dpto { get; set; }
        string CodigoPostal { get; set; }

        int IdEmpresaOrigen { get; set; }
        string EmpresaOrigen { get; set; }
        string CuitOrigen { get; set; }

        int IdEmpresaDestino { get; set; }
        string EmpresaDestino { get; set; }
        string CuitDestino { get; set; }

        IEmpresaVista ObjEmpresaOrigen { get; set; }

    }
}
