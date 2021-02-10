namespace SGE.Model.Entidades.Interfaces
{
    public interface ISubprograma: IComunDatos
    {
        int IdSubprograma { get; set; }
        short? IdPrograma { get; set; }
        string NombreSubprograma { get; set; }
        string Descripcion { get; set; }
        decimal monto { get; set; }

        string Programa { get; set; }

    }
}
