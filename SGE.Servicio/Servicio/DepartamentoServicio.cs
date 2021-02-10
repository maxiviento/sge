using System;
using System.Configuration;
using System.Linq;
using SGE.Model.Entidades.Interfaces;
using SGE.Repositorio;
using SGE.Servicio.ServicioInterfaces;
using SGE.Servicio.Vista;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces;
using SGE.Model.Entidades;

namespace SGE.Servicio.Servicio
{
    public class DepartamentoServicio : IDepartamentoServicio
    {
        private readonly DepartamentoRepositorio _departamentoRepositorio;
        private readonly IAutenticacionServicio _aut;

        public DepartamentoServicio()
        {
            _departamentoRepositorio = new DepartamentoRepositorio();
            _aut = new AutenticacionServicio();
        }

        public IDepartamentosVista GetDepartamentos()
        {
            IDepartamentosVista vista = new DepartamentosVista();

            var pager = new Pager(_departamentoRepositorio.GetDepartamentosCount(),
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexDepartamento", _aut.GetUrl("IndexPager", "Departamentos"));

            vista.Pager = pager;

            vista.Departamentos = _departamentoRepositorio.GetDepartamentos(pager.Skip, pager.PageSize);

            return vista;
        }

        public IDepartamentosVista GetIndex()
        {
            IDepartamentosVista vista = new DepartamentosVista();

            var pager = new Pager(0,
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexDepartamento", _aut.GetUrl("IndexPager", "Departamentos"));

            vista.Pager = pager;

            return vista;
        }

        public IDepartamentosVista GetDepartamentos(string descripcion)
        {
            IDepartamentosVista vista = new DepartamentosVista();

            var pager = new Pager(_departamentoRepositorio.GetDepartamentos(descripcion).Count(),
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexDepartamento", _aut.GetUrl("IndexPager", "Departamentos"));

            vista.Pager = pager;

            vista.Departamentos = _departamentoRepositorio.GetDepartamentos(descripcion, pager.Skip, pager.PageSize);

            return vista;
        }

        public IDepartamentoVista GetDepartamento(int idDepartamento, string accion)
        {
            IDepartamentoVista vista = new DepartamentoVista {Accion = accion};

            if (idDepartamento >= 0)
            {
                IDepartamento objreturn = _departamentoRepositorio.GetDepartamento(idDepartamento);

                vista.Id = objreturn.IdDepartamento;
                vista.Descripcion = objreturn.NombreDepartamento;
            }

            return vista;
        }

        public IDepartamentosVista GetDepartamentos(Pager pPager, int id, string descripcion)
        {
            IDepartamentosVista vista = new DepartamentosVista();

            var pager = new Pager(_departamentoRepositorio.GetDepartamentos(descripcion).Count(),
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexDepartamento", _aut.GetUrl("IndexPager", "Departamentos"));

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

            vista.Departamentos = _departamentoRepositorio.GetDepartamentos(descripcion, vista.Pager.Skip, vista.Pager.PageSize);
            vista.Descripcion = descripcion;

            return vista;
        }

        private static IDepartamento VistaToDatos(IDepartamentoVista vista)
        {
            IDepartamento depto = new Departamento
            {
                IdDepartamento = vista.Id,
                NombreDepartamento = vista.Descripcion,
            };

            return depto;
        }

        public int AddDepartamento(IDepartamentoVista vista)
        {
           
           if (_departamentoRepositorio.ExistsDepartamento(vista.Descripcion))
           {
               return 1;
           }

           vista.Id = _departamentoRepositorio.AddDepartamento(VistaToDatos(vista));
           
           return 0;
            
        }

        public int UpdateDepartamento(IDepartamentoVista vista)
        {
            
           _departamentoRepositorio.UpdateDepartamento(VistaToDatos(vista));

           return 0;
            
        }

        public int DeleteDepartamento(IDepartamentoVista vista)
        {
            
                _departamentoRepositorio.DeleteDepartamento(VistaToDatos(vista));

                return 0;
           
        }
    }
}
