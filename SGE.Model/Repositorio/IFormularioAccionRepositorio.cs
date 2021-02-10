using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Repositorio
{
    public interface IFormularioAccionRepositorio
    {
        IList<IFormularioAccion> GetAccionesDelFormulario(int idFormulario);
        void AddFormularioAccion(IFormularioAccion formularioAccion);
        void DeleteFormularioAccion(IFormularioAccion formularioAccion);
    }
}
