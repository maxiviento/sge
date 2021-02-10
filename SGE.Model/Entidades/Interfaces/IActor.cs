using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGE.Model.Entidades.Interfaces
{
    public interface IActor
    {
        int ID_ACTOR { get; set; }
        string APELLIDO { get; set; }
        string NOMBRE { get; set; }
        string DNI { get; set; }
        string CUIL { get; set; }
        string CELULAR { get; set; }
        string TELEFONO { get; set; }
        string MAIL { get; set; }
        int?   ID_LOCALIDAD { get; set; }
        string N_LOCALIDAD { get; set; }
        int? ID_ACTOR_ROL { get; set; }
        string N_ACTOR_ROL { get; set; }
        int? ID_BARRIO { get; set; }
        string N_BARRIO { get; set; }
        int? ID_PERSONA { get; set; }
    }
}
