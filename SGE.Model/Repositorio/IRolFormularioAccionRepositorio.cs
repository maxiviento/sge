using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Repositorio
{
    public interface IRolFormularioAccionRepositorio
    {
        IList<IRolFormularioAccion> GetFormulariosDelRol(int idRol);
        IRolFormularioAccion GetRolFormularioAccion(int idRol, int idFormulario, int idAccion);
        void AddRolFormularioAccion(IRolFormularioAccion rolFormularioAccion);
        void DeleteRolFormulariosAcciones(IRolFormularioAccion rolFormularioAccion);
        IList<IRolFormularioAccion> GetFormulariosDelUsuario(int idUsuario);
    }
}
