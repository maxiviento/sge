using System;
using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Entidades
{
    public class FichaObservacion: IFichaObservacion
    {
        public int IdFichaObservacion { get; set; }
        public int IdFicha { get; set; }
        public string Observacion { get; set; }

        public DateTime? FechaSistema { get; set; }
        public int? IdUsuarioSistema { get; set; }
        public string UsuarioSistema { get; set; }
    }
}
