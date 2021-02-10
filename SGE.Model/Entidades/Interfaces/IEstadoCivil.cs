using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGE.Model.Entidades.Interfaces
{
    public interface IEstadoCivil
    {
        int Id_Estado_Civil { get; set; }
        string Nombre_Estado_Civil { get; set; }
    }
}
