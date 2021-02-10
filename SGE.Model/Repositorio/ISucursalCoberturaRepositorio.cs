using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Repositorio
{
    public interface ISucursalCoberturaRepositorio
    {
        int GetSucursalesCoberturaCount(int? idSucursal, int? idLocalidad, int? idDepartamento, string asignadas);
        int GetSucursalesCoberturaCount();
        void DeleteLocalidadSucursal(ILocalidad locSuc);
        void AddLocalidadSucursal(ISucursalCobertura locSuc);
    }
}
