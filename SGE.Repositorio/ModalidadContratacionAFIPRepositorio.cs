using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SGE.Model.Entidades;
using SGE.Model.Entidades.Interfaces;
using SGE.Model.Repositorio;
using SGE.Repositorio.Modelo;

namespace SGE.Repositorio
{
    public class ModalidadContratacionAFIPRepositorio : IModalidadContratacionAFIPRepositorio
    {
        private readonly DataSGE _mdb;

        private IQueryable<IModalidadContratacionAFIP> QModalidadContratacionAFIP()
        {
            return
                _mdb.T_MOD_CONT_AFIP.Select(
                    c =>
                    new ModalidadContratacionAFIP { IdModalidadAFIP = c.ID_MOD_CONT_AFIP, ModalidadAFIP = c.N_MOD_CONT_AFIP });

        }

        public ModalidadContratacionAFIPRepositorio()
        {
            _mdb = new DataSGE();
        }

        public IList<IModalidadContratacionAFIP> GetModalidadesContratacionAFIP()
        {
            return QModalidadContratacionAFIP().ToList();
        }
    }
}
