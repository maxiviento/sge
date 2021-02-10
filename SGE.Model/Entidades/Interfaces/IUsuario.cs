using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGE.Model.Entidades.Interfaces
{
    public interface IUsuario :IComunDatos
    {
        int Id_Usuario { get; set; }
        string Nombre_Usuario { get; set; }
        string Apellido_Usuario { get; set; }
        string Login_Usuario { get; set; }
        string Pasword_Usuario { get; set; }
        int Id_Estado_Usuario { get; set; }
        IList<IRol> ListaRoles { get; set; }
    }
}
