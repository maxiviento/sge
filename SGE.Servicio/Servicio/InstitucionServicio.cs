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
using System.Collections.Generic;

namespace SGE.Servicio.Servicio
{
    public class InstitucionServicio: IInstitucionServicio
    {
        private readonly InstitucionRepositorio _institucionRepositorio;
        private readonly IAutenticacionServicio _aut;
        private readonly CarreraRepositorio _carrerasRepositorio; // 08/03/2013 - DI CAMPLI LEANDRO

        public InstitucionServicio()
        {
            _institucionRepositorio= new InstitucionRepositorio();
            _aut=new AutenticacionServicio();

            _carrerasRepositorio = new CarreraRepositorio();// 08/03/2013 - DI CAMPLI LEANDRO
        }

        public IInstitucionesVista GetInstituciones()
        {
            IInstitucionesVista vista= new InstitucionesVista();

            var pager = new Pager(_institucionRepositorio.GetInstitucionesCount(),
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexInstitucion", _aut.GetUrl("IndexPager", "Instituciones"));

            vista.Pager = pager;
                
            vista.Instituciones = _institucionRepositorio.GetInstituciones(pager.Skip, pager.PageSize);

            return vista;
        }

        public IInstitucionesVista GetIndex()
        {
            IInstitucionesVista vista = new InstitucionesVista();

            var pager = new Pager(0,
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexInstitucion", _aut.GetUrl("IndexPager", "Instituciones"));

            vista.Pager = pager;


            return vista;
        }

        public IInstitucionesVista GetInstituciones(string descripcion)
        {
            IInstitucionesVista vista = new InstitucionesVista();

            var pager = new Pager(_institucionRepositorio.GetInstituciones(descripcion).Count(),
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexInstitucion", _aut.GetUrl("IndexPager", "Instituciones"));

            vista.Pager = pager;

            vista.Instituciones = _institucionRepositorio.GetInstituciones(descripcion, pager.Skip, pager.PageSize);

            return vista;
        }

        public IInstitucionVista GetInstitucion(int idInstitucion, string accion)
        {
            IInstitucionVista vista = new InstitucionVista {Accion = accion};

            if (idInstitucion >= 0)
            {
                IInstitucion objreturn = _institucionRepositorio.GetInstitucion(idInstitucion);

                vista.Id = objreturn.IdInstitucion;
                vista.Descripcion = objreturn.NombreInstitucion;

                vista.Sectores = _institucionRepositorio.GetSectoresInstitucion(idInstitucion);
            }

            return vista;
        }

        public IInstitucionesVista GetInstituciones(Pager pPager, int id, string descripcion)
        {
            IInstitucionesVista vista = new InstitucionesVista();

            var pager = new Pager(_institucionRepositorio.GetInstituciones(descripcion).Count(),
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexInstitucion", _aut.GetUrl("IndexPager", "Instituciones"));

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
                
            vista.Instituciones = _institucionRepositorio.GetInstituciones(descripcion, vista.Pager.Skip, vista.Pager.PageSize);
            vista.Descripcion = descripcion;
            return vista;
        }
        
        public int AddInstitucion(IInstitucionVista vista)
        {
              if (_institucionRepositorio.ExistsInstitucion(vista.Descripcion))
                {
                    return 1;
                }

                vista.Id = _institucionRepositorio.AddInstitucion(VistaToDatos(vista));


                return 0;
            
        }

        private static IInstitucion VistaToDatos(IInstitucionVista vista)
        {
            IInstitucion inst = new Institucion
                                {
                                    IdInstitucion = vista.Id,
                                    NombreInstitucion = vista.Descripcion,
                                };

            return inst;
        }

        public int UpdateInstitucion(IInstitucionVista vista)
        {
            _institucionRepositorio.UpdateInstitucion(VistaToDatos(vista));

            return 0;
        }

        public int DeleteInstitucion(IInstitucionVista institucion)
        {
            _institucionRepositorio.DeleteInstitucion(VistaToDatos(institucion));

            return 0;
        }
        
        public IInstitucionVista AddInstitucionLast(string accion)
        {
            IInstitucionVista vista = new InstitucionVista
                                          {
                                              Id = _institucionRepositorio.GetInstitucionMaxId() + 1,
                                              Accion = accion
                                          };

            return vista;
        }

        // 07/03/2013 - DI CAMPLI LEANDRO - LISTAR SECTORES EN INSTITUCIONES
        public int AddAsociarSector(int idInstitucion, int idSector)
        {

            return _institucionRepositorio.AddAsociarSector(idInstitucion, idSector);

            
        }

        // 07/03/2013 - DI CAMPLI LEANDRO - LISTAR SECTORES EN INSTITUCIONES
        public ISectoresVista GetSectorNoAsignado(int idInstitucion)
        {
            ISectoresVista vista = new SectoresVista();

            if (idInstitucion >= 0)
            {
                vista.Sectores = _institucionRepositorio.GetSectorNoAsignado(idInstitucion);

            }
            return vista;
        }

        // 08/03/2013 - DI CAMPLI LEANDRO - LISTAR SECTORES EN INSTITUCIONES

        public string DeleteSectorInstitucion(int idInstitucion, int idSector)
        { 
            //VERIFICAR QUE NO EXISTA EL SECTOR EN ALGUNA INSTITUCION
            IList<ICarrera> carreras = _carrerasRepositorio.GetCarrerasByInstitucionBySector(idInstitucion, idSector);

            if (carreras.Count() > 0)
            {
                return "Existen carreras con el Sector seleccionado";
            }

            _institucionRepositorio.DeleteSectorInstitucion(idInstitucion, idSector);

            return "";
        
        }

    }
}
