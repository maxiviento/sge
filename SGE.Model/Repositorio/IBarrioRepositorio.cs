using System;
using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Repositorio
{
    public interface IBarrioRepositorio
    {
        IList<IBarrio> GetBarrios();
        IBarrio GetBarrio(int idBarrio);
        IList<IBarrio> GetBarrios(string nombre);
        int AddBarrio(IBarrio barrio);
        void UpdateBarrio(IBarrio barrio);
        IList<ISeccional> GetSeccionales();
        IList<IBarrio> GetBarriosLocalidad(int idLocalidad);
    }
}
