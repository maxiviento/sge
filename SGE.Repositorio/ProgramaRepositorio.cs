using System;
using System.Collections.Generic;
using System.Linq;
using SGE.Model.Entidades;
using SGE.Model.Entidades.Interfaces;
using SGE.Model.Repositorio;
using SGE.Repositorio.Modelo;
using System.Collections;

namespace SGE.Repositorio
{
    public class ProgramaRepositorio : BaseRepositorio, IProgramaRepositorio
    {
        private readonly DataSGE _mdb;

        public ProgramaRepositorio()
        {
            _mdb = new DataSGE();
        }

        private IQueryable<IPrograma> QPrograma()
        {
            var a = (from c in _mdb.T_PROGRAMAS
                     select
                         new Programa
                         {
                             IdPrograma = c.ID_PROGRAMA,
                             NombrePrograma = c.N_PROGRAMA,
                             ConvenioBcoCba = c.CONVENIO_BCO_CBA,
                             MontoPrograma = c.MONTO_PROG ?? 0,
                             FechaSistema = c.FEC_SIST,
                             IdUsuarioSistema = c.ID_USR_SIST,
                             UsuarioSistema = c.T_USUARIOS.LOGIN
                         });
            return a;
        }

        public int AddPrograma(IPrograma programa)
        {
            AgregarDatos(programa);
            
            var model = new T_PROGRAMAS
                            {
                                ID_PROGRAMA = (short)programa.IdPrograma,
                                N_PROGRAMA = programa.NombrePrograma,
                                CONVENIO_BCO_CBA = programa.ConvenioBcoCba,
                                MONTO_PROG = programa.MontoPrograma,
                                ID_USR_SIST = programa.IdUsuarioSistema,
                                FEC_SIST = programa.FechaSistema
                            };

            _mdb.T_PROGRAMAS.AddObject(model);
            _mdb.SaveChanges();
            return model.ID_PROGRAMA;
        }

        public IPrograma GetPrograma(int id)
        {
            return QPrograma().Where(c => c.IdPrograma == id).SingleOrDefault();
        }

        public IList<IPrograma> GetProgramas()
        {
            return QPrograma().OrderBy(c => c.IdPrograma).ToList();
        }

        public IList<IPrograma> GetProgramas(int skip, int take)
        {
            return QPrograma().OrderBy(c => c.IdPrograma).Skip(skip).Take(take).ToList();
        }

        private IQueryable<IPrograma> QGetProgramas(string descripcion)
        {
            descripcion = descripcion ?? String.Empty;

            return
                QPrograma().Where(
                    c =>
                    (c.NombrePrograma.ToLower().Contains(descripcion.ToLower()) || String.IsNullOrEmpty(descripcion))).
                    OrderBy(o => o.IdPrograma);
        }

        public IList<IPrograma> GetProgramas(string descripcion)
        {
            return QGetProgramas(descripcion).OrderBy(c => c.IdPrograma).ToList();
        }

        public IList<IPrograma> GetProgramas(string descripcion, int skip, int take)
        {
            return QGetProgramas(descripcion).OrderBy(c => c.IdPrograma).Skip(skip).Take(take).ToList();
        }

        public void UpdatePrograma(IPrograma programa)
        {
            AgregarDatos(programa);
            
            if (programa.IdPrograma != 0)
            {
                var row = _mdb.T_PROGRAMAS.SingleOrDefault(c => c.ID_PROGRAMA == programa.IdPrograma);

                if (row != null)
                {
                    row.N_PROGRAMA = programa.NombrePrograma;
                    row.MONTO_PROG = programa.MontoPrograma;
                    row.CONVENIO_BCO_CBA = programa.ConvenioBcoCba;
                    row.ID_USR_SIST = programa.IdUsuarioSistema;

                    _mdb.SaveChanges();
                }
            }
        }

        public int GetProgramaMaxId()
        {
            var a = (from c in _mdb.T_PROGRAMAS
                     select c.ID_PROGRAMA);

            return a.Max();

        }

        public int GetProgramasCount()
        {
            return QPrograma().Count();
        }


        private IQueryable<IConvenio> Qconvenios()
        {
            var a = (from c in _mdb.T_CONVENIOS
                     select
                         new Convenio
                         {
                             IdConvenio = c.ID_CONVENIO,
                             NConvenio = c.N_CONVENIO
                         });
            return a;
        }

        public IList<IConvenio> GetConvenios()
        {
            return Qconvenios().OrderBy(c=>c.IdConvenio).ToList();
        }

        public IConvenio GetConvenio(int idConvenio)
        {
            return Qconvenios().Where(c => c.IdConvenio== idConvenio).SingleOrDefault();
        }


        private IQueryable<IPrograma> QProgramaRol(int idRol)
        {
            var a = (from c in _mdb.T_PROGRAMAS
                       join r in _mdb.T_PROGRAMAS_ROL on c.ID_PROGRAMA equals r.ID_PROGRAMA
                      where r.ID_ROL == idRol
                     select
                         new Programa
                         {
                             IdPrograma = c.ID_PROGRAMA,
                             NombrePrograma = c.N_PROGRAMA,
                             ConvenioBcoCba = c.CONVENIO_BCO_CBA,
                             MontoPrograma = c.MONTO_PROG ?? 0,
                             FechaSistema = c.FEC_SIST,
                             IdUsuarioSistema = c.ID_USR_SIST,
                             UsuarioSistema = c.T_USUARIOS.LOGIN
                         });
            return a;
        }

        public IList<IPrograma> GetProgramas(int idRol)
        {
            return QProgramaRol(idRol).OrderBy(c => c.IdPrograma).ToList();
        }

        private IQueryable<ITipoFicha> QTipoFichaRol(int idRol) // Como en inscriptos tiene tipo fichas le paso el programa que se corresponde
        {
            var a = (from c in _mdb.T_PROGRAMAS
                     join r in _mdb.T_PROGRAMAS_ROL on c.ID_PROGRAMA equals r.ID_PROGRAMA
                     where r.ID_ROL == idRol
                     select
                         new TipoFicha
                             {
                                 Id_Tipo_Ficha = c.ID_PROGRAMA,
                                 Nombre_Tipo_Ficha = c.N_PROGRAMA
                             });
            return a;
        }

        public IList<ITipoFicha> GetTipoFichas(int idRol)
        {
            return QTipoFichaRol(idRol).OrderBy(c => c.Id_Tipo_Ficha).ToList();
        }


        private IQueryable<IRolPrograma> QTipoFichaRol() // Como en inscriptos tiene tipo fichas le paso el programa que se corresponde
        {
            var a = (from c in _mdb.T_PROGRAMAS
                     join r in _mdb.T_PROGRAMAS_ROL on c.ID_PROGRAMA equals r.ID_PROGRAMA

                     select new RolPrograma
                 
                            {
                                IdPrograma = c.ID_PROGRAMA,
                                NombrePrograma = c.N_PROGRAMA,
                                Id_Rol = r.ID_ROL
                            }
                         );
            return a;
        }

        public IList<IRolPrograma> GetTipoFichasRol()
        {
            return QTipoFichaRol().OrderBy(c => c.IdPrograma).ToList();
        }

    }
}