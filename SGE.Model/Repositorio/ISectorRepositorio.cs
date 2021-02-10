using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Repositorio
{
    public interface ISectorRepositorio
    {
        IList<ISector> GetSectores();
        ISector GetSector(int idSector);
        int GetSectorCount();
    }
}
