using System;
using System.Collections.Generic;
using System.Linq;
using SGE.Model.Entidades;
using SGE.Model.Entidades.Interfaces;
using SGE.Model.Repositorio;
using SGE.Repositorio.Modelo;

namespace SGE.Repositorio
{
    public class InstitucionRepositorio : IInstitucionRepositorio
    {
        private readonly DataSGE _mdb;

        public InstitucionRepositorio()
        {
            _mdb = new DataSGE();
        }

        private IQueryable<IInstitucion> QInstitucion()
        {
            var a = (from i in _mdb.T_INSTITUCIONES
                     select
                         new Institucion
                             {
                                 IdInstitucion = i.ID_INSTITUCION,
                                 NombreInstitucion = i.N_INSTITUCION,
                             });
            return a;
        }

        public IList<IInstitucion> GetInstituciones()
        {
            return QInstitucion().OrderBy(c => c.NombreInstitucion).ToList();
        }

        public IList<IInstitucion> GetInstitucionesBySectorProductivo(int idSector)
        {
            var lcarreras = (
                from ca in _mdb.T_CARRERAS
                where ca.ID_SECTOR == idSector && ca.ID_INSTITUCION != null
                select ca.ID_INSTITUCION).Distinct().Cast<int>().ToArray();

            return QInstitucion().OrderBy(o => o.NombreInstitucion).Where(c => lcarreras.Contains(c.IdInstitucion)).ToList();
        }

        public IList<IInstitucion> GetInstitucionesByNivel(int idNivel)
        {
            var lcarreras = (
                from ca in _mdb.T_CARRERAS
                where ca.ID_NIVEL == idNivel && ca.ID_INSTITUCION != null
                select ca.ID_INSTITUCION ?? 0).Distinct().Cast<int>().ToArray();

            return QInstitucion().OrderBy(o => o.NombreInstitucion).Where(c => lcarreras.Contains(c.IdInstitucion)).ToList();
        }

        public IList<IInstitucion> GetInstituciones(int skip, int take)
        {
            return QInstitucion().OrderBy(c => c.NombreInstitucion).Skip(skip).Take(take).ToList();
        }

        public IList<IInstitucion> GetInstituciones(string descripcion)
        {
            descripcion = descripcion ?? String.Empty;

            return QInstitucion().Where(c => (c.NombreInstitucion.ToLower().Contains(descripcion.ToLower()) || String.IsNullOrEmpty(descripcion))).OrderBy(o => o.NombreInstitucion).ToList();
        }

        public IList<IInstitucion> GetInstituciones(string descripcion, int skip, int take)
        {
            return GetInstituciones(descripcion).Skip(skip).Take(take).ToList();
        }

        public IInstitucion GetInstitucion(int id)
        {
            return QInstitucion().OrderBy(o => o.NombreInstitucion).Where(c => c.IdInstitucion == id).SingleOrDefault();
        }

        public int AddInstitucion(IInstitucion institucion)
        {
            var instModel = new T_INSTITUCIONES
            {
                ID_INSTITUCION = SecuenciaRepositorio.GetId(),
                N_INSTITUCION = institucion.NombreInstitucion,
            };

            _mdb.T_INSTITUCIONES.AddObject(instModel);
            _mdb.SaveChanges();

            return instModel.ID_INSTITUCION;
        }

        public void UpdateInstitucion(IInstitucion institucion)
        {
            if (institucion.IdInstitucion == 0) return;

            var obj = _mdb.T_INSTITUCIONES.SingleOrDefault(c => c.ID_INSTITUCION == institucion.IdInstitucion);

            if (obj == null) return;

            obj.N_INSTITUCION = institucion.NombreInstitucion;

            _mdb.SaveChanges();
        }

        public void DeleteInstitucion(IInstitucion institucion)
        {
            if (institucion.IdInstitucion == 0) return;

            var obj = _mdb.T_INSTITUCIONES.SingleOrDefault(c => c.ID_INSTITUCION == institucion.IdInstitucion);

            if (obj == null) return;

            _mdb.DeleteObject(obj);
            _mdb.SaveChanges();
        }

        public bool ExistsInstitucion(string institucion)
        {
            var t = QInstitucion().Where(c => 0 == institucion.CompareTo(c.NombreInstitucion)).ToList().Count;

            return t > 0 ? true : false;
        }

        public int GetInstitucionesCount()
        {
            return QInstitucion().Count();
        }

        public int GetInstitucionMaxId()
        {
            var a = (from c in _mdb.T_INSTITUCIONES
                     select c.ID_INSTITUCION);

            return a.Max();
        }

        // 07/03/2013 - DI CAMPLI LEANDRO - LISTAR SECTORES EN INSTITUCIONES
        public IList<ISector> GetSectoresInstitucion(int idInstitucion)
        {
            var sectores = QSectoresInstitucion(idInstitucion).ToList();

            return sectores;
        }

        // 07/03/2013 - DI CAMPLI LEANDRO - LISTAR SECTORES EN INSTITUCIONES
        private IQueryable<ISector> QSectoresInstitucion(int idInstitucion)
        {
            var sectores = (from s in _mdb.T_SECTOR
                            join si in _mdb.T_SECTOR_INSTITUCION on s.ID_SECTOR equals si.ID_SECTOR
                            where si.ID_INSTITUCION == idInstitucion
                            select new Sector { IdSector = s.ID_SECTOR, NombreSector = s.N_SECTOR });

            return sectores;
        }
        // 07/03/2013 - DI CAMPLI LEANDRO - LISTAR SECTORES EN INSTITUCIONES
        public int AddAsociarSector(int idInstitucion, int idSector)
        {
            var sectorinstitucion = new T_SECTOR_INSTITUCION
            {
                ID_INSTITUCION = idInstitucion,
                ID_SECTOR = idSector,
            };

            try
            {


                _mdb.T_SECTOR_INSTITUCION.AddObject(sectorinstitucion);
                _mdb.SaveChanges();

                return sectorinstitucion.ID_INSTITUCION;

            }
            catch (Exception)
            {
                return 0;
            }
            finally { }
        }

        // 07/03/2013 - DI CAMPLI LEANDRO - LISTAR SECTORES EN INSTITUCIONES
        private IQueryable<ISector> QSectorNoAsignado(int idInstitucion)
        {
            var sectores = (from s in _mdb.T_SECTOR
                            where  
                                s.ID_SECTOR  != (from si in _mdb.T_SECTOR_INSTITUCION.Where(c => c.ID_INSTITUCION == idInstitucion && c.ID_SECTOR==s.ID_SECTOR) select si.ID_SECTOR).FirstOrDefault()
                            //where si.ID_INSTITUCION == idInstitucion
                            select new Sector { IdSector = s.ID_SECTOR, NombreSector = s.N_SECTOR });

            return sectores;
        }

        // 07/03/2013 - DI CAMPLI LEANDRO - LISTAR SECTORES EN INSTITUCIONES
        public IList<ISector> GetSectorNoAsignado(int idInstitucion)
        {
            var sectores = QSectorNoAsignado(idInstitucion).ToList();

            return sectores;
        }

        // 08/03/2013 - DI CAMPLI LEANDRO - LISTAR SECTORES EN INSTITUCIONES
        public void DeleteSectorInstitucion(int idInstitucion, int idSector)
        {

            //if (idInstitucion == 0) return;

            var obj = _mdb.T_SECTOR_INSTITUCION.SingleOrDefault(c => c.ID_INSTITUCION == idInstitucion && c.ID_SECTOR==idSector);

            if (obj == null) return;

            _mdb.DeleteObject(obj);
            _mdb.SaveChanges();
        
        }

        private IQueryable<IInstitucion> QInstitucion(int idSector)
        {
            var a = (from si in _mdb.T_SECTOR_INSTITUCION
                     join i in _mdb.T_INSTITUCIONES on si.ID_INSTITUCION equals i.ID_INSTITUCION
                     where si.ID_SECTOR == idSector || idSector == 0
                     select
                         new Institucion
                         {
                             IdInstitucion = i.ID_INSTITUCION,
                             NombreInstitucion = i.N_INSTITUCION,
                         });
            return a;
        }


        public IList<IInstitucion> GetInstitucionesBySector(int idSector)
        {

            return QInstitucion(idSector).OrderBy(o => o.NombreInstitucion).ToList();
        }

    }
}
