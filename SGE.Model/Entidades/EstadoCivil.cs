using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Entidades
{
    public class EstadoCivil : IEstadoCivil
    {
        public int Id_Estado_Civil { get; set; }
        public string Nombre_Estado_Civil { get; set; }
    }
}
