using System;
using System.Collections.Generic;
using SGE.Model.Entidades.Interfaces;
using SGE.Servicio.Vista;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces.Shared;

namespace SGE.Servicio.VistaInterfaces
{
    public interface IFichaVista
    {
        int IdFicha { get; set; }
        int TipoFicha { get; set; }
        string Cuil { get; set; }
        string Apellido { get; set; }
        string Nombre { get; set; }
        DateTime FechaNacimineto { get; set; }
        int TipoDocumento { get; set; }
        ComboBox TipoDocumentoCombo { get; set; }
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
        string TelefonoFijo { get; set; }
        string TelefonoCelular { get; set; }
        string Contacto { get; set; }
        string Mail { get; set; }
        bool TieneHijos { get; set; }
        int CantidadHijos { get; set; }
        bool EsDiscapacitado { get; set; }
        bool TieneDeficienciaMotora { get; set; }
        bool TieneDeficienciaMental { get; set; }
        bool TieneDeficienciaSensorial { get; set; }
        bool TieneDeficienciaPsicologia { get; set; }
        string DeficienciaOtra { get; set; }
        bool CertificadoDiscapacidad { get; set; }
        ComboBox Sexo { get; set; }
        string EstadoCivilDesc { get; set; }
        string CodigoSeguridad { get; set; }
        string NombreTipoFicha { get; set; }
        string Accion { get; set; }
        IPager Pager { get; set; }
        ComboBox Localidades { get; set; }
        int IdDepartamento { get; set; }
        ComboBox Departamentos { get; set; }
        ComboBox SectorProductivo { get; set; }
        ComboBox Institucion { get; set; }
        ComboBox Carrera { get; set; }
        int IdEscuela { get; set; }
        decimal? Promedio { get; set; }
        string OtraCarrera { get; set; }
        string OtraInstitucion { get; set; }
        string OtroSector { get; set; }
        ComboBox DepartamentosEscuela { get; set; }
        ComboBox LocalidadesEscuela { get; set; }
        ComboBox Escuelas { get; set; }
        ComboBox EstadoCivil { get; set; }
        int IdInstitucionTerciaria { get; set; }
        int IdLocalidadEscTerc { get; set; }
        int IdInstitucionUniversitaria { get; set; }
        int IdLocalidadEscUniv { get; set; }
        int IdConvertirA { get; set; }
        string ConvertirA { get; set; }
        bool CambioTipo { get; set; }
        int? IdEstadoFicha { get; set; }
        ComboBox Estado { get; set; }
        IList<TipoRechazoVista> TipoRechazos { get; set; }
        IFichaPPP FichaPpp { get; set; }
        IFichaVAT FichaVat { get; set; }
        IFichaPPPP FichaPppp { get; set; }

        IFichaReconversion FichaReconversion { get; set; }
        IFichaEfectoresSociales FichaEfectores { get; set; }

        ComboBox Subprogramas { get; set; }
        int? IdSubprograma { get; set; }
        int? IdProyecto { get; set; }
        string NombreProyecto { get; set; }
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

        int IdNivelEscolaridad { get; set; }
        int Cursando { get; set; }

        int? IdEmpresa { get; set; }
        int? IdEmpresaInicial { get; set; }

        string CuitEmpresa { get; set; }
        string DescripcionEmpresa { get; set; }
        string EmpresaLocalidad { get; set; }
        string EmpresaCodigoActividad { get; set; }
        string EmpresaDomicilioLaboralIdem { get; set; }
        short? EmpresaCantidadEmpleados { get; set; }
        string EmpresaCalle { get; set; }
        string EmpresaNumero { get; set; }
        string EmpresaPiso { get; set; }
        string EmpresaDpto { get; set; }
        string EmpresaCodigoPostal { get; set; }
        string Tareas { get; set; }
        string LugarLlamada { get; set; }
        int IdLlamada { get; set; }

        IComboBox ComboTipoFicha { get; set; }
        IComboBox ComboEstadoFicha { get; set; }
        IComboBox DomicilioLaboralIdem { get; set; }
        IComboBox LocalidadesEmpresa { get; set; }

        DateTime? FechaInicioActividad { get; set; }
        DateTime? FechaFinActividad { get; set; }
        int? IdModalidadAfip { get; set; }
        string ModalidadAfip { get; set; }
        IComboBox ComboModalidadAfip { get; set; }
        IComboBox ComboUniversidad { get; set; }
        IComboBox ComboTitulos { get; set; }
        int IdSede { get; set; }
        IComboBox LocalidadesSedes { get; set; }

        //Horarios Empresa

        List<ComboBox> HorarioLunes { get; set; }
        List<ComboBox> HorarioMartes { get; set; }
        List<ComboBox> HorarioMiercoles { get; set; }
        List<ComboBox> HorarioJueves { get; set; }
        List<ComboBox> HorarioViernes { get; set; }
        List<ComboBox> HorarioSabado { get; set; }
        List<ComboBox> HorarioDomingo { get; set; }

        List<string> ObsLunes { get; set; }
        List<string> ObsMartes { get; set; }
        List<string> ObsMiercoles { get; set; }
        List<string> ObsJueves { get; set; }
        List<string> ObsViernes { get; set; }
        List<string> ObsSabado { get; set; }
        List<string> ObsDomingo { get; set; }

        string DiaSemana { get; set; }
        short HoraInicio { get; set; }
        short MinutoInicio { get; set; }
        short HoraFin { get; set; }
        short MinutoFin { get; set; }
        
        int? benefActivosEmp { get; set; }//contiene la cantidad de beneficiarios activos por programa por empresa
        string FechaCambioHorario { get; set; }

        // 04/12/2013 - PARA EFECTORES SOCIALES
        decimal monto { get; set; }

        // 07/04/2014 - SE AÑADE FICHA CONFIAMOS EN VOS
        IFichaConfVos FichaConfVos { get; set; }
        ComboBox TipoDocumentoApoCombo { get; set; }
        ComboBox SexoApo { get; set; }
        ComboBox LocalidadesApo { get; set; }
        ComboBox EstadoCivilApo { get; set; }
        ComboBox DepartamentosApo { get; set; }

        //int? Abandono_Escuela { get; set; }
        //string Trabaja_Trabajo { get; set; }
        //string cursa { get; set; } //Cursado 2014
        //int? Ultimo_Cursado { get; set; }
        //string pit { get; set; }
        //string Centro_Adultos { get; set; }
        //string Progresar { get; set; }
        //int? Actividades { get; set; }
        //string Benef_progre { get; set; }
        //string Autoriza_Tutor { get; set; }
        //string Apoderado { get; set; }

        //string Apo_Cuil { get; set; }
        //string Apo_Apellido { get; set; }
        //string Apo_Nombre { get; set; }
        //DateTime? Apo_Fer_Nac { get; set; }
        //int? Apo_Tipo_Documento { get; set; }
        //string Apo_Numero_Documento { get; set; }
        //string Apo_Calle { get; set; }
        //string Apo_Numero { get; set; }
        //string Apo_Piso { get; set; }
        //string Apo_Dpto { get; set; }
        //string Apo_Monoblock { get; set; }
        //string Apo_Parcela { get; set; }
        //string Apo_Manzana { get; set; }
        //string Apo_Barrio { get; set; }
        //string Apo_Codigo_Postal { get; set; }
        //int? Apo_Id_Localidad { get; set; }
        //string Apo_Telefono { get; set; }
        //string Apo_Tiene_Hijos { get; set; }
        //int? Apo_Cantidad_Hijos { get; set; }
        //string Apo_Sexo { get; set; }
        //int? Apo_Estado_Civil { get; set; }


        // 05/05/2014 - MENSAJE DE RESULTADOS MODIFICACIÓN
        string strMessageUpdate { get; set; }
        // 11/06/2014
        int idEtapa { get; set; }

        // 17/04/2015
        ComboBox AbandonoCursado { get; set; }
        // 18/06/2015
        IList<IAsistencia> asistencias { get; set; }

        // 16/08/2016
        string NroTramite { get; set; }
        string NombreEtapa { get; set; }

        // 16/10/2016
        IComboBox comboBarrios { get; set; }
        int? idBarrio { get; set; }

        // 11/12/2016
        string Subprograma { get; set; }
        IFichaTer FichaTER { get; set; }
        IFichaUni FichaUNI { get; set; }
        // 17/01/2017
        IComboBox ComboTiposPpp { get; set; }
        int? tipoPrograma { get; set; }

        // 20/01/2017
        IList<IRelacionFam> familiares { get; set; }

        int? benefActDiscEmp { get; set; }

        // 23/10/2019
        int? benefRetenidoscEmp { get; set; }
        int? benefTotalcEmp { get; set; }

        //01/08/2018
        ComboBox InstitucionSector { get; set; }
        int? idInstSector { get; set; }
        ComboBox CarreraSector { get; set; }
        int? idCarreraSector { get; set; }
        string n_descripcion_t { get; set; }
        string n_cursado_ins { get; set; }
        DateTime? egreso { get; set; }
        string cod_uso_interno { get; set; }
        string carga_horaria { get; set; }
        string ch_cual { get; set; }
        string CONSTANCIA_EGRESO { get; set; }
        string MATRICULA { get; set; }
        string CONSTANCIA_CURSO { get; set; }

        int? TipoProgramaAnt { get; set; }
        // 25/09/2018
        string SENAF { get; set; }
    }
}
