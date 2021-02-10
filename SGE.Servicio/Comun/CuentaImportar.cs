namespace SGE.Servicio.Comun
{
    public class CuentaImportar : ICuentaImportar
    {
        public CuentaImportar()
        {
            Importado = false;
            EsApoderado = false;
        }

        public string Sucursal { get; set; }
        public string NombreSucursal { get; set; }
        public string NroCuenta { get; set; }
        public string Apellido { get; set; }
        public string SegundoApellido { get; set; }
        public string Nombre { get; set; }
        public string SegundoNombre { get; set; }
        public string NumeroDocumento { get; set; }
        public string Cbu { get; set; }
        public bool Importado { get; set; }
        public bool EsApoderado { get; set; }
    }
}
