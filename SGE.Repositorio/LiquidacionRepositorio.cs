using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using SGE.Model.Comun;
using SGE.Model.Entidades;
using SGE.Model.Entidades.Interfaces;
using SGE.Model.Repositorio;
using SGE.Repositorio.Modelo;
using System.Data.Objects;

namespace SGE.Repositorio
{
    public class LiquidacionRepositorio : BaseRepositorio, ILiquidacionRepositorio
    {
        private readonly DataSGE _mdb;

        public LiquidacionRepositorio()
        {
            _mdb = new DataSGE();
        }

        private IQueryable<ILiquidacion> QLiquidacion()
        {
            var a =
                _mdb.T_LIQUIDACIONES.Select(
                    c =>
                    new Liquidacion
                        {
                            IdLiquidacion = c.ID_LIQUIDACION,
                            FechaLiquidacion = c.FEC_LIQ ?? DateTime.Now,
                            NroResolucion = c.NRO_RESOLUCION,
                            Observacion = c.OBSERVACION,
                            IdEstadoLiquidacion = c.ID_ESTADO_LIQ ?? 0,
                            NombreEstado = c.T_ESTADOS_LIQUIDACION.N_ESTADO_LIQ,
                            TxtNombre = c.TXT_NOMBRE,
                            Generado = c.GENERADO == "S" ? true : false,
                            FechaSistema = c.FEC_SIST,
                            IdUsuarioSistema = c.ID_USR_SIST,
                            UsuarioSistema = c.T_USUARIOS.LOGIN,
                            CantBenef =
                                c.T_BENEFICIARIO_CONCEP_LIQ.Where(
                                    d =>
                                    d.ID_EST_BEN_CON_LIQ != (int)Enums.EstadoBeneficiarioLiquidacion.ExcluidoNoApto).
                                Count(),
                            Beneficiarios =
                                c.T_BENEFICIARIO_CONCEP_LIQ.Select(
                                    t =>
                                    new Beneficiario
                                        {
                                            IdBeneficiario = t.T_BENEFICIARIOS.ID_BENEFICIARIO,
                                            IdEstado = t.T_BENEFICIARIOS.ID_ESTADO ?? 0,
                                            FechaSistema = t.T_BENEFICIARIOS.FEC_SIST ?? DateTime.Now,
                                            IdFicha = t.T_BENEFICIARIOS.ID_FICHA ?? 0,
                                            IdPrograma = t.T_BENEFICIARIOS.ID_PROGRAMA ?? 0,
                                            IdUsuarioSistema = t.T_BENEFICIARIOS.ID_USR_SIST ?? 0,
                                            IdConvenio = t.T_BENEFICIARIOS.T_PROGRAMAS.CONVENIO_BCO_CBA,
                                            NumeroConvenio = t.T_BENEFICIARIOS.T_PROGRAMAS.CONVENIO_BCO_CBA,
                                            Nacionalidad = t.T_BENEFICIARIOS.NACIONALIDAD ?? 0,
                                            Residente = t.T_BENEFICIARIOS.RESIDENTE,
                                            TipoPersona = t.T_BENEFICIARIOS.TIPO_PERSONA,
                                            NombreEstado = t.T_BENEFICIARIOS.T_ESTADOS.N_ESTADO,
                                            ImportePagoPlan = t.T_BENEFICIARIOS.T_PROGRAMAS.MONTO_PROG ?? 0,
                                            Programa = t.T_BENEFICIARIOS.T_PROGRAMAS.N_PROGRAMA,
                                            MontoPrograma = t.T_BENEFICIARIOS.T_PROGRAMAS.MONTO_PROG ?? 0,
                                            TieneApoderado = t.T_BENEFICIARIOS.TIENE_APODERADO,
                                            IdEstadoBenConcLiq = t.ID_EST_BEN_CON_LIQ ?? 0,
                                            Sucursal =
                                                (t.T_TABLAS_BCO_CBA.COD_BCO_CBA + "-" + t.T_TABLAS_BCO_CBA.DESCRIPCION),
                                            NroCuentadePago = t.NRO_CTA_BCO ?? 0,
                                            PagoaApoderado = t.APODERADO,
                                            EstadoBeneficiarioConcepto = t.T_ESTADOS_BEN_CON_LIQ.N_EST_BEN_CON_LIQ,
                                            IdSucursal = t.T_TABLAS_BCO_CBA.ID_TABLA_BCO_CBA,
                                            Apoderado = new Apoderado { NumeroDocumento = t.T_APODERADOS.NRO_DOCUMENTO, Nombre = t.T_APODERADOS.NOMBRE, Apellido = t.T_APODERADOS.APELLIDO },
                                            Ficha = new Ficha
                                                        {
                                                            IdFicha = t.T_BENEFICIARIOS.T_FICHAS.ID_FICHA,
                                                            Apellido = t.T_BENEFICIARIOS.T_FICHAS.APELLIDO,
                                                            Barrio = t.T_BENEFICIARIOS.T_FICHAS.BARRIO,
                                                            Localidad =
                                                                t.T_BENEFICIARIOS.T_FICHAS.T_LOCALIDADES.N_LOCALIDAD,
                                                            IdLocalidad = t.T_BENEFICIARIOS.T_FICHAS.ID_LOCALIDAD ?? 0,
                                                            Nombre = t.T_BENEFICIARIOS.T_FICHAS.NOMBRE,
                                                            NumeroDocumento =
                                                                t.T_BENEFICIARIOS.T_FICHAS.NUMERO_DOCUMENTO,
                                                            Cuil = t.T_BENEFICIARIOS.T_FICHAS.CUIL

                                                        },
                                            ConceptosBeneficiario = (from co in _mdb.T_CONCEPTOS
                                                                     join bc2 in _mdb.T_BENEFICIARIO_CONCEP_LIQ on co.ID_CONCEPTO equals bc2.ID_CONCEPTO
                                                                     where bc2.ID_BENEFICIARIO == t.T_BENEFICIARIOS.ID_BENEFICIARIO && bc2.ID_LIQUIDACION == t.ID_LIQUIDACION
                                                                     select
                                                                         new Concepto
                                                                             {
                                                                                 Id_Concepto = co.ID_CONCEPTO,
                                                                                 Año = co.ANIO,
                                                                                 Mes = co.MES,
                                                                                 Observacion = co.OBSERVACION
                                                                             }).AsEnumerable()
                                        }).AsEnumerable()
                        });

            return a;
        }

        private IQueryable<ILiquidacion> QLiquidacionRepAnexo()
        {
            var a =
                _mdb.T_LIQUIDACIONES.Select(
                    c =>
                    new Liquidacion
                    {
                        IdLiquidacion = c.ID_LIQUIDACION,
                        FechaLiquidacion = c.FEC_LIQ ?? DateTime.Now,
                        NroResolucion = c.NRO_RESOLUCION,
                        Observacion = c.OBSERVACION,
                        IdEstadoLiquidacion = c.ID_ESTADO_LIQ ?? 0,
                        NombreEstado = c.T_ESTADOS_LIQUIDACION.N_ESTADO_LIQ,
                        TxtNombre = c.TXT_NOMBRE,
                        Generado = c.GENERADO == "S" ? true : false,
                        FechaSistema = c.FEC_SIST,
                        IdUsuarioSistema = c.ID_USR_SIST,
                        UsuarioSistema = c.T_USUARIOS.LOGIN,
                        CantBenef =
                            c.T_BENEFICIARIO_CONCEP_LIQ.Where(
                                d =>
                                (d.ID_EST_BEN_CON_LIQ == (int)Enums.EstadoBeneficiarioLiquidacion.ImputadoEnCuenta) ||
                                 d.ID_EST_BEN_CON_LIQ == (int)Enums.EstadoBeneficiarioLiquidacion.EnviadoAlBanco).
                            Count(),
                        Beneficiarios =
                            c.T_BENEFICIARIO_CONCEP_LIQ.Select(
                                t =>
                                new Beneficiario
                                {
                                    IdBeneficiario = t.T_BENEFICIARIOS.ID_BENEFICIARIO,
                                    IdEstado = t.T_BENEFICIARIOS.ID_ESTADO ?? 0,
                                    FechaSistema = t.T_BENEFICIARIOS.FEC_SIST ?? DateTime.Now,
                                    IdFicha = t.T_BENEFICIARIOS.ID_FICHA ?? 0,
                                    IdPrograma = t.T_BENEFICIARIOS.ID_PROGRAMA ?? 0,
                                    IdUsuarioSistema = t.T_BENEFICIARIOS.ID_USR_SIST ?? 0,
                                    IdConvenio = t.T_BENEFICIARIOS.T_PROGRAMAS.CONVENIO_BCO_CBA,
                                    NumeroConvenio = t.T_BENEFICIARIOS.T_PROGRAMAS.CONVENIO_BCO_CBA,
                                    Nacionalidad = t.T_BENEFICIARIOS.NACIONALIDAD ?? 0,
                                    Residente = t.T_BENEFICIARIOS.RESIDENTE,
                                    TipoPersona = t.T_BENEFICIARIOS.TIPO_PERSONA,
                                    NombreEstado = t.T_BENEFICIARIOS.T_ESTADOS.N_ESTADO,
                                    ImportePagoPlan = t.T_BENEFICIARIOS.T_PROGRAMAS.MONTO_PROG ?? 0,
                                    Programa = t.T_BENEFICIARIOS.T_PROGRAMAS.N_PROGRAMA,
                                    MontoPrograma = t.T_BENEFICIARIOS.T_PROGRAMAS.MONTO_PROG ?? 0,
                                    TieneApoderado = t.T_BENEFICIARIOS.TIENE_APODERADO,
                                    IdEstadoBenConcLiq = t.ID_EST_BEN_CON_LIQ ?? 0,
                                    Sucursal =(t.T_TABLAS_BCO_CBA.COD_BCO_CBA + "-" + t.T_TABLAS_BCO_CBA.DESCRIPCION),
                                    NroCuentadePago = t.NRO_CTA_BCO ?? 0,
                                    PagoaApoderado = t.APODERADO,
                                    EstadoBeneficiarioConcepto = t.T_ESTADOS_BEN_CON_LIQ.N_EST_BEN_CON_LIQ,
                                    IdSucursal = t.T_TABLAS_BCO_CBA.ID_TABLA_BCO_CBA,
                                    SucursalDescripcion =t.T_TABLAS_BCO_CBA.DESCRIPCION,
                                    CodigoSucursal = t.T_TABLAS_BCO_CBA.COD_BCO_CBA,
                                    Apoderado = new Apoderado { TipoDocumento = t.T_APODERADOS.TIPO_DOCUMENTO, NumeroDocumento = t.T_APODERADOS.NRO_DOCUMENTO, Nombre = t.T_APODERADOS.NOMBRE, Apellido = t.T_APODERADOS.APELLIDO },
                                    Ficha = new Ficha
                                    {
                                        IdFicha = t.T_BENEFICIARIOS.T_FICHAS.ID_FICHA,
                                        Apellido = t.T_BENEFICIARIOS.T_FICHAS.APELLIDO,
                                        Barrio = t.T_BENEFICIARIOS.T_FICHAS.BARRIO,
                                        Localidad =t.T_BENEFICIARIOS.T_FICHAS.T_LOCALIDADES.N_LOCALIDAD,
                                        IdLocalidad = t.T_BENEFICIARIOS.T_FICHAS.ID_LOCALIDAD ?? 0,
                                        Nombre = t.T_BENEFICIARIOS.T_FICHAS.NOMBRE,
                                        TipoDocumento= t.T_BENEFICIARIOS.T_FICHAS.TIPO_DOCUMENTO,
                                        NumeroDocumento =t.T_BENEFICIARIOS.T_FICHAS.NUMERO_DOCUMENTO,
                                        Cuil = t.T_BENEFICIARIOS.T_FICHAS.CUIL

                                    },
                                    ConceptosBeneficiario = (from co in _mdb.T_CONCEPTOS
                                                             join bc2 in _mdb.T_BENEFICIARIO_CONCEP_LIQ on co.ID_CONCEPTO equals bc2.ID_CONCEPTO
                                                             where bc2.ID_BENEFICIARIO == t.T_BENEFICIARIOS.ID_BENEFICIARIO && bc2.ID_LIQUIDACION == t.ID_LIQUIDACION
                                                             select
                                                                 new Concepto
                                                                 {
                                                                     Id_Concepto = co.ID_CONCEPTO,
                                                                     Año = co.ANIO,
                                                                     Mes = co.MES,
                                                                     Observacion = co.OBSERVACION
                                                                 }).AsEnumerable()
                                }).Where(i => i.IdEstadoBenConcLiq == (int)Enums.EstadoBeneficiarioLiquidacion.ImputadoEnCuenta || i.IdEstadoBenConcLiq == (int)Enums.EstadoBeneficiarioLiquidacion.EnviadoAlBanco).AsEnumerable()
                    });

            return a;
        }
      
        private IQueryable<ILiquidacion> QLiquidacionSimple()
        {
            var a =
                _mdb.T_LIQUIDACIONES.Select(
                    c =>
                    new Liquidacion
                        {
                            IdLiquidacion = c.ID_LIQUIDACION,
                            FechaLiquidacion = c.FEC_LIQ ?? DateTime.Now,
                            NroResolucion = c.NRO_RESOLUCION,
                            Observacion = c.OBSERVACION,
                            IdEstadoLiquidacion = c.ID_ESTADO_LIQ ?? 0,
                            NombreEstado = c.T_ESTADOS_LIQUIDACION.N_ESTADO_LIQ,
                            TxtNombre = c.TXT_NOMBRE,
                            Generado = c.GENERADO == "S" ? true : false,
                            IdPrograma = c.T_BENEFICIARIO_CONCEP_LIQ.Select(te => te.T_BENEFICIARIOS.T_PROGRAMAS.ID_PROGRAMA).FirstOrDefault(),
                            Programa = c.T_BENEFICIARIO_CONCEP_LIQ.Select(te => te.T_BENEFICIARIOS.T_PROGRAMAS.N_PROGRAMA).FirstOrDefault(),
                            CantBenef = c.T_BENEFICIARIO_CONCEP_LIQ.Where(d => d.ID_EST_BEN_CON_LIQ != (int)Enums.EstadoBeneficiarioLiquidacion.ExcluidoNoApto && d.ID_EST_BEN_CON_LIQ != (int)Enums.EstadoBeneficiarioLiquidacion.Excluido).Count(),
                            Usuario = c.T_USUARIOS.APELLIDO + ", " + c.T_USUARIOS.NOMBRE,
                            Convenio = c.T_BENEFICIARIO_CONCEP_LIQ.Select(te => te.T_BENEFICIARIOS.T_PROGRAMAS.CONVENIO_BCO_CBA).FirstOrDefault(),
                            IdUsuarioSistema = c.ID_USR_SIST,
                            FechaSistema = c.FEC_SIST,
                            UsuarioSistema = c.T_USUARIOS.LOGIN
                        });
            var objectQuery = a as ObjectQuery;
            string consultaSql = objectQuery.ToTraceString();
            return a;
        }

        /// <summary>
        /// 18/07/2013 - QLiquidacionSimple - DI CAMPLI LEANDRO - SE AÑADEN PARAMETROS DIRECTAMENTE A LA CONSULTA LINQ
        /// </summary>
        /// <returns></returns>
        private IQueryable<ILiquidacion> QLiquidacionSimple(DateTime xfrom, DateTime to, int nroResolucion, int estado, int programa)
        {
            //var a =
            //    _mdb.T_LIQUIDACIONES.Where(c => (estado == 0 ? c.ID_ESTADO_LIQ != 0 : c.ID_ESTADO_LIQ == estado) &&
            //        (c.FEC_LIQ >= from && c.FEC_LIQ <= to) &&
            //        ((nroResolucion == 0 ? c.NRO_RESOLUCION >= nroResolucion : c.NRO_RESOLUCION == nroResolucion))).Select(
            //        c =>
            //        new Liquidacion
            //        {
            //            IdLiquidacion = c.ID_LIQUIDACION,
            //            FechaLiquidacion = c.FEC_LIQ ?? DateTime.Now,
            //            NroResolucion = c.NRO_RESOLUCION,
            //            Observacion = c.OBSERVACION,
            //            IdEstadoLiquidacion = c.ID_ESTADO_LIQ ?? 0,
            //            NombreEstado = c.T_ESTADOS_LIQUIDACION.N_ESTADO_LIQ,
            //            TxtNombre = c.TXT_NOMBRE,
            //            Generado = c.GENERADO == "S" ? true : false,
            //            IdPrograma = c.T_BENEFICIARIO_CONCEP_LIQ.Select(te => te.T_BENEFICIARIOS.T_PROGRAMAS.ID_PROGRAMA).FirstOrDefault(),
            //            Programa = c.T_BENEFICIARIO_CONCEP_LIQ.Select(te => te.T_BENEFICIARIOS.T_PROGRAMAS.N_PROGRAMA).FirstOrDefault(),
            //            CantBenef = c.T_BENEFICIARIO_CONCEP_LIQ.Where(d => d.ID_EST_BEN_CON_LIQ != (int)Enums.EstadoBeneficiarioLiquidacion.ExcluidoNoApto && d.ID_EST_BEN_CON_LIQ != (int)Enums.EstadoBeneficiarioLiquidacion.Excluido).Count(),
            //            Usuario = c.T_USUARIOS.APELLIDO + ", " + c.T_USUARIOS.NOMBRE,
            //            Convenio = c.T_BENEFICIARIO_CONCEP_LIQ.Select(te => te.T_BENEFICIARIOS.T_PROGRAMAS.CONVENIO_BCO_CBA).FirstOrDefault(),
            //            IdUsuarioSistema = c.ID_USR_SIST,
            //            FechaSistema = c.FEC_SIST,
            //            UsuarioSistema = c.T_USUARIOS.LOGIN
            //        });

     var liquidaciones = from liq in _mdb.T_LIQUIDACIONES 
                         from elq in _mdb.T_ESTADOS_LIQUIDACION.Where(elq=>elq.ID_ESTADO_LIQ==liq.ID_ESTADO_LIQ).DefaultIfEmpty()
                         from blq in _mdb.T_BENEFICIARIO_CONCEP_LIQ.Where(
                                    blq=>blq.ID_LIQUIDACION==liq.ID_LIQUIDACION && blq.ID_EST_BEN_CON_LIQ != (int)Enums.EstadoBeneficiarioLiquidacion.ExcluidoNoApto && blq.ID_EST_BEN_CON_LIQ != (int)Enums.EstadoBeneficiarioLiquidacion.Excluido).DefaultIfEmpty()
                         from ben in _mdb.T_BENEFICIARIOS.Where(ben=>ben.ID_BENEFICIARIO==blq.ID_BENEFICIARIO).DefaultIfEmpty()
                         from prog in _mdb.T_PROGRAMAS.Where(prog=>prog.ID_PROGRAMA==ben.ID_PROGRAMA).DefaultIfEmpty()
                         from usu in _mdb.T_USUARIOS.Where(usu=>usu.ID_USUARIO==liq.ID_USR_SIST).DefaultIfEmpty()
                         
                         where      (estado == 0 ? liq.ID_ESTADO_LIQ != 0 : liq.ID_ESTADO_LIQ == estado) &&
                                    (liq.FEC_LIQ >= xfrom && liq.FEC_LIQ <= to) &&
                                    ((nroResolucion == 0 ? liq.NRO_RESOLUCION >= nroResolucion : liq.NRO_RESOLUCION == nroResolucion))

                        select 
                        new Liquidacion
                        {
                            IdLiquidacion = liq.ID_LIQUIDACION,
                            FechaLiquidacion = liq.FEC_LIQ ?? DateTime.Now,
                            NroResolucion = liq.NRO_RESOLUCION,
                            Observacion = liq.OBSERVACION,
                            IdEstadoLiquidacion = liq.ID_ESTADO_LIQ ?? 0,
                            NombreEstado = elq.N_ESTADO_LIQ,
                            TxtNombre = liq.TXT_NOMBRE,
                            Generado = liq.GENERADO == "S" ? true : false,
                            IdPrograma = ben.ID_PROGRAMA==null ? 0 : (short)ben.ID_PROGRAMA, //c.T_BENEFICIARIO_CONCEP_LIQ.Select(te => te.T_BENEFICIARIOS.T_PROGRAMAS.ID_PROGRAMA).FirstOrDefault(),
                            Programa = prog.N_PROGRAMA,//c.T_BENEFICIARIO_CONCEP_LIQ.Select(te => te.T_BENEFICIARIOS.T_PROGRAMAS.N_PROGRAMA).FirstOrDefault(),
                            CantBenef = 1,//c.T_BENEFICIARIO_CONCEP_LIQ.Where(d => d.ID_EST_BEN_CON_LIQ != (int)Enums.EstadoBeneficiarioLiquidacion.ExcluidoNoApto && d.ID_EST_BEN_CON_LIQ != (int)Enums.EstadoBeneficiarioLiquidacion.Excluido).Count(),
                            Usuario = usu.APELLIDO + ", " + usu.NOMBRE,
                            Convenio = prog.CONVENIO_BCO_CBA,//c.T_BENEFICIARIO_CONCEP_LIQ.Select(te => te.T_BENEFICIARIOS.T_PROGRAMAS.CONVENIO_BCO_CBA).FirstOrDefault(),
                            IdUsuarioSistema = liq.ID_USR_SIST,
                            FechaSistema = liq.FEC_SIST,
                            UsuarioSistema = usu.LOGIN
                        };

     var a = from liq in  liquidaciones     
             group liq by 
             new 
             {
                 liq.IdLiquidacion,
                 liq.FechaLiquidacion,
                 liq.NroResolucion,
                 liq.Observacion,
                 liq.IdEstadoLiquidacion,
                 liq.NombreEstado,
                 liq.TxtNombre,
                 liq.Generado,
                 liq.IdPrograma,
                 liq.Programa,
                 
                 liq.Usuario,
                 liq.Convenio,
                 liq.IdUsuarioSistema,
                 liq.FechaSistema,
                 liq.UsuarioSistema
             } into grpLiq


             select
                        new Liquidacion
                        {
                            IdLiquidacion = grpLiq.Key.IdLiquidacion,
                            FechaLiquidacion = grpLiq.Key.FechaLiquidacion,
                            NroResolucion = grpLiq.Key.NroResolucion,
                            Observacion = grpLiq.Key.Observacion,
                            IdEstadoLiquidacion = grpLiq.Key.IdEstadoLiquidacion,
                            NombreEstado = grpLiq.Key.NombreEstado,
                            TxtNombre = grpLiq.Key.TxtNombre,
                            Generado = grpLiq.Key.Generado,
                            IdPrograma = grpLiq.Key.IdPrograma,
                            Programa = grpLiq.Key.Programa,
                            CantBenef = grpLiq.Count(),
                            Usuario = grpLiq.Key.Usuario,
                            Convenio = grpLiq.Key.Convenio,
                            IdUsuarioSistema = grpLiq.Key.IdUsuarioSistema,
                            FechaSistema = grpLiq.Key.FechaSistema,
                            UsuarioSistema = grpLiq.Key.UsuarioSistema
                        };
                        
            return a;
        }

        public IList<ILiquidacion> GetLiquidaciones()
        {
            return QLiquidacionSimple().ToList();
        }

        public IList<ILiquidacion> GetLiquidaciones(int skip, int take)
        {
            return QLiquidacionSimple().ToList().OrderBy(c => c.FechaLiquidacion).Skip(skip).Take(take).ToList();
        }

        public IList<ILiquidacion> GetLiquidaciones(DateTime from, DateTime to, int nroResolucion, int estado, int programa)
        {

            //return
            //    QLiquidacionSimple().Where(c => (programa == 0 ? c.IdPrograma != 0 : c.IdPrograma == programa) &&
            //        (estado == 0 ? c.IdEstadoLiquidacion != 0 : c.IdEstadoLiquidacion == estado) &&
            //        (c.FechaLiquidacion >= from && c.FechaLiquidacion <= to) &&
            //        ((nroResolucion == 0 ? c.NroResolucion >= nroResolucion : c.NroResolucion == nroResolucion))).
            //        OrderByDescending(c => c.FechaLiquidacion).ToList();

            return
            QLiquidacionSimple(from, to, nroResolucion, estado, programa).Where(c => (programa == 0 ? c.IdPrograma != 0 : c.IdPrograma == programa)).OrderByDescending(c => c.FechaLiquidacion).ToList();


        }

        public IList<ILiquidacion> GetLiquidaciones(DateTime from, DateTime to, int nroResolucion, int estado, int programa, int skip, int take)
        {
            //return
            //    QLiquidacionSimple().Where(
            //        c => (programa == 0 ? c.IdPrograma != 0 : c.IdPrograma == programa) &&
            //        (estado == 0 ? c.IdEstadoLiquidacion != 0 : c.IdEstadoLiquidacion == estado) &&
            //        (c.FechaLiquidacion >= from && c.FechaLiquidacion <= to) &&
            //        ((nroResolucion == 0 ? c.NroResolucion >= nroResolucion : c.NroResolucion == nroResolucion))).
            //        OrderByDescending(
            //            c => c.FechaLiquidacion).Skip(skip).Take(take)
            //        .ToList();

            return
            QLiquidacionSimple(from, to, nroResolucion, estado, programa).Where(c => (programa == 0 ? c.IdPrograma != 0 : c.IdPrograma == programa)).OrderByDescending(c => c.FechaLiquidacion).Skip(skip).Take(take).ToList();


        }

        public IList<ILiquidacion> GetLiquidaciones(DateTime from, DateTime to, int nroResolucion, int estado, int programa, int skip, int take, int excluirprograma)
        {
            //return
            //    QLiquidacionSimple().Where(
            //        c =>
            //        (programa == 0 ? c.IdPrograma != 0 : c.IdPrograma == programa) && (c.IdPrograma != excluirprograma) &&
            //        (estado == 0 ? c.IdEstadoLiquidacion != 0 : c.IdEstadoLiquidacion == estado) &&
            //        (c.FechaLiquidacion >= from && c.FechaLiquidacion <= to) &&
            //        ((nroResolucion == 0 ? c.NroResolucion >= nroResolucion : c.NroResolucion == nroResolucion))).
            //        OrderByDescending(
            //            c => c.FechaLiquidacion).Skip(skip).Take(take)
            //        .ToList();

            return
            QLiquidacionSimple(from, to, nroResolucion, estado, programa).Where(c => (programa == 0 ? c.IdPrograma != 0 : c.IdPrograma == programa && (c.IdPrograma != excluirprograma))).OrderByDescending(c => c.FechaLiquidacion).Skip(skip).Take(take).ToList();

        }

        public ILiquidacion GetLiquidacion(int idLiquidacion)
        {
            //return QLiquidacion().Where(c => c.IdLiquidacion == idLiquidacion).SingleOrDefault();
            return QLiquidacion(idLiquidacion).SingleOrDefault();

        }

        public ILiquidacion GetLiquidacionImputadoEnCuenta(int idLiquidacion)
        {
            return QLiquidacionRepAnexo().Where(c => c.IdLiquidacion == idLiquidacion).SingleOrDefault();
        }

        public ILiquidacion GetLiquidacionSimple(int idLiquidacion)
        {
            return QLiquidacionSimple().Where(c => c.IdLiquidacion == idLiquidacion).SingleOrDefault();
        }

        public int GetLiquidacionesCount()
        {
            return QLiquidacion().Count();
        }

        public IList<ILiquidacion> GetLiquidacionesPendientes(string apellido, string nombre, string cbu, string numeroDocumento)
        {
            return null;
        }

        public IList<ILiquidacion> GetLiquidacionesPendientes(string apellido, string nombre, string cbu, string numeroDocumento, int skip, int take)
        {
            return null;
        }

        public int AddLiquidacion(ILiquidacion liquidacion)
        {
            AgregarDatos(liquidacion);

            _mdb.Connection.Open();
            DbTransaction localtransaccion = _mdb.Connection.BeginTransaction();

            int idliquidacion = SecuenciaRepositorio.GetId();

            var localliquidacion = new T_LIQUIDACIONES
                                       {
                                           FEC_SIST = liquidacion.FechaSistema,
                                           ID_USR_SIST = liquidacion.IdUsuarioSistema,
                                           FEC_LIQ = liquidacion.FechaLiquidacion,
                                           ID_ESTADO_LIQ = (short)liquidacion.IdEstadoLiquidacion,
                                           NRO_RESOLUCION = liquidacion.NroResolucion,
                                           ID_LIQUIDACION = idliquidacion,
                                           OBSERVACION = liquidacion.Observacion
                                       };

            _mdb.T_LIQUIDACIONES.AddObject(localliquidacion);

            foreach (var itemliquidacion in liquidacion.ListaBeneficiarios)
            {
                foreach (var itemconcepto in itemliquidacion.ListaConceptos)
                {
                    int idbeco = SecuenciaRepositorio.GetId();

                    var beneficiarioconcepto = new T_BENEFICIARIO_CONCEP_LIQ
                    {
                        APROBADO = "S",
                        ID_BENEFICIARIO = itemliquidacion.IdBeneficiario,
                        ID_CONCEPTO = itemconcepto.Id_Concepto,
                        MONTO = (int)itemliquidacion.MontoPrograma,
                        ID_LIQUIDACION = idliquidacion,
                        ID_BENEFICIARIO_CONCEP_LIQ = idbeco,
                        OBSERVACION = liquidacion.Observacion,
                        ID_EST_BEN_CON_LIQ = itemliquidacion.IdEstadoBenConcLiq,
                        NRO_CTA_BCO = itemliquidacion.NroCuentadePago,
                        ID_SUCURSAL = itemliquidacion.IdSucursal,
                        APODERADO = itemliquidacion.PagoaApoderado,
                        ID_USR_SIST = liquidacion.IdUsuarioSistema,
                        ID_APODERADO = itemliquidacion.IdApoderadoPago,
                        ID_EMPRESA = itemliquidacion.IdEmpresa
                    };

                    _mdb.T_BENEFICIARIO_CONCEP_LIQ.AddObject(beneficiarioconcepto);
                }
            }

            _mdb.SaveChanges();
            localtransaccion.Commit();
            return idliquidacion;
        }

        public bool UpdateLiquidacion(ILiquidacion liquidacion)
        {
            AgregarDatos(liquidacion);

            _mdb.Connection.Open();
            DbTransaction localTransaccion = _mdb.Connection.BeginTransaction();

            T_LIQUIDACIONES updateLiquidacion =
                _mdb.T_LIQUIDACIONES.FirstOrDefault(c => c.ID_LIQUIDACION == liquidacion.IdLiquidacion);

            updateLiquidacion.FEC_LIQ = liquidacion.FechaLiquidacion;
            updateLiquidacion.OBSERVACION = liquidacion.Observacion;
            updateLiquidacion.NRO_RESOLUCION = liquidacion.NroResolucion;
            updateLiquidacion.ID_USR_SIST = liquidacion.IdUsuarioSistema;

            _mdb.ExecuteStoreCommand(
               string.Format("delete from " + ConfigurationManager.AppSettings["EsquemaDB"] + ".T_BENEFICIARIO_CONCEP_LIQ where ID_LIQUIDACION = {0}",
                             liquidacion.IdLiquidacion));

            foreach (var item in liquidacion.ListaBeneficiarios)
            {
                foreach (var itemconcepto in item.ListaConceptos)
                {
                    int idBeneficiarioConcepLiq = SecuenciaRepositorio.GetId();

                    var beneficiarioConcepLiq = new T_BENEFICIARIO_CONCEP_LIQ
                    {
                        APROBADO = "S",
                        ID_BENEFICIARIO = item.IdBeneficiario,
                        ID_CONCEPTO = itemconcepto.Id_Concepto,
                        MONTO = (int)item.MontoPrograma,
                        ID_LIQUIDACION = liquidacion.IdLiquidacion,
                        ID_BENEFICIARIO_CONCEP_LIQ = idBeneficiarioConcepLiq,
                        OBSERVACION = liquidacion.Observacion,
                        ID_EST_BEN_CON_LIQ = item.IdEstadoBenConcLiq,
                        NRO_CTA_BCO = item.NroCuentadePago,
                        ID_SUCURSAL = item.IdSucursal,
                        APODERADO = item.PagoaApoderado,
                        ID_USR_SIST = liquidacion.IdUsuarioSistema,
                        ID_APODERADO = item.IdApoderadoPago
                    };

                    _mdb.T_BENEFICIARIO_CONCEP_LIQ.AddObject(beneficiarioConcepLiq);
                }
            }

            _mdb.SaveChanges();
            localTransaccion.Commit();
            return true;
        }

        public bool FileGenerado(ILiquidacion liquidacion)
        {
            AgregarDatos(liquidacion);

            T_LIQUIDACIONES localLiquidacion = _mdb.T_LIQUIDACIONES.FirstOrDefault(c =>
                c.ID_LIQUIDACION == liquidacion.IdLiquidacion);

            localLiquidacion.TXT_NOMBRE = liquidacion.TxtNombre;
            localLiquidacion.GENERADO = "S";
            localLiquidacion.ID_USR_SIST = liquidacion.IdUsuarioSistema;
            _mdb.SaveChanges();
            return true;
        }

        public bool ChangeEstado(ILiquidacion liquidacion)
        {
            AgregarDatos(liquidacion);
            T_LIQUIDACIONES liquidaciones = _mdb.T_LIQUIDACIONES.FirstOrDefault(c =>
                c.ID_LIQUIDACION == liquidacion.IdLiquidacion);

            liquidaciones.ID_ESTADO_LIQ = (short)liquidacion.IdEstadoLiquidacion;
            liquidaciones.ID_USR_SIST = liquidacion.IdUsuarioSistema;

            _mdb.SaveChanges();

            return true;
        }

        public bool UpdateLiquidacionBeneficiarioConcepto(int nrocuenta, int idliquidacion, int idsucursal,
            Enums.EstadoBeneficiarioLiquidacion estado)
        {
            IComunDatos comun = new ComunDatos();

            AgregarDatos(comun);

            var obj =
                _mdb.T_BENEFICIARIO_CONCEP_LIQ.Where(
                    c =>
                    c.NRO_CTA_BCO == nrocuenta && c.ID_SUCURSAL == (short)idsucursal &&
                    c.ID_LIQUIDACION == idliquidacion).FirstOrDefault();

            if (obj != null)
            {
                obj.ID_EST_BEN_CON_LIQ = (short)estado;
                obj.ID_USR_SIST = comun.IdUsuarioSistema;
                _mdb.SaveChanges();
            }
            else
            {
                return false;
            }

            return true;
        }

        public int ExisteLiquidacionBeneficiarioConcepto(int nrocuenta, int idliquidacion, int idsucursal)
        {
            var obj =
               _mdb.T_BENEFICIARIO_CONCEP_LIQ.Where(
                   c =>
                   c.NRO_CTA_BCO == nrocuenta && c.ID_SUCURSAL == (short)idsucursal &&
                   c.ID_LIQUIDACION == idliquidacion).FirstOrDefault();

            return obj != null ? obj.ID_BENEFICIARIO : 0;
        }

        public IBeneficiario LiquidacionBeneficiarioConcepto(int nrocuenta, int idliquidacion, int idsucursal)
        {
            var obj =
                _mdb.T_BENEFICIARIO_CONCEP_LIQ.Where(
                    c =>
                    c.NRO_CTA_BCO == nrocuenta && c.ID_SUCURSAL == (short) idsucursal &&
                    c.ID_LIQUIDACION == idliquidacion).Select(
                        c =>
                        new Beneficiario
                            {
                                IdBeneficiario = c.ID_BENEFICIARIO,
                                PagoaApoderado = c.APODERADO,
                                IdApoderadoPago = c.ID_APODERADO,
                                NumeroCuenta = c.NRO_CTA_BCO,
                                IdSucursal = c.ID_SUCURSAL,
                                Ficha = new Ficha {NumeroDocumento = c.T_BENEFICIARIOS.T_FICHAS.NUMERO_DOCUMENTO}
                            }).FirstOrDefault();

            return obj;
        }

        public IList<IBeneficiario> BeneficiarioOnliquidacion(int idliquidacion)
        {
            var obj =
                _mdb.T_BENEFICIARIO_CONCEP_LIQ.Where(
                    c =>
                    c.ID_LIQUIDACION == idliquidacion).Select(
                        c =>
                        new Beneficiario
                            {
                                IdBeneficiario = c.ID_BENEFICIARIO,
                                PagoaApoderado = c.APODERADO,
                                IdApoderadoPago = c.ID_APODERADO,
                                NumeroCuenta = c.NRO_CTA_BCO,
                                IdSucursal = c.ID_SUCURSAL,
                                Ficha =
                                    new Ficha
                                        {
                                            Apellido =
                                                (c.APODERADO == "N"
                                                     ? c.T_BENEFICIARIOS.T_FICHAS.APELLIDO
                                                     : c.T_APODERADOS.APELLIDO ),
                                            Nombre = (c.APODERADO == "N"
                                                          ? c.T_BENEFICIARIOS.T_FICHAS.NOMBRE
                                                          : c.T_APODERADOS.NOMBRE),
                                            NumeroDocumento = (c.APODERADO == "N"
                                                                    ? c.T_BENEFICIARIOS.T_FICHAS.NUMERO_DOCUMENTO
                                                                    : c.T_APODERADOS.NRO_DOCUMENTO)
                                        }
                            }).ToList().Cast<IBeneficiario>().ToList();

            return obj;
        }

        /// <summary>
        /// 18/07/2013 - DI CAMPLI LEANDRO - SE AÑADE PARÁMETRO DE IDLIQUIDACIÓN PARA NO TRAER TODAS LAS LIQ EXISTENTES
        /// </summary>
        /// <param name="idLiquidacion"></param>
        /// <returns></returns>
        private IQueryable<ILiquidacion> QLiquidacion(int idLiquidacion)
        {

            var a = (from li in _mdb.T_LIQUIDACIONES
                                from elq in _mdb.T_ESTADOS_LIQUIDACION.Where(elq => elq.ID_ESTADO_LIQ == li.ID_ESTADO_LIQ).DefaultIfEmpty()
                                //from blq in _mdb.T_BENEFICIARIO_CONCEP_LIQ.Where(
                                //           blq => blq.ID_LIQUIDACION == li.ID_LIQUIDACION && blq.ID_EST_BEN_CON_LIQ != (int)Enums.EstadoBeneficiarioLiquidacion.ExcluidoNoApto && blq.ID_EST_BEN_CON_LIQ != (int)Enums.EstadoBeneficiarioLiquidacion.Excluido).DefaultIfEmpty()
                                //from ben in _mdb.T_BENEFICIARIOS.Where(ben => ben.ID_BENEFICIARIO == blq.ID_BENEFICIARIO).DefaultIfEmpty()
                                //from prog in _mdb.T_PROGRAMAS.Where(prog => prog.ID_PROGRAMA == ben.ID_PROGRAMA).DefaultIfEmpty()
                                from usu in _mdb.T_USUARIOS.Where(usu => usu.ID_USUARIO == li.ID_USR_SIST).DefaultIfEmpty()

                                where li.ID_LIQUIDACION == idLiquidacion

                                select
                                new Liquidacion
                                {
                                    IdLiquidacion = li.ID_LIQUIDACION,
                                    FechaLiquidacion = li.FEC_LIQ ?? DateTime.Now,
                                    NroResolucion = li.NRO_RESOLUCION,
                                    Observacion = li.OBSERVACION,
                                    IdEstadoLiquidacion = li.ID_ESTADO_LIQ ?? 0,
                                    NombreEstado = elq.N_ESTADO_LIQ,
                                    TxtNombre = li.TXT_NOMBRE,
                                    Generado = li.GENERADO == "S" ? true : false,
                                    FechaSistema = li.FEC_SIST,
                                    IdUsuarioSistema = li.ID_USR_SIST,
                                    UsuarioSistema = usu.LOGIN
                                }).SingleOrDefault();

            


            

            ///CARGAR LOS BENEFICIARIOS
            ///
            var b = GetBeneficiariosLiq(idLiquidacion);

            

            if (b != null)
            {
                if (b.Count() > 0)
                {
                    a.CantBenef = b.Count();

                    a.Beneficiarios = b;
                    a.Programa = b.Select(c => c.Programa).FirstOrDefault();

                }
            }
            
            List<ILiquidacion> liq = new List<ILiquidacion>();

            if (a != null)
            {
                liq.Add(a);
            }
            return liq.AsQueryable();
        }


        private List<Beneficiario> GetBeneficiariosLiq(int idLiquidacion)
        {
            var b = from cbl in _mdb.T_BENEFICIARIO_CONCEP_LIQ
                    from ben in _mdb.T_BENEFICIARIOS.Where(ben => ben.ID_BENEFICIARIO == cbl.ID_BENEFICIARIO).DefaultIfEmpty()
                    where cbl.ID_LIQUIDACION == idLiquidacion
                    select new Beneficiario
                    {
                        IdBeneficiario = ben.ID_BENEFICIARIO,
                        IdEstado = ben.ID_ESTADO ?? 0,
                        FechaSistema = ben.FEC_SIST ?? DateTime.Now,
                        IdFicha = ben.ID_FICHA ?? 0,
                        IdPrograma = ben.ID_PROGRAMA ?? 0,
                        
                    };
            int programa = 0;
            programa = b.Take(1).Select(x => x.IdPrograma).SingleOrDefault();

            switch (programa)
            {
                case 1:
                    var bTer = from cbl in _mdb.T_BENEFICIARIO_CONCEP_LIQ
                            from ebl in _mdb.T_ESTADOS_BEN_CON_LIQ.Where(ebl => ebl.ID_EST_BEN_CON_LIQ == cbl.ID_EST_BEN_CON_LIQ).DefaultIfEmpty()
                            from ben in _mdb.T_BENEFICIARIOS.Where(ben => ben.ID_BENEFICIARIO == cbl.ID_BENEFICIARIO).DefaultIfEmpty()
                            from apo in _mdb.T_APODERADOS.Where(apo => apo.ID_APODERADO == cbl.ID_APODERADO).DefaultIfEmpty()
                            from est in _mdb.T_ESTADOS.Where(est => est.ID_ESTADO == ben.ID_ESTADO).DefaultIfEmpty()
                            from prog in _mdb.T_PROGRAMAS.Where(prog => prog.ID_PROGRAMA == ben.ID_PROGRAMA).DefaultIfEmpty()
                            from fic in _mdb.T_FICHAS.Where(fic => fic.ID_FICHA == ben.ID_FICHA).DefaultIfEmpty()
                            from loc in _mdb.T_LOCALIDADES.Where(loc => loc.ID_LOCALIDAD == fic.ID_LOCALIDAD).DefaultIfEmpty()
                            from cu in _mdb.T_CUENTAS_BANCO.Where(cu => cu.ID_BENEFICIARIO == ben.ID_BENEFICIARIO).DefaultIfEmpty()
                            from suc in _mdb.T_TABLAS_BCO_CBA.Where(suc => suc.ID_TABLA_BCO_CBA == cu.ID_SUCURSAL).DefaultIfEmpty()
                            from sucapo in _mdb.T_TABLAS_BCO_CBA.Where(sucapo => sucapo.ID_TABLA_BCO_CBA == apo.ID_SUCURSAL_BCO).DefaultIfEmpty() //19/09/2013 - DI CAMPLI LEANDRO - AÑADIR LA SUC DEL APODERADO
                            //from con in _mdb.T_CONCEPTOS.Where(con=>con.ID_CONCEPTO==cbl.ID_CONCEPTO).DefaultIfEmpty()
                            from ter in _mdb.T_FICHA_TERCIARIO.Where(ter => ter.ID_FICHA == ben.ID_FICHA).DefaultIfEmpty()
                            from sub in _mdb.T_SUBPROGRAMAS.Where(sub => sub.ID_SUBPROGRAMA == ter.ID_SUBPROGRAMA).DefaultIfEmpty()

                            where cbl.ID_LIQUIDACION == idLiquidacion
                            select new Beneficiario
                            {
                                IdBeneficiario = ben.ID_BENEFICIARIO,
                                IdEstado = ben.ID_ESTADO ?? 0,
                                FechaSistema = ben.FEC_SIST ?? DateTime.Now,
                                IdFicha = ben.ID_FICHA ?? 0,
                                IdPrograma = ben.ID_PROGRAMA ?? 0,
                                IdUsuarioSistema = ben.ID_USR_SIST ?? 0,
                                IdConvenio = prog.CONVENIO_BCO_CBA,
                                NumeroConvenio = prog.CONVENIO_BCO_CBA,
                                Nacionalidad = ben.NACIONALIDAD ?? 0,
                                Residente = ben.RESIDENTE,
                                TipoPersona = ben.TIPO_PERSONA,
                                NombreEstado = est.N_ESTADO,
                                ImportePagoPlan = prog.MONTO_PROG ?? 0,
                                MontoLiquidado = cbl.MONTO,
                                Programa = prog.N_PROGRAMA,
                                MontoPrograma = prog.MONTO_PROG ?? 0,
                                TieneApoderado = ben.TIENE_APODERADO,
                                IdEstadoBenConcLiq = cbl.ID_EST_BEN_CON_LIQ ?? 0,
                                Sucursal = apo.ID_SUCURSAL_BCO == null ?
                                    (suc.COD_BCO_CBA + "-" + suc.DESCRIPCION) : (sucapo.COD_BCO_CBA + "-" + sucapo.DESCRIPCION), // 19/09/2013 - DI CAMPLI - SI TIENE APODERADO EL BENEFICIARIO EN LA LIQ, MUESTRA LA SUCURSA DE SU CUENTA
                                NroCuentadePago = cbl.NRO_CTA_BCO ?? 0,
                                PagoaApoderado = cbl.APODERADO,
                                EstadoBeneficiarioConcepto = ebl.N_EST_BEN_CON_LIQ,
                                IdSucursal = suc.ID_TABLA_BCO_CBA,
                                //TipoPrograma = ter.TIPO_PPP ?? 1,
                                idSubprograma = sub.ID_SUBPROGRAMA,
                                subprograma = sub.N_SUBPROGRAMA,
                                IdEmpresa = cbl.ID_EMPRESA,
                                Apoderado = new Apoderado { NumeroDocumento = apo.NRO_DOCUMENTO, Nombre = apo.NOMBRE, Apellido = apo.APELLIDO },
                                Ficha = new Ficha
                                {
                                    IdFicha = fic.ID_FICHA,
                                    Apellido = fic.APELLIDO,
                                    Barrio = fic.BARRIO,
                                    Localidad = loc.N_LOCALIDAD,
                                    IdLocalidad = fic.ID_LOCALIDAD ?? 0,
                                    Nombre = fic.NOMBRE,
                                    NumeroDocumento = fic.NUMERO_DOCUMENTO,
                                    Cuil = fic.CUIL,
                                    IdEmpresa = 0

                                },
                                ConceptosBeneficiario = (from co in _mdb.T_CONCEPTOS
                                                         where co.ID_CONCEPTO == cbl.ID_CONCEPTO
                                                         select
                                                             new Concepto
                                                             {
                                                                 Id_Concepto = co.ID_CONCEPTO,
                                                                 Año = co.ANIO,
                                                                 Mes = co.MES,
                                                                 Observacion = co.OBSERVACION
                                                             }).AsEnumerable()
                            };

                    return bTer.ToList();
                case 2:
                    var bUni = from cbl in _mdb.T_BENEFICIARIO_CONCEP_LIQ
                            from ebl in _mdb.T_ESTADOS_BEN_CON_LIQ.Where(ebl => ebl.ID_EST_BEN_CON_LIQ == cbl.ID_EST_BEN_CON_LIQ).DefaultIfEmpty()
                            from ben in _mdb.T_BENEFICIARIOS.Where(ben => ben.ID_BENEFICIARIO == cbl.ID_BENEFICIARIO).DefaultIfEmpty()
                            from apo in _mdb.T_APODERADOS.Where(apo => apo.ID_APODERADO == cbl.ID_APODERADO).DefaultIfEmpty()
                            from est in _mdb.T_ESTADOS.Where(est => est.ID_ESTADO == ben.ID_ESTADO).DefaultIfEmpty()
                            from prog in _mdb.T_PROGRAMAS.Where(prog => prog.ID_PROGRAMA == ben.ID_PROGRAMA).DefaultIfEmpty()
                            from fic in _mdb.T_FICHAS.Where(fic => fic.ID_FICHA == ben.ID_FICHA).DefaultIfEmpty()
                            from loc in _mdb.T_LOCALIDADES.Where(loc => loc.ID_LOCALIDAD == fic.ID_LOCALIDAD).DefaultIfEmpty()
                            from cu in _mdb.T_CUENTAS_BANCO.Where(cu => cu.ID_BENEFICIARIO == ben.ID_BENEFICIARIO).DefaultIfEmpty()
                            from suc in _mdb.T_TABLAS_BCO_CBA.Where(suc => suc.ID_TABLA_BCO_CBA == cu.ID_SUCURSAL).DefaultIfEmpty()
                            from sucapo in _mdb.T_TABLAS_BCO_CBA.Where(sucapo => sucapo.ID_TABLA_BCO_CBA == apo.ID_SUCURSAL_BCO).DefaultIfEmpty() //19/09/2013 - DI CAMPLI LEANDRO - AÑADIR LA SUC DEL APODERADO
                            //from con in _mdb.T_CONCEPTOS.Where(con=>con.ID_CONCEPTO==cbl.ID_CONCEPTO).DefaultIfEmpty()
                            from uni in _mdb.T_FICHA_UNIVERSITARIO.Where(uni => uni.ID_FICHA == ben.ID_FICHA).DefaultIfEmpty()
                            from sub in _mdb.T_SUBPROGRAMAS.Where(sub => sub.ID_SUBPROGRAMA == uni.ID_SUBPROGRAMA).DefaultIfEmpty()

                            where cbl.ID_LIQUIDACION == idLiquidacion
                            select new Beneficiario
                            {
                                IdBeneficiario = ben.ID_BENEFICIARIO,
                                IdEstado = ben.ID_ESTADO ?? 0,
                                FechaSistema = ben.FEC_SIST ?? DateTime.Now,
                                IdFicha = ben.ID_FICHA ?? 0,
                                IdPrograma = ben.ID_PROGRAMA ?? 0,
                                IdUsuarioSistema = ben.ID_USR_SIST ?? 0,
                                IdConvenio = prog.CONVENIO_BCO_CBA,
                                NumeroConvenio = prog.CONVENIO_BCO_CBA,
                                Nacionalidad = ben.NACIONALIDAD ?? 0,
                                Residente = ben.RESIDENTE,
                                TipoPersona = ben.TIPO_PERSONA,
                                NombreEstado = est.N_ESTADO,
                                ImportePagoPlan = prog.MONTO_PROG ?? 0,
                                MontoLiquidado = cbl.MONTO,
                                Programa = prog.N_PROGRAMA,
                                MontoPrograma = prog.MONTO_PROG ?? 0,
                                TieneApoderado = ben.TIENE_APODERADO,
                                IdEstadoBenConcLiq = cbl.ID_EST_BEN_CON_LIQ ?? 0,
                                Sucursal = apo.ID_SUCURSAL_BCO == null ?
                                    (suc.COD_BCO_CBA + "-" + suc.DESCRIPCION) : (sucapo.COD_BCO_CBA + "-" + sucapo.DESCRIPCION), // 19/09/2013 - DI CAMPLI - SI TIENE APODERADO EL BENEFICIARIO EN LA LIQ, MUESTRA LA SUCURSA DE SU CUENTA
                                NroCuentadePago = cbl.NRO_CTA_BCO ?? 0,
                                PagoaApoderado = cbl.APODERADO,
                                EstadoBeneficiarioConcepto = ebl.N_EST_BEN_CON_LIQ,
                                IdSucursal = suc.ID_TABLA_BCO_CBA,
                                //TipoPrograma = ter.TIPO_PPP ?? 1,
                                idSubprograma = sub.ID_SUBPROGRAMA,
                                subprograma = sub.N_SUBPROGRAMA,
                                IdEmpresa = cbl.ID_EMPRESA,
                                Apoderado = new Apoderado { NumeroDocumento = apo.NRO_DOCUMENTO, Nombre = apo.NOMBRE, Apellido = apo.APELLIDO },
                                Ficha = new Ficha
                                {
                                    IdFicha = fic.ID_FICHA,
                                    Apellido = fic.APELLIDO,
                                    Barrio = fic.BARRIO,
                                    Localidad = loc.N_LOCALIDAD,
                                    IdLocalidad = fic.ID_LOCALIDAD ?? 0,
                                    Nombre = fic.NOMBRE,
                                    NumeroDocumento = fic.NUMERO_DOCUMENTO,
                                    Cuil = fic.CUIL,
                                    IdEmpresa = 0

                                },
                                ConceptosBeneficiario = (from co in _mdb.T_CONCEPTOS
                                                         where co.ID_CONCEPTO == cbl.ID_CONCEPTO
                                                         select
                                                             new Concepto
                                                             {
                                                                 Id_Concepto = co.ID_CONCEPTO,
                                                                 Año = co.ANIO,
                                                                 Mes = co.MES,
                                                                 Observacion = co.OBSERVACION
                                                             }).AsEnumerable()
                            };

                    return bUni.ToList();
                case 3:

                    var bPpp = from cbl in _mdb.T_BENEFICIARIO_CONCEP_LIQ
                            from ebl in _mdb.T_ESTADOS_BEN_CON_LIQ.Where(ebl => ebl.ID_EST_BEN_CON_LIQ == cbl.ID_EST_BEN_CON_LIQ).DefaultIfEmpty()
                            from ben in _mdb.T_BENEFICIARIOS.Where(ben => ben.ID_BENEFICIARIO == cbl.ID_BENEFICIARIO).DefaultIfEmpty()
                            from apo in _mdb.T_APODERADOS.Where(apo => apo.ID_APODERADO == cbl.ID_APODERADO).DefaultIfEmpty()
                            from est in _mdb.T_ESTADOS.Where(est => est.ID_ESTADO == ben.ID_ESTADO).DefaultIfEmpty()
                            from prog in _mdb.T_PROGRAMAS.Where(prog => prog.ID_PROGRAMA == ben.ID_PROGRAMA).DefaultIfEmpty()
                            from fic in _mdb.T_FICHAS.Where(fic => fic.ID_FICHA == ben.ID_FICHA).DefaultIfEmpty()
                            from loc in _mdb.T_LOCALIDADES.Where(loc => loc.ID_LOCALIDAD == fic.ID_LOCALIDAD).DefaultIfEmpty()
                            from cu in _mdb.T_CUENTAS_BANCO.Where(cu => cu.ID_BENEFICIARIO == ben.ID_BENEFICIARIO).DefaultIfEmpty()
                            from suc in _mdb.T_TABLAS_BCO_CBA.Where(suc => suc.ID_TABLA_BCO_CBA == cu.ID_SUCURSAL).DefaultIfEmpty()
                            from sucapo in _mdb.T_TABLAS_BCO_CBA.Where(sucapo => sucapo.ID_TABLA_BCO_CBA == apo.ID_SUCURSAL_BCO).DefaultIfEmpty() //19/09/2013 - DI CAMPLI LEANDRO - AÑADIR LA SUC DEL APODERADO
                            //from con in _mdb.T_CONCEPTOS.Where(con=>con.ID_CONCEPTO==cbl.ID_CONCEPTO).DefaultIfEmpty()
                            from ppp in _mdb.T_FICHA_PPP.Where(ppp => ppp.ID_FICHA == ben.ID_FICHA).DefaultIfEmpty()
                            from sub in _mdb.T_SUBPROGRAMAS.Where(sub => sub.ID_SUBPROGRAMA == ppp.ID_SUBPROGRAMA).DefaultIfEmpty()

                            where cbl.ID_LIQUIDACION == idLiquidacion
                            select new Beneficiario
                            {
                                IdBeneficiario = ben.ID_BENEFICIARIO,
                                IdEstado = ben.ID_ESTADO ?? 0,
                                FechaSistema = ben.FEC_SIST ?? DateTime.Now,
                                IdFicha = ben.ID_FICHA ?? 0,
                                IdPrograma = ben.ID_PROGRAMA ?? 0,
                                IdUsuarioSistema = ben.ID_USR_SIST ?? 0,
                                IdConvenio = prog.CONVENIO_BCO_CBA,
                                NumeroConvenio = prog.CONVENIO_BCO_CBA,
                                Nacionalidad = ben.NACIONALIDAD ?? 0,
                                Residente = ben.RESIDENTE,
                                TipoPersona = ben.TIPO_PERSONA,
                                NombreEstado = est.N_ESTADO,
                                ImportePagoPlan = prog.MONTO_PROG ?? 0,
                                MontoLiquidado = cbl.MONTO,
                                Programa = prog.N_PROGRAMA,
                                MontoPrograma = prog.MONTO_PROG ?? 0,
                                TieneApoderado = ben.TIENE_APODERADO,
                                IdEstadoBenConcLiq = cbl.ID_EST_BEN_CON_LIQ ?? 0,
                                Sucursal = apo.ID_SUCURSAL_BCO == null ?
                                    (suc.COD_BCO_CBA + "-" + suc.DESCRIPCION) : (sucapo.COD_BCO_CBA + "-" + sucapo.DESCRIPCION), // 19/09/2013 - DI CAMPLI - SI TIENE APODERADO EL BENEFICIARIO EN LA LIQ, MUESTRA LA SUCURSA DE SU CUENTA
                                NroCuentadePago = cbl.NRO_CTA_BCO ?? 0,
                                PagoaApoderado = cbl.APODERADO,
                                EstadoBeneficiarioConcepto = ebl.N_EST_BEN_CON_LIQ,
                                IdSucursal = suc.ID_TABLA_BCO_CBA,
                                TipoPrograma = ppp.TIPO_PPP ?? 1,
                                idSubprograma = sub.ID_SUBPROGRAMA,
                                subprograma = sub.N_SUBPROGRAMA,
                                IdEmpresa = cbl.ID_EMPRESA,
                                Apoderado = new Apoderado { NumeroDocumento = apo.NRO_DOCUMENTO, Nombre = apo.NOMBRE, Apellido = apo.APELLIDO },
                                Ficha = new Ficha
                                {
                                    IdFicha = fic.ID_FICHA,
                                    Apellido = fic.APELLIDO,
                                    Barrio = fic.BARRIO,
                                    Localidad = loc.N_LOCALIDAD,
                                    IdLocalidad = fic.ID_LOCALIDAD ?? 0,
                                    Nombre = fic.NOMBRE,
                                    NumeroDocumento = fic.NUMERO_DOCUMENTO,
                                    Cuil = fic.CUIL,
                                    IdEmpresa = ppp.ID_EMPRESA ?? 0

                                },
                                ConceptosBeneficiario = (from co in _mdb.T_CONCEPTOS
                                                         where co.ID_CONCEPTO == cbl.ID_CONCEPTO
                                                         select
                                                             new Concepto
                                                             {
                                                                 Id_Concepto = co.ID_CONCEPTO,
                                                                 Año = co.ANIO,
                                                                 Mes = co.MES,
                                                                 Observacion = co.OBSERVACION
                                                             }).AsEnumerable()
                            };

                    return bPpp.ToList();
                default:

                  var bdef =  from cbl in _mdb.T_BENEFICIARIO_CONCEP_LIQ
                        from ebl in _mdb.T_ESTADOS_BEN_CON_LIQ.Where(ebl=>ebl.ID_EST_BEN_CON_LIQ==cbl.ID_EST_BEN_CON_LIQ).DefaultIfEmpty()
                        from ben in _mdb.T_BENEFICIARIOS.Where(ben=>ben.ID_BENEFICIARIO==cbl.ID_BENEFICIARIO).DefaultIfEmpty()
                        from apo in _mdb.T_APODERADOS.Where(apo=>apo.ID_APODERADO==cbl.ID_APODERADO).DefaultIfEmpty()
                        from est in _mdb.T_ESTADOS.Where(est=>est.ID_ESTADO==ben.ID_ESTADO).DefaultIfEmpty()
                        from prog in _mdb.T_PROGRAMAS.Where(prog=>prog.ID_PROGRAMA==ben.ID_PROGRAMA).DefaultIfEmpty()
                        from fic in _mdb.T_FICHAS.Where(fic=>fic.ID_FICHA==ben.ID_FICHA).DefaultIfEmpty()
                        from loc in _mdb.T_LOCALIDADES.Where(loc=>loc.ID_LOCALIDAD==fic.ID_LOCALIDAD).DefaultIfEmpty()
                        from cu in _mdb.T_CUENTAS_BANCO.Where(cu=>cu.ID_BENEFICIARIO == ben.ID_BENEFICIARIO).DefaultIfEmpty()
                        from suc in _mdb.T_TABLAS_BCO_CBA.Where(suc => suc.ID_TABLA_BCO_CBA == cu.ID_SUCURSAL).DefaultIfEmpty()
                        from sucapo in _mdb.T_TABLAS_BCO_CBA.Where(sucapo => sucapo.ID_TABLA_BCO_CBA == apo.ID_SUCURSAL_BCO).DefaultIfEmpty() //19/09/2013 - DI CAMPLI LEANDRO - AÑADIR LA SUC DEL APODERADO
                        //from con in _mdb.T_CONCEPTOS.Where(con=>con.ID_CONCEPTO==cbl.ID_CONCEPTO).DefaultIfEmpty()

                        where cbl.ID_LIQUIDACION==idLiquidacion
                         select new Beneficiario
                                {
                                    IdBeneficiario = ben.ID_BENEFICIARIO,
                                    IdEstado = ben.ID_ESTADO ?? 0,
                                    FechaSistema = ben.FEC_SIST ?? DateTime.Now,
                                    IdFicha = ben.ID_FICHA ?? 0,
                                    IdPrograma = ben.ID_PROGRAMA ?? 0,
                                    IdUsuarioSistema = ben.ID_USR_SIST ?? 0,
                                    IdConvenio = prog.CONVENIO_BCO_CBA,
                                    NumeroConvenio = prog.CONVENIO_BCO_CBA,
                                    Nacionalidad = ben.NACIONALIDAD ?? 0,
                                    Residente = ben.RESIDENTE,
                                    TipoPersona = ben.TIPO_PERSONA,
                                    NombreEstado = est.N_ESTADO,
                                    ImportePagoPlan = prog.MONTO_PROG ?? 0,
                                    MontoLiquidado = cbl.MONTO,
                                    Programa = prog.N_PROGRAMA,
                                    MontoPrograma = prog.MONTO_PROG ?? 0,
                                    TieneApoderado = ben.TIENE_APODERADO,
                                    IdEstadoBenConcLiq = cbl.ID_EST_BEN_CON_LIQ ?? 0,
                                    Sucursal = apo.ID_SUCURSAL_BCO == null ?
                                        (suc.COD_BCO_CBA + "-" + suc.DESCRIPCION) : (sucapo.COD_BCO_CBA + "-" + sucapo.DESCRIPCION), // 19/09/2013 - DI CAMPLI - SI TIENE APODERADO EL BENEFICIARIO EN LA LIQ, MUESTRA LA SUCURSA DE SU CUENTA
                                    NroCuentadePago = cbl.NRO_CTA_BCO ?? 0,
                                    PagoaApoderado = cbl.APODERADO,
                                    EstadoBeneficiarioConcepto = ebl.N_EST_BEN_CON_LIQ,
                                    IdSucursal = suc.ID_TABLA_BCO_CBA,
                                    IdEmpresa = cbl.ID_EMPRESA,
                                    Apoderado = new Apoderado { NumeroDocumento = apo.NRO_DOCUMENTO, Nombre = apo.NOMBRE, Apellido = apo.APELLIDO },
                                    Ficha = new Ficha
                                    {
                                        IdFicha = fic.ID_FICHA,
                                        Apellido = fic.APELLIDO,
                                        Barrio = fic.BARRIO,
                                        Localidad = loc.N_LOCALIDAD,
                                        IdLocalidad = fic.ID_LOCALIDAD ?? 0,
                                        Nombre = fic.NOMBRE,
                                        NumeroDocumento = fic.NUMERO_DOCUMENTO,
                                        Cuil = fic.CUIL,
                                        IdEmpresa =  0
                                    },
                                    ConceptosBeneficiario = (from co in _mdb.T_CONCEPTOS
                                                             where co.ID_CONCEPTO==cbl.ID_CONCEPTO
                                                             select
                                                                 new Concepto
                                                                 {
                                                                     Id_Concepto = co.ID_CONCEPTO,
                                                                     Año = co.ANIO,
                                                                     Mes = co.MES,
                                                                     Observacion = co.OBSERVACION
                                                                 }).AsEnumerable()
                                };

                  return bdef.ToList();

            }

        
        
        }


    }
}
