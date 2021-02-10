using System;
using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;

namespace SGE.Model.Entidades
{
    public class Ficha : IFicha
    {
        public Ficha()
        {
            LocalidadFicha = new Localidad();
            EscuelaFicha = new Escuela();
            CarreraFicha = new Carrera();
            BeneficiarioFicha = new Beneficiario();
            Subprogramas = new Subprograma();
        }

        public int IdFicha { get; set; }
        public int? TipoFicha { get; set; }
        public string Cuil { get; set; }
        public string Apellido { get; set; }
        public string Nombre { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public int? TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public string Calle { get; set; }
        public string Numero { get; set; }
        public string Piso { get; set; }
        public string Dpto { get; set; }
        public string Monoblock { get; set; }
        public string Parcela { get; set; }
        public string Manzana { get; set; }
        public string EntreCalles { get; set; }
        public string Barrio { get; set; }
        public string CodigoPostal { get; set; }
        public int? IdLocalidad { get; set; }
        public string Localidad { get; set; }
        public string TelefonoFijo { get; set; }
        public string TelefonoCelular { get; set; }
        public string Contacto { get; set; }
        public string Mail { get; set; }
        public bool TieneHijos { get; set; }
        public int? CantidadHijos { get; set; }
        public bool EsDiscapacitado { get; set; }
        public bool TieneDeficienciaMotora { get; set; }
        public bool TieneDeficienciaMental { get; set; }
        public bool TieneDeficienciaSensorial { get; set; }
        public bool TieneDeficienciaPsicologia { get; set; }
        public string DeficienciaOtra { get; set; }
        public bool CertificadoDiscapacidad { get; set; }
        public string Sexo { get; set; }
        public int? EstadoCivil { get; set; }
        public string EstadoCivilDesc { get; set; }
        public string CodigoSeguridad { get; set; }
        public string NombreTipoFicha { get; set; }
        public string NombreDepartamento { get; set; }
        public int? IdDepartamento { get; set; }

        public int? IdCarreraTerciaria { get; set; }
        public string NombreCarreraTerciaria { get; set; }

        public int? IdCarreraUniversitaria { get; set; }
        public string NombreCarreraUniversitaria { get; set; }

        public int? IdInstitucionTerciaria { get; set; }
        public string NombreInstitucionTerciaria { get; set; }
        public int? IdInstitucionUniversitaria { get; set; }
        public string NombreInstitucionUniversitaria { get; set; }

        public int? IdSectorTerciaria { get; set; }
        public string NombreSectorTerciaria { get; set; }
        public int? IdSectorUniversitaria { get; set; }
        public string NombreSectorUniversitaria { get; set; }


        public int? IdEscuelaTerciaria { get; set; }
        public int? IdEscuelaUniversitaria { get; set; }
        public decimal? PromedioTerciaria { get; set; }
        public decimal? PromedioUniversitaria { get; set; }
        public string OtraCarreraTerciaria { get; set; }
        public string OtraCarreraUniversitaria { get; set; }
        public string OtraInstitucionTerciaria { get; set; }
        public string OtraInstitucionUniversitaria { get; set; }
        public string OtroSectorTerciaria { get; set; }
        public string OtroSectorUniversitaria { get; set; }
        public int? IdDepartamentoEscTerc { get; set; }
        public int? IdDepartamentoEscUniv { get; set; }
        public int? IdLocalidadEscTerc { get; set; }
        public int? IdLocalidadEscUniv { get; set; }
        public int? IdEstadoFicha { get; set; }
        public string NombreEstadoFicha { get; set; }

        public IFichaPPP FichaPpp { get; set; }
        public IFichaVAT FichaVat { get; set; }
        public IFichaPPPP FichaPppp { get; set; }

        public IFichaConfVos FichaConfVos { get; set; }
        //public IFichaEfectoresSociales FichaEfectoresSociales { get; set; }
        //ficha reconversion
        public IFichaReconversion FichaReconversion { get; set; }

        public IFichaEfectoresSociales FichaEfectores { get; set; }

        public string NombreProyecto { get; set; }
        public ISubprograma Subprogramas { get; set; }
        public int? IdSubprograma { get; set; }
        public string Subprograma { get; set; }

        public int? IdProyecto { get; set; }

        public int? CantidadAcapacitar { get; set; }
        public DateTime? FechaInicioProyecto { get; set; }
        public DateTime? FechaFinProyecto { get; set; }
        public int? MesesDuracionProyecto { get; set; }
        public string AportesDeLaEmpresa { get; set; }
        public string ApellidoTutor { get; set; }
        public string NombreTutor { get; set; }
        public string NroDocumentoTutor { get; set; }
        public string TelefonoTutor { get; set; }
        public string EmailTutor { get; set; }
        public string PuestoTutor { get; set; }


        public int? IdNivelEscolaridad { get; set; }
        public bool Cursando { get; set; }
        public bool Finalizado { get; set; }
        public int? IdSede { get; set; }
        public int? Modalidad { get; set; }
        public bool DeseaTermNivel { get; set; }
        public string CuitEmpresa { get; set; }
        public DateTime? FechaInicioActividad { get; set; }
        public DateTime? FechaFinActividad { get; set; }
        public int? IdModalidadAfip { get; set; }
        public string ModalidadAfip { get; set; }
        public string AltaTemprana { get; set; }
        public IEnumerable<ITipoRechazo> TipoRechazos { get; set; }
        public ILocalidad LocalidadFicha { get; set; }
        public string NombreEscuela { get; set; }
        public IEscuela EscuelaFicha { get; set; }
        public ICarrera CarreraFicha { get; set; }
        public IBeneficiario BeneficiarioFicha { get; set; }
        //Ficha PPPP
        public decimal? Promedio { get; set; }
        public DateTime? FechaEgreso { get; set; }
        public short? IdTitulo { get; set; }
        public short? AniosAportes { get; set; }

        public int? IdEmpresa { get; set; }
        public string Tareas { get; set; }

        public DateTime? FechaSistema { get; set; }
        public int? IdUsuarioSistema { get; set; }
        public string UsuarioSistema { get; set; }

        // 21/05/2013 - DI CAMPLI LEANDRO - DATOS DE LA ETAPA
        public int? idEtapa { get; set; }
        public string NombreEtapa { get; set; }

        // 22/05/2013 - DI CAMPLI LEANDRO
        public string OrigenCarga { get; set; }
        public string NroTramite { get; set; }
        public int idCaja { get; set; }
        public int IdTipoRechazo { get; set; }
        public string rechazo { get; set; }


        // 04/12/2013 - MONTO EFECTORES SOCIALES
        public decimal monto_efe { get; set; }


        // 22/04/2014 - LEANDRO DI CAMPLI
        public int? Abandono_Escuela { get; set; }
        public string Trabaja_Trabajo { get; set; }
        public string cursa { get; set; } //Cursado 2014
        public int? Ultimo_Cursado { get; set; }
        public string pit { get; set; }
        public string Centro_Adultos { get; set; }
        public string Progresar { get; set; }
        public int? Actividades { get; set; }
        public string Benef_progre { get; set; }
        public string Autoriza_Tutor { get; set; }
        public string Apoderado { get; set; }

        public string Apo_Cuil { get; set; }
        public string Apo_Apellido { get; set; }
        public string Apo_Nombre { get; set; }
        public DateTime? Apo_Fer_Nac { get; set; }
        public int? Apo_Tipo_Documento { get; set; }
        public string Apo_Numero_Documento { get; set; }
        public string Apo_Calle { get; set; }
        public string Apo_Numero { get; set; }
        public string Apo_Piso { get; set; }
        public string Apo_Dpto { get; set; }
        public string Apo_Monoblock { get; set; }
        public string Apo_Parcela { get; set; }
        public string Apo_Manzana { get; set; }
        public string Apo_Barrio { get; set; }
        public string Apo_Codigo_Postal { get; set; }
        public int? Apo_Id_Localidad { get; set; }
        public string Apo_Telefono { get; set; }
        public string Apo_Tiene_Hijos { get; set; }
        public int? Apo_Cantidad_Hijos { get; set; }
        public string Apo_Sexo { get; set; }
        public int? Apo_Estado_Civil { get; set; }
        // 25/04/2014 - Di Campli
        public int? IdEscuelaConfVos { get; set; }


        // 16/04/2015 - nuevos campos
        public int? ID_FACILITADOR { get; set; }
        public int? ID_GESTOR { get; set; }
        public int? ID_CURSO { get; set; }
        public int? ID_SUBSIDIARIA { get; set; }
        //

        // 17/04/2015
        public int? ID_ABANDONO_CUR { get; set; }
        public short? TRABAJO_CONDICION { get; set; }
        public string SENAF { get; set; }
        public string PERTENECE_ONG { get; set; }
        public int? ID_ONG { get; set; }
        public string APO_CELULAR { get; set; }
        public string APO_MAIL { get; set; }

        // 17/08/2016 - Di Campli
        public int? IdEscuelaPpp { get; set; }
        public string ppp_aprendiz { get; set; }

        // 16/10/2016 - Di Campli
        public int? idBarrio { get; set; }

        public int? TipoPrograma { get; set; }

        // 04/04/2017
        public string sede_rotativa { get; set; }
        public string horario_rotativo { get; set; }

        // 08/01/2018
        public string empadronado { get; set; }
        public int idEmpadronado { get; set; }

        // 14/07/2018
        public string n_descripcion_t { get; set; }
        public string n_cursado_ins { get; set; }
        public DateTime? egreso { get; set; }
        public string cod_uso_interno { get; set; }
        public string carga_horaria { get; set; }
        public string ch_cual { get; set; }
        public int? id_carrera { get; set; }
        public string CONSTANCIA_EGRESO { get; set; }
        public string MATRICULA { get; set; }
        public string CONSTANCIA_CURSO { get; set; }

        public int? TipoProgramaAnt { get; set; }

        // 23/09/2018
        public int? co_financiamiento { get; set; }

    }
}
