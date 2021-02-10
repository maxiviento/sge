using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Repositorio
{
    public interface IFichaPPPRepositorio
    {
        IList<IFichaPPP> GetFichasPPP();
        IList<IFichaPPP> GetFichasPPP(int skip, int make);
        IList<IFichaPPP> GetFichasByEmpresa(int? Id_Empresa);
        IList<IFichaPPP> GetFichasPPP(int? Id_Empresa, int skip, int take);
        IFichaPPP GetFichaPPP(int id);
        int GetCountFichasPPP();

        //IList<IFichaPPP> GetFichasByEmpresa(int id_empresa);
    }
}
