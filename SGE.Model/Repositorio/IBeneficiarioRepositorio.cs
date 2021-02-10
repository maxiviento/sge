using System;
using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Repositorio
{
    public interface IBeneficiarioRepositorio
    {
        /// <summary>
        /// Obtiene el listado de los Beneficiario, filtrado por Nombre, Apellido, Cuil,  Numero de Documento y programa a cual pertenece.
        /// <para>Lugares de Uso: BeneficiarioServicio.</para>
        /// </summary>
        /// <param name="nombre">Nombre del Beneficiario</param>
        /// <param name="apellido">Apellido del Beneficiario</param>
        /// <param name="cuil">Cuil del Beneficiario</param>
        /// <param name="numeroDocumento">Número documento del Beneficiario</param>
        /// <param name="idPrograma">Programa del Beneficiario</param>
        /// <param name="conApoderados">Si el Beneficiario tiene o no apoderado</param>
        /// <param name="modalidad"></param>
        /// <param name="discapacitado"></param>
        /// <param name="altaTemprana"></param>
        /// <param name="idEstado"></param>
        /// <param name="apellidonombreapoderado"></param> 
        /// <returns></returns>
        IList<IBeneficiario> GetBeneficiarios(string nombre, string apellido, string cuil, string numeroDocumento, int idPrograma, string conApoderados, string modalidad, string discapacitado, string altaTemprana, int idEstado, string apellidonombreapoderado, int idEtapa, string nrotramite, int tipoprograma = 0, int subprograma = 0);

        IList<IBeneficiario> GetBeneficiarios(string nombre, string apellido, string cuil, string numeroDocumento, int idPrograma, string conApoderados, string modalidad, string discapacitado, string altaTemprana, int idEstado, string apellidonombreapoderado , int excluirprograma, int idEtapa);

        /// <summary>
        /// Obtiene el listado de los Beneficiario, filtrado por Nombre, Apellido, Cuil,  Numero de Documento y programa a cual pertenece(Para la paginación).
        /// <para>Lugares de Uso: BeneficiarioServicio.</para>
        /// </summary>
        /// <param name="nombre">Nombre del Beneficiario</param>
        /// <param name="apellido">Apellido del Beneficiario</param>
        /// <param name="cuil">Cuil del Beneficiario</param>
        /// <param name="numeroDocumento">Número documento del Beneficiario</param>
        /// <param name="idPrograma">Programa del Beneficiario</param>
        /// <param name="conApoderados"></param>
        /// <param name="modalidad"></param>
        /// <param name="altaTemprana"></param>
        /// <param name="skip">Pagina que va Tomar(son parametros de paginación)</param>
        /// <param name="take">Cantidad que va tomar(son parametros de paginación)</param>
        /// <param name="discapacitado"></param>
        /// <param name="idEstado"></param>
        /// <param name="apellidonombreapoderado"></param> 
        /// <returns></returns>
        IList<IBeneficiario> GetBeneficiarios(string nombre, string apellido, string cuil, string numeroDocumento, int idPrograma, string conApoderados, string modalidad, string discapacitado, string altaTemprana, int idEstado, string apellidonombreapoderado, int skip, int take, int idEtapa);

        IList<IBeneficiario> GetBeneficiarios(string nombre, string apellido, string cuil, string numeroDocumento, int idPrograma, string conApoderados, string modalidad, string discapacitado, string altaTemprana, int idEstado, string apellidonombreapoderado, int skip, int take, int excluirprograma, int idEtapa);

        /// <summary>
        /// Obtiene la cantidad de Beneficiarios, esto solo se utiliza para la paginación.
        /// <para>Lugares de Uso: BeneficiarioServicio.</para> 
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
        int GetBeneficiariosCount(string nombre, string apellido, string cuil, string numeroDocumento, int idPrograma, string conApoderados, string modalidad, string discapacitado, string altatemprana, int idEstado, string apellidonombreapoderado);

        /// <summary>
        /// Obtiene un Beneficiario en particular.
        /// <para>Lugares de Uso: BeneficiarioServicio.</para>
        /// </summary>
        /// <param name="idBeneficiario">Clave identificadora del Beneficiario</param>
        /// <returns></returns>
        IBeneficiario GetBeneficiario(int idBeneficiario);

        /// <summary>
        /// Actualiza los datos del Beneficiario.
        /// <para>Lugar de Uso: BeneficiarioServicio</para>
        /// </summary>
        /// <param name="beneficiario">Modelo del tipo IBeneficiario</param>
        /// <returns></returns>
        bool UpdateBeneficiario(IBeneficiario beneficiario);

        /// <summary>
        /// Obtiene los Beneficiario que no tiene cuentas de bancos. 
        /// <para>Lugar de Uso: BeneficiarioServicio</para>
        /// </summary>
        /// <param name="nombre">Nombre del Beneficiario</param>
        /// <param name="apellido">Apellido del Beneficiario</param>
        /// <param name="cuil">Cuil del Beneficiario</param>
        /// <param name="numeroDocumento">Número documento del Beneficiario</param>
        /// <param name="idPrograma">Programa del Beneficiario</param>
        /// <param name="conApoderados">Si tiene Apoderados</param>
        /// <param name="modalidad">Modalidad</param>
        /// <param name="altatemprana"></param>
        /// <param name="discapacitado"></param>
        /// <param name="idEstado"></param>
        /// <param name="apellidonombreapoderado"></param>
        /// <returns></returns>
        IList<IBeneficiario> GetBeneficiarioSinCuenta(string nombre, string apellido, string cuil,
                                                      string numeroDocumento, int idPrograma, string conApoderados, string modalidad, string discapacitado, string altatemprana, int idEstado, string apellidonombreapoderado, int idEtapa, string nrotramite, int tipoprograma = 0, int subprograma = 0);

        /// <summary>
        /// Obtiene los Beneficiario que no tiene cuentas de bancos (para la paginación).
        /// <para>Lugar de Uso: BeneficiarioServicio</para> 
        /// </summary>
        /// <param name="nombre">Nombre del Beneficiario</param>
        /// <param name="apellido">Apellido del Beneficiario</param>
        /// <param name="cuil">Cuil del Beneficiario</param>
        /// <param name="numeroDocumento">Número documento del Beneficiario</param>
        /// <param name="idPrograma">Programa del Beneficiario</param>
        ///  <param name="conApoderados">Si tiene Apoderados</param>
        /// <param name="discapacitado"></param>
        /// <param name="altatemprana"></param>
        /// <param name="modalidad">Modalidad</param>
        /// <param name="skip">Pagina que va Tomar(son parametros de paginación)</param>
        /// <param name="take">Cantidad que va tomar(son parametros de paginación)</param>
        /// <param name="idEstado"></param>
        /// <param name="apellidonombreapoderado"></param>
        /// <returns></returns>
        IList<IBeneficiario> GetBeneficiarioSinCuenta(string nombre, string apellido, string cuil,
                                                      string numeroDocumento, int idPrograma, string conApoderados, string discapacitado, string altatemprana, string modalidad, int idEstado, string apellidonombreapoderado, int skip, int take, int idEtapa);



        IList<IBeneficiario> GetBeneficiarioSinCuenta(string nombre, string apellido, string cuil,
                                                      string numeroDocumento, int idPrograma, string conApoderados, string discapacitado, string altatemprana, string modalidad, int idEstado, string apellidonombreapoderado, int skip, int take, int excluirprograma, int idEtapa);




        /// <summary>
        /// Obtiene los Beneficiario que no tiene cuentas de bancos for fecha de Solicitud. 
        /// <para>Lugar de Uso: BeneficiarioServicio</para>
        /// </summary>
        /// <param name="nombre">Nombre del Beneficiario</param>
        /// <param name="apellido">Apellido del Beneficiario</param>
        /// <param name="cuil">Cuil del Beneficiario</param>
        /// <param name="numeroDocumento">Número documento del Beneficiario</param>
        /// <param name="idPrograma">Programa del Beneficiario</param>
        /// <param name="conApoderados">Si tiene Apoderados</param>
        /// <param name="modalidad">Modalidad</param>
        /// <param name="altatemprana"></param>
        /// <param name="discapacitado"></param>
        /// <param name="idEstado"></param>
        /// <param name="fechasolicitud"></param>
        /// <param name="apellidonombreapoderado"></param>
        /// <returns></returns>
        IList<IBeneficiario> GetBeneficiarioSinCuentaByFecha(string nombre, string apellido, string cuil,
                                                      string numeroDocumento, int idPrograma, string conApoderados, string modalidad, string discapacitado, string altatemprana, int idEstado, string apellidonombreapoderado, DateTime fechasolicitud);


        /// <summary>
        /// Obtiene los Beneficiarios con cuentas de bancos.
        /// <para>Lugar de Uso: BeneficiarioServicio, LiquidacionServicio</para>
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
        IList<IBeneficiario> GetBeneficiarioConCuenta(string nombre, string apellido, string cuil,
                                                     string numeroDocumento, int idPrograma, string conApoderados, string modalidad, string discapacitado, string altatemprana, int idEstado, string apellidonombreapoderado, int idEtapa, string nrotramite, int tipoprograma = 0, int subprograma = 0);

        /// <summary>
        /// Obtiene los Beneficiarios con cuentas de bancos.
        /// <para>Lugar de Uso: BeneficiarioServicio</para>
        /// </summary>
        /// <param name="nombre">Nombre del Beneficiario</param>
        /// <param name="apellido">Apellido del Beneficiario</param>
        /// <param name="cuil">Cuil del Beneficiario</param>
        /// <param name="numeroDocumento">Número documento del Beneficiario</param>
        /// <param name="idPrograma">Programa del Beneficiario</param>
        /// <param name="modalidad"></param>
        /// <param name="discapacitado"></param>
        /// <param name="altatemprana"></param>
        /// <param name="skip">Pagina que va Tomar(son parametros de paginación)</param>
        /// <param name="take">Cantidad que va tomar(son parametros de paginación)</param>
        /// <param name="conApoderados"></param>
        /// <param name="idEstado"></param>
        /// <param name="apellidonombreapoderado"></param>
        /// <returns></returns>
        IList<IBeneficiario> GetBeneficiarioConCuenta(string nombre, string apellido, string cuil,
                                                      string numeroDocumento, int idPrograma, string conApoderados, string modalidad, string discapacitado, string altatemprana, int idEstado, string apellidonombreapoderado, int skip, int take, int idEtapa);

        IList<IBeneficiario> GetBeneficiarioConCuenta(string nombre, string apellido, string cuil,
                                                      string numeroDocumento, int idPrograma, string conApoderados, string modalidad, string discapacitado, string altatemprana, int idEstado, string apellidonombreapoderado, int skip, int take, int excluirprograma, int idEtapa);

        /// <summary>
        /// Obtinene los Beneficiario habilitados para una liquidacion especiafica por programa y concepto
        /// <para>Lugar de Uso:LiquidacionServicio</para>
        /// </summary>
        /// <param name="idPrograma">Programa de la Liquidación</param>
        /// <param name="idConcepto">Concepto de la Liquidación </param>
        /// <param name="fecinipago">Concepto de la Liquidación </param>
        /// <returns></returns>
        IList<IBeneficiario> GetBeneficiarioForLiquidacion(int idPrograma, int idConcepto, DateTime fecinipago);

        /// <summary>
        /// Actualiza el Numero de cuenta y CBU del Veneficiario cuando se realiza la importación de las cuentas enviadas al Banco para su generación
        /// <para>Lugar de Uso: BeneficiarioServicio</para>
        /// </summary>
        /// <param name="idBeneficiario">Clave identificadora del beneficiario</param>
        /// <param name="nroCuenta">Cuenta a actualizar</param>
        /// <param name="cbu">CBU a actualizar</param>
        /// <param name="idsucursal">Sucursal enviada por el banco</param>
        /// <returns></returns>
        bool UpdateCuentaCbu(int idBeneficiario, string nroCuenta, string cbu, int idsucursal);



        /// <summary>
        /// Obtiene un Beneficiario en particular.
        /// <para>Lugares de Uso: BeneficiarioServicio.</para>
        /// </summary>
        /// <param name="idFicha">Fichas del  Beneficiario</param>
        /// <returns></returns>
        IList<IBeneficiario> GetBeneficiarioByFicha(int idFicha);


        /// <summary>
        /// Da de alta un nuevo beneficiario.
        /// <para>Lugares de Uso: FichaServicio.</para>
        /// </summary>
        /// <param name="beneficiario">Ficha del Beneficiario</param>
        /// <returns></returns>
        int AddBeneficiario(IBeneficiario beneficiario);


        /// <summary>
        /// Obtiene el Listado de Archivos Generados.
        /// <para>Lugares de Uso: BeneficiarioController.</para>
        /// </summary>
        /// <returns></returns>
        IList<IArchivo> GetArchivos();


        /// <summary>
        /// Obtiene el Listado de Archivos Generados.
        /// <para>Lugares de Uso: BeneficiarioController.</para>
        /// </summary>
        /// <returns></returns>
        bool ClearFechaSolicitudArchivobyFecha(DateTime fecha);


        IList<IBeneficiario> GetBeneficiariosCompleto(string nombre, string apellido, string cuil, string numeroDocumento, int idPrograma, string conApoderados, string modalidad, string discapacitado, string altaTemprana, int idEstado, string apellidonombreapoderado);

        IList<IBeneficiario> GetBeneficiariosCompleto(int[] idbeneficiarios, int idprograma);

        IList<IBeneficiario> GetBeneficiariosAnexoActivosRetenidos(int idprograma);

        /// <summary>
        /// Obtiene un Beneficiario en particular.
        /// <para>Lugares de Uso: BeneficiarioServicio.</para>
        /// </summary>
        /// <param name="numerodocumento">Número documento del Beneficiario</param>
        /// <returns></returns>
        IBeneficiario GetBeneficiarioByNroDocumento(string numerodocumento);

        bool ClearFechaNotificacion(int idficha);

        IList<IBeneficiario> GetBeneficiarioForLiquidacionExcluidos(int idPrograma, int idConcepto, DateTime fecinipago);

        IList<IBeneficiario> GetBeneficiarioForReportes(int[] idbeneficiarios);

        IList<IBeneficiario> GetBeneficiariosHorariosArt(int programa, DateTime fecInicioBenef, DateTime fecBenefHasta);

        /// <summary>
        /// Obtinene los Beneficiario habilitados y pendientes para una liquidacion especiafica por programa y concepto
        /// <para>Lugar de Uso:LiquidacionServicio</para>
        /// </summary>
        /// <param name="idPrograma">Programa de la Liquidación</param>
        /// <param name="idConcepto">Concepto de la Liquidación </param>
        /// <param name="fecinipago">Fecha de pago de la Liquidación </param>
        /// <returns></returns>
        IList<IBeneficiario> GetBeneficiariosRetenidosForAnexoI(int idPrograma, int idConcepto, DateTime fecinipago);


        IList<IBeneficiario> GetBeneficiariosCompletoBis(int[] beneficiarios, int idPrograma);//(string nombre, string apellido, string cuil, string numeroDocumento, int idPrograma, string conApoderados, string modalidad, string discapacitado, string altaTemprana, int idEstado, string apellidonombreapoderado);
        //IEnumerable<IBeneficiario> QBeneficiarioExportarBis(string nombre, string apellido, string cuil, string numeroDocumento, int idPrograma, string conApoderados, string modalidad, string discapacitado, string altaTemprana, int idEstado, string apellidonombreapoderado);

        IList<IBeneficiario> GetBeneficiariosCompleto(string nombre, string apellido, string cuil, string numeroDocumento, int idPrograma, string conApoderados, string modalidad, string discapacitado, string altaTemprana, int idEstado, string apellidonombreapoderado, int idEtapa, string conCuenta);

        IList<IBeneficiario> GetBeneficiarioFichaAll(int idFicha);
        IList<IBeneficiario> GetBeneficiariosHorarios(int idPrograma, int tipoprograma, int estadoBenef, DateTime fecInicioBenef, DateTime fecBenefHasta);
    }
}
