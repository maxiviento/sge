using System.Collections.Generic;
using System.Linq;
using SGE.Model.Entidades;
using SGE.Model.Entidades.Interfaces;
using SGE.Model.Repositorio;
using SGE.Repositorio.Modelo;

namespace SGE.Repositorio
{
    public class EstadoApoderadoRepositorio: IEstadoApoderadoRepositorio
    {
        private static DataSGE _mdb;

        public EstadoApoderadoRepositorio()
        {
            _mdb = new DataSGE();
        }

        private static IQueryable<IEstadoApoderado> QEstadoApoderado()
        {
            var a = (_mdb.T_ESTADOS_APODERADO.Select(
                c =>
                new EstadoApoderado
                    {NombreEstadoApoderado = c.N_ESTADO_APODERADO, IdEstadoApoderado = c.ID_ESTADO_APODERADO}));
            return a;
        }

        public IList<IEstadoApoderado> GetEstadosApoderado()
        {
            return QEstadoApoderado().OrderBy(c => c.NombreEstadoApoderado).ToList();
        }
    }
}
