using System.Collections.Generic;
using System.Linq;
using SGE.Model.Entidades;
using SGE.Model.Entidades.Interfaces;
using SGE.Repositorio.Modelo;
using SGE.Model.Repositorio;

namespace SGE.Repositorio
{
    public class NivelRepositorio : INivelRepositorio
    {
        private readonly DataSGE _mdb;

        public NivelRepositorio()
        {
            _mdb = new DataSGE();
        }

        private IQueryable<INivel> QNivel()
        {
            var a = (from c in _mdb.T_NIVEL
                     select
                         new Nivel
                         {
                            IdNivel = c.ID_NIVEL,
                            NombreNivel= c.N_NIVEL,
                         });
            return a;
        }

        public INivel GetNivel(int id)
        {
            return QNivel().Where(c => c.IdNivel== id).SingleOrDefault();
        }

        public IList<INivel> GetNiveles()
        {
            return QNivel().OrderBy(c=>c.NombreNivel).ToList();
        }

        public int GetNivelesCount()
        {
            return QNivel().Count();
        }
    }
}