namespace SGE.Servicio.VistaInterfaces
{
    public interface IImportarExcelVista
    {
        bool Importado { get; set; }
        string Mensaje { get; set; }
        string Archivo { get; set; }
    }
}
