using System;
using System.Collections.Generic;
using System.Linq;
using SGE.Model.Comun;
using SGE.Model.Entidades;
using SGE.Model.Entidades.Interfaces;
using SGE.Model.Repositorio;
using SGE.Repositorio.Modelo;

namespace SGE.Repositorio
{
    public class ConceptoRepositorio : BaseRepositorio, IConceptoRepositorio
    {
        private readonly DataSGE _mdb;

        public ConceptoRepositorio()
        {
            _mdb = new DataSGE();
        }

        private IQueryable<IConcepto> QConcepto()
        {
            var a = from c in _mdb.T_CONCEPTOS
                    select new Concepto
                               {
                                   Id_Concepto = c.ID_CONCEPTO,
                                   Año = c.ANIO,
                                   Mes = c.MES,
                                   Observacion = c.OBSERVACION,
                                   IdUsuarioSistema = c.ID_USR_SIST,
                                   FechaSistema = c.FEC_SIST,
                                   UsuarioSistema = c.T_USUARIOS.LOGIN
                               };

            return a;
        }

        public IList<IConcepto> GetConceptos(short? año, short? mes, string observacion)
        {
            return
                QConcepto().Where(
                    c =>
                    (c.Año == año || año == null) &&
                    (c.Mes == mes || mes == null) &&
                    (c.Observacion.ToUpper().Contains(observacion.ToUpper()) || String.IsNullOrEmpty(observacion.Trim())))
                    .OrderByDescending(c => c.Año).ThenByDescending(c => c.Mes).ToList();

        }

        public IList<IConcepto> GetConceptos(short? año, short? mes, string obserbacion, int skip, int take)
        {
            return QConcepto().Where(c =>
                (c.Año == año || año == null) &&
                (c.Mes == mes || mes == null) &&
                (c.Observacion.Contains(obserbacion.Trim())
                || String.IsNullOrEmpty(obserbacion))).OrderByDescending(c => c.Año)
                .ThenByDescending(c => c.Mes).Skip(skip).Take(take).ToList();
        }

        public IList<IConcepto> GetConceptosPagosBeneficiario(int idbeneficiario)
        {
            return (from c in QConcepto()
                    join belico in _mdb.T_BENEFICIARIO_CONCEP_LIQ on c.Id_Concepto equals belico.ID_CONCEPTO
                    where belico.ID_BENEFICIARIO == idbeneficiario
                    select c).
                ToList();
        }

        public IConcepto GetConcepto(int idConcepto)
        {
            return QConcepto().Where(c => c.Id_Concepto == idConcepto).SingleOrDefault();
        }

        public IConcepto GetProximoConcepto()
        {
            var obj = _mdb.T_CONCEPTOS.OrderByDescending(c => c.ANIO).ThenByDescending(c => c.MES).FirstOrDefault();
            IConcepto concepto = new Concepto();

            if (obj != null)
            {
                if (obj.MES < 12)
                {
                    concepto.Año = obj.ANIO;
                    concepto.Mes = ++obj.MES;
                }
                else
                {
                    concepto.Año = ++obj.ANIO;
                    concepto.Mes = 1;
                }
            }
            else
            {
                concepto.Año = short.Parse(DateTime.Now.Year.ToString());
                concepto.Mes = short.Parse(DateTime.Now.Month.ToString());
            }

            return concepto;
        }

        public int GetCountConceptos()
        {
            return QConcepto().Count();
        }

        public IList<IConcepto> GetConceptos(int skip, int take)
        {
            return QConcepto().OrderByDescending(c => c.Año).ThenByDescending(c => c.Mes).Skip(skip).Take(take).ToList();
        }

        public int AddConcepto(IConcepto concepto)
        {
            AgregarDatos(concepto);
            
            var conceptoModel = new T_CONCEPTOS
                                    {
                                        ID_CONCEPTO = SecuenciaRepositorio.GetId(),
                                        ANIO = concepto.Año,
                                        MES = concepto.Mes,
                                        OBSERVACION = concepto.Observacion,
                                        ID_USR_SIST = concepto.IdUsuarioSistema,
                                        FEC_SIST = concepto.FechaSistema
                                    };

            _mdb.T_CONCEPTOS.AddObject(conceptoModel);
            _mdb.SaveChanges();

            return conceptoModel.ID_CONCEPTO;
        }

        public bool UpdateConcepto(IConcepto concepto)
        {
            AgregarDatos(concepto);
            
            var obj = _mdb.T_CONCEPTOS.FirstOrDefault(c => c.ID_CONCEPTO == concepto.Id_Concepto);
            obj.OBSERVACION = concepto.Observacion;
            obj.ID_USR_SIST = concepto.IdUsuarioSistema;

            _mdb.SaveChanges();

            return true;
        }

        public IList<IConcepto> GetConceptosByBeneficiario(int idbeneficiario)
        {
            var a =
                _mdb.T_BENEFICIARIO_CONCEP_LIQ.Where(
                    c =>
                    c.ID_BENEFICIARIO == idbeneficiario &&
                    c.T_LIQUIDACIONES.ID_ESTADO_LIQ != (int) Enums.EstadoLiquidacion.Cancelado).Select(
                        c =>
                        new Concepto
                            {
                                Año = c.T_CONCEPTOS.ANIO,
                                Mes = c.T_CONCEPTOS.MES,
                                Estado = c.T_ESTADOS_BEN_CON_LIQ.N_EST_BEN_CON_LIQ,
                                NroCuenta = c.NRO_CTA_BCO,
                                SePagoaApoderado = c.APODERADO,
                                Sucursal = (c.T_TABLAS_BCO_CBA.COD_BCO_CBA + " - " + c.T_TABLAS_BCO_CBA.DESCRIPCION),
                                NroResolucion = c.T_LIQUIDACIONES.NRO_RESOLUCION,
                                FechaLiquidacion = c.T_LIQUIDACIONES.FEC_LIQ ?? DateTime.Now,
                                MontoPagado = c.MONTO
                            }).OrderByDescending(o => o.Año).ThenByDescending(o => o.Mes).ToList().Cast<IConcepto>().
                    ToList();

            return a;
        }
        
        public IConcepto GetConceptosByBeneficiarioEfectores(int idbeneficiario, int idLiquidacion)
        {
            var a =
                _mdb.T_BENEFICIARIO_CONCEP_LIQ.Where(
                    c =>
                    c.ID_BENEFICIARIO == idbeneficiario &&
                    c.ID_LIQUIDACION == idLiquidacion /*&& // 18/07/2013 - DI CAMPLI - DA ERROR AL TRAER UN NULL POR FIRSTORDEFAULT - LA CONDICION SE AÑADE EN EL MONTO, SI EL ESTADO ES CANCELADO QUE TOME 0
                    c.T_LIQUIDACIONES.ID_ESTADO_LIQ != (int) Enums.EstadoLiquidacion.Cancelado*/).Select(
                        c =>
                        new Concepto
                        {
                            Año = c.T_CONCEPTOS.ANIO,
                            Mes = c.T_CONCEPTOS.MES,
                            Estado = c.T_ESTADOS_BEN_CON_LIQ.N_EST_BEN_CON_LIQ,
                            NroCuenta = c.NRO_CTA_BCO,
                            SePagoaApoderado = c.APODERADO,
                            Sucursal = (c.T_TABLAS_BCO_CBA.COD_BCO_CBA + " - " + c.T_TABLAS_BCO_CBA.DESCRIPCION),
                            NroResolucion = c.T_LIQUIDACIONES.NRO_RESOLUCION,
                            FechaLiquidacion = c.T_LIQUIDACIONES.FEC_LIQ ?? DateTime.Now,
                            MontoPagado = c.T_LIQUIDACIONES.ID_ESTADO_LIQ != (int) Enums.EstadoLiquidacion.Cancelado ? c.MONTO : 0
                        });

            IConcepto concepto = new Concepto();

            concepto.MontoPagado = a.FirstOrDefault().MontoPagado == null ? 0 : a.FirstOrDefault().MontoPagado;

            return concepto;
        }

        public IList<IConcepto> GetConceptosByBeneficiario(int idbeneficiario, int skip, int take)
        {
            var a =
                _mdb.T_BENEFICIARIO_CONCEP_LIQ.Where(
                    c =>
                    c.ID_BENEFICIARIO == idbeneficiario &&
                    c.T_LIQUIDACIONES.ID_ESTADO_LIQ != (int) Enums.EstadoLiquidacion.Cancelado).Select(
                        c =>
                        new Concepto
                            {
                                Año = c.T_CONCEPTOS.ANIO,
                                Mes = c.T_CONCEPTOS.MES,
                                Estado = c.T_ESTADOS_BEN_CON_LIQ.N_EST_BEN_CON_LIQ,
                                NroCuenta = c.NRO_CTA_BCO,
                                SePagoaApoderado = c.APODERADO,
                                Sucursal = (c.T_TABLAS_BCO_CBA.COD_BCO_CBA + " - " + c.T_TABLAS_BCO_CBA.DESCRIPCION),
                                NroResolucion = c.T_LIQUIDACIONES.NRO_RESOLUCION,
                                FechaLiquidacion = c.T_LIQUIDACIONES.FEC_LIQ ?? DateTime.Now
                            }).OrderByDescending(o => o.Año).ThenByDescending(o => o.Mes).Skip(skip).Take(take).ToList()
                    .Cast<IConcepto>().ToList();

            return a;
        }
    }
}
