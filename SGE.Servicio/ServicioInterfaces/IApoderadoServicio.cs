using SGE.Servicio.VistaInterfaces;

namespace SGE.Servicio.ServicioInterfaces
{
    public interface IApoderadoServicio
    {
        IApoderadosVista GetApoderadosByBeneficiario(int idBeneficiario);
        IApoderadoVista GetApoderado(int idApoderado, string accion);
        int AddApoderado(IApoderadoVista apoderado);
        int UpdateApoderado(IApoderadoVista apoderado);
        bool ExistsApoderado(string nroDocumento,int idBeneficiario);
        bool UpdateCuentaCbuByBeneficiario(int idBeneciario, string nroCuenta, string cbu, int idsucursal);
        /// <summary>
        /// Obtiene el apoderado Activo de una Beneficiario
        /// </summary>
        /// <param name="idBeneficiario">Clave del Beneficiario</param>
        /// <returns></returns>
        IApoderadoVista GetApoderadoByBeneficiarioActivo(int idBeneficiario);
        bool DeleteApoderado(IApoderadoVista apoderado);
        bool CambiarEstado(IApoderadoVista apoderado);

        bool LimpiarFechaSolicitud(IApoderadoVista apoderado);

        /// <summary>
        /// Valida si el Apoderado ya es apoderado de algun otro.
        /// <para>Lugares de Uso: ApoderadoServicio.</para>
        /// </summary>
        /// <param name="nroDocumento">Número de documento del Apoderado</param>
        /// <returns></returns>
        bool EsApoderadoActivo(string nroDocumento);
    }
}
