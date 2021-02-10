using SGE.Servicio.VistaInterfaces;
using SGE.Servicio.Vista.Shared;

namespace SGE.Servicio.ServicioInterfaces
{
    public interface IFichaPPPServicio
    {
        IFichasPPPVista GetFichasPPP();
        IFichasPPPVista GetFichasPPP(int id_empresa);
        IFichaPppVista GetFichaPPP(int id, string accion);
        IFichasPPPVista GetFichasPPP(Pager pager, int Id,int id_empresa);
    }
}
