using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Entidades
{
    public class Universidad : IUniversidad
    {
        public int IdUniversidad { get; set; }
        public string NombreUniversidad { get; set; }
    }
}
