using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Repositorio
{
    public interface ISectorInstitucionRepositorio
    {
        IList<ISectorInstitucion> GetSectoresInstituciones();
        IList<ISectorInstitucion> GetSectoresInstitucionByInstitucion(int idInstitucion);
        int GetSectorInstitucionCount();
    }
}
