using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Repositorio
{
    public interface IFichaObservacionRepositorio
    {
        IFichaObservacion GetObservacion(int idFicha, int idFichaObservacion);
        IList<IFichaObservacion> GetObservacionesByFicha(int idFicha);
        IList<IFichaObservacion> GetObservacionesByFicha(int idFicha, int skip, int take);
        int GetCountObservaciones(int idFicha);

        void UpdateObservacion(IFichaObservacion fichaObservacion);
        int AddObservacion(IFichaObservacion fichaObservacion);
        void DeleteObservacion(IFichaObservacion fichaObservacion);
    }
}
