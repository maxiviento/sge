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
    public class ApoderadoRepositorio : BaseRepositorio, IApoderadoRepositorio
    {
        private readonly DataSGE _mdb;

        public ApoderadoRepositorio()
        {
            _mdb = new DataSGE();
        }

        private IQueryable<IApoderado> QApoderado()
        {
            var a = (from c in _mdb.T_APODERADOS
                     from suc in
                         _mdb.T_TABLAS_BCO_CBA.Where(suc => suc.ID_TABLA_BCO_CBA == c.ID_SUCURSAL_BCO).DefaultIfEmpty()
                     //on cu.ID_SUCURSAL equals suc.ID_TABLA_BCO_CBA
                     from mon in
                         _mdb.T_TABLAS_BCO_CBA.Where(mon => mon.ID_TABLA_BCO_CBA == c.ID_MONEDA).DefaultIfEmpty()
                     //on cu.ID_MONEDA equals mon.ID_TABLA_BCO_CBA
                     from est in
                         _mdb.T_ESTADOS_APODERADO.Where(est => est.ID_ESTADO_APODERADO == c.ID_ESTADO_APODERADO).DefaultIfEmpty()
                     from sis in
                         _mdb.T_TABLAS_BCO_CBA.Where(sis => sis.ID_TABLA_BCO_CBA == c.ID_SISTEMA).DefaultIfEmpty()
                     select
                         new Apoderado
                             {
                                 IdApoderado = c.ID_APODERADO,
                                 IdBeneficiario = c.ID_BENEFICIARIO ?? 0,
                                 Apellido = c.APELLIDO,
                                 Nombre = c.NOMBRE,
                                 TipoDocumento = c.TIPO_DOCUMENTO ?? 0,
                                 NumeroDocumento = c.NRO_DOCUMENTO,
                                 Cuil = c.CUIL,
                                 Sexo = c.SEXO,
                                 IdSistema = c.ID_SISTEMA ?? 0,
                                 NumeroCuentaBco = c.NRO_CUENTA_BCO ?? 0,
                                 Cbu = c.CBU,
                                 UsuarioBanco = c.USUARIO_BANCO,
                                 IdSucursal = suc.ID_TABLA_BCO_CBA,
                                 CodigoSucursalBanco = suc.COD_BCO_CBA,
                                 IdMoneda = 0,
                                 ApoderadoDesde = c.APODERADO_DESDE,
                                 ApoderadoHasta = c.APODERADO_HASTA,
                                 IdEstadoApoderado = c.ID_ESTADO_APODERADO ?? 0,
                                 NombreEstadoApoderado = est.N_ESTADO_APODERADO,
                                 FechaNacimiento = c.FER_NAC ?? DateTime.Now,
                                 Calle = c.CALLE,
                                 Numero = c.NUMERO,
                                 Piso = c.PISO,
                                 Dpto = c.DPTO,
                                 Monoblock = c.MONOBLOCK,
                                 Parcela = c.PARCELA,
                                 Manzana = c.MANZANA,
                                 EntreCalles = c.ENTRECALLES,
                                 Barrio = c.BARRIO,
                                 CodigoPostal = c.CODIGO_POSTAL,
                                 IdLocalidad = c.ID_LOCALIDAD,
                                 TelefonoCelular = c.TEL_CELULAR,
                                 TelefonoFijo = c.TEL_FIJO,
                                 Mail = c.MAIL,
                                 Localidad = c.T_LOCALIDADES.N_LOCALIDAD ?? "",
                                 CodigoMoneda = mon.COD_BCO_CBA,
                                 FechaSolicitudCuenta = c.FEC_SOL_CTA,
                                 FechaSistema = c.FEC_SIST,
                                 IdUsuarioSistema = c.ID_USR_SIST,
                                 UsuarioSistema = c.T_USUARIOS.LOGIN
                             });
            return a;
        }

        public IApoderado GetApoderado(int idApoderado)
        {
            return QApoderado().Where(c => c.IdApoderado == idApoderado).SingleOrDefault();
        }

        public IApoderado GetApoderado(string nroDocumento)
        {
            return QApoderado().Where(c => c.NumeroDocumento == nroDocumento).SingleOrDefault();
        }

        public IList<IApoderado> GetApoderadosByBeneficiario(int idBeneficiario)
        {
            return QApoderado().Where(c => c.IdBeneficiario == idBeneficiario).OrderBy(c => c.IdEstadoApoderado).ThenBy(c => c.ApoderadoDesde).ToList();
        }

        public int AddApoderado(IApoderado apoderado)
        {
            AgregarDatos(apoderado);

            var apoderadoModel = new T_APODERADOS
                                     {
                                         ID_APODERADO = SecuenciaRepositorio.GetId(),
                                         ID_BENEFICIARIO = apoderado.IdBeneficiario,
                                         APELLIDO = (apoderado.Apellido ?? "").ToUpper(),
                                         NOMBRE = (apoderado.Nombre ?? "").ToUpper(),
                                         TIPO_DOCUMENTO = apoderado.TipoDocumento,
                                         NRO_DOCUMENTO = apoderado.NumeroDocumento,
                                         CUIL = apoderado.Cuil,
                                         SEXO = apoderado.Sexo,
                                         ID_SISTEMA = apoderado.IdSistema,
                                         NRO_CUENTA_BCO = apoderado.NumeroCuentaBco == 0 ? null : apoderado.NumeroCuentaBco,
                                         CBU = apoderado.Cbu,
                                         USUARIO_BANCO = apoderado.UsuarioBanco,
                                         ID_SUCURSAL_BCO = apoderado.IdSucursal == 0 ? null : apoderado.IdSucursal,
                                         ID_MONEDA = Convert.ToInt16(apoderado.IdMoneda),
                                         APODERADO_DESDE = apoderado.ApoderadoDesde,
                                         APODERADO_HASTA = apoderado.ApoderadoHasta,
                                         ID_ESTADO_APODERADO = apoderado.IdEstadoApoderado,
                                         FER_NAC = apoderado.FechaNacimiento,
                                         CALLE = (apoderado.Calle ?? "").ToUpper(),
                                         NUMERO = apoderado.Numero,
                                         PISO = apoderado.Piso,
                                         DPTO = apoderado.Dpto,
                                         MONOBLOCK = apoderado.Monoblock,
                                         PARCELA = apoderado.Parcela,
                                         MANZANA = apoderado.Manzana,
                                         ENTRECALLES = (apoderado.EntreCalles ?? "").ToUpper(),
                                         BARRIO = apoderado.Barrio,
                                         CODIGO_POSTAL = apoderado.CodigoPostal,
                                         ID_LOCALIDAD = apoderado.IdLocalidad,
                                         TEL_FIJO = apoderado.TelefonoFijo,
                                         TEL_CELULAR = apoderado.TelefonoCelular,
                                         MAIL = apoderado.Mail,
                                         ID_USR_SIST = apoderado.IdUsuarioSistema,
                                         FEC_SIST = apoderado.FechaSistema
                                     };

            _mdb.T_APODERADOS.AddObject(apoderadoModel);
            _mdb.SaveChanges();

            UpdateFlagTieneApoderado(apoderado);

            return apoderadoModel.ID_APODERADO;
        }

        public void UpdateApoderado(IApoderado apoderado)
        {
            AgregarDatos(apoderado);

            var obj = _mdb.T_APODERADOS.SingleOrDefault(c => c.ID_APODERADO == apoderado.IdApoderado);

            if (obj != null)
            {
                obj.ID_BENEFICIARIO = apoderado.IdBeneficiario;
                obj.APELLIDO = (apoderado.Apellido ?? "").ToUpper();
                obj.NOMBRE = (apoderado.Nombre ?? "").ToUpper();
                obj.TIPO_DOCUMENTO = apoderado.TipoDocumento;
                obj.NRO_DOCUMENTO = apoderado.NumeroDocumento;
                obj.CUIL = apoderado.Cuil;
                obj.SEXO = apoderado.Sexo;
                obj.ID_SISTEMA = apoderado.IdSistema;
                obj.NRO_CUENTA_BCO = apoderado.NumeroCuentaBco == 0 ? null : apoderado.NumeroCuentaBco;
                obj.CBU = apoderado.Cbu;
                obj.USUARIO_BANCO = apoderado.UsuarioBanco;
                obj.ID_SUCURSAL_BCO = apoderado.IdSucursal == 0 ? null : apoderado.IdSucursal;
                obj.ID_MONEDA = Convert.ToInt16(apoderado.IdMoneda);
                obj.APODERADO_DESDE = apoderado.ApoderadoDesde;
                obj.APODERADO_HASTA = apoderado.ApoderadoHasta;
                obj.ID_ESTADO_APODERADO = apoderado.IdEstadoApoderado;
                obj.FER_NAC = apoderado.FechaNacimiento;
                obj.CALLE = (apoderado.Calle ?? "").ToUpper();
                obj.NUMERO = apoderado.Numero;
                obj.PISO = apoderado.Piso;
                obj.DPTO = apoderado.Dpto;
                obj.MONOBLOCK = apoderado.Monoblock;
                obj.PARCELA = apoderado.Parcela;
                obj.MANZANA = apoderado.Manzana;
                obj.ENTRECALLES = (apoderado.EntreCalles ?? "").ToUpper();
                obj.BARRIO = apoderado.Barrio;
                obj.CODIGO_POSTAL = apoderado.CodigoPostal;
                obj.ID_LOCALIDAD = apoderado.IdLocalidad;
                obj.TEL_FIJO = apoderado.TelefonoFijo;
                obj.TEL_CELULAR = apoderado.TelefonoCelular;
                obj.MAIL = apoderado.Mail;
                obj.ID_USR_SIST = apoderado.IdUsuarioSistema;

                _mdb.SaveChanges();

            }
        }

        public bool UpdateApoderadoSimple(IApoderado apoderado)
        {
            AgregarDatos(apoderado);

            var obj = _mdb.T_APODERADOS.SingleOrDefault(c => c.ID_APODERADO == apoderado.IdApoderado);


            if (obj != null)
            {

                obj.APELLIDO = apoderado.Apellido;
                obj.NOMBRE = apoderado.Nombre;
                obj.ID_SUCURSAL_BCO = Convert.ToInt16(apoderado.IdSucursal);
                obj.NRO_CUENTA_BCO = apoderado.NumeroCuentaBco;
                obj.CBU = apoderado.Cbu;
                obj.ID_USR_SIST = apoderado.IdUsuarioSistema;
                _mdb.SaveChanges();

            }

            return true;
        }

        public bool ExistsApoderado(string nroDocumento)
        {
            nroDocumento = nroDocumento == "" ? null : nroDocumento;

            var t = (QApoderado().Where(c => c.NumeroDocumento == nroDocumento)).ToList().Count;

            return t > 0 ? true : false;
        }

        public bool EsApoderadoDeOtroBeneficiario(string nroDocumento, int idBeneficiario)
        {
            var t = (from c in QApoderado()
                     where c.NumeroDocumento == nroDocumento.Trim()
                           && c.IdBeneficiario != idBeneficiario
                           && c.IdEstadoApoderado == (int)Enums.EstadoApoderado.Activo
                     select c).ToList().Count;
            return t > 0 ? true : false;
        }

        public void SuspenderBeneficiarios(string nroDocumento)
        {
            IComunDatos comun = new ComunDatos();

            AgregarDatos(comun);

            var obj = _mdb.T_APODERADOS.Where(c => c.NRO_DOCUMENTO == nroDocumento).ToList();

            if (obj.Count() != 0)
            {
                foreach (var apoderado in obj)
                {
                    apoderado.ID_ESTADO_APODERADO = (int)Enums.EstadoApoderado.Suspendido;
                    apoderado.APODERADO_HASTA = DateTime.Now;
                    apoderado.ID_USR_SIST = comun.IdUsuarioSistema;
                    _mdb.SaveChanges();
                }
            }
        }

        public bool TieneApoderadoActivo(int idBeneficiario)
        {
            var q = (from c in QApoderado()
                     where c.IdBeneficiario == idBeneficiario
                           && c.IdEstadoApoderado == (int)Enums.EstadoApoderado.Activo
                     select c).ToList().Count;

            return q > 0 ? true : false;
        }

        public bool UpdateCuentaCbuByBeneficiario(int idBeneficiario, string nroCuenta, string cbu, int idsucursal)
        {
            IComunDatos comun = new ComunDatos();

            AgregarDatos(comun);

            var apoderados =
                _mdb.T_APODERADOS.Where(
                    c =>
                    c.ID_BENEFICIARIO == idBeneficiario && c.APODERADO_HASTA == null &&
                    c.ID_ESTADO_APODERADO == (int)Enums.EstadoApoderado.Activo).ToList();

            foreach (var apoderado in apoderados)
            {
                apoderado.NRO_CUENTA_BCO = Convert.ToInt32(nroCuenta);
                apoderado.ID_USR_SIST = comun.IdUsuarioSistema;
                apoderado.CBU = cbu;
            }

            _mdb.SaveChanges();
            return true;
        }

        public IApoderado GetApoderadoByBeneficiarioActivo(int idBeneficiario)
        {
            return
                QApoderado().Where(
                    c =>
                    c.IdBeneficiario == idBeneficiario && c.IdEstadoApoderado == (short)Enums.EstadoApoderado.Activo).
                    SingleOrDefault();
        }

        public IList<IApoderado> GetApoderadosByNumeroDocumento(string numeroDocumento)
        {
            return QApoderado().Where(c => c.NumeroDocumento == numeroDocumento && c.IdEstadoApoderado == (short)Enums.EstadoApoderado.Activo).ToList();
        }

        public bool UpdateFechaSolicitud(int idapoderado, DateTime fecha)
        {
            IComunDatos comun = new ComunDatos();

            AgregarDatos(comun);

            var apoderados = _mdb.T_APODERADOS.Where(c => c.ID_APODERADO == idapoderado).ToList();

            foreach (var apoderado in apoderados)
            {
                apoderado.FEC_SOL_CTA = fecha;
                apoderado.ID_USR_SIST = comun.IdUsuarioSistema;

            }

            _mdb.SaveChanges();
            return true;
        }

        public void SuspenderApoderadoActivoByBeneficiario(int idbeneficiario)
        {
            IComunDatos comun = new ComunDatos();

            AgregarDatos(comun);

            var obj = _mdb.T_APODERADOS.Where(c => c.ID_BENEFICIARIO == idbeneficiario && c.ID_ESTADO_APODERADO == (int)Enums.EstadoApoderado.Activo).ToList();

            if (obj.Count() != 0)
            {
                foreach (var apoderado in obj)
                {
                    apoderado.ID_ESTADO_APODERADO = (int)Enums.EstadoApoderado.Suspendido;
                    apoderado.APODERADO_HASTA = DateTime.Now;
                    apoderado.ID_USR_SIST = comun.IdUsuarioSistema;
                    _mdb.SaveChanges();
                }
            }
        }


        public void ActivarApoderadoActivoByBeneficiario(int idbeneficiario)
        {
            IComunDatos comun = new ComunDatos();
            int cant = 0;
            AgregarDatos(comun);

            var obj = _mdb.T_APODERADOS.Where(c => c.ID_BENEFICIARIO == idbeneficiario).OrderByDescending(c=>c.APODERADO_HASTA).ToList();

            if (obj.Count() != 0)
            {
                if (cant == 0)
                {
                    foreach (var apoderado in obj)
                    {
                        apoderado.ID_ESTADO_APODERADO = (int)Enums.EstadoApoderado.Activo;
                        apoderado.APODERADO_HASTA = null;
                        apoderado.ID_USR_SIST = comun.IdUsuarioSistema;
                        _mdb.SaveChanges();
                        cant++;
                    }
                }
            }
        }

        public bool DeleteApoderado(IApoderado apoderado)
        {
            AgregarDatos(apoderado);

            _mdb.Connection.Open();
            DbTransaction localtransaccion = _mdb.Connection.BeginTransaction();
            try
            {
                var obj = _mdb.T_APODERADOS.SingleOrDefault(c => c.ID_APODERADO == apoderado.IdApoderado);

                obj.ID_USR_SIST = apoderado.IdUsuarioSistema;

                _mdb.SaveChanges();

                _mdb.DeleteObject(obj);

                _mdb.SaveChanges();

                localtransaccion.Commit();

                return true;
            }
            catch (Exception)
            {
                localtransaccion.Rollback();
                return false;
            }

        }

        public IApoderado GetApoderadosByNroCuentaAndSucursalActivo(int nrocuenta, int idsucursal)
        {
            return
                QApoderado().Where(
                    c =>
                    c.NumeroCuentaBco == nrocuenta && c.IdSucursal == idsucursal &&
                    c.IdEstadoApoderado == (short)Enums.EstadoApoderado.Activo).FirstOrDefault();
        }

        public bool CambiarEstado(IApoderado apoderado)
        {
            AgregarDatos(apoderado);

            var obj = _mdb.T_APODERADOS.SingleOrDefault(c => c.ID_APODERADO == apoderado.IdApoderado);

            if (obj != null)
            {
                obj.APODERADO_HASTA = (apoderado.IdEstadoApoderado == (short)Enums.EstadoApoderado.Activo
                                           ? null
                                           : apoderado.ApoderadoHasta);
                obj.ID_ESTADO_APODERADO = apoderado.IdEstadoApoderado;
                obj.ID_USR_SIST = apoderado.IdUsuarioSistema;
                _mdb.SaveChanges();

            }

            return true;
        }

        public bool LimpiarFechaSolicitud(int idapoderado)
        {
            IComunDatos comun = new ComunDatos();

            AgregarDatos(comun);

            var obj = _mdb.T_APODERADOS.SingleOrDefault(c => c.ID_APODERADO == idapoderado);

            if (obj != null)
            {
                obj.FEC_SOL_CTA = null;
                obj.ID_USR_SIST = comun.IdUsuarioSistema;
                _mdb.SaveChanges();
            }

            return true;
        }

        public bool EsApoderadoActivo(string nroDocumento)
        {
            return
                QApoderado().Where(
                    c =>
                    c.NumeroDocumento == nroDocumento && c.IdEstadoApoderado == (short)Enums.EstadoApoderado.Activo).
                    Count() > 0
                    ? true
                    : false;
        }

        private void UpdateFlagTieneApoderado(IApoderado apoderado)
        {
            AgregarDatos(apoderado);

            var beneficiario = _mdb.T_BENEFICIARIOS.SingleOrDefault(c => c.ID_BENEFICIARIO == apoderado.IdBeneficiario);

            if (beneficiario != null)
            {
                if (apoderado.IdBeneficiario != null)
                {
                    bool tieneApoderado = TieneApoderadoActivo(apoderado.IdBeneficiario.Value);

                    if (tieneApoderado)
                    {
                        beneficiario.TIENE_APODERADO = "S";
                    }
                    else
                    {
                        beneficiario.TIENE_APODERADO = "N";
                    }
                }

                beneficiario.ID_USR_SIST = apoderado.IdUsuarioSistema;
                _mdb.SaveChanges();
            }
        }

    }
}
