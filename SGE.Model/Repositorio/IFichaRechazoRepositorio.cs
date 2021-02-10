using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Repositorio
{
    public interface IFichaRechazoRepositorio
    {
        IFichaRechazo GetFichaTipoRechazo(int idFicha, int tipoRechazo);
        void AddFichaTipoRechazo(IFichaRechazo fichaRechazo);
        void DeleteFichaTipoRechazo(IFichaRechazo fichaRechazo);
    }
}
