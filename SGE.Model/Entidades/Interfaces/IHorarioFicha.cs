using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGE.Model.Entidades.Interfaces
{
    public interface IHorarioFicha : IComunDatos
    {
        int IdFicha { get; set; }
        int IdEmpresaHorario { get; set; }
        short Corte { get; set; }
        string Observacion { get; set; }
        string DSemana { get; set; }

        int IdEmpresa { get; set; }
        int? Horadesde { get; set; }
        int? Minutosdesde { get; set; }
        int? HoraHasta { get; set; }
        int? MinutosHasta { get; set; }

    }
}
