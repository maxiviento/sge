using System.Collections.Generic;
using System.Linq;
using SGE.Model.Entidades;
using SGE.Model.Entidades.Interfaces;
using SGE.Model.Repositorio;
using SGE.Repositorio.Modelo;

namespace SGE.Repositorio
{
    public class TipoPppRepositorio : ITipoPppRepositorio
    {
         private readonly DataSGE _mdb;

        public TipoPppRepositorio()
        {
            _mdb = new DataSGE();
        }

        private IEnumerable<ITipoPpp> QTiposPpp()
        {
            var a = (from c in _mdb.T_TIPOS_PPP
                     select
                         new TipoPpp
                             {
                                 ID_TIPO_PPP = c.ID_TIPO_PPP,
                                 N_TIPO_PPP = c.N_TIPO_PPP
                             });

            return a;
        }

        public IList<ITipoPpp> GetTiposPpp()
        {
            return QTiposPpp().OrderBy(o => o.N_TIPO_PPP).ToList();
        }
    }
}
