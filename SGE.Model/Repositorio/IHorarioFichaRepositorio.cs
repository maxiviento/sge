using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Repositorio
{
    public interface IHorarioFichaRepositorio
    {
        IList<IHorarioFicha> GetHorariosFicha(int idficha);
        IList<IHorarioFicha> GetHorariosFichaByDia(int idficha, string dia);
        int CountCortes(int idficha, string diasemana);
        bool AddHorarioFicha(IList<IHorarioFicha> lista, int idficha);
        IList<IHorarioFicha> GetHorarioFichaByEmpresa(int idficha,int idempresa);
        string GetFecUltimaModif(int idFicha);
    }
}
