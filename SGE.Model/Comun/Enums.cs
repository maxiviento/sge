using System;

namespace SGE.Model.Comun
{
    public class Enums
    {
        public enum Acciones
        {
            /// <summary>
            /// Agregar = 1
            /// </summary>
            Agregar = 1,
            /// <summary>
            /// Modificar = 2
            /// </summary>
            Modificar = 2,
            /// <summary>
            /// Eliminar = 3
            /// </summary>
            Eliminar = 3,
            /// <summary>
            /// Consultar = 4
            /// </summary>
            Consultar = 4,
            /// <summary>
            /// CambiarEstado = 5
            /// </summary>
            CambiarEstado = 5,
            /// <summary>
            /// Exportar = 6
            /// </summary>
            Exportar = 6,
            /// <summary>
            /// Importar = 7
            /// </summary>
            Importar = 7,
            /// <summary>
            /// GenerarArchivo = 8
            /// </summary>
            GenerarArchivo = 8,
            /// <summary>
            /// VerArchivo = 9
            /// </summary>
            VerArchivo = 9,
            /// <summary>
            /// AgregarApoderado = 10
            /// </summary>
            AgregarApoderado = 10,
            /// <summary>
            /// ModificarApoderado = 11
            /// </summary>
            ModificarApoderado = 11,
            /// <summary>
            /// EliminarApoderado = 12
            /// </summary>
            EliminarApoderado = 12,
            /// <summary>
            /// Consultar = 13
            /// </summary>
            ConsultarApoderado = 13,
            /// <summary>
            /// Ver Efectores Sociales = 14
            /// </summary>
            VerEfectoresSociales = 14,
            /// <summary>
            /// Programas por rol = 15
            /// </summary>
            ProgramasPorRol = 15
        }

        public enum Autentificacion
        {
            /// <summary>
            /// Autenticado = 0
            /// </summary>
            Autenticado = 0,
            /// <summary>
            /// UsuarioInexistente = 1
            /// </summary>
            UsuarioInexistente = 1,
            /// <summary>
            /// ContraseñaInvalida = 2
            /// </summary>
            ContraseñaInvalida = 2
        }

        public enum EstadoApoderado
        {
            /// <summary>
            /// Activo = 1
            /// </summary>
            Activo = 1,
            /// <summary>
            /// Suspendido = 2
            /// </summary>
            Suspendido = 2,
            /// <summary>
            /// Reemplazado = 3
            /// </summary>
            Reemplazado = 3
        }

        public enum EstadoBeneficiario
        {
            /// <summary>
            /// Postulante = 1
            /// </summary>
            Postulante = 1,
            /// <summary>
            /// Activo = 2
            /// </summary>
            Activo = 2,
            /// <summary>
            /// BajaPorTramite = 3
            /// </summary>
            BajaPorRenuncia = 3,
            /// <summary>
            /// BajaDeOficio = 4
            /// </summary>
            BajaDeOficio = 4,
            /// <summary>
            /// BajaPedidoPorEmpresa = 5
            /// </summary>
            BajaPedidoPorEmpresa = 5,
            /// <summary>
            /// BeneficiarioRetenido = 6
            /// </summary>
            BeneficiarioRetenido = 6,
            /// <summary>
            /// Pendiente = 7
            /// </summary>
            Pendiente = 7,
            /// <summary>
            /// BajaPorFinalizacionProgr = 8
            /// </summary>
            BajaPorFinalizacionProgr = 8

        }

        public enum EstadoBeneficiarioLiquidacion
        {
            /// <summary>
            /// EnviadoAlBanco = 1
            /// </summary>
            EnviadoAlBanco = 1,
            /// <summary>
            /// Excluido = 2
            /// </summary>
            Excluido = 2,
            /// <summary>
            /// ImputadoEnCuenta = 3
            /// </summary>
            ImputadoEnCuenta = 3,
            /// <summary>
            /// NoImputado = 4
            /// </summary>
            NoImputado = 4,
            /// <summary>
            /// ExcluidoNoApto = 5
            /// </summary>
            ExcluidoNoApto = 5,
            /// <summary>
            /// CuentaInexistente = 6
            /// </summary>
            CuentaInexistente = 6,
            /// <summary>
            /// CuentaBloqueada = 7
            /// </summary>
            CuentaBloqueada = 7,
            /// <summary>
            /// CuentaCerrada = 8
            /// </summary>
            CuentaCerrada = 8,
            /// <summary>
            /// CatcuentaActualizandose = 9
            /// </summary>
            CatcuentaActualizandose = 9,
            /// <summary>
            /// Ca0CuentaDeAcreditacionEnCero = 10
            /// </summary>
            Ca0CuentaDeAcreditacionEnCero = 10,
            /// <summary>
            /// Ca1CuentaDeAcreditacionInexistente = 11
            /// </summary>
            Ca1CuentaDeAcreditacionInexistente = 11,
            /// <summary>
            /// Ca2CuentaDeAcreditacionConError = 12
            /// </summary>
            Ca2CuentaDeAcreditacionConError = 12,
            /// <summary>
            /// CbdcuentaConBloqueoDeDebitos = 13
            /// </summary>
            CbdcuentaConBloqueoDeDebitos = 13,
            /// <summary>
            /// CbtcuentaConBloqueoTotal = 14
            /// </summary>
            CbtcuentaConBloqueoTotal = 14,
            /// <summary>
            /// CcccuentaCerradaPorRechazoDeCheque = 15
            /// </summary>
            CcccuentaCerradaPorRechazoDeCheque = 15,
            /// <summary>
            /// CcjcuentaConHijas = 16
            /// </summary>
            CcjcuentaConHijas = 16,
            /// <summary>
            /// CcmcuentaConMovimientos = 17
            /// </summary>
            CcmcuentaConMovimientos = 17,
            /// <summary>
            /// CcscuentaConSaldo = 18
            /// </summary>
            CcscuentaConSaldo = 18,
            /// <summary>
            /// CecctaExisteRelCli = 19
            /// </summary>
            CecctaExisteRelCli = 19,
            /// <summary>
            /// CficuentaConFondoInsuficiente = 20
            /// </summary>
            CficuentaConFondoInsuficiente = 20,
            /// <summary>
            /// CiacuentaInactiva = 21
            /// </summary>
            CiacuentaInactiva = 21,
            /// <summary>
            /// CihcuentaInhabilitada = 22
            /// </summary>
            CihcuentaInhabilitada = 22,
            /// <summary>
            /// CilcuentaLibradoraInexistente = 23
            /// </summary>
            CilcuentaLibradoraInexistente = 23,
            /// <summary>
            /// CizcuentaInmovilizada = 24
            /// </summary>
            CizcuentaInmovilizada = 24,
            /// <summary>
            /// Ci1CuentaIncompleta = 25
            /// </summary>
            Ci1CuentaIncompleta = 25,
            /// <summary>
            /// CmecuentaConMovEnActualEjercicio = 26
            /// </summary>
            CmecuentaConMovEnActualEjercicio = 26,
            /// <summary>
            /// CnecuentaNoPuedeRealizarEntrega = 27
            /// </summary>
            CnecuentaNoPuedeRealizarEntrega = 27,
            /// <summary>
            /// CnocuentaNoOperativa = 28
            /// </summary>
            CnocuentaNoOperativa = 28,
            /// <summary>
            /// CsccuentaConSuspensionPagoDeCheque = 29
            /// </summary>
            CsccuentaConSuspensionPagoDeCheque = 29,
            /// <summary>
            /// CsecuentaSuperiorInexistente = 30
            /// </summary>
            CsecuentaSuperiorInexistente = 30,
            /// <summary>
            /// Ct0CuentaEnCero = 31
            /// </summary>
            Ct0CuentaEnCero = 31,
            /// <summary>
            /// Ct1NroCuentaInvalido = 32
            /// </summary>
            Ct1NroCuentaInvalido = 32,
            /// <summary>
            /// CuecuentaEmbargada = 33
            /// </summary>
            CuecuentaEmbargada = 33,
            /// <summary>
            /// CxpcuentaNoEsDePesos = 34
            /// </summary>
            CxpcuentaNoEsDePesos = 34,
            /// <summary>
            /// IntcuentaSinIntereses = 35
            /// </summary>
            IntcuentaSinIntereses = 35,
            /// <summary>
            /// ManmovimientoAnulado = 36
            /// </summary>
            ManmovimientoAnulado = 36,
            /// <summary>
            /// McpcuentaPrincipalNoEsEnPesos = 37
            /// </summary>
            McpcuentaPrincipalNoEsEnPesos = 37,
            /// <summary>
            /// MdcmonedaDistintaDeLaCuenta = 38
            /// </summary>
            MdcmonedaDistintaDeLaCuenta = 38,
            /// <summary>
            /// MdemovimientoDevuelto = 39
            /// </summary>
            MdemovimientoDevuelto = 39,
            /// <summary>
            /// MeimovConEstadoIncorrectoPOperacion = 40
            /// </summary>
            MeimovConEstadoIncorrectoPOperacion = 40,
            /// <summary>
            /// MivmonedaInvalida = 41
            /// </summary>
            MivmonedaInvalida = 41,
            /// <summary>
            /// MoimonedaInexistente = 42
            /// </summary>
            MoimonedaInexistente = 42,
            /// <summary>
            /// NainumeroDeAcuerdoInvalido = 43
            /// </summary>
            NainumeroDeAcuerdoInvalido = 43,
            /// <summary>
            /// SaisucursalDeAcreditacionInvalida = 44
            /// </summary>
            SaisucursalDeAcreditacionInvalida = 44
        }

        public enum EstadoCivil
        {
            /// <summary>
            /// Soltero = 0
            /// </summary>
            Soltero = 0,
            /// <summary>
            /// CasadoConviviente = 1
            /// </summary>
            CasadoConviviente = 1,
            /// <summary>
            /// Viudo = 2
            /// </summary>
            Viudo = 2,
            /// <summary>
            /// SeparadoDivorciado = 3
            /// </summary>
            SeparadoDivorciado = 3
        }

        public enum EstadoFicha
        {
            /// <summary>
            /// Basura = 0
            /// </summary>
            Basura = 0,
            /// <summary>
            /// Apta = 1
            /// </summary>
            Apta = 1,
            /// <summary>
            /// NoApta = 2
            /// </summary>
            NoApta = 2,
            /// <summary>
            /// Beneficiario = 3
            /// </summary>
            Beneficiario = 3,
            /// <summary>
            /// RechazoFormal = 4
            /// </summary>
            RechazoFormal = 4,
            /// <summary>
            /// FueraCupoEmpresa = 5
            /// </summary>
            FueraCupoEmpresa = 5,
            /// <summary>
            /// FueraCupoPrograma = 6
            /// </summary>
            FueraCupoPrograma = 6,
            /// <summary>
            /// Duplicado = 7
            /// </summary>
            Duplicado = 7,
            /// <summary>
            /// SIN ESTADO = -2
            /// </summary>
            SinEstado = -2,
            /// <summary>
            /// Inscripto = 8
            /// </summary>
            Inscripto = 8,
            /// <summary>
            /// EtapaFinalizada = 9
            /// </summary>
            EtapaFinalizada = 9,
            /// <summary>
            /// InscriptoSegundaEtapa = 10
            /// </summary>
            InscriptoSegundaEtapa = 10
        }

        public enum EstadoLiquidacion
        {
            /// <summary>
            /// Pendiente = 1
            /// </summary>
            Pendiente = 1,
            /// <summary>
            /// Liquidado = 2
            /// </summary>
            Liquidado = 2,
            /// <summary>
            /// Cancelado = 3
            /// </summary>
            Cancelado = 3
        }

        public enum EstadoRegistro
        {
            /// <summary>
            /// Activo = 1
            /// </summary>
            Activo = 1,
            /// <summary>
            /// Eliminado = 2
            /// </summary>
            Eliminado = 2
        }

        public enum Formulario
        {
            /// <summary>
            /// AbmUsuario = 1
            /// </summary>
            AbmUsuario = 1,
            /// <summary>
            /// AbmBeneficiario = 2
            /// </summary>
            AbmBeneficiario = 2,
            /// <summary>
            /// AbmFicha = 3
            /// </summary>
            AbmFicha = 3,
            /// <summary>
            /// Liquidaciones = 4
            /// </summary>
            Liquidaciones = 4,
            /// <summary>
            /// AbmCausaRechazo = 5
            /// </summary>
            AbmCausaRechazo = 5,
            /// <summary>
            /// CambiarComntraseña = 6
            /// </summary>
            CambiarComntraseña = 6,
            /// <summary>
            /// AsignarPermisos = 7
            /// </summary>
            AsignarPermisos = 7,
            /// <summary>
            /// AbmConceptos = 8
            /// </summary>
            AbmConceptos = 8,
            /// <summary>
            /// AbmRoles = 9
            /// </summary>
            AbmRoles = 9,
            /// <summary>
            /// AbmTipoRechazo = 10
            /// </summary>
            AbmTipoRechazo = 10,
            /// <summary>
            /// AbmProgramas = 11
            /// </summary>
            AbmProgramas = 11,
            /// <summary>
            /// AbmInstituciones = 12
            /// </summary>
            AbmInstituciones = 12,
            /// <summary>
            /// AbmDepartamentos = 13
            /// </summary>
            AbmDepartamentos = 13,
            /// <summary>
            /// AbmEmpresas = 14
            /// </summary>
            AbmEmpresas = 14,
            /// <summary>
            /// AbmUniversidades = 15
            /// </summary>
            AbmUniversidades = 15,
            /// <summary>
            /// AbmLocalidades = 16
            /// </summary>
            AbmLocalidades = 16,
            /// <summary>
            /// AbmSedes = 17
            /// </summary>
            AbmSedes = 17,
            /// <summary>
            /// AbmCarreras = 18
            /// </summary>
            AbmCarreras = 18,
            /// <summary>
            /// AbmSucursalesCobertura = 19
            /// </summary>
            AbmSucursalesCobertura = 19,
            /// <summary>
            /// AbmApoderados = 20
            /// </summary>
            AbmApoderados = 20,
            /// <summary>
            /// Inicializartablas = 21
            /// </summary>
            InicializarTablas = 21,
            /// <summary>
            /// ControlDatos = 22
            /// </summary>
            ControlDatos = 22,
            /// <summary>
            /// AbmFichaObservacion = 23
            /// </summary>
            AbmFichaObservacion = 23,
            /// <summary>
            /// AbmSubprogramas = 24
            /// </summary>
            AbmSubprogramas = 24,

            /* SECTOR DE REPORTES */
            /// <summary>
            /// RepAnexoIResolucion = 119
            /// </summary>
            RepAnexoIResolucion = 119,
            /// <summary>
            /// RepDepLocProEst = 120
            /// </summary>
            RepDepLocProEst = 120,
            /// <summary>
            /// RepArtHorariosEntPpp = 121
            /// </summary>
            RepArtHorariosEntPpp = 121,
            /// <summary>
            /// RepArtHorariosEntReconversion = 122
            /// </summary>
            RepArtHorariosEntReconversion = 122,
            /// <summary>
            /// RepArtHorariosEntEfectores = 123
            /// </summary>
            RepArtHorariosEntEfectores = 123,
            /// <summary>
            /// RepArtHorariosEntVat = 124
            /// </summary>
            RepArtHorariosEntVat = 124,
            /// <summary>
            /// RepArtHorariosEntPppp = 125
            /// </summary>
            RepArtHorariosEntPppp = 125,
            /// <summary>
            /// AltaMasivaBeneficiarios = 126
            /// </summary>
            AltaMasivaBeneficiarios = 126,
            /// <summary>
            /// RepSectorInstCarrera = 127
            /// </summary>
            RepSectorInstCarrera = 127,
            //07/05/2013 - Di campli leandro - SE AÑADE NUEVA PANTALLA PARA ETAPAS
            /// <summary>
            /// AbmEtapas = 128
            /// </summary>
            AbmEtapas = 128,
            //14/05/2013 - Di campli leandro - SE AÑADE NUEVA PANTALLA PARA ASOCIAR ETAPAS
            /// <summary>
            /// AsociarEtapa = 129
            /// </summary>
            AsociarEtapa = 129,
            //08/01/2014 - Di campli leandro - SE AÑADE NUEVA PANTALLA PARA GESTOR DE ESTADOS FICHAS
            /// <summary>
            /// GestorEstadosFichas = 130
            /// </summary>
            GestorEstadosFichas = 130,
            //11/05/2015 - Di campli leandro - SE AÑADE NUEVA PANTALLA PARA PERSONAS
            /// <summary>
            /// ABMPersonas = 131
            /// </summary>
            ABMPersonas = 131,
            //18/05/2015 - Di campli leandro - SE AÑADE NUEVA PANTALLA PARA CURSOS
            /// <summary>
            /// ABMCursos = 132
            /// </summary>
            ABMCursos = 132,
            //22/05/2015 - Di campli leandro - SE AÑADE NUEVA PANTALLA PARA BARRIOS
            /// <summary>
            /// ABMBarrios = 133
            /// </summary>
            ABMBarrios = 133,
            //26/05/2015 - Di campli leandro - SE AÑADE NUEVA PANTALLA PARA ESCUELAS
            /// <summary>
            /// ABMEscuelas = 134
            /// </summary>
            ABMEscuelas = 134,
            /// <summary>
            /// ABMOng = 135
            /// </summary>
            ABMOng = 135,
            /// <summary>
            /// ReEmpadronar = 136
            /// </summary>
            ReEmpadronar = 136,
            /// <summary>
            /// Recupero = 137
            /// </summary>
            Recupero = 137
        }

        public enum Modalidad
        {
            /// <summary>
            /// Entrenamiento = 1
            /// </summary>
            Entrenamiento = 1,
            /// <summary>
            /// Cti = 2
            /// </summary>
            Cti = 2
        }

        public enum NivelEscolaridad
        {
            /// <summary>
            /// Ninguno = 1
            /// </summary>
            Ninguno = 1,
            /// <summary>
            /// PrimariaOEgb = 2
            /// </summary>
            PrimariaOEgb = 2,
            /// <summary>
            /// CicloBasico = 3
            /// </summary>
            CicloBasico = 3,
            /// <summary>
            /// Secundaria = 4
            /// </summary>
            Secundaria = 4,
            /// <summary>
            /// Terciaria = 5
            /// </summary>
            Terciaria = 5,
            /// <summary>
            /// Universitaria = 6
            /// </summary>
            Universitaria = 6,
            /// <summary>
            /// Inicial = 7
            /// </summary>
            Inicial = 7,
            /// <summary>
            /// Especial = 8
            /// </summary>
            Especial = 8,
            /// <summary>
            /// CursoFormacion = 9
            /// </summary>
            CursoFormacion = 9
        }

        public enum Programas
        {
            /// <summary>
            /// Terciaria = 1
            /// </summary>
            Terciaria = 1,
            /// <summary>
            /// Universitaria = 2
            /// </summary>
            Universitaria = 2,
            /// <summary>
            /// Ppp = 3
            /// </summary>
            Ppp = 3,
            /// <summary>
            /// PppProf = 4
            /// </summary>
            PppProf = 4,
            /// <summary>
            /// Vat = 5
            /// </summary>
            Vat = 5,
            /// <summary>
            /// ReconversionProductiva = 6
            /// </summary>
            ReconversionProductiva = 6,
            /// <summary>
            /// ReconversionProductiva = 7
            /// </summary>
            EfectoresSociales = 7,
            /// <summary>
            /// ConfiamosEnVos = 8
            /// </summary>
            ConfiamosEnVos = 8
        }

        public enum Sexo
        {
            /// <summary>
            /// Mujer = 1
            /// </summary>
            Mujer = 1,
            /// <summary>
            /// Varon = 2
            /// </summary>
            Varon = 2
        }

        public enum TipoDocumento
        {
            /// <summary>
            /// Dni = 0
            /// </summary>
            Dni = 0,
            /// <summary>
            /// Le = 1
            /// </summary>
            Le = 1,
            /// <summary>
            /// Lc = 2
            /// </summary>
            Lc = 2,
            /// <summary>
            /// Ci = 3
            /// </summary>
            Ci = 3,
            /// <summary>
            /// NuncaTuvo = 4
            /// </summary>
            NuncaTuvo = 4,
            /// <summary>
            /// CedulaExtranjera = 5
            /// </summary>
            CedulaExtranjera = 5,
            /// <summary>
            /// Otro = 6
            /// </summary>
            Otro = 6
        }

        public enum TipoFicha
        {
            /// <summary>
            /// Terciaria = 1
            /// </summary>
            Terciaria = 1,
            /// <summary>
            /// Universitaria = 2
            /// </summary>
            Universitaria = 2,
            /// <summary>
            /// Ppp = 3
            /// </summary>
            Ppp = 3,
            /// <summary>
            /// PppProf = 4
            /// </summary>
            PppProf = 4,
            /// <summary>
            /// Vat = 5
            /// </summary>
            Vat = 5,
            /// <summary>
            /// Reconversion=6
            /// </summary>
            ReconversionProductiva = 6,
            /// <summary>
            /// Efectores=7
            /// </summary>
            EfectoresSociales = 7,
            /// <summary>
            /// ConfiamosEnVos=8
            /// </summary>
            ConfiamosEnVos = 8

        }

        public enum TiposTablaBcoCba
        {
            /// <summary>
            /// Sucursales = 1
            /// </summary>
            Sucursales = 1,
            /// <summary>
            /// Monedas = 2
            /// </summary>
            Monedas = 2,
            /// <summary>
            /// Sistemas = 3
            /// </summary>
            Sistemas = 3,
            /// <summary>
            /// TiposDeDocumentos = 4
            /// </summary>
            TiposDeDocumentos = 4,
            /// <summary>
            /// Provincias = 5
            /// </summary>
            Provincias = 5,
            /// <summary>
            /// EstadoCivil = 6
            /// </summary>
            EstadoCivil = 6,
            /// <summary>
            /// Pais = 7
            /// </summary>
            Pais = 7,
            /// <summary>
            /// CondicionIva = 8
            /// </summary>
            CondicionIva = 8,
            /// <summary>
            /// CodigosPostales = 9
            /// </summary>
            CodigosPostales = 9
        }

        public static string GetTipoDocumento(int tipoDocumento)
        {
            switch (tipoDocumento)
            {
                case (int)TipoDocumento.Dni:
                    return "DNI";
                case (int)TipoDocumento.Le:
                    return "LE";
                case (int)TipoDocumento.Lc:
                    return "LC";
                case (int)TipoDocumento.Ci:
                    return "CI";
                case (int)TipoDocumento.NuncaTuvo:
                    return "Nunca Tuvo";
                case (int)TipoDocumento.CedulaExtranjera:
                    return "Cedula Extranjera";
                case (int)TipoDocumento.Otro:
                    return "Otro";
                default:
                    return String.Empty;
            }
        }

        public static string GetEstadoCivil(int estadoCivil)
        {
            switch (estadoCivil)
            {
                case (int)EstadoCivil.Soltero:
                    return "Soltero";
                case (int)EstadoCivil.CasadoConviviente:
                    return "Casado o conviviente";
                case (int)EstadoCivil.Viudo:
                    return "Viudo";
                case (int)EstadoCivil.SeparadoDivorciado:
                    return "Separado o divorciado";
                default:
                    return String.Empty;
            }
        }

        public static string GetTipoFicha(int tipoFicha)
        {
            switch (tipoFicha)
            {
                case (int)TipoFicha.Terciaria:
                    return "Ficha Terciaria";
                case (int)TipoFicha.Universitaria:
                    return "Ficha Universitaria";
                case (int)TipoFicha.Ppp:
                    return "Ficha PPP";
                case (int)TipoFicha.PppProf:
                    return "Ficha PPP Profesional";
                case (int)TipoFicha.Vat:
                    return "Ficha VAT";
                case (int)TipoFicha.ReconversionProductiva:
                    return "Ficha Reconversión Productiva";
                case (int)TipoFicha.EfectoresSociales:
                    return "Ficha Efectores Sociales";
                case (int)TipoFicha.ConfiamosEnVos:
                    return "Ficha Confiamos en Vos";
                default:
                    return String.Empty;
            }
        }


        public enum DiaSemana
        {
            /// <summary>
            /// Lunes = 1,
            /// </summary>
            Lunes = 1,
            /// <summary>
            /// Martes = 2,
            /// </summary>
            Martes = 2,
            /// <summary>
            /// Miércoles = 3,
            /// </summary>
            Miércoles = 3,
            /// <summary>
            /// Jueves = 4,
            /// </summary>
            Jueves = 4,
            /// <summary>
            /// Viernes = 5,
            /// </summary>
            Viernes = 5,
            /// <summary>
            /// Sábado = 6,
            /// </summary>
            Sábado = 6,
            /// <summary>
            /// Domingo = 7,
            /// </summary>
            Domingo = 7
        }


        public enum Actor_Rol
        { 
            /// <summary>
            /// DOCENTE = 1,
            /// </summary>
        	DOCENTE=1,
            /// <summary>
            /// 
            /// </summary>
            COORDINADOR=2,
            /// <summary>
            /// DIRECTOR=3,
            /// </summary>
	        DIRECTOR=3,
            /// <summary>
            /// 	ENCARGADO_ESCUELA=4,
            /// </summary>
	        ENCARGADO_ESCUELA=4,
            /// <summary>
            /// FACILITADOR=5,
            /// </summary>
	        FACILITADOR=5,
            /// <summary>
            /// TUTOR=6,
            /// </summary>
	        TUTOR=6,
            /// <summary>
            /// GESTOR=7,
            /// </summary>
            GESTOR = 7,
            /// <summary>
            /// RESPONSABLE=8,
            /// </summary>
            RESPONSABLE = 8,
            /// <summary>
            /// MONITOR=9,
            /// </summary>
            MONITOR = 9,
        }


    }
}
