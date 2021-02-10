using System;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Entidades
{
    public class Subprograma: ISubprograma
    {
        public int IdSubprograma { get; set; }
        public short? IdPrograma { get; set; }
        public string NombreSubprograma { get; set; }
        public string Descripcion { get; set; }

        public DateTime? FechaSistema { get; set; }
        public int? IdUsuarioSistema { get; set; }
        public string UsuarioSistema { get; set; }
        public decimal monto { get; set; }

        public string Programa { get; set; }
    }
}
