using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Repositorio
{
    public interface ISedeRepositorio
    {
        IList<ISede> GetSedes();
        IList<ISede> GetSedes(int skip, int make);
        IList<ISede> GetSedes(string descripcion,int? idEmpresa);
        IList<ISede> GetSedes(string descripcion,int? idEmpresa, int skip, int take);
        ISede GetSedes(int id);
        int AddSede(ISede sede);
        void UpdateSede(ISede sede);
        void DeleteSede(ISede sede);
        bool ExistsSede(int idSede,string desc);
        int GetSedesCount(string descripcion, int? idEmpresa);
        int GetSedesCount();
        bool SedeTieneFichas(int idSede);
        IList<ISede> GetSedesByEmpresa(int idEmpresa);
        bool ExistsSedeByEmpresa(int idempresa, string desc);
    }
}
