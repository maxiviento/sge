using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using SGE.Model.Entidades.Interfaces;
using SGE.Repositorio;
using SGE.Servicio.ServicioInterfaces;
using SGE.Servicio.Vista;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces;
using System.Transactions;
using SGE.Model.Entidades;
using SGE.Model.Repositorio;

namespace SGE.Servicio.Servicio
{
    public class LocalidadServicio : ILocalidadServicio
    {
        private readonly ILocalidadRepositorio _localidadRepositorio;
        private readonly IAutenticacionServicio _autenticacion;
        private readonly IDepartamentoRepositorio _departamentoRepositorio;

        public LocalidadServicio()
        { 
            _localidadRepositorio=new LocalidadRepositorio();
            _departamentoRepositorio = new DepartamentoRepositorio();
            _autenticacion = new AutenticacionServicio();
        }

        public ILocalidadesVista GetLocalidades()
        {
            ILocalidadesVista vista = new LocalidadesVista();

            var pager = new Pager(_localidadRepositorio.GetLocalidadesCount(),
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexLocalidad", _autenticacion.GetUrl("IndexPager", "Localidades"));

            vista.Pager = pager;

            vista.Localidades = _localidadRepositorio.GetLocalidades(pager.Skip, pager.PageSize);

            return vista;
        }

        public ILocalidadesVista GetIndex()
        {
            ILocalidadesVista vista = new LocalidadesVista();

            var pager = new Pager(0,
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexLocalidad", _autenticacion.GetUrl("IndexPager", "Localidades"));

            vista.Pager = pager;


            return vista;
        }

        public ILocalidadesVista GetLocalidades(string descripcion)
        {
            ILocalidadesVista vista = new LocalidadesVista();

            var pager = new Pager(_localidadRepositorio.GetLocalidades(descripcion).Count(),
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexLocalidad", _autenticacion.GetUrl("IndexPager", "Localidades"));

            vista.Pager = pager;

            vista.Localidades= _localidadRepositorio.GetLocalidades( descripcion, pager.Skip, pager.PageSize);

            return vista;
        }

        public ILocalidadVista GetLocalidad(int idLocalidad, string accion)
        {
            ILocalidadVista vista = new LocalidadVista {Accion = accion};

            ILocalidad row = _localidadRepositorio.GetLocalidad(idLocalidad);

            vista.Id = row.IdLocalidad;
            vista.Nombre = row.NombreLocalidad;
            vista.NombreDepartamento = row.NombreDepartamento;
            vista.NombreDepartamentoCombo.Selected= row.IdDepartamento.ToString();

            if(accion!="Ver")
            {
                CargarDepartamentos(vista,_departamentoRepositorio.GetDepartamentos());
            }

            return vista;
        }

        public ILocalidadesVista GetLocalidades(Pager pPager, int id, string descripcion)
        {
            ILocalidadesVista vista = new LocalidadesVista();

            var pager = new Pager(_localidadRepositorio.GetLocalidades(descripcion).Count(), 
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexLocalidad", _autenticacion.GetUrl("IndexPager", "Localidades"));

            if (pager.TotalCount != pPager.TotalCount)
            {
                pager.PageNumber = 1;
                pager.Skip = 0;
                vista.Pager = pager;
            }
            else
            {
                vista.Pager = pPager;
            }

            vista.Localidades = _localidadRepositorio.GetLocalidades(descripcion, vista.Pager.Skip, vista.Pager.PageSize);
            vista.DescripcionBusca = descripcion;

            return vista;
        }

        public int AddLocalidad(ILocalidadVista vista)
        {
            if (_localidadRepositorio.GetLocalidades(vista.Nombre).Count()>0)
            {
                CargarDepartamentos(vista,_departamentoRepositorio.GetDepartamentos());
                vista.NombreDepartamento = "Departamento";
                return 1;
            }

            vista.Id = _localidadRepositorio.AddLocalidad(VistaToDatos(vista));

            return 0;
        }

        public ILocalidadVista AddLocalidadLast(string accion)
        {
            ILocalidadVista vista = new LocalidadVista
                                        {
                                            Id = _localidadRepositorio.GetLocalidadMaxId() + 1,
                                            Accion = accion
                                        };

            CargarDepartamentos(vista, _departamentoRepositorio.GetDepartamentos());
            vista.NombreDepartamento = "Departamento";

            return vista;
        }

        private static ILocalidad VistaToDatos(ILocalidadVista vista)
        {
            int dep = Convert.ToInt32(vista.NombreDepartamentoCombo.Selected );

            ILocalidad localidad= new Localidad
            {
                IdLocalidad = vista.Id,
                NombreLocalidad = vista.Nombre,
                NombreDepartamento = vista.NombreDepartamento,
                IdDepartamento=dep};
                return localidad;
        }

        public int UpdateLocalidad(ILocalidadVista vista)
        {
             // Define a transaction scope for the operations.
            using (var transaction = new TransactionScope())
            {
                
                    CargarDepartamentos(vista,_departamentoRepositorio.GetDepartamentos());
                    // Actualizar Localidad
                    _localidadRepositorio.UpdateLocalidad(VistaToDatos(vista));
  
                    // Completa la transaccion
                    transaction.Complete();

                    return 0;
               
            }
        }

        public ILocalidadVista GetSucursalCoberturaByLocalidad(int idLocalidad)
        {
            ILocalidadVista vista = new LocalidadVista();

            ILocalidad objreturn = _localidadRepositorio.GetSucursalCoberturaByLocalidad(idLocalidad);

            if(objreturn != null)
            {
                vista.Id = objreturn.IdLocalidad;
                vista.IdSucursal = objreturn.Sucursales != null
                                       ? objreturn.Sucursales.Select(c => c.IdSucursal).FirstOrDefault()
                                       : 0;
                vista.NombreSucursal = objreturn.Sucursales != null
                                           ? objreturn.Sucursales.Select(c => c.Detalle).FirstOrDefault()
                                           : "";
            }

            return vista;
        }

        private static void CargarDepartamentos(ILocalidadVista vista, IEnumerable<IDepartamento> listaDepartamentos)
        {
            vista.NombreDepartamentoCombo.Combo.Add(new ComboItem { Id = 0, Description = "Seleccione un Departamento..." });
            
            foreach (var departamento in listaDepartamentos)
            {
                vista.NombreDepartamentoCombo.Combo.Add(new ComboItem
                                                            {
                    Id = departamento.IdDepartamento,
                    Description = departamento.NombreDepartamento
                });
            }
        }
    }
}
