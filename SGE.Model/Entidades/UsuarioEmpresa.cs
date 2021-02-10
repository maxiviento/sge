using System;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Entidades
{
    public class UsuarioEmpresa : IUsuarioEmpresa
    {
        public int? IdUsuarioEmpresa { get; set; }
        public string NombreUsuario { get; set; }
        public string ApellidoUsuario { get; set; }
        public string Cuil { get; set; }
        public string Telefono { get; set; }
        public string Mail { get; set; }
        public string LoginUsuario { get; set; }
        public string PaswordUsuario { get; set; }

        public DateTime? FechaSistema { get; set; }
        public int? IdUsuarioSistema { get; set; }
        public string UsuarioSistema { get; set; }
    }
}
