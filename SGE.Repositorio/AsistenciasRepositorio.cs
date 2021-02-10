using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using SGE.Model.Comun;
using SGE.Model.Entidades;
using SGE.Model.Entidades.Interfaces;
using SGE.Model.Repositorio;
using SGE.Repositorio.Modelo;

namespace SGE.Repositorio
{
    public class AsistenciasRepositorio : BaseRepositorio, IAsistenciasRepositorio
    {
                private readonly DataSGE _mdb;

        public AsistenciasRepositorio()
        {
            _mdb = new DataSGE();
        }

        private IQueryable<IAsistencia> QAsistencias()
        {

            var a = (from asi in _mdb.T_ASISTENCIAS

                     select new Asistencia
                     {
                        ID_ASISTENCIA=asi.ID_ASISTENCIA,
                        ID_CURSO=asi.ID_CURSO ?? 0,
                        ID_ESCUELA = asi.ID_ESCUELA ?? 0,
                        ID_FICHA = asi.ID_FICHA ?? 0,
                        MES=asi.MES,
                        PERIODO=asi.PERIODO,
                        PORC_ASISTENCIA = asi.PORC_ASISTENCIA ?? 0,
                        ID_USR_SIST = asi.ID_USR_SIST ?? 0,
                        FEC_SIST=asi.FEC_SIST,
                        ID_USR_MODIF = asi.ID_USR_MODIF ?? 0,
                        FEC_MODIF=asi.FEC_MODIF,
                     });

            return a;


        }

        private IQueryable<IAsistencia> QAsistenciasCurso(int idCurso)
        {

            var a = (from asi in _mdb.T_ASISTENCIAS
                     where asi.ID_CURSO == idCurso
                     select new Asistencia
                     {
                         ID_ASISTENCIA = asi.ID_ASISTENCIA,
                         ID_CURSO = asi.ID_CURSO ?? 0,
                         ID_ESCUELA = asi.ID_ESCUELA ?? 0,
                         ID_FICHA = asi.ID_FICHA ?? 0,
                         MES = asi.MES,
                         PERIODO = asi.PERIODO,
                         PORC_ASISTENCIA = asi.PORC_ASISTENCIA ?? 0,
                         ID_USR_SIST = asi.ID_USR_SIST ?? 0,
                         FEC_SIST = asi.FEC_SIST,
                         ID_USR_MODIF = asi.ID_USR_MODIF ?? 0,
                         FEC_MODIF = asi.FEC_MODIF,
                     });

            return a;


        }

        private IQueryable<IAsistencia> QAsistenciasPeriodo(int idCurso, string mesPeriodo)
        {

            var a = (from asi in _mdb.T_ASISTENCIAS
                        from f in _mdb.T_FICHAS.Where(f=>f.ID_FICHA==(int)asi.ID_FICHA  && f.TIPO_FICHA == 8).DefaultIfEmpty()
                     where asi.ID_CURSO == idCurso && asi.MES == mesPeriodo
                     select new Asistencia
                     {
                         ID_ASISTENCIA = asi.ID_ASISTENCIA,
                         ID_CURSO = asi.ID_CURSO ?? 0,
                         ID_ESCUELA = asi.ID_ESCUELA ?? 0,
                         ID_FICHA = asi.ID_FICHA ?? 0,
                         MES = asi.MES,
                         PERIODO = asi.PERIODO,
                         PORC_ASISTENCIA = asi.PORC_ASISTENCIA ?? 0,
                         ID_USR_SIST = asi.ID_USR_SIST ?? 0,
                         FEC_SIST = asi.FEC_SIST,
                         ID_USR_MODIF = asi.ID_USR_MODIF ?? 0,
                         FEC_MODIF = asi.FEC_MODIF,

                         Apellido = f.APELLIDO,
                         Nombre = f.NOMBRE,
                         NumeroDocumento = f.NUMERO_DOCUMENTO,
                         TipoDocumento = f.TIPO_DOCUMENTO,
                         tipoFicha = f.TIPO_FICHA,
                         Cuil = f.CUIL


                     });

            return a;


        }

        private IQueryable<IAsistencia> QAsistenciasFicha(int idCurso, int idFicha)
        {

            var a = (from asi in _mdb.T_ASISTENCIAS
                     where asi.ID_CURSO == idCurso && asi.ID_FICHA == idFicha
                     select new Asistencia
                     {
                         ID_ASISTENCIA = asi.ID_ASISTENCIA,
                         ID_CURSO = asi.ID_CURSO ?? 0,
                         ID_ESCUELA = asi.ID_ESCUELA ?? 0,
                         ID_FICHA = asi.ID_FICHA ?? 0,
                         MES = asi.MES,
                         PERIODO = asi.PERIODO,
                         PORC_ASISTENCIA = asi.PORC_ASISTENCIA ?? 0,
                         ID_USR_SIST = asi.ID_USR_SIST ?? 0,
                         FEC_SIST = asi.FEC_SIST,
                         ID_USR_MODIF = asi.ID_USR_MODIF ?? 0,
                         FEC_MODIF = asi.FEC_MODIF,
                     });

            return a;


        }
        
        public IList<IAsistencia> GetAsistencias(int idCurso)
        {

            return QAsistenciasCurso(idCurso).ToList();

        }
        
        public IList<IAsistencia> GetAsistenciasPeriodo(int idCurso, string mesPeriodo)
        {

            return QAsistenciasPeriodo(idCurso, mesPeriodo).ToList();

        }

        public IList<IAsistencia> GetAsistenciasFicha(int idCurso, int idFicha)
        {

            return QAsistenciasFicha(idCurso, idFicha).OrderBy(x=>x.MES).ToList();

        }

        public int AddAsistencias(IList<IAsistencia> asistencias)
        {
            try
            {
                var a = (from c in _mdb.T_ASISTENCIAS
                         select c.ID_ASISTENCIA);
                int idAsistencia =0;
                idAsistencia = ((int?)a.DefaultIfEmpty().Max() ?? 0) + 1;

                foreach( var asistencia in asistencias)
                {
                    var model = new T_ASISTENCIAS
                                    {
                                        ID_ASISTENCIA = idAsistencia,
                                        ID_CURSO = asistencia.ID_CURSO == 0 ? null : asistencia.ID_CURSO,
                                        ID_ESCUELA = asistencia.ID_ESCUELA == 0 ? null : asistencia.ID_ESCUELA,
                                        ID_FICHA = asistencia.ID_FICHA == 0 ? null : asistencia.ID_FICHA,
                                        MES = asistencia.MES,
                                        PERIODO = asistencia.PERIODO,
                                        PORC_ASISTENCIA = asistencia.PORC_ASISTENCIA ?? 0,
                                        ID_USR_SIST = GetUsuarioLoguer().Id_Usuario,
                                        FEC_SIST = DateTime.Now,
                                        //ID_USR_MODIF = persona.id_usr_modif == 0 ? null : persona.id_usr_modif,
                                        //FEC_MODIF = persona.fec_modif
                    
                                    };
                idAsistencia++;

                _mdb.T_ASISTENCIAS.AddObject(model);
                }
                _mdb.SaveChanges();

                return 0;
            }
            catch (Exception ex)
            {
                return 1;
            }
 
        }

        public int UpdateAsistencias(IList<IAsistencia> asistencias)
        {
            _mdb.Connection.Open();
                using (var trans = _mdb.Connection.BeginTransaction())
                {
                   try
                    {
                        foreach (var asistencia in asistencias)
                        {
                            var row = _mdb.T_ASISTENCIAS.SingleOrDefault(c => c.ID_ASISTENCIA == asistencia.ID_ASISTENCIA);

                            if (row != null)
                            {
                                row.ID_CURSO = asistencia.ID_CURSO == 0 ? null : asistencia.ID_CURSO;
                                row.ID_ESCUELA = asistencia.ID_ESCUELA == 0 ? null : asistencia.ID_ESCUELA;
                                row.ID_FICHA = asistencia.ID_FICHA == 0 ? null : asistencia.ID_FICHA;
                                row.MES = asistencia.MES;
                                row.PERIODO = asistencia.PERIODO;
                                row.PORC_ASISTENCIA = asistencia.PORC_ASISTENCIA ?? 0;
                                row.ID_USR_MODIF = GetUsuarioLoguer().Id_Usuario;
                                row.FEC_MODIF = DateTime.Now;

                                _mdb.SaveChanges();
                            }
                        }
                    trans.Commit();
                    _mdb.Connection.Close();
                    return 0;
                    }
                    catch (Exception ex)
                    { 
                        trans.Rollback();
                        _mdb.Connection.Close();
                        return 1;
                    }

                }


        }

        private IQueryable<IAsistenciaRpt> QAsistenciasRpt(string mesPeriodo, int idCurso, int idEscuela, int idEstadoBenf, int idEtapa)
        {
            if (mesPeriodo == "0") { mesPeriodo = ""; }
            var a = (from asi in _mdb.VT_ASISTENCIAS_CONF_VOS
                     //where (asi.CUR_ID_CURSO == idCurso || idCurso == 0)
                     //&& (asi.ESC_ID_ESCUELA == idEscuela || idEscuela == 0)
                     //&& (asi.BEN_ID_ESTADO == idEstadoBenf || idEstadoBenf == 0)
                     //&& (asi.PERIODO == mesPeriodo || String.IsNullOrEmpty(mesPeriodo))
                     where (asi.PERIODO.ToLower().Contains(mesPeriodo.ToLower()) || String.IsNullOrEmpty(mesPeriodo.Trim()))
                            && (asi.CUR_ID_CURSO == idCurso || idCurso == 0)
                            && (asi.ESC_ID_ESCUELA == idEscuela || idEscuela == 0)
                            && (asi.BEN_ID_ESTADO == idEstadoBenf || idEstadoBenf == 0)
                            && (asi.ID_ETAPA == idEtapa || idEtapa == 0)

                     select new AsistenciaRpt
                     {
                         //ID_ASISTENCIA = asi.ID_ASISTENCIA,
 
                         ID_FICHA = asi.ID_FICHA,
                         APELLIDO = asi.APELLIDO,
                         NOMBRE = asi.NOMBRE,
                         CUIL = asi.CUIL,
                         NUMERO_DOCUMENTO = asi.NUMERO_DOCUMENTO,
                         ID_EST_FIC = asi.ID_EST_FIC ?? 0,
                         N_ESTADO_FICHA = asi.N_ESTADO_FICHA,
                         ID_BENEFICIARIO = asi.ID_BENEFICIARIO,
                         BEN_ID_ESTADO = asi.BEN_ID_ESTADO,
                         BEN_N_ESTADO = asi.BEN_N_ESTADO,
                         BEN_FEC_INICIO = asi.BEN_FEC_INICIO,
                         BEN_FEC_BAJA = asi.BEN_FEC_BAJA,
                         ESC_ID_ESCUELA = asi.ESC_ID_ESCUELA ?? 0,
                         N_ESCUELA = asi.N_ESCUELA,
                         ESC_CALLE = asi.ESC_CALLE,
                         ESC_NUMERO = asi.ESC_NUMERO,
                         ESC_BARRIO = asi.ESC_BARRIO,
                         ESC_ID_LOCALIDAD = asi.ESC_ID_LOCALIDAD ?? 0,
                         ESC_N_LOCALIDAD = asi.ESC_N_LOCALIDAD,
                         COO_APE = asi.COO_APE,
                         COO_NOM = asi.COO_NOM,
                         COO_DNI = asi.COO_DNI,
                         ENC_APE = asi.ENC_APE,
                         ENC_NOM = asi.ENC_NOM,
                         ENC_DNI = asi.ENC_DNI,
                         CUR_ID_CURSO = asi.CUR_ID_CURSO ?? 0,
                         N_CURSO = asi.N_CURSO,
                         PERIODO = asi.PERIODO,
                         PORC_ASISTENCIA = asi.PORC_ASISTENCIA ?? 0,
                         NRO_COND_CURSO = asi.NRO_COND_CURSO ?? 0,
                     });

            return a;


        }

        public IList<IAsistenciaRpt> GetAsistenciasRpt(string mesPeriodo, int idCurso, int idEscuela, int idEstadoBenf, int idEtapa)
        {

            var a = QAsistenciasRpt(mesPeriodo, idCurso, idEscuela, idEstadoBenf, idEtapa).ToList();

            return a.OrderBy(x => x.ID_FICHA).ThenBy(x => x.PERIODO).ToList();


        }


    }
}
