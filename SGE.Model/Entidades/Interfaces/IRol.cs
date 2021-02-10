using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGE.Model.Entidades.Interfaces
{
    public interface IRol : IComunDatos
    {
        int Id_Rol { get; set; }
        string Nombre_Rol { get; set; }
        int Id_Estado_Rol { get; set; }
    }
}
