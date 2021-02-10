using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGE.Model.Entidades.Interfaces
{
    public interface IUsuarioEmpresa: IComunDatos
    {
        int? IdUsuarioEmpresa { get; set; }
        string NombreUsuario { get; set; }
        string ApellidoUsuario { get; set; }
        string Cuil { get; set; }
        string Telefono { get; set; }
        string Mail { get; set; }
        string LoginUsuario { get; set; }
        string PaswordUsuario { get; set; }
    }
}
