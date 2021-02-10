using System;
using System.Collections.Generic;
using SGE.Model.Comun;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Repositorio
{
    public interface IAbandonoCursadoRepositorio
    {
        IAbandonoCursado GetAbandonoCursado(int idAbandonoCursado);
        IList<IAbandonoCursado> GetAll();
        IList<IAbandonoCursado> GetAllcbo();
    }
}
