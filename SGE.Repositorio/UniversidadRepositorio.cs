using System;
using System.Collections.Generic;
using System.Linq;
using SGE.Model.Entidades;
using SGE.Model.Entidades.Interfaces;
using SGE.Model.Repositorio;
using SGE.Repositorio.Modelo;

namespace SGE.Repositorio
{
    public class UniversidadRepositorio : IUniversidadRepositorio
    {
        private readonly DataSGE _mdb;

        public UniversidadRepositorio()
        {
            _mdb = new DataSGE();
        }

        private IQueryable<IUniversidad> QUniversidad()
        {
            var a = (from c in _mdb.T_UNIVERSIDAD
                     select
                         new Universidad
                         {
                            IdUniversidad = c.ID_UNIVERSIDAD,
                            NombreUniversidad= c.N_UNIVERSIDAD
                         });
            return a;
        }

        public int AddUniversidad(IUniversidad universidad)
        {
            var model = new T_UNIVERSIDAD
                            {
                                ID_UNIVERSIDAD = (short) universidad.IdUniversidad,
                                N_UNIVERSIDAD= universidad.NombreUniversidad,
                            };

            _mdb.T_UNIVERSIDAD.AddObject(model);
            _mdb.SaveChanges();
            return model.ID_UNIVERSIDAD;
        }

        public IUniversidad GetUniversidad(int id)
        {
            return QUniversidad().Where(c => c.IdUniversidad== id).SingleOrDefault();
        }

        public IList<IUniversidad> GetUniversidades()
        {
            return QUniversidad().OrderBy(c=>c.NombreUniversidad).ToList();
        }

        public IList<IUniversidad> GetUniversidades(int skip, int take)
        {
            return QUniversidad().OrderBy(c => c.NombreUniversidad).Skip(skip).Take(take).ToList();
        }

        private IQueryable<IUniversidad> QGetUniversidades(string descripcion)
        {
            descripcion = descripcion ?? String.Empty;
            
            return
                QUniversidad().Where(
                    c =>
                    (c.NombreUniversidad.ToLower().Contains(descripcion.ToLower()) || String.IsNullOrEmpty(descripcion))).
                    OrderBy(o => o.NombreUniversidad);
        }

        public IList<IUniversidad> GetUniversidades(string descripcion)
        {
            return QGetUniversidades(descripcion).OrderBy(c => c.NombreUniversidad).ToList();
        }

        public IList<IUniversidad> GetUniversidades(string descripcion, int skip, int take)
        {
            return QGetUniversidades(descripcion).OrderBy(c => c.NombreUniversidad).Skip(skip).Take(take).ToList();
        }

        public void UpdateUniversidad(IUniversidad universidad)
        {
            if (universidad.IdUniversidad!= 0)
            {
                var row = _mdb.T_UNIVERSIDAD.SingleOrDefault(c => c.ID_UNIVERSIDAD == universidad.IdUniversidad);

                if (row != null)
                {
                    row.N_UNIVERSIDAD= universidad.NombreUniversidad;

                    _mdb.SaveChanges();
                }
            }
        }

        public int GetUniversidadMaxId()
        {
            var a = (from c in _mdb.T_UNIVERSIDAD
                    select c.ID_UNIVERSIDAD);

            return a.Max();
        }

        public int GetUniversidadesCount()
        {
            return QUniversidad().Count();
        }
    }
}