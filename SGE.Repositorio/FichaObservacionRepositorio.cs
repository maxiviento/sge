using System.Collections.Generic;
using System.Linq;
using SGE.Model.Entidades;
using SGE.Model.Entidades.Interfaces;
using SGE.Model.Repositorio;
using SGE.Repositorio.Modelo;

namespace SGE.Repositorio
{
    public class FichaObservacionRepositorio : BaseRepositorio, IFichaObservacionRepositorio
    {
        private readonly DataSGE _mdb;

        public FichaObservacionRepositorio()
        {
            _mdb = new DataSGE();
        }

        private IQueryable<IFichaObservacion> QFichaObservacion()
        {
            var a = (from c in _mdb.T_FICHAS_OBS
                     select
                         new FichaObservacion
                             {
                                 IdFichaObservacion = c.ID_FICHA_OBS,
                                 IdFicha = c.ID_FICHA,
                                 Observacion = c.OBSERVACION,
                                 FechaSistema = c.FEC_SIST,
                                 IdUsuarioSistema = c.ID_USR_SIST,
                                 UsuarioSistema = c.T_USUARIOS.LOGIN
                             });
            return a;
        }
        //obtiene la observacion para upd o del.
        public IFichaObservacion GetObservacion(int idFicha, int idFichaObservacion)
        {
            return
                QFichaObservacion().Where(
                    c =>
                    (c.IdFichaObservacion == idFichaObservacion) && (c.IdFicha == idFicha)).SingleOrDefault();
        }

        //obtiene listado de observaciones correspondientes a una ficha
        public IList<IFichaObservacion> GetObservacionesByFicha(int idFicha)
        {
            return QFichaObservacion().Where(c => c.IdFicha == idFicha)
                  .OrderByDescending(q => q.IdFichaObservacion).ToList();
        }

        //obtiene listado de observaciones correspondientes a una ficha para paginado
        public IList<IFichaObservacion> GetObservacionesByFicha(int idFicha, int skip, int take)
        {
            return QFichaObservacion().Where(c => c.IdFicha == idFicha).OrderByDescending(q => q.IdFichaObservacion)
                   .Skip(skip).Take(take).ToList();
        }

        //cant de observaciones de una ficha
        public int GetCountObservaciones(int idFicha)
        {
            return QFichaObservacion().Where(c => c.IdFicha == idFicha).Count();
        }

        //Agrega nueva observacion a ficha
        public int AddObservacion(IFichaObservacion fichaObservacion)
        {
            AgregarDatos(fichaObservacion);

            var obsModel = new T_FICHAS_OBS
                               {
                                   ID_FICHA_OBS = SecuenciaRepositorio.GetId(),
                                   ID_FICHA = fichaObservacion.IdFicha,
                                   OBSERVACION = fichaObservacion.Observacion,
                                   FEC_SIST = fichaObservacion.FechaSistema,
                                   ID_USR_SIST = fichaObservacion.IdUsuarioSistema
                               };

            _mdb.T_FICHAS_OBS.AddObject(obsModel);
            _mdb.SaveChanges();

            return obsModel.ID_FICHA_OBS;
        }

        //update a observacion correspondiente a una ficha 
        public void UpdateObservacion(IFichaObservacion fichaObservacion)
        {
            AgregarDatos(fichaObservacion);

            var obj = _mdb.T_FICHAS_OBS.SingleOrDefault(c => c.ID_FICHA_OBS == fichaObservacion.IdFichaObservacion);

            if (obj == null) return;

            obj.OBSERVACION = fichaObservacion.Observacion;
            obj.FEC_SIST = fichaObservacion.FechaSistema;
            obj.ID_USR_SIST = fichaObservacion.IdUsuarioSistema;

            _mdb.SaveChanges();
        }

        //Elimina la observacion
        public void DeleteObservacion(IFichaObservacion fichaObservacion)
        {
            AgregarDatos(fichaObservacion);

            var obj = _mdb.T_FICHAS_OBS.SingleOrDefault(c => c.ID_FICHA_OBS == fichaObservacion.IdFichaObservacion);

            if (obj == null) return;

            obj.ID_USR_SIST = fichaObservacion.IdUsuarioSistema;
            _mdb.DeleteObject(obj);
            _mdb.SaveChanges();
        }
    }
}
