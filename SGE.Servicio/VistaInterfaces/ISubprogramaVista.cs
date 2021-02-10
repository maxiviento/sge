namespace SGE.Servicio.VistaInterfaces
{
    public interface ISubprogramaVista
    {
        int Id { get; set; }
        string Nombre { get; set; }
        string Descripcion { get; set; }
        string Accion { get; set; }
        string BusquedaAnterior { get; set; }
        decimal monto { get; set; }
        short? IdPrograma { get; set; }
        string Programa { get; set; }

    }
}
