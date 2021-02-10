using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Repositorio
{
    public interface ITablaBcoCbaRepositorio
    {
        IList<ITablaBcoCba> GetTablaBcoCba();
        IList<ITablaBcoCba> GetSucursales();
        IList<ITablaBcoCba> GetMonedas();
        IList<ITablaBcoCba> GetSistemas();
    }
}
