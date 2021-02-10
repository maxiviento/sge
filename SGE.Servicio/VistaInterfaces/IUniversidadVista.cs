namespace SGE.Servicio.VistaInterfaces
{
    public interface IUniversidadVista
    {
        int Id { get; set; }
        string Descripcion { get; set; }
        string Accion { get; set; }
        string BusquedaAnterior { get; set; }
    }
}
