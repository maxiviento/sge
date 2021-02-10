using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGE.Model.Entidades.Interfaces
{
    public interface IRecupero
    {
        //NombreArchivo
        string NombreArchivo { get; set; }
        //FechaProceso
        DateTime? FEC_PROCESO { get; set; }
        //Código de convenio
        string CONVENIO { get; set; }
        //Grupo de convenio
        string GRPCONVENIO { get; set; }
        //Numero de referencia
        int ID_CUPON { get; set; }
        //Fecha de vencimiento
        DateTime? FEC_VENCIMIENTO { get; set; }
        //Importe
        decimal IMPORTE { get; set; }
        //Dias hasta 2do. Vencimiento
        string SEG_VENCIMIENTO { get; set; }
        //Porcentaje de Recargo
        string POR_SEG_VENCIMIENTO { get; set; }
        //Dias hasta 3do. Vencimiento
        string TER_VENCIMIENTO { get; set; }
        //Porcentaje de Recargo
        string POR_TER_VENCIMIENTO { get; set; }

        string N_REG_CUPON { get; set; }
        string N_REGISTRO { get; set; }

        int ID_USR_SIST { get; set; }
        DateTime? FEC_SIST { get; set; }
        int ID_USR_MODIF { get; set; }
        DateTime? FEC_MODIF { get; set; }

        //FechaCobro
        DateTime? FEC_COBRO { get; set; }
        //FechaRendicion
        DateTime? FEC_RENDICION { get; set; }
        //SucursalCobro
        string SUC_COBRO { get; set; }

        string ESTADO { get; set; }
        int ID_EMPRESA { get; set; }
        string N_EMPRESA { get; set; }
        string PERIODO { get; set; }
    }
}
