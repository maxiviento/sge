using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGE.Model.Entidades.Interfaces
{
    public interface IReportes_PPP
    {


        #region Primitive Properties

        Int64 ID_FICHA { get; set; }
        string APELLIDO { get; set; }
        string NOMBRE { get; set; }
        string CUIL { get; set; }
        string NUMERO_DOCUMENTO { get; set; }
        DateTime FER_NAC { get; set; }
        string CALLE { get; set; }
        string NUMERO { get; set; }
        string PISO { get; set; }
        string DPTO { get; set; }
        string MONOBLOCK { get; set; }
        string PARCELA { get; set; }
        string MANZANA { get; set; }
        string ENTRECALLES { get; set; }
        string BARRIO { get; set; }
        string CODIGO_POSTAL { get; set; }
        Int64? ID_LOCALIDAD { get; set; }
        string N_LOCALIDAD { get; set; }
        Int64? ID_DPTO { get; set; }
        string N_DEPARTAMENTO { get; set; }
        string TEL_FIJO { get; set; }
        string TEL_CELULAR { get; set; }
        string CONTACTO { get; set; }
        string MAIL { get; set; }
        string TIENE_HIJOS { get; set; }
        Int64 CANTIDAD_HIJOS { get; set; }
        string ES_DISCAPACITADO { get; set; }
        string TIENE_DEF_MOTORA { get; set; }
        string TIENE_DEF_MENTAL { get; set; }
        string TIENE_DEF_SENSORIAL { get; set; }
        string TIENE_DEF_PSICOLOGICA { get; set; }
        string DEFICIENCIA_OTRA { get; set; }
        string CERTIF_DISCAP { get; set; }
        string SEXO { get; set; }
        Int64 TIPO_FICHA { get; set; }
        Int64 ESTADO_CIVIL { get; set; }
        DateTime? FEC_SIST { get; set; }
        string ORIGEN_CARGA { get; set; }
        string NRO_TRAMITE { get; set; }
        Int64? ID_CAJA { get; set; }
        Int64? ID_TIPO_RECHAZO { get; set; }
        string N_TIPO_RECHAZO { get; set; }
        Int64? ID_EST_FIC { get; set; }
        string N_ESTADO_FICHA { get; set; }
        string NIVEL_ESCOLARIDAD { get; set; }
        Int64? MODALIDAD { get; set; }
        string TAREAS { get; set; }
        string ALTA_TEMPRANA { get; set; }
        DateTime? AT_FECHA_INICIO { get; set; }
        DateTime? AT_FECHA_CESE { get; set; }
        Int64? ID_MOD_CONT_AFIP { get; set; }
        string MOD_CONT_AFIP { get; set; }
        DateTime? FEC_MODIF { get; set; }
        Int64? ID_EMP { get; set; }
        string RAZON_SOCIAL { get; set; }
        string EMP_CUIT { get; set; }
        string COD_ACT { get; set; }
        Int64? CANT_EMP { get; set; }
        string EMP_CALLE { get; set; }
        string EMP_NUMERO { get; set; }
        string EMP_PISO { get; set; }
        string EMP_DPTO { get; set; }
        string EMP_CP { get; set; }
        Int64? ID_LOC { get; set; }
        string EMP_N_LOCALIDAD { get; set; }
        string EMP_N_DEPARTAMENTO { get; set; }
        Int64? EU_ID_USUARIO { get; set; }
        string EU_NOMBRE { get; set; }
        string EMP_APELLIDO { get; set; }
        string EU_MAIL { get; set; }
        string EU_TELEFONO { get; set; }
        Int64? ID_BENEFICIARIO { get; set; }
        Int64? BEN_ID_ESTADO { get; set; }
        string BEN_N_ESTADO { get; set; }
        Int64? BEN_NRO_CTA { get; set; }
        Int64? BEN_ID_SUCURSAL { get; set; }
        string BEN_COD_SUC { get; set; }
        string BEN_SUCURSAL { get; set; }
        DateTime? BEN_FEC_SOL_CTA { get; set; }
        string NOTIFICADO { get; set; }
        DateTime? FEC_NOTIF { get; set; }
        DateTime? BEN_FEC_INICIO { get; set; }
        DateTime? BEN_FEC_BAJA { get; set; }
        string TIENE_APODERADO { get; set; }
        Int64? ID_APODERADO { get; set; }
        string APO_APELLIDO { get; set; }
        string APO_NOMBRE { get; set; }
        string APO_DNI { get; set; }
        string APO_CUIL { get; set; }
        string APO_SEXO { get; set; }
        Int64? APO_NRO_CTA { get; set; }
        Int64? APO_ID_SUC { get; set; }
        string APO_COD_SUC { get; set; }
        string APO_SUCURSAL { get; set; }
        DateTime? APO_FEC_SOL_CTA { get; set; }
        DateTime? APO_FEC_NAC { get; set; }
        string APO_CALLE { get; set; }
        string APO_NRO { get; set; }
        string APO_PISO { get; set; }
        string APO_MONOBLOCK { get; set; }
        string APO_DPTO { get; set; }
        string APO_PARCELA { get; set; }
        string APO_MANZANA { get; set; }
        string APO_ENTRE_CALLES { get; set; }
        string APO_BARRIO { get; set; }
        string APO_CP { get; set; }
        Int64? APO_ID_LOC { get; set; }
        string APO_LOCALIDAD { get; set; }
        Int64? APO_ID_DEPTO { get; set; }
        string APO_DEPARTAMENTO { get; set; }
        string APO_TELEFONO { get; set; }
        string APO_CELULAR { get; set; }
        string APO_EMAIL { get; set; }
        Int64? IDETAPA { get; set; }
        Int64? AP_ID_ESTADO { get; set; }
        string AP_N_ESTADO { get; set; }
        Int64? ID_SUBPROGRAMA { get; set; }
        string SUBPROGRAMA { get; set; }
        Int64? TIPO_PPP { get; set; }
        string SEDE_ROTATIVA { get; set; }
        string HORARIO_ROTATIVO { get; set; }
        string N_DESCRIPCION_T { get; set; }
        string N_CURSADO_INS { get; set; }
        DateTime? EGRESO { get; set; }
        string COD_USO_INTERNO { get; set; }
        string CARGA_HORARIA { get; set; }
        string CH_CUAL { get; set; }
        Int64? ID_CARRERA { get; set; }
        string N_CARRERA { get; set; }
        Int64? ID_INSTITUCION { get; set; }
        string N_INSTITUCION { get; set; }
        string CONSTANCIA_EGRESO { get; set; }
        string MATRICULA { get; set; }
        string CONSTANCIA_CURSO { get; set; }


        #endregion

        
    }
}
