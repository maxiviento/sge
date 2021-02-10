using System.Collections.Generic;
using SGE.Model.Entidades;
using SGE.Model.Entidades.Interfaces;
using SGE.Model.Repositorio;
using SGE.Repositorio.Modelo;
using System.Linq;

namespace SGE.Repositorio
{
    public class SectorInstitucionRepositorio:ISectorInstitucionRepositorio
    {
        private readonly DataSGE _mdb;

        public SectorInstitucionRepositorio()
        {
            _mdb = new DataSGE();
        }

        private IQueryable<ISectorInstitucion> QSectorInstitucion()
        {
            var a = (from i in _mdb.T_SECTOR_INSTITUCION
                     select
                         new SectorInstitucion
                             {
                                IdSector = i.ID_SECTOR,
                                IdInstitucion= i.ID_INSTITUCION
                             });
            return a;
        }

        public IList<ISectorInstitucion> GetSectoresInstituciones()
        {
            return QSectorInstitucion().OrderBy(c => c.IdSector).ToList();
        }

        public IList<ISectorInstitucion> GetSectoresInstitucionByInstitucion(int idInstitucion)
        {
            return QSectorInstitucion().Where(c => c.IdInstitucion == idInstitucion).ToList();
        }

        public int GetSectorInstitucionCount()
        {
            return QSectorInstitucion().ToList().Count;
        }
    }
}
