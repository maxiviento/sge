using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGE.Model.Entidades.Interfaces
{
    public interface IFichasCurso
    {
        int IdFicha { get; set; }
        int? tipoFicha { get; set; }
        string Cuil { get; set; }
        string Apellido { get; set; }
        string Nombre { get; set; }
        int? TipoDocumento { get; set; }
        string NumeroDocumento { get; set; }
        //IFichaConfVos FichaConfVos { get; set; }
        //IBeneficiario BeneficiarioFicha { get; set; }
        string MES { get; set; }
        int? PORC_ASISTENCIA { get; set; }
        int? id_curso { get; set; }
        int idAsistencia { get; set; }
        bool registrar { get; set; }
    }
}
