using System.Collections.Generic;
using System.Linq;
using SGE.Model.Entidades;
using SGE.Model.Entidades.Interfaces;
using SGE.Model.Repositorio;
using SGE.Repositorio.Modelo;
using System;

namespace SGE.Repositorio
{
    public class TituloRepositorio: ITituloRepositorio
    {
        private readonly DataSGE _mdb;

        public TituloRepositorio()
        {
            _mdb = new DataSGE();
        }

        private IQueryable<ITitulo> QTitulos()
        {
            var a = (from c in _mdb.T_TITULOS
                     select
                         new Titulo
                             {
                                 IdTitulo = c.ID_TITULO,
                                 IdUniversidad = c.ID_UNIVERSIDAD,
                                 Descripcion = c.N_TITULO
                             });

            return a;
        }

        public IList<ITitulo> GetTitulos()
        {
            return QTitulos().OrderBy(c => c.Descripcion).ToList();
        }

        public IList<ITitulo> GetTitulos(int skip, int take)
        {
            return QTitulos().OrderBy(c => c.Descripcion).Skip(skip).Take(take).ToList();
        }

        public IList<ITitulo> GetTitulos(string descripcion)
        {
            descripcion = descripcion ?? String.Empty;
            
            return
                QTitulos().Where(
                    c =>
                    (c.Descripcion.ToLower().Contains(descripcion.ToLower()) || String.IsNullOrEmpty(descripcion))).
                    OrderBy(o => o.Descripcion.ToLower()).ToList();
        }

        public IList<ITitulo> GetTitulos(string descripcion, int skip, int take)
        {
            descripcion = descripcion ?? String.Empty;

            return
                QTitulos().Where(
                    c =>
                    (c.Descripcion.ToLower().Contains(descripcion.ToLower()) || String.IsNullOrEmpty(descripcion))).
                    OrderBy(o => o.Descripcion.ToLower()).Skip(skip).Take(take).ToList();
        }

        public ITitulo GetTitulo(int id)
        {
            return QTitulos().Where(c => c.IdTitulo == id).SingleOrDefault();
        }

        public int AddTitulo(ITitulo titulo)
        {
            var tituloModel = new T_TITULOS
                                  {
                                      ID_TITULO = Convert.ToInt16(SecuenciaRepositorio.GetId()),
                                      N_TITULO = titulo.Descripcion,
                                      ID_UNIVERSIDAD = titulo.IdUniversidad
                                  };

            _mdb.T_TITULOS.AddObject(tituloModel);
            _mdb.SaveChanges();
            return tituloModel.ID_TITULO;
        }

        public void UpdateTitulo(ITitulo titulo)
        {
            var obj = _mdb.T_TITULOS.SingleOrDefault(c => c.ID_TITULO == titulo.IdTitulo);

            if (obj != null)
            {
                
                obj.ID_UNIVERSIDAD = titulo.IdUniversidad;
                obj.N_TITULO = titulo.Descripcion;

                _mdb.SaveChanges();
            }
        }

        public void DeleteTitulo(ITitulo titulo)
        {
            var obj = _mdb.T_TITULOS.SingleOrDefault(c => c.ID_TITULO == titulo.IdTitulo);

            if (obj != null)
            {
                _mdb.DeleteObject(obj);
                _mdb.SaveChanges();
            }
        }

        public int GetTituloCount()
        {
            return QTitulos().Count();
        }

        public bool ExistsTitulo(string titulo)
        {
            var t = QTitulos().Where(c => 0 == titulo.CompareTo(c.Descripcion)).ToList().Count;
            return t > 0 ? true : false;
        }

        public IList<ITitulo> GetTitulosByUniversidad(int iduniversidad)
        {
            return QTitulos().Where(c => c.IdUniversidad == iduniversidad).ToList();
        }
    }
}
