using System;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Entidades
{
    public class TablaBcoCba : ITablaBcoCba
    {
        public string CodigoBcoCba { get; set; }
        public string DescripcionBcoCba { get; set; }

        public short IdTablaBcoCba { get; set; }
        public short? IdTipoTablaBcoCba { get; set; }
    }
}
