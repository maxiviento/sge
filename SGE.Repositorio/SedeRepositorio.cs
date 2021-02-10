using System;
using System.Collections.Generic;
using System.Linq;
using SGE.Model.Entidades;
using SGE.Model.Entidades.Interfaces;
using SGE.Repositorio.Modelo;
using SGE.Model.Repositorio;

namespace SGE.Repositorio
{
    public class SedeRepositorio :BaseRepositorio, ISedeRepositorio
    {
        private readonly DataSGE _mdb;

        public SedeRepositorio()
        {
            _mdb = new DataSGE();
        }

        private IQueryable<ISede> QSede()
        {
            var a = (from i in _mdb.T_SEDES
                     select
                         new Sede
                             {
                                 IdSede = i.ID_SEDE,
                                 IdEmpresa = i.ID_EMPRESA,
                                 NombreSede = i.N_SEDE.ToUpper().Trim(),
                                 Calle = i.CALLE,
                                 Numero = i.NUMERO,
                                 Piso = i.PISO,
                                 Dpto = i.DPTO,
                                 CodigoPostal = i.CODIGO_POSTAL,
                                 IdLocalidad = i.ID_LOCALIDAD,
                                 ApellidoContacto = i.APE_CONTACTO.ToUpper().Trim(),
                                 NombreContacto = i.NOM_CONTACTO.ToUpper().Trim(),
                                 Telefono = i.TELEFONO,
                                 Fax = i.FAX,
                                 Email = i.EMAIL,
                                 NombreEmpresa = i.T_EMPRESAS.N_EMPRESA.Trim(),
                                 IdUsuarioSistema = i.ID_USR_SIST,
                                 UsuarioSistema = i.T_USUARIOS.LOGIN,
                                 FechaSistema = i.FEC_SIST,
                                 NombreLocalidad = i.T_LOCALIDADES.N_LOCALIDAD
                             });
            return a;
        }

        public IList<ISede> GetSedes()
        {
            return QSede().OrderBy(c => c.NombreSede.ToUpper()).ToList();
        }

        public IList<ISede> GetSedes(int skip, int take)
        {
            return QSede().OrderBy(c => c.NombreSede.ToUpper()).Skip(skip).Take(take).ToList();
        }

        public IList<ISede> GetSedes(string descripcion, int? idEmpresa)
        {
            descripcion = descripcion ?? String.Empty;
            idEmpresa = idEmpresa.ToString() == String.Empty ? null : idEmpresa;

            return
                QSede().Where(
                    c =>
                    (c.NombreSede.ToUpper().Contains(descripcion.ToUpper().Trim()) || String.IsNullOrEmpty(descripcion.Trim())) &&
                    (c.IdEmpresa == idEmpresa || idEmpresa == 0)).OrderBy(o => o.NombreSede.ToUpper())
                    .ToList();
        }

        public IList<ISede> GetSedes(string descripcion, int? idEmpresa, int skip, int take)
        {
            descripcion = descripcion ?? String.Empty;
            idEmpresa = idEmpresa.ToString() == String.Empty ? null : idEmpresa;

            return
                QSede().Where(
                    c =>
                    (c.NombreSede.ToUpper().Contains(descripcion.ToUpper().Trim()) || String.IsNullOrEmpty(descripcion.Trim())) &&
                    (c.IdEmpresa == idEmpresa || idEmpresa == 0)).OrderBy(o => o.NombreSede.ToUpper())
                    .Skip(skip).Take(take)
                    .ToList();
        }

        public IList<ISede> GetSedesByEmpresa(string descripcion, int? idEmpresa)
        {
            descripcion = descripcion ?? String.Empty;
            idEmpresa = idEmpresa.ToString() == String.Empty ? null : idEmpresa;

            return
                QSede().Where(
                    c =>
                    (c.NombreSede.ToUpper().Contains(descripcion.ToUpper()) || String.IsNullOrEmpty(descripcion)) &&
                    (c.IdEmpresa == idEmpresa || idEmpresa == 0)).OrderBy(o => o.NombreSede.ToUpper())
                    .ToList();
        }

        public ISede GetSedes(int id)
        {
            return QSede().Where(c => c.IdSede == id).SingleOrDefault();
        }

        public int AddSede(ISede sede)
        {
            AgregarDatos(sede);
            
            var empModel = new T_SEDES
            {
                ID_SEDE = SecuenciaRepositorio.GetId(),
                ID_EMPRESA = sede.IdEmpresa,
                N_SEDE = (sede.NombreSede ?? "").ToUpper().Trim(),
                CALLE = sede.Calle,
                NUMERO = sede.Numero,
                PISO = sede.Piso,
                DPTO = sede.Dpto,
                CODIGO_POSTAL = sede.CodigoPostal,
                ID_LOCALIDAD = sede.IdLocalidad,
                APE_CONTACTO = (sede.ApellidoContacto ?? "").ToUpper().Trim(),
                NOM_CONTACTO = (sede.NombreContacto ?? "").ToUpper().Trim(),
                TELEFONO = sede.Telefono,
                FAX = sede.Fax,
                EMAIL = sede.Email,
                ID_USR_SIST = sede.IdUsuarioSistema,
                FEC_SIST = sede.FechaSistema
            };

            _mdb.T_SEDES.AddObject(empModel);
            _mdb.SaveChanges();
            
            return empModel.ID_SEDE;
        }

        public void UpdateSede(ISede sede)
        {
            AgregarDatos(sede);

            var obj = _mdb.T_SEDES.SingleOrDefault(c => c.ID_SEDE == sede.IdSede);

            if (obj == null) return;
            
            obj.N_SEDE = sede.NombreSede.ToUpper().Trim();
            obj.CALLE = sede.Calle;
            obj.NUMERO = sede.Numero;
            obj.PISO = sede.Piso;
            obj.DPTO = sede.Dpto;
            obj.CODIGO_POSTAL = sede.CodigoPostal;
            obj.ID_LOCALIDAD = sede.IdLocalidad;
            obj.APE_CONTACTO = sede.ApellidoContacto.ToUpper().Trim();
            obj.NOM_CONTACTO = sede.NombreContacto.ToUpper().Trim();
            obj.TELEFONO = sede.Telefono;
            obj.FAX = sede.Fax;
            obj.EMAIL = sede.Email;
            obj.ID_USR_SIST = sede.IdUsuarioSistema;

            _mdb.SaveChanges();
        }

        public void DeleteSede(ISede sede)
        {
            AgregarDatos(sede);
            
            var obj = _mdb.T_SEDES.SingleOrDefault(c => c.ID_SEDE == sede.IdSede);

            if (obj == null) return;

            obj.ID_USR_SIST = sede.IdUsuarioSistema;

            _mdb.SaveChanges();
            
            _mdb.DeleteObject(obj);
            
            _mdb.SaveChanges();
        }

        public bool ExistsSede(int idSede, string descripcion)
        {
            var t = (QSede().Where(c => c.IdSede != idSede && 
                c.NombreSede.ToUpper().Trim() == descripcion.ToUpper().Trim())).ToList().Count;

            return t > 0 ? true : false;
        }

        public int GetSedesCount(string descripcion, int? idEmpresa)
        {
            descripcion = descripcion ?? String.Empty;
            idEmpresa = idEmpresa.ToString() == String.Empty ? null : idEmpresa;

            return QSede().Where(c =>
                             (c.NombreSede.ToUpper().Contains(descripcion.ToUpper().Trim()) || 
                             String.IsNullOrEmpty(descripcion.Trim())) &&
                             (c.IdEmpresa == idEmpresa || idEmpresa == 0))
                             .Count();

        }

        public int GetSedesCount()
        {
            return QSede().Count();
        }

        public bool SedeTieneFichas(int idSede)
        {
            var t = _mdb.T_FICHA_PPP.Where(c => c.ID_SEDE == idSede).ToList().Count;

            return t > 0 ? true : false;
        }

        public int GetSedesMaxId()
        {
            var a = (from c in _mdb.T_SEDES
                     select c.ID_SEDE);

            return a.Max();
        }

        public IList<ISede> GetSedesByEmpresa(int idEmpresa)
        {
            return QSede().Where(c => (c.IdEmpresa == idEmpresa)).OrderBy(c => c.NombreSede.ToUpper()).ToList();
        }

        public bool ExistsSedeByEmpresa(int idempresa, string desc)
        {
            var t = (QSede().Where(c => c.IdEmpresa == idempresa &&
              c.NombreSede.ToUpper().Trim() == desc.ToUpper().Trim())).ToList().Count;

            return t > 0 ? true : false;
        }
    }
}
