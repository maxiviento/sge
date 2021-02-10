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
    public class BarrioServicio : IBarrioServicio
    {
        private readonly IBarrioRepositorio _barrioRepositorio;
        private readonly IAutenticacionServicio _autenticacion;
        private readonly ILocalidadRepositorio _localidadRepositorio;
        
       

        public BarrioServicio()
        {
            _barrioRepositorio = new BarrioRepositorio();
            _autenticacion = new AutenticacionServicio();
            _localidadRepositorio = new LocalidadRepositorio();

        }


        public IBarriosVista GetBarrios(string nombre)
        {
            int cantreg = 0;
            IBarriosVista vista = new BarriosVista();
            //IList<ICurso> cursos;

            vista.barrios = _barrioRepositorio.GetBarrios(nombre);



            cantreg = vista.barrios.Count();
            var pager = new Pager(cantreg,
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexBarrio", _autenticacion.GetUrl("IndexPager", "Barrios"));

            vista.Pager = pager;

            vista.barrios = vista.barrios.Skip(vista.Pager.Skip).Take(vista.Pager.PageSize).ToList();

            return vista;
        }

        public IBarriosVista GetIndex()
        {
            IBarriosVista vista = new BarriosVista();

            var pager = new Pager(0,
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexBarrio", _autenticacion.GetUrl("IndexPager", "Barrios"));

            vista.Pager = pager;


            return vista;
        }


        public IBarriosVista GetBarrios(IPager pPager, string nombre)
        {

            IBarriosVista vista = new BarriosVista();
            //IList<ICurso> cursos;
            int cantreg = 0;

            vista.barrios = _barrioRepositorio.GetBarrios(nombre);
            cantreg = vista.barrios.Count();
            var pager = new Pager(cantreg,
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexBarrio", _autenticacion.GetUrl("IndexPager", "Barrios"));

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



            vista.barrios = vista.barrios.Skip(vista.Pager.Skip).Take(vista.Pager.PageSize).ToList();
            
            

            return vista;
        }


        public IBarrioVista GetBarrio(int idBarrio, string accion)
        {

            IBarrioVista vista = new BarrioVista();
            IBarrio c;
            vista.Accion = accion;

            CargarLocalidad(vista, _localidadRepositorio.GetLocalidades());
            CargarSeccionales(vista, _barrioRepositorio.GetSeccionales());


            if (accion != "Agregar")
            {
                c = _barrioRepositorio.GetBarrio(idBarrio);

                                vista.ID_BARRIO = c.ID_BARRIO;
                                vista.N_BARRIO = c.N_BARRIO;
                                vista.ID_LOCALIDAD = c.ID_LOCALIDAD ?? 0;
                                vista.ID_SECCIONAL = c.ID_SECCIONAL ?? 0;
                                vista.ID_USR_SIST = c.ID_USR_SIST;
                                vista.FEC_SIST = c.FEC_SIST;
                                vista.ID_USR_MODIF = c.ID_USR_MODIF;
                                vista.FEC_MODIF = c.FEC_MODIF;

                                int idlocalidad = Convert.ToInt32(c.ID_LOCALIDAD ?? 0);
                                int idseccional = Convert.ToInt32(c.ID_SECCIONAL ?? 0);
                                vista.Localidades.Selected = idlocalidad.ToString();
                                vista.Seccionales.Selected = idseccional.ToString();
                //if (accion != "ModificarActorRol")
                //{
                // CargarActoresRol(vista, _personaRepositorio.GetActoresRol());
                //}
            }
            else
            {
                c = new Barrio();

                                vista.ID_BARRIO = 0;
                                vista.N_BARRIO = "";
                                vista.ID_LOCALIDAD =  0;
                                vista.ID_SECCIONAL =  0;
                                vista.Localidades.Selected = "0";
                                vista.Seccionales.Selected = "0";

            }



            if (accion == "Ver" || accion == "Eliminar")
            {
                vista.Localidades.Enabled = false;
                vista.Seccionales.Enabled = false;

            }

            return vista;
        }


        public int AddBarrio(IBarrioVista vista)
        {


            vista.ID_BARRIO = _barrioRepositorio.AddBarrio(VistaToDatos(vista));

            return 0;
        }

        private static IBarrio VistaToDatos(IBarrioVista vista)
        {
            IBarrio barrio = new Barrio
            {       
                    ID_BARRIO =vista.ID_BARRIO,
                    N_BARRIO = vista.N_BARRIO,
                    ID_LOCALIDAD = vista.Localidades.Selected == "0" ? 0 : Convert.ToInt32(vista.Localidades.Selected),
                    ID_SECCIONAL = vista.Seccionales.Selected == "0" ? 0 : Convert.ToInt32(vista.Seccionales.Selected),

            };

            return barrio;
        }


        public int UpdateBarrio(IBarrioVista vista)
        {

            _barrioRepositorio.UpdateBarrio(VistaToDatos(vista));

            return 0;

        }

        public IBarrioVista GetBarrioReload(IBarrioVista vista)
        {
            IBarrio c;
            string accion = vista.Accion;
            string loc = vista.Localidades.Selected;
            string sec = vista.Seccionales.Selected;
            CargarLocalidad(vista, _localidadRepositorio.GetLocalidades());
            CargarSeccionales(vista, _barrioRepositorio.GetSeccionales());

            vista.Localidades.Selected = loc;
            vista.Seccionales.Selected = sec;

            if (accion != "Agregar")
            {
                c = _barrioRepositorio.GetBarrio(vista.ID_BARRIO);

                vista.ID_BARRIO = c.ID_BARRIO;
                vista.N_BARRIO = c.N_BARRIO;
                vista.ID_LOCALIDAD = c.ID_LOCALIDAD ?? 0;
                vista.ID_SECCIONAL = c.ID_SECCIONAL ?? 0;
                vista.ID_USR_SIST = c.ID_USR_SIST;
                vista.FEC_SIST = c.FEC_SIST;
                vista.ID_USR_MODIF = c.ID_USR_MODIF;
                vista.FEC_MODIF = c.FEC_MODIF;
            }
            else
            {

            }

            if (accion == "Ver" || accion == "Eliminar")
            {
                vista.Localidades.Enabled = false;
                vista.Seccionales.Enabled = false;
                
            }


            return vista;
        }


        private static void CargarLocalidad(IBarrioVista vista, IEnumerable<ILocalidad> listalocalidades)
        {
            vista.Localidades.Combo.Add(new ComboItem { Id = 0, Description = "Seleccione Localidad..." });
            foreach (var listalocalidad in listalocalidades)
            {
                vista.Localidades.Combo.Add(new ComboItem { Id = listalocalidad.IdLocalidad, Description = listalocalidad.NombreLocalidad });

            }
        }

        private static void CargarSeccionales(IBarrioVista vista, IEnumerable<ISeccional> listaseccionales)
        {
            vista.Seccionales.Combo.Add(new ComboItem { Id = 0, Description = "Seleccione la seccional..." });
            foreach (var listaseccional in listaseccionales)
            {
                vista.Seccionales.Combo.Add(new ComboItem { Id = listaseccional.ID_SECCIONAL, Description = listaseccional.N_SECCIONAL });

            }
        }

        public IList<IBarrio> GetBarriosLocalidad(int idLocalidad)
        {
            
            return _barrioRepositorio.GetBarriosLocalidad(idLocalidad);
        }
    }
}
