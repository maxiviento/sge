using System.Collections.Generic;
using System.Linq;
using SGE.Model.Entidades;
using SGE.Model.Entidades.Interfaces;
using SGE.Model.Repositorio;
using SGE.Repositorio.Modelo;

namespace SGE.Repositorio
{
    public class EstadoFichaRepositorio : IEstadoFichaRepositorio
    {
         private readonly DataSGE _mdb;

         public EstadoFichaRepositorio()
        {
            _mdb = new DataSGE();
        }

        private IQueryable<IEstadoFicha> QEstadosFicha()
        {
            var a = (from c in _mdb.T_ESTADOS_FICHA
                     select
                         new EstadoFicha
                             {
                                 Id_Estado_Ficha = c.ID_ESTADO_FICHA,
                                 Nombre_Estado_Ficha = c.N_ESTADO_FICHA
                             });

            return a;
        }

        public IList<IEstadoFicha> GetEstadosFicha()
        {
            return QEstadosFicha().OrderBy(o => o.Nombre_Estado_Ficha).ToList();
        }
    }
}
