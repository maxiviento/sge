using SGE.Servicio.VistaInterfaces;

namespace SGE.Servicio.Vista
{
    public class ImportarExcelVista : IImportarExcelVista
    {
        public bool Importado { get; set; }
        public string Mensaje { get; set; }
        public string Archivo { get; set; }
    }
}
