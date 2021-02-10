using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGE.Model.Entidades.Interfaces
{
    public interface ITitulo
    {
        Int16 IdTitulo { get; set; }
        Int16? IdUniversidad { get; set; }
        string Descripcion { get; set; }
    }
}
