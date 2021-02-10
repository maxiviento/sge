namespace SGE.Servicio.VistaInterfaces
{
    public interface IConceptoVista
    {
        int IdConcepto { get; set; }
        short? Año { get; set; }
        short? Mes { get; set; }
        string Observacion { get; set; }
        string Accion { get; set; }
    }
}
