using System;
using System.Collections.Generic;
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
    public class OngServicio : IOngServicio
    {
        private readonly IOngRepositorio _ongRepositorio;

        private readonly IActorRepositorio _ActoresRepositorio;
        private readonly IAutenticacionServicio _autenticacion;
        //private readonly ILocalidadRepositorio _localidadRepositorio;
        //private readonly IDepartamentoRepositorio _departamentoRepositorio;

        public OngServicio()
        {
            _ongRepositorio = new OngRepositorio();
            _autenticacion = new AutenticacionServicio();
           
            //_localidadRepositorio = new LocalidadRepositorio();
            //_departamentoRepositorio = new DepartamentoRepositorio();
      
        }

        public IList<IOng> getOngs(string cuit)
        {
            return _ongRepositorio.GetOngs(cuit).ToList();
        
        }



        public IOng GetOng(int idOng)
        {
            return _ongRepositorio.GetOng(idOng);
        
        }

        public IList<IOng> getOngsNom(string nombre)
        {
            return _ongRepositorio.GetOngsNom(nombre).ToList();

        }

        public IList<IOng> GetOngsId(int id)
        {
            return _ongRepositorio.GetOngsId(id).ToList();

        }

        public IOngsVista GetOngs(string NombreOng)
        {
            int cantreg = 0;
            IOngsVista vista = new OngsVista();

            vista.ongs = _ongRepositorio.GetOngsNom(NombreOng); ;

            cantreg = vista.ongs.Count();
            var pager = new Pager(cantreg,
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexOng", _autenticacion.GetUrl("IndexPager", "Ong"));

            vista.Pager = pager;

            vista.ongs = vista.ongs.Skip(vista.Pager.Skip).Take(vista.Pager.PageSize).ToList();

            return vista;

        }


        public IOngsVista GetIndex()
        {
            IOngsVista vista = new OngsVista();

            var pager = new Pager(0,
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexOng", _autenticacion.GetUrl("IndexPager", "Ong"));

            vista.Pager = pager;


            return vista;
        }

        public IOngsVista GetOngs(IPager pPager, string NombreOng)
        {

            IOngsVista vista = new OngsVista();

            int cantreg = 0;

            vista.ongs = _ongRepositorio.GetOngsNom(NombreOng);
            cantreg = vista.ongs.Count();
            var pager = new Pager(cantreg,
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexOng", _autenticacion.GetUrl("IndexPager", "Ong"));

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



            vista.ongs = vista.ongs.Skip(vista.Pager.Skip).Take(vista.Pager.PageSize).ToList();



            return vista;
        }


        public IOngVista GetOng(int idOng, string accion)
        {

            IOngVista vista = new OngVista();
            IOng ong;
            vista.Accion = accion;




            if (accion != "Agregar")
            {
                ong = _ongRepositorio.GetOng(idOng);
                vista.ID_ONG = ong.ID_ONG;
                vista.N_NOMBRE = ong.N_NOMBRE;
                vista.CUIT = ong.CUIT;
                vista.CELULAR = ong.CELULAR;
                vista.TELEFONO = ong.TELEFONO;
                vista.MAIL_ONG = ong.MAIL_ONG;
                vista.ID_RESPONSABLE = ong.ID_RESPONSABLE;
                vista.ID_CONTACTO = ong.ID_CONTACTO;
                vista.FACTURACION = ong.FACTURACION;
                vista.RES_APELLIDO = ong.RES_APELLIDO;
                vista.RES_NOMBRE = ong.RES_NOMBRE;
                vista.RES_DNI = ong.RES_DNI;
                vista.RES_CUIL = ong.RES_CUIL;
                vista.RES_CELULAR = ong.RES_CELULAR;
                vista.RES_TELEFONO = ong.RES_TELEFONO;
                vista.RES_MAIL = ong.RES_MAIL;
                vista.RES_ID_LOCALIDAD = ong.RES_ID_LOCALIDAD;
                vista.RES_N_LOCALIDAD = ong.RES_N_LOCALIDAD;
                vista.CON_APELLIDO = ong.CON_APELLIDO;
                vista.CON_NOMBRE = ong.CON_NOMBRE;
                vista.CON_DNI = ong.CON_DNI;
                vista.CON_CUIL = ong.CON_CUIL;
                vista.CON_CELULAR = ong.CON_CELULAR;
                vista.CON_TELEFONO = ong.CON_TELEFONO;
                vista.CON_MAIL = ong.CON_MAIL;
                vista.CON_ID_LOCALIDAD = ong.CON_ID_LOCALIDAD;
                vista.CON_N_LOCALIDAD = ong.CON_N_LOCALIDAD;


                
            }
            else
            {
                ong = new Ong();

                vista.N_NOMBRE = ong.N_NOMBRE;
                vista.ID_CONTACTO = 0;
                vista.ID_RESPONSABLE = 0;
                vista.ID_ONG = 0;


            }



            if (accion == "Ver" || accion == "Eliminar")
            {

            }

            return vista;
        }



        public IOngVista GetOngReload(IOngVista vista)
        {
            IOng ong;

            //ong = _ongRepositorio.GetOng(vista.ID_ONG);
            //vista.ID_ONG = ong.ID_ONG;
            //vista.N_NOMBRE = ong.N_NOMBRE;
            //vista.CUIT = ong.CUIT;
            //vista.CELULAR = ong.CELULAR;
            //vista.TELEFONO = ong.TELEFONO;
            //vista.MAIL_ONG = ong.MAIL_ONG;
            //vista.ID_RESPONSABLE = ong.ID_RESPONSABLE;
            //vista.ID_CONTACTO = ong.ID_CONTACTO;
            //vista.FACTURACION = ong.FACTURACION;
            //vista.RES_APELLIDO = ong.RES_APELLIDO;
            //vista.RES_NOMBRE = ong.RES_NOMBRE;
            //vista.RES_DNI = ong.RES_DNI;
            //vista.RES_CUIL = ong.RES_CUIL;
            //vista.RES_CELULAR = ong.RES_CELULAR;
            //vista.RES_TELEFONO = ong.RES_TELEFONO;
            //vista.RES_MAIL = ong.RES_MAIL;
            //vista.RES_ID_LOCALIDAD = ong.RES_ID_LOCALIDAD;
            //vista.RES_N_LOCALIDAD = ong.RES_N_LOCALIDAD;
            //vista.CON_APELLIDO = ong.CON_APELLIDO;
            //vista.CON_NOMBRE = ong.CON_NOMBRE;
            //vista.CON_DNI = ong.CON_DNI;
            //vista.CON_CUIL = ong.CON_CUIL;
            //vista.CON_CELULAR = ong.CON_CELULAR;
            //vista.CON_TELEFONO = ong.CON_TELEFONO;
            //vista.CON_MAIL = ong.CON_MAIL;
            //vista.CON_ID_LOCALIDAD = ong.CON_ID_LOCALIDAD;
            //vista.CON_N_LOCALIDAD = ong.CON_N_LOCALIDAD;
            
            



            return vista;
        }



        public int AddOng(IOngVista vista)
        {


            vista.ID_ONG = _ongRepositorio.AddOng(VistaToDatos(vista));

            return vista.ID_ONG;
        }

        private static IOng VistaToDatos(IOngVista vista)
        {
            IOng ong = new Ong
            {
                ID_ONG = vista.ID_ONG,
                N_NOMBRE = vista.N_NOMBRE,
                CUIT = vista.CUIT,
                CELULAR = vista.CELULAR,
                TELEFONO = vista.TELEFONO,
                FACTURACION = vista.FACTURACION ?? "N",
                MAIL_ONG = vista.MAIL_ONG,
                ID_CONTACTO = vista.ID_CONTACTO == 0 ? null : vista.ID_CONTACTO,
                ID_RESPONSABLE = vista.ID_RESPONSABLE == 0 ? null : vista.ID_RESPONSABLE,

            };

            return ong;
        }

        public int UpdateOng(IOngVista vista)
        {

            _ongRepositorio.UpdateOng(VistaToDatos(vista));

            return 0;

        }


    }
}
