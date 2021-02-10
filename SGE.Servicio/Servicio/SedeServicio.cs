using System;
using System.Collections.Generic;
using System.Configuration;
using SGE.Model.Entidades.Interfaces;
using SGE.Repositorio;
using SGE.Servicio.ServicioInterfaces;
using SGE.Servicio.Vista;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces;
using SGE.Model.Entidades;
using System.Linq;

namespace SGE.Servicio.Servicio
{
    public class SedeServicio : ISedeServicio
    {
        private readonly SedeRepositorio _sedeRepositorio;
        private readonly EmpresaRepositorio _empresaRepositorio;
        private readonly LocalidadRepositorio _localidadrepositorio;
        private readonly EmpresaServicio _empresaServicio;
        private readonly IAutenticacionServicio _aut;

        public SedeServicio()
        {
            _sedeRepositorio = new SedeRepositorio();
            _empresaRepositorio = new EmpresaRepositorio();
            _localidadrepositorio = new LocalidadRepositorio();
            _empresaServicio=new EmpresaServicio();
            _aut = new AutenticacionServicio();
        }

        public ISedesVista GetSedes()
        {
            ISedesVista vista = new SedesVista();

            var pager = new Pager(_sedeRepositorio.GetSedesCount(),
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexSede", _aut.GetUrl("IndexPager", "Sede"));

            vista.Pager = pager;

            vista.Sedes = _sedeRepositorio.GetSedes(pager.Skip, pager.PageSize);
                
            return vista;
        }

        public ISedeVista GetSede(int idSede, string accion)
        {
            ISedeVista vista = new SedeVista {Accion = accion};

            if (idSede > 0)
            {
                ISede objreturn = _sedeRepositorio.GetSedes(idSede);

                vista.Id = objreturn.IdSede;
                vista.Descripcion = objreturn.NombreSede;

                CargarEmpresas(vista, _empresaRepositorio.GetEmpresas());

                if (objreturn.IdEmpresa != null) vista.IdEmpresa = objreturn.IdEmpresa.Value;

                CargarLocalidades(vista, _localidadrepositorio.GetLocalidades());
                vista.Localidades.Selected = objreturn.IdLocalidad.ToString();

                vista.Calle = objreturn.Calle;
                vista.Numero = objreturn.Numero;
                vista.Piso = objreturn.Piso;
                vista.Dpto = objreturn.Dpto;
                vista.CodigoPostal = objreturn.CodigoPostal;
                   
                vista.ApellidoContacto = objreturn.ApellidoContacto;
                vista.NombreContacto = objreturn.NombreContacto;
                vista.Telefono = objreturn.Telefono;
                vista.Fax = objreturn.Fax;
                vista.Email = objreturn.Email;

                vista.Accion = accion;
                if (accion.Equals("Modificar") || (accion.Equals("Agregar")))
                {
                    vista.Localidades.Enabled = true;
                }
                else if (accion.Equals("Ver") || accion.Equals("Eliminar"))
                {
                    vista.Localidades.Enabled = false;
                }
            }
            else
            {
                CargarLocalidades(vista, _localidadrepositorio.GetLocalidades());
            }

            return vista;
        }

        private static void CargarLocalidades(ISedeVista vista, IEnumerable<ILocalidad> listaLocalidades)
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

        private static void CargarEmpresas(ISedeVista vista, IEnumerable<IEmpresa> listaEmpresas)
        {
            vista.Empresas.Combo.Add(new ComboItem { Id = 0, Description = "Seleccione una empresa..." });
            foreach (var lEmpresas in listaEmpresas)
            {
                vista.Empresas.Combo.Add(new ComboItem
                                             {
                    Id = lEmpresas.IdEmpresa,
                    Description = lEmpresas.NombreEmpresa
                });
            }
        }        

        public ISedesVista GetSedes(string descripcion, int? idEmpresa)
        {
            ISedesVista vista = new SedesVista();

            var pager = new Pager(_sedeRepositorio.GetSedesCount(descripcion, idEmpresa),
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexSede", _aut.GetUrl("IndexPager", "Sede"));

            vista.Pager = pager;

            vista.Sedes = _sedeRepositorio.GetSedes(descripcion, idEmpresa, pager.Skip, pager.PageSize);

            //vista.Busqueda_Descripcion = Descripcion;

            return vista;
        }

        public ISedesVista GetSedes(Pager pPager, string descripcion, int? idEmpresa)
        {
            ISedesVista vista = new SedesVista();

            var pager = new Pager(_sedeRepositorio.GetSedesCount(descripcion, idEmpresa),
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexSede", _aut.GetUrl("IndexPager", "Sede"));

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

            vista.Sedes = _sedeRepositorio.GetSedes(descripcion, idEmpresa, vista.Pager.Skip, vista.Pager.PageSize);
            vista.BusquedaDescripcion = descripcion;
                
            return vista;
        }

        public IEmpresaVista GetSedesByEmpresa(IEmpresaVista model)
        {
            IEmpresaVista empresaVista = _empresaServicio.GetEmpresa(model.Id, model.Accion);
            empresaVista.Sedes = _sedeRepositorio.GetSedes(model.BusquedaSede, model.Id);
            empresaVista.TabSeleccionado = "3";
            empresaVista.BusquedaSede = model.BusquedaSede;
            return empresaVista;
        }

        private static ISede VistaToDatos(ISedeVista vista)
        {
            int? loc = null;

            if (vista.Localidades.Selected != "0")
            {
                loc = Convert.ToInt32(vista.Localidades.Selected);
            }

            ISede sede = new Sede
            {
                IdSede = vista.Id,
                NombreSede = vista.Descripcion,
                IdEmpresa = vista.IdEmpresa,//emp,
                IdLocalidad = loc,
                Calle = vista.Calle,
                Numero = vista.Numero,
                Piso = vista.Piso,
                Dpto = vista.Dpto,
                CodigoPostal = vista.CodigoPostal,
                ApellidoContacto = vista.ApellidoContacto,
                NombreContacto = vista.NombreContacto,
                Telefono = vista.Telefono,
                Fax = vista.Fax,
                Email = vista.Email
            };

            return sede;
        }

        public int AddSede(ISedeVista vista)
        {
            
                if (_sedeRepositorio.ExistsSede(vista.Id, vista.Descripcion))
                {
                    CargarLocalidades(vista, _localidadrepositorio.GetLocalidades());
                    CargarEmpresas(vista, _empresaRepositorio.GetEmpresas());
                    return 1;
                }

                vista.Id = _sedeRepositorio.AddSede(VistaToDatos(vista));

                return 0;
            
            
        }

        public int UpdateSede(ISedeVista vista)
        {
             
                 if (_sedeRepositorio.ExistsSede(vista.Id, vista.Descripcion))
                 {
                     CargarLocalidades(vista, _localidadrepositorio.GetLocalidades());
                     CargarEmpresas(vista, _empresaRepositorio.GetEmpresas());

                     return 1;
                 }

                 _sedeRepositorio.UpdateSede(VistaToDatos(vista));

                 return 0;
            
        }

        public int DeleteSede(ISedeVista vista)
        {
               if (_sedeRepositorio.SedeTieneFichas(vista.Id))
                {
                    return 1;
                }

                _sedeRepositorio.DeleteSede(VistaToDatos(vista));

                return 0;
            
        }

        public bool ExistsSede(int idSede, string descripcion)
        {
            return _sedeRepositorio.ExistsSede(idSede, descripcion);
        }

        public bool SedeTieneFichas(int idSede)
        {
            return _sedeRepositorio.SedeTieneFichas(idSede);
        }

        public ISedesVista GetSedesByEmpresa(int idEmpresa)
        {
            ISedesVista vista = new SedesVista
                                    {
                                        Sedes =
                                            _sedeRepositorio.GetSedesByEmpresa(idEmpresa).OrderBy(c => c.IdSede).ToList()
                                    };

            return vista;
        }

        public int AddSedeFromInscripcion(ISedeVista sede)
        {
            if (_sedeRepositorio.ExistsSedeByEmpresa(sede.IdEmpresa, sede.Descripcion))
            {
                return 1;
            }

            sede.Id = _sedeRepositorio.AddSede(VistaToDatos(sede));

            return 0;
        }
    }
}
