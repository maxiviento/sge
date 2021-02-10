using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Repositorio
{
    public interface ITipoFichaRepositorio
    {
        IList<ITipoFicha> GetTiposFicha();
    }
}
