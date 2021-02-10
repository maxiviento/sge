using System.Collections.Generic;
using SGE.Model.Entidades;
using SGE.Model.Entidades.Interfaces;
using SGE.Model.Repositorio;
using SGE.Repositorio.Modelo;
using System.Linq;

namespace SGE.Repositorio
{
    public class SectorRepositorio:ISectorRepositorio
    {
        private readonly DataSGE _mdb;

        public SectorRepositorio()
        {
            _mdb = new DataSGE();
        }

        private IQueryable<ISector> QSector()
        {
            var a = (from i in _mdb.T_SECTOR
                     select
                         new Sector
                             {
                                IdSector = i.ID_SECTOR,
                                NombreSector= i.N_SECTOR
                             });
            return a;
        }

        public IList<ISector> GetSectores()
        {
            return QSector().OrderBy(c => c.IdSector).ToList();
        }

        public ISector GetSector(int idSector)
        {
            return QSector().Where(c => c.IdSector == idSector).ToList().SingleOrDefault();
        }

        public int GetSectorCount()
        {
            return QSector().ToList().Count;
        }
    }
}
