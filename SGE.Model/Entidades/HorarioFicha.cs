using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Entidades
{
    public class HorarioFicha : IHorarioFicha
    {
        public DateTime? FechaSistema { get; set; }
        public int? IdUsuarioSistema { get; set; }
        public string UsuarioSistema { get; set; }
        public int IdFicha { get; set; }
        public int IdEmpresaHorario { get; set; }
        public short Corte { get; set; }
        public string Observacion { get; set; }
        public string DSemana { get; set; }

        public int IdEmpresa { get; set; }

        public int? Horadesde { get; set; }
        public int? Minutosdesde { get; set; }
        public int? HoraHasta { get; set; }
        public int? MinutosHasta { get; set; }
        
    }
}
