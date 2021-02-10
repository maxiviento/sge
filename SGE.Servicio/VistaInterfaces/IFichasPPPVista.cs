using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces.Shared;

namespace SGE.Servicio.VistaInterfaces
{
    public interface IFichasPPPVista
    {
        IList<IFichaPPP> FichasPPP { get; set; }
        IList<IFicha> Fichas { get; set; }
        IPager Pager { get; set; }
        int Id { get; set; }
        IComboBox Busqueda_Empresas { get; set; }

        string Descripcion_Empresa { get; set; }
        string Descripcion_Sede { get; set; }
    }
}
