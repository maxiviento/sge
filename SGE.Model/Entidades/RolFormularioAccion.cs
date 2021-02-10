using System;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Entidades
{
    public class RolFormularioAccion : IRolFormularioAccion
    {
        public int Id_Rol { get; set; }
        public int Id_Formulario { get; set; }
        public int Id_Accion { get; set; }

        public DateTime? FechaSistema { get; set; }
        public int? IdUsuarioSistema { get; set; }
        public string UsuarioSistema { get; set; }
    }
}
