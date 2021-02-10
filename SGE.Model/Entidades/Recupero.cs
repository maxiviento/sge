using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SGE.Model.Entidades.Interfaces;
namespace SGE.Model.Entidades
{
    public class Recupero : IRecupero
    {
        //NombreArchivo
        public string NombreArchivo { get; set; }
        //FechaProceso
        public DateTime? FEC_PROCESO { get; set; }
        //Código de convenio
        public string CONVENIO { get; set; }
        //Grupo de convenio
        public string GRPCONVENIO { get; set; }
        //Numero de referencia
        public int ID_CUPON { get; set; }
        //Fecha de vencimiento
        public DateTime? FEC_VENCIMIENTO { get; set; }
        //Importe
        public decimal IMPORTE { get; set; }
        //Dias hasta 2do. Vencimiento
        public string SEG_VENCIMIENTO { get; set; }
        //Porcentaje de Recargo
        public string POR_SEG_VENCIMIENTO { get; set; }
        //Dias hasta 3do. Vencimiento
        public string TER_VENCIMIENTO { get; set; }
        //Porcentaje de Recargo
        public string POR_TER_VENCIMIENTO { get; set; }

        public string N_REG_CUPON { get; set; }
        public string N_REGISTRO { get; set; }

        public int ID_USR_SIST { get; set; }
        public DateTime? FEC_SIST { get; set; }
        public int ID_USR_MODIF { get; set; }
        public DateTime? FEC_MODIF { get; set; }

        //FechaCobro
        public DateTime? FEC_COBRO { get; set; }
        //FechaRendicion
        public DateTime? FEC_RENDICION { get; set; }
        //SucursalCobro
        public string SUC_COBRO { get; set; }

        public string ESTADO { get; set; }
        public int ID_EMPRESA { get; set; }
        public string N_EMPRESA { get; set; }
        public string PERIODO { get; set; }
    }
}
