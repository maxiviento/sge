using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGE.Model.Entidades.Interfaces
{
    public interface IRecuperoDebito
    {
        int ID_LIQUIDACION_RECU { get; set; }
        int ID_LIQUIDACION { get; set; }
        int ID_RECUPERO_DEB { get; set; }
        int? ID_EMPRESA { get; set; }
        int ID_CONCEPTO { get; set; }
        string N_REGISTRO { get; set; }
        string PROCESADO { get; set; }
        string N_DEVOLUCION { get; set; }
        string N_ARCHIVO { get; set; }
        decimal MONTO { get; set; }
        DateTime? FEC_PROCESO { get; set; }
        DateTime? FEC_COBRO { get; set; }
        DateTime? FEC_RENDICION { get; set; }
        string SUC_COBRO { get; set; }
        
        int ID_USR_SIST { get; set; }
        DateTime? FEC_SIST { get; set; }
        int ID_USR_MODIF { get; set; }
        DateTime? FEC_MODIF { get; set; }

        string N_EMPRESA { get; set; }
        string N_CONCEPTO { get; set; }
        //IList<IDetalleRecuDeb> detalle { get; set; }

        //public int ID_DETALLE_REC_DEB { get; set; }
        //public int ID_RECUPERO_DEB { get; set; }
        int? ID_BENEFICIARIO_CONCEP_LIQ { get; set; }
        decimal MONTO_PARCIAL { get; set; }
        int? TIPO_PPP { get; set; }
        int? EMPLEADOS { get; set; }
        string COOPERATIVA { get; set; }
        string CBU { get; set; }
        string CUENTA { get; set; }
        string CUENTA_BANCOR { get; set; }
        string CUIT { get; set; }
        int? TIPO_CUENTA { get; set; }
        int? CO_FINANCIAMIENTO { get; set; }

        DateTime? FEC_LIQ { get; set; }
        int? NRO_RESOLUCION { get; set; }
        string N_ESTADO_LIQ { get; set; }
        string N_PROGRAMA { get; set; }


    }
}
