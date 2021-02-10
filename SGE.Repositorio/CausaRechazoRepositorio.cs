using System;
using System.Collections.Generic;
using System.Linq;
using SGE.Model.Entidades;
using SGE.Model.Entidades.Interfaces;
using SGE.Model.Repositorio;
using SGE.Repositorio.Modelo;

namespace SGE.Repositorio
{
    public class CausaRechazoRepositorio : BaseRepositorio, ICausaRechazoRepositorio
    {
        private readonly DataSGE _mdb;

        public CausaRechazoRepositorio()
        {
            _mdb = new DataSGE();
        }

        private IQueryable<ICausaRechazo> QCausaRechazo()
        {
            var a = (from c in _mdb.T_CAUSA_RECHAZO
                     select
                         new CausaRechazo
                         {
                             IdCausaRechazo = c.ID_CAUSA_RECHAZO,
                             NombreCausaRechazo = c.N_CAUSA_RECHAZO,

                         });
            return a;
        }

        public int AddCausaRechazo(ICausaRechazo causaRechazo)
        {
            var model = new T_CAUSA_RECHAZO
                              {
                                  ID_CAUSA_RECHAZO = causaRechazo.IdCausaRechazo,
                                  N_CAUSA_RECHAZO = causaRechazo.NombreCausaRechazo,
                              };

            _mdb.T_CAUSA_RECHAZO.AddObject(model);
            _mdb.SaveChanges();
            return model.ID_CAUSA_RECHAZO;
        }

        public ICausaRechazo GetCausaRechazo(int id)
        {
            return QCausaRechazo().Where(c => c.IdCausaRechazo == id).SingleOrDefault();
        }

        public IList<ICausaRechazo> GetCausasRechazo()
        {
            return QCausaRechazo().OrderBy(c => c.IdCausaRechazo).ToList();
        }

        public int GetCausasRechazoCount()
        {
            return QCausaRechazo().Count();
        }

        public IList<ICausaRechazo> GetCausasRechazo(int skip, int take)
        {
            return GetCausasRechazo().Skip(skip).Take(take).ToList();
        }

        private IQueryable<ICausaRechazo> QGetCausasRechazo(string descripcion)
        {
            descripcion = descripcion ?? String.Empty;

            return
                QCausaRechazo().Where(
                    c =>
                    (c.NombreCausaRechazo.ToLower().Contains(descripcion.ToLower()) || String.IsNullOrEmpty(descripcion))).OrderBy(c => c.IdCausaRechazo);
        }

        public IList<ICausaRechazo> GetCausasRechazo(string descripcion)
        {
            return QGetCausasRechazo(descripcion).OrderBy(c => c.IdCausaRechazo).ToList();
        }

        public IList<ICausaRechazo> GetCausasRechazo(string descripcion, int skip, int take)
        {
            return GetCausasRechazo(descripcion).Skip(skip).Take(take).ToList();
        }

        public bool UpdateCausaRechazo(int idOld, ICausaRechazo causaRechazo)
        {
            var rowOld = _mdb.T_CAUSA_RECHAZO.SingleOrDefault(c => c.ID_CAUSA_RECHAZO == idOld);
            if (idOld == causaRechazo.IdCausaRechazo)
            {
                if (rowOld != null)
                {
                    rowOld.N_CAUSA_RECHAZO = causaRechazo.NombreCausaRechazo;
                }
            }
            else
            {
                if (GetCausaRechazo(causaRechazo.IdCausaRechazo) != null)
                    return false;

                var model = new T_CAUSA_RECHAZO
                                  {
                                      ID_CAUSA_RECHAZO = causaRechazo.IdCausaRechazo,
                                      N_CAUSA_RECHAZO = causaRechazo.NombreCausaRechazo,
                                  };

                _mdb.T_CAUSA_RECHAZO.DeleteObject(rowOld);
                _mdb.T_CAUSA_RECHAZO.AddObject(model);
            }
            _mdb.SaveChanges();
            return true;
        }

        public void DeleteCausaRechazo(ICausaRechazo causaRechazo)
        {
            var row = _mdb.T_CAUSA_RECHAZO.SingleOrDefault(c => c.ID_CAUSA_RECHAZO == causaRechazo.IdCausaRechazo);

            if (row.ID_CAUSA_RECHAZO != 0)
            {
                _mdb.DeleteObject(row);
                _mdb.SaveChanges();
            }
        }

        public bool ExistsCausaRechazo(string causaRechazo)
        {
            var t = QCausaRechazo().Where(c => 0 == causaRechazo.CompareTo(c.NombreCausaRechazo)).ToList().Count;
            return t > 0 ? true : false;
        }

        public int GetCausaRechazosCount()
        {
            return QCausaRechazo().Count();
        }

        public int GetCausaRechazoMaxId()
        {
            var a = (from c in _mdb.T_CAUSA_RECHAZO
                     select c.ID_CAUSA_RECHAZO);

            return a.Count() > 0 ? a.Max() : 0;
        }

        public bool CausaRechazoInUse(int idCausaRechazo)
        {
            var t = (_mdb.T_RECHAZOS.Where(c => c.ID_CAUSA_RECHAZO == idCausaRechazo).Count());
            return t > 0;
        }
    }
}