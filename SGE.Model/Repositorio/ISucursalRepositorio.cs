using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Repositorio
{
    public interface ISucursalRepositorio
    {
        /// <summary>
        /// Obtiene la Sucursal por codigo de Banco
        /// <para>Lugares de Uso: BeneficiarioServicio</para>
        /// </summary>
        /// <param name="codigobanco">Codigo de la sucursal de las tablas de banco</param>
        /// <returns></returns>
        ISucursal GetSucursalByCodigoBanco(string codigobanco);

        IList<ISucursal> GetSucursales();

        short GestSucursalLocalidad(int idFicha);
    }
}
