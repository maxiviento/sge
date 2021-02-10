using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SGE.Model.Entidades.Interfaces;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces;
using SGE.Servicio.VistaInterfaces.Shared;

namespace SGE.Servicio.Vista
{
    public class BeneficiarioVista : IBeneficiarioVista
    {
        public BeneficiarioVista()
        {
            Accion = "Ver";
            Estados = new ComboBox();
            ClearFechaSolicitud = false;
            Sucursales = new ComboBox();
            Monedas = new ComboBox();
            Subprogramas= new ComboBox();
        }

        public int IdBeneficiario { get; set; }
        public int IdFicha { get; set; }
        public int TipoFicha { get; set; }
        [Required(ErrorMessage = "Ingrese el CUIL.")]
        public string Cuil { get; set; }
        [Required(ErrorMessage = "Ingrese el Apellido.")]
        public string Apellido { get; set; }
        [Required(ErrorMessage="Ingrese el Nombre")]
        public string Nombre { get; set; }
        public DateTime FechaNacimineto { get; set; }
        public int TipoDocumento { get; set; }
        public ComboBox TipoDocumentoCombo { get; set; }
        [Required(ErrorMessage = "Ingrese el Nro. de Documento.")]
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
        public string TelefonoFijo { get; set; }
        public string TelefonoCelular { get; set; }
        public string Contacto { get; set; }
        public string Mail { get; set; }
        public bool TieneHijos { get; set; }
        public int CantidadHijos { get; set; }
        public bool EsDiscapacitado { get; set; }
        public bool TieneDeficienciaMotora { get; set; }
        public bool TieneDeficienciaMental { get; set; }
        public bool TieneDeficienciaSensorial { get; set; }
        public bool TieneDeficienciaPsicologia { get; set; }
        public string DeficienciaOtra { get; set; }
        public bool CertificadoDiscapacidad { get; set; }
        public ComboBox Sexo { get; set; }
        public string EstadoCivilDesc { get; set; }
        public string CodigoSeguridad { get; set; }
        public string NombreTipoFicha { get; set; }
        public int IdPrograma { get; set; }
        public int IdEstado { get; set; }
        public string Email { get; set; }
        public ComboBox Estados { get; set; }
        public string Programa { get; set; }
        public string Accion { get; set; }
        public IPager Pager { get; set; }
        public ComboBox Localidades { get; set; }
        public int IdDepartamento { get; set; }
        public ComboBox Departamentos { get; set; }
        public ComboBox SectorProductivo { get; set; }
        public ComboBox Institucion { get; set; }
        public ComboBox Carrera { get; set; }
        public int IdEscuela { get; set; }
        public decimal? Promedio { get; set; }
        public string OtraCarrera { get; set; }
        public string OtraInstitucion { get; set; }
        public string OtroSector { get; set; }
        public ComboBox DepartamentosEscuela { get; set; }
        public ComboBox LocalidadesEscuela { get; set; }
        public ComboBox Escuelas { get; set; }
        public ComboBox EstadoCivil { get; set; }
        public int IdInstitucionTerciaria { get; set; }
        public int IdLocalidadEscTerc { get; set; }
        public int IdInstitucionUniversitaria { get; set; }
        public int IdLocalidadEscUniv { get; set; }
        public int IdConvertirA { get; set; }
        public string ConvertirA { get; set; }
        public bool CambioTipo { get; set; }
        public int? IdEstadoFicha{ get; set; }
        public ComboBox Estado { get; set; }
        public IList<TipoRechazoVista> TipoRechazos { get; set; }
        public IFichaPPP FichaPpp { get; set; }
        public IFichaVAT FichaVat { get; set; }
        public IFichaPPPP FichaPppp { get; set; }

        public IFichaReconversion FichaReconversion { get; set; }
        public IFichaEfectoresSociales FichaEfectores { get; set; }

        public ComboBox Subprogramas { get; set; }
        public int? IdSubprograma { get; set; }

        public int? IdProyecto { get; set; }
        public string NombreProyecto { get; set; }
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

        public int IdNivelEscolaridad { get; set; }
        public int Cursando { get; set; }

        public bool TieneApoderadoActivo { get; set; }
        public bool TieneApoderado { get; set; }
        public IList<IApoderado> ListaApoderados { get; set; }
        public int? NumeroCuenta { get; set; }
        public string Cbu { get; set; }
        public string Sucursal { get; set; }
        public DateTime? FechaSolicitudCuenta { get; set; }
        public bool ClearFechaSolicitud { get; set; }
        public IComboBox Sucursales { get; set; }
        public bool SeGuardo { get; set; }

        public int? IdEmpresaInicial { get; set; }
        public string CuitEmpresa { get; set; }
        public string DescripcionEmpresa { get; set; }
        public string EmpresaLocalidad { get; set; }
        public string EmpresaCodigoActividad { get; set; }
        public string EmpresaDomicilioLaboralIdem { get; set; }

        public short? EmpresaCantidadEmpleados { get; set; }
        public string EmpresaCalle { get; set; }
        public string EmpresaNumero { get; set; }
        public string EmpresaPiso { get; set; }
        public string EmpresaDpto { get; set; }
        public string EmpresaCodigoPostal { get; set; }
        public string Tareas { get; set; }
        public string LugarLlamada { get; set; }

        public int IdLlamada { get; set; }
        public int? IdEmpresa { get; set; }
        public IComboBox ComboTipoFicha { get; set; }
        public IComboBox ComboEstadoFicha { get; set; }
        public IComboBox DomicilioLaboralIdem { get; set; }
        public IComboBox LocalidadesEmpresa { get; set; }
        public DateTime? FechaInicioActividad { get; set; }
        public DateTime? FechaFinActividad { get; set; }
        public int? IdModalidadAfip { get; set; }
        public string ModalidadAfip { get; set; }
        public IComboBox ComboModalidadAfip { get; set; }
        public IComboBox ComboUniversidad { get; set; }
        public IComboBox ComboTitulos { get; set; }
        public int IdSede { get; set; }
        public IComboBox LocalidadesSedes { get; set; }
        public List<ComboBox> HorarioLunes { get; set; }
        public List<ComboBox> HorarioMartes { get; set; }
        public List<ComboBox> HorarioMiercoles { get; set; }
        public List<ComboBox> HorarioJueves { get; set; }
        public List<ComboBox> HorarioViernes { get; set; }
        public List<ComboBox> HorarioSabado { get; set; }
        public List<ComboBox> HorarioDomingo { get; set; }

        List<string> IFichaVista.ObsLunes { get; set; }
        public List<string> ObsMartes { get; set; }
        public List<string> ObsMiercoles { get; set; }
        public List<string> ObsJueves { get; set; }
        public List<string> ObsViernes { get; set; }
        public List<string> ObsSabado { get; set; }
        public List<string> ObsDomingo { get; set; }
        public string DiaSemana { get; set; }
        public short HoraInicio { get; set; }
        public short MinutoInicio { get; set; }
        public short HoraFin { get; set; }
        public short MinutoFin { get; set; }
       

        public IComboBox Monedas { get; set; }
        public IList<IConcepto> ListaConceptosPago { get; set; }

        public DateTime? FechaNotificacion { get; set; }
        public bool Notificado { get; set; }

        public int? benefActivosEmp { get; set; }//contiene la cantidad de beneficiarios activos por programa por empresa
        public string FechaCambioHorario { get; set; }

        //SE AÑADEN EN LA TABLA T_BENEFICIARIOS LA FECHA DE INICIO Y BAJA DEL BENEFICIO PARA REALIZAR
        //LA LIQUIDACION Y DEJAR DE DEPENDER DE LA FECHA DE NOTIFICACION

        public DateTime? FechaInicioBeneficio { get; set; }
        public DateTime? FechaBajaBeneficio { get; set; }

        // 04/12/2013 - PARA EFECTORES SOCIALES
        public decimal monto { get; set; }

        // 07/04/2014 - SE AÑADE FICHA CONFIAMOS EN VOS
        public IFichaConfVos FichaConfVos { get; set; }
        public ComboBox TipoDocumentoApoCombo { get; set; }
        public ComboBox SexoApo { get; set; }
        public ComboBox LocalidadesApo { get; set; }
        public ComboBox EstadoCivilApo { get; set; }
        public ComboBox DepartamentosApo { get; set; }

        // 05/05/2014 - MENSAJE DE RESULTADOS MODIFICACIÓN
        public string strMessageUpdate { get; set; }

        public int idEtapa { get; set; }
        // 17/04/2015
        public ComboBox AbandonoCursado { get; set; }
        // 18/06/2015
        public IList<IAsistencia> asistencias { get; set; }

        // 16/08/2016
        public string NroTramite { get; set; }
        public string NombreEtapa { get; set; }

        // 16/10/2016
        public IComboBox comboBarrios { get; set; }
        public int? idBarrio { get; set; }
        // 11/12/2016
        public string Subprograma { get; set; }
        public IFichaTer FichaTER { get; set; }
        public IFichaUni FichaUNI { get; set; }

        // 17/01/2017
        public IComboBox ComboTiposPpp { get; set; }
        public int? tipoPrograma { get; set; }

        // 20/01/2017
        public IList<IRelacionFam> familiares { get; set; }

        public int? benefActDiscEmp { get; set; }
        // 23/10/2019
        public int? benefRetenidoscEmp { get; set; }
        public int? benefTotalcEmp { get; set; }

        //01/08/2018
        public ComboBox InstitucionSector { get; set; }
        public int? idInstSector { get; set; }
        public ComboBox CarreraSector { get; set; }
        public int? idCarreraSector { get; set; }
        public string n_descripcion_t { get; set; }
        public string n_cursado_ins { get; set; }
        public DateTime? egreso { get; set; }
        public string cod_uso_interno { get; set; }
        public string carga_horaria { get; set; }
        public string ch_cual { get; set; }
        public string CONSTANCIA_EGRESO { get; set; }
        public string MATRICULA { get; set; }
        public string CONSTANCIA_CURSO { get; set; }

        public int? TipoProgramaAnt { get; set; }
        // 25/09/2018
        public string SENAF { get; set; }
    }
}
