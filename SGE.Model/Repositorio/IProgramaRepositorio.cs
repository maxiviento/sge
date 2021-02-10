using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Repositorio
{
    public interface IProgramaRepositorio
    {
        IList<IPrograma> GetProgramas();
        IList<IPrograma> GetProgramas(int skip, int make);
        IList<IPrograma> GetProgramas(string descripcion);
        IList<IPrograma> GetProgramas(string descripcion, int skip, int take);
        IPrograma GetPrograma(int id);
        int AddPrograma(IPrograma programa);
        void UpdatePrograma(IPrograma programa);
        int GetProgramaMaxId();
        int GetProgramasCount();
        IList<IConvenio> GetConvenios();
        IConvenio GetConvenio(int idConvenio);
        IList<IPrograma> GetProgramas(int idRol);
        IList<ITipoFicha> GetTipoFichas(int idRol);
        IList<IRolPrograma> GetTipoFichasRol();
    }
}
