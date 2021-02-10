namespace SGE.Model.Entidades.Interfaces
{
    public interface IHorarioEmpresa : IComunDatos
    {
        int IdEmpresaHorario { get; set; }
        int IdEmpresa { get; set; }
        string IdDia { get; set; }
        short HoraDesde { get; set; }
        short MinutosDesde { get; set; }
        short HoraHasta { get; set; }
        short MinutosHasta { get; set; }
    }
}
