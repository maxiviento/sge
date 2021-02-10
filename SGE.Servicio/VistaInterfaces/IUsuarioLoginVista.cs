namespace SGE.Servicio.VistaInterfaces
{
    public interface IUsuarioLoginVista
    {
        string Login { get; set; }
        string Password { get; set; }
        bool Persistir { get; set; }
    }
}
