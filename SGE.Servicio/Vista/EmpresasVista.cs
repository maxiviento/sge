using System.Collections.Generic;
using SGE.Servicio.VistaInterfaces;
using SGE.Servicio.Vista.Shared;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Servicio.Vista
{
    public class EmpresasVista : IEmpresasVista
    {
        public EmpresasVista()
        {
            ObjEmpresaOrigen = new EmpresaVista();
        }

        public IList<IEmpresa> Empresas { get; set; }
        public IPager Pager { get; set; }
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public string DescripcionBusqueda { get; set; }
        public string Cuit { get; set; }
        public string CuitBusqueda { get; set; }
        public string CodigoActividad { get; set; }
        public string DomicilioLaboralIdem { get; set; }
        public short? CantidadEmpleados { get; set; }
        public string Calle { get; set; }
        public string Numero { get; set; }
        public string Piso { get; set; }
        public string Dpto { get; set; }
        public string CodigoPostal { get; set; }

        public int IdEmpresaOrigen { get; set; }
        public string EmpresaOrigen { get; set; }
        public string CuitOrigen { get; set; }

        public int IdEmpresaDestino { get; set; }
        public string EmpresaDestino { get; set; }
        public string CuitDestino { get; set; }
        public IEmpresaVista ObjEmpresaOrigen { get; set; }
    }
}
