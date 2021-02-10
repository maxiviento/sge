using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Transactions;
using System.Web;
using NPOI.HSSF.UserModel;
using SGE.Model.Comun;
using SGE.Model.Entidades;
using SGE.Model.Entidades.Interfaces;
using SGE.Model.Repositorio;
using SGE.Repositorio;
using SGE.Servicio.ServicioInterfaces;
using SGE.Servicio.Vista;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces;
using SGE.Servicio.Comun;
using System.Linq;
using Excel;
using SGE.Repositorio.Modelo;

namespace SGE.Servicio.Servicio
{
    public class FichaServicio : BaseServicio, IFichaServicio
    {
        private readonly IFichaRepositorio _fichaRepositorio;
        private readonly IAutenticacionServicio _aut;
        private readonly ILocalidadRepositorio _localidadRepositorio;
        private readonly IDepartamentoRepositorio _departamentoRepositorio;
        private readonly IInstitucionRepositorio _institucionRepositorio;
        private readonly ICarreraRepositorio _carreraRepositorio;
        private readonly IEscuelaRepositorio _escuelaRepositorio;
        private readonly IEstadoCivilRepositorio _estadoCivilRepositorio;
        private readonly IEstadoFichaRepositorio _estadoFichaRepositorio;
        private readonly ITipoFichaRepositorio _tipoFichaRepositorio;
        private readonly IFichaRechazoRepositorio _fichaRechazoRepositorio;
        private readonly ITipoRechazoRepositorio _tipoRechazoRepositorio;
        private readonly IEmpresaRepositorio _empresaRepositorio;
        private readonly IBeneficiarioRepositorio _beneficiarioRepositorio;
        private readonly ICuentaBancoRepositorio _cuentaBancoRepositorio;
        private readonly IModalidadContratacionAFIPRepositorio _modalidadContratacionAfipRepositorio;
        private readonly ITituloRepositorio _tituloRepositorio;
        private readonly IUniversidadRepositorio _universidadRepositorio;
        private readonly IHorarioEmpresaRepositorio _horarioempresarepositorio;
        private readonly IHorarioFichaRepositorio _horarioficharepositorio;
        private readonly ISubprogramaRepositorio _subprogramaRepositorio;
        private readonly IProyectoRepositorio _proyectoRepositorio;
        private readonly IRolFormularioAccionRepositorio _rolformularioaccionrepositorio;
        private readonly ISectorRepositorio _sectorrepositorio; //23/02/2013 - DI CAMPLI LEANDRO - QUITAR HARCODEO DE SECTOR PRODUCTIVO
        private readonly ISucursalRepositorio _sucursalrepositorio; // 09/04/2013 - DI CAMPLI LEANDRO - REGISTRAR LA SUCURSAL SEGUN LOCALIDAD DE LA FICHA

        private readonly IProgramaRepositorio _programarepositorio; // 10/06/2013 - DI CAMPLI LEANDRO
        private readonly EtapaRepositorio _etaparepositorio; // 10/06/2013 - DI CAMPLI LEANDRO

        private readonly FichaRepositorio _FichaRepo;

        private readonly IUsuarioRolRepositorio _usuarioRolRepositorio;
        private readonly ICursoRepositorio _cursoRepositorio;
        private readonly IAbandonoCursadoRepositorio _abandonocursadoRepositorio;
        private readonly IActorRepositorio _actorRepositorio;
        private readonly IOngRepositorio _ongRepositorio;
        private readonly IAsistenciasRepositorio _asistenciasRepositorio;
        private readonly IRelFamiliarRepositorio _relfamiliaresRepositorio;

        private readonly ITipoPppRepositorio _tipopppRepositorio;

        public FichaServicio()
        {
            _fichaRepositorio = new FichaRepositorio();
            _aut = new AutenticacionServicio();
            _localidadRepositorio = new LocalidadRepositorio();
            _departamentoRepositorio = new DepartamentoRepositorio();
            _institucionRepositorio = new InstitucionRepositorio();
            _carreraRepositorio = new CarreraRepositorio();
            _escuelaRepositorio = new EscuelaRepositorio();
            _estadoCivilRepositorio = new EstadoCivilRepositorio();
            _estadoFichaRepositorio = new EstadoFichaRepositorio();
            _tipoFichaRepositorio = new TipoFichaRepositorio();
            _tipoRechazoRepositorio = new TipoRechazoRepositorio();
            _fichaRechazoRepositorio = new FichaRechazoRepositorio();
            _empresaRepositorio = new EmpresaRepositorio();
            _beneficiarioRepositorio = new BeneficiarioRepositorio();
            _cuentaBancoRepositorio = new CuentaBancoRepositorio();
            _modalidadContratacionAfipRepositorio = new ModalidadContratacionAFIPRepositorio();
            _tituloRepositorio = new TituloRepositorio();
            _universidadRepositorio = new UniversidadRepositorio();
            _horarioempresarepositorio = new HorarioEmpresaRepositorio();
            _horarioficharepositorio = new HorarioFichaRepositorio();
            _subprogramaRepositorio = new SubprogramaRepositorio();
            _proyectoRepositorio = new ProyectoRepositorio();
            _rolformularioaccionrepositorio = new RolFormularioAccionRepositorio();

            _sectorrepositorio = new SectorRepositorio(); // 23/02/2013 - DI CAMPLI LEAMDRO - cambia el origen del control de harcode a consulta
            _sucursalrepositorio = new SucursalRepositorio(); // 09/04/2013 - DI CAMPLI LEANDRO - REGISTRAR LA SUCURSAL SEGUN LOCALIDAD DE LA FICHA
            _programarepositorio = new ProgramaRepositorio();
            _etaparepositorio = new EtapaRepositorio();

            _FichaRepo = new FichaRepositorio();
            _usuarioRolRepositorio = new UsuarioRolRepositorio();
            _cursoRepositorio = new CursoRepositorio();
            _abandonocursadoRepositorio = new AbandonoCursadoRepositorio();
            _actorRepositorio = new ActorRepositorio();
            _ongRepositorio = new OngRepositorio();
            _asistenciasRepositorio = new AsistenciasRepositorio();
            _relfamiliaresRepositorio = new RelFamiliarRepositorio();

            _tipopppRepositorio = new TipoPppRepositorio();
        }

        public IFichasVista GetFichas()
        {
            IFichasVista vista = new FichasVista();

            var pager = new Pager(_fichaRepositorio.GetCountFichas(), Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexFichas", _aut.GetUrl("IndexPager", "Fichas"));

            vista.pager = pager;

            vista.Fichas = _fichaRepositorio.GetFichas(pager.Skip, pager.PageSize);

            CargarEstadosFichas(vista, _estadoFichaRepositorio.GetEstadosFicha());

            CargarTipoFichas(vista, _tipoFichaRepositorio.GetTiposFicha());

            return vista;
        }

        public IFichasVista GetIndex()
        {
            IFichasVista vista = new FichasVista();

            IList<IFicha> lista = new List<IFicha>();

            var pager = new Pager(0, Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexFichas", _aut.GetUrl("IndexPager", "Fichas"));

            vista.pager = pager;

            vista.Fichas = lista;

            CargarEstadosFichas(vista, _estadoFichaRepositorio.GetEstadosFicha());
            
            
            //**************CARGAR EL COMBO PROGRAMAS***********************************
            // 22/08/2014 - controlar aca si tiene el rol activado para condicionar los programas y subprogramas.
            var accesoprograma =
                                  _rolformularioaccionrepositorio.GetFormulariosDelUsuario(base.GetUsuarioLoguer().Id_Usuario).Where(
                                      c =>
                                      c.Id_Accion == (int)Enums.Acciones.ProgramasPorRol &&
                                      c.Id_Formulario == (int)Enums.Formulario.AbmFicha).Count() > 0
                                      ? true
                                      : false;
            var usuarioRoles = _usuarioRolRepositorio.GetRolesDelUsuario(base.GetUsuarioLoguer().Id_Usuario);

            if (accesoprograma) //verifica si debe traer todos los programas asociados
            {
                vista.ProgPorRol = "S";

                var lprogramas = from rp in _programarepositorio.GetTipoFichasRol()
                                 join u in usuarioRoles on rp.Id_Rol equals u.IdRol
                                 group rp by new {rp.IdPrograma,rp.NombrePrograma } into grouping
                                select new 
                                    TipoFicha
                                    {
                                        Id_Tipo_Ficha = grouping.Key.IdPrograma,
                                        Nombre_Tipo_Ficha = grouping.Key.NombrePrograma
                                    };

                CargarTipoFichasRol(vista, lprogramas.OrderBy(c=>c.Id_Tipo_Ficha));
            }
            else
            {
                vista.ProgPorRol = "N";
                CargarTipoFichas(vista, _tipoFichaRepositorio.GetTiposFicha());
            }
            //*****************************************************************************************
            
            
            var lstProgramas = _programarepositorio.GetProgramas();

            // 10/06/2013 - DI CAMPLI LEANDRO - CARGAN LOS PROGRAMAS PARA LAS ETAPAS
            CargarProgramaEtapa(vista, lstProgramas);

            vista.idFormulario = "FormIndexFichas";

            var tiposppp = _tipopppRepositorio.GetTiposPpp();
            vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = 0, Description = "Todos" });
            foreach (var tipo in tiposppp)
            {
                vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = tipo.ID_TIPO_PPP, Description = tipo.N_TIPO_PPP });
            }

            //vista.ComboTiposPpp.Combo.Add(new ComboItem {Id = 0, Description = "Todos"});
            //vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = 1, Description = "PRIMER PASO" });
            //vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = 2, Description = "APRENDIZ" });
            //vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = 3, Description = "XMÍ" });
            //vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = 4, Description = "PILA" });
            //vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = 5, Description = "PIP" });
            //vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = 6, Description = "CLIP" });
            //vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = 7, Description = "PIL - COMERCIO EXTERIOR" });
            //vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = 8, Description = "PIL - COMERCIO ELECTRONICO" });
            //vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = 9, Description = "PIL - MAQUINARIA AGRICOLA" });
            //vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = 10, Description = "PIL - TURISMO" });
            return vista;
        }

        public IFichasVista GetFichas(IPager pager)
        {
            IFichasVista vista = new FichasVista
                                     {
                                         pager = pager,
                                         Fichas = _fichaRepositorio.GetFichas(pager.Skip, pager.PageSize)
                                     };

            CargarEstadosFichas(vista, _estadoFichaRepositorio.GetEstadosFicha());

            CargarTipoFichas(vista, _tipoFichaRepositorio.GetTiposFicha());

            return vista;
        }

        public IFichaVista GetFicha(int idFicha, string accion)
        {
            IFichaVista vista = new FichaVista();

            IFicha objFicha = _fichaRepositorio.GetFicha(idFicha);
            vista.Nombre = objFicha.Nombre;
            vista.Apellido = objFicha.Apellido;
            vista.Barrio = objFicha.Barrio;
            vista.Calle = objFicha.Calle;
            vista.CantidadHijos = objFicha.TieneHijos ? (objFicha.CantidadHijos ?? 0) : 0;
            vista.CertificadoDiscapacidad = objFicha.CertificadoDiscapacidad;
            vista.CodigoPostal = objFicha.CodigoPostal;
            vista.CodigoSeguridad = objFicha.CodigoSeguridad;
            vista.Contacto = objFicha.Contacto;
            vista.Cuil = objFicha.Cuil;
            vista.DeficienciaOtra = objFicha.DeficienciaOtra;
            vista.Dpto = objFicha.Dpto;
            vista.EntreCalles = objFicha.EntreCalles;
            vista.EsDiscapacitado = objFicha.EsDiscapacitado;
            vista.EstadoCivilDesc = Enums.GetEstadoCivil(objFicha.EstadoCivil ?? 0);
            vista.FechaNacimineto = objFicha.FechaNacimiento ?? DateTime.Now;
            vista.IdFicha = objFicha.IdFicha;
            vista.IdLocalidad = objFicha.IdLocalidad;
            vista.Mail = objFicha.Mail;
            vista.Manzana = objFicha.Manzana;
            vista.Monoblock = objFicha.Monoblock;
            vista.NombreTipoFicha = objFicha.NombreTipoFicha;
            vista.Numero = objFicha.Numero;
            vista.NumeroDocumento = objFicha.NumeroDocumento;
            vista.Parcela = objFicha.Parcela;
            vista.Piso = objFicha.Piso;
            vista.TelefonoCelular = objFicha.TelefonoCelular;
            vista.TelefonoFijo = objFicha.TelefonoFijo;
            vista.TieneDeficienciaMental = objFicha.TieneDeficienciaMental;
            vista.TieneDeficienciaMotora = objFicha.TieneDeficienciaMotora;
            vista.TieneDeficienciaPsicologia = objFicha.TieneDeficienciaPsicologia;
            vista.TieneDeficienciaSensorial = objFicha.TieneDeficienciaSensorial;
            vista.TieneHijos = objFicha.TieneHijos;
            vista.TipoDocumento = objFicha.TipoDocumento ?? 0;
            vista.TipoFicha = objFicha.TipoFicha ?? 0;
            vista.NombreTipoFicha = Enums.GetTipoFicha(objFicha.TipoFicha ?? 0);
            vista.IdInstitucionTerciaria = objFicha.IdInstitucionTerciaria ?? 0;
            vista.IdLocalidadEscTerc = objFicha.IdLocalidadEscTerc ?? 0;
            vista.IdInstitucionUniversitaria = objFicha.IdInstitucionUniversitaria ?? 0;
            vista.IdLocalidadEscUniv = objFicha.IdLocalidadEscUniv ?? 0;
            vista.IdEstadoFicha = objFicha.IdEstadoFicha;
            vista.Accion = accion;
            // 16/08/2016 
            vista.NroTramite = objFicha.NroTramite;
            vista.NombreEtapa = objFicha.NombreEtapa;
            vista.idBarrio = objFicha.idBarrio;

            CargarSexo(vista);
            CargarTipoDocumento(vista);
            CargarDepartamento(vista, _departamentoRepositorio.GetDepartamentos());
            CargarLocalidad(vista, _localidadRepositorio.GetLocalidades());

            CargarSectorProductivo(vista, _sectorrepositorio.GetSectores());// 23/02/2013 - DI CAMPLI LEANDRO - QUITAR HARDCODE DE SECTOR PRODUCTIVO
            CargarInstitucion(vista, _institucionRepositorio.GetInstitucionesByNivel(objFicha.TipoFicha ?? 0));
            CargarEstadoCivil(vista, _estadoCivilRepositorio.GetEstadosCivil());
            CargarEstadosFicha(vista, _estadoFichaRepositorio.GetEstadosFicha());

            CargarTipoFichaAlta(vista, _tipoFichaRepositorio.GetTiposFicha());
            CargarEstadosFichaAlta(vista);

            CargarComboSiNo(vista);

            // apoderado
            CargarLocalidadApo(vista, _localidadRepositorio.GetLocalidades());

            switch (vista.TipoFicha)
            {
                case (int)Enums.TipoFicha.Terciaria:
                    // 11/12/2016 - Nueva consulta para fichas terciarias
                    vista.FichaTER = _fichaRepositorio.GetFichaTer(objFicha.IdFicha);
                                 objFicha.IdCarreraTerciaria = vista.FichaTER.IdCarreraTerciaria;
                                 objFicha.IdInstitucionTerciaria = vista.FichaTER.IdInstitucionTerciaria;
                                 objFicha.IdSectorTerciaria = vista.FichaTER.IdSectorTerciaria;
                                 objFicha.IdEscuelaTerciaria = vista.FichaTER.IdEscuelaTerciaria;
                                 objFicha.PromedioTerciaria = vista.FichaTER.PromedioTerciaria;
                                 objFicha.OtraCarreraTerciaria = vista.FichaTER.OtraCarreraTerciaria;
                                 objFicha.OtraInstitucionTerciaria = vista.FichaTER.OtraInstitucionTerciaria;
                                 objFicha.OtroSectorTerciaria = vista.FichaTER.OtroSectorTerciaria;
                                 objFicha.IdDepartamentoEscTerc = vista.FichaTER.IdDepartamentoEscTerc;
                                 objFicha.IdLocalidadEscTerc = vista.FichaTER.IdLocalidadEscTerc;


                    CargarCarrera(vista,
                                  _carreraRepositorio.GetCarrerasByInstitucion(objFicha.IdInstitucionTerciaria));
                    CargarEscuela(vista, _escuelaRepositorio.GetEscuelasByLocalidad(objFicha.IdLocalidadEscTerc));
                    CargarLocalidadEscuela(vista, _localidadRepositorio.GetLocalidadesByDepto(Convert.ToInt32(objFicha.IdDepartamentoEscTerc)));

                    if (objFicha.IdSectorTerciaria != 0)
                    {
                        vista.SectorProductivo.Selected = objFicha.IdSectorTerciaria.ToString();
                        vista.Institucion.Selected = objFicha.IdInstitucionTerciaria.ToString();
                        vista.Carrera.Selected = objFicha.IdCarreraTerciaria.ToString();
                    }
                    else
                    {
                        vista.SectorProductivo.Selected = "0";
                        vista.Institucion.Selected = "0";
                        vista.Carrera.Selected = "0";
                        vista.OtraCarrera = objFicha.OtraCarreraTerciaria;
                        vista.OtraInstitucion = objFicha.OtraInstitucionTerciaria;
                        vista.OtroSector = objFicha.OtroSectorTerciaria;
                    }

                    vista.DepartamentosEscuela.Selected = objFicha.IdDepartamentoEscTerc.ToString();
                    vista.LocalidadesEscuela.Selected = objFicha.IdLocalidadEscTerc.ToString();
                    vista.Escuelas.Selected = objFicha.IdEscuelaTerciaria.ToString();
                    vista.Promedio = objFicha.PromedioTerciaria;
                    vista.IdConvertirA = (int)Enums.TipoFicha.Universitaria;
                    vista.ConvertirA = "Universitaria";

                    // SUBPROGRAMA
                    ISubprograma subpTer = new Subprograma();
                    if (!((vista.FichaTER.IdSubprograma ?? 0) == 0))
                    {
                        subpTer = _subprogramaRepositorio.GetSubprograma((int)vista.FichaTER.IdSubprograma);
                    }
                    if (subpTer.IdSubprograma > 0)
                    {
                        vista.IdSubprograma = subpTer.IdSubprograma;
                        vista.Subprograma = subpTer.NombreSubprograma;
                        vista.monto = subpTer.monto;
                    }


                    break;
                case (int)Enums.TipoFicha.Universitaria:
                    // 11/12/2016 - Nueva consulta para fichas terciarias
                    vista.FichaUNI = _fichaRepositorio.GetFichaUni(objFicha.IdFicha);
                                 objFicha.IdCarreraUniversitaria = vista.FichaUNI.IdCarreraUniversitaria;
                                 objFicha.IdInstitucionUniversitaria = vista.FichaUNI.IdInstitucionUniversitaria;
                                 objFicha.IdSectorUniversitaria = vista.FichaUNI.IdSectorUniversitaria;
                                 objFicha.IdEscuelaUniversitaria = vista.FichaUNI.IdEscuelaUniversitaria;
                                 objFicha.PromedioUniversitaria = vista.FichaUNI.PromedioUniversitaria;
                                 objFicha.OtraCarreraUniversitaria = vista.FichaUNI.OtraCarreraUniversitaria;
                                 objFicha.OtraInstitucionUniversitaria = vista.FichaUNI.OtraInstitucionUniversitaria;
                                 objFicha.OtroSectorUniversitaria = vista.FichaUNI.OtroSectorUniversitaria;
                                 objFicha.IdDepartamentoEscUniv = vista.FichaUNI.IdDepartamentoEscUniv;
                                 objFicha.IdLocalidadEscUniv = vista.FichaUNI.IdLocalidadEscUniv;

                    CargarCarrera(vista,
                                  _carreraRepositorio.GetCarrerasByInstitucion(objFicha.IdInstitucionUniversitaria));
                    CargarEscuela(vista, _escuelaRepositorio.GetEscuelasByLocalidad(objFicha.IdLocalidadEscUniv));
                    CargarLocalidadEscuela(vista, _localidadRepositorio.GetLocalidadesByDepto(Convert.ToInt32(objFicha.IdDepartamentoEscUniv)));

                    if (objFicha.IdSectorUniversitaria != 0)
                    {
                        vista.SectorProductivo.Selected = objFicha.IdSectorUniversitaria.ToString();
                        vista.Institucion.Selected = objFicha.IdInstitucionUniversitaria.ToString();
                        vista.Carrera.Selected = objFicha.IdCarreraUniversitaria.ToString();
                    }
                    else
                    {
                        vista.SectorProductivo.Selected = "0";
                        vista.Institucion.Selected = "0";
                        vista.Carrera.Selected = "0";
                        vista.OtraCarrera = objFicha.OtraCarreraUniversitaria;
                        vista.OtraInstitucion = objFicha.OtraInstitucionUniversitaria;
                        vista.OtroSector = objFicha.OtroSectorUniversitaria;
                    }

                    vista.DepartamentosEscuela.Selected = objFicha.IdDepartamentoEscUniv.ToString();
                    vista.LocalidadesEscuela.Selected = objFicha.IdLocalidadEscUniv.ToString();
                    vista.Escuelas.Selected = objFicha.IdEscuelaUniversitaria.ToString();
                    vista.Promedio = objFicha.PromedioUniversitaria;
                    vista.IdConvertirA = (int)Enums.TipoFicha.Terciaria;
                    vista.ConvertirA = "Terciaria";

                    // SUBPROGRAMA
                    ISubprograma subpUni = new Subprograma();
                    if (!((vista.FichaUNI.IdSubprograma ?? 0) == 0))
                    {
                        subpUni = _subprogramaRepositorio.GetSubprograma((int)vista.FichaUNI.IdSubprograma);
                    }
                    if (subpUni.IdSubprograma > 0)
                    {
                        vista.IdSubprograma = subpUni.IdSubprograma;
                        vista.Subprograma = subpUni.NombreSubprograma;
                        vista.monto = subpUni.monto;
                    }

                    break;
                case (int)Enums.TipoFicha.Ppp:
                    vista.FichaPpp = _fichaRepositorio.GetFichaPpp(objFicha.IdFicha);
                    vista.IdSede = vista.FichaPpp.IdSede ?? 0;
                    vista.IdEmpresa = vista.FichaPpp.IdEmpresa;
                    vista.IdEmpresaInicial = vista.FichaPpp.IdEmpresa;
                    CargarLocalidadEmpresa(vista, _localidadRepositorio.GetLocalidades());
                    vista.LocalidadesEmpresa.Selected = (vista.FichaPpp.Empresa.IdLocalidad ?? 0).ToString();
                    CargarLocalidadSede(vista, _localidadRepositorio.GetLocalidades());
                    vista.LocalidadesSedes.Selected = (vista.FichaPpp.Sede.IdLocalidad ?? 0).ToString();
                    CargarModalidadAfip(vista, _modalidadContratacionAfipRepositorio.GetModalidadesContratacionAFIP());
                    vista.ComboModalidadAfip.Selected = (vista.FichaPpp.IdModalidadAfip ?? 0).ToString();
                    //01/08/2018
                    int? sector = int.Parse(string.IsNullOrEmpty(ConfigurationManager.AppSettings["sector"]) ? "0" : ConfigurationManager.AppSettings["sector"]);
                    CargarInstitucionSector(vista, _institucionRepositorio.GetInstitucionesBySector((int)sector));
                    vista.InstitucionSector.Selected = "0";
                    //Buscar el IdInstitucion por carrea
                    if ((int)vista.FichaPpp.id_carrera != 0)
                    {

                        ICarrera carrera = new Carrera();
                        carrera = _carreraRepositorio.GetCarrera((int)vista.FichaPpp.id_carrera);


                        if (carrera.IdInstitucion != null)
                        {
                            vista.InstitucionSector.Selected = carrera.IdInstitucion.ToString();
                        }
                        else
                        {
                            vista.InstitucionSector.Selected = "0";
                        }


                        CargarCarreraSector(vista,
                                      _carreraRepositorio.GetCarrerasByInstitucion(carrera.IdInstitucion));
                        vista.CarreraSector.Selected = vista.FichaPpp.id_carrera.ToString();
                        carrera = null;
                    }
                    else
                    {
                        vista.CarreraSector.Combo.Add(new ComboItem { Id = 0, Description = "SELECCIONE UNA CARRERA..." });
                    }
                    // ONG
                    vista.FichaPpp.ong = _ongRepositorio.GetOng((int)vista.FichaPpp.ID_ONG);
                    // ESCUELA
                    //vista.FichaPpp.Id_Escuela = objFicha.FichaPpp.Id_Escuela;
                    IEscuela escuelaPpp = new Escuela();
                    if (!((vista.FichaPpp.Id_Escuela ?? 0) == 0))
                    {
                        escuelaPpp = _escuelaRepositorio.GetEscuelaCompleto((int)vista.FichaPpp.Id_Escuela);
                    }
                    if (escuelaPpp.Id_Escuela > 0)
                    {
                        vista.FichaPpp.N_ESCUELA = escuelaPpp.Nombre_Escuela ?? "";
                        vista.FichaPpp.CALLEEsc = escuelaPpp.CALLE ?? "";
                        vista.FichaPpp.NUMEROEsc = escuelaPpp.NUMERO ?? "";
                        vista.FichaPpp.TELEFONOEsc = escuelaPpp.TELEFONO ?? "";
                        vista.FichaPpp.CueEsc = escuelaPpp.Cue ?? "";
                    }
                    // SUBPROGRAMA
                    ISubprograma subp = new Subprograma();
                    if (!((vista.FichaPpp.IdSubprograma ?? 0) == 0))
                    {
                        subp = _subprogramaRepositorio.GetSubprograma((int)vista.FichaPpp.IdSubprograma);
                    }
                    if (subp.IdSubprograma > 0)
                    {
                        vista.IdSubprograma = subp.IdSubprograma;
                        vista.FichaPpp.IdSubprograma = subp.IdSubprograma;
                        vista.FichaPpp.Subprograma = subp.NombreSubprograma;
                        vista.FichaPpp.monto = subp.monto;
                    }
                    //Cargar datos del curso
                    if ((vista.FichaPpp.ID_CURSO ?? 0) != 0)
                    {
                        vista.FichaPpp.cursoPpp = _cursoRepositorio.getCurso((int)vista.FichaPpp.ID_CURSO);
                    }

                        var tiposppp = _tipopppRepositorio.GetTiposPpp();
                        foreach (var tipo in tiposppp)
                        {
                            vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = tipo.ID_TIPO_PPP, Description = tipo.N_TIPO_PPP });
                        }

                        //vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = 1, Description = "PRIMER PASO" });
                        //vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = 2, Description = "APRENDIZ" });
                        //vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = 3, Description = "XMÍ" });
                        //vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = 4, Description = "PILA" });
                        //vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = 5, Description = "PIP" });
                        //vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = 6, Description = "CLIP" });
                        //vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = 7, Description = "PIL - COMERCIO EXTERIOR" });
                        //vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = 8, Description = "PIL - COMERCIO ELECTRONICO" });
                        //vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = 9, Description = "PIL - MAQUINARIA AGRICOLA" });
                        //vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = 10, Description = "PIL - TURISMO" });

                        vista.ComboTiposPpp.Selected = Convert.ToString(vista.FichaPpp.TIPO_PPP ?? 1);
                        vista.tipoPrograma = vista.FichaPpp.TIPO_PPP ?? 1;
                        vista.TipoProgramaAnt = vista.tipoPrograma;
                    // 20/01/2017 - Traer grupo familiar
                        vista.familiares = _relfamiliaresRepositorio.GetFamiliares(idFicha);

                        // 25/09/2018   
                        vista.SENAF = vista.FichaPpp.SENAF;

                    CargarHorarioLunes(vista);
                    CargarHorarioMartes(vista);
                    CargarHorarioMiercoles(vista);
                    CargarHorarioJueves(vista);
                    CargarHorarioViernes(vista);
                    CargarHorarioSabado(vista);
                    CargarHorarioDomingo(vista);
                    break;
                case (int)Enums.TipoFicha.Vat:
                    vista.FichaVat = _fichaRepositorio.GetFichaVat(objFicha.IdFicha);
                    vista.IdEmpresa = vista.FichaVat.IdEmpresa;
                    vista.IdEmpresaInicial = vista.FichaVat.IdEmpresa;
                    vista.IdSede = vista.FichaVat.IdSede ?? 0;
                    CargarModalidadAfip(vista, _modalidadContratacionAfipRepositorio.GetModalidadesContratacionAFIP());
                    vista.ComboModalidadAfip.Selected = (vista.FichaVat.IdModalidadAFIP ?? 0).ToString();
                    CargarHorarioLunes(vista);
                    CargarHorarioMartes(vista);
                    CargarHorarioMiercoles(vista);
                    CargarHorarioJueves(vista);
                    CargarHorarioViernes(vista);
                    CargarHorarioSabado(vista);
                    CargarHorarioDomingo(vista);
                    break;
                case (int)Enums.TipoFicha.PppProf:
                    vista.FichaPppp = _fichaRepositorio.GetFichaPppp(objFicha.IdFicha);
                    vista.IdEmpresa = vista.FichaPppp.IdEmpresa;
                    vista.IdEmpresaInicial = vista.FichaPppp.IdEmpresa;
                    vista.IdSede = vista.FichaPppp.IdSede ?? 0;
                    CargarModalidadAfip(vista, _modalidadContratacionAfipRepositorio.GetModalidadesContratacionAFIP());
                    vista.ComboModalidadAfip.Selected = (vista.FichaPppp.IdModalidadAFIP ?? 0).ToString();
                    CargarHorarioLunes(vista);
                    CargarHorarioMartes(vista);
                    CargarHorarioMiercoles(vista);
                    CargarHorarioJueves(vista);
                    CargarHorarioViernes(vista);
                    CargarHorarioSabado(vista);
                    CargarHorarioDomingo(vista);

                    if (vista.FichaPppp.IdTitulo != null || vista.FichaPppp.IdTitulo != 0)
                    {
                        ITitulo titulo = _tituloRepositorio.GetTitulo(vista.FichaPppp.IdTitulo ?? 0);
                        CargarUniversidades(vista, _universidadRepositorio.GetUniversidades());
                        CargarTitulos(vista, _tituloRepositorio.GetTitulosByUniversidad(titulo == null ? 0 : titulo.IdUniversidad ?? 0));
                        vista.ComboTitulos.Selected = vista.FichaPppp.IdTitulo.ToString();
                        vista.ComboUniversidad.Selected = titulo == null ? "0" : titulo.IdUniversidad.ToString();
                    }
                    else
                    {
                        IList<IUniversidad> listauniversidad = _universidadRepositorio.GetUniversidades();
                        if (listauniversidad.Count > 0)
                        {
                            int iduniversidad = listauniversidad[0].IdUniversidad;

                            CargarTitulos(vista, _tituloRepositorio.GetTitulosByUniversidad(iduniversidad));
                        }

                        CargarUniversidades(vista, listauniversidad);
                    }
                    break;
                case (int)Enums.TipoFicha.ReconversionProductiva:
                    vista.FichaReconversion = _fichaRepositorio.GetFichaReconversion(objFicha.IdFicha);
                    vista.IdSede = vista.FichaReconversion.IdSede ?? 0;
                    vista.IdEmpresa = vista.FichaReconversion.IdEmpresa;
                    vista.IdEmpresaInicial = vista.FichaReconversion.IdEmpresa;
                    CargarLocalidadEmpresa(vista, _localidadRepositorio.GetLocalidades());
                    vista.LocalidadesEmpresa.Selected = (vista.FichaReconversion.Empresa.IdLocalidad ?? 0).ToString();
                    CargarLocalidadSede(vista, _localidadRepositorio.GetLocalidades());
                    vista.LocalidadesSedes.Selected = (vista.FichaReconversion.Sede.IdLocalidad ?? 0).ToString();
                    CargarModalidadAfip(vista, _modalidadContratacionAfipRepositorio.GetModalidadesContratacionAFIP());
                    vista.ComboModalidadAfip.Selected = (vista.FichaReconversion.IdModalidadAfip ?? 0).ToString();
                    vista.IdProyecto = vista.FichaReconversion.IdProyecto;
                    CargarSubprogramas(vista, _subprogramaRepositorio.GetSubprogramas());
                    vista.Subprogramas.Selected = (vista.FichaReconversion.IdSubprograma ?? 0).ToString();
                    vista.NombreProyecto = vista.FichaReconversion.Proyecto.NombreProyecto;
                    vista.CantidadAcapacitar = vista.FichaReconversion.Proyecto.CantidadAcapacitar;
                    vista.FechaInicioProyecto = vista.FichaReconversion.Proyecto.FechaInicioProyecto;
                    vista.FechaFinProyecto = vista.FichaReconversion.Proyecto.FechaFinProyecto;
                    vista.MesesDuracionProyecto = vista.FichaReconversion.Proyecto.MesesDuracionProyecto;
                    vista.AportesDeLaEmpresa = vista.FichaReconversion.AportesDeLaEmpresa;
                    vista.NombreTutor = vista.FichaReconversion.NombreTutor;
                    vista.ApellidoTutor = vista.FichaReconversion.ApellidoTutor;
                    vista.NroDocumentoTutor = vista.NroDocumentoTutor;
                    vista.TelefonoTutor = vista.FichaReconversion.TelefonoTutor;
                    vista.EmailTutor = vista.FichaReconversion.EmailTutor;
                    vista.PuestoTutor = vista.FichaReconversion.PuestoTutor;
                    CargarHorarioLunes(vista);
                    CargarHorarioMartes(vista);
                    CargarHorarioMiercoles(vista);
                    CargarHorarioJueves(vista);
                    CargarHorarioViernes(vista);
                    CargarHorarioSabado(vista);
                    CargarHorarioDomingo(vista);
                    break;
                case (int)Enums.TipoFicha.EfectoresSociales:
                    vista.FichaEfectores = _fichaRepositorio.GetFichaEfectores(objFicha.IdFicha);
                    vista.IdSede = vista.FichaEfectores.IdSede ?? 0;
                    vista.IdEmpresa = vista.FichaEfectores.IdEmpresa;
                    vista.IdEmpresaInicial = vista.FichaEfectores.IdEmpresa;
                    CargarLocalidadEmpresa(vista, _localidadRepositorio.GetLocalidades());
                    vista.LocalidadesEmpresa.Selected = (vista.FichaEfectores.Empresa.IdLocalidad ?? 0).ToString();
                    CargarLocalidadSede(vista, _localidadRepositorio.GetLocalidades());
                    vista.LocalidadesSedes.Selected = (vista.FichaEfectores.Sede.IdLocalidad ?? 0).ToString();
                    CargarModalidadAfip(vista, _modalidadContratacionAfipRepositorio.GetModalidadesContratacionAFIP());
                    vista.ComboModalidadAfip.Selected = (vista.FichaEfectores.IdModalidadAfip ?? 0).ToString();
                    vista.IdProyecto = vista.FichaEfectores.IdProyecto;
                    CargarSubprogramas(vista, _subprogramaRepositorio.GetSubprogramas());
                    vista.Subprogramas.Selected = (vista.FichaEfectores.IdSubprograma ?? 0).ToString();
                    vista.NombreProyecto = vista.FichaEfectores.Proyecto.NombreProyecto;
                    vista.CantidadAcapacitar = vista.FichaEfectores.Proyecto.CantidadAcapacitar;
                    vista.FechaInicioProyecto = vista.FichaEfectores.Proyecto.FechaInicioProyecto;
                    vista.FechaFinProyecto = vista.FichaEfectores.Proyecto.FechaFinProyecto;
                    vista.MesesDuracionProyecto = vista.FichaEfectores.Proyecto.MesesDuracionProyecto;
                    vista.AportesDeLaEmpresa = vista.FichaEfectores.AportesDeLaEmpresa;
                    vista.NombreTutor = vista.FichaEfectores.NombreTutor;
                    vista.ApellidoTutor = vista.FichaEfectores.ApellidoTutor;
                    vista.NroDocumentoTutor = vista.NroDocumentoTutor;
                    vista.TelefonoTutor = vista.FichaEfectores.TelefonoTutor;
                    vista.EmailTutor = vista.FichaEfectores.EmailTutor;
                    vista.PuestoTutor = vista.FichaEfectores.PuestoTutor;
                    vista.monto = vista.FichaEfectores.monto;
                    CargarHorarioLunes(vista);
                    CargarHorarioMartes(vista);
                    CargarHorarioMiercoles(vista);
                    CargarHorarioJueves(vista);
                    CargarHorarioViernes(vista);
                    CargarHorarioSabado(vista);
                    CargarHorarioDomingo(vista);
                    break;
                case (int)Enums.TipoFicha.ConfiamosEnVos:
                    vista.FichaConfVos = _fichaRepositorio.GetFichaConfVos(objFicha.IdFicha);
                    vista.IdSede = vista.FichaConfVos.IdSede ?? 0;
                    vista.IdEmpresa = vista.FichaConfVos.IdEmpresa;
                    vista.IdEmpresaInicial = vista.FichaConfVos.IdEmpresa;
                    CargarLocalidadEmpresa(vista, _localidadRepositorio.GetLocalidades());
                    vista.LocalidadesEmpresa.Selected = (vista.FichaConfVos.Empresa.IdLocalidad ?? 0).ToString();
                    CargarLocalidadSede(vista, _localidadRepositorio.GetLocalidades());
                    vista.LocalidadesSedes.Selected = (vista.FichaConfVos.Sede.IdLocalidad ?? 0).ToString();
                    CargarModalidadAfip(vista, _modalidadContratacionAfipRepositorio.GetModalidadesContratacionAFIP());
                    vista.ComboModalidadAfip.Selected = (vista.FichaConfVos.IdModalidadAfip ?? 0).ToString();
                    vista.IdProyecto = vista.FichaConfVos.IdProyecto;
                    CargarSubprogramas(vista, _subprogramaRepositorio.GetSubprogramas());
                    vista.Subprogramas.Selected = (vista.FichaConfVos.IdSubprograma ?? 0).ToString();
                    vista.NombreProyecto = vista.FichaConfVos.Proyecto.NombreProyecto;
                    vista.CantidadAcapacitar = vista.FichaConfVos.Proyecto.CantidadAcapacitar;
                    vista.FechaInicioProyecto = vista.FichaConfVos.Proyecto.FechaInicioProyecto;
                    vista.FechaFinProyecto = vista.FichaConfVos.Proyecto.FechaFinProyecto;
                    vista.MesesDuracionProyecto = vista.FichaConfVos.Proyecto.MesesDuracionProyecto;
                    vista.AportesDeLaEmpresa = vista.FichaConfVos.AportesDeLaEmpresa;
                    vista.NombreTutor = vista.FichaConfVos.NombreTutor;
                    vista.ApellidoTutor = vista.FichaConfVos.ApellidoTutor;
                    vista.NroDocumentoTutor = vista.NroDocumentoTutor;
                    vista.TelefonoTutor = vista.FichaConfVos.TelefonoTutor;
                    vista.EmailTutor = vista.FichaConfVos.EmailTutor;
                    vista.PuestoTutor = vista.FichaConfVos.PuestoTutor;

                    vista.TipoDocumentoApoCombo.Selected = vista.FichaConfVos.Apo_Tipo_Documento.ToString();
                    vista.SexoApo.Selected = (vista.FichaConfVos.Apo_Sexo ?? "VARON").ToUpper() == "MUJER" ? ((int)Enums.Sexo.Mujer).ToString() : ((int)Enums.Sexo.Varon).ToString();
                    vista.LocalidadesApo.Selected = vista.FichaConfVos.Apo_Id_Localidad.ToString();

                    vista.DepartamentosApo.Selected = (_localidadRepositorio.GetLocalidad(vista.FichaConfVos.Apo_Id_Localidad == null ? 0 : (int)vista.FichaConfVos.Apo_Id_Localidad).IdDepartamento).ToString();
                    vista.EstadoCivilApo.Selected = (vista.FichaConfVos.Apo_Estado_Civil ?? 0).ToString();
                    //vista.FichaConfVos.Id_Escuela = objFicha.IdEscuelaConfVos;
                    // primero obtener el id_escuela
                    //int idesc = _escuelaRepositorio.GetEscuelas().Where(x => x.Id_Escuela == (int)vista.FichaConfVos.Id_Escuela).Select(x=>x.Id_Localidad).SingleOrDefault() ?? 0;
                    vista.FichaConfVos.escuela = _escuelaRepositorio.GetEscuelaCompleto((int)vista.FichaConfVos.Id_Escuela);//_escuelaRepositorio.GetEscuelasCompleto().Where(x => x.Id_Escuela == (int)vista.FichaConfVos.Id_Escuela).SingleOrDefault();
                    
                    int idesc = 0;
                    if (vista.FichaConfVos.escuela != null)
                    {
                        
                        idesc = vista.FichaConfVos.escuela.Id_Localidad ?? 0;
                    }

                    vista.IdLocalidadEscUniv = idesc;    
                    //Buscar el departamento correspondiente
                    int idDepartamento=0;
                    idDepartamento = _localidadRepositorio.GetLocalidades().Where(x => x.IdLocalidad == vista.IdLocalidadEscUniv).Select(x => x.IdDepartamento).SingleOrDefault() ?? 0;

                    //Cargar las localidades del departamento
                    CargarLocalidadEscuela(vista, _localidadRepositorio.GetLocalidadesByDepto(Convert.ToInt32(idDepartamento)));
                    CargarEscuela(vista, _escuelaRepositorio.GetEscuelasByLocalidad(vista.IdLocalidadEscUniv));

                    vista.DepartamentosEscuela.Selected = idDepartamento.ToString();
                    vista.LocalidadesEscuela.Selected = vista.IdLocalidadEscUniv.ToString();
                    vista.Escuelas.Selected = vista.FichaConfVos.Id_Escuela.ToString();
                    //cargar todos los datos de escuela

                    //Cargar datos del curso
                    if ((vista.FichaConfVos.ID_CURSO ?? 0) != 0)
                    {
                        vista.FichaConfVos.curso = _cursoRepositorio.getCurso((int)vista.FichaConfVos.ID_CURSO);
                    }
                    //

                    //Cargar periodos de abandono de cursado
                    CargarAbandonoCursado(vista, _abandonocursadoRepositorio.GetAllcbo());
                    vista.AbandonoCursado.Selected = vista.FichaConfVos.ID_ABANDONO_CUR.ToString();
                    // 25/09/2018   
                    vista.SENAF = vista.FichaConfVos.SENAF;
                    
                    /*vista.ID_CURSO = ficha.FichaConfVos.curso.ID_CURSO;
                         vista.ID_ABANDONO_CUR = ficha.AbandonoCursado.Selected == "0" ? null : Convert.ToInt32(ficha.AbandonoCursado.Selected);
                         vista.TRABAJO_CONDICION = ficha.FichaConfVos.TRABAJO_CONDICION;
                         vista.SENAF = ficha.FichaConfVos.SENAF;
                         vista.PERTENECE_ONG = ficha.FichaConfVos.PERTENECE_ONG;
                         vista.ID_ONG = ficha.FichaConfVos.ID_ONG;
                         vista.APO_CELULAR = ficha.FichaConfVos.APO_CELULAR;
                         vista.APO_MAIL = ficha.FichaConfVos.APO_MAIL;*/
                    //***************************
                    //vista.Abandono_Escuela = vista.FichaConfVos.Abandono_Escuela;
                    //vista.Trabaja_Trabajo = vista.FichaConfVos.Trabaja_Trabajo;
                    //vista.cursa=vista.FichaConfVos.cursa;
                    //vista.Ultimo_Cursado=vista.FichaConfVos.Ultimo_Cursado;
                    //vista.pit=vista.FichaConfVos.pit;
                    //vista.Centro_Adultos=vista.FichaConfVos.Centro_Adultos;
                    //vista.Progresar=vista.FichaConfVos.Progresar;
                    //vista.Actividades=vista.FichaConfVos.Actividades;
                    //vista.Benef_progre=vista.FichaConfVos.Benef_progre;
                    //vista.Autoriza_Tutor=vista.FichaConfVos.Autoriza_Tutor;
                    //vista.Apoderado=vista.FichaConfVos.Apoderado;
                    //vista.Apo_Cuil = vista.FichaConfVos.Apo_Cuil;
                    //vista.Apo_Apellido =vista.FichaConfVos.Apo_Apellido;
                    //vista.Apo_Nombre =vista.FichaConfVos.Apo_Nombre;
                    //vista.Apo_Fer_Nac =vista.FichaConfVos.Apo_Fer_Nac;
                    //vista.Apo_Tipo_Documento =vista.FichaConfVos.Apo_Tipo_Documento;
                    //vista.Apo_Numero_Documento =vista.FichaConfVos.Apo_Numero_Documento;
                    //vista.Apo_Calle =vista.FichaConfVos.Apo_Calle;
                    //vista.Apo_Numero =vista.FichaConfVos.Apo_Numero;
                    //vista.Apo_Piso =vista.FichaConfVos.Apo_Piso;
                    //vista.Apo_Dpto =vista.FichaConfVos.Apo_Dpto;
                    //vista.Apo_Monoblock =vista.FichaConfVos.Apo_Monoblock;
                    //vista.Apo_Parcela =vista.FichaConfVos.Apo_Parcela;
                    //vista.Apo_Manzana  =vista.FichaConfVos.Apo_Manzana;
                    //vista.Apo_Barrio  =vista.FichaConfVos.Apo_Barrio;
                    //vista.Apo_Codigo_Postal =vista.FichaConfVos.Apo_Codigo_Postal;
                    //vista.Apo_Id_Localidad  =vista.FichaConfVos.Apo_Id_Localidad;
                    //vista.Apo_Telefono  =vista.FichaConfVos.Apo_Telefono;
                    //vista.Apo_Tiene_Hijos   =vista.FichaConfVos.Apo_Tiene_Hijos;
                    //vista.Apo_Cantidad_Hijos    =vista.FichaConfVos.Apo_Cantidad_Hijos ?? 0;
                    //vista.Apo_Sexo    =vista.FichaConfVos.Apo_Sexo;
                    //vista.Apo_Estado_Civil    =vista.FichaConfVos.Apo_Estado_Civil;

                    //***************************

                    vista.FichaConfVos.facilitador_monitor = _actorRepositorio.GetActor((int)vista.FichaConfVos.ID_FACILITADOR).SingleOrDefault();
                    vista.FichaConfVos.gestor = _actorRepositorio.GetActor((int)vista.FichaConfVos.ID_GESTOR).SingleOrDefault();
                    vista.FichaConfVos.ong = _ongRepositorio.GetOng((int)vista.FichaConfVos.ID_ONG);

                    //Traer asistencias
                    if ((vista.FichaConfVos.ID_CURSO ?? 0) != 0)
                    {
                        IList<IAsistencia> asistencias;
                        asistencias =   _asistenciasRepositorio.GetAsistenciasFicha((int)vista.FichaConfVos.ID_CURSO, vista.IdFicha);
                        vista.asistencias = asistencias.OrderBy(x => x.MES).ToList();
                    }

                   
                    CargarHorarioLunes(vista);
                    CargarHorarioMartes(vista);
                    CargarHorarioMiercoles(vista);
                    CargarHorarioJueves(vista);
                    CargarHorarioViernes(vista);
                    CargarHorarioSabado(vista);
                    CargarHorarioDomingo(vista);
                    break;
                default:
                    break;
            }

            vista.TipoDocumentoCombo.Selected = objFicha.TipoDocumento.ToString();
            vista.Sexo.Selected = objFicha.Sexo.ToUpper() == "MUJER" ? ((int)Enums.Sexo.Mujer).ToString() : ((int)Enums.Sexo.Varon).ToString();
            vista.EstadoCivil.Selected = objFicha.EstadoCivil.ToString();
            vista.Departamentos.Selected = objFicha.IdDepartamento.ToString();
            vista.Localidades.Selected = objFicha.IdLocalidad.ToString();
            vista.Estado.Selected = objFicha.IdEstadoFicha.ToString();

            vista.Accion = accion;
            if (accion.Equals("Modificar") || (accion.Equals("Agregar")))
            {
                vista.TipoDocumentoCombo.Enabled = true;
                vista.Sexo.Enabled = true;
                vista.Departamentos.Enabled = true;
                vista.Localidades.Enabled = true;
                vista.SectorProductivo.Enabled = true;
                vista.Institucion.Enabled = true;
                vista.Carrera.Enabled = true;
                vista.DepartamentosEscuela.Enabled = true;
                vista.LocalidadesEscuela.Enabled = true;
                vista.Escuelas.Enabled = true;
                vista.EstadoCivil.Enabled = true;
                vista.ComboTipoFicha.Enabled = true;
                vista.ComboEstadoFicha.Enabled = true;
                vista.ComboModalidadAfip.Enabled = true;
                vista.ComboUniversidad.Enabled = true;
                vista.ComboTitulos.Enabled = true;
                vista.Subprogramas.Enabled = true;

                // 21/04/2014
                vista.TipoDocumentoApoCombo.Enabled = true;
                vista.SexoApo.Enabled = true;
                vista.LocalidadesApo.Enabled = true;
                vista.DepartamentosApo.Enabled = true;
                vista.EstadoCivilApo.Enabled = true;

                // 17/01/2017
                vista.ComboTiposPpp.Enabled = true;

                // 03/08/2018
                vista.CarreraSector.Enabled = true;
                vista.InstitucionSector.Enabled = true;
            }
            else
            {
                vista.TipoDocumentoCombo.Enabled = false;
                vista.Sexo.Enabled = false;
                vista.Departamentos.Enabled = false;
                vista.Localidades.Enabled = false;
                vista.SectorProductivo.Enabled = false;
                vista.Institucion.Enabled = false;
                vista.Carrera.Enabled = false;
                vista.DepartamentosEscuela.Enabled = false;
                vista.LocalidadesEscuela.Enabled = false;
                vista.Escuelas.Enabled = false;
                vista.EstadoCivil.Enabled = false;
                vista.ComboTipoFicha.Enabled = false;
                vista.ComboEstadoFicha.Enabled = false;
                vista.ComboModalidadAfip.Enabled = false;
                vista.ComboUniversidad.Enabled = false;
                vista.ComboTitulos.Enabled = false;
                vista.Subprogramas.Enabled = false;

                // 21/04/2014
                vista.TipoDocumentoApoCombo.Enabled = false;
                vista.SexoApo.Enabled = false;
                vista.LocalidadesApo.Enabled = false;
                vista.DepartamentosApo.Enabled = false;
                vista.EstadoCivilApo.Enabled = false;

                // 17/01/2017
                vista.ComboTiposPpp.Enabled = false;
                // 03/08/2018
                vista.CarreraSector.Enabled = false;
                vista.InstitucionSector.Enabled = false;
                
            }

            vista.TipoRechazos = new List<TipoRechazoVista>();

            if (vista.Accion.Equals("CambiarEstado"))
            {
                vista.TipoRechazos = GetTiposRechazosVista(vista.IdFicha);
            }
            else
            {
                if (vista.Estado.Selected.Equals(((int)Enums.EstadoFicha.RechazoFormal).ToString()))
                {
                    vista.TipoRechazos = GetTiposRechazosVista(vista.IdFicha);
                }
            }

            if (vista.Accion == "Agregar")
            {
                if (vista.ComboEstadoFicha.Selected.Equals(((int)Enums.EstadoFicha.RechazoFormal).ToString()))
                {
                    vista.TipoRechazos = GetTiposRechazosVista(vista.IdFicha);
                }
            }

            //23012013 - DI CAMPLI LEANDRO - AÑADIR CANTIDAD DE BENEFICIARIOS ACTIVOS POR EMPRESA
            //cargar las cantidades de beneficiarios activos en la empresa
            IEmpresaRepositorio _empresaRepositorio = new EmpresaRepositorio();
            vista.benefActivosEmp = _empresaRepositorio.GetBenefActivosEmpCount(vista.IdEmpresa ?? 0, 0/*vista.TipoFicha*/);
            vista.benefActDiscEmp = _empresaRepositorio.GetBenefActDiscEmpCount(vista.IdEmpresa ?? 0, 0/*vista.TipoFicha*/);
            
            vista.benefRetenidoscEmp = _empresaRepositorio.GetBenefRetenidosEmpCount(vista.IdEmpresa ?? 0, 0/*vista.TipoFicha*/);
            vista.benefTotalcEmp = (vista.benefActivosEmp ?? 0) + (vista.benefRetenidoscEmp ?? 0);
            //

            //25012013 - DI CAMPLI LEANDRO - Cargar la fecha de ultima modificación del horario de una ficha

            IHorarioFichaRepositorio _horarioRepositorio = new HorarioFichaRepositorio();
            vista.FechaCambioHorario = _horarioficharepositorio.GetFecUltimaModif(vista.IdFicha);

            //

            return vista;
        }

        public IFichaVista GetFichaIndex()
        {
            IFichaVista vista = new FichaVista();


            return vista;
        }

        public bool UpdateFicha(IFichaVista ficha)
        {
            IComunDatos comun = new ComunDatos(); // 08/04/2013 - DI CAMPLI LEANDRO - SE AÑADE LA FECHA DEL SISTEMA PARA REGISTRAR LA FEC_INICIO COMO BENEFICIARIO
            IApoderado apoderado = new Apoderado();
            IFicha obj = new Ficha
                             {
                                 IdFicha = ficha.IdFicha,
                                 Apellido = ficha.Apellido ?? "*",
                                 Barrio = ficha.Barrio ?? "CENTRO",//"*",
                                 Calle = ficha.Calle,
                                 CantidadHijos = ficha.CantidadHijos,
                                 CertificadoDiscapacidad = ficha.CertificadoDiscapacidad,
                                 CodigoPostal = ficha.CodigoPostal ?? "*",
                                 CodigoSeguridad = ficha.CodigoSeguridad,
                                 Contacto = ficha.Contacto,
                                 Cuil = ficha.Cuil ?? "_-________-_",
                                 DeficienciaOtra = ficha.DeficienciaOtra,
                                 Dpto = ficha.Dpto,
                                 EntreCalles = ficha.EntreCalles,
                                 IdLocalidad = Convert.ToInt32(ficha.Localidades.Selected),
                                 Mail = ficha.Mail ?? "*",
                                 EsDiscapacitado = ficha.EsDiscapacitado,
                                 EstadoCivil = Convert.ToInt32(ficha.EstadoCivil.Selected),
                                 FechaNacimiento = ficha.FechaNacimineto,
                                 Manzana = ficha.Manzana,
                                 Monoblock = ficha.Monoblock,
                                 Nombre = ficha.Nombre ?? "*",
                                 Numero = ficha.Numero,
                                 NumeroDocumento = ficha.NumeroDocumento ?? " ",
                                 Parcela = ficha.Parcela,
                                 Piso = ficha.Piso,
                                 Sexo = ficha.Sexo.Selected == ((int)Enums.Sexo.Mujer).ToString() ? "MUJER" : "VARON",
                                 TelefonoCelular = ficha.TelefonoCelular,
                                 TelefonoFijo = ficha.TelefonoFijo,
                                 TieneDeficienciaMental = ficha.TieneDeficienciaMental,
                                 TieneDeficienciaMotora = ficha.TieneDeficienciaMotora,
                                 TieneDeficienciaPsicologia = ficha.TieneDeficienciaPsicologia,
                                 TieneDeficienciaSensorial = ficha.TieneDeficienciaSensorial,
                                 TieneHijos = ficha.TieneHijos,
                                 TipoDocumento = Convert.ToInt32(ficha.TipoDocumentoCombo.Selected),
                                 TipoFicha = ficha.TipoFicha,
                                 // 17/08/2016
                                 NroTramite = ficha.NroTramite,
                                 idBarrio = ficha.idBarrio == 0 ? null : ficha.idBarrio,


                             };

            switch (obj.TipoFicha)
            {
                case (int)Enums.TipoFicha.Terciaria:
                    if (ficha.Carrera.Selected == "0")
                    {
                        obj.IdCarreraTerciaria = null;
                    }
                    else
                    {
                        obj.IdCarreraTerciaria = Convert.ToInt32(ficha.Carrera.Selected);
                    }

                    if (ficha.Escuelas.Selected == "0")
                    {
                        obj.IdEscuelaTerciaria = null;
                    }
                    else
                    {
                        obj.IdEscuelaTerciaria = Convert.ToInt32(ficha.Escuelas.Selected);
                    }

                    obj.PromedioTerciaria = ficha.Promedio;
                    obj.OtraCarreraTerciaria = ficha.OtraCarrera;
                    obj.OtraInstitucionTerciaria = ficha.OtraInstitucion;
                    obj.OtroSectorTerciaria = ficha.OtroSector;
                    obj.IdSubprograma = ficha.IdSubprograma == 0 ? null : ficha.IdSubprograma;
                    break;
                case (int)Enums.TipoFicha.Universitaria:
                    if (ficha.Carrera.Selected == "0")
                    {
                        obj.IdCarreraUniversitaria = null;
                    }
                    else
                    {
                        obj.IdCarreraUniversitaria = Convert.ToInt32(ficha.Carrera.Selected);
                    }

                    if (ficha.Escuelas.Selected == "0")
                    {
                        obj.IdEscuelaUniversitaria = null;
                    }
                    else
                    {
                        obj.IdEscuelaUniversitaria = Convert.ToInt32(ficha.Escuelas.Selected);
                    }

                    obj.PromedioUniversitaria = ficha.Promedio;
                    obj.OtraCarreraUniversitaria = ficha.OtraCarrera;
                    obj.OtraInstitucionUniversitaria = ficha.OtraInstitucion;
                    obj.OtroSectorUniversitaria = ficha.OtroSector;
                    obj.IdSubprograma = ficha.IdSubprograma == 0 ? null : ficha.IdSubprograma;
                    break;
                case (int)Enums.TipoFicha.Ppp:
                    obj.IdNivelEscolaridad = ficha.FichaPpp.IdNivelEscolaridad;
                    obj.DeseaTermNivel = ficha.FichaPpp.DeseaTermNivel;
                    obj.Cursando = ficha.FichaPpp.Cursando;
                    obj.Finalizado = ficha.FichaPpp.Finalizado;
                    obj.Modalidad = ficha.FichaPpp.Modalidad;
                    obj.AltaTemprana = ficha.FichaPpp.Modalidad == (int)Enums.Modalidad.Entrenamiento
                                           ? "N"
                                           : ficha.FichaPpp.AltaTemprana;
                    obj.Tareas = ficha.FichaPpp.Tareas;
                    obj.IdEmpresa = ficha.IdEmpresa;
                    obj.FechaInicioActividad = ficha.FichaPpp.FechaInicioActividad;
                    obj.FechaFinActividad = ficha.FichaPpp.FechaFinActividad;
                    obj.IdModalidadAfip = Convert.ToInt32(ficha.ComboModalidadAfip.Selected);
                    obj.IdSede = ficha.IdSede;
                    obj.IdSubprograma = ficha.IdSubprograma == 0 ? null : ficha.IdSubprograma;

                    obj.TipoPrograma = ficha.tipoPrograma;
                    obj.TipoProgramaAnt = ficha.TipoProgramaAnt;
                    if (obj.TipoPrograma == 2)
                        {
                            obj.ppp_aprendiz = "S";
                        }
                        else
                        {
                            obj.ppp_aprendiz = "N";
                        }


                    obj.ID_ONG = ficha.FichaPpp.ID_ONG == 0 ? null : ficha.FichaPpp.ID_ONG;
                    
                    // 24/02/2020
                    if (ficha.tipoPrograma == 6)
                    {
                        if (ficha.FichaPpp.IdNivelEscolaridad == 4)
                        {
                            obj.IdEscuelaPpp = ficha.FichaPpp.Id_Escuela == 0 ? null : ficha.FichaPpp.Id_Escuela;
                        }
                        else
                        {
                            obj.IdEscuelaPpp = null;
                        }
                    }
                    else
                    {
                        obj.IdEscuelaPpp = ficha.FichaPpp.Id_Escuela == 0 ? null : ficha.FichaPpp.Id_Escuela;
                    }
                    obj.ID_CURSO = ficha.FichaPpp.ID_CURSO == 0 ? null : ficha.FichaPpp.ID_CURSO;

                    obj.horario_rotativo = ficha.FichaPpp.horario_rotativo;
                    obj.sede_rotativa = ficha.FichaPpp.sede_rotativa;

                    // 05/08/2018

                    obj.egreso = ficha.FichaPpp.egreso;
                    obj.cod_uso_interno = ficha.FichaPpp.cod_uso_interno;
                    obj.carga_horaria = ficha.FichaPpp.carga_horaria;
                    obj.ch_cual = ficha.FichaPpp.ch_cual;
                    // 24/02/2020
                    if (ficha.tipoPrograma == 6)
                    {
                        if (ficha.FichaPpp.IdNivelEscolaridad == 4)
                        {
                            obj.id_carrera = null;
                            obj.n_descripcion_t = "";
                            obj.n_cursado_ins = "";
                        }
                        else
                        {
                            obj.id_carrera = ficha.FichaPpp.id_carrera == 0 ? null : ficha.FichaPpp.id_carrera;
                            obj.n_descripcion_t = ficha.FichaPpp.n_descripcion_t;
                            obj.n_cursado_ins = ficha.FichaPpp.n_cursado_ins;
                        }
                    }
                    else
                    {
                        obj.id_carrera = ficha.FichaPpp.id_carrera == 0 ? null : ficha.FichaPpp.id_carrera;
                        obj.n_descripcion_t = ficha.FichaPpp.n_descripcion_t;
                        obj.n_cursado_ins = ficha.FichaPpp.n_cursado_ins;
                    }
                    obj.CONSTANCIA_EGRESO = ficha.FichaPpp.CONSTANCIA_EGRESO;
                    obj.MATRICULA = ficha.FichaPpp.MATRICULA;
                    obj.CONSTANCIA_CURSO = ficha.FichaPpp.CONSTANCIA_CURSO;
                    obj.co_financiamiento = ficha.FichaPpp.co_financiamiento;

                    // 25/09/2018
                    obj.SENAF = ficha.SENAF;
                    //Agrego Horario---------------------------------------------
                    AddHorarios(ficha);
                    //---------------------------------------------------

                    break;
                case (int)Enums.TipoFicha.Vat:
                    obj.IdNivelEscolaridad = ficha.FichaVat.IdNivelEscolaridad;
                    obj.Cursando = ficha.FichaVat.Cursando;
                    obj.Finalizado = ficha.FichaVat.Finalizado;
                    obj.Modalidad = ficha.FichaVat.Modalidad;
                    obj.AltaTemprana = ficha.FichaVat.Modalidad == (int)Enums.Modalidad.Entrenamiento
                                           ? "N"
                                           : ficha.FichaVat.AltaTemprana;
                    obj.Tareas = ficha.FichaVat.Tareas;
                    obj.IdEmpresa = ficha.IdEmpresa;
                    obj.IdSede = ficha.IdSede;
                    obj.FechaInicioActividad = ficha.FichaVat.Modalidad == (int)Enums.Modalidad.Entrenamiento
                                                   ? null
                                                   : ficha.FichaVat.FechaInicioActividad;
                    obj.FechaFinActividad = ficha.FichaVat.Modalidad == (int)Enums.Modalidad.Entrenamiento
                                                ? null
                                                : ficha.FichaVat.FechaFinActividad;
                    obj.IdModalidadAfip = Convert.ToInt32(ficha.ComboModalidadAfip.Selected);
                    obj.AniosAportes = ficha.FichaVat.AniosAportes;

                    //Agrego Horario---------------------------------------------
                    AddHorarios(ficha);
                    //---------------------------------------------------
                    break;
                case (int)Enums.TipoFicha.PppProf:
                    obj.IdTitulo = ficha.FichaPppp.IdTitulo;
                    obj.Promedio = ficha.FichaPppp.Promedio;
                    obj.FechaEgreso = ficha.FichaPppp.FechaEgreso;
                    obj.Modalidad = ficha.FichaPppp.Modalidad;
                    obj.AltaTemprana = ficha.FichaPppp.Modalidad == (int)Enums.Modalidad.Entrenamiento
                                           ? "N"
                                           : ficha.FichaPppp.AltaTemprana;
                    obj.Tareas = ficha.FichaPppp.Tareas;
                    obj.IdEmpresa = ficha.IdEmpresa;
                    obj.IdSede = ficha.IdSede;
                    obj.FechaInicioActividad = ficha.FichaPppp.Modalidad == (int)Enums.Modalidad.Entrenamiento
                                                   ? null
                                                   : ficha.FichaPppp.FechaInicioActividad;
                    obj.FechaFinActividad = ficha.FichaPppp.Modalidad == (int)Enums.Modalidad.Entrenamiento
                                                ? null
                                                : ficha.FichaPppp.FechaFinActividad;
                    obj.IdModalidadAfip = Convert.ToInt32(ficha.ComboModalidadAfip.Selected);

                    //Agrego Horario---------------------------------------------
                    AddHorarios(ficha);
                    //---------------------------------------------------

                    break;
                case (int)Enums.TipoFicha.ReconversionProductiva:
                    obj.Modalidad = ficha.FichaReconversion.Modalidad;
                    obj.AltaTemprana = ficha.FichaReconversion.Modalidad == (int)Enums.Modalidad.Entrenamiento ? "N" : ficha.FichaReconversion.AltaTemprana;
                    obj.IdEmpresa = ficha.IdEmpresa;
                    obj.FechaInicioActividad = ficha.FichaReconversion.FechaInicioActividad;
                    obj.FechaFinActividad = ficha.FichaReconversion.FechaFinActividad;
                    obj.IdModalidadAfip = Convert.ToInt32(ficha.ComboModalidadAfip.Selected);
                    obj.IdSede = ficha.IdSede;
                    obj.IdSubprograma = Convert.ToInt32(ficha.Subprogramas.Selected);
                    obj.AportesDeLaEmpresa = ficha.FichaReconversion.AportesDeLaEmpresa;
                    obj.ApellidoTutor = ficha.FichaReconversion.ApellidoTutor;
                    obj.NombreTutor = ficha.FichaReconversion.NombreTutor;
                    obj.NroDocumentoTutor = ficha.FichaReconversion.NroDocumentoTutor;
                    obj.TelefonoTutor = ficha.FichaReconversion.TelefonoTutor;
                    obj.EmailTutor = ficha.FichaReconversion.EmailTutor;
                    obj.PuestoTutor = ficha.FichaReconversion.PuestoTutor;
                    obj.IdNivelEscolaridad = ficha.FichaReconversion.IdNivelEscolaridad;
                    obj.DeseaTermNivel = ficha.FichaReconversion.DeseaTermNivel;
                    obj.Cursando = ficha.FichaReconversion.Cursando;
                    obj.Finalizado = ficha.FichaReconversion.Finalizado;
                    // 16/10/2014 - Se normaliza la relacion ficha - proyecto, ahora solo se guarda el idproyecto
                    obj.IdProyecto = ficha.FichaReconversion.Proyecto.IdProyecto;
                    //if (ficha.Accion != Enums.Acciones.CambiarEstado.ToString())
                    //{
                    //    int idProyecto = 0;
                    //    if (ficha.FichaReconversion.Proyecto.IdProyecto != null)
                    //    {
                    //        //if (ficha.FichaReconversion.Proyecto.NombreProyecto != null &&
                    //        //  ficha.FichaReconversion.Proyecto.NombreProyecto.Trim() != "")
                    //        //{
                    //        _proyectoRepositorio.UpdateProyecto(ficha.FichaReconversion.Proyecto);
                    //        idProyecto = ficha.FichaReconversion.Proyecto.IdProyecto.Value;
                    //        //}

                    //    }
                    //    else
                    //    {
                    //        //if (ficha.FichaReconversion.Proyecto.NombreProyecto != null &&
                    //        //    ficha.FichaReconversion.Proyecto.NombreProyecto != " ")
                    //        //{
                    //        idProyecto = _proyectoRepositorio.AddProyecto(ficha.FichaReconversion.Proyecto);
                    //        obj.IdProyecto = idProyecto;
                    //        //}
                    //    }

                    //    obj.NombreProyecto = ficha.FichaReconversion.Proyecto.NombreProyecto;
                    //    obj.CantidadAcapacitar = ficha.FichaReconversion.Proyecto.CantidadAcapacitar;
                    //    obj.FechaInicioProyecto = ficha.FichaReconversion.Proyecto.FechaInicioProyecto;
                    //    obj.FechaFinProyecto = ficha.FichaReconversion.Proyecto.FechaFinProyecto;
                    //    obj.MesesDuracionProyecto = ficha.FichaReconversion.Proyecto.MesesDuracionProyecto;

                    //    if (idProyecto != 0)
                    //        obj.IdProyecto = idProyecto;
                    //}

                    //Agrego Horario---------------------------------------------
                    AddHorarios(ficha);
                    //---------------------------------------------------

                    break;
                case (int)Enums.TipoFicha.EfectoresSociales:
                    obj.Modalidad = ficha.FichaEfectores.Modalidad;
                    obj.AltaTemprana = ficha.FichaEfectores.Modalidad == (int)Enums.Modalidad.Entrenamiento ? "N" : ficha.FichaEfectores.AltaTemprana;
                    obj.IdEmpresa = ficha.IdEmpresa;
                    obj.FechaInicioActividad = ficha.FichaEfectores.FechaInicioActividad;
                    obj.FechaFinActividad = ficha.FichaEfectores.FechaFinActividad;
                    obj.IdModalidadAfip = Convert.ToInt32(ficha.ComboModalidadAfip.Selected);
                    obj.IdSede = ficha.IdSede;
                    obj.IdSubprograma = Convert.ToInt32(ficha.Subprogramas.Selected);
                    obj.AportesDeLaEmpresa = ficha.FichaEfectores.AportesDeLaEmpresa;
                    obj.ApellidoTutor = ficha.FichaEfectores.ApellidoTutor;
                    obj.NombreTutor = ficha.FichaEfectores.NombreTutor;
                    obj.NroDocumentoTutor = ficha.FichaEfectores.NroDocumentoTutor;
                    obj.TelefonoTutor = ficha.FichaEfectores.TelefonoTutor;
                    obj.EmailTutor = ficha.FichaEfectores.EmailTutor;
                    obj.PuestoTutor = ficha.FichaEfectores.PuestoTutor;
                    obj.IdNivelEscolaridad = ficha.FichaEfectores.IdNivelEscolaridad;
                    obj.DeseaTermNivel = ficha.FichaEfectores.DeseaTermNivel;
                    obj.Cursando = ficha.FichaEfectores.Cursando;
                    obj.Finalizado = ficha.FichaEfectores.Finalizado;

                    obj.monto_efe = ficha.monto == null ? 0 : ficha.monto;
                    // 16/10/2014 - Se normaliza la relacion ficha - proyecto, ahora solo se guarda el idproyecto
                    obj.IdProyecto = ficha.FichaEfectores.Proyecto.IdProyecto;
                    //if (ficha.Accion != Enums.Acciones.CambiarEstado.ToString())
                    //{
                    //    int idProyecto = 0;
                    //    if (ficha.FichaEfectores.Proyecto.IdProyecto != null)
                    //    {
                    //        //if (ficha.FichaEfectores.Proyecto.NombreProyecto != null &&
                    //        //  ficha.FichaEfectores.Proyecto.NombreProyecto.Trim() != "")
                    //        //{
                    //        _proyectoRepositorio.UpdateProyecto(ficha.FichaEfectores.Proyecto);
                    //        idProyecto = ficha.FichaEfectores.Proyecto.IdProyecto.Value;
                    //        //}

                    //    }
                    //    else
                    //    {
                    //        ////if (ficha.FichaEfectores.Proyecto.NombreProyecto != null &&
                    //        ////    ficha.FichaEfectores.Proyecto.NombreProyecto != " ")
                    //        ////{
                    //        //idProyecto = _proyectoRepositorio.AddProyecto(ficha.FichaEfectores.Proyecto);
                    //        //obj.IdProyecto = idProyecto;
                    //        ////}
                    //    }

                    //    obj.NombreProyecto = ficha.FichaEfectores.Proyecto.NombreProyecto;
                    //    obj.CantidadAcapacitar = ficha.FichaEfectores.Proyecto.CantidadAcapacitar;
                    //    obj.FechaInicioProyecto = ficha.FichaEfectores.Proyecto.FechaInicioProyecto;
                    //    obj.FechaFinProyecto = ficha.FichaEfectores.Proyecto.FechaFinProyecto;
                    //    obj.MesesDuracionProyecto = ficha.FichaEfectores.Proyecto.MesesDuracionProyecto;
                        

                    //    if (idProyecto != 0)
                    //        obj.IdProyecto = idProyecto;
                    //}

                    //Agrego Horario---------------------------------------------
                    AddHorarios(ficha);
                    //---------------------------------------------------

                    break;

                case (int)Enums.TipoFicha.ConfiamosEnVos:
                    obj.Modalidad = ficha.FichaConfVos.Modalidad;
                    obj.AltaTemprana = ficha.FichaConfVos.Modalidad == (int)Enums.Modalidad.Entrenamiento ? "N" : ficha.FichaConfVos.AltaTemprana;
                    obj.IdEmpresa = ficha.IdEmpresa;
                    obj.FechaInicioActividad = ficha.FichaConfVos.FechaInicioActividad;
                    obj.FechaFinActividad = ficha.FichaConfVos.FechaFinActividad;
                    obj.IdModalidadAfip = Convert.ToInt32(ficha.ComboModalidadAfip.Selected);
                    obj.IdSede = ficha.IdSede;
                    obj.IdSubprograma = Convert.ToInt32(ficha.Subprogramas.Selected);
                    obj.AportesDeLaEmpresa = ficha.FichaConfVos.AportesDeLaEmpresa;
                    obj.ApellidoTutor = ficha.FichaConfVos.ApellidoTutor;
                    obj.NombreTutor = ficha.FichaConfVos.NombreTutor;
                    obj.NroDocumentoTutor = ficha.FichaConfVos.NroDocumentoTutor;
                    obj.TelefonoTutor = ficha.FichaConfVos.TelefonoTutor;
                    obj.EmailTutor = ficha.FichaConfVos.EmailTutor;
                    obj.PuestoTutor = ficha.FichaConfVos.PuestoTutor;
                    obj.IdNivelEscolaridad = ficha.FichaConfVos.IdNivelEscolaridad;
                    obj.DeseaTermNivel = ficha.FichaConfVos.DeseaTermNivel;
                    obj.Cursando = ficha.FichaConfVos.Cursando;
                    obj.Finalizado = ficha.FichaConfVos.Finalizado;

                    // 23/04/2014 - DI CAMPLI LEANDRO - SE DESESTIMA CREAR EL PROYECTO EN CONFIAMOS EN VOS POR EL MOMENTO - SOLO SUBPROGRAMA
                    // 16/10/2014 - Se normaliza la relacion ficha - proyecto, ahora solo se guarda el idproyecto
                    obj.IdProyecto = ficha.FichaConfVos.Proyecto.IdProyecto;
                    //if (ficha.Accion != Enums.Acciones.CambiarEstado.ToString())
                    //{
                    //    int idProyecto = 0;
                    //    if (ficha.FichaConfVos.Proyecto.IdProyecto != null)
                    //    {

                    //        _proyectoRepositorio.UpdateProyecto(ficha.FichaConfVos.Proyecto);
                    //        idProyecto = ficha.FichaConfVos.Proyecto.IdProyecto.Value;

                    //    }
                    //    else
                    //    {

                    //        idProyecto = _proyectoRepositorio.AddProyecto(ficha.FichaConfVos.Proyecto);
                    //        obj.IdProyecto = idProyecto;
                    //    }

                    //    obj.NombreProyecto = ficha.FichaConfVos.Proyecto.NombreProyecto;
                    //    obj.CantidadAcapacitar = ficha.FichaConfVos.Proyecto.CantidadAcapacitar;
                    //    obj.FechaInicioProyecto = ficha.FichaConfVos.Proyecto.FechaInicioProyecto;
                    //    obj.FechaFinProyecto = ficha.FichaConfVos.Proyecto.FechaFinProyecto;
                    //    obj.MesesDuracionProyecto = ficha.FichaConfVos.Proyecto.MesesDuracionProyecto;

                    //    if (idProyecto != 0)
                    //        obj.IdProyecto = idProyecto;


                    //}

                    obj.Abandono_Escuela	=	ficha.FichaConfVos.Abandono_Escuela;
                    obj.Trabaja_Trabajo	=	ficha.FichaConfVos.Trabaja_Trabajo	;
                    obj.cursa	=	ficha.FichaConfVos.cursa;
                    obj.Ultimo_Cursado	=	ficha.FichaConfVos.Ultimo_Cursado	;
                    obj.pit	=	ficha.FichaConfVos.pit	;
                    obj.Centro_Adultos	=	ficha.FichaConfVos.Centro_Adultos	;
                    obj.Progresar	=	ficha.FichaConfVos.Progresar	;
                    obj.Actividades	=	ficha.FichaConfVos.Actividades	;
                    obj.Benef_progre	=	ficha.FichaConfVos.Benef_progre	;
                    obj.Autoriza_Tutor	=	ficha.FichaConfVos.Autoriza_Tutor	;
                    obj.Apoderado	=	ficha.FichaConfVos.Apoderado	;
					
                    obj.Apo_Cuil	=	ficha.FichaConfVos.Apo_Cuil	;
                    obj.Apo_Apellido	=	ficha.FichaConfVos.Apo_Apellido	;
                    obj.Apo_Nombre	=	ficha.FichaConfVos.Apo_Nombre	;
                    obj.Apo_Fer_Nac	=	ficha.FichaConfVos.Apo_Fer_Nac	;
                    obj.Apo_Tipo_Documento = Convert.ToInt32(ficha.TipoDocumentoApoCombo.Selected);
                    obj.Apo_Numero_Documento	=	ficha.FichaConfVos.Apo_Numero_Documento	;
                    obj.Apo_Calle	=	ficha.FichaConfVos.Apo_Calle;
                    obj.Apo_Numero	=	ficha.FichaConfVos.Apo_Numero;
                    obj.Apo_Piso	=	ficha.FichaConfVos.Apo_Piso;
                    obj.Apo_Dpto	=	ficha.FichaConfVos.Apo_Dpto;
                    obj.Apo_Monoblock	=	ficha.FichaConfVos.Apo_Monoblock;
                    obj.Apo_Parcela	=	ficha.FichaConfVos.Apo_Parcela;
                    obj.Apo_Manzana	=	ficha.FichaConfVos.Apo_Manzana;
                    obj.Apo_Barrio	=	ficha.FichaConfVos.Apo_Barrio;
                    obj.Apo_Codigo_Postal	=	ficha.FichaConfVos.Apo_Codigo_Postal;
                    obj.Apo_Id_Localidad = Convert.ToInt32(ficha.LocalidadesApo.Selected);
                    obj.Apo_Telefono	=	ficha.FichaConfVos.Apo_Telefono	;
                    obj.Apo_Tiene_Hijos	=	ficha.FichaConfVos.Apo_Tiene_Hijos	;
                    obj.Apo_Cantidad_Hijos	=	ficha.FichaConfVos.Apo_Cantidad_Hijos	;
                    obj.Apo_Sexo = ficha.SexoApo.Selected == ((int)Enums.Sexo.Mujer).ToString() ? "MUJER" : "VARON";
                    obj.Apo_Estado_Civil = Convert.ToInt32(ficha.EstadoCivilApo.Selected);

                    

                    if (ficha.Escuelas.Selected == "0")
                    {
                        obj.IdEscuelaConfVos = null;
                    }
                    else
                    {
                        obj.IdEscuelaConfVos = Convert.ToInt32(ficha.Escuelas.Selected);
                    }


                         obj.ID_CURSO = ficha.FichaConfVos.ID_CURSO;
                         //obj.FichaConfVos.ID_ABANDONO_CUR = ficha.AbandonoCursado.Selected == "0" ? null : Convert.ToInt32(ficha.AbandonoCursado.Selected);
                         if (ficha.AbandonoCursado.Selected == "0")
                         {
                             obj.ID_ABANDONO_CUR = null;
                         }
                         else
                         {
                             obj.ID_ABANDONO_CUR = Convert.ToInt32(ficha.AbandonoCursado.Selected);
                         }
                         obj.TRABAJO_CONDICION = ficha.FichaConfVos.TRABAJO_CONDICION;
                         obj.SENAF = ficha.SENAF;//ficha.FichaConfVos.SENAF;
                         obj.PERTENECE_ONG = ficha.FichaConfVos.PERTENECE_ONG;
                         obj.ID_ONG = ficha.FichaConfVos.ID_ONG == 0 ? null : ficha.FichaConfVos.ID_ONG;
                         obj.APO_CELULAR = ficha.FichaConfVos.APO_CELULAR;
                         obj.APO_MAIL = ficha.FichaConfVos.APO_MAIL;
                         obj.ID_FACILITADOR = ficha.FichaConfVos.ID_FACILITADOR == 0 ? null : ficha.FichaConfVos.ID_FACILITADOR;
                         obj.ID_GESTOR = ficha.FichaConfVos.ID_GESTOR == 0 ? null : ficha.FichaConfVos.ID_GESTOR;
                    //Agrego Horario---------------------------------------------
                    AddHorarios(ficha);
                    //---------------------------------------------------

                    break;
                default:
                    break;
            }

            if (ficha.Accion.Equals("Modificar"))
            {
                _fichaRepositorio.UpdateFicha(obj);

                //if (ficha.FichaReconversion.Proyecto.NombreProyecto == null &&
                //    Equals(ficha.TipoFicha, (int)Enums.TipoFicha.ReconversionProductiva))
                //{
                //    _proyectoRepositorio.DeleteProyecto(ficha.FichaReconversion.Proyecto);
                //}

                if (obj.TipoFicha == (int)Enums.TipoFicha.PppProf || obj.TipoFicha == (int)Enums.TipoFicha.Vat || obj.TipoFicha == (int)Enums.TipoFicha.Ppp || obj.TipoFicha == (int)Enums.TipoFicha.ReconversionProductiva)
                {
                    if (ficha.IdEmpresaInicial != ficha.IdEmpresa)
                    {
                        _beneficiarioRepositorio.ClearFechaNotificacion(ficha.IdFicha);
                    }
                }

            }
            else
            {
                _fichaRepositorio.UpdateEstadoFicha(ficha.IdFicha, int.Parse(ficha.Estado.Selected));

                DeleteFichaRechazo(ficha);
            }

            if (ficha.Estado.Selected.Equals(((int)Enums.EstadoFicha.RechazoFormal).ToString()))
            {
                AddFichaRechazo(ficha);
            }


            int idbeneficiario = 0;
            if (ficha.Estado.Selected.Equals(((int)Enums.EstadoFicha.Beneficiario).ToString()) && ficha.Accion.Equals("CambiarEstado"))
            {
                // 05/05/2014 - VERIFICAR SI LA FICHA ESTA ASOCIADA A UN BENEFICIARIO
                idbeneficiario = _beneficiarioRepositorio.GetBeneficiarioFichaAll(ficha.IdFicha).Select(x => x.IdBeneficiario).SingleOrDefault();

                string tieneApoderado = EsMenorDeEdad(ficha.FechaNacimineto);

                if (idbeneficiario == 0)
                {
                    IBeneficiario objbeneficiario = new Beneficiario
                                                        {
                                                            IdFicha = ficha.IdFicha,
                                                            IdPrograma = Convert.ToInt16(ficha.TipoFicha),
                                                            IdEstado = (int)Enums.EstadoBeneficiario.Activo, //Estado 2: ACTIVO- Validar si primero es postulante o Activo
                                                            Residente = "S",
                                                            Email = ficha.Mail.Replace('*', '@'),
                                                            TipoPersona = "F",
                                                            CodigoActivacionBancaria = 2,
                                                            CondicionIva = Constantes.CondicionIvaBanco.ToString(),
                                                            TieneApoderado = tieneApoderado,
                                                            FechaInicioBeneficio = comun.FechaSistema,
                                                        };
                    int idBeneficiario = _beneficiarioRepositorio.AddBeneficiario(objbeneficiario);

                    string nroDoc = ficha.NumeroDocumento.Replace('*', ' ');
                    if (tieneApoderado == "N")
                    {
                        //Grabar en T_CUENTAS_BANCO
                        ICuentaBanco cuentaBanco = new CuentaBanco
                                                       {
                                                           IdSucursal = ficha.IdLocalidad == 25 ? (short)143 : _sucursalrepositorio.GestSucursalLocalidad(ficha.IdFicha),//_sucursalrepositorio.GestSucursalLocalidad(ficha.IdFicha),
                                                           IdCuentaBanco = SecuenciaRepositorio.GetId(),
                                                           IdBeneficiario = idBeneficiario,
                                                           IdSistema = Constantes.IdSistemaBanco,
                                                           UsuarioBanco = nroDoc,
                                                           IdMoneda = Constantes.MonedaBanco,
                                                       };

                        _cuentaBancoRepositorio.AddCuentaBanco(cuentaBanco);
                    }

                    // 17/05/2014 - Se añade la carga de apoderado para confiamos (los datos existen en la ficha)
                    if (tieneApoderado == "S")
                    {

                        if (ficha.TipoFicha == (int)Enums.TipoFicha.ConfiamosEnVos)
                        {
                            apoderado.IdBeneficiario = idBeneficiario;
                            apoderado.Apellido = (obj.Apo_Apellido ?? "").ToUpper();
                            apoderado.Nombre = (obj.Apo_Nombre ?? "").ToUpper();
                            apoderado.TipoDocumento = obj.Apo_Tipo_Documento;
                            apoderado.NumeroDocumento = obj.Apo_Numero_Documento;
                            apoderado.Cuil = obj.Apo_Cuil;
                            apoderado.Sexo = obj.Apo_Sexo;
                            //apoderado.IdSistema = obj.IdSistema;
                            //apoderado.Cbu = obj.Cbu;
                            //apoderado.UsuarioBanco = obj.UsuarioBanco;
                            //apoderado.IdSucursal = obj.IdSucursal == 0 ? null : obj.IdSucursal;
                            apoderado.IdMoneda = 144;
                            //apoderado.ApoderadoDesde = apoderado.ApoderadoDesde;
                            //apoderado.ApoderadoHasta = apoderado.ApoderadoHasta;
                            apoderado.IdEstadoApoderado = 1;//apoderado.IdEstadoApoderado;
                            apoderado.FechaNacimiento = obj.Apo_Fer_Nac;
                            apoderado.Calle = (obj.Apo_Calle ?? "").ToUpper();
                            apoderado.Numero = obj.Apo_Numero;
                            apoderado.Piso = obj.Apo_Piso;
                            apoderado.Dpto = obj.Apo_Dpto;
                            apoderado.Monoblock = obj.Apo_Monoblock;
                            apoderado.Parcela = obj.Apo_Parcela;
                            apoderado.Manzana = obj.Apo_Manzana;
                            //apoderado.EntreCalles = (obj.APO_Entrecalles ?? "").ToUpper();
                            apoderado.Barrio = obj.Apo_Barrio;
                            apoderado.CodigoPostal = obj.Apo_Codigo_Postal;
                            apoderado.IdLocalidad = obj.Apo_Id_Localidad;
                            apoderado.TelefonoFijo = obj.Apo_Telefono;
                            //apoderado.TelefonoCelular = obj.APO_TelefonoCelular;
                            //apoderado.Mail = obj.APO_Mail;//,
                            //apoderado.ID_USR_SIST = apoderado.IdUsuarioSistema,
                            //apoderado.FEC_SIST = apoderado.FechaSistema
                            //obj = _;
                            _FichaRepo.AddApoderado(apoderado);
                        }

                    }


                }
            }
            if (idbeneficiario == 0)
            {
                ficha.strMessageUpdate = "El cambio se realizó correctamente.";
            }
            else
            {
                ficha.strMessageUpdate = "El cambio se realizó correctamente. El beneficiario ya existe, verificar el estado del mismo.";            
            }
            return true;
        }

        private bool AddHorarios(IFichaVista ficha)
        {
            try
            {
                IList<IHorarioFicha> listaHFadd = new List<IHorarioFicha>();

                //Lunes---------------------------------------------
                int poslunes = 0;
                short cortelunes = 0;
                foreach (var horarioFicha in ficha.HorarioLunes)
                {
                    if (horarioFicha.Selected != null && horarioFicha.Selected != "0")
                    {
                        IHorarioFicha hf = new HorarioFicha
                                               {
                                                   IdEmpresaHorario = Convert.ToInt32(horarioFicha.Selected),
                                                   IdFicha = ficha.IdFicha,
                                                   Corte = cortelunes,
                                                   Observacion = ficha.ObsLunes[poslunes]
                                               };
                        listaHFadd.Add(hf);
                        cortelunes++;
                    }

                    poslunes++;
                }
                //---------------------------------------------------

                //Martes---------------------------------------------
                int posmartes = 0;
                short cortemartes = 0;
                foreach (var horarioFicha in ficha.HorarioMartes)
                {
                    if (horarioFicha.Selected != null && horarioFicha.Selected != "0")
                    {
                        IHorarioFicha hf = new HorarioFicha
                                               {
                                                   IdEmpresaHorario = Convert.ToInt32(horarioFicha.Selected),
                                                   IdFicha = ficha.IdFicha,
                                                   Corte = cortemartes,
                                                   Observacion = ficha.ObsMartes[posmartes]
                                               };
                        listaHFadd.Add(hf);
                        cortemartes++;
                    }

                    posmartes++;
                }
                //---------------------------------------------------

                //Miercoles---------------------------------------------
                int posmiercoles = 0;
                short cortemiercoles = 0;
                foreach (var horarioFicha in ficha.HorarioMiercoles)
                {
                    if (horarioFicha.Selected != null && horarioFicha.Selected != "0")
                    {
                        IHorarioFicha hf = new HorarioFicha
                                               {
                                                   IdEmpresaHorario = Convert.ToInt32(horarioFicha.Selected),
                                                   IdFicha = ficha.IdFicha,
                                                   Corte = cortemiercoles,
                                                   Observacion = ficha.ObsMiercoles[posmiercoles]
                                               };
                        listaHFadd.Add(hf);
                        cortemiercoles++;
                    }

                    posmiercoles++;
                }
                //---------------------------------------------------



                //Jueves---------------------------------------------
                int posjueves = 0;
                short cortejueves = 0;
                foreach (var horarioFicha in ficha.HorarioJueves)
                {
                    if (horarioFicha.Selected != null && horarioFicha.Selected != "0")
                    {
                        IHorarioFicha hf = new HorarioFicha
                                               {
                                                   IdEmpresaHorario = Convert.ToInt32(horarioFicha.Selected),
                                                   IdFicha = ficha.IdFicha,
                                                   Corte = cortejueves,
                                                   Observacion = ficha.ObsJueves[posjueves]
                                               };
                        listaHFadd.Add(hf);
                        cortejueves++;
                    }

                    posjueves++;
                }
                //---------------------------------------------------

                //Viernes---------------------------------------------
                int posviernes = 0;
                short cortesvienes = 0;
                foreach (var horarioFicha in ficha.HorarioViernes)
                {
                    if (horarioFicha.Selected != null && horarioFicha.Selected != "0")
                    {
                        IHorarioFicha hf = new HorarioFicha
                                               {
                                                   IdEmpresaHorario = Convert.ToInt32(horarioFicha.Selected),
                                                   IdFicha = ficha.IdFicha,
                                                   Corte = cortesvienes,
                                                   Observacion = ficha.ObsViernes[posviernes]
                                               };
                        listaHFadd.Add(hf);
                        cortesvienes++;
                    }

                    posviernes++;
                }
                //---------------------------------------------------

                //Sábado---------------------------------------------
                int possabado = 0;
                short cortesabado = 0;
                foreach (var horarioFicha in ficha.HorarioSabado)
                {
                    if (horarioFicha.Selected != null && horarioFicha.Selected != "0")
                    {
                        IHorarioFicha hf = new HorarioFicha
                                               {
                                                   IdEmpresaHorario = Convert.ToInt32(horarioFicha.Selected),
                                                   IdFicha = ficha.IdFicha,
                                                   Observacion = ficha.ObsSabado[possabado],
                                                   Corte = cortesabado
                                               };
                        listaHFadd.Add(hf);
                        cortesabado++;
                    }

                    possabado++;
                }
                //---------------------------------------------------

                //Domingo---------------------------------------------
                int posdomingo = 0;
                short cortedomingo = 0;
                foreach (var horarioFicha in ficha.HorarioDomingo)
                {
                    if (horarioFicha.Selected != null && horarioFicha.Selected != "0")
                    {
                        IHorarioFicha hf = new HorarioFicha
                                               {
                                                   IdEmpresaHorario = Convert.ToInt32(horarioFicha.Selected),
                                                   IdFicha = ficha.IdFicha,
                                                   Observacion = ficha.ObsDomingo[posdomingo],
                                                   Corte = cortedomingo
                                               };
                        listaHFadd.Add(hf);
                        cortedomingo++;
                    }

                    posdomingo++;
                }


                //---------------------------------------------------

                _horarioficharepositorio.AddHorarioFicha(listaHFadd, ficha.IdFicha);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        private static string EsMenorDeEdad(DateTime fechaNacimiento)
        {
            string esMenor = "N";
            int edad = DateTime.Now.Year - fechaNacimiento.Year;

            //Obtengo la fecha de cumpleaños de este año.
            DateTime nacimientoAhora = fechaNacimiento.AddYears(edad);

            //Le resto un año si la fecha actual es anterior al día de nacimiento.
            if (nacimientoAhora > DateTime.Now.Date)
            {
                edad--;
            }

            if (edad < 18)
            {
                esMenor = "S";
            }

            return esMenor;
        }

        public bool AddFicha(IFichaVista ficha)
        {
            var codSeg = NumberGenerator(Convert.ToInt32(ficha.ComboTipoFicha.Selected),
                _fichaRepositorio.GetFichas(String.Empty, String.Empty, String.Empty, String.Empty,
                Convert.ToInt32(ficha.ComboTipoFicha.Selected), -1,0,"").Count);

            int? idProyecto = null;

            using (var transaction = new TransactionScope())
            {
                IFicha objficha = new Ficha
                                      {
                                          IdFicha = ficha.IdFicha,
                                          Apellido = ficha.Apellido ?? "*",
                                          Barrio = ficha.Barrio ?? "CENTRO",//"*",
                                          CodigoSeguridad = codSeg,
                                          Calle = ficha.Calle,
                                          CantidadHijos = ficha.CantidadHijos,
                                          CertificadoDiscapacidad = ficha.CertificadoDiscapacidad,
                                          CodigoPostal = ficha.CodigoPostal ?? "*",
                                          Contacto = ficha.Contacto,
                                          Cuil = ficha.Cuil ?? "_-________-_",
                                          DeficienciaOtra = ficha.DeficienciaOtra,
                                          Dpto = ficha.Dpto,
                                          EntreCalles = ficha.EntreCalles,
                                          IdLocalidad = Convert.ToInt32(ficha.Localidades.Selected),
                                          Mail = ficha.Mail ?? "*",
                                          EsDiscapacitado = ficha.EsDiscapacitado,
                                          EstadoCivil = Convert.ToInt32(ficha.EstadoCivil.Selected),
                                          FechaNacimiento = ficha.FechaNacimineto,
                                          Manzana = ficha.Manzana,
                                          Monoblock = ficha.Monoblock,
                                          Nombre = ficha.Nombre ?? "*",
                                          Numero = ficha.Numero,
                                          NumeroDocumento = ficha.NumeroDocumento ?? " ",
                                          Parcela = ficha.Parcela,
                                          Piso = ficha.Piso,
                                          Sexo = ficha.Sexo.Selected == ((int)Enums.Sexo.Mujer).ToString() ? "MUJER" : "VARON",
                                          TelefonoCelular = ficha.TelefonoCelular,
                                          TelefonoFijo = ficha.TelefonoFijo,
                                          TieneDeficienciaMental = ficha.TieneDeficienciaMental,
                                          TieneDeficienciaMotora = ficha.TieneDeficienciaMotora,
                                          TieneDeficienciaPsicologia = ficha.TieneDeficienciaPsicologia,
                                          TieneDeficienciaSensorial = ficha.TieneDeficienciaSensorial,
                                          TieneHijos = ficha.TieneHijos,
                                          TipoDocumento = Convert.ToInt32(ficha.TipoDocumentoCombo.Selected),
                                          TipoFicha = Convert.ToInt32(ficha.ComboTipoFicha.Selected),
                                          IdEstadoFicha = Convert.ToInt32(ficha.ComboEstadoFicha.Selected),
                                          idBarrio = ficha.idBarrio == 0 ? null : ficha.idBarrio,
                                          NroTramite = ficha.NroTramite,
                                          
                                      };

                switch (Convert.ToInt32(ficha.ComboTipoFicha.Selected))
                {
                    case (int)Enums.TipoFicha.Terciaria:
                        if (ficha.Carrera.Selected == "0")
                        {
                            objficha.IdCarreraTerciaria = null;
                        }
                        else
                        {
                            objficha.IdCarreraTerciaria = Convert.ToInt32(ficha.Carrera.Selected);
                        }

                        if (ficha.Escuelas.Selected == "0")
                        {
                            objficha.IdEscuelaTerciaria = null;
                        }
                        else
                        {
                            objficha.IdEscuelaTerciaria = Convert.ToInt32(ficha.Escuelas.Selected);
                        }

                        objficha.PromedioTerciaria = ficha.Promedio;
                        objficha.OtraCarreraTerciaria = ficha.OtraCarrera;
                        objficha.OtraInstitucionTerciaria = ficha.OtraInstitucion;
                        objficha.OtroSectorTerciaria = ficha.OtroSector;
                        objficha.IdSubprograma = ficha.IdSubprograma == 0 ? null : ficha.IdSubprograma;
                        break;
                    case (int)Enums.TipoFicha.Universitaria:
                        if (ficha.Carrera.Selected == "0")
                        {
                            objficha.IdCarreraUniversitaria = null;
                        }
                        else
                        {
                            objficha.IdCarreraUniversitaria = Convert.ToInt32(ficha.Carrera.Selected);
                        }

                        if (ficha.Escuelas.Selected == "0")
                        {
                            objficha.IdEscuelaUniversitaria = null;
                        }
                        else
                        {
                            objficha.IdEscuelaUniversitaria = Convert.ToInt32(ficha.Escuelas.Selected);
                        }

                        objficha.PromedioUniversitaria = ficha.Promedio;
                        objficha.OtraCarreraUniversitaria = ficha.OtraCarrera;
                        objficha.OtraInstitucionUniversitaria = ficha.OtraInstitucion;
                        objficha.OtroSectorUniversitaria = ficha.OtroSector;
                        objficha.IdSubprograma = ficha.IdSubprograma == 0 ? null : ficha.IdSubprograma;
                        break;
                    case (int)Enums.TipoFicha.Ppp:
                        objficha.IdNivelEscolaridad = ficha.FichaPpp.IdNivelEscolaridad;
                        objficha.DeseaTermNivel = ficha.FichaPpp.DeseaTermNivel;
                        objficha.Cursando = ficha.FichaPpp.Cursando;
                        objficha.Finalizado = ficha.FichaPpp.Finalizado;
                        objficha.Modalidad = ficha.FichaPpp.Modalidad;
                        objficha.IdEmpresa = ficha.IdEmpresa;
                        objficha.IdSede = ficha.IdSede;
                        objficha.Tareas = ficha.FichaPpp.Tareas;
                        objficha.FechaInicioActividad = ficha.FichaPpp.FechaInicioActividad;
                        objficha.FechaFinActividad = ficha.FichaPpp.FechaFinActividad;
                        objficha.IdModalidadAfip = Convert.ToInt32(ficha.ComboModalidadAfip.Selected);
                        objficha.AltaTemprana = ficha.FichaPpp.Modalidad == (int)Enums.Modalidad.Entrenamiento ? "N" :
                            ficha.FichaPpp.AltaTemprana;
                        objficha.IdSubprograma = ficha.IdSubprograma == 0 ? null : ficha.IdSubprograma;
                        objficha.TipoPrograma = ficha.tipoPrograma;
                        if (objficha.TipoPrograma == 2)
                        {
                            objficha.ppp_aprendiz = "S";
                        }
                        else
                        {
                            objficha.ppp_aprendiz = "N";
                        }

                        // 24/02/2020
                        if (ficha.tipoPrograma == 6)
                        {
                            if (ficha.FichaPpp.IdNivelEscolaridad == 4)
                            {
                                objficha.IdEscuelaPpp = ficha.FichaPpp.Id_Escuela == 0 ? null : ficha.FichaPpp.Id_Escuela;
                            }
                            else
                            {
                                objficha.IdEscuelaPpp = null;
                            }
                        }
                        else
                        {
                            objficha.IdEscuelaPpp = ficha.FichaPpp.Id_Escuela == 0 ? null : ficha.FichaPpp.Id_Escuela;
                        }
                        objficha.ID_CURSO = ficha.FichaPpp.ID_CURSO == 0 ? null : ficha.FichaPpp.ID_CURSO;
                        objficha.ID_ONG = ficha.FichaPpp.ID_ONG == 0 ? null : ficha.FichaPpp.ID_ONG;

                        objficha.horario_rotativo = ficha.FichaPpp.horario_rotativo;
                        objficha.sede_rotativa = ficha.FichaPpp.sede_rotativa;
                        // 05/08/2018

                        objficha.egreso = ficha.FichaPpp.egreso;
                        objficha.cod_uso_interno = ficha.FichaPpp.cod_uso_interno;
                        objficha.carga_horaria = ficha.FichaPpp.carga_horaria;
                        objficha.ch_cual = ficha.FichaPpp.ch_cual;
                        // 24/02/2020
                        if (ficha.tipoPrograma == 6)
                        {
                            if (ficha.FichaPpp.IdNivelEscolaridad == 4)
                            {
                                objficha.id_carrera = null;
                                objficha.n_descripcion_t = "";
                                objficha.n_cursado_ins = "";
                            }
                            else
                            {
                                objficha.id_carrera = ficha.FichaPpp.id_carrera == 0 ? null : ficha.FichaPpp.id_carrera;
                                objficha.n_descripcion_t = ficha.FichaPpp.n_descripcion_t;
                                objficha.n_cursado_ins = ficha.FichaPpp.n_cursado_ins;
                            }
                        }
                        else
                        {
                            objficha.id_carrera = ficha.FichaPpp.id_carrera == 0 ? null : ficha.FichaPpp.id_carrera;
                            objficha.n_descripcion_t = ficha.FichaPpp.n_descripcion_t;
                            objficha.n_cursado_ins = ficha.FichaPpp.n_cursado_ins;
                        }
                        
                        objficha.CONSTANCIA_EGRESO = ficha.FichaPpp.CONSTANCIA_EGRESO;
                        objficha.MATRICULA = ficha.FichaPpp.MATRICULA;
                        objficha.CONSTANCIA_CURSO = ficha.FichaPpp.CONSTANCIA_CURSO;
                        objficha.co_financiamiento = ficha.FichaPpp.co_financiamiento;
                        // 05/08/2018
                        objficha.SENAF = ficha.SENAF;

                        break;
                    case (int)Enums.TipoFicha.Vat:
                        objficha.IdNivelEscolaridad = ficha.FichaVat.IdNivelEscolaridad;
                        objficha.Cursando = ficha.FichaVat.Cursando;
                        objficha.Finalizado = ficha.FichaVat.Finalizado;
                        objficha.Modalidad = ficha.FichaVat.Modalidad;
                        objficha.IdEmpresa = ficha.IdEmpresa;
                        objficha.IdSede = ficha.IdSede;
                        objficha.Tareas = ficha.FichaVat.Tareas;
                        objficha.FechaInicioActividad = ficha.FichaVat.Modalidad == (int)Enums.Modalidad.Entrenamiento
                                                            ? null
                                                            : ficha.FichaVat.FechaInicioActividad;
                        objficha.FechaFinActividad = ficha.FichaVat.Modalidad == (int)Enums.Modalidad.Entrenamiento
                                                         ? null
                                                         : ficha.FichaVat.FechaFinActividad;
                        objficha.IdModalidadAfip = Convert.ToInt32(ficha.ComboModalidadAfip.Selected);
                        objficha.AltaTemprana = ficha.FichaVat.Modalidad == (int)Enums.Modalidad.Entrenamiento ? "N" :
                            ficha.FichaVat.AltaTemprana;
                        objficha.AniosAportes = ficha.FichaVat.AniosAportes;
                        break;
                    case (int)Enums.TipoFicha.PppProf:
                        objficha.Promedio = ficha.FichaPppp.Promedio;
                        objficha.IdTitulo = ficha.FichaPppp.IdTitulo;
                        objficha.FechaEgreso = ficha.FichaPppp.FechaEgreso;
                        objficha.Modalidad = ficha.FichaPppp.Modalidad;
                        objficha.IdEmpresa = ficha.IdEmpresa;
                        objficha.IdSede = ficha.IdSede;
                        objficha.Tareas = ficha.FichaPppp.Tareas;
                        objficha.FechaInicioActividad = ficha.FichaPppp.Modalidad == (int)Enums.Modalidad.Entrenamiento ? null : ficha.FichaPppp.FechaInicioActividad;
                        objficha.FechaFinActividad = ficha.FichaPppp.Modalidad == (int)Enums.Modalidad.Entrenamiento ? null : ficha.FichaPppp.FechaFinActividad;
                        objficha.IdModalidadAfip = Convert.ToInt32(ficha.ComboModalidadAfip.Selected);
                        objficha.AltaTemprana = ficha.FichaPppp.Modalidad == (int)Enums.Modalidad.Entrenamiento ? "N" :
                            ficha.FichaPppp.AltaTemprana;
                        break;
                    case (int)Enums.TipoFicha.ReconversionProductiva:
                        objficha.IdNivelEscolaridad = ficha.FichaReconversion.IdNivelEscolaridad;
                        objficha.DeseaTermNivel = ficha.FichaReconversion.DeseaTermNivel;
                        objficha.Cursando = ficha.FichaReconversion.Cursando;
                        objficha.Finalizado = ficha.FichaReconversion.Finalizado;
                        objficha.IdEmpresa = ficha.IdEmpresa;
                        objficha.IdSede = ficha.IdSede;
                        objficha.Modalidad = ficha.FichaReconversion.Modalidad;
                        objficha.FechaInicioActividad = ficha.FichaPpp.FechaInicioActividad;
                        objficha.FechaFinActividad = ficha.FichaPpp.FechaFinActividad;
                        objficha.IdModalidadAfip = Convert.ToInt32(ficha.ComboModalidadAfip.Selected);
                        objficha.AltaTemprana = ficha.FichaReconversion.Modalidad == (int)Enums.Modalidad.Entrenamiento ? "N" : ficha.FichaReconversion.AltaTemprana;
                        break;
                    case (int)Enums.TipoFicha.EfectoresSociales:
                        objficha.IdNivelEscolaridad = ficha.FichaEfectores.IdNivelEscolaridad;
                        objficha.DeseaTermNivel = ficha.FichaEfectores.DeseaTermNivel;
                        objficha.Cursando = ficha.FichaEfectores.Cursando;
                        objficha.Finalizado = ficha.FichaEfectores.Finalizado;
                        objficha.IdEmpresa = ficha.IdEmpresa;
                        objficha.IdSede = ficha.IdSede;
                        objficha.Modalidad = ficha.FichaEfectores.Modalidad;
                        objficha.FechaInicioActividad = ficha.FichaPpp.FechaInicioActividad;
                        objficha.FechaFinActividad = ficha.FichaPpp.FechaFinActividad;
                        objficha.IdModalidadAfip = Convert.ToInt32(ficha.ComboModalidadAfip.Selected);
                        objficha.AltaTemprana = ficha.FichaEfectores.Modalidad == (int)Enums.Modalidad.Entrenamiento ? "N" : ficha.FichaReconversion.AltaTemprana;
                        objficha.monto_efe = ficha.monto==null ? 0 : ficha.monto;
                        objficha.IdSubprograma = Convert.ToInt32(ficha.Subprogramas.Selected);
                        break;
                    case (int)Enums.TipoFicha.ConfiamosEnVos:
                        objficha.IdNivelEscolaridad = ficha.FichaConfVos.IdNivelEscolaridad;
                        objficha.DeseaTermNivel = ficha.FichaConfVos.DeseaTermNivel;
                        objficha.Cursando = ficha.FichaConfVos.Cursando;
                        objficha.Finalizado = ficha.FichaConfVos.Finalizado;
                        objficha.IdEmpresa = ficha.IdEmpresa;
                        objficha.IdSede = ficha.IdSede;
                        objficha.Modalidad = ficha.FichaConfVos.Modalidad;
                        objficha.FechaInicioActividad = ficha.FichaConfVos.FechaInicioActividad;
                        objficha.FechaFinActividad = ficha.FichaConfVos.FechaFinActividad;
                        objficha.IdModalidadAfip = Convert.ToInt32(ficha.ComboModalidadAfip.Selected);
                        objficha.AltaTemprana = ficha.FichaConfVos.Modalidad == (int)Enums.Modalidad.Entrenamiento ? "N" : ficha.FichaConfVos.AltaTemprana;

                        objficha.IdSubprograma = Convert.ToInt32(ficha.Subprogramas.Selected);
                        objficha.AportesDeLaEmpresa = ficha.FichaConfVos.AportesDeLaEmpresa;
                        objficha.ApellidoTutor = ficha.FichaConfVos.ApellidoTutor;
                        objficha.NombreTutor = ficha.FichaConfVos.NombreTutor;
                        objficha.NroDocumentoTutor = ficha.FichaConfVos.NroDocumentoTutor;
                        objficha.TelefonoTutor = ficha.FichaConfVos.TelefonoTutor;
                        objficha.EmailTutor = ficha.FichaConfVos.EmailTutor;
                        objficha.PuestoTutor = ficha.FichaConfVos.PuestoTutor;
                        objficha.IdNivelEscolaridad = ficha.FichaConfVos.IdNivelEscolaridad;
                        objficha.DeseaTermNivel = ficha.FichaConfVos.DeseaTermNivel;
                        objficha.Cursando = ficha.FichaConfVos.Cursando;
                        objficha.Finalizado = ficha.FichaConfVos.Finalizado;

                    
                        objficha.Abandono_Escuela	=	ficha.FichaConfVos.Abandono_Escuela;
                        objficha.Trabaja_Trabajo	=	ficha.FichaConfVos.Trabaja_Trabajo	;
                        objficha.cursa	=	ficha.FichaConfVos.cursa;
                        objficha.Ultimo_Cursado	=	ficha.FichaConfVos.Ultimo_Cursado	;
                        objficha.pit	=	ficha.FichaConfVos.pit	;
                        objficha.Centro_Adultos	=	ficha.FichaConfVos.Centro_Adultos	;
                        objficha.Progresar	=	ficha.FichaConfVos.Progresar	;
                        objficha.Actividades	=	ficha.FichaConfVos.Actividades	;
                        objficha.Benef_progre	=	ficha.FichaConfVos.Benef_progre	;
                        objficha.Autoriza_Tutor	=	ficha.FichaConfVos.Autoriza_Tutor	;
                        objficha.Apoderado	=	ficha.FichaConfVos.Apoderado	;
					
                        objficha.Apo_Cuil	=	ficha.FichaConfVos.Apo_Cuil	;
                        objficha.Apo_Apellido	=	ficha.FichaConfVos.Apo_Apellido	;
                        objficha.Apo_Nombre	=	ficha.FichaConfVos.Apo_Nombre	;
                        objficha.Apo_Fer_Nac	=	ficha.FichaConfVos.Apo_Fer_Nac	;
                        objficha.Apo_Tipo_Documento = Convert.ToInt32(ficha.TipoDocumentoApoCombo.Selected);
                        objficha.Apo_Numero_Documento	=	ficha.FichaConfVos.Apo_Numero_Documento	;
                        objficha.Apo_Calle	=	ficha.FichaConfVos.Apo_Calle;
                        objficha.Apo_Numero	=	ficha.FichaConfVos.Apo_Numero;
                        objficha.Apo_Piso	=	ficha.FichaConfVos.Apo_Piso;
                        objficha.Apo_Dpto	=	ficha.FichaConfVos.Apo_Dpto;
                        objficha.Apo_Monoblock	=	ficha.FichaConfVos.Apo_Monoblock;
                        objficha.Apo_Parcela	=	ficha.FichaConfVos.Apo_Parcela;
                        objficha.Apo_Manzana	=	ficha.FichaConfVos.Apo_Manzana;
                        objficha.Apo_Barrio	=	ficha.FichaConfVos.Apo_Barrio;
                        objficha.Apo_Codigo_Postal	=	ficha.FichaConfVos.Apo_Codigo_Postal;
                        objficha.Apo_Id_Localidad = Convert.ToInt32(ficha.LocalidadesApo.Selected);
                        objficha.Apo_Telefono	=	ficha.FichaConfVos.Apo_Telefono	;
                        objficha.Apo_Tiene_Hijos	=	ficha.FichaConfVos.Apo_Tiene_Hijos	;
                        objficha.Apo_Cantidad_Hijos	=	ficha.FichaConfVos.Apo_Cantidad_Hijos	;
                        objficha.Apo_Sexo = ficha.SexoApo.Selected == ((int)Enums.Sexo.Mujer).ToString() ? "MUJER" : "VARON";
                        objficha.Apo_Estado_Civil = Convert.ToInt32(ficha.EstadoCivilApo.Selected);

                    

                        if (ficha.Escuelas.Selected == "0")
                        {
                            objficha.IdEscuelaConfVos = null;
                        }
                        else
                        {
                            objficha.IdEscuelaConfVos = Convert.ToInt32(ficha.Escuelas.Selected);
                        }

                        objficha.ID_CURSO = ficha.FichaConfVos.ID_CURSO;
                         if (ficha.AbandonoCursado.Selected == "0")
                         {
                             objficha.ID_ABANDONO_CUR = null;
                         }
                         else
                         {
                             objficha.ID_ABANDONO_CUR = Convert.ToInt32(ficha.AbandonoCursado.Selected);
                         }
                         objficha.TRABAJO_CONDICION = ficha.FichaConfVos.TRABAJO_CONDICION;
                         objficha.SENAF = ficha.SENAF;//ficha.FichaConfVos.SENAF;
                         objficha.PERTENECE_ONG = ficha.FichaConfVos.PERTENECE_ONG;
                         objficha.ID_ONG = ficha.FichaConfVos.ID_ONG == 0 ? null : ficha.FichaConfVos.ID_ONG;
                         objficha.APO_CELULAR = ficha.FichaConfVos.APO_CELULAR;
                         objficha.APO_MAIL = ficha.FichaConfVos.APO_MAIL;
                         objficha.ID_FACILITADOR = ficha.FichaConfVos.ID_FACILITADOR == 0 ? null : ficha.FichaConfVos.ID_FACILITADOR;
                         objficha.ID_GESTOR = ficha.FichaConfVos.ID_GESTOR == 0 ? null : ficha.FichaConfVos.ID_GESTOR;

                        break;
                    default:
                        break;
                }

                _fichaRepositorio.AddFicha(objficha);

                if (ficha.Accion == "Agregar")
                {
                    if (ficha.ComboEstadoFicha.Selected.Equals(((int)Enums.EstadoFicha.RechazoFormal).ToString()))
                    {
                        AddFichaRechazo(ficha);
                    }
                }

                transaction.Complete();

                return true;
            }
        }

        private string NumberGenerator(int tipoFichaEnum, int contador)
        {
            // Algoritmo generacion de codigo de seguridad de 20 digitos con el formato AABBBBBBXXXXXXXXXXXX
            // AA = Tipo de ficha (2 digitos)
            // BBBBBB = Numero secuencial para ese tipo de ficha (6 digitos)
            // XXXXXXXXXXXX = Alfanumerico aleatorio (12 digitos)
            var tipoFicha = (tipoFichaEnum).ToString().PadLeft(2, '0');
            var numeroSecuencial = contador.ToString().PadLeft(6, '0');
            var numeroAleatorio = Guid.NewGuid().ToString().Replace("-", string.Empty).Remove(12);

            return tipoFicha + numeroSecuencial + numeroAleatorio.ToUpper();
        }

        public IFichaVista GetEmpresa(IFichaVista vista)
        {
            IEmpresa objEmp = _empresaRepositorio.GetEmpresa(vista.CuitEmpresa);

            vista.IdEmpresa = objEmp.IdEmpresa;
            vista.CuitEmpresa = objEmp.Cuit;
            vista.DescripcionEmpresa = objEmp.NombreEmpresa;
            vista.EmpresaLocalidad = objEmp.NombreLocalidad;
            vista.EmpresaCodigoActividad = objEmp.CodigoActividad;
            vista.EmpresaDomicilioLaboralIdem = objEmp.DomicilioLaboralIdem;
            vista.EmpresaCantidadEmpleados = Convert.ToInt16(objEmp.CantidadEmpleados);
            vista.EmpresaCalle = objEmp.Calle;
            vista.EmpresaNumero = objEmp.Numero;
            vista.EmpresaPiso = objEmp.Piso;
            vista.EmpresaDpto = objEmp.Dpto;
            vista.EmpresaCodigoPostal = objEmp.CodigoPostal;

            vista.Accion = "Agregar";
            vista.TipoFicha = Convert.ToInt32(vista.ComboTipoFicha.Selected);

            CargarSexo(vista);
            CargarTipoDocumento(vista);
            CargarDepartamento(vista, _departamentoRepositorio.GetDepartamentos());
            CargarLocalidad(vista, _localidadRepositorio.GetLocalidades());
            CargarEstadoCivil(vista, _estadoCivilRepositorio.GetEstadosCivil());
            CargarTipoFichaAlta(vista, _tipoFichaRepositorio.GetTiposFicha());
            CargarEstadosFichaAlta(vista);

            vista.Localidades.Selected = vista.Localidades.Selected;
            vista.Sexo.Selected = vista.Sexo.Selected;

            return vista;
        }

        public IFichasVista GetFichasForReportes(string cuil, string dni, string nombre, string apellido, int tipoficha, int estadoficha)
        {
            IFichasVista vista = new FichasVista
                                     {
                                         Fichas =
                                             _fichaRepositorio.GetFichas(cuil, dni, nombre, apellido, tipoficha,
                                                                         estadoficha,0,"")
                                     };

            return vista;
        }

        public byte[] ExportarFichas(string cuil, string dni, string nombre, string apellido, int tipoficha, int estadoficha)
        {
            var path = String.Empty;

            switch (Convert.ToInt32(tipoficha))
            {
                case (int)Enums.TipoFicha.Ppp:
                    path = HttpContext.Current.Server.MapPath(@"\Archivos\Exportados\TemplateBeneficiariosPPP.xls");
                    break;
                case (int)Enums.TipoFicha.Terciaria:
                    path = HttpContext.Current.Server.MapPath(@"\Archivos\Exportados\TemplateBeneficiarioTerciarios.xls");
                    break;
                case (int)Enums.TipoFicha.Universitaria:
                    path = HttpContext.Current.Server.MapPath(@"\Archivos\Exportados\TemplateBeneficiarioUniversitarios.xls");
                    break;
                case (int)Enums.TipoFicha.PppProf:
                    path = HttpContext.Current.Server.MapPath(@"\Archivos\Exportados\TemplateBeneficiariosPPPP.xls");
                    break;
                case (int)Enums.TipoFicha.Vat:
                    path = HttpContext.Current.Server.MapPath(@"\Archivos\Exportados\TemplateBeneficiariosVAT.xls");
                    break;
                case (int)Enums.TipoFicha.ReconversionProductiva:
                    path = HttpContext.Current.Server.MapPath(@"\Archivos\Exportados\TemplateBeneficiariosReconversion.xls");
                    break;
                case (int)Enums.TipoFicha.EfectoresSociales:
                    path = HttpContext.Current.Server.MapPath(@"\Archivos\Exportados\TemplateBeneficiariosEfectoresSociales.xls");
                    break;
            }

            var fs = new FileStream(path, FileMode.Open, FileAccess.Read);

            var templateWorkbook = new HSSFWorkbook(fs, true);

            var sheet = templateWorkbook.GetSheet("Nomina");

            IList<IFicha> listaparaExcel = _fichaRepositorio.GetFichasCompleto(cuil, dni, nombre, apellido, tipoficha, estadoficha);

            var i = 1;

            switch (Convert.ToInt32(tipoficha))
            {
                case (int)Enums.TipoFicha.Ppp:
                    #region case (int)Enums.TipoFicha.Ppp
                    foreach (var ficha in listaparaExcel)
                    {
                        i++;
                        //Ficha
                        var row = sheet.CreateRow(i);
                        var cellidficha = row.CreateCell(0); cellidficha.SetCellValue(ficha.IdFicha);
                        var cellApellido = row.CreateCell(1); cellApellido.SetCellValue(ficha.Apellido);
                        var cellNombre = row.CreateCell(2); cellNombre.SetCellValue(ficha.Nombre);
                        var cellCuil = row.CreateCell(3); cellCuil.SetCellValue(ficha.Cuil);
                        var cellDni = row.CreateCell(4); cellDni.SetCellValue(ficha.NumeroDocumento);
                        var cellFechaNa = row.CreateCell(5); cellFechaNa.SetCellValue(ficha.FechaNacimiento.Value.ToShortDateString());
                        var cellCalle = row.CreateCell(6); cellCalle.SetCellValue(ficha.Calle);
                        var cellNro = row.CreateCell(7); cellNro.SetCellValue(ficha.Numero);
                        var cellPiso = row.CreateCell(8); cellPiso.SetCellValue(ficha.Piso);
                        var cellDpto = row.CreateCell(9); cellDpto.SetCellValue(ficha.Dpto);
                        var cellMono = row.CreateCell(10); cellMono.SetCellValue(ficha.Monoblock);
                        var cellParcela = row.CreateCell(11); cellParcela.SetCellValue(ficha.Parcela);
                        var cellManzana = row.CreateCell(12); cellManzana.SetCellValue(ficha.Manzana);
                        var cellEntreCalle = row.CreateCell(13); cellEntreCalle.SetCellValue(ficha.EntreCalles);
                        var cellBarrio = row.CreateCell(14); cellBarrio.SetCellValue(ficha.Barrio);
                        var cellCp = row.CreateCell(15); cellCp.SetCellValue(ficha.CodigoPostal);
                        var cellidLocalidad = row.CreateCell(16); cellidLocalidad.SetCellValue(ficha.LocalidadFicha.IdLocalidad);
                        var cellnLocalidad = row.CreateCell(17); cellnLocalidad.SetCellValue(ficha.LocalidadFicha.NombreLocalidad);
                        var cellIdDepartamento = row.CreateCell(18); cellIdDepartamento.SetCellValue(ficha.LocalidadFicha.Departamento.IdDepartamento);
                        var cellnDepartamento = row.CreateCell(19); cellnDepartamento.SetCellValue(ficha.LocalidadFicha.Departamento.NombreDepartamento);
                        var cellTelFijo = row.CreateCell(20); cellTelFijo.SetCellValue(ficha.TelefonoFijo);
                        var cellTelCelular = row.CreateCell(21); cellTelCelular.SetCellValue(ficha.TelefonoCelular);
                        var cellContacto = row.CreateCell(22); cellContacto.SetCellValue(ficha.Contacto);
                        var cellEmail = row.CreateCell(23); cellEmail.SetCellValue(ficha.Mail);
                        var cellTieneHijos = row.CreateCell(24); cellTieneHijos.SetCellValue(ficha.TieneHijos);//TIENE_HIJOS	
                        var cellCantidadHijos = row.CreateCell(25); cellCantidadHijos.SetCellValue(ficha.CantidadHijos == null ? "0" : ficha.CantidadHijos.ToString());//CANTIDAD_HIJOS
                        var cellEsDiscapacitado = row.CreateCell(26); cellEsDiscapacitado.SetCellValue(ficha.EsDiscapacitado == true ? "S" : "N");//ES_DIACAPACITADO
                        var cellTieneDeficienciaMotora = row.CreateCell(27); cellTieneDeficienciaMotora.SetCellValue(ficha.TieneDeficienciaMotora == true ? "S" : "N");//TIENE_DEF_MOTORA
                        var cellTieneDeficienciaMental = row.CreateCell(28); cellTieneDeficienciaMental.SetCellValue(ficha.TieneDeficienciaMental == true ? "S" : "N");//TIENE_DEF_MENTAL
                        var cellTieneDeficienciaSensorial = row.CreateCell(29); cellTieneDeficienciaSensorial.SetCellValue(ficha.TieneDeficienciaSensorial == true ? "S" : "N");//TIENE_DEF_SENSORIAL
                        var cellTieneDeficienciaPsicologia = row.CreateCell(30); cellTieneDeficienciaPsicologia.SetCellValue(ficha.TieneDeficienciaPsicologia == true ? "S" : "N");//TIENE_DEF_PSICOLOGICA
                        var cellDeficienciaOtra = row.CreateCell(31); cellDeficienciaOtra.SetCellValue(ficha.DeficienciaOtra);//DEFICIENCIA_OTRA
                        var cellCertificadoDiscapacidad = row.CreateCell(32); cellCertificadoDiscapacidad.SetCellValue(ficha.CertificadoDiscapacidad == true ? "S" : "N");//CERTIF_DISCAP


                        var cellSexo = row.CreateCell(33); cellSexo.SetCellValue(ficha.Sexo);

                        var cellTipoFicha = row.CreateCell(34); cellTipoFicha.SetCellValue(ficha.TipoFicha.ToString() ?? "0");//TIPO_FICHA
                        var cellEstadoCivilDesc = row.CreateCell(35); cellEstadoCivilDesc.SetCellValue(ficha.EstadoCivilDesc);//ESTADO_CIVIL
                        var cellFechaSistema = row.CreateCell(36); cellFechaSistema.SetCellValue(ficha.FechaSistema == null ? String.Empty : ficha.FechaSistema.Value.ToString());//FECH_SIST
                        var cellOrigenCarga = row.CreateCell(37); cellOrigenCarga.SetCellValue(ficha.OrigenCarga == "M" ? "Cargappp" : "Web");//ORIGEN_CARGA
                        var cellNroTramite = row.CreateCell(38); cellNroTramite.SetCellValue(ficha.NroTramite ?? "");//NRO_TRAMITE
                        var cellidCaja = row.CreateCell(39); cellidCaja.SetCellValue(ficha.idCaja == null ? "" : ficha.idCaja.ToString());//ID_CAJA,
                        var cellIdTipoRechazo = row.CreateCell(40); cellIdTipoRechazo.SetCellValue(ficha.IdTipoRechazo == null ? "" : ficha.IdTipoRechazo.ToString());//ID_TIPO_RECHAZO,
                        var cellrechazo = row.CreateCell(41); cellrechazo.SetCellValue(ficha.rechazo ?? "");//.N_TIPO_RECHAZO,

                        var cellidestadoFicha = row.CreateCell(42); cellidestadoFicha.SetCellValue(ficha.IdEstadoFicha == null ? String.Empty
                            : ficha.IdEstadoFicha.ToString());
                        var cellnestadoFicha = row.CreateCell(43); cellnestadoFicha.SetCellValue(ficha.NombreEstadoFicha);
                        var cellModalidad = row.CreateCell(44);
                        cellModalidad.SetCellValue(ficha.FichaPpp.Modalidad == (int)Enums.Modalidad.Entrenamiento ?
                            "Entrenamineto" : "CTI");
                        var cellTarea = row.CreateCell(45); cellTarea.SetCellValue(ficha.FichaPpp.Tareas);
                        var cellaltaTemprana = row.CreateCell(46); cellaltaTemprana.SetCellValue(ficha.FichaPpp.AltaTemprana == "S" ? "SI" : "NO");
                        var cellatFechaInicio = row.CreateCell(47); cellatFechaInicio.SetCellValue(ficha.FichaPpp.FechaInicioActividad == null ? String.Empty :
                            ficha.FichaPpp.FechaInicioActividad.Value.ToShortDateString());
                        var cellatFechaCese = row.CreateCell(48); cellatFechaCese.SetCellValue(ficha.FichaPpp.FechaFinActividad == null ? String.Empty :
                            ficha.FichaPpp.FechaFinActividad.Value.ToShortDateString());
                        var cellidModContAfip = row.CreateCell(49); cellidModContAfip.SetCellValue(ficha.FichaPpp.IdModalidadAfip == null ? String.Empty :
                            ficha.FichaPpp.IdModalidadAfip.ToString());
                        var cellModContAfip = row.CreateCell(50); cellModContAfip.SetCellValue(ficha.FichaPpp.ModalidadAfip);
                        var cellFechaAlta = row.CreateCell(51); cellFechaAlta.SetCellValue(ficha.FechaSistema.ToString() ?? "");

                        //Empresa
                        var cellidEmp = row.CreateCell(52); cellidEmp.SetCellValue(ficha.FichaPpp.IdEmpresa == null ? String.Empty :
                            ficha.FichaPpp.IdEmpresa.ToString());
                        var cellrazonSocial = row.CreateCell(53); cellrazonSocial.SetCellValue(ficha.FichaPpp.Empresa.NombreEmpresa);
                        var cellCuit = row.CreateCell(54); cellCuit.SetCellValue(ficha.FichaPpp.Empresa.Cuit);
                        var cellCodact = row.CreateCell(55); cellCodact.SetCellValue(ficha.FichaPpp.Empresa.CodigoActividad);
                        var cellCantemp = row.CreateCell(56); cellCantemp.SetCellValue(ficha.FichaPpp.Empresa.CantidadEmpleados ?? 0);
                        var cellCalleemp = row.CreateCell(57); cellCalleemp.SetCellValue(ficha.FichaPpp.Empresa.Calle);
                        var cellNumeroemp = row.CreateCell(58); cellNumeroemp.SetCellValue(ficha.FichaPpp.Empresa.Numero);
                        var cellPisoemp = row.CreateCell(59); cellPisoemp.SetCellValue(ficha.FichaPpp.Empresa.Piso);
                        var cellDptoemp = row.CreateCell(60); cellDptoemp.SetCellValue(ficha.FichaPpp.Empresa.Dpto);
                        var cellCpemp = row.CreateCell(61); cellCpemp.SetCellValue(ficha.FichaPpp.Empresa.CodigoPostal);
                        var cellIdlocemp = row.CreateCell(62); cellIdlocemp.SetCellValue(ficha.FichaPpp.Empresa.LocalidadEmpresa.IdLocalidad);
                        var cellLocemp = row.CreateCell(63); cellLocemp.SetCellValue(ficha.FichaPpp.Empresa.LocalidadEmpresa.NombreLocalidad);
                        var cellDepemp = row.CreateCell(64); cellDepemp.SetCellValue(ficha.FichaPpp.Empresa.LocalidadEmpresa.Departamento.NombreDepartamento);
                        var cellIdusuarioemp = row.CreateCell(65); cellIdusuarioemp.SetCellValue(ficha.FichaPpp.Empresa.Usuario.IdUsuarioEmpresa.ToString() ??
                            String.Empty);
                        var cellNombreusuemp = row.CreateCell(66); cellNombreusuemp.SetCellValue(ficha.FichaPpp.Empresa.Usuario.NombreUsuario);
                        var cellApellidousuemp = row.CreateCell(67); cellApellidousuemp.SetCellValue(ficha.FichaPpp.Empresa.Usuario.ApellidoUsuario);
                        var cellEmailusuemp = row.CreateCell(68); cellEmailusuemp.SetCellValue(ficha.FichaPpp.Empresa.Usuario.Mail);
                        var cellTeleusuemp = row.CreateCell(69); cellTeleusuemp.SetCellValue(ficha.FichaPpp.Empresa.Usuario.Telefono);

                        //Beneficiario
                        var cellIdben = row.CreateCell(70); cellIdben.SetCellValue(ficha.BeneficiarioFicha.IdBeneficiarioExport.ToString() ?? String.Empty);
                        var cellIdestadoben = row.CreateCell(71); cellIdestadoben.SetCellValue(ficha.BeneficiarioFicha.IdEstado == 0 ? String.Empty :
                            ficha.BeneficiarioFicha.IdEstado.ToString());
                        var cellEstadoben = row.CreateCell(72); cellEstadoben.SetCellValue(ficha.BeneficiarioFicha.NombreEstado);
                        var cellNrocuentaben = row.CreateCell(73);
                        cellNrocuentaben.SetCellValue(ficha.BeneficiarioFicha.NumeroCuenta == null
                                                          ? String.Empty
                                                          : ficha.BeneficiarioFicha.NumeroCuenta.ToString());
                        var cellIdsucursalben = row.CreateCell(74); cellIdsucursalben.SetCellValue(ficha.BeneficiarioFicha.IdSucursal.ToString() ??
                            String.Empty);
                        var cellCodsucursalben = row.CreateCell(75); cellCodsucursalben.SetCellValue(ficha.BeneficiarioFicha.CodigoSucursal);
                        var cellSucursalben = row.CreateCell(76); cellSucursalben.SetCellValue(ficha.BeneficiarioFicha.SucursalDescripcion);
                        var cellFecsolben = row.CreateCell(77); cellFecsolben.SetCellValue(ficha.BeneficiarioFicha.FechaSolicitudCuenta == null ?
                            String.Empty : ficha.BeneficiarioFicha.FechaSolicitudCuenta.Value.ToShortDateString());
                        var cellNotificado = row.CreateCell(78); cellNotificado.SetCellValue(ficha.BeneficiarioFicha.Notificado == false ? "NO" : "SI");
                        var cellFecNotif = row.CreateCell(79); cellFecNotif.SetCellValue(ficha.BeneficiarioFicha.FechaNotificacion == null ? "" :
                            ficha.BeneficiarioFicha.FechaNotificacion.Value.ToShortDateString());

                        var cellFechaIniBenf = row.CreateCell(80); cellFechaIniBenf.SetCellValue(ficha.BeneficiarioFicha.FechaInicioBeneficio == null ? string.Empty : ficha.BeneficiarioFicha.FechaInicioBeneficio.Value.ToShortDateString()); // 20/03/2013 - DI CAMPLI LEANDRO
                        var cellBajaBenef = row.CreateCell(81); cellBajaBenef.SetCellValue(ficha.BeneficiarioFicha.FechaBajaBeneficio == null ? string.Empty : ficha.BeneficiarioFicha.FechaBajaBeneficio.Value.ToShortDateString()); // 20/03/2013 - DI CAMPLI LEANDRO

                        var cellTieneapoben = row.CreateCell(82); cellTieneapoben.SetCellValue(ficha.BeneficiarioFicha.TieneApoderado);

                        //Apoderado
                        var cellIdapoderado = row.CreateCell(83); cellIdapoderado.SetCellValue(ficha.BeneficiarioFicha.Apoderado.IdApoderadoNullable == null ?
                            String.Empty : ficha.BeneficiarioFicha.Apoderado.IdApoderadoNullable.ToString());
                        var cellApellidoapo = row.CreateCell(84); cellApellidoapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Apellido);
                        var cellNombreapo = row.CreateCell(85); cellNombreapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Nombre);
                        var cellDniapo = row.CreateCell(86); cellDniapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.NumeroDocumento);
                        var cellCuilapo = row.CreateCell(87); cellCuilapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Cuil);
                        var cellSexoapo = row.CreateCell(88); cellSexoapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Sexo);
                        var cellNroctaapo = row.CreateCell(89); cellNroctaapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.NumeroCuentaBco == null ?
                            String.Empty : ficha.BeneficiarioFicha.Apoderado.NumeroCuentaBco.ToString());
                        var cellIdSuc = row.CreateCell(90); cellIdSuc.SetCellValue(ficha.BeneficiarioFicha.Apoderado.IdSucursal == null ? String.Empty :
                            ficha.BeneficiarioFicha.Apoderado.IdSucursal.ToString());
                        var cellCodsucapo = row.CreateCell(91); cellCodsucapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.SucursalApoderado.CodigoBanco);
                        var cellSucursalapo = row.CreateCell(92); cellSucursalapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.SucursalApoderado.Detalle);
                        var cellFecsolcuentaapo = row.CreateCell(93); cellFecsolcuentaapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.FechaSolicitudCuenta == null ?
                            String.Empty : ficha.BeneficiarioFicha.Apoderado.FechaSolicitudCuenta.Value.ToShortDateString());
                        var cellFecnac = row.CreateCell(94); cellFecnac.SetCellValue(ficha.BeneficiarioFicha.Apoderado.FechaNacimiento == null ? String.Empty :
                            ficha.BeneficiarioFicha.Apoderado.FechaNacimiento.Value.ToShortDateString());
                        var cellCalleapo = row.CreateCell(95); cellCalleapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Calle);
                        var cellNroapo = row.CreateCell(96); cellNroapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Numero);
                        var cellPisoapo = row.CreateCell(97); cellPisoapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Piso);
                        var cellMonoblockapo = row.CreateCell(98); cellMonoblockapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Monoblock);
                        var cellDptoapo = row.CreateCell(99); cellDptoapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Dpto);
                        var cellParcelaapo = row.CreateCell(100); cellParcelaapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Parcela);
                        var cellManzanaapo = row.CreateCell(101); cellManzanaapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Manzana);
                        var cellEntrecalleapo = row.CreateCell(102); cellEntrecalleapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.EntreCalles);
                        var cellBarrioapo = row.CreateCell(103); cellBarrioapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Barrio);
                        var cellCpapo = row.CreateCell(104); cellCpapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.CodigoPostal);
                        var cellIdlocalidadapo = row.CreateCell(105); cellIdlocalidadapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.IdLocalidad == null ?
                            String.Empty : ficha.BeneficiarioFicha.Apoderado.LocalidadApoderado.IdLocalidad.ToString());
                        var cellLocalidadapo = row.CreateCell(106); cellLocalidadapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.LocalidadApoderado.NombreLocalidad);
                        var cellIddeptoapo = row.CreateCell(107); cellIddeptoapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.IdLocalidad == null ? String.Empty :
                            ficha.BeneficiarioFicha.Apoderado.LocalidadApoderado.Departamento.IdDepartamento.ToString());
                        var cellDeptoapo = row.CreateCell(108); cellDeptoapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.LocalidadApoderado.Departamento.NombreDepartamento);
                        var cellTelefonoapo = row.CreateCell(109); cellTelefonoapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.TelefonoFijo);
                        var cellCelularapo = row.CreateCell(110); cellCelularapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.TelefonoCelular);
                        var cellEmailapo = row.CreateCell(111);
                        cellEmailapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Mail);
                    }
                    #endregion
                    break;
                case (int)Enums.TipoFicha.Terciaria:
                    #region case (int)Enums.TipoFicha.Terciaria
                    foreach (var ficha in listaparaExcel)
                    {
                        i++;
                        //Ficha
                        var row = sheet.CreateRow(i);
                        var cellidficha = row.CreateCell(0); cellidficha.SetCellValue(ficha.IdFicha);
                        var cellApellido = row.CreateCell(1); cellApellido.SetCellValue(ficha.Apellido);
                        var cellNombre = row.CreateCell(2); cellNombre.SetCellValue(ficha.Nombre);
                        var cellCuil = row.CreateCell(3); cellCuil.SetCellValue(ficha.Cuil);
                        var cellDni = row.CreateCell(4); cellDni.SetCellValue(ficha.NumeroDocumento);
                        var cellFechaNa = row.CreateCell(5); cellFechaNa.SetCellValue(ficha.FechaNacimiento.ToString());
                        var cellCalle = row.CreateCell(6); cellCalle.SetCellValue(ficha.Calle);
                        var cellNro = row.CreateCell(7); cellNro.SetCellValue(ficha.Numero);
                        var cellPiso = row.CreateCell(8); cellPiso.SetCellValue(ficha.Piso);
                        var cellDpto = row.CreateCell(9); cellDpto.SetCellValue(ficha.Dpto);
                        var cellMono = row.CreateCell(10); cellMono.SetCellValue(ficha.Monoblock);
                        var cellParcela = row.CreateCell(11); cellParcela.SetCellValue(ficha.Parcela);
                        var cellManzana = row.CreateCell(12); cellManzana.SetCellValue(ficha.Manzana);
                        var cellEntreCalle = row.CreateCell(13); cellEntreCalle.SetCellValue(ficha.EntreCalles);
                        var cellBarrio = row.CreateCell(14); cellBarrio.SetCellValue(ficha.Barrio);
                        var cellCp = row.CreateCell(15); cellCp.SetCellValue(ficha.CodigoPostal);
                        var cellidLocalidad = row.CreateCell(16); cellidLocalidad.SetCellValue(ficha.IdLocalidad == null ? String.Empty :
                            ficha.LocalidadFicha.IdLocalidad.ToString());
                        var cellnLocalidad = row.CreateCell(17); cellnLocalidad.SetCellValue(ficha.LocalidadFicha.NombreLocalidad);
                        var cellIdDepartamento = row.CreateCell(18); cellIdDepartamento.SetCellValue(ficha.IdLocalidad == null ? String.Empty :
                            ficha.LocalidadFicha.Departamento.IdDepartamento.ToString());
                        var cellnDepartamento = row.CreateCell(19); cellnDepartamento.SetCellValue(ficha.LocalidadFicha.Departamento.NombreDepartamento);
                        var cellTelFijo = row.CreateCell(20); cellTelFijo.SetCellValue(ficha.TelefonoFijo);
                        var cellTelCelular = row.CreateCell(21); cellTelCelular.SetCellValue(ficha.TelefonoCelular);
                        var cellContacto = row.CreateCell(22); cellContacto.SetCellValue(ficha.Contacto);
                        var cellEmail = row.CreateCell(23); cellEmail.SetCellValue(ficha.Mail);
                        var cellSexo = row.CreateCell(24); cellSexo.SetCellValue(ficha.Sexo);
                        var cellidestadoFicha = row.CreateCell(25); cellidestadoFicha.SetCellValue(ficha.IdEstadoFicha == null ? String.Empty
                            : ficha.IdEstadoFicha.ToString());
                        var cellnestadoFicha = row.CreateCell(26); cellnestadoFicha.SetCellValue(ficha.NombreEstadoFicha);

                        var cellPromedioterc = row.CreateCell(27); cellPromedioterc.SetCellValue((ficha.PromedioTerciaria ?? 0).ToString());
                        var cellOtracarreraterc = row.CreateCell(28); cellOtracarreraterc.SetCellValue(ficha.OtraCarreraTerciaria);
                        var cellOtrainstiterc = row.CreateCell(29); cellOtrainstiterc.SetCellValue(ficha.OtraInstitucionTerciaria);
                        var cellCarreraTerc = row.CreateCell(30); cellCarreraTerc.SetCellValue(ficha.IdCarreraTerciaria == null ? String.Empty :
                            ficha.CarreraFicha.NombreCarrera);
                        var cellIdescuelaterc = row.CreateCell(31); cellIdescuelaterc.SetCellValue(ficha.IdEscuelaTerciaria == null ? String.Empty :
                            ficha.EscuelaFicha.Id_Escuela.ToString());
                        var cellEscuelaterc = row.CreateCell(32); cellEscuelaterc.SetCellValue(ficha.EscuelaFicha.Nombre_Escuela);
                        var cellCueterc = row.CreateCell(33); cellCueterc.SetCellValue(ficha.EscuelaFicha.Cue);
                        var cellAnexoterc = row.CreateCell(34); cellAnexoterc.SetCellValue(ficha.EscuelaFicha.Anexo == null
                            ? String.Empty : ficha.EscuelaFicha.Anexo.ToString());

                        var cellEscBarriterc = row.CreateCell(35); cellEscBarriterc.SetCellValue(ficha.EscuelaFicha.Barrio);
                        var cellEscidlocalidadterc = row.CreateCell(36); cellEscidlocalidadterc.SetCellValue(ficha.EscuelaFicha.Id_Localidad == null ?
                            String.Empty : ficha.EscuelaFicha.LocalidadEscuela.IdLocalidad.ToString());
                        var cellEsclocalidadterc = row.CreateCell(37); cellEsclocalidadterc.SetCellValue(ficha.EscuelaFicha.LocalidadEscuela.NombreLocalidad);
                        var cellOtrosectorterc = row.CreateCell(38); cellOtrosectorterc.SetCellValue(ficha.OtroSectorTerciaria);
                        var cellCarreraterc = row.CreateCell(39); cellCarreraterc.SetCellValue(ficha.CarreraFicha.NombreCarrera);
                        var cellIdnivelterc = row.CreateCell(40); cellIdnivelterc.SetCellValue(ficha.IdCarreraTerciaria == null ? String.Empty :
                            ficha.CarreraFicha.NivelCarrera.IdNivel.ToString());
                        var cellNivelterc = row.CreateCell(41); cellNivelterc.SetCellValue(ficha.CarreraFicha.NivelCarrera.NombreNivel);
                        var cellIdinstitucionterc = row.CreateCell(42); cellIdinstitucionterc.SetCellValue(ficha.CarreraFicha.IdInstitucion == null ? String.Empty :
                            ficha.CarreraFicha.InstitucionCarrera.IdInstitucion.ToString());
                        var cellInstitucionterc = row.CreateCell(43); cellInstitucionterc.SetCellValue(ficha.CarreraFicha.InstitucionCarrera.NombreInstitucion);
                        var cellIdsectorterc = row.CreateCell(44); cellIdsectorterc.SetCellValue(ficha.CarreraFicha.IdSector == null ? String.Empty :
                            ficha.CarreraFicha.SectorCarrera.IdSector.ToString());
                        var cellSectorterc = row.CreateCell(45); cellSectorterc.SetCellValue(ficha.CarreraFicha.SectorCarrera.NombreSector);
                        var cellFechaModif = row.CreateCell(46); cellFechaModif.SetCellValue(ficha.FechaSistema == null ? String.Empty
                            : ficha.FechaSistema.Value.ToShortDateString());

                        //ficha.BeneficiarioFicha
                        var cellIdben = row.CreateCell(47); cellIdben.SetCellValue(ficha.BeneficiarioFicha.IdBeneficiarioExport == null ? String.Empty
                            : ficha.BeneficiarioFicha.IdBeneficiarioExport.ToString());
                        var cellIdestadoben = row.CreateCell(48); cellIdestadoben.SetCellValue(ficha.BeneficiarioFicha.IdBeneficiarioExport == null ? String.Empty
                            : ficha.BeneficiarioFicha.IdEstado.ToString());
                        var cellEstadoben = row.CreateCell(49); cellEstadoben.SetCellValue(ficha.BeneficiarioFicha.NombreEstado);
                        var cellNrocuentaben = row.CreateCell(50); cellNrocuentaben.SetCellValue(ficha.BeneficiarioFicha.NumeroCuenta == null ? String.Empty
                            : ficha.BeneficiarioFicha.NumeroCuenta.ToString());
                        var cellIdsucursalben = row.CreateCell(51); cellIdsucursalben.SetCellValue(ficha.BeneficiarioFicha.IdSucursal == null ? String.Empty
                            : ficha.BeneficiarioFicha.IdSucursal.ToString());
                        var cellCodsucursalben = row.CreateCell(52); cellCodsucursalben.SetCellValue(ficha.BeneficiarioFicha.CodigoSucursal);
                        var cellSucursalben = row.CreateCell(53); cellSucursalben.SetCellValue(ficha.BeneficiarioFicha.SucursalDescripcion);
                        var cellFecsolben = row.CreateCell(54); cellFecsolben.SetCellValue(ficha.BeneficiarioFicha.FechaSolicitudCuenta.ToString());
                        var cellNotificado = row.CreateCell(55); cellNotificado.SetCellValue(ficha.BeneficiarioFicha.Notificado == false ? "NO" : "SI");
                        var cellFecNotif = row.CreateCell(56); cellFecNotif.SetCellValue(ficha.BeneficiarioFicha.FechaNotificacion == null ?
                            String.Empty : ficha.BeneficiarioFicha.FechaNotificacion.Value.ToShortDateString());
                        var cellTieneapoben = row.CreateCell(57); cellTieneapoben.SetCellValue(ficha.BeneficiarioFicha.TieneApoderado);

                        //Apoderado
                        var cellIdapoderado = row.CreateCell(58); cellIdapoderado.SetCellValue(ficha.BeneficiarioFicha.Apoderado.IdApoderadoNullable == null ?
                            String.Empty : ficha.BeneficiarioFicha.Apoderado.IdApoderadoNullable.ToString());
                        var cellApellidoapo = row.CreateCell(59); cellApellidoapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Apellido);
                        var cellNombreapo = row.CreateCell(60); cellNombreapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Nombre);
                        var cellDniapo = row.CreateCell(61); cellDniapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.NumeroDocumento);
                        var cellCuilapo = row.CreateCell(62); cellCuilapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Cuil);
                        var cellSexoapo = row.CreateCell(63); cellSexoapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Sexo);
                        var cellNroctaapo = row.CreateCell(64); cellNroctaapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.NumeroCuentaBco == null ?
                            String.Empty : ficha.BeneficiarioFicha.Apoderado.NumeroCuentaBco.ToString());
                        var cellIdSuc = row.CreateCell(65); cellIdSuc.SetCellValue(ficha.BeneficiarioFicha.Apoderado.IdSucursal == null ? String.Empty
                            : ficha.BeneficiarioFicha.Apoderado.SucursalApoderado.IdSucursal.ToString());
                        var cellCodsucapo = row.CreateCell(66); cellCodsucapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.SucursalApoderado.CodigoBanco);
                        var cellSucursalapo = row.CreateCell(67); cellSucursalapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.SucursalApoderado.Detalle);
                        var cellFecsolcuentaapo = row.CreateCell(68); cellFecsolcuentaapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.FechaSolicitudCuenta == null
                            ? String.Empty : ficha.BeneficiarioFicha.Apoderado.FechaSolicitudCuenta.Value.ToShortDateString());
                        var cellFecnac = row.CreateCell(69); cellFecnac.SetCellValue(ficha.BeneficiarioFicha.Apoderado.FechaNacimiento == null ? String.Empty :
                            ficha.BeneficiarioFicha.Apoderado.FechaNacimiento.Value.ToShortDateString());
                        var cellCalleapo = row.CreateCell(70); cellCalleapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Calle);
                        var cellNroapo = row.CreateCell(71); cellNroapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Numero);
                        var cellPisoapo = row.CreateCell(72); cellPisoapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Piso);
                        var cellMonoblockapo = row.CreateCell(73); cellMonoblockapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Monoblock);
                        var cellDptoapo = row.CreateCell(74); cellDptoapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Dpto);
                        var cellParcelaapo = row.CreateCell(75); cellParcelaapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Parcela);
                        var cellManzanaapo = row.CreateCell(76); cellManzanaapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Manzana);
                        var cellEntrecalleapo = row.CreateCell(77); cellEntrecalleapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.EntreCalles);
                        var cellBarrioapo = row.CreateCell(78); cellBarrioapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Barrio);
                        var cellCpapo = row.CreateCell(79); cellCpapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.CodigoPostal);
                        var cellIdlocalidadapo = row.CreateCell(80); cellIdlocalidadapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.IdLocalidad == null ?
                            String.Empty : ficha.BeneficiarioFicha.Apoderado.LocalidadApoderado.IdLocalidad.ToString());
                        var cellLocalidadapo = row.CreateCell(81); cellLocalidadapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.LocalidadApoderado.NombreLocalidad);
                        var cellIddeptoapo = row.CreateCell(82); cellIddeptoapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.IdLocalidad == null ?
                            String.Empty : ficha.BeneficiarioFicha.Apoderado.LocalidadApoderado.Departamento.IdDepartamento.ToString());
                        var cellDeptoapo = row.CreateCell(83); cellDeptoapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.LocalidadApoderado.Departamento.NombreDepartamento);
                        var cellTelefonoapo = row.CreateCell(84); cellTelefonoapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.TelefonoFijo);
                        var cellCelularapo = row.CreateCell(85); cellCelularapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.TelefonoCelular);
                        var cellEmailapo = row.CreateCell(86); cellEmailapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Mail);
                    }
                    #endregion
                    break;
                case (int)Enums.TipoFicha.Universitaria:
                    #region case (int)Enums.TipoFicha.Universitaria
                    foreach (var ficha in listaparaExcel)
                    {
                        i++;
                        //Ficha
                        var row = sheet.CreateRow(i);
                        var cellidficha = row.CreateCell(0); cellidficha.SetCellValue(ficha.IdFicha);
                        var cellApellido = row.CreateCell(1); cellApellido.SetCellValue(ficha.Apellido);
                        var cellNombre = row.CreateCell(2); cellNombre.SetCellValue(ficha.Nombre);
                        var cellCuil = row.CreateCell(3); cellCuil.SetCellValue(ficha.Cuil);
                        var cellDni = row.CreateCell(4); cellDni.SetCellValue(ficha.NumeroDocumento);
                        var cellFechaNa = row.CreateCell(5); cellFechaNa.SetCellValue(ficha.FechaNacimiento == null ? String.Empty :
                            ficha.FechaNacimiento.Value.ToShortDateString());
                        var cellCalle = row.CreateCell(6); cellCalle.SetCellValue(ficha.Calle);
                        var cellNro = row.CreateCell(7); cellNro.SetCellValue(ficha.Numero);
                        var cellPiso = row.CreateCell(8); cellPiso.SetCellValue(ficha.Piso);
                        var cellDpto = row.CreateCell(9); cellDpto.SetCellValue(ficha.Dpto);
                        var cellMono = row.CreateCell(10); cellMono.SetCellValue(ficha.Monoblock);
                        var cellParcela = row.CreateCell(11); cellParcela.SetCellValue(ficha.Parcela);
                        var cellManzana = row.CreateCell(12); cellManzana.SetCellValue(ficha.Manzana);
                        var cellEntreCalle = row.CreateCell(13); cellEntreCalle.SetCellValue(ficha.EntreCalles);
                        var cellBarrio = row.CreateCell(14); cellBarrio.SetCellValue(ficha.Barrio);
                        var cellCp = row.CreateCell(15); cellCp.SetCellValue(ficha.CodigoPostal);
                        var cellidLocalidad = row.CreateCell(16); cellidLocalidad.SetCellValue(ficha.IdLocalidad == null ? String.Empty :
                            ficha.LocalidadFicha.IdLocalidad.ToString());
                        var cellnLocalidad = row.CreateCell(17); cellnLocalidad.SetCellValue(ficha.LocalidadFicha.NombreLocalidad);
                        var cellIdDepartamento = row.CreateCell(18); cellIdDepartamento.SetCellValue(ficha.IdLocalidad == null ? String.Empty :
                            ficha.LocalidadFicha.Departamento.IdDepartamento.ToString());
                        var cellnDepartamento = row.CreateCell(19); cellnDepartamento.SetCellValue(ficha.LocalidadFicha.Departamento.NombreDepartamento);
                        var cellTelFijo = row.CreateCell(20); cellTelFijo.SetCellValue(ficha.TelefonoFijo);
                        var cellTelCelular = row.CreateCell(21); cellTelCelular.SetCellValue(ficha.TelefonoCelular);
                        var cellContacto = row.CreateCell(22); cellContacto.SetCellValue(ficha.Contacto);
                        var cellEmail = row.CreateCell(23); cellEmail.SetCellValue(ficha.Mail);
                        var cellSexo = row.CreateCell(24); cellSexo.SetCellValue(ficha.Sexo);
                        var cellidestadoFicha = row.CreateCell(25); cellidestadoFicha.SetCellValue(ficha.IdEstadoFicha == null ? String.Empty
                            : ficha.IdEstadoFicha.ToString());
                        var cellnestadoFicha = row.CreateCell(26); cellnestadoFicha.SetCellValue(ficha.NombreEstadoFicha);

                        var cellPromedioUniv = row.CreateCell(27); cellPromedioUniv.SetCellValue(ficha.PromedioUniversitaria == null ? String.Empty
                            : ficha.PromedioUniversitaria.ToString());
                        var cellOtraCarreraUniv = row.CreateCell(28); cellOtraCarreraUniv.SetCellValue(ficha.OtraCarreraUniversitaria);
                        var cellOtraInstiUniv = row.CreateCell(29); cellOtraInstiUniv.SetCellValue(ficha.OtraInstitucionUniversitaria);
                        var cellIdCarreraUniv = row.CreateCell(30); cellIdCarreraUniv.SetCellValue(ficha.IdCarreraUniversitaria == null ? String.Empty
                            : ficha.CarreraFicha.IdCarrera.ToString());
                        var cellCarreraUniv = row.CreateCell(31); cellCarreraUniv.SetCellValue(ficha.CarreraFicha.NombreCarrera);
                        var cellIdEscuelaUniv = row.CreateCell(32); cellIdEscuelaUniv.SetCellValue(ficha.IdEscuelaUniversitaria == null ? String.Empty
                            : ficha.EscuelaFicha.Id_Escuela.ToString());
                        var cellEscuelaUniv = row.CreateCell(33); cellEscuelaUniv.SetCellValue(ficha.EscuelaFicha.Nombre_Escuela);
                        var cellCueUniv = row.CreateCell(34); cellCueUniv.SetCellValue(ficha.EscuelaFicha.Cue);
                        var cellAnexoUniv = row.CreateCell(35); cellAnexoUniv.SetCellValue(ficha.EscuelaFicha.Anexo.ToString() ?? String.Empty);

                        var cellEscBarriUniv = row.CreateCell(36); cellEscBarriUniv.SetCellValue(ficha.EscuelaFicha.Barrio);
                        var cellEscIdLocalidadUniv = row.CreateCell(37); cellEscIdLocalidadUniv.SetCellValue(ficha.IdEscuelaUniversitaria == null ? String.Empty
                            : ficha.EscuelaFicha.LocalidadEscuela.IdLocalidad.ToString());
                        var cellEscLocalidadUniv = row.CreateCell(38); cellEscLocalidadUniv.SetCellValue(ficha.EscuelaFicha.LocalidadEscuela.NombreLocalidad);
                        var cellOtroSectorUniv = row.CreateCell(39); cellOtroSectorUniv.SetCellValue(ficha.OtroSectorTerciaria);

                        var cellIdNivelUniv = row.CreateCell(40); cellIdNivelUniv.SetCellValue(ficha.IdCarreraUniversitaria == null ? String.Empty
                            : ficha.CarreraFicha.NivelCarrera.IdNivel.ToString());
                        var cellNivelUniv = row.CreateCell(41); cellNivelUniv.SetCellValue(ficha.CarreraFicha.NivelCarrera.NombreNivel);
                        var cellIdInstitucionUniv = row.CreateCell(42); cellIdInstitucionUniv.SetCellValue(ficha.IdCarreraUniversitaria == null ? String.Empty
                            : ficha.CarreraFicha.InstitucionCarrera.IdInstitucion.ToString());
                        var cellInstitucionUniv = row.CreateCell(43); cellInstitucionUniv.SetCellValue(ficha.CarreraFicha.InstitucionCarrera.NombreInstitucion);
                        var cellIdSectorUniv = row.CreateCell(44); cellIdSectorUniv.SetCellValue(ficha.IdCarreraUniversitaria == null ? String.Empty
                            : ficha.CarreraFicha.SectorCarrera.IdSector.ToString());
                        var cellSectorUniv = row.CreateCell(45); cellSectorUniv.SetCellValue(ficha.CarreraFicha.SectorCarrera.NombreSector);
                        var cellFechaModif = row.CreateCell(46); cellFechaModif.SetCellValue(ficha.FechaSistema == null ? String.Empty
                            : ficha.FechaSistema.Value.ToShortDateString());

                        //Beneficiario
                        var cellIdben = row.CreateCell(47); cellIdben.SetCellValue(ficha.BeneficiarioFicha.IdBeneficiarioExport == null ?
                            String.Empty : ficha.BeneficiarioFicha.IdBeneficiarioExport.ToString());
                        var cellIdestadoben = row.CreateCell(48); cellIdestadoben.SetCellValue(ficha.BeneficiarioFicha.IdBeneficiarioExport == null ?
                            String.Empty : ficha.BeneficiarioFicha.IdEstado.ToString());
                        var cellEstadoben = row.CreateCell(49); cellEstadoben.SetCellValue(ficha.BeneficiarioFicha.NombreEstado);
                        var cellNrocuentaben = row.CreateCell(50); cellNrocuentaben.SetCellValue(ficha.BeneficiarioFicha.NumeroCuenta == null ? String.Empty
                            : ficha.BeneficiarioFicha.NumeroCuenta.ToString());
                        var cellIdsucursalben = row.CreateCell(51); cellIdsucursalben.SetCellValue(ficha.BeneficiarioFicha.IdSucursal == null ? String.Empty
                            : ficha.BeneficiarioFicha.IdSucursal.ToString());
                        var cellCodsucursalben = row.CreateCell(52); cellCodsucursalben.SetCellValue(ficha.BeneficiarioFicha.CodigoSucursal);
                        var cellSucursalben = row.CreateCell(53); cellSucursalben.SetCellValue(ficha.BeneficiarioFicha.SucursalDescripcion);
                        var cellFecsolben = row.CreateCell(54); cellFecsolben.SetCellValue(ficha.BeneficiarioFicha.FechaSolicitudCuenta == null ?
                            String.Empty : ficha.BeneficiarioFicha.FechaSolicitudCuenta.Value.ToShortDateString());
                        var cellNotificado = row.CreateCell(55); cellNotificado.SetCellValue(ficha.BeneficiarioFicha.Notificado == false ? "NO" : "SI");
                        var cellFecNotif = row.CreateCell(56); cellFecNotif.SetCellValue(ficha.BeneficiarioFicha.FechaNotificacion == null ? String.Empty
                            : ficha.BeneficiarioFicha.FechaNotificacion.Value.ToShortDateString());
                        var cellTieneapoben = row.CreateCell(57); cellTieneapoben.SetCellValue(ficha.BeneficiarioFicha.TieneApoderado);

                        //Apoderado
                        var cellIdapoderado = row.CreateCell(58); cellIdapoderado.SetCellValue(ficha.BeneficiarioFicha.Apoderado.IdApoderadoNullable == null ?
                            String.Empty : ficha.BeneficiarioFicha.Apoderado.IdApoderadoNullable.ToString());
                        var cellApellidoapo = row.CreateCell(59); cellApellidoapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Apellido);
                        var cellNombreapo = row.CreateCell(60); cellNombreapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Nombre);
                        var cellDniapo = row.CreateCell(61); cellDniapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.NumeroDocumento);
                        var cellCuilapo = row.CreateCell(62); cellCuilapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Cuil);
                        var cellSexoapo = row.CreateCell(63); cellSexoapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Sexo);
                        var cellNroctaapo = row.CreateCell(64); cellNroctaapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.NumeroCuentaBco == null ?
                            String.Empty : ficha.BeneficiarioFicha.Apoderado.NumeroCuentaBco.ToString());
                        var cellIdSuc = row.CreateCell(65); cellIdSuc.SetCellValue(ficha.BeneficiarioFicha.Apoderado.IdSucursal == null ? String.Empty
                            : ficha.BeneficiarioFicha.Apoderado.IdSucursal.ToString());
                        var cellCodsucapo = row.CreateCell(66); cellCodsucapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.SucursalApoderado.CodigoBanco);
                        var cellSucursalapo = row.CreateCell(67); cellSucursalapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.SucursalApoderado.Detalle);
                        var cellFecsolcuentaapo = row.CreateCell(68); cellFecsolcuentaapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.FechaSolicitudCuenta == null ?
                            String.Empty : ficha.BeneficiarioFicha.Apoderado.FechaSolicitudCuenta.Value.ToShortDateString());
                        var cellFecnac = row.CreateCell(69); cellFecnac.SetCellValue(ficha.BeneficiarioFicha.Apoderado.FechaNacimiento == null ? String.Empty
                            : ficha.BeneficiarioFicha.Apoderado.FechaNacimiento.Value.ToShortDateString());
                        var cellCalleapo = row.CreateCell(70); cellCalleapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Calle);
                        var cellNroapo = row.CreateCell(71); cellNroapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Numero);
                        var cellPisoapo = row.CreateCell(72); cellPisoapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Piso);
                        var cellMonoblockapo = row.CreateCell(73); cellMonoblockapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Monoblock);
                        var cellDptoapo = row.CreateCell(74); cellDptoapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Dpto);
                        var cellParcelaapo = row.CreateCell(75); cellParcelaapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Parcela);
                        var cellManzanaapo = row.CreateCell(76); cellManzanaapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Manzana);
                        var cellEntrecalleapo = row.CreateCell(77); cellEntrecalleapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.EntreCalles);
                        var cellBarrioapo = row.CreateCell(78); cellBarrioapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Barrio);
                        var cellCpapo = row.CreateCell(79); cellCpapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.CodigoPostal);
                        var cellIdlocalidadapo = row.CreateCell(80); cellIdlocalidadapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.IdLocalidad == null ?
                            String.Empty : ficha.BeneficiarioFicha.Apoderado.LocalidadApoderado.IdLocalidad.ToString());
                        var cellLocalidadapo = row.CreateCell(81); cellLocalidadapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.LocalidadApoderado.NombreLocalidad);
                        var cellIddeptoapo = row.CreateCell(82); cellIddeptoapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.IdLocalidad == null ?
                            String.Empty : ficha.BeneficiarioFicha.Apoderado.LocalidadApoderado.Departamento.IdDepartamento.ToString());
                        var cellDeptoapo = row.CreateCell(83); cellDeptoapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.LocalidadApoderado.Departamento.NombreDepartamento);
                        var cellTelefonoapo = row.CreateCell(84); cellTelefonoapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.TelefonoFijo);
                        var cellCelularapo = row.CreateCell(85); cellCelularapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.TelefonoCelular);
                        var cellEmailapo = row.CreateCell(86);
                        cellEmailapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Mail);
                    }
                    #endregion
                    break;
                case (int)Enums.TipoFicha.PppProf:
                    #region case (int)Enums.TipoFicha.PppProf
                    foreach (var ficha in listaparaExcel)
                    {
                        i++;
                        //Ficha
                        var row = sheet.CreateRow(i);
                        var cellidficha = row.CreateCell(0); cellidficha.SetCellValue(ficha.IdFicha);
                        var cellApellido = row.CreateCell(1); cellApellido.SetCellValue(ficha.Apellido);
                        var cellNombre = row.CreateCell(2); cellNombre.SetCellValue(ficha.Nombre);
                        var cellCuil = row.CreateCell(3); cellCuil.SetCellValue(ficha.Cuil);
                        var cellDni = row.CreateCell(4); cellDni.SetCellValue(ficha.NumeroDocumento);
                        var cellFechaNa = row.CreateCell(5); cellFechaNa.SetCellValue(ficha.FechaNacimiento == null ? String.Empty
                            : ficha.FechaNacimiento.Value.ToShortDateString());
                        var cellCalle = row.CreateCell(6); cellCalle.SetCellValue(ficha.Calle);
                        var cellNro = row.CreateCell(7); cellNro.SetCellValue(ficha.Numero);
                        var cellPiso = row.CreateCell(8); cellPiso.SetCellValue(ficha.Piso);
                        var cellDpto = row.CreateCell(9); cellDpto.SetCellValue(ficha.Dpto);
                        var cellMono = row.CreateCell(10); cellMono.SetCellValue(ficha.Monoblock);
                        var cellParcela = row.CreateCell(11); cellParcela.SetCellValue(ficha.Parcela);
                        var cellManzana = row.CreateCell(12); cellManzana.SetCellValue(ficha.Manzana);
                        var cellEntreCalle = row.CreateCell(13); cellEntreCalle.SetCellValue(ficha.EntreCalles);
                        var cellBarrio = row.CreateCell(14); cellBarrio.SetCellValue(ficha.Barrio);
                        var cellCp = row.CreateCell(15); cellCp.SetCellValue(ficha.CodigoPostal);
                        var cellidLocalidad = row.CreateCell(16); cellidLocalidad.SetCellValue(ficha.LocalidadFicha.IdLocalidad);
                        var cellnLocalidad = row.CreateCell(17); cellnLocalidad.SetCellValue(ficha.LocalidadFicha.NombreLocalidad);
                        var cellIdDepartamento = row.CreateCell(18); cellIdDepartamento.SetCellValue(ficha.LocalidadFicha.Departamento.IdDepartamento);
                        var cellnDepartamento = row.CreateCell(19); cellnDepartamento.SetCellValue(ficha.LocalidadFicha.Departamento.NombreDepartamento);
                        var cellTelFijo = row.CreateCell(20); cellTelFijo.SetCellValue(ficha.TelefonoFijo);
                        var cellTelCelular = row.CreateCell(21); cellTelCelular.SetCellValue(ficha.TelefonoCelular);
                        var cellContacto = row.CreateCell(22); cellContacto.SetCellValue(ficha.Contacto);
                        var cellEmail = row.CreateCell(23); cellEmail.SetCellValue(ficha.Mail);
                        var cellSexo = row.CreateCell(24); cellSexo.SetCellValue(ficha.Sexo);
                        var cellidestadoFicha = row.CreateCell(25); cellidestadoFicha.SetCellValue(ficha.IdEstadoFicha == null ? String.Empty
                            : ficha.IdEstadoFicha.ToString());
                        var cellnestadoFicha = row.CreateCell(26); cellnestadoFicha.SetCellValue(ficha.NombreEstadoFicha);
                        var cellModalidad = row.CreateCell(27);
                        cellModalidad.SetCellValue(ficha.FichaPppp.Modalidad == (int)Enums.Modalidad.Entrenamiento ?
                            "Entrenamiento" : "CTI");
                        var cellTarea = row.CreateCell(28); cellTarea.SetCellValue(ficha.FichaPppp.Tareas);
                        var cellaltaTemprana = row.CreateCell(29); cellaltaTemprana.SetCellValue(ficha.FichaPppp.AltaTemprana);
                        var cellatFechaInicio = row.CreateCell(30); cellatFechaInicio.SetCellValue(ficha.FichaPppp.FechaInicioActividad == null ? String.Empty
                            : ficha.FichaPppp.FechaInicioActividad.Value.ToShortDateString());
                        var cellatFechaCese = row.CreateCell(31); cellatFechaCese.SetCellValue(ficha.FichaPppp.FechaFinActividad == null ? String.Empty
                            : ficha.FichaPppp.FechaFinActividad.Value.ToShortDateString());
                        var cellIdModContAfip = row.CreateCell(32); cellIdModContAfip.SetCellValue(ficha.FichaPppp.IdModalidadAFIP == null ? String.Empty
                            : ficha.FichaPppp.IdModalidadAFIP.ToString());
                        var cellModContAfip = row.CreateCell(33); cellModContAfip.SetCellValue(ficha.FichaPppp.ModalidadAFIP);
                        var cellFechaModif = row.CreateCell(34); cellFechaModif.SetCellValue(ficha.FechaSistema == null ? String.Empty
                            : ficha.FechaSistema.Value.ToShortDateString());

                        //Empresa
                        var cellidEmp = row.CreateCell(35); cellidEmp.SetCellValue(ficha.FichaPppp.IdEmpresa == null ? String.Empty
                            : ficha.FichaPppp.Empresa.IdEmpresa.ToString());
                        var cellrazonSocial = row.CreateCell(36); cellrazonSocial.SetCellValue(ficha.FichaPppp.Empresa.NombreEmpresa);
                        var cellCuit = row.CreateCell(37); cellCuit.SetCellValue(ficha.FichaPppp.Empresa.Cuit);
                        var cellCodact = row.CreateCell(38); cellCodact.SetCellValue(ficha.FichaPppp.Empresa.CodigoActividad);
                        var cellCantemp = row.CreateCell(39); cellCantemp.SetCellValue(ficha.FichaPppp.Empresa.CantidadEmpleados == null ?
                            String.Empty : ficha.FichaPppp.Empresa.CantidadEmpleados.ToString());
                        var cellCalleemp = row.CreateCell(40); cellCalleemp.SetCellValue(ficha.FichaPppp.Empresa.Calle);
                        var cellNumeroemp = row.CreateCell(41); cellNumeroemp.SetCellValue(ficha.FichaPppp.Empresa.Numero);
                        var cellPisoemp = row.CreateCell(42); cellPisoemp.SetCellValue(ficha.FichaPppp.Empresa.Piso);
                        var cellDptoemp = row.CreateCell(43); cellDptoemp.SetCellValue(ficha.FichaPppp.Empresa.Dpto);
                        var cellCpemp = row.CreateCell(44); cellCpemp.SetCellValue(ficha.FichaPppp.Empresa.CodigoPostal);
                        var cellIdlocemp = row.CreateCell(45); cellIdlocemp.SetCellValue(ficha.FichaPppp.Empresa.IdLocalidad == null ?
                            String.Empty : ficha.FichaPppp.Empresa.LocalidadEmpresa.IdLocalidad.ToString());
                        var cellLocemp = row.CreateCell(46); cellLocemp.SetCellValue(ficha.FichaPppp.Empresa.LocalidadEmpresa.NombreLocalidad);
                        var cellIddepemp = row.CreateCell(47); cellIddepemp.SetCellValue(ficha.FichaPppp.Empresa.IdLocalidad == null ?
                            String.Empty : ficha.FichaPppp.Empresa.LocalidadEmpresa.Departamento.NombreDepartamento);
                        var cellIdusuarioemp = row.CreateCell(48); cellIdusuarioemp.SetCellValue(ficha.FichaPppp.Empresa.Usuario.IdUsuarioEmpresa == null ?
                            String.Empty : ficha.FichaPppp.Empresa.Usuario.IdUsuarioEmpresa.ToString());
                        var cellNombreusuemp = row.CreateCell(49); cellNombreusuemp.SetCellValue(ficha.FichaPppp.Empresa.Usuario.NombreUsuario);
                        var cellApellidousuemp = row.CreateCell(50); cellApellidousuemp.SetCellValue(ficha.FichaPppp.Empresa.Usuario.ApellidoUsuario);
                        var cellEmailusuemp = row.CreateCell(51); cellEmailusuemp.SetCellValue(ficha.FichaPppp.Empresa.Usuario.Mail);
                        var cellTeleusuemp = row.CreateCell(52); cellTeleusuemp.SetCellValue(ficha.FichaPppp.Empresa.Usuario.Telefono);

                        //Beneficiario
                        var cellIdben = row.CreateCell(53); cellIdben.SetCellValue(ficha.BeneficiarioFicha.IdBeneficiarioExport == null ?
                            String.Empty : ficha.BeneficiarioFicha.IdBeneficiarioExport.ToString());
                        var cellIdestadoben = row.CreateCell(54); cellIdestadoben.SetCellValue(ficha.BeneficiarioFicha.IdBeneficiarioExport == null ?
                            String.Empty : ficha.BeneficiarioFicha.IdEstado.ToString());
                        var cellEstadoben = row.CreateCell(55); cellEstadoben.SetCellValue(ficha.BeneficiarioFicha.NombreEstado);
                        var cellNrocuentaben = row.CreateCell(56); cellNrocuentaben.SetCellValue(ficha.BeneficiarioFicha.NumeroCuenta == null ?
                            String.Empty : ficha.BeneficiarioFicha.NumeroCuenta.ToString());
                        var cellIdsucursalben = row.CreateCell(57); cellIdsucursalben.SetCellValue(ficha.BeneficiarioFicha.IdSucursal == null ?
                            String.Empty : ficha.BeneficiarioFicha.IdSucursal.ToString());
                        var cellCodsucursalben = row.CreateCell(58); cellCodsucursalben.SetCellValue(ficha.BeneficiarioFicha.CodigoSucursal);
                        var cellSucursalben = row.CreateCell(59); cellSucursalben.SetCellValue(ficha.BeneficiarioFicha.SucursalDescripcion);
                        var cellFecsolben = row.CreateCell(60); cellFecsolben.SetCellValue(ficha.BeneficiarioFicha.FechaSolicitudCuenta == null ?
                            String.Empty : ficha.BeneficiarioFicha.FechaSolicitudCuenta.Value.ToShortDateString());
                        var cellNotificado = row.CreateCell(61); cellNotificado.SetCellValue(ficha.BeneficiarioFicha.Notificado == false ? "NO" : "SI");
                        var cellFecNotif = row.CreateCell(62); cellFecNotif.SetCellValue(ficha.BeneficiarioFicha.FechaNotificacion == null ?
                            String.Empty : ficha.BeneficiarioFicha.FechaNotificacion.Value.ToShortDateString());
                        var cellTieneapoben = row.CreateCell(63); cellTieneapoben.SetCellValue(ficha.BeneficiarioFicha.TieneApoderado);

                        //Apoderado
                        var cellIdapoderado = row.CreateCell(64); cellIdapoderado.SetCellValue(ficha.BeneficiarioFicha.Apoderado.IdApoderadoNullable == null ?
                            String.Empty : ficha.BeneficiarioFicha.Apoderado.IdApoderadoNullable.ToString());
                        var cellApellidoapo = row.CreateCell(65); cellApellidoapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Apellido);
                        var cellNombreapo = row.CreateCell(66); cellNombreapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Nombre);
                        var cellDniapo = row.CreateCell(67); cellDniapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.NumeroDocumento);
                        var cellCuilapo = row.CreateCell(68); cellCuilapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Cuil);
                        var cellSexoapo = row.CreateCell(69); cellSexoapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Sexo);
                        var cellNroctaapo = row.CreateCell(70); cellNroctaapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.NumeroCuentaBco == null ?
                            String.Empty : ficha.BeneficiarioFicha.Apoderado.NumeroCuentaBco.ToString());
                        var cellIdSuc = row.CreateCell(71); cellIdSuc.SetCellValue(ficha.BeneficiarioFicha.Apoderado.IdSucursal == null ?
                            String.Empty : ficha.BeneficiarioFicha.Apoderado.IdSucursal.ToString());
                        var cellCodsucapo = row.CreateCell(72); cellCodsucapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.SucursalApoderado.CodigoBanco);
                        var cellSucursalapo = row.CreateCell(73); cellSucursalapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.SucursalApoderado.Detalle);
                        var cellFecsolcuentaapo = row.CreateCell(74); cellFecsolcuentaapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.FechaSolicitudCuenta == null ?
                            String.Empty : ficha.BeneficiarioFicha.Apoderado.FechaSolicitudCuenta.Value.ToShortDateString());
                        var cellFecnac = row.CreateCell(75); cellFecnac.SetCellValue(ficha.BeneficiarioFicha.Apoderado.FechaNacimiento == null ? String.Empty :
                            ficha.BeneficiarioFicha.Apoderado.FechaNacimiento.Value.ToShortDateString());
                        var cellCalleapo = row.CreateCell(76); cellCalleapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Calle);
                        var cellNroapo = row.CreateCell(77); cellNroapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Numero);
                        var cellPisoapo = row.CreateCell(78); cellPisoapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Piso);
                        var cellMonoblockapo = row.CreateCell(79); cellMonoblockapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Monoblock);
                        var cellDptoapo = row.CreateCell(80); cellDptoapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Dpto);
                        var cellParcelaapo = row.CreateCell(81); cellParcelaapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Parcela);
                        var cellManzanaapo = row.CreateCell(82); cellManzanaapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Manzana);
                        var cellEntrecalleapo = row.CreateCell(83); cellEntrecalleapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.EntreCalles);
                        var cellBarrioapo = row.CreateCell(84); cellBarrioapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Barrio);
                        var cellCpapo = row.CreateCell(85); cellCpapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.CodigoPostal);
                        var cellIdlocalidadapo = row.CreateCell(86); cellIdlocalidadapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.IdLocalidad == null ?
                            String.Empty : ficha.BeneficiarioFicha.Apoderado.LocalidadApoderado.IdLocalidad.ToString());
                        var cellLocalidadapo = row.CreateCell(87); cellLocalidadapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.LocalidadApoderado.NombreLocalidad);
                        var cellIddeptoapo = row.CreateCell(88); cellIddeptoapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.IdLocalidad == null ?
                            String.Empty : ficha.BeneficiarioFicha.Apoderado.LocalidadApoderado.Departamento.IdDepartamento.ToString());
                        var cellDeptoapo = row.CreateCell(89); cellDeptoapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.LocalidadApoderado.Departamento.NombreDepartamento);
                        var cellTelefonoapo = row.CreateCell(90); cellTelefonoapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.TelefonoFijo);
                        var cellCelularapo = row.CreateCell(91); cellCelularapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.TelefonoCelular);
                        var cellEmailapo = row.CreateCell(92);
                        cellEmailapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Mail);
                    }
                    #endregion
                    break;
                case (int)Enums.TipoFicha.Vat:
                    #region case (int)Enums.TipoFicha.Vat
                    foreach (var ficha in listaparaExcel)
                    {
                        i++;
                        //Ficha
                        var row = sheet.CreateRow(i);
                        var cellidficha = row.CreateCell(0); cellidficha.SetCellValue(ficha.IdFicha);
                        var cellApellido = row.CreateCell(1); cellApellido.SetCellValue(ficha.Apellido);
                        var cellNombre = row.CreateCell(2); cellNombre.SetCellValue(ficha.Nombre);
                        var cellCuil = row.CreateCell(3); cellCuil.SetCellValue(ficha.Cuil);
                        var cellDni = row.CreateCell(4); cellDni.SetCellValue(ficha.NumeroDocumento);
                        var cellFechaNa = row.CreateCell(5); cellFechaNa.SetCellValue(ficha.FechaNacimiento == null ? String.Empty :
                            ficha.FechaNacimiento.Value.ToShortDateString());
                        var cellCalle = row.CreateCell(6); cellCalle.SetCellValue(ficha.Calle);
                        var cellNro = row.CreateCell(7); cellNro.SetCellValue(ficha.Numero);
                        var cellPiso = row.CreateCell(8); cellPiso.SetCellValue(ficha.Piso);
                        var cellDpto = row.CreateCell(9); cellDpto.SetCellValue(ficha.Dpto);
                        var cellMono = row.CreateCell(10); cellMono.SetCellValue(ficha.Monoblock);
                        var cellParcela = row.CreateCell(11); cellParcela.SetCellValue(ficha.Parcela);
                        var cellManzana = row.CreateCell(12); cellManzana.SetCellValue(ficha.Manzana);
                        var cellEntreCalle = row.CreateCell(13); cellEntreCalle.SetCellValue(ficha.EntreCalles);
                        var cellBarrio = row.CreateCell(14); cellBarrio.SetCellValue(ficha.Barrio);
                        var cellCp = row.CreateCell(15); cellCp.SetCellValue(ficha.CodigoPostal);
                        var cellidLocalidad = row.CreateCell(16); cellidLocalidad.SetCellValue(ficha.LocalidadFicha.IdLocalidad);
                        var cellnLocalidad = row.CreateCell(17); cellnLocalidad.SetCellValue(ficha.LocalidadFicha.NombreLocalidad);
                        var cellIdDepartamento = row.CreateCell(18); cellIdDepartamento.SetCellValue(ficha.LocalidadFicha.Departamento.IdDepartamento);
                        var cellnDepartamento = row.CreateCell(19); cellnDepartamento.SetCellValue(ficha.LocalidadFicha.Departamento.NombreDepartamento);
                        var cellTelFijo = row.CreateCell(20); cellTelFijo.SetCellValue(ficha.TelefonoFijo);
                        var cellTelCelular = row.CreateCell(21); cellTelCelular.SetCellValue(ficha.TelefonoCelular);
                        var cellContacto = row.CreateCell(22); cellContacto.SetCellValue(ficha.Contacto);
                        var cellEmail = row.CreateCell(23); cellEmail.SetCellValue(ficha.Mail);
                        var cellSexo = row.CreateCell(24); cellSexo.SetCellValue(ficha.Sexo);
                        var cellidestadoFicha = row.CreateCell(25); cellidestadoFicha.SetCellValue(ficha.IdEstadoFicha == null ? String.Empty
                            : ficha.IdEstadoFicha.ToString());
                        var cellnestadoFicha = row.CreateCell(26); cellnestadoFicha.SetCellValue(ficha.NombreEstadoFicha);
                        var cellModalidad = row.CreateCell(27); cellModalidad.SetCellValue(ficha.FichaVat.Modalidad == (int)Enums.Modalidad.Entrenamiento ?
                            "Entrenamineto" : "CTI");
                        var cellTarea = row.CreateCell(28); cellTarea.SetCellValue(ficha.FichaVat.Tareas);
                        var cellaltaTemprana = row.CreateCell(29); cellaltaTemprana.SetCellValue(ficha.FichaVat.AltaTemprana);
                        var cellatFechaInicio = row.CreateCell(30); cellatFechaInicio.SetCellValue(ficha.FichaVat.FechaInicioActividad == null ?
                            String.Empty : ficha.FichaVat.FechaInicioActividad.Value.ToShortDateString());
                        var cellatFechaCese = row.CreateCell(31); cellatFechaCese.SetCellValue(ficha.FichaVat.FechaFinActividad == null ?
                            String.Empty : ficha.FichaVat.FechaFinActividad.Value.ToShortDateString());
                        var cellidModContAfip = row.CreateCell(32); cellidModContAfip.SetCellValue(ficha.FichaVat.IdModalidadAFIP == null ?
                            String.Empty : ficha.FichaVat.IdModalidadAFIP.ToString());
                        var cellModContAfip = row.CreateCell(33); cellModContAfip.SetCellValue(ficha.FichaVat.ModalidadAFIP);
                        var cellFechaModif = row.CreateCell(34); cellFechaModif.SetCellValue(ficha.FechaSistema == null ? String.Empty
                            : ficha.FechaSistema.Value.ToShortDateString());

                        //Empresa
                        var cellidEmp = row.CreateCell(35); cellidEmp.SetCellValue(ficha.FichaVat.IdEmpresa == null ? String.Empty
                            : ficha.FichaVat.Empresa.IdEmpresa.ToString());
                        var cellrazonSocial = row.CreateCell(36); cellrazonSocial.SetCellValue(ficha.FichaVat.Empresa.NombreEmpresa);
                        var cellCuit = row.CreateCell(37); cellCuit.SetCellValue(ficha.FichaVat.Empresa.Cuit);
                        var cellCodact = row.CreateCell(38); cellCodact.SetCellValue(ficha.FichaVat.Empresa.CodigoActividad);
                        var cellCantemp = row.CreateCell(39); cellCantemp.SetCellValue(ficha.FichaVat.Empresa.CantidadEmpleados == null ? String.Empty
                            : ficha.FichaVat.Empresa.CantidadEmpleados.ToString());
                        var cellCalleemp = row.CreateCell(40); cellCalleemp.SetCellValue(ficha.FichaVat.Empresa.Calle);
                        var cellNumeroemp = row.CreateCell(41); cellNumeroemp.SetCellValue(ficha.FichaVat.Empresa.Numero);
                        var cellPisoemp = row.CreateCell(42); cellPisoemp.SetCellValue(ficha.FichaVat.Empresa.Piso);
                        var cellDptoemp = row.CreateCell(43); cellDptoemp.SetCellValue(ficha.FichaVat.Empresa.Dpto);
                        var cellCpemp = row.CreateCell(44); cellCpemp.SetCellValue(ficha.FichaVat.Empresa.CodigoPostal);
                        var cellIdlocemp = row.CreateCell(45); cellIdlocemp.SetCellValue(ficha.FichaVat.Empresa.IdLocalidad == null ? String.Empty
                            : ficha.FichaVat.Empresa.LocalidadEmpresa.IdLocalidad.ToString());
                        var cellLocemp = row.CreateCell(46); cellLocemp.SetCellValue(ficha.FichaVat.Empresa.LocalidadEmpresa.NombreLocalidad);
                        var cellDepemp = row.CreateCell(47); cellDepemp.SetCellValue(ficha.FichaVat.Empresa.IdLocalidad == null ? String.Empty
                            : ficha.FichaVat.Empresa.LocalidadEmpresa.Departamento.NombreDepartamento);
                        var cellIdusuarioemp = row.CreateCell(48); cellIdusuarioemp.SetCellValue(ficha.FichaVat.Empresa.Usuario.IdUsuarioEmpresa == null ?
                            String.Empty : ficha.FichaVat.Empresa.Usuario.IdUsuarioEmpresa.ToString());
                        var cellNombreusuemp = row.CreateCell(49); cellNombreusuemp.SetCellValue(ficha.FichaVat.Empresa.Usuario.NombreUsuario);
                        var cellApellidousuemp = row.CreateCell(50); cellApellidousuemp.SetCellValue(ficha.FichaVat.Empresa.Usuario.ApellidoUsuario);
                        var cellEmailusuemp = row.CreateCell(51); cellEmailusuemp.SetCellValue(ficha.FichaVat.Empresa.Usuario.Mail);
                        var cellTeleusuemp = row.CreateCell(52); cellTeleusuemp.SetCellValue(ficha.FichaVat.Empresa.Usuario.Telefono);

                        //Beneficiario
                        var cellIdben = row.CreateCell(53); cellIdben.SetCellValue(ficha.BeneficiarioFicha.IdBeneficiarioExport == null ?
                            String.Empty : ficha.BeneficiarioFicha.IdBeneficiarioExport.ToString());
                        var cellIdestadoben = row.CreateCell(54); cellIdestadoben.SetCellValue(ficha.BeneficiarioFicha.IdBeneficiarioExport == null ?
                            String.Empty : ficha.BeneficiarioFicha.IdEstado.ToString());
                        var cellEstadoben = row.CreateCell(55); cellEstadoben.SetCellValue(ficha.BeneficiarioFicha.NombreEstado);
                        var cellNrocuentaben = row.CreateCell(56); cellNrocuentaben.SetCellValue(ficha.BeneficiarioFicha.NumeroCuenta == null ?
                            String.Empty : ficha.BeneficiarioFicha.NumeroCuenta.ToString());
                        var cellIdsucursalben = row.CreateCell(57); cellIdsucursalben.SetCellValue(ficha.BeneficiarioFicha.IdSucursal == null ?
                            String.Empty : ficha.BeneficiarioFicha.IdSucursal.ToString());
                        var cellCodsucursalben = row.CreateCell(58); cellCodsucursalben.SetCellValue(ficha.BeneficiarioFicha.CodigoSucursal);
                        var cellSucursalben = row.CreateCell(59); cellSucursalben.SetCellValue(ficha.BeneficiarioFicha.SucursalDescripcion);
                        var cellFecsolben = row.CreateCell(60); cellFecsolben.SetCellValue(ficha.BeneficiarioFicha.FechaSolicitudCuenta == null ?
                            String.Empty : ficha.BeneficiarioFicha.FechaSolicitudCuenta.Value.ToShortDateString());
                        var cellNotificado = row.CreateCell(61); cellNotificado.SetCellValue(ficha.BeneficiarioFicha.Notificado == false ? "NO" : "SI");
                        var cellFecNotif = row.CreateCell(62); cellFecNotif.SetCellValue(ficha.BeneficiarioFicha.FechaNotificacion == null ?
                            String.Empty : ficha.BeneficiarioFicha.FechaNotificacion.Value.ToShortDateString());
                        var cellTieneapoben = row.CreateCell(63); cellTieneapoben.SetCellValue(ficha.BeneficiarioFicha.TieneApoderado);

                        //Apoderado
                        var cellIdapoderado = row.CreateCell(64); cellIdapoderado.SetCellValue(ficha.BeneficiarioFicha.Apoderado.IdApoderadoNullable == null ?
                            String.Empty : ficha.BeneficiarioFicha.Apoderado.IdApoderadoNullable.ToString());
                        var cellApellidoapo = row.CreateCell(65); cellApellidoapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Apellido);
                        var cellNombreapo = row.CreateCell(66); cellNombreapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Nombre);
                        var cellDniapo = row.CreateCell(67); cellDniapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.NumeroDocumento);
                        var cellCuilapo = row.CreateCell(68); cellCuilapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Cuil);
                        var cellSexoapo = row.CreateCell(69); cellSexoapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Sexo);
                        var cellNroctaapo = row.CreateCell(70); cellNroctaapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.NumeroCuentaBco == null ?
                            String.Empty : ficha.BeneficiarioFicha.Apoderado.NumeroCuentaBco.ToString());
                        var cellIdSuc = row.CreateCell(71); cellIdSuc.SetCellValue(ficha.BeneficiarioFicha.Apoderado.IdSucursal == null ? String.Empty
                            : ficha.BeneficiarioFicha.Apoderado.SucursalApoderado.IdSucursal.ToString());
                        var cellCodsucapo = row.CreateCell(72); cellCodsucapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.SucursalApoderado.CodigoBanco);
                        var cellSucursalapo = row.CreateCell(73); cellSucursalapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.SucursalApoderado.Detalle);
                        var cellFecsolcuentaapo = row.CreateCell(74); cellFecsolcuentaapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.FechaSolicitudCuenta == null ?
                            String.Empty : ficha.BeneficiarioFicha.Apoderado.FechaSolicitudCuenta.Value.ToShortDateString());
                        var cellFecnac = row.CreateCell(75); cellFecnac.SetCellValue(ficha.BeneficiarioFicha.Apoderado.FechaNacimiento == null ? String.Empty :
                            ficha.BeneficiarioFicha.Apoderado.FechaNacimiento.Value.ToShortDateString());
                        var cellCalleapo = row.CreateCell(76); cellCalleapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Calle);
                        var cellNroapo = row.CreateCell(77); cellNroapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Numero);
                        var cellPisoapo = row.CreateCell(78); cellPisoapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Piso);
                        var cellMonoblockapo = row.CreateCell(79); cellMonoblockapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Monoblock);
                        var cellDptoapo = row.CreateCell(80); cellDptoapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Dpto);
                        var cellParcelaapo = row.CreateCell(81); cellParcelaapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Parcela);
                        var cellManzanaapo = row.CreateCell(82); cellManzanaapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Manzana);
                        var cellEntrecalleapo = row.CreateCell(83); cellEntrecalleapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.EntreCalles);
                        var cellBarrioapo = row.CreateCell(84); cellBarrioapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Barrio);
                        var cellCpapo = row.CreateCell(85); cellCpapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.CodigoPostal);
                        var cellIdlocalidadapo = row.CreateCell(86); cellIdlocalidadapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.IdLocalidad == null ?
                            String.Empty : ficha.BeneficiarioFicha.Apoderado.LocalidadApoderado.IdLocalidad.ToString());
                        var cellLocalidadapo = row.CreateCell(87); cellLocalidadapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.LocalidadApoderado.NombreLocalidad);
                        var cellIddeptoapo = row.CreateCell(88); cellIddeptoapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.IdLocalidad == null ?
                            String.Empty : ficha.BeneficiarioFicha.Apoderado.LocalidadApoderado.Departamento.IdDepartamento.ToString());
                        var cellDeptoapo = row.CreateCell(89); cellDeptoapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.LocalidadApoderado.Departamento.NombreDepartamento);
                        var cellTelefonoapo = row.CreateCell(90); cellTelefonoapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.TelefonoFijo);
                        var cellCelularapo = row.CreateCell(91); cellCelularapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.TelefonoCelular);
                        var cellEmailapo = row.CreateCell(92);
                        cellEmailapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Mail);
                    }
                    #endregion
                    break;
                case (int)Enums.TipoFicha.ReconversionProductiva:
                    #region case (int)Enums.TipoFicha.ReconversionProductiva
                    foreach (var ficha in listaparaExcel)
                    {
                        i++;
                        //Ficha
                        var row = sheet.CreateRow(i);
                        var cellidficha = row.CreateCell(0); cellidficha.SetCellValue(ficha.IdFicha);
                        var cellApellido = row.CreateCell(1); cellApellido.SetCellValue(ficha.Apellido);
                        var cellNombre = row.CreateCell(2); cellNombre.SetCellValue(ficha.Nombre);
                        var cellCuil = row.CreateCell(3); cellCuil.SetCellValue(ficha.Cuil);
                        var cellDni = row.CreateCell(4); cellDni.SetCellValue(ficha.NumeroDocumento);
                        var cellFechaNa = row.CreateCell(5); cellFechaNa.SetCellValue(ficha.FechaNacimiento.Value.ToShortDateString());
                        var cellCalle = row.CreateCell(6); cellCalle.SetCellValue(ficha.Calle);
                        var cellNro = row.CreateCell(7); cellNro.SetCellValue(ficha.Numero);
                        var cellPiso = row.CreateCell(8); cellPiso.SetCellValue(ficha.Piso);
                        var cellDpto = row.CreateCell(9); cellDpto.SetCellValue(ficha.Dpto);
                        var cellMono = row.CreateCell(10); cellMono.SetCellValue(ficha.Monoblock);
                        var cellParcela = row.CreateCell(11); cellParcela.SetCellValue(ficha.Parcela);
                        var cellManzana = row.CreateCell(12); cellManzana.SetCellValue(ficha.Manzana);
                        var cellEntreCalle = row.CreateCell(13); cellEntreCalle.SetCellValue(ficha.EntreCalles);
                        var cellBarrio = row.CreateCell(14); cellBarrio.SetCellValue(ficha.Barrio);
                        var cellCp = row.CreateCell(15); cellCp.SetCellValue(ficha.CodigoPostal);
                        var cellidLocalidad = row.CreateCell(16); cellidLocalidad.SetCellValue(ficha.LocalidadFicha.IdLocalidad);
                        var cellnLocalidad = row.CreateCell(17); cellnLocalidad.SetCellValue(ficha.LocalidadFicha.NombreLocalidad);
                        var cellIdDepartamento = row.CreateCell(18); cellIdDepartamento.SetCellValue(ficha.LocalidadFicha.Departamento.IdDepartamento);
                        var cellnDepartamento = row.CreateCell(19); cellnDepartamento.SetCellValue(ficha.LocalidadFicha.Departamento.NombreDepartamento);
                        var cellTelFijo = row.CreateCell(20); cellTelFijo.SetCellValue(ficha.TelefonoFijo);
                        var cellTelCelular = row.CreateCell(21); cellTelCelular.SetCellValue(ficha.TelefonoCelular);
                        var cellContacto = row.CreateCell(22); cellContacto.SetCellValue(ficha.Contacto);
                        var cellEmail = row.CreateCell(23); cellEmail.SetCellValue(ficha.Mail);
                        var cellSexo = row.CreateCell(24); cellSexo.SetCellValue(ficha.Sexo);
                        var cellidestadoFicha = row.CreateCell(25); cellidestadoFicha.SetCellValue(ficha.IdEstadoFicha == null ? String.Empty
                            : ficha.IdEstadoFicha.ToString());
                        var cellnestadoFicha = row.CreateCell(26); cellnestadoFicha.SetCellValue(ficha.NombreEstadoFicha);
                        var cellModalidad = row.CreateCell(27);
                        cellModalidad.SetCellValue(ficha.FichaReconversion.Modalidad == (int)Enums.Modalidad.Entrenamiento ?
                            "Entrenamineto" : "CTI");
                        var cellaltaTemprana = row.CreateCell(28); cellaltaTemprana.SetCellValue(ficha.FichaReconversion.AltaTemprana == "S" ? "SI" : "NO");
                        var cellatFechaInicio = row.CreateCell(29); cellatFechaInicio.SetCellValue(ficha.FichaReconversion.FechaInicioActividad == null ? String.Empty :
                            ficha.FichaReconversion.FechaInicioActividad.Value.ToShortDateString());
                        var cellatFechaCese = row.CreateCell(30); cellatFechaCese.SetCellValue(ficha.FichaReconversion.FechaFinActividad == null ? String.Empty :
                            ficha.FichaReconversion.FechaFinActividad.Value.ToShortDateString());
                        var cellidModContAfip = row.CreateCell(31); cellidModContAfip.SetCellValue(ficha.FichaReconversion.IdModalidadAfip == null ? String.Empty :
                            ficha.FichaReconversion.IdModalidadAfip.ToString());
                        var cellModContAfip = row.CreateCell(32); cellModContAfip.SetCellValue(ficha.FichaReconversion.ModalidadAfip);
                        var cellFechaAlta = row.CreateCell(33); cellFechaAlta.SetCellValue(ficha.FechaSistema.ToString() ?? "");
                        var cellnSubprograma = row.CreateCell(34); cellnSubprograma.SetCellValue(ficha.FichaReconversion.Subprograma);

                        //Proyecto
                        var cellnProyecto = row.CreateCell(35); cellnProyecto.SetCellValue(ficha.FichaReconversion.Proyecto.NombreProyecto);
                        var cellCantCapacitar = row.CreateCell(36); cellCantCapacitar.SetCellValue(ficha.FichaReconversion.Proyecto.CantidadAcapacitar == null ? String.Empty : ficha.FichaReconversion.Proyecto.CantidadAcapacitar.ToString());
                        var cellFecInicio = row.CreateCell(37); cellFecInicio.SetCellValue(ficha.FichaReconversion.Proyecto.FechaInicioProyecto == null ? String.Empty : ficha.FichaReconversion.Proyecto.FechaInicioProyecto.Value.ToShortDateString());
                        var cellFecFin = row.CreateCell(38); cellFecFin.SetCellValue(ficha.FichaReconversion.Proyecto.FechaFinProyecto == null ? String.Empty : ficha.FichaReconversion.Proyecto.FechaFinProyecto.Value.ToShortDateString());
                        var cellMesesDuracion = row.CreateCell(39); cellMesesDuracion.SetCellValue(ficha.FichaReconversion.Proyecto.MesesDuracionProyecto == null ? String.Empty : ficha.FichaReconversion.Proyecto.MesesDuracionProyecto.ToString());
                        var cellAportesEmpresa = row.CreateCell(40); cellAportesEmpresa.SetCellValue(ficha.FichaReconversion.AportesDeLaEmpresa);
                        var cellApeTutor = row.CreateCell(41); cellApeTutor.SetCellValue(ficha.FichaReconversion.ApellidoTutor);
                        var cellNomTutor = row.CreateCell(42); cellNomTutor.SetCellValue(ficha.FichaReconversion.NombreTutor);
                        var cellDocTutor = row.CreateCell(43); cellDocTutor.SetCellValue(ficha.FichaReconversion.NroDocumentoTutor);
                        var cellTelTutor = row.CreateCell(44); cellTelTutor.SetCellValue(ficha.FichaReconversion.TelefonoTutor);
                        var cellEmailTutor = row.CreateCell(45); cellEmailTutor.SetCellValue(ficha.FichaReconversion.EmailTutor);
                        var cellPuestoTutor = row.CreateCell(46); cellPuestoTutor.SetCellValue(ficha.FichaReconversion.PuestoTutor);

                        //Empresa
                        var cellidEmp = row.CreateCell(47); cellidEmp.SetCellValue(ficha.FichaReconversion.IdEmpresa == null ? String.Empty : ficha.FichaReconversion.IdEmpresa.ToString());
                        var cellrazonSocial = row.CreateCell(48); cellrazonSocial.SetCellValue(ficha.FichaReconversion.Empresa.NombreEmpresa);
                        var cellCuit = row.CreateCell(49); cellCuit.SetCellValue(ficha.FichaReconversion.Empresa.Cuit);
                        var cellCodact = row.CreateCell(50); cellCodact.SetCellValue(ficha.FichaReconversion.Empresa.CodigoActividad);
                        var cellCantemp = row.CreateCell(51); cellCantemp.SetCellValue(ficha.FichaReconversion.Empresa.CantidadEmpleados ?? 0);
                        var cellCalleemp = row.CreateCell(52); cellCalleemp.SetCellValue(ficha.FichaReconversion.Empresa.Calle);
                        var cellNumeroemp = row.CreateCell(53); cellNumeroemp.SetCellValue(ficha.FichaReconversion.Empresa.Numero);
                        var cellPisoemp = row.CreateCell(54); cellPisoemp.SetCellValue(ficha.FichaReconversion.Empresa.Piso);
                        var cellDptoemp = row.CreateCell(55); cellDptoemp.SetCellValue(ficha.FichaReconversion.Empresa.Dpto);
                        var cellCpemp = row.CreateCell(56); cellCpemp.SetCellValue(ficha.FichaReconversion.Empresa.CodigoPostal);
                        var cellIdlocemp = row.CreateCell(57); cellIdlocemp.SetCellValue(ficha.FichaReconversion.Empresa.LocalidadEmpresa.IdLocalidad);
                        var cellLocemp = row.CreateCell(58); cellLocemp.SetCellValue(ficha.FichaReconversion.Empresa.LocalidadEmpresa.NombreLocalidad);
                        var cellDepemp = row.CreateCell(59); cellDepemp.SetCellValue(ficha.FichaReconversion.Empresa.LocalidadEmpresa.Departamento.NombreDepartamento);
                        var cellIdusuarioemp = row.CreateCell(60); cellIdusuarioemp.SetCellValue(ficha.FichaReconversion.Empresa.Usuario.IdUsuarioEmpresa.ToString() ??
                            String.Empty);
                        var cellNombreusuemp = row.CreateCell(61); cellNombreusuemp.SetCellValue(ficha.FichaReconversion.Empresa.Usuario.NombreUsuario);
                        var cellApellidousuemp = row.CreateCell(62); cellApellidousuemp.SetCellValue(ficha.FichaReconversion.Empresa.Usuario.ApellidoUsuario);
                        var cellEmailusuemp = row.CreateCell(63); cellEmailusuemp.SetCellValue(ficha.FichaReconversion.Empresa.Usuario.Mail);
                        var cellTeleusuemp = row.CreateCell(64); cellTeleusuemp.SetCellValue(ficha.FichaReconversion.Empresa.Usuario.Telefono);

                        //Sede
                        var cellIdSede = row.CreateCell(65); cellIdSede.SetCellValue(ficha.FichaReconversion.IdSede == null ? String.Empty : ficha.FichaReconversion.IdSede.ToString());
                        var cellnSede = row.CreateCell(66); cellnSede.SetCellValue(ficha.FichaReconversion.Sede.NombreSede);
                        var cellCalleSede = row.CreateCell(67); cellCalleSede.SetCellValue(ficha.FichaReconversion.Sede.Calle);
                        var cellNroSede = row.CreateCell(68); cellNroSede.SetCellValue(ficha.FichaReconversion.Sede.Numero);
                        var cellPisoSede = row.CreateCell(69); cellPisoSede.SetCellValue(ficha.FichaReconversion.Sede.Piso);
                        var cellDptoSede = row.CreateCell(70); cellDptoSede.SetCellValue(ficha.FichaReconversion.Sede.Dpto);
                        var cellCpSede = row.CreateCell(71); cellCpSede.SetCellValue(ficha.FichaReconversion.Sede.CodigoPostal);
                        var cellLocSede = row.CreateCell(72); cellLocSede.SetCellValue(ficha.FichaReconversion.Sede.NombreLocalidad);
                        var cellApeContacto = row.CreateCell(73); cellApeContacto.SetCellValue(ficha.FichaReconversion.Sede.ApellidoContacto);
                        var cellNomContacto = row.CreateCell(74); cellNomContacto.SetCellValue(ficha.FichaReconversion.Sede.NombreContacto);
                        var cellTelcontacto = row.CreateCell(75); cellTelcontacto.SetCellValue(ficha.FichaReconversion.Sede.Telefono);
                        var cellFaxContacto = row.CreateCell(76); cellFaxContacto.SetCellValue(ficha.FichaReconversion.Sede.Fax);
                        var cellEmailContacto = row.CreateCell(77); cellEmailContacto.SetCellValue(ficha.FichaReconversion.Sede.Email);

                        //Beneficiario
                        var cellIdben = row.CreateCell(78); cellIdben.SetCellValue(ficha.BeneficiarioFicha.IdBeneficiarioExport.ToString() ?? String.Empty);
                        var cellIdestadoben = row.CreateCell(79); cellIdestadoben.SetCellValue(ficha.BeneficiarioFicha.IdEstado == 0 ? String.Empty :
                            ficha.BeneficiarioFicha.IdEstado.ToString());
                        var cellEstadoben = row.CreateCell(80); cellEstadoben.SetCellValue(ficha.BeneficiarioFicha.NombreEstado);
                        var cellNrocuentaben = row.CreateCell(81);
                        cellNrocuentaben.SetCellValue(ficha.BeneficiarioFicha.NumeroCuenta == null
                                                          ? String.Empty
                                                          : ficha.BeneficiarioFicha.NumeroCuenta.ToString());
                        var cellIdsucursalben = row.CreateCell(82); cellIdsucursalben.SetCellValue(ficha.BeneficiarioFicha.IdSucursal.ToString() ??
                            String.Empty);
                        var cellCodsucursalben = row.CreateCell(83); cellCodsucursalben.SetCellValue(ficha.BeneficiarioFicha.CodigoSucursal);
                        var cellSucursalben = row.CreateCell(84); cellSucursalben.SetCellValue(ficha.BeneficiarioFicha.SucursalDescripcion);
                        var cellFecsolben = row.CreateCell(85); cellFecsolben.SetCellValue(ficha.BeneficiarioFicha.FechaSolicitudCuenta == null ?
                            String.Empty : ficha.BeneficiarioFicha.FechaSolicitudCuenta.Value.ToShortDateString());
                        var cellNotificado = row.CreateCell(86); cellNotificado.SetCellValue(ficha.BeneficiarioFicha.Notificado == false ? "NO" : "SI");
                        var cellFecNotif = row.CreateCell(87); cellFecNotif.SetCellValue(ficha.BeneficiarioFicha.FechaNotificacion == null ? "" :
                            ficha.BeneficiarioFicha.FechaNotificacion.Value.ToShortDateString());
                        var cellTieneapoben = row.CreateCell(88); cellTieneapoben.SetCellValue(ficha.BeneficiarioFicha.TieneApoderado);

                        //Apoderado
                        var cellIdapoderado = row.CreateCell(89); cellIdapoderado.SetCellValue(ficha.BeneficiarioFicha.Apoderado.IdApoderadoNullable == null ?
                            String.Empty : ficha.BeneficiarioFicha.Apoderado.IdApoderadoNullable.ToString());
                        var cellApellidoapo = row.CreateCell(90); cellApellidoapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Apellido);
                        var cellNombreapo = row.CreateCell(91); cellNombreapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Nombre);
                        var cellDniapo = row.CreateCell(92); cellDniapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.NumeroDocumento);
                        var cellCuilapo = row.CreateCell(93); cellCuilapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Cuil);
                        var cellSexoapo = row.CreateCell(94); cellSexoapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Sexo);
                        var cellNroctaapo = row.CreateCell(95); cellNroctaapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.NumeroCuentaBco == null ?
                            String.Empty : ficha.BeneficiarioFicha.Apoderado.NumeroCuentaBco.ToString());
                        var cellIdSuc = row.CreateCell(96); cellIdSuc.SetCellValue(ficha.BeneficiarioFicha.Apoderado.IdSucursal == null ? String.Empty :
                            ficha.BeneficiarioFicha.Apoderado.IdSucursal.ToString());
                        var cellCodsucapo = row.CreateCell(97); cellCodsucapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.SucursalApoderado.CodigoBanco);
                        var cellSucursalapo = row.CreateCell(98); cellSucursalapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.SucursalApoderado.Detalle);
                        var cellFecsolcuentaapo = row.CreateCell(99); cellFecsolcuentaapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.FechaSolicitudCuenta == null ?
                            String.Empty : ficha.BeneficiarioFicha.Apoderado.FechaSolicitudCuenta.Value.ToShortDateString());
                        var cellFecnac = row.CreateCell(100); cellFecnac.SetCellValue(ficha.BeneficiarioFicha.Apoderado.FechaNacimiento == null ? String.Empty :
                            ficha.BeneficiarioFicha.Apoderado.FechaNacimiento.Value.ToShortDateString());
                        var cellCalleapo = row.CreateCell(101); cellCalleapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Calle);
                        var cellNroapo = row.CreateCell(102); cellNroapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Numero);
                        var cellPisoapo = row.CreateCell(103); cellPisoapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Piso);
                        var cellMonoblockapo = row.CreateCell(104); cellMonoblockapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Monoblock);
                        var cellDptoapo = row.CreateCell(105); cellDptoapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Dpto);
                        var cellParcelaapo = row.CreateCell(106); cellParcelaapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Parcela);
                        var cellManzanaapo = row.CreateCell(107); cellManzanaapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Manzana);
                        var cellEntrecalleapo = row.CreateCell(108); cellEntrecalleapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.EntreCalles);
                        var cellBarrioapo = row.CreateCell(109); cellBarrioapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Barrio);
                        var cellCpapo = row.CreateCell(110); cellCpapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.CodigoPostal);
                        var cellIdlocalidadapo = row.CreateCell(111); cellIdlocalidadapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.IdLocalidad == null ?
                            String.Empty : ficha.BeneficiarioFicha.Apoderado.LocalidadApoderado.IdLocalidad.ToString());
                        var cellLocalidadapo = row.CreateCell(112); cellLocalidadapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.LocalidadApoderado.NombreLocalidad);
                        var cellIddeptoapo = row.CreateCell(113); cellIddeptoapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.IdLocalidad == null ? String.Empty :
                            ficha.BeneficiarioFicha.Apoderado.LocalidadApoderado.Departamento.IdDepartamento.ToString());
                        var cellDeptoapo = row.CreateCell(114); cellDeptoapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.LocalidadApoderado.Departamento.NombreDepartamento);
                        var cellTelefonoapo = row.CreateCell(115); cellTelefonoapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.TelefonoFijo);
                        var cellCelularapo = row.CreateCell(116); cellCelularapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.TelefonoCelular);
                        var cellEmailapo = row.CreateCell(117);
                        cellEmailapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Mail);
                    }
                    #endregion
                    break;
                case (int)Enums.TipoFicha.EfectoresSociales:
                    #region case (int)Enums.TipoFicha.EfectoresSociales
                    foreach (var ficha in listaparaExcel)
                    {
                        i++;
                        //Ficha
                        var row = sheet.CreateRow(i);
                        var cellidficha = row.CreateCell(0); cellidficha.SetCellValue(ficha.IdFicha);
                        var cellApellido = row.CreateCell(1); cellApellido.SetCellValue(ficha.Apellido);
                        var cellNombre = row.CreateCell(2); cellNombre.SetCellValue(ficha.Nombre);
                        var cellCuil = row.CreateCell(3); cellCuil.SetCellValue(ficha.Cuil);
                        var cellDni = row.CreateCell(4); cellDni.SetCellValue(ficha.NumeroDocumento);
                        var cellFechaNa = row.CreateCell(5); cellFechaNa.SetCellValue(ficha.FechaNacimiento.Value.ToShortDateString());
                        var cellCalle = row.CreateCell(6); cellCalle.SetCellValue(ficha.Calle);
                        var cellNro = row.CreateCell(7); cellNro.SetCellValue(ficha.Numero);
                        var cellPiso = row.CreateCell(8); cellPiso.SetCellValue(ficha.Piso);
                        var cellDpto = row.CreateCell(9); cellDpto.SetCellValue(ficha.Dpto);
                        var cellMono = row.CreateCell(10); cellMono.SetCellValue(ficha.Monoblock);
                        var cellParcela = row.CreateCell(11); cellParcela.SetCellValue(ficha.Parcela);
                        var cellManzana = row.CreateCell(12); cellManzana.SetCellValue(ficha.Manzana);
                        var cellEntreCalle = row.CreateCell(13); cellEntreCalle.SetCellValue(ficha.EntreCalles);
                        var cellBarrio = row.CreateCell(14); cellBarrio.SetCellValue(ficha.Barrio);
                        var cellCp = row.CreateCell(15); cellCp.SetCellValue(ficha.CodigoPostal);
                        var cellidLocalidad = row.CreateCell(16); cellidLocalidad.SetCellValue(ficha.LocalidadFicha.IdLocalidad);
                        var cellnLocalidad = row.CreateCell(17); cellnLocalidad.SetCellValue(ficha.LocalidadFicha.NombreLocalidad);
                        var cellIdDepartamento = row.CreateCell(18); cellIdDepartamento.SetCellValue(ficha.LocalidadFicha.Departamento.IdDepartamento);
                        var cellnDepartamento = row.CreateCell(19); cellnDepartamento.SetCellValue(ficha.LocalidadFicha.Departamento.NombreDepartamento);
                        var cellTelFijo = row.CreateCell(20); cellTelFijo.SetCellValue(ficha.TelefonoFijo);
                        var cellTelCelular = row.CreateCell(21); cellTelCelular.SetCellValue(ficha.TelefonoCelular);
                        var cellContacto = row.CreateCell(22); cellContacto.SetCellValue(ficha.Contacto);
                        var cellEmail = row.CreateCell(23); cellEmail.SetCellValue(ficha.Mail);
                        var cellSexo = row.CreateCell(24); cellSexo.SetCellValue(ficha.Sexo);
                        var cellidestadoFicha = row.CreateCell(25); cellidestadoFicha.SetCellValue(ficha.IdEstadoFicha == null ? String.Empty
                            : ficha.IdEstadoFicha.ToString());
                        var cellnestadoFicha = row.CreateCell(26); cellnestadoFicha.SetCellValue(ficha.NombreEstadoFicha);
                        var cellModalidad = row.CreateCell(27);
                        cellModalidad.SetCellValue(ficha.FichaEfectores.Modalidad == (int)Enums.Modalidad.Entrenamiento ?
                            "Entrenamineto" : "CTI");
                        var cellaltaTemprana = row.CreateCell(28); cellaltaTemprana.SetCellValue(ficha.FichaEfectores.AltaTemprana == "S" ? "SI" : "NO");
                        var cellatFechaInicio = row.CreateCell(29); cellatFechaInicio.SetCellValue(ficha.FichaEfectores.FechaInicioActividad == null ? String.Empty :
                            ficha.FichaEfectores.FechaInicioActividad.Value.ToShortDateString());
                        var cellatFechaCese = row.CreateCell(30); cellatFechaCese.SetCellValue(ficha.FichaEfectores.FechaFinActividad == null ? String.Empty :
                            ficha.FichaEfectores.FechaFinActividad.Value.ToShortDateString());
                        var cellidModContAfip = row.CreateCell(31); cellidModContAfip.SetCellValue(ficha.FichaEfectores.IdModalidadAfip == null ? String.Empty :
                            ficha.FichaEfectores.IdModalidadAfip.ToString());
                        var cellModContAfip = row.CreateCell(32); cellModContAfip.SetCellValue(ficha.FichaEfectores.ModalidadAfip);
                        var cellFechaAlta = row.CreateCell(33); cellFechaAlta.SetCellValue(ficha.FechaSistema.ToString() ?? "");
                        var cellnSubprograma = row.CreateCell(34); cellnSubprograma.SetCellValue(ficha.FichaEfectores.Subprograma);

                        //Proyecto
                        var cellnProyecto = row.CreateCell(35); cellnProyecto.SetCellValue(ficha.FichaEfectores.Proyecto.NombreProyecto);
                        var cellCantCapacitar = row.CreateCell(36); cellCantCapacitar.SetCellValue(ficha.FichaEfectores.Proyecto.CantidadAcapacitar == null ? String.Empty : ficha.FichaEfectores.Proyecto.CantidadAcapacitar.ToString());
                        var cellFecInicio = row.CreateCell(37); cellFecInicio.SetCellValue(ficha.FichaEfectores.Proyecto.FechaInicioProyecto == null ? String.Empty : ficha.FichaEfectores.Proyecto.FechaInicioProyecto.Value.ToShortDateString());
                        var cellFecFin = row.CreateCell(38); cellFecFin.SetCellValue(ficha.FichaEfectores.Proyecto.FechaFinProyecto == null ? String.Empty : ficha.FichaEfectores.Proyecto.FechaFinProyecto.Value.ToShortDateString());
                        var cellMesesDuracion = row.CreateCell(39); cellMesesDuracion.SetCellValue(ficha.FichaEfectores.Proyecto.MesesDuracionProyecto == null ? String.Empty : ficha.FichaEfectores.Proyecto.MesesDuracionProyecto.ToString());
                        var cellAportesEmpresa = row.CreateCell(40); cellAportesEmpresa.SetCellValue(ficha.FichaEfectores.AportesDeLaEmpresa);
                        var cellApeTutor = row.CreateCell(41); cellApeTutor.SetCellValue(ficha.FichaEfectores.ApellidoTutor);
                        var cellNomTutor = row.CreateCell(42); cellNomTutor.SetCellValue(ficha.FichaEfectores.NombreTutor);
                        var cellDocTutor = row.CreateCell(43); cellDocTutor.SetCellValue(ficha.FichaEfectores.NroDocumentoTutor);
                        var cellTelTutor = row.CreateCell(44); cellTelTutor.SetCellValue(ficha.FichaEfectores.TelefonoTutor);
                        var cellEmailTutor = row.CreateCell(45); cellEmailTutor.SetCellValue(ficha.FichaEfectores.EmailTutor);
                        var cellPuestoTutor = row.CreateCell(46); cellPuestoTutor.SetCellValue(ficha.FichaEfectores.PuestoTutor);

                        //Empresa
                        var cellidEmp = row.CreateCell(47); cellidEmp.SetCellValue(ficha.FichaEfectores.IdEmpresa == null ? String.Empty : ficha.FichaEfectores.IdEmpresa.ToString());
                        var cellrazonSocial = row.CreateCell(48); cellrazonSocial.SetCellValue(ficha.FichaEfectores.Empresa.NombreEmpresa);
                        var cellCuit = row.CreateCell(49); cellCuit.SetCellValue(ficha.FichaEfectores.Empresa.Cuit);
                        var cellCodact = row.CreateCell(50); cellCodact.SetCellValue(ficha.FichaEfectores.Empresa.CodigoActividad);
                        var cellCantemp = row.CreateCell(51); cellCantemp.SetCellValue(ficha.FichaEfectores.Empresa.CantidadEmpleados ?? 0);
                        var cellCalleemp = row.CreateCell(52); cellCalleemp.SetCellValue(ficha.FichaEfectores.Empresa.Calle);
                        var cellNumeroemp = row.CreateCell(53); cellNumeroemp.SetCellValue(ficha.FichaEfectores.Empresa.Numero);
                        var cellPisoemp = row.CreateCell(54); cellPisoemp.SetCellValue(ficha.FichaEfectores.Empresa.Piso);
                        var cellDptoemp = row.CreateCell(55); cellDptoemp.SetCellValue(ficha.FichaEfectores.Empresa.Dpto);
                        var cellCpemp = row.CreateCell(56); cellCpemp.SetCellValue(ficha.FichaEfectores.Empresa.CodigoPostal);
                        var cellIdlocemp = row.CreateCell(57); cellIdlocemp.SetCellValue(ficha.FichaEfectores.Empresa.LocalidadEmpresa.IdLocalidad);
                        var cellLocemp = row.CreateCell(58); cellLocemp.SetCellValue(ficha.FichaEfectores.Empresa.LocalidadEmpresa.NombreLocalidad);
                        var cellDepemp = row.CreateCell(59); cellDepemp.SetCellValue(ficha.FichaEfectores.Empresa.LocalidadEmpresa.Departamento.NombreDepartamento);
                        var cellIdusuarioemp = row.CreateCell(60); cellIdusuarioemp.SetCellValue(ficha.FichaEfectores.Empresa.Usuario.IdUsuarioEmpresa.ToString() ??
                            String.Empty);
                        var cellNombreusuemp = row.CreateCell(61); cellNombreusuemp.SetCellValue(ficha.FichaEfectores.Empresa.Usuario.NombreUsuario);
                        var cellApellidousuemp = row.CreateCell(62); cellApellidousuemp.SetCellValue(ficha.FichaEfectores.Empresa.Usuario.ApellidoUsuario);
                        var cellEmailusuemp = row.CreateCell(63); cellEmailusuemp.SetCellValue(ficha.FichaEfectores.Empresa.Usuario.Mail);
                        var cellTeleusuemp = row.CreateCell(64); cellTeleusuemp.SetCellValue(ficha.FichaEfectores.Empresa.Usuario.Telefono);

                        //Sede
                        var cellIdSede = row.CreateCell(65); cellIdSede.SetCellValue(ficha.FichaEfectores.IdSede == null ? String.Empty : ficha.FichaEfectores.IdSede.ToString());
                        var cellnSede = row.CreateCell(66); cellnSede.SetCellValue(ficha.FichaEfectores.Sede.NombreSede);
                        var cellCalleSede = row.CreateCell(67); cellCalleSede.SetCellValue(ficha.FichaEfectores.Sede.Calle);
                        var cellNroSede = row.CreateCell(68); cellNroSede.SetCellValue(ficha.FichaEfectores.Sede.Numero);
                        var cellPisoSede = row.CreateCell(69); cellPisoSede.SetCellValue(ficha.FichaEfectores.Sede.Piso);
                        var cellDptoSede = row.CreateCell(70); cellDptoSede.SetCellValue(ficha.FichaEfectores.Sede.Dpto);
                        var cellCpSede = row.CreateCell(71); cellCpSede.SetCellValue(ficha.FichaEfectores.Sede.CodigoPostal);
                        var cellLocSede = row.CreateCell(72); cellLocSede.SetCellValue(ficha.FichaEfectores.Sede.NombreLocalidad);
                        var cellApeContacto = row.CreateCell(73); cellApeContacto.SetCellValue(ficha.FichaEfectores.Sede.ApellidoContacto);
                        var cellNomContacto = row.CreateCell(74); cellNomContacto.SetCellValue(ficha.FichaEfectores.Sede.NombreContacto);
                        var cellTelcontacto = row.CreateCell(75); cellTelcontacto.SetCellValue(ficha.FichaEfectores.Sede.Telefono);
                        var cellFaxContacto = row.CreateCell(76); cellFaxContacto.SetCellValue(ficha.FichaEfectores.Sede.Fax);
                        var cellEmailContacto = row.CreateCell(77); cellEmailContacto.SetCellValue(ficha.FichaEfectores.Sede.Email);

                        //Beneficiario
                        var cellIdben = row.CreateCell(78); cellIdben.SetCellValue(ficha.BeneficiarioFicha.IdBeneficiarioExport.ToString() ?? String.Empty);
                        var cellIdestadoben = row.CreateCell(79); cellIdestadoben.SetCellValue(ficha.BeneficiarioFicha.IdEstado == 0 ? String.Empty :
                            ficha.BeneficiarioFicha.IdEstado.ToString());
                        var cellEstadoben = row.CreateCell(80); cellEstadoben.SetCellValue(ficha.BeneficiarioFicha.NombreEstado);
                        var cellNrocuentaben = row.CreateCell(81);
                        cellNrocuentaben.SetCellValue(ficha.BeneficiarioFicha.NumeroCuenta == null
                                                          ? String.Empty
                                                          : ficha.BeneficiarioFicha.NumeroCuenta.ToString());
                        var cellIdsucursalben = row.CreateCell(82); cellIdsucursalben.SetCellValue(ficha.BeneficiarioFicha.IdSucursal.ToString() ??
                            String.Empty);
                        var cellCodsucursalben = row.CreateCell(83); cellCodsucursalben.SetCellValue(ficha.BeneficiarioFicha.CodigoSucursal);
                        var cellSucursalben = row.CreateCell(84); cellSucursalben.SetCellValue(ficha.BeneficiarioFicha.SucursalDescripcion);
                        var cellFecsolben = row.CreateCell(85); cellFecsolben.SetCellValue(ficha.BeneficiarioFicha.FechaSolicitudCuenta == null ?
                            String.Empty : ficha.BeneficiarioFicha.FechaSolicitudCuenta.Value.ToShortDateString());
                        var cellNotificado = row.CreateCell(86); cellNotificado.SetCellValue(ficha.BeneficiarioFicha.Notificado == false ? "NO" : "SI");
                        var cellFecNotif = row.CreateCell(87); cellFecNotif.SetCellValue(ficha.BeneficiarioFicha.FechaNotificacion == null ? "" :
                            ficha.BeneficiarioFicha.FechaNotificacion.Value.ToShortDateString());
                        var cellTieneapoben = row.CreateCell(88); cellTieneapoben.SetCellValue(ficha.BeneficiarioFicha.TieneApoderado);

                        //Apoderado
                        var cellIdapoderado = row.CreateCell(89); cellIdapoderado.SetCellValue(ficha.BeneficiarioFicha.Apoderado.IdApoderadoNullable == null ?
                            String.Empty : ficha.BeneficiarioFicha.Apoderado.IdApoderadoNullable.ToString());
                        var cellApellidoapo = row.CreateCell(90); cellApellidoapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Apellido);
                        var cellNombreapo = row.CreateCell(91); cellNombreapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Nombre);
                        var cellDniapo = row.CreateCell(92); cellDniapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.NumeroDocumento);
                        var cellCuilapo = row.CreateCell(93); cellCuilapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Cuil);
                        var cellSexoapo = row.CreateCell(94); cellSexoapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Sexo);
                        var cellNroctaapo = row.CreateCell(95); cellNroctaapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.NumeroCuentaBco == null ?
                            String.Empty : ficha.BeneficiarioFicha.Apoderado.NumeroCuentaBco.ToString());
                        var cellIdSuc = row.CreateCell(96); cellIdSuc.SetCellValue(ficha.BeneficiarioFicha.Apoderado.IdSucursal == null ? String.Empty :
                            ficha.BeneficiarioFicha.Apoderado.IdSucursal.ToString());
                        var cellCodsucapo = row.CreateCell(97); cellCodsucapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.SucursalApoderado.CodigoBanco);
                        var cellSucursalapo = row.CreateCell(98); cellSucursalapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.SucursalApoderado.Detalle);
                        var cellFecsolcuentaapo = row.CreateCell(99); cellFecsolcuentaapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.FechaSolicitudCuenta == null ?
                            String.Empty : ficha.BeneficiarioFicha.Apoderado.FechaSolicitudCuenta.Value.ToShortDateString());
                        var cellFecnac = row.CreateCell(100); cellFecnac.SetCellValue(ficha.BeneficiarioFicha.Apoderado.FechaNacimiento == null ? String.Empty :
                            ficha.BeneficiarioFicha.Apoderado.FechaNacimiento.Value.ToShortDateString());
                        var cellCalleapo = row.CreateCell(101); cellCalleapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Calle);
                        var cellNroapo = row.CreateCell(102); cellNroapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Numero);
                        var cellPisoapo = row.CreateCell(103); cellPisoapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Piso);
                        var cellMonoblockapo = row.CreateCell(104); cellMonoblockapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Monoblock);
                        var cellDptoapo = row.CreateCell(105); cellDptoapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Dpto);
                        var cellParcelaapo = row.CreateCell(106); cellParcelaapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Parcela);
                        var cellManzanaapo = row.CreateCell(107); cellManzanaapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Manzana);
                        var cellEntrecalleapo = row.CreateCell(108); cellEntrecalleapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.EntreCalles);
                        var cellBarrioapo = row.CreateCell(109); cellBarrioapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Barrio);
                        var cellCpapo = row.CreateCell(110); cellCpapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.CodigoPostal);
                        var cellIdlocalidadapo = row.CreateCell(111); cellIdlocalidadapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.IdLocalidad == null ?
                            String.Empty : ficha.BeneficiarioFicha.Apoderado.LocalidadApoderado.IdLocalidad.ToString());
                        var cellLocalidadapo = row.CreateCell(112); cellLocalidadapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.LocalidadApoderado.NombreLocalidad);
                        var cellIddeptoapo = row.CreateCell(113); cellIddeptoapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.IdLocalidad == null ? String.Empty :
                            ficha.BeneficiarioFicha.Apoderado.LocalidadApoderado.Departamento.IdDepartamento.ToString());
                        var cellDeptoapo = row.CreateCell(114); cellDeptoapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.LocalidadApoderado.Departamento.NombreDepartamento);
                        var cellTelefonoapo = row.CreateCell(115); cellTelefonoapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.TelefonoFijo);
                        var cellCelularapo = row.CreateCell(116); cellCelularapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.TelefonoCelular);
                        var cellEmailapo = row.CreateCell(117);
                        cellEmailapo.SetCellValue(ficha.BeneficiarioFicha.Apoderado.Mail);
                    }
                    #endregion
                    break;
            }

            var ms = new MemoryStream();

            templateWorkbook.Write(ms);

            return ms.ToArray();
        }

        public byte[] ExportarFichasPPP(string cuil, string dni, string nombre, string apellido, int tipoficha, int estadoficha, int idEtapa, int TipoPpp, int idSubprograma, string nrotramite)
        {
            var path = String.Empty;


                    path = HttpContext.Current.Server.MapPath(@"\Archivos\Exportados\TemplateBeneficiariosPPP.xls");

            var fs = new FileStream(path, FileMode.Open, FileAccess.Read);

            var templateWorkbook = new HSSFWorkbook(fs, true);

            var sheet = templateWorkbook.GetSheet("Nomina");

            List<VT_REPORTES_PPP> listaparaExcel = null;

            listaparaExcel = _FichaRepo.getfichaPPP(cuil, dni, nombre, apellido, tipoficha, estadoficha, idEtapa, TipoPpp, idSubprograma, nrotramite);

            var i = 1;

            var tiposppp = _tipopppRepositorio.GetTiposPpp();
            

            switch (tipoficha)
            {

                case (int)Enums.TipoFicha.Ppp:
                    #region case (int)Enums.TipoFicha.Ppp
                    foreach (var beneficiario in listaparaExcel)
                    {
                        i++;
                        //Ficha
                        var row = sheet.CreateRow(i);
                        var cellidficha = row.CreateCell(0); cellidficha.SetCellValue(beneficiario.ID_FICHA);
                        var cellApellido = row.CreateCell(1); cellApellido.SetCellValue(beneficiario.APELLIDO);
                        var cellNombre = row.CreateCell(2); cellNombre.SetCellValue(beneficiario.NOMBRE);
                        var cellCuil = row.CreateCell(3); cellCuil.SetCellValue(beneficiario.CUIL);
                        var cellDni = row.CreateCell(4); cellDni.SetCellValue(beneficiario.NUMERO_DOCUMENTO);
                        var cellFechaNa = row.CreateCell(5); cellFechaNa.SetCellValue(beneficiario.FER_NAC == null ? String.Empty
                            : beneficiario.FER_NAC.ToShortDateString());
                        var cellCalle = row.CreateCell(6); cellCalle.SetCellValue(beneficiario.CALLE);
                        var cellNro = row.CreateCell(7); cellNro.SetCellValue(beneficiario.NUMERO);
                        var cellPiso = row.CreateCell(8); cellPiso.SetCellValue(beneficiario.PISO);
                        var cellDpto = row.CreateCell(9); cellDpto.SetCellValue(beneficiario.DPTO);
                        var cellMono = row.CreateCell(10); cellMono.SetCellValue(beneficiario.MONOBLOCK);
                        var cellParcela = row.CreateCell(11); cellParcela.SetCellValue(beneficiario.PARCELA);
                        var cellManzana = row.CreateCell(12); cellManzana.SetCellValue(beneficiario.MANZANA);
                        var cellEntreCalle = row.CreateCell(13); cellEntreCalle.SetCellValue(beneficiario.ENTRECALLES);
                        var cellBarrio = row.CreateCell(14); cellBarrio.SetCellValue(beneficiario.BARRIO);
                        var cellCp = row.CreateCell(15); cellCp.SetCellValue(beneficiario.CODIGO_POSTAL);
                        var cellidLocalidad = row.CreateCell(16); cellidLocalidad.SetCellValue(beneficiario.ID_LOCALIDAD == null ?
                            String.Empty : beneficiario.ID_LOCALIDAD.ToString());
                        var cellnLocalidad = row.CreateCell(17); cellnLocalidad.SetCellValue(beneficiario.N_LOCALIDAD);
                        var cellIdDepartamento = row.CreateCell(18); cellIdDepartamento.SetCellValue(beneficiario.ID_DPTO == null ?
                            String.Empty : beneficiario.ID_DPTO.ToString());
                        var cellnDepartamento = row.CreateCell(19); cellnDepartamento.SetCellValue(beneficiario.N_DEPARTAMENTO);
                        var cellTelFijo = row.CreateCell(20); cellTelFijo.SetCellValue(beneficiario.TEL_FIJO);
                        var cellTelCelular = row.CreateCell(21); cellTelCelular.SetCellValue(beneficiario.TEL_CELULAR);
                        var cellContacto = row.CreateCell(22); cellContacto.SetCellValue(beneficiario.CONTACTO);
                        var cellEmail = row.CreateCell(23); cellEmail.SetCellValue(beneficiario.MAIL);

                        var cellTieneHijos = row.CreateCell(24); cellTieneHijos.SetCellValue(beneficiario.TIENE_HIJOS);//TIENE_HIJOS	
                        var cellCantidadHijos = row.CreateCell(25); cellCantidadHijos.SetCellValue(beneficiario.CANTIDAD_HIJOS == null ? "0" : beneficiario.CANTIDAD_HIJOS.ToString());//CANTIDAD_HIJOS
                        var cellEsDiscapacitado = row.CreateCell(26); cellEsDiscapacitado.SetCellValue(beneficiario.ES_DISCAPACITADO == null ? "N" : beneficiario.ES_DISCAPACITADO);//ES_DIACAPACITADO
                        var cellTieneDeficienciaMotora = row.CreateCell(27); cellTieneDeficienciaMotora.SetCellValue(beneficiario.TIENE_DEF_MOTORA == null ? "N" : beneficiario.TIENE_DEF_MOTORA);//TIENE_DEF_MOTORA
                        var cellTieneDeficienciaMental = row.CreateCell(28); cellTieneDeficienciaMental.SetCellValue(beneficiario.TIENE_DEF_MENTAL == null ? "N" : beneficiario.TIENE_DEF_MENTAL);//TIENE_DEF_MENTAL
                        var cellTieneDeficienciaSensorial = row.CreateCell(29); cellTieneDeficienciaSensorial.SetCellValue(beneficiario.TIENE_DEF_SENSORIAL == null ? "N" : beneficiario.TIENE_DEF_SENSORIAL);//TIENE_DEF_SENSORIAL
                        var cellTieneDeficienciaPsicologia = row.CreateCell(30); cellTieneDeficienciaPsicologia.SetCellValue(beneficiario.TIENE_DEF_PSICOLOGICA == null ? "N" : beneficiario.TIENE_DEF_PSICOLOGICA);//TIENE_DEF_PSICOLOGICA
                        var cellDeficienciaOtra = row.CreateCell(31); cellDeficienciaOtra.SetCellValue(beneficiario.DEFICIENCIA_OTRA == null ? "N" : beneficiario.TIENE_DEF_PSICOLOGICA);//DEFICIENCIA_OTRA
                        var cellCertificadoDiscapacidad = row.CreateCell(32); cellCertificadoDiscapacidad.SetCellValue(beneficiario.CERTIF_DISCAP == null ? "N" : beneficiario.CERTIF_DISCAP);//CERTIF_DISCAP



                        var cellSexo = row.CreateCell(33); cellSexo.SetCellValue(beneficiario.SEXO);

                        var cellTipoFicha = row.CreateCell(34); cellTipoFicha.SetCellValue(beneficiario.TIPO_FICHA.ToString() ?? "0");//TIPO_FICHA
                        var cellEstadoCivilDesc = row.CreateCell(35); cellEstadoCivilDesc.SetCellValue(beneficiario.ESTADO_CIVIL);//ESTADO_CIVIL
                        var cellFechaSistema = row.CreateCell(36); cellFechaSistema.SetCellValue(beneficiario.FEC_SIST == null ? String.Empty : beneficiario.FEC_SIST.Value.ToString());//FECH_SIST
                        var cellOrigenCarga = row.CreateCell(37); cellOrigenCarga.SetCellValue(beneficiario.ORIGEN_CARGA == "M" ? "Cargappp" : "Web");//ORIGEN_CARGA
                        var cellNroTramite = row.CreateCell(38); cellNroTramite.SetCellValue(beneficiario.NRO_TRAMITE ?? "");//NRO_TRAMITE
                        var cellidCaja = row.CreateCell(39); cellidCaja.SetCellValue(beneficiario.ID_CAJA == null ? "" : beneficiario.ID_CAJA.ToString());//ID_CAJA,
                        var cellIdTipoRechazo = row.CreateCell(40); cellIdTipoRechazo.SetCellValue(beneficiario.ID_TIPO_RECHAZO == null ? "" : beneficiario.ID_TIPO_RECHAZO.ToString());//ID_TIPO_RECHAZO,
                        var cellrechazo = row.CreateCell(41); cellrechazo.SetCellValue(beneficiario.N_TIPO_RECHAZO ?? "");//.N_TIPO_RECHAZO,

                        var cellidestadoFicha = row.CreateCell(42); cellidestadoFicha.SetCellValue(beneficiario.ID_EST_FIC == null ? String.Empty :
                            beneficiario.ID_EST_FIC.ToString());
                        var cellnestadoFicha = row.CreateCell(43); cellnestadoFicha.SetCellValue(beneficiario.N_ESTADO_FICHA);

                        var cellNivelEscolaridad = row.CreateCell(44);
                        if (beneficiario.TIPO_PPP == 5)
                        {
                            if (beneficiario.NIVEL_ESCOLARIDAD == "TERCIARIO")
                            { cellNivelEscolaridad.SetCellValue("NIVEL SUPERIOR"); }
                            else { cellNivelEscolaridad.SetCellValue(beneficiario.NIVEL_ESCOLARIDAD); }

                        }
                        else
                        {
                            cellNivelEscolaridad.SetCellValue(beneficiario.NIVEL_ESCOLARIDAD);
                        }
                        var cellModalidad = row.CreateCell(45); cellModalidad.SetCellValue(beneficiario.MODALIDAD == (int)Enums.Modalidad.Entrenamiento
                            ? "Entrenamiento" : "CTI");
                        var cellTarea = row.CreateCell(46); cellTarea.SetCellValue(beneficiario.TAREAS);
                        var cellaltaTemprana = row.CreateCell(47); cellaltaTemprana.SetCellValue(beneficiario.ALTA_TEMPRANA);
                        var cellatFechaInicio = row.CreateCell(48); cellatFechaInicio.SetCellValue(beneficiario.AT_FECHA_INICIO == null ?
                            String.Empty : beneficiario.AT_FECHA_INICIO.Value.ToShortDateString());
                        var cellatFechaCese = row.CreateCell(49); cellatFechaCese.SetCellValue(beneficiario.AT_FECHA_CESE == null ?
                            String.Empty : beneficiario.AT_FECHA_CESE.Value.ToShortDateString());
                        var cellidModContAfip = row.CreateCell(50); cellidModContAfip.SetCellValue(beneficiario.ID_MOD_CONT_AFIP == null ?
                            String.Empty : beneficiario.ID_MOD_CONT_AFIP.ToString());
                        var cellModContAfip = row.CreateCell(51); cellModContAfip.SetCellValue(beneficiario.MOD_CONT_AFIP);

                        var cellFechaModif = row.CreateCell(52); cellFechaModif.SetCellValue(beneficiario.FEC_MODIF == null ? String.Empty
                            : beneficiario.FEC_MODIF.Value.ToShortDateString());

                        ////Empresa
                        var cellidEmp = row.CreateCell(53); cellidEmp.SetCellValue(beneficiario.ID_EMP == null ? String.Empty : beneficiario.ID_EMP.Value.ToString());
                        var cellrazonSocial = row.CreateCell(54); cellrazonSocial.SetCellValue(beneficiario.RAZON_SOCIAL);
                        var cellCuit = row.CreateCell(55); cellCuit.SetCellValue(beneficiario.EMP_CUIT);
                        var cellCodact = row.CreateCell(56); cellCodact.SetCellValue(beneficiario.COD_ACT);
                        var cellCantemp = row.CreateCell(57); cellCantemp.SetCellValue(beneficiario.CANT_EMP == null ?
                            String.Empty : beneficiario.CANT_EMP.ToString());
                        var cellCalleemp = row.CreateCell(58); cellCalleemp.SetCellValue(beneficiario.EMP_CALLE);
                        var cellNumeroemp = row.CreateCell(59); cellNumeroemp.SetCellValue(beneficiario.EMP_NUMERO);
                        var cellPisoemp = row.CreateCell(60); cellPisoemp.SetCellValue(beneficiario.EMP_PISO);
                        var cellDptoemp = row.CreateCell(61); cellDptoemp.SetCellValue(beneficiario.EMP_DPTO);
                        var cellCpemp = row.CreateCell(62); cellCpemp.SetCellValue(beneficiario.EMP_CP);
                        var cellIdlocemp = row.CreateCell(63); cellIdlocemp.SetCellValue(beneficiario.ID_LOC == null ? String.Empty : beneficiario.ID_LOC.ToString());
                        var cellLocemp = row.CreateCell(64); cellLocemp.SetCellValue(beneficiario.EMP_N_LOCALIDAD);
                        var cellDepartamentoEmp = row.CreateCell(65); cellDepartamentoEmp.SetCellValue(beneficiario.ID_LOC == null ? String.Empty : beneficiario.EMP_N_DEPARTAMENTO);

                        var cellCelularEmp = row.CreateCell(66); cellCelularEmp.SetCellValue(beneficiario.EMP_CELULAR);
                        var cellMailEmp = row.CreateCell(67); cellMailEmp.SetCellValue(beneficiario.EMP_MAIL);
                        var cellEsCoopEmp = row.CreateCell(68); cellEsCoopEmp.SetCellValue(beneficiario.EMP_ES_COOPERATIVA ?? "N");

                        var cellIdusuarioemp = row.CreateCell(69); cellIdusuarioemp.SetCellValue(beneficiario.EU_ID_USUARIO == null ? String.Empty : beneficiario.EU_ID_USUARIO.ToString());
                        var cellNombreusuemp = row.CreateCell(70); cellNombreusuemp.SetCellValue(beneficiario.EU_NOMBRE);
                        var cellApellidousuemp = row.CreateCell(71); cellApellidousuemp.SetCellValue(beneficiario.EMP_APELLIDO);
                        var cellEmailusuemp = row.CreateCell(72); cellEmailusuemp.SetCellValue(beneficiario.EU_MAIL);
                        var cellTeleusuemp = row.CreateCell(73); cellTeleusuemp.SetCellValue(beneficiario.EU_TELEFONO);

                        //Beneficiario
                        var cellIdben = row.CreateCell(74); cellIdben.SetCellValue(beneficiario.ID_BENEFICIARIO == null ? string.Empty : beneficiario.ID_BENEFICIARIO.Value.ToString());
                        var cellIdestadoben = row.CreateCell(75); cellIdestadoben.SetCellValue(beneficiario.BEN_ID_ESTADO == null ? string.Empty : beneficiario.BEN_ID_ESTADO.Value.ToString());
                        var cellEstadoben = row.CreateCell(76); cellEstadoben.SetCellValue(beneficiario.BEN_N_ESTADO);
                        var cellNrocuentaben = row.CreateCell(77); cellNrocuentaben.SetCellValue(beneficiario.BEN_NRO_CTA == null ? String.Empty : beneficiario.BEN_NRO_CTA.ToString());
                        var cellIdsucursalben = row.CreateCell(78); cellIdsucursalben.SetCellValue(beneficiario.BEN_ID_SUCURSAL == null ? String.Empty : beneficiario.BEN_ID_SUCURSAL.ToString());
                        var cellCodsucursalben = row.CreateCell(79); cellCodsucursalben.SetCellValue(beneficiario.BEN_COD_SUC);
                        var cellSucursalben = row.CreateCell(80); cellSucursalben.SetCellValue(beneficiario.BEN_SUCURSAL);
                        var cellFecsolben = row.CreateCell(81); cellFecsolben.SetCellValue(beneficiario.BEN_FEC_SOL_CTA == null ? String.Empty
                            : beneficiario.BEN_FEC_SOL_CTA.Value.ToShortDateString());
                        var cellNotificado = row.CreateCell(82); cellNotificado.SetCellValue(beneficiario.NOTIFICADO == "S" ? "S" : "N");
                        var cellFecNotif = row.CreateCell(83); cellFecNotif.SetCellValue(beneficiario.FEC_NOTIF == null ? String.Empty
                            : beneficiario.FEC_NOTIF.Value.ToShortDateString());

                        var cellFechaIniBenf = row.CreateCell(84); cellFechaIniBenf.SetCellValue(beneficiario.BEN_FEC_INICIO == null ? string.Empty : beneficiario.BEN_FEC_INICIO.Value.ToShortDateString()); // 20/03/2013 - DI CAMPLI LEANDRO
                        var cellBajaBenef = row.CreateCell(85); cellBajaBenef.SetCellValue(beneficiario.BEN_FEC_BAJA == null ? string.Empty : beneficiario.BEN_FEC_BAJA.Value.ToShortDateString()); // 20/03/2013 - DI CAMPLI LEANDRO

                        var cellTieneapoben = row.CreateCell(86); cellTieneapoben.SetCellValue(beneficiario.TIENE_APODERADO);

                        //Apoderado
                        var cellIdapoderado = row.CreateCell(87); cellIdapoderado.SetCellValue(beneficiario.ID_APODERADO == null ?
                            String.Empty : beneficiario.ID_APODERADO.ToString());
                        var cellApellidoapo = row.CreateCell(88); cellApellidoapo.SetCellValue(beneficiario.APO_APELLIDO);
                        var cellNombreapo = row.CreateCell(89); cellNombreapo.SetCellValue(beneficiario.APO_NOMBRE);
                        var cellDniapo = row.CreateCell(90); cellDniapo.SetCellValue(beneficiario.APO_DNI);
                        var cellCuilapo = row.CreateCell(91); cellCuilapo.SetCellValue(beneficiario.APO_CUIL);
                        var cellSexoapo = row.CreateCell(92); cellSexoapo.SetCellValue(beneficiario.APO_SEXO);
                        var cellNroctaapo = row.CreateCell(93); cellNroctaapo.SetCellValue(beneficiario.APO_NRO_CTA == null ? String.Empty : beneficiario.APO_NRO_CTA.ToString());
                        var cellIdSuc = row.CreateCell(94); cellIdSuc.SetCellValue(beneficiario.APO_ID_SUC == null ? String.Empty : beneficiario.APO_ID_SUC.ToString());
                        var cellCodsucapo = row.CreateCell(95); cellCodsucapo.SetCellValue(beneficiario.APO_COD_SUC);
                        var cellSucursalapo = row.CreateCell(96); cellSucursalapo.SetCellValue(beneficiario.APO_SUCURSAL);
                        var cellFecsolcuentaapo = row.CreateCell(97); cellFecsolcuentaapo.SetCellValue(beneficiario.APO_FEC_SOL_CTA == null ? String.Empty : beneficiario.APO_FEC_SOL_CTA.Value.ToShortDateString());
                        var cellFecnac = row.CreateCell(98); cellFecnac.SetCellValue(beneficiario.APO_FEC_NAC == null ? String.Empty : beneficiario.APO_FEC_NAC.Value.ToShortDateString());
                        var cellCalleapo = row.CreateCell(99); cellCalleapo.SetCellValue(beneficiario.APO_CALLE);
                        var cellNroapo = row.CreateCell(100); cellNroapo.SetCellValue(beneficiario.APO_NRO);
                        var cellPisoapo = row.CreateCell(101); cellPisoapo.SetCellValue(beneficiario.APO_PISO);
                        var cellMonoblockapo = row.CreateCell(102); cellMonoblockapo.SetCellValue(beneficiario.APO_MONOBLOCK);
                        var cellDptoapo = row.CreateCell(103); cellDptoapo.SetCellValue(beneficiario.APO_DPTO);
                        var cellParcelaapo = row.CreateCell(104); cellParcelaapo.SetCellValue(beneficiario.APO_PARCELA);
                        var cellManzanaapo = row.CreateCell(105); cellManzanaapo.SetCellValue(beneficiario.APO_MANZANA);
                        var cellEntrecalleapo = row.CreateCell(106); cellEntrecalleapo.SetCellValue(beneficiario.APO_ENTRE_CALLES);
                        var cellBarrioapo = row.CreateCell(107); cellBarrioapo.SetCellValue(beneficiario.APO_BARRIO);
                        var cellCpapo = row.CreateCell(108); cellCpapo.SetCellValue(beneficiario.APO_CP);
                        var cellIdlocalidadapo = row.CreateCell(109); cellIdlocalidadapo.SetCellValue(beneficiario.APO_ID_LOC == null ? String.Empty
                            : beneficiario.APO_ID_LOC.ToString());
                        var cellLocalidadapo = row.CreateCell(110); cellLocalidadapo.SetCellValue(beneficiario.APO_LOCALIDAD);
                        var cellIddeptoapo = row.CreateCell(111); cellIddeptoapo.SetCellValue(beneficiario.APO_ID_LOC == null ? String.Empty
                            : beneficiario.APO_ID_DEPTO.ToString());
                        var cellDeptoapo = row.CreateCell(112); cellDeptoapo.SetCellValue(beneficiario.APO_DEPARTAMENTO);
                        var cellTelefonoapo = row.CreateCell(113); cellTelefonoapo.SetCellValue(beneficiario.APO_TELEFONO);
                        var cellCelularapo = row.CreateCell(114); cellCelularapo.SetCellValue(beneficiario.APO_CELULAR);
                        var cellEmailapo = row.CreateCell(115);
                        cellEmailapo.SetCellValue(beneficiario.APO_EMAIL);
                        var cellApoIdEstado = row.CreateCell(116);
                        cellApoIdEstado.SetCellValue(beneficiario.AP_ID_ESTADO == null ? String.Empty
                            : beneficiario.AP_ID_ESTADO.ToString());
                        var cellApoEstado = row.CreateCell(117);
                        cellApoEstado.SetCellValue(beneficiario.AP_N_ESTADO);

                        var cellIdSubprograma = row.CreateCell(118); cellIdSubprograma.SetCellValue(beneficiario.ID_SUBPROGRAMA == null ? "" : beneficiario.ID_SUBPROGRAMA.ToString());
                        var cellSubprograma = row.CreateCell(119); cellSubprograma.SetCellValue(beneficiario.SUBPROGRAMA ?? "");

                        var cellTipo_PPP = row.CreateCell(120); cellTipo_PPP.SetCellValue(beneficiario.TIPO_PPP == null ? "" : beneficiario.TIPO_PPP.ToString());
                        string tipoPpp = "";
                        foreach (var tipo in tiposppp)
                        {
                            if (tipo.ID_TIPO_PPP == (beneficiario.TIPO_PPP ?? 1))
                            {
                                tipoPpp = tipo.N_TIPO_PPP;
                            }
                            
                        }
                        //switch (beneficiario.TIPO_PPP)
                        //{
                        //    case 1:
                        //        tipoPpp = "PROGRAMA PRIMER PASO";
                        //        break;
                        //    case 2:
                        //        tipoPpp = "PROGRAMA PRIMER PASO APRENDIZ";
                        //        break;
                        //    case 3:
                        //        tipoPpp = "PROGRAMA XMÍ";
                        //        break;
                        //    case 4:
                        //        tipoPpp = "PILA";
                        //        break;
                        //    case 5:
                        //        tipoPpp = "PIP";
                        //        break;
                        //    case 6:
                        //        tipoPpp = "PROGRAMA CLIP";
                        //        break;
                        //    case 7:
                        //        tipoPpp = "PIL - COMERCIO EXTERIOR";
                        //        break;
                        //    case 8:
                        //        tipoPpp = "PIL - COMERCIO ELECTRONICO";
                        //        break;
                        //    case 9:
                        //        tipoPpp = "PIL - MAQUINARIA AGRICOLA";
                        //        break;
                        //    case 10:
                        //        tipoPpp = "PIL - TURISMO";
                        //        break;
                        //}
                        var cellNTipoPPP = row.CreateCell(121); cellNTipoPPP.SetCellValue(tipoPpp);
                        var cellSedeRot = row.CreateCell(122); cellSedeRot.SetCellValue(beneficiario.SEDE_ROTATIVA == "S" ? "S" : "N");
                        var cellHoraRot = row.CreateCell(123); cellHoraRot.SetCellValue(beneficiario.HORARIO_ROTATIVO == "S" ? "S" : "N");

                        //DATOS PIP

                        var cellIdCarrera = row.CreateCell(124); cellIdCarrera.SetCellValue(beneficiario.ID_CARRERA == null ? String.Empty : beneficiario.ID_CARRERA.Value.ToString());
                        var cellNCarrera = row.CreateCell(125); cellNCarrera.SetCellValue(beneficiario.ID_CARRERA == null ? beneficiario.N_DESCRIPCION_T : beneficiario.N_CARRERA);
                        var cellIdInstitucion = row.CreateCell(126); cellIdInstitucion.SetCellValue(beneficiario.ID_INSTITUCION == null ? String.Empty : beneficiario.ID_INSTITUCION.Value.ToString());
                        var cellNInstitucion = row.CreateCell(127); cellNInstitucion.SetCellValue(beneficiario.ID_INSTITUCION == null ? beneficiario.N_CURSADO_INS : beneficiario.N_INSTITUCION);
                        var cellEgreso = row.CreateCell(128); cellEgreso.SetCellValue(beneficiario.EGRESO == null ? string.Empty : beneficiario.EGRESO.Value.ToShortDateString());
                        var cellCodUsoInt = row.CreateCell(129); cellCodUsoInt.SetCellValue(beneficiario.COD_USO_INTERNO);
                        var cellCargaHoraria = row.CreateCell(130); cellCargaHoraria.SetCellValue(beneficiario.CARGA_HORARIA == "S" ? "S" : "N");
                        var cellChCual = row.CreateCell(131); cellChCual.SetCellValue(beneficiario.CH_CUAL);
                        var cellCONSTANCIAEGRESO = row.CreateCell(132); cellCONSTANCIAEGRESO.SetCellValue(beneficiario.CONSTANCIA_EGRESO == "S" ? "S" : "N");
                        var cellMATRICULA = row.CreateCell(133); cellMATRICULA.SetCellValue(beneficiario.MATRICULA == "S" ? "S" : "N");
                        var cellCONSTANCIACURSO = row.CreateCell(134); cellCONSTANCIACURSO.SetCellValue(beneficiario.CONSTANCIA_CURSO == "S" ? "S" : "N");
                    
                    
                    }
                    #endregion
                    break;


            }


            var ms = new MemoryStream();

            templateWorkbook.Write(ms);

            return ms.ToArray();
        }

        public byte[] ExportarFichasTer(string cuil, string dni, string nombre, string apellido, int tipoficha, int estadoficha, int idEtapa, int idSubprograma)
        {
            var path = String.Empty;


            path = HttpContext.Current.Server.MapPath(@"\Archivos\Exportados\TemplateBeneficiarioTerciarios.xls");

            var fs = new FileStream(path, FileMode.Open, FileAccess.Read);

            var templateWorkbook = new HSSFWorkbook(fs, true);

            var sheet = templateWorkbook.GetSheet("Nomina");

            List<VT_REPORTES_TER> listaparaExcel = null;

            listaparaExcel = _FichaRepo.getfichaTER(cuil, dni, nombre, apellido, tipoficha, estadoficha, idEtapa, idSubprograma);

            var i = 1;

            switch (tipoficha)
            {

                case (int)Enums.TipoFicha.Terciaria:
                    #region case (int)Enums.TipoFicha.Terciaria
                    foreach (var beneficiario in listaparaExcel)
                    {
                        i++;
                        //Ficha
                        var row = sheet.CreateRow(i);
                        var cellidficha = row.CreateCell(0); cellidficha.SetCellValue(beneficiario.ID_FICHA);
                        var cellApellido = row.CreateCell(1); cellApellido.SetCellValue(beneficiario.APELLIDO);
                        var cellNombre = row.CreateCell(2); cellNombre.SetCellValue(beneficiario.NOMBRE);
                        var cellCuil = row.CreateCell(3); cellCuil.SetCellValue(beneficiario.CUIL);
                        var cellDni = row.CreateCell(4); cellDni.SetCellValue(beneficiario.DNI);
                        var cellFechaNa = row.CreateCell(5); cellFechaNa.SetCellValue(beneficiario.FEC_NAC == null ? "" : beneficiario.FEC_NAC.ToShortDateString());
                        var cellCalle = row.CreateCell(6); cellCalle.SetCellValue(beneficiario.CALLE);
                        var cellNro = row.CreateCell(7); cellNro.SetCellValue(beneficiario.NRO);
                        var cellPiso = row.CreateCell(8); cellPiso.SetCellValue(beneficiario.PISO);
                        var cellDpto = row.CreateCell(9); cellDpto.SetCellValue(beneficiario.DPTO);
                        var cellMono = row.CreateCell(10); cellMono.SetCellValue(beneficiario.MONOBLOCK);
                        var cellParcela = row.CreateCell(11); cellParcela.SetCellValue(beneficiario.PARCELA);
                        var cellManzana = row.CreateCell(12); cellManzana.SetCellValue(beneficiario.MANZANA);
                        var cellEntreCalle = row.CreateCell(13); cellEntreCalle.SetCellValue(beneficiario.ENTRECALLES);
                        var cellBarrio = row.CreateCell(14); cellBarrio.SetCellValue(beneficiario.BARRIO);
                        var cellCp = row.CreateCell(15); cellCp.SetCellValue(beneficiario.CP);
                        var cellidLocalidad = row.CreateCell(16); cellidLocalidad.SetCellValue(beneficiario.ID_LOC == null ? String.Empty
                            : beneficiario.ID_LOC.ToString());
                        var cellnLocalidad = row.CreateCell(17); cellnLocalidad.SetCellValue(beneficiario.N_LOCALIDAD);
                        var cellIdDepartamento = row.CreateCell(18); cellIdDepartamento.SetCellValue(beneficiario.ID_DPTO == null ? String.Empty
                            : beneficiario.ID_DPTO.ToString());
                        var cellnDepartamento = row.CreateCell(19); cellnDepartamento.SetCellValue(beneficiario.N_DEPARTAMENTO);
                        var cellTelFijo = row.CreateCell(20); cellTelFijo.SetCellValue(beneficiario.TEL_FIJO);
                        var cellTelCelular = row.CreateCell(21); cellTelCelular.SetCellValue(beneficiario.TEL_CELULAR);
                        var cellContacto = row.CreateCell(22); cellContacto.SetCellValue(beneficiario.CONTACTO);
                        var cellEmail = row.CreateCell(23); cellEmail.SetCellValue(beneficiario.EMAIL);

                        var cellTieneHijos = row.CreateCell(24); cellTieneHijos.SetCellValue(beneficiario.TIENE_HIJOS);//TIENE_HIJOS	
                        var cellCantidadHijos = row.CreateCell(25); cellCantidadHijos.SetCellValue(beneficiario.CANTIDAD_HIJOS == null ? "0" : beneficiario.CANTIDAD_HIJOS.ToString());//CANTIDAD_HIJOS
                        var cellEsDiscapacitado = row.CreateCell(26); cellEsDiscapacitado.SetCellValue(beneficiario.ES_DISCAPACITADO == null ? "N" : beneficiario.ES_DISCAPACITADO);//ES_DIACAPACITADO
                        var cellTieneDeficienciaMotora = row.CreateCell(27); cellTieneDeficienciaMotora.SetCellValue(beneficiario.TIENE_DEF_MOTORA == null ? "N" : beneficiario.TIENE_DEF_MOTORA);//TIENE_DEF_MOTORA
                        var cellTieneDeficienciaMental = row.CreateCell(28); cellTieneDeficienciaMental.SetCellValue(beneficiario.TIENE_DEF_MENTAL == null ? "N" : beneficiario.TIENE_DEF_MENTAL);//TIENE_DEF_MENTAL
                        var cellTieneDeficienciaSensorial = row.CreateCell(29); cellTieneDeficienciaSensorial.SetCellValue(beneficiario.TIENE_DEF_SENSORIAL == null ? "N" : beneficiario.TIENE_DEF_SENSORIAL);//TIENE_DEF_SENSORIAL
                        var cellTieneDeficienciaPsicologia = row.CreateCell(30); cellTieneDeficienciaPsicologia.SetCellValue(beneficiario.TIENE_DEF_PSICOLOGICA == null ? "N" : beneficiario.TIENE_DEF_PSICOLOGICA);//TIENE_DEF_PSICOLOGICA
                        var cellDeficienciaOtra = row.CreateCell(31); cellDeficienciaOtra.SetCellValue(beneficiario.DEFICIENCIA_OTRA == null ? "N" : beneficiario.TIENE_DEF_PSICOLOGICA);//DEFICIENCIA_OTRA
                        var cellCertificadoDiscapacidad = row.CreateCell(32); cellCertificadoDiscapacidad.SetCellValue(beneficiario.CERTIF_DISCAP == null ? "N" : beneficiario.CERTIF_DISCAP);//CERTIF_DISCAP

                        var cellSexo = row.CreateCell(33); cellSexo.SetCellValue(beneficiario.SEXO);
                        var cellidestadoFicha = row.CreateCell(34); cellidestadoFicha.SetCellValue(beneficiario.ID_EST_FIC == null ? String.Empty
                            : beneficiario.ID_EST_FIC.ToString());
                        var cellnestadoFicha = row.CreateCell(35); cellnestadoFicha.SetCellValue(beneficiario.N_ESTADO_FICHA);

                        var cellPromedioterc = row.CreateCell(36); cellPromedioterc.SetCellValue(beneficiario.PROMEDIO == null ? String.Empty
                            : beneficiario.PROMEDIO.ToString());
                        var cellOtracarreraterc = row.CreateCell(37); cellOtracarreraterc.SetCellValue(beneficiario.OTRA_CARRERA);
                        var cellOtrainstiterc = row.CreateCell(38); cellOtrainstiterc.SetCellValue(beneficiario.OTRA_INSTITUCION);
                        var cellIdcarreraterc = row.CreateCell(39); cellIdcarreraterc.SetCellValue(beneficiario.CARRERA == null ?
                            String.Empty : beneficiario.CARRERA.ToString());
                        var cellIdescuelaterc = row.CreateCell(40); cellIdescuelaterc.SetCellValue(beneficiario.ID_ESCUELA == null ?
                            String.Empty : beneficiario.ID_ESCUELA.ToString());
                        var cellEscuelaterc = row.CreateCell(41); cellEscuelaterc.SetCellValue(beneficiario.ESCUELA);
                        var cellCueterc = row.CreateCell(42); cellCueterc.SetCellValue(beneficiario.CUE);
                        var cellAnexoterc = row.CreateCell(43); cellAnexoterc.SetCellValue(beneficiario.ANEXO == null ? String.Empty
                            : beneficiario.ANEXO.ToString());

                        var cellEscBarriterc = row.CreateCell(44); cellEscBarriterc.SetCellValue(beneficiario.BARRIO);
                        var cellEscidlocalidadterc = row.CreateCell(45); cellEscidlocalidadterc.SetCellValue(beneficiario.ESC_ID_LOCALIDAD == null ?
                            String.Empty : beneficiario.ESC_ID_LOCALIDAD.ToString());
                        var cellEsclocalidadterc = row.CreateCell(46); cellEsclocalidadterc.SetCellValue(beneficiario.ESC_LOCALIDAD);
                        var cellOtrosectorterc = row.CreateCell(47); cellOtrosectorterc.SetCellValue(beneficiario.OTRO_SECTOR);
                        var cellCarreraterc = row.CreateCell(48); cellCarreraterc.SetCellValue(beneficiario.N_CARRERA);
                        var cellIdnivelterc = row.CreateCell(49); cellIdnivelterc.SetCellValue(beneficiario.ID_NIVEL == null ?
                            String.Empty : beneficiario.ID_NIVEL.ToString());
                        var cellNivelterc = row.CreateCell(50); cellNivelterc.SetCellValue(beneficiario.N_NIVEL);
                        var cellIdinstitucionterc = row.CreateCell(51); cellIdinstitucionterc.SetCellValue(beneficiario.ID_INSTITUCION == null ?
                            String.Empty : beneficiario.ID_INSTITUCION.ToString());
                        var cellInstitucionterc = row.CreateCell(52); cellInstitucionterc.SetCellValue(beneficiario.N_INSTITUCION);
                        var cellIdsectorterc = row.CreateCell(53); cellIdsectorterc.SetCellValue(beneficiario.ID_SECTOR == null ? String.Empty
                            : beneficiario.ID_SECTOR.ToString());
                        var cellSectorterc = row.CreateCell(54); cellSectorterc.SetCellValue(beneficiario.N_SECTOR);
                        var cellFechaModif = row.CreateCell(55); cellFechaModif.SetCellValue(beneficiario.FECHA_MODIF == null ? String.Empty
                            : beneficiario.FECHA_MODIF.Value.ToShortDateString());

                        //Beneficiario
                        var cellIdben = row.CreateCell(56); cellIdben.SetCellValue(beneficiario.ID_BENEFICIARIO == null ? string.Empty : beneficiario.ID_BENEFICIARIO.ToString());
                        var cellIdestadoben = row.CreateCell(57); cellIdestadoben.SetCellValue(beneficiario.ID_ESTADO == null ? string.Empty : beneficiario.ID_ESTADO.ToString());
                        var cellEstadoben = row.CreateCell(58); cellEstadoben.SetCellValue(beneficiario.BEN_N_ESTADO);
                        var cellNrocuentaben = row.CreateCell(59); cellNrocuentaben.SetCellValue(beneficiario.NRO_CTA == null ? String.Empty
                            : beneficiario.NRO_CTA.ToString());
                        var cellIdsucursalben = row.CreateCell(60); cellIdsucursalben.SetCellValue(beneficiario.ID_SUCURSAL == null ? String.Empty
                            : beneficiario.ID_SUCURSAL.ToString());
                        var cellCodsucursalben = row.CreateCell(61); cellCodsucursalben.SetCellValue(beneficiario.COD_SUC);
                        var cellSucursalben = row.CreateCell(62); cellSucursalben.SetCellValue(beneficiario.SUCURSAL);
                        var cellFecsolben = row.CreateCell(63); cellFecsolben.SetCellValue(beneficiario.FEC_SOL_CTA == null ? String.Empty
                            : beneficiario.FEC_SOL_CTA.Value.ToShortDateString());
                        var cellNotificado = row.CreateCell(64); cellNotificado.SetCellValue(beneficiario.NOTIFICADO != null ? beneficiario.NOTIFICADO : "N");
                        var cellFecNotif = row.CreateCell(65); cellFecNotif.SetCellValue(beneficiario.FEC_NOTIF == null ? String.Empty
                            : beneficiario.FEC_NOTIF.Value.ToShortDateString());

                        var cellFechaIniBenf = row.CreateCell(66); cellFechaIniBenf.SetCellValue(beneficiario.FEC_INICIO == null ? string.Empty : beneficiario.FEC_INICIO.Value.ToShortDateString()); // 20/03/2013 - DI CAMPLI LEANDRO
                        var cellBajaBenef = row.CreateCell(67); cellBajaBenef.SetCellValue(beneficiario.FEC_BAJA == null ? string.Empty : beneficiario.FEC_BAJA.Value.ToShortDateString()); // 20/03/2013 - DI CAMPLI LEANDRO

                        var cellTieneapoben = row.CreateCell(68); cellTieneapoben.SetCellValue(beneficiario.APODERADO);

                        //Apoderado
                        var cellIdapoderado = row.CreateCell(69); cellIdapoderado.SetCellValue(beneficiario.ID_APODERADO == null ?
                            String.Empty : beneficiario.ID_APODERADO.ToString());
                        var cellApellidoapo = row.CreateCell(70); cellApellidoapo.SetCellValue(beneficiario.APO_APELLIDO);
                        var cellNombreapo = row.CreateCell(71); cellNombreapo.SetCellValue(beneficiario.APO_NOMBRE);
                        var cellDniapo = row.CreateCell(72); cellDniapo.SetCellValue(beneficiario.APO_DNI);
                        var cellCuilapo = row.CreateCell(73); cellCuilapo.SetCellValue(beneficiario.APO_CUIL);
                        var cellSexoapo = row.CreateCell(74); cellSexoapo.SetCellValue(beneficiario.APO_SEXO);
                        var cellNroctaapo = row.CreateCell(75); cellNroctaapo.SetCellValue(beneficiario.APO_NRO_CTA == null ? String.Empty
                            : beneficiario.APO_NRO_CTA.ToString());
                        var cellIdSuc = row.CreateCell(76); cellIdSuc.SetCellValue(beneficiario.APO_ID_SUC == null ? String.Empty
                            : beneficiario.APO_ID_SUC.ToString());
                        var cellCodsucapo = row.CreateCell(77); cellCodsucapo.SetCellValue(beneficiario.APO_COD_SUC);
                        var cellSucursalapo = row.CreateCell(78); cellSucursalapo.SetCellValue(beneficiario.APO_SUCURSAL);
                        var cellFecsolcuentaapo = row.CreateCell(79); cellFecsolcuentaapo.SetCellValue(beneficiario.APO_FEC_SOL_CTA == null ?
                            String.Empty : beneficiario.APO_FEC_SOL_CTA.Value.ToShortDateString());
                        var cellFecnac = row.CreateCell(80); cellFecnac.SetCellValue(beneficiario.APO_FEC_NAC == null ? String.Empty :
                            beneficiario.APO_FEC_NAC.Value.ToShortDateString());
                        var cellCalleapo = row.CreateCell(81); cellCalleapo.SetCellValue(beneficiario.APO_CALLE);
                        var cellNroapo = row.CreateCell(82); cellNroapo.SetCellValue(beneficiario.APO_NRO);
                        var cellPisoapo = row.CreateCell(83); cellPisoapo.SetCellValue(beneficiario.APO_PISO);
                        var cellMonoblockapo = row.CreateCell(84); cellMonoblockapo.SetCellValue(beneficiario.APO_MONOBLOCK);
                        var cellDptoapo = row.CreateCell(85); cellDptoapo.SetCellValue(beneficiario.APO_DPTO);
                        var cellParcelaapo = row.CreateCell(86); cellParcelaapo.SetCellValue(beneficiario.APO_PARCELA);
                        var cellManzanaapo = row.CreateCell(87); cellManzanaapo.SetCellValue(beneficiario.APO_MANZANA);
                        var cellEntrecalleapo = row.CreateCell(88); cellEntrecalleapo.SetCellValue(beneficiario.APO_ENTRE_CALLES);
                        var cellBarrioapo = row.CreateCell(89); cellBarrioapo.SetCellValue(beneficiario.APO_BARRIO);
                        var cellCpapo = row.CreateCell(90); cellCpapo.SetCellValue(beneficiario.APO_CP);
                        var cellIdlocalidadapo = row.CreateCell(91); cellIdlocalidadapo.SetCellValue(beneficiario.APO_ID_LOC == null ?
                            String.Empty : beneficiario.APO_ID_LOC.ToString());
                        var cellLocalidadapo = row.CreateCell(92); cellLocalidadapo.SetCellValue(beneficiario.APO_LOCALIDAD);
                        var cellIddeptoapo = row.CreateCell(93); cellIddeptoapo.SetCellValue(beneficiario.APO_ID_DEPTO == null ? String.Empty
                            : beneficiario.APO_ID_DEPTO.ToString());
                        var cellDeptoapo = row.CreateCell(94); cellDeptoapo.SetCellValue(beneficiario.APO_DEPARTAMENTO);
                        var cellTelefonoapo = row.CreateCell(95); cellTelefonoapo.SetCellValue(beneficiario.APO_TELEFONO);
                        var cellCelularapo = row.CreateCell(96); cellCelularapo.SetCellValue(beneficiario.APO_CELULAR);
                        var cellEmailapo = row.CreateCell(97);
                        cellEmailapo.SetCellValue(beneficiario.APO_EMAIL);

                        var cellApoIdEstado = row.CreateCell(98);
                        cellApoIdEstado.SetCellValue(beneficiario.AP_ID_ESTADO == null ? String.Empty
                            : beneficiario.AP_ID_ESTADO.ToString());
                        var cellApoEstado = row.CreateCell(99);
                        cellApoEstado.SetCellValue(beneficiario.AP_N_ESTADO);

                        var cellIdSubprograma = row.CreateCell(100); cellIdSubprograma.SetCellValue(beneficiario.ID_SUBPROGRAMA == null ? "" : beneficiario.ID_SUBPROGRAMA.ToString());
                        var cellSubprograma = row.CreateCell(101); cellSubprograma.SetCellValue(beneficiario.SUBPROGRAMA ?? "");

                    }
                    #endregion

                    break;


            }


            var ms = new MemoryStream();

            templateWorkbook.Write(ms);

            return ms.ToArray();
        }

        public byte[] ExportarFichasUni(string cuil, string dni, string nombre, string apellido, int tipoficha, int estadoficha, int idEtapa, int idSubprograma)
        {
            var path = String.Empty;


            path = HttpContext.Current.Server.MapPath(@"\Archivos\Exportados\TemplateBeneficiarioUniversitarios.xls");

            var fs = new FileStream(path, FileMode.Open, FileAccess.Read);

            var templateWorkbook = new HSSFWorkbook(fs, true);

            var sheet = templateWorkbook.GetSheet("Nomina");

            List<VT_REPORTES_UNI> listaparaExcel = null;

            listaparaExcel = _FichaRepo.getfichaUni(cuil, dni, nombre, apellido, tipoficha, estadoficha, idEtapa, idSubprograma);

            var i = 1;

            switch (tipoficha)
            {

                case (int)Enums.TipoFicha.Universitaria:
                    #region case (int)Enums.TipoFicha.Universitaria
                    foreach (var beneficiario in listaparaExcel)
                    {
                        i++;
                        //Ficha
                        var row = sheet.CreateRow(i);
                        var cellidficha = row.CreateCell(0); cellidficha.SetCellValue(beneficiario.ID_FICHA);
                        var cellApellido = row.CreateCell(1); cellApellido.SetCellValue(beneficiario.APELLIDO);
                        var cellNombre = row.CreateCell(2); cellNombre.SetCellValue(beneficiario.NOMBRE);
                        var cellCuil = row.CreateCell(3); cellCuil.SetCellValue(beneficiario.CUIL);
                        var cellDni = row.CreateCell(4); cellDni.SetCellValue(beneficiario.DNI);
                        var cellFechaNa = row.CreateCell(5); cellFechaNa.SetCellValue(beneficiario.FEC_NAC == null ? "" : beneficiario.FEC_NAC.ToShortDateString());
                        var cellCalle = row.CreateCell(6); cellCalle.SetCellValue(beneficiario.CALLE);
                        var cellNro = row.CreateCell(7); cellNro.SetCellValue(beneficiario.NRO);
                        var cellPiso = row.CreateCell(8); cellPiso.SetCellValue(beneficiario.PISO);
                        var cellDpto = row.CreateCell(9); cellDpto.SetCellValue(beneficiario.DPTO);
                        var cellMono = row.CreateCell(10); cellMono.SetCellValue(beneficiario.MONOBLOCK);
                        var cellParcela = row.CreateCell(11); cellParcela.SetCellValue(beneficiario.PARCELA);
                        var cellManzana = row.CreateCell(12); cellManzana.SetCellValue(beneficiario.MANZANA);
                        var cellEntreCalle = row.CreateCell(13); cellEntreCalle.SetCellValue(beneficiario.ENTRECALLES);
                        var cellBarrio = row.CreateCell(14); cellBarrio.SetCellValue(beneficiario.BARRIO);
                        var cellCp = row.CreateCell(15); cellCp.SetCellValue(beneficiario.CP);
                        var cellidLocalidad = row.CreateCell(16); cellidLocalidad.SetCellValue(beneficiario.ID_LOC == null ? String.Empty
                            : beneficiario.ID_LOC.ToString());
                        var cellnLocalidad = row.CreateCell(17); cellnLocalidad.SetCellValue(beneficiario.N_LOCALIDAD);
                        var cellIdDepartamento = row.CreateCell(18); cellIdDepartamento.SetCellValue(beneficiario.ID_DPTO == null ? String.Empty
                            : beneficiario.ID_DPTO.ToString());
                        var cellnDepartamento = row.CreateCell(19); cellnDepartamento.SetCellValue(beneficiario.N_DEPARTAMENTO);
                        var cellTelFijo = row.CreateCell(20); cellTelFijo.SetCellValue(beneficiario.TEL_FIJO);
                        var cellTelCelular = row.CreateCell(21); cellTelCelular.SetCellValue(beneficiario.TEL_CELULAR);
                        var cellContacto = row.CreateCell(22); cellContacto.SetCellValue(beneficiario.CONTACTO);
                        var cellEmail = row.CreateCell(23); cellEmail.SetCellValue(beneficiario.EMAIL);

                        var cellTieneHijos = row.CreateCell(24); cellTieneHijos.SetCellValue(beneficiario.TIENE_HIJOS);//TIENE_HIJOS	
                        var cellCantidadHijos = row.CreateCell(25); cellCantidadHijos.SetCellValue(beneficiario.CANTIDAD_HIJOS == null ? "0" : beneficiario.CANTIDAD_HIJOS.ToString());//CANTIDAD_HIJOS
                        var cellEsDiscapacitado = row.CreateCell(26); cellEsDiscapacitado.SetCellValue(beneficiario.ES_DISCAPACITADO == null ? "N" : beneficiario.ES_DISCAPACITADO);//ES_DIACAPACITADO
                        var cellTieneDeficienciaMotora = row.CreateCell(27); cellTieneDeficienciaMotora.SetCellValue(beneficiario.TIENE_DEF_MOTORA == null ? "N" : beneficiario.TIENE_DEF_MOTORA);//TIENE_DEF_MOTORA
                        var cellTieneDeficienciaMental = row.CreateCell(28); cellTieneDeficienciaMental.SetCellValue(beneficiario.TIENE_DEF_MENTAL == null ? "N" : beneficiario.TIENE_DEF_MENTAL);//TIENE_DEF_MENTAL
                        var cellTieneDeficienciaSensorial = row.CreateCell(29); cellTieneDeficienciaSensorial.SetCellValue(beneficiario.TIENE_DEF_SENSORIAL == null ? "N" : beneficiario.TIENE_DEF_SENSORIAL);//TIENE_DEF_SENSORIAL
                        var cellTieneDeficienciaPsicologia = row.CreateCell(30); cellTieneDeficienciaPsicologia.SetCellValue(beneficiario.TIENE_DEF_PSICOLOGICA == null ? "N" : beneficiario.TIENE_DEF_PSICOLOGICA);//TIENE_DEF_PSICOLOGICA
                        var cellDeficienciaOtra = row.CreateCell(31); cellDeficienciaOtra.SetCellValue(beneficiario.DEFICIENCIA_OTRA == null ? "N" : beneficiario.TIENE_DEF_PSICOLOGICA);//DEFICIENCIA_OTRA
                        var cellCertificadoDiscapacidad = row.CreateCell(32); cellCertificadoDiscapacidad.SetCellValue(beneficiario.CERTIF_DISCAP == null ? "N" : beneficiario.CERTIF_DISCAP);//CERTIF_DISCAP

                        var cellSexo = row.CreateCell(33); cellSexo.SetCellValue(beneficiario.SEXO);
                        var cellidestadoFicha = row.CreateCell(34); cellidestadoFicha.SetCellValue(beneficiario.ID_EST_FIC == null ? String.Empty
                            : beneficiario.ID_EST_FIC.ToString());
                        var cellnestadoFicha = row.CreateCell(35); cellnestadoFicha.SetCellValue(beneficiario.N_ESTADO_FICHA);

                        var cellPromedioterc = row.CreateCell(36); cellPromedioterc.SetCellValue(beneficiario.PROMEDIO == null ? String.Empty
                            : beneficiario.PROMEDIO.ToString());
                        var cellOtracarreraterc = row.CreateCell(37); cellOtracarreraterc.SetCellValue(beneficiario.OTRA_CARRERA);
                        var cellOtrainstiterc = row.CreateCell(38); cellOtrainstiterc.SetCellValue(beneficiario.OTRA_INSTITUCION);
                        var cellIdCarreraUniv = row.CreateCell(39); cellIdCarreraUniv.SetCellValue(beneficiario.CARRERA == null ?
                            String.Empty : beneficiario.CARRERA.ToString());
                        var cellCarreraUniv = row.CreateCell(40); cellCarreraUniv.SetCellValue(beneficiario.N_CARRERA);
                        var cellIdEscuelaUniv = row.CreateCell(41); cellIdEscuelaUniv.SetCellValue(beneficiario.ID_ESCUELA == null ?
                            String.Empty : beneficiario.ID_ESCUELA.ToString());
                        var cellEscuelaUniv = row.CreateCell(42); cellEscuelaUniv.SetCellValue(beneficiario.ESCUELA);
                        var cellCueUniv = row.CreateCell(43); cellCueUniv.SetCellValue(beneficiario.CUE);
                        var cellAnexoUniv = row.CreateCell(44); cellAnexoUniv.SetCellValue(beneficiario.ANEXO == null ?
                            String.Empty : beneficiario.ANEXO.ToString());

                        var cellEscBarrioUniv = row.CreateCell(45); cellEscBarrioUniv.SetCellValue(beneficiario.ESC_BARRIO);
                        var cellEscIdLocalidadUniv = row.CreateCell(46); cellEscIdLocalidadUniv.SetCellValue(beneficiario.ESC_ID_LOCALIDAD == null ?
                            String.Empty : beneficiario.ESC_ID_LOCALIDAD.ToString());
                        var cellEscLocalidadUniv = row.CreateCell(47); cellEscLocalidadUniv.SetCellValue(beneficiario.ESC_LOCALIDAD);
                        var cellOtroSectorUniv = row.CreateCell(48); cellOtroSectorUniv.SetCellValue(beneficiario.OTRO_SECTOR);
                        var cellIdnivelterc = row.CreateCell(49); cellIdnivelterc.SetCellValue(beneficiario.ID_NIVEL == null ?
                            String.Empty : beneficiario.ID_NIVEL.ToString());
                        var cellNivelterc = row.CreateCell(50); cellNivelterc.SetCellValue(beneficiario.N_NIVEL);
                        var cellIdinstitucionterc = row.CreateCell(51); cellIdinstitucionterc.SetCellValue(beneficiario.ID_INSTITUCION == null ?
                            String.Empty : beneficiario.ID_INSTITUCION.ToString());
                        var cellInstitucionterc = row.CreateCell(52); cellInstitucionterc.SetCellValue(beneficiario.N_INSTITUCION);
                        var cellIdsectorterc = row.CreateCell(53); cellIdsectorterc.SetCellValue(beneficiario.ID_SECTOR == null ? String.Empty
                            : beneficiario.ID_SECTOR.ToString());
                        var cellSectorterc = row.CreateCell(54); cellSectorterc.SetCellValue(beneficiario.N_SECTOR);
                        var cellFechaModif = row.CreateCell(55); cellFechaModif.SetCellValue(beneficiario.FECHA_MODIF == null ? String.Empty
                            : beneficiario.FECHA_MODIF.Value.ToShortDateString());

                        //Beneficiario
                        var cellIdben = row.CreateCell(56); cellIdben.SetCellValue(beneficiario.ID_BENEFICIARIO == null ? string.Empty : beneficiario.ID_BENEFICIARIO.ToString());
                        var cellIdestadoben = row.CreateCell(57); cellIdestadoben.SetCellValue(beneficiario.ID_ESTADO == null ? string.Empty : beneficiario.ID_ESTADO.ToString());
                        var cellEstadoben = row.CreateCell(58); cellEstadoben.SetCellValue(beneficiario.BEN_N_ESTADO);
                        var cellNrocuentaben = row.CreateCell(59); cellNrocuentaben.SetCellValue(beneficiario.NRO_CTA == null ? String.Empty
                            : beneficiario.NRO_CTA.ToString());
                        var cellIdsucursalben = row.CreateCell(60); cellIdsucursalben.SetCellValue(beneficiario.ID_SUCURSAL == null ? String.Empty
                            : beneficiario.ID_SUCURSAL.ToString());
                        var cellCodsucursalben = row.CreateCell(61); cellCodsucursalben.SetCellValue(beneficiario.COD_SUC);
                        var cellSucursalben = row.CreateCell(62); cellSucursalben.SetCellValue(beneficiario.SUCURSAL);
                        var cellFecsolben = row.CreateCell(63); cellFecsolben.SetCellValue(beneficiario.FEC_SOL_CTA == null ? String.Empty
                            : beneficiario.FEC_SOL_CTA.Value.ToShortDateString());
                        var cellNotificado = row.CreateCell(64); cellNotificado.SetCellValue(beneficiario.NOTIFICADO != null ? beneficiario.NOTIFICADO : "N");
                        var cellFecNotif = row.CreateCell(65); cellFecNotif.SetCellValue(beneficiario.FEC_NOTIF == null ? String.Empty
                            : beneficiario.FEC_NOTIF.Value.ToShortDateString());

                        var cellFechaIniBenf = row.CreateCell(66); cellFechaIniBenf.SetCellValue(beneficiario.FEC_INICIO == null ? string.Empty : beneficiario.FEC_INICIO.Value.ToShortDateString()); // 20/03/2013 - DI CAMPLI LEANDRO
                        var cellBajaBenef = row.CreateCell(67); cellBajaBenef.SetCellValue(beneficiario.FEC_BAJA == null ? string.Empty : beneficiario.FEC_BAJA.Value.ToShortDateString()); // 20/03/2013 - DI CAMPLI LEANDRO

                        var cellTieneapoben = row.CreateCell(68); cellTieneapoben.SetCellValue(beneficiario.APODERADO);

                        //Apoderado
                        var cellIdapoderado = row.CreateCell(69); cellIdapoderado.SetCellValue(beneficiario.ID_APODERADO == null ?
                            String.Empty : beneficiario.ID_APODERADO.ToString());
                        var cellApellidoapo = row.CreateCell(70); cellApellidoapo.SetCellValue(beneficiario.APO_APELLIDO);
                        var cellNombreapo = row.CreateCell(71); cellNombreapo.SetCellValue(beneficiario.APO_NOMBRE);
                        var cellDniapo = row.CreateCell(72); cellDniapo.SetCellValue(beneficiario.APO_DNI);
                        var cellCuilapo = row.CreateCell(73); cellCuilapo.SetCellValue(beneficiario.APO_CUIL);
                        var cellSexoapo = row.CreateCell(74); cellSexoapo.SetCellValue(beneficiario.APO_SEXO);
                        var cellNroctaapo = row.CreateCell(75); cellNroctaapo.SetCellValue(beneficiario.APO_NRO_CTA == null ? String.Empty
                            : beneficiario.APO_NRO_CTA.ToString());
                        var cellIdSuc = row.CreateCell(76); cellIdSuc.SetCellValue(beneficiario.APO_ID_SUC == null ? String.Empty
                            : beneficiario.APO_ID_SUC.ToString());
                        var cellCodsucapo = row.CreateCell(77); cellCodsucapo.SetCellValue(beneficiario.APO_COD_SUC);
                        var cellSucursalapo = row.CreateCell(78); cellSucursalapo.SetCellValue(beneficiario.APO_SUCURSAL);
                        var cellFecsolcuentaapo = row.CreateCell(79); cellFecsolcuentaapo.SetCellValue(beneficiario.APO_FEC_SOL_CTA == null ?
                            String.Empty : beneficiario.APO_FEC_SOL_CTA.Value.ToShortDateString());
                        var cellFecnac = row.CreateCell(80); cellFecnac.SetCellValue(beneficiario.APO_FEC_NAC == null ? String.Empty :
                            beneficiario.APO_FEC_NAC.Value.ToShortDateString());
                        var cellCalleapo = row.CreateCell(81); cellCalleapo.SetCellValue(beneficiario.APO_CALLE);
                        var cellNroapo = row.CreateCell(82); cellNroapo.SetCellValue(beneficiario.APO_NRO);
                        var cellPisoapo = row.CreateCell(83); cellPisoapo.SetCellValue(beneficiario.APO_PISO);
                        var cellMonoblockapo = row.CreateCell(84); cellMonoblockapo.SetCellValue(beneficiario.APO_MONOBLOCK);
                        var cellDptoapo = row.CreateCell(85); cellDptoapo.SetCellValue(beneficiario.APO_DPTO);
                        var cellParcelaapo = row.CreateCell(86); cellParcelaapo.SetCellValue(beneficiario.APO_PARCELA);
                        var cellManzanaapo = row.CreateCell(87); cellManzanaapo.SetCellValue(beneficiario.APO_MANZANA);
                        var cellEntrecalleapo = row.CreateCell(88); cellEntrecalleapo.SetCellValue(beneficiario.APO_ENTRE_CALLES);
                        var cellBarrioapo = row.CreateCell(89); cellBarrioapo.SetCellValue(beneficiario.APO_BARRIO);
                        var cellCpapo = row.CreateCell(90); cellCpapo.SetCellValue(beneficiario.APO_CP);
                        var cellIdlocalidadapo = row.CreateCell(91); cellIdlocalidadapo.SetCellValue(beneficiario.APO_ID_LOC == null ?
                            String.Empty : beneficiario.APO_ID_LOC.ToString());
                        var cellLocalidadapo = row.CreateCell(92); cellLocalidadapo.SetCellValue(beneficiario.APO_LOCALIDAD);
                        var cellIddeptoapo = row.CreateCell(93); cellIddeptoapo.SetCellValue(beneficiario.APO_ID_DEPTO == null ? String.Empty
                            : beneficiario.APO_ID_DEPTO.ToString());
                        var cellDeptoapo = row.CreateCell(94); cellDeptoapo.SetCellValue(beneficiario.APO_DEPARTAMENTO);
                        var cellTelefonoapo = row.CreateCell(95); cellTelefonoapo.SetCellValue(beneficiario.APO_TELEFONO);
                        var cellCelularapo = row.CreateCell(96); cellCelularapo.SetCellValue(beneficiario.APO_CELULAR);
                        var cellEmailapo = row.CreateCell(97);
                        cellEmailapo.SetCellValue(beneficiario.APO_EMAIL);

                        var cellApoIdEstado = row.CreateCell(98);
                        cellApoIdEstado.SetCellValue(beneficiario.AP_ID_ESTADO == null ? String.Empty
                            : beneficiario.AP_ID_ESTADO.ToString());
                        var cellApoEstado = row.CreateCell(99);
                        cellApoEstado.SetCellValue(beneficiario.AP_N_ESTADO);

                        var cellIdSubprograma = row.CreateCell(100); cellIdSubprograma.SetCellValue(beneficiario.ID_SUBPROGRAMA == null ? "" : beneficiario.ID_SUBPROGRAMA.ToString());
                        var cellSubprograma = row.CreateCell(101); cellSubprograma.SetCellValue(beneficiario.SUBPROGRAMA ?? "");
                    }
                    #endregion

                    break;


            }


            var ms = new MemoryStream();

            templateWorkbook.Write(ms);

            return ms.ToArray();
        }

        public byte[] ExportarFichasProf(string cuil, string dni, string nombre, string apellido, int tipoficha, int estadoficha, int idEtapa)
        {
            var path = String.Empty;


            path = HttpContext.Current.Server.MapPath(@"\Archivos\Exportados\TemplateBeneficiariosPPPP.xls");

            var fs = new FileStream(path, FileMode.Open, FileAccess.Read);

            var templateWorkbook = new HSSFWorkbook(fs, true);

            var sheet = templateWorkbook.GetSheet("Nomina");

            List<VT_REPORTES_PPPPROF> listaparaExcel = null;

            listaparaExcel = _FichaRepo.getfichaProf(cuil, dni, nombre, apellido, tipoficha, estadoficha, idEtapa);

            var i = 1;

            switch (tipoficha)
            {

                case (int)Enums.TipoFicha.PppProf:
                    #region case (int)Enums.TipoFicha.PppProf
                    foreach (var beneficiario in listaparaExcel)
                    {
                        i++;
                        //Ficha
                        var row = sheet.CreateRow(i);
                        var cellidficha = row.CreateCell(0); cellidficha.SetCellValue(beneficiario.ID_FICHA);
                        var cellApellido = row.CreateCell(1); cellApellido.SetCellValue(beneficiario.APELLIDO);
                        var cellNombre = row.CreateCell(2); cellNombre.SetCellValue(beneficiario.NOMBRE);
                        var cellCuil = row.CreateCell(3); cellCuil.SetCellValue(beneficiario.CUIL);
                        var cellDni = row.CreateCell(4); cellDni.SetCellValue(beneficiario.NUMERO_DOCUMENTO);
                        var cellFechaNa = row.CreateCell(5); cellFechaNa.SetCellValue(beneficiario.FER_NAC == null ? String.Empty
                            : beneficiario.FER_NAC.ToShortDateString());
                        var cellCalle = row.CreateCell(6); cellCalle.SetCellValue(beneficiario.CALLE);
                        var cellNro = row.CreateCell(7); cellNro.SetCellValue(beneficiario.NUMERO);
                        var cellPiso = row.CreateCell(8); cellPiso.SetCellValue(beneficiario.PISO);
                        var cellDpto = row.CreateCell(9); cellDpto.SetCellValue(beneficiario.DPTO);
                        var cellMono = row.CreateCell(10); cellMono.SetCellValue(beneficiario.MONOBLOCK);
                        var cellParcela = row.CreateCell(11); cellParcela.SetCellValue(beneficiario.PARCELA);
                        var cellManzana = row.CreateCell(12); cellManzana.SetCellValue(beneficiario.MANZANA);
                        var cellEntreCalle = row.CreateCell(13); cellEntreCalle.SetCellValue(beneficiario.ENTRECALLES);
                        var cellBarrio = row.CreateCell(14); cellBarrio.SetCellValue(beneficiario.BARRIO);
                        var cellCp = row.CreateCell(15); cellCp.SetCellValue(beneficiario.CODIGO_POSTAL);
                        var cellidLocalidad = row.CreateCell(16); cellidLocalidad.SetCellValue(beneficiario.ID_LOCALIDAD == null ? String.Empty
                            : beneficiario.ID_LOCALIDAD.ToString());
                        var cellnLocalidad = row.CreateCell(17); cellnLocalidad.SetCellValue(beneficiario.N_LOCALIDAD);
                        var cellIdDepartamento = row.CreateCell(18); cellIdDepartamento.SetCellValue(beneficiario.ID_DPTO == null ? String.Empty
                            : beneficiario.ID_DPTO.ToString());
                        var cellnDepartamento = row.CreateCell(19); cellnDepartamento.SetCellValue(beneficiario.N_DEPARTAMENTO);
                        var cellTelFijo = row.CreateCell(20); cellTelFijo.SetCellValue(beneficiario.TEL_FIJO);
                        var cellTelCelular = row.CreateCell(21); cellTelCelular.SetCellValue(beneficiario.TEL_CELULAR);
                        var cellContacto = row.CreateCell(22); cellContacto.SetCellValue(beneficiario.CONTACTO);
                        var cellEmail = row.CreateCell(23); cellEmail.SetCellValue(beneficiario.MAIL);

                        var cellTieneHijos = row.CreateCell(24); cellTieneHijos.SetCellValue(beneficiario.TIENE_HIJOS);//TIENE_HIJOS	
                        var cellCantidadHijos = row.CreateCell(25); cellCantidadHijos.SetCellValue(beneficiario.CANTIDAD_HIJOS == null ? "0" : beneficiario.CANTIDAD_HIJOS.ToString());//CANTIDAD_HIJOS
                        var cellEsDiscapacitado = row.CreateCell(26); cellEsDiscapacitado.SetCellValue(beneficiario.ES_DISCAPACITADO == null ? "N" : beneficiario.ES_DISCAPACITADO);//ES_DIACAPACITADO
                        var cellTieneDeficienciaMotora = row.CreateCell(27); cellTieneDeficienciaMotora.SetCellValue(beneficiario.TIENE_DEF_MOTORA == null ? "N" : beneficiario.TIENE_DEF_MOTORA);//TIENE_DEF_MOTORA
                        var cellTieneDeficienciaMental = row.CreateCell(28); cellTieneDeficienciaMental.SetCellValue(beneficiario.TIENE_DEF_MENTAL == null ? "N" : beneficiario.TIENE_DEF_MENTAL);//TIENE_DEF_MENTAL
                        var cellTieneDeficienciaSensorial = row.CreateCell(29); cellTieneDeficienciaSensorial.SetCellValue(beneficiario.TIENE_DEF_SENSORIAL == null ? "N" : beneficiario.TIENE_DEF_SENSORIAL);//TIENE_DEF_SENSORIAL
                        var cellTieneDeficienciaPsicologia = row.CreateCell(30); cellTieneDeficienciaPsicologia.SetCellValue(beneficiario.TIENE_DEF_PSICOLOGICA == null ? "N" : beneficiario.TIENE_DEF_PSICOLOGICA);//TIENE_DEF_PSICOLOGICA
                        var cellDeficienciaOtra = row.CreateCell(31); cellDeficienciaOtra.SetCellValue(beneficiario.DEFICIENCIA_OTRA == null ? "N" : beneficiario.TIENE_DEF_PSICOLOGICA);//DEFICIENCIA_OTRA
                        var cellCertificadoDiscapacidad = row.CreateCell(32); cellCertificadoDiscapacidad.SetCellValue(beneficiario.CERTIF_DISCAP == null ? "N" : beneficiario.CERTIF_DISCAP);//CERTIF_DISCAP

                        var cellSexo = row.CreateCell(33); cellSexo.SetCellValue(beneficiario.SEXO);
                        var cellidestadoFicha = row.CreateCell(34); cellidestadoFicha.SetCellValue(beneficiario.ID_EST_FIC == null ? String.Empty
                            : beneficiario.ID_EST_FIC.ToString());
                        var cellnestadoFicha = row.CreateCell(35); cellnestadoFicha.SetCellValue(beneficiario.N_ESTADO_FICHA);
                        var cellModalidad = row.CreateCell(36); cellModalidad.SetCellValue(beneficiario.MODALIDAD == (int)Enums.Modalidad.Entrenamiento
                            ? "Entrenamiento" : "CTI");
                        var cellTarea = row.CreateCell(37); cellTarea.SetCellValue(beneficiario.TAREAS);
                        var cellaltaTemprana = row.CreateCell(38); cellaltaTemprana.SetCellValue(beneficiario.ALTA_TEMPRANA);
                        var cellatFechaInicio = row.CreateCell(39); cellatFechaInicio.SetCellValue(beneficiario.AT_FECHA_INICIO == null ?
                            String.Empty : beneficiario.AT_FECHA_INICIO.Value.ToShortDateString());
                        var cellatFechaCese = row.CreateCell(40); cellatFechaCese.SetCellValue(beneficiario.AT_FECHA_CESE == null ?
                            String.Empty : beneficiario.AT_FECHA_CESE.Value.ToShortDateString());
                        var cellidModContAfip = row.CreateCell(41); cellidModContAfip.SetCellValue(beneficiario.ID_MOD_CONT_AFIP == null ?
                            String.Empty : beneficiario.ID_MOD_CONT_AFIP.ToString());
                        var cellModContAfip = row.CreateCell(42); cellModContAfip.SetCellValue(beneficiario.MOD_CONT_AFIP);
                        var cellFechaModif = row.CreateCell(43); cellFechaModif.SetCellValue(beneficiario.FEC_SIST == null ? String.Empty
                            : beneficiario.FEC_SIST.Value.ToShortDateString());

                        //Empresa
                        var cellidEmp = row.CreateCell(44); cellidEmp.SetCellValue(beneficiario.ID_EMP == null ? "" : beneficiario.ID_EMP.ToString());
                        var cellrazonSocial = row.CreateCell(45); cellrazonSocial.SetCellValue(beneficiario.RAZON_SOCIAL);
                        var cellCuit = row.CreateCell(46); cellCuit.SetCellValue(beneficiario.EMP_CUIT);
                        var cellCodact = row.CreateCell(47); cellCodact.SetCellValue(beneficiario.COD_ACT);
                        var cellCantemp = row.CreateCell(48); cellCantemp.SetCellValue(beneficiario.CANT_EMP == null ?
                            String.Empty : beneficiario.CANT_EMP.ToString());
                        var cellCalleemp = row.CreateCell(49); cellCalleemp.SetCellValue(beneficiario.EMP_CALLE);
                        var cellNumeroemp = row.CreateCell(50); cellNumeroemp.SetCellValue(beneficiario.EMP_NUMERO);
                        var cellPisoemp = row.CreateCell(51); cellPisoemp.SetCellValue(beneficiario.EMP_PISO);
                        var cellDptoemp = row.CreateCell(52); cellDptoemp.SetCellValue(beneficiario.EMP_DPTO);
                        var cellCpemp = row.CreateCell(53); cellCpemp.SetCellValue(beneficiario.EMP_CP);
                        var cellIdlocemp = row.CreateCell(54); cellIdlocemp.SetCellValue(beneficiario.ID_LOC == null ? String.Empty
                            : beneficiario.ID_LOC.ToString());
                        var cellLocemp = row.CreateCell(55); cellLocemp.SetCellValue(beneficiario.EMP_N_LOCALIDAD);
                        var cellDepartEmp = row.CreateCell(56); cellDepartEmp.SetCellValue(beneficiario.ID_LOC == null ? String.Empty
                            : beneficiario.EMP_N_DEPARTAMENTO);
                        var cellIdusuarioemp = row.CreateCell(57); cellIdusuarioemp.SetCellValue(beneficiario.EU_ID_USUARIO == null ?
                            String.Empty : beneficiario.EU_ID_USUARIO.ToString());
                        var cellNombreusuemp = row.CreateCell(58); cellNombreusuemp.SetCellValue(beneficiario.EU_NOMBRE);
                        var cellApellidousuemp = row.CreateCell(59); cellApellidousuemp.SetCellValue(beneficiario.EMP_APELLIDO);
                        var cellEmailusuemp = row.CreateCell(60); cellEmailusuemp.SetCellValue(beneficiario.EU_MAIL);
                        var cellTeleusuemp = row.CreateCell(61); cellTeleusuemp.SetCellValue(beneficiario.EU_TELEFONO);

                        //Beneficiario
                        var cellIdben = row.CreateCell(62); cellIdben.SetCellValue(beneficiario.ID_BENEFICIARIO.ToString());
                        var cellIdestadoben = row.CreateCell(63); cellIdestadoben.SetCellValue(beneficiario.BEN_ID_ESTADO.ToString());
                        var cellEstadoben = row.CreateCell(64); cellEstadoben.SetCellValue(beneficiario.BEN_N_ESTADO);
                        var cellNrocuentaben = row.CreateCell(65); cellNrocuentaben.SetCellValue(beneficiario.BEN_NRO_CTA == null ? String.Empty
                            : beneficiario.BEN_NRO_CTA.ToString());
                        var cellIdsucursalben = row.CreateCell(66); cellIdsucursalben.SetCellValue(beneficiario.BEN_ID_SUCURSAL == null ? String.Empty
                            : beneficiario.BEN_ID_SUCURSAL.ToString());
                        var cellCodsucursalben = row.CreateCell(67); cellCodsucursalben.SetCellValue(beneficiario.BEN_COD_SUC);
                        var cellSucursalben = row.CreateCell(68); cellSucursalben.SetCellValue(beneficiario.BEN_SUCURSAL);
                        var cellFecsolben = row.CreateCell(69); cellFecsolben.SetCellValue(beneficiario.BEN_FEC_SOL_CTA == null ? String.Empty
                            : beneficiario.BEN_FEC_SOL_CTA.Value.ToShortDateString());
                        var cellNotificado = row.CreateCell(70); cellNotificado.SetCellValue(beneficiario.NOTIFICADO);
                        var cellFecNotif = row.CreateCell(71); cellFecNotif.SetCellValue(beneficiario.FEC_NOTIF == null ? String.Empty
                            : beneficiario.FEC_NOTIF.Value.ToShortDateString());

                        var cellFechaIniBenf = row.CreateCell(72); cellFechaIniBenf.SetCellValue(beneficiario.BEN_FEC_INICIO == null ? string.Empty : beneficiario.BEN_FEC_INICIO.Value.ToShortDateString()); // 20/03/2013 - DI CAMPLI LEANDRO
                        var cellBajaBenef = row.CreateCell(73); cellBajaBenef.SetCellValue(beneficiario.BEN_FEC_INICIO == null ? string.Empty : beneficiario.BEN_FEC_INICIO.Value.ToShortDateString()); // 20/03/2013 - DI CAMPLI LEANDRO

                        var cellTieneapoben = row.CreateCell(74); cellTieneapoben.SetCellValue(beneficiario.TIENE_APODERADO);

                        //Apoderado
                        var cellIdapoderado = row.CreateCell(75); cellIdapoderado.SetCellValue(beneficiario.ID_APODERADO == null ? String.Empty
                            : beneficiario.ID_APODERADO.ToString());
                        var cellApellidoapo = row.CreateCell(76); cellApellidoapo.SetCellValue(beneficiario.APO_APELLIDO);
                        var cellNombreapo = row.CreateCell(78); cellNombreapo.SetCellValue(beneficiario.APO_NOMBRE);
                        var cellDniapo = row.CreateCell(79); cellDniapo.SetCellValue(beneficiario.APO_DNI);
                        var cellCuilapo = row.CreateCell(80); cellCuilapo.SetCellValue(beneficiario.APO_CUIL);
                        var cellSexoapo = row.CreateCell(81); cellSexoapo.SetCellValue(beneficiario.APO_SEXO);
                        var cellNroctaapo = row.CreateCell(82); cellNroctaapo.SetCellValue(beneficiario.APO_NRO_CTA == null ? String.Empty
                            : beneficiario.APO_NRO_CTA.ToString());
                        var cellIdSuc = row.CreateCell(83); cellIdSuc.SetCellValue(beneficiario.APO_ID_SUC == null ? String.Empty
                            : beneficiario.APO_ID_SUC.ToString());
                        var cellCodsucapo = row.CreateCell(84); cellCodsucapo.SetCellValue(beneficiario.APO_COD_SUC);
                        var cellSucursalapo = row.CreateCell(85); cellSucursalapo.SetCellValue(beneficiario.APO_SUCURSAL);
                        var cellFecsolcuentaapo = row.CreateCell(86); cellFecsolcuentaapo.SetCellValue(beneficiario.APO_FEC_SOL_CTA == null ?
                            String.Empty : beneficiario.APO_FEC_SOL_CTA.Value.ToShortDateString());
                        var cellFecnac = row.CreateCell(87); cellFecnac.SetCellValue(beneficiario.APO_FEC_NAC == null ? String.Empty :
                            beneficiario.APO_FEC_NAC.Value.ToShortDateString());
                        var cellCalleapo = row.CreateCell(88); cellCalleapo.SetCellValue(beneficiario.APO_CALLE);
                        var cellNroapo = row.CreateCell(89); cellNroapo.SetCellValue(beneficiario.APO_NRO);
                        var cellPisoapo = row.CreateCell(90); cellPisoapo.SetCellValue(beneficiario.APO_PISO);
                        var cellMonoblockapo = row.CreateCell(91); cellMonoblockapo.SetCellValue(beneficiario.APO_MONOBLOCK);
                        var cellDptoapo = row.CreateCell(92); cellDptoapo.SetCellValue(beneficiario.APO_DPTO);
                        var cellParcelaapo = row.CreateCell(93); cellParcelaapo.SetCellValue(beneficiario.APO_PARCELA);
                        var cellManzanaapo = row.CreateCell(94); cellManzanaapo.SetCellValue(beneficiario.APO_MANZANA);
                        var cellEntrecalleapo = row.CreateCell(95); cellEntrecalleapo.SetCellValue(beneficiario.APO_ENTRE_CALLES);
                        var cellBarrioapo = row.CreateCell(96); cellBarrioapo.SetCellValue(beneficiario.APO_BARRIO);
                        var cellCpapo = row.CreateCell(97); cellCpapo.SetCellValue(beneficiario.APO_CP);
                        var cellIdlocalidadapo = row.CreateCell(98); cellIdlocalidadapo.SetCellValue(beneficiario.APO_ID_LOC == null ? String.Empty
                            : beneficiario.APO_ID_LOC.ToString());
                        var cellLocalidadapo = row.CreateCell(99); cellLocalidadapo.SetCellValue(beneficiario.APO_LOCALIDAD);
                        var cellIddeptoapo = row.CreateCell(100); cellIddeptoapo.SetCellValue(beneficiario.APO_ID_LOC == null ? String.Empty
                            : beneficiario.APO_ID_DEPTO.ToString());
                        var cellDeptoapo = row.CreateCell(101); cellDeptoapo.SetCellValue(beneficiario.APO_DEPARTAMENTO);
                        var cellTelefonoapo = row.CreateCell(102); cellTelefonoapo.SetCellValue(beneficiario.APO_TELEFONO);
                        var cellCelularapo = row.CreateCell(103); cellCelularapo.SetCellValue(beneficiario.APO_CELULAR);
                        var cellEmailapo = row.CreateCell(104);
                        cellEmailapo.SetCellValue(beneficiario.APO_EMAIL);

                        var cellApoIdEstado = row.CreateCell(105);
                        cellApoIdEstado.SetCellValue(beneficiario.AP_ID_ESTADO == null ? String.Empty
                            : beneficiario.AP_ID_ESTADO.ToString());
                        var cellApoEstado = row.CreateCell(106);
                        cellApoEstado.SetCellValue(beneficiario.AP_N_ESTADO);
                    }
                    #endregion
                    break;


            }


            var ms = new MemoryStream();

            templateWorkbook.Write(ms);

            return ms.ToArray();
        }

        public byte[] ExportarFichasVat(string cuil, string dni, string nombre, string apellido, int tipoficha, int estadoficha, int idEtapa)
        {
            var path = String.Empty;


            path = HttpContext.Current.Server.MapPath(@"\Archivos\Exportados\TemplateBeneficiariosVAT.xls");

            var fs = new FileStream(path, FileMode.Open, FileAccess.Read);

            var templateWorkbook = new HSSFWorkbook(fs, true);

            var sheet = templateWorkbook.GetSheet("Nomina");

            List<VT_REPORTES_VAT> listaparaExcel = null;

            listaparaExcel = _FichaRepo.getfichaVat(cuil, dni, nombre, apellido, tipoficha, estadoficha, idEtapa);

            var i = 1;

            switch (tipoficha)
            {

                case (int)Enums.TipoFicha.Vat:
                    #region case (int)Enums.TipoFicha.Vat
                    foreach (var beneficiario in listaparaExcel)
                    {
                        i++;
                        //Ficha
                        var row = sheet.CreateRow(i);
                        var cellidficha = row.CreateCell(0); cellidficha.SetCellValue(beneficiario.ID_FICHA);
                        var cellApellido = row.CreateCell(1); cellApellido.SetCellValue(beneficiario.APELLIDO);
                        var cellNombre = row.CreateCell(2); cellNombre.SetCellValue(beneficiario.NOMBRE);
                        var cellCuil = row.CreateCell(3); cellCuil.SetCellValue(beneficiario.CUIL);
                        var cellDni = row.CreateCell(4); cellDni.SetCellValue(beneficiario.NUMERO_DOCUMENTO);
                        var cellFechaNa = row.CreateCell(5); cellFechaNa.SetCellValue(beneficiario.FER_NAC == null ? String.Empty
                            : beneficiario.FER_NAC.ToShortDateString());
                        var cellCalle = row.CreateCell(6); cellCalle.SetCellValue(beneficiario.CALLE);
                        var cellNro = row.CreateCell(7); cellNro.SetCellValue(beneficiario.NUMERO);
                        var cellPiso = row.CreateCell(8); cellPiso.SetCellValue(beneficiario.PISO);
                        var cellDpto = row.CreateCell(9); cellDpto.SetCellValue(beneficiario.DPTO);
                        var cellMono = row.CreateCell(10); cellMono.SetCellValue(beneficiario.MONOBLOCK);
                        var cellParcela = row.CreateCell(11); cellParcela.SetCellValue(beneficiario.PARCELA);
                        var cellManzana = row.CreateCell(12); cellManzana.SetCellValue(beneficiario.MANZANA);
                        var cellEntreCalle = row.CreateCell(13); cellEntreCalle.SetCellValue(beneficiario.ENTRECALLES);
                        var cellBarrio = row.CreateCell(14); cellBarrio.SetCellValue(beneficiario.BARRIO);
                        var cellCp = row.CreateCell(15); cellCp.SetCellValue(beneficiario.CODIGO_POSTAL);
                        var cellidLocalidad = row.CreateCell(16); cellidLocalidad.SetCellValue(beneficiario.ID_LOCALIDAD == null ?
                            String.Empty : beneficiario.ID_LOCALIDAD.ToString());
                        var cellnLocalidad = row.CreateCell(17); cellnLocalidad.SetCellValue(beneficiario.N_LOCALIDAD);
                        var cellIdDepartamento = row.CreateCell(18); cellIdDepartamento.SetCellValue(beneficiario.ID_DPTO == null ?
                            String.Empty : beneficiario.ID_DPTO.ToString());
                        var cellnDepartamento = row.CreateCell(19); cellnDepartamento.SetCellValue(beneficiario.N_DEPARTAMENTO);
                        var cellTelFijo = row.CreateCell(20); cellTelFijo.SetCellValue(beneficiario.TEL_FIJO);
                        var cellTelCelular = row.CreateCell(21); cellTelCelular.SetCellValue(beneficiario.TEL_CELULAR);
                        var cellContacto = row.CreateCell(22); cellContacto.SetCellValue(beneficiario.CONTACTO);
                        var cellEmail = row.CreateCell(23); cellEmail.SetCellValue(beneficiario.MAIL);


                        var cellTieneHijos = row.CreateCell(24); cellTieneHijos.SetCellValue(beneficiario.TIENE_HIJOS);//TIENE_HIJOS	
                        var cellCantidadHijos = row.CreateCell(25); cellCantidadHijos.SetCellValue(beneficiario.CANTIDAD_HIJOS == null ? "0" : beneficiario.CANTIDAD_HIJOS.ToString());//CANTIDAD_HIJOS
                        var cellEsDiscapacitado = row.CreateCell(26); cellEsDiscapacitado.SetCellValue(beneficiario.ES_DISCAPACITADO == null ? "N" : beneficiario.ES_DISCAPACITADO);//ES_DIACAPACITADO
                        var cellTieneDeficienciaMotora = row.CreateCell(27); cellTieneDeficienciaMotora.SetCellValue(beneficiario.TIENE_DEF_MOTORA == null ? "N" : beneficiario.TIENE_DEF_MOTORA);//TIENE_DEF_MOTORA
                        var cellTieneDeficienciaMental = row.CreateCell(28); cellTieneDeficienciaMental.SetCellValue(beneficiario.TIENE_DEF_MENTAL == null ? "N" : beneficiario.TIENE_DEF_MENTAL);//TIENE_DEF_MENTAL
                        var cellTieneDeficienciaSensorial = row.CreateCell(29); cellTieneDeficienciaSensorial.SetCellValue(beneficiario.TIENE_DEF_SENSORIAL == null ? "N" : beneficiario.TIENE_DEF_SENSORIAL);//TIENE_DEF_SENSORIAL
                        var cellTieneDeficienciaPsicologia = row.CreateCell(30); cellTieneDeficienciaPsicologia.SetCellValue(beneficiario.TIENE_DEF_PSICOLOGICA == null ? "N" : beneficiario.TIENE_DEF_PSICOLOGICA);//TIENE_DEF_PSICOLOGICA
                        var cellDeficienciaOtra = row.CreateCell(31); cellDeficienciaOtra.SetCellValue(beneficiario.DEFICIENCIA_OTRA == null ? "N" : beneficiario.TIENE_DEF_PSICOLOGICA);//DEFICIENCIA_OTRA
                        var cellCertificadoDiscapacidad = row.CreateCell(32); cellCertificadoDiscapacidad.SetCellValue(beneficiario.CERTIF_DISCAP == null ? "N" : beneficiario.CERTIF_DISCAP);//CERTIF_DISCAP


                        var cellSexo = row.CreateCell(33); cellSexo.SetCellValue(beneficiario.SEXO);

                        var cellidestadoFicha = row.CreateCell(34); cellidestadoFicha.SetCellValue(beneficiario.ID_EST_FIC == null ? String.Empty :
                            beneficiario.ID_EST_FIC.ToString());
                        var cellnestadoFicha = row.CreateCell(35); cellnestadoFicha.SetCellValue(beneficiario.N_ESTADO_FICHA);

                        var cellNivelEscolaridad = row.CreateCell(36); cellNivelEscolaridad.SetCellValue(beneficiario.NIVEL_ESCOLARIDAD);

                        var cellModalidad = row.CreateCell(37); cellModalidad.SetCellValue(beneficiario.MODALIDAD == (int)Enums.Modalidad.Entrenamiento
                            ? "Entrenamiento" : "CTI");
                        var cellTarea = row.CreateCell(38); cellTarea.SetCellValue(beneficiario.TAREAS);
                        var cellaltaTemprana = row.CreateCell(39); cellaltaTemprana.SetCellValue(beneficiario.ALTA_TEMPRANA);
                        var cellatFechaInicio = row.CreateCell(40); cellatFechaInicio.SetCellValue(beneficiario.AT_FECHA_INICIO == null ?
                            String.Empty : beneficiario.AT_FECHA_INICIO.Value.ToShortDateString());
                        var cellatFechaCese = row.CreateCell(41); cellatFechaCese.SetCellValue(beneficiario.AT_FECHA_CESE == null ?
                            String.Empty : beneficiario.AT_FECHA_CESE.Value.ToShortDateString());
                        var cellidModContAfip = row.CreateCell(42); cellidModContAfip.SetCellValue(beneficiario.ID_MOD_CONT_AFIP == null ?
                            String.Empty : beneficiario.ID_MOD_CONT_AFIP.ToString());
                        var cellModContAfip = row.CreateCell(43); cellModContAfip.SetCellValue(beneficiario.MOD_CONT_AFIP);

                        var cellFechaModif = row.CreateCell(44); cellFechaModif.SetCellValue(beneficiario.FEC_MODIF == null ? String.Empty
                            : beneficiario.FEC_MODIF.Value.ToShortDateString());

                        ////Empresa
                        var cellidEmp = row.CreateCell(45); cellidEmp.SetCellValue(beneficiario.ID_EMP == null ? String.Empty : beneficiario.ID_EMP.Value.ToString());
                        var cellrazonSocial = row.CreateCell(46); cellrazonSocial.SetCellValue(beneficiario.RAZON_SOCIAL);
                        var cellCuit = row.CreateCell(47); cellCuit.SetCellValue(beneficiario.EMP_CUIT);
                        var cellCodact = row.CreateCell(8); cellCodact.SetCellValue(beneficiario.COD_ACT);
                        var cellCantemp = row.CreateCell(49); cellCantemp.SetCellValue(beneficiario.CANT_EMP == null ?
                            String.Empty : beneficiario.CANT_EMP.ToString());
                        var cellCalleemp = row.CreateCell(50); cellCalleemp.SetCellValue(beneficiario.EMP_CALLE);
                        var cellNumeroemp = row.CreateCell(51); cellNumeroemp.SetCellValue(beneficiario.EMP_NUMERO);
                        var cellPisoemp = row.CreateCell(52); cellPisoemp.SetCellValue(beneficiario.EMP_PISO);
                        var cellDptoemp = row.CreateCell(53); cellDptoemp.SetCellValue(beneficiario.EMP_DPTO);
                        var cellCpemp = row.CreateCell(54); cellCpemp.SetCellValue(beneficiario.EMP_CP);
                        var cellIdlocemp = row.CreateCell(55); cellIdlocemp.SetCellValue(beneficiario.ID_LOC == null ? String.Empty : beneficiario.ID_LOC.ToString());
                        var cellLocemp = row.CreateCell(56); cellLocemp.SetCellValue(beneficiario.EMP_N_LOCALIDAD);
                        var cellDepartamentoEmp = row.CreateCell(57); cellDepartamentoEmp.SetCellValue(beneficiario.ID_LOC == null ? String.Empty : beneficiario.EMP_N_DEPARTAMENTO);
                        var cellIdusuarioemp = row.CreateCell(58); cellIdusuarioemp.SetCellValue(beneficiario.EU_ID_USUARIO == null ? String.Empty : beneficiario.EU_ID_USUARIO.ToString());
                        var cellNombreusuemp = row.CreateCell(59); cellNombreusuemp.SetCellValue(beneficiario.EU_NOMBRE);
                        var cellApellidousuemp = row.CreateCell(60); cellApellidousuemp.SetCellValue(beneficiario.EMP_APELLIDO);
                        var cellEmailusuemp = row.CreateCell(61); cellEmailusuemp.SetCellValue(beneficiario.EU_MAIL);
                        var cellTeleusuemp = row.CreateCell(62); cellTeleusuemp.SetCellValue(beneficiario.EU_TELEFONO);

                        //Beneficiario
                        var cellIdben = row.CreateCell(63); cellIdben.SetCellValue(beneficiario.ID_BENEFICIARIO == null ? string.Empty : beneficiario.ID_BENEFICIARIO.Value.ToString());
                        var cellIdestadoben = row.CreateCell(64); cellIdestadoben.SetCellValue(beneficiario.BEN_ID_ESTADO == null ? string.Empty : beneficiario.BEN_ID_ESTADO.Value.ToString());
                        var cellEstadoben = row.CreateCell(65); cellEstadoben.SetCellValue(beneficiario.BEN_N_ESTADO);
                        var cellNrocuentaben = row.CreateCell(66); cellNrocuentaben.SetCellValue(beneficiario.BEN_NRO_CTA == null ? String.Empty : beneficiario.BEN_NRO_CTA.ToString());
                        var cellIdsucursalben = row.CreateCell(67); cellIdsucursalben.SetCellValue(beneficiario.BEN_ID_SUCURSAL == null ? String.Empty : beneficiario.BEN_ID_SUCURSAL.ToString());
                        var cellCodsucursalben = row.CreateCell(68); cellCodsucursalben.SetCellValue(beneficiario.BEN_COD_SUC);
                        var cellSucursalben = row.CreateCell(69); cellSucursalben.SetCellValue(beneficiario.BEN_SUCURSAL);
                        var cellFecsolben = row.CreateCell(70); cellFecsolben.SetCellValue(beneficiario.BEN_FEC_SOL_CTA == null ? String.Empty
                            : beneficiario.BEN_FEC_SOL_CTA.Value.ToShortDateString());
                        var cellNotificado = row.CreateCell(71); cellNotificado.SetCellValue(beneficiario.NOTIFICADO == "S" ? "S" : "N");
                        var cellFecNotif = row.CreateCell(72); cellFecNotif.SetCellValue(beneficiario.FEC_NOTIF == null ? String.Empty
                            : beneficiario.FEC_NOTIF.Value.ToShortDateString());

                        var cellFechaIniBenf = row.CreateCell(73); cellFechaIniBenf.SetCellValue(beneficiario.BEN_FEC_INICIO == null ? string.Empty : beneficiario.BEN_FEC_INICIO.Value.ToShortDateString()); // 20/03/2013 - DI CAMPLI LEANDRO
                        var cellBajaBenef = row.CreateCell(74); cellBajaBenef.SetCellValue(beneficiario.BEN_FEC_BAJA == null ? string.Empty : beneficiario.BEN_FEC_BAJA.Value.ToShortDateString()); // 20/03/2013 - DI CAMPLI LEANDRO

                        var cellTieneapoben = row.CreateCell(75); cellTieneapoben.SetCellValue(beneficiario.TIENE_APODERADO);

                        //Apoderado
                        var cellIdapoderado = row.CreateCell(76); cellIdapoderado.SetCellValue(beneficiario.ID_APODERADO == null ?
                            String.Empty : beneficiario.ID_APODERADO.ToString());
                        var cellApellidoapo = row.CreateCell(77); cellApellidoapo.SetCellValue(beneficiario.APO_APELLIDO);
                        var cellNombreapo = row.CreateCell(78); cellNombreapo.SetCellValue(beneficiario.APO_NOMBRE);
                        var cellDniapo = row.CreateCell(79); cellDniapo.SetCellValue(beneficiario.APO_DNI);
                        var cellCuilapo = row.CreateCell(80); cellCuilapo.SetCellValue(beneficiario.APO_CUIL);
                        var cellSexoapo = row.CreateCell(81); cellSexoapo.SetCellValue(beneficiario.APO_SEXO);
                        var cellNroctaapo = row.CreateCell(82); cellNroctaapo.SetCellValue(beneficiario.APO_NRO_CTA == null ? String.Empty : beneficiario.APO_NRO_CTA.ToString());
                        var cellIdSuc = row.CreateCell(83); cellIdSuc.SetCellValue(beneficiario.APO_ID_SUC == null ? String.Empty : beneficiario.APO_ID_SUC.ToString());
                        var cellCodsucapo = row.CreateCell(84); cellCodsucapo.SetCellValue(beneficiario.APO_COD_SUC);
                        var cellSucursalapo = row.CreateCell(85); cellSucursalapo.SetCellValue(beneficiario.APO_SUCURSAL);
                        var cellFecsolcuentaapo = row.CreateCell(86); cellFecsolcuentaapo.SetCellValue(beneficiario.APO_FEC_SOL_CTA == null ? String.Empty : beneficiario.APO_FEC_SOL_CTA.Value.ToShortDateString());
                        var cellFecnac = row.CreateCell(87); cellFecnac.SetCellValue(beneficiario.APO_FEC_NAC == null ? String.Empty : beneficiario.APO_FEC_NAC.Value.ToShortDateString());
                        var cellCalleapo = row.CreateCell(88); cellCalleapo.SetCellValue(beneficiario.APO_CALLE);
                        var cellNroapo = row.CreateCell(89); cellNroapo.SetCellValue(beneficiario.APO_NRO);
                        var cellPisoapo = row.CreateCell(90); cellPisoapo.SetCellValue(beneficiario.APO_PISO);
                        var cellMonoblockapo = row.CreateCell(91); cellMonoblockapo.SetCellValue(beneficiario.APO_MONOBLOCK);
                        var cellDptoapo = row.CreateCell(92); cellDptoapo.SetCellValue(beneficiario.APO_DPTO);
                        var cellParcelaapo = row.CreateCell(93); cellParcelaapo.SetCellValue(beneficiario.APO_PARCELA);
                        var cellManzanaapo = row.CreateCell(94); cellManzanaapo.SetCellValue(beneficiario.APO_MANZANA);
                        var cellEntrecalleapo = row.CreateCell(95); cellEntrecalleapo.SetCellValue(beneficiario.APO_ENTRE_CALLES);
                        var cellBarrioapo = row.CreateCell(96); cellBarrioapo.SetCellValue(beneficiario.APO_BARRIO);
                        var cellCpapo = row.CreateCell(97); cellCpapo.SetCellValue(beneficiario.APO_CP);
                        var cellIdlocalidadapo = row.CreateCell(98); cellIdlocalidadapo.SetCellValue(beneficiario.APO_ID_LOC == null ? String.Empty
                            : beneficiario.APO_ID_LOC.ToString());
                        var cellLocalidadapo = row.CreateCell(99); cellLocalidadapo.SetCellValue(beneficiario.APO_LOCALIDAD);
                        var cellIddeptoapo = row.CreateCell(100); cellIddeptoapo.SetCellValue(beneficiario.APO_ID_LOC == null ? String.Empty
                            : beneficiario.APO_ID_DEPTO.ToString());
                        var cellDeptoapo = row.CreateCell(101); cellDeptoapo.SetCellValue(beneficiario.APO_DEPARTAMENTO);
                        var cellTelefonoapo = row.CreateCell(102); cellTelefonoapo.SetCellValue(beneficiario.APO_TELEFONO);
                        var cellCelularapo = row.CreateCell(103); cellCelularapo.SetCellValue(beneficiario.APO_CELULAR);
                        var cellEmailapo = row.CreateCell(104);
                        cellEmailapo.SetCellValue(beneficiario.APO_EMAIL);

                        var cellApoIdEstado = row.CreateCell(105);
                        cellApoIdEstado.SetCellValue(beneficiario.AP_ID_ESTADO == null ? String.Empty
                            : beneficiario.AP_ID_ESTADO.ToString());
                        var cellApoEstado = row.CreateCell(106);
                        cellApoEstado.SetCellValue(beneficiario.AP_N_ESTADO);

                    }
                    #endregion
                    break;


            }


            var ms = new MemoryStream();

            templateWorkbook.Write(ms);

            return ms.ToArray();
        }

        public byte[] ExportarFichasReco_Prod(string cuil, string dni, string nombre, string apellido, int tipoficha, int estadoficha, int idEtapa, FichasVista vista)
        {
            var path = String.Empty;


            path = HttpContext.Current.Server.MapPath(@"\Archivos\Exportados\TemplateBeneficiariosReconversion.xls");

            var fs = new FileStream(path, FileMode.Open, FileAccess.Read);

            var templateWorkbook = new HSSFWorkbook(fs, true);

            var sheet = templateWorkbook.GetSheet("Nomina");

            List<VT_REPORTES_REC_PROD> listaparaExcel = null;

            listaparaExcel = _FichaRepo.getfichaRec_Prod(cuil, dni, nombre, apellido, tipoficha, estadoficha, idEtapa);
            if (vista.ProgPorRol == "S")
            {
                       
                        //**********************************************
                        if (vista.idSubprograma > 0) // si se consulta por subprograma
                        {
                            listaparaExcel = listaparaExcel.Where(x => x.ID_SUBPROGRAMA == vista.idSubprograma || x.ID_SUBPROGRAMA==null).ToList();
                        }
                        
                        //**********************************************

            }

            var i = 1;

            switch (tipoficha)
            {

                case (int)Enums.TipoFicha.ReconversionProductiva:
                    #region case (int)Enums.TipoFicha.Reconversion
                    foreach (var beneficiario in listaparaExcel)
                    {
                        i++;
                        //Ficha
                        var row = sheet.CreateRow(i);
                        var cellidficha = row.CreateCell(0); cellidficha.SetCellValue(beneficiario.ID_FICHA);
                        var cellApellido = row.CreateCell(1); cellApellido.SetCellValue(beneficiario.APELLIDO);
                        var cellNombre = row.CreateCell(2); cellNombre.SetCellValue(beneficiario.NOMBRE);
                        var cellCuil = row.CreateCell(3); cellCuil.SetCellValue(beneficiario.CUIL);
                        var cellDni = row.CreateCell(4); cellDni.SetCellValue(beneficiario.NUMERO_DOCUMENTO);
                        var cellFechaNa = row.CreateCell(5); cellFechaNa.SetCellValue(beneficiario.FER_NAC == null ? String.Empty
                            : beneficiario.FER_NAC.ToShortDateString());
                        var cellCalle = row.CreateCell(6); cellCalle.SetCellValue(beneficiario.CALLE);
                        var cellNro = row.CreateCell(7); cellNro.SetCellValue(beneficiario.NUMERO);
                        var cellPiso = row.CreateCell(8); cellPiso.SetCellValue(beneficiario.PISO);
                        var cellDpto = row.CreateCell(9); cellDpto.SetCellValue(beneficiario.DPTO);
                        var cellMono = row.CreateCell(10); cellMono.SetCellValue(beneficiario.MONOBLOCK);
                        var cellParcela = row.CreateCell(11); cellParcela.SetCellValue(beneficiario.PARCELA);
                        var cellManzana = row.CreateCell(12); cellManzana.SetCellValue(beneficiario.MANZANA);
                        var cellEntreCalle = row.CreateCell(13); cellEntreCalle.SetCellValue(beneficiario.ENTRECALLES);
                        var cellBarrio = row.CreateCell(14); cellBarrio.SetCellValue(beneficiario.BARRIO);
                        var cellCp = row.CreateCell(15); cellCp.SetCellValue(beneficiario.CODIGO_POSTAL);
                        var cellidLocalidad = row.CreateCell(16); cellidLocalidad.SetCellValue(beneficiario.ID_LOCALIDAD == null ?
                            String.Empty : beneficiario.ID_LOCALIDAD.ToString());
                        var cellnLocalidad = row.CreateCell(17); cellnLocalidad.SetCellValue(beneficiario.N_LOCALIDAD);
                        var cellIdDepartamento = row.CreateCell(18); cellIdDepartamento.SetCellValue(beneficiario.ID_LOCALIDAD == null ?
                            String.Empty : beneficiario.ID_DPTO.ToString());
                        var cellnDepartamento = row.CreateCell(19); cellnDepartamento.SetCellValue(beneficiario.N_DEPARTAMENTO);
                        var cellTelFijo = row.CreateCell(20); cellTelFijo.SetCellValue(beneficiario.TEL_FIJO);
                        var cellTelCelular = row.CreateCell(21); cellTelCelular.SetCellValue(beneficiario.TEL_CELULAR);
                        var cellContacto = row.CreateCell(22); cellContacto.SetCellValue(beneficiario.CONTACTO);
                        var cellEmail = row.CreateCell(23); cellEmail.SetCellValue(beneficiario.MAIL);

                        var cellTieneHijos = row.CreateCell(24); cellTieneHijos.SetCellValue(beneficiario.TIENE_HIJOS);//TIENE_HIJOS	
                        var cellCantidadHijos = row.CreateCell(25); cellCantidadHijos.SetCellValue(beneficiario.CANTIDAD_HIJOS == null ? "0" : beneficiario.CANTIDAD_HIJOS.ToString());//CANTIDAD_HIJOS
                        var cellEsDiscapacitado = row.CreateCell(26); cellEsDiscapacitado.SetCellValue(beneficiario.ES_DISCAPACITADO == null ? "N" : beneficiario.ES_DISCAPACITADO);//ES_DIACAPACITADO
                        var cellTieneDeficienciaMotora = row.CreateCell(27); cellTieneDeficienciaMotora.SetCellValue(beneficiario.TIENE_DEF_MOTORA == null ? "N" : beneficiario.TIENE_DEF_MOTORA);//TIENE_DEF_MOTORA
                        var cellTieneDeficienciaMental = row.CreateCell(28); cellTieneDeficienciaMental.SetCellValue(beneficiario.TIENE_DEF_MENTAL == null ? "N" : beneficiario.TIENE_DEF_MENTAL);//TIENE_DEF_MENTAL
                        var cellTieneDeficienciaSensorial = row.CreateCell(29); cellTieneDeficienciaSensorial.SetCellValue(beneficiario.TIENE_DEF_SENSORIAL == null ? "N" : beneficiario.TIENE_DEF_SENSORIAL);//TIENE_DEF_SENSORIAL
                        var cellTieneDeficienciaPsicologia = row.CreateCell(30); cellTieneDeficienciaPsicologia.SetCellValue(beneficiario.TIENE_DEF_PSICOLOGICA == null ? "N" : beneficiario.TIENE_DEF_PSICOLOGICA);//TIENE_DEF_PSICOLOGICA
                        var cellDeficienciaOtra = row.CreateCell(31); cellDeficienciaOtra.SetCellValue(beneficiario.DEFICIENCIA_OTRA == null ? "N" : beneficiario.TIENE_DEF_PSICOLOGICA);//DEFICIENCIA_OTRA
                        var cellCertificadoDiscapacidad = row.CreateCell(32); cellCertificadoDiscapacidad.SetCellValue(beneficiario.CERTIF_DISCAP == null ? "N" : beneficiario.CERTIF_DISCAP);//CERTIF_DISCAP

                        var cellSexo = row.CreateCell(33); cellSexo.SetCellValue(beneficiario.SEXO);
                        var cellidestadoFicha = row.CreateCell(34); cellidestadoFicha.SetCellValue(beneficiario.ID_EST_FIC == null ? String.Empty :
                            beneficiario.ID_EST_FIC.ToString());
                        var cellnestadoFicha = row.CreateCell(35); cellnestadoFicha.SetCellValue(beneficiario.N_ESTADO_FICHA);

                        var cellNivelEscolaridad = row.CreateCell(36); cellNivelEscolaridad.SetCellValue(beneficiario.NIVEL_ESCOLARIDAD);

                        var cellModalidad = row.CreateCell(37); cellModalidad.SetCellValue(beneficiario.MODALIDAD == (int)Enums.Modalidad.Entrenamiento
                            ? "Entrenamiento" : "CTI");
                        var cellaltaTemprana = row.CreateCell(38); cellaltaTemprana.SetCellValue(beneficiario.ALTA_TEMPRANA);
                        var cellatFechaInicio = row.CreateCell(39); cellatFechaInicio.SetCellValue(beneficiario.AT_FECHA_INICIO == null ?
                            String.Empty : beneficiario.AT_FECHA_INICIO.Value.ToShortDateString());
                        var cellatFechaCese = row.CreateCell(40); cellatFechaCese.SetCellValue(beneficiario.AT_FECHA_CESE == null ?
                            String.Empty : beneficiario.AT_FECHA_CESE.Value.ToShortDateString());
                        var cellidModContAfip = row.CreateCell(41); cellidModContAfip.SetCellValue(beneficiario.ID_MOD_CONT_AFIP == null ?
                            String.Empty : beneficiario.ID_MOD_CONT_AFIP.ToString());
                        var cellModContAfip = row.CreateCell(42); cellModContAfip.SetCellValue(beneficiario.MOD_CONT_AFIP);

                        var cellFechaModif = row.CreateCell(43); cellFechaModif.SetCellValue(beneficiario.FEC_MODIF == null ? String.Empty
                            : beneficiario.FEC_MODIF.Value.ToShortDateString());

                        var cellnSubprograma = row.CreateCell(44); cellnSubprograma.SetCellValue(beneficiario.SUBPROGRAMA);

                        //Proyecto
                        var cellnProyecto = row.CreateCell(45); cellnProyecto.SetCellValue(beneficiario.N_PROYECTO);
                        var cellCantCapacitar = row.CreateCell(46); cellCantCapacitar.SetCellValue(beneficiario.CANT_A_CAPACITAR == null ? String.Empty : beneficiario.CANT_A_CAPACITAR.ToString());
                        var cellFecInicio = row.CreateCell(47); cellFecInicio.SetCellValue(beneficiario.PRO_FEC_INICIO == null ? String.Empty : beneficiario.PRO_FEC_INICIO.Value.ToShortDateString());
                        var cellFecFin = row.CreateCell(48); cellFecFin.SetCellValue(beneficiario.PRO_FEC_FIN == null ? String.Empty : beneficiario.PRO_FEC_FIN.Value.ToShortDateString());
                        var cellMesesDuracion = row.CreateCell(49); cellMesesDuracion.SetCellValue(beneficiario.MESES_DURACION == null ? String.Empty : beneficiario.MESES_DURACION.ToString());
                        var cellAportesEmpresa = row.CreateCell(50); cellAportesEmpresa.SetCellValue(beneficiario.APORTES_DE_EMPRESA);
                        var cellApeTutor = row.CreateCell(51); cellApeTutor.SetCellValue(beneficiario.APELLIDO_TUTOR);
                        var cellNomTutor = row.CreateCell(52); cellNomTutor.SetCellValue(beneficiario.NOMBRE_TUTOR);
                        var cellDocTutor = row.CreateCell(53); cellDocTutor.SetCellValue(beneficiario.DNI_TUTOR);
                        var cellTelTutor = row.CreateCell(54); cellTelTutor.SetCellValue(beneficiario.TELEFONO_TUTOR);
                        var cellEmailTutor = row.CreateCell(55); cellEmailTutor.SetCellValue(beneficiario.EMAIL_TUTOR);
                        var cellPuestoTutor = row.CreateCell(56); cellPuestoTutor.SetCellValue(beneficiario.PUESTO_TUTOR);

                        //Empresa
                        var cellidEmp = row.CreateCell(57); cellidEmp.SetCellValue(beneficiario.ID_EMP == null ? "" : beneficiario.ID_EMP.ToString());
                        var cellrazonSocial = row.CreateCell(58); cellrazonSocial.SetCellValue(beneficiario.RAZON_SOCIAL);
                        var cellCuit = row.CreateCell(59); cellCuit.SetCellValue(beneficiario.EMP_CUIT);
                        var cellCodact = row.CreateCell(60); cellCodact.SetCellValue(beneficiario.COD_ACT);
                        var cellCantemp = row.CreateCell(61); cellCantemp.SetCellValue(beneficiario.CANT_EMP == null ?
                            String.Empty : beneficiario.CANT_EMP.ToString());
                        var cellCalleemp = row.CreateCell(62); cellCalleemp.SetCellValue(beneficiario.EMP_CALLE);
                        var cellNumeroemp = row.CreateCell(63); cellNumeroemp.SetCellValue(beneficiario.EMP_NUMERO);
                        var cellPisoemp = row.CreateCell(64); cellPisoemp.SetCellValue(beneficiario.EMP_PISO);
                        var cellDptoemp = row.CreateCell(65); cellDptoemp.SetCellValue(beneficiario.EMP_DPTO);
                        var cellCpemp = row.CreateCell(66); cellCpemp.SetCellValue(beneficiario.EMP_CP);
                        var cellIdlocemp = row.CreateCell(67); cellIdlocemp.SetCellValue(beneficiario.ID_LOC == null ?
                            String.Empty : beneficiario.ID_LOC.ToString());
                        var cellLocemp = row.CreateCell(68); cellLocemp.SetCellValue(beneficiario.EMP_N_LOCALIDAD);
                        var cellDepartamentoEmp = row.CreateCell(69); cellDepartamentoEmp.SetCellValue(beneficiario.ID_LOC == null ?
                            String.Empty : beneficiario.EMP_N_DEPARTAMENTO);
                        var cellIdusuarioemp = row.CreateCell(70); cellIdusuarioemp.SetCellValue(beneficiario.EU_ID_USUARIO == null ?
                            String.Empty : beneficiario.EU_ID_USUARIO.ToString());
                        var cellNombreusuemp = row.CreateCell(71); cellNombreusuemp.SetCellValue(beneficiario.EU_NOMBRE);
                        var cellApellidousuemp = row.CreateCell(72); cellApellidousuemp.SetCellValue(beneficiario.EMP_APELLIDO);
                        var cellEmailusuemp = row.CreateCell(73); cellEmailusuemp.SetCellValue(beneficiario.EU_MAIL);
                        var cellTeleusuemp = row.CreateCell(74); cellTeleusuemp.SetCellValue(beneficiario.EU_TELEFONO);

                        //Sede
                        var cellIdSede = row.CreateCell(75); cellIdSede.SetCellValue(beneficiario.ID_SEDE == null ? String.Empty : beneficiario.ID_SEDE.ToString());
                        var cellnSede = row.CreateCell(76); cellnSede.SetCellValue(beneficiario.N_SEDE);
                        var cellCalleSede = row.CreateCell(77); cellCalleSede.SetCellValue(beneficiario.SEDE_CALLE);
                        var cellNroSede = row.CreateCell(78); cellNroSede.SetCellValue(beneficiario.SEDE_NRO);
                        var cellPisoSede = row.CreateCell(79); cellPisoSede.SetCellValue(beneficiario.SEDE_PISO);
                        var cellDptoSede = row.CreateCell(80); cellDptoSede.SetCellValue(beneficiario.SEDE_DPTO);
                        var cellCpSede = row.CreateCell(81); cellCpSede.SetCellValue(beneficiario.SEDE_CP);
                        var cellLocSede = row.CreateCell(82); cellLocSede.SetCellValue(beneficiario.SEDE_LOCALIDAD);
                        var cellApeContacto = row.CreateCell(83); cellApeContacto.SetCellValue(beneficiario.SEDE_APE_CONTACTO);
                        var cellNomContacto = row.CreateCell(84); cellNomContacto.SetCellValue(beneficiario.SEDE_NOM_CONTACTO);
                        var cellTelcontacto = row.CreateCell(85); cellTelcontacto.SetCellValue(beneficiario.SEDE_TELEFONO);
                        var cellFaxContacto = row.CreateCell(86); cellFaxContacto.SetCellValue(beneficiario.SEDE_FAX);
                        var cellEmailContacto = row.CreateCell(87); cellEmailContacto.SetCellValue(beneficiario.SEDE_EMAIL);

                        //Beneficiario
                        var cellIdben = row.CreateCell(88); cellIdben.SetCellValue(beneficiario.ID_BENEFICIARIO == null ? "" : beneficiario.ID_BENEFICIARIO.ToString());
                        var cellIdestadoben = row.CreateCell(89); cellIdestadoben.SetCellValue(beneficiario.BEN_ID_ESTADO == null ? "" : beneficiario.BEN_ID_ESTADO.ToString());
                        var cellEstadoben = row.CreateCell(90); cellEstadoben.SetCellValue(beneficiario.BEN_N_ESTADO);
                        var cellNrocuentaben = row.CreateCell(91); cellNrocuentaben.SetCellValue(beneficiario.BEN_NRO_CTA == null ? String.Empty
                            : beneficiario.BEN_NRO_CTA.ToString());
                        var cellIdsucursalben = row.CreateCell(92); cellIdsucursalben.SetCellValue(beneficiario.BEN_ID_SUCURSAL == null ? String.Empty
                            : beneficiario.BEN_ID_SUCURSAL.ToString());
                        var cellCodsucursalben = row.CreateCell(93); cellCodsucursalben.SetCellValue(beneficiario.BEN_COD_SUC);
                        var cellSucursalben = row.CreateCell(94); cellSucursalben.SetCellValue(beneficiario.BEN_SUCURSAL);
                        var cellFecsolben = row.CreateCell(95); cellFecsolben.SetCellValue(beneficiario.BEN_FEC_SOL_CTA == null ? String.Empty
                            : beneficiario.BEN_FEC_SOL_CTA.Value.ToShortDateString());
                        var cellNotificado = row.CreateCell(96); cellNotificado.SetCellValue(beneficiario.NOTIFICADO);
                        var cellFecNotif = row.CreateCell(97); cellFecNotif.SetCellValue(beneficiario.FEC_NOTIF == null ? String.Empty
                            : beneficiario.FEC_NOTIF.Value.ToShortDateString());

                        var cellFechaIniBenf = row.CreateCell(98); cellFechaIniBenf.SetCellValue(beneficiario.BEN_FEC_INICIO == null ? string.Empty : beneficiario.BEN_FEC_INICIO.Value.ToShortDateString());
                        var cellBajaBenef = row.CreateCell(99); cellBajaBenef.SetCellValue(beneficiario.BEN_FEC_BAJA == null ? string.Empty : beneficiario.BEN_FEC_BAJA.Value.ToShortDateString());

                        var cellTieneapoben = row.CreateCell(100); cellTieneapoben.SetCellValue(beneficiario.TIENE_APODERADO);

                        //Apoderado
                        var cellIdapoderado = row.CreateCell(101); cellIdapoderado.SetCellValue(beneficiario.ID_APODERADO == null ?
                            String.Empty : beneficiario.ID_APODERADO.ToString());
                        var cellApellidoapo = row.CreateCell(102); cellApellidoapo.SetCellValue(beneficiario.APO_APELLIDO);
                        var cellNombreapo = row.CreateCell(103); cellNombreapo.SetCellValue(beneficiario.APO_NOMBRE);
                        var cellDniapo = row.CreateCell(104); cellDniapo.SetCellValue(beneficiario.APO_DNI);
                        var cellCuilapo = row.CreateCell(105); cellCuilapo.SetCellValue(beneficiario.APO_CUIL);
                        var cellSexoapo = row.CreateCell(106); cellSexoapo.SetCellValue(beneficiario.APO_SEXO);
                        var cellNroctaapo = row.CreateCell(107); cellNroctaapo.SetCellValue(beneficiario.APO_NRO_CTA == null ? String.Empty
                            : beneficiario.APO_NRO_CTA.ToString());
                        var cellIdSuc = row.CreateCell(108); cellIdSuc.SetCellValue(beneficiario.APO_ID_SUC == null ? String.Empty
                            : beneficiario.APO_ID_SUC.ToString());
                        var cellCodsucapo = row.CreateCell(109); cellCodsucapo.SetCellValue(beneficiario.APO_COD_SUC);
                        var cellSucursalapo = row.CreateCell(110); cellSucursalapo.SetCellValue(beneficiario.APO_SUCURSAL);
                        var cellFecsolcuentaapo = row.CreateCell(111); cellFecsolcuentaapo.SetCellValue(beneficiario.APO_FEC_SOL_CTA == null ?
                            String.Empty : beneficiario.APO_FEC_SOL_CTA.Value.ToShortDateString());
                        var cellFecnac = row.CreateCell(112); cellFecnac.SetCellValue(beneficiario.APO_FEC_NAC == null ? String.Empty :
                            beneficiario.APO_FEC_NAC.Value.ToShortDateString());
                        var cellCalleapo = row.CreateCell(113); cellCalleapo.SetCellValue(beneficiario.APO_CALLE);
                        var cellNroapo = row.CreateCell(114); cellNroapo.SetCellValue(beneficiario.APO_NRO);
                        var cellPisoapo = row.CreateCell(115); cellPisoapo.SetCellValue(beneficiario.APO_PISO);
                        var cellMonoblockapo = row.CreateCell(116); cellMonoblockapo.SetCellValue(beneficiario.APO_MONOBLOCK);
                        var cellDptoapo = row.CreateCell(117); cellDptoapo.SetCellValue(beneficiario.APO_DPTO);
                        var cellParcelaapo = row.CreateCell(118); cellParcelaapo.SetCellValue(beneficiario.APO_PARCELA);
                        var cellManzanaapo = row.CreateCell(119); cellManzanaapo.SetCellValue(beneficiario.APO_MANZANA);
                        var cellEntrecalleapo = row.CreateCell(120); cellEntrecalleapo.SetCellValue(beneficiario.APO_ENTRE_CALLES);
                        var cellBarrioapo = row.CreateCell(121); cellBarrioapo.SetCellValue(beneficiario.APO_BARRIO);
                        var cellCpapo = row.CreateCell(122); cellCpapo.SetCellValue(beneficiario.APO_CP);
                        var cellIdlocalidadapo = row.CreateCell(123); cellIdlocalidadapo.SetCellValue(beneficiario.APO_ID_LOC == null ? String.Empty
                            : beneficiario.APO_ID_LOC.ToString());
                        var cellLocalidadapo = row.CreateCell(124); cellLocalidadapo.SetCellValue(beneficiario.APO_LOCALIDAD);
                        var cellIddeptoapo = row.CreateCell(125); cellIddeptoapo.SetCellValue(beneficiario.APO_ID_LOC == null ? String.Empty
                            : beneficiario.APO_ID_DEPTO.ToString());
                        var cellDeptoapo = row.CreateCell(126); cellDeptoapo.SetCellValue(beneficiario.APO_DEPARTAMENTO);
                        var cellTelefonoapo = row.CreateCell(127); cellTelefonoapo.SetCellValue(beneficiario.APO_TELEFONO);
                        var cellCelularapo = row.CreateCell(128); cellCelularapo.SetCellValue(beneficiario.APO_CELULAR);
                        var cellEmailapo = row.CreateCell(129);
                        cellEmailapo.SetCellValue(beneficiario.APO_EMAIL);

                        var cellApoIdEstado = row.CreateCell(130);
                        cellApoIdEstado.SetCellValue(beneficiario.AP_ID_ESTADO == null ? String.Empty
                            : beneficiario.AP_ID_ESTADO.ToString());
                        var cellApoEstado = row.CreateCell(131);
                        cellApoEstado.SetCellValue(beneficiario.AP_N_ESTADO);
                    }
                    #endregion
                    break;

            }


            var ms = new MemoryStream();

            templateWorkbook.Write(ms);

            return ms.ToArray();
        }

        public byte[] ExportarFichasEfec_Soc(string cuil, string dni, string nombre, string apellido, int tipoficha, int estadoficha, int idEtapa, FichasVista vista)
        {
            var path = String.Empty;


            path = HttpContext.Current.Server.MapPath(@"\Archivos\Exportados\TemplateBeneficiariosEfectoresSociales.xls");

            var fs = new FileStream(path, FileMode.Open, FileAccess.Read);

            var templateWorkbook = new HSSFWorkbook(fs, true);

            var sheet = templateWorkbook.GetSheet("Nomina");

            List<VT_REPORTES_EFEC_SOC> listaparaExcel = null;

            listaparaExcel = _FichaRepo.getfichaEfec_Soc(cuil, dni, nombre, apellido, tipoficha, estadoficha, idEtapa);
            if (vista.ProgPorRol == "S")
            {
                var usuarioRoles = _usuarioRolRepositorio.GetRolesDelUsuario(base.GetUsuarioLoguer().Id_Usuario);
                //**********************************************
                if (vista.idSubprograma > 0) // si se consulta por subprograma
                {

                        listaparaExcel = listaparaExcel.Where(x => x.ID_SUBPROGRAMA == vista.idSubprograma).ToList();
                    
                }
                else
                {
                    if (tipoficha == (int)Enums.TipoFicha.EfectoresSociales) // para el caso de efectores no debe traer todos
                    {
                        //verifico que subprogramas tiene disponible
                        var lsubprogramas = (from rs in _subprogramaRepositorio.GetSubprogramasRol()
                                             join u in usuarioRoles on rs.Id_Rol equals u.IdRol
                                             where rs.IdPrograma == tipoficha
                                             group rs by new { rs.IdSubprograma, rs.NombreSubprograma } into grouping
                                             select new
                                                 Subprograma
                                             {
                                                 IdSubprograma = grouping.Key.IdSubprograma,
                                                 NombreSubprograma = grouping.Key.NombreSubprograma
                                             }).ToList();


                        listaparaExcel = (from f in listaparaExcel
                                          join sub in lsubprogramas on f.ID_SUBPROGRAMA equals sub.IdSubprograma
                                          where   f.ID_SUBPROGRAMA != null
                                          select f).ToList();
                    }
                }
                //**********************************************
            }
            var i = 1;

            switch (tipoficha)
            {

                case (int)Enums.TipoFicha.EfectoresSociales:
                    #region case (int)Enums.TipoFicha.EfectoresSociales
                    foreach (var beneficiario in listaparaExcel)
                    {
                        i++;
                        //Ficha
                        var row = sheet.CreateRow(i);
                        var cellidficha = row.CreateCell(0); cellidficha.SetCellValue(beneficiario.ID_FICHA);
                        var cellApellido = row.CreateCell(1); cellApellido.SetCellValue(beneficiario.APELLIDO);
                        var cellNombre = row.CreateCell(2); cellNombre.SetCellValue(beneficiario.NOMBRE);
                        var cellCuil = row.CreateCell(3); cellCuil.SetCellValue(beneficiario.CUIL);
                        var cellDni = row.CreateCell(4); cellDni.SetCellValue(beneficiario.NUMERO_DOCUMENTO);
                        var cellFechaNa = row.CreateCell(5); cellFechaNa.SetCellValue(beneficiario.FER_NAC == null ? String.Empty
                            : beneficiario.FER_NAC.ToShortDateString());
                        var cellCalle = row.CreateCell(6); cellCalle.SetCellValue(beneficiario.CALLE);
                        var cellNro = row.CreateCell(7); cellNro.SetCellValue(beneficiario.NUMERO);
                        var cellPiso = row.CreateCell(8); cellPiso.SetCellValue(beneficiario.PISO);
                        var cellDpto = row.CreateCell(9); cellDpto.SetCellValue(beneficiario.DPTO);
                        var cellMono = row.CreateCell(10); cellMono.SetCellValue(beneficiario.MONOBLOCK);
                        var cellParcela = row.CreateCell(11); cellParcela.SetCellValue(beneficiario.PARCELA);
                        var cellManzana = row.CreateCell(12); cellManzana.SetCellValue(beneficiario.MANZANA);
                        var cellEntreCalle = row.CreateCell(13); cellEntreCalle.SetCellValue(beneficiario.ENTRECALLES);
                        var cellBarrio = row.CreateCell(14); cellBarrio.SetCellValue(beneficiario.BARRIO);
                        var cellCp = row.CreateCell(15); cellCp.SetCellValue(beneficiario.CODIGO_POSTAL);
                        var cellidLocalidad = row.CreateCell(16); cellidLocalidad.SetCellValue(beneficiario.ID_LOCALIDAD == null ?
                            String.Empty : beneficiario.ID_LOCALIDAD.ToString());
                        var cellnLocalidad = row.CreateCell(17); cellnLocalidad.SetCellValue(beneficiario.N_LOCALIDAD);
                        var cellIdDepartamento = row.CreateCell(18); cellIdDepartamento.SetCellValue(beneficiario.ID_LOCALIDAD == null ?
                            String.Empty : beneficiario.ID_DPTO.ToString());
                        var cellnDepartamento = row.CreateCell(19); cellnDepartamento.SetCellValue(beneficiario.N_DEPARTAMENTO);
                        var cellTelFijo = row.CreateCell(20); cellTelFijo.SetCellValue(beneficiario.TEL_FIJO);
                        var cellTelCelular = row.CreateCell(21); cellTelCelular.SetCellValue(beneficiario.TEL_CELULAR);
                        var cellContacto = row.CreateCell(22); cellContacto.SetCellValue(beneficiario.CONTACTO);
                        var cellEmail = row.CreateCell(23); cellEmail.SetCellValue(beneficiario.MAIL);

                        var cellTieneHijos = row.CreateCell(24); cellTieneHijos.SetCellValue(beneficiario.TIENE_HIJOS);//TIENE_HIJOS	
                        var cellCantidadHijos = row.CreateCell(25); cellCantidadHijos.SetCellValue(beneficiario.CANTIDAD_HIJOS == null ? "0" : beneficiario.CANTIDAD_HIJOS.ToString());//CANTIDAD_HIJOS
                        var cellEsDiscapacitado = row.CreateCell(26); cellEsDiscapacitado.SetCellValue(beneficiario.ES_DISCAPACITADO == null ? "N" : beneficiario.ES_DISCAPACITADO);//ES_DIACAPACITADO
                        var cellTieneDeficienciaMotora = row.CreateCell(27); cellTieneDeficienciaMotora.SetCellValue(beneficiario.TIENE_DEF_MOTORA == null ? "N" : beneficiario.TIENE_DEF_MOTORA);//TIENE_DEF_MOTORA
                        var cellTieneDeficienciaMental = row.CreateCell(28); cellTieneDeficienciaMental.SetCellValue(beneficiario.TIENE_DEF_MENTAL == null ? "N" : beneficiario.TIENE_DEF_MENTAL);//TIENE_DEF_MENTAL
                        var cellTieneDeficienciaSensorial = row.CreateCell(29); cellTieneDeficienciaSensorial.SetCellValue(beneficiario.TIENE_DEF_SENSORIAL == null ? "N" : beneficiario.TIENE_DEF_SENSORIAL);//TIENE_DEF_SENSORIAL
                        var cellTieneDeficienciaPsicologia = row.CreateCell(30); cellTieneDeficienciaPsicologia.SetCellValue(beneficiario.TIENE_DEF_PSICOLOGICA == null ? "N" : beneficiario.TIENE_DEF_PSICOLOGICA);//TIENE_DEF_PSICOLOGICA
                        var cellDeficienciaOtra = row.CreateCell(31); cellDeficienciaOtra.SetCellValue(beneficiario.DEFICIENCIA_OTRA == null ? "N" : beneficiario.TIENE_DEF_PSICOLOGICA);//DEFICIENCIA_OTRA
                        var cellCertificadoDiscapacidad = row.CreateCell(32); cellCertificadoDiscapacidad.SetCellValue(beneficiario.CERTIF_DISCAP == null ? "N" : beneficiario.CERTIF_DISCAP);//CERTIF_DISCAP

                        var cellSexo = row.CreateCell(33); cellSexo.SetCellValue(beneficiario.SEXO);
                        var cellidestadoFicha = row.CreateCell(34); cellidestadoFicha.SetCellValue(beneficiario.ID_EST_FIC == null ? String.Empty :
                            beneficiario.ID_EST_FIC.ToString());
                        var cellnestadoFicha = row.CreateCell(35); cellnestadoFicha.SetCellValue(beneficiario.N_ESTADO_FICHA);

                        var cellNivelEscolaridad = row.CreateCell(36); cellNivelEscolaridad.SetCellValue(beneficiario.NIVEL_ESCOLARIDAD);

                        var cellModalidad = row.CreateCell(37); cellModalidad.SetCellValue(beneficiario.MODALIDAD == (int)Enums.Modalidad.Entrenamiento
                            ? "Entrenamiento" : "CTI");
                        var cellaltaTemprana = row.CreateCell(38); cellaltaTemprana.SetCellValue(beneficiario.ALTA_TEMPRANA);
                        var cellatFechaInicio = row.CreateCell(39); cellatFechaInicio.SetCellValue(beneficiario.AT_FECHA_INICIO == null ?
                            String.Empty : beneficiario.AT_FECHA_INICIO.Value.ToShortDateString());
                        var cellatFechaCese = row.CreateCell(40); cellatFechaCese.SetCellValue(beneficiario.AT_FECHA_CESE == null ?
                            String.Empty : beneficiario.AT_FECHA_CESE.Value.ToShortDateString());
                        var cellidModContAfip = row.CreateCell(41); cellidModContAfip.SetCellValue(beneficiario.ID_MOD_CONT_AFIP == null ?
                            String.Empty : beneficiario.ID_MOD_CONT_AFIP.ToString());
                        var cellModContAfip = row.CreateCell(42); cellModContAfip.SetCellValue(beneficiario.MOD_CONT_AFIP);

                        var cellFechaModif = row.CreateCell(43); cellFechaModif.SetCellValue(beneficiario.FEC_MODIF == null ? String.Empty
                            : beneficiario.FEC_MODIF.Value.ToShortDateString());

                        var cellnSubprograma = row.CreateCell(44); cellnSubprograma.SetCellValue(beneficiario.SUBPROGRAMA);

                        //Proyecto
                        var cellnProyecto = row.CreateCell(45); cellnProyecto.SetCellValue(beneficiario.N_PROYECTO);
                        var cellCantCapacitar = row.CreateCell(46); cellCantCapacitar.SetCellValue(beneficiario.CANT_A_CAPACITAR == null ? String.Empty : beneficiario.CANT_A_CAPACITAR.ToString());
                        var cellFecInicio = row.CreateCell(47); cellFecInicio.SetCellValue(beneficiario.PRO_FEC_INICIO == null ? String.Empty : beneficiario.PRO_FEC_INICIO.Value.ToShortDateString());
                        var cellFecFin = row.CreateCell(48); cellFecFin.SetCellValue(beneficiario.PRO_FEC_FIN == null ? String.Empty : beneficiario.PRO_FEC_FIN.Value.ToShortDateString());
                        var cellMesesDuracion = row.CreateCell(49); cellMesesDuracion.SetCellValue(beneficiario.MESES_DURACION == null ? String.Empty : beneficiario.MESES_DURACION.ToString());
                        var cellAportesEmpresa = row.CreateCell(50); cellAportesEmpresa.SetCellValue(beneficiario.APORTES_DE_EMPRESA);
                        var cellApeTutor = row.CreateCell(51); cellApeTutor.SetCellValue(beneficiario.APELLIDO_TUTOR);
                        var cellNomTutor = row.CreateCell(52); cellNomTutor.SetCellValue(beneficiario.NOMBRE_TUTOR);
                        var cellDocTutor = row.CreateCell(53); cellDocTutor.SetCellValue(beneficiario.DNI_TUTOR);
                        var cellTelTutor = row.CreateCell(54); cellTelTutor.SetCellValue(beneficiario.TELEFONO_TUTOR);
                        var cellEmailTutor = row.CreateCell(55); cellEmailTutor.SetCellValue(beneficiario.EMAIL_TUTOR);
                        var cellPuestoTutor = row.CreateCell(56); cellPuestoTutor.SetCellValue(beneficiario.PUESTO_TUTOR);

                        //Empresa
                        var cellidEmp = row.CreateCell(57); cellidEmp.SetCellValue(beneficiario.ID_EMP == null ? "" : beneficiario.ID_EMP.ToString());
                        var cellrazonSocial = row.CreateCell(58); cellrazonSocial.SetCellValue(beneficiario.RAZON_SOCIAL);
                        var cellCuit = row.CreateCell(59); cellCuit.SetCellValue(beneficiario.EMP_CUIT);
                        var cellCodact = row.CreateCell(60); cellCodact.SetCellValue(beneficiario.COD_ACT);
                        var cellCantemp = row.CreateCell(61); cellCantemp.SetCellValue(beneficiario.CANT_EMP == null ?
                            String.Empty : beneficiario.CANT_EMP.ToString());
                        var cellCalleemp = row.CreateCell(62); cellCalleemp.SetCellValue(beneficiario.EMP_CALLE);
                        var cellNumeroemp = row.CreateCell(63); cellNumeroemp.SetCellValue(beneficiario.EMP_NUMERO);
                        var cellPisoemp = row.CreateCell(64); cellPisoemp.SetCellValue(beneficiario.EMP_PISO);
                        var cellDptoemp = row.CreateCell(65); cellDptoemp.SetCellValue(beneficiario.EMP_DPTO);
                        var cellCpemp = row.CreateCell(66); cellCpemp.SetCellValue(beneficiario.EMP_CP);
                        var cellIdlocemp = row.CreateCell(67); cellIdlocemp.SetCellValue(beneficiario.ID_LOC == null ?
                            String.Empty : beneficiario.ID_LOC.ToString());
                        var cellLocemp = row.CreateCell(68); cellLocemp.SetCellValue(beneficiario.EMP_N_LOCALIDAD);
                        var cellDepartamentoEmp = row.CreateCell(69); cellDepartamentoEmp.SetCellValue(beneficiario.ID_LOC == null ?
                            String.Empty : beneficiario.EMP_N_DEPARTAMENTO);
                        var cellIdusuarioemp = row.CreateCell(70); cellIdusuarioemp.SetCellValue(beneficiario.EU_ID_USUARIO == null ?
                            String.Empty : beneficiario.EU_ID_USUARIO.ToString());
                        var cellNombreusuemp = row.CreateCell(71); cellNombreusuemp.SetCellValue(beneficiario.EU_NOMBRE);
                        var cellApellidousuemp = row.CreateCell(72); cellApellidousuemp.SetCellValue(beneficiario.EMP_APELLIDO);
                        var cellEmailusuemp = row.CreateCell(73); cellEmailusuemp.SetCellValue(beneficiario.EU_MAIL);
                        var cellTeleusuemp = row.CreateCell(74); cellTeleusuemp.SetCellValue(beneficiario.EU_TELEFONO);

                        //Sede
                        var cellIdSede = row.CreateCell(75); cellIdSede.SetCellValue(beneficiario.ID_SEDE == null ? String.Empty : beneficiario.ID_SEDE.ToString());
                        var cellnSede = row.CreateCell(76); cellnSede.SetCellValue(beneficiario.N_SEDE);
                        var cellCalleSede = row.CreateCell(77); cellCalleSede.SetCellValue(beneficiario.SEDE_CALLE);
                        var cellNroSede = row.CreateCell(78); cellNroSede.SetCellValue(beneficiario.SEDE_NRO);
                        var cellPisoSede = row.CreateCell(79); cellPisoSede.SetCellValue(beneficiario.SEDE_PISO);
                        var cellDptoSede = row.CreateCell(80); cellDptoSede.SetCellValue(beneficiario.SEDE_DPTO);
                        var cellCpSede = row.CreateCell(81); cellCpSede.SetCellValue(beneficiario.SEDE_CP);
                        var cellLocSede = row.CreateCell(82); cellLocSede.SetCellValue(beneficiario.SEDE_LOCALIDAD);
                        var cellApeContacto = row.CreateCell(83); cellApeContacto.SetCellValue(beneficiario.SEDE_APE_CONTACTO);
                        var cellNomContacto = row.CreateCell(84); cellNomContacto.SetCellValue(beneficiario.SEDE_NOM_CONTACTO);
                        var cellTelcontacto = row.CreateCell(85); cellTelcontacto.SetCellValue(beneficiario.SEDE_TELEFONO);
                        var cellFaxContacto = row.CreateCell(86); cellFaxContacto.SetCellValue(beneficiario.SEDE_FAX);
                        var cellEmailContacto = row.CreateCell(87); cellEmailContacto.SetCellValue(beneficiario.SEDE_EMAIL);

                        //Beneficiario
                        var cellIdben = row.CreateCell(88); cellIdben.SetCellValue(beneficiario.ID_BENEFICIARIO == null ? "" : beneficiario.ID_BENEFICIARIO.ToString());
                        var cellIdestadoben = row.CreateCell(89); cellIdestadoben.SetCellValue(beneficiario.BEN_ID_ESTADO == null ? "" : beneficiario.BEN_ID_ESTADO.ToString());
                        var cellEstadoben = row.CreateCell(90); cellEstadoben.SetCellValue(beneficiario.BEN_N_ESTADO);
                        var cellNrocuentaben = row.CreateCell(91); cellNrocuentaben.SetCellValue(beneficiario.BEN_NRO_CTA == null ? String.Empty
                            : beneficiario.BEN_NRO_CTA.ToString());
                        var cellIdsucursalben = row.CreateCell(92); cellIdsucursalben.SetCellValue(beneficiario.BEN_ID_SUCURSAL == null ? String.Empty
                            : beneficiario.BEN_ID_SUCURSAL.ToString());
                        var cellCodsucursalben = row.CreateCell(93); cellCodsucursalben.SetCellValue(beneficiario.BEN_COD_SUC);
                        var cellSucursalben = row.CreateCell(94); cellSucursalben.SetCellValue(beneficiario.BEN_SUCURSAL);
                        var cellFecsolben = row.CreateCell(95); cellFecsolben.SetCellValue(beneficiario.BEN_FEC_SOL_CTA == null ? String.Empty
                            : beneficiario.BEN_FEC_SOL_CTA.Value.ToShortDateString());
                        var cellNotificado = row.CreateCell(96); cellNotificado.SetCellValue(beneficiario.NOTIFICADO);
                        var cellFecNotif = row.CreateCell(97); cellFecNotif.SetCellValue(beneficiario.FEC_NOTIF == null ? String.Empty
                            : beneficiario.FEC_NOTIF.Value.ToShortDateString());

                        var cellFechaIniBenf = row.CreateCell(98); cellFechaIniBenf.SetCellValue(beneficiario.BEN_FEC_INICIO == null ? string.Empty : beneficiario.BEN_FEC_INICIO.Value.ToShortDateString());
                        var cellBajaBenef = row.CreateCell(99); cellBajaBenef.SetCellValue(beneficiario.BEN_FEC_BAJA == null ? string.Empty : beneficiario.BEN_FEC_BAJA.Value.ToShortDateString());

                        var cellTieneapoben = row.CreateCell(100); cellTieneapoben.SetCellValue(beneficiario.TIENE_APODERADO);

                        //Apoderado
                        var cellIdapoderado = row.CreateCell(101); cellIdapoderado.SetCellValue(beneficiario.ID_APODERADO == null ?
                            String.Empty : beneficiario.ID_APODERADO.ToString());
                        var cellApellidoapo = row.CreateCell(102); cellApellidoapo.SetCellValue(beneficiario.APO_APELLIDO);
                        var cellNombreapo = row.CreateCell(103); cellNombreapo.SetCellValue(beneficiario.APO_NOMBRE);
                        var cellDniapo = row.CreateCell(104); cellDniapo.SetCellValue(beneficiario.APO_DNI);
                        var cellCuilapo = row.CreateCell(105); cellCuilapo.SetCellValue(beneficiario.APO_CUIL);
                        var cellSexoapo = row.CreateCell(106); cellSexoapo.SetCellValue(beneficiario.APO_SEXO);
                        var cellNroctaapo = row.CreateCell(107); cellNroctaapo.SetCellValue(beneficiario.APO_NRO_CTA == null ? String.Empty
                            : beneficiario.APO_NRO_CTA.ToString());
                        var cellIdSuc = row.CreateCell(108); cellIdSuc.SetCellValue(beneficiario.APO_ID_SUC == null ? String.Empty
                            : beneficiario.APO_ID_SUC.ToString());
                        var cellCodsucapo = row.CreateCell(109); cellCodsucapo.SetCellValue(beneficiario.APO_COD_SUC);
                        var cellSucursalapo = row.CreateCell(110); cellSucursalapo.SetCellValue(beneficiario.APO_SUCURSAL);
                        var cellFecsolcuentaapo = row.CreateCell(111); cellFecsolcuentaapo.SetCellValue(beneficiario.APO_FEC_SOL_CTA == null ?
                            String.Empty : beneficiario.APO_FEC_SOL_CTA.Value.ToShortDateString());
                        var cellFecnac = row.CreateCell(112); cellFecnac.SetCellValue(beneficiario.APO_FEC_NAC == null ? String.Empty :
                            beneficiario.APO_FEC_NAC.Value.ToShortDateString());
                        var cellCalleapo = row.CreateCell(113); cellCalleapo.SetCellValue(beneficiario.APO_CALLE);
                        var cellNroapo = row.CreateCell(114); cellNroapo.SetCellValue(beneficiario.APO_NRO);
                        var cellPisoapo = row.CreateCell(115); cellPisoapo.SetCellValue(beneficiario.APO_PISO);
                        var cellMonoblockapo = row.CreateCell(116); cellMonoblockapo.SetCellValue(beneficiario.APO_MONOBLOCK);
                        var cellDptoapo = row.CreateCell(117); cellDptoapo.SetCellValue(beneficiario.APO_DPTO);
                        var cellParcelaapo = row.CreateCell(118); cellParcelaapo.SetCellValue(beneficiario.APO_PARCELA);
                        var cellManzanaapo = row.CreateCell(119); cellManzanaapo.SetCellValue(beneficiario.APO_MANZANA);
                        var cellEntrecalleapo = row.CreateCell(120); cellEntrecalleapo.SetCellValue(beneficiario.APO_ENTRE_CALLES);
                        var cellBarrioapo = row.CreateCell(121); cellBarrioapo.SetCellValue(beneficiario.APO_BARRIO);
                        var cellCpapo = row.CreateCell(123); cellCpapo.SetCellValue(beneficiario.APO_CP);
                        var cellIdlocalidadapo = row.CreateCell(124); cellIdlocalidadapo.SetCellValue(beneficiario.APO_ID_LOC == null ? String.Empty
                            : beneficiario.APO_ID_LOC.ToString());
                        var cellLocalidadapo = row.CreateCell(125); cellLocalidadapo.SetCellValue(beneficiario.APO_LOCALIDAD);
                        var cellIddeptoapo = row.CreateCell(126); cellIddeptoapo.SetCellValue(beneficiario.APO_ID_LOC == null ? String.Empty
                            : beneficiario.APO_ID_DEPTO.ToString());
                        var cellDeptoapo = row.CreateCell(127); cellDeptoapo.SetCellValue(beneficiario.APO_DEPARTAMENTO);
                        var cellTelefonoapo = row.CreateCell(128); cellTelefonoapo.SetCellValue(beneficiario.APO_TELEFONO);
                        var cellCelularapo = row.CreateCell(129); cellCelularapo.SetCellValue(beneficiario.APO_CELULAR);
                        var cellEmailapo = row.CreateCell(130);
                        cellEmailapo.SetCellValue(beneficiario.APO_EMAIL);
                        var cellApoEstado = row.CreateCell(131);
                        cellApoEstado.SetCellValue(beneficiario.AP_N_ESTADO);
                    }
                    #endregion
                    break;

            }


            var ms = new MemoryStream();

            templateWorkbook.Write(ms);

            return ms.ToArray();
        }

        public byte[] ExportarFichasConfVos(string cuil, string dni, string nombre, string apellido, int tipoficha, int estadoficha, int idEtapa, FichasVista vista)
        {
            var path = String.Empty;


            path = HttpContext.Current.Server.MapPath(@"\Archivos\Exportados\TemplateBeneficiariosConfVos.xls");

            var fs = new FileStream(path, FileMode.Open, FileAccess.Read);

            var templateWorkbook = new HSSFWorkbook(fs, true);

            var sheet = templateWorkbook.GetSheet("Nomina");

            List<VT_REPORTES_CONF_VOS> listaparaExcel = null;

            listaparaExcel = _FichaRepo.getfichaConfVos(cuil, dni, nombre, apellido, tipoficha, estadoficha, idEtapa);

            if (vista.ProgPorRol == "S")
            {

                //**********************************************
                if (vista.idSubprograma > 0) // si se consulta por subprograma
                {
                    listaparaExcel = listaparaExcel.Where(x => x.ID_SUBPROGRAMA == vista.idSubprograma || x.ID_SUBPROGRAMA == null).ToList();
                }

                //**********************************************
            }


            var i = 1;

            switch (tipoficha)
            {

                case (int)Enums.TipoFicha.ConfiamosEnVos:
                    #region case (int)Enums.TipoFicha.ConfiamosEnVos
                    foreach (var beneficiario in listaparaExcel)
                    {
                        i++;
                        //Ficha
                        var row = sheet.CreateRow(i);
                        var cellidficha = row.CreateCell(0); cellidficha.SetCellValue(beneficiario.ID_FICHA);
                        var cellApellido = row.CreateCell(1); cellApellido.SetCellValue(beneficiario.APELLIDO);
                        var cellNombre = row.CreateCell(2); cellNombre.SetCellValue(beneficiario.NOMBRE);
                        var cellCuil = row.CreateCell(3); cellCuil.SetCellValue(beneficiario.CUIL);
                        var cellDni = row.CreateCell(4); cellDni.SetCellValue(beneficiario.NUMERO_DOCUMENTO);
                        var cellFechaNa = row.CreateCell(5); cellFechaNa.SetCellValue(beneficiario.FER_NAC == null ? String.Empty
                            : beneficiario.FER_NAC.ToShortDateString());
                        var cellCalle = row.CreateCell(6); cellCalle.SetCellValue(beneficiario.CALLE);
                        var cellNro = row.CreateCell(7); cellNro.SetCellValue(beneficiario.NUMERO);
                        var cellPiso = row.CreateCell(8); cellPiso.SetCellValue(beneficiario.PISO);
                        var cellDpto = row.CreateCell(9); cellDpto.SetCellValue(beneficiario.DPTO);
                        var cellMono = row.CreateCell(10); cellMono.SetCellValue(beneficiario.MONOBLOCK);
                        var cellParcela = row.CreateCell(11); cellParcela.SetCellValue(beneficiario.PARCELA);
                        var cellManzana = row.CreateCell(12); cellManzana.SetCellValue(beneficiario.MANZANA);
                        var cellEntreCalle = row.CreateCell(13); cellEntreCalle.SetCellValue(beneficiario.ENTRECALLES);
                        var cellBarrio = row.CreateCell(14); cellBarrio.SetCellValue(beneficiario.BARRIO);
                        var cellCp = row.CreateCell(15); cellCp.SetCellValue(beneficiario.CODIGO_POSTAL);
                        var cellidLocalidad = row.CreateCell(16); cellidLocalidad.SetCellValue(beneficiario.ID_LOCALIDAD == null ?
                            String.Empty : beneficiario.ID_LOCALIDAD.ToString());
                        var cellnLocalidad = row.CreateCell(17); cellnLocalidad.SetCellValue(beneficiario.N_LOCALIDAD);
                        var cellIdDepartamento = row.CreateCell(18); cellIdDepartamento.SetCellValue(beneficiario.ID_LOCALIDAD == null ?
                            String.Empty : beneficiario.ID_DPTO.ToString());
                        var cellnDepartamento = row.CreateCell(19); cellnDepartamento.SetCellValue(beneficiario.N_DEPARTAMENTO);
                        var cellTelFijo = row.CreateCell(20); cellTelFijo.SetCellValue(beneficiario.TEL_FIJO);
                        var cellTelCelular = row.CreateCell(21); cellTelCelular.SetCellValue(beneficiario.TEL_CELULAR);
                        var cellContacto = row.CreateCell(22); cellContacto.SetCellValue(beneficiario.CONTACTO);
                        var cellEmail = row.CreateCell(23); cellEmail.SetCellValue(beneficiario.MAIL);

                        var cellTieneHijos = row.CreateCell(24); cellTieneHijos.SetCellValue(beneficiario.TIENE_HIJOS);//TIENE_HIJOS	
                        var cellCantidadHijos = row.CreateCell(25); cellCantidadHijos.SetCellValue(beneficiario.CANTIDAD_HIJOS == null ? "0" : beneficiario.CANTIDAD_HIJOS.ToString());//CANTIDAD_HIJOS
                        var cellEsDiscapacitado = row.CreateCell(26); cellEsDiscapacitado.SetCellValue(beneficiario.ES_DISCAPACITADO == null ? "N" : beneficiario.ES_DISCAPACITADO);//ES_DIACAPACITADO
                        var cellTieneDeficienciaMotora = row.CreateCell(27); cellTieneDeficienciaMotora.SetCellValue(beneficiario.TIENE_DEF_MOTORA == null ? "N" : beneficiario.TIENE_DEF_MOTORA);//TIENE_DEF_MOTORA
                        var cellTieneDeficienciaMental = row.CreateCell(28); cellTieneDeficienciaMental.SetCellValue(beneficiario.TIENE_DEF_MENTAL == null ? "N" : beneficiario.TIENE_DEF_MENTAL);//TIENE_DEF_MENTAL
                        var cellTieneDeficienciaSensorial = row.CreateCell(29); cellTieneDeficienciaSensorial.SetCellValue(beneficiario.TIENE_DEF_SENSORIAL == null ? "N" : beneficiario.TIENE_DEF_SENSORIAL);//TIENE_DEF_SENSORIAL
                        var cellTieneDeficienciaPsicologia = row.CreateCell(30); cellTieneDeficienciaPsicologia.SetCellValue(beneficiario.TIENE_DEF_PSICOLOGICA == null ? "N" : beneficiario.TIENE_DEF_PSICOLOGICA);//TIENE_DEF_PSICOLOGICA
                        var cellDeficienciaOtra = row.CreateCell(31); cellDeficienciaOtra.SetCellValue(beneficiario.DEFICIENCIA_OTRA == null ? "N" : beneficiario.TIENE_DEF_PSICOLOGICA);//DEFICIENCIA_OTRA
                        var cellCertificadoDiscapacidad = row.CreateCell(32); cellCertificadoDiscapacidad.SetCellValue(beneficiario.CERTIF_DISCAP == null ? "N" : beneficiario.CERTIF_DISCAP);//CERTIF_DISCAP

                        var cellSexo = row.CreateCell(33); cellSexo.SetCellValue(beneficiario.SEXO);
                        var cellidestadoFicha = row.CreateCell(34); cellidestadoFicha.SetCellValue(beneficiario.ID_EST_FIC == null ? String.Empty :
                            beneficiario.ID_EST_FIC.ToString());
                        var cellnestadoFicha = row.CreateCell(35); cellnestadoFicha.SetCellValue(beneficiario.N_ESTADO_FICHA);

                        var cellNivelEscolaridad = row.CreateCell(36); cellNivelEscolaridad.SetCellValue(beneficiario.NIVEL_ESCOLARIDAD);

                        var cellModalidad = row.CreateCell(37); cellModalidad.SetCellValue(beneficiario.MODALIDAD == (int)Enums.Modalidad.Entrenamiento
                            ? "Entrenamiento" : "CTI");
                        var cellaltaTemprana = row.CreateCell(38); cellaltaTemprana.SetCellValue(beneficiario.ALTA_TEMPRANA);
                        var cellatFechaInicio = row.CreateCell(39); cellatFechaInicio.SetCellValue(beneficiario.AT_FECHA_INICIO == null ?
                            String.Empty : beneficiario.AT_FECHA_INICIO.Value.ToShortDateString());
                        var cellatFechaCese = row.CreateCell(40); cellatFechaCese.SetCellValue(beneficiario.AT_FECHA_CESE == null ?
                            String.Empty : beneficiario.AT_FECHA_CESE.Value.ToShortDateString());
                        var cellidModContAfip = row.CreateCell(41); cellidModContAfip.SetCellValue(beneficiario.ID_MOD_CONT_AFIP == null ?
                            String.Empty : beneficiario.ID_MOD_CONT_AFIP.ToString());
                        var cellModContAfip = row.CreateCell(42); cellModContAfip.SetCellValue(beneficiario.MOD_CONT_AFIP);

                        var cellFechaModif = row.CreateCell(43); cellFechaModif.SetCellValue(beneficiario.FEC_MODIF == null ? String.Empty
                            : beneficiario.FEC_MODIF.Value.ToShortDateString());

                        //07/07/2015*****NUEVOS CAMPOS****

                        var cellSENAF = row.CreateCell(44); cellSENAF.SetCellValue(beneficiario.SENAF);
                        var cellIdONG = row.CreateCell(45); cellIdONG.SetCellValue(beneficiario.ID_ONG == null ? "" : beneficiario.ID_ONG.ToString());
                        var cellONG_N_NOMBRE = row.CreateCell(46); cellONG_N_NOMBRE.SetCellValue(beneficiario.ONG_N_NOMBRE);
                        var cellTRABAJA_TRABAJO = row.CreateCell(47); cellTRABAJA_TRABAJO.SetCellValue(beneficiario.TRABAJA_TRABAJO);
                        var cellPROGRESAR = row.CreateCell(48); cellPROGRESAR.SetCellValue(beneficiario.PROGRESAR);
                        var cellBENEF_PROGRE = row.CreateCell(49); cellBENEF_PROGRE.SetCellValue(beneficiario.BENEF_PROGRE);
                        var cellCURSA = row.CreateCell(50); cellCURSA.SetCellValue(beneficiario.CURSA);
                        var cellCENTRO_ADULTOS = row.CreateCell(51); cellCENTRO_ADULTOS.SetCellValue(beneficiario.CENTRO_ADULTOS);
                        var cellPIT = row.CreateCell(52); cellPIT.SetCellValue(beneficiario.PIT);
                        var cellID_CAJA = row.CreateCell(53); cellID_CAJA.SetCellValue(beneficiario.ID_CAJA == null ? "" : beneficiario.ID_CAJA.ToString());
                        var cellGES_APE = row.CreateCell(54); cellGES_APE.SetCellValue(beneficiario.GES_APE);
                        var cellGES_NOM = row.CreateCell(55); cellGES_NOM.SetCellValue(beneficiario.GES_NOM);
                        var cellGES_DNI = row.CreateCell(56); cellGES_DNI.SetCellValue(beneficiario.GES_DNI);
                        var cellFAM_APE = row.CreateCell(57); cellFAM_APE.SetCellValue(beneficiario.FAM_APE);
                        var cellFAM_NOM = row.CreateCell(58); cellFAM_NOM.SetCellValue(beneficiario.FAM_NOM);
                        var cellFAM_DNI = row.CreateCell(59); cellFAM_DNI.SetCellValue(beneficiario.FAM_DNI);

                        //********************************
                        var cellnSubprograma = row.CreateCell(60); cellnSubprograma.SetCellValue(beneficiario.SUBPROGRAMA);

                        //Proyecto
                        var cellnProyecto = row.CreateCell(61); cellnProyecto.SetCellValue(beneficiario.N_PROYECTO);
                        var cellCantCapacitar = row.CreateCell(62); cellCantCapacitar.SetCellValue(beneficiario.CANT_A_CAPACITAR == null ? String.Empty : beneficiario.CANT_A_CAPACITAR.ToString());
                        var cellFecInicio = row.CreateCell(63); cellFecInicio.SetCellValue(beneficiario.PRO_FEC_INICIO == null ? String.Empty : beneficiario.PRO_FEC_INICIO.Value.ToShortDateString());
                        var cellFecFin = row.CreateCell(64); cellFecFin.SetCellValue(beneficiario.PRO_FEC_FIN == null ? String.Empty : beneficiario.PRO_FEC_FIN.Value.ToShortDateString());
                        var cellMesesDuracion = row.CreateCell(65); cellMesesDuracion.SetCellValue(beneficiario.MESES_DURACION == null ? String.Empty : beneficiario.MESES_DURACION.ToString());
                        var cellAportesEmpresa = row.CreateCell(66); cellAportesEmpresa.SetCellValue(beneficiario.APORTES_DE_EMPRESA);
                        var cellApeTutor = row.CreateCell(67); cellApeTutor.SetCellValue(beneficiario.APELLIDO_TUTOR);
                        var cellNomTutor = row.CreateCell(68); cellNomTutor.SetCellValue(beneficiario.NOMBRE_TUTOR);
                        var cellDocTutor = row.CreateCell(69); cellDocTutor.SetCellValue(beneficiario.DNI_TUTOR);
                        var cellTelTutor = row.CreateCell(70); cellTelTutor.SetCellValue(beneficiario.TELEFONO_TUTOR);
                        var cellEmailTutor = row.CreateCell(71); cellEmailTutor.SetCellValue(beneficiario.EMAIL_TUTOR);
                        var cellPuestoTutor = row.CreateCell(72); cellPuestoTutor.SetCellValue(beneficiario.PUESTO_TUTOR);



                        //Empresa
                        var cellidEmp = row.CreateCell(73); cellidEmp.SetCellValue(beneficiario.ID_EMP == null ? "" : beneficiario.ID_EMP.ToString());
                        var cellrazonSocial = row.CreateCell(74); cellrazonSocial.SetCellValue(beneficiario.RAZON_SOCIAL);
                        var cellCuit = row.CreateCell(75); cellCuit.SetCellValue(beneficiario.EMP_CUIT);
                        var cellCodact = row.CreateCell(76); cellCodact.SetCellValue(beneficiario.COD_ACT);
                        var cellCantemp = row.CreateCell(77); cellCantemp.SetCellValue(beneficiario.CANT_EMP == null ?
                            String.Empty : beneficiario.CANT_EMP.ToString());
                        var cellCalleemp = row.CreateCell(78); cellCalleemp.SetCellValue(beneficiario.EMP_CALLE);
                        var cellNumeroemp = row.CreateCell(79); cellNumeroemp.SetCellValue(beneficiario.EMP_NUMERO);
                        var cellPisoemp = row.CreateCell(80); cellPisoemp.SetCellValue(beneficiario.EMP_PISO);
                        var cellDptoemp = row.CreateCell(81); cellDptoemp.SetCellValue(beneficiario.EMP_DPTO);
                        var cellCpemp = row.CreateCell(82); cellCpemp.SetCellValue(beneficiario.EMP_CP);
                        var cellIdlocemp = row.CreateCell(83); cellIdlocemp.SetCellValue(beneficiario.ID_LOC == null ?
                            String.Empty : beneficiario.ID_LOC.ToString());
                        var cellLocemp = row.CreateCell(84); cellLocemp.SetCellValue(beneficiario.EMP_N_LOCALIDAD);
                        var cellDepartamentoEmp = row.CreateCell(85); cellDepartamentoEmp.SetCellValue(beneficiario.ID_LOC == null ?
                            String.Empty : beneficiario.EMP_N_DEPARTAMENTO);
                        var cellIdusuarioemp = row.CreateCell(86); cellIdusuarioemp.SetCellValue(beneficiario.EU_ID_USUARIO == null ?
                            String.Empty : beneficiario.EU_ID_USUARIO.ToString());
                        var cellNombreusuemp = row.CreateCell(87); cellNombreusuemp.SetCellValue(beneficiario.EU_NOMBRE);
                        var cellApellidousuemp = row.CreateCell(88); cellApellidousuemp.SetCellValue(beneficiario.EMP_APELLIDO);
                        var cellEmailusuemp = row.CreateCell(89); cellEmailusuemp.SetCellValue(beneficiario.EU_MAIL);
                        var cellTeleusuemp = row.CreateCell(90); cellTeleusuemp.SetCellValue(beneficiario.EU_TELEFONO);

                        //Sede
                        var cellIdSede = row.CreateCell(91); cellIdSede.SetCellValue(beneficiario.ID_SEDE == null ? String.Empty : beneficiario.ID_SEDE.ToString());
                        var cellnSede = row.CreateCell(92); cellnSede.SetCellValue(beneficiario.N_SEDE);
                        var cellCalleSede = row.CreateCell(93); cellCalleSede.SetCellValue(beneficiario.SEDE_CALLE);
                        var cellNroSede = row.CreateCell(94); cellNroSede.SetCellValue(beneficiario.SEDE_NRO);
                        var cellPisoSede = row.CreateCell(95); cellPisoSede.SetCellValue(beneficiario.SEDE_PISO);
                        var cellDptoSede = row.CreateCell(96); cellDptoSede.SetCellValue(beneficiario.SEDE_DPTO);
                        var cellCpSede = row.CreateCell(97); cellCpSede.SetCellValue(beneficiario.SEDE_CP);
                        var cellLocSede = row.CreateCell(98); cellLocSede.SetCellValue(beneficiario.SEDE_LOCALIDAD);
                        var cellApeContacto = row.CreateCell(99); cellApeContacto.SetCellValue(beneficiario.SEDE_APE_CONTACTO);
                        var cellNomContacto = row.CreateCell(100); cellNomContacto.SetCellValue(beneficiario.SEDE_NOM_CONTACTO);
                        var cellTelcontacto = row.CreateCell(101); cellTelcontacto.SetCellValue(beneficiario.SEDE_TELEFONO);
                        var cellFaxContacto = row.CreateCell(102); cellFaxContacto.SetCellValue(beneficiario.SEDE_FAX);
                        var cellEmailContacto = row.CreateCell(103); cellEmailContacto.SetCellValue(beneficiario.SEDE_EMAIL);

                        //Beneficiario
                        var cellIdben = row.CreateCell(104); cellIdben.SetCellValue(beneficiario.ID_BENEFICIARIO == null ? "" : beneficiario.ID_BENEFICIARIO.ToString());
                        var cellIdestadoben = row.CreateCell(105); cellIdestadoben.SetCellValue(beneficiario.BEN_ID_ESTADO == null ? "" : beneficiario.BEN_ID_ESTADO.ToString());
                        var cellEstadoben = row.CreateCell(106); cellEstadoben.SetCellValue(beneficiario.BEN_N_ESTADO);
                        var cellNrocuentaben = row.CreateCell(107); cellNrocuentaben.SetCellValue(beneficiario.BEN_NRO_CTA == null ? String.Empty
                            : beneficiario.BEN_NRO_CTA.ToString());
                        var cellIdsucursalben = row.CreateCell(108); cellIdsucursalben.SetCellValue(beneficiario.BEN_ID_SUCURSAL == null ? String.Empty
                            : beneficiario.BEN_ID_SUCURSAL.ToString());
                        var cellCodsucursalben = row.CreateCell(109); cellCodsucursalben.SetCellValue(beneficiario.BEN_COD_SUC);
                        var cellSucursalben = row.CreateCell(110); cellSucursalben.SetCellValue(beneficiario.BEN_SUCURSAL);
                        var cellFecsolben = row.CreateCell(111); cellFecsolben.SetCellValue(beneficiario.BEN_FEC_SOL_CTA == null ? String.Empty
                            : beneficiario.BEN_FEC_SOL_CTA.Value.ToShortDateString());
                        var cellNotificado = row.CreateCell(112); cellNotificado.SetCellValue(beneficiario.NOTIFICADO);
                        var cellFecNotif = row.CreateCell(113); cellFecNotif.SetCellValue(beneficiario.FEC_NOTIF == null ? String.Empty
                            : beneficiario.FEC_NOTIF.Value.ToShortDateString());

                        var cellFechaIniBenf = row.CreateCell(114); cellFechaIniBenf.SetCellValue(beneficiario.BEN_FEC_INICIO == null ? string.Empty : beneficiario.BEN_FEC_INICIO.Value.ToShortDateString());
                        var cellBajaBenef = row.CreateCell(115); cellBajaBenef.SetCellValue(beneficiario.BEN_FEC_BAJA == null ? string.Empty : beneficiario.BEN_FEC_BAJA.Value.ToShortDateString());

                        var cellTieneapoben = row.CreateCell(116); cellTieneapoben.SetCellValue(beneficiario.TIENE_APODERADO);

                        //Apoderado
                        var cellIdapoderado = row.CreateCell(117); cellIdapoderado.SetCellValue(beneficiario.ID_APODERADO == null ?
                            String.Empty : beneficiario.ID_APODERADO.ToString());
                        var cellApellidoapo = row.CreateCell(118); cellApellidoapo.SetCellValue(beneficiario.APO_APELLIDO);
                        var cellNombreapo = row.CreateCell(119); cellNombreapo.SetCellValue(beneficiario.APO_NOMBRE);
                        var cellDniapo = row.CreateCell(120); cellDniapo.SetCellValue(beneficiario.APO_DNI);
                        var cellCuilapo = row.CreateCell(121); cellCuilapo.SetCellValue(beneficiario.APO_CUIL);
                        var cellSexoapo = row.CreateCell(122); cellSexoapo.SetCellValue(beneficiario.APO_SEXO);
                        var cellNroctaapo = row.CreateCell(123); cellNroctaapo.SetCellValue(beneficiario.APO_NRO_CTA == null ? String.Empty
                            : beneficiario.APO_NRO_CTA.ToString());
                        var cellIdSuc = row.CreateCell(124); cellIdSuc.SetCellValue(beneficiario.APO_ID_SUC == null ? String.Empty
                            : beneficiario.APO_ID_SUC.ToString());
                        var cellCodsucapo = row.CreateCell(125); cellCodsucapo.SetCellValue(beneficiario.APO_COD_SUC);
                        var cellSucursalapo = row.CreateCell(126); cellSucursalapo.SetCellValue(beneficiario.APO_SUCURSAL);
                        var cellFecsolcuentaapo = row.CreateCell(127); cellFecsolcuentaapo.SetCellValue(beneficiario.APO_FEC_SOL_CTA == null ?
                            String.Empty : beneficiario.APO_FEC_SOL_CTA.Value.ToShortDateString());
                        var cellFecnac = row.CreateCell(128); cellFecnac.SetCellValue(beneficiario.APO_FEC_NAC == null ? String.Empty :
                            beneficiario.APO_FEC_NAC.Value.ToShortDateString());
                        var cellCalleapo = row.CreateCell(129); cellCalleapo.SetCellValue(beneficiario.APO_CALLE);
                        var cellNroapo = row.CreateCell(130); cellNroapo.SetCellValue(beneficiario.APO_NRO);
                        var cellPisoapo = row.CreateCell(131); cellPisoapo.SetCellValue(beneficiario.APO_PISO);
                        var cellMonoblockapo = row.CreateCell(132); cellMonoblockapo.SetCellValue(beneficiario.APO_MONOBLOCK);
                        var cellDptoapo = row.CreateCell(133); cellDptoapo.SetCellValue(beneficiario.APO_DPTO);
                        var cellParcelaapo = row.CreateCell(134); cellParcelaapo.SetCellValue(beneficiario.APO_PARCELA);
                        var cellManzanaapo = row.CreateCell(135); cellManzanaapo.SetCellValue(beneficiario.APO_MANZANA);
                        var cellEntrecalleapo = row.CreateCell(136); cellEntrecalleapo.SetCellValue(beneficiario.APO_ENTRE_CALLES);
                        var cellBarrioapo = row.CreateCell(137); cellBarrioapo.SetCellValue(beneficiario.APO_BARRIO);
                        var cellCpapo = row.CreateCell(138); cellCpapo.SetCellValue(beneficiario.APO_CP);
                        var cellIdlocalidadapo = row.CreateCell(139); cellIdlocalidadapo.SetCellValue(beneficiario.APO_ID_LOC == null ? String.Empty
                            : beneficiario.APO_ID_LOC.ToString());
                        var cellLocalidadapo = row.CreateCell(140); cellLocalidadapo.SetCellValue(beneficiario.APO_LOCALIDAD);
                        var cellIddeptoapo = row.CreateCell(141); cellIddeptoapo.SetCellValue(beneficiario.APO_ID_LOC == null ? String.Empty
                            : beneficiario.APO_ID_DEPTO.ToString());
                        var cellDeptoapo = row.CreateCell(142); cellDeptoapo.SetCellValue(beneficiario.APO_DEPARTAMENTO);
                        var cellTelefonoapo = row.CreateCell(143); cellTelefonoapo.SetCellValue(beneficiario.APO_TELEFONO);
                        var cellCelularapo = row.CreateCell(144); cellCelularapo.SetCellValue(beneficiario.APO_CELULAR);
                        var cellEmailapo = row.CreateCell(145);
                        cellEmailapo.SetCellValue(beneficiario.APO_EMAIL);

                        var cellApoIdEstado = row.CreateCell(146);
                        cellApoIdEstado.SetCellValue(beneficiario.AP_ID_ESTADO == null ? String.Empty
                            : beneficiario.AP_ID_ESTADO.ToString());
                        var cellApoEstado = row.CreateCell(147);
                        cellApoEstado.SetCellValue(beneficiario.AP_N_ESTADO);

                        //ESCUELAS
                        var cellIdEscuela = row.CreateCell(148);
                        cellIdEscuela.SetCellValue(beneficiario.ESC_ID_ESCUELA == null ? "" : beneficiario.ESC_ID_ESCUELA.ToString());
                        var cellNEscuela = row.CreateCell(149);
                        cellNEscuela.SetCellValue(beneficiario.N_ESCUELA);
                        var cellEscCalle = row.CreateCell(150);
                        cellEscCalle.SetCellValue(beneficiario.ESC_CALLE);
                        var cellEscNumero = row.CreateCell(151);
                        cellEscNumero.SetCellValue(beneficiario.ESC_NUMERO);
                        var cellEscBarrio = row.CreateCell(152);
                        cellEscBarrio.SetCellValue(beneficiario.ESC_BARRIO);
                        var cellEscIdLocalidad = row.CreateCell(153);
                        cellEscIdLocalidad.SetCellValue(beneficiario.ESC_ID_LOCALIDAD == null ? "" : beneficiario.ESC_ID_LOCALIDAD.ToString());
                        var cellEscLocalidad = row.CreateCell(154);
                        cellEscLocalidad.SetCellValue(beneficiario.ESC_N_LOCALIDAD);

                        var cellCOO_APE = row.CreateCell(155); cellCOO_APE.SetCellValue(beneficiario.COO_APE);
                        var cellCOO_NOM = row.CreateCell(156); cellCOO_NOM.SetCellValue(beneficiario.COO_NOM);
                        var cellCOO_DNI = row.CreateCell(157); cellCOO_DNI.SetCellValue(beneficiario.COO_DNI);
                        var cellENC_APE = row.CreateCell(158); cellENC_APE.SetCellValue(beneficiario.ENC_APE);
                        var cellENC_NOM = row.CreateCell(159); cellENC_NOM.SetCellValue(beneficiario.ENC_NOM);
                        var cellENC_DNI = row.CreateCell(160); cellENC_DNI.SetCellValue(beneficiario.ENC_DNI);

                    }
                    #endregion
                    break;

            }


            var ms = new MemoryStream();

            templateWorkbook.Write(ms);

            return ms.ToArray();
        }

        public bool AgregarHorarioEmpresa(IFichaVista ficha)
        {
            int idempresa = 0;

            switch (ficha.TipoFicha)
            {
                case (int)Enums.TipoFicha.Ppp:
                    idempresa = ficha.IdEmpresa ?? 0;
                    break;
                case (int)Enums.TipoFicha.PppProf:
                    idempresa = ficha.IdEmpresa ?? 0;
                    break;
                case (int)Enums.TipoFicha.Vat:
                    idempresa = ficha.IdEmpresa ?? 0;
                    break;
                case (int)Enums.TipoFicha.ReconversionProductiva:
                    idempresa = ficha.IdEmpresa ?? 0;
                    break;
                case (int)Enums.TipoFicha.EfectoresSociales:
                    idempresa = ficha.IdEmpresa ?? 0;
                    break;
            }

            IHorarioEmpresa horarioempresa = new HorarioEmpresa
                                                 {
                                                     HoraDesde = ficha.HoraInicio,
                                                     HoraHasta = ficha.HoraFin,
                                                     IdDia = ficha.DiaSemana,
                                                     MinutosDesde = ficha.MinutoInicio,
                                                     MinutosHasta = ficha.MinutoFin,
                                                     IdEmpresa = idempresa
                                                 };


            return _horarioempresarepositorio.AddHoarioEmpresa(horarioempresa);
        }

        public IFichaVista AgregarCicloEmpresa(IFichaVista ficha)
        {
            IFichaVista vista = GetFicha(ficha.IdFicha, "Modificar");


            switch (Convert.ToInt32(ficha.DiaSemana))
            {
                case (int)Enums.DiaSemana.Lunes:

                    IList<IHorarioEmpresa> listaHorarioEmpresa = _horarioempresarepositorio.GetHorariosEmpresa(vista.IdEmpresa, "01");

                    int countcombos = ficha.HorarioLunes.Count;

                    IList<IHorarioFicha> listahorarioficha =
                        _horarioficharepositorio.GetHorariosFichaByDia(vista.IdFicha, "01");

                    vista.HorarioLunes.Clear();
                    vista.ObsLunes.Clear();

                    for (int i = 0; i <= countcombos; i++)
                    {
                        var combo = new ComboBox();
                        combo.Combo.Add(new ComboItem
                        {
                            Id = 0,
                            Description = "Seleccione..."
                        });

                        foreach (var horarioEmpresa in listaHorarioEmpresa)
                        {
                            combo.Combo.Add(new ComboItem
                            {
                                Id = horarioEmpresa.IdEmpresaHorario,
                                Description =
                                    (horarioEmpresa.HoraDesde.ToString().PadLeft(2, '0') + ":" +
                                     horarioEmpresa.MinutosDesde.ToString().PadLeft(2, '0') + " - " +
                                     horarioEmpresa.HoraHasta.ToString().PadLeft(2, '0') + ":" +
                                     horarioEmpresa.MinutosHasta.ToString().PadLeft(2, '0'))
                            });
                        }

                        IHorarioFicha horarioficha = listahorarioficha.Where(c => c.Corte == i).FirstOrDefault();

                        if (horarioficha != null)
                        {
                            combo.Selected = horarioficha.IdEmpresaHorario.ToString();
                            vista.ObsLunes.Add(horarioficha.Observacion);
                        }
                        else
                        {
                            vista.ObsLunes.Add("");
                        }

                        vista.HorarioLunes.Add(combo);
                    }

                    break;
                case (int)Enums.DiaSemana.Martes:
                    IList<IHorarioEmpresa> listaHorarioEmpresaMar = _horarioempresarepositorio.GetHorariosEmpresa(vista.IdEmpresa, "02");

                    int countcombosMar = ficha.HorarioMartes.Count;

                    IList<IHorarioFicha> listahorariofichaMar = _horarioficharepositorio.GetHorariosFichaByDia(vista.IdFicha, "02");

                    vista.HorarioMartes.Clear();
                    vista.ObsMartes.Clear();

                    for (int i = 0; i <= countcombosMar; i++)
                    {
                        var combo = new ComboBox();
                        combo.Combo.Add(new ComboItem
                        {
                            Id = 0,
                            Description = "Seleccione..."

                        });

                        foreach (var horarioEmpresa in listaHorarioEmpresaMar)
                        {
                            combo.Combo.Add(new ComboItem
                            {
                                Id = horarioEmpresa.IdEmpresaHorario,
                                Description =
                                    (horarioEmpresa.HoraDesde.ToString().PadLeft(2, '0') + ":" +
                                     horarioEmpresa.MinutosDesde.ToString().PadLeft(2, '0') + " - " +
                                     horarioEmpresa.HoraHasta.ToString().PadLeft(2, '0') + ":" +
                                     horarioEmpresa.MinutosHasta.ToString().PadLeft(2, '0'))
                            });
                        }

                        IHorarioFicha horarioficha = listahorariofichaMar.Where(c => c.Corte == i).FirstOrDefault();

                        if (horarioficha != null)
                        {
                            combo.Selected = horarioficha.IdEmpresaHorario.ToString();
                            vista.ObsMartes.Add(horarioficha.Observacion);
                        }
                        else
                        {
                            vista.ObsMartes.Add("");
                        }

                        vista.HorarioMartes.Add(combo);
                    }
                    break;
                case (int)Enums.DiaSemana.Miércoles:
                    IList<IHorarioEmpresa> listaHorarioEmpresaMie = _horarioempresarepositorio.GetHorariosEmpresa(vista.IdEmpresa, "03");

                    int countcombosMie = ficha.HorarioMiercoles.Count;

                    IList<IHorarioFicha> listahorariofichaMie = _horarioficharepositorio.GetHorariosFichaByDia(vista.IdFicha, "03");

                    vista.HorarioMiercoles.Clear();
                    vista.ObsMiercoles.Clear();

                    for (int i = 0; i <= countcombosMie; i++)
                    {
                        var combo = new ComboBox();
                        combo.Combo.Add(new ComboItem
                        {
                            Id = 0,
                            Description = "Seleccione..."

                        });

                        foreach (var horarioEmpresa in listaHorarioEmpresaMie)
                        {
                            combo.Combo.Add(new ComboItem
                            {
                                Id = horarioEmpresa.IdEmpresaHorario,
                                Description =
                                    (horarioEmpresa.HoraDesde.ToString().PadLeft(2, '0') + ":" +
                                     horarioEmpresa.MinutosDesde.ToString().PadLeft(2, '0') + " - " +
                                     horarioEmpresa.HoraHasta.ToString().PadLeft(2, '0') + ":" +
                                     horarioEmpresa.MinutosHasta.ToString().PadLeft(2, '0'))
                            });
                        }

                        IHorarioFicha horarioficha = listahorariofichaMie.Where(c => c.Corte == i).FirstOrDefault();

                        if (horarioficha != null)
                        {
                            combo.Selected = horarioficha.IdEmpresaHorario.ToString();
                            vista.ObsMiercoles.Add(horarioficha.Observacion);
                        }
                        else
                        {
                            vista.ObsMiercoles.Add("");
                        }

                        vista.HorarioMiercoles.Add(combo);
                    }
                    break;
                case (int)Enums.DiaSemana.Jueves:
                    IList<IHorarioEmpresa> listaHorarioEmpresaJue = _horarioempresarepositorio.GetHorariosEmpresa(vista.IdEmpresa, "04");

                    int countcombosJue = ficha.HorarioJueves.Count;

                    IList<IHorarioFicha> listahorariofichaJue = _horarioficharepositorio.GetHorariosFichaByDia(vista.IdFicha, "04");

                    vista.HorarioJueves.Clear();
                    vista.ObsJueves.Clear();

                    for (int i = 0; i <= countcombosJue; i++)
                    {
                        var combo = new ComboBox();
                        combo.Combo.Add(new ComboItem
                        {
                            Id = 0,
                            Description = "Seleccione..."

                        });

                        foreach (var horarioEmpresa in listaHorarioEmpresaJue)
                        {
                            combo.Combo.Add(new ComboItem
                            {
                                Id = horarioEmpresa.IdEmpresaHorario,
                                Description =
                                    (horarioEmpresa.HoraDesde.ToString().PadLeft(2, '0') + ":" +
                                     horarioEmpresa.MinutosDesde.ToString().PadLeft(2, '0') + " - " +
                                     horarioEmpresa.HoraHasta.ToString().PadLeft(2, '0') + ":" +
                                     horarioEmpresa.MinutosHasta.ToString().PadLeft(2, '0'))
                            });
                        }

                        IHorarioFicha horarioficha = listahorariofichaJue.Where(c => c.Corte == i).FirstOrDefault();

                        if (horarioficha != null)
                        {
                            combo.Selected = horarioficha.IdEmpresaHorario.ToString();
                            vista.ObsJueves.Add(horarioficha.Observacion);
                        }
                        else
                        {
                            vista.ObsJueves.Add("");
                        }

                        vista.HorarioJueves.Add(combo);
                    }
                    break;
                case (int)Enums.DiaSemana.Viernes:
                    IList<IHorarioEmpresa> listaHorarioEmpresaVie = _horarioempresarepositorio.GetHorariosEmpresa(vista.IdEmpresa, "05");

                    int countcombosVie = ficha.HorarioViernes.Count;

                    IList<IHorarioFicha> listahorariofichaVie = _horarioficharepositorio.GetHorariosFichaByDia(vista.IdFicha, "05");

                    vista.HorarioViernes.Clear();
                    vista.ObsViernes.Clear();

                    for (int i = 0; i <= countcombosVie; i++)
                    {
                        var combo = new ComboBox();
                        combo.Combo.Add(new ComboItem
                        {
                            Id = 0,
                            Description = "Seleccione..."

                        });

                        foreach (var horarioEmpresa in listaHorarioEmpresaVie)
                        {
                            combo.Combo.Add(new ComboItem
                            {
                                Id = horarioEmpresa.IdEmpresaHorario,
                                Description =
                                    (horarioEmpresa.HoraDesde.ToString().PadLeft(2, '0') + ":" +
                                     horarioEmpresa.MinutosDesde.ToString().PadLeft(2, '0') + " - " +
                                     horarioEmpresa.HoraHasta.ToString().PadLeft(2, '0') + ":" +
                                     horarioEmpresa.MinutosHasta.ToString().PadLeft(2, '0'))
                            });
                        }

                        IHorarioFicha horarioficha = listahorariofichaVie.Where(c => c.Corte == i).FirstOrDefault();

                        if (horarioficha != null)
                        {
                            combo.Selected = horarioficha.IdEmpresaHorario.ToString();
                            vista.ObsViernes.Add(horarioficha.Observacion);
                        }
                        else
                        {
                            vista.ObsViernes.Add("");
                        }

                        vista.HorarioViernes.Add(combo);
                    }
                    break;
                case (int)Enums.DiaSemana.Sábado:
                    IList<IHorarioEmpresa> listaHorarioEmpresaSab = _horarioempresarepositorio.GetHorariosEmpresa(vista.IdEmpresa, "06");

                    int countcombosSab = ficha.HorarioSabado.Count;

                    IList<IHorarioFicha> listahorariofichaSab = _horarioficharepositorio.GetHorariosFichaByDia(vista.IdFicha, "06");

                    vista.HorarioSabado.Clear();
                    vista.ObsSabado.Clear();

                    for (int i = 0; i <= countcombosSab; i++)
                    {
                        var combo = new ComboBox();
                        combo.Combo.Add(new ComboItem
                        {
                            Id = 0,
                            Description = "Seleccione..."

                        });

                        foreach (var horarioEmpresa in listaHorarioEmpresaSab)
                        {
                            combo.Combo.Add(new ComboItem
                            {
                                Id = horarioEmpresa.IdEmpresaHorario,
                                Description =
                                    (horarioEmpresa.HoraDesde.ToString().PadLeft(2, '0') + ":" +
                                     horarioEmpresa.MinutosDesde.ToString().PadLeft(2, '0') + " - " +
                                     horarioEmpresa.HoraHasta.ToString().PadLeft(2, '0') + ":" +
                                     horarioEmpresa.MinutosHasta.ToString().PadLeft(2, '0'))
                            });
                        }

                        IHorarioFicha horarioficha = listahorariofichaSab.Where(c => c.Corte == i).FirstOrDefault();

                        if (horarioficha != null)
                        {
                            combo.Selected = horarioficha.IdEmpresaHorario.ToString();
                            vista.ObsSabado.Add(horarioficha.Observacion);
                        }
                        else
                        {
                            vista.ObsSabado.Add("");
                        }

                        vista.HorarioSabado.Add(combo);
                    }
                    break;
                case (int)Enums.DiaSemana.Domingo:
                    IList<IHorarioEmpresa> listaHorarioEmpresaDom = _horarioempresarepositorio.GetHorariosEmpresa(vista.IdEmpresa, "07");

                    int countcombosDom = ficha.HorarioDomingo.Count;

                    IList<IHorarioFicha> listahorariofichaDom = _horarioficharepositorio.GetHorariosFichaByDia(vista.IdFicha, "07");

                    vista.HorarioDomingo.Clear();
                    vista.ObsDomingo.Clear();

                    for (int i = 0; i <= countcombosDom; i++)
                    {
                        var combo = new ComboBox();
                        combo.Combo.Add(new ComboItem
                        {
                            Id = 0,
                            Description = "Seleccione..."

                        });

                        foreach (var horarioEmpresa in listaHorarioEmpresaDom)
                        {
                            combo.Combo.Add(new ComboItem
                            {
                                Id = horarioEmpresa.IdEmpresaHorario,
                                Description =
                                    (horarioEmpresa.HoraDesde.ToString().PadLeft(2, '0') + ":" +
                                     horarioEmpresa.MinutosDesde.ToString().PadLeft(2, '0') + " - " +
                                     horarioEmpresa.HoraHasta.ToString().PadLeft(2, '0') + ":" +
                                     horarioEmpresa.MinutosHasta.ToString().PadLeft(2, '0'))
                            });
                        }

                        IHorarioFicha horarioficha = listahorariofichaDom.Where(c => c.Corte == i).FirstOrDefault();

                        if (horarioficha != null)
                        {
                            combo.Selected = horarioficha.IdEmpresaHorario.ToString();
                            vista.ObsDomingo.Add(horarioficha.Observacion);
                        }
                        else
                        {
                            vista.ObsDomingo.Add("");
                        }

                        vista.HorarioDomingo.Add(combo);
                    }
                    break;

            }

            return vista;
        }

        public IFichasVista GetFichas(string cuil, string dni, string nombre, string apellido, int tipoficha, int estadoficha, int idEstapa, int idSubprograma, int TipoPrograma, string nrotramite)
        {
            IFichasVista vista = new FichasVista();

            bool accesoprograma = base.AccesoPrograma(Enums.Formulario.AbmFicha);

            // 22/08/2014 - controlar aca si tiene el rol activado para condicionar los programas y subprogramas.
            var accesoprogr =
                                  _rolformularioaccionrepositorio.GetFormulariosDelUsuario(base.GetUsuarioLoguer().Id_Usuario).Where(
                                      c =>
                                      c.Id_Accion == (int)Enums.Acciones.ProgramasPorRol &&
                                      c.Id_Formulario == (int)Enums.Formulario.AbmFicha).Count() > 0
                                      ? true
                                      : false;
            var usuarioRoles = _usuarioRolRepositorio.GetRolesDelUsuario(base.GetUsuarioLoguer().Id_Usuario);

            IList<IFicha> lfichas;

            if (accesoprogr) //verifica si debe traer todos los programas asociados
            {
                vista.ProgPorRol = "S";
                if (tipoficha == -1) tipoficha = 0;
                lfichas = _fichaRepositorio.GetFichas(cuil, dni, nombre, apellido, tipoficha, estadoficha, idEstapa, nrotramite, TipoPrograma).ToList();


                
                if (tipoficha > 5 && tipoficha < 9) // Los programas en este rango tienen subprogramas
                {
                    if (idSubprograma > 0) // si se consulta por subprograma
                    {
                        if (tipoficha == (int)Enums.TipoFicha.EfectoresSociales) // para el caso de efectores no debe traer los null
                        {
                            lfichas = lfichas.Where(c => c.IdSubprograma == idSubprograma).ToList();
                        }
                        else
                        {
                            lfichas = lfichas.Where(c => c.IdSubprograma == idSubprograma || c.IdSubprograma == null).ToList();
                        }
                    }
                    else
                    {
                        if (tipoficha == (int)Enums.TipoFicha.EfectoresSociales) // para el caso de efectores no debe traer todos
                        {
                            //verifico que subprogramas tiene disponible
                            var lsubprogramas = (from rs in _subprogramaRepositorio.GetSubprogramasRol()
                                    join u in usuarioRoles on rs.Id_Rol equals u.IdRol
                                    where rs.IdPrograma == tipoficha
                                    group rs by new { rs.IdSubprograma, rs.NombreSubprograma } into grouping
                                 select new
                                     Subprograma
                                 {
                                     IdSubprograma = grouping.Key.IdSubprograma,
                                     NombreSubprograma = grouping.Key.NombreSubprograma
                                 }).ToList();


                            lfichas = (from f in lfichas
                                          //from sub in lsubprogramas.Where(sub=>sub.IdSubprograma == f.IdSubprograma).DefaultIfEmpty()
                                       join sub in lsubprogramas on f.IdSubprograma equals sub.IdSubprograma   
                                       where  f.IdSubprograma != null
                                          select f).ToList();
                        }
                    }

                }
                else
                {
                    // 23/04/2015 ---------------------------------------
                    var lprogr = from rp in _programarepositorio.GetTipoFichasRol()
                                 join u in usuarioRoles on rp.Id_Rol equals u.IdRol
                                 group rp by new { rp.IdPrograma, rp.NombrePrograma } into grouping
                                 select new
                                     TipoFicha
                                 {
                                     Id_Tipo_Ficha = grouping.Key.IdPrograma,
                                     Nombre_Tipo_Ficha = grouping.Key.NombrePrograma
                                 };
                    lfichas = (from f in lfichas
                               
                               join pro in lprogr on f.TipoFicha equals pro.Id_Tipo_Ficha
                               select f).ToList();
                    // --------------------------------------------------
                }

            }
            else
            {
                vista.ProgPorRol = "N";
                // 08/08/2013 - DI CAMPLI LEANDRO - SE QUITA LAS CONSULTAS INECESARIAS
                lfichas = accesoprograma
                      ? _fichaRepositorio.GetFichas(cuil, dni, nombre, apellido, tipoficha, estadoficha, idEstapa, nrotramite, TipoPrograma).ToList()
                      : _fichaRepositorio.GetFichas(cuil, dni, nombre, apellido, tipoficha, estadoficha, idEstapa, nrotramite, TipoPrograma).Where(
                          c => c.TipoFicha != (int)Enums.TipoFicha.EfectoresSociales).ToList();

                if (idSubprograma > 0) // si se consulta por subprograma
                {

                        lfichas = lfichas.Where(c => c.IdSubprograma == idSubprograma).ToList();

                }




            }

            if (TipoPrograma > 0) // si se consulta por tipo programa
            {

                lfichas = lfichas.Where(c => c.TipoPrograma == TipoPrograma).ToList();

            }

            int cantreg = 0;

            cantreg = lfichas.Count();

            var pager = new Pager(cantreg,
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexFichas", _aut.GetUrl("IndexPager", "Fichas"));

            vista.pager = pager;

            vista.Fichas = lfichas.Skip(pager.Skip).Take(pager.PageSize).ToList();

            CargarEstadosFichas(vista, _estadoFichaRepositorio.GetEstadosFicha());

            //**************CARGAR EL COMBO PROGRAMAS O TIPO DE FICHA EN INSCRIPCIONES***********************************
            
            if (accesoprogr) //verifica si debe traer todos los programas asociados
            {

                var lprogramas = from rp in _programarepositorio.GetTipoFichasRol()
                                 join u in usuarioRoles on rp.Id_Rol equals u.IdRol
                                 group rp by new { rp.IdPrograma, rp.NombrePrograma } into grouping
                                 select new
                                     TipoFicha
                                 {
                                     Id_Tipo_Ficha = grouping.Key.IdPrograma,
                                     Nombre_Tipo_Ficha = grouping.Key.NombrePrograma
                                 };

                CargarTipoFichasRol(vista, lprogramas.OrderBy(c => c.Id_Tipo_Ficha));
            }
            else
            {

                CargarTipoFichas(vista, _tipoFichaRepositorio.GetTiposFicha());
            }
            //*****************************************************************************************

            vista.ComboEstadoFicha.Selected = estadoficha.ToString();

            //vista.ComboTipoFicha.Selected = tipoficha.ToString();


            var lstProgramas = _programarepositorio.GetProgramas();

            // 10/06/2013 - DI CAMPLI LEANDRO - CARGAN LOS PROGRAMAS PARA LAS ETAPAS
            CargarProgramaEtapa(vista, lstProgramas);

            var tiposppp = _tipopppRepositorio.GetTiposPpp();
            vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = 0, Description = "Todos" });
            foreach (var tipo in tiposppp)
            {
                vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = tipo.ID_TIPO_PPP, Description = tipo.N_TIPO_PPP });
            }

            //vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = 0, Description = "Todos" });
            //vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = 1, Description = "PRIMER PASO" });
            //vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = 2, Description = "APRENDIZ" });
            //vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = 3, Description = "XMÍ" });
            //vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = 4, Description = "PILA" });
            //vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = 5, Description = "PIP" });
            //vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = 6, Description = "CLIP" });
            //vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = 7, Description = "PIL - COMERCIO EXTERIOR" });
            //vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = 8, Description = "PIL - COMERCIO ELECTRONICO" });
            //vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = 9, Description = "PIL - MAQUINARIA AGRICOLA" });
            //vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = 10, Description = "PIL - TURISMO" });


            return vista;
        }

        public IFichasVista GetFichas(IPager pPager, string cuil, string dni, string nombre, string apellido, int tipoficha, int estadoficha, int idEtapa, int idSubprograma, int TipoPrograma, string nrotramite)
        {
            IFichasVista vista = new FichasVista();

            bool accesoprograma = base.AccesoPrograma(Enums.Formulario.AbmFicha);


            // 22/08/2014 - controlar aca si tiene el rol activado para condicionar los programas y subprogramas.
            var accesoprogr =
                                  _rolformularioaccionrepositorio.GetFormulariosDelUsuario(base.GetUsuarioLoguer().Id_Usuario).Where(
                                      c =>
                                      c.Id_Accion == (int)Enums.Acciones.ProgramasPorRol &&
                                      c.Id_Formulario == (int)Enums.Formulario.AbmFicha).Count() > 0
                                      ? true
                                      : false;
            var usuarioRoles = _usuarioRolRepositorio.GetRolesDelUsuario(base.GetUsuarioLoguer().Id_Usuario);

            int cantreg = 0;

            if (accesoprogr) //verifica si debe traer todos los programas asociados
            {
                vista.ProgPorRol = "S";
                if (tipoficha == -1) tipoficha = 0;
                vista.Fichas = _fichaRepositorio.GetFichas(cuil, dni, nombre, apellido, tipoficha, estadoficha, idEtapa, nrotramite, TipoPrograma);
                if (tipoficha > 5 && tipoficha < 9)
                {
                    //vista.Fichas = vista.Fichas.Where(c => c.IdSubprograma == idSubprograma).ToList();
                    
                        //**********************************************
                        if (idSubprograma > 0) // si se consulta por subprograma
                        {

                            if (tipoficha == (int)Enums.TipoFicha.EfectoresSociales) // para el caso de efectores no debe traer los null
                            {
                                vista.Fichas = vista.Fichas.Where(c => c.IdSubprograma == idSubprograma).ToList();
                            }
                            else
                            {
                                vista.Fichas = vista.Fichas.Where(c => c.IdSubprograma == idSubprograma || c.IdSubprograma == null).ToList();
                            }
                        
                        
                        }
                        else
                        {
                            if (tipoficha == (int)Enums.TipoFicha.EfectoresSociales) // para el caso de efectores no debe traer todos
                            {
                                //verifico que subprogramas tiene disponible
                                var lsubprogramas = (from rs in _subprogramaRepositorio.GetSubprogramasRol()
                                                     join u in usuarioRoles on rs.Id_Rol equals u.IdRol
                                                     where rs.IdPrograma == tipoficha
                                                     group rs by new { rs.IdSubprograma, rs.NombreSubprograma } into grouping
                                                     select new
                                                         Subprograma
                                                     {
                                                         IdSubprograma = grouping.Key.IdSubprograma,
                                                         NombreSubprograma = grouping.Key.NombreSubprograma
                                                     }).ToList();


                                vista.Fichas = (from f in vista.Fichas
                                                //from sub in lsubprogramas.Where(sub => sub.IdSubprograma == f.IdSubprograma).DefaultIfEmpty()
                                                join sub in lsubprogramas on f.IdSubprograma equals sub.IdSubprograma
                                                where   f.IdSubprograma != null
                                                select f).ToList();
                            }
                        }

                    
                    //**********************************************
                }
                else
                {
                    // 23/04/2015 ---------------------------------------
                    var lprogr = from rp in _programarepositorio.GetTipoFichasRol()
                                 join u in usuarioRoles on rp.Id_Rol equals u.IdRol
                                 group rp by new { rp.IdPrograma, rp.NombrePrograma } into grouping
                                 select new
                                     TipoFicha
                                 {
                                     Id_Tipo_Ficha = grouping.Key.IdPrograma,
                                     Nombre_Tipo_Ficha = grouping.Key.NombrePrograma
                                 };
                    vista.Fichas = (from f in vista.Fichas

                               join pro in lprogr on f.TipoFicha equals pro.Id_Tipo_Ficha
                               select f).ToList();
                    // --------------------------------------------------

                }

                if (TipoPrograma > 0) // si se consulta por tipo programa
                {

                    vista.Fichas = vista.Fichas.Where(c => c.TipoPrograma == TipoPrograma).ToList();

                }
                cantreg = vista.Fichas.Count();
            }
            else
            {
                vista.ProgPorRol = "N";
                //cantreg = accesoprograma
                //                  ? _fichaRepositorio.GetFichas(cuil, dni, nombre, apellido, tipoficha, estadoficha, idEtapa).Count
                //                  : _fichaRepositorio.GetFichas(cuil, dni, nombre, apellido, tipoficha, estadoficha, idEtapa).Where(
                //                      c => c.TipoFicha != (int)Enums.TipoFicha.EfectoresSociales).Count();
                vista.Fichas = accesoprograma
                                  ? _fichaRepositorio.GetFichas(cuil, dni, nombre, apellido, tipoficha, estadoficha, idEtapa, nrotramite, TipoPrograma).ToList()
                                  : _fichaRepositorio.GetFichas(cuil, dni, nombre, apellido, tipoficha, estadoficha, idEtapa, nrotramite, TipoPrograma).Where(
                                      c => c.TipoFicha != (int)Enums.TipoFicha.EfectoresSociales).ToList();
                if (idSubprograma > 0) // si se consulta por subprograma
                {

                    vista.Fichas = vista.Fichas.Where(c => c.IdSubprograma == idSubprograma).ToList();

                }

                if (TipoPrograma > 0) // si se consulta por tipo programa
                {

                    vista.Fichas = vista.Fichas.Where(c => c.TipoPrograma == TipoPrograma).ToList();

                }
                cantreg = vista.Fichas.Count();

            }



            var pager = new Pager(cantreg,
                                  Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                                  "FormIndexFichas", _aut.GetUrl("IndexPager", "Fichas"));

            if (pager.TotalCount != pPager.TotalCount)
            {
                pager.PageNumber = 1;
                pager.Skip = 0;
                vista.pager = pager;
            }
            else
            {
                vista.pager = pPager;
            }


            //***********VERIFICAR SI DEBE APLICAR LA ACCIÓN BUSQUEDA POR PROGRAMAS Y SUBPROGRAMAS********************************



            if (accesoprogr) //verifica si debe traer todos los programas asociados
            {
                //vista.Fichas = _fichaRepositorio.GetFichas(cuil, dni, nombre, apellido, tipoficha, estadoficha, vista.pager.Skip,
                //                                                           vista.pager.PageSize, idEtapa);
                //if (tipoficha > 5 && tipoficha < 9)
                //{
                //    vista.Fichas = vista.Fichas.Where(c => c.IdSubprograma == idSubprograma).ToList(); 
                //}
                //else
                //{

                //}
                vista.Fichas = vista.Fichas.Skip(vista.pager.Skip).Take(vista.pager.PageSize).ToList();
            }
            else
            {
                //if (accesoprograma)
                //    vista.Fichas = _fichaRepositorio.GetFichas(cuil, dni, nombre, apellido, tipoficha, estadoficha, vista.pager.Skip,
                //                                                           vista.pager.PageSize, idEtapa);
                //else
                //    vista.Fichas = _fichaRepositorio.GetFichas(cuil, dni, nombre, apellido, tipoficha, estadoficha,
                //                                               vista.pager.Skip,
                //                                               vista.pager.PageSize, (int)Enums.TipoFicha.EfectoresSociales, idEtapa);
                vista.Fichas = vista.Fichas.Skip(vista.pager.Skip).Take(vista.pager.PageSize).ToList();
            }
            //********************************************************************************************************************
            vista.cuil_busqueda = cuil;
            vista.nro_doc_busqueda = dni;
            vista.nombre_busqueda = nombre;
            vista.apellido_busqueda = apellido;

            CargarEstadosFichas(vista, _estadoFichaRepositorio.GetEstadosFicha());

            //**************CARGAR EL COMBO PROGRAMAS O TIPO DE FICHA EN INSCRIPCIONES***********************************
            

            //var roles =  base.GetUsuarioLoguer().ListaRoles();


            if (accesoprogr) //verifica si debe traer todos los programas asociados
            {

                var lprogramas = from rp in _programarepositorio.GetTipoFichasRol()
                                 join u in usuarioRoles on rp.Id_Rol equals u.IdRol
                                 group rp by new { rp.IdPrograma, rp.NombrePrograma } into grouping
                                 select new
                                     TipoFicha
                                 {
                                     Id_Tipo_Ficha = grouping.Key.IdPrograma,
                                     Nombre_Tipo_Ficha = grouping.Key.NombrePrograma
                                 };

                CargarTipoFichasRol(vista, lprogramas.OrderBy(c => c.Id_Tipo_Ficha));
            }
            else
            {

                CargarTipoFichas(vista, _tipoFichaRepositorio.GetTiposFicha());
            }
            //*****************************************************************************************

            vista.ComboEstadoFicha.Selected = estadoficha.ToString();

            //vista.ComboTipoFicha.Selected = tipoficha.ToString();

            var lstProgramas = _programarepositorio.GetProgramas();

            // 10/06/2013 - DI CAMPLI LEANDRO - CARGAN LOS PROGRAMAS PARA LAS ETAPAS
            var tiposppp = _tipopppRepositorio.GetTiposPpp();
            vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = 0, Description = "Todos" });
            foreach (var tipo in tiposppp)
            {
                vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = tipo.ID_TIPO_PPP, Description = tipo.N_TIPO_PPP });
            }
            //CargarProgramaEtapa(vista, lstProgramas);
            //vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = 0, Description = "Todos" });
            //vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = 1, Description = "PRIMER PASO" });
            //vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = 2, Description = "APRENDIZ" });
            //vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = 3, Description = "XMÍ" });
            //vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = 4, Description = "PILA" });
            //vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = 5, Description = "PIP" });
            //vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = 6, Description = "CLIP" });
            //vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = 7, Description = "PIL - COMERCIO EXTERIOR" });
            //vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = 8, Description = "PIL - COMERCIO ELECTRONICO" });
            //vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = 9, Description = "PIL - MAQUINARIA AGRICOLA" });
            //vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = 10, Description = "PIL - TURISMO" });
            return vista;
        }

        public IFichaVista GetFicha(IFichaVista ficha, string accion)
        {
            ficha.Accion = accion;
            if (accion == "Agregar")
                ficha.TipoFicha = Convert.ToInt32(ficha.ComboTipoFicha.Selected);

            ficha.NombreTipoFicha = Enums.GetTipoFicha(ficha.TipoFicha);

            CargarSexo(ficha);
            CargarTipoDocumento(ficha);
            CargarDepartamento(ficha, _departamentoRepositorio.GetDepartamentos());
            CargarSectorProductivo(ficha, _sectorrepositorio.GetSectores()); // 23/02/2013 - DI CAMPLI LEANDRO - QUITAR HARCODEO DE SECTORES PRODUCTIVOS
            CargarInstitucion(ficha, _institucionRepositorio.GetInstitucionesBySectorProductivo(Convert.ToInt32(ficha.SectorProductivo.Selected)));//.GetInstitucionesByNivel(ficha.TipoFicha));

            CargarEstadoCivil(ficha, _estadoCivilRepositorio.GetEstadosCivil());
            CargarLocalidad(ficha, _localidadRepositorio.GetLocalidadesByDepto(Convert.ToInt32(ficha.Departamentos.Selected)));
            CargarLocalidadEmpresa(ficha, _localidadRepositorio.GetLocalidades());
            CargarLocalidadSede(ficha, _localidadRepositorio.GetLocalidades());

            CargarLocalidadEscuela(ficha, _localidadRepositorio.GetLocalidadesByDepto(Convert.ToInt32(ficha.DepartamentosEscuela.Selected)));
            CargarEstadosFicha(ficha, _estadoFichaRepositorio.GetEstadosFicha());
            CargarTipoFichaAlta(ficha, _tipoFichaRepositorio.GetTiposFicha());
            CargarEstadosFichaAlta(ficha);
            CargarComboSiNo(ficha);

            // apoderado
            CargarLocalidadApo(ficha, _localidadRepositorio.GetLocalidadesByDepto(Convert.ToInt32(ficha.DepartamentosApo.Selected)));


            switch (ficha.TipoFicha)
            {
                case (int)Enums.TipoFicha.Terciaria:
                    CargarCarrera(ficha, _carreraRepositorio.GetCarrerasByInstitucionBySector(Convert.ToInt32(ficha.Institucion.Selected), Convert.ToInt32(ficha.SectorProductivo.Selected)));
                    CargarEscuela(ficha, _escuelaRepositorio.GetEscuelasByLocalidad(Convert.ToInt32(ficha.LocalidadesEscuela.Selected)));
                    ficha.IdConvertirA = (int)Enums.TipoFicha.Universitaria;
                    ficha.ConvertirA = "Universitaria";
                    break;
                case (int)Enums.TipoFicha.Universitaria:
                    CargarCarrera(ficha, _carreraRepositorio.GetCarrerasByInstitucionBySector(Convert.ToInt32(ficha.Institucion.Selected), Convert.ToInt32(ficha.SectorProductivo.Selected)));
                    CargarEscuela(ficha, _escuelaRepositorio.GetEscuelasByLocalidad(Convert.ToInt32(ficha.LocalidadesEscuela.Selected)));
                    ficha.IdConvertirA = (int)Enums.TipoFicha.Terciaria;
                    ficha.ConvertirA = "Terciaria";
                    break;
                case (int)Enums.TipoFicha.Ppp:
                    //01/08/2018
                    int? sector = int.Parse(string.IsNullOrEmpty(ConfigurationManager.AppSettings["sector"]) ? "0" : ConfigurationManager.AppSettings["sector"]);
                        
                    if (ficha.IdFicha == 0)
                    {
                        ficha.Accion = "Agregar";
                        String ppp_aprendiz = "";
                        ppp_aprendiz = ficha.FichaPpp.ppp_aprendiz;
                        string tipoppp = "";
                        tipoppp = Convert.ToString(ficha.tipoPrograma ?? 1);
                        ficha.FichaPpp = _fichaRepositorio.GetFormFichaPpp();
                        ficha.FichaPpp.Modalidad = (int)Enums.Modalidad.Entrenamiento;
                        ficha.FichaPpp.IdNivelEscolaridad = (int)Enums.NivelEscolaridad.Ninguno;
                        ficha.FichaPpp.AltaTemprana = "N";
                        ficha.FichaPpp.ppp_aprendiz = ppp_aprendiz;
                        CargarModalidadAfip(ficha, _modalidadContratacionAfipRepositorio.GetModalidadesContratacionAFIP());

                        var tiposppp = _tipopppRepositorio.GetTiposPpp();
                        
                        foreach (var tipo in tiposppp)
                        {
                            ficha.ComboTiposPpp.Combo.Add(new ComboItem { Id = tipo.ID_TIPO_PPP, Description = tipo.N_TIPO_PPP });
                        }
                        //ficha.ComboTiposPpp.Combo.Add(new ComboItem { Id = 1, Description = "PRIMER PASO" });
                        //ficha.ComboTiposPpp.Combo.Add(new ComboItem { Id = 2, Description = "APRENDIZ" });
                        //ficha.ComboTiposPpp.Combo.Add(new ComboItem { Id = 3, Description = "XMÍ" });
                        //ficha.ComboTiposPpp.Combo.Add(new ComboItem { Id = 4, Description = "PILA" });
                        //ficha.ComboTiposPpp.Combo.Add(new ComboItem { Id = 5, Description = "PIP" });
                        //ficha.ComboTiposPpp.Combo.Add(new ComboItem { Id = 6, Description = "CLIP" });
                        //ficha.ComboTiposPpp.Combo.Add(new ComboItem { Id = 7, Description = "PIL - COMERCIO EXTERIOR" });
                        //ficha.ComboTiposPpp.Combo.Add(new ComboItem { Id = 8, Description = "PIL - COMERCIO ELECTRONICO" });
                        //ficha.ComboTiposPpp.Combo.Add(new ComboItem { Id = 9, Description = "PIL - MAQUINARIA AGRICOLA" });
                        //ficha.ComboTiposPpp.Combo.Add(new ComboItem { Id = 10, Description = "PIL - TURISMO" });
                        ficha.ComboTiposPpp.Selected = tipoppp;
                        ficha.tipoPrograma = Convert.ToInt32(tipoppp);

                        //01/08/2018
                        CargarInstitucionSector(ficha, _institucionRepositorio.GetInstitucionesBySector((int)sector));
                        ficha.InstitucionSector.Selected = "0";
                        ficha.CarreraSector.Combo.Add(new ComboItem { Id = 0, Description = "SELECCIONE UNA CARRERA..." });

                    }
                    else
                    {
                        ficha.FichaPpp = _fichaRepositorio.GetFichaPpp(ficha.IdFicha);
                        CargarModalidadAfip(ficha, _modalidadContratacionAfipRepositorio.GetModalidadesContratacionAFIP());
                        CargarInstitucionSector(ficha, _institucionRepositorio.GetInstitucionesBySector((int)sector));
                        ficha.InstitucionSector.Selected = "0";
                        //Buscar el IdInstitucion por carrea
                        if ((int)ficha.FichaPpp.id_carrera != 0)
                        {

                            ICarrera carrera = new Carrera();
                            carrera = _carreraRepositorio.GetCarrera((int)ficha.FichaPpp.id_carrera);


                            if (carrera.IdInstitucion != null)
                            {
                                ficha.InstitucionSector.Selected = carrera.IdInstitucion.ToString();
                            }
                            else
                            {
                                ficha.InstitucionSector.Selected = "0";
                            }


                            CargarCarreraSector(ficha,
                                          _carreraRepositorio.GetCarrerasByInstitucion(carrera.IdInstitucion));
                            ficha.CarreraSector.Selected = ficha.FichaPpp.id_carrera.ToString();
                            carrera = null;
                        }
                        else
                        {
                            ficha.CarreraSector.Combo.Add(new ComboItem { Id = 0, Description = "SELECCIONE UNA CARRERA..." });
                        }
                    }

                    break;
                case (int)Enums.TipoFicha.Vat:
                    if (ficha.IdFicha == 0)
                    {
                        ficha.Accion = "Agregar";
                        ficha.FichaVat = new FichaVAT
                                             {
                                                 Modalidad = (int)Enums.Modalidad.Entrenamiento,
                                                 IdNivelEscolaridad = (int)Enums.NivelEscolaridad.Ninguno,
                                                 AltaTemprana = "N"
                                             };
                        CargarModalidadAfip(ficha, _modalidadContratacionAfipRepositorio.GetModalidadesContratacionAFIP());
                    }
                    else
                    {
                        ficha.FichaVat = _fichaRepositorio.GetFichaVat(ficha.IdFicha);
                        CargarModalidadAfip(ficha, _modalidadContratacionAfipRepositorio.GetModalidadesContratacionAFIP());
                    }
                    break;
                case (int)Enums.TipoFicha.PppProf:
                    if (ficha.IdFicha == 0)
                    {
                        ficha.Accion = "Agregar";
                        ficha.FichaPppp = new FichaPPPP
                        {
                            AltaTemprana = "N",
                            Modalidad = (int)Enums.Modalidad.Entrenamiento
                        };
                        CargarModalidadAfip(ficha, _modalidadContratacionAfipRepositorio.GetModalidadesContratacionAFIP());
                        IList<IUniversidad> listauniversidad = _universidadRepositorio.GetUniversidades();
                        if (listauniversidad.Count > 0)
                        {
                            int iduniversidad = Convert.ToInt32(ficha.ComboUniversidad.Selected);//listauniversidad[0].IdUniversidad;

                            CargarTitulos(ficha, _tituloRepositorio.GetTitulosByUniversidad(iduniversidad));
                        }

                        CargarUniversidades(ficha, listauniversidad);
                    }
                    else
                    {
                        ficha.FichaPppp = _fichaRepositorio.GetFichaPppp(ficha.IdFicha);
                        CargarModalidadAfip(ficha, _modalidadContratacionAfipRepositorio.GetModalidadesContratacionAFIP());
                        //ITitulo titulo = _tituloRepositorio.GetTitulo(ficha.FichaPppp.IdTitulo ?? 0);
                        IList<IUniversidad> listauniversidad = _universidadRepositorio.GetUniversidades();
                        if (listauniversidad.Count > 0)
                        {
                            int iduniversidad = Convert.ToInt32(ficha.ComboUniversidad.Selected);//listauniversidad[0].IdUniversidad;

                            CargarTitulos(ficha, _tituloRepositorio.GetTitulosByUniversidad(iduniversidad));
                        }

                        CargarUniversidades(ficha, listauniversidad);
                    }
                    break;
                case (int)Enums.TipoFicha.ReconversionProductiva:
                    if (ficha.IdFicha == 0)
                    {
                        ficha.Accion = "Agregar";
                        ficha.FichaReconversion = _fichaRepositorio.GetFormFichaReconversion();
                        ficha.FichaReconversion.Modalidad = (int)Enums.Modalidad.Entrenamiento;
                        ficha.FichaReconversion.IdNivelEscolaridad = (int)Enums.NivelEscolaridad.Ninguno;
                        ficha.FichaReconversion.AltaTemprana = "N";
                        CargarModalidadAfip(ficha, _modalidadContratacionAfipRepositorio.GetModalidadesContratacionAFIP());
                        CargarSubprogramas(ficha, _subprogramaRepositorio.GetSubprogramas());
                    }
                    else
                    {
                        ficha.FichaReconversion = _fichaRepositorio.GetFichaReconversion(ficha.IdFicha);
                        CargarSubprogramas(ficha, _subprogramaRepositorio.GetSubprogramas());
                        CargarModalidadAfip(ficha, _modalidadContratacionAfipRepositorio.GetModalidadesContratacionAFIP());
                    }
                    break;
                case (int)Enums.TipoFicha.EfectoresSociales:
                    if (ficha.IdFicha == 0)
                    {
                        ficha.Accion = "Agregar";
                        ficha.FichaEfectores = _fichaRepositorio.GetFormFichaEfectores();
                        ficha.FichaEfectores.Modalidad = (int)Enums.Modalidad.Entrenamiento;
                        ficha.FichaEfectores.IdNivelEscolaridad = (int)Enums.NivelEscolaridad.Ninguno;
                        ficha.FichaEfectores.AltaTemprana = "N";
                        CargarModalidadAfip(ficha, _modalidadContratacionAfipRepositorio.GetModalidadesContratacionAFIP());
                        CargarSubprogramas(ficha, _subprogramaRepositorio.GetSubprogramas());
                    }
                    else
                    {
                        ficha.FichaEfectores = _fichaRepositorio.GetFichaEfectores(ficha.IdFicha);
                        CargarSubprogramas(ficha, _subprogramaRepositorio.GetSubprogramas());
                        CargarModalidadAfip(ficha, _modalidadContratacionAfipRepositorio.GetModalidadesContratacionAFIP());
                    }
                    break;

                case (int)Enums.TipoFicha.ConfiamosEnVos:
                    if (ficha.IdFicha == 0)
                    {
                        ficha.Accion = "Agregar";
                        ficha.FichaConfVos = _fichaRepositorio.GetFormFichaConfVos();
                        ficha.FichaConfVos.Modalidad = (int)Enums.Modalidad.Entrenamiento;
                        ficha.FichaConfVos.IdNivelEscolaridad = (int)Enums.NivelEscolaridad.Ninguno;
                        ficha.FichaConfVos.AltaTemprana = "N";
                        CargarModalidadAfip(ficha, _modalidadContratacionAfipRepositorio.GetModalidadesContratacionAFIP());
                        CargarSubprogramas(ficha, _subprogramaRepositorio.GetSubprogramas());
                        CargarEscuela(ficha, _escuelaRepositorio.GetEscuelasByLocalidad(Convert.ToInt32(ficha.LocalidadesEscuela.Selected)));
                        //CargarEscuela(ficha, _escuelaRepositorio.GetEscuelasByLocalidad(Convert.ToInt32(ficha.LocalidadesEscuela.Selected)));
                        //Cargar periodos de abandono de cursado
                        CargarAbandonoCursado(ficha, _abandonocursadoRepositorio.GetAllcbo());
                    }
                    else
                    {
                        ficha.FichaConfVos = _fichaRepositorio.GetFichaConfVos(ficha.IdFicha);
                        CargarSubprogramas(ficha, _subprogramaRepositorio.GetSubprogramas());
                        CargarModalidadAfip(ficha, _modalidadContratacionAfipRepositorio.GetModalidadesContratacionAFIP());
                        CargarEscuela(ficha, _escuelaRepositorio.GetEscuelasByLocalidad(Convert.ToInt32(ficha.LocalidadesEscuela.Selected)));
                        CargarAbandonoCursado(ficha, _abandonocursadoRepositorio.GetAllcbo());
                    }
                    break;
                default:
                    break;
            }

            if (ficha.Accion == "Agregar")
            {
                if (ficha.ComboEstadoFicha.Selected.Equals(((int)Enums.EstadoFicha.RechazoFormal).ToString()))
                {
                    ficha.TipoRechazos = GetTiposRechazos();
                }
            }

            ficha.TipoDocumentoCombo.Enabled = true;
            ficha.Sexo.Enabled = true;
            ficha.Departamentos.Enabled = true;
            ficha.Localidades.Enabled = true;
            ficha.SectorProductivo.Enabled = true;
            ficha.Institucion.Enabled = true;
            ficha.Carrera.Enabled = true;
            ficha.DepartamentosEscuela.Enabled = true;
            ficha.LocalidadesEscuela.Enabled = true;
            ficha.Escuelas.Enabled = true;
            ficha.EstadoCivil.Enabled = true;
            ficha.ComboEstadoFicha.Enabled = true;
            ficha.ComboTipoFicha.Enabled = true;

            // 21/04/2014
            ficha.TipoDocumentoApoCombo.Enabled = true;
            ficha.SexoApo.Enabled = true;
            ficha.LocalidadesApo.Enabled = true;
            ficha.DepartamentosApo.Enabled = true;
            ficha.EstadoCivilApo.Enabled = true;


            return ficha;
        }

        public IImportarExcelVista ImportarExcel(HttpPostedFileBase file)
        {
            IImportarExcelVista vista = new ImportarExcelVista();

            if (file != null && file.ContentLength > 0)
            {
                // extract only the fielname
                var fileName = Path.GetFileName(file.FileName);
                var nombre = DateTime.Now.Year + DateTime.Now.Month.ToString() + DateTime.Now.Day +
                             DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + ".xls";

                // store the file inside ~/App_Data/uploads folder
                var path = Path.Combine(HttpContext.Current.Server.MapPath("~/Archivos/Importados"), nombre);
                file.SaveAs(path);

                vista.Importado = true;
                vista.Mensaje = "El archivo \"" + fileName + "\" fue subido al servidor correctamente. Confirme la acción de Procesar si lo cree conveniente.";
                vista.Archivo = nombre;
            }
            else
            {
                vista.Importado = true;
                vista.Mensaje = "El archivo no pudo ser subido al servidor. Vuelva a intentar.";
            }

            return vista;
        }

        public bool ProcesarExcel(IImportarExcelVista vista)
        {
            string path = HttpContext.Current.Server.MapPath(@"\Archivos\Importados") + @"\" + vista.Archivo;

            string connectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";Extended Properties=\"Excel 8.0;HDR=YES;\"";

            DbProviderFactory factory = DbProviderFactories.GetFactory("System.Data.OleDb");

            using (DbConnection connection = factory.CreateConnection())
            {
                if (connection != null)
                {
                    connection.ConnectionString = connectionString;
                    connection.Open();

                    using (DbCommand command = connection.CreateCommand())
                    {
                        var ds = new DataSet();

                        var dbDataAdapter = factory.CreateDataAdapter();
                        command.CommandText = "SELECT * FROM [Fichas$]";
                        if (dbDataAdapter != null)
                        {
                            dbDataAdapter.SelectCommand = command;
                            dbDataAdapter.Fill(ds);
                        }

                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            int idFicha;
                            int idEstadoFicha;

                            if (int.TryParse(row["ID Ficha"].ToString(), out idFicha) &&
                                int.TryParse(row["Estado"].ToString(), out idEstadoFicha))
                            {
                                _fichaRepositorio.UpdateEstadoFicha(idFicha, idEstadoFicha);
                            }
                        }
                    }
                }
            }

            return true;
        }

        public bool ConvertirFicha(int idFicha)
        {
            return _fichaRepositorio.ConvertirFicha(idFicha);
        }


        private IFichaVista CargarHorarioLunes(IFichaVista vista)
        {
            IList<IHorarioEmpresa> listaHorarioEmpresa = _horarioempresarepositorio.GetHorariosEmpresa(vista.IdEmpresa, "01");

            int countcorte = _horarioficharepositorio.CountCortes(vista.IdFicha, "01") == 0 ? 1 : _horarioficharepositorio.CountCortes(vista.IdFicha, "01");

            IList<IHorarioFicha> listahorarioficha = _horarioficharepositorio.GetHorariosFichaByDia(vista.IdFicha, "01");

            for (int i = 0; i <= countcorte - 1; i++)
            {
                var combo = new ComboBox();
                combo.Combo.Add(new ComboItem
                                    {
                                        Id = 0,
                                        Description = "Seleccione..."

                                    });

                foreach (var horarioEmpresa in listaHorarioEmpresa)
                {


                    combo.Combo.Add(new ComboItem
                                        {
                                            Id = horarioEmpresa.IdEmpresaHorario,
                                            Description =
                                                (horarioEmpresa.HoraDesde.ToString().PadLeft(2, '0') + ":" +
                                                 horarioEmpresa.MinutosDesde.ToString().PadLeft(2, '0') + " - " +
                                                 horarioEmpresa.HoraHasta.ToString().PadLeft(2, '0') + ":" +
                                                 horarioEmpresa.MinutosHasta.ToString().PadLeft(2, '0'))
                                        });
                }

                IHorarioFicha horarioficha = listahorarioficha.Where(c => c.Corte == i).FirstOrDefault();
                if (vista.Accion == "Ver")
                {
                    combo.Enabled = false;
                }
                if (horarioficha != null)
                {
                    combo.Selected = horarioficha.IdEmpresaHorario.ToString();

                    vista.ObsLunes.Add(horarioficha.Observacion);
                }
                else
                {
                    vista.ObsLunes.Add("");
                }
                vista.HorarioLunes.Add(combo);
            }

            return vista;
        }

        private IFichaVista CargarHorarioMartes(IFichaVista vista)
        {
            IList<IHorarioEmpresa> listaHorarioEmpresa = _horarioempresarepositorio.GetHorariosEmpresa(vista.IdEmpresa, "02");

            int countcorte = _horarioficharepositorio.CountCortes(vista.IdFicha, "02") == 0 ? 1 : _horarioficharepositorio.CountCortes(vista.IdFicha, "02");


            IList<IHorarioFicha> listahorarioficha = _horarioficharepositorio.GetHorariosFichaByDia(vista.IdFicha, "02");

            for (int i = 0; i <= countcorte - 1; i++)
            {
                var combo = new ComboBox();
                combo.Combo.Add(new ComboItem
                {
                    Id = 0,
                    Description = "Seleccione..."

                });

                foreach (var horarioEmpresa in listaHorarioEmpresa)
                {


                    combo.Combo.Add(new ComboItem
                    {
                        Id = horarioEmpresa.IdEmpresaHorario,
                        Description =
                            (horarioEmpresa.HoraDesde.ToString().PadLeft(2, '0') + ":" +
                             horarioEmpresa.MinutosDesde.ToString().PadLeft(2, '0') + " - " +
                             horarioEmpresa.HoraHasta.ToString().PadLeft(2, '0') + ":" +
                             horarioEmpresa.MinutosHasta.ToString().PadLeft(2, '0'))
                    });
                }

                IHorarioFicha horarioficha = listahorarioficha.Where(c => c.Corte == i).FirstOrDefault();
                if (vista.Accion == "Ver")
                {
                    combo.Enabled = false;
                }

                if (horarioficha != null)
                {
                    combo.Selected = horarioficha.IdEmpresaHorario.ToString();
                    vista.ObsMartes.Add(horarioficha.Observacion);
                }
                else
                {
                    vista.ObsMartes.Add("");
                }
                vista.HorarioMartes.Add(combo);
            }

            return vista;
        }

        private IFichaVista CargarHorarioMiercoles(IFichaVista vista)
        {
            IList<IHorarioEmpresa> listaHorarioEmpresa = _horarioempresarepositorio.GetHorariosEmpresa(vista.IdEmpresa, "03");

            int countcorte = _horarioficharepositorio.CountCortes(vista.IdFicha, "03") == 0 ? 1 : _horarioficharepositorio.CountCortes(vista.IdFicha, "03");

            IList<IHorarioFicha> listahorarioficha = _horarioficharepositorio.GetHorariosFichaByDia(vista.IdFicha, "03");

            for (int i = 0; i <= countcorte - 1; i++)
            {
                var combo = new ComboBox();
                combo.Combo.Add(new ComboItem
                {
                    Id = 0,
                    Description = "Seleccione..."

                });

                foreach (var horarioEmpresa in listaHorarioEmpresa)
                {


                    combo.Combo.Add(new ComboItem
                    {
                        Id = horarioEmpresa.IdEmpresaHorario,
                        Description =
                            (horarioEmpresa.HoraDesde.ToString().PadLeft(2, '0') + ":" +
                             horarioEmpresa.MinutosDesde.ToString().PadLeft(2, '0') + " - " +
                             horarioEmpresa.HoraHasta.ToString().PadLeft(2, '0') + ":" +
                             horarioEmpresa.MinutosHasta.ToString().PadLeft(2, '0'))
                    });
                }

                IHorarioFicha horarioficha = listahorarioficha.Where(c => c.Corte == i).FirstOrDefault();
                if (vista.Accion == "Ver")
                {
                    combo.Enabled = false;
                }

                if (horarioficha != null)
                {
                    combo.Selected = horarioficha.IdEmpresaHorario.ToString();
                    vista.ObsMiercoles.Add(horarioficha.Observacion);
                }
                else
                {
                    vista.ObsMiercoles.Add("");
                }
                vista.HorarioMiercoles.Add(combo);
            }

            return vista;
        }

        private IFichaVista CargarHorarioJueves(IFichaVista vista)
        {
            IList<IHorarioEmpresa> listaHorarioEmpresa = _horarioempresarepositorio.GetHorariosEmpresa(vista.IdEmpresa, "04");

            int countcorte = _horarioficharepositorio.CountCortes(vista.IdFicha, "04") == 0 ? 1 : _horarioficharepositorio.CountCortes(vista.IdFicha, "04");

            IList<IHorarioFicha> listahorarioficha = _horarioficharepositorio.GetHorariosFichaByDia(vista.IdFicha, "04");

            for (int i = 0; i <= countcorte - 1; i++)
            {
                var combo = new ComboBox();
                combo.Combo.Add(new ComboItem
                {
                    Id = 0,
                    Description = "Seleccione..."

                });

                foreach (var horarioEmpresa in listaHorarioEmpresa)
                {


                    combo.Combo.Add(new ComboItem
                    {
                        Id = horarioEmpresa.IdEmpresaHorario,
                        Description =
                            (horarioEmpresa.HoraDesde.ToString().PadLeft(2, '0') + ":" +
                             horarioEmpresa.MinutosDesde.ToString().PadLeft(2, '0') + " - " +
                             horarioEmpresa.HoraHasta.ToString().PadLeft(2, '0') + ":" +
                             horarioEmpresa.MinutosHasta.ToString().PadLeft(2, '0'))
                    });
                }

                IHorarioFicha horarioficha = listahorarioficha.Where(c => c.Corte == i).FirstOrDefault();
                if (vista.Accion == "Ver")
                {
                    combo.Enabled = false;
                }

                if (horarioficha != null)
                {
                    combo.Selected = horarioficha.IdEmpresaHorario.ToString();
                    vista.ObsJueves.Add(horarioficha.Observacion);
                }
                else
                {
                    vista.ObsJueves.Add("");
                }
                vista.HorarioJueves.Add(combo);
            }

            return vista;
        }

        private IFichaVista CargarHorarioViernes(IFichaVista vista)
        {
            IList<IHorarioEmpresa> listaHorarioEmpresa = _horarioempresarepositorio.GetHorariosEmpresa(vista.IdEmpresa, "05");

            int countcorte = _horarioficharepositorio.CountCortes(vista.IdFicha, "05") == 0 ? 1 : _horarioficharepositorio.CountCortes(vista.IdFicha, "05");

            IList<IHorarioFicha> listahorarioficha = _horarioficharepositorio.GetHorariosFichaByDia(vista.IdFicha, "05");

            for (int i = 0; i <= countcorte - 1; i++)
            {
                var combo = new ComboBox();
                combo.Combo.Add(new ComboItem
                {
                    Id = 0,
                    Description = "Seleccione..."

                });

                foreach (var horarioEmpresa in listaHorarioEmpresa)
                {


                    combo.Combo.Add(new ComboItem
                    {
                        Id = horarioEmpresa.IdEmpresaHorario,
                        Description =
                            (horarioEmpresa.HoraDesde.ToString().PadLeft(2, '0') + ":" +
                             horarioEmpresa.MinutosDesde.ToString().PadLeft(2, '0') + " - " +
                             horarioEmpresa.HoraHasta.ToString().PadLeft(2, '0') + ":" +
                             horarioEmpresa.MinutosHasta.ToString().PadLeft(2, '0'))
                    });
                }

                IHorarioFicha horarioficha = listahorarioficha.Where(c => c.Corte == i).FirstOrDefault();
                if (vista.Accion == "Ver")
                {
                    combo.Enabled = false;
                }

                if (horarioficha != null)
                {
                    combo.Selected = horarioficha.IdEmpresaHorario.ToString();
                    vista.ObsViernes.Add(horarioficha.Observacion);
                }
                else
                {
                    vista.ObsViernes.Add("");
                }
                vista.HorarioViernes.Add(combo);
            }

            return vista;
        }

        private IFichaVista CargarHorarioSabado(IFichaVista vista)
        {
            IList<IHorarioEmpresa> listaHorarioEmpresa = _horarioempresarepositorio.GetHorariosEmpresa(vista.IdEmpresa, "06");

            int countcorte = _horarioficharepositorio.CountCortes(vista.IdFicha, "06") == 0 ? 1 : _horarioficharepositorio.CountCortes(vista.IdFicha, "06");

            IList<IHorarioFicha> listahorarioficha = _horarioficharepositorio.GetHorariosFichaByDia(vista.IdFicha, "06");

            for (int i = 0; i <= countcorte - 1; i++)
            {
                var combo = new ComboBox();
                combo.Combo.Add(new ComboItem
                {
                    Id = 0,
                    Description = "Seleccione..."

                });

                foreach (var horarioEmpresa in listaHorarioEmpresa)
                {
                    combo.Combo.Add(new ComboItem
                    {
                        Id = horarioEmpresa.IdEmpresaHorario,
                        Description =
                            (horarioEmpresa.HoraDesde.ToString().PadLeft(2, '0') + ":" +
                             horarioEmpresa.MinutosDesde.ToString().PadLeft(2, '0') + " - " +
                             horarioEmpresa.HoraHasta.ToString().PadLeft(2, '0') + ":" +
                             horarioEmpresa.MinutosHasta.ToString().PadLeft(2, '0'))
                    });
                }

                IHorarioFicha horarioficha = listahorarioficha.Where(c => c.Corte == i).FirstOrDefault();
                if (vista.Accion == "Ver")
                {
                    combo.Enabled = false;
                }

                if (horarioficha != null)
                {
                    combo.Selected = horarioficha.IdEmpresaHorario.ToString();
                    vista.ObsSabado.Add(horarioficha.Observacion);
                }
                else
                {
                    vista.ObsSabado.Add("");
                }
                vista.HorarioSabado.Add(combo);
            }

            return vista;
        }

        private IFichaVista CargarHorarioDomingo(IFichaVista vista)
        {
            IList<IHorarioEmpresa> listaHorarioEmpresa = _horarioempresarepositorio.GetHorariosEmpresa(vista.IdEmpresa, "07");

            int countcorte = _horarioficharepositorio.CountCortes(vista.IdFicha, "07") == 0 ? 1 : _horarioficharepositorio.CountCortes(vista.IdFicha, "07");

            IList<IHorarioFicha> listahorarioficha = _horarioficharepositorio.GetHorariosFichaByDia(vista.IdFicha, "07");

            for (int i = 0; i <= countcorte - 1; i++)
            {
                var combo = new ComboBox();
                combo.Combo.Add(new ComboItem
                {
                    Id = 0,
                    Description = "Seleccione..."

                });

                foreach (var horarioEmpresa in listaHorarioEmpresa)
                {


                    combo.Combo.Add(new ComboItem
                    {
                        Id = horarioEmpresa.IdEmpresaHorario,
                        Description =
                            (horarioEmpresa.HoraDesde.ToString().PadLeft(2, '0') + ":" +
                             horarioEmpresa.MinutosDesde.ToString().PadLeft(2, '0') + " - " +
                             horarioEmpresa.HoraHasta.ToString().PadLeft(2, '0') + ":" +
                             horarioEmpresa.MinutosHasta.ToString().PadLeft(2, '0'))
                    });
                }

                IHorarioFicha horarioficha = listahorarioficha.Where(c => c.Corte == i).FirstOrDefault();
                if (vista.Accion == "Ver")
                {
                    combo.Enabled = false;
                }

                if (horarioficha != null)
                {
                    combo.Selected = horarioficha.IdEmpresaHorario.ToString();
                    vista.ObsDomingo.Add(horarioficha.Observacion);
                }
                else
                {
                    vista.ObsDomingo.Add("");
                }
                vista.HorarioDomingo.Add(combo);
            }

            return vista;
        }


        // 18/02/2013 - DI CAMPLI LEANDRO - ALTA MASIVA DE BENEFICIARIOS

        public IFichasVista SubirArchivoFichas(HttpPostedFileBase archivo, IFichasVista model)
        {
            if (archivo != null && archivo.ContentLength > 0)
            {
                var fileName = Path.GetFileName(archivo.FileName);
                var nombre = DateTime.Now.Year + DateTime.Now.Month.ToString() + DateTime.Now.Day +
                             DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + ".xls";

                var path = Path.Combine(HttpContext.Current.Server.MapPath("~/Archivos/Importados"), nombre);
                archivo.SaveAs(path);

                //Verifíco el Formato del Archivo
                var fileInfo = new FileInfo(path);

                IExcelDataReader excelReader = null;

                FileStream stream = File.Open(path, FileMode.Open, FileAccess.Read);

                string file = fileInfo.Name;

                if (file.Split('.')[1].Equals("xls"))
                {
                    excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                }
                else if (file.Split('.')[1].Equals("xlsx"))
                {
                    excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                }

                if (excelReader != null) excelReader.IsFirstRowAsColumnNames = true;
                var dsExcel = new DataSet();
                if (excelReader != null)
                {
                    dsExcel = excelReader.AsDataSet();
                    excelReader.Close();
                }
                if (dsExcel != null)
                {
                    if (dsExcel.Tables[0].Rows.Count > 0)
                    {
                        if (dsExcel.Tables[0].Columns.Contains("ID_FICHA"))
                        {
                            //model.Importado = true;
                            model.ProcesarExcel = true;
                            //model.Mensaje = "El archivo \"" + fileName + "\" fue subido al servidor correctamente. Confirme la acción de Procesar si lo cree conveniente.";
                            model.ExcelNombre = nombre;
                        }
                        else
                        {
                            //model.Importado = false;
                            model.ProcesarExcel = false;
                            //model.Mensaje = "El archivo \"" + fileName + "\" fue subido al servidor correctamente. Pero el Archivo no tiene el Formato correcto.";
                            model.ExcelNombre = null;
                            File.Delete(path);
                        }
                    }
                }
            }
            else
            {
                model.ProcesarExcel = true;
               // model.Mensaje = "El archivo no pudo ser subido al servidor. Vuelva a intentar.";
            }
            IList<IFicha> lista = new List<IFicha>();
            model.Fichas =lista;

            return model;
        }

        // 18/02/2013 - DI CAMPLI LEANDRO

        public IFichasVista indexAltaMasiviaBenef()
        {
            IFichasVista vista = new FichasVista();

            IList<IFicha> lista = new List<IFicha>();

            //var pager = new Pager(0, Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
            //                      "FormAltaMasivaBenef", _aut.GetUrl("IndexPager", "Fichas"));

            //vista.pager = pager;

            vista.Fichas = lista;

            //CargarEstadosFichas(vista, _estadoFichaRepositorio.GetEstadosFicha());
            
            CargarTipoFichas(vista, _tipoFichaRepositorio.GetTiposFicha());

            return vista;
        
        }


        public IFichasVista CargarMasivaFichasXLS(string ExcelUrl)
        {

            IFichasVista vista = new FichasVista();
            //********************15/02/2013 - DI CAMPLI LEANDRO -******************************
            vista.ExcelNombre = ExcelUrl;

            List<int> idBeneficiariosExport = new List<int>();

            string path = HttpContext.Current.Server.MapPath(@"\Archivos\Importados") + @"\" + ExcelUrl;

            vista.ExcelUrl = path;

            var fileInfo = new FileInfo(path);

            IExcelDataReader excelReader = null;

            FileStream stream = File.Open(path, FileMode.Open, FileAccess.Read);

            string file = fileInfo.Name;

            if (file.Split('.')[1].Equals("xls"))
            {
                excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
            }
            else if (file.Split('.')[1].Equals("xlsx"))
            {
                excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            }

            if (excelReader != null) excelReader.IsFirstRowAsColumnNames = true;
            var dsExcel = new DataSet();
            if (excelReader != null)
            {
                dsExcel = excelReader.AsDataSet();
                excelReader.Close();
            }
            if (dsExcel != null)
            {

                if (dsExcel.Tables[0].Rows.Count > 0)
                {

                    foreach (DataRow row in dsExcel.Tables[0].Rows)
                    {
                        idBeneficiariosExport.Add(Convert.ToInt32(row["ID_FICHA"].ToString()));

                    }
                }
            }

            //var pager = new Pager(0, Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
            //                      "FormIndexFichas", _aut.GetUrl("IndexPager", "Fichas"));
            //vista.pager = pager;
            vista.ProcesarExcel = true;
            //vista.Fichas = _fichaRepositorio.GetFichas().Where(c=>idBeneficiariosExport.Contains(c.IdFicha)).ToList();
            vista.Fichas = _fichaRepositorio.GetFichas(idBeneficiariosExport).ToList();

            return vista;
        }


        //20/02/2013 - DI CAMPLI LEANDRO

        public List<IFicha> GetFichaNotificacion(int[] idFichas)
        {
            List<IFicha> lstFichas = _fichaRepositorio.GetFichasNotificacion(idFichas).ToList();

            return lstFichas;

        
        }

        public List<IFicha> GetFichaNotificacion()
        {
            List<IFicha> lstFichas = _fichaRepositorio.GetFichasNotificacion().ToList();

            return lstFichas;


        }

        //21/10/2012 - DI CAMPLI LEANDRO - CARGA MASIVA DE BENEFICIARIOS

        public int UpdateCargaMasiva(IFichasVista fichas)

        {
            IComunDatos comun = new ComunDatos();
            FichaRepositorio f = new FichaRepositorio();
            f.AgregarDatos(comun);
            int cant = 0;
            T_FICHAS_CONF_VOS fcv = new T_FICHAS_CONF_VOS();
            IApoderado apoderado = new Apoderado(); ;
            foreach(IFicha ficha in fichas.Fichas)
            {
                if (ficha.IdEstadoFicha != (int)Enums.EstadoFicha.RechazoFormal && ficha.IdEstadoFicha != (int)Enums.EstadoFicha.Duplicado)
                {
                    //if (ficha.IdEstadoFicha == (int)Enums.EstadoFicha.Inscripto)
                    if (ficha.IdEstadoFicha == (int)Enums.EstadoFicha.Apta || ficha.IdEstadoFicha == (int)Enums.EstadoFicha.Inscripto)
                    {

                        //cambiar el estado de la ficha
                        _fichaRepositorio.UpdateEstadoFicha(ficha.IdFicha, (int)Enums.EstadoFicha.Beneficiario);


                        //crear el beneficiario y el registro para la cuenta bancaria si no es menor de edad
                        if (ficha.TipoFicha == (int)Enums.TipoFicha.ConfiamosEnVos)
                        {
                            fcv = _FichaRepo.getApoConfVos(ficha.IdFicha);
                        }
                        string tieneApoderado = ficha.TipoFicha != (int)Enums.TipoFicha.ConfiamosEnVos ? EsMenorDeEdad((DateTime)ficha.FechaNacimiento) : fcv.APODERADO ?? "N";

                        //cargo el apoderado para el beneficiario que se esta editando
                        //vista.IdApoderado = _apoderadorepositorio.AddApoderado(VistaToDatos(vista));


                        IBeneficiario objbeneficiario = new Beneficiario
                        {
                            IdFicha = ficha.IdFicha,
                            IdPrograma = Convert.ToInt16(ficha.TipoFicha),
                            IdEstado = (int)Enums.EstadoBeneficiario.Activo, //Estado 2: ACTIVO- Validar si primero es postulante o Activo
                            Residente = "S",
                            Email = ficha.Mail.Replace('*', '@'),
                            TipoPersona = "F",
                            CodigoActivacionBancaria = 2,
                            CondicionIva = Constantes.CondicionIvaBanco.ToString(),
                            TieneApoderado = tieneApoderado,
                            FechaInicioBeneficio = fichas.FechaInicioBeneficiario,//comun.FechaSistema,
                        };
                        int idBeneficiario = _beneficiarioRepositorio.AddBeneficiario(objbeneficiario);


                        // 10/04/2014 - La validacion de menor de edad para el programa confiamos a vos ya es calculado
                        //CREAR CONSULTA PARA TRAER EL APODERADO SI LO TIENE
                        if (ficha.TipoFicha == (int)Enums.TipoFicha.ConfiamosEnVos)
                        {
                            apoderado.IdBeneficiario = idBeneficiario;
                            apoderado.Apellido = (fcv.APO_APELLIDO ?? "").ToUpper();
                            apoderado.Nombre = (fcv.APO_NOMBRE ?? "").ToUpper();
                            apoderado.TipoDocumento = fcv.APO_TIPO_DOCUMENTO;
                            apoderado.NumeroDocumento = fcv.APO_NUMERO_DOCUMENTO;
                            apoderado.Cuil = fcv.APO_CUIL;
                            apoderado.Sexo = fcv.APO_SEXO;
                            //apoderado.IdSistema = fcv.IdSistema;
                            //apoderado.Cbu = fcv.Cbu;
                            //apoderado.UsuarioBanco = fcv.UsuarioBanco;
                            //apoderado.IdSucursal = fcv.IdSucursal == 0 ? null : fcv.IdSucursal;
                            apoderado.IdMoneda = 144;
                            //apoderado.ApoderadoDesde = apoderado.ApoderadoDesde;
                            //apoderado.ApoderadoHasta = apoderado.ApoderadoHasta;
                            apoderado.IdEstadoApoderado = 1;//apoderado.IdEstadoApoderado;
                            apoderado.FechaNacimiento = fcv.APO_FER_NAC;
                            apoderado.Calle = (fcv.APO_CALLE ?? "").ToUpper();
                            apoderado.Numero = fcv.APO_NUMERO;
                            apoderado.Piso = fcv.APO_PISO;
                            apoderado.Dpto = fcv.APO_DPTO;
                            apoderado.Monoblock = fcv.APO_MONOBLOCK;
                            apoderado.Parcela = fcv.APO_PARCELA;
                            apoderado.Manzana = fcv.APO_MANZANA;
                            //apoderado.EntreCalles = (fcv.APO_Entrecalles ?? "").ToUpper();
                            apoderado.Barrio = fcv.APO_BARRIO;
                            apoderado.CodigoPostal = fcv.APO_CODIGO_POSTAL;
                            apoderado.IdLocalidad = fcv.APO_ID_LOCALIDAD;
                            apoderado.TelefonoFijo = fcv.APO_TELEFONO;
                            //apoderado.TelefonoCelular = fcv.APO_TelefonoCelular;
                            //apoderado.Mail = fcv.APO_Mail;//,
                            //apoderado.ID_USR_SIST = apoderado.IdUsuarioSistema,
                            //apoderado.FEC_SIST = apoderado.FechaSistema
                        }


                        string nroDoc = ficha.NumeroDocumento.Replace('*', ' ');
                        if (tieneApoderado == "N")
                        {
                            //Grabar en T_CUENTAS_BANCO
                            ICuentaBanco cuentaBanco = new CuentaBanco
                            {
                                IdSucursal = ficha.IdLocalidad == 25 ? (short)143 : _sucursalrepositorio.GestSucursalLocalidad(ficha.IdFicha),
                                IdCuentaBanco = SecuenciaRepositorio.GetId(),
                                IdBeneficiario = idBeneficiario,
                                IdSistema = Constantes.IdSistemaBanco,
                                UsuarioBanco = nroDoc,
                                IdMoneda = Constantes.MonedaBanco,
                            };


                            _cuentaBancoRepositorio.AddCuentaBanco(cuentaBanco);
                        }

                        if (tieneApoderado == "S")
                        {

                            if (ficha.TipoFicha == (int)Enums.TipoFicha.ConfiamosEnVos)
                            {
                                //fcv = _;
                                _FichaRepo.AddApoderado(apoderado);
                            }

                        }


                        cant++;
                    }
                }
                
            }
            return cant;
        
        }

        public IFichasVista GetFichas(IFichasVista modelo)
        {
            IFichasVista vista = new FichasVista();
            List<int> idBeneficiariosExport = new List<int>();
                        if (modelo != null)
            {

                if (modelo.Fichas.Count > 0)
                {

                    foreach (IFicha row in modelo.Fichas)
                    {
                        idBeneficiariosExport.Add(Convert.ToInt32(row.IdFicha));

                    }
                }
            }



            //vista.Fichas = _fichaRepositorio.GetFichas().Where(c => idBeneficiariosExport.Contains(c.IdFicha)).ToList();
            vista.Fichas = _fichaRepositorio.GetFichas(idBeneficiariosExport).ToList();
            return vista;
            
        }


        //11/06/2014
        public bool UpdateFichaEtapa(int idFicha, int idEtapa)
        {


            return _fichaRepositorio.UpdateFichaEtapa(idFicha, idEtapa);
        }


        // 18/02/2013 - DI CAMPLI LEANDRO
#region "Re Empadronar Fichas"
        public IFichasVista indexReempadronarFichas()
        {
            IFichasVista vista = new FichasVista();

            IList<IFicha> lista = new List<IFicha>();

            vista.Fichas = lista;

            CargarTipoFichas(vista, _tipoFichaRepositorio.GetTiposFicha());

            return vista;

        }
        
        public IFichasVista EmpadronarFichasXLS(string ExcelUrl)
        {

            IFichasVista vista = new FichasVista();
            //********************15/02/2013 - DI CAMPLI LEANDRO -******************************
            vista.ExcelNombre = ExcelUrl;

            List<int> idBeneficiariosExport = new List<int>();

            string path = HttpContext.Current.Server.MapPath(@"\Archivos\Importados") + @"\" + ExcelUrl;

            vista.ExcelUrl = path;

            var fileInfo = new FileInfo(path);

            IExcelDataReader excelReader = null;

            FileStream stream = File.Open(path, FileMode.Open, FileAccess.Read);

            string file = fileInfo.Name;

            if (file.Split('.')[1].Equals("xls"))
            {
                excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
            }
            else if (file.Split('.')[1].Equals("xlsx"))
            {
                excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            }

            if (excelReader != null) excelReader.IsFirstRowAsColumnNames = true;
            var dsExcel = new DataSet();
            if (excelReader != null)
            {
                dsExcel = excelReader.AsDataSet();
                excelReader.Close();
            }
            if (dsExcel != null)
            {

                if (dsExcel.Tables[0].Rows.Count > 0)
                {

                    foreach (DataRow row in dsExcel.Tables[0].Rows)
                    {
                        idBeneficiariosExport.Add(Convert.ToInt32(row["ID_FICHA"].ToString()));

                    }
                }
            }

            //var pager = new Pager(0, Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
            //                      "FormIndexFichas", _aut.GetUrl("IndexPager", "Fichas"));
            //vista.pager = pager;
            vista.ProcesarExcel = true;
            //vista.Fichas = _fichaRepositorio.GetFichas().Where(c=>idBeneficiariosExport.Contains(c.IdFicha)).ToList();
            vista.Fichas = _fichaRepositorio.GetFichas(idBeneficiariosExport).ToList();

            return vista;
        }

        public IFichasVista ReEmpdronarFichas(string ExcelUrl)
        {

            IFichasVista vista = new FichasVista();
            //********************08/01/2018 - DI CAMPLI LEANDRO -******************************
            vista.ExcelNombre = ExcelUrl;

            List<int> idBeneficiariosExport = new List<int>();

            string path = HttpContext.Current.Server.MapPath(@"\Archivos\Importados") + @"\" + ExcelUrl;

            vista.ExcelUrl = path;

            var fileInfo = new FileInfo(path);

            IExcelDataReader excelReader = null;

            FileStream stream = File.Open(path, FileMode.Open, FileAccess.Read);

            string file = fileInfo.Name;

            if (file.Split('.')[1].Equals("xls"))
            {
                excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
            }
            else if (file.Split('.')[1].Equals("xlsx"))
            {
                excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            }

            if (excelReader != null) excelReader.IsFirstRowAsColumnNames = true;
            var dsExcel = new DataSet();
            if (excelReader != null)
            {
                dsExcel = excelReader.AsDataSet();
                excelReader.Close();
            }
            if (dsExcel != null)
            {

                if (dsExcel.Tables[0].Rows.Count > 0)
                {

                    foreach (DataRow row in dsExcel.Tables[0].Rows)
                    {
                        idBeneficiariosExport.Add(Convert.ToInt32(row["ID_FICHA"].ToString()));

                    }
                }
            }

            //var pager = new Pager(0, Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
            //                      "FormIndexFichas", _aut.GetUrl("IndexPager", "Fichas"));
            //vista.pager = pager;
            vista.ProcesarExcel = true;
            //vista.Fichas = _fichaRepositorio.GetFichas().Where(c=>idBeneficiariosExport.Contains(c.IdFicha)).ToList();
            vista.Fichas = _fichaRepositorio.GetFichas(idBeneficiariosExport).ToList();

            return vista;
        }

        public IFichasVista ReEmpadronarFichas(IFichasVista fichas)
        {
            IComunDatos comun = new ComunDatos();
            FichaRepositorio f = new FichaRepositorio();
            f.AgregarDatos(comun);
            int cant = 0;
            int err = 0;

            foreach (IFicha ficha in fichas.Fichas)
            {

                try
                {
                    //RE EMPADRONAR FICHA
                    ficha.idEmpadronado = _fichaRepositorio.ReEmpadronarFicha(ficha.IdFicha);

                    cant++;
                    ficha.empadronado = "EMPADRONADA";
                }
                catch(Exception ex)
                {
                    err++;
                    ficha.idEmpadronado = 0;
                    ficha.empadronado = "NO EMPADRONADA";

                }

            }
            fichas.Empadronados = cant;
            fichas.EmpadronadosFail = err;

            return fichas;

        }

        public byte[] ExportEmpadronados(IFichasVista model)
        {
            string path = HttpContext.Current.Server.MapPath(@"\Archivos\Exportados\TemplateEmpadronados.xls");

            var fs =
                new FileStream(path, FileMode.Open, FileAccess.Read);

            var templateWorkbook = new HSSFWorkbook(fs, true);

            var sheet = templateWorkbook.GetSheet("Empadronados");
            int i = 0;
            foreach (var ficha in model.Fichas)
            {
                i++;

                var row = sheet.CreateRow(i);
                var celIdFicha = row.CreateCell(0);
                celIdFicha.SetCellValue(ficha.IdFicha.ToString());

                var celapellido = row.CreateCell(1);
                celapellido.SetCellValue(ficha.Apellido);

                var celnombre = row.CreateCell(2);
                celnombre.SetCellValue(ficha.Nombre);

                var celcuil = row.CreateCell(3);
                celcuil.SetCellValue(ficha.Cuil);

                var celdni = row.CreateCell(4);
                celdni.SetCellValue(ficha.NumeroDocumento);

                var celTipoFicha = row.CreateCell(5);
                celTipoFicha.SetCellValue(ficha.TipoFicha == null ? "0" : ficha.TipoFicha.ToString());

                var celresultado = row.CreateCell(6);
                celresultado.SetCellValue(ficha.empadronado);

                var celIdNuevo = row.CreateCell(7);
                celIdNuevo.SetCellValue(ficha.idEmpadronado.ToString());

                
            }

            var ms = new MemoryStream();

            templateWorkbook.Write(ms);

            return ms.ToArray();
        }

#endregion
        #region "Gestor estados Fichas"

        // 08/01/2014 - DI CAMPLI LEANDRO
        public IFichasVista indexGestorEstadosFichas()
        {
            IFichasVista vista = new FichasVista();

            IList<IFicha> lista = new List<IFicha>();
            vista.Fichas = lista;

            CargarEstadosFicha(vista, _estadoFichaRepositorio.GetEstadosFicha());

            CargarTipoFichas(vista, _tipoFichaRepositorio.GetTiposFicha());

            return vista;

        }
        
        
        #endregion

        #region Privates

        private static void CargarSexo(IFichaVista vista)
        {
            vista.Sexo.Combo.Add(new ComboItem { Id = (int)Enums.Sexo.Mujer, Description = "MUJER" });
            vista.Sexo.Combo.Add(new ComboItem { Id = (int)Enums.Sexo.Varon, Description = "VARON" });
            if (vista.TipoFicha == 8) // 21/04/2014 - DI CAMPLI LEANDRO - DATOS INICIALES DEL APODERADO
            {

                vista.SexoApo.Combo.Add(new ComboItem { Id = (int)Enums.Sexo.Mujer, Description = "MUJER" });
                vista.SexoApo.Combo.Add(new ComboItem { Id = (int)Enums.Sexo.Varon, Description = "VARON" });
            }
        }

        private static void CargarLocalidad(IFichaVista vista, IEnumerable<ILocalidad> listalocalidades)
        {
            foreach (var listalocalidad in listalocalidades)
            {
                vista.Localidades.Combo.Add(new ComboItem { Id = listalocalidad.IdLocalidad, Description = listalocalidad.NombreLocalidad });
                //vista.LocalidadesEmpresa.Combo.Add(new ComboItem { Id = listalocalidad.IdLocalidad, Description = listalocalidad.NombreLocalidad });
                //vista.LocalidadesSedes.Combo.Add(new ComboItem { Id = listalocalidad.IdLocalidad, Description = listalocalidad.NombreLocalidad });
                //if (vista.TipoFicha == 8) // 21/04/2014 - DI CAMPLI LEANDRO - DATOS INICIALES DEL APODERADO
                //{
                //    vista.LocalidadesApo.Combo.Add(new ComboItem { Id = listalocalidad.IdLocalidad, Description = listalocalidad.NombreLocalidad });
                //}
            }
        }

        private static void CargarLocalidadApo(IFichaVista vista, IEnumerable<ILocalidad> listalocalidades)
        {
            foreach (var listalocalidad in listalocalidades)
            {

                    vista.LocalidadesApo.Combo.Add(new ComboItem { Id = listalocalidad.IdLocalidad, Description = listalocalidad.NombreLocalidad });

            }
        }

        private static void CargarLocalidadSede(IFichaVista vista, IEnumerable<ILocalidad> listalocalidades)
        {
            foreach (var listalocalidad in listalocalidades)
            {
                vista.LocalidadesSedes.Combo.Add(new ComboItem { Id = listalocalidad.IdLocalidad, Description = listalocalidad.NombreLocalidad });
            }
        }

        private static void CargarLocalidadEmpresa(IFichaVista vista, IEnumerable<ILocalidad> listalocalidades)
        {
            foreach (var listalocalidad in listalocalidades)
            {
                vista.LocalidadesEmpresa.Combo.Add(new ComboItem { Id = listalocalidad.IdLocalidad, Description = listalocalidad.NombreLocalidad });

            }
        }

        private static void CargarLocalidadEscuela(IFichaVista vista, IEnumerable<ILocalidad> listalocalidadesByDpto)
        {
            var cambiarSelected = true;

            foreach (var listalocalidad in listalocalidadesByDpto)
            {
                vista.LocalidadesEscuela.Combo.Add(new ComboItem { Id = listalocalidad.IdLocalidad, Description = listalocalidad.NombreLocalidad });

                if (listalocalidad.IdLocalidad.ToString() == vista.LocalidadesEscuela.Selected)
                    cambiarSelected = false;
            }

            if (cambiarSelected)
                vista.LocalidadesEscuela.Selected =
                    ((IList<ILocalidad>)listalocalidadesByDpto)[0].IdLocalidad.ToString();
        }

        private static void CargarDepartamento(IFichaVista vista, IEnumerable<IDepartamento> listadepartamentos)
        {
            foreach (var listadepartamento in listadepartamentos)
            {
                vista.Departamentos.Combo.Add(new ComboItem { Id = listadepartamento.IdDepartamento, Description = listadepartamento.NombreDepartamento });
                vista.DepartamentosEscuela.Combo.Add(new ComboItem { Id = listadepartamento.IdDepartamento, Description = listadepartamento.NombreDepartamento });

                if (vista.TipoFicha == 8) // 21/04/2014 - DI CAMPLI LEANDRO - DATOS INICIALES DEL APODERADO
                {
                    vista.DepartamentosApo.Combo.Add(new ComboItem { Id = listadepartamento.IdDepartamento, Description = listadepartamento.NombreDepartamento });
                }
            
            }
        }

        private static void CargarTipoDocumento(IFichaVista vista)
        {
            vista.TipoDocumentoCombo.Combo.Add(new ComboItem { Id = (int)Enums.TipoDocumento.Dni, Description = "DNI" });
            vista.TipoDocumentoCombo.Combo.Add(new ComboItem { Id = (int)Enums.TipoDocumento.Le, Description = "LE" });
            vista.TipoDocumentoCombo.Combo.Add(new ComboItem { Id = (int)Enums.TipoDocumento.Lc, Description = "LC" });
            vista.TipoDocumentoCombo.Combo.Add(new ComboItem { Id = (int)Enums.TipoDocumento.Ci, Description = "CI" });
            vista.TipoDocumentoCombo.Combo.Add(new ComboItem { Id = (int)Enums.TipoDocumento.NuncaTuvo, Description = "Nunca Tuvo" });
            vista.TipoDocumentoCombo.Combo.Add(new ComboItem { Id = (int)Enums.TipoDocumento.CedulaExtranjera, Description = "Cedula Extranjera" });
            vista.TipoDocumentoCombo.Combo.Add(new ComboItem { Id = (int)Enums.TipoDocumento.Otro, Description = "Otro" });

            if (vista.TipoFicha == 8) // 21/04/2014 - DI CAMPLI LEANDRO - DATOS INICIALES DEL APODERADO
            {
                vista.TipoDocumentoApoCombo.Combo.Add(new ComboItem { Id = (int)Enums.TipoDocumento.Dni, Description = "DNI" });
                vista.TipoDocumentoApoCombo.Combo.Add(new ComboItem { Id = (int)Enums.TipoDocumento.Le, Description = "LE" });
                vista.TipoDocumentoApoCombo.Combo.Add(new ComboItem { Id = (int)Enums.TipoDocumento.Lc, Description = "LC" });
                vista.TipoDocumentoApoCombo.Combo.Add(new ComboItem { Id = (int)Enums.TipoDocumento.Ci, Description = "CI" });
                vista.TipoDocumentoApoCombo.Combo.Add(new ComboItem { Id = (int)Enums.TipoDocumento.NuncaTuvo, Description = "Nunca Tuvo" });
                vista.TipoDocumentoApoCombo.Combo.Add(new ComboItem { Id = (int)Enums.TipoDocumento.CedulaExtranjera, Description = "Cedula Extranjera" });
                vista.TipoDocumentoApoCombo.Combo.Add(new ComboItem { Id = (int)Enums.TipoDocumento.Otro, Description = "Otro" });
            }
        }

        private static void CargarEstadoCivil(IFichaVista vista, IEnumerable<IEstadoCivil> listaestadocivil)
        {
            foreach (var estadocivil in listaestadocivil)
            {
                vista.EstadoCivil.Combo.Add(new ComboItem { Id = estadocivil.Id_Estado_Civil, Description = estadocivil.Nombre_Estado_Civil });

                if (vista.TipoFicha == 8) // 21/04/2014 - DI CAMPLI LEANDRO - DATOS INICIALES DEL APODERADO
                {
                    vista.EstadoCivilApo.Combo.Add(new ComboItem { Id = estadocivil.Id_Estado_Civil, Description = estadocivil.Nombre_Estado_Civil });
                }
            
            
            }
        }

        private static void CargarSectorProductivo(IFichaVista vista, IEnumerable<ISector> listasectores)
        {
            if (vista == null) return;

            vista.SectorProductivo.Combo.Add(new ComboItem { Id = 0, Description = "OTRO SECTOR..." });
            //vista.SectorProductivo.Combo.Add(new ComboItem { Id = 1, Description = "INFORMATICO" });
            //vista.SectorProductivo.Combo.Add(new ComboItem { Id = 2, Description = "METAL-MECANICA" });
            //vista.SectorProductivo.Combo.Add(new ComboItem { Id = 3, Description = "ALIMENTO" });
            //vista.SectorProductivo.Combo.Add(new ComboItem { Id = 4, Description = "TURISMO Y HOTELERIA" });


            foreach (var listasector in listasectores)
            {
                vista.SectorProductivo.Combo.Add(new ComboItem { Id = listasector.IdSector, Description = listasector.NombreSector });
            }


        }

        private static void CargarInstitucion(IFichaVista vista, IEnumerable<IInstitucion> listainstituciones)
        {
            vista.Institucion.Combo.Add(new ComboItem { Id = 0, Description = "OTRA INSTITUCIÓN..." });

            foreach (var listainstitucion in listainstituciones)
            {
                vista.Institucion.Combo.Add(new ComboItem { Id = listainstitucion.IdInstitucion, Description = listainstitucion.NombreInstitucion });
            }
        }

        private static void CargarInstitucionSector(IFichaVista vista, IEnumerable<IInstitucion> listainstituciones)
        {
            vista.InstitucionSector.Combo.Add(new ComboItem { Id = 0, Description = "OTRA INSTITUCIÓN..." });

            foreach (var listainstitucion in listainstituciones)
            {
                vista.InstitucionSector.Combo.Add(new ComboItem { Id = listainstitucion.IdInstitucion, Description = listainstitucion.NombreInstitucion });
            }
        }

        private static void CargarCarrera(IFichaVista vista, IEnumerable<ICarrera> listacarreras)
        {
            vista.Carrera.Combo.Add(new ComboItem { Id = 0, Description = "OTRA CARRERA..." });

            foreach (var listacarrera in listacarreras)
            {
                vista.Carrera.Combo.Add(new ComboItem { Id = listacarrera.IdCarrera, Description = listacarrera.NombreCarrera });
            }
        }

        private static void CargarCarreraSector(IFichaVista vista, IEnumerable<ICarrera> listacarreras)
        {
            vista.CarreraSector.Combo.Add(new ComboItem { Id = 0, Description = "SELECCIONE UNA CARRERA..." });

            foreach (var listacarrera in listacarreras)
            {
                vista.CarreraSector.Combo.Add(new ComboItem { Id = listacarrera.IdCarrera, Description = listacarrera.NombreCarrera });
            }
        }

        private static void CargarEscuela(IFichaVista vista, IEnumerable<IEscuela> listaescuelas)
        {
            vista.Escuelas.Combo.Add(new ComboItem { Id = 0, Description = "Seleccione ESCUELA..." });

            foreach (var listaescuela in listaescuelas)
            {
                vista.Escuelas.Combo.Add(new ComboItem { Id = listaescuela.Id_Escuela, Description = listaescuela.Nombre_Escuela });
            }
        }

        private static void CargarAbandonoCursado(IFichaVista vista, IEnumerable<IAbandonoCursado> listaPeriodosAbandono)
        {
            vista.AbandonoCursado.Combo.Add(new ComboItem { Id = 0, Description = "Seleccione el periodo..." });

            foreach (var listaPeriodoAbandono in listaPeriodosAbandono)
            {
                vista.AbandonoCursado.Combo.Add(new ComboItem { Id = listaPeriodoAbandono.ID_ABANDONO_CUR, Description = listaPeriodoAbandono.PERIODO });
            }
        }
        private void CargarTipoFichas(IFichasVista vista, IEnumerable<ITipoFicha> listaTipoFicha)
        {

            vista.ComboTipoFicha.Combo.Add(new ComboItem { Id = 0, Description = "Seleccione el Tipo Ficha..." });

            foreach (var item in listaTipoFicha)
            {
                if ((int)Enums.TipoFicha.EfectoresSociales != item.Id_Tipo_Ficha)
                {
                    vista.ComboTipoFicha.Combo.Add(new ComboItem { Id = item.Id_Tipo_Ficha, Description = item.Nombre_Tipo_Ficha });
                }
                else
                {
                    if (base.AccesoPrograma(Enums.Formulario.AbmFicha))
                        vista.ComboTipoFicha.Combo.Add(new ComboItem { Id = item.Id_Tipo_Ficha, Description = item.Nombre_Tipo_Ficha });
                }
            }

        }

        private void CargarTipoFichasRol(IFichasVista vista, IEnumerable<ITipoFicha> listaTipoFicha)
        {

            vista.ComboTipoFicha.Combo.Add(new ComboItem { Id = -1, Description = "Seleccione el Tipo Ficha..." });

            foreach (var item in listaTipoFicha)
            {

                    vista.ComboTipoFicha.Combo.Add(new ComboItem { Id = item.Id_Tipo_Ficha, Description = item.Nombre_Tipo_Ficha });
                    
            }

        }

        private void CargarTipoFichaAlta(IFichaVista vista, IEnumerable<ITipoFicha> listaTipoFicha)
        {
            vista.ComboTipoFicha.Combo.Add(new ComboItem { Id = 0, Description = "Seleccione el Tipo Ficha..." });

            foreach (var tipo in listaTipoFicha)
            {
                if ((int)Enums.TipoFicha.EfectoresSociales != tipo.Id_Tipo_Ficha)
                {
                    vista.ComboTipoFicha.Combo.Add(new ComboItem { Id = tipo.Id_Tipo_Ficha, Description = tipo.Nombre_Tipo_Ficha });
                }
                else
                {
                    if (base.AccesoPrograma(Enums.Formulario.AbmFicha))
                        vista.ComboTipoFicha.Combo.Add(new ComboItem { Id = tipo.Id_Tipo_Ficha, Description = tipo.Nombre_Tipo_Ficha });
                }
            }




        }

        private static void CargarEstadosFicha(IFichaVista vista, IEnumerable<IEstadoFicha> listaEstadoFicha)
        {
            foreach (var estado in listaEstadoFicha)
            {
                vista.Estado.Combo.Add(new ComboItem { Id = estado.Id_Estado_Ficha, Description = estado.Nombre_Estado_Ficha });
            }
        }

        private static void CargarEstadosFicha(IFichasVista vista, IEnumerable<IEstadoFicha> listaEstadoFicha)
        {
            foreach (var estado in listaEstadoFicha)
            {
                vista.ComboEstadoFicha.Combo.Add(new ComboItem { Id = estado.Id_Estado_Ficha, Description = estado.Nombre_Estado_Ficha });
            }
        }

        private static void CargarEstadosFichas(IFichasVista vista, IEnumerable<IEstadoFicha> listaEstadoFicha)
        {
            vista.ComboEstadoFicha.Combo.Add(new ComboItem { Id = -1, Description = "Seleccione el Estado Ficha..." });

            foreach (var estado in listaEstadoFicha)
            {
                vista.ComboEstadoFicha.Combo.Add(new ComboItem { Id = estado.Id_Estado_Ficha, Description = estado.Nombre_Estado_Ficha });
            }

            vista.ComboEstadoFicha.Combo.Add(new ComboItem { Id = -2, Description = "SIN ESTADO" });
        }


        private static void CargarModalidadAfip(IFichaVista vista, IEnumerable<IModalidadContratacionAFIP> listaModalida)
        {
            vista.ComboModalidadAfip.Combo.Add(new ComboItem { Id = 0, Description = "Seleccione una Modalidad" });

            foreach (var modalidad in listaModalida)
            {
                vista.ComboModalidadAfip.Combo.Add(new ComboItem { Id = modalidad.IdModalidadAFIP, Description = modalidad.ModalidadAFIP });
            }
        }

        private static void CargarTitulos(IFichaVista vista, IEnumerable<ITitulo> listatitulo)
        {
            vista.ComboTitulos.Combo.Add(new ComboItem { Id = 0, Description = "Seleccione un Título" });

            foreach (var titulo in listatitulo)
            {
                vista.ComboTitulos.Combo.Add(new ComboItem { Id = titulo.IdTitulo, Description = titulo.Descripcion });
            }
        }

        private static void CargarUniversidades(IFichaVista vista, IEnumerable<IUniversidad> listauniversidades)
        {
            vista.ComboUniversidad.Combo.Add(new ComboItem { Id = 0, Description = "Seleccione una Universidad" });

            foreach (var universidad in listauniversidades)
            {
                vista.ComboUniversidad.Combo.Add(new ComboItem { Id = universidad.IdUniversidad, Description = universidad.NombreUniversidad });
            }
        }

        private static void CargarEstadosFichaAlta(IFichaVista vista)
        {
            //vista.ComboEstadoFicha.Combo.Add(new ComboItem { Id = (int)Enums.EstadoFicha.Apta, Description = "APTA" });
            //vista.ComboEstadoFicha.Combo.Add(new ComboItem { Id = (int)Enums.EstadoFicha.NoApta, Description = "NO APTA" });
            vista.ComboEstadoFicha.Combo.Add(new ComboItem { Id = (int)Enums.EstadoFicha.Inscripto, Description = "Inscripto" });
        }

        private static void CargarComboSiNo(IFichaVista vista)
        {
            vista.DomicilioLaboralIdem.Combo.Add(new ComboItem { Id = 0, Description = "Seleccione un valor..." });
            vista.DomicilioLaboralIdem.Combo.Add(new ComboItem { Id = 1, Description = "Si" });
            vista.DomicilioLaboralIdem.Combo.Add(new ComboItem { Id = 2, Description = "No" });
        }

        private static void CargarSubprogramas(IFichaVista vista, IEnumerable<ISubprograma> listaSubprogramas)
        {
            vista.Subprogramas.Combo.Add(new ComboItem { Id = 0, Description = "Seleccione un Subprograma" });

            foreach (var subprograma in listaSubprogramas)
            {
                vista.Subprogramas.Combo.Add(new ComboItem { Id = subprograma.IdSubprograma, Description = subprograma.NombreSubprograma });
            }
        }

        private IList<TipoRechazoVista> GetTiposRechazosVista(int idFicha)
        {
            IList<TipoRechazoVista> tipoRechazosVista = new List<TipoRechazoVista>();

            IList<ITipoRechazo> tipoRechazos = _tipoRechazoRepositorio.GetTiposRechazo();

            foreach (var tipoRechazo in tipoRechazos)
            {
                var tipoRechazoVista = new TipoRechazoVista
                                           {
                                               Id = tipoRechazo.IdTipoRechazo,
                                               Descripcion = tipoRechazo.NombreTipoRechazo,
                                               Selected = false
                                           };

                if (_fichaRechazoRepositorio.GetFichaTipoRechazo(idFicha, tipoRechazo.IdTipoRechazo) != null)
                {
                    tipoRechazoVista.Selected = true;
                }

                tipoRechazosVista.Add(tipoRechazoVista);
            }

            return tipoRechazosVista;
        }

        private IList<TipoRechazoVista> GetTiposRechazos()
        {
            IList<TipoRechazoVista> tipoRechazosVista = new List<TipoRechazoVista>();

            IList<ITipoRechazo> tipoRechazos = _tipoRechazoRepositorio.GetTiposRechazo();

            foreach (var tipoRechazo in tipoRechazos)
            {
                var tipoRechazoVista = new TipoRechazoVista
                {
                    Id = tipoRechazo.IdTipoRechazo,
                    Descripcion = tipoRechazo.NombreTipoRechazo,
                    Selected = false
                };

                tipoRechazosVista.Add(tipoRechazoVista);
            }

            return tipoRechazosVista;
        }

        private void AddFichaRechazo(IFichaVista vista)
        {
            foreach (var tipoRechazo in vista.TipoRechazos)
            {
                var fichaRechazo = GetFichaRechazo(vista, tipoRechazo);

                // Elimina antes de insertar
                _fichaRechazoRepositorio.DeleteFichaTipoRechazo(fichaRechazo);

                // Insertar valores seleccionados
                if (tipoRechazo.Selected)
                {
                    _fichaRechazoRepositorio.AddFichaTipoRechazo(fichaRechazo);
                }
            }
        }

        private void DeleteFichaRechazo(IFichaVista vista)
        {
            foreach (var tipoRechazo in vista.TipoRechazos)
            {
                var fichaRechazo = GetFichaRechazo(vista, tipoRechazo);
                _fichaRechazoRepositorio.DeleteFichaTipoRechazo(fichaRechazo);
            }
        }

        private static IFichaRechazo GetFichaRechazo(IFichaVista vista, TipoRechazoVista tipoRechazo)
        {
            IFichaRechazo fichaRechazo = new FichaRechazo
                                             {
                                                 IdFicha = vista.IdFicha,
                                                 IdTipoRechazo = tipoRechazo.Id
                                             };
            return fichaRechazo;
        }


        private void CargarProgramaEtapa(IFichasVista vista, IEnumerable<IPrograma> listaProgramas)
        {
            vista.EtapaProgramas.Combo.Add(new ComboItem
            {
                Id = 0,
                Description = "TODOS"

            });


            foreach (var item in listaProgramas)
            {
                if ((int)Enums.Programas.EfectoresSociales != item.IdPrograma)
                {
                    vista.EtapaProgramas.Combo.Add(new ComboItem
                    {
                        Id = item.IdPrograma,
                        Description = item.NombrePrograma

                    });
                }
                else
                {
                    if (base.AccesoPrograma(Enums.Formulario.AbmBeneficiario))
                        vista.EtapaProgramas.Combo.Add(new ComboItem
                        {
                            Id = item.IdPrograma,
                            Description = item.NombrePrograma

                        });
                }
            }



        }



        public void CargarLocalidadEscuela(ComboBox cbo, int idDepartamento)
        {
            //var cambiarSelected = true;
            IEnumerable<ILocalidad> listalocalidadesByDpto = _localidadRepositorio.GetLocalidadesByDepto(Convert.ToInt32(idDepartamento));
            foreach (var listalocalidad in listalocalidadesByDpto)
            {


                cbo.Combo.Add(new ComboItem { Id = listalocalidad.IdLocalidad, Description = listalocalidad.NombreLocalidad });

                //if (listalocalidad.IdLocalidad.ToString() == vista.LocalidadesEscuela.Selected)
                //    cambiarSelected = false;
            }

            //if (cambiarSelected)
            //    vista.LocalidadesEscuela.Selected =
            //        ((IList<ILocalidad>)listalocalidadesByDpto)[0].IdLocalidad.ToString();7

            //return cbo;
        }


        public void CargarEscuela(ComboBox cbo, int idLocalidad)
        {

            IEnumerable<IEscuela> listaescuelas = _escuelaRepositorio.GetEscuelasByLocalidad(Convert.ToInt32(idLocalidad));

            foreach (var listaescuela in listaescuelas)
            {
                cbo.Combo.Add(new ComboItem { Id = listaescuela.Id_Escuela, Description = listaescuela.Nombre_Escuela });
            }
        }


        public IEscuela TraerEscuela(int idEscuela)
        {
            try
            {

                return _escuelaRepositorio.GetEscuelaCompleto(idEscuela);//_escuelaRepositorio.GetEscuelasCompleto().Where(x=>x.Id_Escuela == Convert.ToInt32(idEscuela)).SingleOrDefault();

            }
            catch (Exception ex)
            {
                return null;
            }
        }


        #endregion
    }
}
