using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Repositorio
{
    public interface ICarreraRepositorio
    {
        IList<ICarrera> GetCarreras();
        IList<ICarrera> GetCarrerasByInstitucion(int? idInstitucion);
        IList<ICarrera> GetCarrerasByInstitucionBySector(int? idInstitucion, int? idSector);
        int AddCarrera(ICarrera carrera);
        ICarrera GetCarrera(int id);
        IList<ICarrera> GetCarreras(int skip, int take);
        IList<ICarrera> GetCarreras(string descripcion);
        IList<ICarrera> GetCarreras(string descripcion, int skip, int take);
        void UpdateCarrera(ICarrera carrera);
        int GetCarrerasCount();
        void DeleteCarrera(ICarrera carrera);
        bool CarreraInUse(int idCarrera);
        IList<ICarrera> GetCarreras(int Inst);
    }
}
