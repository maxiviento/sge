using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGE.Model.Entidades.Interfaces
{
    public interface IArchivo
    {
        int Id { get; set; }
        DateTime? Fecha { get; set; }
        string Nombre { get; set; }
        int CantidadRegistros { get; set; }
    }
}
