using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces.Shared;
using System;

namespace SGE.Servicio.VistaInterfaces
{
    public interface IRecuperoDebitoVista
    {
        IList<IRecuperoDebito> Registros { get; set; }
        IPager pager { get; set; }
        IComboBox Conceptos { get; set; }
        string TIPOCONVENIO { get; set; }
        string SUCURSAL { get; set; }
        string MONEDA { get; set; }
        string SISTEMA { get; set; }
        string NROCTA { get; set; }
        string IMPORTE { get; set; }
        string FECHA { get; set; }

        string CBU { get; set; }
        string CUOTA { get; set; }
        string USUARIO { get; set; }

        int NROCOMPROBANTE { get; set; }
        string N_ARCHIVO { get; set; }
        string N_REGISTRO { get; set; }

        int ID_USR_SIST { get; set; }
        DateTime FEC_SIST { get; set; }
        int ID_USR_MODIF { get; set; }
        DateTime FEC_MODIF { get; set; }

        string Mensaje { get; set; }

        string ArchivoNombre { get; set; }
        string ArchivoUrl { get; set; }
        bool ProcesarArchivo { get; set; }
        int cantProcesados { get; set; }
        string idEstado { get; set; }
        string idFormulario { get; set; }
        int procesados { get; set; }
        int procesadosFail { get; set; }
        int idliquidacion { get; set; }

        DateTime fechaDebito { get; set; }
        int idconcepto { get; set; }
        int idrecuperodeb { get; set; }

        
    }
}
