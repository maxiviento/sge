using System;
using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Entidades
{
    public class Usuario : IUsuario
    {
        public Usuario()
        {
            ListaRoles = new List<IRol>();
        }

        public int Id_Usuario { get; set; }
        public string Nombre_Usuario { get; set; }
        public string Apellido_Usuario { get; set; }
        public string Login_Usuario { get; set; }
        public string Pasword_Usuario { get; set; }
        public int Id_Estado_Usuario { get; set; }
        public IList<IRol> ListaRoles { get; set; }

        public DateTime? FechaSistema { get; set; }
        public int? IdUsuarioSistema { get; set; }
        public string UsuarioSistema { get; set; }
    }
}
