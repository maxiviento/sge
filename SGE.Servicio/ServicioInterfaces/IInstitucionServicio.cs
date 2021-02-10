using SGE.Servicio.VistaInterfaces;
using SGE.Servicio.Vista.Shared;

namespace SGE.Servicio.ServicioInterfaces
{
    public interface IInstitucionServicio
    {
        IInstitucionesVista GetInstituciones();
        IInstitucionesVista GetIndex();
        IInstitucionesVista GetInstituciones(string descripcion);
        IInstitucionVista GetInstitucion(int id, string accion);
        IInstitucionesVista GetInstituciones(Pager pager, int id, string descripcion);
        int AddInstitucion(IInstitucionVista inst);
        int UpdateInstitucion(IInstitucionVista inst);
        int DeleteInstitucion(IInstitucionVista inst);
        IInstitucionVista AddInstitucionLast(string accion);

        int AddAsociarSector(int idInstitucion, int idSector); // 07/03/2013 - DI CAMPLI LEANDRO - ASOCIAR INSTITUCION/SECTOR
        ISectoresVista GetSectorNoAsignado(int idInstitucion);// 07/03/2013 - DI CAMPLI LEANDRO - ASOCIAR INSTITUCION/SECTOR
        string DeleteSectorInstitucion(int idInstitucion, int idSector);// 08/03/2013 - DI CAMPLI LEANDRO - ASOCIAR INSTITUCION/SECTOR
    }
}
