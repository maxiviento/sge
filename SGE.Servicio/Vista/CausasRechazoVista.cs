using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SGE.Model.Entidades.Interfaces;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces;

namespace SGE.Servicio.Vista
{
    public class CausasRechazoVista : ICausasRechazoVista
    {
        public IList<ICausaRechazo> CausasRechazo { get; set; }
        public IPager Pager { get; set; }
        public int Id { get; set; }

        [StringLength(90)]
        public string BuscarDescripcion { get; set; }
    }
}
