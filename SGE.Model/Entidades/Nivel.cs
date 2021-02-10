using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Entidades
{
    public class Nivel: INivel
    {
        public int IdNivel { get; set; }
        public string NombreNivel { get; set; }
    }
}
