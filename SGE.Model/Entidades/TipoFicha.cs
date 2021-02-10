using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Entidades
{
    public class TipoFicha : ITipoFicha
    {
        public int Id_Tipo_Ficha { get; set; }
        public string Nombre_Tipo_Ficha { get; set; }
    }
}
