namespace SGE.Servicio.VistaInterfaces
{
    public interface ITipoRechazoVista
    {
        int Id { get; set; }
        int IdOld { get; set; }
        string Descripcion { get; set; }
        string Accion { get; set; }
        bool Selected { get; set; }
    }
}
