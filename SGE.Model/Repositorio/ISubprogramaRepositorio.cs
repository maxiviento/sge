using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;
using SGE.Model.Entidades;

namespace SGE.Model.Repositorio
{
    public interface ISubprogramaRepositorio
    {
        IList<ISubprograma> GetSubprogramas();
        IList<ISubprograma> GetSubprogramas(int skip, int make);
        IList<ISubprograma> GetSubprogramas(string descripcion);
        IList<ISubprograma> GetSubprogramas(string descripcion, int skip, int take);
        ISubprograma GetSubprograma(int id);
        int AddSubprograma(ISubprograma subprograma);
        void UpdateSubprograma(ISubprograma subprograma);
        int GetSubprogramasCount();
        IList<ISubprogramaRol> GetSubprogramasRol();
        List<Subprograma> GetSubprogr();
        List<Subprograma> GetSubprogramas(int idPrograma);
        int udpSubprogramaFichas(IList<IFicha> fichas, int idSubprograma);
    }
}
