using System;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Entidades
{
    public class Conyuge : IConyuge
    {   
        public int IdConyugue { get; set; }
        public int IdBeneficiario { get; set; }
        public string Cuil { get; set; }
        public string Apellido { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public int? TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public string Sexo { get; set; }
        public int? Nacionalidad { get; set; }

        public DateTime? FechaSistema { get; set; }
        public int? IdUsuarioSistema { get; set; }
        public string UsuarioSistema { get; set; }
    }
}
