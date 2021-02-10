using System;
using System.Collections.Generic;
using System.Linq;
using SGE.Model.Comun;
using SGE.Model.Entidades;
using SGE.Model.Entidades.Interfaces;
using SGE.Model.Repositorio;
using SGE.Repositorio.Modelo;

namespace SGE.Repositorio
{
    public class FichaRepositorio : BaseRepositorio, IFichaRepositorio
    {
        private readonly DataSGE _mdb;

        public FichaRepositorio()
        {
            _mdb = new DataSGE();
        }

        private IQueryable<IFicha> QFicha()
        {
            var a = (from c in _mdb.T_FICHAS
                        from b in _mdb.T_BENEFICIARIOS.Where(b=>b.ID_FICHA==c.ID_FICHA).DefaultIfEmpty()
                        from eb in _mdb.T_ESTADOS.Where(eb=>eb.ID_ESTADO==b.ID_ESTADO).DefaultIfEmpty()
                     select
                         new Ficha
                             {
                                 Apellido = c.APELLIDO,
                                 Barrio = c.BARRIO,
                                 Localidad = c.T_LOCALIDADES.N_LOCALIDAD,
                                 IdLocalidad = c.ID_LOCALIDAD ?? 0,
                                 IdFicha = c.ID_FICHA,
                                 Calle = c.CALLE,
                                 CantidadHijos = c.CANTIDAD_HIJOS,
                                 CertificadoDiscapacidad = c.CERTIF_DISCAP == "S" ? true : false,
                                 CodigoPostal = c.CODIGO_POSTAL,
                                 CodigoSeguridad = c.CODIGO_SEGURIDAD,
                                 Contacto = c.CONTACTO,
                                 Cuil = c.CUIL,
                                 DeficienciaOtra = c.DEFICIENCIA_OTRA,
                                 TieneDeficienciaMental = c.TIENE_DEF_MENTAL == "S" ? true : false,
                                 TieneDeficienciaMotora = c.TIENE_DEF_MOTORA == "S" ? true : false,
                                 TieneDeficienciaPsicologia = c.TIENE_DEF_PSICOLOGICA == "S" ? true : false,
                                 TieneDeficienciaSensorial = c.TIENE_DEF_SENSORIAL == "S" ? true : false,
                                 Dpto = c.DPTO,
                                 EntreCalles = c.ENTRECALLES,
                                 EsDiscapacitado = c.ES_DISCAPACITADO == "S" ? true : false,
                                 EstadoCivil = c.ESTADO_CIVIL,
                                 FechaNacimiento = c.FER_NAC,
                                 Mail = c.MAIL,
                                 Manzana = c.MANZANA,
                                 Monoblock = c.MONOBLOCK,
                                 Nombre = c.NOMBRE,
                                 Numero = c.NUMERO,
                                 NumeroDocumento = c.NUMERO_DOCUMENTO,
                                 Parcela = c.PARCELA,
                                 Piso = c.PISO,
                                 Sexo = c.SEXO,
                                 TelefonoCelular = c.TEL_CELULAR,
                                 TelefonoFijo = c.TEL_FIJO,
                                 TieneHijos = c.TIENE_HIJOS == "S" ? true : false,
                                 TipoDocumento = c.TIPO_DOCUMENTO,
                                 TipoFicha = c.TIPO_FICHA,
                                 NombreTipoFicha = c.T_TIPO_FICHA.N_TIPO_FICHA,
                                 NombreDepartamento = c.T_LOCALIDADES.T_DEPARTAMENTOS.N_DEPARTAMENTO,
                                 IdDepartamento = c.T_LOCALIDADES.T_DEPARTAMENTOS.ID_DEPARTAMENTO,

                                 //IdCarreraTerciaria = c.T_FICHA_TERCIARIO.ID_CARRERA ?? 0,
                                 //IdCarreraUniversitaria = c.T_FICHA_UNIVERSITARIO.ID_CARRERA ?? 0,
                                 //IdInstitucionTerciaria = c.T_FICHA_TERCIARIO.T_CARRERAS.T_SECTOR_INSTITUCION.ID_INSTITUCION,
                                 //IdInstitucionUniversitaria = c.T_FICHA_UNIVERSITARIO.T_CARRERAS.T_SECTOR_INSTITUCION.ID_INSTITUCION,
                                 //IdSectorTerciaria = c.T_FICHA_TERCIARIO.T_CARRERAS.ID_SECTOR ?? 0,
                                 //IdSectorUniversitaria = c.T_FICHA_UNIVERSITARIO.T_CARRERAS.ID_SECTOR ?? 0,
                                 //IdEscuelaTerciaria = c.T_FICHA_TERCIARIO.ID_ESCUELA ?? 0,
                                 //IdEscuelaUniversitaria = c.T_FICHA_UNIVERSITARIO.ID_ESCUELA ?? 0,
                                 //PromedioTerciaria = c.T_FICHA_TERCIARIO.PROMEDIO,
                                 //PromedioUniversitaria = c.T_FICHA_UNIVERSITARIO.PROMEDIO,
                                 //OtraCarreraTerciaria = c.T_FICHA_TERCIARIO.OTRA_CARRERA,
                                 //OtraCarreraUniversitaria = c.T_FICHA_UNIVERSITARIO.OTRA_CARRERA,
                                 //OtraInstitucionTerciaria = c.T_FICHA_TERCIARIO.OTRA_INSTITUCION,
                                 //OtraInstitucionUniversitaria = c.T_FICHA_UNIVERSITARIO.OTRA_INSTITUCION,
                                 //OtroSectorTerciaria = c.T_FICHA_TERCIARIO.OTRO_SECTOR,
                                 //OtroSectorUniversitaria = c.T_FICHA_UNIVERSITARIO.OTRO_SECTOR,
                                 //IdDepartamentoEscTerc = c.T_FICHA_TERCIARIO.T_ESCUELAS.T_LOCALIDADES.ID_DEPARTAMENTO ?? 0,
                                 //IdDepartamentoEscUniv = c.T_FICHA_UNIVERSITARIO.T_ESCUELAS.T_LOCALIDADES.ID_DEPARTAMENTO ?? 0,
                                 //IdLocalidadEscTerc = c.T_FICHA_TERCIARIO.T_ESCUELAS.ID_LOCALIDAD ?? 0,
                                 //IdLocalidadEscUniv = c.T_FICHA_UNIVERSITARIO.T_ESCUELAS.ID_LOCALIDAD ?? 0,

                                 IdEstadoFicha = c.ID_ESTADO_FICHA ?? (int)Enums.EstadoFicha.SinEstado,
                                 NombreEstadoFicha = c.T_ESTADOS_FICHA.N_ESTADO_FICHA ?? "",
                                 FechaSistema = c.FEC_SIST,
                                 IdUsuarioSistema = c.ID_USR_SIST,
                                 UsuarioSistema = c.T_USUARIOS.LOGIN,
                                 // 21/05/2013 - DI CAMPLI LEANDRO - DATOS DE LA ETAPA
                                 idEtapa = c.ID_ETAPA ?? 0,
                                 NombreEtapa = c.T_ETAPAS.N_ETAPA,
                                 //IdEscuelaConfVos = c.T_FICHAS_CONF_VOS.ID_ESCUELA ?? 0, //ver esto no es performante -- llevar la consulta al servicio como se hizo luego en ppp
                                 NroTramite = c.NRO_TRAMITE,
                                 idBarrio = c.ID_BARRIO,
                                 BeneficiarioFicha = new Beneficiario
                                                                {
                                                                    IdBeneficiario = b.ID_BENEFICIARIO == null ? 0 : b.ID_BENEFICIARIO,
                                                                    IdEstado = eb.ID_ESTADO == null ? 0 : eb.ID_ESTADO,
                                                                    NombreEstado = eb.N_ESTADO
                                                                }

                             });

            return a;
        }

        /// <summary>
        /// 27/06/2013 - prueba qficha con filtro idfichas
        /// </summary>
        /// <returns></returns>
        private IQueryable<IFicha> QFicha(List<int> idBeneficiariosExport)
        {
            
            var a = (from c in _mdb.T_FICHAS
                     //join d in idBeneficiariosExport.ToList()
                     //   on c.ID_FICHA equals d
                     where  idBeneficiariosExport.Contains(c.ID_FICHA)
                     select
                         new Ficha
                         {
                             Apellido = c.APELLIDO,
                             Barrio = c.BARRIO,
                             Localidad = c.T_LOCALIDADES.N_LOCALIDAD,
                             IdLocalidad = c.ID_LOCALIDAD ?? 0,
                             IdFicha = c.ID_FICHA,
                             Calle = c.CALLE,
                             CantidadHijos = c.CANTIDAD_HIJOS,
                             CertificadoDiscapacidad = c.CERTIF_DISCAP == "S" ? true : false,
                             CodigoPostal = c.CODIGO_POSTAL,
                             CodigoSeguridad = c.CODIGO_SEGURIDAD,
                             Contacto = c.CONTACTO,
                             Cuil = c.CUIL,
                             DeficienciaOtra = c.DEFICIENCIA_OTRA,
                             TieneDeficienciaMental = c.TIENE_DEF_MENTAL == "S" ? true : false,
                             TieneDeficienciaMotora = c.TIENE_DEF_MOTORA == "S" ? true : false,
                             TieneDeficienciaPsicologia = c.TIENE_DEF_PSICOLOGICA == "S" ? true : false,
                             TieneDeficienciaSensorial = c.TIENE_DEF_SENSORIAL == "S" ? true : false,
                             Dpto = c.DPTO,
                             EntreCalles = c.ENTRECALLES,
                             EsDiscapacitado = c.ES_DISCAPACITADO == "S" ? true : false,
                             EstadoCivil = c.ESTADO_CIVIL,
                             FechaNacimiento = c.FER_NAC,
                             Mail = c.MAIL,
                             Manzana = c.MANZANA,
                             Monoblock = c.MONOBLOCK,
                             Nombre = c.NOMBRE,
                             Numero = c.NUMERO,
                             NumeroDocumento = c.NUMERO_DOCUMENTO,
                             Parcela = c.PARCELA,
                             Piso = c.PISO,
                             Sexo = c.SEXO,
                             TelefonoCelular = c.TEL_CELULAR,
                             TelefonoFijo = c.TEL_FIJO,
                             TieneHijos = c.TIENE_HIJOS == "S" ? true : false,
                             TipoDocumento = c.TIPO_DOCUMENTO,
                             TipoFicha = c.TIPO_FICHA,
                             NombreTipoFicha = c.T_TIPO_FICHA.N_TIPO_FICHA,
                             NombreDepartamento = c.T_LOCALIDADES.T_DEPARTAMENTOS.N_DEPARTAMENTO,
                             IdDepartamento = c.T_LOCALIDADES.T_DEPARTAMENTOS.ID_DEPARTAMENTO,
                             IdCarreraTerciaria = c.T_FICHA_TERCIARIO.ID_CARRERA ?? 0,
                             IdCarreraUniversitaria = c.T_FICHA_UNIVERSITARIO.ID_CARRERA ?? 0,
                             IdInstitucionTerciaria = c.T_FICHA_TERCIARIO.T_CARRERAS.T_SECTOR_INSTITUCION.ID_INSTITUCION,
                             IdInstitucionUniversitaria = c.T_FICHA_UNIVERSITARIO.T_CARRERAS.T_SECTOR_INSTITUCION.ID_INSTITUCION,
                             IdSectorTerciaria = c.T_FICHA_TERCIARIO.T_CARRERAS.ID_SECTOR ?? 0,
                             IdSectorUniversitaria = c.T_FICHA_UNIVERSITARIO.T_CARRERAS.ID_SECTOR ?? 0,
                             IdEscuelaTerciaria = c.T_FICHA_TERCIARIO.ID_ESCUELA ?? 0,
                             IdEscuelaUniversitaria = c.T_FICHA_UNIVERSITARIO.ID_ESCUELA ?? 0,
                             PromedioTerciaria = c.T_FICHA_TERCIARIO.PROMEDIO,
                             PromedioUniversitaria = c.T_FICHA_UNIVERSITARIO.PROMEDIO,
                             OtraCarreraTerciaria = c.T_FICHA_TERCIARIO.OTRA_CARRERA,
                             OtraCarreraUniversitaria = c.T_FICHA_UNIVERSITARIO.OTRA_CARRERA,
                             OtraInstitucionTerciaria = c.T_FICHA_TERCIARIO.OTRA_INSTITUCION,
                             OtraInstitucionUniversitaria = c.T_FICHA_UNIVERSITARIO.OTRA_INSTITUCION,
                             OtroSectorTerciaria = c.T_FICHA_TERCIARIO.OTRO_SECTOR,
                             OtroSectorUniversitaria = c.T_FICHA_UNIVERSITARIO.OTRO_SECTOR,
                             IdDepartamentoEscTerc = c.T_FICHA_TERCIARIO.T_ESCUELAS.T_LOCALIDADES.ID_DEPARTAMENTO ?? 0,
                             IdDepartamentoEscUniv = c.T_FICHA_UNIVERSITARIO.T_ESCUELAS.T_LOCALIDADES.ID_DEPARTAMENTO ?? 0,
                             IdLocalidadEscTerc = c.T_FICHA_TERCIARIO.T_ESCUELAS.ID_LOCALIDAD ?? 0,
                             IdLocalidadEscUniv = c.T_FICHA_UNIVERSITARIO.T_ESCUELAS.ID_LOCALIDAD ?? 0,
                             IdEstadoFicha = c.ID_ESTADO_FICHA ?? (int)Enums.EstadoFicha.SinEstado,
                             NombreEstadoFicha = c.T_ESTADOS_FICHA.N_ESTADO_FICHA ?? "",
                             FechaSistema = c.FEC_SIST,
                             IdUsuarioSistema = c.ID_USR_SIST,
                             UsuarioSistema = c.T_USUARIOS.LOGIN,
                             // 21/05/2013 - DI CAMPLI LEANDRO - DATOS DE LA ETAPA
                             idEtapa = c.ID_ETAPA ?? 0,
                             NombreEtapa = c.T_ETAPAS.N_ETAPA

                         });

            return a;
        }

        private IQueryable<IFicha> QFicha(List<int> idBeneficiariosExport, int idPrograma)
        {
                switch(idPrograma)
                {
                    #region "Ppp"
                    case (int)Enums.TipoFicha.Ppp:

                    var ppp = (from c in _mdb.T_FICHAS
                                join fppp in _mdb.T_FICHA_PPP on c.ID_FICHA equals fppp.ID_FICHA
                                from sub in _mdb.T_SUBPROGRAMAS.Where(sub=>sub.ID_SUBPROGRAMA == fppp.ID_SUBPROGRAMA).DefaultIfEmpty()
                             //   on c.ID_FICHA equals d
                             where idBeneficiariosExport.Contains(c.ID_FICHA)
                             select
                                 new Ficha
                                 {
                                     Apellido = c.APELLIDO,
                                     Barrio = c.BARRIO,
                                     //Localidad = c.T_LOCALIDADES.N_LOCALIDAD,
                                     IdLocalidad = c.ID_LOCALIDAD ?? 0,
                                     IdFicha = c.ID_FICHA,
                                     Calle = c.CALLE,
                                     CantidadHijos = c.CANTIDAD_HIJOS,
                                     CertificadoDiscapacidad = c.CERTIF_DISCAP == "S" ? true : false,
                                     CodigoPostal = c.CODIGO_POSTAL,
                                     CodigoSeguridad = c.CODIGO_SEGURIDAD,
                                     Contacto = c.CONTACTO,
                                     Cuil = c.CUIL,
                                     DeficienciaOtra = c.DEFICIENCIA_OTRA,
                                     TieneDeficienciaMental = c.TIENE_DEF_MENTAL == "S" ? true : false,
                                     TieneDeficienciaMotora = c.TIENE_DEF_MOTORA == "S" ? true : false,
                                     TieneDeficienciaPsicologia = c.TIENE_DEF_PSICOLOGICA == "S" ? true : false,
                                     TieneDeficienciaSensorial = c.TIENE_DEF_SENSORIAL == "S" ? true : false,
                                     Dpto = c.DPTO,
                                     EntreCalles = c.ENTRECALLES,
                                     EsDiscapacitado = c.ES_DISCAPACITADO == "S" ? true : false,
                                     EstadoCivil = c.ESTADO_CIVIL,
                                     FechaNacimiento = c.FER_NAC,
                                     Mail = c.MAIL,
                                     Manzana = c.MANZANA,
                                     Monoblock = c.MONOBLOCK,
                                     Nombre = c.NOMBRE,
                                     Numero = c.NUMERO,
                                     NumeroDocumento = c.NUMERO_DOCUMENTO,
                                     Parcela = c.PARCELA,
                                     Piso = c.PISO,
                                     Sexo = c.SEXO,
                                     TelefonoCelular = c.TEL_CELULAR,
                                     TelefonoFijo = c.TEL_FIJO,
                                     TieneHijos = c.TIENE_HIJOS == "S" ? true : false,
                                     TipoDocumento = c.TIPO_DOCUMENTO,
                                     TipoFicha = c.TIPO_FICHA,
                                     NombreTipoFicha = c.T_TIPO_FICHA.N_TIPO_FICHA,
                                     //NombreDepartamento = c.T_LOCALIDADES.T_DEPARTAMENTOS.N_DEPARTAMENTO,
                                     //IdDepartamento = c.T_LOCALIDADES.T_DEPARTAMENTOS.ID_DEPARTAMENTO,
                                     IdEstadoFicha = c.ID_ESTADO_FICHA ?? (int)Enums.EstadoFicha.SinEstado,
                                     NombreEstadoFicha = c.T_ESTADOS_FICHA.N_ESTADO_FICHA ?? "",
                                     FechaSistema = c.FEC_SIST,
                                     IdUsuarioSistema = c.ID_USR_SIST,
                                     UsuarioSistema = c.T_USUARIOS.LOGIN,
                                     idEtapa = c.ID_ETAPA ?? 0,
                                     //NombreEtapa = c.T_ETAPAS.N_ETAPA,
                                     IdSubprograma = fppp.ID_SUBPROGRAMA ?? 0,
                                     Subprograma = sub.N_SUBPROGRAMA,

                                 });
            
                return ppp;
                    #endregion

                    default:

                return null;
            }
        }

        private IQueryable<IFicha> QFichaSimple()
        {
            return
                _mdb.T_FICHAS.Select(
                    c =>
                    new Ficha
                        {
                            IdFicha = c.ID_FICHA,
                            Nombre = c.NOMBRE,
                            Apellido = c.APELLIDO,
                            NumeroDocumento = c.NUMERO_DOCUMENTO,
                            IdEstadoFicha = c.ID_ESTADO_FICHA ?? (int)Enums.EstadoFicha.SinEstado,
                            TipoFicha = c.TIPO_FICHA,
                            Cuil = c.CUIL,
                            NombreEstadoFicha = c.T_ESTADOS_FICHA.N_ESTADO_FICHA ?? "",
                            NombreTipoFicha = c.T_TIPO_FICHA.N_TIPO_FICHA,
                            IdDepartamento = c.T_LOCALIDADES.ID_DEPARTAMENTO,
                            IdLocalidad = c.ID_LOCALIDAD,
                            NombreDepartamento = c.T_LOCALIDADES.T_DEPARTAMENTOS.N_DEPARTAMENTO,
                            Localidad = c.T_LOCALIDADES.N_LOCALIDAD,
                            idEtapa = c.ID_ETAPA, // 14/06/2013 - DI CAMPLI LEANDRO - SE AÑADE LA ETAPA A LA CONSULTA PARA PODER FILTRAR
                            NombreEtapa = c.T_ETAPAS.N_ETAPA, // 14/06/2013 - DI CAMPLI LEANDRO - SE AÑADE LA ETAPA A LA CONSULTA PARA PODER FILTRAR
                            NroTramite = c.NRO_TRAMITE
                             
                        });
        }

        private IQueryable<IFicha> QFichaSimple(int idPrograma)
        {
            switch (idPrograma)
            {

                case (int)Enums.TipoFicha.EfectoresSociales:

                    return
                            from c in _mdb.T_FICHAS.Where(c=>c.TIPO_FICHA == (int)Enums.TipoFicha.EfectoresSociales)
                            from efe in _mdb.T_FICHA_EFEC_SOC.Where(efe=>efe.ID_FICHA == c.ID_FICHA).DefaultIfEmpty()
                            from sub in _mdb.T_SUBPROGRAMAS.Where(sub=>sub.ID_SUBPROGRAMA == efe.ID_SUBPROGRAMA).DefaultIfEmpty()
                
                            select
                 
                                new Ficha
                                {
                                    IdFicha = c.ID_FICHA,
                                    Nombre = c.NOMBRE,
                                    Apellido = c.APELLIDO,
                                    NumeroDocumento = c.NUMERO_DOCUMENTO,
                                    IdEstadoFicha = c.ID_ESTADO_FICHA ?? (int)Enums.EstadoFicha.SinEstado,
                                    TipoFicha = c.TIPO_FICHA,
                                    Cuil = c.CUIL,
                                    NombreEstadoFicha = c.T_ESTADOS_FICHA.N_ESTADO_FICHA ?? "",
                                    NombreTipoFicha = c.T_TIPO_FICHA.N_TIPO_FICHA,
                                    IdDepartamento = c.T_LOCALIDADES.ID_DEPARTAMENTO,
                                    IdLocalidad = c.ID_LOCALIDAD,
                                    NombreDepartamento = c.T_LOCALIDADES.T_DEPARTAMENTOS.N_DEPARTAMENTO,
                                    Localidad = c.T_LOCALIDADES.N_LOCALIDAD,
                                    idEtapa = c.ID_ETAPA, 
                                    NombreEtapa = c.T_ETAPAS.N_ETAPA,
                                    IdSubprograma = sub.ID_SUBPROGRAMA,
                                    NroTramite = c.NRO_TRAMITE,
                                    TipoPrograma = 0,
                                };

                case (int)Enums.TipoFicha.ReconversionProductiva:

                    return
                            from c in _mdb.T_FICHAS.Where(c => c.TIPO_FICHA == (int)Enums.TipoFicha.ReconversionProductiva)
                            from reco in _mdb.T_FICHA_REC_PROD.Where(reco => reco.ID_FICHA == c.ID_FICHA).DefaultIfEmpty()
                            from sub in _mdb.T_SUBPROGRAMAS.Where(sub => sub.ID_SUBPROGRAMA == reco.ID_SUBPROGRAMA).DefaultIfEmpty()

                            select

                                new Ficha
                                {
                                    IdFicha = c.ID_FICHA,
                                    Nombre = c.NOMBRE,
                                    Apellido = c.APELLIDO,
                                    NumeroDocumento = c.NUMERO_DOCUMENTO,
                                    IdEstadoFicha = c.ID_ESTADO_FICHA ?? (int)Enums.EstadoFicha.SinEstado,
                                    TipoFicha = c.TIPO_FICHA,
                                    Cuil = c.CUIL,
                                    NombreEstadoFicha = c.T_ESTADOS_FICHA.N_ESTADO_FICHA ?? "",
                                    NombreTipoFicha = c.T_TIPO_FICHA.N_TIPO_FICHA,
                                    IdDepartamento = c.T_LOCALIDADES.ID_DEPARTAMENTO,
                                    IdLocalidad = c.ID_LOCALIDAD,
                                    NombreDepartamento = c.T_LOCALIDADES.T_DEPARTAMENTOS.N_DEPARTAMENTO,
                                    Localidad = c.T_LOCALIDADES.N_LOCALIDAD,
                                    idEtapa = c.ID_ETAPA,
                                    NombreEtapa = c.T_ETAPAS.N_ETAPA,
                                    IdSubprograma = sub.ID_SUBPROGRAMA,
                                    NroTramite = c.NRO_TRAMITE,
                                    TipoPrograma = 0,
                                };
                case (int)Enums.TipoFicha.ConfiamosEnVos:

                    return
                            from c in _mdb.T_FICHAS.Where(c => c.TIPO_FICHA == (int)Enums.TipoFicha.ConfiamosEnVos)
                            from conf in _mdb.T_FICHAS_CONF_VOS.Where(conf => conf.ID_FICHA == c.ID_FICHA).DefaultIfEmpty()
                            from sub in _mdb.T_SUBPROGRAMAS.Where(sub => sub.ID_SUBPROGRAMA == conf.ID_SUBPROGRAMA).DefaultIfEmpty()

                            select

                                new Ficha
                                {
                                    IdFicha = c.ID_FICHA,
                                    Nombre = c.NOMBRE,
                                    Apellido = c.APELLIDO,
                                    NumeroDocumento = c.NUMERO_DOCUMENTO,
                                    IdEstadoFicha = c.ID_ESTADO_FICHA ?? (int)Enums.EstadoFicha.SinEstado,
                                    TipoFicha = c.TIPO_FICHA,
                                    Cuil = c.CUIL,
                                    NombreEstadoFicha = c.T_ESTADOS_FICHA.N_ESTADO_FICHA ?? "",
                                    NombreTipoFicha = c.T_TIPO_FICHA.N_TIPO_FICHA,
                                    IdDepartamento = c.T_LOCALIDADES.ID_DEPARTAMENTO,
                                    IdLocalidad = c.ID_LOCALIDAD,
                                    NombreDepartamento = c.T_LOCALIDADES.T_DEPARTAMENTOS.N_DEPARTAMENTO,
                                    Localidad = c.T_LOCALIDADES.N_LOCALIDAD,
                                    idEtapa = c.ID_ETAPA,
                                    NombreEtapa = c.T_ETAPAS.N_ETAPA,
                                    IdSubprograma = sub.ID_SUBPROGRAMA,
                                    NroTramite = c.NRO_TRAMITE,
                                    TipoPrograma = 0,
                                };
                case (int)Enums.TipoFicha.Ppp:

                    return
                            from c in _mdb.T_FICHAS.Where(c => c.TIPO_FICHA == (int)Enums.TipoFicha.Ppp)
                            from Ppp in _mdb.T_FICHA_PPP.Where(Ppp => Ppp.ID_FICHA == c.ID_FICHA).DefaultIfEmpty()
                            from cf in _mdb.T_FICHAS_CAJA.Where(cf => cf.ID_FICHA == c.ID_FICHA).DefaultIfEmpty()
                            from sub in _mdb.T_SUBPROGRAMAS.Where(sub => sub.ID_SUBPROGRAMA == Ppp.ID_SUBPROGRAMA).DefaultIfEmpty()

                            select

                                new Ficha
                                {
                                    IdFicha = c.ID_FICHA,
                                    Nombre = c.NOMBRE,
                                    Apellido = c.APELLIDO,
                                    NumeroDocumento = c.NUMERO_DOCUMENTO,
                                    IdEstadoFicha = c.ID_ESTADO_FICHA ?? (int)Enums.EstadoFicha.SinEstado,
                                    TipoFicha = c.TIPO_FICHA,
                                    Cuil = c.CUIL,
                                    NombreEstadoFicha = c.T_ESTADOS_FICHA.N_ESTADO_FICHA ?? "",
                                    NombreTipoFicha = c.T_TIPO_FICHA.N_TIPO_FICHA,
                                    IdDepartamento = c.T_LOCALIDADES.ID_DEPARTAMENTO,
                                    IdLocalidad = c.ID_LOCALIDAD,
                                    NombreDepartamento = c.T_LOCALIDADES.T_DEPARTAMENTOS.N_DEPARTAMENTO,
                                    Localidad = c.T_LOCALIDADES.N_LOCALIDAD,
                                    idEtapa = c.ID_ETAPA,
                                    NombreEtapa = c.T_ETAPAS.N_ETAPA,
                                    IdSubprograma = sub.ID_SUBPROGRAMA,
                                    TipoPrograma = Ppp.TIPO_PPP ?? 0,
                                    idCaja = cf.ID_CAJA == null ? 0 : cf.ID_CAJA,
                                    NroTramite = c.NRO_TRAMITE,
                                   
                                };


                case (int)Enums.TipoFicha.Terciaria:

                    return
                            from c in _mdb.T_FICHAS.Where(c => c.TIPO_FICHA == (int)Enums.TipoFicha.Terciaria)
                            from ter in _mdb.T_FICHA_TERCIARIO.Where(ter => ter.ID_FICHA == c.ID_FICHA).DefaultIfEmpty()
                            from sub in _mdb.T_SUBPROGRAMAS.Where(sub => sub.ID_SUBPROGRAMA == ter.ID_SUBPROGRAMA).DefaultIfEmpty()

                            select

                                new Ficha
                                {
                                    IdFicha = c.ID_FICHA,
                                    Nombre = c.NOMBRE,
                                    Apellido = c.APELLIDO,
                                    NumeroDocumento = c.NUMERO_DOCUMENTO,
                                    IdEstadoFicha = c.ID_ESTADO_FICHA ?? (int)Enums.EstadoFicha.SinEstado,
                                    TipoFicha = c.TIPO_FICHA,
                                    Cuil = c.CUIL,
                                    NombreEstadoFicha = c.T_ESTADOS_FICHA.N_ESTADO_FICHA ?? "",
                                    NombreTipoFicha = c.T_TIPO_FICHA.N_TIPO_FICHA,
                                    IdDepartamento = c.T_LOCALIDADES.ID_DEPARTAMENTO,
                                    IdLocalidad = c.ID_LOCALIDAD,
                                    NombreDepartamento = c.T_LOCALIDADES.T_DEPARTAMENTOS.N_DEPARTAMENTO,
                                    Localidad = c.T_LOCALIDADES.N_LOCALIDAD,
                                    idEtapa = c.ID_ETAPA,
                                    NombreEtapa = c.T_ETAPAS.N_ETAPA,
                                    IdSubprograma = sub.ID_SUBPROGRAMA,
                                    NroTramite = c.NRO_TRAMITE,
                                    TipoPrograma = 0,
                                };



                case (int)Enums.TipoFicha.Universitaria:

                    return
                            from c in _mdb.T_FICHAS.Where(c => c.TIPO_FICHA == (int)Enums.TipoFicha.Universitaria)
                            from uni in _mdb.T_FICHA_UNIVERSITARIO.Where(uni => uni.ID_FICHA == c.ID_FICHA).DefaultIfEmpty()
                            from sub in _mdb.T_SUBPROGRAMAS.Where(sub => sub.ID_SUBPROGRAMA == uni.ID_SUBPROGRAMA).DefaultIfEmpty()

                            select

                                new Ficha
                                {
                                    IdFicha = c.ID_FICHA,
                                    Nombre = c.NOMBRE,
                                    Apellido = c.APELLIDO,
                                    NumeroDocumento = c.NUMERO_DOCUMENTO,
                                    IdEstadoFicha = c.ID_ESTADO_FICHA ?? (int)Enums.EstadoFicha.SinEstado,
                                    TipoFicha = c.TIPO_FICHA,
                                    Cuil = c.CUIL,
                                    NombreEstadoFicha = c.T_ESTADOS_FICHA.N_ESTADO_FICHA ?? "",
                                    NombreTipoFicha = c.T_TIPO_FICHA.N_TIPO_FICHA,
                                    IdDepartamento = c.T_LOCALIDADES.ID_DEPARTAMENTO,
                                    IdLocalidad = c.ID_LOCALIDAD,
                                    NombreDepartamento = c.T_LOCALIDADES.T_DEPARTAMENTOS.N_DEPARTAMENTO,
                                    Localidad = c.T_LOCALIDADES.N_LOCALIDAD,
                                    idEtapa = c.ID_ETAPA,
                                    NombreEtapa = c.T_ETAPAS.N_ETAPA,
                                    IdSubprograma = sub.ID_SUBPROGRAMA,
                                    NroTramite = c.NRO_TRAMITE,
                                    TipoPrograma = 0,

                                };

                default:
                    return
                         from c in _mdb.T_FICHAS
                             from cf in _mdb.T_FICHAS_CAJA.Where(cf => cf.ID_FICHA == c.ID_FICHA).DefaultIfEmpty()
                             select
                                    new Ficha
                                    {
                                        IdFicha = c.ID_FICHA,
                                        Nombre = c.NOMBRE,
                                        Apellido = c.APELLIDO,
                                        NumeroDocumento = c.NUMERO_DOCUMENTO,
                                        IdEstadoFicha = c.ID_ESTADO_FICHA ?? (int)Enums.EstadoFicha.SinEstado,
                                        TipoFicha = c.TIPO_FICHA,
                                        Cuil = c.CUIL,
                                        NombreEstadoFicha = c.T_ESTADOS_FICHA.N_ESTADO_FICHA ?? "",
                                        NombreTipoFicha = c.T_TIPO_FICHA.N_TIPO_FICHA,
                                        IdDepartamento = c.T_LOCALIDADES.ID_DEPARTAMENTO,
                                        IdLocalidad = c.ID_LOCALIDAD,
                                        NombreDepartamento = c.T_LOCALIDADES.T_DEPARTAMENTOS.N_DEPARTAMENTO,
                                        Localidad = c.T_LOCALIDADES.N_LOCALIDAD,
                                        idEtapa = c.ID_ETAPA, // 14/06/2013 - DI CAMPLI LEANDRO - SE AÑADE LA ETAPA A LA CONSULTA PARA PODER FILTRAR
                                        NombreEtapa = c.T_ETAPAS.N_ETAPA, // 14/06/2013 - DI CAMPLI LEANDRO - SE AÑADE LA ETAPA A LA CONSULTA PARA PODER FILTRAR
                                        NroTramite = c.NRO_TRAMITE,
                                        TipoPrograma = c.TIPO_FICHA == 3 ? c.T_FICHA_PPP.TIPO_PPP : 0,
                                        idCaja = cf.ID_CAJA == null ? 0 : cf.ID_CAJA // 13/08/2018

                                    };
            
            
            
            }
            
        }

        private IQueryable<IFicha> QFichaSimple(string cuil, string dni, string nombre, string apellido, int tipoficha, int estadoficha, int idEtapa)
        {
            return
                from c in _mdb.T_FICHAS
                
                where 

                    (c.APELLIDO.ToLower().Contains(apellido.ToLower()) || String.IsNullOrEmpty(apellido.Trim())) &&
                    (c.NOMBRE.ToLower().Contains(nombre.ToLower()) || String.IsNullOrEmpty(nombre.Trim())) &&
                    (c.ID_ESTADO_FICHA == estadoficha || estadoficha == -1) &&
                    (c.NUMERO_DOCUMENTO.Contains(cuil) || String.IsNullOrEmpty(cuil.Trim())) &&
                    (c.CUIL.Contains(dni) || String.IsNullOrEmpty(dni.Trim())) &&
                    (c.TIPO_FICHA == tipoficha || tipoficha == 0) &&
                    (c.ID_ETAPA == idEtapa || idEtapa == 0)

                
                select
                    new Ficha
                    {
                        IdFicha = c.ID_FICHA,
                        Nombre = c.NOMBRE,
                        Apellido = c.APELLIDO,
                        NumeroDocumento = c.NUMERO_DOCUMENTO,
                        IdEstadoFicha = c.ID_ESTADO_FICHA ?? (int)Enums.EstadoFicha.SinEstado,
                        TipoFicha = c.TIPO_FICHA,
                        Cuil = c.CUIL,
                        NombreEstadoFicha = c.T_ESTADOS_FICHA.N_ESTADO_FICHA ?? "",
                        NombreTipoFicha = c.T_TIPO_FICHA.N_TIPO_FICHA,
                        IdDepartamento = c.T_LOCALIDADES.ID_DEPARTAMENTO,
                        IdLocalidad = c.ID_LOCALIDAD,
                        NombreDepartamento = c.T_LOCALIDADES.T_DEPARTAMENTOS.N_DEPARTAMENTO,
                        Localidad = c.T_LOCALIDADES.N_LOCALIDAD,
                        idEtapa = c.ID_ETAPA, // 14/06/2013 - DI CAMPLI LEANDRO - SE AÑADE LA ETAPA A LA CONSULTA PARA PODER FILTRAR
                        NombreEtapa = c.T_ETAPAS.N_ETAPA, // 14/06/2013 - DI CAMPLI LEANDRO - SE AÑADE LA ETAPA A LA CONSULTA PARA PODER FILTRAR


                    };
        }

        private IQueryable<IFicha> QFichaSimpleReportes()
        {
            var a = (from c in _mdb.T_FICHAS
                     from be in
                         _mdb.T_BENEFICIARIOS.Where(fiter => fiter.ID_FICHA == c.ID_FICHA).DefaultIfEmpty()
                     select new Ficha
                     {
                         IdFicha = c.ID_FICHA,
                         Nombre = c.NOMBRE,
                         Apellido = c.APELLIDO,
                         NumeroDocumento = c.NUMERO_DOCUMENTO,
                         IdEstadoFicha = c.ID_ESTADO_FICHA ?? (int)Enums.EstadoFicha.SinEstado,
                         TipoFicha = c.TIPO_FICHA,
                         Cuil = c.CUIL,
                         NombreEstadoFicha = c.T_ESTADOS_FICHA.N_ESTADO_FICHA ?? "",
                         NombreTipoFicha = c.T_TIPO_FICHA.N_TIPO_FICHA,
                         IdDepartamento = c.T_LOCALIDADES.ID_DEPARTAMENTO,
                         IdLocalidad = c.ID_LOCALIDAD,
                         NombreDepartamento = c.T_LOCALIDADES.T_DEPARTAMENTOS.N_DEPARTAMENTO,
                         Localidad = c.T_LOCALIDADES.N_LOCALIDAD,
                         BeneficiarioFicha = new Beneficiario { IdBeneficiarioExport = be.ID_BENEFICIARIO, IdEstadoExport = be.ID_ESTADO, IdPrograma = be.ID_PROGRAMA ?? 0 }
                        
                     }

                  );

            return a;
        }

        private IQueryable<IFicha> QFichaSimpleReportes(int idPrograma)
        {
            switch(idPrograma)
            {
                case 3:
                    var ppp = (from c in _mdb.T_FICHAS
                               join fppp in _mdb.T_FICHA_PPP on c.ID_FICHA equals fppp.ID_FICHA
                                 from be in
                                     _mdb.T_BENEFICIARIOS.Where(fiter => fiter.ID_FICHA == c.ID_FICHA).DefaultIfEmpty()
                                    
                                 select new Ficha
                                 {
                                     IdFicha = c.ID_FICHA,
                                     Nombre = c.NOMBRE,
                                     Apellido = c.APELLIDO,
                                     NumeroDocumento = c.NUMERO_DOCUMENTO,
                                     IdEstadoFicha = c.ID_ESTADO_FICHA ?? (int)Enums.EstadoFicha.SinEstado,
                                     TipoFicha = c.TIPO_FICHA,
                                     Cuil = c.CUIL,
                                     NombreEstadoFicha = c.T_ESTADOS_FICHA.N_ESTADO_FICHA ?? "",
                                     NombreTipoFicha = c.T_TIPO_FICHA.N_TIPO_FICHA,
                                     IdDepartamento = c.T_LOCALIDADES.ID_DEPARTAMENTO,
                                     IdLocalidad = c.ID_LOCALIDAD,
                                     NombreDepartamento = c.T_LOCALIDADES.T_DEPARTAMENTOS.N_DEPARTAMENTO,
                                     Localidad = c.T_LOCALIDADES.N_LOCALIDAD,
                                     BeneficiarioFicha = new Beneficiario { IdBeneficiarioExport = be.ID_BENEFICIARIO, IdEstadoExport = be.ID_ESTADO, IdPrograma = be.ID_PROGRAMA ?? 0 },
                                     TipoPrograma = fppp.TIPO_PPP ?? 1



                                 }

                              );
                    return ppp;

                default:
                    var a = (from c in _mdb.T_FICHAS
                               from be in
                                   _mdb.T_BENEFICIARIOS.Where(fiter => fiter.ID_FICHA == c.ID_FICHA).DefaultIfEmpty()
                               select new Ficha
                               {
                                   IdFicha = c.ID_FICHA,
                                   Nombre = c.NOMBRE,
                                   Apellido = c.APELLIDO,
                                   NumeroDocumento = c.NUMERO_DOCUMENTO,
                                   IdEstadoFicha = c.ID_ESTADO_FICHA ?? (int)Enums.EstadoFicha.SinEstado,
                                   TipoFicha = c.TIPO_FICHA,
                                   Cuil = c.CUIL,
                                   NombreEstadoFicha = c.T_ESTADOS_FICHA.N_ESTADO_FICHA ?? "",
                                   NombreTipoFicha = c.T_TIPO_FICHA.N_TIPO_FICHA,
                                   IdDepartamento = c.T_LOCALIDADES.ID_DEPARTAMENTO,
                                   IdLocalidad = c.ID_LOCALIDAD,
                                   NombreDepartamento = c.T_LOCALIDADES.T_DEPARTAMENTOS.N_DEPARTAMENTO,
                                   Localidad = c.T_LOCALIDADES.N_LOCALIDAD,
                                   BeneficiarioFicha = new Beneficiario { IdBeneficiarioExport = be.ID_BENEFICIARIO, IdEstadoExport = be.ID_ESTADO, IdPrograma = be.ID_PROGRAMA ?? 0 }

                               });
                    return a;
        }


        }

        private IQueryable<IFicha> QFichaDetalleReportes(int? tipoficha)
        {
            switch (tipoficha)
            {
                case (int)Enums.TipoFicha.Ppp:
                    var a = (from fic in _mdb.T_FICHAS
                             // from be in
                             //_mdb.T_BENEFICIARIOS.Where(fiter => fiter.ID_FICHA == c.ID_FICHA && fiter.ID_ESTADO == (int)Enums.EstadoBeneficiario.Activo).DefaultIfEmpty()
                             from c in
                                 _mdb.T_BENEFICIARIOS.Where(fiter => fiter.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                             from cu in
                                 _mdb.T_CUENTAS_BANCO.Where(cu => cu.ID_BENEFICIARIO == c.ID_BENEFICIARIO).
                                 DefaultIfEmpty()
                             from suc in
                                 _mdb.T_TABLAS_BCO_CBA.Where(suc => suc.ID_TABLA_BCO_CBA == cu.ID_SUCURSAL).
                                 DefaultIfEmpty()
                             from mon in
                                 _mdb.T_TABLAS_BCO_CBA.Where(mon => mon.ID_TABLA_BCO_CBA == cu.ID_MONEDA).DefaultIfEmpty
                                 ()
                             from iva in
                                 _mdb.T_TABLAS_BCO_CBA.Where(iva => iva.ID_TABLA_BCO_CBA == c.CONDICION_IVA).
                                 DefaultIfEmpty()
                             from nac in
                                 _mdb.T_TABLAS_BCO_CBA.Where(nac => nac.ID_TABLA_BCO_CBA == c.NACIONALIDAD).
                                 DefaultIfEmpty()
                             from fichappp in
                                 _mdb.T_FICHA_PPP.Where(
                                     fichappp => fichappp.ID_FICHA == fic.ID_FICHA && fic.TIPO_FICHA == tipoficha).
                                 DefaultIfEmpty()
                             from ap in
                                 _mdb.T_APODERADOS.Where(
                                     ap =>
                                     ap.ID_BENEFICIARIO == c.ID_BENEFICIARIO &&
                                     ap.ID_ESTADO_APODERADO == (short)Enums.EstadoApoderado.Activo).DefaultIfEmpty()
                             from sucapo in
                                 _mdb.T_TABLAS_BCO_CBA.Where(
                                     sucapo =>
                                     sucapo.ID_TABLA_BCO_CBA == ap.ID_SUCURSAL_BCO &&
                                     sucapo.ID_TIPO_TABLA_BCO_CBA == (int)Enums.TiposTablaBcoCba.Sucursales).
                                 DefaultIfEmpty()
                             where fic.TIPO_FICHA == tipoficha
                             select new Ficha
                             {
                                 IdFicha = fic.ID_FICHA,
                                 Nombre = fic.NOMBRE,
                                 Apellido = fic.APELLIDO,
                                 NumeroDocumento = fic.NUMERO_DOCUMENTO,
                                 IdEstadoFicha = fic.ID_ESTADO_FICHA ?? (int)Enums.EstadoFicha.SinEstado,
                                 TipoFicha = fic.TIPO_FICHA,
                                 NombreEstadoFicha = fic.T_ESTADOS_FICHA.N_ESTADO_FICHA ?? "",
                                 NombreTipoFicha = fic.T_TIPO_FICHA.N_TIPO_FICHA,
                                 Localidad = fic.T_LOCALIDADES.N_LOCALIDAD,
                                 IdLocalidad = fic.ID_LOCALIDAD ?? 0,
                                 IdDepartamento = fic.T_LOCALIDADES.ID_DEPARTAMENTO,
                                 FechaSistema = c.FEC_SIST,
                                 IdUsuarioSistema = c.ID_USR_SIST,
                                 UsuarioSistema = c.T_USUARIOS.LOGIN,
                                 TipoPrograma = fichappp.TIPO_PPP ?? 1,
                                 //Beneficiarios
                                 BeneficiarioFicha = new Beneficiario
                                 {
                                     IdEstadoExport = c.ID_ESTADO,
                                     IdBeneficiarioExport = c.ID_BENEFICIARIO,
                                     IdEstado = c.ID_ESTADO ?? 0,
                                     NombreEstado = c.T_ESTADOS.N_ESTADO,
                                     FechaSistema = c.FEC_SIST,
                                     IdFicha = c.ID_FICHA ?? 0,
                                     IdPrograma = c.ID_PROGRAMA ?? 0,
                                     IdUsuarioSistema = c.ID_USR_SIST,
                                     Programa = c.T_PROGRAMAS.N_PROGRAMA
                                 },
                                 FichaPpp =
                                     new FichaPPP
                                     {
                                         IdEmpresa = fichappp.ID_EMPRESA,
                                         Empresa =
                                             new Empresa
                                             {
                                                 Cuit = fichappp.T_EMPRESAS.CUIT,
                                                 IdEmpresa = fichappp.ID_EMPRESA ?? 0,
                                                 NombreEmpresa = fichappp.T_EMPRESAS.N_EMPRESA,
                                             }
                                     },
                                 LocalidadFicha =
                                     new Localidad
                                     {
                                         IdLocalidad = fic.ID_LOCALIDAD ?? 0,
                                         NombreLocalidad = fic.T_LOCALIDADES.N_LOCALIDAD,
                                         Departamento =
                                             new Departamento
                                             {
                                                 IdDepartamento = fic.T_LOCALIDADES.ID_DEPARTAMENTO ?? 0,
                                                 NombreDepartamento = fic.T_LOCALIDADES.T_DEPARTAMENTOS.N_DEPARTAMENTO
                                             }
                                     }
                             }
                       );
                    return a;
                case (int)Enums.TipoFicha.PppProf:
                    var pppp = (from fic in _mdb.T_FICHAS
                                from c in
                                    _mdb.T_BENEFICIARIOS.Where(fiter => fiter.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                                from fiterc in
                                    _mdb.T_FICHA_TERCIARIO.Where(fiter => fiter.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                                from fiUniv in
                                    _mdb.T_FICHA_UNIVERSITARIO.Where(fiter => fiter.ID_FICHA == fic.ID_FICHA).
                                    DefaultIfEmpty()
                                from cu in
                                    _mdb.T_CUENTAS_BANCO.Where(cu => cu.ID_BENEFICIARIO == c.ID_BENEFICIARIO).
                                    DefaultIfEmpty()
                                from suc in
                                    _mdb.T_TABLAS_BCO_CBA.Where(suc => suc.ID_TABLA_BCO_CBA == cu.ID_SUCURSAL).
                                    DefaultIfEmpty()
                                from mon in
                                    _mdb.T_TABLAS_BCO_CBA.Where(mon => mon.ID_TABLA_BCO_CBA == cu.ID_MONEDA).DefaultIfEmpty()
                                from iva in
                                    _mdb.T_TABLAS_BCO_CBA.Where(iva => iva.ID_TABLA_BCO_CBA == c.CONDICION_IVA).
                                    DefaultIfEmpty()
                                from nac in
                                    _mdb.T_TABLAS_BCO_CBA.Where(nac => nac.ID_TABLA_BCO_CBA == c.NACIONALIDAD).
                                    DefaultIfEmpty()
                                from fichaPppp in
                                    _mdb.T_FICHA_PPP_PROF.Where(
                                        fipppp => fipppp.ID_FICHA == fic.ID_FICHA && fic.TIPO_FICHA == tipoficha).
                                    DefaultIfEmpty()
                                from ap in
                                    _mdb.T_APODERADOS.Where(
                                        ap =>
                                        ap.ID_BENEFICIARIO == c.ID_BENEFICIARIO &&
                                        ap.ID_ESTADO_APODERADO == (short)Enums.EstadoApoderado.Activo).DefaultIfEmpty()
                                from sucapo in
                                    _mdb.T_TABLAS_BCO_CBA.Where(
                                        sucapo =>
                                        sucapo.ID_TABLA_BCO_CBA == ap.ID_SUCURSAL_BCO &&
                                        sucapo.ID_TIPO_TABLA_BCO_CBA == (int)Enums.TiposTablaBcoCba.Sucursales).
                                    DefaultIfEmpty()
                                where fic.TIPO_FICHA == tipoficha
                                select new Ficha
                                {
                                     IdFicha = fic.ID_FICHA,
                                     Nombre = fic.NOMBRE,
                                     Apellido = fic.APELLIDO,
                                     NumeroDocumento = fic.NUMERO_DOCUMENTO,
                                     IdEstadoFicha = fic.ID_ESTADO_FICHA ?? (int)Enums.EstadoFicha.SinEstado,
                                     TipoFicha = fic.TIPO_FICHA,
                                     NombreEstadoFicha = fic.T_ESTADOS_FICHA.N_ESTADO_FICHA ?? "",
                                     NombreTipoFicha = fic.T_TIPO_FICHA.N_TIPO_FICHA,
                                     Localidad = fic.T_LOCALIDADES.N_LOCALIDAD,
                                     IdLocalidad = fic.ID_LOCALIDAD ?? 0,
                                     IdDepartamento = fic.T_LOCALIDADES.ID_DEPARTAMENTO,
                                     FechaSistema = c.FEC_SIST,
                                     IdUsuarioSistema = c.ID_USR_SIST,
                                     UsuarioSistema = c.T_USUARIOS.LOGIN,
                                     //Beneficiarios
                                     BeneficiarioFicha = new Beneficiario
                                     {
                                         IdEstadoExport = c.ID_ESTADO,
                                         IdBeneficiarioExport = c.ID_BENEFICIARIO,
                                         IdEstado = c.ID_ESTADO ?? 0,
                                         NombreEstado = c.T_ESTADOS.N_ESTADO,
                                         FechaSistema = c.FEC_SIST,
                                         IdFicha = c.ID_FICHA ?? 0,
                                         IdPrograma = c.ID_PROGRAMA ?? 0,
                                         IdUsuarioSistema = c.ID_USR_SIST,
                                         Programa = c.T_PROGRAMAS.N_PROGRAMA
                                     },
                                  FichaPppp =
                                        new FichaPPPP
                                        {
                                            IdEmpresa = fichaPppp.ID_EMPRESA,
                                            Empresa =
                                                new Empresa
                                                {
                                                    Cuit = fichaPppp.T_EMPRESAS.CUIT,
                                                    IdEmpresa = fichaPppp.ID_EMPRESA ?? 0,
                                                    NombreEmpresa = fichaPppp.T_EMPRESAS.N_EMPRESA,
                                                }
                                        },
                                    LocalidadFicha =
                                        new Localidad
                                        {
                                            IdLocalidad = fic.ID_LOCALIDAD ?? 0,
                                            NombreLocalidad = fic.T_LOCALIDADES.N_LOCALIDAD,
                                            Departamento =
                                                new Departamento
                                                {
                                                    IdDepartamento =fic.T_LOCALIDADES.ID_DEPARTAMENTO ?? 0,
                                                    NombreDepartamento =fic.T_LOCALIDADES.T_DEPARTAMENTOS.N_DEPARTAMENTO
                                                }
                                        }
                                }
                            );

                    return pppp;
                case (int)Enums.TipoFicha.Vat:
                    var vat = (from fic in _mdb.T_FICHAS
                               from c in
                                   _mdb.T_BENEFICIARIOS.Where(fiter => fiter.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                               from cu in
                                   _mdb.T_CUENTAS_BANCO.Where(cu => cu.ID_BENEFICIARIO == c.ID_BENEFICIARIO).
                                   DefaultIfEmpty()
                               from suc in
                                   _mdb.T_TABLAS_BCO_CBA.Where(suc => suc.ID_TABLA_BCO_CBA == cu.ID_SUCURSAL).
                                   DefaultIfEmpty()
                               from mon in
                                   _mdb.T_TABLAS_BCO_CBA.Where(mon => mon.ID_TABLA_BCO_CBA == cu.ID_MONEDA).
                                   DefaultIfEmpty()
                               from iva in
                                   _mdb.T_TABLAS_BCO_CBA.Where(iva => iva.ID_TABLA_BCO_CBA == c.CONDICION_IVA).
                                   DefaultIfEmpty()
                               from nac in
                                   _mdb.T_TABLAS_BCO_CBA.Where(nac => nac.ID_TABLA_BCO_CBA == c.NACIONALIDAD).
                                   DefaultIfEmpty()
                               from fichaVat in
                                   _mdb.T_FICHA_VAT.Where(
                                       fivat => fivat.ID_FICHA == fic.ID_FICHA && fic.TIPO_FICHA == tipoficha).
                                   DefaultIfEmpty()
                               from ap in
                                   _mdb.T_APODERADOS.Where(
                                       ap =>
                                       ap.ID_BENEFICIARIO == c.ID_BENEFICIARIO &&
                                       ap.ID_ESTADO_APODERADO == (short)Enums.EstadoApoderado.Activo).DefaultIfEmpty()
                               from sucapo in
                                   _mdb.T_TABLAS_BCO_CBA.Where(
                                       sucapo =>
                                       sucapo.ID_TABLA_BCO_CBA == ap.ID_SUCURSAL_BCO &&
                                       sucapo.ID_TIPO_TABLA_BCO_CBA == (int)Enums.TiposTablaBcoCba.Sucursales).
                                   DefaultIfEmpty()
                               where fic.TIPO_FICHA == tipoficha
                               select new Ficha
                               {
                                 IdFicha = fic.ID_FICHA,
                                 Nombre = fic.NOMBRE,
                                 Apellido = fic.APELLIDO,
                                 NumeroDocumento = fic.NUMERO_DOCUMENTO,
                                 IdEstadoFicha = fic.ID_ESTADO_FICHA ?? (int)Enums.EstadoFicha.SinEstado,
                                 TipoFicha = fic.TIPO_FICHA,
                                 NombreEstadoFicha = fic.T_ESTADOS_FICHA.N_ESTADO_FICHA ?? "",
                                 NombreTipoFicha = fic.T_TIPO_FICHA.N_TIPO_FICHA,
                                 Localidad = fic.T_LOCALIDADES.N_LOCALIDAD,
                                 IdLocalidad = fic.ID_LOCALIDAD ?? 0,
                                 IdDepartamento = fic.T_LOCALIDADES.ID_DEPARTAMENTO,
                                 FechaSistema = c.FEC_SIST,
                                 IdUsuarioSistema = c.ID_USR_SIST,
                                 UsuarioSistema = c.T_USUARIOS.LOGIN,
                                 //Beneficiarios
                                 BeneficiarioFicha = new Beneficiario
                                 {
                                     IdEstadoExport = c.ID_ESTADO,
                                     IdBeneficiarioExport = c.ID_BENEFICIARIO,
                                     IdEstado = c.ID_ESTADO ?? 0,
                                     NombreEstado = c.T_ESTADOS.N_ESTADO,
                                     FechaSistema = c.FEC_SIST,
                                     IdFicha = c.ID_FICHA ?? 0,
                                     IdPrograma = c.ID_PROGRAMA ?? 0,
                                     IdUsuarioSistema = c.ID_USR_SIST,
                                     Programa = c.T_PROGRAMAS.N_PROGRAMA
                                 },
                                   FichaVat =
                                       new FichaVAT
                                       {
                                           IdEmpresa = fichaVat.ID_EMPRESA,
                                           Empresa =
                                               new Empresa
                                               {
                                                   Cuit = fichaVat.T_EMPRESAS.CUIT,
                                                   IdEmpresa = fichaVat.ID_EMPRESA ?? 0,
                                                   NombreEmpresa = fichaVat.T_EMPRESAS.N_EMPRESA,
                                               }
                                       },
                                   LocalidadFicha =
                                       new Localidad
                                       {
                                           IdLocalidad = fic.ID_LOCALIDAD ?? 0,
                                           NombreLocalidad = fic.T_LOCALIDADES.N_LOCALIDAD,
                                           Departamento =
                                               new Departamento
                                               {
                                                   IdDepartamento = fic.T_LOCALIDADES.ID_DEPARTAMENTO ?? 0,
                                                   NombreDepartamento = fic.T_LOCALIDADES.T_DEPARTAMENTOS.N_DEPARTAMENTO
                                               }
                                       }
                               }
                              );

                    return vat;
                case (int)Enums.TipoFicha.Terciaria:
                    var terciario = (from fic in _mdb.T_FICHAS
                                     from c in
                                         _mdb.T_BENEFICIARIOS.Where(fiter => fiter.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                                     from fiterc in
                                         _mdb.T_FICHA_TERCIARIO.Where(fiter => fiter.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                                     from cu in
                                         _mdb.T_CUENTAS_BANCO.Where(cu => cu.ID_BENEFICIARIO == c.ID_BENEFICIARIO).DefaultIfEmpty()
                                     from suc in
                                         _mdb.T_TABLAS_BCO_CBA.Where(suc => suc.ID_TABLA_BCO_CBA == cu.ID_SUCURSAL).DefaultIfEmpty()
                                     from mon in
                                         _mdb.T_TABLAS_BCO_CBA.Where(mon => mon.ID_TABLA_BCO_CBA == cu.ID_MONEDA).DefaultIfEmpty()
                                     from iva in
                                         _mdb.T_TABLAS_BCO_CBA.Where(iva => iva.ID_TABLA_BCO_CBA == c.CONDICION_IVA).DefaultIfEmpty()
                                     from nac in
                                         _mdb.T_TABLAS_BCO_CBA.Where(nac => nac.ID_TABLA_BCO_CBA == c.NACIONALIDAD).DefaultIfEmpty()
                                     from ap in
                                         _mdb.T_APODERADOS.Where(
                                             ap =>
                                             ap.ID_BENEFICIARIO == c.ID_BENEFICIARIO &&
                                             ap.ID_ESTADO_APODERADO == (short)Enums.EstadoApoderado.Activo).DefaultIfEmpty()
                                     from sucapo in
                                         _mdb.T_TABLAS_BCO_CBA.Where(
                                             sucapo =>
                                             sucapo.ID_TABLA_BCO_CBA == ap.ID_SUCURSAL_BCO &&
                                             sucapo.ID_TIPO_TABLA_BCO_CBA == (int)Enums.TiposTablaBcoCba.Sucursales).
                                         DefaultIfEmpty()
                                     where fic.TIPO_FICHA == tipoficha
                                     select new Ficha
                                     {
                                         IdFicha = fic.ID_FICHA,
                                         Nombre = fic.NOMBRE,
                                         Apellido = fic.APELLIDO,
                                         NumeroDocumento = fic.NUMERO_DOCUMENTO,
                                         IdEstadoFicha = fic.ID_ESTADO_FICHA ?? (int)Enums.EstadoFicha.SinEstado,
                                         TipoFicha = fic.TIPO_FICHA,
                                         NombreEstadoFicha = fic.T_ESTADOS_FICHA.N_ESTADO_FICHA ?? "",
                                         NombreTipoFicha = fic.T_TIPO_FICHA.N_TIPO_FICHA,
                                         Localidad = fic.T_LOCALIDADES.N_LOCALIDAD,
                                         IdLocalidad = fic.ID_LOCALIDAD ?? 0,
                                         IdDepartamento = fic.T_LOCALIDADES.ID_DEPARTAMENTO,
                                         FechaSistema = c.FEC_SIST,
                                         IdUsuarioSistema = c.ID_USR_SIST,
                                         UsuarioSistema = c.T_USUARIOS.LOGIN,
                                         //Beneficiarios
                                         BeneficiarioFicha = new Beneficiario
                                         {
                                             IdEstadoExport = c.ID_ESTADO,
                                             IdBeneficiarioExport = c.ID_BENEFICIARIO,
                                             IdEstado = c.ID_ESTADO ?? 0,
                                             NombreEstado = c.T_ESTADOS.N_ESTADO,
                                             FechaSistema = c.FEC_SIST,
                                             IdFicha = c.ID_FICHA ?? 0,
                                             IdPrograma = c.ID_PROGRAMA ?? 0,
                                             IdUsuarioSistema = c.ID_USR_SIST,
                                             Programa = c.T_PROGRAMAS.N_PROGRAMA
                                         },
                                         //Fichas Terciarias
                                         IdCarreraTerciaria = fiterc.ID_CARRERA,
                                         IdEscuelaTerciaria = fiterc.ID_ESCUELA,
                                         NombreEscuela = fiterc.T_ESCUELAS.N_ESCUELA,
                                         EscuelaFicha =
                                             new Escuela
                                             {
                                                 Id_Escuela = fiterc.ID_ESCUELA ?? 0,
                                                 Nombre_Escuela = fiterc.T_ESCUELAS.N_ESCUELA,
                                                 Id_Localidad = fiterc.T_ESCUELAS.ID_LOCALIDAD,
                                                 LocalidadEscuela =
                                                     new Localidad
                                                     {
                                                         IdLocalidad = fiterc.T_ESCUELAS.ID_LOCALIDAD ?? 0,
                                                         NombreLocalidad = fiterc.T_ESCUELAS.T_LOCALIDADES.N_LOCALIDAD,
                                                         Departamento =
                                                             new Departamento
                                                             {
                                                                 IdDepartamento = fiterc.T_ESCUELAS.T_LOCALIDADES.ID_DEPARTAMENTO ?? 0,
                                                                 NombreDepartamento = fiterc.T_ESCUELAS.T_LOCALIDADES.T_DEPARTAMENTOS.N_DEPARTAMENTO
                                                             }
                                                     }
                                             },
                                         CarreraFicha =
                                             new Carrera
                                             {
                                                 IdCarrera = fiterc.ID_CARRERA ?? 0,
                                                 NombreCarrera = fiterc.T_CARRERAS.N_CARRERA,
                                                 IdInstitucion = fiterc.T_CARRERAS.ID_INSTITUCION,
                                                 InstitucionCarrera =
                                                     new Institucion
                                                     {
                                                         IdInstitucion = fiterc.T_CARRERAS.ID_INSTITUCION ?? 0,
                                                         NombreInstitucion = fiterc.T_CARRERAS.T_SECTOR_INSTITUCION.T_INSTITUCIONES.N_INSTITUCION
                                                     }
                                             },
                                         LocalidadFicha =
                                             new Localidad
                                             {
                                                 IdLocalidad = fic.ID_LOCALIDAD ?? 0,
                                                 NombreLocalidad = fic.T_LOCALIDADES.N_LOCALIDAD,
                                                 Departamento =
                                                     new Departamento
                                                     {
                                                         IdDepartamento = fic.T_LOCALIDADES.ID_DEPARTAMENTO ?? 0,
                                                         NombreDepartamento = fic.T_LOCALIDADES.T_DEPARTAMENTOS.N_DEPARTAMENTO
                                                     }
                                             }
                                     }
                    );
                    return terciario;
                case (int)Enums.TipoFicha.Universitaria:
                    var universitaria = (from fic in _mdb.T_FICHAS
                                         from c in
                                             _mdb.T_BENEFICIARIOS.Where(fiter => fiter.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                                         from fiUniv in
                                             _mdb.T_FICHA_UNIVERSITARIO.Where(fiter => fiter.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                                         from cu in
                                             _mdb.T_CUENTAS_BANCO.Where(cu => cu.ID_BENEFICIARIO == c.ID_BENEFICIARIO).DefaultIfEmpty()
                                         from suc in
                                             _mdb.T_TABLAS_BCO_CBA.Where(suc => suc.ID_TABLA_BCO_CBA == cu.ID_SUCURSAL).DefaultIfEmpty()
                                         from mon in
                                             _mdb.T_TABLAS_BCO_CBA.Where(mon => mon.ID_TABLA_BCO_CBA == cu.ID_MONEDA).DefaultIfEmpty()
                                         from iva in
                                             _mdb.T_TABLAS_BCO_CBA.Where(iva => iva.ID_TABLA_BCO_CBA == c.CONDICION_IVA).DefaultIfEmpty()
                                         from nac in
                                             _mdb.T_TABLAS_BCO_CBA.Where(nac => nac.ID_TABLA_BCO_CBA == c.NACIONALIDAD).DefaultIfEmpty()
                                         from ap in
                                             _mdb.T_APODERADOS.Where(
                                                 ap =>
                                                 ap.ID_BENEFICIARIO == c.ID_BENEFICIARIO &&
                                                 ap.ID_ESTADO_APODERADO == (short)Enums.EstadoApoderado.Activo).DefaultIfEmpty()
                                         from sucapo in
                                             _mdb.T_TABLAS_BCO_CBA.Where(
                                                 sucapo =>
                                                 sucapo.ID_TABLA_BCO_CBA == ap.ID_SUCURSAL_BCO &&
                                                 sucapo.ID_TIPO_TABLA_BCO_CBA == (int)Enums.TiposTablaBcoCba.Sucursales).
                                             DefaultIfEmpty()
                                         where fic.TIPO_FICHA == tipoficha
                                         select new Ficha
                                         {
                                             IdFicha = fic.ID_FICHA,
                                             Nombre = fic.NOMBRE,
                                             Apellido = fic.APELLIDO,
                                             NumeroDocumento = fic.NUMERO_DOCUMENTO,
                                             IdEstadoFicha = fic.ID_ESTADO_FICHA ?? (int)Enums.EstadoFicha.SinEstado,
                                             TipoFicha = fic.TIPO_FICHA,
                                             NombreEstadoFicha = fic.T_ESTADOS_FICHA.N_ESTADO_FICHA ?? "",
                                             NombreTipoFicha = fic.T_TIPO_FICHA.N_TIPO_FICHA,
                                             Localidad = fic.T_LOCALIDADES.N_LOCALIDAD,
                                             IdLocalidad = fic.ID_LOCALIDAD ?? 0,
                                             IdDepartamento = fic.T_LOCALIDADES.ID_DEPARTAMENTO,
                                             FechaSistema = c.FEC_SIST,
                                             IdUsuarioSistema = c.ID_USR_SIST,
                                             UsuarioSistema = c.T_USUARIOS.LOGIN,
                                             //Beneficiarios
                                             BeneficiarioFicha = new Beneficiario
                                             {
                                                 IdEstadoExport = c.ID_ESTADO,
                                                 IdBeneficiarioExport = c.ID_BENEFICIARIO,
                                                 IdEstado = c.ID_ESTADO ?? 0,
                                                 NombreEstado = c.T_ESTADOS.N_ESTADO,
                                                 FechaSistema = c.FEC_SIST,
                                                 IdFicha = c.ID_FICHA ?? 0,
                                                 IdPrograma = c.ID_PROGRAMA ?? 0,
                                                 IdUsuarioSistema = c.ID_USR_SIST,
                                                 Programa = c.T_PROGRAMAS.N_PROGRAMA
                                             },
                                             // Fichas Universitarias
                                             IdCarreraUniversitaria = fiUniv.ID_CARRERA,
                                             IdEscuelaUniversitaria = fiUniv.ID_ESCUELA,
                                             EscuelaFicha =
                                                 new Escuela
                                                 {
                                                     Id_Escuela = fiUniv.ID_ESCUELA ?? 0,
                                                     Nombre_Escuela = fiUniv.T_ESCUELAS.N_ESCUELA,
                                                     LocalidadEscuela =
                                                         new Localidad
                                                         {
                                                             IdLocalidad = fiUniv.T_ESCUELAS.ID_LOCALIDAD ?? 0,
                                                             NombreLocalidad = fiUniv.T_ESCUELAS.T_LOCALIDADES.N_LOCALIDAD,
                                                             Departamento =
                                                                 new Departamento
                                                                 {
                                                                     IdDepartamento = fiUniv.T_ESCUELAS.T_LOCALIDADES.ID_DEPARTAMENTO ?? 0,
                                                                     NombreDepartamento = fiUniv.T_ESCUELAS.T_LOCALIDADES.T_DEPARTAMENTOS.N_DEPARTAMENTO
                                                                 }
                                                         }
                                                 },
                                             CarreraFicha =
                                                 new Carrera
                                                 {
                                                     IdCarrera = fiUniv.ID_CARRERA ?? 0,
                                                     NombreCarrera = fiUniv.T_CARRERAS.N_CARRERA,
                                                     InstitucionCarrera =
                                                         new Institucion
                                                         {
                                                             IdInstitucion = fiUniv.T_CARRERAS.ID_INSTITUCION ?? 0,
                                                             NombreInstitucion = fiUniv.T_CARRERAS.T_SECTOR_INSTITUCION.T_INSTITUCIONES.N_INSTITUCION
                                                         }
                                                 },

                                             LocalidadFicha =
                                                 new Localidad
                                                 {
                                                     IdLocalidad = fic.ID_LOCALIDAD ?? 0,
                                                     NombreLocalidad = fic.T_LOCALIDADES.N_LOCALIDAD,
                                                     Departamento =
                                                         new Departamento
                                                         {
                                                             IdDepartamento = fic.T_LOCALIDADES.ID_DEPARTAMENTO ?? 0,
                                                             NombreDepartamento = fic.T_LOCALIDADES.T_DEPARTAMENTOS.N_DEPARTAMENTO
                                                         }
                                                 }
                                         }
                   );

                    return universitaria;

                case (int)Enums.TipoFicha.ReconversionProductiva:
                    var ficharec = (from fic in _mdb.T_FICHAS
                                    from c in
                                        _mdb.T_BENEFICIARIOS.Where(fiter => fiter.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                                    from fiterc in
                                        _mdb.T_FICHA_TERCIARIO.Where(fiter => fiter.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                                    from fiUniv in
                                        _mdb.T_FICHA_UNIVERSITARIO.Where(fiter => fiter.ID_FICHA == fic.ID_FICHA).
                                        DefaultIfEmpty()
                                    from cu in
                                        _mdb.T_CUENTAS_BANCO.Where(cu => cu.ID_BENEFICIARIO == c.ID_BENEFICIARIO).
                                        DefaultIfEmpty()
                                    from suc in
                                        _mdb.T_TABLAS_BCO_CBA.Where(suc => suc.ID_TABLA_BCO_CBA == cu.ID_SUCURSAL).
                                        DefaultIfEmpty()
                                    from mon in
                                        _mdb.T_TABLAS_BCO_CBA.Where(mon => mon.ID_TABLA_BCO_CBA == cu.ID_MONEDA).DefaultIfEmpty
                                        ()
                                    from iva in
                                        _mdb.T_TABLAS_BCO_CBA.Where(iva => iva.ID_TABLA_BCO_CBA == c.CONDICION_IVA).
                                        DefaultIfEmpty()
                                    from nac in
                                        _mdb.T_TABLAS_BCO_CBA.Where(nac => nac.ID_TABLA_BCO_CBA == c.NACIONALIDAD).
                                        DefaultIfEmpty()
                                    from ficharecprod in
                                        _mdb.T_FICHA_REC_PROD.Where(
                                            ficharecprod => ficharecprod.ID_FICHA == fic.ID_FICHA && fic.TIPO_FICHA == tipoficha).
                                        DefaultIfEmpty()
                                    from ap in
                                        _mdb.T_APODERADOS.Where(
                                            ap =>
                                            ap.ID_BENEFICIARIO == c.ID_BENEFICIARIO &&
                                            ap.ID_ESTADO_APODERADO == (short)Enums.EstadoApoderado.Activo).DefaultIfEmpty()
                                    from sucapo in
                                        _mdb.T_TABLAS_BCO_CBA.Where(
                                            sucapo =>
                                            sucapo.ID_TABLA_BCO_CBA == ap.ID_SUCURSAL_BCO &&
                                            sucapo.ID_TIPO_TABLA_BCO_CBA == (int)Enums.TiposTablaBcoCba.Sucursales).
                                        DefaultIfEmpty()
                                    where fic.TIPO_FICHA == tipoficha
                                    select new Ficha
                                    {
                                         IdFicha = fic.ID_FICHA,
                                         Nombre = fic.NOMBRE,
                                         Apellido = fic.APELLIDO,
                                         NumeroDocumento = fic.NUMERO_DOCUMENTO,
                                         IdEstadoFicha = fic.ID_ESTADO_FICHA ?? (int)Enums.EstadoFicha.SinEstado,
                                         TipoFicha = fic.TIPO_FICHA,
                                         NombreEstadoFicha = fic.T_ESTADOS_FICHA.N_ESTADO_FICHA ?? "",
                                         NombreTipoFicha = fic.T_TIPO_FICHA.N_TIPO_FICHA,
                                         Localidad = fic.T_LOCALIDADES.N_LOCALIDAD,
                                         IdLocalidad = fic.ID_LOCALIDAD ?? 0,
                                         IdDepartamento = fic.T_LOCALIDADES.ID_DEPARTAMENTO,
                                         FechaSistema = c.FEC_SIST,
                                         IdUsuarioSistema = c.ID_USR_SIST,
                                         UsuarioSistema = c.T_USUARIOS.LOGIN,
                                         //Beneficiarios
                                         BeneficiarioFicha = new Beneficiario
                                         {
                                             IdEstadoExport = c.ID_ESTADO,
                                             IdBeneficiarioExport = c.ID_BENEFICIARIO,
                                             IdEstado = c.ID_ESTADO ?? 0,
                                             NombreEstado = c.T_ESTADOS.N_ESTADO,
                                             FechaSistema = c.FEC_SIST,
                                             IdFicha = c.ID_FICHA ?? 0,
                                             IdPrograma = c.ID_PROGRAMA ?? 0,
                                             IdUsuarioSistema = c.ID_USR_SIST,
                                             Programa = c.T_PROGRAMAS.N_PROGRAMA
                                         },
                                        FichaReconversion =
                                            new FichaReconversion
                                            {
                                                IdEmpresa = ficharecprod.ID_EMPRESA,
                                                Empresa =
                                                    new Empresa
                                                    {
                                                        IdEmpresa = ficharecprod.ID_EMPRESA ?? 0,
                                                        NombreEmpresa = ficharecprod.T_EMPRESAS.N_EMPRESA,
                                                        
                                                    }
                                            },
                                        LocalidadFicha =
                                            new Localidad
                                            {
                                                IdLocalidad = fic.ID_LOCALIDAD ?? 0,
                                                NombreLocalidad = fic.T_LOCALIDADES.N_LOCALIDAD,
                                                Departamento =
                                                    new Departamento
                                                    {
                                                        IdDepartamento = fic.T_LOCALIDADES.ID_DEPARTAMENTO ?? 0,
                                                        NombreDepartamento = fic.T_LOCALIDADES.T_DEPARTAMENTOS.N_DEPARTAMENTO
                                                    }
                                            }
                                    }
                       );
                    return ficharec;

                case (int)Enums.TipoFicha.EfectoresSociales:
                    var fichaefec = (from fic in _mdb.T_FICHAS
                                     from c in
                                         _mdb.T_BENEFICIARIOS.Where(fiter => fiter.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                                     from fiterc in
                                         _mdb.T_FICHA_TERCIARIO.Where(fiter => fiter.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                                     from fiUniv in
                                         _mdb.T_FICHA_UNIVERSITARIO.Where(fiter => fiter.ID_FICHA == fic.ID_FICHA).
                                         DefaultIfEmpty()
                                     from cu in
                                         _mdb.T_CUENTAS_BANCO.Where(cu => cu.ID_BENEFICIARIO == c.ID_BENEFICIARIO).
                                         DefaultIfEmpty()
                                     from suc in
                                         _mdb.T_TABLAS_BCO_CBA.Where(suc => suc.ID_TABLA_BCO_CBA == cu.ID_SUCURSAL).
                                         DefaultIfEmpty()
                                     from mon in
                                         _mdb.T_TABLAS_BCO_CBA.Where(mon => mon.ID_TABLA_BCO_CBA == cu.ID_MONEDA).DefaultIfEmpty
                                         ()
                                     from iva in
                                         _mdb.T_TABLAS_BCO_CBA.Where(iva => iva.ID_TABLA_BCO_CBA == c.CONDICION_IVA).
                                         DefaultIfEmpty()
                                     from nac in
                                         _mdb.T_TABLAS_BCO_CBA.Where(nac => nac.ID_TABLA_BCO_CBA == c.NACIONALIDAD).
                                         DefaultIfEmpty()
                                     from fichaefecsoc in
                                         _mdb.T_FICHA_EFEC_SOC.Where(
                                             fichaefecsoc => fichaefecsoc.ID_FICHA == fic.ID_FICHA && fic.TIPO_FICHA == tipoficha).
                                         DefaultIfEmpty()
                                     from ap in
                                         _mdb.T_APODERADOS.Where(
                                             ap =>
                                             ap.ID_BENEFICIARIO == c.ID_BENEFICIARIO &&
                                             ap.ID_ESTADO_APODERADO == (short)Enums.EstadoApoderado.Activo).DefaultIfEmpty()
                                     from sucapo in
                                         _mdb.T_TABLAS_BCO_CBA.Where(
                                             sucapo =>
                                             sucapo.ID_TABLA_BCO_CBA == ap.ID_SUCURSAL_BCO &&
                                             sucapo.ID_TIPO_TABLA_BCO_CBA == (int)Enums.TiposTablaBcoCba.Sucursales).
                                         DefaultIfEmpty()
                                     where fic.TIPO_FICHA == tipoficha
                                     select new Ficha
                                     {
                                        IdFicha = fic.ID_FICHA,
                                         Nombre = fic.NOMBRE,
                                         Apellido = fic.APELLIDO,
                                         NumeroDocumento = fic.NUMERO_DOCUMENTO,
                                         IdEstadoFicha = fic.ID_ESTADO_FICHA ?? (int)Enums.EstadoFicha.SinEstado,
                                         TipoFicha = fic.TIPO_FICHA,
                                         NombreEstadoFicha = fic.T_ESTADOS_FICHA.N_ESTADO_FICHA ?? "",
                                         NombreTipoFicha = fic.T_TIPO_FICHA.N_TIPO_FICHA,
                                         Localidad = fic.T_LOCALIDADES.N_LOCALIDAD,
                                         IdLocalidad = fic.ID_LOCALIDAD ?? 0,
                                         IdDepartamento = fic.T_LOCALIDADES.ID_DEPARTAMENTO,
                                         FechaSistema = c.FEC_SIST,
                                         IdUsuarioSistema = c.ID_USR_SIST,
                                         UsuarioSistema = c.T_USUARIOS.LOGIN,
                                         //Beneficiarios
                                         BeneficiarioFicha = new Beneficiario
                                         {
                                             IdEstadoExport = c.ID_ESTADO,
                                             IdBeneficiarioExport = c.ID_BENEFICIARIO,
                                             IdEstado = c.ID_ESTADO ?? 0,
                                             NombreEstado = c.T_ESTADOS.N_ESTADO,
                                             FechaSistema = c.FEC_SIST,
                                             IdFicha = c.ID_FICHA ?? 0,
                                             IdPrograma = c.ID_PROGRAMA ?? 0,
                                             IdUsuarioSistema = c.ID_USR_SIST,
                                             Programa = c.T_PROGRAMAS.N_PROGRAMA
                                         },
                                         FichaEfectores =
                                             new FichaEfectoresSociales
                                             {
                                                 IdEmpresa = fichaefecsoc.ID_EMPRESA,
                                                 Empresa =
                                                     new Empresa
                                                     {
                                                         Cuit = fichaefecsoc.T_EMPRESAS.CUIT,
                                                         IdEmpresa = fichaefecsoc.ID_EMPRESA ?? 0,
                                                         NombreEmpresa = fichaefecsoc.T_EMPRESAS.N_EMPRESA
                                                     }
                                             },
                                         LocalidadFicha =
                                             new Localidad
                                             {
                                                 IdLocalidad = fic.ID_LOCALIDAD ?? 0,
                                                 NombreLocalidad = fic.T_LOCALIDADES.N_LOCALIDAD,
                                                 Departamento =
                                                     new Departamento
                                                     {
                                                         IdDepartamento = fic.T_LOCALIDADES.ID_DEPARTAMENTO ?? 0,
                                                         NombreDepartamento = fic.T_LOCALIDADES.T_DEPARTAMENTOS.N_DEPARTAMENTO
                                                     }
                                             }
                                     }
                       );
                    return fichaefec;
                default:
                    return null;
            }
        }

        private IQueryable<IFicha> QFichaCompleto(int tipoficha)
        {
            switch (tipoficha)
            {
                case (int)Enums.TipoFicha.Ppp:
                    var a = (from fic in _mdb.T_FICHAS
                             from c in
                                 _mdb.T_BENEFICIARIOS.Where(fiter => fiter.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                             from fiterc in
                                 _mdb.T_FICHA_TERCIARIO.Where(fiter => fiter.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                             from fiUniv in
                                 _mdb.T_FICHA_UNIVERSITARIO.Where(fiter => fiter.ID_FICHA == fic.ID_FICHA).
                                 DefaultIfEmpty()
                             from cu in
                                 _mdb.T_CUENTAS_BANCO.Where(cu => cu.ID_BENEFICIARIO == c.ID_BENEFICIARIO).
                                 DefaultIfEmpty()
                             from suc in
                                 _mdb.T_TABLAS_BCO_CBA.Where(suc => suc.ID_TABLA_BCO_CBA == cu.ID_SUCURSAL).
                                 DefaultIfEmpty()
                             from mon in
                                 _mdb.T_TABLAS_BCO_CBA.Where(mon => mon.ID_TABLA_BCO_CBA == cu.ID_MONEDA).DefaultIfEmpty
                                 ()
                             from iva in
                                 _mdb.T_TABLAS_BCO_CBA.Where(iva => iva.ID_TABLA_BCO_CBA == c.CONDICION_IVA).
                                 DefaultIfEmpty()
                             from nac in
                                 _mdb.T_TABLAS_BCO_CBA.Where(nac => nac.ID_TABLA_BCO_CBA == c.NACIONALIDAD).
                                 DefaultIfEmpty()
                             from fichappp in
                                 _mdb.T_FICHA_PPP.Where(
                                     fichappp => fichappp.ID_FICHA == fic.ID_FICHA && fic.TIPO_FICHA == tipoficha).
                                 DefaultIfEmpty()
                             from ap in
                                 _mdb.T_APODERADOS.Where(
                                     ap =>
                                     ap.ID_BENEFICIARIO == c.ID_BENEFICIARIO &&
                                     ap.ID_ESTADO_APODERADO == (short)Enums.EstadoApoderado.Activo).DefaultIfEmpty()
                             from sucapo in
                                 _mdb.T_TABLAS_BCO_CBA.Where(
                                     sucapo =>
                                     sucapo.ID_TABLA_BCO_CBA == ap.ID_SUCURSAL_BCO &&
                                     sucapo.ID_TIPO_TABLA_BCO_CBA == (int)Enums.TiposTablaBcoCba.Sucursales).
                                 DefaultIfEmpty()
                             // 22/05/2013 - DI CAMPLI LEANDRO - SE AÑADE A FICHAS PPP LA CAJA A LA QUE PERTENECE
                             from cf in _mdb.T_FICHAS_CAJA.Where(cf => cf.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                             from refic in _mdb.T_FICHAS_RECHAZO.Where(refic => refic.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()


                             where fic.TIPO_FICHA == tipoficha

                             //let UltimoRechazo = (from recha in _mdb.T_FICHAS_RECHAZO.Where(recha => recha.ID_FICHA == fic.ID_FICHA) select recha).FirstOrDefault()

                             select new Ficha
                             {
                                 IdFicha = fic.ID_FICHA,
                                 Nombre = fic.NOMBRE,
                                 Apellido = fic.APELLIDO,
                                 NumeroDocumento = fic.NUMERO_DOCUMENTO,
                                 IdEstadoFicha = fic.ID_ESTADO_FICHA ?? (int)Enums.EstadoFicha.SinEstado,
                                 TipoFicha = fic.TIPO_FICHA,
                                 Cuil = fic.CUIL,
                                 NombreEstadoFicha = fic.T_ESTADOS_FICHA.N_ESTADO_FICHA ?? "",
                                 NombreTipoFicha = fic.T_TIPO_FICHA.N_TIPO_FICHA,
                                 Barrio = fic.BARRIO,
                                 Localidad = fic.T_LOCALIDADES.N_LOCALIDAD,
                                 IdLocalidad = fic.ID_LOCALIDAD ?? 0,
                                 Calle = fic.CALLE,
                                 CantidadHijos = fic.CANTIDAD_HIJOS,
                                 CodigoPostal = fic.CODIGO_POSTAL,
                                 CodigoSeguridad = fic.CODIGO_SEGURIDAD,
                                 Contacto = fic.CONTACTO,
                                 DeficienciaOtra = fic.DEFICIENCIA_OTRA,
                                 TieneDeficienciaMental =
                                     fic.TIENE_DEF_MENTAL == "S" ? true : false,
                                 TieneDeficienciaMotora =
                                     fic.TIENE_DEF_MOTORA == "S" ? true : false,
                                 TieneDeficienciaPsicologia =
                                     fic.TIENE_DEF_PSICOLOGICA == "S" ? true : false,
                                 TieneDeficienciaSensorial =
                                     fic.TIENE_DEF_SENSORIAL == "S" ? true : false,
                                 CertificadoDiscapacidad = fic.CERTIF_DISCAP == "S" ? true : false,
                                 Dpto = fic.DPTO ?? " ",
                                 EntreCalles = fic.ENTRECALLES ?? " ",
                                 EsDiscapacitado = fic.ES_DISCAPACITADO == "S" ? true : false,
                                 EstadoCivil = fic.ESTADO_CIVIL,
                                 FechaNacimiento = fic.FER_NAC,
                                 Mail = fic.MAIL,
                                 Manzana = fic.MANZANA ?? " ",
                                 Monoblock = fic.MONOBLOCK ?? " ",
                                 Numero = fic.NUMERO,
                                 Parcela = (c.T_FICHAS.PARCELA == "0" ? " " : fic.PARCELA) ?? " ",
                                 Piso = fic.PISO ?? " ",
                                 Sexo = fic.SEXO,
                                 TelefonoCelular = fic.TEL_CELULAR,
                                 TelefonoFijo = fic.TEL_FIJO,
                                 TieneHijos = fic.TIENE_HIJOS == "S" ? true : false,
                                 TipoDocumento = fic.TIPO_DOCUMENTO,
                                 FechaSistema = c.FEC_SIST,
                                 IdUsuarioSistema = c.ID_USR_SIST,
                                 UsuarioSistema = c.T_USUARIOS.LOGIN,
                                 // 22/05/2013 - DI CAMPLI LEANDRO
                                 OrigenCarga = fic.ORIGEN_CARGA,
                                 NroTramite = fic.NRO_TRAMITE,
                                 idCaja = cf.ID_CAJA == null ? 0 : cf.ID_CAJA,
                                 IdTipoRechazo = refic.ID_TIPO_RECHAZO == null ? 0 : refic.ID_TIPO_RECHAZO, //UltimoRechazo.ID_TIPO_RECHAZO == null ? 0 : UltimoRechazo.ID_TIPO_RECHAZO, //
                                 rechazo = refic.T_TIPOS_RECHAZO.N_TIPO_RECHAZO ?? "", //UltimoRechazo.T_TIPOS_RECHAZO.N_TIPO_RECHAZO ?? "",//
                                 //
                                 //Beneficiarios
                                            BeneficiarioFicha = new Beneficiario
                                                                    {
                                                                        IdBeneficiarioExport = c.ID_BENEFICIARIO,
                                                                        IdEstado = c.ID_ESTADO ?? 0,
                                                                        FechaSistema = c.FEC_SIST,
                                                                        IdFicha = c.ID_FICHA ?? 0,
                                                                        IdPrograma = c.ID_PROGRAMA ?? 0,
                                                                        IdUsuarioSistema = c.ID_USR_SIST,
                                                                        IdConvenio = c.T_PROGRAMAS.CONVENIO_BCO_CBA,
                                                                        NumeroConvenio = c.T_PROGRAMAS.CONVENIO_BCO_CBA,
                                                                        CodigoActivacionBancaria =
                                                                            c.COD_ACT_BANCARIO ?? 0,
                                                                        CodigoNaturalezaJuridica =
                                                                            c.COD_ACT_BANCARIO ?? 0,
                                                                        CondicionIva = iva.COD_BCO_CBA ?? "0",
                                                                        Nacionalidad = c.NACIONALIDAD ?? 0,
                                                                        Residente = c.RESIDENTE,
                                                                        TipoPersona = c.TIPO_PERSONA,
                                                                        CodigoMoneda = mon.COD_BCO_CBA ?? "0",
                                                                        CodigoSucursal = suc.COD_BCO_CBA,
                                                                        NombreEstado = c.T_ESTADOS.N_ESTADO,
                                                                        Cbu = cu.CBU ?? "",
                                                                        ImportePagoPlan = c.T_PROGRAMAS.MONTO_PROG ?? 0,
                                                                        NumeroCuenta = cu.NRO_CTA,
                                                                        Programa = c.T_PROGRAMAS.N_PROGRAMA,
                                                                        MontoPrograma = c.T_PROGRAMAS.MONTO_PROG ?? 0,
                                                                        TieneApoderado = c.TIENE_APODERADO,
                                                                        Sucursal =
                                                                            suc.COD_BCO_CBA + " - " + suc.DESCRIPCION,
                                                                        SucursalDescripcion = suc.DESCRIPCION,
                                                                        FechaSolicitudCuenta = cu.FEC_SOL_CTA,
                                                                        FechaNotificacion = c.FEC_NOTIF,
                                                                        Notificado = c.NOTIFICADO == "S" ? true : false,
                                                                        IdSucursal = cu.ID_SUCURSAL,
                                                                        Apoderado =
                                                                            new Apoderado
                                                                                {
                                                                                    IdApoderadoNullable = ap.ID_APODERADO,
                                                                                    Nombre = ap.NOMBRE,
                                                                                    Apellido = ap.APELLIDO,
                                                                                    Barrio = ap.BARRIO,
                                                                                    Calle = ap.CALLE,
                                                                                    Cbu = ap.CBU,
                                                                                    CodigoPostal = ap.CODIGO_POSTAL,
                                                                                    Cuil = ap.CUIL,
                                                                                    Dpto = ap.DPTO,
                                                                                    EntreCalles = ap.ENTRECALLES,
                                                                                    FechaNacimiento = ap.FER_NAC,
                                                                                    FechaSolicitudCuenta =
                                                                                        ap.FEC_SOL_CTA,
                                                                                    IdEstadoApoderado =
                                                                                        ap.ID_ESTADO_APODERADO ?? 0,
                                                                                    IdSistema = ap.ID_SISTEMA ?? 0,
                                                                                    IdSucursal = ap.ID_SUCURSAL_BCO,
                                                                                    Mail = ap.MAIL,
                                                                                    Numero = ap.NUMERO,
                                                                                    IdLocalidad = ap.ID_LOCALIDAD,
                                                                                    LocalidadApoderado =
                                                                                        new Localidad
                                                                                        {
                                                                                            IdLocalidad = ap.ID_LOCALIDAD ?? 0,
                                                                                            NombreLocalidad = ap.T_LOCALIDADES.N_LOCALIDAD,
                                                                                            Departamento =
                                                                                                new Departamento
                                                                                                {
                                                                                                    IdDepartamento =
                                                                                                        ap.T_LOCALIDADES.ID_DEPARTAMENTO ?? 0,
                                                                                                    NombreDepartamento =
                                                                                                        ap.T_LOCALIDADES.T_DEPARTAMENTOS.N_DEPARTAMENTO
                                                                                                }
                                                                                        },
                                                                                    Manzana = ap.MANZANA,
                                                                                    Monoblock = ap.MONOBLOCK,
                                                                                    NombreEstadoApoderado = ap.T_ESTADOS_APODERADO.N_ESTADO_APODERADO,
                                                                                    NumeroCuentaBco = ap.NRO_CUENTA_BCO,
                                                                                    NumeroDocumento = ap.NRO_DOCUMENTO,
                                                                                    Parcela = ap.PARCELA,
                                                                                    Piso = ap.PISO,
                                                                                    Sexo = ap.SEXO,
                                                                                    TelefonoCelular = ap.TEL_CELULAR,
                                                                                    TipoDocumento = ap.TIPO_DOCUMENTO ?? 0,
                                                                                    TelefonoFijo = ap.TEL_FIJO,
                                                                                    UsuarioBanco = ap.USUARIO_BANCO,
                                                                                    SucursalApoderado =
                                                                                        new Sucursal
                                                                                        {
                                                                                            CodigoBanco = sucapo.COD_BCO_CBA,
                                                                                            IdSucursal = ap.ID_SUCURSAL_BCO ?? 0,
                                                                                            Detalle = sucapo.DESCRIPCION
                                                                                        }
                                                                                }
                                                                    },
                                            FichaPpp =
                                                new FichaPPP
                                                {
                                                    AltaTemprana = fichappp.ALTA_TEMPRANA,
                                                    Finalizado = (fichappp.FINALIZADO == "S" ? true : false),
                                                    Cursando = (fichappp.CURSANDO == "S" ? true : false),
                                                    IdEmpresa = fichappp.ID_EMPRESA,
                                                    DeseaTermNivel = (fichappp.DESEA_TERM_NIVEL == "S" ? true : false),
                                                    IdNivelEscolaridad = fichappp.ID_NIVEL_ESCOLARIDAD,
                                                    Modalidad = fichappp.MODALIDAD,
                                                    IdSede = fichappp.ID_SEDE,
                                                    Tareas = fichappp.TAREAS,
                                                    FechaInicioActividad = fichappp.AT_FECHA_INICIO,
                                                    FechaFinActividad = fichappp.AT_FECHA_CESE,
                                                    IdModalidadAfip = fichappp.ID_MOD_CONT_AFIP,
                                                    ModalidadAfip = fichappp.T_MOD_CONT_AFIP.N_MOD_CONT_AFIP,
                                                    Empresa =
                                                        new Empresa
                                                        {
                                                            Calle = fichappp.T_EMPRESAS.CALLE,
                                                            CantidadEmpleados =
                                                                fichappp.T_EMPRESAS.CANTIDAD_EMPLEADOS,
                                                            CodigoActividad =
                                                                fichappp.T_EMPRESAS.CODIGO_ACTIVIDAD,
                                                            CodigoPostal = fichappp.T_EMPRESAS.CODIGO_POSTAL,
                                                            Cuit = fichappp.T_EMPRESAS.CUIT,
                                                            DomicilioLaboralIdem =
                                                                fichappp.T_EMPRESAS.DOMICLIO_LABORAL_IDEM,
                                                            IdEmpresa = fichappp.ID_EMPRESA ?? 0,
                                                            NombreEmpresa = fichappp.T_EMPRESAS.N_EMPRESA,
                                                            Numero = fichappp.T_EMPRESAS.NUMERO,
                                                            Piso = fichappp.T_EMPRESAS.PISO,
                                                            Telefono = fichappp.T_EMPRESAS.PISO,
                                                            LocalidadEmpresa =
                                                                new Localidad
                                                                {
                                                                    IdLocalidad =
                                                                        fichappp.T_EMPRESAS.ID_LOCALIDAD ?? 0,
                                                                    NombreLocalidad =
                                                                        fichappp.T_EMPRESAS.T_LOCALIDADES.N_LOCALIDAD,
                                                                    Departamento =
                                                                        new Departamento
                                                                        {
                                                                            IdDepartamento =
                                                                                fichappp.T_EMPRESAS.T_LOCALIDADES.ID_DEPARTAMENTO ?? 0,
                                                                            NombreDepartamento =
                                                                                fichappp.T_EMPRESAS.T_LOCALIDADES.T_DEPARTAMENTOS.
                                                                                N_DEPARTAMENTO
                                                                        }
                                                                },
                                                            Usuario =
                                                                new UsuarioEmpresa
                                                                {
                                                                    ApellidoUsuario =
                                                                        fichappp.T_EMPRESAS.
                                                                        T_USUARIOS_EMPRESA.APELLIDO,
                                                                    Cuil =
                                                                        fichappp.T_EMPRESAS.
                                                                        T_USUARIOS_EMPRESA.CUIL,
                                                                    IdUsuarioEmpresa =
                                                                        fichappp.T_EMPRESAS.ID_USUARIO,
                                                                    Mail =
                                                                        fichappp.T_EMPRESAS.
                                                                        T_USUARIOS_EMPRESA.MAIL,
                                                                    NombreUsuario =
                                                                        fichappp.T_EMPRESAS.
                                                                        T_USUARIOS_EMPRESA.NOMBRE
                                                                }
                                                        }
                                                },
                                            LocalidadFicha =
                                                new Localidad
                                                {
                                                    IdLocalidad = fic.ID_LOCALIDAD ?? 0,
                                                    NombreLocalidad = fic.T_LOCALIDADES.N_LOCALIDAD,
                                                    Departamento =
                                                        new Departamento
                                                        {
                                                            IdDepartamento = fic.T_LOCALIDADES.ID_DEPARTAMENTO ?? 0,
                                                            NombreDepartamento = fic.T_LOCALIDADES.T_DEPARTAMENTOS.N_DEPARTAMENTO
                                                        }
                                                }
                                        }
                       );
                    return a;
                case (int)Enums.TipoFicha.PppProf:
                    var pppp = (from fic in _mdb.T_FICHAS
                                from c in
                                    _mdb.T_BENEFICIARIOS.Where(fiter => fiter.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                                from fiterc in
                                    _mdb.T_FICHA_TERCIARIO.Where(fiter => fiter.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                                from fiUniv in
                                    _mdb.T_FICHA_UNIVERSITARIO.Where(fiter => fiter.ID_FICHA == fic.ID_FICHA).
                                    DefaultIfEmpty()
                                from cu in
                                    _mdb.T_CUENTAS_BANCO.Where(cu => cu.ID_BENEFICIARIO == c.ID_BENEFICIARIO).
                                    DefaultIfEmpty()
                                from suc in
                                    _mdb.T_TABLAS_BCO_CBA.Where(suc => suc.ID_TABLA_BCO_CBA == cu.ID_SUCURSAL).
                                    DefaultIfEmpty()
                                from mon in
                                    _mdb.T_TABLAS_BCO_CBA.Where(mon => mon.ID_TABLA_BCO_CBA == cu.ID_MONEDA).DefaultIfEmpty()
                                from iva in
                                    _mdb.T_TABLAS_BCO_CBA.Where(iva => iva.ID_TABLA_BCO_CBA == c.CONDICION_IVA).
                                    DefaultIfEmpty()
                                from nac in
                                    _mdb.T_TABLAS_BCO_CBA.Where(nac => nac.ID_TABLA_BCO_CBA == c.NACIONALIDAD).
                                    DefaultIfEmpty()
                                from fichaPppp in
                                    _mdb.T_FICHA_PPP_PROF.Where(
                                        fipppp => fipppp.ID_FICHA == fic.ID_FICHA && fic.TIPO_FICHA == tipoficha).
                                    DefaultIfEmpty()
                                from ap in
                                    _mdb.T_APODERADOS.Where(
                                        ap =>
                                        ap.ID_BENEFICIARIO == c.ID_BENEFICIARIO &&
                                        ap.ID_ESTADO_APODERADO == (short)Enums.EstadoApoderado.Activo).DefaultIfEmpty()
                                from sucapo in
                                    _mdb.T_TABLAS_BCO_CBA.Where(
                                        sucapo =>
                                        sucapo.ID_TABLA_BCO_CBA == ap.ID_SUCURSAL_BCO &&
                                        sucapo.ID_TIPO_TABLA_BCO_CBA == (int)Enums.TiposTablaBcoCba.Sucursales).
                                    DefaultIfEmpty()
                                where fic.TIPO_FICHA == tipoficha
                                select new Ficha
                                           {
                                               IdFicha = fic.ID_FICHA,
                                               Nombre = fic.NOMBRE,
                                               Apellido = fic.APELLIDO,
                                               NumeroDocumento = fic.NUMERO_DOCUMENTO,
                                               IdEstadoFicha = fic.ID_ESTADO_FICHA,
                                               TipoFicha = fic.TIPO_FICHA,
                                               Cuil = fic.CUIL,
                                               NombreEstadoFicha = fic.T_ESTADOS_FICHA.N_ESTADO_FICHA ?? "",
                                               NombreTipoFicha = fic.T_TIPO_FICHA.N_TIPO_FICHA,
                                               Barrio = fic.BARRIO,
                                               Localidad = fic.T_LOCALIDADES.N_LOCALIDAD,
                                               IdLocalidad = fic.ID_LOCALIDAD ?? 0,
                                               Calle = fic.CALLE,
                                               CantidadHijos = fic.CANTIDAD_HIJOS,
                                               CodigoPostal = fic.CODIGO_POSTAL,
                                               CodigoSeguridad = fic.CODIGO_SEGURIDAD,
                                               Contacto = fic.CONTACTO,
                                               DeficienciaOtra = fic.DEFICIENCIA_OTRA,
                                               TieneDeficienciaMental =
                                                   fic.TIENE_DEF_MENTAL == "S" ? true : false,
                                               TieneDeficienciaMotora =
                                                   fic.TIENE_DEF_MOTORA == "S" ? true : false,
                                               TieneDeficienciaPsicologia =
                                                   fic.TIENE_DEF_PSICOLOGICA == "S" ? true : false,
                                               TieneDeficienciaSensorial =
                                                   fic.TIENE_DEF_SENSORIAL == "S" ? true : false,
                                               CertificadoDiscapacidad = fic.CERTIF_DISCAP == "S" ? true : false,
                                               Dpto = fic.DPTO ?? " ",
                                               EntreCalles = fic.ENTRECALLES ?? " ",
                                               EsDiscapacitado = fic.ES_DISCAPACITADO == "S" ? true : false,
                                               EstadoCivil = fic.ESTADO_CIVIL,
                                               FechaNacimiento = fic.FER_NAC,
                                               Mail = fic.MAIL,
                                               Manzana = fic.MANZANA ?? " ",
                                               Monoblock = fic.MONOBLOCK ?? " ",
                                               Numero = fic.NUMERO,
                                               Parcela = (c.T_FICHAS.PARCELA == "0" ? " " : fic.PARCELA) ?? " ",
                                               Piso = fic.PISO ?? " ",
                                               Sexo = fic.SEXO,
                                               TelefonoCelular = fic.TEL_CELULAR,
                                               TelefonoFijo = fic.TEL_FIJO,
                                               TieneHijos = fic.TIENE_HIJOS == "S" ? true : false,
                                               TipoDocumento = fic.TIPO_DOCUMENTO,
                                               FechaSistema = c.FEC_SIST,
                                               IdUsuarioSistema = c.ID_USR_SIST,
                                               UsuarioSistema = c.T_USUARIOS.LOGIN,
                                               //Beneficiarios
                                               BeneficiarioFicha = new Beneficiario
                                                                       {
                                                                           IdBeneficiarioExport = c.ID_BENEFICIARIO,
                                                                           IdEstado = c.ID_ESTADO ?? 0,
                                                                           FechaSistema = c.FEC_SIST,
                                                                           IdFicha = c.ID_FICHA ?? 0,
                                                                           IdPrograma = c.ID_PROGRAMA ?? 0,
                                                                           IdUsuarioSistema = c.ID_USR_SIST,
                                                                           IdConvenio = c.T_PROGRAMAS.CONVENIO_BCO_CBA,
                                                                           NumeroConvenio = c.T_PROGRAMAS.CONVENIO_BCO_CBA,
                                                                           CodigoActivacionBancaria =
                                                                               c.COD_ACT_BANCARIO ?? 0,
                                                                           CodigoNaturalezaJuridica =
                                                                               c.COD_ACT_BANCARIO ?? 0,
                                                                           CondicionIva = iva.COD_BCO_CBA ?? "0",
                                                                           Nacionalidad = c.NACIONALIDAD ?? 0,
                                                                           Residente = c.RESIDENTE,
                                                                           TipoPersona = c.TIPO_PERSONA,
                                                                           CodigoMoneda = mon.COD_BCO_CBA ?? "0",
                                                                           CodigoSucursal = suc.COD_BCO_CBA,
                                                                           NombreEstado = c.T_ESTADOS.N_ESTADO,
                                                                           Cbu = cu.CBU ?? "",
                                                                           ImportePagoPlan = c.T_PROGRAMAS.MONTO_PROG ?? 0,
                                                                           NumeroCuenta = cu.NRO_CTA,
                                                                           Programa = c.T_PROGRAMAS.N_PROGRAMA,
                                                                           MontoPrograma = c.T_PROGRAMAS.MONTO_PROG ?? 0,
                                                                           TieneApoderado = c.TIENE_APODERADO,
                                                                           Sucursal = suc.COD_BCO_CBA + " - " + suc.DESCRIPCION,
                                                                           FechaSolicitudCuenta = cu.FEC_SOL_CTA,
                                                                           IdSucursal = cu.ID_SUCURSAL,
                                                                           FechaNotificacion = c.FEC_NOTIF,
                                                                           Notificado = c.NOTIFICADO == "S" ? true : false,
                                                                           Apoderado =
                                                                               new Apoderado
                                                                                   {
                                                                                       IdApoderadoNullable = ap.ID_APODERADO,
                                                                                       Nombre = ap.NOMBRE,
                                                                                       Apellido = ap.APELLIDO,
                                                                                       Barrio = ap.BARRIO,
                                                                                       Calle = ap.CALLE,
                                                                                       Cbu = ap.CBU,
                                                                                       CodigoPostal = ap.CODIGO_POSTAL,
                                                                                       Cuil = ap.CUIL,
                                                                                       Dpto = ap.DPTO,
                                                                                       EntreCalles = ap.ENTRECALLES,
                                                                                       FechaNacimiento = ap.FER_NAC,
                                                                                       FechaSolicitudCuenta = ap.FEC_SOL_CTA,
                                                                                       IdEstadoApoderado = ap.ID_ESTADO_APODERADO,
                                                                                       IdLocalidad = ap.ID_LOCALIDAD,
                                                                                       IdSistema = ap.ID_SISTEMA ?? 0,
                                                                                       IdSucursal = ap.ID_SUCURSAL_BCO,
                                                                                       Mail = ap.MAIL,
                                                                                       Numero = ap.NUMERO,
                                                                                       LocalidadApoderado =
                                                                                           new Localidad
                                                                                               {
                                                                                                   IdLocalidad = ap.ID_LOCALIDAD ?? 0,
                                                                                                   NombreLocalidad = ap.T_LOCALIDADES.N_LOCALIDAD,
                                                                                                   Departamento =
                                                                                                       new Departamento
                                                                                                           {
                                                                                                               IdDepartamento = ap.T_LOCALIDADES.ID_DEPARTAMENTO ?? 0,
                                                                                                               NombreDepartamento = ap.T_LOCALIDADES.T_DEPARTAMENTOS.N_DEPARTAMENTO
                                                                                                           }
                                                                                               },
                                                                                       Manzana = ap.MANZANA,
                                                                                       Monoblock = ap.MONOBLOCK,
                                                                                       NombreEstadoApoderado = ap.T_ESTADOS_APODERADO.N_ESTADO_APODERADO,
                                                                                       NumeroCuentaBco = ap.NRO_CUENTA_BCO,
                                                                                       NumeroDocumento = ap.NRO_DOCUMENTO,
                                                                                       Parcela = ap.PARCELA,
                                                                                       Piso = ap.PISO,
                                                                                       Sexo = ap.SEXO,
                                                                                       TelefonoCelular = ap.TEL_CELULAR,
                                                                                       TipoDocumento = ap.TIPO_DOCUMENTO ?? 0,
                                                                                       TelefonoFijo = ap.TEL_FIJO,
                                                                                       UsuarioBanco = ap.USUARIO_BANCO,
                                                                                       SucursalApoderado =
                                                                                           new Sucursal
                                                                                               {
                                                                                                   CodigoBanco =
                                                                                                       sucapo.COD_BCO_CBA,
                                                                                                   IdSucursal = ap.ID_SUCURSAL_BCO ?? 0,
                                                                                                   Detalle = sucapo.DESCRIPCION
                                                                                               }
                                                                                   }
                                                                       },
                                               FichaPppp =
                                                   new FichaPPPP
                                                       {
                                                           AltaTemprana = fichaPppp.ALTA_TEMPRANA,
                                                           Promedio = fichaPppp.PROMEDIO,
                                                           IdTitulo = fichaPppp.ID_TITULO,
                                                           FechaEgreso = fichaPppp.FECHA_EGRESO,
                                                           IdEmpresa = fichaPppp.ID_EMPRESA,
                                                           Modalidad = fichaPppp.MODALIDAD,
                                                           IdSede = fichaPppp.ID_SEDE,
                                                           Tareas = fichaPppp.TAREAS,
                                                           FechaInicioActividad = fichaPppp.AT_FECHA_INICIO,
                                                           FechaFinActividad = fichaPppp.AT_FECHA_CESE,
                                                           IdModalidadAFIP = fichaPppp.ID_MOD_CONT_AFIP,
                                                           ModalidadAFIP = fichaPppp.T_MOD_CONT_AFIP.N_MOD_CONT_AFIP,
                                                           Empresa =
                                                               new Empresa
                                                                   {
                                                                       Calle = fichaPppp.T_EMPRESAS.CALLE,
                                                                       CantidadEmpleados =
                                                                           fichaPppp.T_EMPRESAS.CANTIDAD_EMPLEADOS,
                                                                       CodigoActividad =
                                                                           fichaPppp.T_EMPRESAS.CODIGO_ACTIVIDAD,
                                                                       CodigoPostal = fichaPppp.T_EMPRESAS.CODIGO_POSTAL,
                                                                       Cuit = fichaPppp.T_EMPRESAS.CUIT,
                                                                       DomicilioLaboralIdem =
                                                                           fichaPppp.T_EMPRESAS.DOMICLIO_LABORAL_IDEM,
                                                                       IdEmpresa = fichaPppp.ID_EMPRESA ?? 0,
                                                                       NombreEmpresa = fichaPppp.T_EMPRESAS.N_EMPRESA,
                                                                       Numero = fichaPppp.T_EMPRESAS.NUMERO,
                                                                       Piso = fichaPppp.T_EMPRESAS.PISO,
                                                                       Telefono = fichaPppp.T_EMPRESAS.PISO,
                                                                       IdLocalidad = fichaPppp.T_EMPRESAS.ID_LOCALIDAD,
                                                                       LocalidadEmpresa =
                                                                           new Localidad
                                                                               {
                                                                                   IdLocalidad = fichaPppp.T_EMPRESAS.ID_LOCALIDAD ?? 0,
                                                                                   NombreLocalidad = fichaPppp.T_EMPRESAS.T_LOCALIDADES.N_LOCALIDAD,
                                                                                   Departamento =
                                                                                       new Departamento
                                                                                           {
                                                                                               IdDepartamento =
                                                                                                   fichaPppp.T_EMPRESAS.
                                                                                                       T_LOCALIDADES.
                                                                                                       ID_DEPARTAMENTO ?? 0,
                                                                                               NombreDepartamento =
                                                                                                   fichaPppp.T_EMPRESAS.
                                                                                                   T_LOCALIDADES.
                                                                                                   T_DEPARTAMENTOS.
                                                                                                   N_DEPARTAMENTO
                                                                                           }
                                                                               },
                                                                       Usuario =
                                                                           new UsuarioEmpresa
                                                                               {
                                                                                   ApellidoUsuario =
                                                                                       fichaPppp.T_EMPRESAS.
                                                                                       T_USUARIOS_EMPRESA.APELLIDO,
                                                                                   Cuil =
                                                                                       fichaPppp.T_EMPRESAS.
                                                                                       T_USUARIOS_EMPRESA.CUIL,
                                                                                   IdUsuarioEmpresa =
                                                                                       fichaPppp.T_EMPRESAS.ID_USUARIO,
                                                                                   Mail =
                                                                                       fichaPppp.T_EMPRESAS.
                                                                                       T_USUARIOS_EMPRESA.MAIL,
                                                                                   NombreUsuario =
                                                                                       fichaPppp.T_EMPRESAS.
                                                                                       T_USUARIOS_EMPRESA.NOMBRE
                                                                               }
                                                                   }
                                                       },
                                               LocalidadFicha =
                                                   new Localidad
                                                       {
                                                           IdLocalidad = fic.ID_LOCALIDAD ?? 0,
                                                           NombreLocalidad = fic.T_LOCALIDADES.N_LOCALIDAD,
                                                           Departamento =
                                                               new Departamento
                                                                   {
                                                                       IdDepartamento =
                                                                           fic.T_LOCALIDADES.ID_DEPARTAMENTO ?? 0,
                                                                       NombreDepartamento =
                                                                           fic.T_LOCALIDADES.T_DEPARTAMENTOS.N_DEPARTAMENTO
                                                                   }
                                                       }
                                           }
                            );

                    return pppp;
                case (int)Enums.TipoFicha.Vat:
                    var vat = (from fic in _mdb.T_FICHAS
                               from c in
                                   _mdb.T_BENEFICIARIOS.Where(fiter => fiter.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                               from cu in
                                   _mdb.T_CUENTAS_BANCO.Where(cu => cu.ID_BENEFICIARIO == c.ID_BENEFICIARIO).
                                   DefaultIfEmpty()
                               from suc in
                                   _mdb.T_TABLAS_BCO_CBA.Where(suc => suc.ID_TABLA_BCO_CBA == cu.ID_SUCURSAL).
                                   DefaultIfEmpty()
                               from mon in
                                   _mdb.T_TABLAS_BCO_CBA.Where(mon => mon.ID_TABLA_BCO_CBA == cu.ID_MONEDA).
                                   DefaultIfEmpty()
                               from iva in
                                   _mdb.T_TABLAS_BCO_CBA.Where(iva => iva.ID_TABLA_BCO_CBA == c.CONDICION_IVA).
                                   DefaultIfEmpty()
                               from nac in
                                   _mdb.T_TABLAS_BCO_CBA.Where(nac => nac.ID_TABLA_BCO_CBA == c.NACIONALIDAD).
                                   DefaultIfEmpty()
                               from fichaVat in
                                   _mdb.T_FICHA_VAT.Where(
                                       fivat => fivat.ID_FICHA == fic.ID_FICHA && fic.TIPO_FICHA == tipoficha).
                                   DefaultIfEmpty()
                               from ap in
                                   _mdb.T_APODERADOS.Where(
                                       ap =>
                                       ap.ID_BENEFICIARIO == c.ID_BENEFICIARIO &&
                                       ap.ID_ESTADO_APODERADO == (short)Enums.EstadoApoderado.Activo).DefaultIfEmpty()
                               from sucapo in
                                   _mdb.T_TABLAS_BCO_CBA.Where(
                                       sucapo =>
                                       sucapo.ID_TABLA_BCO_CBA == ap.ID_SUCURSAL_BCO &&
                                       sucapo.ID_TIPO_TABLA_BCO_CBA == (int)Enums.TiposTablaBcoCba.Sucursales).
                                   DefaultIfEmpty()
                               where fic.TIPO_FICHA == tipoficha
                               select new Ficha
                                          {
                                              IdFicha = fic.ID_FICHA,
                                              Nombre = fic.NOMBRE,
                                              Apellido = fic.APELLIDO,
                                              NumeroDocumento = fic.NUMERO_DOCUMENTO,
                                              IdEstadoFicha = fic.ID_ESTADO_FICHA ?? (int)Enums.EstadoFicha.SinEstado,
                                              TipoFicha = fic.TIPO_FICHA,
                                              Cuil = fic.CUIL,
                                              NombreEstadoFicha = fic.T_ESTADOS_FICHA.N_ESTADO_FICHA ?? "",
                                              NombreTipoFicha = fic.T_TIPO_FICHA.N_TIPO_FICHA,
                                              Barrio = fic.BARRIO,
                                              Localidad = fic.T_LOCALIDADES.N_LOCALIDAD,
                                              IdLocalidad = fic.ID_LOCALIDAD,
                                              Calle = fic.CALLE,
                                              CantidadHijos = fic.CANTIDAD_HIJOS,
                                              CodigoPostal = fic.CODIGO_POSTAL,
                                              CodigoSeguridad = fic.CODIGO_SEGURIDAD,
                                              Contacto = fic.CONTACTO,
                                              DeficienciaOtra = fic.DEFICIENCIA_OTRA,
                                              TieneDeficienciaMental =
                                                  fic.TIENE_DEF_MENTAL == "S" ? true : false,
                                              TieneDeficienciaMotora =
                                                  fic.TIENE_DEF_MOTORA == "S" ? true : false,
                                              TieneDeficienciaPsicologia =
                                                  fic.TIENE_DEF_PSICOLOGICA == "S" ? true : false,
                                              TieneDeficienciaSensorial =
                                                  fic.TIENE_DEF_SENSORIAL == "S" ? true : false,
                                              CertificadoDiscapacidad = fic.CERTIF_DISCAP == "S" ? true : false,
                                              Dpto = fic.DPTO ?? " ",
                                              EntreCalles = fic.ENTRECALLES ?? " ",
                                              EsDiscapacitado = fic.ES_DISCAPACITADO == "S" ? true : false,
                                              EstadoCivil = fic.ESTADO_CIVIL,
                                              FechaNacimiento = fic.FER_NAC,
                                              Mail = fic.MAIL,
                                              Manzana = fic.MANZANA ?? " ",
                                              Monoblock = fic.MONOBLOCK ?? " ",
                                              Numero = fic.NUMERO,
                                              Parcela = (c.T_FICHAS.PARCELA == "0" ? " " : fic.PARCELA) ?? " ",
                                              Piso = fic.PISO ?? " ",
                                              Sexo = fic.SEXO,
                                              TelefonoCelular = fic.TEL_CELULAR,
                                              TelefonoFijo = fic.TEL_FIJO,
                                              TieneHijos = fic.TIENE_HIJOS == "S" ? true : false,
                                              TipoDocumento = fic.TIPO_DOCUMENTO,
                                              FechaSistema = c.FEC_SIST,
                                              IdUsuarioSistema = c.ID_USR_SIST,
                                              UsuarioSistema = c.T_USUARIOS.LOGIN,
                                              //Beneficiarios
                                              BeneficiarioFicha = new Beneficiario
                                                                      {
                                                                          IdBeneficiarioExport = c.ID_BENEFICIARIO,
                                                                          IdEstado = c.ID_ESTADO ?? 0,
                                                                          FechaSistema = c.FEC_SIST,
                                                                          IdFicha = c.ID_FICHA ?? 0,
                                                                          IdPrograma = c.ID_PROGRAMA ?? 0,
                                                                          IdUsuarioSistema = c.ID_USR_SIST,
                                                                          IdConvenio = c.T_PROGRAMAS.CONVENIO_BCO_CBA,
                                                                          NumeroConvenio = c.T_PROGRAMAS.CONVENIO_BCO_CBA,
                                                                          CodigoActivacionBancaria = c.COD_ACT_BANCARIO ?? 0,
                                                                          CodigoNaturalezaJuridica = c.COD_ACT_BANCARIO ?? 0,
                                                                          CondicionIva = iva.COD_BCO_CBA ?? "0",
                                                                          Nacionalidad = c.NACIONALIDAD ?? 0,
                                                                          Residente = c.RESIDENTE,
                                                                          TipoPersona = c.TIPO_PERSONA,
                                                                          CodigoMoneda = mon.COD_BCO_CBA ?? "0",
                                                                          CodigoSucursal = suc.COD_BCO_CBA ?? " ",
                                                                          NombreEstado = c.T_ESTADOS.N_ESTADO,
                                                                          Cbu = cu.CBU ?? "",
                                                                          ImportePagoPlan = c.T_PROGRAMAS.MONTO_PROG ?? 0,
                                                                          NumeroCuenta = cu.NRO_CTA,
                                                                          Programa = c.T_PROGRAMAS.N_PROGRAMA,
                                                                          MontoPrograma = c.T_PROGRAMAS.MONTO_PROG ?? 0,
                                                                          TieneApoderado = c.TIENE_APODERADO,
                                                                          Sucursal = suc.COD_BCO_CBA + " - " + suc.DESCRIPCION,
                                                                          SucursalDescripcion = suc.DESCRIPCION,
                                                                          FechaSolicitudCuenta = cu.FEC_SOL_CTA,
                                                                          IdSucursal = cu.ID_SUCURSAL,
                                                                          FechaNotificacion = c.FEC_NOTIF,
                                                                          Notificado = c.NOTIFICADO == "S" ? true : false,
                                                                          Apoderado =
                                                                              new Apoderado
                                                                                  {
                                                                                      IdApoderadoNullable = ap.ID_APODERADO,
                                                                                      Nombre = ap.NOMBRE,
                                                                                      Apellido = ap.APELLIDO,
                                                                                      Barrio = ap.BARRIO,
                                                                                      Calle = ap.CALLE,
                                                                                      Cbu = ap.CBU,
                                                                                      CodigoPostal = ap.CODIGO_POSTAL,
                                                                                      Cuil = ap.CUIL,
                                                                                      Dpto = ap.DPTO,
                                                                                      EntreCalles = ap.ENTRECALLES,
                                                                                      FechaNacimiento = ap.FER_NAC,
                                                                                      FechaSolicitudCuenta = ap.FEC_SOL_CTA,
                                                                                      IdEstadoApoderado = ap.ID_ESTADO_APODERADO,
                                                                                      IdLocalidad = ap.ID_LOCALIDAD,
                                                                                      IdSistema = ap.ID_SISTEMA ?? 0,
                                                                                      IdSucursal = ap.ID_SUCURSAL_BCO,
                                                                                      Mail = ap.MAIL,
                                                                                      Numero = ap.NUMERO,
                                                                                      LocalidadApoderado =
                                                                                          new Localidad
                                                                                              {
                                                                                                  IdLocalidad =
                                                                                                      ap.ID_LOCALIDAD ?? 0,
                                                                                                  NombreLocalidad =
                                                                                                      ap.T_LOCALIDADES.N_LOCALIDAD,
                                                                                                  Departamento =
                                                                                                      new Departamento
                                                                                                          {
                                                                                                              IdDepartamento = ap.T_LOCALIDADES.ID_DEPARTAMENTO ?? 0,
                                                                                                              NombreDepartamento
                                                                                                                  = ap.T_LOCALIDADES.T_DEPARTAMENTOS.N_DEPARTAMENTO
                                                                                                          }
                                                                                              },
                                                                                      Manzana = ap.MANZANA,
                                                                                      Monoblock = ap.MONOBLOCK,
                                                                                      NombreEstadoApoderado = ap.T_ESTADOS_APODERADO.N_ESTADO_APODERADO,
                                                                                      NumeroCuentaBco = ap.NRO_CUENTA_BCO,
                                                                                      NumeroDocumento = ap.NRO_DOCUMENTO,
                                                                                      Parcela = ap.PARCELA,
                                                                                      Piso = ap.PISO,
                                                                                      Sexo = ap.SEXO,
                                                                                      TelefonoCelular = ap.TEL_CELULAR,
                                                                                      TipoDocumento = ap.TIPO_DOCUMENTO ?? 0,
                                                                                      TelefonoFijo = ap.TEL_FIJO,
                                                                                      UsuarioBanco = ap.USUARIO_BANCO,
                                                                                      SucursalApoderado =
                                                                                          new Sucursal
                                                                                              {
                                                                                                  CodigoBanco = sucapo.COD_BCO_CBA,
                                                                                                  IdSucursal = ap.ID_SUCURSAL_BCO ?? 0,
                                                                                                  Detalle = sucapo.DESCRIPCION
                                                                                              }
                                                                                  }
                                                                      },
                                              FichaVat =
                                                  new FichaVAT
                                                      {
                                                          AltaTemprana = fichaVat.ALTA_TEMPRANA,
                                                          AniosAportes = fichaVat.ANIOS_APORTES,
                                                          Finalizado = (fichaVat.FINALIZADO == "S" ? true : false),
                                                          Cursando = (fichaVat.CURSANDO == "S" ? true : false),
                                                          IdEmpresa = fichaVat.ID_EMPRESA,
                                                          IdNivelEscolaridad = fichaVat.ID_NIVEL_ESCOLARIDAD,
                                                          Modalidad = fichaVat.MODALIDAD,
                                                          IdSede = fichaVat.ID_SEDE,
                                                          Tareas = fichaVat.TAREAS,
                                                          FechaInicioActividad = fichaVat.AT_FECHA_INICIO,
                                                          FechaFinActividad = fichaVat.AT_FECHA_CESE,
                                                          IdModalidadAFIP = fichaVat.ID_MOD_CONT_AFIP,
                                                          ModalidadAFIP = fichaVat.T_MOD_CONT_AFIP.N_MOD_CONT_AFIP,
                                                          Empresa =
                                                              new Empresa
                                                                  {
                                                                      Calle = fichaVat.T_EMPRESAS.CALLE,
                                                                      CantidadEmpleados =
                                                                          fichaVat.T_EMPRESAS.CANTIDAD_EMPLEADOS,
                                                                      CodigoActividad =
                                                                          fichaVat.T_EMPRESAS.CODIGO_ACTIVIDAD,
                                                                      CodigoPostal = fichaVat.T_EMPRESAS.CODIGO_POSTAL,
                                                                      Cuit = fichaVat.T_EMPRESAS.CUIT,
                                                                      DomicilioLaboralIdem =
                                                                          fichaVat.T_EMPRESAS.DOMICLIO_LABORAL_IDEM,
                                                                      IdEmpresa = fichaVat.ID_EMPRESA ?? 0,
                                                                      IdLocalidad = fichaVat.T_EMPRESAS.ID_LOCALIDAD,
                                                                      NombreEmpresa = fichaVat.T_EMPRESAS.N_EMPRESA,
                                                                      Numero = fichaVat.T_EMPRESAS.NUMERO,
                                                                      Piso = fichaVat.T_EMPRESAS.PISO,
                                                                      Telefono = fichaVat.T_EMPRESAS.PISO,
                                                                      LocalidadEmpresa =
                                                                          new Localidad
                                                                              {
                                                                                  IdLocalidad =
                                                                                      fichaVat.T_EMPRESAS.ID_LOCALIDAD ??
                                                                                      0,
                                                                                  NombreLocalidad =
                                                                                      fichaVat.T_EMPRESAS.T_LOCALIDADES.
                                                                                      N_LOCALIDAD,
                                                                                  Departamento =
                                                                                      new Departamento
                                                                                          {
                                                                                              IdDepartamento =
                                                                                                  fichaVat.T_EMPRESAS.
                                                                                                      T_LOCALIDADES.
                                                                                                      ID_DEPARTAMENTO ??
                                                                                                  0,
                                                                                              NombreDepartamento =
                                                                                                  fichaVat.T_EMPRESAS.
                                                                                                  T_LOCALIDADES.
                                                                                                  T_DEPARTAMENTOS.
                                                                                                  N_DEPARTAMENTO
                                                                                          }
                                                                              },
                                                                      Usuario =
                                                                          new UsuarioEmpresa
                                                                              {
                                                                                  ApellidoUsuario =
                                                                                      fichaVat.T_EMPRESAS.
                                                                                      T_USUARIOS_EMPRESA.APELLIDO,
                                                                                  Cuil =
                                                                                      fichaVat.T_EMPRESAS.
                                                                                      T_USUARIOS_EMPRESA.CUIL,
                                                                                  IdUsuarioEmpresa = fichaVat.T_EMPRESAS.ID_USUARIO,
                                                                                  Mail =
                                                                                      fichaVat.T_EMPRESAS.
                                                                                      T_USUARIOS_EMPRESA.MAIL,
                                                                                  NombreUsuario =
                                                                                      fichaVat.T_EMPRESAS.
                                                                                      T_USUARIOS_EMPRESA.NOMBRE
                                                                              }
                                                                  }
                                                      },
                                              LocalidadFicha =
                                                  new Localidad
                                                      {
                                                          IdLocalidad = fic.ID_LOCALIDAD ?? 0,
                                                          NombreLocalidad = fic.T_LOCALIDADES.N_LOCALIDAD,
                                                          Departamento =
                                                              new Departamento
                                                                  {
                                                                      IdDepartamento = fic.T_LOCALIDADES.ID_DEPARTAMENTO ?? 0,
                                                                      NombreDepartamento = fic.T_LOCALIDADES.T_DEPARTAMENTOS.N_DEPARTAMENTO
                                                                  }
                                                      }
                                          }
                              );

                    return vat;
                case (int)Enums.TipoFicha.Terciaria:
                    var terciario = (from fic in _mdb.T_FICHAS
                                     from c in
                                         _mdb.T_BENEFICIARIOS.Where(fiter => fiter.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                                     from fiterc in
                                         _mdb.T_FICHA_TERCIARIO.Where(fiter => fiter.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                                     from cu in
                                         _mdb.T_CUENTAS_BANCO.Where(cu => cu.ID_BENEFICIARIO == c.ID_BENEFICIARIO).DefaultIfEmpty()
                                     from suc in
                                         _mdb.T_TABLAS_BCO_CBA.Where(suc => suc.ID_TABLA_BCO_CBA == cu.ID_SUCURSAL).DefaultIfEmpty()
                                     from mon in
                                         _mdb.T_TABLAS_BCO_CBA.Where(mon => mon.ID_TABLA_BCO_CBA == cu.ID_MONEDA).DefaultIfEmpty()
                                     from iva in
                                         _mdb.T_TABLAS_BCO_CBA.Where(iva => iva.ID_TABLA_BCO_CBA == c.CONDICION_IVA).DefaultIfEmpty()
                                     from nac in
                                         _mdb.T_TABLAS_BCO_CBA.Where(nac => nac.ID_TABLA_BCO_CBA == c.NACIONALIDAD).DefaultIfEmpty()
                                     from ap in
                                         _mdb.T_APODERADOS.Where(
                                             ap =>
                                             ap.ID_BENEFICIARIO == c.ID_BENEFICIARIO &&
                                             ap.ID_ESTADO_APODERADO == (short)Enums.EstadoApoderado.Activo).DefaultIfEmpty()
                                     from sucapo in
                                         _mdb.T_TABLAS_BCO_CBA.Where(
                                             sucapo =>
                                             sucapo.ID_TABLA_BCO_CBA == ap.ID_SUCURSAL_BCO &&
                                             sucapo.ID_TIPO_TABLA_BCO_CBA == (int)Enums.TiposTablaBcoCba.Sucursales).
                                         DefaultIfEmpty()
                                     where fic.TIPO_FICHA == tipoficha
                                     select new Ficha
                                                {
                                                    IdFicha = fic.ID_FICHA,
                                                    Nombre = fic.NOMBRE,
                                                    Apellido = fic.APELLIDO,
                                                    NumeroDocumento = fic.NUMERO_DOCUMENTO,
                                                    IdEstadoFicha = fic.ID_ESTADO_FICHA,
                                                    TipoFicha = fic.TIPO_FICHA,
                                                    Cuil = fic.CUIL,
                                                    NombreEstadoFicha = fic.T_ESTADOS_FICHA.N_ESTADO_FICHA,
                                                    NombreTipoFicha = fic.T_TIPO_FICHA.N_TIPO_FICHA,
                                                    Barrio = fic.BARRIO,
                                                    Localidad = fic.T_LOCALIDADES.N_LOCALIDAD,
                                                    IdLocalidad = fic.ID_LOCALIDAD,
                                                    Calle = fic.CALLE,
                                                    CantidadHijos = fic.CANTIDAD_HIJOS,
                                                    CodigoPostal = fic.CODIGO_POSTAL,
                                                    CodigoSeguridad = fic.CODIGO_SEGURIDAD,
                                                    Contacto = fic.CONTACTO,
                                                    DeficienciaOtra = fic.DEFICIENCIA_OTRA,
                                                    TieneDeficienciaMental =
                                                       fic.TIENE_DEF_MENTAL == "S" ? true : false,
                                                    TieneDeficienciaMotora =
                                                       fic.TIENE_DEF_MOTORA == "S" ? true : false,
                                                    TieneDeficienciaPsicologia =
                                                       fic.TIENE_DEF_PSICOLOGICA == "S" ? true : false,
                                                    TieneDeficienciaSensorial =
                                                       fic.TIENE_DEF_SENSORIAL == "S" ? true : false,
                                                    CertificadoDiscapacidad = fic.CERTIF_DISCAP == "S" ? true : false,
                                                    Dpto = fic.DPTO ?? " ",
                                                    EntreCalles = fic.ENTRECALLES ?? " ",
                                                    EsDiscapacitado = fic.ES_DISCAPACITADO == "S" ? true : false,
                                                    EstadoCivil = fic.ESTADO_CIVIL,
                                                    FechaNacimiento = fic.FER_NAC,
                                                    Mail = fic.MAIL,
                                                    Manzana = fic.MANZANA ?? " ",
                                                    Monoblock = fic.MONOBLOCK ?? " ",
                                                    Numero = fic.NUMERO,
                                                    Parcela = (c.T_FICHAS.PARCELA == "0" ? " " : fic.PARCELA) ?? " ",
                                                    Piso = fic.PISO ?? " ",
                                                    Sexo = fic.SEXO,
                                                    TelefonoCelular = fic.TEL_CELULAR,
                                                    TelefonoFijo = fic.TEL_FIJO,
                                                    TieneHijos = fic.TIENE_HIJOS == "S" ? true : false,
                                                    TipoDocumento = fic.TIPO_DOCUMENTO,
                                                    FechaSistema = c.FEC_SIST,
                                                    IdUsuarioSistema = c.ID_USR_SIST,
                                                    UsuarioSistema = c.T_USUARIOS.LOGIN,
                                                    //Beneficiarios
                                                    BeneficiarioFicha = new Beneficiario
                                                                            {
                                                                                IdBeneficiarioExport = c.ID_BENEFICIARIO,
                                                                                IdEstado = c.ID_ESTADO ?? 0,
                                                                                FechaSistema = c.FEC_SIST,
                                                                                IdFicha = c.ID_FICHA ?? 0,
                                                                                IdPrograma = c.ID_PROGRAMA ?? 0,
                                                                                IdUsuarioSistema = c.ID_USR_SIST,
                                                                                IdConvenio = c.T_PROGRAMAS.CONVENIO_BCO_CBA,
                                                                                NumeroConvenio = c.T_PROGRAMAS.CONVENIO_BCO_CBA,
                                                                                CodigoActivacionBancaria = c.COD_ACT_BANCARIO ?? 0,
                                                                                CodigoNaturalezaJuridica = c.COD_ACT_BANCARIO ?? 0,
                                                                                CondicionIva = iva.COD_BCO_CBA ?? "0",
                                                                                Nacionalidad = c.NACIONALIDAD ?? 0,
                                                                                Residente = c.RESIDENTE,
                                                                                TipoPersona = c.TIPO_PERSONA,
                                                                                CodigoMoneda = mon.COD_BCO_CBA ?? "0",
                                                                                CodigoSucursal = suc.COD_BCO_CBA ?? " ",
                                                                                NombreEstado = c.T_ESTADOS.N_ESTADO,
                                                                                Cbu = cu.CBU ?? "",
                                                                                ImportePagoPlan = c.T_PROGRAMAS.MONTO_PROG ?? 0,
                                                                                NumeroCuenta = cu.NRO_CTA,
                                                                                Programa = c.T_PROGRAMAS.N_PROGRAMA,
                                                                                MontoPrograma = c.T_PROGRAMAS.MONTO_PROG ?? 0,
                                                                                TieneApoderado = c.TIENE_APODERADO,
                                                                                Sucursal = suc.COD_BCO_CBA + " - " + suc.DESCRIPCION,
                                                                                FechaSolicitudCuenta = cu.FEC_SOL_CTA,
                                                                                IdSucursal = cu.ID_SUCURSAL,
                                                                                SucursalDescripcion = suc.DESCRIPCION,
                                                                                FechaNotificacion = c.FEC_NOTIF,
                                                                                Notificado = c.NOTIFICADO == "S" ? true : false,
                                                                                Apoderado =
                                                                                    new Apoderado
                                                                                    {
                                                                                        IdApoderadoNullable = ap.ID_APODERADO,
                                                                                        Nombre = ap.NOMBRE,
                                                                                        Apellido = ap.APELLIDO,
                                                                                        Barrio = ap.BARRIO,
                                                                                        Calle = ap.CALLE,
                                                                                        Cbu = ap.CBU,
                                                                                        CodigoPostal = ap.CODIGO_POSTAL,
                                                                                        Cuil = ap.CUIL,
                                                                                        Dpto = ap.DPTO,
                                                                                        EntreCalles = ap.ENTRECALLES,
                                                                                        FechaNacimiento = ap.FER_NAC,
                                                                                        FechaSolicitudCuenta = ap.FEC_SOL_CTA,
                                                                                        IdEstadoApoderado = ap.ID_ESTADO_APODERADO,
                                                                                        IdSistema = ap.ID_SISTEMA,
                                                                                        IdSucursal = ap.ID_SUCURSAL_BCO,
                                                                                        Mail = ap.MAIL,
                                                                                        Numero = ap.NUMERO,
                                                                                        IdLocalidad = ap.ID_LOCALIDAD,
                                                                                        LocalidadApoderado =
                                                                                            new Localidad
                                                                                            {
                                                                                                IdLocalidad = ap.ID_LOCALIDAD ?? 0,
                                                                                                NombreLocalidad = ap.T_LOCALIDADES.N_LOCALIDAD,
                                                                                                Departamento =
                                                                                                    new Departamento
                                                                                                    {
                                                                                                        IdDepartamento =
                                                                                                            ap.T_LOCALIDADES.ID_DEPARTAMENTO ?? 0,
                                                                                                        NombreDepartamento =
                                                                                                            ap.T_LOCALIDADES.T_DEPARTAMENTOS.N_DEPARTAMENTO
                                                                                                    }
                                                                                            },
                                                                                        Manzana = ap.MANZANA,
                                                                                        Monoblock = ap.MONOBLOCK,
                                                                                        NombreEstadoApoderado = ap.T_ESTADOS_APODERADO.N_ESTADO_APODERADO,
                                                                                        NumeroCuentaBco = ap.NRO_CUENTA_BCO,
                                                                                        NumeroDocumento = ap.NRO_DOCUMENTO,
                                                                                        Parcela = ap.PARCELA,
                                                                                        Piso = ap.PISO,
                                                                                        Sexo = ap.SEXO,
                                                                                        TelefonoCelular = ap.TEL_CELULAR,
                                                                                        TipoDocumento = ap.TIPO_DOCUMENTO ?? 0,
                                                                                        TelefonoFijo = ap.TEL_FIJO,
                                                                                        UsuarioBanco = ap.USUARIO_BANCO,
                                                                                        SucursalApoderado =
                                                                                            new Sucursal
                                                                                            {
                                                                                                CodigoBanco = sucapo.COD_BCO_CBA,
                                                                                                IdSucursal = ap.ID_SUCURSAL_BCO ?? 0,
                                                                                                Detalle = sucapo.DESCRIPCION
                                                                                            }
                                                                                    }
                                                                            },
                                                    //Fichas Terciarias
                                                    PromedioTerciaria = fiterc.PROMEDIO,
                                                    OtraCarreraTerciaria = fiterc.OTRA_CARRERA,
                                                    OtraInstitucionTerciaria = fiterc.OTRA_INSTITUCION,
                                                    IdCarreraTerciaria = fiterc.ID_CARRERA,
                                                    //NombreCarreraTerciaria = fiterc.T_CARRERAS.N_CARRERA,
                                                    IdEscuelaTerciaria = fiterc.ID_ESCUELA,
                                                    NombreEscuela = fiterc.T_ESCUELAS.N_ESCUELA,
                                                    OtroSectorTerciaria = fiterc.OTRO_SECTOR,
                                                    EscuelaFicha =
                                                        new Escuela
                                                        {
                                                            Anexo = fiterc.T_ESCUELAS.ANEXO,
                                                            Barrio = fiterc.T_ESCUELAS.BARRIO,
                                                            Cue = fiterc.T_ESCUELAS.CUE,
                                                            Id_Escuela = fiterc.ID_ESCUELA ?? 0,
                                                            Nombre_Escuela = fiterc.T_ESCUELAS.N_ESCUELA,
                                                            Id_Localidad = fiterc.T_ESCUELAS.ID_LOCALIDAD,
                                                            LocalidadEscuela =
                                                                new Localidad
                                                                {
                                                                    IdLocalidad = fiterc.T_ESCUELAS.ID_LOCALIDAD ?? 0,
                                                                    NombreLocalidad = fiterc.T_ESCUELAS.T_LOCALIDADES.N_LOCALIDAD,
                                                                    Departamento =
                                                                        new Departamento
                                                                        {
                                                                            IdDepartamento = fiterc.T_ESCUELAS.T_LOCALIDADES.ID_DEPARTAMENTO ?? 0,
                                                                            NombreDepartamento = fiterc.T_ESCUELAS.T_LOCALIDADES.T_DEPARTAMENTOS.N_DEPARTAMENTO
                                                                        }
                                                                }
                                                        },
                                                    CarreraFicha =
                                                        new Carrera
                                                        {
                                                            IdCarrera = fiterc.ID_CARRERA ?? 0,
                                                            NombreCarrera = fiterc.T_CARRERAS.N_CARRERA,
                                                            IdSector = fiterc.T_CARRERAS.ID_SECTOR,
                                                            SectorCarrera =
                                                                new Sector
                                                                {
                                                                    IdSector = fiterc.T_CARRERAS.ID_SECTOR ?? 0,
                                                                    NombreSector = fiterc.T_CARRERAS.T_SECTOR_INSTITUCION.T_SECTOR.N_SECTOR
                                                                },
                                                            NivelCarrera =
                                                                new Nivel
                                                                {
                                                                    IdNivel = fiterc.T_CARRERAS.ID_NIVEL ?? 0,
                                                                    NombreNivel = fiterc.T_CARRERAS.T_NIVEL.N_NIVEL
                                                                },
                                                            IdInstitucion = fiterc.T_CARRERAS.ID_INSTITUCION,
                                                            InstitucionCarrera =
                                                                new Institucion
                                                                {
                                                                    IdInstitucion = fiterc.T_CARRERAS.ID_INSTITUCION ?? 0,
                                                                    NombreInstitucion = fiterc.T_CARRERAS.T_SECTOR_INSTITUCION.T_INSTITUCIONES.N_INSTITUCION
                                                                }
                                                        },
                                                    LocalidadFicha =
                                                        new Localidad
                                                        {
                                                            IdLocalidad = fic.ID_LOCALIDAD ?? 0,
                                                            NombreLocalidad = fic.T_LOCALIDADES.N_LOCALIDAD,
                                                            Departamento =
                                                                new Departamento
                                                                {
                                                                    IdDepartamento = fic.T_LOCALIDADES.ID_DEPARTAMENTO ?? 0,
                                                                    NombreDepartamento = fic.T_LOCALIDADES.T_DEPARTAMENTOS.N_DEPARTAMENTO
                                                                }
                                                        }
                                                }
                    );
                    return terciario;
                case (int)Enums.TipoFicha.Universitaria:
                    var universitaria = (from fic in _mdb.T_FICHAS
                                         from c in
                                             _mdb.T_BENEFICIARIOS.Where(fiter => fiter.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                                         from fiUniv in
                                             _mdb.T_FICHA_UNIVERSITARIO.Where(fiter => fiter.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                                         from cu in
                                             _mdb.T_CUENTAS_BANCO.Where(cu => cu.ID_BENEFICIARIO == c.ID_BENEFICIARIO).DefaultIfEmpty()
                                         from suc in
                                             _mdb.T_TABLAS_BCO_CBA.Where(suc => suc.ID_TABLA_BCO_CBA == cu.ID_SUCURSAL).DefaultIfEmpty()
                                         from mon in
                                             _mdb.T_TABLAS_BCO_CBA.Where(mon => mon.ID_TABLA_BCO_CBA == cu.ID_MONEDA).DefaultIfEmpty()
                                         from iva in
                                             _mdb.T_TABLAS_BCO_CBA.Where(iva => iva.ID_TABLA_BCO_CBA == c.CONDICION_IVA).DefaultIfEmpty()
                                         from nac in
                                             _mdb.T_TABLAS_BCO_CBA.Where(nac => nac.ID_TABLA_BCO_CBA == c.NACIONALIDAD).DefaultIfEmpty()
                                         from ap in
                                             _mdb.T_APODERADOS.Where(
                                                 ap =>
                                                 ap.ID_BENEFICIARIO == c.ID_BENEFICIARIO &&
                                                 ap.ID_ESTADO_APODERADO == (short)Enums.EstadoApoderado.Activo).DefaultIfEmpty()
                                         from sucapo in
                                             _mdb.T_TABLAS_BCO_CBA.Where(
                                                 sucapo =>
                                                 sucapo.ID_TABLA_BCO_CBA == ap.ID_SUCURSAL_BCO &&
                                                 sucapo.ID_TIPO_TABLA_BCO_CBA == (int)Enums.TiposTablaBcoCba.Sucursales).
                                             DefaultIfEmpty()
                                         where fic.TIPO_FICHA == tipoficha
                                         select new Ficha
                                                    {
                                                        IdFicha = fic.ID_FICHA,
                                                        Nombre = fic.NOMBRE,
                                                        Apellido = fic.APELLIDO,
                                                        NumeroDocumento = fic.NUMERO_DOCUMENTO,
                                                        IdEstadoFicha = fic.ID_ESTADO_FICHA,
                                                        TipoFicha = fic.TIPO_FICHA,
                                                        Cuil = fic.CUIL,
                                                        NombreEstadoFicha = fic.T_ESTADOS_FICHA.N_ESTADO_FICHA ?? "",
                                                        NombreTipoFicha = fic.T_TIPO_FICHA.N_TIPO_FICHA,
                                                        Barrio = fic.BARRIO,
                                                        Localidad = fic.T_LOCALIDADES.N_LOCALIDAD,
                                                        IdLocalidad = fic.ID_LOCALIDAD,
                                                        Calle = fic.CALLE,
                                                        CantidadHijos = fic.CANTIDAD_HIJOS,
                                                        CodigoPostal = fic.CODIGO_POSTAL,
                                                        CodigoSeguridad = fic.CODIGO_SEGURIDAD,
                                                        Contacto = fic.CONTACTO,
                                                        DeficienciaOtra = fic.DEFICIENCIA_OTRA,
                                                        TieneDeficienciaMental =
                                                           fic.TIENE_DEF_MENTAL == "S" ? true : false,
                                                        TieneDeficienciaMotora =
                                                           fic.TIENE_DEF_MOTORA == "S" ? true : false,
                                                        TieneDeficienciaPsicologia =
                                                           fic.TIENE_DEF_PSICOLOGICA == "S" ? true : false,
                                                        TieneDeficienciaSensorial =
                                                           fic.TIENE_DEF_SENSORIAL == "S" ? true : false,
                                                        CertificadoDiscapacidad = fic.CERTIF_DISCAP == "S" ? true : false,
                                                        Dpto = fic.DPTO ?? " ",
                                                        EntreCalles = fic.ENTRECALLES ?? " ",
                                                        EsDiscapacitado = fic.ES_DISCAPACITADO == "S" ? true : false,
                                                        EstadoCivil = fic.ESTADO_CIVIL,
                                                        FechaNacimiento = fic.FER_NAC,
                                                        Mail = fic.MAIL,
                                                        Manzana = fic.MANZANA ?? " ",
                                                        Monoblock = fic.MONOBLOCK ?? " ",
                                                        Numero = fic.NUMERO,
                                                        Parcela = (c.T_FICHAS.PARCELA == "0" ? " " : fic.PARCELA) ?? " ",
                                                        Piso = fic.PISO ?? " ",
                                                        Sexo = fic.SEXO,
                                                        TelefonoCelular = fic.TEL_CELULAR,
                                                        TelefonoFijo = fic.TEL_FIJO,
                                                        TieneHijos = fic.TIENE_HIJOS == "S" ? true : false,
                                                        TipoDocumento = fic.TIPO_DOCUMENTO,
                                                        FechaSistema = c.FEC_SIST,
                                                        IdUsuarioSistema = c.ID_USR_SIST,
                                                        UsuarioSistema = c.T_USUARIOS.LOGIN,
                                                        //Beneficiarios
                                                        BeneficiarioFicha = new Beneficiario
                                                                                {
                                                                                    IdBeneficiarioExport = c.ID_BENEFICIARIO,
                                                                                    IdEstado = c.ID_ESTADO ?? 0,
                                                                                    FechaSistema = c.FEC_SIST,
                                                                                    IdFicha = c.ID_FICHA ?? 0,
                                                                                    IdPrograma = c.ID_PROGRAMA ?? 0,
                                                                                    IdUsuarioSistema = c.ID_USR_SIST,
                                                                                    IdConvenio = c.T_PROGRAMAS.CONVENIO_BCO_CBA,
                                                                                    NumeroConvenio = c.T_PROGRAMAS.CONVENIO_BCO_CBA,
                                                                                    CodigoActivacionBancaria = c.COD_ACT_BANCARIO ?? 0,
                                                                                    CodigoNaturalezaJuridica = c.COD_ACT_BANCARIO ?? 0,
                                                                                    CondicionIva = iva.COD_BCO_CBA ?? "0",
                                                                                    Nacionalidad = c.NACIONALIDAD ?? 0,
                                                                                    Residente = c.RESIDENTE,
                                                                                    TipoPersona = c.TIPO_PERSONA,
                                                                                    CodigoMoneda = mon.COD_BCO_CBA ?? "0",
                                                                                    CodigoSucursal = suc.COD_BCO_CBA,
                                                                                    NombreEstado = c.T_ESTADOS.N_ESTADO,
                                                                                    Cbu = cu.CBU ?? "",
                                                                                    ImportePagoPlan = c.T_PROGRAMAS.MONTO_PROG ?? 0,
                                                                                    NumeroCuenta = cu.NRO_CTA,
                                                                                    Programa = c.T_PROGRAMAS.N_PROGRAMA,
                                                                                    MontoPrograma = c.T_PROGRAMAS.MONTO_PROG ?? 0,
                                                                                    TieneApoderado = c.TIENE_APODERADO,
                                                                                    Sucursal = suc.COD_BCO_CBA + " - " + suc.DESCRIPCION,
                                                                                    FechaSolicitudCuenta = cu.FEC_SOL_CTA,
                                                                                    IdSucursal = cu.ID_SUCURSAL,
                                                                                    SucursalDescripcion = suc.DESCRIPCION,
                                                                                    FechaNotificacion = c.FEC_NOTIF,
                                                                                    Notificado = c.NOTIFICADO == "S" ? true : false,
                                                                                    Apoderado =
                                                                                        new Apoderado
                                                                                        {
                                                                                            IdApoderadoNullable = ap.ID_APODERADO,
                                                                                            Nombre = ap.NOMBRE,
                                                                                            Apellido = ap.APELLIDO,
                                                                                            Barrio = ap.BARRIO,
                                                                                            Calle = ap.CALLE,
                                                                                            Cbu = ap.CBU,
                                                                                            CodigoPostal = ap.CODIGO_POSTAL,
                                                                                            Cuil = ap.CUIL,
                                                                                            Dpto = ap.DPTO,
                                                                                            EntreCalles = ap.ENTRECALLES,
                                                                                            FechaNacimiento = ap.FER_NAC,
                                                                                            FechaSolicitudCuenta = ap.FEC_SOL_CTA,
                                                                                            IdEstadoApoderado = ap.ID_ESTADO_APODERADO ?? 0,
                                                                                            IdSistema = ap.ID_SISTEMA ?? 0,
                                                                                            IdSucursal = ap.ID_SUCURSAL_BCO,
                                                                                            Mail = ap.MAIL,
                                                                                            Numero = ap.NUMERO,
                                                                                            IdLocalidad = ap.ID_LOCALIDAD,
                                                                                            LocalidadApoderado =
                                                                                                new Localidad
                                                                                                {
                                                                                                    IdLocalidad = ap.ID_LOCALIDAD ?? 0,
                                                                                                    NombreLocalidad = ap.T_LOCALIDADES.N_LOCALIDAD,
                                                                                                    Departamento =
                                                                                                        new Departamento
                                                                                                        {
                                                                                                            IdDepartamento =
                                                                                                                ap.T_LOCALIDADES.ID_DEPARTAMENTO ?? 0,
                                                                                                            NombreDepartamento =
                                                                                                                ap.T_LOCALIDADES.T_DEPARTAMENTOS.N_DEPARTAMENTO
                                                                                                        }
                                                                                                },
                                                                                            Manzana = ap.MANZANA,
                                                                                            Monoblock = ap.MONOBLOCK,
                                                                                            NombreEstadoApoderado = ap.T_ESTADOS_APODERADO.N_ESTADO_APODERADO,
                                                                                            NumeroCuentaBco = ap.NRO_CUENTA_BCO,
                                                                                            NumeroDocumento = ap.NRO_DOCUMENTO,
                                                                                            Parcela = ap.PARCELA,
                                                                                            Piso = ap.PISO,
                                                                                            Sexo = ap.SEXO,
                                                                                            TelefonoCelular = ap.TEL_CELULAR,
                                                                                            TipoDocumento = ap.TIPO_DOCUMENTO ?? 0,
                                                                                            TelefonoFijo = ap.TEL_FIJO,
                                                                                            UsuarioBanco = ap.USUARIO_BANCO,
                                                                                            SucursalApoderado =
                                                                                                new Sucursal
                                                                                                {
                                                                                                    CodigoBanco = sucapo.COD_BCO_CBA,
                                                                                                    IdSucursal = ap.ID_SUCURSAL_BCO ?? 0,
                                                                                                    Detalle = sucapo.DESCRIPCION
                                                                                                }

                                                                                        }

                                                                                },
                                                        // Fichas Universitarias
                                                        PromedioUniversitaria = fiUniv.PROMEDIO,
                                                        OtraCarreraUniversitaria = fiUniv.OTRA_CARRERA,
                                                        OtraInstitucionUniversitaria = fiUniv.OTRA_INSTITUCION,
                                                        IdCarreraUniversitaria = fiUniv.ID_CARRERA,
                                                        IdEscuelaUniversitaria = fiUniv.ID_ESCUELA,
                                                        OtroSectorUniversitaria = fiUniv.OTRO_SECTOR,
                                                        EscuelaFicha =
                                                            new Escuela
                                                            {
                                                                Anexo = fiUniv.T_ESCUELAS.ANEXO,
                                                                Cue = fiUniv.T_ESCUELAS.CUE,
                                                                Id_Escuela = fiUniv.ID_ESCUELA ?? 0,
                                                                Nombre_Escuela = fiUniv.T_ESCUELAS.N_ESCUELA,
                                                                LocalidadEscuela =
                                                                    new Localidad
                                                                    {
                                                                        IdLocalidad = fiUniv.T_ESCUELAS.ID_LOCALIDAD ?? 0,
                                                                        NombreLocalidad = fiUniv.T_ESCUELAS.T_LOCALIDADES.N_LOCALIDAD,
                                                                        Departamento =
                                                                            new Departamento
                                                                            {
                                                                                IdDepartamento = fiUniv.T_ESCUELAS.T_LOCALIDADES.ID_DEPARTAMENTO ?? 0,
                                                                                NombreDepartamento = fiUniv.T_ESCUELAS.T_LOCALIDADES.T_DEPARTAMENTOS.N_DEPARTAMENTO
                                                                            }
                                                                    }
                                                            },
                                                        CarreraFicha =
                                                            new Carrera
                                                            {
                                                                IdCarrera = fiUniv.ID_CARRERA ?? 0,
                                                                NombreCarrera = fiUniv.T_CARRERAS.N_CARRERA,
                                                                SectorCarrera =
                                                                    new Sector
                                                                    {
                                                                        IdSector = fiUniv.T_CARRERAS.ID_SECTOR ?? 0,
                                                                        NombreSector = fiUniv.T_CARRERAS.T_SECTOR_INSTITUCION.T_SECTOR.N_SECTOR
                                                                    },
                                                                NivelCarrera =
                                                                    new Nivel
                                                                    {
                                                                        IdNivel = fiUniv.T_CARRERAS.ID_NIVEL ?? 0,
                                                                        NombreNivel = fiUniv.T_CARRERAS.T_NIVEL.N_NIVEL
                                                                    },
                                                                InstitucionCarrera =
                                                                    new Institucion
                                                                    {
                                                                        IdInstitucion = fiUniv.T_CARRERAS.ID_INSTITUCION ?? 0,
                                                                        NombreInstitucion = fiUniv.T_CARRERAS.T_SECTOR_INSTITUCION.T_INSTITUCIONES.N_INSTITUCION
                                                                    }
                                                            },

                                                        LocalidadFicha =
                                                            new Localidad
                                                            {
                                                                IdLocalidad = fic.ID_LOCALIDAD ?? 0,
                                                                NombreLocalidad = fic.T_LOCALIDADES.N_LOCALIDAD,
                                                                Departamento =
                                                                    new Departamento
                                                                    {
                                                                        IdDepartamento = fic.T_LOCALIDADES.ID_DEPARTAMENTO ?? 0,
                                                                        NombreDepartamento = fic.T_LOCALIDADES.T_DEPARTAMENTOS.N_DEPARTAMENTO
                                                                    }
                                                            }
                                                    }
                   );

                    return universitaria;

                case (int)Enums.TipoFicha.ReconversionProductiva:
                    var ficharec = (from fic in _mdb.T_FICHAS
                             from c in
                                 _mdb.T_BENEFICIARIOS.Where(fiter => fiter.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                             from fiterc in
                                 _mdb.T_FICHA_TERCIARIO.Where(fiter => fiter.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                             from fiUniv in
                                 _mdb.T_FICHA_UNIVERSITARIO.Where(fiter => fiter.ID_FICHA == fic.ID_FICHA).
                                 DefaultIfEmpty()
                             from cu in
                                 _mdb.T_CUENTAS_BANCO.Where(cu => cu.ID_BENEFICIARIO == c.ID_BENEFICIARIO).
                                 DefaultIfEmpty()
                             from suc in
                                 _mdb.T_TABLAS_BCO_CBA.Where(suc => suc.ID_TABLA_BCO_CBA == cu.ID_SUCURSAL).
                                 DefaultIfEmpty()
                             from mon in
                                 _mdb.T_TABLAS_BCO_CBA.Where(mon => mon.ID_TABLA_BCO_CBA == cu.ID_MONEDA).DefaultIfEmpty
                                 ()
                             from iva in
                                 _mdb.T_TABLAS_BCO_CBA.Where(iva => iva.ID_TABLA_BCO_CBA == c.CONDICION_IVA).
                                 DefaultIfEmpty()
                             from nac in
                                 _mdb.T_TABLAS_BCO_CBA.Where(nac => nac.ID_TABLA_BCO_CBA == c.NACIONALIDAD).
                                 DefaultIfEmpty()
                             from ficharecprod in
                                 _mdb.T_FICHA_REC_PROD.Where(
                                     ficharecprod => ficharecprod.ID_FICHA == fic.ID_FICHA && fic.TIPO_FICHA == tipoficha).
                                 DefaultIfEmpty()
                             from ap in
                                 _mdb.T_APODERADOS.Where(
                                     ap =>
                                     ap.ID_BENEFICIARIO == c.ID_BENEFICIARIO &&
                                     ap.ID_ESTADO_APODERADO == (short)Enums.EstadoApoderado.Activo).DefaultIfEmpty()
                             from sucapo in
                                 _mdb.T_TABLAS_BCO_CBA.Where(
                                     sucapo =>
                                     sucapo.ID_TABLA_BCO_CBA == ap.ID_SUCURSAL_BCO &&
                                     sucapo.ID_TIPO_TABLA_BCO_CBA == (int)Enums.TiposTablaBcoCba.Sucursales).
                                 DefaultIfEmpty()
                             where fic.TIPO_FICHA == tipoficha
                             select new Ficha
                             {
                                 IdFicha = fic.ID_FICHA,
                                 Nombre = fic.NOMBRE,
                                 Apellido = fic.APELLIDO,
                                 NumeroDocumento = fic.NUMERO_DOCUMENTO,
                                 IdEstadoFicha = fic.ID_ESTADO_FICHA ?? (int)Enums.EstadoFicha.SinEstado,
                                 TipoFicha = fic.TIPO_FICHA,
                                 Cuil = fic.CUIL,
                                 NombreEstadoFicha = fic.T_ESTADOS_FICHA.N_ESTADO_FICHA ?? "",
                                 NombreTipoFicha = fic.T_TIPO_FICHA.N_TIPO_FICHA,
                                 Barrio = fic.BARRIO,
                                 Localidad = fic.T_LOCALIDADES.N_LOCALIDAD,
                                 IdLocalidad = fic.ID_LOCALIDAD ?? 0,
                                 Calle = fic.CALLE,
                                 CantidadHijos = fic.CANTIDAD_HIJOS,
                                 CodigoPostal = fic.CODIGO_POSTAL,
                                 CodigoSeguridad = fic.CODIGO_SEGURIDAD,
                                 Contacto = fic.CONTACTO,
                                 DeficienciaOtra = fic.DEFICIENCIA_OTRA,
                                 TieneDeficienciaMental =
                                     fic.TIENE_DEF_MENTAL == "S" ? true : false,
                                 TieneDeficienciaMotora =
                                     fic.TIENE_DEF_MOTORA == "S" ? true : false,
                                 TieneDeficienciaPsicologia =
                                     fic.TIENE_DEF_PSICOLOGICA == "S" ? true : false,
                                 TieneDeficienciaSensorial =
                                     fic.TIENE_DEF_SENSORIAL == "S" ? true : false,
                                 CertificadoDiscapacidad = fic.CERTIF_DISCAP == "S" ? true : false,
                                 Dpto = fic.DPTO ?? " ",
                                 EntreCalles = fic.ENTRECALLES ?? " ",
                                 EsDiscapacitado = fic.ES_DISCAPACITADO == "S" ? true : false,
                                 EstadoCivil = fic.ESTADO_CIVIL,
                                 FechaNacimiento = fic.FER_NAC,
                                 Mail = fic.MAIL,
                                 Manzana = fic.MANZANA ?? " ",
                                 Monoblock = fic.MONOBLOCK ?? " ",
                                 Numero = fic.NUMERO,
                                 Parcela = (c.T_FICHAS.PARCELA == "0" ? " " : fic.PARCELA) ?? " ",
                                 Piso = fic.PISO ?? " ",
                                 Sexo = fic.SEXO,
                                 TelefonoCelular = fic.TEL_CELULAR,
                                 TelefonoFijo = fic.TEL_FIJO,
                                 TieneHijos = fic.TIENE_HIJOS == "S" ? true : false,
                                 TipoDocumento = fic.TIPO_DOCUMENTO,
                                 FechaSistema = c.FEC_SIST,
                                 IdUsuarioSistema = c.ID_USR_SIST,
                                 UsuarioSistema = c.T_USUARIOS.LOGIN,
                                 //Beneficiarios
                                 BeneficiarioFicha = new Beneficiario
                                 {
                                     IdBeneficiarioExport = c.ID_BENEFICIARIO,
                                     IdEstado = c.ID_ESTADO ?? 0,
                                     FechaSistema = c.FEC_SIST,
                                     IdFicha = c.ID_FICHA ?? 0,
                                     IdPrograma = c.ID_PROGRAMA ?? 0,
                                     IdUsuarioSistema = c.ID_USR_SIST,
                                     IdConvenio = c.T_PROGRAMAS.CONVENIO_BCO_CBA,
                                     NumeroConvenio = c.T_PROGRAMAS.CONVENIO_BCO_CBA,
                                     CodigoActivacionBancaria =
                                         c.COD_ACT_BANCARIO ?? 0,
                                     CodigoNaturalezaJuridica =
                                         c.COD_ACT_BANCARIO ?? 0,
                                     CondicionIva = iva.COD_BCO_CBA ?? "0",
                                     Nacionalidad = c.NACIONALIDAD ?? 0,
                                     Residente = c.RESIDENTE,
                                     TipoPersona = c.TIPO_PERSONA,
                                     CodigoMoneda = mon.COD_BCO_CBA ?? "0",
                                     CodigoSucursal = suc.COD_BCO_CBA,
                                     NombreEstado = c.T_ESTADOS.N_ESTADO,
                                     Cbu = cu.CBU ?? "",
                                     ImportePagoPlan = c.T_PROGRAMAS.MONTO_PROG ?? 0,
                                     NumeroCuenta = cu.NRO_CTA,
                                     Programa = c.T_PROGRAMAS.N_PROGRAMA,
                                     MontoPrograma = c.T_PROGRAMAS.MONTO_PROG ?? 0,
                                     TieneApoderado = c.TIENE_APODERADO,
                                     Sucursal =
                                         suc.COD_BCO_CBA + " - " + suc.DESCRIPCION,
                                     SucursalDescripcion = suc.DESCRIPCION,
                                     FechaSolicitudCuenta = cu.FEC_SOL_CTA,
                                     FechaNotificacion = c.FEC_NOTIF,
                                     Notificado = c.NOTIFICADO == "S" ? true : false,
                                     IdSucursal = cu.ID_SUCURSAL,
                                     Apoderado =
                                         new Apoderado
                                         {
                                             IdApoderadoNullable = ap.ID_APODERADO,
                                             Nombre = ap.NOMBRE,
                                             Apellido = ap.APELLIDO,
                                             Barrio = ap.BARRIO,
                                             Calle = ap.CALLE,
                                             Cbu = ap.CBU,
                                             CodigoPostal = ap.CODIGO_POSTAL,
                                             Cuil = ap.CUIL,
                                             Dpto = ap.DPTO,
                                             EntreCalles = ap.ENTRECALLES,
                                             FechaNacimiento = ap.FER_NAC,
                                             FechaSolicitudCuenta =
                                                 ap.FEC_SOL_CTA,
                                             IdEstadoApoderado =
                                                 ap.ID_ESTADO_APODERADO ?? 0,
                                             IdSistema = ap.ID_SISTEMA ?? 0,
                                             IdSucursal = ap.ID_SUCURSAL_BCO,
                                             Mail = ap.MAIL,
                                             Numero = ap.NUMERO,
                                             IdLocalidad = ap.ID_LOCALIDAD,
                                             LocalidadApoderado =
                                                 new Localidad
                                                 {
                                                     IdLocalidad = ap.ID_LOCALIDAD ?? 0,
                                                     NombreLocalidad = ap.T_LOCALIDADES.N_LOCALIDAD,
                                                     Departamento =
                                                         new Departamento
                                                         {
                                                             IdDepartamento =
                                                                 ap.T_LOCALIDADES.ID_DEPARTAMENTO ?? 0,
                                                             NombreDepartamento =
                                                                 ap.T_LOCALIDADES.T_DEPARTAMENTOS.N_DEPARTAMENTO
                                                         }
                                                 },
                                             Manzana = ap.MANZANA,
                                             Monoblock = ap.MONOBLOCK,
                                             NombreEstadoApoderado = ap.T_ESTADOS_APODERADO.N_ESTADO_APODERADO,
                                             NumeroCuentaBco = ap.NRO_CUENTA_BCO,
                                             NumeroDocumento = ap.NRO_DOCUMENTO,
                                             Parcela = ap.PARCELA,
                                             Piso = ap.PISO,
                                             Sexo = ap.SEXO,
                                             TelefonoCelular = ap.TEL_CELULAR,
                                             TipoDocumento = ap.TIPO_DOCUMENTO ?? 0,
                                             TelefonoFijo = ap.TEL_FIJO,
                                             UsuarioBanco = ap.USUARIO_BANCO,
                                             SucursalApoderado =
                                                 new Sucursal
                                                 {
                                                     CodigoBanco = sucapo.COD_BCO_CBA,
                                                     IdSucursal = ap.ID_SUCURSAL_BCO ?? 0,
                                                     Detalle = sucapo.DESCRIPCION
                                                 }
                                         }
                                 },
                                 FichaReconversion = 
                                     new FichaReconversion
                                     {
                                         AltaTemprana = ficharecprod.ALTA_TEMPRANA,
                                         IdEmpresa = ficharecprod.ID_EMPRESA,
                                         Modalidad = ficharecprod.MODALIDAD,
                                         IdSede = ficharecprod.ID_SEDE,
                                         FechaInicioActividad = ficharecprod.AT_FECHA_INICIO,
                                         FechaFinActividad = ficharecprod.AT_FECHA_CESE,
                                         IdModalidadAfip = ficharecprod.ID_MOD_CONT_AFIP,
                                         ModalidadAfip = ficharecprod.T_MOD_CONT_AFIP.N_MOD_CONT_AFIP,
                                         DeseaTermNivel = (ficharecprod.DESEA_TERM_NIVEL == "S" ? true : false),
                                         IdNivelEscolaridad = ficharecprod.ID_NIVEL_ESCOLARIDAD,
                                         Finalizado = (ficharecprod.FINALIZADO == "S" ? true : false),
                                         Cursando = (ficharecprod.CURSANDO == "S" ? true : false),
                                         IdSubprograma = ficharecprod.ID_SUBPROGRAMA,
                                         Subprograma = ficharecprod.T_SUBPROGRAMAS.N_SUBPROGRAMA,
                                         AportesDeLaEmpresa = ficharecprod.APORTES_EMP,
                                         ApellidoTutor = ficharecprod.APELLIDO_TUTOR,
                                         NombreTutor = ficharecprod.NOMBRE_TUTOR,
                                         NroDocumentoTutor = ficharecprod.DNI_TUTOR,
                                         PuestoTutor = ficharecprod.PUESTO_TUTOR,
                                         Proyecto = 
                                         new Proyecto
                                             {
                                                 IdProyecto = ficharecprod.T_PROYECTOS.ID_PROYECTO,
                                                 NombreProyecto = ficharecprod.T_PROYECTOS.N_PROYECTO,
                                                 CantidadAcapacitar = ficharecprod.T_PROYECTOS.CANT_CAPACITAR,
                                                 FechaInicioProyecto = ficharecprod.T_PROYECTOS.FEC_INICIO,
                                                 FechaFinProyecto = ficharecprod.T_PROYECTOS.FEC_FIN,
                                                 MesesDuracionProyecto = ficharecprod.T_PROYECTOS.MESES_DURAC
                                             },
                                         Sede = 
                                         new Sede
                                             {
                                                     IdSede= ficharecprod.ID_SEDE ?? 0,
                                                     NombreSede = ficharecprod.T_SEDES.N_SEDE,
                                                     Calle = ficharecprod.T_SEDES.CALLE,
                                                     Numero = ficharecprod.T_SEDES.NUMERO,
                                                     Piso = ficharecprod.T_SEDES.PISO,
                                                     Dpto = ficharecprod.T_SEDES.DPTO,
                                                     CodigoPostal = ficharecprod.T_SEDES.CODIGO_POSTAL,
                                                     NombreLocalidad =  ficharecprod.T_SEDES.T_LOCALIDADES.N_LOCALIDAD,
                                                     ApellidoContacto = ficharecprod.T_SEDES.APE_CONTACTO,
                                                     NombreContacto = ficharecprod.T_SEDES.NOM_CONTACTO,
                                                     Telefono = ficharecprod.T_SEDES.TELEFONO,
                                                     Fax = ficharecprod.T_SEDES.FAX,
                                                     Email = ficharecprod.T_SEDES.EMAIL
                                             },
                                         Empresa =
                                             new Empresa
                                             {
                                                 Calle = ficharecprod.T_EMPRESAS.CALLE,
                                                 CantidadEmpleados =ficharecprod.T_EMPRESAS.CANTIDAD_EMPLEADOS,
                                                 CodigoActividad =ficharecprod.T_EMPRESAS.CODIGO_ACTIVIDAD,
                                                 CodigoPostal = ficharecprod.T_EMPRESAS.CODIGO_POSTAL,
                                                 Cuit = ficharecprod.T_EMPRESAS.CUIT,
                                                 DomicilioLaboralIdem =ficharecprod.T_EMPRESAS.DOMICLIO_LABORAL_IDEM,
                                                 IdEmpresa = ficharecprod.ID_EMPRESA ?? 0,
                                                 NombreEmpresa = ficharecprod.T_EMPRESAS.N_EMPRESA,
                                                 Numero = ficharecprod.T_EMPRESAS.NUMERO,
                                                 Piso = ficharecprod.T_EMPRESAS.PISO,
                                                 Telefono = ficharecprod.T_EMPRESAS.PISO,
                                                 LocalidadEmpresa =
                                                     new Localidad
                                                     {
                                                         IdLocalidad =ficharecprod.T_EMPRESAS.ID_LOCALIDAD ?? 0,
                                                         NombreLocalidad =ficharecprod.T_EMPRESAS.T_LOCALIDADES.N_LOCALIDAD,
                                                         Departamento =
                                                             new Departamento
                                                             {
                                                                 IdDepartamento =
                                                                     ficharecprod.T_EMPRESAS.T_LOCALIDADES.ID_DEPARTAMENTO ?? 0,
                                                                 NombreDepartamento =
                                                                     ficharecprod.T_EMPRESAS.T_LOCALIDADES.T_DEPARTAMENTOS.
                                                                     N_DEPARTAMENTO
                                                             }
                                                     },
                                                 Usuario =
                                                     new UsuarioEmpresa
                                                     {
                                                         ApellidoUsuario =
                                                             ficharecprod.T_EMPRESAS.
                                                             T_USUARIOS_EMPRESA.APELLIDO,
                                                         Cuil =
                                                             ficharecprod.T_EMPRESAS.
                                                             T_USUARIOS_EMPRESA.CUIL,
                                                         IdUsuarioEmpresa =
                                                             ficharecprod.T_EMPRESAS.ID_USUARIO,
                                                         Mail =
                                                             ficharecprod.T_EMPRESAS.
                                                             T_USUARIOS_EMPRESA.MAIL,
                                                         NombreUsuario =
                                                             ficharecprod.T_EMPRESAS.
                                                             T_USUARIOS_EMPRESA.NOMBRE
                                                     }
                                             }
                                     },
                                 LocalidadFicha =
                                     new Localidad
                                     {
                                         IdLocalidad = fic.ID_LOCALIDAD ?? 0,
                                         NombreLocalidad = fic.T_LOCALIDADES.N_LOCALIDAD,
                                         Departamento =
                                             new Departamento
                                             {
                                                 IdDepartamento = fic.T_LOCALIDADES.ID_DEPARTAMENTO ?? 0,
                                                 NombreDepartamento = fic.T_LOCALIDADES.T_DEPARTAMENTOS.N_DEPARTAMENTO
                                             }
                                     }
                             }
                       );
                    return ficharec;

                case (int)Enums.TipoFicha.EfectoresSociales:
                    var fichaefec = (from fic in _mdb.T_FICHAS
                                    from c in
                                        _mdb.T_BENEFICIARIOS.Where(fiter => fiter.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                                    from fiterc in
                                        _mdb.T_FICHA_TERCIARIO.Where(fiter => fiter.ID_FICHA == fic.ID_FICHA).DefaultIfEmpty()
                                    from fiUniv in
                                        _mdb.T_FICHA_UNIVERSITARIO.Where(fiter => fiter.ID_FICHA == fic.ID_FICHA).
                                        DefaultIfEmpty()
                                    from cu in
                                        _mdb.T_CUENTAS_BANCO.Where(cu => cu.ID_BENEFICIARIO == c.ID_BENEFICIARIO).
                                        DefaultIfEmpty()
                                    from suc in
                                        _mdb.T_TABLAS_BCO_CBA.Where(suc => suc.ID_TABLA_BCO_CBA == cu.ID_SUCURSAL).
                                        DefaultIfEmpty()
                                    from mon in
                                        _mdb.T_TABLAS_BCO_CBA.Where(mon => mon.ID_TABLA_BCO_CBA == cu.ID_MONEDA).DefaultIfEmpty
                                        ()
                                    from iva in
                                        _mdb.T_TABLAS_BCO_CBA.Where(iva => iva.ID_TABLA_BCO_CBA == c.CONDICION_IVA).
                                        DefaultIfEmpty()
                                    from nac in
                                        _mdb.T_TABLAS_BCO_CBA.Where(nac => nac.ID_TABLA_BCO_CBA == c.NACIONALIDAD).
                                        DefaultIfEmpty()
                                    from fichaefecsoc in
                                        _mdb.T_FICHA_EFEC_SOC.Where(
                                            fichaefecsoc => fichaefecsoc.ID_FICHA == fic.ID_FICHA && fic.TIPO_FICHA == tipoficha).
                                        DefaultIfEmpty()
                                    from ap in
                                        _mdb.T_APODERADOS.Where(
                                            ap =>
                                            ap.ID_BENEFICIARIO == c.ID_BENEFICIARIO &&
                                            ap.ID_ESTADO_APODERADO == (short)Enums.EstadoApoderado.Activo).DefaultIfEmpty()
                                    from sucapo in
                                        _mdb.T_TABLAS_BCO_CBA.Where(
                                            sucapo =>
                                            sucapo.ID_TABLA_BCO_CBA == ap.ID_SUCURSAL_BCO &&
                                            sucapo.ID_TIPO_TABLA_BCO_CBA == (int)Enums.TiposTablaBcoCba.Sucursales).
                                        DefaultIfEmpty()
                                    where fic.TIPO_FICHA == tipoficha
                                    select new Ficha
                                    {
                                        IdFicha = fic.ID_FICHA,
                                        Nombre = fic.NOMBRE,
                                        Apellido = fic.APELLIDO,
                                        NumeroDocumento = fic.NUMERO_DOCUMENTO,
                                        IdEstadoFicha = fic.ID_ESTADO_FICHA ?? (int)Enums.EstadoFicha.SinEstado,
                                        TipoFicha = fic.TIPO_FICHA,
                                        Cuil = fic.CUIL,
                                        NombreEstadoFicha = fic.T_ESTADOS_FICHA.N_ESTADO_FICHA ?? "",
                                        NombreTipoFicha = fic.T_TIPO_FICHA.N_TIPO_FICHA,
                                        Barrio = fic.BARRIO,
                                        Localidad = fic.T_LOCALIDADES.N_LOCALIDAD,
                                        IdLocalidad = fic.ID_LOCALIDAD ?? 0,
                                        Calle = fic.CALLE,
                                        CantidadHijos = fic.CANTIDAD_HIJOS,
                                        CodigoPostal = fic.CODIGO_POSTAL,
                                        CodigoSeguridad = fic.CODIGO_SEGURIDAD,
                                        Contacto = fic.CONTACTO,
                                        DeficienciaOtra = fic.DEFICIENCIA_OTRA,
                                        TieneDeficienciaMental =fic.TIENE_DEF_MENTAL == "S" ? true : false,
                                        TieneDeficienciaMotora =fic.TIENE_DEF_MOTORA == "S" ? true : false,
                                        TieneDeficienciaPsicologia =fic.TIENE_DEF_PSICOLOGICA == "S" ? true : false,
                                        TieneDeficienciaSensorial =fic.TIENE_DEF_SENSORIAL == "S" ? true : false,
                                        CertificadoDiscapacidad = fic.CERTIF_DISCAP == "S" ? true : false,
                                        Dpto = fic.DPTO ?? " ",
                                        EntreCalles = fic.ENTRECALLES ?? " ",
                                        EsDiscapacitado = fic.ES_DISCAPACITADO == "S" ? true : false,
                                        EstadoCivil = fic.ESTADO_CIVIL,
                                        FechaNacimiento = fic.FER_NAC,
                                        Mail = fic.MAIL,
                                        Manzana = fic.MANZANA ?? " ",
                                        Monoblock = fic.MONOBLOCK ?? " ",
                                        Numero = fic.NUMERO,
                                        Parcela = (c.T_FICHAS.PARCELA == "0" ? " " : fic.PARCELA) ?? " ",
                                        Piso = fic.PISO ?? " ",
                                        Sexo = fic.SEXO,
                                        TelefonoCelular = fic.TEL_CELULAR,
                                        TelefonoFijo = fic.TEL_FIJO,
                                        TieneHijos = fic.TIENE_HIJOS == "S" ? true : false,
                                        TipoDocumento = fic.TIPO_DOCUMENTO,
                                        FechaSistema = c.FEC_SIST,
                                        IdUsuarioSistema = c.ID_USR_SIST,
                                        UsuarioSistema = c.T_USUARIOS.LOGIN,
                                        //Beneficiarios
                                        BeneficiarioFicha = new Beneficiario
                                        {
                                            IdBeneficiarioExport = c.ID_BENEFICIARIO,
                                            IdEstado = c.ID_ESTADO ?? 0,
                                            FechaSistema = c.FEC_SIST,
                                            IdFicha = c.ID_FICHA ?? 0,
                                            IdPrograma = c.ID_PROGRAMA ?? 0,
                                            IdUsuarioSistema = c.ID_USR_SIST,
                                            IdConvenio = c.T_PROGRAMAS.CONVENIO_BCO_CBA,
                                            NumeroConvenio = c.T_PROGRAMAS.CONVENIO_BCO_CBA,
                                            CodigoActivacionBancaria = c.COD_ACT_BANCARIO ?? 0,
                                            CodigoNaturalezaJuridica = c.COD_ACT_BANCARIO ?? 0,
                                            CondicionIva = iva.COD_BCO_CBA ?? "0",
                                            Nacionalidad = c.NACIONALIDAD ?? 0,
                                            Residente = c.RESIDENTE,
                                            TipoPersona = c.TIPO_PERSONA,
                                            CodigoMoneda = mon.COD_BCO_CBA ?? "0",
                                            CodigoSucursal = suc.COD_BCO_CBA,
                                            NombreEstado = c.T_ESTADOS.N_ESTADO,
                                            Cbu = cu.CBU ?? "",
                                            ImportePagoPlan = c.T_PROGRAMAS.MONTO_PROG ?? 0,
                                            NumeroCuenta = cu.NRO_CTA,
                                            Programa = c.T_PROGRAMAS.N_PROGRAMA,
                                            MontoPrograma = c.T_PROGRAMAS.MONTO_PROG ?? 0,
                                            TieneApoderado = c.TIENE_APODERADO,
                                            Sucursal = suc.COD_BCO_CBA + " - " + suc.DESCRIPCION,
                                            SucursalDescripcion = suc.DESCRIPCION,
                                            FechaSolicitudCuenta = cu.FEC_SOL_CTA,
                                            FechaNotificacion = c.FEC_NOTIF,
                                            Notificado = c.NOTIFICADO == "S" ? true : false,
                                            IdSucursal = cu.ID_SUCURSAL,
                                            Apoderado =
                                                new Apoderado
                                                {
                                                    IdApoderadoNullable = ap.ID_APODERADO,
                                                    Nombre = ap.NOMBRE,
                                                    Apellido = ap.APELLIDO,
                                                    Barrio = ap.BARRIO,
                                                    Calle = ap.CALLE,
                                                    Cbu = ap.CBU,
                                                    CodigoPostal = ap.CODIGO_POSTAL,
                                                    Cuil = ap.CUIL,
                                                    Dpto = ap.DPTO,
                                                    EntreCalles = ap.ENTRECALLES,
                                                    FechaNacimiento = ap.FER_NAC,
                                                    FechaSolicitudCuenta =ap.FEC_SOL_CTA,
                                                    IdEstadoApoderado =ap.ID_ESTADO_APODERADO ?? 0,
                                                    IdSistema = ap.ID_SISTEMA ?? 0,
                                                    IdSucursal = ap.ID_SUCURSAL_BCO,
                                                    Mail = ap.MAIL,
                                                    Numero = ap.NUMERO,
                                                    IdLocalidad = ap.ID_LOCALIDAD,
                                                    LocalidadApoderado =
                                                        new Localidad
                                                        {
                                                            IdLocalidad = ap.ID_LOCALIDAD ?? 0,
                                                            NombreLocalidad = ap.T_LOCALIDADES.N_LOCALIDAD,
                                                            Departamento =
                                                                new Departamento
                                                                {
                                                                    IdDepartamento =
                                                                        ap.T_LOCALIDADES.ID_DEPARTAMENTO ?? 0,
                                                                    NombreDepartamento =
                                                                        ap.T_LOCALIDADES.T_DEPARTAMENTOS.N_DEPARTAMENTO
                                                                }
                                                        },
                                                    Manzana = ap.MANZANA,
                                                    Monoblock = ap.MONOBLOCK,
                                                    NombreEstadoApoderado = ap.T_ESTADOS_APODERADO.N_ESTADO_APODERADO,
                                                    NumeroCuentaBco = ap.NRO_CUENTA_BCO,
                                                    NumeroDocumento = ap.NRO_DOCUMENTO,
                                                    Parcela = ap.PARCELA,
                                                    Piso = ap.PISO,
                                                    Sexo = ap.SEXO,
                                                    TelefonoCelular = ap.TEL_CELULAR,
                                                    TipoDocumento = ap.TIPO_DOCUMENTO ?? 0,
                                                    TelefonoFijo = ap.TEL_FIJO,
                                                    UsuarioBanco = ap.USUARIO_BANCO,
                                                    SucursalApoderado =
                                                        new Sucursal
                                                        {
                                                            CodigoBanco = sucapo.COD_BCO_CBA,
                                                            IdSucursal = ap.ID_SUCURSAL_BCO ?? 0,
                                                            Detalle = sucapo.DESCRIPCION
                                                        }
                                                }
                                        },
                                        FichaEfectores = 
                                            new FichaEfectoresSociales
                                            {
                                                AltaTemprana = fichaefecsoc.ALTA_TEMPRANA,
                                                IdEmpresa = fichaefecsoc.ID_EMPRESA,
                                                Modalidad = fichaefecsoc.MODALIDAD,
                                                IdSede = fichaefecsoc.ID_SEDE,
                                                FechaInicioActividad = fichaefecsoc.AT_FECHA_INICIO,
                                                FechaFinActividad = fichaefecsoc.AT_FECHA_CESE,
                                                IdModalidadAfip = fichaefecsoc.ID_MOD_CONT_AFIP,
                                                ModalidadAfip = fichaefecsoc.T_MOD_CONT_AFIP.N_MOD_CONT_AFIP,
                                                DeseaTermNivel = (fichaefecsoc.DESEA_TERM_NIVEL == "S" ? true : false),
                                                IdNivelEscolaridad = fichaefecsoc.ID_NIVEL_ESCOLARIDAD,
                                                Finalizado = (fichaefecsoc.FINALIZADO == "S" ? true : false),
                                                Cursando = (fichaefecsoc.CURSANDO == "S" ? true : false),
                                                IdSubprograma = fichaefecsoc.ID_SUBPROGRAMA,
                                                Subprograma = fichaefecsoc.T_SUBPROGRAMAS.N_SUBPROGRAMA,
                                                AportesDeLaEmpresa = fichaefecsoc.APORTES_EMP,
                                                ApellidoTutor = fichaefecsoc.APELLIDO_TUTOR,
                                                NombreTutor = fichaefecsoc.NOMBRE_TUTOR,
                                                NroDocumentoTutor = fichaefecsoc.DNI_TUTOR,
                                                PuestoTutor = fichaefecsoc.PUESTO_TUTOR,
                                                Proyecto =
                                                new Proyecto
                                                {
                                                    IdProyecto = fichaefecsoc.T_PROYECTOS.ID_PROYECTO,
                                                    NombreProyecto = fichaefecsoc.T_PROYECTOS.N_PROYECTO,
                                                    CantidadAcapacitar = fichaefecsoc.T_PROYECTOS.CANT_CAPACITAR,
                                                    FechaInicioProyecto = fichaefecsoc.T_PROYECTOS.FEC_INICIO,
                                                    FechaFinProyecto = fichaefecsoc.T_PROYECTOS.FEC_FIN,
                                                    MesesDuracionProyecto = fichaefecsoc.T_PROYECTOS.MESES_DURAC
                                                },
                                                Sede =
                                                new Sede
                                                {
                                                    IdSede = fichaefecsoc.ID_SEDE ?? 0,
                                                    NombreSede = fichaefecsoc.T_SEDES.N_SEDE,
                                                    Calle = fichaefecsoc.T_SEDES.CALLE,
                                                    Numero = fichaefecsoc.T_SEDES.NUMERO,
                                                    Piso = fichaefecsoc.T_SEDES.PISO,
                                                    Dpto = fichaefecsoc.T_SEDES.DPTO,
                                                    CodigoPostal = fichaefecsoc.T_SEDES.CODIGO_POSTAL,
                                                    NombreLocalidad = fichaefecsoc.T_SEDES.T_LOCALIDADES.N_LOCALIDAD,
                                                    ApellidoContacto = fichaefecsoc.T_SEDES.APE_CONTACTO,
                                                    NombreContacto = fichaefecsoc.T_SEDES.NOM_CONTACTO,
                                                    Telefono = fichaefecsoc.T_SEDES.TELEFONO,
                                                    Fax = fichaefecsoc.T_SEDES.FAX,
                                                    Email = fichaefecsoc.T_SEDES.EMAIL
                                                },
                                                Empresa =
                                                    new Empresa
                                                    {
                                                        Calle = fichaefecsoc.T_EMPRESAS.CALLE,
                                                        CantidadEmpleados = fichaefecsoc.T_EMPRESAS.CANTIDAD_EMPLEADOS,
                                                        CodigoActividad = fichaefecsoc.T_EMPRESAS.CODIGO_ACTIVIDAD,
                                                        CodigoPostal = fichaefecsoc.T_EMPRESAS.CODIGO_POSTAL,
                                                        Cuit = fichaefecsoc.T_EMPRESAS.CUIT,
                                                        DomicilioLaboralIdem = fichaefecsoc.T_EMPRESAS.DOMICLIO_LABORAL_IDEM,
                                                        IdEmpresa = fichaefecsoc.ID_EMPRESA ?? 0,
                                                        NombreEmpresa = fichaefecsoc.T_EMPRESAS.N_EMPRESA,
                                                        Numero = fichaefecsoc.T_EMPRESAS.NUMERO,
                                                        Piso = fichaefecsoc.T_EMPRESAS.PISO,
                                                        Telefono = fichaefecsoc.T_EMPRESAS.PISO,
                                                        LocalidadEmpresa =
                                                            new Localidad
                                                            {
                                                                IdLocalidad = fichaefecsoc.T_EMPRESAS.ID_LOCALIDAD ?? 0,
                                                                NombreLocalidad = fichaefecsoc.T_EMPRESAS.T_LOCALIDADES.N_LOCALIDAD,
                                                                Departamento =
                                                                    new Departamento
                                                                    {
                                                                        IdDepartamento = fichaefecsoc.T_EMPRESAS.T_LOCALIDADES.ID_DEPARTAMENTO ?? 0,
                                                                        NombreDepartamento = fichaefecsoc.T_EMPRESAS.T_LOCALIDADES.T_DEPARTAMENTOS.N_DEPARTAMENTO
                                                                    }
                                                            },
                                                        Usuario =
                                                            new UsuarioEmpresa
                                                            {
                                                                ApellidoUsuario =
                                                                    fichaefecsoc.T_EMPRESAS.
                                                                    T_USUARIOS_EMPRESA.APELLIDO,
                                                                Cuil = fichaefecsoc.T_EMPRESAS.T_USUARIOS_EMPRESA.CUIL,
                                                                IdUsuarioEmpresa = fichaefecsoc.T_EMPRESAS.ID_USUARIO,
                                                                Mail = fichaefecsoc.T_EMPRESAS.T_USUARIOS_EMPRESA.MAIL,
                                                                NombreUsuario = fichaefecsoc.T_EMPRESAS.T_USUARIOS_EMPRESA.NOMBRE
                                                            }
                                                    }
                                            },
                                        LocalidadFicha =
                                            new Localidad
                                            {
                                                IdLocalidad = fic.ID_LOCALIDAD ?? 0,
                                                NombreLocalidad = fic.T_LOCALIDADES.N_LOCALIDAD,
                                                Departamento =
                                                    new Departamento
                                                    {
                                                        IdDepartamento = fic.T_LOCALIDADES.ID_DEPARTAMENTO ?? 0,
                                                        NombreDepartamento = fic.T_LOCALIDADES.T_DEPARTAMENTOS.N_DEPARTAMENTO
                                                    }
                                            }
                                    }
                       );
                    return fichaefec;
                default:
                    return null;
            }
        }

        public IList<IFicha> GetFichas()
        {
            return QFicha().OrderBy(c => c.Apellido.Trim().ToUpper()).ThenBy(o => o.Nombre.Trim().ToUpper()).ToList();
        }

        public IFicha GetFicha(int idFicha)
        {
            return QFicha().Where(c => c.IdFicha == idFicha).SingleOrDefault();
        }

        public int GetCountFichas()
        {
            return QFichaSimple().Count();
        }

        public IList<IFicha> GetFichas(int skip, int take)
        {
            return QFichaSimple().OrderBy(c => c.Apellido.ToLower().Trim()).ThenBy(o => o.Nombre.Trim().ToUpper())
                .Skip(skip).Take(take).ToList();
        }

        public bool UpdateFicha(IFicha ficha)
        {
            AgregarDatos(ficha);

            var obj = _mdb.T_FICHAS.FirstOrDefault(c => c.ID_FICHA == ficha.IdFicha);
            obj.APELLIDO = ficha.Apellido.ToUpper();
            obj.BARRIO = ficha.Barrio;
            obj.CALLE = ficha.Calle;
            obj.CANTIDAD_HIJOS = ficha.CantidadHijos ?? 0;
            obj.CERTIF_DISCAP = ficha.CertificadoDiscapacidad ? "S" : "N";
            obj.CODIGO_POSTAL = ficha.CodigoPostal;
            obj.CONTACTO = ficha.Contacto;
            obj.CUIL = ficha.Cuil;
            obj.DEFICIENCIA_OTRA = ficha.DeficienciaOtra;
            obj.DPTO = ficha.Dpto;
            obj.ENTRECALLES = ficha.EntreCalles;
            obj.ID_LOCALIDAD = ficha.IdLocalidad;
            obj.MAIL = ficha.Mail;
            obj.ES_DISCAPACITADO = ficha.EsDiscapacitado ? "S" : "N";
            obj.ESTADO_CIVIL = ficha.EstadoCivil ?? 0;
            obj.FER_NAC = ficha.FechaNacimiento ?? DateTime.Now;
            obj.MANZANA = ficha.Manzana;
            obj.MONOBLOCK = ficha.Monoblock;
            obj.NOMBRE = ficha.Nombre.ToUpper();
            obj.NUMERO = ficha.Numero;
            obj.NUMERO_DOCUMENTO = ficha.NumeroDocumento;
            obj.PARCELA = ficha.Parcela;
            obj.PISO = ficha.Piso;
            obj.SEXO = ficha.Sexo;
            obj.TEL_CELULAR = ficha.TelefonoCelular;
            obj.TEL_FIJO = ficha.TelefonoFijo;
            obj.TIENE_DEF_MENTAL = ficha.TieneDeficienciaMental ? "S" : "N";
            obj.TIENE_DEF_MOTORA = ficha.TieneDeficienciaMotora ? "S" : "N";
            obj.TIENE_DEF_PSICOLOGICA = ficha.TieneDeficienciaPsicologia ? "S" : "N";
            obj.TIENE_DEF_SENSORIAL = ficha.TieneDeficienciaSensorial ? "S" : "N";
            obj.TIENE_HIJOS = ficha.TieneHijos ? "S" : "N";
            obj.TIPO_DOCUMENTO = ficha.TipoDocumento ?? 0;
            obj.TIPO_FICHA = Convert.ToInt16(ficha.TipoFicha);
            obj.ID_USR_SIST = ficha.IdUsuarioSistema;
            obj.NRO_TRAMITE = ficha.NroTramite;
            obj.ID_BARRIO = ficha.idBarrio;

            switch (obj.TIPO_FICHA)
            {
                case (int)Enums.TipoFicha.Terciaria:
                    var objTerciario = _mdb.T_FICHA_TERCIARIO.FirstOrDefault(c => c.ID_FICHA == ficha.IdFicha);
                    objTerciario.ID_CARRERA = ficha.IdCarreraTerciaria;
                    objTerciario.ID_ESCUELA = ficha.IdEscuelaTerciaria;
                    objTerciario.PROMEDIO = ficha.PromedioTerciaria ?? 0;
                    objTerciario.OTRA_CARRERA = ficha.OtraCarreraTerciaria;
                    objTerciario.OTRA_INSTITUCION = ficha.OtraInstitucionTerciaria;
                    objTerciario.OTRO_SECTOR = ficha.OtroSectorTerciaria;
                    objTerciario.PROMEDIO = ficha.PromedioTerciaria ?? 0;
                    objTerciario.ID_USR_SIST = ficha.IdUsuarioSistema;
                    objTerciario.ID_SUBPROGRAMA = ficha.IdSubprograma == 0 ? null : ficha.IdSubprograma;
                    break;
                case (int)Enums.TipoFicha.Universitaria:
                    var objUniversitario = _mdb.T_FICHA_UNIVERSITARIO.FirstOrDefault(c => c.ID_FICHA == ficha.IdFicha);
                    objUniversitario.ID_CARRERA = ficha.IdCarreraUniversitaria;
                    objUniversitario.ID_ESCUELA = ficha.IdEscuelaUniversitaria;
                    objUniversitario.PROMEDIO = ficha.PromedioUniversitaria;
                    objUniversitario.OTRA_CARRERA = ficha.OtraCarreraUniversitaria;
                    objUniversitario.OTRA_INSTITUCION = ficha.OtraInstitucionUniversitaria;
                    objUniversitario.OTRO_SECTOR = ficha.OtroSectorUniversitaria;
                    objUniversitario.PROMEDIO = ficha.PromedioUniversitaria;
                    objUniversitario.ID_USR_SIST = ficha.IdUsuarioSistema;
                    objUniversitario.ID_SUBPROGRAMA = ficha.IdSubprograma == 0 ? null : ficha.IdSubprograma;
                    break;
                case (int)Enums.TipoFicha.Ppp:
                    var objPpp = _mdb.T_FICHA_PPP.FirstOrDefault(c => c.ID_FICHA == ficha.IdFicha);
                    objPpp.CURSANDO = ficha.Cursando ? "S" : "N";
                    objPpp.DESEA_TERM_NIVEL = ficha.DeseaTermNivel ? "S" : "N";
                    objPpp.FINALIZADO = ficha.Finalizado ? "S" : "N";
                    objPpp.ID_NIVEL_ESCOLARIDAD = Convert.ToInt16(ficha.IdNivelEscolaridad);
                    objPpp.MODALIDAD = ficha.Modalidad;
                    objPpp.ALTA_TEMPRANA = ficha.AltaTemprana == "N" ? null : "S";
                    objPpp.TAREAS = ficha.Tareas;
                    objPpp.ID_EMPRESA = ficha.IdEmpresa;
                    objPpp.ID_USR_SIST = ficha.IdUsuarioSistema;
                    objPpp.AT_FECHA_CESE = ficha.FechaFinActividad;
                    objPpp.AT_FECHA_INICIO = ficha.FechaInicioActividad;
                    objPpp.ID_MOD_CONT_AFIP = ficha.IdModalidadAfip == 0 ? null : ficha.IdModalidadAfip;
                    objPpp.ID_SEDE = (ficha.IdSede == 0 ? null : ficha.IdSede);
                    objPpp.ID_SUBPROGRAMA = ficha.IdSubprograma == 0 ? null : ficha.IdSubprograma;
                    objPpp.ID_ONG = ficha.ID_ONG == 0 ? null : ficha.ID_ONG;
                    objPpp.ID_ESCUELA = ficha.IdEscuelaPpp == 0 ? null : ficha.IdEscuelaPpp;
                    objPpp.ID_CURSO = ficha.ID_CURSO == 0 ? null : ficha.ID_CURSO;
                    objPpp.PPP_APRENDIZ = ficha.ppp_aprendiz;
                    objPpp.TIPO_PPP = ficha.TipoPrograma;
                    objPpp.HORARIO_ROTATIVO = ficha.horario_rotativo ?? "N";
                    objPpp.SEDE_ROTATIVA = ficha.sede_rotativa ?? "N";

                    if (ficha.TipoProgramaAnt == 5)
                    {
                        objPpp.N_DESCRIPCION_T = ficha.n_descripcion_t;
                        objPpp.N_CURSADO_INS = ficha.n_cursado_ins;
                        objPpp.EGRESO = ficha.egreso;
                        objPpp.COD_USO_INTERNO = ficha.cod_uso_interno;
                        objPpp.CARGA_HORARIA = ficha.carga_horaria ?? "N";
                        objPpp.CH_CUAL = ficha.ch_cual;
                        objPpp.ID_CARRERA = ficha.id_carrera;
                        objPpp.CONSTANCIA_EGRESO = ficha.CONSTANCIA_EGRESO ?? "N";
                        objPpp.MATRICULA = ficha.MATRICULA ?? "N";
                        objPpp.CONSTANCIA_CURSO = ficha.CONSTANCIA_CURSO ?? "N";
                    }
                    if (ficha.TipoPrograma == 6)
                    {
                        objPpp.N_DESCRIPCION_T = ficha.n_descripcion_t;
                        objPpp.N_CURSADO_INS = ficha.n_cursado_ins;
                        objPpp.COD_USO_INTERNO = ficha.cod_uso_interno;
                        objPpp.ID_CARRERA = ficha.id_carrera;

                    }
                    objPpp.SENAF = ficha.SENAF ?? "N";
                    objPpp.CO_FINANCIAMIENTO = ficha.co_financiamiento == 0 ? null : ficha.co_financiamiento;
                    break;
                case (int)Enums.TipoFicha.Vat:
                    var objVat = _mdb.T_FICHA_VAT.FirstOrDefault(c => c.ID_FICHA == ficha.IdFicha);
                    objVat.CURSANDO = ficha.Cursando ? "S" : "N";
                    objVat.ANIOS_APORTES = ficha.AniosAportes;
                    objVat.FINALIZADO = ficha.Finalizado ? "S" : "N";
                    objVat.ID_NIVEL_ESCOLARIDAD = Convert.ToInt16(ficha.IdNivelEscolaridad);
                    objVat.MODALIDAD = ficha.Modalidad;
                    objVat.ALTA_TEMPRANA = ficha.AltaTemprana == "N" ? null : "S";
                    objVat.TAREAS = ficha.Tareas;
                    objVat.ID_EMPRESA = ficha.IdEmpresa;
                    objVat.ID_USR_SIST = ficha.IdUsuarioSistema;
                    objVat.AT_FECHA_CESE = ficha.FechaFinActividad;
                    objVat.AT_FECHA_INICIO = ficha.FechaInicioActividad;
                    objVat.ID_MOD_CONT_AFIP = ficha.IdModalidadAfip == 0 ? null : ficha.IdModalidadAfip;
                    objVat.ID_SEDE = (ficha.IdSede == 0 ? null : ficha.IdSede);
                    break;
                case (int)Enums.TipoFicha.PppProf:
                    var objPppp = _mdb.T_FICHA_PPP_PROF.FirstOrDefault(c => c.ID_FICHA == ficha.IdFicha);
                    objPppp.MODALIDAD = ficha.Modalidad;
                    objPppp.ALTA_TEMPRANA = ficha.AltaTemprana == "N" ? null : "S";
                    objPppp.TAREAS = ficha.Tareas;
                    objPppp.ID_EMPRESA = ficha.IdEmpresa;
                    objPppp.ID_USR_SIST = ficha.IdUsuarioSistema;
                    objPppp.AT_FECHA_CESE = ficha.FechaFinActividad;
                    objPppp.AT_FECHA_INICIO = ficha.FechaInicioActividad;
                    objPppp.ID_MOD_CONT_AFIP = ficha.IdModalidadAfip == 0 ? null : ficha.IdModalidadAfip;
                    objPppp.PROMEDIO = ficha.Promedio;
                    objPppp.ID_TITULO = ficha.IdTitulo;
                    objPppp.FECHA_EGRESO = ficha.FechaEgreso;
                    objPppp.ID_SEDE = (ficha.IdSede == 0 ? null : ficha.IdSede);
                    break;
                case (int)Enums.TipoFicha.ReconversionProductiva:
                    var objRecProd= _mdb.T_FICHA_REC_PROD.FirstOrDefault(c => c.ID_FICHA == ficha.IdFicha);
                    objRecProd.MODALIDAD = ficha.Modalidad;
                    objRecProd.ALTA_TEMPRANA = ficha.AltaTemprana == "N" ? null : "S";
                    objRecProd.ID_EMPRESA = ficha.IdEmpresa;
                    objRecProd.ID_USR_SIST = ficha.IdUsuarioSistema;
                    objRecProd.AT_FECHA_CESE = ficha.FechaFinActividad;
                    objRecProd.AT_FECHA_INICIO = ficha.FechaInicioActividad;
                    objRecProd.ID_MOD_CONT_AFIP = ficha.IdModalidadAfip == 0 ? null : ficha.IdModalidadAfip;
                    objRecProd.ID_SEDE = (ficha.IdSede == 0 ? null : ficha.IdSede);
                    objRecProd.ID_SUBPROGRAMA = ficha.IdSubprograma == 0 ? null : ficha.IdSubprograma;
                    objRecProd.ID_PROYECTO = ficha.IdProyecto;
                    objRecProd.APORTES_EMP = ficha.AportesDeLaEmpresa;
                    objRecProd.APELLIDO_TUTOR = ficha.ApellidoTutor;
                    objRecProd.NOMBRE_TUTOR = ficha.NombreTutor;
                    objRecProd.DNI_TUTOR = ficha.NroDocumentoTutor;
                    objRecProd.TELEFONO_TUTOR = ficha.TelefonoTutor;
                    objRecProd.EMAIL_TUTOR = ficha.EmailTutor;
                    objRecProd.PUESTO_TUTOR = ficha.PuestoTutor;
                    objRecProd.CURSANDO = ficha.Cursando ? "S" : "N";
                    objRecProd.DESEA_TERM_NIVEL = ficha.DeseaTermNivel ? "S" : "N";
                    objRecProd.FINALIZADO = ficha.Finalizado ? "S" : "N";
                    objRecProd.ID_NIVEL_ESCOLARIDAD = Convert.ToInt16(ficha.IdNivelEscolaridad);
                    break;
                case (int)Enums.TipoFicha.EfectoresSociales:
                    var objEfecSoc = _mdb.T_FICHA_EFEC_SOC.FirstOrDefault(c => c.ID_FICHA == ficha.IdFicha);
                    objEfecSoc.MODALIDAD = ficha.Modalidad;
                    objEfecSoc.ALTA_TEMPRANA = ficha.AltaTemprana == "N" ? null : "S";
                    objEfecSoc.ID_EMPRESA = ficha.IdEmpresa;
                    objEfecSoc.ID_USR_SIST = ficha.IdUsuarioSistema;
                    objEfecSoc.AT_FECHA_CESE = ficha.FechaFinActividad;
                    objEfecSoc.AT_FECHA_INICIO = ficha.FechaInicioActividad;
                    objEfecSoc.ID_MOD_CONT_AFIP = ficha.IdModalidadAfip == 0 ? null : ficha.IdModalidadAfip;
                    objEfecSoc.ID_SEDE = (ficha.IdSede == 0 ? null : ficha.IdSede);
                    objEfecSoc.ID_SUBPROGRAMA = ficha.IdSubprograma == 0 ? null : ficha.IdSubprograma;
                    objEfecSoc.ID_PROYECTO = ficha.IdProyecto;
                    objEfecSoc.APORTES_EMP = ficha.AportesDeLaEmpresa;
                    objEfecSoc.APELLIDO_TUTOR = ficha.ApellidoTutor;
                    objEfecSoc.NOMBRE_TUTOR = ficha.NombreTutor;
                    objEfecSoc.DNI_TUTOR = ficha.NroDocumentoTutor;
                    objEfecSoc.TELEFONO_TUTOR = ficha.TelefonoTutor;
                    objEfecSoc.EMAIL_TUTOR = ficha.EmailTutor;
                    objEfecSoc.PUESTO_TUTOR = ficha.PuestoTutor;
                    objEfecSoc.CURSANDO = ficha.Cursando ? "S" : "N";
                    objEfecSoc.DESEA_TERM_NIVEL = ficha.DeseaTermNivel ? "S" : "N";
                    objEfecSoc.FINALIZADO = ficha.Finalizado ? "S" : "N";
                    objEfecSoc.ID_NIVEL_ESCOLARIDAD = Convert.ToInt16(ficha.IdNivelEscolaridad);
                    objEfecSoc.MONTO_EFE = ficha.monto_efe == 0 ? null : (decimal?)ficha.monto_efe;
                    break;

                case (int)Enums.TipoFicha.ConfiamosEnVos:
                    var objConfVos = _mdb.T_FICHAS_CONF_VOS.FirstOrDefault(c => c.ID_FICHA == ficha.IdFicha);
                    objConfVos.MODALIDAD = ficha.Modalidad;
                    objConfVos.ALTA_TEMPRANA = ficha.AltaTemprana == "N" ? null : "S";
                    objConfVos.ID_EMPRESA = ficha.IdEmpresa;
                    objConfVos.ID_USR_SIST = ficha.IdUsuarioSistema;
                    objConfVos.AT_FECHA_CESE = ficha.FechaFinActividad;
                    objConfVos.AT_FECHA_INICIO = ficha.FechaInicioActividad;
                    objConfVos.ID_MOD_CONT_AFIP = ficha.IdModalidadAfip == 0 ? null : ficha.IdModalidadAfip;
                    objConfVos.ID_SEDE = (ficha.IdSede == 0 ? null : ficha.IdSede);
                    objConfVos.ID_SUBPROGRAMA = ficha.IdSubprograma == 0 ? null : ficha.IdSubprograma;
                    objConfVos.ID_PROYECTO = ficha.IdProyecto;
                    objConfVos.APORTES_EMP = ficha.AportesDeLaEmpresa;
                    objConfVos.APELLIDO_TUTOR = ficha.ApellidoTutor;
                    objConfVos.NOMBRE_TUTOR = ficha.NombreTutor;
                    objConfVos.DNI_TUTOR = ficha.NroDocumentoTutor;
                    objConfVos.TELEFONO_TUTOR = ficha.TelefonoTutor;
                    objConfVos.EMAIL_TUTOR = ficha.EmailTutor;
                    objConfVos.PUESTO_TUTOR = ficha.PuestoTutor;
                    objConfVos.CURSANDO = ficha.Cursando ? "S" : "N";
                    objConfVos.DESEA_TERM_NIVEL = ficha.DeseaTermNivel ? "S" : "N";
                    objConfVos.FINALIZADO = ficha.Finalizado ? "S" : "N";
                    objConfVos.ID_NIVEL_ESCOLARIDAD = Convert.ToInt16(ficha.IdNivelEscolaridad);


                    objConfVos.ABANDONO_ESCUELA	=	(short?)ficha.Abandono_Escuela	;
                    objConfVos.TRABAJA_TRABAJO	=	ficha.	Trabaja_Trabajo	;
                    objConfVos.CURSA	=	ficha.	cursa	;
                    objConfVos.ULTIMO_CURSADO = (short?)ficha.Ultimo_Cursado;
                    objConfVos.PIT	=	ficha.	pit	;
                    objConfVos.CENTRO_ADULTOS	=	ficha.	Centro_Adultos	;
                    objConfVos.PROGRESAR	=	ficha.	Progresar	;
                    objConfVos.ACTIVIDADES = (short?)ficha.Actividades;
                    objConfVos.BENEF_PROGRE	=	ficha.	Benef_progre	;
                    objConfVos.AUTORIZA_TUTOR	=	ficha.	Autoriza_Tutor	;
                    objConfVos.APODERADO	=	ficha.	Apoderado	;
                    objConfVos.APO_CUIL	=	ficha.	Apo_Cuil	;
                    objConfVos.APO_APELLIDO	=	ficha.	Apo_Apellido	;
                    objConfVos.APO_NOMBRE	=	ficha.	Apo_Nombre	;
                    objConfVos.APO_FER_NAC	=	ficha.	Apo_Fer_Nac	;
                    objConfVos.APO_TIPO_DOCUMENTO	=	ficha.	Apo_Tipo_Documento	;
                    objConfVos.APO_NUMERO_DOCUMENTO	=	ficha.	Apo_Numero_Documento	;
                    objConfVos.APO_CALLE	=	ficha.	Apo_Calle	;
                    objConfVos.APO_NUMERO	=	ficha.	Apo_Numero	;
                    objConfVos.APO_PISO	=	ficha.	Apo_Piso	;
                    objConfVos.APO_DPTO	=	ficha.	Apo_Dpto	;
                    objConfVos.APO_MONOBLOCK	=	ficha.	Apo_Monoblock	;
                    objConfVos.APO_PARCELA	=	ficha.	Apo_Parcela	;
                    objConfVos.APO_MANZANA	=	ficha.	Apo_Manzana	;
                    objConfVos.APO_BARRIO	=	ficha.	Apo_Barrio	;
                    objConfVos.APO_CODIGO_POSTAL	=	ficha.	Apo_Codigo_Postal	;
                    objConfVos.APO_ID_LOCALIDAD	=	ficha.	Apo_Id_Localidad	;
                    objConfVos.APO_TELEFONO	=	ficha.	Apo_Telefono	;
                    objConfVos.APO_TIENE_HIJOS	=	ficha.	Apo_Tiene_Hijos	;
                    objConfVos.APO_CANTIDAD_HIJOS	=	ficha.	Apo_Cantidad_Hijos	;
                    objConfVos.APO_SEXO	=	ficha.	Apo_Sexo	;
                    objConfVos.APO_ESTADO_CIVIL	=	(short?)ficha.	Apo_Estado_Civil	;
                    objConfVos.ID_ESCUELA = ficha.IdEscuelaConfVos;
                    objConfVos.ID_CURSO = ficha.ID_CURSO == 0 ? null : ficha.ID_CURSO;
                    objConfVos.ID_ABANDONO_CUR =ficha.ID_ABANDONO_CUR;//(ficha.FichaConfVos.ID_ABANDONO_CUR == 0 ? null : ficha.FichaConfVos.ID_ABANDONO_CUR);
                    objConfVos.TRABAJO_CONDICION = ficha.TRABAJO_CONDICION;
                    objConfVos.SENAF = ficha.SENAF;
                    objConfVos.PERTENECE_ONG = ficha.PERTENECE_ONG;
                    objConfVos.ID_ONG = ficha.ID_ONG;//(ficha.FichaConfVos.ID_ONG == 0 ? null : ficha.FichaConfVos.ID_ONG);
                    objConfVos.APO_CELULAR = ficha.APO_CELULAR;
                    objConfVos.APO_MAIL = ficha.APO_MAIL;
                    objConfVos.ID_FACILITADOR = ficha.ID_FACILITADOR == 0 ? null : ficha.ID_FACILITADOR;
                    objConfVos.ID_GESTOR = ficha.ID_GESTOR == 0 ? null : ficha.ID_GESTOR;
                    break;

                default:
                    break;
            }

            _mdb.SaveChanges();

            return true;
        }

        public bool UpdateEstadoFicha(int idFicha, int idEstadoFicha)
        {
            IComunDatos comun = new ComunDatos();

            AgregarDatos(comun);

            var obj = _mdb.T_FICHAS.FirstOrDefault(c => c.ID_FICHA == idFicha);
            obj.ID_ESTADO_FICHA = idEstadoFicha;
            obj.ID_USR_SIST = comun.IdUsuarioSistema;

            _mdb.SaveChanges();

            return true;
        }

        public IList<IFicha> GetFichas(string cuil, string dni, string nombre, string apellido, int tipoficha, int estadoficha, int idEtapa, string nrotramite, int tipoprograma = 0)
        {
            return
                QFichaSimple(tipoficha).Where( // QFichaSimple().Where(
                    c =>
                    (c.Apellido.ToLower().Contains(apellido.ToLower()) || String.IsNullOrEmpty(apellido.Trim())) &&
                    (c.Nombre.ToLower().Contains(nombre.ToLower()) || String.IsNullOrEmpty(nombre.Trim())) &&
                    (c.IdEstadoFicha == estadoficha || estadoficha == -1) &&
                    (c.Cuil.Contains(cuil) || String.IsNullOrEmpty(cuil.Trim())) &&
                    (c.NumeroDocumento.Contains(dni) || String.IsNullOrEmpty(dni.Trim())) &&
                    (c.TipoFicha == tipoficha || tipoficha == 0) &&
                    (c.NroTramite.Contains(nrotramite) || String.IsNullOrEmpty(nrotramite.Trim())) &&
                    (c.idEtapa == idEtapa || idEtapa == 0) &&// 14/06/2013 - DI CAMPLI LEANDRO - SE AÑADE FILTRO DE ETAPAS PARA INSCRIPCIONES
                    (c.TipoPrograma == tipoprograma || tipoprograma == 0) // 14/06/2013 - DI CAMPLI LEANDRO - SE AÑADE FILTRO DE TIPO PPP PARA ACOTAR RESULTADOS
                    ).OrderBy(c => c.Apellido.ToUpper().Trim())
                    .ThenBy(o => o.Nombre.ToUpper().Trim()).ToList();




        }

        public IList<IFicha> GetFichas(string cuil, string dni, string nombre, string apellido, int tipoficha, int estadoficha,
            int skip, int take, int idEtapa)
        {
            return
                QFichaSimple(tipoficha).Where( // QFichaSimple().Where(
                    c =>
                    (c.Apellido.ToLower().Contains(apellido.ToLower()) || String.IsNullOrEmpty(apellido.Trim())) &&
                    (c.Nombre.ToLower().Contains(nombre.ToLower()) || String.IsNullOrEmpty(nombre.Trim())) &&
                    (c.IdEstadoFicha == estadoficha || estadoficha == -1) &&
                    (c.Cuil.Contains(cuil) || String.IsNullOrEmpty(cuil.Trim())) &&
                    (c.NumeroDocumento.Contains(dni) || String.IsNullOrEmpty(dni.Trim())) &&
                    (c.TipoFicha == tipoficha || tipoficha == 0)  &&
                    (c.idEtapa == idEtapa || idEtapa == 0) // 14/06/2013 - DI CAMPLI LEANDRO - SE AÑADE FILTRO DE ETAPAS PARA INSCRIPCIONES
                    ).OrderBy(c => c.Apellido.ToUpper().Trim()).ThenBy(o => o.Nombre.ToUpper().Trim())
                    .Skip(skip).Take(take).ToList();
        }

        public IList<IFicha> GetFichas(string cuil, string dni, string nombre, string apellido, int tipoficha, int estadoficha, int skip, int take, int excluirtipoficha, int idEtapa)
        {
            return
                QFichaSimple().Where(
                    c =>
                    (c.Apellido.ToLower().Contains(apellido.ToLower()) || String.IsNullOrEmpty(apellido.Trim())) &&
                    (c.Nombre.ToLower().Contains(nombre.ToLower()) || String.IsNullOrEmpty(nombre.Trim())) &&
                    (c.IdEstadoFicha == estadoficha || estadoficha == -1) &&
                    (c.Cuil.Contains(cuil) || String.IsNullOrEmpty(cuil.Trim())) &&
                    (c.NumeroDocumento.Contains(dni) || String.IsNullOrEmpty(dni.Trim())) &&
                    (c.TipoFicha == tipoficha || tipoficha == 0) && (c.TipoFicha != excluirtipoficha) &&
                    (c.idEtapa == idEtapa || idEtapa == 0) // 14/06/2013 - DI CAMPLI LEANDRO - SE AÑADE FILTRO DE ETAPAS PARA INSCRIPCIONES
                    ).OrderBy(c => c.Apellido.ToUpper().Trim()).ThenBy(o => o.Nombre.ToUpper().Trim())
                    .Skip(skip).Take(take).ToList();
        }

        public bool ConvertirFicha(int idFicha)
        {
            IComunDatos comun = new ComunDatos();
            int idTipoFicha = 0;
            int idPrograma = 0;
            AgregarDatos(comun);

            //var ficha = _mdb.T_FICHAS.FirstOrDefault(c => c.ID_FICHA == idFicha);

            _mdb.Connection.Open();

            var trans = _mdb.Connection.BeginTransaction();

            var ficha = _mdb.T_FICHAS.FirstOrDefault(c => c.ID_FICHA == idFicha);

            switch (ficha.TIPO_FICHA)
            {
                case (int)Enums.TipoFicha.Terciaria:
                    var objTerciario = _mdb.T_FICHA_TERCIARIO.FirstOrDefault(c => c.ID_FICHA == idFicha);

                    var objConvertirUniversitario = new T_FICHA_UNIVERSITARIO
                    {
                        //22/01/2013 - Di Campli Leandro
                        //BLANQUEA LA CARRERA QUE PUEDE PERTENECE A TERCIARIO
                        //ID_CARRERA = objTerciario.ID_CARRERA,
                        ID_CARRERA = null,

                        ID_ESCUELA = objTerciario.ID_ESCUELA,
                        PROMEDIO = objTerciario.PROMEDIO,
                        OTRA_CARRERA = "",//objTerciario.OTRA_CARRERA,
                        OTRA_INSTITUCION = "",//objTerciario.OTRA_INSTITUCION,
                        OTRO_SECTOR = "",//objTerciario.OTRO_SECTOR,
                        ID_FICHA = idFicha,
                        ID_USR_SIST = comun.IdUsuarioSistema
                    };

                    _mdb.T_FICHA_TERCIARIO.DeleteObject(objTerciario);
                    _mdb.T_FICHA_UNIVERSITARIO.AddObject(objConvertirUniversitario);

                    /*ficha.TIPO_FICHA*/ 
                    idTipoFicha = (int)Enums.TipoFicha.Universitaria;
                    idPrograma = (int)Enums.Programas.Universitaria;
                    

                    break;
                case (int)Enums.TipoFicha.Universitaria:
                    var objUniversitario = _mdb.T_FICHA_UNIVERSITARIO.FirstOrDefault(c => c.ID_FICHA == idFicha);

                    var objConvertirTerciario = new T_FICHA_TERCIARIO
                    {   //BLANQUEA LA CARRERA QUE PUEDE PERTENECE A UNIVERSITARIO
                        ID_CARRERA = null,//objUniversitario.ID_CARRERA,
                        ID_ESCUELA = objUniversitario.ID_ESCUELA,
                        PROMEDIO = objUniversitario.PROMEDIO ?? 0,
                        OTRA_CARRERA = "",//objUniversitario.OTRA_CARRERA,
                        OTRA_INSTITUCION = "",//objUniversitario.OTRA_INSTITUCION,
                        OTRO_SECTOR = "",//objUniversitario.OTRO_SECTOR,
                        ID_FICHA = idFicha,
                        ID_USR_SIST = comun.IdUsuarioSistema
                    };

                    _mdb.T_FICHA_UNIVERSITARIO.DeleteObject(objUniversitario);
                    _mdb.T_FICHA_TERCIARIO.AddObject(objConvertirTerciario);

                    /*ficha.TIPO_FICHA*/
                    idTipoFicha = (int)Enums.TipoFicha.Terciaria;
                    idPrograma = (int)Enums.Programas.Terciaria;
                    break;
                default:
                    break;
            }

            //22/01/2013 Se actualiza el TIPO_FICHA EN T_FICHAS - DI CAMPLI LEANDRO

            ficha.TIPO_FICHA = (short)idTipoFicha;
            ficha.ID_USR_SIST = comun.IdUsuarioSistema;
            ficha.FEC_SIST = comun.FechaSistema;

            // 27/02/2013 - LEANDRO DI CAMPLI - VERIFICAR SI EXISTE REGISTRO DE BENEFICIARIO
            int ben = _mdb.T_BENEFICIARIOS.Where(c => c.ID_FICHA == idFicha).Count();
            if(ben > 0)
            {
            //22/01/2013 Se actualiza el ID_PROGRAMA EN T_BENEFICIARIOS - DI CAMPLI LEANDRO
            var beneficiario = _mdb.T_BENEFICIARIOS.FirstOrDefault(c => c.ID_FICHA == idFicha);
            beneficiario.ID_PROGRAMA = (short)idPrograma;
            }

            _mdb.SaveChanges();
            trans.Commit();

            return true;
        }

        public IFichaPPP GetFichaPpp(int idficha)
        {
            var fichappp = _mdb.T_FICHA_PPP.Where(c => c.ID_FICHA == idficha).Select(
                c =>
                new FichaPPP
                    {
                        CuitEmpresa = c.T_EMPRESAS.CUIT ?? "",
                        Cursando = (c.CURSANDO == "S" ? true : false),
                        DescripcionEmpresa = c.T_EMPRESAS.N_EMPRESA,
                        DeseaTermNivel = (c.DESEA_TERM_NIVEL == "S" ? true : false),
                        Finalizado = (c.FINALIZADO == "S" ? true : false),
                        IdEmpresa = c.ID_EMPRESA,
                        IdNivelEscolaridad = c.ID_NIVEL_ESCOLARIDAD,
                        IdSede = c.ID_SEDE,
                        Modalidad = c.MODALIDAD ?? 0,
                        Tareas = c.TAREAS,
                        AltaTemprana = c.ALTA_TEMPRANA == null ? "N" : c.ALTA_TEMPRANA,
                        FechaFinActividad = c.AT_FECHA_CESE,
                        FechaInicioActividad = c.AT_FECHA_INICIO,
                        IdModalidadAfip = c.ID_MOD_CONT_AFIP,
                        ModalidadAfip = c.T_MOD_CONT_AFIP.N_MOD_CONT_AFIP,
                        ppp_aprendiz = c.PPP_APRENDIZ == null ? "N" : c.PPP_APRENDIZ,
                        IdSubprograma = c.ID_SUBPROGRAMA,
                        ID_ONG = c.ID_ONG ?? 0,
                        Id_Escuela = c.ID_ESCUELA ?? 0,
                        ID_CURSO = c.ID_CURSO ?? 0,
                        TIPO_PPP = c.PPP_APRENDIZ == "S" ? 2 : (c.TIPO_PPP ?? 1), //Esto es porque antes no se definio un tipo de ppp y se diferenciaba con un campo PPP de PPP aprendiz
                        sede_rotativa = c.SEDE_ROTATIVA == null ? "N" : c.SEDE_ROTATIVA,
                        horario_rotativo = c.HORARIO_ROTATIVO == null ? "N" : c.HORARIO_ROTATIVO,
                        n_descripcion_t = c.N_DESCRIPCION_T,
                        n_cursado_ins = c.N_CURSADO_INS,
                        egreso = c.EGRESO,
                        cod_uso_interno = c.COD_USO_INTERNO,
                        carga_horaria = c.CARGA_HORARIA == null ? "N" : c.CARGA_HORARIA,
                        ch_cual = c.CH_CUAL,
                        id_carrera = c.ID_CARRERA ?? 0,
                        CONSTANCIA_EGRESO = c.CONSTANCIA_EGRESO ?? "N",
                        MATRICULA = c.MATRICULA ?? "N",
                        CONSTANCIA_CURSO = c.CONSTANCIA_CURSO ?? "N",
                        co_financiamiento = c.CO_FINANCIAMIENTO,
                        SENAF = c.SENAF ?? "N",
                        Empresa =
                            new Empresa
                                {
                                    NombreEmpresa = c.T_EMPRESAS.N_EMPRESA,
                                    Calle = c.T_EMPRESAS.CALLE,
                                    CantidadEmpleados = c.T_EMPRESAS.CANTIDAD_EMPLEADOS,
                                    NombreLocalidad = c.T_EMPRESAS.T_LOCALIDADES.N_LOCALIDAD,
                                    IdLocalidad = c.T_EMPRESAS.ID_LOCALIDAD,
                                    Piso = c.T_EMPRESAS.PISO,
                                    Numero = c.T_EMPRESAS.NUMERO,
                                    CodigoActividad = c.T_EMPRESAS.CODIGO_ACTIVIDAD,
                                    Cuit = c.T_EMPRESAS.CUIT,
                                    IdUsuario = c.T_EMPRESAS.ID_USUARIO,
                                    Dpto = c.T_EMPRESAS.DPTO,
                                    CodigoPostal = c.T_EMPRESAS.CODIGO_POSTAL,
                                    DomicilioLaboralIdem = (c.T_EMPRESAS.DOMICLIO_LABORAL_IDEM == null ||
                                    c.T_EMPRESAS.DOMICLIO_LABORAL_IDEM == "N") ? "No" : "Si",
                                    celular = c.T_EMPRESAS.CELULAR,
                                    mailEmp = c.T_EMPRESAS.MAIL,
                                    verificada = c.T_EMPRESAS.VERIFICADA
                                },

                        Sede =
                            new Sede
                                {
                                    ApellidoContacto = c.T_SEDES.APE_CONTACTO,
                                    Calle = c.T_SEDES.CALLE,
                                    CodigoPostal = c.T_SEDES.CODIGO_POSTAL,
                                    Dpto = c.T_SEDES.DPTO,
                                    Email = c.T_SEDES.EMAIL,
                                    Fax = c.T_SEDES.FAX,
                                    NombreSede = c.T_SEDES.N_SEDE,
                                    NombreContacto = c.T_SEDES.NOM_CONTACTO,
                                    Numero = c.T_SEDES.NUMERO,
                                    Piso = c.T_SEDES.PISO,
                                    Telefono = c.T_SEDES.TELEFONO,
                                    NombreLocalidad = c.T_SEDES.T_LOCALIDADES.N_LOCALIDAD
                                }


                    }).SingleOrDefault();

            if (fichappp != null)
            {
                if (fichappp.Empresa != null)
                {
                    IUsuarioEmpresa usuario =
                        _mdb.T_USUARIOS_EMPRESA.Where(usu => usu.ID_USUARIO == fichappp.Empresa.IdUsuario).
                            Select(
                                tue =>
                                new UsuarioEmpresa
                                    {
                                        ApellidoUsuario = tue.APELLIDO,
                                        NombreUsuario = tue.NOMBRE,
                                        Cuil = tue.CUIL,
                                        Telefono = tue.TELEFONO,
                                        Mail = tue.MAIL,
                                        LoginUsuario = tue.LOGIN,
                                    }).SingleOrDefault();

                    fichappp.Empresa.Usuario = usuario;
                }
            }

            return fichappp;
        }

        public IFichaVAT GetFichaVat(int idficha)
        {
            var fichavat = _mdb.T_FICHA_VAT.Where(c => c.ID_FICHA == idficha).Select(
                c =>
                new FichaVAT
                {
                    CuitEmpresa = c.T_EMPRESAS.CUIT ?? "",
                    Cursando = (c.CURSANDO == "S" ? true : false),
                    DescripcionEmpresa = c.T_EMPRESAS.N_EMPRESA,
                    Finalizado = (c.FINALIZADO == "S" ? true : false),
                    IdEmpresa = c.ID_EMPRESA,
                    IdNivelEscolaridad = c.ID_NIVEL_ESCOLARIDAD,
                    IdSede = c.ID_SEDE,
                    Modalidad = c.MODALIDAD ?? 0,
                    Tareas = c.TAREAS,
                    AltaTemprana = c.ALTA_TEMPRANA == null ? "N" : "S",
                    FechaFinActividad = c.AT_FECHA_CESE,
                    FechaInicioActividad = c.AT_FECHA_INICIO,
                    IdModalidadAFIP = c.ID_MOD_CONT_AFIP,
                    ModalidadAFIP = c.T_MOD_CONT_AFIP.N_MOD_CONT_AFIP,
                    AniosAportes = c.ANIOS_APORTES,
                    Empresa =
                        new Empresa
                        {
                            NombreEmpresa = c.T_EMPRESAS.N_EMPRESA,
                            Calle = c.T_EMPRESAS.CALLE,
                            CantidadEmpleados = c.T_EMPRESAS.CANTIDAD_EMPLEADOS,
                            NombreLocalidad = c.T_EMPRESAS.T_LOCALIDADES.N_LOCALIDAD,
                            IdLocalidad = c.T_EMPRESAS.ID_LOCALIDAD,
                            Piso = c.T_EMPRESAS.PISO,
                            Numero = c.T_EMPRESAS.NUMERO,
                            CodigoActividad = c.T_EMPRESAS.CODIGO_ACTIVIDAD,
                            Cuit = c.T_EMPRESAS.CUIT,
                            IdUsuario = c.T_EMPRESAS.ID_USUARIO,
                            Dpto = c.T_EMPRESAS.DPTO,
                            CodigoPostal = c.T_EMPRESAS.CODIGO_POSTAL,
                            DomicilioLaboralIdem = (c.T_EMPRESAS.DOMICLIO_LABORAL_IDEM == null ||
                            c.T_EMPRESAS.DOMICLIO_LABORAL_IDEM == "N") ? "No" : "Si"
                        },

                    Sede =
                        new Sede
                        {
                            ApellidoContacto = c.T_SEDES.APE_CONTACTO,
                            Calle = c.T_SEDES.CALLE,
                            CodigoPostal = c.T_SEDES.CODIGO_POSTAL,
                            Dpto = c.T_SEDES.DPTO,
                            Email = c.T_SEDES.EMAIL,
                            Fax = c.T_SEDES.FAX,
                            NombreSede = c.T_SEDES.N_SEDE,
                            NombreContacto = c.T_SEDES.NOM_CONTACTO,
                            Numero = c.T_SEDES.NUMERO,
                            Piso = c.T_SEDES.PISO,
                            Telefono = c.T_SEDES.TELEFONO,
                            NombreLocalidad = c.T_SEDES.T_LOCALIDADES.N_LOCALIDAD
                        }


                }).SingleOrDefault();

            if (fichavat != null)
            {
                if (fichavat.Empresa != null)
                {
                    IUsuarioEmpresa usuario =
                        _mdb.T_USUARIOS_EMPRESA.Where(usu => usu.ID_USUARIO == fichavat.Empresa.IdUsuario).
                            Select(
                                tue =>
                                new UsuarioEmpresa
                                {
                                    ApellidoUsuario = tue.APELLIDO,
                                    NombreUsuario = tue.NOMBRE,
                                    Cuil = tue.CUIL,
                                    Telefono = tue.TELEFONO,
                                    Mail = tue.MAIL,
                                    LoginUsuario = tue.LOGIN,
                                }).SingleOrDefault();

                    fichavat.Empresa.Usuario = usuario;
                }
            }

            return fichavat;
        }

        public IFichaPPPP GetFichaPppp(int idficha)
        {
            var fichapppp = _mdb.T_FICHA_PPP_PROF.Where(c => c.ID_FICHA == idficha).Select(
                c =>
                new FichaPPPP
                {
                    Promedio = c.PROMEDIO,
                    IdTitulo = c.ID_TITULO,
                    FechaEgreso = c.FECHA_EGRESO,
                    CuitEmpresa = c.T_EMPRESAS.CUIT ?? "",
                    DescripcionEmpresa = c.T_EMPRESAS.N_EMPRESA,
                    IdEmpresa = c.ID_EMPRESA,
                    IdSede = c.ID_SEDE,
                    Modalidad = c.MODALIDAD ?? 0,
                    Tareas = c.TAREAS,
                    AltaTemprana = c.ALTA_TEMPRANA == null ? "N" : "S",
                    FechaFinActividad = c.AT_FECHA_CESE,
                    FechaInicioActividad = c.AT_FECHA_INICIO,
                    IdModalidadAFIP = c.ID_MOD_CONT_AFIP,
                    ModalidadAFIP = c.T_MOD_CONT_AFIP.N_MOD_CONT_AFIP,
                    Empresa =
                        new Empresa
                        {
                            NombreEmpresa = c.T_EMPRESAS.N_EMPRESA,
                            Calle = c.T_EMPRESAS.CALLE,
                            CantidadEmpleados = c.T_EMPRESAS.CANTIDAD_EMPLEADOS,
                            NombreLocalidad = c.T_EMPRESAS.T_LOCALIDADES.N_LOCALIDAD,
                            IdLocalidad = c.T_EMPRESAS.ID_LOCALIDAD,
                            Piso = c.T_EMPRESAS.PISO,
                            Numero = c.T_EMPRESAS.NUMERO,
                            CodigoActividad = c.T_EMPRESAS.CODIGO_ACTIVIDAD,
                            Cuit = c.T_EMPRESAS.CUIT,
                            IdUsuario = c.T_EMPRESAS.ID_USUARIO,
                            Dpto = c.T_EMPRESAS.DPTO,
                            CodigoPostal = c.T_EMPRESAS.CODIGO_POSTAL,
                            DomicilioLaboralIdem = (c.T_EMPRESAS.DOMICLIO_LABORAL_IDEM == null ||
                            c.T_EMPRESAS.DOMICLIO_LABORAL_IDEM == "N") ? "NO" : "SI",
                            celular = c.T_EMPRESAS.CELULAR,
                            mailEmp = c.T_EMPRESAS.MAIL,
                            verificada = c.T_EMPRESAS.VERIFICADA
                        },

                    Sede =
                        new Sede
                        {
                            ApellidoContacto = c.T_SEDES.APE_CONTACTO,
                            Calle = c.T_SEDES.CALLE,
                            CodigoPostal = c.T_SEDES.CODIGO_POSTAL,
                            Dpto = c.T_SEDES.DPTO,
                            Email = c.T_SEDES.EMAIL,
                            Fax = c.T_SEDES.FAX,
                            NombreSede = c.T_SEDES.N_SEDE,
                            NombreContacto = c.T_SEDES.NOM_CONTACTO,
                            Numero = c.T_SEDES.NUMERO,
                            Piso = c.T_SEDES.PISO,
                            Telefono = c.T_SEDES.TELEFONO,
                            NombreLocalidad = c.T_SEDES.T_LOCALIDADES.N_LOCALIDAD
                        }

                }).SingleOrDefault();

            if (fichapppp != null)
            {
                if (fichapppp.Empresa != null)
                {
                    IUsuarioEmpresa usuario =
                        _mdb.T_USUARIOS_EMPRESA.Where(usu => usu.ID_USUARIO == fichapppp.Empresa.IdUsuario).
                            Select(
                                tue =>
                                new UsuarioEmpresa
                                {
                                    ApellidoUsuario = tue.APELLIDO,
                                    NombreUsuario = tue.NOMBRE,
                                    Cuil = tue.CUIL,
                                    Telefono = tue.TELEFONO,
                                    Mail = tue.MAIL,
                                    LoginUsuario = tue.LOGIN,
                                }).SingleOrDefault();

                    fichapppp.Empresa.Usuario = usuario;
                }
            }

            return fichapppp;
        }

        public IFichaTer GetFichaTer(int idficha)
        {
            var fichater = _mdb.T_FICHA_TERCIARIO.Where(c => c.ID_FICHA == idficha).Select(
                c =>
                new FichaTer
                {
                    IdFicha = c.ID_FICHA,
                    IdCarreraTerciaria = c.ID_CARRERA ?? 0,
                    IdInstitucionTerciaria = c.T_CARRERAS.T_SECTOR_INSTITUCION.ID_INSTITUCION,
                    IdSectorTerciaria = c.T_CARRERAS.ID_SECTOR ?? 0,
                    IdEscuelaTerciaria = c.ID_ESCUELA ?? 0,
                    PromedioTerciaria = c.PROMEDIO,
                    OtraCarreraTerciaria = c.OTRA_CARRERA,
                    OtraInstitucionTerciaria = c.OTRA_INSTITUCION,
                    OtroSectorTerciaria = c.OTRO_SECTOR,
                    IdDepartamentoEscTerc = c.T_ESCUELAS.T_LOCALIDADES.ID_DEPARTAMENTO ?? 0,
                    IdLocalidadEscTerc = c.T_ESCUELAS.ID_LOCALIDAD ?? 0,
                    IdSubprograma = c.ID_SUBPROGRAMA,

                }).SingleOrDefault();



            return fichater;
        }

        public IFichaUni GetFichaUni(int idficha)
        {
            var fichauni = _mdb.T_FICHA_UNIVERSITARIO.Where(c => c.ID_FICHA == idficha).Select(
                c =>
                new FichaUni
                {
                    IdFicha = c.ID_FICHA,
                    IdCarreraUniversitaria = c.ID_CARRERA ?? 0,
                    IdInstitucionUniversitaria = c.T_CARRERAS.T_SECTOR_INSTITUCION.ID_INSTITUCION,
                    IdSectorUniversitaria = c.T_CARRERAS.ID_SECTOR ?? 0,
                    IdEscuelaUniversitaria = c.ID_ESCUELA ?? 0,
                    PromedioUniversitaria = c.PROMEDIO,
                    OtraCarreraUniversitaria = c.OTRA_CARRERA,
                    OtraInstitucionUniversitaria = c.OTRA_INSTITUCION,
                    OtroSectorUniversitaria = c.OTRO_SECTOR,
                    IdDepartamentoEscUniv = c.T_ESCUELAS.T_LOCALIDADES.ID_DEPARTAMENTO ?? 0,
                    IdLocalidadEscUniv = c.T_ESCUELAS.ID_LOCALIDAD ?? 0,
                    IdSubprograma = c.ID_SUBPROGRAMA,

                }).SingleOrDefault();



            return fichauni;
        }

        public IList<IFicha> GetFichaByNumeroDocumento(string numeroDocumento)
        {
            return QFichaSimple().Where(c => c.NumeroDocumento == numeroDocumento.Trim() && c.IdEstadoFicha == (int)Enums.EstadoFicha.Beneficiario).ToList();
        }

        public bool UpdateFichaSimple(IFicha ficha)
        {
            AgregarDatos(ficha);

            var obj = _mdb.T_FICHAS.FirstOrDefault(c => c.ID_FICHA == ficha.IdFicha);
            obj.APELLIDO = ficha.Apellido;
            obj.NOMBRE = ficha.Nombre;
            obj.ID_USR_SIST = ficha.IdUsuarioSistema;

            _mdb.SaveChanges();

            return true;
        }

        public IList<IFicha> GetFichasByEmpresa(int? idEmpresa)
        {
            idEmpresa = idEmpresa.ToString() == "" ? null : idEmpresa;

            //var ppp = (from c in QFicha()
            //         join f in _mdb.T_FICHA_PPP on c.IdFicha equals f.ID_FICHA
            //         where f.ID_EMPRESA == idEmpresa
            //         select c).OrderBy(o => o.IdEstadoFicha).ThenBy(o => o.Apellido).ThenBy(o => o.Nombre);

            //var pppp = (from c in QFicha()
            //         join f in _mdb.T_FICHA_PPP_PROF on c.IdFicha equals f.ID_FICHA
            //         where f.ID_EMPRESA == idEmpresa
            //         select c).OrderBy(o => o.IdEstadoFicha).ThenBy(o => o.Apellido).ThenBy(o => o.Nombre);

            //var vat = (from c in QFicha()
            //                join f in _mdb.T_FICHA_VAT on c.IdFicha equals f.ID_FICHA
            //                where f.ID_EMPRESA == idEmpresa
            //                select c).OrderBy(o => o.IdEstadoFicha).ThenBy(o => o.Apellido).ThenBy(o => o.Nombre);

            //var reco_prof = (from c in QFicha()
            //                join f in _mdb.T_FICHA_REC_PROD on c.IdFicha equals f.ID_FICHA
            //                where f.ID_EMPRESA == idEmpresa
            //                select c).OrderBy(o => o.IdEstadoFicha).ThenBy(o => o.Apellido).ThenBy(o => o.Nombre);

            //var t = ppp.Union(pppp.Union(vat.Union(reco_prof)));
            //return t.OrderBy(c => c.NombreTipoFicha).ThenBy(c => c.idEtapa).ThenBy(c => c.IdEstadoFicha).ToList();


            var ppp = (from c in QFichaEmpresa()
                       join f in _mdb.T_FICHA_PPP on c.IdFicha equals f.ID_FICHA
                       where f.ID_EMPRESA == idEmpresa
                       select new Ficha
                         {
                             Apellido = c.Apellido,
                             Barrio = c.Barrio,
                             Localidad = c.Localidad,
                             IdLocalidad = c.IdLocalidad,
                             IdFicha = c.IdFicha,
                             Calle = c.Calle,
                             CantidadHijos = c.CantidadHijos,
                             CertificadoDiscapacidad = c.CertificadoDiscapacidad,
                             CodigoPostal = c.CodigoPostal,
                             CodigoSeguridad = c.CodigoSeguridad,
                             Contacto = c.Contacto,
                             Cuil = c.Cuil,
                             DeficienciaOtra = c.DeficienciaOtra,
                             TieneDeficienciaMental = c.TieneDeficienciaMental,
                             TieneDeficienciaMotora = c.TieneDeficienciaMotora,
                             TieneDeficienciaPsicologia = c.TieneDeficienciaPsicologia,
                             TieneDeficienciaSensorial = c.TieneDeficienciaSensorial,
                             Dpto = c.Dpto,
                             EntreCalles = c.EntreCalles,
                             EsDiscapacitado = c.EsDiscapacitado,
                             EstadoCivil = c.EstadoCivil,
                             FechaNacimiento = c.FechaNacimiento,
                             Mail = c.Mail,
                             Manzana = c.Manzana,
                             Monoblock = c.Monoblock,
                             Nombre = c.Nombre,
                             Numero = c.Numero,
                             NumeroDocumento = c.NumeroDocumento,
                             Parcela = c.Parcela,
                             Piso = c.Piso,
                             Sexo = c.Sexo,
                             TelefonoCelular = c.TelefonoCelular,
                             TelefonoFijo = c.TelefonoFijo,
                             TieneHijos = c.TieneHijos,
                             TipoDocumento = c.TipoDocumento,
                             TipoFicha = c.TipoFicha,
                             NombreTipoFicha = f.TIPO_PPP == 5 ? "PIP" : (f.TIPO_PPP == 4 ? "PILA" : (f.TIPO_PPP == 3 ? "XMÍ" : (f.TIPO_PPP == 2 ? "APRENDIZ" : "PRIMER PASO"))),//c.NombreTipoFicha,
                             IdEstadoFicha = c.IdEstadoFicha,
                             NombreEstadoFicha = c.NombreEstadoFicha,
                             FechaSistema = c.FechaSistema,
                             IdUsuarioSistema = c.IdUsuarioSistema,
                             UsuarioSistema = c.UsuarioSistema,
                             idEtapa = c.idEtapa ?? 0,
                             NombreEtapa = c.NombreEtapa,

                             BeneficiarioFicha = new Beneficiario
                             {
                                 IdBeneficiario = c.BeneficiarioFicha.IdBeneficiario,
                                 IdEstado = c.BeneficiarioFicha.IdEstado,
                                 NombreEstado = c.BeneficiarioFicha.NombreEstado
                             }
                         }).ToList();

            var pppp = (from c in QFichaEmpresa()
                        join f in _mdb.T_FICHA_PPP_PROF on c.IdFicha equals f.ID_FICHA
                        where f.ID_EMPRESA == idEmpresa
                        select c).ToList();

            var vat = (from c in QFichaEmpresa()
                       join f in _mdb.T_FICHA_VAT on c.IdFicha equals f.ID_FICHA
                       where f.ID_EMPRESA == idEmpresa
                       select c).ToList();

            var reco_prof = (from c in QFichaEmpresa()
                             join f in _mdb.T_FICHA_REC_PROD on c.IdFicha equals f.ID_FICHA
                             where f.ID_EMPRESA == idEmpresa
                             select c).ToList();

            var t = ppp.Union(pppp.Union(vat.Union(reco_prof)));
            //return t.OrderBy(c => c.NombreTipoFicha).ThenBy(c => c.idEtapa).ThenBy(c => c.IdEstadoFicha).ToList();
            return t.OrderByDescending(c => c.idEtapa).ThenBy(c => c.NombreEstadoFicha).ThenBy(c => c.BeneficiarioFicha.NombreEstado ?? "").ToList();
        }

        public IList<IFicha> GetFichasByEmpresa(int? idEmpresa, int skip, int take)
        {
            idEmpresa = idEmpresa.ToString() == "" ? null : idEmpresa;

            //var a = (from c in QFicha()
            //         join f in _mdb.T_FICHA_PPP on c.IdFicha equals f.ID_FICHA
            //         where f.ID_EMPRESA == idEmpresa
            //         select c).OrderBy(o => o.IdEstadoFicha).ThenBy(o => o.Apellido).ThenBy(o => o.Nombre).Skip(skip).Take(take).ToList();

            //return a;
            //var ppp = (from c in QFicha()
            //           join f in _mdb.T_FICHA_PPP on c.IdFicha equals f.ID_FICHA
            //           where f.ID_EMPRESA == idEmpresa
            //           select c).OrderBy(o => o.IdEstadoFicha).ThenBy(o => o.Apellido).ThenBy(o => o.Nombre).Skip(skip).Take(take).ToList();

            //var pppp = (from c in QFicha()
            //            join f in _mdb.T_FICHA_PPP_PROF on c.IdFicha equals f.ID_FICHA
            //            where f.ID_EMPRESA == idEmpresa
            //            select c).OrderBy(o => o.IdEstadoFicha).ThenBy(o => o.Apellido).ThenBy(o => o.Nombre).Skip(skip).Take(take).ToList();

            //var vat = (from c in QFicha()
            //           join f in _mdb.T_FICHA_VAT on c.IdFicha equals f.ID_FICHA
            //           where f.ID_EMPRESA == idEmpresa
            //           select c).OrderBy(o => o.IdEstadoFicha).ThenBy(o => o.Apellido).ThenBy(o => o.Nombre).Skip(skip).Take(take).ToList();

            //var reco_prof = (from c in QFicha()
            //                 join f in _mdb.T_FICHA_REC_PROD on c.IdFicha equals f.ID_FICHA
            //                 where f.ID_EMPRESA == idEmpresa
            //                 select c).OrderBy(o => o.IdEstadoFicha).ThenBy(o => o.Apellido).ThenBy(o => o.Nombre).Skip(skip).Take(take).ToList();
            var ppp = (from c in QFichaEmpresa()
                       join f in _mdb.T_FICHA_PPP on c.IdFicha equals f.ID_FICHA
                       where f.ID_EMPRESA == idEmpresa
                       select new Ficha
                         {
                             Apellido = c.Apellido,
                             Barrio = c.Barrio,
                             Localidad = c.Localidad,
                             IdLocalidad = c.IdLocalidad,
                             IdFicha = c.IdFicha,
                             Calle = c.Calle,
                             CantidadHijos = c.CantidadHijos,
                             CertificadoDiscapacidad = c.CertificadoDiscapacidad,
                             CodigoPostal = c.CodigoPostal,
                             CodigoSeguridad = c.CodigoSeguridad,
                             Contacto = c.Contacto,
                             Cuil = c.Cuil,
                             DeficienciaOtra = c.DeficienciaOtra,
                             TieneDeficienciaMental = c.TieneDeficienciaMental,
                             TieneDeficienciaMotora = c.TieneDeficienciaMotora,
                             TieneDeficienciaPsicologia = c.TieneDeficienciaPsicologia,
                             TieneDeficienciaSensorial = c.TieneDeficienciaSensorial,
                             Dpto = c.Dpto,
                             EntreCalles = c.EntreCalles,
                             EsDiscapacitado = c.EsDiscapacitado,
                             EstadoCivil = c.EstadoCivil,
                             FechaNacimiento = c.FechaNacimiento,
                             Mail = c.Mail,
                             Manzana = c.Manzana,
                             Monoblock = c.Monoblock,
                             Nombre = c.Nombre,
                             Numero = c.Numero,
                             NumeroDocumento = c.NumeroDocumento,
                             Parcela = c.Parcela,
                             Piso = c.Piso,
                             Sexo = c.Sexo,
                             TelefonoCelular = c.TelefonoCelular,
                             TelefonoFijo = c.TelefonoFijo,
                             TieneHijos = c.TieneHijos,
                             TipoDocumento = c.TipoDocumento,
                             TipoFicha = c.TipoFicha,
                             NombreTipoFicha = f.TIPO_PPP == 4 ? "PILA" : f.TIPO_PPP == 3 ? "XMÍ" : f.TIPO_PPP == 2 ? "APRENDIZ" : "PRIMER PASO",//c.NombreTipoFicha,
                             IdEstadoFicha = c.IdEstadoFicha,
                             NombreEstadoFicha = c.NombreEstadoFicha,
                             FechaSistema = c.FechaSistema,
                             IdUsuarioSistema = c.IdUsuarioSistema,
                             UsuarioSistema = c.UsuarioSistema,
                             idEtapa = c.idEtapa ?? 0,
                             NombreEtapa = c.NombreEtapa,

                             BeneficiarioFicha = new Beneficiario
                             {
                                 IdBeneficiario = c.BeneficiarioFicha.IdBeneficiario,
                                 IdEstado = c.BeneficiarioFicha.IdEstado,
                                 NombreEstado = c.BeneficiarioFicha.NombreEstado
                             }
                         }).Skip(skip).Take(take).ToList();

            var pppp = (from c in QFicha()
                        join f in _mdb.T_FICHA_PPP_PROF on c.IdFicha equals f.ID_FICHA
                        where f.ID_EMPRESA == idEmpresa
                        select c).Skip(skip).Take(take).ToList();

            var vat = (from c in QFicha()
                       join f in _mdb.T_FICHA_VAT on c.IdFicha equals f.ID_FICHA
                       where f.ID_EMPRESA == idEmpresa
                       select c).Skip(skip).Take(take).ToList();

            var reco_prof = (from c in QFicha()
                             join f in _mdb.T_FICHA_REC_PROD on c.IdFicha equals f.ID_FICHA
                             where f.ID_EMPRESA == idEmpresa
                             select c).Skip(skip).Take(take).ToList();

            var t = ppp.Union(pppp.Union(vat.Union(reco_prof)));
            //return t.OrderBy(c => c.NombreTipoFicha).ThenBy(c => c.idEtapa).ThenBy(c => c.IdEstadoFicha).ToList();
            return t.OrderByDescending(c => c.idEtapa).ThenBy(c => c.NombreEstadoFicha).ThenBy(c => c.BeneficiarioFicha.NombreEstado ?? "").ToList();

        }

        public int AddFicha(IFicha ficha)
        {
            int idFicha = SecuenciaRepositorio.GetId();

            AgregarDatos(ficha);

            var fichaModel = new T_FICHAS
            {
                ID_FICHA = idFicha,
                APELLIDO = (ficha.Apellido ?? "").ToUpper(),
                BARRIO = ficha.Barrio.ToUpper(),
                CALLE = (ficha.Calle ?? "").ToUpper(),
                CANTIDAD_HIJOS = ficha.CantidadHijos ?? 0,
                CERTIF_DISCAP = ficha.CertificadoDiscapacidad ? "S" : "N",
                CODIGO_POSTAL = ficha.CodigoPostal,
                CODIGO_SEGURIDAD = ficha.CodigoSeguridad,
                CONTACTO = ficha.Contacto,
                CUIL = ficha.Cuil,
                DEFICIENCIA_OTRA = ficha.DeficienciaOtra,
                DPTO = ficha.Dpto,
                ENTRECALLES = (ficha.EntreCalles ?? "").ToUpper(),
                ID_LOCALIDAD = ficha.IdLocalidad,
                MAIL = ficha.Mail,
                ES_DISCAPACITADO = ficha.EsDiscapacitado ? "S" : "N",
                ESTADO_CIVIL = ficha.EstadoCivil ?? (int)Enums.EstadoCivil.Soltero,
                FER_NAC = ficha.FechaNacimiento ?? DateTime.Now,
                MANZANA = ficha.Manzana,
                MONOBLOCK = ficha.Monoblock,
                NOMBRE = (ficha.Nombre ?? "").ToUpper(),
                NUMERO = ficha.Numero,
                NUMERO_DOCUMENTO = ficha.NumeroDocumento,
                PARCELA = ficha.Parcela,
                PISO = ficha.Piso,
                SEXO = ficha.Sexo,
                TEL_CELULAR = ficha.TelefonoCelular,
                TEL_FIJO = ficha.TelefonoFijo,
                TIENE_DEF_MENTAL = ficha.TieneDeficienciaMental ? "S" : "N",
                TIENE_DEF_MOTORA = ficha.TieneDeficienciaMotora ? "S" : "N",
                TIENE_DEF_PSICOLOGICA = ficha.TieneDeficienciaPsicologia ? "S" : "N",
                TIENE_DEF_SENSORIAL = ficha.TieneDeficienciaSensorial ? "S" : "N",
                TIENE_HIJOS = ficha.TieneHijos ? "S" : "N",
                TIPO_DOCUMENTO = ficha.TipoDocumento ?? (int)Enums.TipoDocumento.Dni,
                TIPO_FICHA = Convert.ToInt16(ficha.TipoFicha),
                ID_ESTADO_FICHA = ficha.IdEstadoFicha,
                ID_USR_SIST = ficha.IdUsuarioSistema,
                FEC_SIST = ficha.FechaSistema,
                NRO_TRAMITE = ficha.NroTramite,
                ID_BARRIO = ficha.idBarrio,
            };

            _mdb.T_FICHAS.AddObject(fichaModel);

            switch (fichaModel.TIPO_FICHA)
            {
                case (int)Enums.TipoFicha.Terciaria:
                    var terciarioModel = new T_FICHA_TERCIARIO
                    {
                        ID_FICHA = idFicha,
                        ID_CARRERA = ficha.IdCarreraTerciaria,
                        ID_ESCUELA = ficha.IdEscuelaTerciaria,
                        PROMEDIO = ficha.PromedioTerciaria ?? 0,
                        OTRA_CARRERA = ficha.OtraCarreraTerciaria,
                        OTRA_INSTITUCION = ficha.OtraInstitucionTerciaria,
                        OTRO_SECTOR = ficha.OtroSectorTerciaria,
                        ID_SUBPROGRAMA = ficha.IdSubprograma == 0 ? null : ficha.IdSubprograma
                    };

                    _mdb.T_FICHA_TERCIARIO.AddObject(terciarioModel);
                    break;
                case (int)Enums.TipoFicha.Universitaria:
                    var universitarioModel = new T_FICHA_UNIVERSITARIO
                    {
                        ID_FICHA = idFicha,
                        ID_CARRERA = ficha.IdCarreraUniversitaria,
                        ID_ESCUELA = ficha.IdEscuelaUniversitaria,
                        PROMEDIO = ficha.PromedioUniversitaria,
                        OTRA_CARRERA = ficha.OtraCarreraUniversitaria,
                        OTRA_INSTITUCION = ficha.OtraInstitucionUniversitaria,
                        OTRO_SECTOR = ficha.OtroSectorUniversitaria,
                        ID_SUBPROGRAMA = ficha.IdSubprograma == 0 ? null : ficha.IdSubprograma
                    };

                    _mdb.T_FICHA_UNIVERSITARIO.AddObject(universitarioModel);
                    break;
                case (int)Enums.TipoFicha.Ppp:
                    var pppModel = new T_FICHA_PPP
                                       {
                                           ID_FICHA = idFicha,
                                           CURSANDO = ficha.Cursando ? "S" : "N",
                                           DESEA_TERM_NIVEL = ficha.DeseaTermNivel ? "S" : "N",
                                           FINALIZADO = ficha.Finalizado ? "S" : "N",
                                           ID_NIVEL_ESCOLARIDAD = Convert.ToInt16(ficha.IdNivelEscolaridad),
                                           MODALIDAD = ficha.Modalidad,
                                           ID_EMPRESA = ficha.IdEmpresa,
                                           TAREAS = ficha.Tareas,
                                           ALTA_TEMPRANA = ficha.AltaTemprana == "N" ? null : "S",
                                           ID_USR_SIST = ficha.IdUsuarioSistema,
                                           AT_FECHA_INICIO = ficha.FechaInicioActividad,
                                           AT_FECHA_CESE = ficha.FechaFinActividad,
                                           ID_MOD_CONT_AFIP = ficha.IdModalidadAfip == 0 ? null : ficha.IdModalidadAfip,
                                           ID_SEDE = (ficha.IdSede == 0 ? null : ficha.IdSede),
                                           ID_SUBPROGRAMA = ficha.IdSubprograma == 0 ? null : ficha.IdSubprograma,
                                           ID_ONG = ficha.ID_ONG == 0 ? null : ficha.ID_ONG,
                                           ID_ESCUELA = ficha.IdEscuelaPpp,
                                           ID_CURSO = ficha.ID_CURSO == 0 ? null : ficha.ID_CURSO,
                                           PPP_APRENDIZ =  ficha.ppp_aprendiz ?? "N",
                                           TIPO_PPP = ficha.TipoPrograma,
                                           HORARIO_ROTATIVO = ficha.horario_rotativo ?? "N",
                                           SEDE_ROTATIVA = ficha.sede_rotativa ?? "N",
                                           N_DESCRIPCION_T = ficha.n_descripcion_t,
                                           N_CURSADO_INS = ficha.n_cursado_ins,
                                           EGRESO = ficha.egreso,
                                           COD_USO_INTERNO = ficha.cod_uso_interno,
                                           CARGA_HORARIA = ficha.carga_horaria ?? "N",
                                           CH_CUAL = ficha.ch_cual,
                                           ID_CARRERA = ficha.id_carrera,
                                           CONSTANCIA_EGRESO = ficha.CONSTANCIA_EGRESO ?? "N",
                                           MATRICULA = ficha.MATRICULA ?? "N",
                                           CONSTANCIA_CURSO = ficha.CONSTANCIA_CURSO ?? "N",
                                           CO_FINANCIAMIENTO = ficha.co_financiamiento == 0 ? null : ficha.co_financiamiento,
                                           SENAF = ficha.SENAF ?? "N",
                                       };

                    _mdb.T_FICHA_PPP.AddObject(pppModel);
                    break;
                case (int)Enums.TipoFicha.Vat:
                    var pppModelVat = new T_FICHA_VAT
                                       {
                                           ID_FICHA = idFicha,
                                           CURSANDO = ficha.Cursando ? "S" : "N",
                                           FINALIZADO = ficha.Finalizado ? "S" : "N",
                                           ANIOS_APORTES = ficha.AniosAportes,
                                           ID_NIVEL_ESCOLARIDAD = Convert.ToInt16(ficha.IdNivelEscolaridad),
                                           MODALIDAD = ficha.Modalidad,
                                           ID_EMPRESA = ficha.IdEmpresa,
                                           TAREAS = ficha.Tareas,
                                           ALTA_TEMPRANA = ficha.AltaTemprana == "N" ? null : "S",
                                           ID_USR_SIST = ficha.IdUsuarioSistema,
                                           AT_FECHA_INICIO = ficha.FechaInicioActividad,
                                           AT_FECHA_CESE = ficha.FechaFinActividad,
                                           ID_MOD_CONT_AFIP = ficha.IdModalidadAfip == 0 ? null : ficha.IdModalidadAfip,
                                           ID_SEDE = (ficha.IdSede == 0 ? null : ficha.IdSede)
                                       };

                    _mdb.T_FICHA_VAT.AddObject(pppModelVat);
                    break;
                case (int)Enums.TipoFicha.PppProf:
                    var pppModelPppp = new T_FICHA_PPP_PROF
                    {
                        ID_FICHA = idFicha,
                        MODALIDAD = ficha.Modalidad,
                        ID_EMPRESA = ficha.IdEmpresa,
                        TAREAS = ficha.Tareas,
                        ALTA_TEMPRANA = ficha.AltaTemprana == "N" ? null : "S",
                        ID_USR_SIST = ficha.IdUsuarioSistema,
                        AT_FECHA_INICIO = ficha.FechaInicioActividad,
                        AT_FECHA_CESE = ficha.FechaFinActividad,
                        ID_MOD_CONT_AFIP = ficha.IdModalidadAfip == 0 ? null : ficha.IdModalidadAfip,
                        PROMEDIO = ficha.Promedio,
                        ID_TITULO = ficha.IdTitulo,
                        FECHA_EGRESO = ficha.FechaEgreso,
                        ID_SEDE = (ficha.IdSede == 0 ? null : ficha.IdSede)
                    };

                    _mdb.T_FICHA_PPP_PROF.AddObject(pppModelPppp);
                    break;
                case (int)Enums.TipoFicha.ReconversionProductiva:
                    var pppModelReconversion = new T_FICHA_REC_PROD()
                    {
                        ID_FICHA = idFicha, 
                        ID_EMPRESA = ficha.IdEmpresa,
                        ID_SEDE = (ficha.IdSede == 0 ? null : ficha.IdSede),
                        MODALIDAD = ficha.Modalidad,
                        ALTA_TEMPRANA = ficha.AltaTemprana == "N" ? null : "S",
                        AT_FECHA_INICIO = ficha.FechaInicioActividad,
                        AT_FECHA_CESE = ficha.FechaFinActividad,
                        CURSANDO = ficha.Cursando ? "S" : "N",
                        DESEA_TERM_NIVEL = ficha.DeseaTermNivel ? "S" : "N",
                        FINALIZADO = ficha.Finalizado ? "S" : "N",
                        ID_NIVEL_ESCOLARIDAD = Convert.ToInt16(ficha.IdNivelEscolaridad),
                        ID_USR_SIST = ficha.IdUsuarioSistema,
                        FEC_SIST = ficha.FechaSistema
                    };

                    _mdb.T_FICHA_REC_PROD.AddObject(pppModelReconversion);
                    break;
                case (int)Enums.TipoFicha.EfectoresSociales:
                    var pppModelEfectores = new T_FICHA_EFEC_SOC()
                    {
                        ID_FICHA = idFicha,
                        ID_EMPRESA = ficha.IdEmpresa,
                        ID_SEDE = (ficha.IdSede == 0 ? null : ficha.IdSede),
                        MODALIDAD = ficha.Modalidad,
                        ALTA_TEMPRANA = ficha.AltaTemprana == "N" ? null : "S",
                        AT_FECHA_INICIO = ficha.FechaInicioActividad,
                        AT_FECHA_CESE = ficha.FechaFinActividad,
                        CURSANDO = ficha.Cursando ? "S" : "N",
                        DESEA_TERM_NIVEL = ficha.DeseaTermNivel ? "S" : "N",
                        FINALIZADO = ficha.Finalizado ? "S" : "N",
                        ID_NIVEL_ESCOLARIDAD = Convert.ToInt16(ficha.IdNivelEscolaridad),
                        ID_USR_SIST = ficha.IdUsuarioSistema,
                        FEC_SIST = ficha.FechaSistema,
                        MONTO_EFE = ficha.monto_efe==0 ? null : (decimal?)ficha.monto_efe,
                        ID_SUBPROGRAMA = ficha.IdSubprograma == 0 ? null : ficha.IdSubprograma,
                    };

                    _mdb.T_FICHA_EFEC_SOC.AddObject(pppModelEfectores);
                    break;
                 case (int)Enums.TipoFicha.ConfiamosEnVos:
                    var ModelConfvos = new T_FICHAS_CONF_VOS()
                    {
                        ID_FICHA = idFicha, 
                        ID_EMPRESA = ficha.IdEmpresa,
                        ID_SEDE = (ficha.IdSede == 0 ? null : ficha.IdSede),
                        MODALIDAD = ficha.Modalidad,
                        ALTA_TEMPRANA = ficha.AltaTemprana == "N" ? null : "S",
                        AT_FECHA_INICIO = ficha.FechaInicioActividad,
                        AT_FECHA_CESE = ficha.FechaFinActividad,
                        CURSANDO = ficha.Cursando ? "S" : "N",
                        DESEA_TERM_NIVEL = ficha.DeseaTermNivel ? "S" : "N",
                        FINALIZADO = ficha.Finalizado ? "S" : "N",
                        ID_NIVEL_ESCOLARIDAD = Convert.ToInt16(ficha.IdNivelEscolaridad),
                        ID_USR_SIST = ficha.IdUsuarioSistema,
                        FEC_SIST = ficha.FechaSistema,


                        ABANDONO_ESCUELA	=	(short?)ficha.Abandono_Escuela,
                        TRABAJA_TRABAJO	=	ficha.Trabaja_Trabajo,
                        CURSA	=	ficha.cursa,
                        ULTIMO_CURSADO = (short?)ficha.Ultimo_Cursado,
                        PIT	=	ficha.pit,
                        CENTRO_ADULTOS	=	ficha.Centro_Adultos,
                        PROGRESAR	=	ficha.Progresar,
                        ACTIVIDADES = (short?)ficha.Actividades,
                        BENEF_PROGRE	=	ficha.	Benef_progre,
                        AUTORIZA_TUTOR	=	ficha.	Autoriza_Tutor,
                        APODERADO	=	ficha.	Apoderado,
                        APO_CUIL	=	ficha.	Apo_Cuil,
                        APO_APELLIDO	=	ficha.	Apo_Apellido,
                        APO_NOMBRE	=	ficha.	Apo_Nombre,
                        APO_FER_NAC	=	ficha.	Apo_Fer_Nac,
                        APO_TIPO_DOCUMENTO	=	ficha.	Apo_Tipo_Documento,
                        APO_NUMERO_DOCUMENTO	=	ficha.	Apo_Numero_Documento,
                        APO_CALLE	=	ficha.	Apo_Calle,
                        APO_NUMERO	=	ficha.	Apo_Numero,
                        APO_PISO	=	ficha.	Apo_Piso,
                        APO_DPTO	=	ficha.	Apo_Dpto,
                        APO_MONOBLOCK	=	ficha.	Apo_Monoblock,
                        APO_PARCELA	=	ficha.	Apo_Parcela,
                        APO_MANZANA	=	ficha.	Apo_Manzana,
                        APO_BARRIO	=	ficha.	Apo_Barrio,
                        APO_CODIGO_POSTAL	=	ficha.Apo_Codigo_Postal,
                        APO_ID_LOCALIDAD	=	ficha.Apo_Id_Localidad,
                        APO_TELEFONO	=	ficha.	Apo_Telefono,
                        APO_TIENE_HIJOS	=	ficha.	Apo_Tiene_Hijos,
                        APO_CANTIDAD_HIJOS	=	ficha.Apo_Cantidad_Hijos,
                        APO_SEXO	=	ficha.Apo_Sexo,
                        APO_ESTADO_CIVIL	=	(short?)ficha.Apo_Estado_Civil,
                        ID_CURSO = ficha.ID_CURSO == 0 ? null : ficha.ID_CURSO,
                        ID_ABANDONO_CUR =ficha.ID_ABANDONO_CUR,//(ficha.FichaConfVos.ID_ABANDONO_CUR == 0 ? null : ficha.FichaConfVos.ID_ABANDONO_CUR);
                        TRABAJO_CONDICION = ficha.TRABAJO_CONDICION,
                        SENAF = ficha.SENAF,
                        PERTENECE_ONG = ficha.PERTENECE_ONG,
                        ID_ONG = ficha.ID_ONG,//(ficha.FichaConfVos.ID_ONG == 0 ? null : ficha.FichaConfVos.ID_ONG);
                        APO_CELULAR = ficha.APO_CELULAR,
                        APO_MAIL = ficha.APO_MAIL,
                        ID_ESCUELA = ficha.IdEscuelaConfVos,
                        ID_FACILITADOR = ficha.ID_FACILITADOR == 0 ? null : ficha.ID_FACILITADOR,
                        ID_GESTOR = ficha.ID_GESTOR == 0 ? null : ficha.ID_GESTOR,

                    };

                    _mdb.T_FICHAS_CONF_VOS.AddObject(ModelConfvos);
                    break;
                default:
                    break;
            }

            _mdb.SaveChanges();

            return fichaModel.ID_FICHA;
        }

        public IFichaPPP GetFormFichaPpp()
        {
            var fichappp = new FichaPPP();

            return fichappp;
        }

        public IList<IFicha> GetFichasCompleto(string cuil, string dni, string nombre, string apellido, int tipoficha, int estadoficha)
        {
            nombre = nombre ?? String.Empty;
            apellido = apellido ?? String.Empty;
            cuil = cuil ?? String.Empty;
            dni = dni ?? String.Empty;

            IList<IFicha> lista = QFichaCompleto(tipoficha).ToList();

            return
                lista.Where(
                    c => (c.Apellido.ToLower().Contains(apellido.ToLower()) || String.IsNullOrEmpty(apellido.Trim())) &&
                         (c.Nombre.ToLower().Contains(nombre.ToLower()) || String.IsNullOrEmpty(nombre.Trim())) &&
                         (c.IdEstadoFicha == estadoficha || estadoficha == -1) &&
                         (c.NumeroDocumento.Contains(cuil) || String.IsNullOrEmpty(cuil.Trim())) &&
                         (c.Cuil.Contains(dni) || String.IsNullOrEmpty(dni.Trim())) &&
                         (c.TipoFicha == tipoficha || tipoficha == 0)).OrderBy(c => c.Apellido.ToUpper().Trim())
                         .ThenBy(o => o.Nombre.ToUpper().Trim()).ToList();
        }

        public IList<IFicha> GetFichasRDepLocProgEst(int? iddepartamento, int? idprograma, int? idlocalidad, string estado, int EstadoBenef)
        {
            iddepartamento = iddepartamento == -1 ? null : iddepartamento;
            idprograma = idprograma == 0 ? null : idprograma;
            idlocalidad = idlocalidad == -1 ? null : idlocalidad;

            if (idprograma > 0)
            {
                return
               QFichaSimpleReportes((int)idprograma).Where(
                   c =>
                   (c.IdDepartamento == iddepartamento || iddepartamento == null) &&
                   (c.IdLocalidad == idlocalidad || idlocalidad == null) &&
                   (c.TipoFicha == idprograma || idprograma == null) &&
                   (estado == "0"
                        ? c.IdEstadoFicha != -3
                        : c.IdEstadoFicha == (int)Enums.EstadoFicha.Beneficiario) && (EstadoBenef == 0 ? (c.BeneficiarioFicha.IdEstadoExport ?? 0) != -3 : c.BeneficiarioFicha.IdEstadoExport == EstadoBenef)).
                   ToList();
            }
            else
            {
                return
                   QFichaSimpleReportes().Where(
                       c =>
                       (c.IdDepartamento == iddepartamento || iddepartamento == null) &&
                       (c.IdLocalidad == idlocalidad || idlocalidad == null) &&
                       (c.TipoFicha == idprograma || idprograma == null) &&
                       (estado == "0"
                            ? c.IdEstadoFicha != -3
                            : c.IdEstadoFicha == (int)Enums.EstadoFicha.Beneficiario) && (EstadoBenef == 0 ? (c.BeneficiarioFicha.IdEstadoExport ?? 0) != -3 : c.BeneficiarioFicha.IdEstadoExport == EstadoBenef)).
                       ToList();
            }
        }

        //para el export detalle
        public IList<IFicha> GetFichasRDepLocProgEstDetalle(int? iddepartamento, int? idprograma, int? idlocalidad, string estado, int estadoBenef)
        {
            iddepartamento = iddepartamento == -1 ? null : iddepartamento;
            idprograma = idprograma == 0 ? null : idprograma;
            idlocalidad = idlocalidad == -1 ? null : idlocalidad;

            return
                QFichaDetalleReportes(idprograma).Where(
                    c =>
                    (c.IdDepartamento == iddepartamento || iddepartamento == null) &&
                    (c.IdLocalidad == idlocalidad || idlocalidad == null) &&
                    (c.TipoFicha == idprograma || idprograma == null) &&
                    (estado == "0"
                         ? c.IdEstadoFicha != -3
                         : c.IdEstadoFicha == (int)Enums.EstadoFicha.Beneficiario) //&&
                           //c.BeneficiarioFicha.IdEstadoExport == (int)Enums.EstadoBeneficiario.Activo)
                           && (estadoBenef == 0 ? (c.BeneficiarioFicha.IdEstadoExport ?? 0) != -3 : c.BeneficiarioFicha.IdEstadoExport == estadoBenef)
                           ).
                    ToList();
        }
       
        public IList<IFicha> GetFichasRBecasAcademicas(int? idsector, int? idinstitucion, int? idcarrera, int? idprograma, string estado)
        {
            idsector = idsector == -1 ? null : idsector;
            idinstitucion = idinstitucion == -1 ? null : idinstitucion;
            idcarrera = idcarrera == -1 ? null : idcarrera;
            idprograma = idprograma == 0 ? null : idprograma;

                return
                QFichaCompleto(idprograma ?? 0).Where(
                   c =>
                   (c.CarreraFicha.SectorCarrera.IdSector == idsector || idsector == null) &&
                   (c.CarreraFicha.IdInstitucion == idinstitucion || idinstitucion == null) &&
                   (c.CarreraFicha.IdCarrera == idcarrera || idcarrera == null) &&
                   (c.TipoFicha == idprograma || idprograma == null) &&
                   (estado == "0"
                        ? c.IdEstadoFicha != -3
                        : c.IdEstadoFicha == (int)Enums.EstadoFicha.Beneficiario)).
                   ToList();
            
        }

        public IFichaEfectoresSociales GetFichaEfectores(int idficha)
        {
            var fichaefectores = _mdb.T_FICHA_EFEC_SOC.Where(c => c.ID_FICHA == idficha).Select(
                 c =>
                 new FichaEfectoresSociales()
                 {
                     CuitEmpresa = c.T_EMPRESAS.CUIT ?? "",
                     DescripcionEmpresa = c.T_EMPRESAS.N_EMPRESA,
                     IdEmpresa = c.ID_EMPRESA,
                     IdSede = c.ID_SEDE,
                     IdSubprograma = c.ID_SUBPROGRAMA,
                     AltaTemprana = c.ALTA_TEMPRANA == null ? "N" : "S",
                     Modalidad = c.MODALIDAD ?? 0,
                     FechaFinActividad = c.AT_FECHA_CESE,
                     FechaInicioActividad = c.AT_FECHA_INICIO,
                     IdModalidadAfip = c.ID_MOD_CONT_AFIP,
                     ModalidadAfip = c.T_MOD_CONT_AFIP.N_MOD_CONT_AFIP,
                     AportesDeLaEmpresa = c.APORTES_EMP,
                     ApellidoTutor = c.APELLIDO_TUTOR,
                     NombreTutor = c.NOMBRE_TUTOR,
                     NroDocumentoTutor = c.DNI_TUTOR,
                     TelefonoTutor = c.TELEFONO_TUTOR,
                     EmailTutor = c.EMAIL_TUTOR,
                     PuestoTutor = c.PUESTO_TUTOR,
                     DeseaTermNivel = (c.DESEA_TERM_NIVEL == "S" ? true : false),
                     Finalizado = (c.FINALIZADO == "S" ? true : false),
                     IdNivelEscolaridad = c.ID_NIVEL_ESCOLARIDAD,
                     Cursando = (c.CURSANDO == "S" ? true : false),
                     monto = c.MONTO_EFE ?? 0,
                     Proyecto =
                     new Proyecto
                     {
                         IdProyecto = c.T_PROYECTOS.ID_PROYECTO,
                         NombreProyecto = c.T_PROYECTOS.N_PROYECTO,
                         CantidadAcapacitar = c.T_PROYECTOS.CANT_CAPACITAR,
                         FechaInicioProyecto = c.T_PROYECTOS.FEC_INICIO,
                         FechaFinProyecto = c.T_PROYECTOS.FEC_FIN,
                         MesesDuracionProyecto = c.T_PROYECTOS.MESES_DURAC,
                     },
                     Empresa =
                     new Empresa
                     {
                         NombreEmpresa = c.T_EMPRESAS.N_EMPRESA,
                         Calle = c.T_EMPRESAS.CALLE,
                         CantidadEmpleados = c.T_EMPRESAS.CANTIDAD_EMPLEADOS,
                         NombreLocalidad = c.T_EMPRESAS.T_LOCALIDADES.N_LOCALIDAD,
                         IdLocalidad = c.T_EMPRESAS.ID_LOCALIDAD,
                         Piso = c.T_EMPRESAS.PISO,
                         Numero = c.T_EMPRESAS.NUMERO,
                         CodigoActividad = c.T_EMPRESAS.CODIGO_ACTIVIDAD,
                         Cuit = c.T_EMPRESAS.CUIT,
                         IdUsuario = c.T_EMPRESAS.ID_USUARIO,
                         Dpto = c.T_EMPRESAS.DPTO,
                         CodigoPostal = c.T_EMPRESAS.CODIGO_POSTAL,
                         DomicilioLaboralIdem = (c.T_EMPRESAS.DOMICLIO_LABORAL_IDEM == null ||
                         c.T_EMPRESAS.DOMICLIO_LABORAL_IDEM == "N") ? "No" : "Si"
                     },
                     Sede =
                         new Sede
                         {
                             ApellidoContacto = c.T_SEDES.APE_CONTACTO,
                             Calle = c.T_SEDES.CALLE,
                             CodigoPostal = c.T_SEDES.CODIGO_POSTAL,
                             Dpto = c.T_SEDES.DPTO,
                             Email = c.T_SEDES.EMAIL,
                             Fax = c.T_SEDES.FAX,
                             NombreSede = c.T_SEDES.N_SEDE,
                             NombreContacto = c.T_SEDES.NOM_CONTACTO,
                             Numero = c.T_SEDES.NUMERO,
                             Piso = c.T_SEDES.PISO,
                             Telefono = c.T_SEDES.TELEFONO,
                             NombreLocalidad = c.T_SEDES.T_LOCALIDADES.N_LOCALIDAD
                         }


                 }).SingleOrDefault();

            if (fichaefectores != null)
            {
                if (fichaefectores.Empresa != null)
                {
                    IUsuarioEmpresa usuario =
                        _mdb.T_USUARIOS_EMPRESA.Where(usu => usu.ID_USUARIO == fichaefectores.Empresa.IdUsuario).
                            Select(
                                tue =>
                                new UsuarioEmpresa
                                {
                                    ApellidoUsuario = tue.APELLIDO,
                                    NombreUsuario = tue.NOMBRE,
                                    Cuil = tue.CUIL,
                                    Telefono = tue.TELEFONO,
                                    Mail = tue.MAIL,
                                    LoginUsuario = tue.LOGIN,
                                }).SingleOrDefault();

                    fichaefectores.Empresa.Usuario = usuario;
                }
            }

            return fichaefectores;
        }

        public IFichaEfectoresSociales GetFormFichaEfectores()
        {
            var fichaefectores = new FichaEfectoresSociales();

            return fichaefectores;
        }

        public IFichaReconversion GetFichaReconversion(int idficha)
        {
            var fichareconv = _mdb.T_FICHA_REC_PROD.Where(c => c.ID_FICHA == idficha).Select(
                c =>
                new FichaReconversion
                    {
                        CuitEmpresa = c.T_EMPRESAS.CUIT ?? "",
                        DescripcionEmpresa = c.T_EMPRESAS.N_EMPRESA,
                        IdEmpresa = c.ID_EMPRESA,
                        IdSede = c.ID_SEDE,
                        IdSubprograma = c.ID_SUBPROGRAMA,
                        AltaTemprana = c.ALTA_TEMPRANA == null ? "N" : "S",
                        Modalidad = c.MODALIDAD ?? 0,
                        FechaFinActividad = c.AT_FECHA_CESE,
                        FechaInicioActividad = c.AT_FECHA_INICIO,
                        IdModalidadAfip = c.ID_MOD_CONT_AFIP,
                        ModalidadAfip = c.T_MOD_CONT_AFIP.N_MOD_CONT_AFIP,
                        AportesDeLaEmpresa = c.APORTES_EMP,
                        ApellidoTutor =c.APELLIDO_TUTOR,
                        NombreTutor =c.NOMBRE_TUTOR,
                        NroDocumentoTutor =c.DNI_TUTOR,
                        TelefonoTutor=c.TELEFONO_TUTOR,
                        EmailTutor =c.EMAIL_TUTOR,
                        PuestoTutor =c.PUESTO_TUTOR,
                        DeseaTermNivel =(c.DESEA_TERM_NIVEL == "S" ? true : false),
                        Finalizado = (c.FINALIZADO == "S" ? true : false),
                        IdNivelEscolaridad = c.ID_NIVEL_ESCOLARIDAD,
                        Cursando = (c.CURSANDO == "S" ? true : false),
                        Proyecto = 
                        new Proyecto
                            {
                                IdProyecto = c.T_PROYECTOS.ID_PROYECTO,
                                NombreProyecto = c.T_PROYECTOS.N_PROYECTO,
                                CantidadAcapacitar = c.T_PROYECTOS.CANT_CAPACITAR,
                                FechaInicioProyecto = c.T_PROYECTOS.FEC_INICIO,
                                FechaFinProyecto = c.T_PROYECTOS.FEC_FIN,
                                MesesDuracionProyecto = c.T_PROYECTOS.MESES_DURAC,
                            },
                        Empresa =
                        new Empresa
                        {
                            NombreEmpresa = c.T_EMPRESAS.N_EMPRESA,
                            Calle = c.T_EMPRESAS.CALLE,
                            CantidadEmpleados = c.T_EMPRESAS.CANTIDAD_EMPLEADOS,
                            NombreLocalidad = c.T_EMPRESAS.T_LOCALIDADES.N_LOCALIDAD,
                            IdLocalidad = c.T_EMPRESAS.ID_LOCALIDAD,
                            Piso = c.T_EMPRESAS.PISO,
                            Numero = c.T_EMPRESAS.NUMERO,
                            CodigoActividad = c.T_EMPRESAS.CODIGO_ACTIVIDAD,
                            Cuit = c.T_EMPRESAS.CUIT,
                            IdUsuario = c.T_EMPRESAS.ID_USUARIO,
                            Dpto = c.T_EMPRESAS.DPTO,
                            CodigoPostal = c.T_EMPRESAS.CODIGO_POSTAL,
                            DomicilioLaboralIdem = (c.T_EMPRESAS.DOMICLIO_LABORAL_IDEM == null ||
                            c.T_EMPRESAS.DOMICLIO_LABORAL_IDEM == "N") ? "No" : "Si"
                        },
                    Sede =
                        new Sede
                        {
                            ApellidoContacto = c.T_SEDES.APE_CONTACTO,
                            Calle = c.T_SEDES.CALLE,
                            CodigoPostal = c.T_SEDES.CODIGO_POSTAL,
                            Dpto = c.T_SEDES.DPTO,
                            Email = c.T_SEDES.EMAIL,
                            Fax = c.T_SEDES.FAX,
                            NombreSede = c.T_SEDES.N_SEDE,
                            NombreContacto = c.T_SEDES.NOM_CONTACTO,
                            Numero = c.T_SEDES.NUMERO,
                            Piso = c.T_SEDES.PISO,
                            Telefono = c.T_SEDES.TELEFONO,
                            NombreLocalidad = c.T_SEDES.T_LOCALIDADES.N_LOCALIDAD
                        }


                }).SingleOrDefault();

            if (fichareconv != null)
            {
                if (fichareconv.Empresa != null)
                {
                    IUsuarioEmpresa usuario =
                        _mdb.T_USUARIOS_EMPRESA.Where(usu => usu.ID_USUARIO == fichareconv.Empresa.IdUsuario).
                            Select(
                                tue =>
                                new UsuarioEmpresa
                                {
                                    ApellidoUsuario = tue.APELLIDO,
                                    NombreUsuario = tue.NOMBRE,
                                    Cuil = tue.CUIL,
                                    Telefono = tue.TELEFONO,
                                    Mail = tue.MAIL,
                                    LoginUsuario = tue.LOGIN,
                                }).SingleOrDefault();

                    fichareconv.Empresa.Usuario = usuario;
                }
            }

            return fichareconv;
        }
        
        public IFichaConfVos GetFichaConfVos(int idficha)
        {
            var fichaconfvos = _mdb.T_FICHAS_CONF_VOS.Where(c => c.ID_FICHA == idficha).Select(
                c =>
                new FichaConfVos
                {
                    CuitEmpresa = c.T_EMPRESAS.CUIT ?? "",
                    DescripcionEmpresa = c.T_EMPRESAS.N_EMPRESA,
                    IdEmpresa = c.ID_EMPRESA,
                    IdSede = c.ID_SEDE,
                    IdSubprograma = c.ID_SUBPROGRAMA,
                    AltaTemprana = c.ALTA_TEMPRANA == null ? "N" : "S",
                    Modalidad = c.MODALIDAD ?? 0,
                    FechaFinActividad = c.AT_FECHA_CESE,
                    FechaInicioActividad = c.AT_FECHA_INICIO,
                    IdModalidadAfip = c.ID_MOD_CONT_AFIP,
                    ModalidadAfip = c.T_MOD_CONT_AFIP.N_MOD_CONT_AFIP,
                    AportesDeLaEmpresa = c.APORTES_EMP,
                    ApellidoTutor = c.APELLIDO_TUTOR,
                    NombreTutor = c.NOMBRE_TUTOR,
                    NroDocumentoTutor = c.DNI_TUTOR,
                    TelefonoTutor = c.TELEFONO_TUTOR,
                    EmailTutor = c.EMAIL_TUTOR,
                    PuestoTutor = c.PUESTO_TUTOR,
                    DeseaTermNivel = (c.DESEA_TERM_NIVEL == "S" ? true : false),
                    Finalizado = (c.FINALIZADO == "S" ? true : false),
                    IdNivelEscolaridad = c.ID_NIVEL_ESCOLARIDAD,
                    Cursando = (c.CURSANDO == "S" ? true : false),

                    Abandono_Escuela = c.ABANDONO_ESCUELA,
                    Trabaja_Trabajo =c.TRABAJA_TRABAJO,
                    cursa=c.CURSA,
                    Ultimo_Cursado=c.ULTIMO_CURSADO,
                    pit=c.PIT,
                    Centro_Adultos=c.CENTRO_ADULTOS,
                    Progresar=c.PROGRESAR,
                    Actividades=c.ACTIVIDADES,
                    Benef_progre=c.BENEF_PROGRE,
                    Autoriza_Tutor=c.AUTORIZA_TUTOR,
                    Apoderado=c.APODERADO,
                    Apo_Cuil = c.APO_CUIL,
                    Apo_Apellido =c.APO_APELLIDO,
                    Apo_Nombre =c.APO_NOMBRE,
                    Apo_Fer_Nac =c.APO_FER_NAC,
                    Apo_Tipo_Documento =c.APO_TIPO_DOCUMENTO,
                    Apo_Numero_Documento =c.APO_NUMERO_DOCUMENTO,
                    Apo_Calle =c.APO_CALLE,
                    Apo_Numero =c.APO_NUMERO,
                    Apo_Piso =c.APO_PISO,
                    Apo_Dpto =c.APO_DPTO,
                    Apo_Monoblock =c.APO_MONOBLOCK,
                    Apo_Parcela =c.APO_PARCELA,
                    Apo_Manzana  =c.APO_MANZANA,
                    Apo_Barrio  =c.APO_BARRIO,
                    Apo_Codigo_Postal =c.APO_CODIGO_POSTAL,
                    Apo_Id_Localidad  =c.APO_ID_LOCALIDAD,
                    Apo_Telefono  =c.APO_TELEFONO,
                    Apo_Tiene_Hijos   =c.APO_TIENE_HIJOS,
                    Apo_Cantidad_Hijos    =c.APO_CANTIDAD_HIJOS ?? 0,
                    Apo_Sexo    =c.APO_SEXO,
                    Apo_Estado_Civil    =c.APO_ESTADO_CIVIL,

                    Proyecto =
                    new Proyecto
                    {
                        IdProyecto = c.T_PROYECTOS.ID_PROYECTO,
                        NombreProyecto = c.T_PROYECTOS.N_PROYECTO,
                        CantidadAcapacitar = c.T_PROYECTOS.CANT_CAPACITAR,
                        FechaInicioProyecto = c.T_PROYECTOS.FEC_INICIO,
                        FechaFinProyecto = c.T_PROYECTOS.FEC_FIN,
                        MesesDuracionProyecto = c.T_PROYECTOS.MESES_DURAC,
                    },
                    Empresa =
                    new Empresa
                    {
                        NombreEmpresa = c.T_EMPRESAS.N_EMPRESA,
                        Calle = c.T_EMPRESAS.CALLE,
                        CantidadEmpleados = c.T_EMPRESAS.CANTIDAD_EMPLEADOS,
                        NombreLocalidad = c.T_EMPRESAS.T_LOCALIDADES.N_LOCALIDAD,
                        IdLocalidad = c.T_EMPRESAS.ID_LOCALIDAD,
                        Piso = c.T_EMPRESAS.PISO,
                        Numero = c.T_EMPRESAS.NUMERO,
                        CodigoActividad = c.T_EMPRESAS.CODIGO_ACTIVIDAD,
                        Cuit = c.T_EMPRESAS.CUIT,
                        IdUsuario = c.T_EMPRESAS.ID_USUARIO,
                        Dpto = c.T_EMPRESAS.DPTO,
                        CodigoPostal = c.T_EMPRESAS.CODIGO_POSTAL,
                        DomicilioLaboralIdem = (c.T_EMPRESAS.DOMICLIO_LABORAL_IDEM == null ||
                        c.T_EMPRESAS.DOMICLIO_LABORAL_IDEM == "N") ? "No" : "Si"
                    },
                    Sede =
                        new Sede
                        {
                            ApellidoContacto = c.T_SEDES.APE_CONTACTO,
                            Calle = c.T_SEDES.CALLE,
                            CodigoPostal = c.T_SEDES.CODIGO_POSTAL,
                            Dpto = c.T_SEDES.DPTO,
                            Email = c.T_SEDES.EMAIL,
                            Fax = c.T_SEDES.FAX,
                            NombreSede = c.T_SEDES.N_SEDE,
                            NombreContacto = c.T_SEDES.NOM_CONTACTO,
                            Numero = c.T_SEDES.NUMERO,
                            Piso = c.T_SEDES.PISO,
                            Telefono = c.T_SEDES.TELEFONO,
                            NombreLocalidad = c.T_SEDES.T_LOCALIDADES.N_LOCALIDAD
                        },
                    ID_CURSO = c.ID_CURSO ?? 0,

                    ID_FACILITADOR = c.ID_FACILITADOR ?? 0,
                    ID_GESTOR = c.ID_GESTOR ?? 0,
                    ID_SUBSIDIARIA = c.ID_SUBSIDIARIA ?? 0,
                    ID_ABANDONO_CUR = c.ID_ABANDONO_CUR ?? 0,
                    TRABAJO_CONDICION = c.TRABAJO_CONDICION,
                    SENAF = c.SENAF,
                    PERTENECE_ONG = c.PERTENECE_ONG,
                    ID_ONG = c.ID_ONG ?? 0,
                    APO_CELULAR = c.APO_CELULAR,
                    APO_MAIL = c.APO_MAIL,
                    Id_Escuela = c.ID_ESCUELA ?? 0,

                }).SingleOrDefault();

            if (fichaconfvos != null)
            {
                if (fichaconfvos.Empresa != null)
                {
                    IUsuarioEmpresa usuario =
                        _mdb.T_USUARIOS_EMPRESA.Where(usu => usu.ID_USUARIO == fichaconfvos.Empresa.IdUsuario).
                            Select(
                                tue =>
                                new UsuarioEmpresa
                                {
                                    ApellidoUsuario = tue.APELLIDO,
                                    NombreUsuario = tue.NOMBRE,
                                    Cuil = tue.CUIL,
                                    Telefono = tue.TELEFONO,
                                    Mail = tue.MAIL,
                                    LoginUsuario = tue.LOGIN,
                                }).SingleOrDefault();

                    fichaconfvos.Empresa.Usuario = usuario;
                }




            }

            return fichaconfvos;
        }

        public IFichaReconversion GetFormFichaReconversion()
        {
            var fichareconversion = new FichaReconversion();

            return fichareconversion;
        }

        //20/02/2013 - DI CAMPLI LEANDRO - TRAER FICHAS PARA NOTIFICACION
        public List<IFicha> GetFichasNotificacion(int[] idFichas)
        {
            return QGetFichasNotificacion(idFichas).ToList();
        }

        public List<IFicha> GetFichasNotificacion()
        {
            return QGetFichasNotificacion().ToList();
        }

        private IQueryable<IFicha> QGetFichasNotificacion(int[] idFichas)
        {
            //List<IFicha> fichas;

            var ppp = from f in _mdb.T_FICHAS
                      from fppp in _mdb.T_FICHA_PPP.Where(pppc => pppc.ID_FICHA == f.ID_FICHA).DefaultIfEmpty()
                      from emp in _mdb.T_EMPRESAS.Where(ec => ec.ID_EMPRESA == fppp.ID_EMPRESA).DefaultIfEmpty()
                      from loc in _mdb.T_LOCALIDADES.Where(lc => lc.ID_LOCALIDAD == emp.ID_LOCALIDAD).DefaultIfEmpty()
                      where idFichas.Contains(f.ID_FICHA)
                      select new Ficha
                      {
                          IdFicha = f.ID_FICHA,
                          Apellido = f.APELLIDO,
                          Nombre = f.NOMBRE,
                          FichaPpp = new FichaPPP
                                              {
                                                  IdFicha = fppp.ID_FICHA,
                                                  IdEmpresa = fppp.ID_EMPRESA,
                                                  Empresa = new Empresa 
                                                                { 
                                                                   IdEmpresa = emp.ID_EMPRESA,
                                                                   NombreEmpresa = emp.N_EMPRESA,
                                                                   Calle = emp.DOMICLIO_LABORAL_IDEM,
                                                                   Numero = emp.NUMERO,
                                                                   IdLocalidad = loc.ID_LOCALIDAD,
                                                                   NombreLocalidad = loc.N_LOCALIDAD

                                                                }
                                              }

                      };

            var Pppp = from f in _mdb.T_FICHAS
                      from fppp in _mdb.T_FICHA_PPP_PROF.Where(pppc => pppc.ID_FICHA == f.ID_FICHA).DefaultIfEmpty()
                      from emp in _mdb.T_EMPRESAS.Where(ec => ec.ID_EMPRESA == fppp.ID_EMPRESA).DefaultIfEmpty()
                      from loc in _mdb.T_LOCALIDADES.Where(lc => lc.ID_LOCALIDAD == emp.ID_LOCALIDAD).DefaultIfEmpty()
                      where idFichas.Contains(f.ID_FICHA)
                      select new Ficha
                      {
                          IdFicha = f.ID_FICHA,
                          Apellido = f.APELLIDO,
                          Nombre = f.NOMBRE,
                          FichaPppp = new FichaPPPP
                          {
                              IdFicha = fppp.ID_FICHA,
                              IdEmpresa = fppp.ID_EMPRESA,
                              Empresa = new Empresa
                              {
                                  IdEmpresa = emp.ID_EMPRESA,
                                  NombreEmpresa = emp.N_EMPRESA,
                                  Calle = emp.DOMICLIO_LABORAL_IDEM,
                                  Numero = emp.NUMERO,
                                  IdLocalidad = loc.ID_LOCALIDAD,
                                  NombreLocalidad = loc.N_LOCALIDAD

                              }
                          }

                      };


            ///fichas = ppp;
            return ppp.Union(Pppp);
        
        }

        private IQueryable<IFicha> QGetFichasNotificacion()
        {
            //List<IFicha> fichas;

            var ppp = from f in _mdb.T_FICHAS
                      from fppp in _mdb.T_FICHA_PPP.Where(pppc => pppc.ID_FICHA == f.ID_FICHA).DefaultIfEmpty()
                      from emp in _mdb.T_EMPRESAS.Where(ec => ec.ID_EMPRESA == fppp.ID_EMPRESA).DefaultIfEmpty()
                      from loc in _mdb.T_LOCALIDADES.Where(lc => lc.ID_LOCALIDAD == emp.ID_LOCALIDAD).DefaultIfEmpty()
           //           where idFichas.Contains(f.ID_FICHA)
                      select new Ficha
                      {
                          IdFicha = f.ID_FICHA,
                          Apellido = f.APELLIDO,
                          Nombre = f.NOMBRE,
                          FichaPpp = new FichaPPP
                          {
                              IdFicha = fppp.ID_FICHA,
                              IdEmpresa = fppp.ID_EMPRESA,
                              Empresa = new Empresa
                              {
                                  IdEmpresa = emp.ID_EMPRESA,
                                  NombreEmpresa = emp.N_EMPRESA,
                                  Calle = emp.DOMICLIO_LABORAL_IDEM,
                                  Numero = emp.NUMERO,
                                  IdLocalidad = loc.ID_LOCALIDAD,
                                  NombreLocalidad = loc.N_LOCALIDAD

                              }
                          }

                      };

            var Pppp = from f in _mdb.T_FICHAS
                       from fppp in _mdb.T_FICHA_PPP_PROF.Where(pppc => pppc.ID_FICHA == f.ID_FICHA).DefaultIfEmpty()
                       from emp in _mdb.T_EMPRESAS.Where(ec => ec.ID_EMPRESA == fppp.ID_EMPRESA).DefaultIfEmpty()
                       from loc in _mdb.T_LOCALIDADES.Where(lc => lc.ID_LOCALIDAD == emp.ID_LOCALIDAD).DefaultIfEmpty()
         //              where idFichas.Contains(f.ID_FICHA)
                       select new Ficha
                       {
                           IdFicha = f.ID_FICHA,
                           Apellido = f.APELLIDO,
                           Nombre = f.NOMBRE,
                           FichaPppp = new FichaPPPP
                           {
                               IdFicha = fppp.ID_FICHA,
                               IdEmpresa = fppp.ID_EMPRESA,
                               Empresa = new Empresa
                               {
                                   IdEmpresa = emp.ID_EMPRESA,
                                   NombreEmpresa = emp.N_EMPRESA,
                                   Calle = emp.DOMICLIO_LABORAL_IDEM,
                                   Numero = emp.NUMERO,
                                   IdLocalidad = loc.ID_LOCALIDAD,
                                   NombreLocalidad = loc.N_LOCALIDAD

                               }
                           }

                       };


            ///fichas = ppp;
            return ppp.Union(Pppp);

        }


        // 21/02/2013 - DI CAMPLI LEANDRO - CARGA MASIVA DE BENEFICIARIOS
        //public bool UpdateEstadoBeneficiario(List<IFicha> fichas)
        //{
        //    IComunDatos comun = new ComunDatos();

        //    AgregarDatos(comun);


        //    var obj = _mdb.T_FICHAS.FirstOrDefault(c => c.ID_FICHA == idFicha);
        //    obj.ID_ESTADO_FICHA = idEstadoFicha;
        //    obj.ID_USR_SIST = comun.IdUsuarioSistema;

        //    _mdb.SaveChanges();

        //    return true;
        //}


        // 21/05/2013 - DI CAMPLI LEANDRO - MEJORA PERFORMANCE DE CONSULTA
        public IList<IFicha> GetFichas(List<int> idBeneficiariosExport)
        {
            //return QFicha().Where(c => idBeneficiariosExport.Contains(c.IdFicha)).OrderBy(c => c.Apellido.Trim().ToUpper()).ThenBy(o => o.Nombre.Trim().ToUpper()).ToList();
            return QFicha(idBeneficiariosExport).OrderBy(c => c.Apellido.Trim().ToUpper()).ThenBy(o => o.Nombre.Trim().ToUpper()).ToList();
        }

        public IList<IFicha> GetFichas(List<int> idBeneficiariosExport, int idPrograma)
        {       
            return QFicha(idBeneficiariosExport, idPrograma).OrderBy(c => c.Apellido.Trim().ToUpper()).ThenBy(o => o.Nombre.Trim().ToUpper()).ToList();
        }


        // 08/08/2013 - DI CAMPLI LEANDRO - USO DE VISTA PARA EXPORTAR EXCEL

        private List<VT_REPORTES_PPP> QVT_REPORTES_PPP(string cuil, string dni, string nombre, string apellido, int tipoficha, int estadoficha, int idEtapa, int tipoPpp, int idSubprograma, string nrotramite)
        {


            //List<VT_REPORTES_PPP> listabeneficiariototal;

                    var lficha = (from ppp in _mdb.VT_REPORTES_PPP
                                                  where 

                                                  (ppp.APELLIDO.ToLower().Contains(apellido.ToLower()) || String.IsNullOrEmpty(apellido.Trim())) &&
                                                  (ppp.NOMBRE.ToLower().Contains(nombre.ToLower()) || String.IsNullOrEmpty(nombre.Trim())) &&
                                                  (ppp.ID_EST_FIC == estadoficha || estadoficha == -1) &&
                                                  (ppp.CUIL.Contains(cuil) || String.IsNullOrEmpty(cuil.Trim())) &&
                                                  (ppp.NUMERO_DOCUMENTO.Contains(dni) || String.IsNullOrEmpty(dni.Trim())) &&
                                                   ppp.TIPO_FICHA == 3// tipoficha || tipoficha == 0
                                                   && (ppp.NRO_TRAMITE.ToLower().Contains(nrotramite.ToLower()) || String.IsNullOrEmpty(nrotramite.Trim())) 
                                                   && (ppp.IDETAPA == idEtapa || idEtapa == 0)
                                                   && (ppp.TIPO_PPP == tipoPpp || tipoPpp == 0)
                                                   && (ppp.ID_SUBPROGRAMA == idSubprograma || idSubprograma == 0)


                                                  select ppp

                          ).ToList();

                    return lficha.ToList();

            }

        public List<VT_REPORTES_PPP> getfichaPPP(string cuil, string dni, string nombre, string apellido, int tipoficha, int estadoficha, int idEtapa, int TipoPpp, int idSubprograma, string nrotramite)
        {

            nombre = nombre ?? String.Empty;
            apellido = apellido ?? String.Empty;
            cuil = cuil ?? String.Empty;
            dni = dni ?? String.Empty;
            return QVT_REPORTES_PPP(cuil, dni, nombre, apellido, tipoficha, estadoficha, idEtapa, TipoPpp, idSubprograma, nrotramite).ToList();

        }

        private List<VT_REPORTES_TER> QVT_REPORTES_TER(string cuil, string dni, string nombre, string apellido, int tipoficha, int estadoficha, int idEtapa, int idSubprograma)
        {

            var lficha = (from ter in _mdb.VT_REPORTES_TER
                          where

                          (ter.APELLIDO.ToLower().Contains(apellido.ToLower()) || String.IsNullOrEmpty(apellido.Trim())) &&
                          (ter.NOMBRE.ToLower().Contains(nombre.ToLower()) || String.IsNullOrEmpty(nombre.Trim())) &&
                          (ter.ID_EST_FIC == estadoficha || estadoficha == -1) &&
                          (ter.CUIL.Contains(cuil) || String.IsNullOrEmpty(cuil.Trim())) &&
                          (ter.DNI.Contains(dni) || String.IsNullOrEmpty(dni.Trim())) 
                           // tipoficha || tipoficha == 0
                           && (ter.IDETAPA == idEtapa || idEtapa == 0)
                           && (ter.ID_SUBPROGRAMA == idSubprograma || idSubprograma == 0)

                          select ter

                  ).ToList();

            return lficha.ToList();

        }

        public List<VT_REPORTES_TER> getfichaTER(string cuil, string dni, string nombre, string apellido, int tipoficha, int estadoficha, int idEtapa, int idSubprograma)
        {

            nombre = nombre ?? String.Empty;
            apellido = apellido ?? String.Empty;
            cuil = cuil ?? String.Empty;
            dni = dni ?? String.Empty;
            return QVT_REPORTES_TER(cuil, dni, nombre, apellido, tipoficha, estadoficha, idEtapa, idSubprograma).ToList();

        }

        private List<VT_REPORTES_UNI> QVT_REPORTES_UNI(string cuil, string dni, string nombre, string apellido, int tipoficha, int estadoficha, int idEtapa, int idSubprograma)
        {

            var lficha = (from uni in _mdb.VT_REPORTES_UNI
                          where

                          (uni.APELLIDO.ToLower().Contains(apellido.ToLower()) || String.IsNullOrEmpty(apellido.Trim())) &&
                          (uni.NOMBRE.ToLower().Contains(nombre.ToLower()) || String.IsNullOrEmpty(nombre.Trim())) &&
                          (uni.ID_EST_FIC == estadoficha || estadoficha == -1) &&
                          (uni.CUIL.Contains(cuil) || String.IsNullOrEmpty(cuil.Trim())) &&
                          (uni.DNI.Contains(dni) || String.IsNullOrEmpty(dni.Trim()))
                              // tipoficha || tipoficha == 0
                           && (uni.IDETAPA == idEtapa || idEtapa == 0)
                           && (uni.ID_SUBPROGRAMA == idSubprograma || idSubprograma == 0)

                          select uni

                  ).ToList();

            return lficha.ToList();

        }

        public List<VT_REPORTES_UNI> getfichaUni(string cuil, string dni, string nombre, string apellido, int tipoficha, int estadoficha, int idEtapa, int idSubprograma)
        {

            nombre = nombre ?? String.Empty;
            apellido = apellido ?? String.Empty;
            cuil = cuil ?? String.Empty;
            dni = dni ?? String.Empty;
            return QVT_REPORTES_UNI(cuil, dni, nombre, apellido, tipoficha, estadoficha, idEtapa, idSubprograma).ToList();

        }

        private List<VT_REPORTES_PPPPROF> QVT_REPORTES_PPPPROF(string cuil, string dni, string nombre, string apellido, int tipoficha, int estadoficha, int idEtapa)
        {

            var lficha = (from prof in _mdb.VT_REPORTES_PPPPROF
                          where

                          (prof.APELLIDO.ToLower().Contains(apellido.ToLower()) || String.IsNullOrEmpty(apellido.Trim())) &&
                          (prof.NOMBRE.ToLower().Contains(nombre.ToLower()) || String.IsNullOrEmpty(nombre.Trim())) &&
                          (prof.ID_EST_FIC == estadoficha || estadoficha == -1) &&
                          (prof.CUIL.Contains(cuil) || String.IsNullOrEmpty(cuil.Trim())) &&
                          (prof.NUMERO_DOCUMENTO.Contains(dni) || String.IsNullOrEmpty(dni.Trim()))
                              // tipoficha || tipoficha == 0
                           && (prof.IDETAPA == idEtapa || idEtapa == 0)


                          select prof

                  ).ToList();

            return lficha.ToList();

        }

        public List<VT_REPORTES_PPPPROF> getfichaProf(string cuil, string dni, string nombre, string apellido, int tipoficha, int estadoficha, int idEtapa)
        {

            nombre = nombre ?? String.Empty;
            apellido = apellido ?? String.Empty;
            cuil = cuil ?? String.Empty;
            dni = dni ?? String.Empty;
            return QVT_REPORTES_PPPPROF(cuil, dni, nombre, apellido, tipoficha, estadoficha, idEtapa).ToList();

        }

        private List<VT_REPORTES_VAT> QVT_REPORTES_VAT(string cuil, string dni, string nombre, string apellido, int tipoficha, int estadoficha, int idEtapa)
        {

            var lficha = (from vat in _mdb.VT_REPORTES_VAT
                          where

                          (vat.APELLIDO.ToLower().Contains(apellido.ToLower()) || String.IsNullOrEmpty(apellido.Trim())) &&
                          (vat.NOMBRE.ToLower().Contains(nombre.ToLower()) || String.IsNullOrEmpty(nombre.Trim())) &&
                          (vat.ID_EST_FIC == estadoficha || estadoficha == -1) &&
                          (vat.CUIL.Contains(cuil) || String.IsNullOrEmpty(cuil.Trim())) &&
                          (vat.NUMERO_DOCUMENTO.Contains(dni) || String.IsNullOrEmpty(dni.Trim()))
                              // tipoficha || tipoficha == 0
                           && (vat.IDETAPA == idEtapa || idEtapa == 0)


                          select vat

                  ).ToList();

            return lficha.ToList();

        }

        public List<VT_REPORTES_VAT> getfichaVat(string cuil, string dni, string nombre, string apellido, int tipoficha, int estadoficha, int idEtapa)
        {

            nombre = nombre ?? String.Empty;
            apellido = apellido ?? String.Empty;
            cuil = cuil ?? String.Empty;
            dni = dni ?? String.Empty;
            return QVT_REPORTES_VAT(cuil, dni, nombre, apellido, tipoficha, estadoficha, idEtapa).ToList();

        }

        private List<VT_REPORTES_REC_PROD> QVT_REPORTES_REC_PROD(string cuil, string dni, string nombre, string apellido, int tipoficha, int estadoficha, int idEtapa)
        {

            var lficha = (from reco in _mdb.VT_REPORTES_REC_PROD
                          where

                          (reco.APELLIDO.ToLower().Contains(apellido.ToLower()) || String.IsNullOrEmpty(apellido.Trim())) &&
                          (reco.NOMBRE.ToLower().Contains(nombre.ToLower()) || String.IsNullOrEmpty(nombre.Trim())) &&
                          (reco.ID_EST_FIC == estadoficha || estadoficha == -1) &&
                          (reco.CUIL.Contains(cuil) || String.IsNullOrEmpty(cuil.Trim())) &&
                          (reco.NUMERO_DOCUMENTO.Contains(dni) || String.IsNullOrEmpty(dni.Trim()))
                              // tipoficha || tipoficha == 0
                           && (reco.IDETAPA == idEtapa || idEtapa == 0)


                          select reco

                  ).ToList();

            return lficha.ToList();

        }

        public List<VT_REPORTES_REC_PROD> getfichaRec_Prod(string cuil, string dni, string nombre, string apellido, int tipoficha, int estadoficha, int idEtapa)
        {

            nombre = nombre ?? String.Empty;
            apellido = apellido ?? String.Empty;
            cuil = cuil ?? String.Empty;
            dni = dni ?? String.Empty;
            return QVT_REPORTES_REC_PROD(cuil, dni, nombre, apellido, tipoficha, estadoficha, idEtapa).ToList();

        }

        private List<VT_REPORTES_EFEC_SOC> QVT_REPORTES_EFEC_SOC(string cuil, string dni, string nombre, string apellido, int tipoficha, int estadoficha, int idEtapa)
        {

            var lficha = (from efe in _mdb.VT_REPORTES_EFEC_SOC
                          where

                          (efe.APELLIDO.ToLower().Contains(apellido.ToLower()) || String.IsNullOrEmpty(apellido.Trim())) &&
                          (efe.NOMBRE.ToLower().Contains(nombre.ToLower()) || String.IsNullOrEmpty(nombre.Trim())) &&
                          (efe.ID_EST_FIC == estadoficha || estadoficha == -1) &&
                          (efe.CUIL.Contains(cuil) || String.IsNullOrEmpty(cuil.Trim())) &&
                          (efe.NUMERO_DOCUMENTO.Contains(dni) || String.IsNullOrEmpty(dni.Trim()))
                              // tipoficha || tipoficha == 0
                           && (efe.IDETAPA == idEtapa || idEtapa == 0)


                          select efe

                  ).ToList();

            return lficha.ToList();

        }

        public List<VT_REPORTES_EFEC_SOC> getfichaEfec_Soc(string cuil, string dni, string nombre, string apellido, int tipoficha, int estadoficha, int idEtapa)
        {

            nombre = nombre ?? String.Empty;
            apellido = apellido ?? String.Empty;
            cuil = cuil ?? String.Empty;
            dni = dni ?? String.Empty;
            return QVT_REPORTES_EFEC_SOC(cuil, dni, nombre, apellido, tipoficha, estadoficha, idEtapa).ToList();

        }

        private List<VT_REPORTES_CONF_VOS> QVT_REPORTES_CONF_VOS(string cuil, string dni, string nombre, string apellido, int tipoficha, int estadoficha, int idEtapa)
        {

            var lficha = (from CON in _mdb.VT_REPORTES_CONF_VOS
                          where

                          (CON.APELLIDO.ToLower().Contains(apellido.ToLower()) || String.IsNullOrEmpty(apellido.Trim())) &&
                          (CON.NOMBRE.ToLower().Contains(nombre.ToLower()) || String.IsNullOrEmpty(nombre.Trim())) &&
                          (CON.ID_EST_FIC == estadoficha || estadoficha == -1) &&
                          (CON.CUIL.Contains(cuil) || String.IsNullOrEmpty(cuil.Trim())) &&
                          (CON.NUMERO_DOCUMENTO.Contains(dni) || String.IsNullOrEmpty(dni.Trim()))
                            
                              // tipoficha || tipoficha == 0
                           && (CON.IDETAPA == idEtapa || idEtapa == 0)


                          select CON

                  ).ToList();

            return lficha.ToList();

        }

        public List<VT_REPORTES_CONF_VOS> getfichaConfVos(string cuil, string dni, string nombre, string apellido, int tipoficha, int estadoficha, int idEtapa)
        {

            nombre = nombre ?? String.Empty;
            apellido = apellido ?? String.Empty;
            cuil = cuil ?? String.Empty;
            dni = dni ?? String.Empty;
            return QVT_REPORTES_CONF_VOS(cuil, dni, nombre, apellido, tipoficha, estadoficha, idEtapa).ToList();

        }

        public T_FICHAS_CONF_VOS getApoConfVos(int idFicha)
        {
            var apo = _mdb.T_FICHAS_CONF_VOS.Where(x => x.ID_FICHA == idFicha)/*.Select(x => x.APODERADO)*/.SingleOrDefault();

            return apo;
        }

        public int AddApoderado(IApoderado apoderado)
        {
            AgregarDatos(apoderado);

            //obtengo la sucursal id de la tabla sucursales cobertura
            var ss = _mdb.T_LOCALIDADES.FirstOrDefault(x => x.ID_LOCALIDAD == apoderado.IdLocalidad).T_TABLAS_BCO_CBA.Select(x => x.ID_TABLA_BCO_CBA).SingleOrDefault();

            apoderado.IdSucursal = (ss == 0 ? null : (short?)ss);
            var apoderadoModel = new T_APODERADOS
            {
                ID_APODERADO = SecuenciaRepositorio.GetId(),
                ID_BENEFICIARIO = apoderado.IdBeneficiario,
                APELLIDO = (apoderado.Apellido ?? "").ToUpper(),
                NOMBRE = (apoderado.Nombre ?? "").ToUpper(),
                TIPO_DOCUMENTO = apoderado.TipoDocumento,
                NRO_DOCUMENTO = apoderado.NumeroDocumento,
                CUIL = apoderado.Cuil,
                SEXO = apoderado.Sexo,
                ID_SISTEMA = apoderado.IdSistema,
                NRO_CUENTA_BCO = apoderado.NumeroCuentaBco == 0 ? null : apoderado.NumeroCuentaBco,
                CBU = apoderado.Cbu,
                USUARIO_BANCO = apoderado.UsuarioBanco,
                ID_SUCURSAL_BCO = apoderado.IdLocalidad == 25 ? 143 : (apoderado.IdSucursal == 0 ? null : apoderado.IdSucursal),
                ID_MONEDA = Convert.ToInt16(apoderado.IdMoneda),
                APODERADO_DESDE = apoderado.ApoderadoDesde,
                APODERADO_HASTA = apoderado.ApoderadoHasta,
                ID_ESTADO_APODERADO = apoderado.IdEstadoApoderado,
                FER_NAC = apoderado.FechaNacimiento,
                CALLE = (apoderado.Calle ?? "").ToUpper(),
                NUMERO = apoderado.Numero,
                PISO = apoderado.Piso,
                DPTO = apoderado.Dpto,
                MONOBLOCK = apoderado.Monoblock,
                PARCELA = apoderado.Parcela,
                MANZANA = apoderado.Manzana,
                ENTRECALLES = (apoderado.EntreCalles ?? "").ToUpper(),
                BARRIO = apoderado.Barrio,
                CODIGO_POSTAL = apoderado.CodigoPostal,
                ID_LOCALIDAD = apoderado.IdLocalidad,
                TEL_FIJO = apoderado.TelefonoFijo,
                TEL_CELULAR = apoderado.TelefonoCelular,
                MAIL = apoderado.Mail,
                ID_USR_SIST = apoderado.IdUsuarioSistema,
                FEC_SIST = apoderado.FechaSistema
            };

            _mdb.T_APODERADOS.AddObject(apoderadoModel);
            _mdb.SaveChanges();

            //UpdateFlagTieneApoderado(apoderado);

            return apoderadoModel.ID_APODERADO;
        }

        public IFichaConfVos GetFormFichaConfVos()
        {
            var fichareconfvos = new FichaConfVos();

            return fichareconfvos;
        }

        //11/06/2014
        public bool UpdateFichaEtapa(int idFicha, int idEtapa)
        {
            //AgregarDatos(ficha);

            var obj = _mdb.T_FICHAS.FirstOrDefault(c => c.ID_FICHA == idFicha);
            obj.ID_ETAPA = idEtapa;
            
            _mdb.SaveChanges();

            return true;
        }

        // 17/06/2014

        private IQueryable<IFicha> QFichaEmpresa()
        {
            var a = (from c in _mdb.T_FICHAS
                     from b in _mdb.T_BENEFICIARIOS.Where(b => b.ID_FICHA == c.ID_FICHA).DefaultIfEmpty()
                     from eb in _mdb.T_ESTADOS.Where(eb => eb.ID_ESTADO == b.ID_ESTADO).DefaultIfEmpty()
                     from eta in _mdb.T_ETAPAS.Where(eta => eta.ID_ETAPA == c.ID_ETAPA).DefaultIfEmpty()
                     from tf in _mdb.T_TIPO_FICHA.Where(tf => tf.ID_TIPO_FICHA == c.TIPO_FICHA).DefaultIfEmpty()
                     from ef in _mdb.T_ESTADOS_FICHA.Where(ef => ef.ID_ESTADO_FICHA == c.ID_ESTADO_FICHA).DefaultIfEmpty()
                     select
                         new Ficha
                         {
                             Apellido = c.APELLIDO,
                             Barrio = c.BARRIO,
                             Localidad = c.T_LOCALIDADES.N_LOCALIDAD,
                             IdLocalidad = c.ID_LOCALIDAD ?? 0,
                             IdFicha = c.ID_FICHA,
                             Calle = c.CALLE,
                             CantidadHijos = c.CANTIDAD_HIJOS,
                             CertificadoDiscapacidad = c.CERTIF_DISCAP == "S" ? true : false,
                             CodigoPostal = c.CODIGO_POSTAL,
                             CodigoSeguridad = c.CODIGO_SEGURIDAD,
                             Contacto = c.CONTACTO,
                             Cuil = c.CUIL,
                             DeficienciaOtra = c.DEFICIENCIA_OTRA,
                             TieneDeficienciaMental = c.TIENE_DEF_MENTAL == "S" ? true : false,
                             TieneDeficienciaMotora = c.TIENE_DEF_MOTORA == "S" ? true : false,
                             TieneDeficienciaPsicologia = c.TIENE_DEF_PSICOLOGICA == "S" ? true : false,
                             TieneDeficienciaSensorial = c.TIENE_DEF_SENSORIAL == "S" ? true : false,
                             Dpto = c.DPTO,
                             EntreCalles = c.ENTRECALLES,
                             EsDiscapacitado = c.ES_DISCAPACITADO == "S" ? true : false,
                             EstadoCivil = c.ESTADO_CIVIL,
                             FechaNacimiento = c.FER_NAC,
                             Mail = c.MAIL,
                             Manzana = c.MANZANA,
                             Monoblock = c.MONOBLOCK,
                             Nombre = c.NOMBRE,
                             Numero = c.NUMERO,
                             NumeroDocumento = c.NUMERO_DOCUMENTO,
                             Parcela = c.PARCELA,
                             Piso = c.PISO,
                             Sexo = c.SEXO,
                             TelefonoCelular = c.TEL_CELULAR,
                             TelefonoFijo = c.TEL_FIJO,
                             TieneHijos = c.TIENE_HIJOS == "S" ? true : false,
                             TipoDocumento = c.TIPO_DOCUMENTO,
                             TipoFicha = c.TIPO_FICHA,
                             NombreTipoFicha = tf.N_TIPO_FICHA,
                             IdEstadoFicha = c.ID_ESTADO_FICHA ?? (int)Enums.EstadoFicha.SinEstado,
                             NombreEstadoFicha = ef.N_ESTADO_FICHA ?? "",
                             FechaSistema = c.FEC_SIST,
                             IdUsuarioSistema = c.ID_USR_SIST,
                             UsuarioSistema = c.T_USUARIOS.LOGIN,
                             idEtapa = c.ID_ETAPA ?? 0,
                             NombreEtapa = eta.N_ETAPA ?? "",
                             
                             BeneficiarioFicha = new Beneficiario
                             {
                                 IdBeneficiario = b.ID_BENEFICIARIO == null ? 0 : b.ID_BENEFICIARIO,
                                 IdEstado = eb.ID_ESTADO == null ? 0 : eb.ID_ESTADO,
                                 NombreEstado = eb.N_ESTADO
                             }

                         });

            return a;
        }

        // 08/01/2017
        public int ReEmpadronarFicha(int IdFicha)
        {


            try
            {
                //AgregarDatos(ficha);

                var ficha = _mdb.T_FICHAS.FirstOrDefault(c => c.ID_FICHA == IdFicha);


                int id = SecuenciaRepositorio.GetId();



                var fichaModel = new T_FICHAS
                {
                    ID_FICHA = id,
                    APELLIDO = (ficha.APELLIDO ?? "").ToUpper(),
                    BARRIO = ficha.BARRIO.ToUpper(),
                    CALLE = (ficha.CALLE ?? "").ToUpper(),
                    CANTIDAD_HIJOS = ficha.CANTIDAD_HIJOS,
                    CERTIF_DISCAP = ficha.CERTIF_DISCAP,
                    CODIGO_POSTAL = ficha.CODIGO_POSTAL,
                    CODIGO_SEGURIDAD = ficha.CODIGO_SEGURIDAD,
                    CONTACTO = ficha.CONTACTO,
                    CUIL = ficha.CUIL,
                    DEFICIENCIA_OTRA = ficha.DEFICIENCIA_OTRA,
                    DPTO = ficha.DPTO,
                    ENTRECALLES = (ficha.ENTRECALLES ?? "").ToUpper(),
                    ID_LOCALIDAD = ficha.ID_LOCALIDAD,
                    MAIL = ficha.MAIL,
                    ES_DISCAPACITADO = ficha.ES_DISCAPACITADO,
                    ESTADO_CIVIL = ficha.ESTADO_CIVIL,
                    FER_NAC = ficha.FER_NAC,
                    MANZANA = ficha.MANZANA,
                    MONOBLOCK = ficha.MONOBLOCK,
                    NOMBRE = (ficha.NOMBRE ?? "").ToUpper(),
                    NUMERO = ficha.NUMERO,
                    NUMERO_DOCUMENTO = ficha.NUMERO_DOCUMENTO,
                    PARCELA = ficha.PARCELA,
                    PISO = ficha.PISO,
                    SEXO = ficha.SEXO,
                    TEL_CELULAR = ficha.TEL_CELULAR,
                    TEL_FIJO = ficha.TEL_FIJO,
                    TIENE_DEF_MENTAL = ficha.TIENE_DEF_MENTAL,
                    TIENE_DEF_MOTORA = ficha.TIENE_DEF_MOTORA,
                    TIENE_DEF_PSICOLOGICA = ficha.TIENE_DEF_PSICOLOGICA,
                    TIENE_DEF_SENSORIAL = ficha.TIENE_DEF_SENSORIAL,
                    TIENE_HIJOS = ficha.TIENE_HIJOS,
                    TIPO_DOCUMENTO = ficha.TIPO_DOCUMENTO,
                    TIPO_FICHA = Convert.ToInt16(ficha.TIPO_FICHA),
                    ID_ESTADO_FICHA = (int)Enums.EstadoFicha.Inscripto,
                    ID_USR_SIST = GetUsuarioLoguer().Id_Usuario,
                    FEC_SIST = DateTime.Now,
                    NRO_TRAMITE = ficha.NRO_TRAMITE,
                    ID_BARRIO = ficha.ID_BARRIO,
                };

                _mdb.T_FICHAS.AddObject(fichaModel);




                switch (ficha.TIPO_FICHA)
                {

                    case (int)Enums.TipoFicha.Ppp:
                        var objPpp = _mdb.T_FICHA_PPP.FirstOrDefault(c => c.ID_FICHA == IdFicha);
                        var pppModel = new T_FICHA_PPP
                                           {
                                               ID_FICHA = id,
                                               CURSANDO = objPpp.CURSANDO,
                                               DESEA_TERM_NIVEL = objPpp.DESEA_TERM_NIVEL,
                                               FINALIZADO = objPpp.FINALIZADO,
                                               ID_NIVEL_ESCOLARIDAD = objPpp.ID_NIVEL_ESCOLARIDAD,
                                               MODALIDAD = objPpp.MODALIDAD,
                                               ID_EMPRESA = objPpp.ID_EMPRESA,
                                               TAREAS = objPpp.TAREAS,
                                               ALTA_TEMPRANA = objPpp.ALTA_TEMPRANA,
                                               ID_USR_SIST = objPpp.ID_USR_SIST,
                                               FEC_SIST = DateTime.Now,
                                               AT_FECHA_INICIO = objPpp.AT_FECHA_INICIO,
                                               AT_FECHA_CESE = objPpp.AT_FECHA_CESE,
                                               ID_MOD_CONT_AFIP = objPpp.ID_MOD_CONT_AFIP == 0 ? null : objPpp.ID_MOD_CONT_AFIP,
                                               ID_SEDE = (objPpp.ID_SEDE == 0 ? null : objPpp.ID_SEDE),
                                               //ID_SUBPROGRAMA = objPpp.ID_SUBPROGRAMA == 0 ? null : objPpp.ID_SUBPROGRAMA,
                                               ID_ONG = objPpp.ID_ONG == 0 ? null : objPpp.ID_ONG,
                                               ID_ESCUELA = objPpp.ID_ESCUELA,
                                               ID_CURSO = objPpp.ID_CURSO == 0 ? null : objPpp.ID_CURSO,
                                               PPP_APRENDIZ = objPpp.PPP_APRENDIZ ?? "N",
                                               TIPO_PPP = objPpp.TIPO_PPP,
                                               HORARIO_ROTATIVO = objPpp.HORARIO_ROTATIVO ?? "N",
                                               SEDE_ROTATIVA = objPpp.SEDE_ROTATIVA ?? "N",
                                               
                                           };

                        _mdb.T_FICHA_PPP.AddObject(pppModel);

                        break;


                    default:
                        break;
                }

                //Buscar los familiares de la ficha y crear los nuevos registros para la nueva
                var familiares = from f in _mdb.T_RELACIONES_FAM
                                 from v in _mdb.T_VINCULOS.Where(v => v.ID_VINCULO == f.ID_VINCULO).DefaultIfEmpty()
                                 from p in _mdb.T_PERSONAS.Where(p => p.ID_PERSONA == f.ID_PERSONA).DefaultIfEmpty()
                                 where f.ID_FICHA == IdFicha
                                 select new RelacionFam
                                 {
                                     ID_FICHA = IdFicha,
                                     ID_VINCULO = v.ID_VINCULO,
                                     N_VINCULO = v.N_VINCULO,
                                     ID_PERSONA = f.ID_PERSONA,
                                     APELLIDO = p.APELLIDO,
                                     NOMBRE = p.NOMBRE,
                                     DNI = p.DNI

                                 };

                foreach (RelacionFam f in familiares)
                {
                    var familiarModel = new T_RELACIONES_FAM
                    {
                        ID_FICHA = id,
                        ID_PERSONA = (int)f.ID_PERSONA,
                        ID_VINCULO = f.ID_VINCULO,
                        ID_USR_SIST = GetUsuarioLoguer().Id_Usuario,
                        FEC_SIST = DateTime.Now,
                    };

                    _mdb.T_RELACIONES_FAM.AddObject(familiarModel);

                }


                _mdb.SaveChanges();

                return id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
                //return 0;
            }

        }
    }
}