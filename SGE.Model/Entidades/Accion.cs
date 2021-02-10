using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Entidades
{
    public class Accion : IAccion
    {
        public int Id_Accion { get; set; }
        public string Nombre_Accion { get; set; }
    }
}
