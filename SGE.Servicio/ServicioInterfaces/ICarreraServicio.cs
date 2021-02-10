using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces;
using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Servicio.ServicioInterfaces
{
    public interface ICarreraServicio
    {
        ICarrerasVista GetCarreras();
        ICarrerasVista GetIndex();
        ICarrerasVista GetCarreras(string descripcion);
        ICarreraVista GetCarrera(int id, string accion);
        ICarrerasVista GetCarreras(Pager pager, int id, string descripcion);
        int AddCarrera(ICarreraVista carrera);
        int UpdateCarrera(ICarreraVista carrera);
        int DeleteCarrera(ICarreraVista carrera);
        void FormCarreraChangeInstitucion(ICarreraVista vista);
        bool CarreraInUse(int idCarrera);
        ICarrerasVista GetCarrerasSectorInstitucion(int idNivel);
        IList<ICarrera> GetCarreras(int idInst);
    }
}
