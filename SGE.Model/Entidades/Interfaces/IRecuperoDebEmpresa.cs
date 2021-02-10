using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGE.Model.Entidades.Interfaces
{
    public interface IRecuperoDebEmpresa
    {
        int ID_EMPRESA_DEB { get; set; }
        int ID_RECUPERO_DEB { get; set; }
        int? ID_EMPRESA { get; set; }
        int? ID_CONCEPTO { get; set; }
        string N_REGISTRO { get; set; }
        string PROCESADO { get; set; }
        string DEVOLUCION { get; set; }
        string N_ARCHIVO { get; set; }
        decimal? MONTO { get; set; }
        DateTime? FEC_PROCESO { get; set; }
        DateTime? FEC_COBRO { get; set; }
        DateTime? FEC_RENDICION { get; set; }
        string SUC_COBRO { get; set; }
        int? ID_USR_SIST { get; set; }
        DateTime? FEC_SIST { get; set; }
        int? ID_USR_MODIF { get; set; }
        DateTime? FEC_MODIF { get; set; }
        string CBU { get; set; }
        string CUENTA { get; set; }
        string CUENTA_BANCOR { get; set; }
        string CUIT { get; set; }
        string N_EMPRESA { get; set; }
        int ID_LIQUIDACION_REC { get; set; }
        int ID_LIQUIDACION { get; set; }
        string COOPERATIVA { get; set; }
        int? EMPLEADOS { get; set; }
        int? TIPO_CUENTA { get; set; }
        List<DetalleRecuperoEmpresa> detalle { get; set; }

    }
}
