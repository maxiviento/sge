using System;
using System.Collections.Generic;

namespace SGE.Model.Entidades.Interfaces
{
    public interface IFicha : IComunDatos
    {
        int IdFicha { get; set; }
        int? TipoFicha { get; set; }
        string Cuil { get; set; }
        string Apellido { get; set; }
        string Nombre { get; set; }
        DateTime? FechaNacimiento { get; set; }
        int? TipoDocumento { get; set; }
        string NumeroDocumento { get; set; }
        string Calle { get; set; }
        string Numero { get; set; }
        string Piso { get; set; }
        string Dpto { get; set; }
        string Monoblock { get; set; }
        string Parcela { get; set; }
        string Manzana { get; set; }
        string EntreCalles { get; set; }
        string Barrio { get; set; }
        string CodigoPostal { get; set; }
        int? IdLocalidad { get; set; }
        string Localidad { get; set; }
        string TelefonoFijo { get; set; }
        string TelefonoCelular { get; set; }
        string Contacto { get; set; }
        string Mail { get; set; }
        bool TieneHijos { get; set; }
        int? CantidadHijos { get; set; }
        bool EsDiscapacitado { get; set; }
        bool TieneDeficienciaMotora { get; set; }
        bool TieneDeficienciaMental { get; set; }
        bool TieneDeficienciaSensorial { get; set; }
        bool TieneDeficienciaPsicologia { get; set; }
        string DeficienciaOtra { get; set; }
        bool CertificadoDiscapacidad { get; set; }
        string Sexo { get; set; }
        int? EstadoCivil { get; set; }
        string EstadoCivilDesc { get; set; }
        string CodigoSeguridad { get; set; }
        string NombreTipoFicha { get; set; }
        string NombreDepartamento { get; set; }
        int? IdDepartamento { get; set; }

        int? IdCarreraTerciaria { get; set; }
        string NombreCarreraTerciaria { get; set; }
        int? IdCarreraUniversitaria { get; set; }
        string NombreCarreraUniversitaria { get; set; }

        int? IdInstitucionTerciaria { get; set; }
        string NombreInstitucionTerciaria { get; set; }
        int? IdInstitucionUniversitaria { get; set; }
        string NombreInstitucionUniversitaria { get; set; }

        int? IdSectorTerciaria { get; set; }
        string NombreSectorTerciaria { get; set; }
        int? IdSectorUniversitaria { get; set; }
        string NombreSectorUniversitaria { get; set; }


        int? IdEscuelaTerciaria { get; set; }
        int? IdEscuelaUniversitaria { get; set; }
        decimal? PromedioTerciaria { get; set; }
        decimal? PromedioUniversitaria { get; set; }
        string OtraCarreraTerciaria { get; set; }
        string OtraCarreraUniversitaria { get; set; }
        string OtraInstitucionTerciaria { get; set; }
        string OtraInstitucionUniversitaria { get; set; }
        string OtroSectorTerciaria { get; set; }
        string OtroSectorUniversitaria { get; set; }
        int? IdDepartamentoEscTerc { get; set; }
        int? IdDepartamentoEscUniv { get; set; }
        int? IdLocalidadEscTerc { get; set; }
        int? IdLocalidadEscUniv { get; set; }
        int? IdEstadoFicha { get; set; }
        string NombreEstadoFicha { get; set; }

        IFichaPPP FichaPpp { get; set; }
        IFichaVAT FichaVat { get; set; }
        IFichaPPPP FichaPppp { get; set; }
        IFichaReconversion FichaReconversion { get; set; }
        IFichaEfectoresSociales FichaEfectores { get; set; }

        IFichaConfVos FichaConfVos { get; set; }
        
        string NombreProyecto { get; set; }
        ISubprograma Subprogramas { get; set; }
        int? IdSubprograma { get; set; }
        string Subprograma { get; set; }

        int? IdProyecto { get; set; }
        int? CantidadAcapacitar { get; set; }
        DateTime? FechaInicioProyecto { get; set; }
        DateTime? FechaFinProyecto { get; set; }
        int? MesesDuracionProyecto { get; set; }

        string AportesDeLaEmpresa { get; set; }

        string ApellidoTutor { get; set; }
        string NombreTutor { get; set; }
        string NroDocumentoTutor { get; set; }
        string TelefonoTutor { get; set; }
        string EmailTutor { get; set; }
        string PuestoTutor { get; set; }
        //---

        int? IdNivelEscolaridad { get; set; }
        bool Cursando { get; set; }
        bool Finalizado { get; set; }
        int? IdEmpresa { get; set; }
        int? IdSede { get; set; }
        int? Modalidad { get; set; }
        string Tareas { get; set; }
        bool DeseaTermNivel { get; set; }
        string CuitEmpresa { get; set; }
        DateTime? FechaInicioActividad { get; set; }
        DateTime? FechaFinActividad { get; set; }
        int? IdModalidadAfip { get; set; }
        string ModalidadAfip { get; set; }

        string AltaTemprana { get; set; }

        IEnumerable<ITipoRechazo> TipoRechazos { get; set; }
        ILocalidad LocalidadFicha { get; set; }

        string NombreEscuela { get; set; }

        IEscuela EscuelaFicha { get; set; }
        ICarrera CarreraFicha { get; set; }

        IBeneficiario BeneficiarioFicha { get; set; }

        decimal? Promedio { get; set; }
        DateTime? FechaEgreso { get; set; }
        short? IdTitulo { get; set; }
        short? AniosAportes { get; set; }
        // 21/05/2013 - DI CAMPLI LEANDRO - DATOS DE LA ETAPA
        int? idEtapa { get; set; }
        string NombreEtapa { get; set; }

        // 22/05/2013 - DI CAMPLI LEANDRO
        string OrigenCarga { get; set; }
        string NroTramite { get; set; }
        int idCaja { get; set; }
        int IdTipoRechazo { get; set; }
        string rechazo { get; set; }

        // 04/12/2013 - MONTO EFECTORES SOCIALES
        decimal monto_efe { get; set; }

        // 22/04/2014 - LEANDRO DI CAMPLI
         int? Abandono_Escuela { get; set; }
         string Trabaja_Trabajo { get; set; }
         string cursa { get; set; } //Cursado 2014
         int? Ultimo_Cursado { get; set; }
         string pit { get; set; }
         string Centro_Adultos { get; set; }
         string Progresar { get; set; }
         int? Actividades { get; set; }
         string Benef_progre { get; set; }
         string Autoriza_Tutor { get; set; }
         string Apoderado { get; set; }

         string Apo_Cuil { get; set; }
         string Apo_Apellido { get; set; }
         string Apo_Nombre { get; set; }
         DateTime? Apo_Fer_Nac { get; set; }
         int? Apo_Tipo_Documento { get; set; }
         string Apo_Numero_Documento { get; set; }
         string Apo_Calle { get; set; }
         string Apo_Numero { get; set; }
         string Apo_Piso { get; set; }
         string Apo_Dpto { get; set; }
         string Apo_Monoblock { get; set; }
         string Apo_Parcela { get; set; }
         string Apo_Manzana { get; set; }
         string Apo_Barrio { get; set; }
         string Apo_Codigo_Postal { get; set; }
         int? Apo_Id_Localidad { get; set; }
         string Apo_Telefono { get; set; }
         string Apo_Tiene_Hijos { get; set; }
         int? Apo_Cantidad_Hijos { get; set; }
         string Apo_Sexo { get; set; }
         int? Apo_Estado_Civil { get; set; }

        // 25/04/2014 - Di Campli
         int? IdEscuelaConfVos { get; set; }

         // 16/04/2015 - nuevos campos
         int? ID_FACILITADOR { get; set; }
         int? ID_GESTOR { get; set; }
         int? ID_CURSO { get; set; }
         int? ID_SUBSIDIARIA { get; set; }
         //

         // 17/04/2015
         int? ID_ABANDONO_CUR { get; set; }
         short? TRABAJO_CONDICION { get; set; }
         string SENAF { get; set; }
         string PERTENECE_ONG { get; set; }
         int? ID_ONG { get; set; }
         string APO_CELULAR { get; set; }
         string APO_MAIL { get; set; }
         // 17/08/2016 - Di Campli
         int? IdEscuelaPpp { get; set; }
         string ppp_aprendiz { get; set; }
         // 16/10/2016 - Di Campli
         int? idBarrio { get; set; }

         int? TipoPrograma { get; set; }
         // 04/04/2017
         string sede_rotativa { get; set; }
         string horario_rotativo { get; set; }

         // 08/01/2018
         string empadronado { get; set; }
         int idEmpadronado { get; set; }

         // 14/07/2018
         string n_descripcion_t { get; set; }
         string n_cursado_ins { get; set; }
         DateTime? egreso { get; set; }
         string cod_uso_interno { get; set; }
         string carga_horaria { get; set; }
         string ch_cual { get; set; }
         int? id_carrera { get; set; }
         string CONSTANCIA_EGRESO { get; set; }
         string MATRICULA { get; set; }
         string CONSTANCIA_CURSO { get; set; }

         int? TipoProgramaAnt { get; set; }
         // 23/09/2018
         int? co_financiamiento { get; set; }
    }
}
