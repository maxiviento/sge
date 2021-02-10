using System.Linq;
using SGE.Model.Entidades;
using SGE.Model.Entidades.Interfaces;
using SGE.Model.Repositorio;
using SGE.Repositorio.Modelo;

namespace SGE.Repositorio
{
    public class FichaRechazoRepositorio : BaseRepositorio, IFichaRechazoRepositorio
    {
        private readonly DataSGE _mdb;

        public FichaRechazoRepositorio()
        {
            _mdb = new DataSGE();
            _mdb.ContextOptions.LazyLoadingEnabled = false;
        }

        public IFichaRechazo GetFichaTipoRechazo(int idFicha, int tipoRechazo)
        {
            IFicha ficha =
                _mdb.T_FICHAS.Where(c => c.ID_FICHA == idFicha).Select(
                    c =>
                    new Ficha
                        {
                            IdFicha = c.ID_FICHA,
                            TipoRechazos =
                                c.T_FICHAS_RECHAZO.Select(tp => new TipoRechazo { IdTipoRechazo = tp.ID_TIPO_RECHAZO }).
                                AsEnumerable()
                        }).
                    SingleOrDefault();

            ITipoRechazo rechazo =
                ficha.TipoRechazos.Where(c => c.IdTipoRechazo == tipoRechazo).Select(
                    c => new TipoRechazo { IdTipoRechazo = c.IdTipoRechazo }).SingleOrDefault();

            if (rechazo != null)
            {
                IFichaRechazo ficharechazo = new FichaRechazo
                                                 {
                                                     IdFicha = idFicha,
                                                     IdTipoRechazo = tipoRechazo
                                                 };

                return ficharechazo;
            }

            return null;
        }

        public void AddFichaTipoRechazo(IFichaRechazo fichaRechazo)
        {
            AgregarDatos(fichaRechazo);

            var ficharechazo = new T_FICHAS_RECHAZO
                                   {
                                       ID_FICHA = fichaRechazo.IdFicha,
                                       ID_TIPO_RECHAZO = fichaRechazo.IdTipoRechazo,
                                       ID_USR_SIST = fichaRechazo.IdUsuarioSistema,
                                       FEC_SIST = fichaRechazo.FechaSistema
                                   };

            _mdb.T_FICHAS_RECHAZO.AddObject(ficharechazo);

            _mdb.SaveChanges();
        }

        public void DeleteFichaTipoRechazo(IFichaRechazo fichaRechazo)
        {
            AgregarDatos(fichaRechazo);

            var rechazos =
                _mdb.T_FICHAS_RECHAZO.Where(
                    c => c.ID_FICHA == fichaRechazo.IdFicha && c.ID_TIPO_RECHAZO == fichaRechazo.IdTipoRechazo).
                    FirstOrDefault();

            if (rechazos == null) return;
            
            rechazos.ID_USR_SIST = fichaRechazo.IdUsuarioSistema;

            _mdb.T_FICHAS_RECHAZO.DeleteObject(rechazos);

            _mdb.SaveChanges();
        }
    }
}
