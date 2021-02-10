using System;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Entidades
{
    public class FichaRechazo : IFichaRechazo
    {
        public int IdFicha { get; set; }
        public int IdTipoRechazo { get; set; }
        public DateTime? FechaSistema { get; set; }
        public int? IdUsuarioSistema { get; set; }
        public string UsuarioSistema { get; set; }
    }
}
