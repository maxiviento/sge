using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces.Shared;
using System;
namespace SGE.Servicio.VistaInterfaces
{
    public interface IRecuperoVista
    {
        IList<IRecupero> Registros { get; set; }
        IPager pager { get; set; }

        //NombreArchivo
        string NombreArchivo { get; set; }
        //FechaProceso
        DateTime FEC_PROCESO { get; set; }
        //Código de convenio
        string CONVENIO { get; set; }
        //Grupo de convenio
        string GRPCONVENIO { get; set; }
        //Numero de referencia
        int ID_EMPRESA { get; set; }
        //Fecha de vencimiento
        DateTime FEC_VENCIMIENTO { get; set; }
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

        string N_REGISTRO { get; set; }

        int ID_USR_SIST { get; set; }
        DateTime FEC_SIST { get; set; }
        int ID_USR_MODIF { get; set; }
        DateTime FEC_MODIF { get; set; }

        //FechaCobro
        DateTime FEC_COBRO { get; set; }
        //FechaRendicion
        DateTime FEC_RENDICION { get; set; }
        //SucursalCobro
        string SUC_COBRO { get; set; }

        string ESTADO { get; set; }
        string Mensaje { get; set; }

//Fin

//TipoRegistro
//FechaProceso
//CantidadRegistros
//ImporteControl
//DigitoVerificador



        string ArchivoNombre { get; set; }
        string ArchivoUrl { get; set; }
        bool ProcesarArchivo { get; set; }
        int cantProcesados { get; set; }
        string idEstado { get; set; }
        string idFormulario { get; set; }
        // 08/01/2018
        int procesados { get; set; }
        int procesadosFail { get; set; }

    }
}
