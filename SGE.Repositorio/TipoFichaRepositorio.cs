using System.Collections.Generic;
using System.Linq;
using SGE.Model.Entidades;
using SGE.Model.Entidades.Interfaces;
using SGE.Model.Repositorio;
using SGE.Repositorio.Modelo;

namespace SGE.Repositorio
{
    public class TipoFichaRepositorio : ITipoFichaRepositorio
    {
        private readonly DataSGE _mdb;

        public TipoFichaRepositorio()
        {
            _mdb = new DataSGE();
        }

        private IEnumerable<ITipoFicha> QTiposFicha()
        {
            var a = (from c in _mdb.T_TIPO_FICHA
                     select
                         new TipoFicha
                             {
                                 Id_Tipo_Ficha = c.ID_TIPO_FICHA,
                                 Nombre_Tipo_Ficha = c.N_TIPO_FICHA
                             });

            return a;
        }

        public IList<ITipoFicha> GetTiposFicha()
        {
            return QTiposFicha().OrderBy(o => o.Nombre_Tipo_Ficha).ToList();
        }
    }
}
