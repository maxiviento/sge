using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;
using SGE.Servicio.Vista.Shared;

namespace SGE.Servicio.VistaInterfaces
{
    public interface IInstitucionesVista
    {
        IList<IInstitucion> Instituciones { get; set; }
        IPager Pager { get; set; }
        int Id { get; set; }
        string Descripcion { get; set; }
        //string Descripcion { get; set; }
    }
}
