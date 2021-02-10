using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGE.Model.Entidades.Interfaces
{
    public interface IAsistencia
    {
        int ID_ASISTENCIA {get; set;}
        int? ID_CURSO {get; set;}
        int? ID_ESCUELA {get; set;}
        int? ID_FICHA {get; set;}
        string MES {get; set;}
        string PERIODO {get; set;}
        int? PORC_ASISTENCIA { get; set; }
        int? ID_USR_SIST {get; set;}
        DateTime? FEC_SIST {get; set;}
        int? ID_USR_MODIF {get; set;}
        DateTime? FEC_MODIF { get; set; }

        int? tipoFicha { get; set; }
        string Cuil { get; set; }
        string Apellido { get; set; }
        string Nombre { get; set; }
        int? TipoDocumento { get; set; }
        string NumeroDocumento { get; set; }
    }
}
