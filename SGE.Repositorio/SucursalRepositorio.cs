using System.Collections.Generic;
using System.Linq;
using SGE.Model.Entidades;
using SGE.Model.Entidades.Interfaces;
using SGE.Model.Repositorio;
using SGE.Repositorio.Modelo;

namespace SGE.Repositorio
{
    public class SucursalRepositorio : ISucursalRepositorio
    {
        private readonly DataSGE _mdb;

        public SucursalRepositorio()
        {
            _mdb = new DataSGE();
        }

        private IQueryable<ISucursal> QSucursal()
        {
            return
                _mdb.T_TABLAS_BCO_CBA.Where(c => c.ID_TIPO_TABLA_BCO_CBA == 1).Select(
                    c =>
                    new Sucursal {CodigoBanco = c.COD_BCO_CBA, Detalle = c.DESCRIPCION, IdSucursal = c.ID_TABLA_BCO_CBA});
        }

        public ISucursal GetSucursalByCodigoBanco(string codigobanco)
        {
            return QSucursal().Where(c => c.CodigoBanco == codigobanco).SingleOrDefault();
        }

        public IList<ISucursal> GetSucursales()
        {
            return QSucursal().OrderBy(c=>c.CodigoBanco).ToList();
        }

        public short GestSucursalLocalidad(int idFicha)
        {
            //26/01/2013 - DI CAMPLI LEANDRO
            //var idSuc = (from f in _mdb.T_FICHAS.Where(fi => fi.ID_FICHA == idFicha)
            //             from loc in _mdb.T_LOCALIDADES.Where(l => l.ID_LOCALIDAD == f.ID_LOCALIDAD)
  
            //                 ).FirstOrDefault();

            var cuba = _mdb.T_FICHAS.Where(f => f.ID_FICHA == idFicha).Select(s => s.ID_LOCALIDAD).FirstOrDefault();//.First();
            int sucCba = 25;

            var idSuc = (from loc in _mdb.T_LOCALIDADES
                         where loc.ID_LOCALIDAD == cuba && loc.ID_LOCALIDAD != sucCba //solo para localidades del interior
                         select loc.T_TABLAS_BCO_CBA.Where(c => c.ID_TIPO_TABLA_BCO_CBA == 1).Select(c=>c.ID_TABLA_BCO_CBA).FirstOrDefault()//.Max(tb => tb.ID_TABLA_BCO_CBA)
                         ).FirstOrDefault();

            //var idSuc = (from loc in _mdb.T_LOCALIDADES
            //             join tsc in _mdb.t_s
            //             join tbc in _mdb.T_TABLAS_BCO_CBA on loc.T equals tbc.ID_TABLA_BCO_CBA
            //             where loc.ID_LOCALIDAD == cuba && loc.ID_LOCALIDAD != sucCba //solo para localidades del interior
            //             && tbc.ID_TIPO_TABLA_BCO_CBA == 1
            //             select tbc.ID_TABLA_BCO_CBA
            //             );

            //short resu = 0;

            //resu = idSuc.Count() > 0 ? idSuc.Max() : 0;

            //return resu;

            if (idSuc != 0)
            {
                return idSuc;
            }
            else
            {
                return 0;
            }

        
        }

    }
}
