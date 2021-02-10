using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGE.Model.Entidades.Interfaces
{
    public interface IControlDatos
    {
        int Id { get; set; }
        int IdPrograma { get; set; }
        string Identificador1 { get; set; }
        string Identificador2 { get; set; }
        string Identificador3 { get; set; }
        string Identificador4 { get; set; }
        string Descripcion { get; set; }
        string ModuloAccion { get; set; }

        int? idEtapa { get; set; }
        string Etapa { get; set; }
        string N_Programa { get; set; }


    }
}
