using System.Collections.Generic;
using System.Linq;
using SGE.Model.Entidades;
using SGE.Model.Entidades.Interfaces;
using SGE.Model.Repositorio;
using SGE.Repositorio.Modelo;

namespace SGE.Repositorio
{
    public class EstadoBeneficiarioRepositorio : IEstadoBeneficiarioRepositorio
    {
        private readonly DataSGE _mdb;

        public EstadoBeneficiarioRepositorio()
        {
            _mdb = new DataSGE();
        }

        private IEnumerable<IEstadoBeneficiario> QEstadoBeneficiario()
        {
            var a = (from c in _mdb.T_ESTADOS
                     select
                         new EstadoBeneficiario {Estado_Beneficiario = c.N_ESTADO, Id_Estado_Beneficiario = c.ID_ESTADO});
            return a;
        }


        public IList<IEstadoBeneficiario> GetEstadosBeneficiario()
        {
            return QEstadoBeneficiario().OrderBy(o => o.Estado_Beneficiario).ToList();
        }
    }
}
