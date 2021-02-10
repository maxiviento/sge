using System;
using SGE.Servicio.VistaInterfaces;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Servicio.Vista
{
    public class SubprogramaVista : ISubprogramaVista
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Accion { get; set; }
        public string BusquedaAnterior { get; set; }
        [Display(Name = "Monto del subPrograma")]
        [Range(0, 99999999.99, ErrorMessage = "Valor fuera de rango.")]
        public decimal monto { get; set; }

        public short? IdPrograma { get; set; }
        public string Programa { get; set; }


        //MIEMBROS PARA PROCESAR FICHAS
        public int cantProcesados { get; set; }
        public string ExcelNombre { get; set; }
        public string ExcelUrl { get; set; }
        public bool ProcesarExcel { get; set; }
        public IList<IFicha> Fichas { get; set; }
        public string subprograma { get; set; }
        public string paso { get; set; }
        public int idsubprograma { get; set; }
        public string mensaje { get; set; }
        public string Selsubprograma { get; set; }
        public int tipoficha { get; set; }


    }
}
