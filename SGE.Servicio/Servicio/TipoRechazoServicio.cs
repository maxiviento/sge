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
    public class TipoRechazoServicio : ITipoRechazoServicio
    {
        private readonly ITipoRechazoRepositorio _tipoRechazoRepositorio;
        private readonly IAutenticacionServicio _autenticacion;

        public TipoRechazoServicio()
        {
            _tipoRechazoRepositorio = new TipoRechazoRepositorio();
            _autenticacion = new AutenticacionServicio();
        }

        public ITiposRechazoVista GetTiposRechazo()
        {
            ITiposRechazoVista vista = new TiposRechazoVista();

            var pager = new Pager(_tipoRechazoRepositorio.GetTiposRechazoCount(),
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexTipoRechazo", _autenticacion.GetUrl("IndexPager", "TiposRechazo"));

            vista.Pager = pager;

            vista.TiposRechazo = _tipoRechazoRepositorio.GetTiposRechazo(pager.Skip, pager.PageSize);
            
            return vista;
        }

        public ITiposRechazoVista GetIndex()
        {
            ITiposRechazoVista vista = new TiposRechazoVista();

            var pager = new Pager(0,
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexTipoRechazo", _autenticacion.GetUrl("IndexPager", "TiposRechazo"));

            vista.Pager = pager;

            return vista;
        }

        public ITiposRechazoVista GetTiposRechazo(string descripcion)
        {
            ITiposRechazoVista vista = new TiposRechazoVista();

            var pager = new Pager(_tipoRechazoRepositorio.GetTiposRechazo(descripcion).Count(),
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexTipoRechazo", _autenticacion.GetUrl("IndexPager", "TiposRechazo"));

            vista.Pager = pager;

            vista.TiposRechazo = _tipoRechazoRepositorio.GetTiposRechazo(descripcion, pager.Skip, pager.PageSize);

            return vista;
        }

        public ITipoRechazoVista GetTipoRechazo(int idTipoRechazo, string accion)
        {
            ITipoRechazoVista vista = new TipoRechazoVista { Accion = accion };

            if (idTipoRechazo >= 0)
            {
                var row = _tipoRechazoRepositorio.GetTipoRechazo(idTipoRechazo);

                vista.Id = row.IdTipoRechazo;
                vista.Descripcion = row.NombreTipoRechazo;
            }

            return vista;
        }

        public ITiposRechazoVista GetTiposRechazo(Pager pPager, int id, string descripcion)
        {
            ITiposRechazoVista vista = new TiposRechazoVista();

            var pager = new Pager(_tipoRechazoRepositorio.GetTiposRechazo(descripcion).Count(),
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexTipoRechazo", _autenticacion.GetUrl("IndexPager", "TiposRechazo"));

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

            vista.TiposRechazo = _tipoRechazoRepositorio.GetTiposRechazo(descripcion, vista.Pager.Skip, vista.Pager.PageSize);

            vista.DescripcionBusca = descripcion;

            return vista;
        }

        public int AddTipoRechazo(ITipoRechazoVista vista)
        {
            if (_tipoRechazoRepositorio.ExistsTipoRechazo(vista.Descripcion) || _tipoRechazoRepositorio.GetTipoRechazo(vista.Id)!=null)
            {
                return 1;
            }

            vista.Id = _tipoRechazoRepositorio.AddTipoRechazo(VistaToDatos(vista));

            return 0;
        }

        private static ITipoRechazo VistaToDatos(ITipoRechazoVista vista)
        {
            ITipoRechazo row = new TipoRechazo
                               {
                                   IdTipoRechazo = vista.Id,
                                   NombreTipoRechazo = vista.Descripcion,
                               };
            return row;
        }

        public int UpdateTipoRechazo(ITipoRechazoVista vista)
        {
                if(!_tipoRechazoRepositorio.UpdateTipoRechazo(vista.IdOld, VistaToDatos(vista))) 
                    return 1;

                return 0;
        }

        public bool TipoRechazoInUse(int idTipoRechazo)
        {
            return _tipoRechazoRepositorio.TipoRechazoInUse(idTipoRechazo);
        }

        public ITipoRechazoVista AddTipoRechazoLast(string accion)
        {
            ITipoRechazoVista vista = new TipoRechazoVista
                                           {
                                               Id = _tipoRechazoRepositorio.GetTipoRechazoMaxId() + 1,
                                               Accion = accion
                                           };
            return vista;
        }

        public int DeleteTipoRechazo(ITipoRechazoVista vista)
        {
            
                _tipoRechazoRepositorio.DeleteTipoRechazo(VistaToDatos(vista));
                return 0;
          
        }
    }
}
