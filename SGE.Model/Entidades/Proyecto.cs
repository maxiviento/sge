using System;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Entidades
{
    public class Proyecto: IProyecto
    {
        public int? IdProyecto { get; set; }
        public string NombreProyecto { get; set; }
        public short? CantidadAcapacitar { get; set; }
        public DateTime? FechaInicioProyecto { get; set; }
        public DateTime? FechaFinProyecto { get; set; }
        public short? MesesDuracionProyecto { get; set; }
        public DateTime? FechaSistema { get; set; }
        public int? IdUsuarioSistema { get; set; }
        public string UsuarioSistema { get; set; }
    }
}
