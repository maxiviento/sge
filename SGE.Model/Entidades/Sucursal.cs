using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Entidades
{
    public class Sucursal : ISucursal
    {
        public int IdSucursal { get; set; }
        public string CodigoBanco { get; set; }
        public string Detalle { get; set; }
    }
}
