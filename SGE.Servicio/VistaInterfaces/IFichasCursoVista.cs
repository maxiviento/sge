using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGE.Servicio.VistaInterfaces
{
    public interface IFichasCursoVista
    {

        int IdFicha { get; set; }
        int? tipoFicha { get; set; }
        string Cuil { get; set; }
        string Apellido { get; set; }
        string Nombre { get; set; }
        int? TipoDocumento { get; set; }
        string NumeroDocumento { get; set; }
        string MES { get; set; }
        string PORC_ASISTENCIA { get; set; }
        int? id_curso { get; set; }
    }
}
