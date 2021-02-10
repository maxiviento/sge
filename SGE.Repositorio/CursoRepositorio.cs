using System.Collections.Generic;
using System.Linq;
using SGE.Model.Entidades;
using SGE.Model.Entidades.Interfaces;
using SGE.Model.Repositorio;
using SGE.Repositorio.Modelo;
using SGE.Model.Comun;
using System;

namespace SGE.Repositorio
{
    public class CursoRepositorio : BaseRepositorio, ICursoRepositorio
    {
                private readonly DataSGE _mdb;

        public CursoRepositorio()
        {
            _mdb = new DataSGE();
        }


        private IQueryable<ICurso> QCursoCompleto()
        {
            var a = (from c in _mdb.T_CURSOS
                     from doc in _mdb.T_ACTORES.Where(doc => doc.ID_ACTOR == c.ID_DOCENTE && doc.ID_ACTOR_ROL == (int)Enums.Actor_Rol.DOCENTE).DefaultIfEmpty()
                     from pdoc in _mdb.T_PERSONAS.Where(pdoc => pdoc.ID_PERSONA == doc.ID_PERSONA).DefaultIfEmpty()
                     from ldoc in _mdb.T_LOCALIDADES.Where(ldoc => ldoc.ID_LOCALIDAD == pdoc.ID_LOCALIDAD).DefaultIfEmpty()
                     from tut in _mdb.T_ACTORES.Where(tut => tut.ID_ACTOR == c.ID_TUTOR && tut.ID_ACTOR_ROL == (int)Enums.Actor_Rol.TUTOR).DefaultIfEmpty()
                     from ptut in _mdb.T_PERSONAS.Where(ptut => ptut.ID_PERSONA == tut.ID_PERSONA).DefaultIfEmpty()
                     from ltut in _mdb.T_LOCALIDADES.Where(ltut => ltut.ID_LOCALIDAD == ptut.ID_LOCALIDAD).DefaultIfEmpty()


                     select
                         new Curso
                         {
                                ID_CURSO = c.ID_CURSO,
                                N_CURSO = c.N_CURSO,
                                ID_DOCENTE = c.ID_DOCENTE ?? 0,
                                ID_TUTOR = c.ID_TUTOR ?? 0,
                                ID_ESCUELA = c.ID_ESCUELA ?? 0,
                                ID_ONG = c.ID_ONG ?? 0,
                                F_INICIO = c.FEC_INICIO,
                                F_FIN = c.FEC_FIN,
                                DURACION = c.DURACION,
                                EXPEDIENTE_LEG = c.EXPEDIENTE_LEG,
                                ID_USR_SIST = c.ID_USR_SIST ?? 0,
                                FEC_SIST = c.FEC_SIST,
                                ID_USR_MODIF = c.ID_USR_MODIF ?? 0,
                                FEC_MODIF = c.FEC_MODIF,
                                MESES = c.MESES,

                                DOC_APELLIDO = pdoc.APELLIDO,
                                DOC_NOMBRE = pdoc.NOMBRE,
                                DOC_DNI = pdoc.DNI,
                                DOC_CUIL = pdoc.CUIL,
                                DOC_CELULAR = pdoc.CELULAR,
                                DOC_TELEFONO = pdoc.TELEFONO,
                                DOC_MAIL = pdoc.MAIL,
                                DOC_ID_LOCALIDAD = pdoc.ID_LOCALIDAD  ?? 0,
                                DOC_N_LOCALIDAD = ldoc.N_LOCALIDAD,

                                TUT_APELLIDO = ptut.APELLIDO,
                                TUT_NOMBRE = ptut.NOMBRE,
                                TUT_DNI = ptut.DNI,
                                TUT_CUIL = ptut.CUIL,
                                TUT_CELULAR = ptut.CELULAR,
                                TUT_TELEFONO = ptut.TELEFONO,
                                TUT_MAIL = ptut.MAIL,
                                TUT_ID_LOCALIDAD = ptut.ID_LOCALIDAD  ?? 0,
                                TUT_N_LOCALIDAD = ltut.N_LOCALIDAD,
                                NRO_COND_CURSO = c.NRO_COND_CURSO,

                         });

            return a;
        }

        private IQueryable<ICurso> QCursoSimple()
        {
            var a = (from c in _mdb.T_CURSOS
                     


                     select
                         new Curso
                         {
                             ID_CURSO = c.ID_CURSO,
                             N_CURSO = c.N_CURSO,
                             ID_DOCENTE = c.ID_DOCENTE ?? 0,
                             ID_TUTOR = c.ID_TUTOR ?? 0,
                             ID_ESCUELA = c.ID_ESCUELA ?? 0,
                             ID_ONG = c.ID_ONG ?? 0,
                             F_INICIO = c.FEC_INICIO,
                             F_FIN = c.FEC_FIN,
                             DURACION = c.DURACION,
                             EXPEDIENTE_LEG = c.EXPEDIENTE_LEG,
                             ID_USR_SIST = c.ID_USR_SIST ?? 0,
                             FEC_SIST = c.FEC_SIST,
                             ID_USR_MODIF = c.ID_USR_MODIF ?? 0,
                             FEC_MODIF = c.FEC_MODIF,
                             NRO_COND_CURSO = c.NRO_COND_CURSO,
                             MESES = c.MESES,

                         });

            return a;
        }

        private IQueryable<ICurso> QCursoEscuela()
        {
            var a = (from c in _mdb.T_CURSOS
                     from e in _mdb.T_ESCUELAS.Where(e => e.ID_ESCUELA == c.ID_ESCUELA).DefaultIfEmpty()
                    
                     select
                         new Curso
                         {
                             ID_CURSO = c.ID_CURSO,
                             N_CURSO = c.N_CURSO,
                             ID_DOCENTE = c.ID_DOCENTE ?? 0,
                             ID_TUTOR = c.ID_TUTOR ?? 0,
                             ID_ESCUELA = c.ID_ESCUELA ?? 0,
                             ID_ONG = c.ID_ONG ?? 0,
                             F_INICIO = c.FEC_INICIO,
                             F_FIN = c.FEC_FIN,
                             DURACION = c.DURACION,
                             EXPEDIENTE_LEG = c.EXPEDIENTE_LEG,
                             ID_USR_SIST = c.ID_USR_SIST ?? 0,
                             FEC_SIST = c.FEC_SIST,
                             ID_USR_MODIF = c.ID_USR_MODIF ?? 0,
                             FEC_MODIF = c.FEC_MODIF,
                             NRO_COND_CURSO = c.NRO_COND_CURSO,
                             MESES = c.MESES,
                             //Id_Escuela = e.ID_ESCUELA == null ? 0 : e.ID_ESCUELA,
                             IdLocalidadCurso = e.ID_LOCALIDAD,
                             NEscuelaCurso = e.N_ESCUELA,
                             CueCurso = e.CUE,
                             AnexoCurso = e.ANEXO,
                             BarrioCurso = e.BARRIO,
                             CALLECurso = e.CALLE,
                             NUMEROCurso = e.NUMERO,
                             TELEFONOCurso = e.TELEFONO,
                             regiseCurso = e.REGISE,

                         });

            return a;
        }

        public IList<ICurso> GetCursosEscuela(string nombre)
        {
            return QCursoEscuela().Where(o => o.N_CURSO.ToUpper().Contains(nombre.ToUpper()) || String.IsNullOrEmpty(nombre)).ToList();

        }

        public ICurso getCurso(int idCurso)
        {
            return QCursoCompleto().Where(o => o.ID_CURSO == idCurso).SingleOrDefault();
        }

        public IList<ICurso> GetCursos(string nombre)
        {
            return QCursoCompleto().Where(o => o.N_CURSO.ToUpper().Contains(nombre.ToUpper()) || String.IsNullOrEmpty(nombre)).ToList();
            
        }

        public IList<ICurso> GetCursos(int nroCurso)
        {
            nroCurso = nroCurso==null ? 0 : nroCurso;
            return QCursoCompleto().Where(o => o.NRO_COND_CURSO == nroCurso || nroCurso==0).ToList();

        }

        public IList<ICurso> getCursos(int nroCurso, string nombre)
        {
            return QCursoCompleto().Where(o => (o.NRO_COND_CURSO == nroCurso || nroCurso == 0) && (o.N_CURSO.ToUpper().Contains(nombre.ToUpper()) || String.IsNullOrEmpty(nombre))).ToList();
        }

        public int AddCurso(ICurso curso)
        {
            var a = (from c in _mdb.T_CURSOS
                     select c.ID_CURSO);
            //AgregarDatos(persona);
            //int id = 0;
            //try
            //{ id = (int)a.Max() + 1; }
            //catch(ArgumentNullException ex)
            //{id=1;}

            var model = new T_CURSOS
            {
                ID_CURSO = ((int?)a.DefaultIfEmpty().Max() ?? 0) + 1,
                NRO_COND_CURSO = curso.NRO_COND_CURSO,
                N_CURSO = curso.N_CURSO,
                ID_DOCENTE = curso.ID_DOCENTE == 0 ? null : curso.ID_DOCENTE,
                ID_TUTOR = curso.ID_TUTOR == 0 ? null : curso.ID_TUTOR,
                ID_ONG = curso.ID_ONG == 0 ? null : curso.ID_ONG,
                ID_ESCUELA = curso.ID_ESCUELA == 0 ? null : curso.ID_ESCUELA,
                FEC_INICIO = curso.F_INICIO,
                FEC_FIN = curso.F_FIN,
                DURACION = curso.DURACION,
                EXPEDIENTE_LEG = curso.EXPEDIENTE_LEG,
                ID_USR_SIST = GetUsuarioLoguer().Id_Usuario,
                FEC_SIST = DateTime.Now,
                MESES = curso.MESES,
                //ID_USR_MODIF = persona.id_usr_modif == 0 ? null : persona.id_usr_modif,
                //FEC_MODIF = persona.fec_modif
            };

            _mdb.T_CURSOS.AddObject(model);
            _mdb.SaveChanges();
            return model.ID_CURSO;
        }

        public void UpdateCurso(ICurso curso)
        {
            if (curso.ID_CURSO != 0)
            {
                var row = _mdb.T_CURSOS.SingleOrDefault(c => c.ID_CURSO == curso.ID_CURSO);

                if (row != null)
                {
                    row.NRO_COND_CURSO = curso.NRO_COND_CURSO;
                    row.N_CURSO = curso.N_CURSO;
                    row.ID_DOCENTE = curso.ID_DOCENTE == 0 ? null : curso.ID_DOCENTE;
                    row.ID_TUTOR = curso.ID_TUTOR == 0 ? null : curso.ID_TUTOR;
                    row.ID_ONG = curso.ID_ONG == 0 ? null : curso.ID_ONG;
                    row.ID_ESCUELA = curso.ID_ESCUELA == 0 ? null : curso.ID_ESCUELA;
                    row.FEC_INICIO = curso.F_INICIO;
                    row.FEC_FIN = curso.F_FIN;
                    row.DURACION = curso.DURACION;
                    row.EXPEDIENTE_LEG = curso.EXPEDIENTE_LEG;
                    //row.ID_USR_SIST = GetUsuarioLoguer().Id_Usuario,
                    //row.FEC_SIST = DateTime.Now,
                    row.ID_USR_MODIF = GetUsuarioLoguer().Id_Usuario;
                    row.FEC_MODIF = DateTime.Now;
                    row.MESES = curso.MESES;
                    _mdb.SaveChanges();
                }
            }
        }

        private IQueryable<IFichasCurso> QFichasCurso(int idcurso)
        {
            var a = (from c in _mdb.T_FICHAS
                      join conf in _mdb.T_FICHAS_CONF_VOS on c.ID_FICHA equals conf.ID_FICHA
                      where conf.ID_CURSO == idcurso



                     select
                         new FichasCurso
                         {
                             
                            IdFicha = c.ID_FICHA,
                            tipoFicha = c.TIPO_FICHA,
                            Cuil = c.CUIL,
                            Apellido = c.APELLIDO,
                            Nombre =c.NOMBRE,
                            TipoDocumento = c.TIPO_DOCUMENTO,
                            NumeroDocumento = c.NUMERO_DOCUMENTO,
                            id_curso= conf.ID_CURSO,

                         });

            return a;
        }

        public IList<IFichasCurso> GetFichasCurso(int idcurso)
        {
            return QFichasCurso(idcurso).ToList();
        }
    }
}
