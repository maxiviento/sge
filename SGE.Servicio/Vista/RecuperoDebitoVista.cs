using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces;
using SGE.Model.Entidades;
using System;
using SGE.Servicio.VistaInterfaces.Shared;


namespace SGE.Servicio.Vista
{
    public class RecuperoDebitoVista : IRecuperoDebitoVista
    {
        public RecuperoDebitoVista()
        {
         Conceptos = new ComboBox();
         Registros = new List<IRecuperoDebito>();
        }
        public IList<IRecuperoDebito> Registros { get; set; }
        public IPager pager { get; set; }
        public IComboBox Conceptos { get; set; } 
        public string TIPOCONVENIO { get; set; }
        public string SUCURSAL { get; set; }
        public string MONEDA { get; set; }
        public string SISTEMA { get; set; }
        public string NROCTA { get; set; }
        public string IMPORTE { get; set; }
        public string FECHA { get; set; }
         
        public string CBU { get; set; }
        public string CUOTA { get; set; }
        public string USUARIO { get; set; }
         
        public int NROCOMPROBANTE { get; set; }
        public string N_ARCHIVO { get; set; }
        public string N_REGISTRO { get; set; }
         
        public int ID_USR_SIST { get; set; }
        public DateTime FEC_SIST { get; set; }
        public int ID_USR_MODIF { get; set; }
        public DateTime FEC_MODIF { get; set; }
         
        public string Mensaje { get; set; }
         
        public string ArchivoNombre { get; set; }
        public string ArchivoUrl { get; set; }
        public bool ProcesarArchivo { get; set; }
        public int cantProcesados { get; set; }
        public string idEstado { get; set; }
        public string idFormulario { get; set; }
        public int procesados { get; set; }
        public int procesadosFail { get; set; }
        public int idliquidacion { get; set; }

        public DateTime fechaDebito { get; set; }
        public int idconcepto { get; set; }
        public int idrecuperodeb { get; set; }
    }
}
