using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces;
using SGE.Model.Entidades;

namespace SGE.Servicio.Vista
{
    public class FichasCursoVista : IFichasCursoVista
    {
        public int IdFicha { get; set; }
        public int? tipoFicha { get; set; }
        public string Cuil { get; set; }
        public string Apellido { get; set; }
        public string Nombre { get; set; }
        public int? TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public string MES { get; set; }
        public string PORC_ASISTENCIA { get; set; }
        public int? id_curso { get; set; }
    }
}
