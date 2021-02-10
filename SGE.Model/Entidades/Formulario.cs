using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Entidades
{
    public class Formulario : IFormulario
    {
        public int Id_Formulario { get; set; }
        public string Nombre_Formulario { get; set; }
    }
}
