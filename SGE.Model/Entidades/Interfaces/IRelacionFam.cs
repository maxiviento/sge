using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGE.Model.Entidades.Interfaces
{
    public interface IRelacionFam
    {
        int? ID_FICHA { get; set; }
        int? ID_PERSONA { get; set; }
        string APELLIDO { get; set; }
        string NOMBRE { get; set; }
        string DNI { get; set; }
        int? ID_VINCULO { get; set; }
        string N_VINCULO { get; set; }
    }
}
