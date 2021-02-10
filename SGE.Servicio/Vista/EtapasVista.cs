using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SGE.Model.Entidades.Interfaces;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces;
using SGE.Servicio.VistaInterfaces.Shared;


namespace SGE.Servicio.Vista
{
    public class EtapasVista
    {

        public EtapasVista()
        { 
         Programas = new ComboBox();
         ProcesarExcel = false;
         etapas = new ComboBox();
        }

        public IList<IEtapa> listEtapas { get; set; }

        public int ID_ETAPA { get; set; }
        public int ID_PROGRAMA { get; set; }
        public string N_ETAPA { get; set; }
        public decimal? MONTO_ETAPA { get; set; }
        public string EJERCICIO { get; set; }
        public DateTime FEC_INCIO { get; set; }
        public DateTime? FEC_FIN { get; set; }
        public int? ID_USR_SIST { get; set; }
        public DateTime? FEC_SIST { get; set; }
        public int? ID_USR_MODIF { get; set; }
        public DateTime? FEC_MODIF { get; set; }

        public IComboBox Programas { get; set; }


        //FILTROS DE BUSQUEDA
        public string ejerc { get; set; }


        //ACCIONES

        public string Accion { get; set; }


        //MIEMBROS PARA PROCESAR FICHAS
        public int cantProcesados { get; set; }
        public string ExcelNombre { get; set; }
        public string ExcelUrl { get; set; }
        public bool ProcesarExcel { get; set; }
        public IList<IFicha> Fichas { get; set; }
        public IComboBox etapas { get; set; }
        public string paso { get; set; }
        public int filtroEtapa { get; set; }
        public int programaSel { get; set; }
        public string nprograma { get; set; }
        public string mensaje { get; set; }
        public string filtroEjerc { get; set; }
        public string filtroNombre {get;set;}
        public bool filtroUrl { get; set; }


    }
}
