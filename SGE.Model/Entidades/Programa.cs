using System;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Entidades
{
    public class Programa : IPrograma
    {
        public int IdPrograma { get; set; }
        public string NombrePrograma { get; set; }
        public decimal MontoPrograma { get; set; }
        public string ConvenioBcoCba { get; set; }

        public DateTime? FechaSistema { get; set; }
        public int? IdUsuarioSistema { get; set; }
        public string UsuarioSistema { get; set; }
    }
}
