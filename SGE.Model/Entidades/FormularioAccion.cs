using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Entidades
{
    public class FormularioAccion : IFormularioAccion
    {
        public int Id_Formulario { get; set; }
        public int Id_Accion { get; set; }
        public string Nombre_Accion { get; set; }
    }
}
