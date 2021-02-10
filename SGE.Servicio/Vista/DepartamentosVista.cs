using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces;
using System.ComponentModel.DataAnnotations;

namespace SGE.Servicio.Vista
{
    public class DepartamentosVista : IDepartamentosVista
    {
        public IList<IDepartamento> Departamentos { get; set; }
        public IPager Pager { get; set; }
        public int Id{ get; set; }

        [StringLength(90)]
        public string Descripcion { get; set; }
    }
}
