using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Entidades
{
    public class EstadoFicha : IEstadoFicha
    {
        public int Id_Estado_Ficha { get; set; }
        public string Nombre_Estado_Ficha { get; set; }
    }
}
