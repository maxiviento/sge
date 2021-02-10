namespace SGE.Servicio.VistaInterfaces
{
    public interface IAccionVista
    {
        int Id { get; set; }
        string Accion { get; set; }
        bool Selected { get; set; }
    }
}
