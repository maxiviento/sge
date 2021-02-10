namespace SGE.Servicio.VistaInterfaces
{
    public interface IProgramaVista
    {
        int Id { get; set; }
        string Descripcion { get; set; }
        string Accion { get; set; }
        decimal MontoPrograma { get; set; }
        string ConvenioBcoCba { get; set; }
        string BusquedaAnterior { get; set; }
    }
}
