using System.Collections.Generic;
using System.Linq;
using SGE.Model.Entidades;
using SGE.Model.Entidades.Interfaces;
using SGE.Model.Repositorio;
using SGE.Repositorio.Modelo;
using System;

namespace SGE.Repositorio
{
    public class DepartamentoRepositorio : IDepartamentoRepositorio
    {
        private readonly DataSGE _mdb;

        public DepartamentoRepositorio()
        {
            _mdb = new DataSGE();
        }

        private IQueryable<IDepartamento> QDepartamento()
        {
            var a = (from c in _mdb.T_DEPARTAMENTOS
                     select
                         new Departamento
                             {
                                 IdDepartamento = c.ID_DEPARTAMENTO,
                                 NombreDepartamento = c.N_DEPARTAMENTO
                             });

            return a;
        }

        public IList<IDepartamento> GetDepartamentos()
        {
            return QDepartamento().OrderBy(o => o.NombreDepartamento).ToList();
        }

        public IList<IDepartamento> GetDepartamentos(int skip, int take)
        {
            return QDepartamento().OrderBy(c => c.NombreDepartamento).Skip(skip).Take(take).ToList();
        }

        private IQueryable<IDepartamento> QDepartamento(string descripcion)
        {
            descripcion = descripcion ?? String.Empty;

            return
                QDepartamento().Where(
                    c =>
                    (c.NombreDepartamento.ToLower().Contains(descripcion.ToLower()) || String.IsNullOrEmpty(descripcion))).OrderBy(o => o.NombreDepartamento.ToLower());
        }

        public IList<IDepartamento> GetDepartamentos(string descripcion)
        {
            return QDepartamento(descripcion).OrderBy(c => c.NombreDepartamento).ToList();
        }

        public IList<IDepartamento> GetDepartamentos(string descripcion, int skip, int take)
        {
            return QDepartamento(descripcion).OrderBy(c => c.NombreDepartamento).Skip(skip).Take(take).ToList();
        }

        public IDepartamento GetDepartamento(int id)
        {
            return QDepartamento().Where(c => c.IdDepartamento == id).SingleOrDefault();
        }

        public int AddDepartamento(IDepartamento depto)
        {
            var deptoModel = new T_DEPARTAMENTOS
            {
                ID_DEPARTAMENTO = SecuenciaRepositorio.GetId(),
                N_DEPARTAMENTO = depto.NombreDepartamento,
            };

            _mdb.T_DEPARTAMENTOS.AddObject(deptoModel);
            _mdb.SaveChanges();

            return deptoModel.ID_DEPARTAMENTO;
        }

        public void UpdateDepartamento(IDepartamento depto)
        {
            var obj = _mdb.T_DEPARTAMENTOS.SingleOrDefault(c => c.ID_DEPARTAMENTO == depto.IdDepartamento);

            if (obj != null)
            {
                obj.N_DEPARTAMENTO = depto.NombreDepartamento;
                _mdb.SaveChanges();
            }
        }

        public void DeleteDepartamento(IDepartamento depto)
        {
            var obj = _mdb.T_DEPARTAMENTOS.SingleOrDefault(c => c.ID_DEPARTAMENTO == depto.IdDepartamento);
            if (obj != null)
            {
                _mdb.DeleteObject(obj);

                _mdb.SaveChanges();
            }
        }

        public bool ExistsDepartamento(string depto)
        {
            var t = QDepartamento().Where(c => 0 == depto.CompareTo(c.NombreDepartamento)).ToList().Count;
            return t > 0 ? true : false;
        }

        public int GetDepartamentosCount()
        {
            return QDepartamento().Count();
        }

        public int GetDepartamentoMaxId()
        {
            var q = (from c in _mdb.T_DEPARTAMENTOS
                     select c.ID_DEPARTAMENTO);

            return q.Max();
        }
    }
}
