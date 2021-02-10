using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces;
using SGE.Model.Entidades;
using System;

namespace SGE.Servicio.Vista
{
    public class RecuperoVista : IRecuperoVista
    {
        public IList<IRecupero> Registros { get; set; }
        public IPager pager { get; set; }

        //NombreArchivo
        public string NombreArchivo { get; set; }
        //FechaProceso
        public DateTime FEC_PROCESO { get; set; }
        //Código de convenio
        public string CONVENIO { get; set; }
        //Grupo de convenio
        public string GRPCONVENIO { get; set; }
        //Numero de referencia
        public int ID_EMPRESA { get; set; }
        //Fecha de vencimiento
        public DateTime FEC_VENCIMIENTO { get; set; }
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

        public string N_REGISTRO { get; set; }

        public int ID_USR_SIST { get; set; }
        public DateTime FEC_SIST { get; set; }
        public int ID_USR_MODIF { get; set; }
        public DateTime FEC_MODIF { get; set; }

        //FechaCobro
        public DateTime FEC_COBRO { get; set; }
        //FechaRendicion
        public DateTime FEC_RENDICION { get; set; }
        //SucursalCobro
        public string SUC_COBRO { get; set; }

        public string ESTADO { get; set; }
        //Fin

        //TipoRegistro
        //FechaProceso
        //CantidadRegistros
        //ImporteControl
        //DigitoVerificador



        public string ArchivoNombre { get; set; }
        public string ArchivoUrl { get; set; }
        public bool ProcesarArchivo { get; set; }
        public int cantProcesados { get; set; }
        public string idEstado { get; set; }
        public string idFormulario { get; set; }
        public int procesados { get; set; }
        public int procesadosFail { get; set; }
        public string Mensaje { get; set; }
    }
}
