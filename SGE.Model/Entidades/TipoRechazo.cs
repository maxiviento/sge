using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Entidades
{
    public class TipoRechazo : ITipoRechazo
    {
        public int IdTipoRechazo { get; set; }
        public string NombreTipoRechazo { get; set; }
    }
}
