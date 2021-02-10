using System.Collections.Generic;
using System.Linq;
using SGE.Model.Entidades;
using SGE.Model.Entidades.Interfaces;
using SGE.Model.Repositorio;
using SGE.Repositorio.Modelo;

namespace SGE.Repositorio
{
    public class EstadoCivilRepositorio : IEstadoCivilRepositorio
    {
      private readonly DataSGE _mdb;

      public EstadoCivilRepositorio()
        {
            _mdb = new DataSGE();
        }

        private IEnumerable<IEstadoCivil> QEstadosCivil()
        {
            var a = (from c in _mdb.T_ESTADO_CIVIL
                     select
                         new EstadoCivil
                             {
                                 Id_Estado_Civil = c.ID_ESTADO_CIVIL,
                                 Nombre_Estado_Civil = c.N_ESTADO_CIVIL
                             });

            return a;
        }

        public IList<IEstadoCivil> GetEstadosCivil()
        {
            return QEstadosCivil().OrderBy(o => o.Nombre_Estado_Civil).ToList();
        }
    }
}
