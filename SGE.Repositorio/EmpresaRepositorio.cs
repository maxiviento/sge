using System;
using System.Collections.Generic;
using System.Linq;
using SGE.Model.Entidades;
using SGE.Model.Entidades.Interfaces;
using SGE.Model.Repositorio;
using SGE.Repositorio.Modelo;
using SGE.Model.Comun;
using System.Configuration;
using System.Data.Objects;

namespace SGE.Repositorio
{
    public class EmpresaRepositorio : BaseRepositorio, IEmpresaRepositorio
    {
        private readonly DataSGE _mdb;

        public EmpresaRepositorio()
        {
            _mdb = new DataSGE();
        }

        private IQueryable<IEmpresa> QEmpresa()
        {
            var a = (from i in _mdb.T_EMPRESAS
                     select
                         new Empresa
                             {
                                 IdEmpresa = i.ID_EMPRESA,
                                 IdLocalidad = i.ID_LOCALIDAD,
                                 NombreEmpresa = (i.N_EMPRESA ?? "").ToUpper().Trim(),
                                 Cuit = i.CUIT,
                                 CodigoActividad = i.CODIGO_ACTIVIDAD,
                                 DomicilioLaboralIdem = i.DOMICLIO_LABORAL_IDEM,
                                 CantidadEmpleados = i.CANTIDAD_EMPLEADOS ?? 0,
                                 Calle = i.CALLE,
                                 Numero = i.NUMERO,
                                 Piso = i.PISO,
                                 Dpto = i.DPTO,
                                 CodigoPostal = i.CODIGO_POSTAL,
                                 IdUsuario = i.ID_USUARIO,
                                 NombreUsuario = i.T_USUARIOS_EMPRESA.NOMBRE.ToUpper().Trim(),
                                 ApellidoUsuario = i.T_USUARIOS_EMPRESA.APELLIDO.ToUpper().Trim(),
                                 Cuil = i.T_USUARIOS_EMPRESA.CUIL,
                                 Mail = i.T_USUARIOS_EMPRESA.MAIL,
                                 Telefono = i.T_USUARIOS_EMPRESA.TELEFONO,
                                 NombreLocalidad = i.T_LOCALIDADES.N_LOCALIDAD,
                                 IdUsuarioSistema = i.ID_USR_SIST,
                                 UsuarioSistema = i.T_USUARIOS.NOMBRE,
                                 FechaSistema = i.FEC_SIST,
                                 //PasswordUsuario =  i.T_USUARIOS.HASHED_PASS // 05/03/2013 - DI CAMPLI LEANDRO
                                 LoginUsuario = i.T_USUARIOS_EMPRESA.LOGIN,
                                 celular = i.CELULAR,
                                 mailEmp = i.MAIL,
                                 verificada = i.VERIFICADA == "S" ? i.VERIFICADA : "N",
                                 EsCooperativa = i.ES_COOPERATIVA == "S" ? i.ES_COOPERATIVA : "N",
                                 CBU = i.CBU,
                                 CUENTA = i.CUENTA,
                                 TIPOCUENTA = i.ID_TABLA_BCO_CBA,
                                 CUENTA_BANCOR = i.CTA_BANCOR ?? "N",
                                 FEC_ADHESION = i.FEC_ADHESION

                             });
            return a;
        }

        public IList<IEmpresa> GetEmpresas()
        {
            return QEmpresa().OrderBy(c => c.NombreEmpresa).ToList();
        }

        public IList<IEmpresa> GetEmpresas(int skip, int take)
        {
            return QEmpresa().OrderBy(c => c.NombreEmpresa).Skip(skip).Take(take).ToList();
        }

        public IList<IEmpresa> GetEmpresas(string descripcion, string cuit)
        {
            descripcion = descripcion ?? String.Empty;
            cuit = cuit ?? String.Empty;

            return
                QEmpresa().Where(
                    c =>
                    (c.NombreEmpresa.ToUpper().Contains(descripcion.ToUpper().Trim()) || String.IsNullOrEmpty(descripcion.Trim()))
                    && (c.Cuit.ToUpper().Contains(cuit.ToUpper().Trim()) || String.IsNullOrEmpty(cuit.Trim()))).
                    OrderBy(o => o.NombreEmpresa).ToList();
        }

        public IList<IEmpresa> GetEmpresas(string descripcion, string cuit, int skip, int take)
        {
            descripcion = descripcion ?? String.Empty;
            cuit = cuit ?? String.Empty;

            return
                QEmpresa().Where(
                    c =>
                    (c.NombreEmpresa.ToUpper().Contains(descripcion.ToUpper().Trim()) || String.IsNullOrEmpty(descripcion.Trim()))
                    && (c.Cuit.ToUpper().Contains(cuit.ToUpper()) || String.IsNullOrEmpty(cuit))).
                    OrderBy(o => o.NombreEmpresa).Skip(skip).Take(take).ToList();
        }

        public IEmpresa GetEmpresa(int id)
        {
            return QEmpresa().Where(c => c.IdEmpresa == id).SingleOrDefault();
        }

        public IEmpresa GetEmpresa(string cuit)
        {
            cuit = cuit ?? String.Empty;
            return QEmpresa().Where(c =>
                                   (c.Cuit.ToUpper().Contains(cuit.ToUpper()) || String.IsNullOrEmpty(cuit.Trim()))).
                                   Take(1).SingleOrDefault();
        }

        public int AddEmpresa(IEmpresa empresa)
        {
            AgregarDatos(empresa);
            
            var empModel = new T_EMPRESAS
            {
                ID_EMPRESA = SecuenciaRepositorio.GetId(),
                ID_LOCALIDAD = empresa.IdLocalidad,
                N_EMPRESA = empresa.NombreEmpresa.ToUpper().Trim(),
                CUIT = empresa.Cuit,
                CODIGO_ACTIVIDAD = empresa.CodigoActividad,
                DOMICLIO_LABORAL_IDEM = empresa.DomicilioLaboralIdem,
                CANTIDAD_EMPLEADOS = Convert.ToInt16(empresa.CantidadEmpleados),
                CALLE = empresa.Calle,
                NUMERO = empresa.Numero,
                PISO = empresa.Piso,
                DPTO = empresa.Dpto,
                CODIGO_POSTAL = empresa.CodigoPostal,
                ID_USR_SIST = empresa.IdUsuarioSistema,
                FEC_SIST = empresa.FechaSistema,
                CELULAR = empresa.celular,
                MAIL = empresa.mailEmp,
                VERIFICADA = empresa.verificada,
                ES_COOPERATIVA = empresa.EsCooperativa == "S" ? empresa.EsCooperativa : "N",
                CBU = empresa.CBU,
                CUENTA = empresa.CUENTA,
                ID_TABLA_BCO_CBA = (short?)empresa.TIPOCUENTA,
                CTA_BANCOR = empresa.CUENTA_BANCOR ?? "N" ,
                FEC_ADHESION = empresa.FEC_ADHESION
            };

            _mdb.T_EMPRESAS.AddObject(empModel);
            _mdb.SaveChanges();

            return empModel.ID_EMPRESA;
        }

        public void UpdateEmpresa(IEmpresa emp)
        {
            AgregarDatos(emp);
            
            var obj = _mdb.T_EMPRESAS.SingleOrDefault(c => c.ID_EMPRESA == emp.IdEmpresa);

            if (obj == null) return;

            obj.ID_LOCALIDAD = emp.IdLocalidad;
            obj.N_EMPRESA = emp.NombreEmpresa.ToUpper().Trim();
            obj.CUIT = emp.Cuit;
            obj.CODIGO_ACTIVIDAD = emp.CodigoActividad;
            obj.DOMICLIO_LABORAL_IDEM = emp.DomicilioLaboralIdem;
            obj.CANTIDAD_EMPLEADOS = Convert.ToInt16(emp.CantidadEmpleados);
            obj.CALLE = emp.Calle;
            obj.NUMERO = emp.Numero;
            obj.PISO = emp.Piso;
            obj.DPTO = emp.Dpto;
            obj.CODIGO_POSTAL = emp.CodigoPostal;
            obj.ID_USR_SIST = emp.IdUsuarioSistema;
            obj.CELULAR = emp.celular;
            obj.MAIL = emp.mailEmp;
            obj.VERIFICADA = emp.verificada;
            obj.ES_COOPERATIVA = emp.EsCooperativa == "S" ? emp.EsCooperativa : "N";
            obj.CBU = emp.CBU;
            obj.CUENTA = emp.CUENTA;
            obj.ID_TABLA_BCO_CBA = (short?)emp.TIPOCUENTA;
            obj.CTA_BANCOR = emp.CUENTA_BANCOR ?? "N";
            obj.FEC_ADHESION = emp.FEC_ADHESION;
            _mdb.SaveChanges();
        }

        public void DeleteEmpresa(IEmpresa emp)
        {
            AgregarDatos(emp);
            
            var obj = _mdb.T_EMPRESAS.SingleOrDefault(c => c.ID_EMPRESA == emp.IdEmpresa);

            if (obj == null) return;

            obj.ID_USR_SIST = emp.IdUsuarioSistema;
            _mdb.DeleteObject(obj);
            _mdb.SaveChanges();
        }

        public bool ExistsCuit(string cuit)
        {
            cuit = cuit ?? String.Empty;

            var t = QEmpresa().Where(
                   c =>
                   (c.Cuit.ToUpper().Contains(cuit.ToUpper().Trim()) || String.IsNullOrEmpty(cuit.Trim())))
                   .ToList().Count;

            return t > 0 ? true : false;
        }

        public bool EmpresaEnUso(int idEmpresa)
        {
            var t = (_mdb.T_FICHA_PPP.Where(c => c.ID_EMPRESA == idEmpresa)).Count();

            return t > 0 ? true : false;
        }

        public int GetEmpresasCount()
        {
            return QEmpresa().Count();
        }

        public int GetFichasCount(int idEmpresa)
        {
            var t = (_mdb.T_FICHA_PPP.Where(c => c.ID_EMPRESA == idEmpresa)).Count();

            return t;
        }

        public int AddUsuariosEmpresa(IEmpresa emp)
        {
            AgregarDatos(emp);
            
            var empModel = new T_USUARIOS_EMPRESA
            {
                ID_USUARIO = SecuenciaRepositorio.GetId(),
                CUIL = emp.Cuil,
                LOGIN = emp.LoginUsuario, // 18/04/2013 - DI CAMPLI LEANDRO - SE MODIFICA LA CREACION DEL LOGIN CUIT POR LoginUsuario
                HASHED_PASS = emp.PasswordUsuario,
                APELLIDO = emp.ApellidoUsuario.ToUpper().Trim(),
                NOMBRE = emp.NombreUsuario.ToUpper().Trim(),
                TELEFONO = emp.Telefono,
                MAIL = emp.Mail,
                ID_USR_SIST = emp.IdUsuarioSistema
            };

            _mdb.T_USUARIOS_EMPRESA.AddObject(empModel);

            var obj = _mdb.T_EMPRESAS.SingleOrDefault(c => c.ID_EMPRESA == emp.IdEmpresa);

            if (obj != null)
            {
                obj.ID_USUARIO = empModel.ID_USUARIO;
                _mdb.SaveChanges();
            }
            _mdb.SaveChanges();

            return empModel.ID_USUARIO;
        }

        public void UpdateUsuarioEmpresa(IEmpresa emp)
        {
            AgregarDatos(emp);
            
            var obj = _mdb.T_USUARIOS_EMPRESA.SingleOrDefault(c => c.ID_USUARIO == emp.IdUsuario);

            if (obj == null) return;

            obj.NOMBRE = emp.NombreUsuario.ToUpper().Trim();
            obj.APELLIDO = emp.ApellidoUsuario.ToUpper().Trim();
            obj.CUIL = emp.Cuil;
            obj.TELEFONO = emp.Telefono;
            obj.MAIL = emp.Mail;
            obj.ID_USR_SIST = emp.IdUsuarioSistema;

            _mdb.SaveChanges();
        }

        public bool ExistsEmpresa(string cuit, string descripcion)
        {
            descripcion = descripcion ?? String.Empty;
            cuit = cuit ?? String.Empty;

            var t = QEmpresa().Where(
                    c =>
                    (c.NombreEmpresa.ToUpper().Contains(descripcion.ToUpper()) || String.IsNullOrEmpty(descripcion)) &&
                    (c.Cuit.ToUpper().Contains(cuit.ToUpper().Trim()) || String.IsNullOrEmpty(cuit.Trim())))
                    .ToList().Count;

            return t > 0 ? true : false;
        }

        public bool UpdateFichaPpp(int origen, int destino)
        {
            IComunDatos comun = new ComunDatos();

            AgregarDatos(comun);

            var fichaPpp = _mdb.T_FICHA_PPP.Where(f => f.ID_EMPRESA == origen);

            foreach (var f in fichaPpp)
            {
                f.ID_EMPRESA = destino;
                f.ID_USR_SIST = comun.IdUsuarioSistema;
                _mdb.SaveChanges();
            }
            return true;
        }

        public bool EsNuevaEmpresa(int? idEmpresa)
        {
            var t = QEmpresa().Where(c => c.IdEmpresa == idEmpresa).ToList().Count;

            return t > 0 ? true : false;
        }

        public int GetBenefActivosEmpCount(int idEmpresa, int intPrograma)
        {
            var t = 0;
            int ppp = 0, pppp = 0, vat = 0, reco_prof = 0, confvos = 0;

            switch (intPrograma)
            {
                case (int)Enums.TipoFicha.Terciaria:

                    break;
                case (int)Enums.TipoFicha.Universitaria:

                    break;
                case (int)Enums.TipoFicha.Ppp:
                t = (from fic in _mdb.T_FICHA_PPP
                        from b in _mdb.T_BENEFICIARIOS.Where(filter => filter.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                     from f in _mdb.T_FICHAS.Where(filter => filter.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                     where fic.ID_EMPRESA == idEmpresa && b.ID_ESTADO == 2 && f.ID_ESTADO_FICHA == 3
                     select fic.ID_FICHA).Count();
            
                    break;
                case (int)Enums.TipoFicha.PppProf:
                    t = (from fic in _mdb.T_FICHA_PPP_PROF
                         from b in _mdb.T_BENEFICIARIOS.Where(filter => filter.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                         from f in _mdb.T_FICHAS.Where(filter => filter.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                         where fic.ID_EMPRESA == idEmpresa && b.ID_ESTADO == 2 && f.ID_ESTADO_FICHA == 3
                         select fic.ID_FICHA).Count();

                    break;
                case (int)Enums.TipoFicha.Vat:
                    t = (from fic in _mdb.T_FICHA_VAT
                         from b in _mdb.T_BENEFICIARIOS.Where(filter => filter.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                         from f in _mdb.T_FICHAS.Where(filter => filter.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                         where fic.ID_EMPRESA == idEmpresa && b.ID_ESTADO == 2 && f.ID_ESTADO_FICHA == 3
                         select fic.ID_FICHA).Count();

                    break;
                case (int)Enums.TipoFicha.ReconversionProductiva:
                    t = (from fic in _mdb.T_FICHA_REC_PROD
                         from b in _mdb.T_BENEFICIARIOS.Where(filter => filter.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                         from f in _mdb.T_FICHAS.Where(filter => filter.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                         where fic.ID_EMPRESA == idEmpresa && b.ID_ESTADO == 2 && f.ID_ESTADO_FICHA == 3
                         select fic.ID_FICHA).Count();

                    break;

                //case (int)Enums.TipoFicha.ConfiamosEnVos:
                //    t = (from fic in _mdb.T_FICHAS_CONF_VOS
                //               from b in _mdb.T_BENEFICIARIOS.Where(filter => filter.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                //               from f in _mdb.T_FICHAS.Where(filter => filter.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                //               where fic.ID_EMPRESA == idEmpresa && b.ID_ESTADO == 2 && f.ID_ESTADO_FICHA == 3
                //               select fic.ID_FICHA).Count();
                //    break;

                case 0: //TODOS LOS PROGRAMAS
                    ppp = (from fic in _mdb.T_FICHA_PPP
                         from b in _mdb.T_BENEFICIARIOS.Where(filter => filter.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                         from f in _mdb.T_FICHAS.Where(filter => filter.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                         where fic.ID_EMPRESA == idEmpresa && b.ID_ESTADO == 2 && f.ID_ESTADO_FICHA == 3
                         select fic.ID_FICHA).Count();

                    pppp = (from fic in _mdb.T_FICHA_PPP_PROF
                         from b in _mdb.T_BENEFICIARIOS.Where(filter => filter.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                         from f in _mdb.T_FICHAS.Where(filter => filter.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                         where fic.ID_EMPRESA == idEmpresa && b.ID_ESTADO == 2 && f.ID_ESTADO_FICHA == 3
                         select fic.ID_FICHA).Count();

                    vat = (from fic in _mdb.T_FICHA_VAT
                         from b in _mdb.T_BENEFICIARIOS.Where(filter => filter.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                         from f in _mdb.T_FICHAS.Where(filter => filter.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                         where fic.ID_EMPRESA == idEmpresa && b.ID_ESTADO == 2 && f.ID_ESTADO_FICHA == 3
                         select fic.ID_FICHA).Count();

                    reco_prof = (from fic in _mdb.T_FICHA_REC_PROD
                         from b in _mdb.T_BENEFICIARIOS.Where(filter => filter.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                         from f in _mdb.T_FICHAS.Where(filter => filter.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                         where fic.ID_EMPRESA == idEmpresa && b.ID_ESTADO == 2 && f.ID_ESTADO_FICHA == 3
                         select fic.ID_FICHA).Count();

                    confvos = (from fic in _mdb.T_FICHAS_CONF_VOS
                         from b in _mdb.T_BENEFICIARIOS.Where(filter => filter.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                         from f in _mdb.T_FICHAS.Where(filter => filter.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                         where fic.ID_EMPRESA == idEmpresa && b.ID_ESTADO == 2 && f.ID_ESTADO_FICHA == 3
                         select fic.ID_FICHA).Count();

                    t = ppp + pppp + vat + reco_prof + confvos; 

                    break;
             }
            return t;
        }

        public int GetBenefRetenidosEmpCount(int idEmpresa, int intPrograma)
        {
            var t = 0;
            int ppp = 0, pppp = 0, vat = 0, reco_prof = 0, confvos = 0;

            switch (intPrograma)
            {
                case (int)Enums.TipoFicha.Terciaria:

                    break;
                case (int)Enums.TipoFicha.Universitaria:

                    break;
                case (int)Enums.TipoFicha.Ppp:
                    t = (from fic in _mdb.T_FICHA_PPP
                         from b in _mdb.T_BENEFICIARIOS.Where(filter => filter.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                         from f in _mdb.T_FICHAS.Where(filter => filter.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                         where fic.ID_EMPRESA == idEmpresa && b.ID_ESTADO == 6 && f.ID_ESTADO_FICHA == 3
                         select fic.ID_FICHA).Count();

                    break;
                case (int)Enums.TipoFicha.PppProf:
                    t = (from fic in _mdb.T_FICHA_PPP_PROF
                         from b in _mdb.T_BENEFICIARIOS.Where(filter => filter.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                         from f in _mdb.T_FICHAS.Where(filter => filter.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                         where fic.ID_EMPRESA == idEmpresa && b.ID_ESTADO == 6 && f.ID_ESTADO_FICHA == 3
                         select fic.ID_FICHA).Count();

                    break;
                case (int)Enums.TipoFicha.Vat:
                    t = (from fic in _mdb.T_FICHA_VAT
                         from b in _mdb.T_BENEFICIARIOS.Where(filter => filter.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                         from f in _mdb.T_FICHAS.Where(filter => filter.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                         where fic.ID_EMPRESA == idEmpresa && b.ID_ESTADO == 6 && f.ID_ESTADO_FICHA == 3
                         select fic.ID_FICHA).Count();

                    break;
                case (int)Enums.TipoFicha.ReconversionProductiva:
                    t = (from fic in _mdb.T_FICHA_REC_PROD
                         from b in _mdb.T_BENEFICIARIOS.Where(filter => filter.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                         from f in _mdb.T_FICHAS.Where(filter => filter.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                         where fic.ID_EMPRESA == idEmpresa && b.ID_ESTADO == 6 && f.ID_ESTADO_FICHA == 3
                         select fic.ID_FICHA).Count();

                    break;

                //case (int)Enums.TipoFicha.ConfiamosEnVos:
                //    t = (from fic in _mdb.T_FICHAS_CONF_VOS
                //               from b in _mdb.T_BENEFICIARIOS.Where(filter => filter.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                //               from f in _mdb.T_FICHAS.Where(filter => filter.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                //               where fic.ID_EMPRESA == idEmpresa && b.ID_ESTADO == 2 && f.ID_ESTADO_FICHA == 3
                //               select fic.ID_FICHA).Count();
                //    break;

                case 0: //TODOS LOS PROGRAMAS
                    ppp = (from fic in _mdb.T_FICHA_PPP
                           from b in _mdb.T_BENEFICIARIOS.Where(filter => filter.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                           from f in _mdb.T_FICHAS.Where(filter => filter.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                           where fic.ID_EMPRESA == idEmpresa && b.ID_ESTADO == 6 && f.ID_ESTADO_FICHA == 3
                           select fic.ID_FICHA).Count();

                    pppp = (from fic in _mdb.T_FICHA_PPP_PROF
                            from b in _mdb.T_BENEFICIARIOS.Where(filter => filter.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                            from f in _mdb.T_FICHAS.Where(filter => filter.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                            where fic.ID_EMPRESA == idEmpresa && b.ID_ESTADO == 6 && f.ID_ESTADO_FICHA == 3
                            select fic.ID_FICHA).Count();

                    vat = (from fic in _mdb.T_FICHA_VAT
                           from b in _mdb.T_BENEFICIARIOS.Where(filter => filter.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                           from f in _mdb.T_FICHAS.Where(filter => filter.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                           where fic.ID_EMPRESA == idEmpresa && b.ID_ESTADO == 6 && f.ID_ESTADO_FICHA == 3
                           select fic.ID_FICHA).Count();

                    reco_prof = (from fic in _mdb.T_FICHA_REC_PROD
                                 from b in _mdb.T_BENEFICIARIOS.Where(filter => filter.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                                 from f in _mdb.T_FICHAS.Where(filter => filter.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                                 where fic.ID_EMPRESA == idEmpresa && b.ID_ESTADO == 6 && f.ID_ESTADO_FICHA == 3
                                 select fic.ID_FICHA).Count();

                    confvos = (from fic in _mdb.T_FICHAS_CONF_VOS
                               from b in _mdb.T_BENEFICIARIOS.Where(filter => filter.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                               from f in _mdb.T_FICHAS.Where(filter => filter.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                               where fic.ID_EMPRESA == idEmpresa && b.ID_ESTADO == 6 && f.ID_ESTADO_FICHA == 3
                               select fic.ID_FICHA).Count();

                    t = ppp + pppp + vat + reco_prof + confvos;

                    break;
            }
            return t;
        }
        /// <summary>
        /// PERMITE BLANQUEAR EL PASS DE LA CUENTA DE UNA EMPRESA
        /// </summary>
        /// <param name="emp"></param>
        public void BlanquearCuentaUsr(IEmpresa emp)
        {
            // 26/03/2013 - DI CAMPLI LEANDRO
            AgregarDatos(emp);
            
            var obj = _mdb.T_USUARIOS_EMPRESA.SingleOrDefault(c => c.ID_USUARIO == emp.IdUsuario);

            if (obj == null) return;

            obj.HASHED_PASS = emp.PasswordUsuario;
            obj.ID_USR_SIST = emp.IdUsuarioSistema;
            obj.FEC_SIST = emp.FechaSistema;

            _mdb.SaveChanges();


        }

        public int GetBenefActDiscEmpCount(int idEmpresa, int intPrograma)
        {
            var t = 0;
            int ppp = 0, pppp = 0, vat = 0, reco_prof = 0, confvos = 0;

            switch (intPrograma)
            {
                case (int)Enums.TipoFicha.Terciaria:

                    break;
                case (int)Enums.TipoFicha.Universitaria:

                    break;
                case (int)Enums.TipoFicha.Ppp:
                    t = (from fic in _mdb.T_FICHA_PPP
                         from b in _mdb.T_BENEFICIARIOS.Where(filter => filter.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                         from f in _mdb.T_FICHAS.Where(filter => filter.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                         where fic.ID_EMPRESA == idEmpresa && b.ID_ESTADO == 2 && f.ID_ESTADO_FICHA == 3 && f.ES_DISCAPACITADO == "S"
                         select fic.ID_FICHA).Count();

                    break;
                case (int)Enums.TipoFicha.PppProf:
                    t = (from fic in _mdb.T_FICHA_PPP_PROF
                         from b in _mdb.T_BENEFICIARIOS.Where(filter => filter.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                         from f in _mdb.T_FICHAS.Where(filter => filter.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                         where fic.ID_EMPRESA == idEmpresa && b.ID_ESTADO == 2 && f.ID_ESTADO_FICHA == 3 && f.ES_DISCAPACITADO == "S"
                         select fic.ID_FICHA).Count();

                    break;
                case (int)Enums.TipoFicha.Vat:
                    t = (from fic in _mdb.T_FICHA_VAT
                         from b in _mdb.T_BENEFICIARIOS.Where(filter => filter.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                         from f in _mdb.T_FICHAS.Where(filter => filter.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                         where fic.ID_EMPRESA == idEmpresa && b.ID_ESTADO == 2 && f.ID_ESTADO_FICHA == 3 && f.ES_DISCAPACITADO == "S"
                         select fic.ID_FICHA).Count();

                    break;
                case (int)Enums.TipoFicha.ReconversionProductiva:
                    t = (from fic in _mdb.T_FICHA_REC_PROD
                         from b in _mdb.T_BENEFICIARIOS.Where(filter => filter.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                         from f in _mdb.T_FICHAS.Where(filter => filter.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                         where fic.ID_EMPRESA == idEmpresa && b.ID_ESTADO == 2 && f.ID_ESTADO_FICHA == 3 && f.ES_DISCAPACITADO == "S"
                         select fic.ID_FICHA).Count();

                    break;

                case (int)Enums.TipoFicha.ConfiamosEnVos:
                    t = (from fic in _mdb.T_FICHAS_CONF_VOS
                         from b in _mdb.T_BENEFICIARIOS.Where(filter => filter.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                         from f in _mdb.T_FICHAS.Where(filter => filter.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                         where fic.ID_EMPRESA == idEmpresa && b.ID_ESTADO == 2 && f.ID_ESTADO_FICHA == 3 && f.ES_DISCAPACITADO == "S"
                         select fic.ID_FICHA).Count();
                    break;

                case 0: //TODOS LOS PROGRAMAS
                    ppp = (from fic in _mdb.T_FICHA_PPP
                           from b in _mdb.T_BENEFICIARIOS.Where(filter => filter.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                           from f in _mdb.T_FICHAS.Where(filter => filter.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                           where fic.ID_EMPRESA == idEmpresa && b.ID_ESTADO == 2 && f.ID_ESTADO_FICHA == 3 && f.ES_DISCAPACITADO == "S"
                           select fic.ID_FICHA).Count();

                    pppp = (from fic in _mdb.T_FICHA_PPP_PROF
                            from b in _mdb.T_BENEFICIARIOS.Where(filter => filter.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                            from f in _mdb.T_FICHAS.Where(filter => filter.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                            where fic.ID_EMPRESA == idEmpresa && b.ID_ESTADO == 2 && f.ID_ESTADO_FICHA == 3 && f.ES_DISCAPACITADO == "S"
                            select fic.ID_FICHA).Count();

                    vat = (from fic in _mdb.T_FICHA_VAT
                           from b in _mdb.T_BENEFICIARIOS.Where(filter => filter.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                           from f in _mdb.T_FICHAS.Where(filter => filter.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                           where fic.ID_EMPRESA == idEmpresa && b.ID_ESTADO == 2 && f.ID_ESTADO_FICHA == 3 && f.ES_DISCAPACITADO == "S"
                           select fic.ID_FICHA).Count();

                    reco_prof = (from fic in _mdb.T_FICHA_REC_PROD
                                 from b in _mdb.T_BENEFICIARIOS.Where(filter => filter.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                                 from f in _mdb.T_FICHAS.Where(filter => filter.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                                 where fic.ID_EMPRESA == idEmpresa && b.ID_ESTADO == 2 && f.ID_ESTADO_FICHA == 3 && f.ES_DISCAPACITADO == "S"
                                 select fic.ID_FICHA).Count();

                    confvos = (from fic in _mdb.T_FICHAS_CONF_VOS
                               from b in _mdb.T_BENEFICIARIOS.Where(filter => filter.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                               from f in _mdb.T_FICHAS.Where(filter => filter.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                               where fic.ID_EMPRESA == idEmpresa && b.ID_ESTADO == 2 && f.ID_ESTADO_FICHA == 3 && f.ES_DISCAPACITADO == "S"
                               select fic.ID_FICHA).Count();

                    t = ppp + pppp + vat + reco_prof + confvos;

                    break;
            }
            return t;
        }

        public IList<IRecupero> udpCuponRecupero(IList<IRecupero> lrecupero)
        {
            //AgregarDatos(emp);
            foreach(var recupero in lrecupero)
            {
                try
                {
                    var obj = _mdb.T_CUPONES.SingleOrDefault(c => c.ID_CUPON == recupero.ID_CUPON);
                    recupero.CONVENIO = obj.CONVENIO;
                    recupero.GRPCONVENIO = obj.GRPCONVENIO;
                    recupero.IMPORTE = obj.IMPORTE ?? 0;
                    recupero.ID_EMPRESA = obj.ID_EMPRESA;
                    recupero.N_REG_CUPON = obj.CODIGOBARRA;
                    recupero.ESTADO = "REGISTRADO";
                    //if (obj == null) return lrecupero
                    obj.FEC_COBRO = recupero.FEC_COBRO;
                    obj.FEC_PROCESO = recupero.FEC_PROCESO;
                    obj.FEC_RENDICION = recupero.FEC_RENDICION;
                    obj.FEC_MODIF = DateTime.Now;
                    obj.N_ARCHIVO = recupero.NombreArchivo;
                    obj.PAGADO = "S";
                    obj.N_SUC_COBRO = recupero.SUC_COBRO;
                    _mdb.SaveChanges();
                }
                catch (Exception)
                {

                    recupero.ESTADO = "Error al procesar";
                }
                

            }

            return lrecupero;

        }

        public IList<IRecupero> getRecupero(string nombreArchivo)
        {

                try
                {

                    IQueryable<IRecupero> cupones = (from cup in _mdb.T_CUPONES
                                   from emp in _mdb.T_EMPRESAS.Where(c => c.ID_EMPRESA == cup.ID_EMPRESA).DefaultIfEmpty()
                                   where cup.N_ARCHIVO == nombreArchivo
                                   select new Recupero
                                   {
                                       ID_CUPON = cup.ID_CUPON,
                                       CONVENIO = cup.CONVENIO ?? "",
                                       GRPCONVENIO = cup.GRPCONVENIO ?? "",
                                       IMPORTE = cup.IMPORTE ?? 0,
                                       ID_EMPRESA = cup.ID_EMPRESA,
                                       N_EMPRESA = emp.N_EMPRESA,
                                       N_REG_CUPON = cup.CODIGOBARRA ?? "",
                                       ESTADO = cup.PAGADO == "S" ? "COBRADO" : "NO COBRADO",
                                       FEC_COBRO = cup.FEC_COBRO,
                                       FEC_PROCESO = cup.FEC_PROCESO,
                                       FEC_RENDICION = cup.FEC_RENDICION,
                                       FEC_MODIF = cup.FEC_MODIF,
                                       NombreArchivo = cup.N_ARCHIVO ?? "",
                                       SUC_COBRO = cup.N_SUC_COBRO ?? "",
                                       PERIODO = cup.PERIODO ?? ""
                                   });
                    return cupones.ToList();
                }
                catch (Exception)
                {
                    return null;
                    
                }



        }

        private IQueryable<IRecuperoDebito> QRecuperosDebito(int idrecupero, int idconcepto)
        {
            if (_mdb.Connection.State != System.Data.ConnectionState.Open)
            {
                //_mdb.Connection.Open();
            }

            var recuperos = (from rec in _mdb.T_RECUPEROS_DEBITO
                          join con in _mdb.T_CONCEPTOS on rec.ID_CONCEPTO equals con.ID_CONCEPTO
                          where (rec.ID_RECUPERO_DEB == idrecupero || idrecupero == 0)
                          && (rec.ID_CONCEPTO == idconcepto || idconcepto == 0)
                         select new RecuperoDebito
                          {

                              ID_RECUPERO_DEB = rec.ID_RECUPERO_DEB,
                              ID_CONCEPTO = rec.ID_CONCEPTO,
                              //N_CONCEPTO = Convert.ToString(con.MES) + "/" + Convert.ToString(con.ANIO),
                              N_ARCHIVO = rec.N_ARCHIVO,
                              PROCESADO = rec.PROCESADO,
                              FEC_PROCESO = rec.FEC_PROCESO
                              
                          }

                              );
            //var objectQuery = recuperos as ObjectQuery;
            //string consultaSql = objectQuery.ToTraceString();
            return recuperos;

        }

        private IQueryable<IRecuperoDebito> QLiqEmpresaDebito(int idconcepto)
        {
            // 25/03/2020
            //Obtener los datos del concepto
            var concepto = _mdb.T_CONCEPTOS.Where(x => x.ID_CONCEPTO == idconcepto).Single();
            DateTime fechaConcepto = Convert.ToDateTime("01/" + Convert.ToString(concepto.MES).PadLeft(1, '0') + "/" + concepto.ANIO.ToString());

            var empliq = (from bliq in _mdb.T_BENEFICIARIO_CONCEP_LIQ.Where(bliq => bliq.ID_CONCEPTO == idconcepto /*&& bliq.ID_EST_BEN_CON_LIQ == (int)Enums.EstadoBeneficiarioLiquidacion.ImputadoEnCuenta*/).DefaultIfEmpty()
                          join emp in _mdb.T_EMPRESAS on bliq.ID_EMPRESA equals emp.ID_EMPRESA
                          join liq in _mdb.T_LIQUIDACIONES on bliq.ID_LIQUIDACION equals liq.ID_LIQUIDACION
                          from be in _mdb.T_BENEFICIARIOS.Where(be => be.ID_BENEFICIARIO == bliq.ID_BENEFICIARIO).DefaultIfEmpty()
                          from fic in _mdb.T_FICHAS.Where(fic => fic.ID_FICHA == be.ID_FICHA).DefaultIfEmpty()
                          from fppp in _mdb.T_FICHA_PPP.Where(fppp => fppp.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                          from cof in _mdb.T_COFINACIAMIENTOS.Where(cof => cof.ID_TIPO_PPP == (fppp.TIPO_PPP ?? 1) && cof.CANT_EMP_MIN <= (emp.CANTIDAD_EMPLEADOS ?? 0) && cof.CANT_EMP_MAX > (emp.CANTIDAD_EMPLEADOS ?? 0)  ).DefaultIfEmpty()
                          where fppp.MODALIDAD == (int)Enums.Modalidad.Entrenamiento
                               && liq.ID_ESTADO_LIQ != (short)Enums.EstadoLiquidacion.Cancelado //ver esto, deben estar como liquidado
                          //Solo tener en cuenta las empresas con CBU presentado y que no sean cooperativas
                          && !string.IsNullOrEmpty(emp.CBU) && (emp.ES_COOPERATIVA == "N" || emp.ES_COOPERATIVA == null)
                          && emp.FEC_ADHESION <= fechaConcepto // 25/03/2020  
                          && (liq.ID_ESTADO_LIQ == (short)Enums.EstadoLiquidacion.Pendiente || (liq.ID_ESTADO_LIQ == (short)Enums.EstadoLiquidacion.Liquidado && bliq.ID_EST_BEN_CON_LIQ == (int)Enums.EstadoBeneficiarioLiquidacion.ImputadoEnCuenta))
                          select new RecuperoDebito 
                          {
                              
                              ID_BENEFICIARIO_CONCEP_LIQ = bliq.ID_BENEFICIARIO_CONCEP_LIQ,
                              ID_LIQUIDACION = bliq.ID_LIQUIDACION,
                              ID_EMPRESA = emp.ID_EMPRESA,
                              N_EMPRESA = emp.N_EMPRESA,
                              ID_CONCEPTO = bliq.ID_CONCEPTO,
                              TIPO_PPP = fppp.TIPO_PPP ?? 1,
                              COOPERATIVA = emp.ES_COOPERATIVA ?? "N",
                              EMPLEADOS = emp.CANTIDAD_EMPLEADOS,
                              CBU = emp.CBU,
                              CUENTA = emp.CUENTA,
                              CUENTA_BANCOR = emp.CTA_BANCOR ?? "N",
                              CUIT = emp.CUIT,
                              MONTO = cof.MONTO ?? 0,
                              TIPO_CUENTA = emp.ID_TABLA_BCO_CBA ?? 145,
                              CO_FINANCIAMIENTO = fppp.CO_FINANCIAMIENTO ?? 1
                              

                          }

                              );

            var objectQuery = empliq as ObjectQuery;
            string consultaSql = objectQuery.ToTraceString();      
            
            return empliq;
        
        }

        private IQueryable<IRecuperoDebito> QRecuperosDebGenerados(int idconcepto)
        {
            var recuperos = (from rec in _mdb.T_RECUPEROS_DEBITO.Where(rec=>rec.ID_CONCEPTO == idconcepto)
                             join emp in _mdb.T_EMPRESAS_RECUPERO on rec.ID_RECUPERO_DEB equals emp.ID_RECUPERO_DEB
                             join det in _mdb.T_DETALLES_RECUPERO_DEB on emp.ID_EMPRESA_DEB equals det.ID_EMPRESA_DEB
                             where (rec.PROCESADO == "S" && emp.DEVOLUCION == "COB") || (rec.PROCESADO == "N")
                             //&& (rec.ID_CONCEPTO == idconcepto || idconcepto == 0)
                          select new RecuperoDebito
                          {

                              ID_BENEFICIARIO_CONCEP_LIQ = det.ID_BENEFICIARIO_CONCEP_LIQ,
                              ID_EMPRESA = emp.ID_EMPRESA,
                              ID_CONCEPTO = rec.ID_CONCEPTO
                              
                          }

                              );

            var objectQuery = recuperos as ObjectQuery;
            string consultaSql = objectQuery.ToTraceString();

            return recuperos;

        }

        private IQueryable<IRecuperoDebEmpresa> QRecuperosEmpresa(int idrecupero)
        {
            var recemp = (from rec in _mdb.T_EMPRESAS_RECUPERO                            
                             where rec.ID_RECUPERO_DEB == idrecupero
                             select new RecuperoDebEmpresa
                             {
                                 ID_EMPRESA_DEB = rec.ID_EMPRESA_DEB,
                                 ID_RECUPERO_DEB = rec.ID_RECUPERO_DEB,
                                 MONTO = rec.MONTO,
                                 N_REGISTRO = rec.N_REGISTRO,
                                 PROCESADO = rec.PROCESADO,
                                 DEVOLUCION = rec.DEVOLUCION,
                                 FEC_SIST = rec.FEC_SIST

                             }

                              );
            //var objectQuery = recuperos as ObjectQuery;
            //string consultaSql = objectQuery.ToTraceString();
            return recemp;

        }

        public IList<IRecuperoDebito> getRecuperosDebito(int idrecupero, int idconcepto)
        {
            return QRecuperosDebito(idrecupero, idconcepto).ToList();
        }

        public IList<IRecuperoDebito> getLiqEmpresaDebito(int idconcepto)
        {
            //TRAER TODOS LOS QUE CUMPLEN CON LAS CONDICIONES
            var liqempdeb = QLiqEmpresaDebito(idconcepto).ToList();
            //TRAER TODOS LOS RECUPEROS ANTERIORES NO PROCESADOS Y PROCESADOS CON EXITO
            var nogenerar = QRecuperosDebGenerados(idconcepto).ToList();
            IList<IRecuperoDebito> arecuperar = new List<IRecuperoDebito>();
            string quitar = "N";
            foreach(var l in liqempdeb)
            {
                quitar = "N";
                foreach (var n in nogenerar)
                {
                    if (l.ID_BENEFICIARIO_CONCEP_LIQ == n.ID_BENEFICIARIO_CONCEP_LIQ)
                    {
                        quitar = "S";
                    }
                }
                if (quitar == "N")
                {
                    arecuperar.Add(l);
                }
                
            }
            return arecuperar;
        }

        public int AddRecupero(IList<RecuperoDebEmpresa> recupero, int idConcepto, DateTime fechaproceso, string nombreArchivo)
        {
            
            try
            {

                if (_mdb.Connection.State != System.Data.ConnectionState.Open)
                {
                    _mdb.Connection.Open();
                }

                int idRecupero = 0;
                using (var trans = _mdb.Connection.BeginTransaction())
                {
                    try
                    {
                        var vRecupero = new T_RECUPEROS_DEBITO
                        {
                            ID_RECUPERO_DEB = GetIdSEQCUSTOM(),
                            ID_CONCEPTO = idConcepto,
                            PROCESADO = "N",
                            N_ARCHIVO = nombreArchivo,
                            FEC_PROCESO = fechaproceso,
                            ID_USR_SIST = GetUsuarioLoguer().Id_Usuario,
                            FEC_SIST = DateTime.Now
                        };
                        _mdb.T_RECUPEROS_DEBITO.AddObject(vRecupero);
                        idRecupero = vRecupero.ID_RECUPERO_DEB;
                        //REGISTRAR EL DETALLE DEL RECUPERO CON LOS DATOS DE LAS EMPRESAS
                        foreach (var item in recupero)
                        {
                            // 11/11/2020 - NO REGISTRAR EMPRESAS CON MONTO 0
                            if(item.MONTO > 0 )
                            {
                                var empresa = new T_EMPRESAS_RECUPERO
                                {
                                    ID_EMPRESA_DEB = GetIdSEQCUSTOM(),
                                    ID_RECUPERO_DEB = vRecupero.ID_RECUPERO_DEB,
                                    ID_EMPRESA = (int)item.ID_EMPRESA,
                                    MONTO = item.MONTO,
                                    N_REGISTRO = item.N_REGISTRO,
                                    PROCESADO = "N",
                                    ID_USR_SIST = GetUsuarioLoguer().Id_Usuario,
                                    FEC_SIST = DateTime.Now
                                };
                                _mdb.T_EMPRESAS_RECUPERO.AddObject(empresa);

                                //REGISTRAR LOS DETALLES POR EMPRESA DEL RECUPERO
                                foreach (var fliq in item.detalle)
                                {
                                    // 11/11/2020 - NO REGISTRAR LIQUIDACIONES DE BENEFICIARIO CON MONTO 0
                                    if (fliq.MONTO_PARCIAL > 0)
                                    {
                                        var itemdetalle = new T_DETALLES_RECUPERO_DEB
                                        {
                                            ID_DETALLE_REC_DEB = GetIdSEQCUSTOM(),
                                            ID_EMPRESA_DEB = empresa.ID_EMPRESA_DEB,
                                            ID_BENEFICIARIO_CONCEP_LIQ = (int)fliq.ID_BENEFICIARIO_CONCEP_LIQ,
                                            MONTO_PARCIAL = fliq.MONTO_PARCIAL,
                                            ID_USR_SIST = GetUsuarioLoguer().Id_Usuario,
                                            FEC_SIST = DateTime.Now
                                        };
                                        _mdb.T_DETALLES_RECUPERO_DEB.AddObject(itemdetalle);
                                    }
                                }

                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        if (_mdb.Connection.State == System.Data.ConnectionState.Open)
                        {
                            _mdb.Connection.Close();
                            //_mdb.Connection.Dispose();
                        }
                        //return -1;
                        //throw new Exception(ex.InnerException.ToString());
                        throw ex;
                    }
                    _mdb.SaveChanges();
                    trans.Commit();

                }
                if (_mdb.Connection.State == System.Data.ConnectionState.Open)
                {
                    _mdb.Connection.Close();
                    //_mdb.Connection.Dispose();
                }
                return idRecupero;
            }
            catch (Exception ex)
            {
                if (_mdb.Connection.State == System.Data.ConnectionState.Open)
                {
                    _mdb.Connection.Close();
                    //_mdb.Connection.Dispose();
                }
                //return -1;
                //throw new Exception(ex.InnerException.ToString());
                throw ex;
            }

        }


        public int AddRecuperoBatch(IList<RecuperoDebEmpresa> recupero, int idConcepto, DateTime fechaproceso, string nombreArchivo)
        {

            try
            {
               
                int idRecupero = 0;
                idRecupero =  GetIdSEQCUSTOM();

                        var vRecupero = new T_RECUPEROS_DEBITO
                        {
                            ID_RECUPERO_DEB = idRecupero,
                            ID_CONCEPTO = idConcepto,
                            PROCESADO = "N",
                            N_ARCHIVO = nombreArchivo,
                            FEC_PROCESO = fechaproceso,
                            ID_USR_SIST = GetUsuarioLoguer().Id_Usuario,
                            FEC_SIST = DateTime.Now
                        };
                        _mdb.T_RECUPEROS_DEBITO.AddObject(vRecupero);
                        _mdb.SaveChanges();
                        //idRecupero = vRecupero.ID_RECUPERO_DEB;
                        //REGISTRAR EL DETALLE DEL RECUPERO CON LOS DATOS DE LAS EMPRESAS
                        foreach (var item in recupero)
                        {
                                                        // 11/11/2020 - NO REGISTRAR EMPRESAS CON MONTO 0
                            if (item.MONTO > 0)
                            {
                                var empresa = new T_EMPRESAS_RECUPERO
                                {
                                    ID_EMPRESA_DEB = GetIdSEQCUSTOM(),
                                    ID_RECUPERO_DEB = idRecupero,//vRecupero.ID_RECUPERO_DEB,
                                    ID_EMPRESA = (int)item.ID_EMPRESA,
                                    MONTO = item.MONTO,
                                    N_REGISTRO = item.N_REGISTRO,
                                    PROCESADO = "N",
                                    ID_USR_SIST = GetUsuarioLoguer().Id_Usuario,
                                    FEC_SIST = DateTime.Now
                                };
                                _mdb.T_EMPRESAS_RECUPERO.AddObject(empresa);
                                _mdb.SaveChanges();

                                //REGISTRAR LOS DETALLES POR EMPRESA DEL RECUPERO
                                foreach (var fliq in item.detalle)
                                {
                                    // 11/11/2020 - NO REGISTRAR LIQUIDACIONES DE BENEFICIARIO CON MONTO 0
                                    if (fliq.MONTO_PARCIAL > 0)
                                    {
                                        var itemdetalle = new T_DETALLES_RECUPERO_DEB
                                        {
                                            ID_DETALLE_REC_DEB = GetIdSEQCUSTOM(),
                                            ID_EMPRESA_DEB = empresa.ID_EMPRESA_DEB,
                                            ID_BENEFICIARIO_CONCEP_LIQ = (int)fliq.ID_BENEFICIARIO_CONCEP_LIQ,
                                            MONTO_PARCIAL = fliq.MONTO_PARCIAL,
                                            ID_USR_SIST = GetUsuarioLoguer().Id_Usuario,
                                            FEC_SIST = DateTime.Now
                                        };
                                        _mdb.T_DETALLES_RECUPERO_DEB.AddObject(itemdetalle);
                                        _mdb.SaveChanges();
                                    }

                                }
                            }

                        }
                    


                return idRecupero;
            }
            catch (Exception ex)
            {

                //return -1;
                //throw new Exception(ex.InnerException.ToString());
                throw ex;
            }

        }

        public IList<IRecuperoDebEmpresa> getRecuperosEmpresa(int idrecupero)
        {
            return QRecuperosEmpresa(idrecupero).ToList();
        
        }

        private IQueryable<IAporteDebito> QAporteDebito()
        {
            var aportes = (from cof in _mdb.T_COFINACIAMIENTOS
                           
                           select new AporteDebito 
                           {
                               ID_COFINANCIMIENTO=cof.ID_COFINANCIMIENTO,
                               CANT_EMPLEADOS = cof.CANT_EMP_MAX,
                               MONTO=cof.MONTO 
                           });

            return aportes;
        }

        public IList<IAporteDebito> getAporteDebito()
        {
            return QAporteDebito().ToList();
        }

        public int udpRecuperoDebito(IList<IRecuperoDebEmpresa> lrecupero, int idRecupero)
        {
            _mdb.Connection.Open();
            try
            {
                
                using (var trans = _mdb.Connection.BeginTransaction())
                {
                    try
                    {
                        var vRecupero = _mdb.T_RECUPEROS_DEBITO.SingleOrDefault(c => c.ID_RECUPERO_DEB == idRecupero);

                            vRecupero.PROCESADO = "S";
                            vRecupero.ID_USR_MODIF = GetUsuarioLoguer().Id_Usuario;
                            vRecupero.FEC_MODIF = DateTime.Now;

                        idRecupero = vRecupero.ID_RECUPERO_DEB;
                        //ACTUALIZAR EL ESTADO DEL RECUPERO DE LAS EMPRESAS
                        foreach (var recupero in lrecupero)
                        {

                                var obj = _mdb.T_EMPRESAS_RECUPERO.SingleOrDefault(c => c.ID_EMPRESA_DEB == recupero.ID_EMPRESA_DEB);
                                obj.PROCESADO = "S";
                                obj.DEVOLUCION = recupero.DEVOLUCION;
                                obj.ID_USR_MODIF = GetUsuarioLoguer().Id_Usuario;
                                obj.FEC_MODIF = DateTime.Now;

                        }
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        _mdb.Connection.Close();
                        return -1;
                        // throw new Exception(ex.Message);
                    }
                    _mdb.SaveChanges();
                    trans.Commit();

                }
                _mdb.Connection.Close();
                return idRecupero;
            }
            catch (Exception ex)
            {

                _mdb.Connection.Close();
                return -1;
                // throw new Exception(ex.Message);
            }

        }

        public int GetIdSEQCUSTOM()
        {
            var mdb = new DataSGE();

            var esquemaDb = ConfigurationManager.AppSettings["EsquemaDB"];

            var queryString = "SELECT " + esquemaDb + ".SEQ_RECUPEROS_DEBITO.nextval from dual";

            var contactQuery = mdb.ExecuteStoreQuery<decimal>(queryString).FirstOrDefault();
            mdb.Dispose();
            return (int)contactQuery;
        }
    }
}
