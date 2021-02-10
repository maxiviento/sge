namespace SGE.Model.Entidades.Interfaces
{
    public interface IPrograma:IComunDatos
    {
        int IdPrograma { get; set; }
        string NombrePrograma { get; set; }
        decimal MontoPrograma { get; set; }
        string ConvenioBcoCba { get; set; }
    }
}
