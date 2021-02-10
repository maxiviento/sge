using System;
using System.Configuration;
using System.Linq;
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
    public class UniversidadServicio : IUniversidadServicio
    {
        private readonly IUniversidadRepositorio _universidadRepositorio;
        private readonly IAutenticacionServicio _autenticacion;

        public UniversidadServicio()
        { 
            _universidadRepositorio=new UniversidadRepositorio();
            _autenticacion = new AutenticacionServicio();
        }

        public IUniversidadesVista GetUniversidades()
        {
            IUniversidadesVista vista = new UniversidadesVista();

            var pager = new Pager(_universidadRepositorio.GetUniversidadesCount(),
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexUniversidad", _autenticacion.GetUrl("IndexPager", "Universidades"));

            vista.Pager = pager;

            vista.Universidades = _universidadRepositorio.GetUniversidades(pager.Skip, pager.PageSize);

            return vista;
        }

        public IUniversidadesVista GetIndex()
        {
            IUniversidadesVista vista = new UniversidadesVista();

            var pager = new Pager(0,
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexUniversidad", _autenticacion.GetUrl("IndexPager", "Universidades"));

            vista.Pager = pager;

            return vista;
        }

        public IUniversidadesVista GetUniversidades(string descripcion)
        {
            IUniversidadesVista vista = new UniversidadesVista();

            var pager = new Pager(_universidadRepositorio.GetUniversidades(descripcion).Count(),
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexUniversidad", _autenticacion.GetUrl("IndexPager", "Universidades"));

            vista.Pager = pager;

            vista.Universidades= _universidadRepositorio.GetUniversidades( descripcion, pager.Skip, pager.PageSize);

            return vista;
        }

        public IUniversidadVista GetUniversidad(int idUniversidad, string accion)
        {
            IUniversidadVista vista = new UniversidadVista {Accion = accion};

            if (idUniversidad > 0)
            {
                IUniversidad row = _universidadRepositorio.GetUniversidad(idUniversidad);

                vista.Id = row.IdUniversidad;
                vista.Descripcion = row.NombreUniversidad;
            }

            return vista;
        }

        public IUniversidadesVista GetUniversidades(Pager pPager, int id, string descripcion)
        {
            IUniversidadesVista vista = new UniversidadesVista();

            var pager = new Pager(_universidadRepositorio.GetUniversidades(descripcion).Count(), 
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexUniversidad", _autenticacion.GetUrl("IndexPager", "Universidades"));

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

            vista.Universidades = _universidadRepositorio.GetUniversidades(descripcion, vista.Pager.Skip, vista.Pager.PageSize);
            vista.DescripcionBusca = descripcion;

            return vista;
        }

        public int AddUniversidad(IUniversidadVista vista)
        {
            if (_universidadRepositorio.GetUniversidades(vista.Descripcion).Count()>0)
            {
                return 1;
            }

            vista.Id = _universidadRepositorio.AddUniversidad(VistaToDatos(vista));

            return 0;
        }

        public IUniversidadVista AddUniversidadLast(string accion)
        {
            IUniversidadVista vista = new UniversidadVista
                                          {
                                              Id = _universidadRepositorio.GetUniversidadMaxId() + 1,
                                              Accion = accion
                                          };
            return vista;
        }

        private static IUniversidad VistaToDatos(IUniversidadVista vista)
        {
            IUniversidad universidad = new Universidad
                               {
                                   IdUniversidad = vista.Id,
                                   NombreUniversidad = vista.Descripcion,
                               };
            return universidad;
        }

        public int UpdateUniversidad(IUniversidadVista vista)
        {
          
                _universidadRepositorio.UpdateUniversidad(VistaToDatos(vista));

                return 0;
           
        }
    }
}
