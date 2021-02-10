namespace SGE.Model.Entidades.Interfaces
{
    public interface IFichaRechazo : IComunDatos
    {
        int IdFicha { get; set; }
        int IdTipoRechazo { get; set; }
    }
}
