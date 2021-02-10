using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Entidades
{
    public class RecuperoDebEmpresa : IRecuperoDebEmpresa
    {
        public RecuperoDebEmpresa()
        {
            detalle = new List<DetalleRecuperoEmpresa>();
        }
        public int ID_EMPRESA_DEB { get; set; }
        public int ID_RECUPERO_DEB { get; set; }
        public int? ID_EMPRESA { get; set; }
        public int? ID_CONCEPTO { get; set; }
        public string N_REGISTRO { get; set; }
        public string PROCESADO { get; set; }
        public string DEVOLUCION { get; set; }
        public string N_ARCHIVO { get; set; }
        public decimal? MONTO { get; set; }
        public DateTime? FEC_PROCESO { get; set; }
        public DateTime? FEC_COBRO { get; set; }
        public DateTime? FEC_RENDICION { get; set; }
        public string SUC_COBRO { get; set; }
        public int? ID_USR_SIST { get; set; }
        public DateTime? FEC_SIST { get; set; }
        public int? ID_USR_MODIF { get; set; }
        public DateTime? FEC_MODIF { get; set; }

        public string CBU { get; set; }
        public string CUENTA { get; set; }
        public string CUENTA_BANCOR { get; set; }
        public string CUIT { get; set; }
        public string N_EMPRESA { get; set; }
        public int ID_LIQUIDACION_REC { get; set; }
        public int ID_LIQUIDACION { get; set; }
        public string COOPERATIVA { get; set; }
        public int? EMPLEADOS { get; set; }
        public int? TIPO_CUENTA { get; set; }
        public List<DetalleRecuperoEmpresa> detalle { get; set; }

    }
}
