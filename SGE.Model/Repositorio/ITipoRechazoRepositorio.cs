using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Repositorio
{
    public interface ITipoRechazoRepositorio
    {
        IList<ITipoRechazo> GetTiposRechazo();
        IList<ITipoRechazo> GetTiposRechazo(int skip, int make);
        IList<ITipoRechazo> GetTiposRechazo(string descripcion);
        IList<ITipoRechazo> GetTiposRechazo(string descripcion, int skip, int take);
        ITipoRechazo GetTipoRechazo(int id);
        int AddTipoRechazo(ITipoRechazo tipoRechazo);
        bool UpdateTipoRechazo(int idOld, ITipoRechazo tipoRechazo);
        void DeleteTipoRechazo(ITipoRechazo tipoRechazo);
        bool ExistsTipoRechazo(string tipoRechazo);
        bool TipoRechazoInUse(int idTipoRechazo);
        int GetTiposRechazoCount();
        int GetTipoRechazoMaxId();
    }
}
