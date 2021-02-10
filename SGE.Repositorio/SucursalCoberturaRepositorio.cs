using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using SGE.Model.Entidades;
using SGE.Model.Entidades.Interfaces;
using SGE.Model.Repositorio;
using SGE.Repositorio.Modelo;

namespace SGE.Repositorio
{
    public class SucursalCoberturaRepositorio : ISucursalCoberturaRepositorio
    {
        private readonly DataSGE _mdb;

        public SucursalCoberturaRepositorio()
        {
            _mdb = new DataSGE();
        }
       
        public IQueryable<ILocalidad> QSucursalesLocalidad()
        {
            var a = (from c in _mdb.T_LOCALIDADES
                     select
                     new Localidad
                     {
                         IdLocalidad = c.ID_LOCALIDAD,
                         NombreLocalidad = c.N_LOCALIDAD,
                         IdDepartamento = c.ID_DEPARTAMENTO,
                         NombreDepartamento = c.T_DEPARTAMENTOS.N_DEPARTAMENTO,
                         Sucursales = c.T_TABLAS_BCO_CBA.Select(su => new Sucursal { IdSucursal = su.ID_TABLA_BCO_CBA, Detalle = su.DESCRIPCION, CodigoBanco = su.COD_BCO_CBA }).AsEnumerable(),
                         TieneSucursalAsignada =
                                   (c.T_TABLAS_BCO_CBA.Select(
                                       su =>
                                       new Sucursal
                                       {
                                           IdSucursal = su.ID_TABLA_BCO_CBA,
                                           Detalle = su.DESCRIPCION,
                                           CodigoBanco = su.COD_BCO_CBA
                                       }).Count()) > 0
                                       ? false
                                       : true

                     });

            return a;
        }
        
        public void AddLocalidadSucursal(ISucursalCobertura locSuc)
        {
            var localidad = _mdb.T_LOCALIDADES.FirstOrDefault(c => c.ID_LOCALIDAD == locSuc.IdLocalidad);
            var sucursal = _mdb.T_TABLAS_BCO_CBA.FirstOrDefault(c => c.ID_TABLA_BCO_CBA == locSuc.IdTablaBcoCba);

            localidad.T_TABLAS_BCO_CBA.Add(sucursal);

            _mdb.ObjectStateManager.ChangeObjectState(sucursal, EntityState.Unchanged);

            _mdb.SaveChanges();
        }

        public void DeleteLocalidadSucursal(ILocalidad locSuc)
        {
            if (locSuc.Sucursales.Count()== 0) return;

            int idSuc = locSuc.Sucursales.FirstOrDefault().IdSucursal;

            var localidad = _mdb.T_LOCALIDADES.FirstOrDefault(c => c.ID_LOCALIDAD == locSuc.IdLocalidad);
            var sucursal = _mdb.T_TABLAS_BCO_CBA.FirstOrDefault(c => c.ID_TABLA_BCO_CBA == idSuc);

            localidad.T_TABLAS_BCO_CBA.Remove(sucursal);

            foreach (var item in localidad.T_TABLAS_BCO_CBA)
            {
                _mdb.ObjectStateManager.ChangeObjectState(item, EntityState.Unchanged);
            }

            _mdb.SaveChanges();
        }
       
        public int GetSucursalesCoberturaCount()
        {
            return QSucursalesLocalidad().Count();
        }

        public int GetSucursalesCoberturaCount(int? idSucursal, int? idLocalidad, int? idDepartamento, string asignadas)
        {
            idSucursal = idSucursal.ToString() == "" ? null : idSucursal;
            idLocalidad = idLocalidad.ToString() == "" ? null : idLocalidad;
            idDepartamento = idDepartamento.ToString() == "" ? null : idDepartamento;
            asignadas = asignadas ?? "T";

            return
                QSucursalesLocalidad().Where(
                    c =>
                    (c.IdLocalidad == idLocalidad || idLocalidad == 0) &&
                    (asignadas == "T"
                         ? (true)
                         : (asignadas == "C" ? (c.TieneSucursalAsignada) : c.TieneSucursalAsignada == false)) &&
                    (c.IdDepartamento == idDepartamento || idDepartamento == 0)).OrderBy(c => c.NombreDepartamento).
                    Count();
        }
    }
}
