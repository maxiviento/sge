using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Transactions;
using System.Web;
using NPOI.HSSF.UserModel;
using SGE.Model.Comun;
using SGE.Model.Entidades;
using SGE.Model.Entidades.Interfaces;
using SGE.Model.Repositorio;
using SGE.Repositorio;
using SGE.Servicio.ServicioInterfaces;
using SGE.Servicio.Vista;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces;
using SGE.Servicio.Comun;

namespace SGE.Servicio.Servicio
{
    public class FichaObservacionServicio :IFichaObservacionServicio
    {
        private readonly FichaObservacionRepositorio _fichaObservacionRepositorio;
        private readonly IAutenticacionServicio _aut;

        public FichaObservacionServicio()
        {
            _fichaObservacionRepositorio = new FichaObservacionRepositorio();
            _aut = new AutenticacionServicio();
        }

        public IFichaObservacionesVista GetIndex()
        {
            IFichaObservacionesVista vista = new FichaObservacionesVista();

            var pager = new Pager(0,
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexObservaciones", _aut.GetUrl("IndexPager", "FichaObservacion"));
            
            vista.Pager = pager;

            return vista;
        }

        //para delete y update
        public IFichaObservacionVista GetObservacion(int idFicha, int idFichaObservacion, string accion)
        {
            IFichaObservacionVista vista = new FichaObservacionVista() { Accion = accion };

            if (idFichaObservacion >= 0)
            {
                IFichaObservacion objreturn = _fichaObservacionRepositorio.GetObservacion(idFicha,idFichaObservacion);

                vista.IdFichaObservacion = objreturn.IdFichaObservacion;
                vista.IdFicha = objreturn.IdFicha;
                vista.Descripcion = objreturn.Observacion;
                vista.Accion = accion;
            }

            return vista;
        }
        //listado
        public IFichaObservacionesVista GetObservacionesByFicha(int idFicha)
        {
            IFichaObservacionesVista vista = new FichaObservacionesVista();

            var pager = new Pager(_fichaObservacionRepositorio.GetCountObservaciones(idFicha),
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexObservaciones", _aut.GetUrl("IndexPager", "FichaObservacion"));

            vista.Pager = pager;
            vista.IdFicha = idFicha;
            vista.Observaciones = _fichaObservacionRepositorio.GetObservacionesByFicha(idFicha, pager.Skip, pager.PageSize);

            return vista;
        }

        //listado paginado
        public IFichaObservacionesVista GetObservacionesByFicha(Pager pPager, int idFicha)
        {
            IFichaObservacionesVista vista = new FichaObservacionesVista();

            var pager = new Pager(_fichaObservacionRepositorio.GetCountObservaciones(idFicha),
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexObservaciones", _aut.GetUrl("IndexPager", "FichaObservacion"));

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

            vista.IdFicha = idFicha;
            vista.Observaciones = _fichaObservacionRepositorio.GetObservacionesByFicha(idFicha, vista.Pager.Skip, vista.Pager.PageSize);
            
            return vista;
        }

        private static IFichaObservacion VistaToDatos(IFichaObservacionVista vista)
        {
            IFichaObservacion fichaObservacion = new FichaObservacion()
            {
                IdFichaObservacion = vista.IdFichaObservacion,
                IdFicha = vista.IdFicha,
                Observacion = vista.Descripcion,
            };

            return fichaObservacion;
        }

        public int AddObservacion(IFichaObservacionVista vista)
        {
            vista.IdFichaObservacion = _fichaObservacionRepositorio.AddObservacion(VistaToDatos(vista));

            return 0;
        }

        public int UpdateObservacion(IFichaObservacionVista vista)
        {
            _fichaObservacionRepositorio.UpdateObservacion(VistaToDatos(vista));

            return 0;
        }

        public int DeleteObservacion(IFichaObservacionVista vista)
        {
            _fichaObservacionRepositorio.DeleteObservacion(VistaToDatos(vista));

            return  0;
        }
    }
}
