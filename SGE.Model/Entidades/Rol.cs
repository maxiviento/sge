using System;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Entidades
{
    public class Rol : IRol
    {
        public int Id_Rol { get; set; }
        public string Nombre_Rol { get; set; }
        public int Id_Estado_Rol { get; set; }

        public DateTime? FechaSistema { get; set; }
        public int? IdUsuarioSistema { get; set; }
        public string UsuarioSistema { get; set; }
    }
}
