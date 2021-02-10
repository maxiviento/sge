using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGE.Model.Entidades.Interfaces
{
    public interface IRolFormularioAccion: IComunDatos
    {
        int Id_Rol { get; set; }
        int Id_Formulario { get; set; }
        int Id_Accion { get; set; }
    }
}
