using System.Collections.Generic;
using System.Linq;
using SGE.Model.Comun;
using SGE.Model.Entidades;
using SGE.Model.Entidades.Interfaces;
using SGE.Model.Repositorio;
using SGE.Repositorio.Modelo;

namespace SGE.Repositorio
{
    public class TablaBcoCbaRepositorio : ITablaBcoCbaRepositorio
    {
        private readonly DataSGE _mdb;

        public TablaBcoCbaRepositorio()
        {
            _mdb = new DataSGE();
        }

        private IQueryable<ITablaBcoCba> QTablaBcoCba()
        {
            var a = (from e in _mdb.T_TABLAS_BCO_CBA
                     select
                         new TablaBcoCba
                             {
                                 CodigoBcoCba = e.COD_BCO_CBA,
                                 DescripcionBcoCba = e.DESCRIPCION,
                                 IdTablaBcoCba = e.ID_TABLA_BCO_CBA,
                                 IdTipoTablaBcoCba = e.ID_TIPO_TABLA_BCO_CBA
                             });

            return a;
        }

        public IList<ITablaBcoCba> GetTablaBcoCba()
        {
            return QTablaBcoCba().ToList();
        }

        public IList<ITablaBcoCba> GetSucursales()
        {
            var a = (from c in QTablaBcoCba()
                     join t in _mdb.T_TIPOS_TABLA_BCO_CBA on c.IdTipoTablaBcoCba equals t.ID_TIPO_TABLA_BCO_CBA
                     where t.ID_TIPO_TABLA_BCO_CBA == (int)Enums.TiposTablaBcoCba.Sucursales
                     select c).ToList();

            return a;
        }

        public IList<ITablaBcoCba> GetMonedas()
        {
            var a = (from c in QTablaBcoCba()
                     join t in _mdb.T_TIPOS_TABLA_BCO_CBA on c.IdTipoTablaBcoCba equals t.ID_TIPO_TABLA_BCO_CBA
                     where t.ID_TIPO_TABLA_BCO_CBA == (int)Enums.TiposTablaBcoCba.Monedas
                     select c).ToList();

            return a;
        }

        public IList<ITablaBcoCba> GetSistemas()
        {
            var a = (from c in QTablaBcoCba()
                     join t in _mdb.T_TIPOS_TABLA_BCO_CBA on c.IdTipoTablaBcoCba equals t.ID_TIPO_TABLA_BCO_CBA
                     where t.ID_TIPO_TABLA_BCO_CBA == (int)Enums.TiposTablaBcoCba.Sistemas
                     select c).ToList();

            return a;
        }
    }
}
