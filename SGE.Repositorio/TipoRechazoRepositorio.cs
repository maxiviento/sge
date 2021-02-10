using System;
using System.Collections.Generic;
using System.Linq;
using SGE.Model.Entidades;
using SGE.Model.Entidades.Interfaces;
using SGE.Repositorio.Modelo;
using SGE.Model.Repositorio;

namespace SGE.Repositorio
{
    public class TipoRechazoRepositorio : ITipoRechazoRepositorio
    {
        private readonly DataSGE _mdb;

        public TipoRechazoRepositorio()
        {
            _mdb = new DataSGE();
        }

        private IQueryable<ITipoRechazo> QTipoRechazo()
        {
            var a = (from c in _mdb.T_TIPOS_RECHAZO
                     select
                         new TipoRechazo
                         {
                            IdTipoRechazo = c.ID_TIPO_RECHAZO,
                            NombreTipoRechazo= c.N_TIPO_RECHAZO
                         });
            return a;
        }

        public int AddTipoRechazo(ITipoRechazo tipoRechazo)
        {
            var model = new T_TIPOS_RECHAZO
                              {
                                  ID_TIPO_RECHAZO = tipoRechazo.IdTipoRechazo,
                                  N_TIPO_RECHAZO = tipoRechazo.NombreTipoRechazo
                              };

            _mdb.T_TIPOS_RECHAZO.AddObject(model);
            _mdb.SaveChanges();
            return model.ID_TIPO_RECHAZO;
        }

        public ITipoRechazo GetTipoRechazo(int id)
        {
            return QTipoRechazo().Where(c => c.IdTipoRechazo== id).SingleOrDefault();
        }

        public IList<ITipoRechazo> GetTiposRechazo()
        {
            return QTipoRechazo().OrderBy(c=>c.IdTipoRechazo).ToList();
        }

        public IList<ITipoRechazo> GetTiposRechazo(int skip, int take)
        {
            return QTipoRechazo().OrderBy(c => c.IdTipoRechazo).Skip(skip).Take(take).ToList();
        }

        private IQueryable<ITipoRechazo> QGetTiposRechazo(string descripcion)
        {
            descripcion = descripcion ?? String.Empty;
            
            return
                QTipoRechazo().Where(
                    c =>
                    (c.NombreTipoRechazo.ToLower().Contains(descripcion.ToLower()) || String.IsNullOrEmpty(descripcion))).OrderBy(c => c.IdTipoRechazo);
        }

        public IList<ITipoRechazo> GetTiposRechazo(string descripcion)
        {
            return QGetTiposRechazo(descripcion).ToList();
        }

        public IList<ITipoRechazo> GetTiposRechazo(string descripcion, int skip, int take)
        {
            return QGetTiposRechazo(descripcion).Skip(skip).Take(take).ToList();
        }

        public int GetTiposRechazoCount()
        {
            return QTipoRechazo().Count();
        }

        public int GetTipoRechazoMaxId()
        {
            var a = (from c in _mdb.T_TIPOS_RECHAZO
                     select c.ID_TIPO_RECHAZO);

            return a.Max();
        }

        public bool UpdateTipoRechazo(int idOld, ITipoRechazo tipoRechazo)
        {
            var rowOld = _mdb.T_TIPOS_RECHAZO.SingleOrDefault(c => c.ID_TIPO_RECHAZO == idOld);
            if (idOld== tipoRechazo.IdTipoRechazo)
            {
                if (rowOld != null)
                {
                    rowOld.N_TIPO_RECHAZO= tipoRechazo.NombreTipoRechazo;
                }
            }
            else
            {
                if(GetTipoRechazo(tipoRechazo.IdTipoRechazo)!=null)
                    return false;
                _mdb.T_TIPOS_RECHAZO.DeleteObject(rowOld);
                var model = new T_TIPOS_RECHAZO
                                  {
                                      ID_TIPO_RECHAZO = tipoRechazo.IdTipoRechazo,
                                      N_TIPO_RECHAZO = tipoRechazo.NombreTipoRechazo,
                                  };

                _mdb.T_TIPOS_RECHAZO.AddObject(model);
            }
            _mdb.SaveChanges();
            return true;
        }

        public void DeleteTipoRechazo(ITipoRechazo tipoRechazo)
        {
            var row = _mdb.T_TIPOS_RECHAZO.SingleOrDefault(c => c.ID_TIPO_RECHAZO == tipoRechazo.IdTipoRechazo);

            if (row.ID_TIPO_RECHAZO == 0) return;
            
            _mdb.DeleteObject(row);
            _mdb.SaveChanges();
        }

        public bool ExistsTipoRechazo(string tipoRechazo)
        {
            var t = QTipoRechazo().Where(c => 0 == tipoRechazo.CompareTo(c.NombreTipoRechazo)).ToList().Count;
            return t > 0 ? true : false;
        }

        public int GetTipoRechazosCount()
        {
            return QTipoRechazo().Count();
        }

        public bool TipoRechazoInUse(int idTipoRechazo)
        {
            //var t = _mdb.T_FICHAS_RECHAZO.Where(c => c.ID_TIPO_RECHAZO == idTipoRechazo).Count();
            //return t>0;
            return false;
        }
    }
}