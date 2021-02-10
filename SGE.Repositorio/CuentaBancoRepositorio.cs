using System;
using System.Collections.Generic;
using System.Linq;
using SGE.Model.Entidades;
using SGE.Model.Entidades.Interfaces;
using SGE.Model.Repositorio;
using SGE.Repositorio.Modelo;

namespace SGE.Repositorio
{
    public class CuentaBancoRepositorio : BaseRepositorio, ICuentaBancoRepositorio
    {
        private readonly DataSGE _mdb;

        public CuentaBancoRepositorio()
        {
            _mdb = new DataSGE();
        }

        private IQueryable<ICuentaBanco> QCuentaBanco()
        {
            return
                _mdb.T_CUENTAS_BANCO.Select(
                    c =>
                    new CuentaBanco
                        {
                            Cbu = c.CBU,
                            NroCta = c.NRO_CTA,
                            FechaSolicitudCuenta = c.FEC_SOL_CTA,
                            IdBeneficiario = c.ID_BENEFICIARIO,
                            IdCuentaBanco = c.ID_CUENTA_BANCO,
                            IdMoneda = c.ID_MONEDA,
                            IdSistema = c.ID_SISTEMA,
                            IdSucursal = c.ID_SUCURSAL,
                            UsuarioBanco = c.USUARIO_BANCO,
                            IdUsuarioSistema = c.ID_USR_SIST,
                            FechaSistema = c.FEC_SIST,
                            UsuarioSistema = c.T_USUARIOS.LOGIN
                        });


        }

        public int AddCuentaBanco(ICuentaBanco cuentaBanco)
        {
            AgregarDatos(cuentaBanco);
            
            var cuentaBancoModel = new T_CUENTAS_BANCO
                                       {
                                           ID_CUENTA_BANCO = SecuenciaRepositorio.GetId(),
                                           ID_BENEFICIARIO = cuentaBanco.IdBeneficiario,
                                           ID_SISTEMA = cuentaBanco.IdSistema,
                                           NRO_CTA = cuentaBanco.NroCta == 0 ? null : cuentaBanco.NroCta,
                                           CBU = cuentaBanco.Cbu,
                                           USUARIO_BANCO = cuentaBanco.UsuarioBanco,
                                           ID_SUCURSAL = cuentaBanco.IdSucursal == 0 ? null : cuentaBanco.IdSucursal,
                                           ID_MONEDA = cuentaBanco.IdMoneda,
                                           FEC_SOL_CTA = null,
                                           ID_USR_SIST = cuentaBanco.IdUsuarioSistema,
                                           FEC_SIST = cuentaBanco.FechaSistema
                                       };

            _mdb.T_CUENTAS_BANCO.AddObject(cuentaBancoModel);
            _mdb.SaveChanges();

            return cuentaBancoModel.ID_CUENTA_BANCO;
        }

        public bool UpdateFechaSolicitud(int idbeneficiario, DateTime fecha)
        {
            IComunDatos comun = new ComunDatos();

            AgregarDatos(comun);
            
            var cuentas = _mdb.T_CUENTAS_BANCO.Where(c => c.ID_BENEFICIARIO == idbeneficiario).ToList();

            foreach (var cuentasBanco in cuentas)
            {
                cuentasBanco.FEC_SOL_CTA = fecha;
                cuentasBanco.ID_USR_SIST = comun.IdUsuarioSistema;
            }

            _mdb.SaveChanges();
            return true;
        }

        public bool ClearFechaSolicitud(int idBeneficiario)
        {
            IComunDatos comun = new ComunDatos();

            AgregarDatos(comun);
            
            _mdb.Connection.Open();

            using (var trans = _mdb.Connection.BeginTransaction())
            {
                var dbbeneficiariocuentas =
                    _mdb.T_CUENTAS_BANCO.Where(c => c.ID_BENEFICIARIO == idBeneficiario);

                foreach (var beneficiariocuentas in dbbeneficiariocuentas)
                {
                    beneficiariocuentas.FEC_SOL_CTA = null;
                    beneficiariocuentas.ID_USR_SIST = comun.IdUsuarioSistema;
                }

                _mdb.SaveChanges();
                trans.Commit();
            }

            return true;
        }

        public bool UpdateCuentaBanco(ICuentaBanco cuentaBanco)
        {
            AgregarDatos(cuentaBanco);
            
            var dbcuentas =
                _mdb.T_CUENTAS_BANCO.FirstOrDefault(c => c.ID_CUENTA_BANCO == cuentaBanco.IdCuentaBanco);

            dbcuentas.NRO_CTA = cuentaBanco.NroCta == 0 ? null : cuentaBanco.NroCta;
            dbcuentas.ID_SUCURSAL = cuentaBanco.IdSucursal == 0 ? null : cuentaBanco.IdSucursal;
            dbcuentas.ID_MONEDA = cuentaBanco.IdMoneda;
            dbcuentas.CBU = cuentaBanco.Cbu == "" ? null : cuentaBanco.Cbu;
            dbcuentas.ID_USR_SIST = cuentaBanco.IdUsuarioSistema;

            _mdb.SaveChanges();
            return true;
        }

        public IList<ICuentaBanco> GetCuentaBancoByBeneficiario(int idbeneficiario)
        {
            return QCuentaBanco().Where(c => c.IdBeneficiario == idbeneficiario).ToList();
        }
    }
}
