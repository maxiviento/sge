using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces;
using SGE.Servicio.VistaInterfaces.Shared;

namespace SGE.Servicio.Vista
{
    public class FichasPppVista : IFichasPPPVista
    {
        public FichasPppVista()
        {
            Busqueda_Empresas = new ComboBox();
        }

        public IList<IFichaPPP> FichasPPP { get; set; }
        public IList<IFicha> Fichas { get; set; }
        public int Id { get; set; }
        public IComboBox Busqueda_Empresas { get; set; }
        public IPager Pager { get; set; }
        
        public string Descripcion_Empresa { get; set; }
        public string Descripcion_Sede { get; set; }
    }
}
