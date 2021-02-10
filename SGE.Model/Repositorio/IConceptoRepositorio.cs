using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Repositorio
{
    public interface IConceptoRepositorio
    {
        IList<IConcepto> GetConceptos(short? año, short? mes, string observacion);
        IList<IConcepto> GetConceptos(short? año, short? mes, string observacion, int skip, int take);
        IList<IConcepto> GetConceptosPagosBeneficiario(int idbeneficiario);
        IConcepto GetConcepto(int id_concepto);
        IConcepto GetProximoConcepto();
        int GetCountConceptos();
        IList<IConcepto> GetConceptos(int skip, int take);
        int AddConcepto(IConcepto concepto);
        bool UpdateConcepto(IConcepto concepto);

        IList<IConcepto> GetConceptosByBeneficiario(int idbeneficiario);
        IConcepto GetConceptosByBeneficiarioEfectores(int idbeneficiario, int idLiquidacion);
        IList<IConcepto> GetConceptosByBeneficiario(int idbeneficiario, int skip, int take);

    }
}
