using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGE.Model.Entidades.Interfaces
{
    public interface ITablaBcoCba
    {
        string CodigoBcoCba {get;set;}
        string DescripcionBcoCba {get;set;}

        Int16 IdTablaBcoCba { get; set; }
        Int16? IdTipoTablaBcoCba { get; set; }
    }
}
