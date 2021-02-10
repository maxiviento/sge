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
    public class TituloServicio : ITituloServicio
    {
        private readonly TituloRepositorio _tituloRepositorio;
        private readonly IAutenticacionServicio _aut;

        public TituloServicio()
        {
            _tituloRepositorio = new TituloRepositorio();
            _aut = new AutenticacionServicio();
        }

        public ITitulosVista GetTitulos()
        {
            ITitulosVista vista = new TitulosVista();

            var pager = new Pager(_tituloRepositorio.GetTituloCount(),
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexTitulo", _aut.GetUrl("IndexPager", "Titulos"));

            vista.Pager = pager;

            vista.Titulos = _tituloRepositorio.GetTitulos(pager.Skip, pager.PageSize);

            return vista;
        }

        public ITitulosVista GetTitulos(string descripcion)
        {
            ITitulosVista vista = new TitulosVista();

            var pager = new Pager(_tituloRepositorio.GetTitulos(descripcion).Count(),
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexTitulo", _aut.GetUrl("IndexPager", "Titulos"));

            vista.Pager = pager;

            vista.Titulos = _tituloRepositorio.GetTitulos(descripcion, pager.Skip, pager.PageSize);

            return vista;
        }

        public ITituloVista GetTitulo(int idTitulo, string accion)
        {
            ITituloVista vista = new TituloVista() { Accion = accion };

            if (idTitulo >= 0)
            {
                ITitulo objreturn = _tituloRepositorio.GetTitulo(idTitulo);

                vista.Id = objreturn.IdTitulo;
                vista.Descripcion = objreturn.Descripcion;
                vista.IdUniversidad = Convert.ToInt16(objreturn.IdUniversidad);
                vista.Universidades.Selected = objreturn.IdUniversidad.ToString();
            }

            return vista;
        }

        public ITitulosVista GetTitulos(Pager pPager, int id, string descripcion)
        {
            ITitulosVista vista = new TitulosVista();

            var pager = new Pager(_tituloRepositorio.GetTitulos(descripcion).Count(),
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexTitulo", _aut.GetUrl("IndexPager", "Titulos"));

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

            vista.Titulos = _tituloRepositorio.GetTitulos(descripcion, vista.Pager.Skip, vista.Pager.PageSize);
            vista.BusquedaPorDescripcion = descripcion;

            return vista;
        }

        private static ITitulo VistaToDatos(ITituloVista vista)
        {
            ITitulo titulo = new Titulo()
            {
                IdTitulo = Convert.ToInt16(vista.Id),
                Descripcion= vista.Descripcion,
            };

            return titulo;
        }

        public int AddTitulo(ITituloVista vista)
        {
            if (_tituloRepositorio.ExistsTitulo(vista.Descripcion))
            {
                return 1;
            }

            vista.Id = _tituloRepositorio.AddTitulo(VistaToDatos(vista));

            return 0;
        }

        public int UpdateTitulo(ITituloVista vista)
        {
            _tituloRepositorio.UpdateTitulo(VistaToDatos(vista));

            return 0;
        }

        public int DeleteTitulo(ITituloVista vista)
        {
            _tituloRepositorio.DeleteTitulo(VistaToDatos(vista));

            return 0;
        }
    }
}
