namespace SGE.Model.Entidades.Interfaces
{
    public interface IUsuarioRol:IComunDatos
    {
        int IdUsuario { get; set; }
        int IdRol { get; set; }
    }
}
