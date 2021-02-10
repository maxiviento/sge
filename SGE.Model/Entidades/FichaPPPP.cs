using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Entidades
{
    public class FichaPPPP : IFichaPPPP
    {
        public int IdFicha { get; set; }
        public IFicha ficha { get; set; }
        public decimal? Promedio { get; set; }
        public DateTime? FechaEgreso { get; set; }
        public short? IdTitulo { get; set; }
        public int? IdEmpresa { get; set; }
        public int? Modalidad { get; set; }
        public string Tareas { get; set; }
        public int? IdSede { get; set; }
        public string DescripcionEmpresa { get; set; }
        public string DescripcionSede { get; set; }
        public string CuitEmpresa { get; set; }
        public IEmpresa Empresa { get; set; }
        public ISede Sede { get; set; }
        public string AltaTemprana { get; set; }
        public DateTime? FechaInicioActividad { get; set; }
        public DateTime? FechaFinActividad { get; set; }
        public int? IdModalidadAFIP { get; set; }
        public string ModalidadAFIP { get; set; }
        public DateTime? FechaNotificacion { get; set; }
        public bool Notificado { get; set; }
    }
}
