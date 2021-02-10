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
    public class CarreraServicio : ICarreraServicio
    {
        private readonly ICarreraRepositorio _carreraRepositorio;
        private readonly IAutenticacionServicio _autenticacion;
        private readonly IInstitucionRepositorio _institucionRepositorio;
        private readonly INivelRepositorio _nivelRepositorio;
        private readonly ISectorInstitucionRepositorio _sectorInstitucionRepositorio;
        private readonly ISectorRepositorio _sectorRepositorio;

        public CarreraServicio()
        {
            _carreraRepositorio = new CarreraRepositorio();
            _institucionRepositorio = new InstitucionRepositorio();
            _nivelRepositorio = new NivelRepositorio();
            _sectorInstitucionRepositorio = new SectorInstitucionRepositorio();
            _sectorRepositorio = new SectorRepositorio();
            _autenticacion = new AutenticacionServicio();
        }

        public ICarrerasVista GetCarreras()
        {
            ICarrerasVista vista = new CarrerasVista();

            var pager = new Pager(_carreraRepositorio.GetCarrerasCount(),
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexCarrera", _autenticacion.GetUrl("IndexPager", "Carreras"));

            vista.Pager = pager;
            vista.Carreras = _carreraRepositorio.GetCarreras(pager.Skip, pager.PageSize);

            return vista;
        }

        public ICarrerasVista GetIndex()
        {
            ICarrerasVista vista = new CarrerasVista();

            var pager = new Pager(0,
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexCarrera", _autenticacion.GetUrl("IndexPager", "Carreras"));

            vista.Pager = pager;
            

            return vista;
        }

        public ICarrerasVista GetCarreras(string descripcion)
        {
            ICarrerasVista vista = new CarrerasVista();

            var pager = new Pager(_carreraRepositorio.GetCarreras(descripcion).Count(),
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexCarrera", _autenticacion.GetUrl("IndexPager", "Carreras"));

            vista.Pager = pager;
            vista.Carreras = _carreraRepositorio.GetCarreras(descripcion, pager.Skip, pager.PageSize);

            return vista;
        }

        public ICarreraVista GetCarrera(int idCarrera, string accion)
        {
            ICarreraVista vista = new CarreraVista();
            ICarrera carrera;
            vista.Accion = accion;

            if (accion != "Agregar")
            {
                carrera = _carreraRepositorio.GetCarrera(idCarrera);

                vista.Id = carrera.IdCarrera;
                vista.Nombre = carrera.NombreCarrera;
                vista.IdNivel = carrera.IdNivel;
                vista.IdInstitucion = carrera.IdInstitucion;
                vista.IdSector = carrera.IdSector;
                if (vista.IdInstitucion != null)
                    CargarSectores(vista, _sectorInstitucionRepositorio.GetSectoresInstitucionByInstitucion((int)vista.IdInstitucion));
            }
            else
            {
                carrera = new Carrera();

                vista.Nombre = carrera.NombreCarrera;
                vista.IdNivel = 0;
                vista.IdInstitucion = 0;
                vista.IdSector = 0;
                CargarSectores(vista, new List<ISectorInstitucion>());
            }

            CargarInstituciones(vista, _institucionRepositorio.GetInstituciones());
            CargarNiveles(vista, _nivelRepositorio.GetNiveles());
            vista.NombreNivel.Selected = carrera.IdNivel.ToString();
            vista.NombreInstitucion.Selected = carrera.IdInstitucion.ToString();
            vista.NombreSector.Selected = carrera.IdSector.ToString();

            if (accion == "Ver" || accion=="Eliminar")
            {
                vista.NombreInstitucion.Enabled = false;
                vista.NombreNivel.Enabled = false;
                vista.NombreSector.Enabled = false;
            }

            return vista;
        }

        public ICarrerasVista GetCarreras(Pager pPager, int id, string descripcion)
        {
            ICarrerasVista vista = new CarrerasVista();

            var pager = new Pager(_carreraRepositorio.GetCarreras(descripcion).Count(),
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexCarrera", _autenticacion.GetUrl("IndexPager", "Carreras"));

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

            vista.Carreras = _carreraRepositorio.GetCarreras(descripcion, vista.Pager.Skip, vista.Pager.PageSize);
            vista.DescripcionBusca = descripcion;

            return vista;
        }

        public int AddCarrera(ICarreraVista vista)
        {
            // Combos sin seleccionar
            if (vista.NombreInstitucion.Selected == "0" || vista.NombreNivel.Selected == "0" || vista.NombreSector.Selected == "0")
            {
                CargarInstituciones(vista, _institucionRepositorio.GetInstituciones());
                CargarNiveles(vista, _nivelRepositorio.GetNiveles());
                CargarSectores(vista, _sectorInstitucionRepositorio.GetSectoresInstitucionByInstitucion(Convert.ToInt32(vista.NombreInstitucion.Selected)));
                return 2;
            }
            // ya existe
            if (_carreraRepositorio.GetCarreras(vista.Nombre).Count() > 0)
            {
                CargarInstituciones(vista, _institucionRepositorio.GetInstituciones());
                CargarNiveles(vista, _nivelRepositorio.GetNiveles());
                CargarSectores(vista, _sectorInstitucionRepositorio.GetSectoresInstitucionByInstitucion(Convert.ToInt32(vista.NombreInstitucion.Selected)));
                return 1;
            }

            vista.Id = _carreraRepositorio.AddCarrera(VistaToDatos(vista));

            return 0;
        }

        private static ICarrera VistaToDatos(ICarreraVista vista)
        {
            ICarrera carrera = new Carrera
                {
                    IdCarrera = vista.Id,
                    NombreCarrera = vista.Nombre,
                    IdInstitucion = Convert.ToInt32(vista.NombreInstitucion.Selected),
                    IdNivel = (short?)Convert.ToInt32(vista.NombreNivel.Selected),
                    IdSector = Convert.ToInt32(vista.NombreSector.Selected),
                    NombreInstitucion = vista.NombreInstitucion.Selected,
                    NombreNivel = vista.NombreNivel.Selected,
                    NombreSector = vista.NombreSector.Selected
                };

            return carrera;
        }

        public int UpdateCarrera(ICarreraVista vista)
        {
            // Combos sin seleccionar
            if (vista.NombreInstitucion.Selected == "0" || vista.NombreNivel.Selected == "0" || vista.NombreSector.Selected == "0")
            {
                CargarInstituciones(vista, _institucionRepositorio.GetInstituciones());
                CargarNiveles(vista, _nivelRepositorio.GetNiveles());
                CargarSectores(vista, _sectorInstitucionRepositorio.GetSectoresInstitucionByInstitucion(Convert.ToInt32(vista.NombreInstitucion.Selected)));
                return 2;
            }

            
                // Actualizar Carrera
                _carreraRepositorio.UpdateCarrera(VistaToDatos(vista));

                return 0;
           
        }

        public int DeleteCarrera(ICarreraVista vista)
        {
           
                _carreraRepositorio.DeleteCarrera(VistaToDatos(vista));
                return 0;
            
        }

        public void FormCarreraChangeInstitucion(ICarreraVista vista)
        {
            vista.IdInstitucion = Convert.ToInt32(vista.NombreInstitucion.Selected);
            if (vista.IdInstitucion > 0)
            {
                CargarInstituciones(vista, _institucionRepositorio.GetInstituciones());
                CargarNiveles(vista, _nivelRepositorio.GetNiveles());
                CargarSectores(vista, _sectorInstitucionRepositorio.GetSectoresInstitucionByInstitucion((int)vista.IdInstitucion));
                vista.NombreSector.Selected = vista.IdSector.ToString();
            }

            return;
        }

        private static void CargarInstituciones(ICarreraVista vista, IEnumerable<IInstitucion> listaInstituciones)
        {
            vista.NombreInstitucion.Combo.Add(new ComboItem { Id = 0, Description = "Seleccione una Institución..." });

            foreach (var lInstitucion in listaInstituciones)
            {
                vista.NombreInstitucion.Combo.Add(new ComboItem
                                                      {
                                                          Id = lInstitucion.IdInstitucion,
                                                          Description = lInstitucion.NombreInstitucion
                                                      });
            }
        }

        private static void CargarNiveles(ICarreraVista vista, IEnumerable<INivel> listaNiveles)
        {
            vista.NombreNivel.Combo.Add(new ComboItem { Id = 0, Description = "Seleccione un Nivel..." });
            foreach (var lNivel in listaNiveles)
            {
                vista.NombreNivel.Combo.Add(new ComboItem
                                                {
                                                    Id = lNivel.IdNivel,
                                                    Description = lNivel.NombreNivel
                                                });
            }
        }

        private void CargarSectores(ICarreraVista vista, IEnumerable<ISectorInstitucion> listaSectoresInstituciones)
        {
            vista.NombreSector.Combo.Add(new ComboItem { Id = 0, Description = "Seleccione un Sector..." });
            foreach (var lSectorInstitucion in listaSectoresInstituciones)
            {
                vista.NombreSector.Combo.Add(new ComboItem
                                                 {
                                                     Id = lSectorInstitucion.IdSector,
                                                     Description = _sectorRepositorio.GetSector(lSectorInstitucion.IdSector).NombreSector
                                                 });
            }
        }

        public bool CarreraInUse(int idCarrera)
        {
            return _carreraRepositorio.CarreraInUse(idCarrera);
        }

        // 13/03/2013 - DI CAMPLI LEANDRO - SECTOR INSTITUCION CARRERA
        public ICarrerasVista GetCarrerasSectorInstitucion(int idNivel)
        {
            ICarrerasVista vista = new CarrerasVista();

           // var pager = new Pager(_carreraRepositorio.GetCarrerasCount(),
           //                       Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
           //                       "FormIndexCarrera", _autenticacion.GetUrl("IndexPager", "Carreras"));

           //vista.Pager = pager;
           // vista.Carreras = _carreraRepositorio.GetCarreras(pager.Skip, pager.PageSize);
            switch(idNivel)
            {
                case 0:
                        vista.Carreras = _carreraRepositorio.GetCarreras().OrderBy(c=>c.IdSector).ThenBy(c=>c.NombreInstitucion).ToList();
                        break;
                default:
                        vista.Carreras = _carreraRepositorio.GetCarreras().Where(c=>c.IdNivel==idNivel).OrderBy(c=>c.IdSector).ThenBy(c=>c.NombreInstitucion).ToList();
                        break;
            }
            return vista;
        }

        public IList<ICarrera> GetCarreras(int idInst)
        {
            return _carreraRepositorio.GetCarreras(idInst);
        }

    }
}
