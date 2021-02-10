using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces;

namespace SGE.Servicio.ServicioInterfaces
{
    public interface IConceptoServicio
    {
        IConceptosVista GetConceptos();
        IConceptosVista GetIndex();
        IConceptosVista GetConceptos(IPager pager);
        IConceptoVista GetConcepto(int idConcepto, string accion);
        IConceptosVista GetConceptos(short? año, short? mes, string observacion);
        IConceptosVista GetConceptos(IPager pPager, short? año, short? mes, string observacion);
        IConceptoVista GetProximoConcepto();
        int AddConcepto(IConceptoVista modelo);
        bool UpdateConcepto(IConceptoVista modelo);
        IConceptosVista GetConceptosPagosBeneficiario(int idbeneficiario);
    }
}
