using System;
using System.Collections.Generic;
using SGE.Model.Comun;
using SGE.Model.Entidades;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Repositorio
{
    public interface ICuentaBancoRepositorio
    {
        int AddCuentaBanco(ICuentaBanco cuentaBanco);

        /// <summary>
        /// Actualiza Fecha Solicitud del Beneficiario.
        /// <para>Lugares de Uso: BeneficiarioController.</para>
        /// </summary>
        /// <returns></returns>
        bool UpdateFechaSolicitud(int idbeneficiario, DateTime fecha);

        /// <summary>
        /// Limpia Fecha Solicitud del Beneficiario.
        /// <para>Lugares de Uso: BeneficiarioController.</para>
        /// </summary>
        /// <returns></returns>
        bool ClearFechaSolicitud(int idBeneficiario);


        bool UpdateCuentaBanco(ICuentaBanco cuentaBanco);

        IList<ICuentaBanco> GetCuentaBancoByBeneficiario(int idbeneficiario);
    }
}
