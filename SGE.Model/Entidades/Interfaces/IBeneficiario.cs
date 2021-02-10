using System;
using System.Collections.Generic;

namespace SGE.Model.Entidades.Interfaces
{
    public interface IBeneficiario : IComunDatos
    {

        int IdBeneficiario { get; set; }
        int IdFicha { get; set; }
        int IdPrograma { get; set; }
        int IdEstado { get; set; }
        IFicha Ficha { get; set; }
        IConyuge Conyugue { get; set; }
        string IdConvenio { get; set; }
        string NumeroConvenio { get; set; }
        string Residente { get; set; }
        int Nacionalidad { get; set; }
        string Email { get; set; }
        string TipoPersona { get; set; }
        int CodigoActivacionBancaria { get; set; }
        int CodigoNaturalezaJuridica { get; set; }
        string CondicionIva { get; set; }
        string CodigoSucursal { get; set; }
        string CodigoMoneda { get; set; }
        string NombreEstado { get; set; }
        int? NumeroCuenta { get; set; }
        decimal ImportePagoPlan { get; set; }
        string Cbu { get; set; }
        IList<IConcepto> ListaConceptos { get; set; }
        string Programa { get; set; }
        decimal MontoPrograma { get; set; }
        string TieneApoderado { get; set; }
        IEnumerable<IConcepto> ConceptosBeneficiario { get; set; }
        string Modalidad { get; set; }
        IEnumerable<IApoderado> ListaApoderados { get; set; }
        string AltaTemprana { get; set; }
        bool Excluido { get; set; }
        short IdEstadoBenConcLiq { get; set; }
        string Sucursal { get; set; }
        string SucursalDescripcion { get; set; }
        DateTime? FechaSolicitudCuenta { get; set; }
        short? IdSucursal { get; set; }
        string ApellidoNombreApoderado { get; set; }
        IApoderado Apoderado { get; set; }
        string ResultadoLiquidacion { get; set; }
        DateTime? FechaNotificacion { get; set; }
        bool Notificado { get; set; }


        //Para Liquidacion
        int NroCuentadePago {get;set;}
        string PagoaApoderado { get; set; }
        string EstadoBeneficiarioConcepto { get; set; }
        int? IdApoderadoPago { get; set; }
        DateTime? FechaInicio { get; set; }
        DateTime? FechaFin { get; set; }

        //Para Exportar y si es Null el IdBeneficiario

        int? IdBeneficiarioExport { get; set; }
        short? IdEstadoExport { get; set; }


        //SE AÑADEN EN LA TABLA T_BENEFICIARIOS LA FECHA DE INICIO Y BAJA DEL BENEFICIO PARA REALIZAR
        //LA LIQUIDACION Y DEJAR DE DEPENDER DE LA FECHA DE NOTIFICACION

        DateTime? FechaInicioBeneficio { get; set; }
        DateTime? FechaBajaBeneficio { get; set; }

        // 07/11/2013 - DI CAMPLI LEANDRO - AL CONSULTAR LAS LIQUIDACIONES SE ESTABAN LEVANTANDO EL MONTO DEL PROGRAMA Y NO LO LIQ.
        int MontoLiquidado { get; set; } // por el momento quedan en int ya que asi está en la tabla

        // 14/05/2014 - DI CAMPLI LEANDRO - VERIFICA SI EL BENEFICIARIO DE CONFIAMOS ES BENEFICIARIO DEL PROGRESAR POR LO CUAL NO LIQUIDA
        string EsBenfProgresar { get; set; }

        // 14/05/2014 - DI CAMPLI LEANDRO - VERIFICA SI EL BENEFICIARIO TIENE ESCUELA ASIGNADA PARA LIQUIDAR
        int idEscuela { get; set; }
        // 17/07/2014 - DI CAMPLI LEANDRO - SE AÑADE PARA LA BUSQUEDA POR CONVENIO PARA LA LIQUIDACIÓN
        string convenio { get; set; }

        // 09/09/2014 - para filtrar por subprograma
        int? idSubprograma { get; set; }

        int? TipoPrograma { get; set; }
        string subprograma { get; set; }

        int? IdEmpresa { get; set; }
    }
}
