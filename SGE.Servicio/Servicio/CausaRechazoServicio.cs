using System;
using System.Configuration;
using System.Linq;
using SGE.Model.Entidades;
using SGE.Model.Entidades.Interfaces;
using SGE.Repositorio;
using SGE.Servicio.ServicioInterfaces;
using SGE.Servicio.Vista;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces;

namespace SGE.Servicio.Servicio
{
    public class CausaRechazoServicio : ICausaRechazoServicio
    {
        private readonly CausaRechazoRepositorio _causaRechazoRepositorio;
        private readonly IAutenticacionServicio _autenticacion;

        public CausaRechazoServicio()
        {
            _causaRechazoRepositorio = new CausaRechazoRepositorio();
            _autenticacion = new AutenticacionServicio();
        }

        public ICausasRechazoVista GetCausasRechazo(ICausasRechazoVista vista)
        {
            var pager = new Pager(_causaRechazoRepositorio.GetCausasRechazoCount(),
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexCausaRechazo", _autenticacion.GetUrl("IndexPager", "CausasRechazo"));

            vista.Pager = pager;

            vista.CausasRechazo = _causaRechazoRepositorio.GetCausasRechazo(pager.Skip, pager.PageSize);

            return vista;
        }

        public ICausasRechazoVista GetIndex()
        {
            ICausasRechazoVista vista = new CausasRechazoVista();
            
            var pager = new Pager(0,
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexCausaRechazo", _autenticacion.GetUrl("IndexPager", "CausasRechazo"));

            vista.Pager = pager;

            return vista;
        }

        public ICausasRechazoVista GetCausasRechazo(string descripcion)
        {
            ICausasRechazoVista vista = new CausasRechazoVista();

            var pager = new Pager(_causaRechazoRepositorio.GetCausasRechazo(descripcion).Count(),
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexCausaRechazo", _autenticacion.GetUrl("IndexPager", "CausasRechazo"));

            vista.Pager = pager;

            vista.CausasRechazo = _causaRechazoRepositorio.GetCausasRechazo(descripcion, pager.Skip, pager.PageSize);

            return vista;
        }

        public ICausaRechazoVista GetCausaRechazo(int idCausaRechazo, string accion)
        {
            ICausaRechazoVista vista = new CausaRechazoVista();
            vista.Accion = accion;

            if (idCausaRechazo > 0)
            {
                ICausaRechazo causaRechazo = _causaRechazoRepositorio.GetCausaRechazo(idCausaRechazo);

                vista.Id = causaRechazo.IdCausaRechazo;
                vista.Descripcion = causaRechazo.NombreCausaRechazo;
            }

            return vista;
        }

        public ICausasRechazoVista GetCausasRechazo(Pager pPager, int id, string descripcion)
        {
            ICausasRechazoVista vista = new CausasRechazoVista();

            var pager = new Pager(_causaRechazoRepositorio.GetCausasRechazo(descripcion).Count(),
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexCausaRechazo", _autenticacion.GetUrl("IndexPager", "CausasRechazo"));

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

            vista.CausasRechazo = _causaRechazoRepositorio.GetCausasRechazo(descripcion, vista.Pager.Skip, vista.Pager.PageSize);
            vista.BuscarDescripcion = descripcion;

            return vista;
        }

        public int AddCausaRechazo(ICausaRechazoVista vista)
        {
            if (_causaRechazoRepositorio.ExistsCausaRechazo(vista.Descripcion) || _causaRechazoRepositorio.GetCausaRechazo(vista.Id)!=null)
            {
                return 1;
            }

            vista.Id = _causaRechazoRepositorio.AddCausaRechazo(VistaToDatos(vista));

            return 0;
        }

        private static ICausaRechazo VistaToDatos(ICausaRechazoVista vista)
        {
            ICausaRechazo causaRechazo = new CausaRechazo
                               {
                                   IdCausaRechazo = vista.Id,
                                   NombreCausaRechazo = vista.Descripcion,
                               };
            return causaRechazo;
        }

        public int UpdateCausaRechazo(ICausaRechazoVista vista)
        {
            if(!_causaRechazoRepositorio.UpdateCausaRechazo(vista.IdOld, VistaToDatos(vista))) 
                return 1;

            return 0;
        }

        public int DeleteCausaRechazo(ICausaRechazoVista causaRechazo)
        {
            
                _causaRechazoRepositorio.DeleteCausaRechazo(VistaToDatos(causaRechazo));
                return 0;   
            

        }

        public bool CausaRechazoInUse(int idCausaRechazo)
        {
            return _causaRechazoRepositorio.CausaRechazoInUse(idCausaRechazo);
        }

        public ICausaRechazoVista AddCausaRechazoLast(string accion)
        {
            ICausaRechazoVista vista = new CausaRechazoVista
                                           {
                                               Id = _causaRechazoRepositorio.GetCausaRechazoMaxId() + 1,
                                               Accion = accion
                                           };
            return vista;
        }
    }
}