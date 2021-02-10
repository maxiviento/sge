using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Transactions;
using SGE.Model.Entidades;
using SGE.Model.Entidades.Interfaces;
using SGE.Model.Repositorio;
using SGE.Repositorio;
using SGE.Servicio.ServicioInterfaces;
using SGE.Servicio.Vista;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces;

namespace SGE.Servicio.Servicio
{
    public class SucursalCoberturaServicio : ISucursalCoberturaServicio
    {
        private readonly SucursalCoberturaRepositorio _sucursalCoberturaRepositorio;
        private readonly LocalidadRepositorio _localidadRepositorio;
        private readonly DepartamentoRepositorio _departamentoRepositorio;
        private readonly SucursalRepositorio _sucursalRepositorio;
        private readonly IAutenticacionServicio _aut;

        public SucursalCoberturaServicio()
        {
            _sucursalCoberturaRepositorio = new SucursalCoberturaRepositorio();
            _localidadRepositorio = new LocalidadRepositorio();
            _departamentoRepositorio = new DepartamentoRepositorio();
            _sucursalRepositorio = new SucursalRepositorio();
            _aut = new AutenticacionServicio();
        }

        public ISucursalesCoberturaVista GetSucursalesCobertura()
        {
            ISucursalesCoberturaVista vista = new SucursalesCoberturaVista();

            var pager = new Pager(_localidadRepositorio.GetSucursalesforLocalidadCount(),
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexSucursalCobertura", _aut.GetUrl("IndexPager", "SucursalCobertura"));

            vista.Pager = pager;

            vista.ListaSucursalesCobertura = _localidadRepositorio.GetSucursalesforLocalidad(pager.Skip, pager.PageSize);
            
            CargarDepartamentos(vista, _departamentoRepositorio.GetDepartamentos());
            CargarSucursales(vista, _sucursalRepositorio.GetSucursales());
            CargarLocalidades(vista, _localidadRepositorio.GetLocalidades());

            return vista;
        }

        public ISucursalesCoberturaVista GetIndex()
        {
            ISucursalesCoberturaVista vista = new SucursalesCoberturaVista();

            var pager = new Pager(0,
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexSucursalCobertura", _aut.GetUrl("IndexPager", "SucursalCobertura"));

            vista.Pager = pager;

            CargarDepartamentos(vista, _departamentoRepositorio.GetDepartamentos());
            CargarSucursales(vista, _sucursalRepositorio.GetSucursales());
            CargarLocalidades(vista, _localidadRepositorio.GetLocalidades());

            return vista;
        }

        public ISucursalesCoberturaVista GetSucursalesCobertura(int? idSucursal, int? idLocalidad, int? idDepartamento, string asignadas)
        {
            ISucursalesCoberturaVista vista = new SucursalesCoberturaVista();

            var pager = new Pager(_localidadRepositorio.GetSucursalesCoberturaCount(idSucursal, idLocalidad, idDepartamento, asignadas),
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexSucursalCobertura", _aut.GetUrl("IndexPager", "SucursalCobertura"));

            vista.Pager = pager;

            vista.ListaSucursalesCobertura = _localidadRepositorio.GetSucursalesforLocalidad(idSucursal, idLocalidad, idDepartamento, asignadas, pager.Skip, pager.PageSize);
            
            CargarDepartamentos(vista, _departamentoRepositorio.GetDepartamentos());
            CargarSucursales(vista, _sucursalRepositorio.GetSucursales());

            if (idDepartamento != null)
                CargarLocalidades(vista, _localidadRepositorio.GetLocalidadesByDepto(idDepartamento.Value));

            vista.Asignadas = asignadas;

            return vista;
        }

        public ISucursalesCoberturaVista GetSucursalesCobertura(Pager pPager, int? idSucursal, int? idLocalidad, int? idDepartamento, string asignadas)
        {
            ISucursalesCoberturaVista vista = new SucursalesCoberturaVista();

            var pager = new Pager(_localidadRepositorio.GetSucursalesCoberturaCount(idSucursal, idLocalidad, idDepartamento, asignadas),
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexSucursalCobertura", _aut.GetUrl("IndexPager", "SucursalCobertura"));

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

            vista.ListaSucursalesCobertura = _localidadRepositorio.GetSucursalesforLocalidad(idSucursal, idLocalidad, idDepartamento, asignadas, vista.Pager.Skip, vista.Pager.PageSize);
            
            CargarDepartamentos(vista, _departamentoRepositorio.GetDepartamentos());
            CargarSucursales(vista, _sucursalRepositorio.GetSucursales());
            
            CargarLocalidades(vista, _localidadRepositorio.GetLocalidadesByDepto(Convert.ToInt32(vista.BusquedaDepartamentos.Selected)));

            vista.Asignadas = asignadas;

            return vista;
        }
        
        public ISucursalCoberturaVista GetSucursalCoberturaByLocalidad(int idLocalidad, string accion)
        {
            ISucursalCoberturaVista vista = new SucursalCoberturaVista();
            
            ILocalidad objreturn = _localidadRepositorio.GetSucursalCoberturaByLocalidad(idLocalidad);

            vista.IdLocalidad = objreturn.IdLocalidad;

            int idSucural = objreturn.Sucursales.Count() != 0 ? objreturn.Sucursales.FirstOrDefault().IdSucursal : 0;
            vista.IdTablaBcoCba = idSucural;

            CargarSucursal(vista, _sucursalRepositorio.GetSucursales());
            CargarLocalidad(vista, _localidadRepositorio.GetLocalidades());
            vista.Localidades.Selected = objreturn.IdLocalidad.ToString();
            vista.Sucursales.Selected = idSucural.ToString();
            vista.Accion = accion;

            if(accion=="Modificar")
            {
                vista.Localidades.Enabled = false;
                vista.Sucursales.Enabled = true;
            }
            else if (accion=="Ver")
            {
                vista.Localidades.Enabled = false;
                vista.Sucursales.Enabled = false;
            }

            return vista;
        }
        
        public int UpdateSucursalCobertura(ISucursalCoberturaVista vista)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    // Actualizar la sucursal de la localidad
                    // ---------------------------------------------------
                    DeleteLocalidadSucursal(vista);
                    AddLocalidadSucursal(vista);

                    transaction.Complete();
                
                    return 0;
                }
            }
            catch (Exception)
            {
                CargarSucursal(vista, _sucursalRepositorio.GetSucursales());
                CargarLocalidad(vista, _localidadRepositorio.GetLocalidades());

                return 1;
            }
        }

        public void AddLocalidadSucursal(ISucursalCoberturaVista vista)
        {
           ISucursalCobertura objsucursalCobertura = new SucursalCobertura()
                                                              {
                                                                  IdTablaBcoCba = Convert.ToInt32(vista.Sucursales.Selected),
                                                                  IdLocalidad = vista.IdLocalidad
                                                              };

           _sucursalCoberturaRepositorio.AddLocalidadSucursal(objsucursalCobertura);
        }

        public void DeleteLocalidadSucursal(ISucursalCoberturaVista sucursalCobertura)
        {
            var objSucursalesAsignadas = _localidadRepositorio.GetSucursalAsignadaForLocalidad(sucursalCobertura.IdLocalidad, sucursalCobertura.IdTablaBcoCba);

            _sucursalCoberturaRepositorio.DeleteLocalidadSucursal(objSucursalesAsignadas);
        }

        public ISucursalesCoberturaVista GetSucursalesCobertura(SucursalesCoberturaVista sucursalesCoberturaVista)
        {
            CargarDepartamentos(sucursalesCoberturaVista, _departamentoRepositorio.GetDepartamentos());
            CargarLocalidades(sucursalesCoberturaVista, _localidadRepositorio.GetLocalidadesByDepto(Convert.ToInt32(sucursalesCoberturaVista.BusquedaDepartamentos.Selected)));
            CargarSucursales(sucursalesCoberturaVista, _sucursalRepositorio.GetSucursales());
            
            var pager = new Pager(_localidadRepositorio.GetSucursalesforLocalidadCount(),
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexSucursalCobertura", _aut.GetUrl("IndexPager", "SucursalCobertura"));

            sucursalesCoberturaVista.Pager = pager;
            sucursalesCoberturaVista.ListaSucursalesCobertura = _localidadRepositorio.GetSucursalesforLocalidad(pager.Skip, pager.PageSize);

            return sucursalesCoberturaVista;
        }

        #region Privates
       
        private static void CargarLocalidades(ISucursalesCoberturaVista vista, IEnumerable<ILocalidad> listaLocalidades)
        {
            vista.BusquedaLocalidades.Combo.Add(new ComboItem { Id = 0, Description = "Seleccione una Localidad..." });
            foreach (var lLocalidad in listaLocalidades)
            {
                vista.BusquedaLocalidades.Combo.Add(new ComboItem
                {
                    Id = lLocalidad.IdLocalidad,
                    Description = lLocalidad.NombreLocalidad
                });
            }
        }

        private static void CargarSucursales(ISucursalesCoberturaVista vista, IEnumerable<ISucursal> listaSucursales)
        {
            vista.BusquedaSucursales.Combo.Add(new ComboItem { Id = 0, Description = "Seleccione una Sucursal..." });
            foreach (var lSucursal in listaSucursales)
            {
                vista.BusquedaSucursales.Combo.Add(new ComboItem
                {
                    Id = lSucursal.IdSucursal,
                    Description = lSucursal.CodigoBanco + " - " + lSucursal.Detalle
                });
            }
        }

        private static void CargarDepartamentos(ISucursalesCoberturaVista vista, IEnumerable<IDepartamento> listaDepartamentos)
        {
            vista.BusquedaDepartamentos.Combo.Add(new ComboItem { Id = 0, Description = "Seleccione un Departamento..." });
            foreach (var lDepartamento in listaDepartamentos)
            {
                vista.BusquedaDepartamentos.Combo.Add(new ComboItem
                {
                    Id = lDepartamento.IdDepartamento,
                    Description = lDepartamento.NombreDepartamento
                });
            }
        }

        private static void CargarLocalidad(ISucursalCoberturaVista vista, IEnumerable<ILocalidad> listaLocalidades)
        {
            vista.Localidades.Combo.Add(new ComboItem { Id = 0, Description = "Seleccione una localidad..." });
            foreach (var lLocalidad in listaLocalidades)
            {
                vista.Localidades.Combo.Add(new ComboItem
                {
                    Id = lLocalidad.IdLocalidad,
                    Description = lLocalidad.NombreLocalidad
                });
            }
        }

        private static void CargarSucursal(ISucursalCoberturaVista vista, IEnumerable<ISucursal> listaSucursales)
        {
            vista.Sucursales.Combo.Add(new ComboItem { Id = 0, Description = "Seleccione una Sucursal..." });
            foreach (var lSucursal in listaSucursales)
            {
                vista.Sucursales.Combo.Add(new ComboItem
                {
                    Id = lSucursal.IdSucursal,
                    Description = lSucursal.CodigoBanco + " - " + lSucursal.Detalle
                });
            }
        }

        #endregion


    }
}
