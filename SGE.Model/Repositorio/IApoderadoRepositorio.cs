using System;
using System.Collections.Generic;
using SGE.Model.Comun;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Repositorio
{
    public interface IApoderadoRepositorio
    {
        /// <summary>
        /// Obtiene un histórico de apoderados por beneficiario.
        /// <para>Lugares de Uso: ApoderadoServicio.</para>
        /// </summary>
        /// <param name="idBeneficiario">Identificador del Beneficiario</param>
        /// <returns></returns>
        IList<IApoderado> GetApoderadosByBeneficiario(int idBeneficiario);

        IApoderado GetApoderado(int idApoderado);

        IApoderado GetApoderado(string nroDocumento);
        
        int AddApoderado(IApoderado apoderado);
        void UpdateApoderado(IApoderado apoderado);

        bool UpdateApoderadoSimple(IApoderado apoderado);

        /// <summary>
        /// Valida si el Apoderado ya existe.
        /// <para>Lugares de Uso: ApoderadoServicio.</para>
        /// </summary>
        /// <param name="nroDocumento">Número de documento del Apoderado</param>
        /// <returns></returns>
        bool ExistsApoderado(string nroDocumento);

        /// <summary>
        /// Valida si el apoderado es apoderado activo o reemplazado de algun beneficiario
        /// <para>Lugares de Uso: ApoderadoServicio.</para>
        /// </summary>
        /// <param name="nroDocumento"></param>
        /// <param name="idBeneficiario"></param>
        /// <returns></returns>
        bool EsApoderadoDeOtroBeneficiario(string nroDocumento, int idBeneficiario);

        /// <summary>
        /// Se utiliza para suspender los beneficiarios del apoderado y asignarlo al beneficiario que se esta editando
        /// <para>Lugares de Uso: ApoderadoServicio.</para>
        /// </summary>
        /// <param name="nroDocumento">Número de documento del Apoderado</param>
        /// <returns></returns>
        void SuspenderBeneficiarios(string nroDocumento);


        /// <summary>
        /// Valida si el el beneficiario ya tiene un apoderado ACTIVO, en este caso no le permite asignarle otro apoderado.
        /// <para>Lugares de Uso: ApoderadoServicio.</para>
        /// </summary>
        /// <param name="idBeneficiario">Identificador del Beneficiario</param>
        /// <returns></returns>
        bool TieneApoderadoActivo(int idBeneficiario);

        /// <summary>
        /// Actualiza el numero de cuenta y cbu del apoderado para un beneficiario determinado
        /// <para>Lugares de Uso: BeneficiarioServicio.</para>
        /// </summary>
        /// <param name="idBeneficiario">Clave del Beneficiario del apoderado</param>
        /// <param name="nroCuenta">Número de cuenta del Apoderado</param>
        /// <param name="cbu">CBU del Apoderado</param>
        /// <param name="idsucursal">Sucursal enviada por el banco</param>
        /// <returns></returns>
        bool UpdateCuentaCbuByBeneficiario(int idBeneficiario, string nroCuenta, string cbu , int idsucursal);

        /// <summary>
        /// Obtiene el apoderado Activo de una Beneficiario
        /// </summary>
        /// <param name="idBeneficiario">Clave del Beneficiario</param>
        /// <returns></returns>
        IApoderado GetApoderadoByBeneficiarioActivo(int idBeneficiario);

        /// <summary>
        /// Obtiene un histórico de apoderados por beneficiario.
        /// <para>Lugares de Uso: ApoderadoServicio.</para>
        /// </summary>
        /// <param name="numeroDocumento">Documento del Apoderado</param>
        /// <returns></returns>
        IList<IApoderado> GetApoderadosByNumeroDocumento(string numeroDocumento);


        /// <summary>
        /// Actualiza Fecha Solicitud del Beneficiario.
        /// <para>Lugares de Uso: BeneficiarioController.</para>
        /// </summary>
        /// <returns></returns>
        bool UpdateFechaSolicitud(int idapoderado, DateTime fecha);


        /// <summary>
        /// Suspende Apoderado Activo de un Beneficiario
        /// <para>Lugares de Uso: ApoderadoServicio.</para>
        /// </summary>
        /// <param name="idbeneficiario">Clave del Beneficiario</param>
        /// <returns></returns>
        void SuspenderApoderadoActivoByBeneficiario(int idbeneficiario);

        bool DeleteApoderado(IApoderado apoderado);

        /// <summary>
        /// Obtiene un apoderado especifico por Nro. de cuenta y Código interno de sucursal.
        /// <para>Lugares de Uso: ApoderadoServicio.</para>
        /// </summary>
        /// <param name="nrocuenta">Nro de cuenta</param>
        /// <param name="idsucursal">Código interno de sucursal</param>
        /// <returns></returns>
        IApoderado GetApoderadosByNroCuentaAndSucursalActivo(int nrocuenta, int idsucursal);


        bool CambiarEstado(IApoderado apoderado);

        bool LimpiarFechaSolicitud(int idapoderado);

        /// <summary>
        /// Valida si el Apoderado ya existe.
        /// <para>Lugares de Uso: ApoderadoServicio.</para>
        /// </summary>
        /// <param name="nroDocumento">Número de documento del Apoderado</param>
        /// <returns></returns>
        bool EsApoderadoActivo(string nroDocumento);

        void ActivarApoderadoActivoByBeneficiario(int idbeneficiario);
    }
}
