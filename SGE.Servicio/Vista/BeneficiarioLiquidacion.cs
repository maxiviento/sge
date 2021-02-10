using System;
using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces;
using SGE.Servicio.VistaInterfaces.Shared;

namespace SGE.Servicio.Vista
{
    public class BeneficiarioLiquidacion : IBeneficiarioLiquidacion
    {
        public BeneficiarioLiquidacion()
        {
            Conceptos = new List<IConcepto>();
            ComboConceptos = new ComboBox();
            ListaConceptos = new List<IConcepto>();
            ListaApoderados = new List<IApoderado>();
            DniEncontrado = false;
        }
       
        public int IdBeneficiario { get; set; }
        public int IdFicha { get; set; }
        public int IdPrograma { get; set; }
        public int IdEstado { get; set; }
        public IFicha Ficha { get; set; }
        public IConyuge Conyugue { get; set; }
        public string IdConvenio { get; set; }
        public string NumeroConvenio { get; set; }
        public string Residente { get; set; }
        public int Nacionalidad { get; set; }
        public string Email { get; set; }
        public string TipoPersona { get; set; }
        public int CodigoActivacionBancaria { get; set; }
        public int CodigoNaturalezaJuridica { get; set; }
        public string CondicionIva { get; set; }
        public string CodigoSucursal { get; set; }
        public string CodigoMoneda { get; set; }
        public string NombreEstado { get; set; }
        public int? NumeroCuenta { get; set; }
        public decimal ImportePagoPlan { get; set; }
        public string Cbu { get; set; }
        public IList<IConcepto> ListaConceptos { get; set; }
        public string Programa { get; set; }
        public decimal MontoPrograma { get; set; }
        public string TieneApoderado { get; set; }
        public IEnumerable<IConcepto> ConceptosBeneficiario { get; set; }
        public string Modalidad { get; set; }
        public IEnumerable<short> Moneda { get; set; }
        public IList<IConcepto> Conceptos { get; set; }
        public IComboBox ComboConceptos { get; set; }
        public bool Apto { get; set; }
        public bool DniEncontrado { get; set; }
        decimal IBeneficiarioLiquidacion.Test { get; set; }
        public bool Excluido { get; set; }
        public short IdEstadoBenConcLiq { get; set; }
        public string Sucursal { get; set; }
        public string SucursalDescripcion { get; set; }
        public DateTime? FechaSolicitudCuenta { get; set; }
        public short? IdSucursal { get; set; }
        public string ApellidoNombreApoderado { get; set; }
        public IFichaPPP FichaPpp { get; set; }
        public IApoderado Apoderado { get; set; }
        public string ResultadoLiquidacion { get; set; }
        public DateTime? FechaNotificacion { get; set; }
        public bool Notificado { get; set; }
        public int NroCuentadePago { get; set; }
        public string PagoaApoderado { get; set; }
        public string EstadoBeneficiarioConcepto { get; set; }
        public int? IdApoderadoPago { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public int? IdBeneficiarioExport { get; set; }
        public short? IdEstadoExport { get; set; }
        public string Test { get; set; }
        public IEnumerable<IApoderado> ListaApoderados { get; set; }
        public string AltaTemprana { get; set; }
        public DateTime? FechaSistema { get; set; }
        public int? IdUsuarioSistema { get; set; }
        public string UsuarioSistema { get; set; }


        //SE AÑADEN EN LA TABLA T_BENEFICIARIOS LA FECHA DE INICIO Y BAJA DEL BENEFICIO PARA REALIZAR
        //LA LIQUIDACION Y DEJAR DE DEPENDER DE LA FECHA DE NOTIFICACION

        public DateTime? FechaInicioBeneficio { get; set; }
        public DateTime? FechaBajaBeneficio { get; set; }

        // 07/11/2013 - DI CAMPLI LEANDRO - AL CONSULTAR LAS LIQUIDACIONES SE ESTABAN LEVANTANDO EL MONTO DEL PROGRAMA Y NO LO LIQ.
        public int MontoLiquidado { get; set; } // por el momento quedan en int ya que asi está en la tabla

        // 14/05/2014 - DI CAMPLI LEANDRO - VERIFICA SI EL BENEFICIARIO DE CONFIAMOS ES BENEFICIARIO DEL PROGRESAR POR LO CUAL NO LIQUIDA
        public string EsBenfProgresar { get; set; }
        // 14/05/2014 - DI CAMPLI LEANDRO - VERIFICA SI EL BENEFICIARIO TIENE ESCUELA ASIGNADA PARA LIQUIDAR
        public int idEscuela { get; set; }
        // 17/07/2014 - DI CAMPLI LEANDRO - SE AÑADE PARA LA BUSQUEDA POR CONVENIO PARA LA LIQUIDACIÓN
        public string convenio { get; set; }

        // 09/09/2014 - para filtrar por subprograma
        public int? idSubprograma { get; set; }

        public int? TipoPrograma { get; set; }
        public string subprograma { get; set; }

        public int? IdEmpresa { get; set; }
    }
}
