using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Entidades
{
    public class HorarioEmpresa : IHorarioEmpresa
    {
        public DateTime? FechaSistema { get; set; }
        public int? IdUsuarioSistema { get; set; }
        public string UsuarioSistema { get; set; }
        public int IdEmpresaHorario { get; set; }
        public int IdEmpresa { get; set; }
        public string IdDia { get; set; }
        public short HoraDesde { get; set; }
        public short MinutosDesde { get; set; }
        public short HoraHasta { get; set; }
        public short MinutosHasta { get; set; }
    }
}
