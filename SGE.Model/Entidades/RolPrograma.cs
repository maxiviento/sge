using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Entidades
{
    public class RolPrograma : IRolPrograma
    {
        public int Id_Rol { get; set; }
        public string Nombre_Rol { get; set; }
        public int Id_Estado_Rol { get; set; }
        public int IdPrograma { get; set; }
        public string NombrePrograma { get; set; }
    }
}
