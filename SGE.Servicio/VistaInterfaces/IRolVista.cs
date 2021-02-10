namespace SGE.Servicio.VistaInterfaces
{
    public interface IRolVista
    {
        int Id { get; set; }
        string Nombre { get; set; }
        int IdEstado { get; set; }
        string Accion { get; set; }
    }
}
