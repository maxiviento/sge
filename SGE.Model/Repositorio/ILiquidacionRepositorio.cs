using System.Collections.Generic;
using SGE.Model.Comun;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Repositorio
{
    public interface ILiquidacionRepositorio
    {
        IList<ILiquidacion> GetLiquidaciones();
        IList<ILiquidacion> GetLiquidaciones(int skip, int make);
        IList<ILiquidacion> GetLiquidaciones(System.DateTime from, System.DateTime to, int nroResolucion, int estado, int programa);
        IList<ILiquidacion> GetLiquidaciones(System.DateTime from, System.DateTime to, int nroResolucion, int estado, int programa, int skip, int make);
        IList<ILiquidacion> GetLiquidaciones(System.DateTime from, System.DateTime to, int nroResolucion, int estado, int programa, int skip, int make, int excluirprograma);
        ILiquidacion GetLiquidacion(int idLiquidacion);
        ILiquidacion GetLiquidacionSimple(int idLiquidacion);
        int GetLiquidacionesCount();
        IList<ILiquidacion> GetLiquidacionesPendientes(string apellido, string nombre, string cbu,
                                                       string numeroDocumento);
        IList<ILiquidacion> GetLiquidacionesPendientes(string apellido, string nombre, string cbu,
                                                       string numeroDocumento, int skip, int take);

        
        int AddLiquidacion(ILiquidacion liquidacion);
        bool UpdateLiquidacion(ILiquidacion liquidacion);
        bool FileGenerado(ILiquidacion liquidacion);
        bool ChangeEstado(ILiquidacion liquidacion);
        bool UpdateLiquidacionBeneficiarioConcepto(int nrocuenta, int idliquidacion, int idsucursal , Enums.EstadoBeneficiarioLiquidacion estado);
        int ExisteLiquidacionBeneficiarioConcepto(int nrocuenta, int idliquidacion, int idsucursal);
        IBeneficiario LiquidacionBeneficiarioConcepto(int nrocuenta, int idliquidacion, int idsucursal);
        IList< IBeneficiario> BeneficiarioOnliquidacion(int idliquidacion);
        ILiquidacion GetLiquidacionImputadoEnCuenta(int idLiquidacion);
        
    }
}
