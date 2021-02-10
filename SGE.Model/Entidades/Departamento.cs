using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Entidades
{
    public class Departamento : IDepartamento
    {
        public int IdDepartamento { get; set; }
        public string NombreDepartamento { get; set; }
    }
}
