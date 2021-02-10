using System;
using System.Collections.Generic;
using System.Linq;
using SGE.Model.Entidades;
using SGE.Model.Entidades.Interfaces;
using SGE.Model.Repositorio;
using SGE.Repositorio.Modelo;

namespace SGE.Repositorio
{
    public class SubprogramaRepositorio : BaseRepositorio, ISubprogramaRepositorio
    { 
        private readonly DataSGE _mdb;

        public SubprogramaRepositorio()
        {
            _mdb = new DataSGE();
        }

        private IQueryable<ISubprograma> QSubprograma()
        {
            //QUEDA ASI HASTA CREAR LA TABLA SUBPROGRAMAS
            var a = (from c in _mdb.T_SUBPROGRAMAS
                     from p in _mdb.T_PROGRAMAS.Where(p => p.ID_PROGRAMA == c.ID_PROGRAMA).DefaultIfEmpty()
                     select
                         new Subprograma()
                         {
                             IdSubprograma = c.ID_SUBPROGRAMA,
                             NombreSubprograma = c.N_SUBPROGRAMA,
                             Descripcion = c.DESCRIPCION,
                             IdUsuarioSistema = c.ID_USR_SIST,
                             UsuarioSistema = c.T_USUARIOS.LOGIN,
                             monto = c.MONTO_SUB ?? 0,
                             IdPrograma = c.ID_PROGRAMA ?? 0,
                             Programa = p.N_PROGRAMA

                         });
            return a;
        }

        private IQueryable<ISubprograma> QGetSubprogramas(string descripcion)
        {
            descripcion = descripcion ?? String.Empty;

            return
                QSubprograma().Where(
                    c =>
                    (c.NombreSubprograma.ToLower().Contains(descripcion.ToLower()) || String.IsNullOrEmpty(descripcion))).
                    OrderBy(o => o.IdSubprograma);
        }

        public IList<ISubprograma> GetSubprogramas()
        {
            return QSubprograma().OrderBy(c => c.IdSubprograma).ToList();
        }

        public IList<ISubprograma> GetSubprogramas(int skip, int take)
        {
            return QSubprograma().OrderBy(c => c.IdSubprograma).Skip(skip).Take(take).ToList();
        }

        public IList<ISubprograma> GetSubprogramas(string descripcion)
        {
            return QGetSubprogramas(descripcion).OrderBy(c => c.IdSubprograma).ToList();
        }

        public IList<ISubprograma> GetSubprogramas(string descripcion, int skip, int take)
        {
            return QGetSubprogramas(descripcion).OrderBy(c => c.IdSubprograma).Skip(skip).Take(take).ToList();
        }

        public ISubprograma GetSubprograma(int id)
        {
            return QSubprograma().Where(c => c.IdSubprograma == id).SingleOrDefault();
        }

        public int AddSubprograma(ISubprograma subprograma)
        {
            AgregarDatos(subprograma);

            //cambiar nombre tabla
            var model = new T_SUBPROGRAMAS()
            {
                ID_SUBPROGRAMA = SecuenciaRepositorio.GetId(),
                N_SUBPROGRAMA = subprograma.NombreSubprograma,
                DESCRIPCION= subprograma.Descripcion,
                ID_USR_SIST = subprograma.IdUsuarioSistema,
                FEC_SIST = subprograma.FechaSistema,
                MONTO_SUB  = subprograma.monto > 0 ? (decimal?)subprograma.monto : null,
                ID_PROGRAMA = subprograma.IdPrograma == 0 ? null : subprograma.IdPrograma,
            };

            _mdb.T_SUBPROGRAMAS.AddObject(model);
            _mdb.SaveChanges();
            return model.ID_SUBPROGRAMA;
        }

        public void UpdateSubprograma(ISubprograma subprograma)
        {
            AgregarDatos(subprograma);

            if (subprograma.IdSubprograma != 0)
            {
                //cambiar la tabla y campos
                var row = _mdb.T_SUBPROGRAMAS.SingleOrDefault(c => c.ID_SUBPROGRAMA == subprograma.IdSubprograma);

                if (row != null)
                {
                    row.N_SUBPROGRAMA = subprograma.NombreSubprograma;
                    row.DESCRIPCION = subprograma.Descripcion;
                    row.ID_USR_SIST = subprograma.IdUsuarioSistema;
                    row.FEC_SIST = subprograma.FechaSistema;
                    row.MONTO_SUB = subprograma.monto > 0 ? (decimal?)subprograma.monto : null;
                    row.ID_PROGRAMA = subprograma.IdPrograma == 0 ? null : subprograma.IdPrograma;
                    _mdb.SaveChanges();
                }
            }
        }
        public int GetSubprogramasCount()
        {
            return QSubprograma().Count();
        }

        private IQueryable<ISubprogramaRol> QSubprogramaRol()
        {
            var a = (from s in _mdb.T_SUBPROGRAMAS
                    join sr in _mdb.T_SUBPROGRAMAS_ROL on s.ID_SUBPROGRAMA equals sr.ID_SUBPROGRAMA

                    select new SubprogramaRol
                    {
                        IdSubprograma = s.ID_SUBPROGRAMA,
                        NombreSubprograma = s.N_SUBPROGRAMA,
                        IdPrograma = (short)sr.ID_PROGRAMA,
                        Id_Rol = sr.ID_ROL


                    });

            return a;
        }

        public IList<ISubprogramaRol> GetSubprogramasRol()
        {
           return QSubprogramaRol().OrderBy(c=>c.IdSubprograma).ToList();
    
        }

        public List<Subprograma> GetSubprogr()
        {
            return QSubprogramas().OrderBy(c => c.IdSubprograma).ToList();
        }


        private IQueryable<Subprograma> QSubprogramas()
        {
            //QUEDA ASI HASTA CREAR LA TABLA SUBPROGRAMAS
            var a = (from c in _mdb.T_SUBPROGRAMAS
                     from p in _mdb.T_PROGRAMAS.Where(p => p.ID_PROGRAMA == c.ID_PROGRAMA).DefaultIfEmpty()
                     select
                         new Subprograma()
                         {
                             IdSubprograma = c.ID_SUBPROGRAMA,
                             NombreSubprograma = c.N_SUBPROGRAMA,
                             Descripcion = c.DESCRIPCION,
                             IdUsuarioSistema = c.ID_USR_SIST,
                             UsuarioSistema = c.T_USUARIOS.LOGIN,
                             monto = c.MONTO_SUB ?? 0,
                             IdPrograma = c.ID_PROGRAMA ?? 0,
                             Programa = p.N_PROGRAMA

                         });
            return a;
        }

        public List<Subprograma> GetSubprogramas(int idPrograma)
        {
            return QSubprogramas(idPrograma).OrderBy(c => c.NombreSubprograma).ToList();
        }

        private IQueryable<Subprograma> QSubprogramas(int idPrograma)
        {
            
            var a = (from c in _mdb.T_SUBPROGRAMAS
                     from p in _mdb.T_PROGRAMAS.Where(p => p.ID_PROGRAMA == c.ID_PROGRAMA).DefaultIfEmpty()
                     where c.ID_PROGRAMA == idPrograma
                     select
                         new Subprograma()
                         {
                             IdSubprograma = c.ID_SUBPROGRAMA,
                             NombreSubprograma = c.N_SUBPROGRAMA,
                             Descripcion = c.DESCRIPCION,
                             IdUsuarioSistema = c.ID_USR_SIST,
                             UsuarioSistema = c.T_USUARIOS.LOGIN,
                             monto = c.MONTO_SUB ?? 0,
                             IdPrograma = c.ID_PROGRAMA ?? 0,
                             Programa = p.N_PROGRAMA

                         });
            return a;
        }


        public int udpSubprogramaFichas(IList<IFicha> fichas, int idSubprograma)
        {
            int cant = 0;
            try
            {
                foreach (Ficha item in fichas)
                {
                    IComunDatos comun = new ComunDatos();

                    AgregarDatos(comun);

                    var f = _mdb.T_FICHA_PPP.FirstOrDefault(c => c.ID_FICHA == item.IdFicha);

                    f.ID_SUBPROGRAMA = idSubprograma;
                    f.ID_USR_MODIF = comun.IdUsuarioSistema;
                    f.FEC_MODIF = comun.FechaSistema;

                    _mdb.SaveChanges();

                    cant++;
                    item.Subprograma = "Cargado Correctamente";

                }
                return cant;
            }
            catch (Exception ex)
            {
                return cant;
            }
            finally
            {

            }

        }
    }
}
