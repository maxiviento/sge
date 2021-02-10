using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Repositorio
{
    public interface IInstitucionRepositorio
    {
        IList<IInstitucion> GetInstituciones(); 
        IList<IInstitucion> GetInstituciones(int skip, int make);
        IList<IInstitucion> GetInstituciones(string descripcion);
        IList<IInstitucion> GetInstituciones(string descripcion, int skip, int take);
        IInstitucion GetInstitucion(int id);
        int AddInstitucion(IInstitucion inst);
        void UpdateInstitucion(IInstitucion inst);
        void DeleteInstitucion(IInstitucion inst);
        bool ExistsInstitucion(string cr);
        IList<IInstitucion> GetInstitucionesBySectorProductivo(int idSector);
        IList<IInstitucion> GetInstitucionesByNivel(int idNivel);
        int GetInstitucionesCount();
        int GetInstitucionMaxId();
        IList<ISector> GetSectoresInstitucion(int idInstitucion);// 07/03/2013 - DI CAMPLI LEANDRO - ASOCIAR INSTITUCION/SECTOR
        int AddAsociarSector(int idInstitucion, int idSector); // 07/03/2013 - DI CAMPLI LEANDRO - ASOCIAR INSTITUCION/SECTOR
        IList<ISector> GetSectorNoAsignado(int idInstitucion);// 07/03/2013 - DI CAMPLI LEANDRO - ASOCIAR INSTITUCION/SECTOR
        void DeleteSectorInstitucion(int idInstitucion, int idSector);// 08/03/2013 - DI CAMPLI LEANDRO - ASOCIAR INSTITUCION/SECTOR

        IList<IInstitucion> GetInstitucionesBySector(int idSector);// 01/08/2018 - DI CAMPLI LEANDRO - AGREGAR INST POR SECTOR PARA PIP
    }
}
