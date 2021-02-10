using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SGE.Model.Entidades.Interfaces;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces;
using SGE.Servicio.VistaInterfaces.Shared;

namespace SGE.Servicio.Vista
{
    public class EtapaVista
    {

        public EtapaVista()
        { 
         Programas = new ComboBox();
         ProcesarExcel = false;
         etapas = new ComboBox();
        }

        public IList<IEtapa> listEtapas { get; set; }

        public int ID_ETAPA { get; set; }
        [Display(Name = "*Programa")]
        [Required(ErrorMessage = "Debes seleccionar un programa.")]
        public int ID_PROGRAMA { get; set; }
        [Display(Name = "*Etapa")]
        [Required(ErrorMessage = "Debes ingresar la descripción de la etapa.")]
        public string N_ETAPA { get; set; }
        [Display(Name = "*Monto Etapa")]
        //[Required(ErrorMessage = "Debes ingresar un monto o '0'.")]
        public decimal? MONTO_ETAPA { get; set; }
        [Display(Name = "*Ejercicio")]
        [Required(ErrorMessage = "Debes ingresar el año del ejercicio.")]
        public string EJERCICIO { get; set; }
        [Display(Name = "*Fec Inicio")]
        [Required(ErrorMessage = "Debes ingresar la fecha de inicio.")]
        public DateTime FEC_INCIO { get; set; }
        [Display(Name = "*Fec Fin")]
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
        public int etapaSele { get; set; }
        public int programaSel { get; set; }
        public string nprograma { get; set; }
        public string mensaje { get; set; }
        public string progseleccionado { get; set; }
    }
}
