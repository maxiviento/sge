using System.Collections.Generic;
using System.Linq;
using SGE.Model.Entidades;
using SGE.Model.Entidades.Interfaces;
using SGE.Model.Repositorio;
using SGE.Repositorio.Modelo;

namespace SGE.Repositorio
{
    public class EstadoLiquidacionRepositorio : IEstadoLiquidacionRepositorio
    {
        private readonly DataSGE _mdb;

        public EstadoLiquidacionRepositorio()
        {
            _mdb = new DataSGE();
        }

        private IEnumerable<IEstadoLiquidacion> QEstadoLiquidacion()
        {
            var a = (from c in _mdb.T_ESTADOS_LIQUIDACION
                     select new EstadoLiquidacion {IdEstadoLiquidacion = c.ID_ESTADO_LIQ, NombreEstado = c.N_ESTADO_LIQ});

            return a;
        }

        public IList<IEstadoLiquidacion> GetEstadosLiquidacion()
        {
            return QEstadoLiquidacion().OrderBy(o => o.NombreEstado).ToList();
        }
    }
}
