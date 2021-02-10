using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Entidades
{
    public class EstadoApoderado: IEstadoApoderado
    {
        public int IdEstadoApoderado { get; set; }
        public string NombreEstadoApoderado { get; set; }
    }
}
