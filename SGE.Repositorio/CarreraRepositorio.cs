using System;
using System.Collections.Generic;
using System.Linq;
using SGE.Model.Entidades;
using SGE.Model.Entidades.Interfaces;
using SGE.Model.Repositorio;
using SGE.Repositorio.Modelo;

namespace SGE.Repositorio
{
    public class CarreraRepositorio : ICarreraRepositorio
    {
        private readonly DataSGE _mdb;

        public CarreraRepositorio()
        {
            _mdb = new DataSGE();
        }

        private IQueryable<ICarrera> QCarrera()
        {
            var a = (from i in _mdb.T_CARRERAS
                     select
                         new Carrera
                             {
                                 IdCarrera = i.ID_CARRERA,
                                 NombreCarrera = i.N_CARRERA,
                                 IdSector = i.ID_SECTOR,
                                 IdNivel = i.ID_NIVEL,
                                 IdInstitucion = i.ID_INSTITUCION,
                                 NombreInstitucion = i.T_SECTOR_INSTITUCION.T_INSTITUCIONES.N_INSTITUCION,
                                 NombreSector = i.T_SECTOR_INSTITUCION.T_SECTOR.N_SECTOR,
                                 NombreNivel = i.T_NIVEL.N_NIVEL
                             });

            return a;
        }
        public IList<ICarrera> GetCarreras()
        {
            return QCarrera().ToList();
        }

        public IList<ICarrera> GetCarrerasByInstitucion(int? idInstitucion)
        {
            return QCarrera().Where(c => c.IdInstitucion == idInstitucion).ToList();
        }

        public IList<ICarrera> GetCarrerasByInstitucionBySector(int? idInstitucion, int? idSector)
        {
            return QCarrera().Where(c => c.IdInstitucion == idInstitucion && c.IdSector==idSector).ToList();
        }

        public int AddCarrera(ICarrera carrera)
        {
            var a = (from c in _mdb.T_CARRERAS
                     select c.ID_CARRERA);

            var model = new T_CARRERAS
                              {
                                  ID_CARRERA = (short) a.Max()+1,
                                  N_CARRERA = carrera.NombreCarrera,
                                  NIVEL = carrera.IdNivel == 1 ? "TERCIARIO" : "UNIVERSITARIO",
                                  ID_INSTITUCION = carrera.IdInstitucion,
                                  ID_NIVEL = carrera.IdNivel,
                                  ID_SECTOR = carrera.IdSector
                              };

            _mdb.T_CARRERAS.AddObject(model);
            _mdb.SaveChanges();
            return model.ID_CARRERA;
        }

        public ICarrera GetCarrera(int id)
        {
            return QCarrera().Where(c => c.IdCarrera == id).SingleOrDefault();
        }

        public IList<ICarrera> GetCarreras(int skip, int take)
        {
            return QCarrera().OrderBy(c => c.NombreCarrera).Skip(skip).Take(take).ToList();
        }

        private IQueryable<ICarrera> QGetCarreras(string descripcion)
        {
            descripcion = descripcion ?? String.Empty;

            return
                QCarrera().Where(
                    c =>
                    (c.NombreCarrera.ToLower().Contains(descripcion.ToLower()) || string.IsNullOrEmpty(descripcion))).OrderBy(c => c.NombreCarrera);
        }

        public IList<ICarrera> GetCarreras(string descripcion)
        {
            return QGetCarreras(descripcion).OrderBy(c => c.NombreCarrera).ToList();
        }

        public IList<ICarrera> GetCarreras(string descripcion, int skip, int take)
        {
            return QGetCarreras(descripcion).OrderBy(c => c.NombreCarrera).Skip(skip).Take(take).ToList();
        }

        public void UpdateCarrera(ICarrera carrera)
        {
            if (carrera.IdCarrera != 0)
            {
                var row = _mdb.T_CARRERAS.SingleOrDefault(c => c.ID_CARRERA == carrera.IdCarrera);

                if (row != null)
                {
                    row.ID_CARRERA = carrera.IdCarrera;
                    row.N_CARRERA = carrera.NombreCarrera;
                    row.NIVEL = carrera.IdNivel == 1 ? "TERCIARIO" : "UNIVERSITARIO";
                    row.ID_INSTITUCION = carrera.IdInstitucion;
                    row.ID_NIVEL = carrera.IdNivel;
                    row.ID_SECTOR = carrera.IdSector;

                    _mdb.SaveChanges();
                }
            }
        }

        public void DeleteCarrera(ICarrera carrera)
        {
            var row = _mdb.T_CARRERAS.SingleOrDefault(c => c.ID_CARRERA == carrera.IdCarrera);

            if (row.ID_CARRERA == 0) return;
            
            _mdb.DeleteObject(row);
            _mdb.SaveChanges();
        }

        public int GetCarrerasCount()
        {
            return QCarrera().Count();
        }

        public bool CarreraInUse(int idCarrera)
        {
            var t = (_mdb.T_FICHA_TERCIARIO.Where(c => c.ID_CARRERA == idCarrera)).Count() + (_mdb.T_FICHA_UNIVERSITARIO.Where(c=> c.ID_CARRERA == idCarrera).Count());
            return t>0;
        }


        public IList<ICarrera> GetCarreras(int Inst)
        {
            return QCarrera().Where(c => c.IdInstitucion == Inst).OrderBy(c => c.NombreCarrera).ToList();
        }
    }
}
