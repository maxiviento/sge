using System;
using System.Collections.Generic;
using System.Linq;
using SGE.Model.Entidades;
using SGE.Model.Entidades.Interfaces;
using SGE.Model.Repositorio;
using SGE.Repositorio.Modelo;

namespace SGE.Repositorio
{
    public class ConyugeRepositorio : BaseRepositorio, IConyugeRepositorio
    {
        private readonly DataSGE _mdb;

        public ConyugeRepositorio()
        {
            _mdb = new DataSGE();
        }

        private IQueryable<IConyuge> QConyuges()
        {
            var a = (from c in _mdb.T_CONYUGES
                     select
                         new Conyuge
                             {
                                 Apellido = c.APELLIDO,
                                 Cuil = c.CUIL,
                                 FechaNacimiento = c.FEC_NAC ?? DateTime.Now,
                                 Nacionalidad = c.NACIONALIDAD ?? 0,
                                 Nombre = c.NOMBRE,
                                 NumeroDocumento = c.NUMERO_DOCUMENTO,
                                 Sexo = c.SEXO,
                                 TipoDocumento = c.TIPO_DOCUMENTO ?? 0,
                                 IdBeneficiario = c.ID_BENEFICIARIO,
                                 IdConyugue = c.ID_BENEFICIARIO,
                                 IdUsuarioSistema = c.ID_USR_SIST,
                                 FechaSistema = c.FEC_SIST,
                                 UsuarioSistema = c.T_USUARIOS.LOGIN
                             });
            return a;
        }

        public IConyuge GetConyugeByBeneficiario(int idBeneficiario)
        {
            return QConyuges().Where(c => c.IdBeneficiario == idBeneficiario).SingleOrDefault();
        }

        public IList<IConyuge> GetConyuges()
        {
            return QConyuges().OrderBy(c => c.Apellido.ToLower()).ToList();
        }

        public IList<IConyuge> GetConyuges(int skip, int take)
        {
            return QConyuges().OrderBy(c => c.Apellido.ToLower()).Skip(skip).Take(take).ToList();
        }

        public IList<IConyuge> GetSedes(string apellido, string nombre, string cuil)
        {
            apellido = apellido ?? string.Empty;
            nombre = nombre ?? string.Empty;
            cuil = cuil ?? string.Empty;

            return QConyuges().Where(c =>
                                     (c.Nombre.ToLower().Contains(nombre.ToLower()) || String.IsNullOrEmpty(nombre)) &&
                                     (c.Apellido.ToLower().Contains(apellido.ToLower()) || string.IsNullOrEmpty(apellido)) &&
                                     (c.Cuil.ToLower().Contains(cuil.ToLower()) || string.IsNullOrEmpty(cuil))).OrderBy(o=> o.Apellido).ToList();
        }

        public IList<IConyuge> GetSedes(string apellido, string nombre, string cuil, int skip, int take)
        {
            apellido = apellido ?? string.Empty;
            nombre = nombre ?? string.Empty;
            cuil = cuil ?? string.Empty;

            return QConyuges().Where(c =>
                                     (c.Nombre.ToLower().Contains(nombre.ToLower()) || String.IsNullOrEmpty(nombre)) &&
                                     (c.Apellido.ToLower().Contains(apellido.ToLower()) || string.IsNullOrEmpty(apellido)) &&
                                     (c.Cuil.ToLower().Contains(cuil.ToLower()) || string.IsNullOrEmpty(cuil))).OrderBy(o => o.Apellido)
                                     .Skip(skip).Take(take).ToList();
        }

        public IConyuge GetConyuge(int id)
        {
            return QConyuges().Where(c => c.IdConyugue == id).SingleOrDefault();
        }

        public int AddConyuge(IConyuge conyuge)
        {
            AgregarDatos(conyuge);
            
            var conyModel = new T_CONYUGES
                                {
                ID_CONYUGE = SecuenciaRepositorio.GetId(),
                ID_BENEFICIARIO = conyuge.IdBeneficiario,
                CUIL = conyuge.Cuil,
                APELLIDO = conyuge.Apellido,
                NOMBRE = conyuge.Nombre,
                FEC_NAC = conyuge.FechaNacimiento,
                TIPO_DOCUMENTO = conyuge.TipoDocumento,
                NUMERO_DOCUMENTO = conyuge.NumeroDocumento,
                SEXO = conyuge.Sexo,
                NACIONALIDAD = conyuge.Nacionalidad,
                ID_USR_SIST = conyuge.IdUsuarioSistema,
                FEC_SIST = conyuge.FechaSistema
            };

            _mdb.T_CONYUGES.AddObject(conyModel);
            _mdb.SaveChanges();
            return conyModel.ID_CONYUGE;
        }

        public void UpdateConyuge(IConyuge conyuge)
        {
            AgregarDatos(conyuge);
            
            var obj = _mdb.T_CONYUGES.SingleOrDefault(c => c.ID_CONYUGE == conyuge.IdConyugue);

            if (obj != null)
            {
                obj.ID_BENEFICIARIO = conyuge.IdBeneficiario;
                obj.CUIL = conyuge.Cuil;
                obj.APELLIDO = conyuge.Apellido;
                obj.NOMBRE = conyuge.Nombre;
                obj.FEC_NAC = conyuge.FechaNacimiento;
                obj.TIPO_DOCUMENTO = conyuge.TipoDocumento;
                obj.NUMERO_DOCUMENTO = conyuge.NumeroDocumento;
                obj.SEXO = conyuge.Sexo;
                obj.NACIONALIDAD = conyuge.Nacionalidad;
                obj.ID_USR_SIST = conyuge.IdUsuarioSistema;

                _mdb.SaveChanges();
            }
        }

        public void DeleteConyuge(IConyuge sede)
        {
            throw new NotImplementedException();
        }

        public bool ExistsConyuge(string cuil)
        {
            throw new NotImplementedException();
        }

        public int GetConyugesCount(string apellido, string nombre, string cuil)
        {
            throw new NotImplementedException();
        }

        public int GetConyugesCount()
        {
            throw new NotImplementedException();
        }
    }
}
