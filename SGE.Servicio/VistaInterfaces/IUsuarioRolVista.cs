namespace SGE.Servicio.VistaInterfaces
{
    public interface IUsuarioRolVista
    {
        int IdRol { get; set; }
        string NombreRol { get; set; }
        bool Selected { get; set; }
    }
}
