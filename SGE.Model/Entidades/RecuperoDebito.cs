using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SGE.Model.Entidades.Interfaces;


namespace SGE.Model.Entidades
{
    public class RecuperoDebito : IRecuperoDebito
    {
        //public RecuperoDebito()
        //{
        //    detalle = new List<IDetalleRecuDeb>();
        
        //}
        public int ID_LIQUIDACION_RECU { get; set; }
        public int ID_LIQUIDACION { get; set; }
        public int ID_RECUPERO_DEB { get; set; }
        public int? ID_EMPRESA { get; set; }
        public int ID_CONCEPTO { get; set; }
        public string N_REGISTRO { get; set; }
        public string PROCESADO { get; set; }
        public string N_DEVOLUCION { get; set; }
        public string N_ARCHIVO { get; set; }
        public decimal MONTO { get; set; }
        public DateTime? FEC_PROCESO { get; set; }
        public DateTime? FEC_COBRO { get; set; }
        public DateTime? FEC_RENDICION { get; set; }
        public string SUC_COBRO { get; set; }
         
        public int ID_USR_SIST { get; set; }
        public DateTime? FEC_SIST { get; set; }
        public int ID_USR_MODIF { get; set; }
        public DateTime? FEC_MODIF { get; set; }
         
        public string N_EMPRESA { get; set; }
        public string N_CONCEPTO { get; set; }


        //public int ID_DETALLE_REC_DEB { get; set; }
        //public int ID_RECUPERO_DEB { get; set; }
        public int? ID_BENEFICIARIO_CONCEP_LIQ { get; set; }
        public decimal MONTO_PARCIAL { get; set; }
        public int? TIPO_PPP { get; set; }
        public int? EMPLEADOS { get; set; }
        public string COOPERATIVA { get; set; }
        public string CBU { get; set; }
        public string CUENTA { get; set; }
        public string CUENTA_BANCOR { get; set; }
        public string CUIT { get; set; }
        public int? TIPO_CUENTA { get; set; }
        public int? CO_FINANCIAMIENTO { get; set; }

        public DateTime? FEC_LIQ { get; set; }
        public int? NRO_RESOLUCION { get; set; }
        public string N_ESTADO_LIQ { get; set; }
        public string N_PROGRAMA { get; set; }

        //public List<IDetalleRecuDeb> detalle { get; set; }
    }
}
