using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SGE.Model.Comun;
using SGE.Model.Entidades;
using SGE.Model.Entidades.Interfaces;
using SGE.Model.Repositorio;
using SGE.Repositorio.Modelo;
using System.Data.Common;

namespace SGE.Repositorio
{
    public class AbandonoCursadoRepositorio : IAbandonoCursadoRepositorio
    {
        private readonly DataSGE db;

        public AbandonoCursadoRepositorio(DataSGE db)
        {
            this.db = db;
        }

        public AbandonoCursadoRepositorio()
        {
            this.db = new DataSGE();
        }
        public IAbandonoCursado GetAbandonoCursado(int idAbandonoCursado)
        {
            return QAbandonoCursado().Where(x => x.ID_ABANDONO_CUR == idAbandonoCursado).SingleOrDefault();
        }
        private IQueryable<IAbandonoCursado> QAbandonoCursado()
        {
            var a = (from ab in this.db.T_ABANDONO_CURSADO
                select new AbandonoCursado{ID_ABANDONO_CUR=ab.ID_ABANDONO_CUR,PERIODO=ab.PERIODO});

            return a;
        }


        public IList<IAbandonoCursado> GetAll()
        {
            return QAbandonoCursado().OrderBy(x => x.PERIODO).ToList();
        }

        public IList<IAbandonoCursado> GetAllcbo()
        {
            return QAbandonoCursado().Take(10).OrderBy(x => x.PERIODO).ToList();
        }
    }
}
