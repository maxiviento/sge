using System;
using System.Collections.Generic;
using SGE.Model.Comun;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Repositorio
{
    public interface IRelFamiliarRepositorio
    {
        IList<IRelacionFam> GetFamiliares(int idficha);
        IList<IVinculo> getVinculos();
        int AddFamiliar(IRelacionFam familiar);
        void DelFamiliar(int idficha, int idpersona);
    }
}
