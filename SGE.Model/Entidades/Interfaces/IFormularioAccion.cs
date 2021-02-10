using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGE.Model.Entidades.Interfaces
{
    public interface IFormularioAccion
    {
        int Id_Formulario { get; set; }
        int Id_Accion { get; set; }
        string Nombre_Accion { get; set; }
    }
}
