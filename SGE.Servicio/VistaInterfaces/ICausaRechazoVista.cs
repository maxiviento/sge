namespace SGE.Servicio.VistaInterfaces
{
    public interface ICausaRechazoVista
    {
        int Id { get; set; }
        int IdOld { get; set; }
        string Descripcion { get; set; }
        string Accion { get; set; }
        string BusquedaAnterior { get; set; }
    }
}
