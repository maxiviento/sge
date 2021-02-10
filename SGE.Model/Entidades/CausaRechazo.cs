using System;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Entidades
{
    public class CausaRechazo : ICausaRechazo
    {
        public int IdCausaRechazo { get; set; }
        public string NombreCausaRechazo { get; set; }
        
        public DateTime? FechaSistema { get; set; }
        public int? IdUsuarioSistema { get; set; }
        public string UsuarioSistema { get; set; }
    }
}
