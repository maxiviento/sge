using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Entidades
{
    public class ControlDatos : IControlDatos
    {
        public int Id { get; set; }
        public int IdPrograma { get; set; }
        public string Identificador1 { get; set; }
        public string Identificador2 { get; set; }
        public string Identificador3 { get; set; }
        public string Identificador4 { get; set; }
        public string Descripcion { get; set; }
        public string ModuloAccion { get; set; }

        public int? idEtapa { get; set; }
        public string Etapa { get; set; }
        public string N_Programa { get; set; }
    }
}
