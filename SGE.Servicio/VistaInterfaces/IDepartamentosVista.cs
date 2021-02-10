using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;
using SGE.Servicio.Vista.Shared;

namespace SGE.Servicio.VistaInterfaces
{
    public interface IDepartamentosVista
    {
        IList<IDepartamento> Departamentos { get; set; }
        IPager Pager { get; set; }
        int Id { get; set; }
        string Descripcion { get; set; }
        //string Cuit { get; set; }
    }
}
