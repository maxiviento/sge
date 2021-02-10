using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Entidades
{
    public class EstadoLiquidacion : IEstadoLiquidacion
    {
        public int IdEstadoLiquidacion { get; set; }
        public string NombreEstado { get; set; }
    }
}
