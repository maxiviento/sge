using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Repositorio
{
    public interface ICausaRechazoRepositorio
    {
        IList<ICausaRechazo> GetCausasRechazo();
        IList<ICausaRechazo> GetCausasRechazo(int skip, int make);
        IList<ICausaRechazo> GetCausasRechazo(string descripcion);
        IList<ICausaRechazo> GetCausasRechazo(string descripcion, int skip, int take);
        ICausaRechazo GetCausaRechazo(int id);
        int AddCausaRechazo(ICausaRechazo causaRechazo);
        bool UpdateCausaRechazo(int idOld, ICausaRechazo causaRechazo);
        void DeleteCausaRechazo(ICausaRechazo causaRechazo);
        bool ExistsCausaRechazo(string causaRechazo);
        int GetCausaRechazosCount();
        bool CausaRechazoInUse(int idCausaRechazo);
    }
}
