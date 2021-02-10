using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Repositorio
{
    public interface IConyugeRepositorio
    {
        IConyuge GetConyugeByBeneficiario(int idBeneficiario);
        IList<IConyuge> GetConyuges();
        IList<IConyuge> GetConyuges(int skip, int make);
        IList<IConyuge> GetSedes(string apellido, string nombre, string cuil);
        IList<IConyuge> GetSedes(string apellido, string nombre, string cuil, int skip, int take);

        IConyuge GetConyuge(int id);
        int AddConyuge(IConyuge sede);
        void UpdateConyuge(IConyuge sede);
        void DeleteConyuge(IConyuge sede);
        bool ExistsConyuge(string cuil);
        int GetConyugesCount(string apellido, string nombre, string cuil);
        int GetConyugesCount();

    }
}
