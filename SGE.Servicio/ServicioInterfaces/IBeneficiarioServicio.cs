using System;
using System.Collections.Generic;
using System.Web;
using SGE.Servicio.Comun;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces;

namespace SGE.Servicio.ServicioInterfaces
{
    public interface IBeneficiarioServicio : IBaseServicio
    {
        /// <summary>
        /// Obtiene el listado de los Beneficiario, filtrado por Nombre, Apellido, Cuil,  Numero de Documento y programa a cual pertenece.
        /// <para>Lugares de Uso: BeneficiarioController.</para>
        /// </summary>
        /// <param name="nombre">Nombre del Beneficiario</param>
        /// <param name="apellido">Apellido del Beneficiario</param>
        /// <param name="cuil">Cuil del Beneficiario</param>
        /// <param name="numeroDocumento">Número documento del Beneficiario</param>
        /// <param name="idPrograma">Programa del Beneficiario</param>
        /// <param name="conApoderados"></param>
        /// <param name="modalidad"></param>
        /// <param name="discapacitado"></param>
        /// <param name="altatemprana"></param>
        /// <param name="idEstado"></param>
        /// <param name="apellidonombreapoderado"></param> 
        /// <returns></returns>
        IBeneficiariosVista GetBeneficiarios(string nombre, string apellido, string cuil, string numeroDocumento, int idPrograma, string conApoderados, string modalidad, string discapacitado, string altatemprana, int idEstado, string apellidonombreapoderado, int idEtapa, int idSubprograma, int TipoPrograma, string nrotramite);

        IBeneficiariosVista GetBeneficiariosIndex();

        /// <summary>
        /// Obtiene el listado de los Beneficiario, filtrado por Nombre, Apellido, Cuil,  Numero de Documento y programa a cual pertenece(Para la paginación).
        /// <para>Lugares de Uso: BeneficiarioController.</para>
        /// </summary>
        /// <param name="nombre">Nombre del Beneficiario</param>
        /// <param name="apellido">Apellido del Beneficiario</param>
        /// <param name="cuil">Cuil del Beneficiario</param>
        /// <param name="numeroDocumento">Número documento del Beneficiario</param>
        /// <param name="idPrograma">Programa del Beneficiario</param>
        /// <param name="pager">Objeto de Paginación</param>
        /// <param name="conApoderados"></param>
        /// <param name="modalidad"></param>
        /// <param name="discapacitado"></param>
        /// <param name="altatemprana"></param>
        /// <param name="idEstado"></param>
        /// <param name="apellidonombreapoderado"></param>
        /// <returns></returns>
        IBeneficiariosVista GetBeneficiarios(IPager pager, string nombre, string apellido, string cuil, string numeroDocumento, int idPrograma, string conApoderados, string modalidad, string discapacitado, string altatemprana, int idEstado, string apellidonombreapoderado, int idEtapa, int idSubprograma, int TipoPrograma, string nrotramite);

        /// <summary>
        /// Obtiene un Beneficiario en particular.
        /// <para>Lugares de Uso: BeneficiarioController.</para>
        /// </summary>
        /// <param name="idBeneficiario">Clave identificadora del Beneficiario</param>
        /// <returns></returns>
        IBeneficiarioVista GetBeneficiario(int idBeneficiario);

        /// <summary>
        /// Actualiza los datos del Beneficiario.
        /// <para>Lugar de Uso: BeneficiarioController</para>
        /// </summary>
        /// <param name="vista">Vista con los Datos de Beneficiario para Actualizar</param>
        /// <returns></returns>
        bool UpdateBeneficiario(IBeneficiarioVista vista);

        /// <summary>
        /// Obtiene los Beneficiario que no tiene cuentas de bancos. 
        /// <para>Lugar de Uso: BeneficiarioController</para>
        /// </summary>
        /// <param name="nombre">Nombre del Beneficiario</param>
        /// <param name="apellido">Apellido del Beneficiario</param>
        /// <param name="cuil">Cuil del Beneficiario</param>
        /// <param name="numeroDocumento">Número documento del Beneficiario</param>
        /// <param name="idPrograma">Programa del Beneficiario</param>
        /// <param name="conApoderados">Que tenga Apoderados</param>
        /// <param name="modalidad">La modalidad</param>
        /// <param name="discapacitado"></param>
        /// <param name="altatemprana"></param>
        /// <param name="idEstado"></param>
        /// <param name="apellidonombreapoderado"></param>
        /// <returns></returns>
        IBeneficiariosVista GetBeneficiarioSinCuenta(string nombre, string apellido, string cuil,
                                                     string numeroDocumento, int idPrograma, string conApoderados, string modalidad, string discapacitado, string altatemprana, int idEstado, string apellidonombreapoderado, int idEtapa, int idSubprograma, int TipoPrograma, string nrotramite);

        /// <summary>
        /// Obtiene los Beneficiario que no tiene cuentas de bancos (para la paginación).
        /// <para>Lugar de Uso: BeneficiarioController</para> 
        /// </summary>
        /// <param name="fileNombre"></param>
        /// <param name="nombre">Nombre del Beneficiario</param>
        /// <param name="apellido">Apellido del Beneficiario</param>
        /// <param name="cuil">Cuil del Beneficiario</param>
        /// <param name="numeroDocumento">Número documento del Beneficiario</param>
        /// <param name="idPrograma">Programa del Beneficiario</param>
        /// <param name="conApoderados">Que tenga Apoderados</param>
        /// <param name="modalidad">La modalidad</param>
        /// <param name="pager">Objeto de Paginación</param>
        /// <param name="fileDownload"></param>
        /// <param name="discapacitado"></param>
        /// <param name="altatemprana"></param>
        /// <param name="idEstado"></param>
        /// <param name="apellidonombreapoderado"></param>
        /// <returns></returns>
        IBeneficiariosVista GetBeneficiarioSinCuenta(IPager pager, string fileDownload, string fileNombre, string nombre,
                                                     string apellido, string cuil, string numeroDocumento,
                                                     int idPrograma, string conApoderados, string modalidad, string discapacitado, string altatemprana, int idEstado, string apellidonombreapoderado, int idEtapa, int idSubprograma, int TipoPrograma, string nrotramite);

        /// <summary>
        /// Obtiene los Beneficiarios sin cuentas de bancos para generar el archivo que se envia al banco.
        /// <para>Lugar de Uso: BeneficiarioController </para>
        /// </summary>
        /// <param name="nombre">Nombre del Beneficiario</param>
        /// <param name="apellido">Apellido del Beneficiario</param>
        /// <param name="cuil">Cuil del Beneficiario</param>
        /// <param name="numeroDocumento">Número documento del Beneficiario</param>
        /// <param name="idPrograma">Programa del Beneficiario</param>
        /// <param name="conApoderados">Que tenga Apoderados</param>
        /// <param name="modalidad">La modalidad</param>
        /// <param name="discapacitado"></param>
        /// <param name="altatemprana"></param>
        /// <param name="idEstado"></param>
        /// <returns></returns>
        IBeneficiariosVista GetBeneficiarioSinCuentaFile(string nombre, string apellido, string cuil,
                                                     string numeroDocumento, int idPrograma, string conApoderados, string modalidad, string discapacitado, string altatemprana, int idEstado, int idSubprograma, int TipoPrograma, string nrotramite);

        /// <summary>
        /// Obtiene los Beneficiarios con cuentas de bancos.
        /// <para>Lugar de Uso: BeneficiarioController </para>
        /// </summary>
        /// <param name="nombre">Nombre del Beneficiario</param>
        /// <param name="apellido">Apellido del Beneficiario</param>
        /// <param name="cuil">Cuil del Beneficiario</param>
        /// <param name="numeroDocumento">Número documento del Beneficiario</param>
        /// <param name="idPrograma">Programa del Beneficiario</param>
        /// <param name="conApoderados"></param>
        /// <param name="modalidad"></param>
        /// <param name="discapacitado"></param>
        /// <param name="altatemprana"></param>
        /// <param name="idEstado"></param>
        /// <param name="apellidonombreapoderado"></param>
        /// <returns></returns>
        IBeneficiariosVista GetBeneficiarioConCuenta(string nombre, string apellido, string cuil,
                                                     string numeroDocumento, int idPrograma, string conApoderados, string modalidad, string discapacitado, string altatemprana, int idEstado, string apellidonombreapoderado, int idEtapa, int idSubprograma, int TipoPrograma, string nrotramite);

        /// <summary>
        /// Obtiene los Beneficiarios con cuentas de bancos.
        /// <para>Lugar de Uso: BeneficiarioController </para>
        /// </summary>
        /// <param name="fileNombre"></param>
        /// <param name="nombre">Nombre del Beneficiario</param>
        /// <param name="apellido">Apellido del Beneficiario</param>
        /// <param name="cuil">Cuil del Beneficiario</param>
        /// <param name="numeroDocumento">Número documento del Beneficiario</param>
        /// <param name="idPrograma">Programa del Beneficiario</param>
        /// <param name="pager">Objeto de Paginación</param>
        /// <param name="fileDownload"></param>
        /// <param name="conApoderados"></param>
        /// <param name="modalidad"></param>
        /// <param name="discapacitado"></param>
        /// <param name="altatemprana"></param>
        /// <param name="idEstado"></param>
        /// <param name="apellidonombreapoderado"></param>
        /// <returns></returns>
        IBeneficiariosVista GetBeneficiarioConCuenta(IPager pager, string fileDownload, string fileNombre, string nombre,
                                                     string apellido, string cuil, string numeroDocumento, int idPrograma, string conApoderados, string modalidad, string discapacitado, string altatemprana, int idEstado, string apellidonombreapoderado, int idEtapa, int idSubprograma, int TipoPrograma, string nrotramite);
        /// <summary>
        /// Vista para Casos de error en la actualización u otra accion.
        /// <para>Lugar de Uso: BeneficiarioController </para>
        /// </summary>
        /// <param name="vista">Vista de Beneficiario para mostrar en caso de error</param>
        /// <returns></returns>
        IBeneficiarioVista GetBeneficiarioForError(IBeneficiarioVista vista);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="archivo"></param>
        /// <returns></returns>
        IImportarCuentasVista ImportarCuentas(HttpPostedFileBase archivo);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="archivo"></param>
        /// <returns></returns>
        IList<ICuentaImportar> CargarCuentas(string archivo);

        /// <summary>
        /// Limpia la Fecha de Solicitud de cuenta.
        /// <para>Lugares de Uso: BeneficiarioController.</para>
        /// </summary>
        /// <param name="idBeneficiario">Clave identificadora del Beneficiario</param>
        /// <returns></returns>
        IBeneficiarioVista ClearFechaSolicitud(int idBeneficiario);


        /// <summary>
        /// Obtiene el Listado de Archivos Generados.
        /// <para>Lugares de Uso: BeneficiarioController.</para>
        /// </summary>
        /// <returns></returns>
        IArchivosVista GetArchivos();


        /// <summary>
        /// Limpia la Fecha de Solicitud de cuenta.
        /// <para>Lugares de Uso: BeneficiarioController.</para>
        /// </summary>
        /// <param name="fecha">FechaPorLimpiar</param>
        /// <returns></returns>
        bool ClearFechaSolicitudArchivobyFecha(DateTime fecha);

        /// <summary>
        /// Vuelve a Generar el Archivo.
        /// <para>Lugares de Uso: BeneficiarioController.</para>
        /// </summary>
        /// <param name="fecha">FechaPorLimpiar</param>
        /// <returns></returns>
        IArchivosVista VolverGenerarArchivo(DateTime fecha);

        byte[] ReporteBeneficiarioArt();

        byte[] ReporteBeneficiarioNomina();
        
        byte[] ExportBeneficiario(IBeneficiariosVista vista);

        byte[] ExportReporteArtHorariosEntrenamientoPpp(IBeneficiariosVista vista);

        byte[] ExportReporteArtHorariosEntrenamientoReconversion(IBeneficiariosVista vista);

        byte[] ExportReporteArtHorariosEntrenamientoEfectores(IBeneficiariosVista vista);

        byte[] ExportReporteArtHorariosEntrenamientoVat(IBeneficiariosVista vista);

        byte[] ExportReporteArtHorariosEntrenamientoPppp(IBeneficiariosVista vista);
        // 19/08/2014 - Reporte ART para confiamos en Vos
        byte[] ExportReporteArtHorariosEntrenamientoConfVos(IBeneficiariosVista vista);

        // 2D6/02/2013 - LEANDRO DI CAMPLI - MEJORA DE RENDIMIENTO EXPORT EXCEL BENEFICIARIOS
        byte[] ExportBeneficiarioBis(IBeneficiariosVista vista);
        // 26/02/2013 - LEANDRO DI CAMPLI - MEJORAR PERFORMANCE DE EXPORTAR EXCEL DE BENEFICIARIOS
        IBeneficiariosVista GetBeneficiarioConCuentaBis(string nombre, string apellido, string cuil, string numeroDocumento, int idPrograma, string conApoderados, string modalidad, string discapacitado, string altatemprana, int idEstado, string apellidonombreapoderado, int idEtapa, string nrotramite);
        // 26/02/2013 - LEANDRO DI CAMPLI - MEJORAR PERFORMANCE DE EXPORTAR EXCEL DE BENEFICIARIOS
        IBeneficiariosVista GetBeneficiariosBis(string nombre, string apellido, string cuil, string numeroDocumento, int idPrograma, string conApoderados, string modalidad, string discapacitado, string altatemprana, int idEstado, string apellidonombreapoderado, int idEtapa);

        // 05/07/2013 - DI CAMPLI LEANDRO
        byte[] ExportPPP(IBeneficiariosVista vista);
        // 16/07/2013 - DI CAMPLI LEANDRO
        byte[] ExportTerciario(IBeneficiariosVista vista);
        // 30/07/2013 - DI CAMPLI LEANDRO
        byte[] ExportUniversitario(IBeneficiariosVista vista);
        // 01/08/2013 - DI CAMPLI LEANDRO
        byte[] ExportPPPprof(IBeneficiariosVista vista);
        // 02/08/2013 - DI CAMPLI LEANDRO
        byte[] ExportVat(IBeneficiariosVista vista);
        // 02/08/2013 - DI CAMPLI LEANDRO
        byte[] ExportRec_Prod(IBeneficiariosVista vista);
        // 05/08/2013 - DI CAMPLI LEANDRO
        byte[] ExportEfec_Soc(IBeneficiariosVista vista);

        // 22/04/2014 - DI CAMPLI LEANDRO
        byte[] ExportConf_Vos(IBeneficiariosVista vista);

        byte[] ExportBeneficiarioHorarios(IBeneficiariosVista vista);
        IBeneficiariosVista GetBeneficiarioHorarios();
    }
}
