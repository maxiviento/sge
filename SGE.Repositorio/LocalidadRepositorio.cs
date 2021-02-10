using System;
using System.Collections.Generic;
using System.Linq;
using SGE.Model.Entidades;
using SGE.Model.Entidades.Interfaces;
using SGE.Model.Repositorio;
using SGE.Repositorio.Modelo;

namespace SGE.Repositorio
{
    public class LocalidadRepositorio : ILocalidadRepositorio
    {
        private readonly DataSGE _mdb;

        public LocalidadRepositorio()
        {
            _mdb = new DataSGE();
        }

        private IQueryable<ILocalidad> QLocalidad()
        {
            var a = (from c in _mdb.T_LOCALIDADES
                     select
                         new Localidad
                             {
                                 IdLocalidad = c.ID_LOCALIDAD,
                                 NombreLocalidad = c.N_LOCALIDAD,
                                 IdDepartamento = c.ID_DEPARTAMENTO,
                                 NombreDepartamento = c.T_DEPARTAMENTOS.N_DEPARTAMENTO
                             });

            return a;
        }

        private IQueryable<ILocalidad> QLocalidadSucursal()
        {
            var a = (from c in _mdb.T_LOCALIDADES
                     select
                         new Localidad
                             {
                                 IdLocalidad = c.ID_LOCALIDAD,
                                 NombreLocalidad = c.N_LOCALIDAD,
                                 IdDepartamento = c.ID_DEPARTAMENTO,
                                 NombreDepartamento = c.T_DEPARTAMENTOS.N_DEPARTAMENTO,
                                 Sucursales =
                                     c.T_TABLAS_BCO_CBA.Select(
                                         su =>
                                         new Sucursal
                                             {
                                                 IdSucursal = su.ID_TABLA_BCO_CBA,
                                                 Detalle = su.DESCRIPCION,
                                                 CodigoBanco = su.COD_BCO_CBA
                                             }).AsEnumerable(),
                                 TieneSucursalAsignada =
                                     (c.T_TABLAS_BCO_CBA.Select(
                                         su =>
                                         new Sucursal
                                             {
                                                 IdSucursal = su.ID_TABLA_BCO_CBA,
                                                 Detalle = su.DESCRIPCION,
                                                 CodigoBanco = su.COD_BCO_CBA
                                             }).Count()) > 0
                                         ? true
                                         : false

                             });
            return a;
        }

        public IList<ILocalidad> GetLocalidades()
        {
            return QLocalidad().OrderBy(o => o.NombreLocalidad).ToList();
        }

        public IList<ILocalidad> GetLocalidadesByDepto(int idDepartamento)
        {
            return QLocalidad().Where(c => c.IdDepartamento == idDepartamento).OrderBy(o => o.NombreLocalidad).ToList();
        }

        public int AddLocalidad(ILocalidad localidad)
        {
            var prModel = new T_LOCALIDADES
                              {
                                  ID_LOCALIDAD = (short)localidad.IdLocalidad,
                                  N_LOCALIDAD = localidad.NombreLocalidad,
                                  ID_DEPARTAMENTO = localidad.IdDepartamento
                              };

            _mdb.T_LOCALIDADES.AddObject(prModel);
            _mdb.SaveChanges();
            return prModel.ID_LOCALIDAD;
        }

        public ILocalidad GetLocalidad(int id)
        {
            return QLocalidad().Where(c => c.IdLocalidad == id).SingleOrDefault();
        }

        public IList<ILocalidad> GetLocalidades(int skip, int take)
        {
            return QLocalidad().OrderBy(c => c.NombreLocalidad).Skip(skip).Take(take).ToList();
        }

        private IQueryable<ILocalidad> QGetLocalidades(string descripcion)
        {
            descripcion = descripcion ?? String.Empty;

            return
                QLocalidad().Where(
                    c =>
                    (c.NombreLocalidad.ToLower().Contains(descripcion.ToLower()) || String.IsNullOrEmpty(descripcion))).
                    OrderBy(o => o.NombreLocalidad);
        }

        public IList<ILocalidad> GetLocalidades(string descripcion)
        {
            return QGetLocalidades(descripcion).OrderBy(c => c.NombreLocalidad).ToList();
        }

        public IList<ILocalidad> GetLocalidades(string descripcion, int skip, int take)
        {
            return QGetLocalidades(descripcion).OrderBy(c => c.NombreLocalidad).Skip(skip).Take(take).ToList();
        }

        public IList<ILocalidad> GetSucursalesforLocalidades()
        {
            return QLocalidadSucursal().ToList();
        }

        public void UpdateLocalidad(ILocalidad loc)
        {
            if (loc.IdLocalidad == 0) return;

            var obj = _mdb.T_LOCALIDADES.SingleOrDefault(c => c.ID_LOCALIDAD == loc.IdLocalidad);

            if (obj == null) return;

            obj.N_LOCALIDAD = loc.NombreLocalidad;
            obj.ID_DEPARTAMENTO = loc.IdDepartamento;

            _mdb.SaveChanges();
        }

        public int GetLocalidadMaxId()
        {
            var a = (from c in _mdb.T_LOCALIDADES
                     select c.ID_LOCALIDAD);

            return a.Max();
        }

        public int GetLocalidadesCount()
        {
            return QLocalidad().Count();
        }

        public IList<ILocalidad> GetSucursalesforLocalidad(int skip, int take)
        {
            return
                QLocalidadSucursal().OrderBy(c => c.NombreDepartamento).Skip(skip).Take(take).ToList();
        }

        public ILocalidad GetSucursalAsignadaForLocalidad(int idLocalidad, int idSucursal)
        {
            return QLocalidadSucursal()
                .Where(c =>
                      (c.IdLocalidad == idLocalidad)
                      ).SingleOrDefault();
        }

        public IList<ILocalidad> GetSucursalesforLocalidad(int? idSucursal, int? idLocalidad, int? idDepartamento, string asignadas, int skip, int take)
        {
            idSucursal = idSucursal.ToString() == "" ? null : idSucursal;
            idLocalidad = idLocalidad.ToString() == "" ? null : idLocalidad;
            idDepartamento = idDepartamento.ToString() == "" ? null : idDepartamento;
            asignadas = asignadas ?? "T";

            return
                QLocalidadSucursal().Where(
                    c =>
                    (idSucursal != 0 ? (c.Sucursales.Where(su => su.IdSucursal == idSucursal).Count() > 0) : (c.Sucursales.Where(su => su.IdSucursal == idSucursal).Count() > -1)) &&
                    (c.IdLocalidad == idLocalidad || idLocalidad == 0) &&
                    (asignadas == "T" ? (true) : (asignadas == "C" ? (c.TieneSucursalAsignada) : c.TieneSucursalAsignada == false)) &&
                    (c.IdDepartamento == idDepartamento || idDepartamento == 0)).OrderBy(c => c.NombreDepartamento).Skip(skip).Take(take).ToList();
        }

        public int GetSucursalesCoberturaCount(int? idSucursal, int? idLocalidad, int? idDepartamento, string asignadas)
        {
            idSucursal = idSucursal.ToString() == "" ? null : idSucursal;
            idLocalidad = idLocalidad.ToString() == "" ? null : idLocalidad;
            idDepartamento = idDepartamento.ToString() == "" ? null : idDepartamento;
            asignadas = asignadas ?? "T";

            return
                QLocalidadSucursal().Where(
                    c =>
                    (idSucursal != 0 ? (c.Sucursales.Where(su => su.IdSucursal == idSucursal).Count() > 0) : (c.Sucursales.Where(su => su.IdSucursal == idSucursal).Count() > -1)) &&
                    (c.IdLocalidad == idLocalidad || idLocalidad == 0) &&
                    (asignadas == "T" ? (true) : (asignadas == "C" ? (c.TieneSucursalAsignada) : c.TieneSucursalAsignada == false)) &&
                    (c.IdDepartamento == idDepartamento || idDepartamento == 0)).OrderBy(c => c.NombreDepartamento).
                    Count();
        }

        public ILocalidad GetSucursalCoberturaByLocalidad(int? idLocalidad)
        {
            idLocalidad = idLocalidad.ToString() == "" ? null : idLocalidad;

            return QLocalidadSucursal().Where(c => (c.IdLocalidad == idLocalidad || idLocalidad == 0)).SingleOrDefault();
        }

        public int GetSucursalesforLocalidadCount()
        {
            return QLocalidadSucursal().Count();
        }


        public IList<IBarrio> GetBarrios()
        {
            return Qbarrios().ToList();
        }

        private IQueryable<IBarrio> Qbarrios()
        {
            var a = (from b in _mdb.T_BARRIOS
                     select new Barrio
                     {
                         ID_BARRIO = b.ID_BARRIO,
                         N_BARRIO = b.N_BARRIO,
                         ID_LOCALIDAD = b.ID_LOCALIDAD,
                         ID_SECCIONAL = b.ID_SECCIONAL,
                     });

                    return a;
        
        }
    }
}
