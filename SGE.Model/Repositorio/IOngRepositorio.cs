using System;
using System.Collections.Generic;
using SGE.Model.Comun;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Repositorio
{
    public interface IOngRepositorio
    {
        List<IOng> GetOngs(string cuit);
        IOng GetOng(int idOng);
        List<IOng> GetOngsNom(string nombre);
        int AddOng(IOng ong);
        void UpdateOng(IOng ong);
        List<IOng> GetOngsId(int id);
    }
}
