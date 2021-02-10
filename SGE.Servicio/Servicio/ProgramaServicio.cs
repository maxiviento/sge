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
using System.Collections.Generic;

namespace SGE.Servicio.Servicio
{
    public class ProgramaServicio : IProgramaServicio
    {
        private readonly IProgramaRepositorio _programaRepositorio;
        private readonly IAutenticacionServicio _autenticacion;

        public ProgramaServicio()
        { 
            _programaRepositorio=new ProgramaRepositorio();
            _autenticacion = new AutenticacionServicio();
        }

        public IProgramasVista GetProgramas()
        {
            IProgramasVista vista = new ProgramasVista();

            var pager = new Pager(_programaRepositorio.GetProgramasCount(),
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexPrograma", _autenticacion.GetUrl("IndexPager", "Programas"));

            vista.Pager = pager;

            vista.Programas = _programaRepositorio.GetProgramas(pager.Skip, pager.PageSize);

            return vista;
        }

        public IProgramasVista GetIndex()
        {
            IProgramasVista vista = new ProgramasVista();

            var pager = new Pager(0,
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexPrograma", _autenticacion.GetUrl("IndexPager", "Programas"));

            vista.Pager = pager;


            return vista;
        }

        public IProgramasVista GetProgramas(string descripcion)
        {
            IProgramasVista vista = new ProgramasVista();

            var pager = new Pager(_programaRepositorio.GetProgramas(descripcion).Count(),
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexPrograma", _autenticacion.GetUrl("IndexPager", "Programas"));

            vista.Pager = pager;

            vista.Programas= _programaRepositorio.GetProgramas( descripcion, pager.Skip, pager.PageSize);

            return vista;
        }

        public IProgramaVista GetPrograma(int idPrograma, string accion)
        {
            IProgramaVista vista = new ProgramaVista {Accion = accion};

            if (idPrograma > 0)
            {
                IPrograma row = _programaRepositorio.GetPrograma(idPrograma);

                vista.Id = row.IdPrograma;
                vista.Descripcion = row.NombrePrograma;
                vista.ConvenioBcoCba = row.ConvenioBcoCba;
                vista.MontoPrograma = row.MontoPrograma;
            }

            return vista;
        }

        public IProgramasVista GetProgramas(Pager pPager, int id, string descripcion)
        {
            IProgramasVista vista = new ProgramasVista();

            var pager = new Pager(_programaRepositorio.GetProgramas(descripcion).Count(), 
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexPrograma", _autenticacion.GetUrl("IndexPager", "Programas"));

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

            vista.Programas = _programaRepositorio.GetProgramas(descripcion, vista.Pager.Skip, vista.Pager.PageSize);
            vista.DescripcionBusca = descripcion;

            return vista;
        }

        public int AddPrograma(IProgramaVista vista)
        {
            if (_programaRepositorio.GetProgramas(vista.Descripcion).Count()>0)
            {
                return 1;
            }

            vista.Id = _programaRepositorio.AddPrograma(VistaToDatos(vista));

            return 0;
        }

        public IProgramaVista AddProgramaLast(string accion)
        {
            IProgramaVista vista = new ProgramaVista
                                       {
                                           Id = _programaRepositorio.GetProgramaMaxId() + 1,
                                           Accion = accion
                                       };

            return vista;
        }

        private static IPrograma VistaToDatos(IProgramaVista vista)
        {
            IPrograma programa = new Programa
                               {
                                   IdPrograma = vista.Id,
                                   NombrePrograma = vista.Descripcion,
                                   ConvenioBcoCba = vista.ConvenioBcoCba,
                                   MontoPrograma = vista.MontoPrograma};
            return programa;
        }

        public int UpdatePrograma(IProgramaVista vista)
        {
           
                _programaRepositorio.UpdatePrograma(VistaToDatos(vista));

                return 0;
           
        }

        public IList<IPrograma> GetPrograma(string programa)
        {
            return _programaRepositorio.GetProgramas(programa);
        
        }
    }
}
