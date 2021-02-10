using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using Excel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using NPOI.HSSF.UserModel;
using SGE.Model.Comun;
using SGE.Model.Entidades;
using SGE.Model.Entidades.Interfaces;
using SGE.Model.Repositorio;
using SGE.Repositorio;
using SGE.Servicio.Comun;
using SGE.Servicio.ServicioInterfaces;
using SGE.Servicio.Vista;
using SGE.Servicio.Vista.Shared;
using SGE.Servicio.VistaInterfaces;
using IError = SGE.Servicio.Comun.IError;
using Color = iTextSharp.text.BaseColor;
using FontPdf = iTextSharp.text.Font;
using Rectangle = iTextSharp.text.Rectangle;
using System.Text;

using SGE.Repositorio.Modelo;

namespace SGE.Servicio.Servicio
{

    public class BeneficiarioServicio : BaseServicio, IBeneficiarioServicio
    {

        private readonly IBeneficiarioRepositorio _beneficiariorepositorio;
        private readonly IEstadoBeneficiarioRepositorio _estadobeneficiariorepositorio;
        private readonly IProgramaRepositorio _programarepositorio;
        private readonly IAutenticacionServicio _au;
        private readonly IConyugeRepositorio _conyuguerepositorio;
        private readonly IApoderadoRepositorio _apoderadorepositorio;
        private readonly IFichaRepositorio _ficharepositorio;
        private readonly ISucursalRepositorio _sucursalrepositorio;
        private readonly ICuentaBancoRepositorio _cuentabancorepositorio;
        private readonly IControlDatosRepositorio _controldatosrepositorio;
        private readonly ITablaBcoCbaRepositorio _tablabcocbarepositorio;
        private readonly IConceptoRepositorio _conceptorepositorio;
        private readonly IHorarioEmpresaRepositorio _horarioempresarepositorio;
        private readonly IHorarioFichaRepositorio _horarioficharepositorio;
        private readonly IRolFormularioAccionRepositorio _rolformularioaccionrepositorio;

        private readonly EtapaRepositorio _etaparepositorio;
        private readonly BeneficiarioRepositorio _beneficiarioRepo;
        private readonly IUsuarioRolRepositorio _usuarioRolRepositorio;
        private readonly ISubprogramaRepositorio _subprogramaRepositorio;

        private readonly ITipoPppRepositorio _tipopppRepositorio;

        public BeneficiarioServicio()
        {
            _beneficiariorepositorio = new BeneficiarioRepositorio();
            _au = new AutenticacionServicio();
            _conyuguerepositorio = new ConyugeRepositorio();
            _estadobeneficiariorepositorio = new EstadoBeneficiarioRepositorio();
            _programarepositorio = new ProgramaRepositorio();
            _apoderadorepositorio = new ApoderadoRepositorio();
            _ficharepositorio = new FichaRepositorio();
            _sucursalrepositorio = new SucursalRepositorio();
            _cuentabancorepositorio = new CuentaBancoRepositorio();
            _controldatosrepositorio = new ControlDatosRepositorio();
            _tablabcocbarepositorio = new TablaBcoCbaRepositorio();
            _conceptorepositorio = new ConceptoRepositorio();
            _horarioficharepositorio = new HorarioFichaRepositorio();
            _horarioempresarepositorio = new HorarioEmpresaRepositorio();
            _rolformularioaccionrepositorio = new RolFormularioAccionRepositorio();

            _etaparepositorio = new EtapaRepositorio();
            _beneficiarioRepo = new BeneficiarioRepositorio();
            _usuarioRolRepositorio = new UsuarioRolRepositorio();
            _subprogramaRepositorio = new SubprogramaRepositorio();
            _tipopppRepositorio = new TipoPppRepositorio();
        }

        public IBeneficiariosVista GetBeneficiarios(string nombre, string apellido, string cuil, string numeroDocumento, int idPrograma, string conApoderados, string modalidad, string discapacitado, string altatemprana, int idEstado, string apellidonombreapoderado, int idEtapa, int idSubprograma, int TipoPrograma, string nrotramite)
        {
            IBeneficiariosVista vista = new BeneficiariosVista();

            vista.idSubprograma = idSubprograma;
            bool accesoprograma = base.AccesoPrograma(Enums.Formulario.AbmBeneficiario);

            // 10/11/2020

            var tiposppp = _tipopppRepositorio.GetTiposPpp();
            vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = 0, Description = "Todos" });
            foreach (var tipo in tiposppp)
            {
                vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = tipo.ID_TIPO_PPP, Description = tipo.N_TIPO_PPP });
            }

            // 09/09/2014 - controlar aca si tiene el rol activado para condicionar los programas y subprogramas.
            var accesoprogr =
                                  _rolformularioaccionrepositorio.GetFormulariosDelUsuario(base.GetUsuarioLoguer().Id_Usuario).Where(
                                      c =>
                                      c.Id_Accion == (int)Enums.Acciones.ProgramasPorRol &&
                                      c.Id_Formulario == (int)Enums.Formulario.AbmBeneficiario).Count() > 0
                                      ? true
                                      : false;
            var usuarioRoles = _usuarioRolRepositorio.GetRolesDelUsuario(base.GetUsuarioLoguer().Id_Usuario);

            // 01/07/2013 - DI CAMPLI LEANDRO - SE CREA LISTA PARA NO LLAMAR DOS VECES A LA BASE DE DATOS
            IList<IBeneficiario> beneficiarios;
            //*****************************************************************************************************************
            if (accesoprogr) //verifica si debe traer todos los programas asociados
            {
                vista.ProgPorRol = "S";
                beneficiarios = _beneficiariorepositorio.GetBeneficiarios(nombre, apellido, cuil,
                                                                                  numeroDocumento, idPrograma,
                                                                                  conApoderados, modalidad,
                                                                                  discapacitado, altatemprana, idEstado,
                                                                                  apellidonombreapoderado, idEtapa, nrotramite, TipoPrograma, idSubprograma).ToList();
                if (idPrograma > 5 && idPrograma < 9) // Los programas en este rango tienen subprogramas
                {
                    if (vista.idSubprograma > 0) // si se consulta por subprograma
                    {
                        if (idPrograma == (int)Enums.TipoFicha.EfectoresSociales) // para el caso de efectores no debe traer los null
                        {
                            beneficiarios = beneficiarios.Where(c => c.idSubprograma == vista.idSubprograma).ToList();
                        }
                        else
                        {
                            beneficiarios = beneficiarios.Where(c => c.idSubprograma == vista.idSubprograma || c.idSubprograma == null).ToList();
                        }
                    }
                    else
                    {
                        if (idPrograma == (int)Enums.TipoFicha.EfectoresSociales) // para el caso de efectores no debe traer todos
                        {
                            //verifico que subprogramas tiene disponible
                            var lsubprogramas = (from rs in _subprogramaRepositorio.GetSubprogramasRol()
                                                 join u in usuarioRoles on rs.Id_Rol equals u.IdRol
                                                 where rs.IdPrograma == idPrograma
                                                 group rs by new { rs.IdSubprograma, rs.NombreSubprograma } into grouping
                                                 select new
                                                     Subprograma
                                                 {
                                                     IdSubprograma = grouping.Key.IdSubprograma,
                                                     NombreSubprograma = grouping.Key.NombreSubprograma
                                                 }).ToList();


                            beneficiarios = (from f in beneficiarios

                                             join sub in lsubprogramas on f.idSubprograma equals sub.IdSubprograma
                                             where f.idSubprograma != null
                                             select f).ToList();
                        }
                    }

                }
                else
                {

                }

            }

            //*****************************************************************************************************************
            else
            {
                //26/02/2013 - DI CAMPLI LEANDRO - SE MODIFICA PARA AÑADIR A LA VISTA UN ARRAY CON LOS IDBENEFICIARIO CONSULTADOS PARA MEJORAR EL RENDIMIENTO DE LAS CONSULTAS
                beneficiarios = accesoprograma == true
                                   ? _beneficiariorepositorio.GetBeneficiarios(nombre, apellido, cuil, numeroDocumento,
                                                                               idPrograma, conApoderados, modalidad,
                                                                               discapacitado, altatemprana, idEstado,
                                                                               apellidonombreapoderado, idEtapa, nrotramite, TipoPrograma, idSubprograma)//.Select(c => c.IdBeneficiario).ToArray() //.Count()
                                   : _beneficiariorepositorio.GetBeneficiarios(nombre, apellido, cuil, numeroDocumento,
                                                                               idPrograma, conApoderados, modalidad,
                                                                               discapacitado, altatemprana, idEstado,
                                                                               apellidonombreapoderado, idEtapa, nrotramite, TipoPrograma, idSubprograma).Where(c => c.IdPrograma != (int)Enums.Programas.EfectoresSociales).ToList();//.Select(c => c.IdBeneficiario).ToArray(); //Count();
                if (idSubprograma > 0) // si se consulta por subprograma
                {

                    beneficiarios = beneficiarios.Where(c => c.idSubprograma == idSubprograma).ToList();

                }
            }

            if (TipoPrograma > 0) // si se consulta por tipo programa
            {

                beneficiarios = beneficiarios.Where(c => c.TipoPrograma == TipoPrograma).ToList();

            }
            //26/02/2013 - DI CAMPLI LEANDRO - SE MODIFICA PARA AÑADIR A LA VISTA UN ARRAY CON LOS IDBENEFICIARIO CONSULTADOS PARA MEJORAR EL RENDIMIENTO DE LAS CONSULTAS
            int countreg = 0;
            //countreg = vista.idBeneficiarios.Count();
            vista.idBeneficiarios = beneficiarios.Select(c => c.IdBeneficiario).ToArray();

            countreg = beneficiarios.Count();
            var pager =
                new Pager(countreg,
                          Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                          "FormIndexBeneficiarios", _au.GetUrl("IndexPager", "Beneficiarios"));

            vista.Pager = pager;

            vista.FileDownload = "../Archivos/" + vista.FileNombre;


            vista.ListaBeneficiarios = beneficiarios.Skip(pager.Skip).Take(pager.PageSize).ToList(); // 01/07/2013 - DI CAMPLI LEANDRO

            vista.ApellidoBusqueda = apellido;
            vista.CuilBusqueda = cuil;
            vista.NombreBusqueda = nombre;
            vista.NumeroDocumentoBusqueda = numeroDocumento;
            vista.ConCuenta = "T";

            vista.ConAltaTemprana = altatemprana ?? "T";
            vista.ConApoderado = conApoderados ?? "T";
            vista.ConDiscapacidad = discapacitado ?? "T";
            vista.ConModalidad = modalidad ?? "T";

            var lista = _programarepositorio.GetProgramas();

            if (accesoprogr) //verifica si debe traer todos los programas asociados
            {
                vista.ProgPorRol = "S";

                var lprogramas = from rp in _programarepositorio.GetTipoFichasRol()
                                 join u in usuarioRoles on rp.Id_Rol equals u.IdRol
                                 group rp by new { rp.IdPrograma, rp.NombrePrograma } into grouping
                                 select new
                                     Programa
                                 {
                                     IdPrograma = grouping.Key.IdPrograma,
                                     NombrePrograma = grouping.Key.NombrePrograma
                                 };

                CargarProgramaRol(vista, lprogramas.OrderBy(c => c.IdPrograma));
            }
            else
            {
                vista.ProgPorRol = "N";
                CargarPrograma(vista, lista);
            }
            // 10/06/2013 - DI CAMPLI LEANDRO - CARGAN LOS PROGRAMAS PARA LAS ETAPAS
            CargarProgramaEtapa(vista, lista);

            CargarEstadosBeneficiarioforIndex(vista, _estadobeneficiariorepositorio.GetEstadosBeneficiario());

            return vista;
        }

        public IBeneficiariosVista GetBeneficiariosIndex()
        {
            IBeneficiariosVista vista = new BeneficiariosVista();

            var lista = _programarepositorio.GetProgramas();

            vista.ListaBeneficiarios = new List<IBeneficiario>();


            var pager =
                new Pager(0,
                          Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                          "FormIndexBeneficiarios", _au.GetUrl("IndexPager", "Beneficiarios"));

            vista.Pager = pager;

            vista.ConCuenta = "T";
            vista.ConAltaTemprana = "T";
            vista.ConApoderado = "T";
            vista.ConDiscapacidad = "T";
            vista.ConModalidad = "T";

            //**************CARGAR EL COMBO PROGRAMAS***********************************
            // 22/08/2014 - controlar aca si tiene el rol activado para condicionar los programas y subprogramas.
            var accesoprograma =
                                  _rolformularioaccionrepositorio.GetFormulariosDelUsuario(base.GetUsuarioLoguer().Id_Usuario).Where(
                                      c =>
                                      c.Id_Accion == (int)Enums.Acciones.ProgramasPorRol &&
                                      c.Id_Formulario == (int)Enums.Formulario.AbmBeneficiario).Count() > 0
                                      ? true
                                      : false;
            var usuarioRoles = _usuarioRolRepositorio.GetRolesDelUsuario(base.GetUsuarioLoguer().Id_Usuario);

            if (accesoprograma) //verifica si debe traer todos los programas asociados
            {
                vista.ProgPorRol = "S";

                var lprogramas = from rp in _programarepositorio.GetTipoFichasRol()
                                 join u in usuarioRoles on rp.Id_Rol equals u.IdRol
                                 group rp by new { rp.IdPrograma, rp.NombrePrograma } into grouping
                                 select new
                                     Programa
                                 {
                                     IdPrograma = grouping.Key.IdPrograma,
                                     NombrePrograma = grouping.Key.NombrePrograma
                                 };

                CargarProgramaRol(vista, lprogramas.OrderBy(c => c.IdPrograma));
            }
            else
            {
                vista.ProgPorRol = "N";
                CargarPrograma(vista, lista);
            }
            //*****************************************************************************************

            // 10/06/2013 - DI CAMPLI LEANDRO - CARGAN LOS PROGRAMAS PARA LAS ETAPAS
            CargarProgramaEtapa(vista, lista);

            CargarEstadosBeneficiarioforIndex(vista, _estadobeneficiariorepositorio.GetEstadosBeneficiario());
            vista.idFormulario = "FormIndexBeneficiarios";
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

        public IBeneficiariosVista GetBeneficiarios(IPager pPager, string nombre, string apellido, string cuil, string numeroDocumento, int idPrograma, string conApoderados, string modalidad, string discapacitado, string altatemprana, int idEstado, string apellidonombreapoderado, int idEtapa, int idSubprograma, int TipoPrograma, string nrotramite)
        {
            IBeneficiariosVista vista = new BeneficiariosVista();
            vista.idSubprograma = idSubprograma;
            bool accesoprograma = base.AccesoPrograma(Enums.Formulario.AbmBeneficiario);

            // 10/11/2020
            var tiposppp = _tipopppRepositorio.GetTiposPpp();
            vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = 0, Description = "Todos" });
            foreach (var tipo in tiposppp)
            {
                vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = tipo.ID_TIPO_PPP, Description = tipo.N_TIPO_PPP });
            }

            // 09/09/2014 - controlar aca si tiene el rol activado para condicionar los programas y subprogramas.
            var accesoprogr =
                                  _rolformularioaccionrepositorio.GetFormulariosDelUsuario(base.GetUsuarioLoguer().Id_Usuario).Where(
                                      c =>
                                      c.Id_Accion == (int)Enums.Acciones.ProgramasPorRol &&
                                      c.Id_Formulario == (int)Enums.Formulario.AbmBeneficiario).Count() > 0
                                      ? true
                                      : false;
            var usuarioRoles = _usuarioRolRepositorio.GetRolesDelUsuario(base.GetUsuarioLoguer().Id_Usuario);

            // 01/07/2013 - DI CAMPLI LEANDRO - SE CREA LISTA PARA NO LLAMAR DOS VECES A LA BASE DE DATOS
            IList<IBeneficiario> beneficiarios;

            //*****************************************************************************************************************
            if (accesoprogr) //verifica si debe traer todos los programas asociados
            {
                vista.ProgPorRol = "S";
                beneficiarios = _beneficiariorepositorio.GetBeneficiarios(nombre, apellido, cuil,
                                                                                  numeroDocumento, idPrograma,
                                                                                  conApoderados, modalidad,
                                                                                  discapacitado, altatemprana, idEstado,
                                                                                  apellidonombreapoderado, idEtapa, nrotramite).ToList();
                if (idPrograma > 5 && idPrograma < 9) // Los programas en este rango tienen subprogramas
                {
                    if (vista.idSubprograma > 0) // si se consulta por subprograma
                    {
                        if (idPrograma == (int)Enums.TipoFicha.EfectoresSociales) // para el caso de efectores no debe traer los null
                        {
                            beneficiarios = beneficiarios.Where(c => c.idSubprograma == vista.idSubprograma).ToList();
                        }
                        else
                        {
                            beneficiarios = beneficiarios.Where(c => c.idSubprograma == vista.idSubprograma || c.idSubprograma == null).ToList();
                        }
                    }
                    else
                    {
                        if (idPrograma == (int)Enums.TipoFicha.EfectoresSociales) // para el caso de efectores no debe traer todos
                        {
                            //verifico que subprogramas tiene disponible
                            var lsubprogramas = (from rs in _subprogramaRepositorio.GetSubprogramasRol()
                                                 join u in usuarioRoles on rs.Id_Rol equals u.IdRol
                                                 where rs.IdPrograma == idPrograma
                                                 group rs by new { rs.IdSubprograma, rs.NombreSubprograma } into grouping
                                                 select new
                                                     Subprograma
                                                 {
                                                     IdSubprograma = grouping.Key.IdSubprograma,
                                                     NombreSubprograma = grouping.Key.NombreSubprograma
                                                 }).ToList();


                            beneficiarios = (from f in beneficiarios

                                             join sub in lsubprogramas on f.idSubprograma equals sub.IdSubprograma
                                             where f.idSubprograma != null
                                             select f).ToList();
                        }
                    }

                }
                else
                {

                }

            }

            //*****************************************************************************************************************
            else
            {
                beneficiarios = accesoprograma == true
                                   ? _beneficiariorepositorio.GetBeneficiarios(nombre, apellido, cuil, numeroDocumento,
                                                                               idPrograma, conApoderados, modalidad,
                                                                               discapacitado, altatemprana, idEstado,
                                                                               apellidonombreapoderado, idEtapa, nrotramite)//.Count()
                                   : _beneficiariorepositorio.GetBeneficiarios(nombre, apellido, cuil, numeroDocumento,
                                                                               idPrograma, conApoderados, modalidad,
                                                                               discapacitado, altatemprana, idEstado,
                                                                               apellidonombreapoderado, idEtapa, nrotramite).Where(c => c.IdPrograma != (int)Enums.Programas.EfectoresSociales).ToList();//.Count();

                if (idSubprograma > 0) // si se consulta por subprograma
                {

                    beneficiarios = beneficiarios.Where(c => c.idSubprograma == idSubprograma).ToList();

                }
            }

            if (TipoPrograma > 0) // si se consulta por tipo programa
            {

                beneficiarios = beneficiarios.Where(c => c.TipoPrograma == TipoPrograma).ToList();

            } 
            int countreg = 0;
            countreg = beneficiarios.Count();

            var pager =
                new Pager(countreg,
                          Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                          "FormIndexBeneficiarios", _au.GetUrl("IndexPager", "Beneficiarios"));


            if (pager.TotalCount != pPager.TotalCount)
            {
                pager.PageNumber = 1;
                pager.Skip = 0;
                vista.Pager = pager;
            }
            else
            {
                vista.Pager = pPager;
            }

            vista.ApellidoBusqueda = apellido;
            vista.CuilBusqueda = cuil;
            vista.NombreBusqueda = nombre;
            vista.NumeroDocumentoBusqueda = numeroDocumento;
            vista.ConCuenta = "T";
            vista.ConAltaTemprana = altatemprana;
            vista.ConApoderado = conApoderados;
            vista.ConDiscapacidad = discapacitado;
            vista.ConModalidad = modalidad;

            vista.ListaBeneficiarios = beneficiarios.Skip(vista.Pager.Skip).Take(vista.Pager.PageSize).ToList(); // 01/07/2013 - DI CAMPLI LEANDRO

            var lista = _programarepositorio.GetProgramas();

            if (accesoprogr) //verifica si debe traer todos los programas asociados
            {
                vista.ProgPorRol = "S";

                var lprogramas = from rp in _programarepositorio.GetTipoFichasRol()
                                 join u in usuarioRoles on rp.Id_Rol equals u.IdRol
                                 group rp by new { rp.IdPrograma, rp.NombrePrograma } into grouping
                                 select new
                                     Programa
                                 {
                                     IdPrograma = grouping.Key.IdPrograma,
                                     NombrePrograma = grouping.Key.NombrePrograma
                                 };

                CargarProgramaRol(vista, lprogramas.OrderBy(c => c.IdPrograma));
            }
            else
            {
                vista.ProgPorRol = "N";
                CargarPrograma(vista, lista);
            }
            // 10/06/2013 - DI CAMPLI LEANDRO - CARGAN LOS PROGRAMAS PARA LAS ETAPAS
            CargarProgramaEtapa(vista, lista);


            CargarEstadosBeneficiarioforIndex(vista, _estadobeneficiariorepositorio.GetEstadosBeneficiario());

            vista.Programas.Selected = idPrograma.ToString();

            vista.EstadosBeneficiarios.Selected = idEstado.ToString();

            return vista;
        }

        public IBeneficiarioVista GetBeneficiario(int idBeneficiario)
        {
            //IBeneficiarioVista por BeneficiarioVista en la interface no se implementó nunca los otros miembros de la clase
            //fichasPpp, fichappP, etc.
            BeneficiarioVista mreturn = new BeneficiarioVista();
            IBeneficiario obj = _beneficiariorepositorio.GetBeneficiario(idBeneficiario);

            mreturn.Email = obj.Email ?? "SIN@SIN.COM";
            mreturn.IdEstado = obj.IdEstado;
            mreturn.IdFicha = obj.IdFicha;
            mreturn.Apellido = obj.Ficha.Apellido;
            mreturn.Nombre = obj.Ficha.Nombre;
            mreturn.Cuil = obj.Ficha.Cuil;
            mreturn.NumeroDocumento = obj.Ficha.NumeroDocumento;
            mreturn.IdBeneficiario = obj.IdBeneficiario;
            mreturn.Programa = obj.Programa;

            mreturn.TieneApoderadoActivo = _apoderadorepositorio.TieneApoderadoActivo(obj.IdBeneficiario);
            mreturn.TieneApoderado = obj.TieneApoderado == "N" ? false : true;
            mreturn.ListaApoderados = _apoderadorepositorio.GetApoderadosByBeneficiario(obj.IdBeneficiario);
            CargarEstadosBeneficiario(mreturn, _estadobeneficiariorepositorio.GetEstadosBeneficiario());

            CargarMonedas(mreturn, _tablabcocbarepositorio.GetMonedas());
            mreturn.Monedas.Enabled = false;
            mreturn.Monedas.Selected = Constantes.MonedaBanco.ToString();

            CargarSucursales(mreturn, _sucursalrepositorio.GetSucursales());


            mreturn.Sucursales.Selected = obj.IdSucursal != 0 && obj.IdSucursal != null ? obj.IdSucursal.ToString() : _sucursalrepositorio.GestSucursalLocalidad(mreturn.IdFicha).ToString();
            mreturn.Sucursales.Enabled = obj.TieneApoderado == "N" ? true : false;
            mreturn.Sucursal = obj.Sucursal;

            //04/02/2013 cargar fecha inicio como beneficiario y fecha de baja como beneficiario
            mreturn.FechaInicioBeneficio = obj.FechaInicioBeneficio;
            mreturn.FechaBajaBeneficio = obj.FechaBajaBeneficio;



            //int s = 0;
            //s = _sucursalrepositorio.GestSucursalLocalidad(mreturn.IdFicha);

            mreturn.NumeroCuenta = obj.NumeroCuenta;
            mreturn.Cbu = obj.Cbu;
            mreturn.FechaSolicitudCuenta = obj.FechaSolicitudCuenta;
            mreturn.Estados.Selected = obj.IdEstado.ToString();
            mreturn.Estados.Enabled = false;

            mreturn.ListaConceptosPago = _conceptorepositorio.GetConceptosByBeneficiario(idBeneficiario);

            mreturn.FechaNotificacion = obj.FechaNotificacion;
            mreturn.Notificado = obj.Notificado;
            //22012013
            IFicha fichaBeneficiario = new Ficha();
            IFichaRepositorio _fichaBeneficiario = new FichaRepositorio();
            fichaBeneficiario = _ficharepositorio.GetFicha(mreturn.IdFicha);
            mreturn.TipoFicha = fichaBeneficiario.TipoFicha??0;

            //22012013
            //cargar los datos de la ficha

            switch (mreturn.TipoFicha)
            {
                case (int)Enums.TipoFicha.Terciaria:
                    
                    break;
                case (int)Enums.TipoFicha.Universitaria:
                    
                    break;
                case (int)Enums.TipoFicha.Ppp:
                    mreturn.FichaPpp = _ficharepositorio.GetFichaPpp(mreturn.IdFicha);
                    mreturn.IdSede = mreturn.FichaPpp.IdSede ?? 0;
                    mreturn.IdEmpresa = mreturn.FichaPpp.IdEmpresa;
                    mreturn.IdEmpresaInicial = mreturn.FichaPpp.IdEmpresa;
                    //CargarLocalidadEmpresa(vista, _localidadRepositorio.GetLocalidades());
                    //vista.LocalidadesEmpresa.Selected = (vista.FichaPpp.Empresa.IdLocalidad ?? 0).ToString();
                    //CargarLocalidadSede(vista, _localidadRepositorio.GetLocalidades());
                    //vista.LocalidadesSedes.Selected = (vista.FichaPpp.Sede.IdLocalidad ?? 0).ToString();
                    //CargarModalidadAfip(vista, _modalidadContratacionAfipRepositorio.GetModalidadesContratacionAFIP());
                    //vista.ComboModalidadAfip.Selected = (vista.FichaPpp.IdModalidadAfip ?? 0).ToString();
                    //CargarHorarioLunes(vista);
                    //CargarHorarioMartes(vista);
                    //CargarHorarioMiercoles(vista);
                    //CargarHorarioJueves(vista);
                    //CargarHorarioViernes(vista);
                    //CargarHorarioSabado(vista);
                    //CargarHorarioDomingo(vista);
                    break;
                case (int)Enums.TipoFicha.Vat:
                    mreturn.FichaVat = _ficharepositorio.GetFichaVat(mreturn.IdFicha);
                    mreturn.IdEmpresa = mreturn.FichaVat.IdEmpresa;
                    mreturn.IdEmpresaInicial = mreturn.FichaVat.IdEmpresa;
                    mreturn.IdSede = mreturn.FichaVat.IdSede ?? 0;
                    //CargarModalidadAfip(vista, _modalidadContratacionAfipRepositorio.GetModalidadesContratacionAFIP());
                    //vista.ComboModalidadAfip.Selected = (vista.FichaVat.IdModalidadAFIP ?? 0).ToString();
                    //CargarHorarioLunes(vista);
                    //CargarHorarioMartes(vista);
                    //CargarHorarioMiercoles(vista);
                    //CargarHorarioJueves(vista);
                    //CargarHorarioViernes(vista);
                    //CargarHorarioSabado(vista);
                    //CargarHorarioDomingo(vista);
                    break;
                case (int)Enums.TipoFicha.PppProf:
                    mreturn.FichaPppp = _ficharepositorio.GetFichaPppp(mreturn.IdFicha);
                    mreturn.IdEmpresa = mreturn.FichaPppp.IdEmpresa;
                    mreturn.IdEmpresaInicial = mreturn.FichaPppp.IdEmpresa;
                    mreturn.IdSede = mreturn.FichaPppp.IdSede ?? 0;
                    break;
                case (int)Enums.TipoFicha.ReconversionProductiva:
                    mreturn.FichaReconversion = _ficharepositorio.GetFichaReconversion(mreturn.IdFicha);
                    mreturn.IdSede = mreturn.FichaReconversion.IdSede ?? 0;
                    mreturn.IdEmpresa = mreturn.FichaReconversion.IdEmpresa;
                    mreturn.IdEmpresaInicial = mreturn.FichaReconversion.IdEmpresa;

                    break;
                case (int)Enums.TipoFicha.EfectoresSociales:
                    mreturn.FichaEfectores = _ficharepositorio.GetFichaEfectores(mreturn.IdFicha);
                    mreturn.IdSede = mreturn.FichaEfectores.IdSede ?? 0;
                    mreturn.IdEmpresa = mreturn.FichaEfectores.IdEmpresa;
                    mreturn.IdEmpresaInicial = mreturn.FichaEfectores.IdEmpresa;
                    //CargarLocalidadEmpresa(vista, _localidadRepositorio.GetLocalidades());
                    //vista.LocalidadesEmpresa.Selected = (vista.FichaEfectores.Empresa.IdLocalidad ?? 0).ToString();
                    //CargarLocalidadSede(vista, _localidadRepositorio.GetLocalidades());
                    //vista.LocalidadesSedes.Selected = (vista.FichaEfectores.Sede.IdLocalidad ?? 0).ToString();
                    //CargarModalidadAfip(vista, _modalidadContratacionAfipRepositorio.GetModalidadesContratacionAFIP());
                    //vista.ComboModalidadAfip.Selected = (vista.FichaEfectores.IdModalidadAfip ?? 0).ToString();
                    mreturn.IdProyecto = mreturn.FichaEfectores.IdProyecto;
                    //CargarSubprogramas(vista, _subprogramaRepositorio.GetSubprogramas());
                    //vista.Subprogramas.Selected = (vista.FichaEfectores.IdSubprograma ?? 0).ToString();
                    mreturn.NombreProyecto = mreturn.FichaEfectores.Proyecto.NombreProyecto;
                    mreturn.CantidadAcapacitar = mreturn.FichaEfectores.Proyecto.CantidadAcapacitar;
                    mreturn.FechaInicioProyecto = mreturn.FichaEfectores.Proyecto.FechaInicioProyecto;
                    mreturn.FechaFinProyecto = mreturn.FichaEfectores.Proyecto.FechaFinProyecto;
                    mreturn.MesesDuracionProyecto = mreturn.FichaEfectores.Proyecto.MesesDuracionProyecto;
                    mreturn.AportesDeLaEmpresa = mreturn.FichaEfectores.AportesDeLaEmpresa;
                    mreturn.NombreTutor = mreturn.FichaEfectores.NombreTutor;
                    mreturn.ApellidoTutor = mreturn.FichaEfectores.ApellidoTutor;
                    mreturn.NroDocumentoTutor = mreturn.NroDocumentoTutor;
                    mreturn.TelefonoTutor = mreturn.FichaEfectores.TelefonoTutor;
                    mreturn.EmailTutor = mreturn.FichaEfectores.EmailTutor;
                    mreturn.PuestoTutor = mreturn.FichaEfectores.PuestoTutor;

                    break;
                default:
                    break;
            }

            //fin cargar los datos de la ficha

            //cargar las cantidades de beneficiarios activos en la empresa
            IEmpresaRepositorio _empresaRepositorio = new EmpresaRepositorio();
            mreturn.benefActivosEmp = _empresaRepositorio.GetBenefActivosEmpCount(mreturn.IdEmpresa ?? 0, 0/*mreturn.TipoFicha*/);

            //


            return mreturn;

        }

        public bool UpdateBeneficiario(IBeneficiarioVista vista)
        {

            IBeneficiario objbeneficiario = new Beneficiario
                                                {
                                                    Email = vista.Email,
                                                    IdEstado = Convert.ToInt32(vista.Estados.Selected),
                                                    IdBeneficiario = vista.IdBeneficiario,
                                                    NumeroCuenta = vista.NumeroCuenta ?? 0,
                                                    Cbu = vista.Cbu,
                                                    IdSucursal = Convert.ToInt16(vista.Sucursales.Selected),
                                                    TieneApoderado = vista.TieneApoderado == false ? "N" : "S",
                                                    FechaNotificacion = vista.FechaNotificacion,
                                                    Notificado = (vista.FechaNotificacion == null ? false : vista.Notificado),
                                                    FechaInicioBeneficio = vista.FechaInicioBeneficio,
                                                    FechaBajaBeneficio = vista.FechaBajaBeneficio,
                                                    Ficha =
                                                        new Ficha
                                                            {
                                                                IdFicha = vista.IdFicha,
                                                                Apellido = vista.Apellido,
                                                                Nombre = vista.Nombre.ToLower(),
                                                                Cuil = vista.Cuil,
                                                                NumeroDocumento = vista.NumeroDocumento
                                                            }
                                                };



            if (objbeneficiario.TieneApoderado == "N")
            {
                var cuentas = _cuentabancorepositorio.GetCuentaBancoByBeneficiario(objbeneficiario.IdBeneficiario);

                if (cuentas.Count != 0)
                {
                    foreach (var cuenta in cuentas)
                    {
                        cuenta.IdSucursal = objbeneficiario.IdSucursal == 0 ? null : objbeneficiario.IdSucursal;
                        cuenta.NroCta = objbeneficiario.NumeroCuenta;
                        cuenta.IdMoneda = Convert.ToInt16(vista.Monedas.Selected);
                        cuenta.Cbu = objbeneficiario.Cbu;
                        cuenta.FechaSolicitudCuenta = cuenta.FechaSolicitudCuenta ?? DateTime.Now;
                        _cuentabancorepositorio.UpdateCuentaBanco(cuenta);
                    }
                }
                else
                {
                    ICuentaBanco cuenta = new CuentaBanco
                                              {
                                                  Cbu = objbeneficiario.Cbu,
                                                  NroCta = objbeneficiario.NumeroCuenta,
                                                  IdSucursal =
                                                      (objbeneficiario.IdSucursal == 0
                                                           ? null
                                                           : objbeneficiario.IdSucursal),
                                                  IdBeneficiario = objbeneficiario.IdBeneficiario,
                                                  IdMoneda = Constantes.MonedaBanco,
                                                  IdSistema = Constantes.IdSistemaBanco,
                                                  FechaSolicitudCuenta = DateTime.Now
                                              };


                    _cuentabancorepositorio.AddCuentaBanco(cuenta);
                }

            }
            else
            {
                if (objbeneficiario.IdEstado != (int)Enums.EstadoBeneficiario.Activo)
                {
                    _apoderadorepositorio.SuspenderApoderadoActivoByBeneficiario(objbeneficiario.IdBeneficiario);
                }

                if (objbeneficiario.IdEstado == (int)Enums.EstadoBeneficiario.Activo)
                {
                    _apoderadorepositorio.ActivarApoderadoActivoByBeneficiario(objbeneficiario.IdBeneficiario);
                }

            }

            return _beneficiariorepositorio.UpdateBeneficiario(objbeneficiario);

        }

        public IBeneficiariosVista GetBeneficiarioSinCuenta(string nombre, string apellido, string cuil, string numeroDocumento, int idPrograma, string conApoderados, string modalidad, string discapacitado, string altatemprana, int idEstado, string apellidonombreapoderado, int idEtapa, int idSubprograma, int TipoPrograma, string nrotramite)
        {
            IBeneficiariosVista vista = new BeneficiariosVista();
            vista.idSubprograma = idSubprograma;
            var accesoprograma = base.AccesoPrograma(Enums.Formulario.AbmBeneficiario);

            // 10/11/2020
            var tiposppp = _tipopppRepositorio.GetTiposPpp();
            vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = 0, Description = "Todos" });
            foreach (var tipo in tiposppp)
            {
                vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = tipo.ID_TIPO_PPP, Description = tipo.N_TIPO_PPP });
            }

            // 11/09/2014 - controlar aca si tiene el rol activado para condicionar los programas y subprogramas.
            var accesoprogr =
                                  _rolformularioaccionrepositorio.GetFormulariosDelUsuario(base.GetUsuarioLoguer().Id_Usuario).Where(
                                      c =>
                                      c.Id_Accion == (int)Enums.Acciones.ProgramasPorRol &&
                                      c.Id_Formulario == (int)Enums.Formulario.AbmBeneficiario).Count() > 0
                                      ? true
                                      : false;
            var usuarioRoles = _usuarioRolRepositorio.GetRolesDelUsuario(base.GetUsuarioLoguer().Id_Usuario);

            IList<IBeneficiario> beneficiarios;

            //*****************************************************************************************************************
            if (accesoprogr) //verifica si debe traer todos los programas asociados
            {
                vista.ProgPorRol = "S";
                beneficiarios = _beneficiariorepositorio.GetBeneficiarioSinCuenta(nombre, apellido, cuil,
                                                                                      numeroDocumento, idPrograma,
                                                                                      conApoderados, modalidad,
                                                                                      discapacitado, altatemprana, idEstado,
                                                                                      apellidonombreapoderado, idEtapa, nrotramite).ToList();
                if (idPrograma > 5 && idPrograma < 9) // Los programas en este rango tienen subprogramas
                {
                    if (vista.idSubprograma > 0) // si se consulta por subprograma
                    {
                        if (idPrograma == (int)Enums.TipoFicha.EfectoresSociales) // para el caso de efectores no debe traer los null
                        {
                            beneficiarios = beneficiarios.Where(c => c.idSubprograma == vista.idSubprograma).ToList();
                        }
                        else
                        {
                            beneficiarios = beneficiarios.Where(c => c.idSubprograma == vista.idSubprograma || c.idSubprograma == null).ToList();
                        }
                    }
                    else
                    {
                        if (idPrograma == (int)Enums.TipoFicha.EfectoresSociales) // para el caso de efectores no debe traer todos
                        {
                            //verifico que subprogramas tiene disponible
                            var lsubprogramas = (from rs in _subprogramaRepositorio.GetSubprogramasRol()
                                                 join u in usuarioRoles on rs.Id_Rol equals u.IdRol
                                                 where rs.IdPrograma == idPrograma
                                                 group rs by new { rs.IdSubprograma, rs.NombreSubprograma } into grouping
                                                 select new
                                                     Subprograma
                                                 {
                                                     IdSubprograma = grouping.Key.IdSubprograma,
                                                     NombreSubprograma = grouping.Key.NombreSubprograma
                                                 }).ToList();


                            beneficiarios = (from f in beneficiarios

                                             join sub in lsubprogramas on f.idSubprograma equals sub.IdSubprograma
                                             where f.idSubprograma != null
                                             select f).ToList();
                        }
                    }

                }
                else
                {

                }

            }

            //*****************************************************************************************************************
            else
            {

                //26/02/2013 - DI CAMPLI LEANDRO - SE MODIFICA PARA AÑADIR A LA VISTA UN ARRAY CON LOS IDBENEFICIARIO CONSULTADOS PARA MEJORAR EL RENDIMIENTO DE LAS CONSULTAS
                beneficiarios = accesoprograma == true
                                  ? _beneficiariorepositorio.GetBeneficiarioSinCuenta(nombre, apellido, cuil,
                                                                                      numeroDocumento, idPrograma,
                                                                                      conApoderados, modalidad,
                                                                                      discapacitado, altatemprana, idEstado,
                                                                                      apellidonombreapoderado, idEtapa, nrotramite).ToList() //.Count
                                  : _beneficiariorepositorio.GetBeneficiarioSinCuenta(nombre, apellido, cuil,
                                                                                      numeroDocumento, idPrograma,
                                                                                      conApoderados, modalidad,
                                                                                      discapacitado, altatemprana, idEstado,
                                                                                      apellidonombreapoderado, idEtapa, nrotramite).Where(
                                                                                          c =>
                                                                                          c.IdPrograma !=
                                                                                          (int)
                                                                                          Enums.Programas.EfectoresSociales).ToList(); //.Count();
                if (idSubprograma > 0) // si se consulta por subprograma
                {

                    beneficiarios = beneficiarios.Where(c => c.idSubprograma == idSubprograma).ToList();

                }
            }

            if (TipoPrograma > 0) // si se consulta por tipo programa
            {

                beneficiarios = beneficiarios.Where(c => c.TipoPrograma == TipoPrograma).ToList();

            }
            //26/02/2013 - DI CAMPLI LEANDRO - SE MODIFICA PARA AÑADIR A LA VISTA UN ARRAY CON LOS IDBENEFICIARIO CONSULTADOS PARA MEJORAR EL RENDIMIENTO DE LAS CONSULTAS
            int cantreg = 0;
            cantreg = beneficiarios.Count();
            vista.idBeneficiarios = beneficiarios.Select(c => c.IdBeneficiario).ToArray();
            var pager =
                new Pager(cantreg
                    ,
                    Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                    "FormIndexBeneficiarios", _au.GetUrl("IndexPager", "Beneficiarios"));

            vista.Pager = pager;

            vista.ListaBeneficiarios = beneficiarios.Skip(pager.Skip).Take(pager.PageSize).ToList(); // 11/09/2013 - DI CAMPLI LEANDRO

            

            vista.ConCuenta = "N";
            vista.ConAltaTemprana = altatemprana;
            vista.ConApoderado = conApoderados;
            vista.ConDiscapacidad = discapacitado;
            vista.ConModalidad = modalidad;

            foreach (var beneficiario in vista.ListaBeneficiarios.Where(beneficiario => beneficiario.Ficha.EstadoCivil == (int)Enums.EstadoCivil.CasadoConviviente))
            {
                beneficiario.Conyugue = _conyuguerepositorio.GetConyugeByBeneficiario(beneficiario.IdBeneficiario) ?? new Conyuge();
            }

            var lista = _programarepositorio.GetProgramas();

            if (accesoprogr) //verifica si debe traer todos los programas asociados
            {
                vista.ProgPorRol = "S";

                var lprogramas = from rp in _programarepositorio.GetTipoFichasRol()
                                 join u in usuarioRoles on rp.Id_Rol equals u.IdRol
                                 group rp by new { rp.IdPrograma, rp.NombrePrograma } into grouping
                                 select new
                                     Programa
                                 {
                                     IdPrograma = grouping.Key.IdPrograma,
                                     NombrePrograma = grouping.Key.NombrePrograma
                                 };

                CargarProgramaRol(vista, lprogramas.OrderBy(c => c.IdPrograma));
            }
            else
            {
                vista.ProgPorRol = "N";
                CargarPrograma(vista, lista);
            }
            // 10/06/2013 - DI CAMPLI LEANDRO - CARGAN LOS PROGRAMAS PARA LAS ETAPAS
            CargarProgramaEtapa(vista, lista);

            vista.Programas.Selected = idPrograma.ToString();

            CargarEstadosBeneficiarioforIndex(vista, _estadobeneficiariorepositorio.GetEstadosBeneficiario());

            return vista;
        }

        public IBeneficiariosVista GetBeneficiarioSinCuenta(IPager pPager, string fileDownload, string fileNombre, string nombre, string apellido, string cuil,
            string numeroDocumento, int idPrograma, string conApoderados, string modalidad, string discapacitado, string altatemprana, int idEstado, string apellidonombreapoderado, int idEtapa, int idSubprograma, int TipoPrograma, string nrotramite)
        {
            IBeneficiariosVista vista = new BeneficiariosVista();
            vista.idSubprograma = idSubprograma;

            // 10/11/2020
            var tiposppp = _tipopppRepositorio.GetTiposPpp();
            vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = 0, Description = "Todos" });
            foreach (var tipo in tiposppp)
            {
                vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = tipo.ID_TIPO_PPP, Description = tipo.N_TIPO_PPP });
            }

            bool accesoprograma = base.AccesoPrograma(Enums.Formulario.AbmBeneficiario);

            // 11/09/2014 - controlar aca si tiene el rol activado para condicionar los programas y subprogramas.
            var accesoprogr =
                                  _rolformularioaccionrepositorio.GetFormulariosDelUsuario(base.GetUsuarioLoguer().Id_Usuario).Where(
                                      c =>
                                      c.Id_Accion == (int)Enums.Acciones.ProgramasPorRol &&
                                      c.Id_Formulario == (int)Enums.Formulario.AbmBeneficiario).Count() > 0
                                      ? true
                                      : false;
            var usuarioRoles = _usuarioRolRepositorio.GetRolesDelUsuario(base.GetUsuarioLoguer().Id_Usuario);


            IList<IBeneficiario> beneficiarios;
            //*****************************************************************************************************************
            if (accesoprogr) //verifica si debe traer todos los programas asociados
            {
                vista.ProgPorRol = "S";
                beneficiarios = _beneficiariorepositorio.GetBeneficiarioSinCuenta(nombre, apellido, cuil,
                                                                                      numeroDocumento, idPrograma,
                                                                                      conApoderados, modalidad,
                                                                                      discapacitado, altatemprana, idEstado,
                                                                                      apellidonombreapoderado, idEtapa, nrotramite).ToList();
                if (idPrograma > 5 && idPrograma < 9) // Los programas en este rango tienen subprogramas
                {
                    if (vista.idSubprograma > 0) // si se consulta por subprograma
                    {
                        if (idPrograma == (int)Enums.TipoFicha.EfectoresSociales) // para el caso de efectores no debe traer los null
                        {
                            beneficiarios = beneficiarios.Where(c => c.idSubprograma == vista.idSubprograma).ToList();
                        }
                        else
                        {
                            beneficiarios = beneficiarios.Where(c => c.idSubprograma == vista.idSubprograma || c.idSubprograma == null).ToList();
                        }
                    }
                    else
                    {
                        if (idPrograma == (int)Enums.TipoFicha.EfectoresSociales) // para el caso de efectores no debe traer todos
                        {
                            //verifico que subprogramas tiene disponible
                            var lsubprogramas = (from rs in _subprogramaRepositorio.GetSubprogramasRol()
                                                 join u in usuarioRoles on rs.Id_Rol equals u.IdRol
                                                 where rs.IdPrograma == idPrograma
                                                 group rs by new { rs.IdSubprograma, rs.NombreSubprograma } into grouping
                                                 select new
                                                     Subprograma
                                                 {
                                                     IdSubprograma = grouping.Key.IdSubprograma,
                                                     NombreSubprograma = grouping.Key.NombreSubprograma
                                                 }).ToList();


                            beneficiarios = (from f in beneficiarios

                                             join sub in lsubprogramas on f.idSubprograma equals sub.IdSubprograma
                                             where f.idSubprograma != null
                                             select f).ToList();
                        }
                    }

                }
                else
                {

                }

            }

            //*****************************************************************************************************************
            else
            {
                beneficiarios = accesoprograma
                                  ? _beneficiariorepositorio.GetBeneficiarioSinCuenta(nombre, apellido, cuil,
                                                                                      numeroDocumento, idPrograma,
                                                                                      conApoderados, modalidad,
                                                                                      discapacitado, altatemprana, idEstado,
                                                                                      apellidonombreapoderado, idEtapa, nrotramite).ToList()
                                  : _beneficiariorepositorio.GetBeneficiarioSinCuenta(nombre, apellido, cuil,
                                                                                      numeroDocumento, idPrograma,
                                                                                      conApoderados, modalidad,
                                                                                      discapacitado, altatemprana, idEstado,
                                                                                      apellidonombreapoderado, idEtapa, nrotramite).Where(
                                                                                          c =>
                                                                                          c.IdPrograma !=
                                                                                          (int)
                                                                                          Enums.Programas.EfectoresSociales).ToList();
                if (idSubprograma > 0) // si se consulta por subprograma
                {

                    beneficiarios = beneficiarios.Where(c => c.idSubprograma == idSubprograma).ToList();

                }
            }

            if (TipoPrograma > 0) // si se consulta por tipo programa
            {

                beneficiarios = beneficiarios.Where(c => c.TipoPrograma == TipoPrograma).ToList();

            }

            int cantreg = beneficiarios.Count();

            var pager =
                new Pager(cantreg
                    ,
                    Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                    "FormIndexBeneficiarios", _au.GetUrl("IndexPager", "Beneficiarios"));

            if (pager.TotalCount != pPager.TotalCount)
            {
                pager.PageNumber = 1;
                pager.Skip = 0;
                vista.Pager = pager;
            }
            else
            {
                vista.Pager = pPager;
            }

            vista.ApellidoBusqueda = apellido;
            vista.CuilBusqueda = cuil;
            vista.NombreBusqueda = nombre;
            vista.NumeroDocumentoBusqueda = numeroDocumento;

            vista.ListaBeneficiarios = beneficiarios.Skip(vista.Pager.Skip).Take(vista.Pager.PageSize).ToList(); // 12/09/2013 - DI CAMPLI LEANDRO

            vista.ConCuenta = "N";
            vista.ConAltaTemprana = altatemprana;
            vista.ConApoderado = conApoderados;
            vista.ConDiscapacidad = discapacitado;
            vista.ConModalidad = modalidad;

            foreach (var beneficiario in vista.ListaBeneficiarios)
            {
                if (beneficiario.Ficha.EstadoCivil == 1)
                {
                    beneficiario.Conyugue =
                        _conyuguerepositorio.GetConyugeByBeneficiario(beneficiario.IdBeneficiario) ?? new Conyuge();
                }

            }

            var lista = _programarepositorio.GetProgramas();

            if (accesoprogr) //verifica si debe traer todos los programas asociados
            {
                vista.ProgPorRol = "S";

                var lprogramas = from rp in _programarepositorio.GetTipoFichasRol()
                                 join u in usuarioRoles on rp.Id_Rol equals u.IdRol
                                 group rp by new { rp.IdPrograma, rp.NombrePrograma } into grouping
                                 select new
                                     Programa
                                 {
                                     IdPrograma = grouping.Key.IdPrograma,
                                     NombrePrograma = grouping.Key.NombrePrograma
                                 };

                CargarProgramaRol(vista, lprogramas.OrderBy(c => c.IdPrograma));
            }
            else
            {
                vista.ProgPorRol = "N";
                CargarPrograma(vista, lista);
            }
            // 10/06/2013 - DI CAMPLI LEANDRO - CARGAN LOS PROGRAMAS PARA LAS ETAPAS
            CargarProgramaEtapa(vista, lista);

            CargarEstadosBeneficiarioforIndex(vista, _estadobeneficiariorepositorio.GetEstadosBeneficiario());

            vista.Programas.Selected = idPrograma.ToString();

            vista.EstadosBeneficiarios.Selected = idEstado.ToString();

            return vista;
        }

        private void CargarPrograma(IBeneficiariosVista vista, IEnumerable<IPrograma> listaProgramas)
        {
            vista.Programas.Combo.Add(new ComboItem
                                          {
                                              Id = 0,
                                              Description = "TODOS"

                                          });


            foreach (var item in listaProgramas)
            {
                if ((int)Enums.Programas.EfectoresSociales != item.IdPrograma)
                {
                    vista.Programas.Combo.Add(new ComboItem
                                                  {
                                                      Id = item.IdPrograma,
                                                      Description = item.NombrePrograma

                                                  });
                }
                else
                {
                    if (base.AccesoPrograma(Enums.Formulario.AbmBeneficiario))
                        vista.Programas.Combo.Add(new ComboItem
                        {
                            Id = item.IdPrograma,
                            Description = item.NombrePrograma

                        });
                }
            }



        }

        private void CargarProgramaRol(IBeneficiariosVista vista, IEnumerable<IPrograma> listaProgramas)
        {
            vista.Programas.Combo.Add(new ComboItem
            {
                Id = -1,
                Description = "Seleccione un Programa..."

            });


            foreach (var item in listaProgramas)
            {

                        vista.Programas.Combo.Add(new ComboItem
                        {
                            Id = item.IdPrograma,
                            Description = item.NombrePrograma

                        });
                
            }



        }

        public IBeneficiariosVista GetBeneficiarioSinCuentaFile(string nombre, string apellido, string cuil, string numeroDocumento, int idPrograma, string conApoderados, string modalidad, string discapacitado, string altatemprana, int idEstado, int idSubprograma, int TipoPrograma, string nrotramite)
        {
            IBeneficiariosVista vista = new BeneficiariosVista();

            vista.idSubprograma = idSubprograma;
            
            // 11/09/2014 - controlar aca si tiene el rol activado para condicionar los programas y subprogramas.
            var accesoprogr =
                                  _rolformularioaccionrepositorio.GetFormulariosDelUsuario(base.GetUsuarioLoguer().Id_Usuario).Where(
                                      c =>
                                      c.Id_Accion == (int)Enums.Acciones.ProgramasPorRol &&
                                      c.Id_Formulario == (int)Enums.Formulario.AbmBeneficiario).Count() > 0
                                      ? true
                                      : false;
            var usuarioRoles = _usuarioRolRepositorio.GetRolesDelUsuario(base.GetUsuarioLoguer().Id_Usuario);


            IList<IBeneficiario> listabeneficiariogenerarcuenta = _beneficiariorepositorio.GetBeneficiarioSinCuenta("", "",
                                                                                                                  "", "",
                                                                                                                  idPrograma,
                                                                                                                  conApoderados,
                                                                                                                  modalidad,
                                                                                                                  discapacitado,
                                                                                                                  altatemprana,
                                                                                                                  idEstado,
                                                                                                                  "", 0, nrotramite);



            //*****************************************************************************************************************

            if (accesoprogr) //verifica si debe traer todos los programas asociados
            {


                if (idPrograma > 5 && idPrograma < 9) // Los programas en este rango tienen subprogramas
                {
                    if (vista.idSubprograma > 0) // si se consulta por subprograma
                    {
                        if (idPrograma == (int)Enums.TipoFicha.EfectoresSociales) // para el caso de efectores no debe traer los null
                        {
                            listabeneficiariogenerarcuenta = listabeneficiariogenerarcuenta.Where(c => c.idSubprograma == vista.idSubprograma).ToList();
                        }
                        else
                        {
                            listabeneficiariogenerarcuenta = listabeneficiariogenerarcuenta.Where(c => c.idSubprograma == vista.idSubprograma || c.idSubprograma == null).ToList();
                        }
                    }
                    else
                    {
                        if (idPrograma == (int)Enums.TipoFicha.EfectoresSociales) // para el caso de efectores no debe traer todos
                        {
                            //verifico que subprogramas tiene disponible
                            var lsubprogramas = (from rs in _subprogramaRepositorio.GetSubprogramasRol()
                                                 join u in usuarioRoles on rs.Id_Rol equals u.IdRol
                                                 where rs.IdPrograma == idPrograma
                                                 group rs by new { rs.IdSubprograma, rs.NombreSubprograma } into grouping
                                                 select new
                                                     Subprograma
                                                 {
                                                     IdSubprograma = grouping.Key.IdSubprograma,
                                                     NombreSubprograma = grouping.Key.NombreSubprograma
                                                 }).ToList();


                            listabeneficiariogenerarcuenta = (from f in listabeneficiariogenerarcuenta

                                                              join sub in lsubprogramas on f.idSubprograma equals sub.IdSubprograma
                                                              where f.idSubprograma != null
                                                              select f).ToList();
                        }
                    }

                }
                else
                {

                }

            }
            else
            {

                if (idSubprograma > 0) // si se consulta por subprograma
                {

                    listabeneficiariogenerarcuenta = listabeneficiariogenerarcuenta.Where(c => c.idSubprograma == idSubprograma).ToList();

                }
            }

            if (TipoPrograma > 0) // si se consulta por tipo programa
            {

                listabeneficiariogenerarcuenta = listabeneficiariogenerarcuenta.Where(c => c.TipoPrograma == TipoPrograma).ToList();

            }
            

            int[] listabeneficiarioarray = listabeneficiariogenerarcuenta.Select(c => c.IdBeneficiario).ToArray();

            //Vemos los Errores de Datos---------------------------------------------------------------------------

            IList<IControlDatos> listadatoserroneos =
                _controldatosrepositorio.GetApoderadosDuplicadosActivos().Union(
                    _controldatosrepositorio.GetBeneficiariosMonedaIncorrecta()).ToList();

            int cantidadconerror = listadatoserroneos.Where(c => listabeneficiarioarray.Contains(c.Id)).Count();


            //-----------------------------------------------------------------------------------------------------

            vista.FileNombre = cantidadconerror == 0 ? GenerarFile(listabeneficiariogenerarcuenta) : "Error en Datos";



            vista.FileDownload = "../Archivos/" + vista.FileNombre;

            vista.ConAltaTemprana = altatemprana;
            vista.ConApoderado = conApoderados;
            vista.ConDiscapacidad = discapacitado;
            vista.ConModalidad = modalidad;
            vista.ConCuenta = "N";
            vista.ErrorDatos = vista.FileNombre != "Error en Datos" ? false : true;

            return vista;
        }

        /// <summary>
        /// GetBeneficiarioConCuenta para BUSCAR BENEFICIARIO
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="apellido"></param>
        /// <param name="cuil"></param>
        /// <param name="numeroDocumento"></param>
        /// <param name="idPrograma"></param>
        /// <param name="conApoderados"></param>
        /// <param name="modalidad"></param>
        /// <param name="discapacitado"></param>
        /// <param name="altatemprana"></param>
        /// <param name="idEstado"></param>
        /// <param name="apellidonombreapoderado"></param>
        /// <param name="Filtroejerc"></param>
        /// <param name="progrEtapaSel"></param>
        /// <param name="etapaSel"></param>
        /// <returns></returns>
        public IBeneficiariosVista GetBeneficiarioConCuenta(string nombre, string apellido, string cuil, string numeroDocumento, int idPrograma, string conApoderados, string modalidad, string discapacitado, string altatemprana, int idEstado, string apellidonombreapoderado, int idEtapa, int idSubprograma, int TipoPrograma, string nrotramite)
        {
            IBeneficiariosVista vista = new BeneficiariosVista();
            vista.idSubprograma = idSubprograma;
            bool accesoprograma = base.AccesoPrograma(Enums.Formulario.AbmBeneficiario);

            // 10/11/2020
            var tiposppp = _tipopppRepositorio.GetTiposPpp();
            vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = 0, Description = "Todos" });
            foreach (var tipo in tiposppp)
            {
                vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = tipo.ID_TIPO_PPP, Description = tipo.N_TIPO_PPP });
            }


            // 09/09/2014 - controlar aca si tiene el rol activado para condicionar los programas y subprogramas.
            var accesoprogr =
                                  _rolformularioaccionrepositorio.GetFormulariosDelUsuario(base.GetUsuarioLoguer().Id_Usuario).Where(
                                      c =>
                                      c.Id_Accion == (int)Enums.Acciones.ProgramasPorRol &&
                                      c.Id_Formulario == (int)Enums.Formulario.AbmBeneficiario).Count() > 0
                                      ? true
                                      : false;
            var usuarioRoles = _usuarioRolRepositorio.GetRolesDelUsuario(base.GetUsuarioLoguer().Id_Usuario);


            // 01/07/2013 - DI CAMPLI LEANDRO - SE CREA LISTA PARA NO LLAMAR DOS VECES A LA BASE DE DATOS
            IList<IBeneficiario> beneficiarios;



            //*****************************************************************************************************************
            if (accesoprogr) //verifica si debe traer todos los programas asociados
            {
                vista.ProgPorRol = "S";
                beneficiarios = _beneficiariorepositorio.GetBeneficiarioConCuenta(nombre, apellido, cuil,
                                                                                  numeroDocumento, idPrograma,
                                                                                  conApoderados, modalidad,
                                                                                  discapacitado, altatemprana, idEstado,
                                                                                  apellidonombreapoderado, idEtapa, nrotramite).ToList();
                if (idPrograma > 5 && idPrograma < 9) // Los programas en este rango tienen subprogramas
                {
                    if (vista.idSubprograma > 0) // si se consulta por subprograma
                    {
                        if (idPrograma == (int)Enums.TipoFicha.EfectoresSociales) // para el caso de efectores no debe traer los null
                        {
                            beneficiarios = beneficiarios.Where(c => c.idSubprograma == vista.idSubprograma).ToList();
                        }
                        else
                        {
                            beneficiarios = beneficiarios.Where(c => c.idSubprograma == vista.idSubprograma || c.idSubprograma == null).ToList();
                        }
                    }
                    else
                    {
                        if (idPrograma == (int)Enums.TipoFicha.EfectoresSociales) // para el caso de efectores no debe traer todos
                        {
                            //verifico que subprogramas tiene disponible
                            var lsubprogramas = (from rs in _subprogramaRepositorio.GetSubprogramasRol()
                                                 join u in usuarioRoles on rs.Id_Rol equals u.IdRol
                                                 where rs.IdPrograma == idPrograma
                                                 group rs by new { rs.IdSubprograma, rs.NombreSubprograma } into grouping
                                                 select new
                                                     Subprograma
                                                 {
                                                     IdSubprograma = grouping.Key.IdSubprograma,
                                                     NombreSubprograma = grouping.Key.NombreSubprograma
                                                 }).ToList();


                            beneficiarios = (from f in beneficiarios

                                             join sub in lsubprogramas on f.idSubprograma equals sub.IdSubprograma
                                             where f.idSubprograma != null
                                             select f).ToList();
                        }
                    }

                }
                else
                {

                }

            }

            //*****************************************************************************************************************
            else
            {

                //26/02/2013 - DI CAMPLI LEANDRO - SE MODIFICA PARA AÑADIR A LA VISTA UN ARRAY CON LOS IDBENEFICIARIO CONSULTADOS PARA MEJORAR EL RENDIMIENTO DE LAS CONSULTAS
                beneficiarios = accesoprograma
                                  ? _beneficiariorepositorio.GetBeneficiarioConCuenta(nombre, apellido, cuil,
                                                                                      numeroDocumento, idPrograma,
                                                                                      conApoderados, modalidad,
                                                                                      discapacitado, altatemprana, idEstado,
                                                                                      apellidonombreapoderado, idEtapa, nrotramite).ToList()//.Select(c => c.IdBeneficiario).ToArray() //.Count
                                  : _beneficiariorepositorio.GetBeneficiarioConCuenta(nombre, apellido, cuil,
                                                                                      numeroDocumento, idPrograma,
                                                                                      conApoderados, modalidad,
                                                                                      discapacitado, altatemprana, idEstado,
                                                                                      apellidonombreapoderado, idEtapa, nrotramite).Where(c => c.IdPrograma != (int)Enums.Programas.EfectoresSociales).ToList();//.Select(c => c.IdBeneficiario).ToArray(); //.Count();

                if (idSubprograma > 0) // si se consulta por subprograma
                {

                    beneficiarios = beneficiarios.Where(c => c.idSubprograma == idSubprograma).ToList();

                }
            }

            if (TipoPrograma > 0) // si se consulta por tipo programa
            {

                beneficiarios = beneficiarios.Where(c => c.TipoPrograma == TipoPrograma).ToList();

            }
            //26/02/2013 - DI CAMPLI LEANDRO - SE MODIFICA PARA AÑADIR A LA VISTA UN ARRAY CON LOS IDBENEFICIARIO CONSULTADOS PARA MEJORAR EL RENDIMIENTO DE LAS CONSULTAS
            int cantreg = 0;
            //cantreg =vista.idBeneficiarios.Count();
            cantreg = beneficiarios.Count();
            vista.idBeneficiarios = beneficiarios.Select(c => c.IdBeneficiario).ToArray();
            var pager =
                new Pager(cantreg
                    ,
                    Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                    "FormIndexBeneficiarios", _au.GetUrl("IndexPager", "Beneficiarios"));

            vista.Pager = pager;
            
            vista.ListaBeneficiarios = beneficiarios.Skip(pager.Skip).Take(pager.PageSize).ToList(); // 01/07/2013 - DI CAMPLI LEANDRO
            

            vista.ConAltaTemprana = altatemprana;
            vista.ConApoderado = conApoderados;
            vista.ConDiscapacidad = discapacitado;
            vista.ConModalidad = modalidad;
            vista.ConCuenta = "S";

            foreach (var beneficiario in vista.ListaBeneficiarios.
                Where(beneficiario => beneficiario.Ficha.EstadoCivil == (int)Enums.EstadoCivil.CasadoConviviente))
            {
                beneficiario.Conyugue = _conyuguerepositorio.GetConyugeByBeneficiario(beneficiario.IdBeneficiario) ?? new Conyuge();
            }

            var lista = _programarepositorio.GetProgramas();

            if (accesoprogr) //verifica si debe traer todos los programas asociados
            {
                vista.ProgPorRol = "S";

                var lprogramas = from rp in _programarepositorio.GetTipoFichasRol()
                                 join u in usuarioRoles on rp.Id_Rol equals u.IdRol
                                 group rp by new { rp.IdPrograma, rp.NombrePrograma } into grouping
                                 select new
                                     Programa
                                 {
                                     IdPrograma = grouping.Key.IdPrograma,
                                     NombrePrograma = grouping.Key.NombrePrograma
                                 };

                CargarProgramaRol(vista, lprogramas.OrderBy(c => c.IdPrograma));
            }
            else
            {
                vista.ProgPorRol = "N";
                CargarPrograma(vista, lista);
            }
            
            // 10/06/2013 - DI CAMPLI LEANDRO - CARGAN LOS PROGRAMAS PARA LAS ETAPAS
            CargarProgramaEtapa(vista, lista);

            CargarEstadosBeneficiarioforIndex(vista, _estadobeneficiariorepositorio.GetEstadosBeneficiario());

            vista.Programas.Selected = idPrograma.ToString();

            vista.EstadosBeneficiarios.Selected = idEstado.ToString();

            return vista;
        }

        public IBeneficiariosVista GetBeneficiarioConCuenta(IPager pPager, string fileDownload, string fileNombre, string nombre, string apellido,
            string cuil, string numeroDocumento, int idPrograma, string conApoderados, string modalidad, string discapacitado, string altatemprana, int idEstado, string apellidonombreapoderado, int idEtapa, int idSubprograma, int TipoPrograma, string nrotramite)
        {
            IBeneficiariosVista vista = new BeneficiariosVista();
            vista.idSubprograma = idSubprograma;

            // 10/11/2020
            var tiposppp = _tipopppRepositorio.GetTiposPpp();
            vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = 0, Description = "Todos" });
            foreach (var tipo in tiposppp)
            {
                vista.ComboTiposPpp.Combo.Add(new ComboItem { Id = tipo.ID_TIPO_PPP, Description = tipo.N_TIPO_PPP });
            }

            // 09/09/2014 - controlar aca si tiene el rol activado para condicionar los programas y subprogramas.
            var accesoprogr =
                                  _rolformularioaccionrepositorio.GetFormulariosDelUsuario(base.GetUsuarioLoguer().Id_Usuario).Where(
                                      c =>
                                      c.Id_Accion == (int)Enums.Acciones.ProgramasPorRol &&
                                      c.Id_Formulario == (int)Enums.Formulario.AbmBeneficiario).Count() > 0
                                      ? true
                                      : false;
            var usuarioRoles = _usuarioRolRepositorio.GetRolesDelUsuario(base.GetUsuarioLoguer().Id_Usuario);



            // 01/07/2013 - DI CAMPLI LEANDRO - SE CREA LISTA PARA NO LLAMAR DOS VECES A LA BASE DE DATOS
            IList<IBeneficiario> beneficiarios;

            bool accesoprograma = base.AccesoPrograma(Enums.Formulario.AbmBeneficiario);
            
            //*****************************************************************************************************************
            if (accesoprogr) //verifica si debe traer todos los programas asociados
            {
                vista.ProgPorRol = "S";
                beneficiarios = _beneficiariorepositorio.GetBeneficiarioConCuenta(nombre, apellido, cuil,
                                                                                  numeroDocumento, idPrograma,
                                                                                  conApoderados, modalidad,
                                                                                  discapacitado, altatemprana, idEstado,
                                                                                  apellidonombreapoderado, idEtapa, nrotramite).ToList();
                if (idPrograma > 5 && idPrograma < 9) // Los programas en este rango tienen subprogramas
                {
                    if (vista.idSubprograma > 0) // si se consulta por subprograma
                    {
                        if (idPrograma == (int)Enums.TipoFicha.EfectoresSociales) // para el caso de efectores no debe traer los null
                        {
                            beneficiarios = beneficiarios.Where(c => c.idSubprograma == vista.idSubprograma).ToList();
                        }
                        else
                        {
                            beneficiarios = beneficiarios.Where(c => c.idSubprograma == vista.idSubprograma || c.idSubprograma == null).ToList();
                        }
                    }
                    else
                    {
                        if (idPrograma == (int)Enums.TipoFicha.EfectoresSociales) // para el caso de efectores no debe traer todos
                        {
                            //verifico que subprogramas tiene disponible
                            var lsubprogramas = (from rs in _subprogramaRepositorio.GetSubprogramasRol()
                                                 join u in usuarioRoles on rs.Id_Rol equals u.IdRol
                                                 where rs.IdPrograma == idPrograma
                                                 group rs by new { rs.IdSubprograma, rs.NombreSubprograma } into grouping
                                                 select new
                                                     Subprograma
                                                 {
                                                     IdSubprograma = grouping.Key.IdSubprograma,
                                                     NombreSubprograma = grouping.Key.NombreSubprograma
                                                 }).ToList();


                            beneficiarios = (from f in beneficiarios

                                             join sub in lsubprogramas on f.idSubprograma equals sub.IdSubprograma
                                             where f.idSubprograma != null
                                             select f).ToList();
                        }
                    }

                }
                else
                {

                }

            }

            //*****************************************************************************************************************
            else
            {
            beneficiarios = accesoprograma
                              ? _beneficiariorepositorio.GetBeneficiarioConCuenta(nombre, apellido, cuil,
                                                                                  numeroDocumento, idPrograma,
                                                                                  conApoderados, modalidad,
                                                                                  discapacitado, altatemprana, idEstado,
                                                                                  apellidonombreapoderado, idEtapa, nrotramite).ToList()//.Count
                              : _beneficiariorepositorio.GetBeneficiarioConCuenta(nombre, apellido, cuil,
                                                                                  numeroDocumento, idPrograma,
                                                                                  conApoderados, modalidad,
                                                                                  discapacitado, altatemprana,
                                                                                  idEstado, apellidonombreapoderado, idEtapa, nrotramite).Where(c => c.IdPrograma != (int)Enums.Programas.EfectoresSociales).ToList();//.Count();

                if (idSubprograma > 0) // si se consulta por subprograma
                {

                    beneficiarios = beneficiarios.Where(c => c.idSubprograma == idSubprograma).ToList();

                }
            }

            if (TipoPrograma > 0) // si se consulta por tipo programa
            {

                beneficiarios = beneficiarios.Where(c => c.TipoPrograma == TipoPrograma).ToList();

            }
            int cantreg = 0;
            cantreg = beneficiarios.Count();

            var pager =
                new Pager(cantreg
                    ,
                    Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                    "FormIndexBeneficiarios", _au.GetUrl("IndexPager", "Beneficiarios"));

            if (pager.TotalCount != pPager.TotalCount)
            {
                pager.PageNumber = 1;
                pager.Skip = 0;
                vista.Pager = pager;
            }
            else
            {
                vista.Pager = pPager;
            }

            vista.ApellidoBusqueda = apellido;
            vista.CuilBusqueda = cuil;
            vista.NombreBusqueda = nombre;
            vista.NumeroDocumentoBusqueda = numeroDocumento;

          
            vista.ListaBeneficiarios = beneficiarios.Skip(vista.Pager.Skip).Take(vista.Pager.PageSize).ToList(); // 01/07/2013 - DI CAMPLI LEANDRO

            vista.ConAltaTemprana = altatemprana;
            vista.ConApoderado = conApoderados;
            vista.ConDiscapacidad = discapacitado;
            vista.ConModalidad = modalidad;
            vista.ConCuenta = "S";

            foreach (var beneficiario in
                vista.ListaBeneficiarios.Where(beneficiario => beneficiario.Ficha.EstadoCivil == (int)Enums.EstadoCivil.CasadoConviviente))
            {
                beneficiario.Conyugue = _conyuguerepositorio.GetConyugeByBeneficiario(beneficiario.IdBeneficiario) ?? new Conyuge();
            }

            var lista = _programarepositorio.GetProgramas();
            if (accesoprogr) //verifica si debe traer todos los programas asociados
            {
                vista.ProgPorRol = "S";

                var lprogramas = from rp in _programarepositorio.GetTipoFichasRol()
                                 join u in usuarioRoles on rp.Id_Rol equals u.IdRol
                                 group rp by new { rp.IdPrograma, rp.NombrePrograma } into grouping
                                 select new
                                     Programa
                                 {
                                     IdPrograma = grouping.Key.IdPrograma,
                                     NombrePrograma = grouping.Key.NombrePrograma
                                 };

                CargarProgramaRol(vista, lprogramas.OrderBy(c => c.IdPrograma));
            }
            else
            {
                vista.ProgPorRol = "N";
                CargarPrograma(vista, lista);
            }
            // 10/06/2013 - DI CAMPLI LEANDRO - CARGAN LOS PROGRAMAS PARA LAS ETAPAS
            CargarProgramaEtapa(vista, lista);

            CargarEstadosBeneficiarioforIndex(vista, _estadobeneficiariorepositorio.GetEstadosBeneficiario());

            vista.Programas.Selected = idPrograma.ToString();

            vista.EstadosBeneficiarios.Selected = idEstado.ToString();

            return vista;
        }

        //public string GenerarFile(int idPrograma, string conApoderados, string modalidad, string discapacitado, string altatemprana, int idEstado)
        public string GenerarFile(IList<IBeneficiario> lista)
        {

            var listabeneficiario = lista;
            var fechasolicitud = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"));

            List<IApoderado> lstApo = new List<IApoderado>();
            int Notwrite = 0;
            string Apodocumento = string.Empty;

            if (listabeneficiario.Count <= 0)
            {
                return "";
            }

            string nomfile = string.Empty;
            nomfile = "cli" + listabeneficiario[0].NumeroConvenio.PadLeft(5, '0') + "_" + Convert.ToString(DateTime.Now.Day) + Convert.ToString(DateTime.Now.Month) + Convert.ToString(DateTime.Now.Year) + Convert.ToString(DateTime.Now.Hour) + Convert.ToString(DateTime.Now.Minute) + Convert.ToString(DateTime.Now.Second) +
                                                   ".hab";

            var file =
                HttpContext.Current.Server.MapPath("/Archivos/" + nomfile);

            var fiinfo = new FileInfo(file);

            if (fiinfo.Exists)
            {

            }
            else
            {
                StreamWriter sw1 = new StreamWriter(file);
                sw1.Close();
            
            }
            FileStream fs = new FileStream(file, FileMode.Truncate, FileAccess.ReadWrite);
            
            //using (fiinfo.CreateText())
            //{
            //}

            if (fs.CanWrite/*fiinfo.Exists*/)
            {
                
                using (StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default))
                {

                foreach (var item in listabeneficiario)
                {
                    IBeneficiario beneficiario = item;
                    //using (StreamWriter sw =/* new StreamWriter(fs, System.Text.Encoding.UTF8) */fiinfo.AppendText())
                    //{
                        
                        string convenio = (beneficiario.NumeroConvenio).PadLeft(5, '0').Substring(0, 5);

                        if (beneficiario.Conyugue == null)
                        {
                            beneficiario.Conyugue = new Conyuge();
                        }

                        Notwrite = 0;
                        IApoderado apoderado = null;
                        if (beneficiario.TieneApoderado == "S")
                        {
                            apoderado = _apoderadorepositorio.GetApoderadoByBeneficiarioActivo(beneficiario.IdBeneficiario);
                            
                            _apoderadorepositorio.UpdateFechaSolicitud(apoderado.IdApoderado, fechasolicitud);

                            if (beneficiario.IdPrograma == 8)
                            {
                                Apodocumento = lstApo.Where(x => x.NumeroDocumento == apoderado.NumeroDocumento).Select(x => x.NumeroDocumento).SingleOrDefault() ?? "";
                                if (Apodocumento == "")
                                {
                                    lstApo.Add(apoderado);
                                    //agregar bandera para permitir insertar linea
                                    Notwrite = 0;
                                }
                                else
                                {
                                    //agregar bandera para impedir insertar linea
                                    Notwrite = 1;
                                }


                            
                            }

                            beneficiario = null;

                        }
                        else
                        {
                            _cuentabancorepositorio.UpdateFechaSolicitud(beneficiario.IdBeneficiario, fechasolicitud);
                        }


                        const string tiporegistro = "A";
                        var sucursal =
                            (apoderado == null ? beneficiario.CodigoSucursal : apoderado.CodigoSucursalBanco).PadLeft(5, '0').
                                Substring(0, 5);

                        const string moneda = "01";

                        var tipodocumento =
                            (apoderado == null
                                 ? TipoDniBco(beneficiario.Ficha.TipoDocumento ?? 0)
                                 : TipoDniBco(Convert.ToInt32((apoderado.TipoDocumento ?? 1)))).PadLeft(3, '0')
                                       .Substring(0, 3);

                        var nrodocumento =
                            (apoderado == null ? beneficiario.Ficha.NumeroDocumento ?? "" : apoderado.NumeroDocumento)
                                .PadLeft(11, '0').Substring(0, 11);

                        const string clavefiscal = "007";

                        var nroclavefiscal =
                            (apoderado == null ? beneficiario.Ficha.Cuil ?? "" : apoderado.Cuil ?? "").Replace("-",
                                                                                                         String.Empty).
                                PadLeft(11, '0').Substring(0, 11);

                        const string tipocuenta = "00";
                        const string nrocuenta = "000000000";
                        var fechaalta = DateTime.Now.ToString("yyyyMMdd").Replace("/", String.Empty).Trim().PadLeft(8, '0').Substring(0, 8);

                        if (fechaalta == "00010101")
                        {
                            fechaalta = "00000000";
                        }

                        var nombre = (apoderado == null ? beneficiario.Ficha.Nombre ?? "" : apoderado.Nombre).Split(' ');
                        var mSegundoNombre = "";
                        var mPrimerNombre = "";

                        if (nombre.Length > 1)
                        {
                            for (var i = 0; i < nombre.Length; i++)
                            {


                                if (nombre[i].Length <= 3 && mSegundoNombre == "")
                                {
                                    if (mPrimerNombre == "")
                                    {
                                        mPrimerNombre = nombre[i].Trim();
                                    }
                                    else
                                    {
                                        if (nombre[i - 1].Length <= 3 && mSegundoNombre == "")
                                        {
                                            mPrimerNombre = mPrimerNombre + " " + nombre[i].Trim();
                                        }
                                        else
                                        {
                                            if (mSegundoNombre == "")
                                            {
                                                mSegundoNombre = nombre[i].Trim();
                                            }
                                            else
                                            {
                                                mSegundoNombre = mSegundoNombre + " " + nombre[i].Trim();
                                            }
                                        }
                                    }


                                }
                                else
                                {
                                    if (mPrimerNombre == "")
                                    {
                                        mPrimerNombre = nombre[i].Trim();
                                    }
                                    else
                                    {
                                        if (nombre[i - 1].Length <= 3 && mSegundoNombre == "")
                                        {
                                            mPrimerNombre = mPrimerNombre + " " + nombre[i].Trim();
                                        }
                                        else
                                        {
                                            if (mSegundoNombre == "")
                                            {
                                                mSegundoNombre = nombre[i].Trim();
                                            }
                                            else
                                            {
                                                mSegundoNombre = mSegundoNombre + " " + nombre[i].Trim();
                                            }
                                        }
                                    }


                                }


                            }
                        }
                        else
                        {
                            mPrimerNombre = nombre[0].Trim();
                        }

                        var apellido =
                            (apoderado == null ? beneficiario.Ficha.Apellido ?? "" : apoderado.Apellido).Split(' ');

                        var mSegundoApellido = "";
                        var mPrimerApellido = "";

                        if (apellido.Length > 1)
                        {
                            for (int i = 0; i < apellido.Length; i++)
                            {



                                if (apellido[i].Length <= 3 && mSegundoApellido == "")
                                {
                                    if (mPrimerApellido == "")
                                    {
                                        mPrimerApellido = apellido[i].Trim();
                                    }
                                    else
                                    {
                                        if (apellido[i - 1].Length <= 3 && mSegundoApellido == "")
                                        {
                                            mPrimerApellido = mPrimerApellido + " " + apellido[i].Trim();
                                        }
                                        else
                                        {
                                            if (mSegundoApellido == "")
                                            {
                                                mSegundoApellido = apellido[i].Trim();
                                            }
                                            else
                                            {
                                                mSegundoApellido = mSegundoApellido + " " + apellido[i].Trim();
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (mPrimerApellido == "")
                                    {
                                        mPrimerApellido = apellido[i].Trim();
                                    }
                                    else
                                    {
                                        if (apellido[i - 1].Length <= 3 && mSegundoApellido == "")
                                        {
                                            mPrimerApellido = mPrimerApellido + " " + apellido[i].Trim();
                                        }
                                        else
                                        {
                                            if (mSegundoApellido == "")
                                            {
                                                mSegundoApellido = apellido[i].Trim();
                                            }
                                            else
                                            {
                                                mSegundoApellido = mSegundoApellido + " " + apellido[i].Trim();
                                            }
                                        }
                                    }


                                }

                            }


                        }
                        else
                        {
                            mPrimerApellido = apellido[0].Trim();
                        }

                        var primerapellido = RemoverSignosAcentosBis((mPrimerApellido ?? "").Trim()).PadRight(15, ' ').Substring(0, 15);
                        var segundoapellido = RemoverSignosAcentosBis((mSegundoApellido ?? "").Trim()).PadRight(15, ' ').Substring(0, 15);
                        var primernombre = RemoverSignosAcentosBis((mPrimerNombre ?? "").Trim()).PadRight(15, ' ').Substring(0, 15);
                        var segundonombre = RemoverSignosAcentosBis((mSegundoNombre ?? "").Trim()).PadRight(15, ' ').Substring(0, 15);

                        const string condicioniva = "04"; //se modificó 01 por 04

                        if (beneficiario != null)
                        {
                            if ((beneficiario.Ficha.Calle ?? "").Length > 30 || (beneficiario.Ficha.Calle ?? "") == "" || (beneficiario.Ficha.Numero ?? "") == "0" || (beneficiario.Ficha.Numero ?? "").Length == 1)
                            {
                                beneficiario.Ficha.Calle = ConfigurationManager.AppSettings["CalleFueraRango"];
                                beneficiario.Ficha.Numero = ConfigurationManager.AppSettings["NroFueraRango"];
                                beneficiario.Ficha.Barrio = ConfigurationManager.AppSettings["BarrioFueraRango"];
                            }
                        }

                        if (apoderado != null)
                        {
                            if ((apoderado.Calle ?? "").Length > 30 || (apoderado.Calle ?? "") == "" || (apoderado.Numero ?? "") == "0" || (apoderado.Calle ?? "").Length == 1)
                            {
                                apoderado.Calle = ConfigurationManager.AppSettings["CalleFueraRango"];
                                apoderado.Numero = ConfigurationManager.AppSettings["NroFueraRango"];
                                apoderado.Barrio = ConfigurationManager.AppSettings["BarrioFueraRango"];
                            }
                        }
                        var domicilioparticular =
                            (apoderado == null
                                 ? RemoverSignosAcentosBis(RemoverCaracter((beneficiario.Ficha.Calle ?? "")))
                                 : (RemoverSignosAcentosBis(RemoverCaracter(((apoderado.Calle ?? "")))) != ""
                                        ? RemoverSignosAcentosBis(RemoverCaracter((apoderado.Calle)))
                                        : ConfigurationManager.AppSettings["ApoderadoCalle"])).PadRight(
                                            30, ' ').Substring(0, 30);


                        if (beneficiario != null)
                        {
                            int numero;

                            int.TryParse(beneficiario.Ficha.Numero, out numero);

                            beneficiario.Ficha.Numero = numero.ToString();
                        }

                        var nrodomicilio =
                            (apoderado == null ? RemoverCaracter(beneficiario.Ficha.Numero ?? "") : ((apoderado.Numero ?? "") != "" ? apoderado.Numero : ConfigurationManager.AppSettings["ApoderadoNro"])).PadLeft(5, '0').Substring(0, 5);


                        var piso =
                            (apoderado == null ? RemoverCaracter((beneficiario.Ficha.Piso == " " ? "" : beneficiario.Ficha.Piso) ?? "") : ((apoderado.Piso ?? "") != "" ? apoderado.Piso : ConfigurationManager.AppSettings["ApoderadoPiso"])).PadLeft(2, '0').Substring(0, 2);

                        var departamento =
                            (apoderado == null ? RemoverCaracter((beneficiario.Ficha.Dpto == " " ? "" : beneficiario.Ficha.Dpto) ?? "") : ((apoderado.Dpto ?? "") != "" ? apoderado.Dpto : ConfigurationManager.AppSettings["ApoderadoDpto"])).PadRight(3, '0').Substring(0, 3);

                        if (beneficiario != null)
                        {
                            if (beneficiario.Ficha.Barrio.Length > 30)
                            {
                                beneficiario.Ficha.Barrio = ConfigurationManager.AppSettings["BarrioFueraRango"];
                            }
                        }

                        if (apoderado != null)
                        {
                            if ((apoderado.Barrio ?? "").Length > 30 || (apoderado.Barrio ?? "") == "0")
                            {
                                apoderado.Barrio = ConfigurationManager.AppSettings["BarrioFueraRango"];
                            }
                        }

                        var barrio =
                            (apoderado == null
                                 ? RemoverSignosAcentosBis(RemoverCaracter(beneficiario.Ficha.Barrio ?? ""))
                                 : (RemoverSignosAcentosBis(RemoverCaracter(((apoderado.Barrio ?? "")))) != ""
                                        ? RemoverSignosAcentosBis(RemoverCaracter((apoderado.Barrio)))
                                        : ConfigurationManager.AppSettings["ApoderadoBarrio"])).
                                PadRight(
                                    30, ' ').Substring(0, 30);

                        var localidad =
                            (apoderado == null
                                 ? (RemoverSignosAcentosBis(beneficiario.Ficha.Localidad) ?? "")
                                 : (RemoverSignosAcentosBis(RemoverCaracter((apoderado.Localidad ?? ""))) != ""
                                        ? RemoverSignosAcentosBis(RemoverCaracter(apoderado.Localidad))
                                        : ConfigurationManager.AppSettings["ApoderadoLocalidad"])).PadRight(30, ' ').
                                Substring(
                                    0, 30);

                        const string codigoprovincia = "004";

                        var codigopostal =
                            (apoderado == null ? RemoverCaracter(beneficiario.Ficha.CodigoPostal ?? "") : ((apoderado.CodigoPostal ?? "") != "" ? apoderado.CodigoPostal : ConfigurationManager.AppSettings["ApoderadoCodigoPostal"])).PadLeft(5, '0').
                                Substring(0, 5);

                        var codigopostalextendido = String.Empty.PadRight(8, '0');
                        var preftelparticular = String.Empty.PadRight(5, '0');

                        var telefonoparticular = (apoderado == null ? RemoverCaracter((beneficiario.Ficha.TelefonoFijo ?? ""))
                            : (RemoverCaracter(apoderado.TelefonoFijo ?? "") != "" ? RemoverCaracter(apoderado.TelefonoFijo)
                            : ConfigurationManager.AppSettings["ApoderadoTelefono"])).Replace(@" ", string.Empty).PadLeft(11, '0').Substring(0, 11);


                        var preftelcelularmovil = String.Empty.PadRight(5, '0');

                        var telefonomovil =
                            (apoderado == null ? RemoverCaracter(beneficiario.Ficha.TelefonoCelular ?? "") : "").Replace(@" ", string.Empty).PadLeft(11, '0').Substring(0, 11);



                        var domiciliocomercial =
                            (apoderado == null
                                 ? RemoverSignosAcentosBis(RemoverCaracter((beneficiario.Ficha.Calle ?? "")))
                                 : (RemoverSignosAcentosBis(RemoverCaracter((apoderado.Calle ?? ""))) != ""
                                        ? RemoverSignosAcentosBis(RemoverCaracter(apoderado.Calle))
                                        : ConfigurationManager.AppSettings["ApoderadoCalle"])).PadRight(
                                            30, ' ').Substring(0, 30);


                        var nrodomiciliocomercial =
                            (apoderado == null ? RemoverCaracter((beneficiario.Ficha.Numero == " " ? "" : beneficiario.Ficha.Numero) ?? "") : ((apoderado.Numero ?? "") != "" ? apoderado.Numero : ConfigurationManager.AppSettings["ApoderadoNro"])).PadLeft(5, '0').Substring(0, 5);

                        var pisocomercial =
                            (apoderado == null ? RemoverCaracter((beneficiario.Ficha.Piso == " " ? "" : beneficiario.Ficha.Piso) ?? "") : ((apoderado.Piso ?? "") != "" ? apoderado.Piso : ConfigurationManager.AppSettings["ApoderadoPiso"])).PadLeft(2, '0').Substring(0, 2);

                        var departamentocomercial =
                            (apoderado == null ? RemoverCaracter((beneficiario.Ficha.Dpto == " " ? "" : beneficiario.Ficha.Dpto) ?? "") : ((apoderado.Dpto ?? "") != "" ? apoderado.Dpto : ConfigurationManager.AppSettings["ApoderadoDpto"])).PadRight(3, ' ').Substring(0, 3);

                        var barriocomercial =
                            (apoderado == null
                                 ? RemoverSignosAcentosBis(RemoverCaracter((beneficiario.Ficha.Barrio ?? "")))
                                 : (RemoverSignosAcentosBis(RemoverCaracter(apoderado.Barrio ?? "")) != ""
                                        ? RemoverSignosAcentosBis(RemoverCaracter(apoderado.Barrio))
                                        : ConfigurationManager.AppSettings["ApoderadoBarrio"])).PadRight(
                                            30, ' ').Substring(0, 30);

                        var localidadcomercial =
                            (apoderado == null
                                 ? RemoverSignosAcentosBis(RemoverCaracter(beneficiario.Ficha.Localidad ?? ""))
                                 : (RemoverSignosAcentosBis(RemoverCaracter((apoderado.Localidad ?? ""))) != ""
                                        ? RemoverSignosAcentosBis(RemoverCaracter(apoderado.Localidad))
                                        : ConfigurationManager.AppSettings["ApoderadoLocalidad"])).
                                PadRight(30, ' ').Substring(0, 30);

                        const string codprovcomercial = "004";

                        var codpostalcomercial =
                            (apoderado == null ? RemoverCaracter(beneficiario.Ficha.CodigoPostal ?? "") : ((apoderado.CodigoPostal ?? "") != "" ?
                            apoderado.CodigoPostal : ConfigurationManager.AppSettings["ApoderadoCodigoPostal"])).PadLeft(5, '0').
                                Substring(0, 5);

                        var codextendidopostalcomercial = String.Empty.PadRight(8, '0');
                        var preftelcomercial = String.Empty.PadRight(5, '0');

                        var telefonocomercial =
                            (apoderado == null ? RemoverCaracter(beneficiario.Ficha.TelefonoFijo ?? "") : ((apoderado.TelefonoFijo ?? "") != "" ?
                            apoderado.TelefonoFijo : ConfigurationManager.AppSettings["ApoderadoTelefono"]))
                            .Replace(@" ", string.Empty).PadLeft(11, '0').Substring(0, 11);

                        var fechanacimiento = (apoderado == null
                                                   ? beneficiario.Ficha.FechaNacimiento ?? DateTime.Now
                                                   : apoderado.FechaNacimiento ?? DateTime.Now).ToString("yyyyMMdd").Replace("/", String.Empty).Trim().PadLeft(8, '0').Substring(0, 8);

                        if (fechanacimiento == "00010101")
                        {
                            fechanacimiento = "00000000";
                        }

                        const string estadocivil = "0001";
                        const string residente = "S";

                        var sexo = (apoderado == null
                                        ? beneficiario.Ficha.Sexo.ToUpper() == "VARON" ? "M" : "F"
                                        : apoderado.Sexo.ToUpper() == "VARON" ? "M" : "F");

                        var nacionalidad =
                            (apoderado == null
                                 ? RemoverSignosAcentosBis(beneficiario.Nacionalidad.ToString() == "0"
                                                            ? "1"
                                                            : beneficiario.Nacionalidad.ToString())
                                 : ConfigurationManager.AppSettings["ApoderadoNacionalidad"]).
                                PadLeft(3, '0').Substring(0, 3);

                        //var email = (apoderado == null ? (beneficiario.Email ?? "") : "").PadRight(30, ' ').Substring(
                        //    0, 30);

                        //var email = String.Empty.PadRight(30, ' ');
                        var email = (apoderado == null ? (beneficiario.Ficha.Mail ?? "AA@BB.COM") : (apoderado.Mail ?? "AA@BB.COM")).PadRight(30, ' ').Substring(0, 30);
                        const string tipopersona = "F";

                        const string codactbancaria = "00002";

                        const string codnatjuridica = "027";

                        var primerapellidoconyugue = ("" ?? "").PadRight(15, ' ').Substring(0, 15);
                        var segundoapellidoconyugue = String.Empty.PadRight(15, ' ');
                        var primernombreconyugue = ("" ?? "").PadRight(15, ' ').Substring(0, 15);
                        var segundonombreconyugue = String.Empty.PadRight(15, ' ');
                        var sexoconyugue = ("" ?? "").PadRight(1, ' ').Substring(0, 1);
                        const string tipodocumentoconyugue = "000";
                        var nrodocumentoconyugue = ("" ?? "").PadLeft(11, '0').Substring(0, 11);
                        var cuitconyugue = ("" ?? "").PadLeft(11, '0').Substring(0, 11);
                        var fechanacimientoconyugue = "00010101";// (beneficiario.Conyugue.Fecha_Nacimiento).ToString("yyyyMMdd").Replace("/", String.Empty).Trim().PadLeft(8, '0').Substring(0, 8);

                        if (fechanacimientoconyugue == "00010101")
                        {
                            fechanacimientoconyugue = "00000000";
                        }

                        const string nacionalidadconyugue = "000";//RemoverSignosAcentos(beneficiario.Conyugue.Nacionalidad.ToString()).PadLeft(3, '0').Substring(0, 3);
                        string nroadherente = convenio;

                        const string tipoconvenio = "000";
                        var validanombre = String.Empty.PadRight(1, '1');
                        var nombreclientesegunpadron = String.Empty.PadRight(30, ' ');
                        var filler = String.Empty.PadRight(409, ' ');
                        const string tiposolicitud = "00";
                    // 06/10/2014 - Di Campli Leandro - Nuevos campos para la solicitud de cuentas
                        /*
                            NCBU_Q66	NRO. CBU	CHARACTER	22
                            CUIE_Q66	NRO. DE CUIT E	NUMERIC	11
                            TICE_Q66	TIPO DE CUENTA	NUMERIC	2
                            TDEB_Q66	EMITE TARJETA	CHARACTER	1

                         */
                        var NCBU_Q66 = String.Empty.PadRight(22, ' ');
                        var CUIE_Q66 = "30715119567";
                        var TICE_Q66 = "63";
                        var TDEB_Q66 = "1";
                    
                        var filler2 = String.Empty.PadRight(/*352*/372, ' ');
                        //var datosparaempresa = String.Empty.PadRight(21, ' ');
                        //var motivodelerror1 = String.Empty.PadRight(5, ' ');
                        //var motivodelerror2 = String.Empty.PadRight(5, ' ');
                        //var motivodelerror3 = String.Empty.PadRight(5, ' ');
                        //var motivodelerror4 = String.Empty.PadRight(5, ' ');
                        //var motivodelerror5 = String.Empty.PadRight(5, ' ');
                        //var motivodelerror6 = String.Empty.PadRight(5, ' ');
                        //var motivodelerror7 = String.Empty.PadRight(5, ' ');

                        var text = tiporegistro.ToUpper() + sucursal.ToUpper() + moneda.ToUpper() + tipodocumento.ToUpper() + nrodocumento.ToUpper()
                            + clavefiscal.ToUpper() + nroclavefiscal.ToUpper() + tipocuenta.ToUpper() + nrocuenta.ToUpper() + fechaalta.ToUpper()
                            + primerapellido.ToUpper() + segundoapellido.ToUpper() + primernombre.ToUpper() + segundonombre.ToUpper() + condicioniva.ToUpper()
                            + domicilioparticular.ToUpper() + nrodomicilio.ToUpper() + piso.ToUpper() + departamento.ToUpper() + barrio.ToUpper()
                            + localidad.ToUpper() + codigoprovincia.ToUpper() + codigopostal.ToUpper() + codigopostalextendido.ToUpper() + preftelparticular.ToUpper()
                            + telefonoparticular.ToUpper() + preftelcelularmovil.ToUpper() + telefonomovil.ToUpper() + domiciliocomercial.ToUpper()
                            + nrodomiciliocomercial.ToUpper() + pisocomercial.ToUpper() + departamentocomercial.ToUpper() + barriocomercial.ToUpper()
                            + localidadcomercial.ToUpper() + codprovcomercial.ToUpper() + codpostalcomercial.ToUpper() + codextendidopostalcomercial.ToUpper()
                            + preftelcomercial.ToUpper() + telefonocomercial.ToUpper() + fechanacimiento.ToUpper() + estadocivil.ToUpper() + residente
                            + sexo.ToUpper() + nacionalidad.ToUpper() + email.ToUpper() + tipopersona.ToUpper() + codactbancaria.ToUpper() + codnatjuridica.ToUpper()
                            + primerapellidoconyugue.ToUpper() + segundoapellidoconyugue.ToUpper() + primernombreconyugue.ToUpper() + segundonombreconyugue.ToUpper()
                            + sexoconyugue.ToUpper() + tipodocumentoconyugue.ToUpper() + nrodocumentoconyugue.ToUpper() + cuitconyugue.ToUpper()
                            + fechanacimientoconyugue.ToUpper() + nacionalidadconyugue.ToUpper() + nroadherente.ToUpper() + tipoconvenio.ToUpper()
                            + validanombre.ToUpper() + nombreclientesegunpadron.ToUpper() + filler.ToUpper() + tiposolicitud.ToUpper() + NCBU_Q66.ToUpper() + CUIE_Q66.ToUpper() + TICE_Q66.ToUpper() + TDEB_Q66.ToUpper() + filler2.ToUpper()
                            ;//+ datosparaempresa.ToUpper() + motivodelerror1.ToUpper() + motivodelerror2.ToUpper() + motivodelerror3.ToUpper() + motivodelerror4.ToUpper()
                            //+ motivodelerror5.ToUpper() + motivodelerror6.ToUpper() + motivodelerror7.ToUpper();

                        //Byte [] info = new UTF8Encoding(true).GetBytes(text);
                        //fs.Write(info, 0, info.Length);
                        //sw.Write(info, 0, info.Length);

                        if (Notwrite == 0)
                        {
                            sw.WriteLine(text);
                        }

                    }
                sw.Close();
                fs.Close();
                }
            }

            return nomfile;//"cli" + listabeneficiario[0].NumeroConvenio.PadLeft(5, '0') + "_" + Convert.ToString(DateTime.Now.Day) + Convert.ToString(DateTime.Now.Month) + Convert.ToString(DateTime.Now.Year) + Convert.ToString(DateTime.Now.Hour) + Convert.ToString(DateTime.Now.Minute) + Convert.ToString(DateTime.Now.Second) + ".hab";

        }

        /// <summary>
        /// 01/07/2013 - DI CAMPLI LEANDRO - Permite generar el archivo sin actualizar la fecha de solicitud de cuenta cuando se descarga el archivo en "Archivos Generados"
        /// </summary>
        /// <param name="lista"></param>
        /// <returns></returns>
        public string VolverGenerarFile(IList<IBeneficiario> lista)
        {

            var listabeneficiario = lista;
            var fechasolicitud = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"));

            if (listabeneficiario.Count <= 0)
            {
                return "";
            }

            string nomfile = string.Empty;
            nomfile = "cli" + listabeneficiario[0].NumeroConvenio.PadLeft(5, '0') + "_" + Convert.ToString(DateTime.Now.Day) + Convert.ToString(DateTime.Now.Month) + Convert.ToString(DateTime.Now.Year) + Convert.ToString(DateTime.Now.Hour) + Convert.ToString(DateTime.Now.Minute) + Convert.ToString(DateTime.Now.Second) +
                                                   ".hab";

            var file =
                HttpContext.Current.Server.MapPath("/Archivos/" + nomfile);

            var fiinfo = new FileInfo(file);

            if (fiinfo.Exists)
            {

            }
            else
            {
                StreamWriter sw1 = new StreamWriter(file);
                sw1.Close();

            }
            FileStream fs = new FileStream(file, FileMode.Truncate, FileAccess.ReadWrite);

            //using (fiinfo.CreateText())
            //{
            //}

            if (fs.CanWrite/*fiinfo.Exists*/)
            {

                using (StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default))
                {

                    foreach (var item in listabeneficiario)
                    {
                        IBeneficiario beneficiario = item;
                        //using (StreamWriter sw =/* new StreamWriter(fs, System.Text.Encoding.UTF8) */fiinfo.AppendText())
                        //{

                        string convenio = (beneficiario.NumeroConvenio).PadLeft(5, '0').Substring(0, 5);

                        if (beneficiario.Conyugue == null)
                        {
                            beneficiario.Conyugue = new Conyuge();
                        }


                        IApoderado apoderado = null;
                        if (beneficiario.TieneApoderado == "S")// 01/07/2013 - DI CAMPLI LEANDRO - SE COMENTAN LOS UPDATE
                        {
                            apoderado = _apoderadorepositorio.GetApoderadoByBeneficiarioActivo(beneficiario.IdBeneficiario);
                            beneficiario = null;
                            //_apoderadorepositorio.UpdateFechaSolicitud(apoderado.IdApoderado, fechasolicitud);
                        }
                        else
                        {
                            //_cuentabancorepositorio.UpdateFechaSolicitud(beneficiario.IdBeneficiario, fechasolicitud);
                        }


                        const string tiporegistro = "A";
                        var sucursal =
                            (apoderado == null ? beneficiario.CodigoSucursal : apoderado.CodigoSucursalBanco).PadLeft(5, '0').
                                Substring(0, 5);

                        const string moneda = "01";

                        var tipodocumento =
                            (apoderado == null
                                 ? TipoDniBco(beneficiario.Ficha.TipoDocumento ?? 0)
                                 : TipoDniBco(Convert.ToInt32((apoderado.TipoDocumento ?? 1)))).PadLeft(3, '0')
                                       .Substring(0, 3);

                        var nrodocumento =
                            (apoderado == null ? beneficiario.Ficha.NumeroDocumento ?? "" : apoderado.NumeroDocumento)
                                .PadLeft(11, '0').Substring(0, 11);

                        const string clavefiscal = "007";

                        var nroclavefiscal =
                            (apoderado == null ? beneficiario.Ficha.Cuil ?? "" : apoderado.Cuil ?? "").Replace("-",
                                                                                                         String.Empty).
                                PadLeft(11, '0').Substring(0, 11);

                        const string tipocuenta = "00";
                        const string nrocuenta = "000000000";
                        var fechaalta = DateTime.Now.ToString("yyyyMMdd").Replace("/", String.Empty).Trim().PadLeft(8, '0').Substring(0, 8);

                        if (fechaalta == "00010101")
                        {
                            fechaalta = "00000000";
                        }

                        var nombre = (apoderado == null ? beneficiario.Ficha.Nombre ?? "" : apoderado.Nombre).Split(' ');
                        var mSegundoNombre = "";
                        var mPrimerNombre = "";

                        if (nombre.Length > 1)
                        {
                            for (var i = 0; i < nombre.Length; i++)
                            {


                                if (nombre[i].Length <= 3 && mSegundoNombre == "")
                                {
                                    if (mPrimerNombre == "")
                                    {
                                        mPrimerNombre = nombre[i].Trim();
                                    }
                                    else
                                    {
                                        if (nombre[i - 1].Length <= 3 && mSegundoNombre == "")
                                        {
                                            mPrimerNombre = mPrimerNombre + " " + nombre[i].Trim();
                                        }
                                        else
                                        {
                                            if (mSegundoNombre == "")
                                            {
                                                mSegundoNombre = nombre[i].Trim();
                                            }
                                            else
                                            {
                                                mSegundoNombre = mSegundoNombre + " " + nombre[i].Trim();
                                            }
                                        }
                                    }


                                }
                                else
                                {
                                    if (mPrimerNombre == "")
                                    {
                                        mPrimerNombre = nombre[i].Trim();
                                    }
                                    else
                                    {
                                        if (nombre[i - 1].Length <= 3 && mSegundoNombre == "")
                                        {
                                            mPrimerNombre = mPrimerNombre + " " + nombre[i].Trim();
                                        }
                                        else
                                        {
                                            if (mSegundoNombre == "")
                                            {
                                                mSegundoNombre = nombre[i].Trim();
                                            }
                                            else
                                            {
                                                mSegundoNombre = mSegundoNombre + " " + nombre[i].Trim();
                                            }
                                        }
                                    }


                                }


                            }
                        }
                        else
                        {
                            mPrimerNombre = nombre[0].Trim();
                        }

                        var apellido =
                            (apoderado == null ? beneficiario.Ficha.Apellido ?? "" : apoderado.Apellido).Split(' ');

                        var mSegundoApellido = "";
                        var mPrimerApellido = "";

                        if (apellido.Length > 1)
                        {
                            for (int i = 0; i < apellido.Length; i++)
                            {



                                if (apellido[i].Length <= 3 && mSegundoApellido == "")
                                {
                                    if (mPrimerApellido == "")
                                    {
                                        mPrimerApellido = apellido[i].Trim();
                                    }
                                    else
                                    {
                                        if (apellido[i - 1].Length <= 3 && mSegundoApellido == "")
                                        {
                                            mPrimerApellido = mPrimerApellido + " " + apellido[i].Trim();
                                        }
                                        else
                                        {
                                            if (mSegundoApellido == "")
                                            {
                                                mSegundoApellido = apellido[i].Trim();
                                            }
                                            else
                                            {
                                                mSegundoApellido = mSegundoApellido + " " + apellido[i].Trim();
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (mPrimerApellido == "")
                                    {
                                        mPrimerApellido = apellido[i].Trim();
                                    }
                                    else
                                    {
                                        if (apellido[i - 1].Length <= 3 && mSegundoApellido == "")
                                        {
                                            mPrimerApellido = mPrimerApellido + " " + apellido[i].Trim();
                                        }
                                        else
                                        {
                                            if (mSegundoApellido == "")
                                            {
                                                mSegundoApellido = apellido[i].Trim();
                                            }
                                            else
                                            {
                                                mSegundoApellido = mSegundoApellido + " " + apellido[i].Trim();
                                            }
                                        }
                                    }


                                }

                            }


                        }
                        else
                        {
                            mPrimerApellido = apellido[0].Trim();
                        }

                        var primerapellido = RemoverSignosAcentosBis((mPrimerApellido ?? "").Trim()).PadRight(15, ' ').Substring(0, 15);
                        var segundoapellido = RemoverSignosAcentosBis((mSegundoApellido ?? "").Trim()).PadRight(15, ' ').Substring(0, 15);
                        var primernombre = RemoverSignosAcentosBis((mPrimerNombre ?? "").Trim()).PadRight(15, ' ').Substring(0, 15);
                        var segundonombre = RemoverSignosAcentosBis((mSegundoNombre ?? "").Trim()).PadRight(15, ' ').Substring(0, 15);

                        const string condicioniva = "01";

                        if (beneficiario != null)
                        {
                            if ((beneficiario.Ficha.Calle ?? "").Length > 30 || (beneficiario.Ficha.Calle ?? "") == "" || (beneficiario.Ficha.Numero ?? "") == "0" || (beneficiario.Ficha.Numero ?? "").Length == 1)
                            {
                                beneficiario.Ficha.Calle = ConfigurationManager.AppSettings["CalleFueraRango"];
                                beneficiario.Ficha.Numero = ConfigurationManager.AppSettings["NroFueraRango"];
                                beneficiario.Ficha.Barrio = ConfigurationManager.AppSettings["BarrioFueraRango"];
                            }
                        }

                        if (apoderado != null)
                        {
                            if ((apoderado.Calle ?? "").Length > 30 || (apoderado.Calle ?? "") == "" || (apoderado.Numero ?? "") == "0" || (apoderado.Calle ?? "").Length == 1)
                            {
                                apoderado.Calle = ConfigurationManager.AppSettings["CalleFueraRango"];
                                apoderado.Numero = ConfigurationManager.AppSettings["NroFueraRango"];
                                apoderado.Barrio = ConfigurationManager.AppSettings["BarrioFueraRango"];
                            }
                        }
                        var domicilioparticular =
                            (apoderado == null
                                 ? RemoverSignosAcentosBis(RemoverCaracter((beneficiario.Ficha.Calle ?? "")))
                                 : (RemoverSignosAcentosBis(RemoverCaracter(((apoderado.Calle ?? "")))) != ""
                                        ? RemoverSignosAcentosBis(RemoverCaracter((apoderado.Calle)))
                                        : ConfigurationManager.AppSettings["ApoderadoCalle"])).PadRight(
                                            30, ' ').Substring(0, 30);


                        if (beneficiario != null)
                        {
                            int numero;

                            int.TryParse(beneficiario.Ficha.Numero, out numero);

                            beneficiario.Ficha.Numero = numero.ToString();
                        }

                        var nrodomicilio =
                            (apoderado == null ? RemoverCaracter(beneficiario.Ficha.Numero ?? "") : ((apoderado.Numero ?? "") != "" ? apoderado.Numero : ConfigurationManager.AppSettings["ApoderadoNro"])).PadLeft(5, '0').Substring(0, 5);


                        var piso =
                            (apoderado == null ? RemoverCaracter((beneficiario.Ficha.Piso == " " ? "" : beneficiario.Ficha.Piso) ?? "") : ((apoderado.Piso ?? "") != "" ? apoderado.Piso : ConfigurationManager.AppSettings["ApoderadoPiso"])).PadLeft(2, '0').Substring(0, 2);

                        var departamento =
                            (apoderado == null ? RemoverCaracter((beneficiario.Ficha.Dpto == " " ? "" : beneficiario.Ficha.Dpto) ?? "") : ((apoderado.Dpto ?? "") != "" ? apoderado.Dpto : ConfigurationManager.AppSettings["ApoderadoDpto"])).PadRight(3, '0').Substring(0, 3);

                        if (beneficiario != null)
                        {
                            if (beneficiario.Ficha.Barrio.Length > 30)
                            {
                                beneficiario.Ficha.Barrio = ConfigurationManager.AppSettings["BarrioFueraRango"];
                            }
                        }

                        if (apoderado != null)
                        {
                            if ((apoderado.Barrio ?? "").Length > 30 || (apoderado.Barrio ?? "") == "0")
                            {
                                apoderado.Barrio = ConfigurationManager.AppSettings["BarrioFueraRango"];
                            }
                        }

                        var barrio =
                            (apoderado == null
                                 ? RemoverSignosAcentosBis(RemoverCaracter(beneficiario.Ficha.Barrio ?? ""))
                                 : (RemoverSignosAcentosBis(RemoverCaracter(((apoderado.Barrio ?? "")))) != ""
                                        ? RemoverSignosAcentosBis(RemoverCaracter((apoderado.Barrio)))
                                        : ConfigurationManager.AppSettings["ApoderadoBarrio"])).
                                PadRight(
                                    30, ' ').Substring(0, 30);

                        var localidad =
                            (apoderado == null
                                 ? (RemoverSignosAcentosBis(beneficiario.Ficha.Localidad) ?? "")
                                 : (RemoverSignosAcentosBis(RemoverCaracter((apoderado.Localidad ?? ""))) != ""
                                        ? RemoverSignosAcentosBis(RemoverCaracter(apoderado.Localidad))
                                        : ConfigurationManager.AppSettings["ApoderadoLocalidad"])).PadRight(30, ' ').
                                Substring(
                                    0, 30);

                        const string codigoprovincia = "004";

                        var codigopostal =
                            (apoderado == null ? RemoverCaracter(beneficiario.Ficha.CodigoPostal ?? "") : ((apoderado.CodigoPostal ?? "") != "" ? apoderado.CodigoPostal : ConfigurationManager.AppSettings["ApoderadoCodigoPostal"])).PadLeft(5, '0').
                                Substring(0, 5);

                        var codigopostalextendido = String.Empty.PadRight(8, '0');
                        var preftelparticular = String.Empty.PadRight(5, '0');

                        var telefonoparticular = (apoderado == null ? RemoverCaracter((beneficiario.Ficha.TelefonoFijo ?? ""))
                            : (RemoverCaracter(apoderado.TelefonoFijo ?? "") != "" ? RemoverCaracter(apoderado.TelefonoFijo)
                            : ConfigurationManager.AppSettings["ApoderadoTelefono"])).Replace(@" ", string.Empty).PadLeft(11, '0').Substring(0, 11);


                        var preftelcelularmovil = String.Empty.PadRight(5, '0');

                        var telefonomovil =
                            (apoderado == null ? RemoverCaracter(beneficiario.Ficha.TelefonoCelular ?? "") : "").Replace(@" ", string.Empty).PadLeft(11, '0').Substring(0, 11);



                        var domiciliocomercial =
                            (apoderado == null
                                 ? RemoverSignosAcentosBis(RemoverCaracter((beneficiario.Ficha.Calle ?? "")))
                                 : (RemoverSignosAcentosBis(RemoverCaracter((apoderado.Calle ?? ""))) != ""
                                        ? RemoverSignosAcentosBis(RemoverCaracter(apoderado.Calle))
                                        : ConfigurationManager.AppSettings["ApoderadoCalle"])).PadRight(
                                            30, ' ').Substring(0, 30);


                        var nrodomiciliocomercial =
                            (apoderado == null ? RemoverCaracter((beneficiario.Ficha.Numero == " " ? "" : beneficiario.Ficha.Numero) ?? "") : ((apoderado.Numero ?? "") != "" ? apoderado.Numero : ConfigurationManager.AppSettings["ApoderadoNro"])).PadLeft(5, '0').Substring(0, 5);

                        var pisocomercial =
                            (apoderado == null ? RemoverCaracter((beneficiario.Ficha.Piso == " " ? "" : beneficiario.Ficha.Piso) ?? "") : ((apoderado.Piso ?? "") != "" ? apoderado.Piso : ConfigurationManager.AppSettings["ApoderadoPiso"])).PadLeft(2, '0').Substring(0, 2);

                        var departamentocomercial =
                            (apoderado == null ? RemoverCaracter((beneficiario.Ficha.Dpto == " " ? "" : beneficiario.Ficha.Dpto) ?? "") : ((apoderado.Dpto ?? "") != "" ? apoderado.Dpto : ConfigurationManager.AppSettings["ApoderadoDpto"])).PadRight(3, ' ').Substring(0, 3);

                        var barriocomercial =
                            (apoderado == null
                                 ? RemoverSignosAcentosBis(RemoverCaracter((beneficiario.Ficha.Barrio ?? "")))
                                 : (RemoverSignosAcentosBis(RemoverCaracter(apoderado.Barrio ?? "")) != ""
                                        ? RemoverSignosAcentosBis(RemoverCaracter(apoderado.Barrio))
                                        : ConfigurationManager.AppSettings["ApoderadoBarrio"])).PadRight(
                                            30, ' ').Substring(0, 30);

                        var localidadcomercial =
                            (apoderado == null
                                 ? RemoverSignosAcentosBis(RemoverCaracter(beneficiario.Ficha.Localidad ?? ""))
                                 : (RemoverSignosAcentosBis(RemoverCaracter((apoderado.Localidad ?? ""))) != ""
                                        ? RemoverSignosAcentosBis(RemoverCaracter(apoderado.Localidad))
                                        : ConfigurationManager.AppSettings["ApoderadoLocalidad"])).
                                PadRight(30, ' ').Substring(0, 30);

                        const string codprovcomercial = "004";

                        var codpostalcomercial =
                            (apoderado == null ? RemoverCaracter(beneficiario.Ficha.CodigoPostal ?? "") : ((apoderado.CodigoPostal ?? "") != "" ?
                            apoderado.CodigoPostal : ConfigurationManager.AppSettings["ApoderadoCodigoPostal"])).PadLeft(5, '0').
                                Substring(0, 5);

                        var codextendidopostalcomercial = String.Empty.PadRight(8, '0');
                        var preftelcomercial = String.Empty.PadRight(5, '0');

                        var telefonocomercial =
                            (apoderado == null ? RemoverCaracter(beneficiario.Ficha.TelefonoFijo ?? "") : ((apoderado.TelefonoFijo ?? "") != "" ?
                            apoderado.TelefonoFijo : ConfigurationManager.AppSettings["ApoderadoTelefono"]))
                            .Replace(@" ", string.Empty).PadLeft(11, '0').Substring(0, 11);

                        var fechanacimiento = (apoderado == null
                                                   ? beneficiario.Ficha.FechaNacimiento ?? DateTime.Now
                                                   : apoderado.FechaNacimiento ?? DateTime.Now).ToString("yyyyMMdd").Replace("/", String.Empty).Trim().PadLeft(8, '0').Substring(0, 8);

                        if (fechanacimiento == "00010101")
                        {
                            fechanacimiento = "00000000";
                        }

                        const string estadocivil = "0001";
                        const string residente = "S";

                        var sexo = (apoderado == null
                                        ? beneficiario.Ficha.Sexo.ToUpper() == "VARON" ? "M" : "F"
                                        : apoderado.Sexo.ToUpper() == "VARON" ? "M" : "F");

                        var nacionalidad =
                            (apoderado == null
                                 ? RemoverSignosAcentosBis(beneficiario.Nacionalidad.ToString() == "0"
                                                            ? "1"
                                                            : beneficiario.Nacionalidad.ToString())
                                 : ConfigurationManager.AppSettings["ApoderadoNacionalidad"]).
                                PadLeft(3, '0').Substring(0, 3);

                        //var email = (apoderado == null ? (beneficiario.Email ?? "") : "").PadRight(30, ' ').Substring(
                        //    0, 30);

                        var email = String.Empty.PadRight(30, ' ');

                        const string tipopersona = "F";

                        const string codactbancaria = "00002";

                        const string codnatjuridica = "027";

                        var primerapellidoconyugue = ("" ?? "").PadRight(15, ' ').Substring(0, 15);
                        var segundoapellidoconyugue = String.Empty.PadRight(15, ' ');
                        var primernombreconyugue = ("" ?? "").PadRight(15, ' ').Substring(0, 15);
                        var segundonombreconyugue = String.Empty.PadRight(15, ' ');
                        var sexoconyugue = ("" ?? "").PadRight(1, ' ').Substring(0, 1);
                        const string tipodocumentoconyugue = "000";
                        var nrodocumentoconyugue = ("" ?? "").PadLeft(11, '0').Substring(0, 11);
                        var cuitconyugue = ("" ?? "").PadLeft(11, '0').Substring(0, 11);
                        var fechanacimientoconyugue = "00010101";// (beneficiario.Conyugue.Fecha_Nacimiento).ToString("yyyyMMdd").Replace("/", String.Empty).Trim().PadLeft(8, '0').Substring(0, 8);

                        if (fechanacimientoconyugue == "00010101")
                        {
                            fechanacimientoconyugue = "00000000";
                        }

                        const string nacionalidadconyugue = "000";//RemoverSignosAcentos(beneficiario.Conyugue.Nacionalidad.ToString()).PadLeft(3, '0').Substring(0, 3);
                        string nroadherente = convenio;

                        const string tipoconvenio = "000";
                        var validanombre = String.Empty.PadRight(1, '1');
                        var nombreclientesegunpadron = String.Empty.PadRight(30, ' ');
                        var filler = String.Empty.PadRight(409, ' ');
                        const string tiposolicitud = "00";
                        var filler2 = String.Empty.PadRight(352, ' ');
                        var datosparaempresa = String.Empty.PadRight(21, ' ');
                        var motivodelerror1 = String.Empty.PadRight(5, ' ');
                        var motivodelerror2 = String.Empty.PadRight(5, ' ');
                        var motivodelerror3 = String.Empty.PadRight(5, ' ');
                        var motivodelerror4 = String.Empty.PadRight(5, ' ');
                        var motivodelerror5 = String.Empty.PadRight(5, ' ');
                        var motivodelerror6 = String.Empty.PadRight(5, ' ');
                        var motivodelerror7 = String.Empty.PadRight(5, ' ');

                        var text = tiporegistro.ToUpper() + sucursal.ToUpper() + moneda.ToUpper() + tipodocumento.ToUpper() + nrodocumento.ToUpper()
                            + clavefiscal.ToUpper() + nroclavefiscal.ToUpper() + tipocuenta.ToUpper() + nrocuenta.ToUpper() + fechaalta.ToUpper()
                            + primerapellido.ToUpper() + segundoapellido.ToUpper() + primernombre.ToUpper() + segundonombre.ToUpper() + condicioniva.ToUpper()
                            + domicilioparticular.ToUpper() + nrodomicilio.ToUpper() + piso.ToUpper() + departamento.ToUpper() + barrio.ToUpper()
                            + localidad.ToUpper() + codigoprovincia.ToUpper() + codigopostal.ToUpper() + codigopostalextendido.ToUpper() + preftelparticular.ToUpper()
                            + telefonoparticular.ToUpper() + preftelcelularmovil.ToUpper() + telefonomovil.ToUpper() + domiciliocomercial.ToUpper()
                            + nrodomiciliocomercial.ToUpper() + pisocomercial.ToUpper() + departamentocomercial.ToUpper() + barriocomercial.ToUpper()
                            + localidadcomercial.ToUpper() + codprovcomercial.ToUpper() + codpostalcomercial.ToUpper() + codextendidopostalcomercial.ToUpper()
                            + preftelcomercial.ToUpper() + telefonocomercial.ToUpper() + fechanacimiento.ToUpper() + estadocivil.ToUpper() + residente
                            + sexo.ToUpper() + nacionalidad.ToUpper() + email.ToUpper() + tipopersona.ToUpper() + codactbancaria.ToUpper() + codnatjuridica.ToUpper()
                            + primerapellidoconyugue.ToUpper() + segundoapellidoconyugue.ToUpper() + primernombreconyugue.ToUpper() + segundonombreconyugue.ToUpper()
                            + sexoconyugue.ToUpper() + tipodocumentoconyugue.ToUpper() + nrodocumentoconyugue.ToUpper() + cuitconyugue.ToUpper()
                            + fechanacimientoconyugue.ToUpper() + nacionalidadconyugue.ToUpper() + nroadherente.ToUpper() + tipoconvenio.ToUpper()
                            + validanombre.ToUpper() + nombreclientesegunpadron.ToUpper() + filler.ToUpper() + tiposolicitud.ToUpper() + filler2.ToUpper()
                            + datosparaempresa.ToUpper() + motivodelerror1.ToUpper() + motivodelerror2.ToUpper() + motivodelerror3.ToUpper() + motivodelerror4.ToUpper()
                            + motivodelerror5.ToUpper() + motivodelerror6.ToUpper() + motivodelerror7.ToUpper();

                        //Byte [] info = new UTF8Encoding(true).GetBytes(text);
                        //fs.Write(info, 0, info.Length);
                        //sw.Write(info, 0, info.Length);
                        sw.WriteLine(text);

                    }
                    sw.Close();
                    fs.Close();
                }
            }

            return nomfile;//"cli" + listabeneficiario[0].NumeroConvenio.PadLeft(5, '0') + "_" + Convert.ToString(DateTime.Now.Day) + Convert.ToString(DateTime.Now.Month) + Convert.ToString(DateTime.Now.Year) + Convert.ToString(DateTime.Now.Hour) + Convert.ToString(DateTime.Now.Minute) + Convert.ToString(DateTime.Now.Second) + ".hab";

        }

        private static string TipoDniBco(int tipo)
        {
            string mReturn;

            switch (tipo)
            {
                case 0:
                    mReturn = "1";
                    break;
                case 1:
                    mReturn = "2";
                    break;
                case 2:
                    mReturn = "3";
                    break;
                case 3:
                    mReturn = "4";
                    break;
                default:
                    mReturn = "1";
                    break;

            }
            return mReturn;
        }

        private static void CargarEstadosBeneficiario(IBeneficiarioVista vista, IEnumerable<IEstadoBeneficiario> listaEstado)
        {
            foreach (var lEstado in listaEstado)
            {

                vista.Estados.Combo.Add(new ComboItem { Id = lEstado.Id_Estado_Beneficiario, Description = lEstado.Estado_Beneficiario });
            }
        }

        private static void CargarEstadosBeneficiarioforIndex(IBeneficiariosVista vista, IEnumerable<IEstadoBeneficiario> listaEstado)
        {
            vista.EstadosBeneficiarios.Combo.Add(new ComboItem
            {
                Id = 0,
                Description = "TODOS"

            });
            foreach (var lEstado in listaEstado)
            {

                vista.EstadosBeneficiarios.Combo.Add(new ComboItem { Id = lEstado.Id_Estado_Beneficiario, Description = lEstado.Estado_Beneficiario });
            }
        }

        private static void CargarSucursales(IBeneficiarioVista vista, IEnumerable<ISucursal> listaSucursales)
        {
            vista.Sucursales.Combo.Add(new ComboItem
            {
                Id = 0,
                Description = "0 - Ninguna"

            });
            foreach (var lsucursal in listaSucursales)
            {
                vista.Sucursales.Combo.Add(new ComboItem { Id = lsucursal.IdSucursal, Description = lsucursal.CodigoBanco + " - " + lsucursal.Detalle });
            }
        }

        public IBeneficiarioVista GetBeneficiarioForError(IBeneficiarioVista vista)
        {

            var mreturn = vista;

            CargarEstadosBeneficiario(mreturn, _estadobeneficiariorepositorio.GetEstadosBeneficiario());

            mreturn.Estados.Selected = vista.IdEstado.ToString();

            return mreturn;

        }

        public IImportarCuentasVista ImportarCuentas(HttpPostedFileBase archivo)
        {
            IImportarCuentasVista vista = new ImportarCuentasVista();

            if (archivo != null && archivo.ContentLength > 0)
            {

                var fileName = Path.GetFileName(archivo.FileName);
                var nombre = DateTime.Now.Year + DateTime.Now.Month.ToString() + DateTime.Now.Day +
                             DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + ".xls";

                var path = Path.Combine(HttpContext.Current.Server.MapPath("~/Archivos/Importados"), nombre);
                archivo.SaveAs(path);

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

        private static string RemoverSignosAcentos(string texto)
        {
            const string conSignos = "áàäéèëíìïóòöúùuñÁÀÄÉÈËÍÌÏÓÒÖÚÙÜçÇÑñÂ";
            const string sinSignos = "aaaeeeiiiooouuunAAAEEEIIIOOOUUUcCNnA";
            var textoSinAcentos = string.Empty;

            foreach (var caracter in texto)
            {
                var indexConAcento = conSignos.IndexOf(caracter);
                if (indexConAcento > -1)
                    textoSinAcentos = textoSinAcentos + (sinSignos.Substring(indexConAcento, 1));
                else
                    textoSinAcentos = textoSinAcentos + (caracter);
            }

            return textoSinAcentos.ToUpper();
        }

        //07/02/2013 - SE CREA PARA INPEDIR QUE SE GENERE EL ARCHIVO PARA CUENTAS BANCARIAS CON LA LETRA Ñ - Di Campli Leandro
        //14/02/2013 - SE VUELVE A 'RemoverSignosAcentos' YA QUE EL ENCRIPTADOR DEL BANCO DE CORDOBA NO ADMITE Ñ NI CARACTERES ESPECIALES.
        private static string RemoverSignosAcentosBis(string texto)
        {
            const string conSignos = "áàäéèëíìïóòöúùuñÁÀÄÉÈËÍÌÏÓÒÖÚÙÜçÇÂ";
            const string sinSignos = "aaaeeeiiiooouuunAAAEEEIIIOOOUUUcCA";
            var textoSinAcentos = string.Empty;

            foreach (var caracter in texto)
            {
                var indexConAcento = conSignos.IndexOf(caracter);
                if (indexConAcento > -1)
                    textoSinAcentos = textoSinAcentos + (sinSignos.Substring(indexConAcento, 1));
                else
                    textoSinAcentos = textoSinAcentos + (caracter);
            }

            return textoSinAcentos.ToUpper();
        }



        private static string RemoverCaracter(string texto)
        {
            const string caracter = @"[]¥ªÂ¿?\/()-'^_¨´º.°*:""";
            var textoSinCaracteres = string.Empty;
            const string test = "'";

            foreach (var letra in texto)
            {
                var indexcaracter = caracter.IndexOf(letra);
                if (indexcaracter > -1)
                    textoSinCaracteres = textoSinCaracteres + ("");
                else
                    if (letra == Convert.ToChar(test))
                    {
                        textoSinCaracteres = textoSinCaracteres + (" ");
                    }
                    else
                    {
                        textoSinCaracteres = textoSinCaracteres + (letra);
                    }


            }

            return textoSinCaracteres.ToUpper();
        }

        public IList<ICuentaImportar> CargarCuentas(string archivo)
        {
            IList<ICuentaImportar> listaresultado = new List<ICuentaImportar>();

            string path = HttpContext.Current.Server.MapPath(@"\Archivos\Importados") + @"\" + archivo;

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
                        var importado = new CuentaImportar
                                            {
                                                NumeroDocumento = row["NDOC_Q66"].ToString().Trim(),
                                                Sucursal = row["SUCU_Q66"].ToString().Trim(),
                                                Nombre = row["PRNO_Q66"].ToString(),
                                                SegundoNombre = row["SGNO_Q66"].ToString(),
                                                Apellido = row["APEL_Q66"].ToString(),
                                                SegundoApellido = row["SGAP_Q66"].ToString(),
                                                NroCuenta = row["NUCU_Q66"].ToString(),
                                                Cbu = row["NCBU_Q66"].ToString()
                                            };

                        IList<IFicha> listafichas =
                            _ficharepositorio.GetFichaByNumeroDocumento(importado.NumeroDocumento.Trim());

                        foreach (var listaficha in listafichas)
                        {
                            listaficha.Apellido = importado.Apellido.ToUpper().Trim() + " " +
                                                  importado.SegundoApellido.ToUpper().Trim();
                            listaficha.Nombre = importado.Nombre.ToUpper().Trim() + " " +
                                                importado.SegundoNombre.ToUpper().Trim();

                            _ficharepositorio.UpdateFichaSimple(listaficha);


                            IList<IBeneficiario> listabeneficiario =
                                _beneficiariorepositorio.GetBeneficiarioByFicha(listaficha.IdFicha);

                            foreach (var beneficiario in listabeneficiario)
                            {
                                ISucursal objsucursal = _sucursalrepositorio.GetSucursalByCodigoBanco(importado.Sucursal);

                                importado.NombreSucursal = objsucursal.CodigoBanco + " - " + objsucursal.Detalle;

                                if (_beneficiariorepositorio.UpdateCuentaCbu(beneficiario.IdBeneficiario, importado.NroCuenta,
                                                                         importado.Cbu, objsucursal.IdSucursal))
                                {
                                    importado.Importado = true;
                                }
                            }
                        }

                        IList<IApoderado> listaapoderados =
                            _apoderadorepositorio.GetApoderadosByNumeroDocumento(importado.NumeroDocumento);

                        foreach (var listaapoderado in listaapoderados)
                        {
                            string nombre = importado.Nombre.ToUpper().Trim() + " " +
                                            importado.SegundoNombre.ToUpper().Trim();
                            string apellido = importado.Apellido.ToUpper().Trim() + " " +
                                              importado.SegundoApellido.ToUpper().Trim();
                            string nrocuenta = importado.NroCuenta;
                            string cbu = importado.Cbu;
                            ISucursal objsucursal = _sucursalrepositorio.GetSucursalByCodigoBanco(importado.Sucursal);

                            importado.NombreSucursal = objsucursal.CodigoBanco + " - " + objsucursal.Detalle;

                            listaapoderado.Nombre = nombre;
                            listaapoderado.Apellido = apellido;
                            listaapoderado.Cbu = cbu;
                            listaapoderado.NumeroCuentaBco = Convert.ToInt32(nrocuenta);
                            listaapoderado.IdSucursal = (short)objsucursal.IdSucursal;

                            importado.EsApoderado = true;

                            if (_apoderadorepositorio.UpdateApoderadoSimple(listaapoderado))
                            {
                                importado.Importado = true;
                            }
                        }

                        if (importado.Importado == false)
                        {
                            ISucursal objsucursal = _sucursalrepositorio.GetSucursalByCodigoBanco(importado.Sucursal);
                            importado.NombreSucursal = objsucursal.CodigoBanco + " - " + objsucursal.Detalle;
                        }

                        listaresultado.Add(importado);
                    }
                }
            }

            return listaresultado.OrderByDescending(c => c.Importado).ThenBy(c => c.Apellido).ThenBy(c => c.Nombre).ToList();

        }

        public IBeneficiarioVista ClearFechaSolicitud(int idBeneficiario)
        {

            bool mclearoK = _cuentabancorepositorio.ClearFechaSolicitud(idBeneficiario);

            IBeneficiarioVista mreturnclear = new BeneficiarioVista();
            IBeneficiario obj = _beneficiariorepositorio.GetBeneficiario(idBeneficiario);

            mreturnclear.Email = obj.Email;
            mreturnclear.IdEstado = obj.IdEstado;
            mreturnclear.IdFicha = obj.IdFicha;
            mreturnclear.Apellido = obj.Ficha.Apellido;
            mreturnclear.Nombre = obj.Ficha.Nombre;
            mreturnclear.Cuil = obj.Ficha.Cuil;
            mreturnclear.NumeroDocumento = obj.Ficha.NumeroDocumento;
            mreturnclear.IdBeneficiario = obj.IdBeneficiario;
            mreturnclear.Programa = obj.Programa;

            mreturnclear.TieneApoderadoActivo = _apoderadorepositorio.TieneApoderadoActivo(obj.IdBeneficiario);
            mreturnclear.ListaApoderados = _apoderadorepositorio.GetApoderadosByBeneficiario(obj.IdBeneficiario);
            CargarEstadosBeneficiario(mreturnclear, _estadobeneficiariorepositorio.GetEstadosBeneficiario());

            CargarSucursales(mreturnclear, _sucursalrepositorio.GetSucursales());

            mreturnclear.Sucursal = obj.Sucursal;
            mreturnclear.NumeroCuenta = obj.NumeroCuenta;
            mreturnclear.Cbu = obj.Cbu;
            mreturnclear.FechaSolicitudCuenta = obj.FechaSolicitudCuenta;
            mreturnclear.ClearFechaSolicitud = mclearoK;
            mreturnclear.FechaNotificacion = obj.FechaNotificacion;

            mreturnclear.Estados.Selected = obj.IdEstado.ToString();
            mreturnclear.Estados.Enabled = false;

            CargarMonedas(mreturnclear, _tablabcocbarepositorio.GetMonedas());
            mreturnclear.Monedas.Enabled = false;
            mreturnclear.Monedas.Selected = Constantes.MonedaBanco.ToString();

            mreturnclear.FechaInicioBeneficio = obj.FechaInicioBeneficio;
            mreturnclear.FechaBajaBeneficio = obj.FechaBajaBeneficio;

            mreturnclear.ListaConceptosPago = _conceptorepositorio.GetConceptosByBeneficiario(idBeneficiario);

            return mreturnclear;

        }

        public IArchivosVista GetArchivos()
        {
            IArchivosVista vista = new ArchivosVista
                                       {
                                           ListaArchivos = _beneficiariorepositorio.GetArchivos()
                                       };

            return vista;
        }

        public bool ClearFechaSolicitudArchivobyFecha(DateTime fecha)
        {
            try
            {
                return _beneficiariorepositorio.ClearFechaSolicitudArchivobyFecha(fecha);
            }
            catch (Exception)
            {
                AddError("Sucedio un Error verifique los Datos");
                return false;
            }
        }

        public IArchivosVista VolverGenerarArchivo(DateTime fecha)
        {
            IArchivosVista vista = new ArchivosVista();

            IList<IBeneficiario> lista = _beneficiariorepositorio.GetBeneficiarioSinCuentaByFecha("", "", "", "", 0, "T",
                                                                                                  "T", "T", "T",
                                                                                                  0, "", fecha);

            vista.Nombre = VolverGenerarFile(lista);

            vista.Url = "../Archivos/" + vista.Nombre;

            return vista;

        }

        public byte[] ExportBeneficiario(IBeneficiariosVista vista)
        {
            var path = String.Empty;

            switch (Convert.ToInt32(vista.Programas.Selected))
            {
                case (int)Enums.TipoFicha.Terciaria:
                    path = HttpContext.Current.Server.MapPath(@"\Archivos\Exportados\TemplateBeneficiarioTerciarios.xls");
                    break;
                case (int)Enums.TipoFicha.Universitaria:
                    path = HttpContext.Current.Server.MapPath(@"\Archivos\Exportados\TemplateBeneficiarioUniversitarios.xls");
                    break;
                case (int)Enums.TipoFicha.Ppp:
                    path = HttpContext.Current.Server.MapPath(@"\Archivos\Exportados\TemplateBeneficiariosPPP.xls");
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
                default:
                    return new byte[] { };
            }

            var fs = new FileStream(path, FileMode.Open, FileAccess.Read);

            var templateWorkbook = new HSSFWorkbook(fs, true);

            var sheet = templateWorkbook.GetSheet("Nomina");

            IList<IBeneficiario> listaparaExcel;
            IList<IBeneficiario> lista;
            int[] beneficiarios;

            switch (vista.ConCuenta)
            {
                case "S":
                    lista = _beneficiariorepositorio.GetBeneficiarioConCuenta(vista.NombreBusqueda,
                                                                               vista.ApellidoBusqueda,
                                                                               vista.CuilBusqueda,
                                                                               vista.NumeroDocumentoBusqueda,
                                                                               Convert.ToInt32(vista.Programas.Selected),
                                                                               vista.ConApoderado, vista.ConModalidad,
                                                                               vista.ConDiscapacidad,
                                                                               vista.ConAltaTemprana,
                                                                               Convert.ToInt32(vista.EstadosBeneficiarios.Selected),
                                                                               vista.ApellidoNombreApoderadoBusqueda, Convert.ToInt32(vista.etapas.Selected), vista.nrotramite_busqueda);

                    beneficiarios = lista.Select(c => c.IdBeneficiario).ToArray();

                    //listaparaExcel =
                    //  _beneficiariorepositorio.GetBeneficiariosCompleto(beneficiarios, Convert.ToInt32(vista.Programas.Selected));
                    listaparaExcel = _beneficiariorepositorio.GetBeneficiariosCompletoBis(beneficiarios, Convert.ToInt32(vista.Programas.Selected));
                    break;
                case "N":
                    lista = _beneficiariorepositorio.GetBeneficiarioSinCuenta(vista.NombreBusqueda,
                                                                                       vista.ApellidoBusqueda,
                                                                                       vista.CuilBusqueda,
                                                                                       vista.NumeroDocumentoBusqueda,
                                                                                       Convert.ToInt32(vista.Programas.Selected),
                                                                                       vista.ConApoderado,
                                                                                       vista.ConModalidad,
                                                                                       vista.ConDiscapacidad,
                                                                                       vista.ConAltaTemprana,
                                                                                       Convert.ToInt32(vista.EstadosBeneficiarios.Selected),
                                                                                       vista.ApellidoNombreApoderadoBusqueda, Convert.ToInt32(vista.etapas.Selected), vista.nrotramite_busqueda);

                    beneficiarios = lista.Select(c => c.IdBeneficiario).ToArray();

                    //listaparaExcel = _beneficiariorepositorio.GetBeneficiariosCompleto(beneficiarios, Convert.ToInt32(vista.Programas.Selected));
                    listaparaExcel = _beneficiariorepositorio.GetBeneficiariosCompletoBis(beneficiarios, Convert.ToInt32(vista.Programas.Selected));
                    break;
                default:
                    lista = _beneficiariorepositorio.GetBeneficiarios(vista.NombreBusqueda,
                                                                    vista.ApellidoBusqueda,
                                                                    vista.CuilBusqueda,
                                                                    vista.NumeroDocumentoBusqueda,
                                                                    Convert.ToInt32(vista.Programas.Selected),
                                                                    vista.ConApoderado,
                                                                    vista.ConModalidad,
                                                                    vista.ConDiscapacidad,
                                                                    vista.ConAltaTemprana,
                                                                    Convert.ToInt32(vista.EstadosBeneficiarios.Selected),
                                                                    vista.ApellidoNombreApoderadoBusqueda, Convert.ToInt32(vista.etapas.Selected), vista.nrotramite_busqueda);

                    beneficiarios = lista.Select(c => c.IdBeneficiario).ToArray();

                    //listaparaExcel = _beneficiariorepositorio.GetBeneficiariosCompleto(beneficiarios, Convert.ToInt32(vista.Programas.Selected));

                    
                    listaparaExcel = _beneficiariorepositorio.GetBeneficiariosCompletoBis(beneficiarios, Convert.ToInt32(vista.Programas.Selected));

                    break;
            }

            var i = 1;

            switch (Convert.ToInt32(vista.Programas.Selected))
            {
                case (int)Enums.TipoFicha.Terciaria:
                    #region case (int)Enums.TipoFicha.Terciaria
                    foreach (var beneficiario in listaparaExcel)
                    {
                        i++;
                        //Ficha
                        var row = sheet.CreateRow(i);
                        var cellidficha = row.CreateCell(0); cellidficha.SetCellValue(beneficiario.Ficha.IdFicha);
                        var cellApellido = row.CreateCell(1); cellApellido.SetCellValue(beneficiario.Ficha.Apellido);
                        var cellNombre = row.CreateCell(2); cellNombre.SetCellValue(beneficiario.Ficha.Nombre);
                        var cellCuil = row.CreateCell(3); cellCuil.SetCellValue(beneficiario.Ficha.Cuil);
                        var cellDni = row.CreateCell(4); cellDni.SetCellValue(beneficiario.Ficha.NumeroDocumento);
                        var cellFechaNa = row.CreateCell(5); cellFechaNa.SetCellValue(beneficiario.Ficha.FechaNacimiento.ToString());
                        var cellCalle = row.CreateCell(6); cellCalle.SetCellValue(beneficiario.Ficha.Calle);
                        var cellNro = row.CreateCell(7); cellNro.SetCellValue(beneficiario.Ficha.Numero);
                        var cellPiso = row.CreateCell(8); cellPiso.SetCellValue(beneficiario.Ficha.Piso);
                        var cellDpto = row.CreateCell(9); cellDpto.SetCellValue(beneficiario.Ficha.Dpto);
                        var cellMono = row.CreateCell(10); cellMono.SetCellValue(beneficiario.Ficha.Monoblock);
                        var cellParcela = row.CreateCell(11); cellParcela.SetCellValue(beneficiario.Ficha.Parcela);
                        var cellManzana = row.CreateCell(12); cellManzana.SetCellValue(beneficiario.Ficha.Manzana);
                        var cellEntreCalle = row.CreateCell(13); cellEntreCalle.SetCellValue(beneficiario.Ficha.EntreCalles);
                        var cellBarrio = row.CreateCell(14); cellBarrio.SetCellValue(beneficiario.Ficha.Barrio);
                        var cellCp = row.CreateCell(15); cellCp.SetCellValue(beneficiario.Ficha.CodigoPostal);
                        var cellidLocalidad = row.CreateCell(16); cellidLocalidad.SetCellValue(beneficiario.Ficha.IdLocalidad == null ? String.Empty
                            : beneficiario.Ficha.LocalidadFicha.IdLocalidad.ToString());
                        var cellnLocalidad = row.CreateCell(17); cellnLocalidad.SetCellValue(beneficiario.Ficha.LocalidadFicha.NombreLocalidad);
                        var cellIdDepartamento = row.CreateCell(18); cellIdDepartamento.SetCellValue(beneficiario.Ficha.IdLocalidad == null ? String.Empty
                            : beneficiario.Ficha.LocalidadFicha.Departamento.IdDepartamento.ToString());
                        var cellnDepartamento = row.CreateCell(19); cellnDepartamento.SetCellValue(beneficiario.Ficha.LocalidadFicha.Departamento.NombreDepartamento);
                        var cellTelFijo = row.CreateCell(20); cellTelFijo.SetCellValue(beneficiario.Ficha.TelefonoFijo);
                        var cellTelCelular = row.CreateCell(21); cellTelCelular.SetCellValue(beneficiario.Ficha.TelefonoCelular);
                        var cellContacto = row.CreateCell(22); cellContacto.SetCellValue(beneficiario.Ficha.Contacto);
                        var cellEmail = row.CreateCell(23); cellEmail.SetCellValue(beneficiario.Ficha.Mail);
                        var cellSexo = row.CreateCell(24); cellSexo.SetCellValue(beneficiario.Ficha.Sexo);
                        var cellidestadoFicha = row.CreateCell(25); cellidestadoFicha.SetCellValue(beneficiario.Ficha.IdEstadoFicha == null ? String.Empty
                            : beneficiario.Ficha.IdEstadoFicha.ToString());
                        var cellnestadoFicha = row.CreateCell(26); cellnestadoFicha.SetCellValue(beneficiario.Ficha.NombreEstadoFicha);

                        var cellPromedioterc = row.CreateCell(27); cellPromedioterc.SetCellValue(beneficiario.Ficha.PromedioTerciaria == null ? String.Empty
                            : beneficiario.Ficha.PromedioTerciaria.ToString());
                        var cellOtracarreraterc = row.CreateCell(28); cellOtracarreraterc.SetCellValue(beneficiario.Ficha.OtraCarreraTerciaria);
                        var cellOtrainstiterc = row.CreateCell(29); cellOtrainstiterc.SetCellValue(beneficiario.Ficha.OtraInstitucionTerciaria);
                        var cellIdcarreraterc = row.CreateCell(30); cellIdcarreraterc.SetCellValue(beneficiario.Ficha.IdCarreraTerciaria == null ?
                            String.Empty : beneficiario.Ficha.CarreraFicha.IdCarrera.ToString());
                        var cellIdescuelaterc = row.CreateCell(31); cellIdescuelaterc.SetCellValue(beneficiario.Ficha.IdEscuelaTerciaria == null ?
                            String.Empty : beneficiario.Ficha.EscuelaFicha.Id_Escuela.ToString());
                        var cellEscuelaterc = row.CreateCell(32); cellEscuelaterc.SetCellValue(beneficiario.Ficha.EscuelaFicha.Nombre_Escuela);
                        var cellCueterc = row.CreateCell(33); cellCueterc.SetCellValue(beneficiario.Ficha.EscuelaFicha.Cue);
                        var cellAnexoterc = row.CreateCell(34); cellAnexoterc.SetCellValue(beneficiario.Ficha.EscuelaFicha.Anexo == null ? String.Empty
                            : beneficiario.Ficha.EscuelaFicha.Anexo.ToString());

                        var cellEscBarriterc = row.CreateCell(35); cellEscBarriterc.SetCellValue(beneficiario.Ficha.EscuelaFicha.Barrio);
                        var cellEscidlocalidadterc = row.CreateCell(36); cellEscidlocalidadterc.SetCellValue(beneficiario.Ficha.IdLocalidadEscTerc == null ?
                            String.Empty : beneficiario.Ficha.EscuelaFicha.LocalidadEscuela.IdLocalidad.ToString());
                        var cellEsclocalidadterc = row.CreateCell(37); cellEsclocalidadterc.SetCellValue(beneficiario.Ficha.EscuelaFicha.LocalidadEscuela.NombreLocalidad);
                        var cellOtrosectorterc = row.CreateCell(38); cellOtrosectorterc.SetCellValue(beneficiario.Ficha.OtroSectorTerciaria);
                        var cellCarreraterc = row.CreateCell(39); cellCarreraterc.SetCellValue(beneficiario.Ficha.CarreraFicha.NombreCarrera);
                        var cellIdnivelterc = row.CreateCell(40); cellIdnivelterc.SetCellValue(beneficiario.Ficha.CarreraFicha.IdNivel == null ?
                            String.Empty : beneficiario.Ficha.CarreraFicha.NivelCarrera.IdNivel.ToString());
                        var cellNivelterc = row.CreateCell(41); cellNivelterc.SetCellValue(beneficiario.Ficha.CarreraFicha.NivelCarrera.NombreNivel);
                        var cellIdinstitucionterc = row.CreateCell(42); cellIdinstitucionterc.SetCellValue(beneficiario.Ficha.CarreraFicha.IdInstitucion == null ?
                            String.Empty : beneficiario.Ficha.CarreraFicha.InstitucionCarrera.IdInstitucion.ToString());
                        var cellInstitucionterc = row.CreateCell(43); cellInstitucionterc.SetCellValue(beneficiario.Ficha.CarreraFicha.InstitucionCarrera.NombreInstitucion);
                        var cellIdsectorterc = row.CreateCell(44); cellIdsectorterc.SetCellValue(beneficiario.Ficha.CarreraFicha.IdSector == null ? String.Empty
                            : beneficiario.Ficha.CarreraFicha.SectorCarrera.IdSector.ToString());
                        var cellSectorterc = row.CreateCell(45); cellSectorterc.SetCellValue(beneficiario.Ficha.CarreraFicha.SectorCarrera.NombreSector);
                        var cellFechaModif = row.CreateCell(46); cellFechaModif.SetCellValue(beneficiario.Ficha.FechaSistema == null ? String.Empty
                            : beneficiario.Ficha.FechaSistema.Value.ToShortDateString());

                        //Beneficiario
                        var cellIdben = row.CreateCell(47); cellIdben.SetCellValue(beneficiario.IdBeneficiario);
                        var cellIdestadoben = row.CreateCell(48); cellIdestadoben.SetCellValue(beneficiario.IdEstado);
                        var cellEstadoben = row.CreateCell(49); cellEstadoben.SetCellValue(beneficiario.NombreEstado);
                        var cellNrocuentaben = row.CreateCell(50); cellNrocuentaben.SetCellValue(beneficiario.NumeroCuenta == null ? String.Empty
                            : beneficiario.NumeroCuenta.ToString());
                        var cellIdsucursalben = row.CreateCell(51); cellIdsucursalben.SetCellValue(beneficiario.IdSucursal == null ? String.Empty
                            : beneficiario.IdSucursal.ToString());
                        var cellCodsucursalben = row.CreateCell(52); cellCodsucursalben.SetCellValue(beneficiario.CodigoSucursal);
                        var cellSucursalben = row.CreateCell(53); cellSucursalben.SetCellValue(beneficiario.SucursalDescripcion);
                        var cellFecsolben = row.CreateCell(54); cellFecsolben.SetCellValue(beneficiario.FechaSolicitudCuenta == null ? String.Empty
                            : beneficiario.FechaSolicitudCuenta.Value.ToShortDateString());
                        var cellNotificado = row.CreateCell(55); cellNotificado.SetCellValue(beneficiario.Notificado ? "S" : "N");
                        var cellFecNotif = row.CreateCell(56); cellFecNotif.SetCellValue(beneficiario.FechaNotificacion == null ? String.Empty
                            : beneficiario.FechaNotificacion.Value.ToShortDateString());

                        var cellFechaIniBenf = row.CreateCell(57); cellFechaIniBenf.SetCellValue(beneficiario.FechaInicioBeneficio == null ? string.Empty : beneficiario.FechaInicioBeneficio.Value.ToShortDateString()); // 20/03/2013 - DI CAMPLI LEANDRO
                        var cellBajaBenef = row.CreateCell(58); cellBajaBenef.SetCellValue(beneficiario.FechaBajaBeneficio == null ? string.Empty : beneficiario.FechaBajaBeneficio.Value.ToShortDateString()); // 20/03/2013 - DI CAMPLI LEANDRO

                        var cellTieneapoben = row.CreateCell(59); cellTieneapoben.SetCellValue(beneficiario.TieneApoderado);

                        //Apoderado
                        var cellIdapoderado = row.CreateCell(60); cellIdapoderado.SetCellValue(beneficiario.Apoderado.IdApoderadoNullable == null ?
                            String.Empty : beneficiario.Apoderado.IdApoderadoNullable.ToString());
                        var cellApellidoapo = row.CreateCell(61); cellApellidoapo.SetCellValue(beneficiario.Apoderado.Apellido);
                        var cellNombreapo = row.CreateCell(62); cellNombreapo.SetCellValue(beneficiario.Apoderado.Nombre);
                        var cellDniapo = row.CreateCell(63); cellDniapo.SetCellValue(beneficiario.Apoderado.NumeroDocumento);
                        var cellCuilapo = row.CreateCell(64); cellCuilapo.SetCellValue(beneficiario.Apoderado.Cuil);
                        var cellSexoapo = row.CreateCell(65); cellSexoapo.SetCellValue(beneficiario.Apoderado.Sexo);
                        var cellNroctaapo = row.CreateCell(66); cellNroctaapo.SetCellValue(beneficiario.Apoderado.NumeroCuentaBco == null ? String.Empty
                            : beneficiario.Apoderado.NumeroCuentaBco.ToString());
                        var cellIdSuc = row.CreateCell(67); cellIdSuc.SetCellValue(beneficiario.Apoderado.IdSucursal == null ? String.Empty
                            : beneficiario.Apoderado.SucursalApoderado.IdSucursal.ToString());
                        var cellCodsucapo = row.CreateCell(68); cellCodsucapo.SetCellValue(beneficiario.Apoderado.SucursalApoderado.CodigoBanco);
                        var cellSucursalapo = row.CreateCell(69); cellSucursalapo.SetCellValue(beneficiario.Apoderado.SucursalApoderado.Detalle);
                        var cellFecsolcuentaapo = row.CreateCell(70); cellFecsolcuentaapo.SetCellValue(beneficiario.Apoderado.FechaSolicitudCuenta == null ?
                            String.Empty : beneficiario.Apoderado.FechaSolicitudCuenta.Value.ToShortDateString());
                        var cellFecnac = row.CreateCell(71); cellFecnac.SetCellValue(beneficiario.Apoderado.FechaNacimiento == null ? String.Empty :
                            beneficiario.Apoderado.FechaNacimiento.Value.ToShortDateString());
                        var cellCalleapo = row.CreateCell(72); cellCalleapo.SetCellValue(beneficiario.Apoderado.Calle);
                        var cellNroapo = row.CreateCell(73); cellNroapo.SetCellValue(beneficiario.Apoderado.Numero);
                        var cellPisoapo = row.CreateCell(74); cellPisoapo.SetCellValue(beneficiario.Apoderado.Piso);
                        var cellMonoblockapo = row.CreateCell(75); cellMonoblockapo.SetCellValue(beneficiario.Apoderado.Monoblock);
                        var cellDptoapo = row.CreateCell(76); cellDptoapo.SetCellValue(beneficiario.Apoderado.Dpto);
                        var cellParcelaapo = row.CreateCell(77); cellParcelaapo.SetCellValue(beneficiario.Apoderado.Parcela);
                        var cellManzanaapo = row.CreateCell(78); cellManzanaapo.SetCellValue(beneficiario.Apoderado.Manzana);
                        var cellEntrecalleapo = row.CreateCell(79); cellEntrecalleapo.SetCellValue(beneficiario.Apoderado.EntreCalles);
                        var cellBarrioapo = row.CreateCell(80); cellBarrioapo.SetCellValue(beneficiario.Apoderado.Barrio);
                        var cellCpapo = row.CreateCell(81); cellCpapo.SetCellValue(beneficiario.Apoderado.CodigoPostal);
                        var cellIdlocalidadapo = row.CreateCell(82); cellIdlocalidadapo.SetCellValue(beneficiario.Apoderado.IdLocalidad == null ?
                            String.Empty : beneficiario.Apoderado.LocalidadApoderado.IdLocalidad.ToString());
                        var cellLocalidadapo = row.CreateCell(83); cellLocalidadapo.SetCellValue(beneficiario.Apoderado.LocalidadApoderado.NombreLocalidad);
                        var cellIddeptoapo = row.CreateCell(84); cellIddeptoapo.SetCellValue(beneficiario.Apoderado.IdLocalidad == null ? String.Empty
                            : beneficiario.Apoderado.LocalidadApoderado.Departamento.IdDepartamento.ToString());
                        var cellDeptoapo = row.CreateCell(85); cellDeptoapo.SetCellValue(beneficiario.Apoderado.LocalidadApoderado.Departamento.NombreDepartamento);
                        var cellTelefonoapo = row.CreateCell(86); cellTelefonoapo.SetCellValue(beneficiario.Apoderado.TelefonoFijo);
                        var cellCelularapo = row.CreateCell(87); cellCelularapo.SetCellValue(beneficiario.Apoderado.TelefonoCelular);
                        var cellEmailapo = row.CreateCell(88);
                        cellEmailapo.SetCellValue(beneficiario.Apoderado.Mail);
                    }
                    #endregion
                    break;
                case (int)Enums.TipoFicha.Universitaria:
                    #region case (int)Enums.TipoFicha.Universitaria
                    foreach (var beneficiario in listaparaExcel)
                    {
                        i++;
                        //Ficha
                        var row = sheet.CreateRow(i);
                        var cellidficha = row.CreateCell(0); cellidficha.SetCellValue(beneficiario.Ficha.IdFicha);
                        var cellApellido = row.CreateCell(1); cellApellido.SetCellValue(beneficiario.Ficha.Apellido);
                        var cellNombre = row.CreateCell(2); cellNombre.SetCellValue(beneficiario.Ficha.Nombre);
                        var cellCuil = row.CreateCell(3); cellCuil.SetCellValue(beneficiario.Ficha.Cuil);
                        var cellDni = row.CreateCell(4); cellDni.SetCellValue(beneficiario.Ficha.NumeroDocumento);
                        var cellFechaNa = row.CreateCell(5); cellFechaNa.SetCellValue(beneficiario.Ficha.FechaNacimiento == null ? String.Empty
                            : beneficiario.Ficha.FechaNacimiento.Value.ToShortDateString());
                        var cellCalle = row.CreateCell(6); cellCalle.SetCellValue(beneficiario.Ficha.Calle);
                        var cellNro = row.CreateCell(7); cellNro.SetCellValue(beneficiario.Ficha.Numero);
                        var cellPiso = row.CreateCell(8); cellPiso.SetCellValue(beneficiario.Ficha.Piso);
                        var cellDpto = row.CreateCell(9); cellDpto.SetCellValue(beneficiario.Ficha.Dpto);
                        var cellMono = row.CreateCell(10); cellMono.SetCellValue(beneficiario.Ficha.Monoblock);
                        var cellParcela = row.CreateCell(11); cellParcela.SetCellValue(beneficiario.Ficha.Parcela);
                        var cellManzana = row.CreateCell(12); cellManzana.SetCellValue(beneficiario.Ficha.Manzana);
                        var cellEntreCalle = row.CreateCell(13); cellEntreCalle.SetCellValue(beneficiario.Ficha.EntreCalles);
                        var cellBarrio = row.CreateCell(14); cellBarrio.SetCellValue(beneficiario.Ficha.Barrio);
                        var cellCp = row.CreateCell(15); cellCp.SetCellValue(beneficiario.Ficha.CodigoPostal);
                        var cellidLocalidad = row.CreateCell(16); cellidLocalidad.SetCellValue(beneficiario.Ficha.IdLocalidad == null ?
                            String.Empty : beneficiario.Ficha.LocalidadFicha.IdLocalidad.ToString());
                        var cellnLocalidad = row.CreateCell(17); cellnLocalidad.SetCellValue(beneficiario.Ficha.LocalidadFicha.NombreLocalidad);
                        var cellIdDepartamento = row.CreateCell(18); cellIdDepartamento.SetCellValue(beneficiario.Ficha.IdLocalidad == null ?
                            String.Empty : beneficiario.Ficha.LocalidadFicha.Departamento.IdDepartamento.ToString());
                        var cellnDepartamento = row.CreateCell(19); cellnDepartamento.SetCellValue(beneficiario.Ficha.LocalidadFicha.Departamento.NombreDepartamento);
                        var cellTelFijo = row.CreateCell(20); cellTelFijo.SetCellValue(beneficiario.Ficha.TelefonoFijo);
                        var cellTelCelular = row.CreateCell(21); cellTelCelular.SetCellValue(beneficiario.Ficha.TelefonoCelular);
                        var cellContacto = row.CreateCell(22); cellContacto.SetCellValue(beneficiario.Ficha.Contacto);
                        var cellEmail = row.CreateCell(23); cellEmail.SetCellValue(beneficiario.Ficha.Mail);
                        var cellSexo = row.CreateCell(24); cellSexo.SetCellValue(beneficiario.Ficha.Sexo);
                        var cellidestadoFicha = row.CreateCell(25); cellidestadoFicha.SetCellValue(beneficiario.Ficha.IdEstadoFicha == null ? String.Empty :
                            beneficiario.Ficha.IdEstadoFicha.ToString());
                        var cellnestadoFicha = row.CreateCell(26); cellnestadoFicha.SetCellValue(beneficiario.Ficha.NombreEstadoFicha);

                        var cellPromedioUniv = row.CreateCell(27); cellPromedioUniv.SetCellValue(beneficiario.Ficha.PromedioUniversitaria == null ? String.Empty
                            : beneficiario.Ficha.PromedioUniversitaria.ToString());
                        var cellOtraCarreraUniv = row.CreateCell(28); cellOtraCarreraUniv.SetCellValue(beneficiario.Ficha.OtraCarreraUniversitaria);
                        var cellOtraInstiUniv = row.CreateCell(29); cellOtraInstiUniv.SetCellValue(beneficiario.Ficha.OtraInstitucionUniversitaria);
                        var cellIdCarreraUniv = row.CreateCell(30); cellIdCarreraUniv.SetCellValue(beneficiario.Ficha.IdCarreraUniversitaria == null ?
                            String.Empty : beneficiario.Ficha.CarreraFicha.IdCarrera.ToString());
                        var cellCarreraUniv = row.CreateCell(31); cellCarreraUniv.SetCellValue(beneficiario.Ficha.CarreraFicha.NombreCarrera);
                        var cellIdEscuelaUniv = row.CreateCell(32); cellIdEscuelaUniv.SetCellValue(beneficiario.Ficha.IdEscuelaUniversitaria == null ?
                            String.Empty : beneficiario.Ficha.EscuelaFicha.Id_Escuela.ToString());
                        var cellEscuelaUniv = row.CreateCell(33); cellEscuelaUniv.SetCellValue(beneficiario.Ficha.EscuelaFicha.Nombre_Escuela);
                        var cellCueUniv = row.CreateCell(34); cellCueUniv.SetCellValue(beneficiario.Ficha.EscuelaFicha.Cue);
                        var cellAnexoUniv = row.CreateCell(35); cellAnexoUniv.SetCellValue(beneficiario.Ficha.EscuelaFicha.Anexo == null ?
                            String.Empty : beneficiario.Ficha.EscuelaFicha.Anexo.ToString());

                        var cellEscBarrioUniv = row.CreateCell(36); cellEscBarrioUniv.SetCellValue(beneficiario.Ficha.EscuelaFicha.Barrio);
                        var cellEscIdLocalidadUniv = row.CreateCell(37); cellEscIdLocalidadUniv.SetCellValue(beneficiario.Ficha.EscuelaFicha.Id_Localidad == null ?
                            String.Empty : beneficiario.Ficha.EscuelaFicha.LocalidadEscuela.IdLocalidad.ToString());
                        var cellEscLocalidadUniv = row.CreateCell(38); cellEscLocalidadUniv.SetCellValue(beneficiario.Ficha.EscuelaFicha.LocalidadEscuela.NombreLocalidad);
                        var cellOtroSectorUniv = row.CreateCell(39); cellOtroSectorUniv.SetCellValue(beneficiario.Ficha.OtroSectorTerciaria);
                        var cellIdNivelUniv = row.CreateCell(40); cellIdNivelUniv.SetCellValue(beneficiario.Ficha.CarreraFicha.IdNivel == null ?
                            String.Empty : beneficiario.Ficha.CarreraFicha.NivelCarrera.IdNivel.ToString());
                        var cellNivelUniv = row.CreateCell(41); cellNivelUniv.SetCellValue(beneficiario.Ficha.CarreraFicha.NivelCarrera.NombreNivel);
                        var cellIdInstitucionUniv = row.CreateCell(42); cellIdInstitucionUniv.SetCellValue(beneficiario.Ficha.CarreraFicha.IdInstitucion == null ?
                            String.Empty : beneficiario.Ficha.CarreraFicha.InstitucionCarrera.IdInstitucion.ToString());
                        var cellInstitucionUniv = row.CreateCell(43); cellInstitucionUniv.SetCellValue(beneficiario.Ficha.CarreraFicha.InstitucionCarrera.NombreInstitucion);
                        var cellIdSectorUniv = row.CreateCell(44); cellIdSectorUniv.SetCellValue(beneficiario.Ficha.CarreraFicha.IdSector == null ?
                            String.Empty : beneficiario.Ficha.CarreraFicha.SectorCarrera.IdSector.ToString());
                        var cellSectorUniv = row.CreateCell(45); cellSectorUniv.SetCellValue(beneficiario.Ficha.CarreraFicha.SectorCarrera.NombreSector);
                        var cellFechaModif = row.CreateCell(46); cellFechaModif.SetCellValue(beneficiario.Ficha.FechaSistema == null ? String.Empty
                            : beneficiario.Ficha.FechaSistema.Value.ToShortDateString());

                        //Beneficiario
                        var cellIdben = row.CreateCell(47); cellIdben.SetCellValue(beneficiario.IdBeneficiario);
                        var cellIdestadoben = row.CreateCell(48); cellIdestadoben.SetCellValue(beneficiario.IdEstado);
                        var cellEstadoben = row.CreateCell(49); cellEstadoben.SetCellValue(beneficiario.NombreEstado);
                        var cellNrocuentaben = row.CreateCell(50); cellNrocuentaben.SetCellValue(beneficiario.NumeroCuenta == null ? String.Empty
                            : beneficiario.NumeroCuenta.ToString());
                        var cellIdsucursalben = row.CreateCell(51); cellIdsucursalben.SetCellValue(beneficiario.IdSucursal == null ? String.Empty
                            : beneficiario.IdSucursal.ToString());
                        var cellCodsucursalben = row.CreateCell(52); cellCodsucursalben.SetCellValue(beneficiario.CodigoSucursal);
                        var cellSucursalben = row.CreateCell(53); cellSucursalben.SetCellValue(beneficiario.SucursalDescripcion);
                        var cellFecsolben = row.CreateCell(54); cellFecsolben.SetCellValue(beneficiario.FechaSolicitudCuenta == null ?
                            String.Empty : beneficiario.FechaSolicitudCuenta.Value.ToShortDateString());
                        var cellNotificado = row.CreateCell(55); cellNotificado.SetCellValue(beneficiario.Notificado ? "S" : "N");
                        var cellFecNotif = row.CreateCell(56); cellFecNotif.SetCellValue(beneficiario.FechaNotificacion.ToString() ?? "");

                        var cellFechaIniBenf = row.CreateCell(57); cellFechaIniBenf.SetCellValue(beneficiario.FechaInicioBeneficio == null ? string.Empty : beneficiario.FechaInicioBeneficio.Value.ToShortDateString()); // 20/03/2013 - DI CAMPLI LEANDRO
                        var cellBajaBenef = row.CreateCell(58); cellBajaBenef.SetCellValue(beneficiario.FechaBajaBeneficio == null ? string.Empty : beneficiario.FechaBajaBeneficio.Value.ToShortDateString()); // 20/03/2013 - DI CAMPLI LEANDRO

                        var cellTieneapoben = row.CreateCell(59); cellTieneapoben.SetCellValue(beneficiario.TieneApoderado);

                        //Apoderado
                        var cellIdapoderado = row.CreateCell(60); cellIdapoderado.SetCellValue(beneficiario.Apoderado.IdApoderadoNullable == null ?
                            String.Empty : beneficiario.Apoderado.IdApoderadoNullable.ToString());
                        var cellApellidoapo = row.CreateCell(61); cellApellidoapo.SetCellValue(beneficiario.Apoderado.Apellido);
                        var cellNombreapo = row.CreateCell(62); cellNombreapo.SetCellValue(beneficiario.Apoderado.Nombre);
                        var cellDniapo = row.CreateCell(63); cellDniapo.SetCellValue(beneficiario.Apoderado.NumeroDocumento);
                        var cellCuilapo = row.CreateCell(64); cellCuilapo.SetCellValue(beneficiario.Apoderado.Cuil);
                        var cellSexoapo = row.CreateCell(65); cellSexoapo.SetCellValue(beneficiario.Apoderado.Sexo);
                        var cellNroctaapo = row.CreateCell(66); cellNroctaapo.SetCellValue(beneficiario.Apoderado.NumeroCuentaBco == null ? String.Empty
                            : beneficiario.Apoderado.NumeroCuentaBco.ToString());
                        var cellIdSuc = row.CreateCell(67); cellIdSuc.SetCellValue(beneficiario.Apoderado.IdSucursal == null ? String.Empty
                            : beneficiario.Apoderado.SucursalApoderado.IdSucursal.ToString());
                        var cellCodsucapo = row.CreateCell(68); cellCodsucapo.SetCellValue(beneficiario.Apoderado.SucursalApoderado.CodigoBanco);
                        var cellSucursalapo = row.CreateCell(69); cellSucursalapo.SetCellValue(beneficiario.Apoderado.SucursalApoderado.Detalle);
                        var cellFecsolcuentaapo = row.CreateCell(70); cellFecsolcuentaapo.SetCellValue(beneficiario.Apoderado.FechaSolicitudCuenta == null ?
                            String.Empty : beneficiario.Apoderado.FechaSolicitudCuenta.Value.ToShortDateString());
                        var cellFecnac = row.CreateCell(71); cellFecnac.SetCellValue(beneficiario.Apoderado.FechaNacimiento == null ? String.Empty :
                            beneficiario.Apoderado.FechaNacimiento.Value.ToShortDateString());
                        var cellCalleapo = row.CreateCell(72); cellCalleapo.SetCellValue(beneficiario.Apoderado.Calle);
                        var cellNroapo = row.CreateCell(73); cellNroapo.SetCellValue(beneficiario.Apoderado.Numero);
                        var cellPisoapo = row.CreateCell(74); cellPisoapo.SetCellValue(beneficiario.Apoderado.Piso);
                        var cellMonoblockapo = row.CreateCell(75); cellMonoblockapo.SetCellValue(beneficiario.Apoderado.Monoblock);
                        var cellDptoapo = row.CreateCell(76); cellDptoapo.SetCellValue(beneficiario.Apoderado.Dpto);
                        var cellParcelaapo = row.CreateCell(77); cellParcelaapo.SetCellValue(beneficiario.Apoderado.Parcela);
                        var cellManzanaapo = row.CreateCell(78); cellManzanaapo.SetCellValue(beneficiario.Apoderado.Manzana);
                        var cellEntrecalleapo = row.CreateCell(79); cellEntrecalleapo.SetCellValue(beneficiario.Apoderado.EntreCalles);
                        var cellBarrioapo = row.CreateCell(80); cellBarrioapo.SetCellValue(beneficiario.Apoderado.Barrio);
                        var cellCpapo = row.CreateCell(81); cellCpapo.SetCellValue(beneficiario.Apoderado.CodigoPostal);
                        var cellIdlocalidadapo = row.CreateCell(82); cellIdlocalidadapo.SetCellValue(beneficiario.Apoderado.IdLocalidad == null ?
                            String.Empty : beneficiario.Apoderado.LocalidadApoderado.IdLocalidad.ToString());
                        var cellLocalidadapo = row.CreateCell(83); cellLocalidadapo.SetCellValue(beneficiario.Apoderado.LocalidadApoderado.NombreLocalidad);
                        var cellIddeptoapo = row.CreateCell(84); cellIddeptoapo.SetCellValue(beneficiario.Apoderado.IdLocalidad == null ?
                            String.Empty : beneficiario.Apoderado.LocalidadApoderado.Departamento.IdDepartamento.ToString());
                        var cellDeptoapo = row.CreateCell(85); cellDeptoapo.SetCellValue(beneficiario.Apoderado.LocalidadApoderado.Departamento.NombreDepartamento);
                        var cellTelefonoapo = row.CreateCell(86); cellTelefonoapo.SetCellValue(beneficiario.Apoderado.TelefonoFijo);
                        var cellCelularapo = row.CreateCell(87); cellCelularapo.SetCellValue(beneficiario.Apoderado.TelefonoCelular);
                        var cellEmailapo = row.CreateCell(88); cellEmailapo.SetCellValue(beneficiario.Apoderado.Mail);
                    }
                    #endregion
                    break;
                case (int)Enums.TipoFicha.Ppp:
                    #region case (int)Enums.TipoFicha.Ppp
                    foreach (var beneficiario in listaparaExcel)
                    {
                        i++;
                        //Ficha
                        var row = sheet.CreateRow(i);
                        var cellidficha = row.CreateCell(0); cellidficha.SetCellValue(beneficiario.Ficha.IdFicha);
                        var cellApellido = row.CreateCell(1); cellApellido.SetCellValue(beneficiario.Ficha.Apellido);
                        var cellNombre = row.CreateCell(2); cellNombre.SetCellValue(beneficiario.Ficha.Nombre);
                        var cellCuil = row.CreateCell(3); cellCuil.SetCellValue(beneficiario.Ficha.Cuil);
                        var cellDni = row.CreateCell(4); cellDni.SetCellValue(beneficiario.Ficha.NumeroDocumento);
                        var cellFechaNa = row.CreateCell(5); cellFechaNa.SetCellValue(beneficiario.Ficha.FechaNacimiento == null ? String.Empty
                            : beneficiario.Ficha.FechaNacimiento.Value.ToShortDateString());
                        var cellCalle = row.CreateCell(6); cellCalle.SetCellValue(beneficiario.Ficha.Calle);
                        var cellNro = row.CreateCell(7); cellNro.SetCellValue(beneficiario.Ficha.Numero);
                        var cellPiso = row.CreateCell(8); cellPiso.SetCellValue(beneficiario.Ficha.Piso);
                        var cellDpto = row.CreateCell(9); cellDpto.SetCellValue(beneficiario.Ficha.Dpto);
                        var cellMono = row.CreateCell(10); cellMono.SetCellValue(beneficiario.Ficha.Monoblock);
                        var cellParcela = row.CreateCell(11); cellParcela.SetCellValue(beneficiario.Ficha.Parcela);
                        var cellManzana = row.CreateCell(12); cellManzana.SetCellValue(beneficiario.Ficha.Manzana);
                        var cellEntreCalle = row.CreateCell(13); cellEntreCalle.SetCellValue(beneficiario.Ficha.EntreCalles);
                        var cellBarrio = row.CreateCell(14); cellBarrio.SetCellValue(beneficiario.Ficha.Barrio);
                        var cellCp = row.CreateCell(15); cellCp.SetCellValue(beneficiario.Ficha.CodigoPostal);
                        var cellidLocalidad = row.CreateCell(16); cellidLocalidad.SetCellValue(beneficiario.Ficha.IdLocalidad == null ?
                            String.Empty : beneficiario.Ficha.LocalidadFicha.IdLocalidad.ToString());
                        var cellnLocalidad = row.CreateCell(17); cellnLocalidad.SetCellValue(beneficiario.Ficha.LocalidadFicha.NombreLocalidad);
                        var cellIdDepartamento = row.CreateCell(18); cellIdDepartamento.SetCellValue(beneficiario.Ficha.IdLocalidad == null ?
                            String.Empty : beneficiario.Ficha.LocalidadFicha.Departamento.IdDepartamento.ToString());
                        var cellnDepartamento = row.CreateCell(19); cellnDepartamento.SetCellValue(beneficiario.Ficha.LocalidadFicha.Departamento.NombreDepartamento);
                        var cellTelFijo = row.CreateCell(20); cellTelFijo.SetCellValue(beneficiario.Ficha.TelefonoFijo);
                        var cellTelCelular = row.CreateCell(21); cellTelCelular.SetCellValue(beneficiario.Ficha.TelefonoCelular);
                        var cellContacto = row.CreateCell(22); cellContacto.SetCellValue(beneficiario.Ficha.Contacto);
                        var cellEmail = row.CreateCell(23); cellEmail.SetCellValue(beneficiario.Ficha.Mail);

                        var cellTieneHijos = row.CreateCell(24); cellTieneHijos.SetCellValue(beneficiario.Ficha.TieneHijos);//TIENE_HIJOS	
                        var cellCantidadHijos = row.CreateCell(25); cellCantidadHijos.SetCellValue(beneficiario.Ficha.CantidadHijos == null ? "0" : beneficiario.Ficha.CantidadHijos.ToString());//CANTIDAD_HIJOS
                        var cellEsDiscapacitado = row.CreateCell(26); cellEsDiscapacitado.SetCellValue(beneficiario.Ficha.EsDiscapacitado == true ? "S" : "N");//ES_DIACAPACITADO
                        var cellTieneDeficienciaMotora = row.CreateCell(27); cellTieneDeficienciaMotora.SetCellValue(beneficiario.Ficha.TieneDeficienciaMotora == true ? "S" : "N");//TIENE_DEF_MOTORA
                        var cellTieneDeficienciaMental = row.CreateCell(28); cellTieneDeficienciaMental.SetCellValue(beneficiario.Ficha.TieneDeficienciaMental == true ? "S" : "N");//TIENE_DEF_MENTAL
                        var cellTieneDeficienciaSensorial = row.CreateCell(29); cellTieneDeficienciaSensorial.SetCellValue(beneficiario.Ficha.TieneDeficienciaSensorial == true ? "S" : "N");//TIENE_DEF_SENSORIAL
                        var cellTieneDeficienciaPsicologia = row.CreateCell(30); cellTieneDeficienciaPsicologia.SetCellValue(beneficiario.Ficha.TieneDeficienciaPsicologia == true ? "S" : "N");//TIENE_DEF_PSICOLOGICA
                        var cellDeficienciaOtra = row.CreateCell(31); cellDeficienciaOtra.SetCellValue(beneficiario.Ficha.DeficienciaOtra);//DEFICIENCIA_OTRA
                        var cellCertificadoDiscapacidad = row.CreateCell(32); cellCertificadoDiscapacidad.SetCellValue(beneficiario.Ficha.CertificadoDiscapacidad == true ? "S" : "N");//CERTIF_DISCAP



                        var cellSexo = row.CreateCell(33); cellSexo.SetCellValue(beneficiario.Ficha.Sexo);

                        var cellTipoFicha = row.CreateCell(34); cellTipoFicha.SetCellValue(beneficiario.Ficha.TipoFicha.ToString() ?? "0");//TIPO_FICHA
                        var cellEstadoCivilDesc = row.CreateCell(35); cellEstadoCivilDesc.SetCellValue(beneficiario.Ficha.EstadoCivilDesc);//ESTADO_CIVIL
                        var cellFechaSistema = row.CreateCell(36); cellFechaSistema.SetCellValue(beneficiario.Ficha.FechaSistema == null ? String.Empty : beneficiario.Ficha.FechaSistema.Value.ToString());//FECH_SIST
                        var cellOrigenCarga = row.CreateCell(37); cellOrigenCarga.SetCellValue(beneficiario.Ficha.OrigenCarga == "M" ? "Cargappp" : "Web");//ORIGEN_CARGA
                        var cellNroTramite = row.CreateCell(38); cellNroTramite.SetCellValue(beneficiario.Ficha.NroTramite ?? "");//NRO_TRAMITE
                        var cellidCaja = row.CreateCell(39); cellidCaja.SetCellValue(beneficiario.Ficha.idCaja == null ? "" : beneficiario.Ficha.idCaja.ToString());//ID_CAJA,
                        var cellIdTipoRechazo = row.CreateCell(40); cellIdTipoRechazo.SetCellValue(beneficiario.Ficha.IdTipoRechazo == null ? "" : beneficiario.Ficha.IdTipoRechazo.ToString());//ID_TIPO_RECHAZO,
                        var cellrechazo = row.CreateCell(41); cellrechazo.SetCellValue(beneficiario.Ficha.rechazo ?? "");//.N_TIPO_RECHAZO,

                        var cellidestadoFicha = row.CreateCell(42); cellidestadoFicha.SetCellValue(beneficiario.Ficha.IdEstadoFicha == null ? String.Empty :
                            beneficiario.Ficha.IdEstadoFicha.ToString());
                        var cellnestadoFicha = row.CreateCell(43); cellnestadoFicha.SetCellValue(beneficiario.Ficha.NombreEstadoFicha);
                        var cellModalidad = row.CreateCell(44); cellModalidad.SetCellValue(beneficiario.Ficha.FichaPpp.Modalidad == (int)Enums.Modalidad.Entrenamiento
                            ? "Entrenamiento" : "CTI");
                        var cellTarea = row.CreateCell(45); cellTarea.SetCellValue(beneficiario.Ficha.FichaPpp.Tareas);
                        var cellaltaTemprana = row.CreateCell(46); cellaltaTemprana.SetCellValue(beneficiario.Ficha.FichaPpp.AltaTemprana);
                        var cellatFechaInicio = row.CreateCell(47); cellatFechaInicio.SetCellValue(beneficiario.Ficha.FichaPpp.FechaInicioActividad == null ?
                            String.Empty : beneficiario.Ficha.FichaPpp.FechaInicioActividad.Value.ToShortDateString());
                        var cellatFechaCese = row.CreateCell(48); cellatFechaCese.SetCellValue(beneficiario.Ficha.FichaPpp.FechaFinActividad == null ?
                            String.Empty : beneficiario.Ficha.FichaPpp.FechaFinActividad.Value.ToShortDateString());
                        var cellidModContAfip = row.CreateCell(49); cellidModContAfip.SetCellValue(beneficiario.Ficha.FichaPpp.IdModalidadAfip == null ?
                            String.Empty : beneficiario.Ficha.FichaPpp.IdModalidadAfip.ToString());
                        var cellModContAfip = row.CreateCell(50); cellModContAfip.SetCellValue(beneficiario.Ficha.FichaPpp.ModalidadAfip);

                        var cellFechaModif = row.CreateCell(51); cellFechaModif.SetCellValue(beneficiario.Ficha.FechaSistema == null ? String.Empty
                            : beneficiario.Ficha.FechaSistema.Value.ToShortDateString());

                        //Empresa
                        var cellidEmp = row.CreateCell(52); cellidEmp.SetCellValue(beneficiario.Ficha.FichaPpp.Empresa.IdEmpresa);
                        var cellrazonSocial = row.CreateCell(53); cellrazonSocial.SetCellValue(beneficiario.Ficha.FichaPpp.Empresa.NombreEmpresa);
                        var cellCuit = row.CreateCell(54); cellCuit.SetCellValue(beneficiario.Ficha.FichaPpp.Empresa.Cuit);
                        var cellCodact = row.CreateCell(55); cellCodact.SetCellValue(beneficiario.Ficha.FichaPpp.Empresa.CodigoActividad);
                        var cellCantemp = row.CreateCell(56); cellCantemp.SetCellValue(beneficiario.Ficha.FichaPpp.Empresa.CantidadEmpleados == null ?
                            String.Empty : beneficiario.Ficha.FichaPpp.Empresa.CantidadEmpleados.ToString());
                        var cellCalleemp = row.CreateCell(57); cellCalleemp.SetCellValue(beneficiario.Ficha.FichaPpp.Empresa.Calle);
                        var cellNumeroemp = row.CreateCell(58); cellNumeroemp.SetCellValue(beneficiario.Ficha.FichaPpp.Empresa.Numero);
                        var cellPisoemp = row.CreateCell(59); cellPisoemp.SetCellValue(beneficiario.Ficha.FichaPpp.Empresa.Piso);
                        var cellDptoemp = row.CreateCell(60); cellDptoemp.SetCellValue(beneficiario.Ficha.FichaPpp.Empresa.Dpto);
                        var cellCpemp = row.CreateCell(61); cellCpemp.SetCellValue(beneficiario.Ficha.FichaPpp.Empresa.CodigoPostal);
                        var cellIdlocemp = row.CreateCell(62); cellIdlocemp.SetCellValue(beneficiario.Ficha.FichaPpp.Empresa.IdLocalidad == null ?
                            String.Empty : beneficiario.Ficha.FichaPpp.Empresa.LocalidadEmpresa.IdLocalidad.ToString());
                        var cellLocemp = row.CreateCell(63); cellLocemp.SetCellValue(beneficiario.Ficha.FichaPpp.Empresa.LocalidadEmpresa.NombreLocalidad);
                        var cellDepartamentoEmp = row.CreateCell(64); cellDepartamentoEmp.SetCellValue(beneficiario.Ficha.FichaPpp.Empresa.IdLocalidad == null ?
                            String.Empty : beneficiario.Ficha.FichaPpp.Empresa.LocalidadEmpresa.Departamento.NombreDepartamento);
                        var cellIdusuarioemp = row.CreateCell(65); cellIdusuarioemp.SetCellValue(beneficiario.Ficha.FichaPpp.Empresa.Usuario.IdUsuarioEmpresa == null ?
                            String.Empty : beneficiario.Ficha.FichaPpp.Empresa.Usuario.IdUsuarioEmpresa.ToString());
                        var cellNombreusuemp = row.CreateCell(66); cellNombreusuemp.SetCellValue(beneficiario.Ficha.FichaPpp.Empresa.Usuario.NombreUsuario);
                        var cellApellidousuemp = row.CreateCell(67); cellApellidousuemp.SetCellValue(beneficiario.Ficha.FichaPpp.Empresa.Usuario.ApellidoUsuario);
                        var cellEmailusuemp = row.CreateCell(68); cellEmailusuemp.SetCellValue(beneficiario.Ficha.FichaPpp.Empresa.Usuario.Mail);
                        var cellTeleusuemp = row.CreateCell(69); cellTeleusuemp.SetCellValue(beneficiario.Ficha.FichaPpp.Empresa.Usuario.Telefono);

                        //Beneficiario
                        var cellIdben = row.CreateCell(70); cellIdben.SetCellValue(beneficiario.IdBeneficiario);
                        var cellIdestadoben = row.CreateCell(71); cellIdestadoben.SetCellValue(beneficiario.IdEstado);
                        var cellEstadoben = row.CreateCell(72); cellEstadoben.SetCellValue(beneficiario.NombreEstado);
                        var cellNrocuentaben = row.CreateCell(73); cellNrocuentaben.SetCellValue(beneficiario.NumeroCuenta == null ? String.Empty
                            : beneficiario.NumeroCuenta.ToString());
                        var cellIdsucursalben = row.CreateCell(74); cellIdsucursalben.SetCellValue(beneficiario.IdSucursal == null ? String.Empty
                            : beneficiario.IdSucursal.ToString());
                        var cellCodsucursalben = row.CreateCell(75); cellCodsucursalben.SetCellValue(beneficiario.CodigoSucursal);
                        var cellSucursalben = row.CreateCell(76); cellSucursalben.SetCellValue(beneficiario.SucursalDescripcion);
                        var cellFecsolben = row.CreateCell(77); cellFecsolben.SetCellValue(beneficiario.FechaSolicitudCuenta == null ? String.Empty
                            : beneficiario.FechaSolicitudCuenta.Value.ToShortDateString());
                        var cellNotificado = row.CreateCell(78); cellNotificado.SetCellValue(beneficiario.Notificado ? "S" : "N");
                        var cellFecNotif = row.CreateCell(79); cellFecNotif.SetCellValue(beneficiario.FechaNotificacion == null ? String.Empty
                            : beneficiario.FechaNotificacion.Value.ToShortDateString());

                        var cellFechaIniBenf = row.CreateCell(80); cellFechaIniBenf.SetCellValue(beneficiario.FechaInicioBeneficio == null ? string.Empty : beneficiario.FechaInicioBeneficio.Value.ToShortDateString()); // 20/03/2013 - DI CAMPLI LEANDRO
                        var cellBajaBenef = row.CreateCell(81); cellBajaBenef.SetCellValue(beneficiario.FechaBajaBeneficio == null ? string.Empty : beneficiario.FechaBajaBeneficio.Value.ToShortDateString()); // 20/03/2013 - DI CAMPLI LEANDRO

                        var cellTieneapoben = row.CreateCell(82); cellTieneapoben.SetCellValue(beneficiario.TieneApoderado);

                        //Apoderado
                        var cellIdapoderado = row.CreateCell(83); cellIdapoderado.SetCellValue(beneficiario.Apoderado.IdApoderadoNullable == null ?
                            String.Empty : beneficiario.Apoderado.IdApoderadoNullable.ToString());
                        var cellApellidoapo = row.CreateCell(84); cellApellidoapo.SetCellValue(beneficiario.Apoderado.Apellido);
                        var cellNombreapo = row.CreateCell(85); cellNombreapo.SetCellValue(beneficiario.Apoderado.Nombre);
                        var cellDniapo = row.CreateCell(86); cellDniapo.SetCellValue(beneficiario.Apoderado.NumeroDocumento);
                        var cellCuilapo = row.CreateCell(87); cellCuilapo.SetCellValue(beneficiario.Apoderado.Cuil);
                        var cellSexoapo = row.CreateCell(88); cellSexoapo.SetCellValue(beneficiario.Apoderado.Sexo);
                        var cellNroctaapo = row.CreateCell(89); cellNroctaapo.SetCellValue(beneficiario.Apoderado.NumeroCuentaBco == null ? String.Empty
                            : beneficiario.Apoderado.NumeroCuentaBco.ToString());
                        var cellIdSuc = row.CreateCell(90); cellIdSuc.SetCellValue(beneficiario.Apoderado.IdSucursal == null ? String.Empty
                            : beneficiario.Apoderado.SucursalApoderado.IdSucursal.ToString());
                        var cellCodsucapo = row.CreateCell(91); cellCodsucapo.SetCellValue(beneficiario.Apoderado.SucursalApoderado.CodigoBanco);
                        var cellSucursalapo = row.CreateCell(92); cellSucursalapo.SetCellValue(beneficiario.Apoderado.SucursalApoderado.Detalle);
                        var cellFecsolcuentaapo = row.CreateCell(93); cellFecsolcuentaapo.SetCellValue(beneficiario.Apoderado.FechaSolicitudCuenta == null ?
                            String.Empty : beneficiario.Apoderado.FechaSolicitudCuenta.Value.ToShortDateString());
                        var cellFecnac = row.CreateCell(94); cellFecnac.SetCellValue(beneficiario.Apoderado.FechaNacimiento == null ? String.Empty :
                            beneficiario.Apoderado.FechaNacimiento.Value.ToShortDateString());
                        var cellCalleapo = row.CreateCell(95); cellCalleapo.SetCellValue(beneficiario.Apoderado.Calle);
                        var cellNroapo = row.CreateCell(96); cellNroapo.SetCellValue(beneficiario.Apoderado.Numero);
                        var cellPisoapo = row.CreateCell(97); cellPisoapo.SetCellValue(beneficiario.Apoderado.Piso);
                        var cellMonoblockapo = row.CreateCell(98); cellMonoblockapo.SetCellValue(beneficiario.Apoderado.Monoblock);
                        var cellDptoapo = row.CreateCell(99); cellDptoapo.SetCellValue(beneficiario.Apoderado.Dpto);
                        var cellParcelaapo = row.CreateCell(100); cellParcelaapo.SetCellValue(beneficiario.Apoderado.Parcela);
                        var cellManzanaapo = row.CreateCell(101); cellManzanaapo.SetCellValue(beneficiario.Apoderado.Manzana);
                        var cellEntrecalleapo = row.CreateCell(102); cellEntrecalleapo.SetCellValue(beneficiario.Apoderado.EntreCalles);
                        var cellBarrioapo = row.CreateCell(103); cellBarrioapo.SetCellValue(beneficiario.Apoderado.Barrio);
                        var cellCpapo = row.CreateCell(105); cellCpapo.SetCellValue(beneficiario.Apoderado.CodigoPostal);
                        var cellIdlocalidadapo = row.CreateCell(105); cellIdlocalidadapo.SetCellValue(beneficiario.Apoderado.IdLocalidad == null ? String.Empty
                            : beneficiario.Apoderado.LocalidadApoderado.IdLocalidad.ToString());
                        var cellLocalidadapo = row.CreateCell(106); cellLocalidadapo.SetCellValue(beneficiario.Apoderado.LocalidadApoderado.NombreLocalidad);
                        var cellIddeptoapo = row.CreateCell(107); cellIddeptoapo.SetCellValue(beneficiario.Apoderado.IdLocalidad == null ? String.Empty
                            : beneficiario.Apoderado.LocalidadApoderado.Departamento.IdDepartamento.ToString());
                        var cellDeptoapo = row.CreateCell(108); cellDeptoapo.SetCellValue(beneficiario.Apoderado.LocalidadApoderado.Departamento.NombreDepartamento);
                        var cellTelefonoapo = row.CreateCell(109); cellTelefonoapo.SetCellValue(beneficiario.Apoderado.TelefonoFijo);
                        var cellCelularapo = row.CreateCell(110); cellCelularapo.SetCellValue(beneficiario.Apoderado.TelefonoCelular);
                        var cellEmailapo = row.CreateCell(111);
                        cellEmailapo.SetCellValue(beneficiario.Apoderado.Mail);
                    }
                    #endregion
                    break;
                case (int)Enums.TipoFicha.PppProf:
                    #region case (int)Enums.TipoFicha.PppProf
                    foreach (var beneficiario in listaparaExcel)
                    {
                        i++;
                        //Ficha
                        var row = sheet.CreateRow(i);
                        var cellidficha = row.CreateCell(0); cellidficha.SetCellValue(beneficiario.Ficha.IdFicha);
                        var cellApellido = row.CreateCell(1); cellApellido.SetCellValue(beneficiario.Ficha.Apellido);
                        var cellNombre = row.CreateCell(2); cellNombre.SetCellValue(beneficiario.Ficha.Nombre);
                        var cellCuil = row.CreateCell(3); cellCuil.SetCellValue(beneficiario.Ficha.Cuil);
                        var cellDni = row.CreateCell(4); cellDni.SetCellValue(beneficiario.Ficha.NumeroDocumento);
                        var cellFechaNa = row.CreateCell(5); cellFechaNa.SetCellValue(beneficiario.Ficha.FechaNacimiento == null ? String.Empty
                            : beneficiario.Ficha.FechaNacimiento.Value.ToShortDateString());
                        var cellCalle = row.CreateCell(6); cellCalle.SetCellValue(beneficiario.Ficha.Calle);
                        var cellNro = row.CreateCell(7); cellNro.SetCellValue(beneficiario.Ficha.Numero);
                        var cellPiso = row.CreateCell(8); cellPiso.SetCellValue(beneficiario.Ficha.Piso);
                        var cellDpto = row.CreateCell(9); cellDpto.SetCellValue(beneficiario.Ficha.Dpto);
                        var cellMono = row.CreateCell(10); cellMono.SetCellValue(beneficiario.Ficha.Monoblock);
                        var cellParcela = row.CreateCell(11); cellParcela.SetCellValue(beneficiario.Ficha.Parcela);
                        var cellManzana = row.CreateCell(12); cellManzana.SetCellValue(beneficiario.Ficha.Manzana);
                        var cellEntreCalle = row.CreateCell(13); cellEntreCalle.SetCellValue(beneficiario.Ficha.EntreCalles);
                        var cellBarrio = row.CreateCell(14); cellBarrio.SetCellValue(beneficiario.Ficha.Barrio);
                        var cellCp = row.CreateCell(15); cellCp.SetCellValue(beneficiario.Ficha.CodigoPostal);
                        var cellidLocalidad = row.CreateCell(16); cellidLocalidad.SetCellValue(beneficiario.Ficha.IdLocalidad == null ? String.Empty
                            : beneficiario.Ficha.LocalidadFicha.IdLocalidad.ToString());
                        var cellnLocalidad = row.CreateCell(17); cellnLocalidad.SetCellValue(beneficiario.Ficha.LocalidadFicha.NombreLocalidad);
                        var cellIdDepartamento = row.CreateCell(18); cellIdDepartamento.SetCellValue(beneficiario.Ficha.IdLocalidad == null ? String.Empty
                            : beneficiario.Ficha.LocalidadFicha.Departamento.IdDepartamento.ToString());
                        var cellnDepartamento = row.CreateCell(19); cellnDepartamento.SetCellValue(beneficiario.Ficha.LocalidadFicha.Departamento.NombreDepartamento);
                        var cellTelFijo = row.CreateCell(20); cellTelFijo.SetCellValue(beneficiario.Ficha.TelefonoFijo);
                        var cellTelCelular = row.CreateCell(21); cellTelCelular.SetCellValue(beneficiario.Ficha.TelefonoCelular);
                        var cellContacto = row.CreateCell(22); cellContacto.SetCellValue(beneficiario.Ficha.Contacto);
                        var cellEmail = row.CreateCell(23); cellEmail.SetCellValue(beneficiario.Ficha.Mail);
                        var cellSexo = row.CreateCell(24); cellSexo.SetCellValue(beneficiario.Ficha.Sexo);
                        var cellidestadoFicha = row.CreateCell(25); cellidestadoFicha.SetCellValue(beneficiario.Ficha.IdEstadoFicha == null ? String.Empty
                            : beneficiario.Ficha.IdEstadoFicha.ToString());
                        var cellnestadoFicha = row.CreateCell(26); cellnestadoFicha.SetCellValue(beneficiario.Ficha.NombreEstadoFicha);
                        var cellModalidad = row.CreateCell(27); cellModalidad.SetCellValue(beneficiario.Ficha.FichaPppp.Modalidad == (int)Enums.Modalidad.Entrenamiento
                            ? "Entrenamiento" : "CTI");
                        var cellTarea = row.CreateCell(28); cellTarea.SetCellValue(beneficiario.Ficha.FichaPppp.Tareas);
                        var cellaltaTemprana = row.CreateCell(29); cellaltaTemprana.SetCellValue(beneficiario.Ficha.FichaPppp.AltaTemprana);
                        var cellatFechaInicio = row.CreateCell(30); cellatFechaInicio.SetCellValue(beneficiario.Ficha.FichaPppp.FechaInicioActividad == null ?
                            String.Empty : beneficiario.Ficha.FichaPppp.FechaInicioActividad.Value.ToShortDateString());
                        var cellatFechaCese = row.CreateCell(31); cellatFechaCese.SetCellValue(beneficiario.Ficha.FichaPppp.FechaFinActividad == null ?
                            String.Empty : beneficiario.Ficha.FichaPppp.FechaFinActividad.Value.ToShortDateString());
                        var cellidModContAfip = row.CreateCell(32); cellidModContAfip.SetCellValue(beneficiario.Ficha.FichaPppp.IdModalidadAFIP == null ?
                            String.Empty : beneficiario.Ficha.FichaPppp.IdModalidadAFIP.ToString());
                        var cellModContAfip = row.CreateCell(33); cellModContAfip.SetCellValue(beneficiario.Ficha.FichaPppp.ModalidadAFIP);
                        var cellFechaModif = row.CreateCell(34); cellFechaModif.SetCellValue(beneficiario.Ficha.FechaSistema == null ? String.Empty
                            : beneficiario.Ficha.FechaSistema.Value.ToShortDateString());

                        //Empresa
                        var cellidEmp = row.CreateCell(35); cellidEmp.SetCellValue(beneficiario.Ficha.FichaPppp.Empresa.IdEmpresa);
                        var cellrazonSocial = row.CreateCell(36); cellrazonSocial.SetCellValue(beneficiario.Ficha.FichaPppp.Empresa.NombreEmpresa);
                        var cellCuit = row.CreateCell(37); cellCuit.SetCellValue(beneficiario.Ficha.FichaPppp.Empresa.Cuit);
                        var cellCodact = row.CreateCell(38); cellCodact.SetCellValue(beneficiario.Ficha.FichaPppp.Empresa.CodigoActividad);
                        var cellCantemp = row.CreateCell(39); cellCantemp.SetCellValue(beneficiario.Ficha.FichaPppp.Empresa.CantidadEmpleados == null ?
                            String.Empty : beneficiario.Ficha.FichaPppp.Empresa.CantidadEmpleados.ToString());
                        var cellCalleemp = row.CreateCell(40); cellCalleemp.SetCellValue(beneficiario.Ficha.FichaPppp.Empresa.Calle);
                        var cellNumeroemp = row.CreateCell(41); cellNumeroemp.SetCellValue(beneficiario.Ficha.FichaPppp.Empresa.Numero);
                        var cellPisoemp = row.CreateCell(42); cellPisoemp.SetCellValue(beneficiario.Ficha.FichaPppp.Empresa.Piso);
                        var cellDptoemp = row.CreateCell(42); cellDptoemp.SetCellValue(beneficiario.Ficha.FichaPppp.Empresa.Dpto);
                        var cellCpemp = row.CreateCell(44); cellCpemp.SetCellValue(beneficiario.Ficha.FichaPppp.Empresa.CodigoPostal);
                        var cellIdlocemp = row.CreateCell(45); cellIdlocemp.SetCellValue(beneficiario.Ficha.FichaPppp.Empresa.IdLocalidad == null ? String.Empty
                            : beneficiario.Ficha.FichaPppp.Empresa.LocalidadEmpresa.IdLocalidad.ToString());
                        var cellLocemp = row.CreateCell(46); cellLocemp.SetCellValue(beneficiario.Ficha.FichaPppp.Empresa.LocalidadEmpresa.NombreLocalidad);
                        var cellDepartEmp = row.CreateCell(47); cellDepartEmp.SetCellValue(beneficiario.Ficha.FichaPppp.Empresa.IdLocalidad == null ? String.Empty
                            : beneficiario.Ficha.FichaPppp.Empresa.LocalidadEmpresa.Departamento.NombreDepartamento);
                        var cellIdusuarioemp = row.CreateCell(48); cellIdusuarioemp.SetCellValue(beneficiario.Ficha.FichaPppp.Empresa.IdUsuario == null ?
                            String.Empty : beneficiario.Ficha.FichaPppp.Empresa.Usuario.IdUsuarioEmpresa.ToString());
                        var cellNombreusuemp = row.CreateCell(49); cellNombreusuemp.SetCellValue(beneficiario.Ficha.FichaPppp.Empresa.Usuario.NombreUsuario);
                        var cellApellidousuemp = row.CreateCell(50); cellApellidousuemp.SetCellValue(beneficiario.Ficha.FichaPppp.Empresa.Usuario.ApellidoUsuario);
                        var cellEmailusuemp = row.CreateCell(51); cellEmailusuemp.SetCellValue(beneficiario.Ficha.FichaPppp.Empresa.Usuario.Mail);
                        var cellTeleusuemp = row.CreateCell(52); cellTeleusuemp.SetCellValue(beneficiario.Ficha.FichaPppp.Empresa.Usuario.Telefono);

                        //Beneficiario
                        var cellIdben = row.CreateCell(53); cellIdben.SetCellValue(beneficiario.IdBeneficiario);
                        var cellIdestadoben = row.CreateCell(54); cellIdestadoben.SetCellValue(beneficiario.IdEstado);
                        var cellEstadoben = row.CreateCell(55); cellEstadoben.SetCellValue(beneficiario.NombreEstado);
                        var cellNrocuentaben = row.CreateCell(56); cellNrocuentaben.SetCellValue(beneficiario.NumeroCuenta == null ? String.Empty
                            : beneficiario.NumeroCuenta.ToString());
                        var cellIdsucursalben = row.CreateCell(57); cellIdsucursalben.SetCellValue(beneficiario.IdSucursal == null ? String.Empty
                            : beneficiario.IdSucursal.ToString());
                        var cellCodsucursalben = row.CreateCell(58); cellCodsucursalben.SetCellValue(beneficiario.CodigoSucursal);
                        var cellSucursalben = row.CreateCell(59); cellSucursalben.SetCellValue(beneficiario.SucursalDescripcion);
                        var cellFecsolben = row.CreateCell(60); cellFecsolben.SetCellValue(beneficiario.FechaSolicitudCuenta == null ? String.Empty
                            : beneficiario.FechaSolicitudCuenta.Value.ToShortDateString());
                        var cellNotificado = row.CreateCell(61); cellNotificado.SetCellValue(beneficiario.Notificado ? "S" : "N");
                        var cellFecNotif = row.CreateCell(62); cellFecNotif.SetCellValue(beneficiario.FechaNotificacion == null ? String.Empty
                            : beneficiario.FechaNotificacion.Value.ToShortDateString());

                        var cellFechaIniBenf = row.CreateCell(63); cellFechaIniBenf.SetCellValue(beneficiario.FechaInicioBeneficio == null ? string.Empty : beneficiario.FechaInicioBeneficio.Value.ToShortDateString()); // 20/03/2013 - DI CAMPLI LEANDRO
                        var cellBajaBenef = row.CreateCell(64); cellBajaBenef.SetCellValue(beneficiario.FechaBajaBeneficio == null ? string.Empty : beneficiario.FechaBajaBeneficio.Value.ToShortDateString()); // 20/03/2013 - DI CAMPLI LEANDRO

                        var cellTieneapoben = row.CreateCell(65); cellTieneapoben.SetCellValue(beneficiario.TieneApoderado);

                        //Apoderado
                        var cellIdapoderado = row.CreateCell(66); cellIdapoderado.SetCellValue(beneficiario.Apoderado.IdApoderadoNullable == null ? String.Empty
                            : beneficiario.Apoderado.IdApoderadoNullable.ToString());
                        var cellApellidoapo = row.CreateCell(67); cellApellidoapo.SetCellValue(beneficiario.Apoderado.Apellido);
                        var cellNombreapo = row.CreateCell(68); cellNombreapo.SetCellValue(beneficiario.Apoderado.Nombre);
                        var cellDniapo = row.CreateCell(69); cellDniapo.SetCellValue(beneficiario.Apoderado.NumeroDocumento);
                        var cellCuilapo = row.CreateCell(70); cellCuilapo.SetCellValue(beneficiario.Apoderado.Cuil);
                        var cellSexoapo = row.CreateCell(71); cellSexoapo.SetCellValue(beneficiario.Apoderado.Sexo);
                        var cellNroctaapo = row.CreateCell(72); cellNroctaapo.SetCellValue(beneficiario.Apoderado.NumeroCuentaBco == null ? String.Empty
                            : beneficiario.Apoderado.NumeroCuentaBco.ToString());
                        var cellIdSuc = row.CreateCell(73); cellIdSuc.SetCellValue(beneficiario.Apoderado.IdSucursal == null ? String.Empty
                            : beneficiario.Apoderado.SucursalApoderado.IdSucursal.ToString());
                        var cellCodsucapo = row.CreateCell(74); cellCodsucapo.SetCellValue(beneficiario.Apoderado.SucursalApoderado.CodigoBanco);
                        var cellSucursalapo = row.CreateCell(75); cellSucursalapo.SetCellValue(beneficiario.Apoderado.SucursalApoderado.Detalle);
                        var cellFecsolcuentaapo = row.CreateCell(76); cellFecsolcuentaapo.SetCellValue(beneficiario.Apoderado.FechaSolicitudCuenta == null ?
                            String.Empty : beneficiario.Apoderado.FechaSolicitudCuenta.Value.ToShortDateString());
                        var cellFecnac = row.CreateCell(77); cellFecnac.SetCellValue(beneficiario.Apoderado.FechaNacimiento == null ? String.Empty :
                            beneficiario.Apoderado.FechaNacimiento.Value.ToShortDateString());
                        var cellCalleapo = row.CreateCell(78); cellCalleapo.SetCellValue(beneficiario.Apoderado.Calle);
                        var cellNroapo = row.CreateCell(79); cellNroapo.SetCellValue(beneficiario.Apoderado.Numero);
                        var cellPisoapo = row.CreateCell(80); cellPisoapo.SetCellValue(beneficiario.Apoderado.Piso);
                        var cellMonoblockapo = row.CreateCell(81); cellMonoblockapo.SetCellValue(beneficiario.Apoderado.Monoblock);
                        var cellDptoapo = row.CreateCell(82); cellDptoapo.SetCellValue(beneficiario.Apoderado.Dpto);
                        var cellParcelaapo = row.CreateCell(83); cellParcelaapo.SetCellValue(beneficiario.Apoderado.Parcela);
                        var cellManzanaapo = row.CreateCell(84); cellManzanaapo.SetCellValue(beneficiario.Apoderado.Manzana);
                        var cellEntrecalleapo = row.CreateCell(85); cellEntrecalleapo.SetCellValue(beneficiario.Apoderado.EntreCalles);
                        var cellBarrioapo = row.CreateCell(86); cellBarrioapo.SetCellValue(beneficiario.Apoderado.Barrio);
                        var cellCpapo = row.CreateCell(87); cellCpapo.SetCellValue(beneficiario.Apoderado.CodigoPostal);
                        var cellIdlocalidadapo = row.CreateCell(88); cellIdlocalidadapo.SetCellValue(beneficiario.Apoderado.IdLocalidad == null ? String.Empty
                            : beneficiario.Apoderado.LocalidadApoderado.IdLocalidad.ToString());
                        var cellLocalidadapo = row.CreateCell(89); cellLocalidadapo.SetCellValue(beneficiario.Apoderado.LocalidadApoderado.NombreLocalidad);
                        var cellIddeptoapo = row.CreateCell(90); cellIddeptoapo.SetCellValue(beneficiario.Apoderado.IdLocalidad == null ? String.Empty
                            : beneficiario.Apoderado.LocalidadApoderado.Departamento.IdDepartamento.ToString());
                        var cellDeptoapo = row.CreateCell(91); cellDeptoapo.SetCellValue(beneficiario.Apoderado.LocalidadApoderado.Departamento.NombreDepartamento);
                        var cellTelefonoapo = row.CreateCell(92); cellTelefonoapo.SetCellValue(beneficiario.Apoderado.TelefonoFijo);
                        var cellCelularapo = row.CreateCell(93); cellCelularapo.SetCellValue(beneficiario.Apoderado.TelefonoCelular);
                        var cellEmailapo = row.CreateCell(94);
                        cellEmailapo.SetCellValue(beneficiario.Apoderado.Mail);
                    }
                    #endregion
                    break;
                case (int)Enums.TipoFicha.Vat:
                    #region case (int)Enums.TipoFicha.Vat
                    foreach (var beneficiario in listaparaExcel)
                    {
                        i++;
                        //Ficha
                        var row = sheet.CreateRow(i);
                        var cellidficha = row.CreateCell(0); cellidficha.SetCellValue(beneficiario.Ficha.IdFicha);
                        var cellApellido = row.CreateCell(1); cellApellido.SetCellValue(beneficiario.Ficha.Apellido);
                        var cellNombre = row.CreateCell(2); cellNombre.SetCellValue(beneficiario.Ficha.Nombre);
                        var cellCuil = row.CreateCell(3); cellCuil.SetCellValue(beneficiario.Ficha.Cuil);
                        var cellDni = row.CreateCell(4); cellDni.SetCellValue(beneficiario.Ficha.NumeroDocumento);
                        var cellFechaNa = row.CreateCell(5); cellFechaNa.SetCellValue(beneficiario.Ficha.FechaNacimiento == null ? String.Empty
                            : beneficiario.Ficha.FechaNacimiento.Value.ToShortDateString());
                        var cellCalle = row.CreateCell(6); cellCalle.SetCellValue(beneficiario.Ficha.Calle);
                        var cellNro = row.CreateCell(7); cellNro.SetCellValue(beneficiario.Ficha.Numero);
                        var cellPiso = row.CreateCell(8); cellPiso.SetCellValue(beneficiario.Ficha.Piso);
                        var cellDpto = row.CreateCell(9); cellDpto.SetCellValue(beneficiario.Ficha.Dpto);
                        var cellMono = row.CreateCell(10); cellMono.SetCellValue(beneficiario.Ficha.Monoblock);
                        var cellParcela = row.CreateCell(11); cellParcela.SetCellValue(beneficiario.Ficha.Parcela);
                        var cellManzana = row.CreateCell(12); cellManzana.SetCellValue(beneficiario.Ficha.Manzana);
                        var cellEntreCalle = row.CreateCell(13); cellEntreCalle.SetCellValue(beneficiario.Ficha.EntreCalles);
                        var cellBarrio = row.CreateCell(14); cellBarrio.SetCellValue(beneficiario.Ficha.Barrio);
                        var cellCp = row.CreateCell(15); cellCp.SetCellValue(beneficiario.Ficha.CodigoPostal);
                        var cellidLocalidad = row.CreateCell(16); cellidLocalidad.SetCellValue(beneficiario.Ficha.IdLocalidad == null ? String.Empty
                            : beneficiario.Ficha.LocalidadFicha.IdLocalidad.ToString());
                        var cellnLocalidad = row.CreateCell(17); cellnLocalidad.SetCellValue(beneficiario.Ficha.LocalidadFicha.NombreLocalidad);
                        var cellIdDepartamento = row.CreateCell(18); cellIdDepartamento.SetCellValue(beneficiario.Ficha.IdLocalidad == null ? String.Empty
                            : beneficiario.Ficha.LocalidadFicha.Departamento.IdDepartamento.ToString());
                        var cellnDepartamento = row.CreateCell(19); cellnDepartamento.SetCellValue(beneficiario.Ficha.LocalidadFicha.Departamento.NombreDepartamento);
                        var cellTelFijo = row.CreateCell(20); cellTelFijo.SetCellValue(beneficiario.Ficha.TelefonoFijo);
                        var cellTelCelular = row.CreateCell(21); cellTelCelular.SetCellValue(beneficiario.Ficha.TelefonoCelular);
                        var cellContacto = row.CreateCell(22); cellContacto.SetCellValue(beneficiario.Ficha.Contacto);
                        var cellEmail = row.CreateCell(23); cellEmail.SetCellValue(beneficiario.Ficha.Mail);
                        var cellSexo = row.CreateCell(24); cellSexo.SetCellValue(beneficiario.Ficha.Sexo);
                        var cellidestadoFicha = row.CreateCell(25); cellidestadoFicha.SetCellValue(beneficiario.Ficha.IdEstadoFicha == null ? String.Empty
                            : beneficiario.Ficha.IdEstadoFicha.ToString());
                        var cellnestadoFicha = row.CreateCell(26); cellnestadoFicha.SetCellValue(beneficiario.Ficha.NombreEstadoFicha);
                        var cellModalidad = row.CreateCell(27); cellModalidad.SetCellValue(beneficiario.Ficha.FichaVat.Modalidad == (int)Enums.Modalidad.Entrenamiento
                            ? "Entrenamiento" : "CTI");
                        var cellTarea = row.CreateCell(28); cellTarea.SetCellValue(beneficiario.Ficha.FichaVat.Tareas);
                        var cellaltaTemprana = row.CreateCell(29); cellaltaTemprana.SetCellValue(beneficiario.Ficha.FichaVat.AltaTemprana);
                        var cellatFechaInicio = row.CreateCell(30); cellatFechaInicio.SetCellValue(beneficiario.Ficha.FichaVat.FechaInicioActividad == null ?
                            String.Empty : beneficiario.Ficha.FichaVat.FechaInicioActividad.Value.ToShortDateString());
                        var cellatFechaCese = row.CreateCell(31); cellatFechaCese.SetCellValue(beneficiario.Ficha.FichaVat.FechaFinActividad == null ?
                            String.Empty : beneficiario.Ficha.FichaVat.FechaFinActividad.Value.ToShortDateString());
                        var cellidModContAfip = row.CreateCell(32); cellidModContAfip.SetCellValue(beneficiario.Ficha.FichaVat.IdModalidadAFIP == null ? String.Empty
                            : beneficiario.Ficha.FichaVat.IdModalidadAFIP.ToString());
                        var cellModContAfip = row.CreateCell(33); cellModContAfip.SetCellValue(beneficiario.Ficha.FichaVat.ModalidadAFIP);
                        var cellFechaModif = row.CreateCell(34); cellFechaModif.SetCellValue(beneficiario.Ficha.FechaSistema == null ? String.Empty
                            : beneficiario.Ficha.FechaSistema.Value.ToShortDateString());

                        //Empresa
                        var cellidEmp = row.CreateCell(35); cellidEmp.SetCellValue(beneficiario.Ficha.FichaVat.Empresa.IdEmpresa);
                        var cellrazonSocial = row.CreateCell(36); cellrazonSocial.SetCellValue(beneficiario.Ficha.FichaVat.Empresa.NombreEmpresa);
                        var cellCuit = row.CreateCell(37); cellCuit.SetCellValue(beneficiario.Ficha.FichaVat.Empresa.Cuit);
                        var cellCodact = row.CreateCell(38); cellCodact.SetCellValue(beneficiario.Ficha.FichaVat.Empresa.CodigoActividad);
                        var cellCantemp = row.CreateCell(39); cellCantemp.SetCellValue(beneficiario.Ficha.FichaVat.Empresa.CantidadEmpleados == null ? String.Empty
                            : beneficiario.Ficha.FichaVat.Empresa.CantidadEmpleados.ToString());
                        var cellCalleemp = row.CreateCell(40); cellCalleemp.SetCellValue(beneficiario.Ficha.FichaVat.Empresa.Calle);
                        var cellNumeroemp = row.CreateCell(41); cellNumeroemp.SetCellValue(beneficiario.Ficha.FichaVat.Empresa.Numero);
                        var cellPisoemp = row.CreateCell(42); cellPisoemp.SetCellValue(beneficiario.Ficha.FichaVat.Empresa.Piso);
                        var cellDptoemp = row.CreateCell(43); cellDptoemp.SetCellValue(beneficiario.Ficha.FichaVat.Empresa.Dpto);
                        var cellCpemp = row.CreateCell(44); cellCpemp.SetCellValue(beneficiario.Ficha.FichaVat.Empresa.CodigoPostal);
                        var cellIdlocemp = row.CreateCell(45); cellIdlocemp.SetCellValue(beneficiario.Ficha.FichaVat.Empresa.IdLocalidad == null ? String.Empty
                            : beneficiario.Ficha.FichaVat.Empresa.LocalidadEmpresa.IdLocalidad.ToString());
                        var cellLocemp = row.CreateCell(46); cellLocemp.SetCellValue(beneficiario.Ficha.FichaVat.Empresa.LocalidadEmpresa.NombreLocalidad);
                        var cellIddepemp = row.CreateCell(47); cellIddepemp.SetCellValue(beneficiario.Ficha.FichaVat.Empresa.IdLocalidad == null ? String.Empty
                            : beneficiario.Ficha.FichaVat.Empresa.LocalidadEmpresa.Departamento.NombreDepartamento);
                        var cellIdusuarioemp = row.CreateCell(48); cellIdusuarioemp.SetCellValue(beneficiario.Ficha.FichaVat.Empresa.IdUsuario == null ?
                            String.Empty : beneficiario.Ficha.FichaVat.Empresa.Usuario.IdUsuarioEmpresa.ToString());
                        var cellNombreusuemp = row.CreateCell(49); cellNombreusuemp.SetCellValue(beneficiario.Ficha.FichaVat.Empresa.Usuario.NombreUsuario);
                        var cellApellidousuemp = row.CreateCell(50); cellApellidousuemp.SetCellValue(beneficiario.Ficha.FichaVat.Empresa.Usuario.ApellidoUsuario);
                        var cellEmailusuemp = row.CreateCell(51); cellEmailusuemp.SetCellValue(beneficiario.Ficha.FichaVat.Empresa.Usuario.Mail);
                        var cellTeleusuemp = row.CreateCell(52); cellTeleusuemp.SetCellValue(beneficiario.Ficha.FichaVat.Empresa.Usuario.Telefono);

                        //Beneficiario
                        var cellIdben = row.CreateCell(53); cellIdben.SetCellValue(beneficiario.IdBeneficiario);
                        var cellIdestadoben = row.CreateCell(54); cellIdestadoben.SetCellValue(beneficiario.IdEstado);
                        var cellEstadoben = row.CreateCell(55); cellEstadoben.SetCellValue(beneficiario.NombreEstado);
                        var cellNrocuentaben = row.CreateCell(56); cellNrocuentaben.SetCellValue(beneficiario.NumeroCuenta == null ? String.Empty
                            : beneficiario.NumeroCuenta.ToString());
                        var cellIdsucursalben = row.CreateCell(57); cellIdsucursalben.SetCellValue(beneficiario.IdSucursal == null ? String.Empty
                            : beneficiario.IdSucursal.ToString());
                        var cellCodsucursalben = row.CreateCell(58); cellCodsucursalben.SetCellValue(beneficiario.CodigoSucursal);
                        var cellSucursalben = row.CreateCell(59); cellSucursalben.SetCellValue(beneficiario.SucursalDescripcion);
                        var cellFecsolben = row.CreateCell(60); cellFecsolben.SetCellValue(beneficiario.FechaSolicitudCuenta == null ? String.Empty
                            : beneficiario.FechaSolicitudCuenta.Value.ToShortDateString());
                        var cellNotificado = row.CreateCell(61); cellNotificado.SetCellValue(beneficiario.Notificado ? "S" : "N");
                        var cellFecNotif = row.CreateCell(62); cellFecNotif.SetCellValue(beneficiario.FechaNotificacion == null ? String.Empty
                            : beneficiario.FechaNotificacion.Value.ToShortDateString());

                        var cellFechaIniBenf = row.CreateCell(63); cellFechaIniBenf.SetCellValue(beneficiario.FechaInicioBeneficio == null ? string.Empty : beneficiario.FechaInicioBeneficio.Value.ToShortDateString()); // 20/03/2013 - DI CAMPLI LEANDRO
                        var cellBajaBenef = row.CreateCell(64); cellBajaBenef.SetCellValue(beneficiario.FechaBajaBeneficio == null ? string.Empty : beneficiario.FechaBajaBeneficio.Value.ToShortDateString()); // 20/03/2013 - DI CAMPLI LEANDRO

                        var cellTieneapoben = row.CreateCell(65); cellTieneapoben.SetCellValue(beneficiario.TieneApoderado);

                        //Apoderado
                        var cellIdapoderado = row.CreateCell(66); cellIdapoderado.SetCellValue(beneficiario.Apoderado.IdApoderadoNullable == null ? String.Empty
                            : beneficiario.Apoderado.IdApoderadoNullable.ToString());
                        var cellApellidoapo = row.CreateCell(67); cellApellidoapo.SetCellValue(beneficiario.Apoderado.Apellido);
                        var cellNombreapo = row.CreateCell(68); cellNombreapo.SetCellValue(beneficiario.Apoderado.Nombre);
                        var cellDniapo = row.CreateCell(69); cellDniapo.SetCellValue(beneficiario.Apoderado.NumeroDocumento);
                        var cellCuilapo = row.CreateCell(70); cellCuilapo.SetCellValue(beneficiario.Apoderado.Cuil);
                        var cellSexoapo = row.CreateCell(71); cellSexoapo.SetCellValue(beneficiario.Apoderado.Sexo);
                        var cellNroctaapo = row.CreateCell(72); cellNroctaapo.SetCellValue(beneficiario.Apoderado.NumeroCuentaBco == null ? String.Empty
                            : beneficiario.Apoderado.NumeroCuentaBco.ToString());
                        var cellIdSuc = row.CreateCell(73); cellIdSuc.SetCellValue(beneficiario.Apoderado.IdSucursal == null ? String.Empty
                            : beneficiario.Apoderado.SucursalApoderado.IdSucursal.ToString());
                        var cellCodsucapo = row.CreateCell(74); cellCodsucapo.SetCellValue(beneficiario.Apoderado.SucursalApoderado.CodigoBanco);
                        var cellSucursalapo = row.CreateCell(75); cellSucursalapo.SetCellValue(beneficiario.Apoderado.SucursalApoderado.Detalle);
                        var cellFecsolcuentaapo = row.CreateCell(76); cellFecsolcuentaapo.SetCellValue(beneficiario.Apoderado.FechaSolicitudCuenta == null ?
                            String.Empty : beneficiario.Apoderado.FechaSolicitudCuenta.Value.ToShortDateString());
                        var cellFecnac = row.CreateCell(77); cellFecnac.SetCellValue(beneficiario.Apoderado.FechaNacimiento == null ? String.Empty :
                            beneficiario.Apoderado.FechaNacimiento.Value.ToShortDateString());
                        var cellCalleapo = row.CreateCell(78); cellCalleapo.SetCellValue(beneficiario.Apoderado.Calle);
                        var cellNroapo = row.CreateCell(79); cellNroapo.SetCellValue(beneficiario.Apoderado.Numero);
                        var cellPisoapo = row.CreateCell(80); cellPisoapo.SetCellValue(beneficiario.Apoderado.Piso);
                        var cellMonoblockapo = row.CreateCell(81); cellMonoblockapo.SetCellValue(beneficiario.Apoderado.Monoblock);
                        var cellDptoapo = row.CreateCell(82); cellDptoapo.SetCellValue(beneficiario.Apoderado.Dpto);
                        var cellParcelaapo = row.CreateCell(83); cellParcelaapo.SetCellValue(beneficiario.Apoderado.Parcela);
                        var cellManzanaapo = row.CreateCell(84); cellManzanaapo.SetCellValue(beneficiario.Apoderado.Manzana);
                        var cellEntrecalleapo = row.CreateCell(85); cellEntrecalleapo.SetCellValue(beneficiario.Apoderado.EntreCalles);
                        var cellBarrioapo = row.CreateCell(86); cellBarrioapo.SetCellValue(beneficiario.Apoderado.Barrio);
                        var cellCpapo = row.CreateCell(87); cellCpapo.SetCellValue(beneficiario.Apoderado.CodigoPostal);
                        var cellIdlocalidadapo = row.CreateCell(88); cellIdlocalidadapo.SetCellValue(beneficiario.Apoderado.IdLocalidad == null ?
                            String.Empty : beneficiario.Apoderado.LocalidadApoderado.IdLocalidad.ToString());
                        var cellLocalidadapo = row.CreateCell(89); cellLocalidadapo.SetCellValue(beneficiario.Apoderado.LocalidadApoderado.NombreLocalidad);
                        var cellIddeptoapo = row.CreateCell(90); cellIddeptoapo.SetCellValue(beneficiario.Apoderado.IdLocalidad == null ?
                            String.Empty : beneficiario.Apoderado.LocalidadApoderado.Departamento.IdDepartamento.ToString());
                        var cellDeptoapo = row.CreateCell(91); cellDeptoapo.SetCellValue(beneficiario.Apoderado.LocalidadApoderado.Departamento.NombreDepartamento);
                        var cellTelefonoapo = row.CreateCell(92); cellTelefonoapo.SetCellValue(beneficiario.Apoderado.TelefonoFijo);
                        var cellCelularapo = row.CreateCell(93); cellCelularapo.SetCellValue(beneficiario.Apoderado.TelefonoCelular);
                        var cellEmailapo = row.CreateCell(94);
                        cellEmailapo.SetCellValue(beneficiario.Apoderado.Mail);
                    }
                    #endregion
                    break;
                case (int)Enums.TipoFicha.ReconversionProductiva:
                    #region case (int)Enums.TipoFicha.Reconversion
                    foreach (var beneficiario in listaparaExcel)
                    {
                        i++;
                        //Ficha
                        var row = sheet.CreateRow(i);
                        var cellidficha = row.CreateCell(0); cellidficha.SetCellValue(beneficiario.Ficha.IdFicha);
                        var cellApellido = row.CreateCell(1); cellApellido.SetCellValue(beneficiario.Ficha.Apellido);
                        var cellNombre = row.CreateCell(2); cellNombre.SetCellValue(beneficiario.Ficha.Nombre);
                        var cellCuil = row.CreateCell(3); cellCuil.SetCellValue(beneficiario.Ficha.Cuil);
                        var cellDni = row.CreateCell(4); cellDni.SetCellValue(beneficiario.Ficha.NumeroDocumento);
                        var cellFechaNa = row.CreateCell(5); cellFechaNa.SetCellValue(beneficiario.Ficha.FechaNacimiento == null ? String.Empty
                            : beneficiario.Ficha.FechaNacimiento.Value.ToShortDateString());
                        var cellCalle = row.CreateCell(6); cellCalle.SetCellValue(beneficiario.Ficha.Calle);
                        var cellNro = row.CreateCell(7); cellNro.SetCellValue(beneficiario.Ficha.Numero);
                        var cellPiso = row.CreateCell(8); cellPiso.SetCellValue(beneficiario.Ficha.Piso);
                        var cellDpto = row.CreateCell(9); cellDpto.SetCellValue(beneficiario.Ficha.Dpto);
                        var cellMono = row.CreateCell(10); cellMono.SetCellValue(beneficiario.Ficha.Monoblock);
                        var cellParcela = row.CreateCell(11); cellParcela.SetCellValue(beneficiario.Ficha.Parcela);
                        var cellManzana = row.CreateCell(12); cellManzana.SetCellValue(beneficiario.Ficha.Manzana);
                        var cellEntreCalle = row.CreateCell(13); cellEntreCalle.SetCellValue(beneficiario.Ficha.EntreCalles);
                        var cellBarrio = row.CreateCell(14); cellBarrio.SetCellValue(beneficiario.Ficha.Barrio);
                        var cellCp = row.CreateCell(15); cellCp.SetCellValue(beneficiario.Ficha.CodigoPostal);
                        var cellidLocalidad = row.CreateCell(16); cellidLocalidad.SetCellValue(beneficiario.Ficha.IdLocalidad == null ?
                            String.Empty : beneficiario.Ficha.LocalidadFicha.IdLocalidad.ToString());
                        var cellnLocalidad = row.CreateCell(17); cellnLocalidad.SetCellValue(beneficiario.Ficha.LocalidadFicha.NombreLocalidad);
                        var cellIdDepartamento = row.CreateCell(18); cellIdDepartamento.SetCellValue(beneficiario.Ficha.IdLocalidad == null ?
                            String.Empty : beneficiario.Ficha.LocalidadFicha.Departamento.IdDepartamento.ToString());
                        var cellnDepartamento = row.CreateCell(19); cellnDepartamento.SetCellValue(beneficiario.Ficha.LocalidadFicha.Departamento.NombreDepartamento);
                        var cellTelFijo = row.CreateCell(20); cellTelFijo.SetCellValue(beneficiario.Ficha.TelefonoFijo);
                        var cellTelCelular = row.CreateCell(21); cellTelCelular.SetCellValue(beneficiario.Ficha.TelefonoCelular);
                        var cellContacto = row.CreateCell(22); cellContacto.SetCellValue(beneficiario.Ficha.Contacto);
                        var cellEmail = row.CreateCell(23); cellEmail.SetCellValue(beneficiario.Ficha.Mail);
                        var cellSexo = row.CreateCell(24); cellSexo.SetCellValue(beneficiario.Ficha.Sexo);
                        var cellidestadoFicha = row.CreateCell(25); cellidestadoFicha.SetCellValue(beneficiario.Ficha.IdEstadoFicha == null ? String.Empty :
                            beneficiario.Ficha.IdEstadoFicha.ToString());
                        var cellnestadoFicha = row.CreateCell(26); cellnestadoFicha.SetCellValue(beneficiario.Ficha.NombreEstadoFicha);
                        var cellModalidad = row.CreateCell(27); cellModalidad.SetCellValue(beneficiario.Ficha.FichaReconversion.Modalidad == (int)Enums.Modalidad.Entrenamiento
                            ? "Entrenamiento" : "CTI");
                        var cellaltaTemprana = row.CreateCell(28); cellaltaTemprana.SetCellValue(beneficiario.Ficha.FichaReconversion.AltaTemprana);
                        var cellatFechaInicio = row.CreateCell(29); cellatFechaInicio.SetCellValue(beneficiario.Ficha.FichaReconversion.FechaInicioActividad == null ?
                            String.Empty : beneficiario.Ficha.FichaReconversion.FechaInicioActividad.Value.ToShortDateString());
                        var cellatFechaCese = row.CreateCell(30); cellatFechaCese.SetCellValue(beneficiario.Ficha.FichaReconversion.FechaFinActividad == null ?
                            String.Empty : beneficiario.Ficha.FichaReconversion.FechaFinActividad.Value.ToShortDateString());
                        var cellidModContAfip = row.CreateCell(31); cellidModContAfip.SetCellValue(beneficiario.Ficha.FichaReconversion.IdModalidadAfip == null ?
                            String.Empty : beneficiario.Ficha.FichaReconversion.IdModalidadAfip.ToString());
                        var cellModContAfip = row.CreateCell(32); cellModContAfip.SetCellValue(beneficiario.Ficha.FichaReconversion.ModalidadAfip);

                        var cellFechaModif = row.CreateCell(33); cellFechaModif.SetCellValue(beneficiario.Ficha.FechaSistema == null ? String.Empty
                            : beneficiario.Ficha.FechaSistema.Value.ToShortDateString());

                        var cellnSubprograma = row.CreateCell(34); cellnSubprograma.SetCellValue(beneficiario.Ficha.FichaReconversion.Subprograma);

                        //Proyecto
                        var cellnProyecto = row.CreateCell(35); cellnProyecto.SetCellValue(beneficiario.Ficha.FichaReconversion.Proyecto.NombreProyecto);
                        var cellCantCapacitar = row.CreateCell(36); cellCantCapacitar.SetCellValue(beneficiario.Ficha.FichaReconversion.Proyecto.CantidadAcapacitar == null ? String.Empty : beneficiario.Ficha.FichaReconversion.Proyecto.CantidadAcapacitar.ToString());
                        var cellFecInicio = row.CreateCell(37); cellFecInicio.SetCellValue(beneficiario.Ficha.FichaReconversion.Proyecto.FechaInicioProyecto == null ? String.Empty : beneficiario.Ficha.FichaReconversion.Proyecto.FechaInicioProyecto.Value.ToShortDateString());
                        var cellFecFin = row.CreateCell(38); cellFecFin.SetCellValue(beneficiario.Ficha.FichaReconversion.Proyecto.FechaFinProyecto == null ? String.Empty : beneficiario.Ficha.FichaReconversion.Proyecto.FechaFinProyecto.Value.ToShortDateString());
                        var cellMesesDuracion = row.CreateCell(39); cellMesesDuracion.SetCellValue(beneficiario.Ficha.FichaReconversion.Proyecto.MesesDuracionProyecto == null ? String.Empty : beneficiario.Ficha.FichaReconversion.Proyecto.MesesDuracionProyecto.ToString());
                        var cellAportesEmpresa = row.CreateCell(40); cellAportesEmpresa.SetCellValue(beneficiario.Ficha.FichaReconversion.AportesDeLaEmpresa);
                        var cellApeTutor = row.CreateCell(41); cellApeTutor.SetCellValue(beneficiario.Ficha.FichaReconversion.ApellidoTutor);
                        var cellNomTutor = row.CreateCell(42); cellNomTutor.SetCellValue(beneficiario.Ficha.FichaReconversion.NombreTutor);
                        var cellDocTutor = row.CreateCell(43); cellDocTutor.SetCellValue(beneficiario.Ficha.FichaReconversion.NroDocumentoTutor);
                        var cellTelTutor = row.CreateCell(44); cellTelTutor.SetCellValue(beneficiario.Ficha.FichaReconversion.TelefonoTutor);
                        var cellEmailTutor = row.CreateCell(45); cellEmailTutor.SetCellValue(beneficiario.Ficha.FichaReconversion.EmailTutor);
                        var cellPuestoTutor = row.CreateCell(46); cellPuestoTutor.SetCellValue(beneficiario.Ficha.FichaReconversion.PuestoTutor);

                        //Empresa
                        var cellidEmp = row.CreateCell(47); cellidEmp.SetCellValue(beneficiario.Ficha.FichaReconversion.Empresa.IdEmpresa);
                        var cellrazonSocial = row.CreateCell(48); cellrazonSocial.SetCellValue(beneficiario.Ficha.FichaReconversion.Empresa.NombreEmpresa);
                        var cellCuit = row.CreateCell(49); cellCuit.SetCellValue(beneficiario.Ficha.FichaReconversion.Empresa.Cuit);
                        var cellCodact = row.CreateCell(50); cellCodact.SetCellValue(beneficiario.Ficha.FichaReconversion.Empresa.CodigoActividad);
                        var cellCantemp = row.CreateCell(51); cellCantemp.SetCellValue(beneficiario.Ficha.FichaReconversion.Empresa.CantidadEmpleados == null ?
                            String.Empty : beneficiario.Ficha.FichaReconversion.Empresa.CantidadEmpleados.ToString());
                        var cellCalleemp = row.CreateCell(52); cellCalleemp.SetCellValue(beneficiario.Ficha.FichaReconversion.Empresa.Calle);
                        var cellNumeroemp = row.CreateCell(53); cellNumeroemp.SetCellValue(beneficiario.Ficha.FichaReconversion.Empresa.Numero);
                        var cellPisoemp = row.CreateCell(54); cellPisoemp.SetCellValue(beneficiario.Ficha.FichaReconversion.Empresa.Piso);
                        var cellDptoemp = row.CreateCell(55); cellDptoemp.SetCellValue(beneficiario.Ficha.FichaReconversion.Empresa.Dpto);
                        var cellCpemp = row.CreateCell(56); cellCpemp.SetCellValue(beneficiario.Ficha.FichaReconversion.Empresa.CodigoPostal);
                        var cellIdlocemp = row.CreateCell(57); cellIdlocemp.SetCellValue(beneficiario.Ficha.FichaReconversion.Empresa.IdLocalidad == null ?
                            String.Empty : beneficiario.Ficha.FichaReconversion.Empresa.LocalidadEmpresa.IdLocalidad.ToString());
                        var cellLocemp = row.CreateCell(58); cellLocemp.SetCellValue(beneficiario.Ficha.FichaReconversion.Empresa.LocalidadEmpresa.NombreLocalidad);
                        var cellDepartamentoEmp = row.CreateCell(59); cellDepartamentoEmp.SetCellValue(beneficiario.Ficha.FichaReconversion.Empresa.IdLocalidad == null ?
                            String.Empty : beneficiario.Ficha.FichaReconversion.Empresa.LocalidadEmpresa.Departamento.NombreDepartamento);
                        var cellIdusuarioemp = row.CreateCell(60); cellIdusuarioemp.SetCellValue(beneficiario.Ficha.FichaReconversion.Empresa.Usuario.IdUsuarioEmpresa == null ?
                            String.Empty : beneficiario.Ficha.FichaReconversion.Empresa.Usuario.IdUsuarioEmpresa.ToString());
                        var cellNombreusuemp = row.CreateCell(61); cellNombreusuemp.SetCellValue(beneficiario.Ficha.FichaReconversion.Empresa.Usuario.NombreUsuario);
                        var cellApellidousuemp = row.CreateCell(62); cellApellidousuemp.SetCellValue(beneficiario.Ficha.FichaReconversion.Empresa.Usuario.ApellidoUsuario);
                        var cellEmailusuemp = row.CreateCell(63); cellEmailusuemp.SetCellValue(beneficiario.Ficha.FichaReconversion.Empresa.Usuario.Mail);
                        var cellTeleusuemp = row.CreateCell(64); cellTeleusuemp.SetCellValue(beneficiario.Ficha.FichaReconversion.Empresa.Usuario.Telefono);

                        //Sede
                        var cellIdSede = row.CreateCell(65); cellIdSede.SetCellValue(beneficiario.Ficha.FichaReconversion.IdSede == null ? String.Empty : beneficiario.Ficha.FichaReconversion.IdSede.ToString());
                        var cellnSede = row.CreateCell(66); cellnSede.SetCellValue(beneficiario.Ficha.FichaReconversion.Sede.NombreSede);
                        var cellCalleSede = row.CreateCell(67); cellCalleSede.SetCellValue(beneficiario.Ficha.FichaReconversion.Sede.Calle);
                        var cellNroSede = row.CreateCell(68); cellNroSede.SetCellValue(beneficiario.Ficha.FichaReconversion.Sede.Numero);
                        var cellPisoSede = row.CreateCell(69); cellPisoSede.SetCellValue(beneficiario.Ficha.FichaReconversion.Sede.Piso);
                        var cellDptoSede = row.CreateCell(70); cellDptoSede.SetCellValue(beneficiario.Ficha.FichaReconversion.Sede.Dpto);
                        var cellCpSede = row.CreateCell(71); cellCpSede.SetCellValue(beneficiario.Ficha.FichaReconversion.Sede.CodigoPostal);
                        var cellLocSede = row.CreateCell(72); cellLocSede.SetCellValue(beneficiario.Ficha.FichaReconversion.Sede.NombreLocalidad);
                        var cellApeContacto = row.CreateCell(73); cellApeContacto.SetCellValue(beneficiario.Ficha.FichaReconversion.Sede.ApellidoContacto);
                        var cellNomContacto = row.CreateCell(74); cellNomContacto.SetCellValue(beneficiario.Ficha.FichaReconversion.Sede.NombreContacto);
                        var cellTelcontacto = row.CreateCell(75); cellTelcontacto.SetCellValue(beneficiario.Ficha.FichaReconversion.Sede.Telefono);
                        var cellFaxContacto = row.CreateCell(76); cellFaxContacto.SetCellValue(beneficiario.Ficha.FichaReconversion.Sede.Fax);
                        var cellEmailContacto = row.CreateCell(77); cellEmailContacto.SetCellValue(beneficiario.Ficha.FichaReconversion.Sede.Email);

                        //Beneficiario
                        var cellIdben = row.CreateCell(78); cellIdben.SetCellValue(beneficiario.IdBeneficiario);
                        var cellIdestadoben = row.CreateCell(79); cellIdestadoben.SetCellValue(beneficiario.IdEstado);
                        var cellEstadoben = row.CreateCell(80); cellEstadoben.SetCellValue(beneficiario.NombreEstado);
                        var cellNrocuentaben = row.CreateCell(81); cellNrocuentaben.SetCellValue(beneficiario.NumeroCuenta == null ? String.Empty
                            : beneficiario.NumeroCuenta.ToString());
                        var cellIdsucursalben = row.CreateCell(82); cellIdsucursalben.SetCellValue(beneficiario.IdSucursal == null ? String.Empty
                            : beneficiario.IdSucursal.ToString());
                        var cellCodsucursalben = row.CreateCell(83); cellCodsucursalben.SetCellValue(beneficiario.CodigoSucursal);
                        var cellSucursalben = row.CreateCell(84); cellSucursalben.SetCellValue(beneficiario.SucursalDescripcion);
                        var cellFecsolben = row.CreateCell(85); cellFecsolben.SetCellValue(beneficiario.FechaSolicitudCuenta == null ? String.Empty
                            : beneficiario.FechaSolicitudCuenta.Value.ToShortDateString());
                        var cellNotificado = row.CreateCell(86); cellNotificado.SetCellValue(beneficiario.Notificado ? "S" : "N");
                        var cellFecNotif = row.CreateCell(87); cellFecNotif.SetCellValue(beneficiario.FechaNotificacion == null ? String.Empty
                            : beneficiario.FechaNotificacion.Value.ToShortDateString());

                        var cellFechaIniBenf = row.CreateCell(88); cellFechaIniBenf.SetCellValue(beneficiario.FechaInicioBeneficio == null ? string.Empty : beneficiario.FechaInicioBeneficio.Value.ToShortDateString()); // 20/03/2013 - DI CAMPLI LEANDRO
                        var cellBajaBenef = row.CreateCell(89); cellBajaBenef.SetCellValue(beneficiario.FechaBajaBeneficio == null ? string.Empty : beneficiario.FechaBajaBeneficio.Value.ToShortDateString()); // 20/03/2013 - DI CAMPLI LEANDRO

                        var cellTieneapoben = row.CreateCell(90); cellTieneapoben.SetCellValue(beneficiario.TieneApoderado);

                        //Apoderado
                        var cellIdapoderado = row.CreateCell(91); cellIdapoderado.SetCellValue(beneficiario.Apoderado.IdApoderadoNullable == null ?
                            String.Empty : beneficiario.Apoderado.IdApoderadoNullable.ToString());
                        var cellApellidoapo = row.CreateCell(92); cellApellidoapo.SetCellValue(beneficiario.Apoderado.Apellido);
                        var cellNombreapo = row.CreateCell(93); cellNombreapo.SetCellValue(beneficiario.Apoderado.Nombre);
                        var cellDniapo = row.CreateCell(94); cellDniapo.SetCellValue(beneficiario.Apoderado.NumeroDocumento);
                        var cellCuilapo = row.CreateCell(95); cellCuilapo.SetCellValue(beneficiario.Apoderado.Cuil);
                        var cellSexoapo = row.CreateCell(96); cellSexoapo.SetCellValue(beneficiario.Apoderado.Sexo);
                        var cellNroctaapo = row.CreateCell(97); cellNroctaapo.SetCellValue(beneficiario.Apoderado.NumeroCuentaBco == null ? String.Empty
                            : beneficiario.Apoderado.NumeroCuentaBco.ToString());
                        var cellIdSuc = row.CreateCell(98); cellIdSuc.SetCellValue(beneficiario.Apoderado.IdSucursal == null ? String.Empty
                            : beneficiario.Apoderado.SucursalApoderado.IdSucursal.ToString());
                        var cellCodsucapo = row.CreateCell(99); cellCodsucapo.SetCellValue(beneficiario.Apoderado.SucursalApoderado.CodigoBanco);
                        var cellSucursalapo = row.CreateCell(100); cellSucursalapo.SetCellValue(beneficiario.Apoderado.SucursalApoderado.Detalle);
                        var cellFecsolcuentaapo = row.CreateCell(101); cellFecsolcuentaapo.SetCellValue(beneficiario.Apoderado.FechaSolicitudCuenta == null ?
                            String.Empty : beneficiario.Apoderado.FechaSolicitudCuenta.Value.ToShortDateString());
                        var cellFecnac = row.CreateCell(102); cellFecnac.SetCellValue(beneficiario.Apoderado.FechaNacimiento == null ? String.Empty :
                            beneficiario.Apoderado.FechaNacimiento.Value.ToShortDateString());
                        var cellCalleapo = row.CreateCell(103); cellCalleapo.SetCellValue(beneficiario.Apoderado.Calle);
                        var cellNroapo = row.CreateCell(104); cellNroapo.SetCellValue(beneficiario.Apoderado.Numero);
                        var cellPisoapo = row.CreateCell(105); cellPisoapo.SetCellValue(beneficiario.Apoderado.Piso);
                        var cellMonoblockapo = row.CreateCell(106); cellMonoblockapo.SetCellValue(beneficiario.Apoderado.Monoblock);
                        var cellDptoapo = row.CreateCell(107); cellDptoapo.SetCellValue(beneficiario.Apoderado.Dpto);
                        var cellParcelaapo = row.CreateCell(108); cellParcelaapo.SetCellValue(beneficiario.Apoderado.Parcela);
                        var cellManzanaapo = row.CreateCell(109); cellManzanaapo.SetCellValue(beneficiario.Apoderado.Manzana);
                        var cellEntrecalleapo = row.CreateCell(110); cellEntrecalleapo.SetCellValue(beneficiario.Apoderado.EntreCalles);
                        var cellBarrioapo = row.CreateCell(111); cellBarrioapo.SetCellValue(beneficiario.Apoderado.Barrio);
                        var cellCpapo = row.CreateCell(112); cellCpapo.SetCellValue(beneficiario.Apoderado.CodigoPostal);
                        var cellIdlocalidadapo = row.CreateCell(113); cellIdlocalidadapo.SetCellValue(beneficiario.Apoderado.IdLocalidad == null ? String.Empty
                            : beneficiario.Apoderado.LocalidadApoderado.IdLocalidad.ToString());
                        var cellLocalidadapo = row.CreateCell(114); cellLocalidadapo.SetCellValue(beneficiario.Apoderado.LocalidadApoderado.NombreLocalidad);
                        var cellIddeptoapo = row.CreateCell(115); cellIddeptoapo.SetCellValue(beneficiario.Apoderado.IdLocalidad == null ? String.Empty
                            : beneficiario.Apoderado.LocalidadApoderado.Departamento.IdDepartamento.ToString());
                        var cellDeptoapo = row.CreateCell(116); cellDeptoapo.SetCellValue(beneficiario.Apoderado.LocalidadApoderado.Departamento.NombreDepartamento);
                        var cellTelefonoapo = row.CreateCell(117); cellTelefonoapo.SetCellValue(beneficiario.Apoderado.TelefonoFijo);
                        var cellCelularapo = row.CreateCell(118); cellCelularapo.SetCellValue(beneficiario.Apoderado.TelefonoCelular);
                        var cellEmailapo = row.CreateCell(119);
                        cellEmailapo.SetCellValue(beneficiario.Apoderado.Mail);
                    }
                    #endregion
                    break;
                case (int)Enums.TipoFicha.EfectoresSociales:
                    #region case (int)Enums.TipoFicha.EfectoresSociales
                    foreach (var beneficiario in listaparaExcel)
                    {
                        i++;
                        //Ficha
                        var row = sheet.CreateRow(i);
                        var cellidficha = row.CreateCell(0); cellidficha.SetCellValue(beneficiario.Ficha.IdFicha);
                        var cellApellido = row.CreateCell(1); cellApellido.SetCellValue(beneficiario.Ficha.Apellido);
                        var cellNombre = row.CreateCell(2); cellNombre.SetCellValue(beneficiario.Ficha.Nombre);
                        var cellCuil = row.CreateCell(3); cellCuil.SetCellValue(beneficiario.Ficha.Cuil);
                        var cellDni = row.CreateCell(4); cellDni.SetCellValue(beneficiario.Ficha.NumeroDocumento);
                        var cellFechaNa = row.CreateCell(5); cellFechaNa.SetCellValue(beneficiario.Ficha.FechaNacimiento == null ? String.Empty
                            : beneficiario.Ficha.FechaNacimiento.Value.ToShortDateString());
                        var cellCalle = row.CreateCell(6); cellCalle.SetCellValue(beneficiario.Ficha.Calle);
                        var cellNro = row.CreateCell(7); cellNro.SetCellValue(beneficiario.Ficha.Numero);
                        var cellPiso = row.CreateCell(8); cellPiso.SetCellValue(beneficiario.Ficha.Piso);
                        var cellDpto = row.CreateCell(9); cellDpto.SetCellValue(beneficiario.Ficha.Dpto);
                        var cellMono = row.CreateCell(10); cellMono.SetCellValue(beneficiario.Ficha.Monoblock);
                        var cellParcela = row.CreateCell(11); cellParcela.SetCellValue(beneficiario.Ficha.Parcela);
                        var cellManzana = row.CreateCell(12); cellManzana.SetCellValue(beneficiario.Ficha.Manzana);
                        var cellEntreCalle = row.CreateCell(13); cellEntreCalle.SetCellValue(beneficiario.Ficha.EntreCalles);
                        var cellBarrio = row.CreateCell(14); cellBarrio.SetCellValue(beneficiario.Ficha.Barrio);
                        var cellCp = row.CreateCell(15); cellCp.SetCellValue(beneficiario.Ficha.CodigoPostal);
                        var cellidLocalidad = row.CreateCell(16); cellidLocalidad.SetCellValue(beneficiario.Ficha.IdLocalidad == null ?
                            String.Empty : beneficiario.Ficha.LocalidadFicha.IdLocalidad.ToString());
                        var cellnLocalidad = row.CreateCell(17); cellnLocalidad.SetCellValue(beneficiario.Ficha.LocalidadFicha.NombreLocalidad);
                        var cellIdDepartamento = row.CreateCell(18); cellIdDepartamento.SetCellValue(beneficiario.Ficha.IdLocalidad == null ?
                            String.Empty : beneficiario.Ficha.LocalidadFicha.Departamento.IdDepartamento.ToString());
                        var cellnDepartamento = row.CreateCell(19); cellnDepartamento.SetCellValue(beneficiario.Ficha.LocalidadFicha.Departamento.NombreDepartamento);
                        var cellTelFijo = row.CreateCell(20); cellTelFijo.SetCellValue(beneficiario.Ficha.TelefonoFijo);
                        var cellTelCelular = row.CreateCell(21); cellTelCelular.SetCellValue(beneficiario.Ficha.TelefonoCelular);
                        var cellContacto = row.CreateCell(22); cellContacto.SetCellValue(beneficiario.Ficha.Contacto);
                        var cellEmail = row.CreateCell(23); cellEmail.SetCellValue(beneficiario.Ficha.Mail);
                        var cellSexo = row.CreateCell(24); cellSexo.SetCellValue(beneficiario.Ficha.Sexo);
                        var cellidestadoFicha = row.CreateCell(25); cellidestadoFicha.SetCellValue(beneficiario.Ficha.IdEstadoFicha == null ? String.Empty :
                            beneficiario.Ficha.IdEstadoFicha.ToString());
                        var cellnestadoFicha = row.CreateCell(26); cellnestadoFicha.SetCellValue(beneficiario.Ficha.NombreEstadoFicha);
                        var cellModalidad = row.CreateCell(27); cellModalidad.SetCellValue(beneficiario.Ficha.FichaEfectores.Modalidad == (int)Enums.Modalidad.Entrenamiento
                            ? "Entrenamiento" : "CTI");
                        var cellaltaTemprana = row.CreateCell(28); cellaltaTemprana.SetCellValue(beneficiario.Ficha.FichaEfectores.AltaTemprana);
                        var cellatFechaInicio = row.CreateCell(29); cellatFechaInicio.SetCellValue(beneficiario.Ficha.FichaEfectores.FechaInicioActividad == null ?
                            String.Empty : beneficiario.Ficha.FichaEfectores.FechaInicioActividad.Value.ToShortDateString());
                        var cellatFechaCese = row.CreateCell(30); cellatFechaCese.SetCellValue(beneficiario.Ficha.FichaEfectores.FechaFinActividad == null ?
                            String.Empty : beneficiario.Ficha.FichaEfectores.FechaFinActividad.Value.ToShortDateString());
                        var cellidModContAfip = row.CreateCell(31); cellidModContAfip.SetCellValue(beneficiario.Ficha.FichaEfectores.IdModalidadAfip == null ?
                            String.Empty : beneficiario.Ficha.FichaEfectores.IdModalidadAfip.ToString());
                        var cellModContAfip = row.CreateCell(32); cellModContAfip.SetCellValue(beneficiario.Ficha.FichaEfectores.ModalidadAfip);

                        var cellFechaModif = row.CreateCell(33); cellFechaModif.SetCellValue(beneficiario.Ficha.FechaSistema == null ? String.Empty
                            : beneficiario.Ficha.FechaSistema.Value.ToShortDateString());

                        var cellnSubprograma = row.CreateCell(34); cellnSubprograma.SetCellValue(beneficiario.Ficha.FichaEfectores.Subprograma);

                        //Proyecto
                        var cellnProyecto = row.CreateCell(35); cellnProyecto.SetCellValue(beneficiario.Ficha.FichaEfectores.Proyecto.NombreProyecto);
                        var cellCantCapacitar = row.CreateCell(36); cellCantCapacitar.SetCellValue(beneficiario.Ficha.FichaEfectores.Proyecto.CantidadAcapacitar == null ? String.Empty : beneficiario.Ficha.FichaEfectores.Proyecto.CantidadAcapacitar.ToString());
                        var cellFecInicio = row.CreateCell(37); cellFecInicio.SetCellValue(beneficiario.Ficha.FichaEfectores.Proyecto.FechaInicioProyecto == null ? String.Empty : beneficiario.Ficha.FichaEfectores.Proyecto.FechaInicioProyecto.Value.ToShortDateString());
                        var cellFecFin = row.CreateCell(38); cellFecFin.SetCellValue(beneficiario.Ficha.FichaEfectores.Proyecto.FechaFinProyecto == null ? String.Empty : beneficiario.Ficha.FichaEfectores.Proyecto.FechaFinProyecto.Value.ToShortDateString());
                        var cellMesesDuracion = row.CreateCell(39); cellMesesDuracion.SetCellValue(beneficiario.Ficha.FichaEfectores.Proyecto.MesesDuracionProyecto == null ? String.Empty : beneficiario.Ficha.FichaEfectores.Proyecto.MesesDuracionProyecto.ToString());
                        var cellAportesEmpresa = row.CreateCell(40); cellAportesEmpresa.SetCellValue(beneficiario.Ficha.FichaEfectores.AportesDeLaEmpresa);
                        var cellApeTutor = row.CreateCell(41); cellApeTutor.SetCellValue(beneficiario.Ficha.FichaEfectores.ApellidoTutor);
                        var cellNomTutor = row.CreateCell(42); cellNomTutor.SetCellValue(beneficiario.Ficha.FichaEfectores.NombreTutor);
                        var cellDocTutor = row.CreateCell(43); cellDocTutor.SetCellValue(beneficiario.Ficha.FichaEfectores.NroDocumentoTutor);
                        var cellTelTutor = row.CreateCell(44); cellTelTutor.SetCellValue(beneficiario.Ficha.FichaEfectores.TelefonoTutor);
                        var cellEmailTutor = row.CreateCell(45); cellEmailTutor.SetCellValue(beneficiario.Ficha.FichaEfectores.EmailTutor);
                        var cellPuestoTutor = row.CreateCell(46); cellPuestoTutor.SetCellValue(beneficiario.Ficha.FichaEfectores.PuestoTutor);

                        //Empresa
                        var cellidEmp = row.CreateCell(47); cellidEmp.SetCellValue(beneficiario.Ficha.FichaEfectores.Empresa.IdEmpresa);
                        var cellrazonSocial = row.CreateCell(48); cellrazonSocial.SetCellValue(beneficiario.Ficha.FichaEfectores.Empresa.NombreEmpresa);
                        var cellCuit = row.CreateCell(49); cellCuit.SetCellValue(beneficiario.Ficha.FichaEfectores.Empresa.Cuit);
                        var cellCodact = row.CreateCell(50); cellCodact.SetCellValue(beneficiario.Ficha.FichaEfectores.Empresa.CodigoActividad);
                        var cellCantemp = row.CreateCell(51); cellCantemp.SetCellValue(beneficiario.Ficha.FichaEfectores.Empresa.CantidadEmpleados == null ?
                            String.Empty : beneficiario.Ficha.FichaEfectores.Empresa.CantidadEmpleados.ToString());
                        var cellCalleemp = row.CreateCell(52); cellCalleemp.SetCellValue(beneficiario.Ficha.FichaEfectores.Empresa.Calle);
                        var cellNumeroemp = row.CreateCell(53); cellNumeroemp.SetCellValue(beneficiario.Ficha.FichaEfectores.Empresa.Numero);
                        var cellPisoemp = row.CreateCell(54); cellPisoemp.SetCellValue(beneficiario.Ficha.FichaEfectores.Empresa.Piso);
                        var cellDptoemp = row.CreateCell(55); cellDptoemp.SetCellValue(beneficiario.Ficha.FichaEfectores.Empresa.Dpto);
                        var cellCpemp = row.CreateCell(56); cellCpemp.SetCellValue(beneficiario.Ficha.FichaEfectores.Empresa.CodigoPostal);
                        var cellIdlocemp = row.CreateCell(57); cellIdlocemp.SetCellValue(beneficiario.Ficha.FichaEfectores.Empresa.IdLocalidad == null ?
                            String.Empty : beneficiario.Ficha.FichaEfectores.Empresa.LocalidadEmpresa.IdLocalidad.ToString());
                        var cellLocemp = row.CreateCell(58); cellLocemp.SetCellValue(beneficiario.Ficha.FichaEfectores.Empresa.LocalidadEmpresa.NombreLocalidad);
                        var cellDepartamentoEmp = row.CreateCell(59); cellDepartamentoEmp.SetCellValue(beneficiario.Ficha.FichaEfectores.Empresa.IdLocalidad == null ?
                            String.Empty : beneficiario.Ficha.FichaEfectores.Empresa.LocalidadEmpresa.Departamento.NombreDepartamento);
                        var cellIdusuarioemp = row.CreateCell(60); cellIdusuarioemp.SetCellValue(beneficiario.Ficha.FichaEfectores.Empresa.Usuario.IdUsuarioEmpresa == null ?
                            String.Empty : beneficiario.Ficha.FichaEfectores.Empresa.Usuario.IdUsuarioEmpresa.ToString());
                        var cellNombreusuemp = row.CreateCell(61); cellNombreusuemp.SetCellValue(beneficiario.Ficha.FichaEfectores.Empresa.Usuario.NombreUsuario);
                        var cellApellidousuemp = row.CreateCell(62); cellApellidousuemp.SetCellValue(beneficiario.Ficha.FichaEfectores.Empresa.Usuario.ApellidoUsuario);
                        var cellEmailusuemp = row.CreateCell(63); cellEmailusuemp.SetCellValue(beneficiario.Ficha.FichaEfectores.Empresa.Usuario.Mail);
                        var cellTeleusuemp = row.CreateCell(64); cellTeleusuemp.SetCellValue(beneficiario.Ficha.FichaEfectores.Empresa.Usuario.Telefono);

                        //Sede
                        var cellIdSede = row.CreateCell(65); cellIdSede.SetCellValue(beneficiario.Ficha.FichaEfectores.IdSede == null ? String.Empty : beneficiario.Ficha.FichaEfectores.IdSede.ToString());
                        var cellnSede = row.CreateCell(66); cellnSede.SetCellValue(beneficiario.Ficha.FichaEfectores.Sede.NombreSede);
                        var cellCalleSede = row.CreateCell(67); cellCalleSede.SetCellValue(beneficiario.Ficha.FichaEfectores.Sede.Calle);
                        var cellNroSede = row.CreateCell(68); cellNroSede.SetCellValue(beneficiario.Ficha.FichaEfectores.Sede.Numero);
                        var cellPisoSede = row.CreateCell(69); cellPisoSede.SetCellValue(beneficiario.Ficha.FichaEfectores.Sede.Piso);
                        var cellDptoSede = row.CreateCell(70); cellDptoSede.SetCellValue(beneficiario.Ficha.FichaEfectores.Sede.Dpto);
                        var cellCpSede = row.CreateCell(71); cellCpSede.SetCellValue(beneficiario.Ficha.FichaEfectores.Sede.CodigoPostal);
                        var cellLocSede = row.CreateCell(72); cellLocSede.SetCellValue(beneficiario.Ficha.FichaEfectores.Sede.NombreLocalidad);
                        var cellApeContacto = row.CreateCell(73); cellApeContacto.SetCellValue(beneficiario.Ficha.FichaEfectores.Sede.ApellidoContacto);
                        var cellNomContacto = row.CreateCell(74); cellNomContacto.SetCellValue(beneficiario.Ficha.FichaEfectores.Sede.NombreContacto);
                        var cellTelcontacto = row.CreateCell(75); cellTelcontacto.SetCellValue(beneficiario.Ficha.FichaEfectores.Sede.Telefono);
                        var cellFaxContacto = row.CreateCell(76); cellFaxContacto.SetCellValue(beneficiario.Ficha.FichaEfectores.Sede.Fax);
                        var cellEmailContacto = row.CreateCell(77); cellEmailContacto.SetCellValue(beneficiario.Ficha.FichaEfectores.Sede.Email);

                        //Beneficiario
                        var cellIdben = row.CreateCell(78); cellIdben.SetCellValue(beneficiario.IdBeneficiario);
                        var cellIdestadoben = row.CreateCell(79); cellIdestadoben.SetCellValue(beneficiario.IdEstado);
                        var cellEstadoben = row.CreateCell(80); cellEstadoben.SetCellValue(beneficiario.NombreEstado);
                        var cellNrocuentaben = row.CreateCell(81); cellNrocuentaben.SetCellValue(beneficiario.NumeroCuenta == null ? String.Empty
                            : beneficiario.NumeroCuenta.ToString());
                        var cellIdsucursalben = row.CreateCell(82); cellIdsucursalben.SetCellValue(beneficiario.IdSucursal == null ? String.Empty
                            : beneficiario.IdSucursal.ToString());
                        var cellCodsucursalben = row.CreateCell(83); cellCodsucursalben.SetCellValue(beneficiario.CodigoSucursal);
                        var cellSucursalben = row.CreateCell(84); cellSucursalben.SetCellValue(beneficiario.SucursalDescripcion);
                        var cellFecsolben = row.CreateCell(85); cellFecsolben.SetCellValue(beneficiario.FechaSolicitudCuenta == null ? String.Empty
                            : beneficiario.FechaSolicitudCuenta.Value.ToShortDateString());
                        var cellNotificado = row.CreateCell(86); cellNotificado.SetCellValue(beneficiario.Notificado ? "S" : "N");
                        var cellFecNotif = row.CreateCell(87); cellFecNotif.SetCellValue(beneficiario.FechaNotificacion == null ? String.Empty
                            : beneficiario.FechaNotificacion.Value.ToShortDateString());

                        var cellFechaIniBenf = row.CreateCell(88); cellFechaIniBenf.SetCellValue(beneficiario.FechaInicioBeneficio == null ? string.Empty : beneficiario.FechaInicioBeneficio.Value.ToShortDateString()); // 20/03/2013 - DI CAMPLI LEANDRO
                        var cellBajaBenef = row.CreateCell(89); cellBajaBenef.SetCellValue(beneficiario.FechaBajaBeneficio == null ? string.Empty : beneficiario.FechaBajaBeneficio.Value.ToShortDateString()); // 20/03/2013 - DI CAMPLI LEANDRO


                        var cellTieneapoben = row.CreateCell(90); cellTieneapoben.SetCellValue(beneficiario.TieneApoderado);

                        //Apoderado
                        var cellIdapoderado = row.CreateCell(91); cellIdapoderado.SetCellValue(beneficiario.Apoderado.IdApoderadoNullable == null ?
                            String.Empty : beneficiario.Apoderado.IdApoderadoNullable.ToString());
                        var cellApellidoapo = row.CreateCell(92); cellApellidoapo.SetCellValue(beneficiario.Apoderado.Apellido);
                        var cellNombreapo = row.CreateCell(93); cellNombreapo.SetCellValue(beneficiario.Apoderado.Nombre);
                        var cellDniapo = row.CreateCell(94); cellDniapo.SetCellValue(beneficiario.Apoderado.NumeroDocumento);
                        var cellCuilapo = row.CreateCell(95); cellCuilapo.SetCellValue(beneficiario.Apoderado.Cuil);
                        var cellSexoapo = row.CreateCell(96); cellSexoapo.SetCellValue(beneficiario.Apoderado.Sexo);
                        var cellNroctaapo = row.CreateCell(97); cellNroctaapo.SetCellValue(beneficiario.Apoderado.NumeroCuentaBco == null ? String.Empty
                            : beneficiario.Apoderado.NumeroCuentaBco.ToString());
                        var cellIdSuc = row.CreateCell(98); cellIdSuc.SetCellValue(beneficiario.Apoderado.IdSucursal == null ? String.Empty
                            : beneficiario.Apoderado.SucursalApoderado.IdSucursal.ToString());
                        var cellCodsucapo = row.CreateCell(99); cellCodsucapo.SetCellValue(beneficiario.Apoderado.SucursalApoderado.CodigoBanco);
                        var cellSucursalapo = row.CreateCell(100); cellSucursalapo.SetCellValue(beneficiario.Apoderado.SucursalApoderado.Detalle);
                        var cellFecsolcuentaapo = row.CreateCell(101); cellFecsolcuentaapo.SetCellValue(beneficiario.Apoderado.FechaSolicitudCuenta == null ?
                            String.Empty : beneficiario.Apoderado.FechaSolicitudCuenta.Value.ToShortDateString());
                        var cellFecnac = row.CreateCell(102); cellFecnac.SetCellValue(beneficiario.Apoderado.FechaNacimiento == null ? String.Empty :
                            beneficiario.Apoderado.FechaNacimiento.Value.ToShortDateString());
                        var cellCalleapo = row.CreateCell(103); cellCalleapo.SetCellValue(beneficiario.Apoderado.Calle);
                        var cellNroapo = row.CreateCell(104); cellNroapo.SetCellValue(beneficiario.Apoderado.Numero);
                        var cellPisoapo = row.CreateCell(105); cellPisoapo.SetCellValue(beneficiario.Apoderado.Piso);
                        var cellMonoblockapo = row.CreateCell(106); cellMonoblockapo.SetCellValue(beneficiario.Apoderado.Monoblock);
                        var cellDptoapo = row.CreateCell(107); cellDptoapo.SetCellValue(beneficiario.Apoderado.Dpto);
                        var cellParcelaapo = row.CreateCell(108); cellParcelaapo.SetCellValue(beneficiario.Apoderado.Parcela);
                        var cellManzanaapo = row.CreateCell(109); cellManzanaapo.SetCellValue(beneficiario.Apoderado.Manzana);
                        var cellEntrecalleapo = row.CreateCell(110); cellEntrecalleapo.SetCellValue(beneficiario.Apoderado.EntreCalles);
                        var cellBarrioapo = row.CreateCell(111); cellBarrioapo.SetCellValue(beneficiario.Apoderado.Barrio);
                        var cellCpapo = row.CreateCell(112); cellCpapo.SetCellValue(beneficiario.Apoderado.CodigoPostal);
                        var cellIdlocalidadapo = row.CreateCell(113); cellIdlocalidadapo.SetCellValue(beneficiario.Apoderado.IdLocalidad == null ? String.Empty
                            : beneficiario.Apoderado.LocalidadApoderado.IdLocalidad.ToString());
                        var cellLocalidadapo = row.CreateCell(114); cellLocalidadapo.SetCellValue(beneficiario.Apoderado.LocalidadApoderado.NombreLocalidad);
                        var cellIddeptoapo = row.CreateCell(115); cellIddeptoapo.SetCellValue(beneficiario.Apoderado.IdLocalidad == null ? String.Empty
                            : beneficiario.Apoderado.LocalidadApoderado.Departamento.IdDepartamento.ToString());
                        var cellDeptoapo = row.CreateCell(116); cellDeptoapo.SetCellValue(beneficiario.Apoderado.LocalidadApoderado.Departamento.NombreDepartamento);
                        var cellTelefonoapo = row.CreateCell(117); cellTelefonoapo.SetCellValue(beneficiario.Apoderado.TelefonoFijo);
                        var cellCelularapo = row.CreateCell(118); cellCelularapo.SetCellValue(beneficiario.Apoderado.TelefonoCelular);
                        var cellEmailapo = row.CreateCell(119);
                        cellEmailapo.SetCellValue(beneficiario.Apoderado.Mail);
                    }
                    #endregion
                    break;
            }

            var ms = new MemoryStream();

            templateWorkbook.Write(ms);

            return ms.ToArray();
        }

        public byte[] ExportReporteArtHorariosEntrenamientoPpp(IBeneficiariosVista vista)
        {
            string path = "";

            path = HttpContext.Current.Server.MapPath(@"\Archivos\Exportados\TemplateArtHorarios.xls");

            var fs = new FileStream(path, FileMode.Open, FileAccess.Read);

            var templateWorkbook = new HSSFWorkbook(fs, true);

            var sheet = templateWorkbook.GetSheet("Nomina");

            IList<IBeneficiario> listaparaExcel = _beneficiariorepositorio.GetBeneficiariosHorariosArt((int)Enums.Programas.Ppp, vista.FechaInicioBeneficio, vista.FechaBenefHasta);

            int i = 2;
            foreach (var beneficiario in listaparaExcel)
            {
                //int tieneHorasCargadas =_horarioficharepositorio.GetHorarioFichaByEmpresa(beneficiario.Ficha.IdFicha,beneficiario.Ficha.FichaPpp.Empresa.IdEmpresa).Count;
                //if (tieneHorasCargadas != 0)
                //{
                i++;

                //Ficha
                var row = sheet.CreateRow(i);
                var cellidficha = row.CreateCell(0);
                cellidficha.SetCellValue(beneficiario.Ficha.IdFicha);
                var cellApellido = row.CreateCell(1);
                cellApellido.SetCellValue(beneficiario.Ficha.Apellido);
                var cellNombre = row.CreateCell(2);
                cellNombre.SetCellValue(beneficiario.Ficha.Nombre);
                var cellCuil = row.CreateCell(3);
                cellCuil.SetCellValue(beneficiario.Ficha.Cuil);
                var cellDni = row.CreateCell(4);
                cellDni.SetCellValue(beneficiario.Ficha.NumeroDocumento);
                var cellFechaNa = row.CreateCell(5);
                cellFechaNa.SetCellValue(beneficiario.Ficha.FechaNacimiento.ToString());
                var cellCalle = row.CreateCell(6);
                cellCalle.SetCellValue(beneficiario.Ficha.Calle);
                var cellNro = row.CreateCell(7);
                cellNro.SetCellValue(beneficiario.Ficha.Numero);
                var cellPiso = row.CreateCell(8);
                cellPiso.SetCellValue(beneficiario.Ficha.Piso);
                var cellDpto = row.CreateCell(9);
                cellDpto.SetCellValue(beneficiario.Ficha.Dpto);
                var cellBarrio = row.CreateCell(10);
                cellBarrio.SetCellValue(beneficiario.Ficha.Barrio);
                var cellCp = row.CreateCell(11);
                cellCp.SetCellValue(beneficiario.Ficha.CodigoPostal);
                var cellidLocalidad = row.CreateCell(12);
                cellidLocalidad.SetCellValue(beneficiario.Ficha.LocalidadFicha.IdLocalidad);
                var cellnLocalidad = row.CreateCell(13);
                cellnLocalidad.SetCellValue(beneficiario.Ficha.LocalidadFicha.NombreLocalidad);
                var cellIdDepartamento = row.CreateCell(14);
                cellIdDepartamento.SetCellValue(beneficiario.Ficha.LocalidadFicha.Departamento.IdDepartamento);
                var cellnDepartamento = row.CreateCell(15);
                cellnDepartamento.SetCellValue(beneficiario.Ficha.LocalidadFicha.Departamento.NombreDepartamento);
                var celldiscapacidad = row.CreateCell(16);
                celldiscapacidad.SetCellValue(beneficiario.Notificado ? "S" : "N");

                //Empresa
                var cellrazonSocial = row.CreateCell(17);
                cellrazonSocial.SetCellValue(beneficiario.Ficha.FichaPpp.Empresa.NombreEmpresa);
                var cellCuit = row.CreateCell(18);
                cellCuit.SetCellValue(beneficiario.Ficha.FichaPpp.Empresa.Cuit);
                var cellCalleemp = row.CreateCell(19);
                cellCalleemp.SetCellValue(beneficiario.Ficha.FichaPpp.Empresa.Calle);
                var cellNumeroemp = row.CreateCell(20);
                cellNumeroemp.SetCellValue(beneficiario.Ficha.FichaPpp.Empresa.Numero);
                var cellPisoemp = row.CreateCell(21);
                cellPisoemp.SetCellValue(beneficiario.Ficha.FichaPpp.Empresa.Piso);
                var cellDptoemp = row.CreateCell(22);
                cellDptoemp.SetCellValue(beneficiario.Ficha.FichaPpp.Empresa.Dpto);
                var cellCpemp = row.CreateCell(23);
                cellCpemp.SetCellValue(beneficiario.Ficha.FichaPpp.Empresa.CodigoPostal);
                var cellIdlocemp = row.CreateCell(24);
                cellIdlocemp.SetCellValue(beneficiario.Ficha.FichaPpp.Empresa.IdLocalidad == null
                                              ? String.Empty
                                              : beneficiario.Ficha.FichaPpp.Empresa.LocalidadEmpresa.IdLocalidad.
                                                    ToString());
                var cellLocemp = row.CreateCell(25);
                cellLocemp.SetCellValue(beneficiario.Ficha.FichaPpp.Empresa.LocalidadEmpresa.NombreLocalidad);
                var cellDepartamentoEmp = row.CreateCell(26);
                cellDepartamentoEmp.SetCellValue(beneficiario.Ficha.FichaPpp.Empresa.IdLocalidad == null
                                                     ? String.Empty
                                                     : beneficiario.Ficha.FichaPpp.Empresa.LocalidadEmpresa.
                                                           Departamento.NombreDepartamento);

                //Sede
                var cellCallesede = row.CreateCell(27);
                cellCallesede.SetCellValue(beneficiario.Ficha.FichaPpp.Sede.Calle);
                var cellNumerosede = row.CreateCell(28);
                cellNumerosede.SetCellValue(beneficiario.Ficha.FichaPpp.Sede.Numero);
                var cellPisosede = row.CreateCell(29);
                cellPisosede.SetCellValue(beneficiario.Ficha.FichaPpp.Sede.Piso);
                var cellDptosede = row.CreateCell(30);
                cellDptosede.SetCellValue(beneficiario.Ficha.FichaPpp.Sede.Dpto);
                var cellCpsede = row.CreateCell(31);
                cellCpsede.SetCellValue(beneficiario.Ficha.FichaPpp.Sede.CodigoPostal);
                var cellIdlocsede = row.CreateCell(32);
                cellIdlocsede.SetCellValue(beneficiario.Ficha.FichaPpp.Sede.IdLocalidad == null
                                               ? String.Empty
                                               : beneficiario.Ficha.FichaPpp.Sede.IdLocalidad.ToString());
                var cellLocsede = row.CreateCell(33);
                cellLocsede.SetCellValue(beneficiario.Ficha.FichaPpp.Sede.NombreLocalidad);


                IList<IHorarioFicha> listahorarioficha =
                    _horarioficharepositorio.GetHorarioFichaByEmpresa(beneficiario.Ficha.IdFicha,
                                                                      beneficiario.Ficha.FichaPpp.Empresa.IdEmpresa);

                foreach (var horarioFicha in listahorarioficha)
                {
                    IList<IHorarioEmpresa> listahorarioEmpresa =
                        _horarioempresarepositorio.GetHorarioEmpresaForFicha(horarioFicha.IdEmpresaHorario);

                    foreach (var horarioempresa in listahorarioEmpresa)
                    {
                        string horario = horarioempresa.HoraDesde.ToString().PadLeft(2, '0') + ":" +
                                         horarioempresa.MinutosDesde.ToString().PadLeft(2, '0') + " - " +
                                         horarioempresa.HoraHasta.ToString().PadLeft(2, '0') + ":" +
                                         horarioempresa.MinutosHasta.ToString().PadLeft(2, '0');

                        switch (horarioFicha.DSemana)
                        {
                            case "01":
                                if (horarioFicha.Corte == 0)
                                {
                                    var cellLunes0 = row.CreateCell(34);
                                    cellLunes0.SetCellValue(horario);
                                }
                                if (horarioFicha.Corte == 1)
                                {
                                    var cellLunes1 = row.CreateCell(41);
                                    cellLunes1.SetCellValue(horario);
                                }
                                break;
                            case "02":
                                if (horarioFicha.Corte == 0)
                                {
                                    var cellMartes0 = row.CreateCell(35);
                                    cellMartes0.SetCellValue(horario);
                                }
                                if (horarioFicha.Corte == 1)
                                {
                                    var cellMartes0 = row.CreateCell(42);
                                    cellMartes0.SetCellValue(horario);
                                }
                                break;
                            case "03":
                                if (horarioFicha.Corte == 0)
                                {
                                    var cellMier0 = row.CreateCell(36);
                                    cellMier0.SetCellValue(horario);
                                }
                                if (horarioFicha.Corte == 1)
                                {
                                    var cellMier1 = row.CreateCell(43);
                                    cellMier1.SetCellValue(horario);
                                }
                                break;
                            case "04":
                                if (horarioFicha.Corte == 0)
                                {
                                    var cellJuev0 = row.CreateCell(37);
                                    cellJuev0.SetCellValue(horario);
                                }
                                if (horarioFicha.Corte == 1)
                                {
                                    var cellJuev1 = row.CreateCell(44);
                                    cellJuev1.SetCellValue(horario);
                                }
                                break;
                            case "05":
                                if (horarioFicha.Corte == 0)
                                {
                                    var cellVier0 = row.CreateCell(38);
                                    cellVier0.SetCellValue(horario);
                                }
                                if (horarioFicha.Corte == 1)
                                {
                                    var cellVier1 = row.CreateCell(45);
                                    cellVier1.SetCellValue(horario);
                                }
                                break;
                            case "06":
                                if (horarioFicha.Corte == 0)
                                {
                                    var cellSab0 = row.CreateCell(39);
                                    cellSab0.SetCellValue(horario);
                                }
                                if (horarioFicha.Corte == 1)
                                {
                                    var cellSab1 = row.CreateCell(46);
                                    cellSab1.SetCellValue(horario);
                                }
                                break;
                            case "07":
                                if (horarioFicha.Corte == 0)
                                {
                                    var cellDom0 = row.CreateCell(40);
                                    cellDom0.SetCellValue(horario);
                                }
                                if (horarioFicha.Corte == 1)
                                {
                                    var cellDom1 = row.CreateCell(47);
                                    cellDom1.SetCellValue(horario);
                                }
                                break;
                        }
                    }
                }




            }

            var ms = new MemoryStream();

            templateWorkbook.Write(ms);

            return ms.ToArray();

        }

        public byte[] ExportReporteArtHorariosEntrenamientoReconversion(IBeneficiariosVista vista)
        {
            string path = "";

            path = HttpContext.Current.Server.MapPath(@"\Archivos\Exportados\TemplateArtHorarios.xls");

            var fs = new FileStream(path, FileMode.Open, FileAccess.Read);

            var templateWorkbook = new HSSFWorkbook(fs, true);

            var sheet = templateWorkbook.GetSheet("Nomina");

            IList<IBeneficiario> listaparaExcel = _beneficiariorepositorio.GetBeneficiariosHorariosArt((int)Enums.Programas.ReconversionProductiva, vista.FechaInicioBeneficio, vista.FechaBenefHasta);

            int i = 2;
            foreach (var beneficiario in listaparaExcel)
            {
                i++;

                //Ficha
                var row = sheet.CreateRow(i);
                var cellidficha = row.CreateCell(0);
                cellidficha.SetCellValue(beneficiario.Ficha.IdFicha);
                var cellApellido = row.CreateCell(1);
                cellApellido.SetCellValue(beneficiario.Ficha.Apellido);
                var cellNombre = row.CreateCell(2);
                cellNombre.SetCellValue(beneficiario.Ficha.Nombre);
                var cellCuil = row.CreateCell(3);
                cellCuil.SetCellValue(beneficiario.Ficha.Cuil);
                var cellDni = row.CreateCell(4);
                cellDni.SetCellValue(beneficiario.Ficha.NumeroDocumento);
                var cellFechaNa = row.CreateCell(5);
                cellFechaNa.SetCellValue(beneficiario.Ficha.FechaNacimiento.ToString());
                var cellCalle = row.CreateCell(6);
                cellCalle.SetCellValue(beneficiario.Ficha.Calle);
                var cellNro = row.CreateCell(7);
                cellNro.SetCellValue(beneficiario.Ficha.Numero);
                var cellPiso = row.CreateCell(8);
                cellPiso.SetCellValue(beneficiario.Ficha.Piso);
                var cellDpto = row.CreateCell(9);
                cellDpto.SetCellValue(beneficiario.Ficha.Dpto);
                var cellBarrio = row.CreateCell(10);
                cellBarrio.SetCellValue(beneficiario.Ficha.Barrio);
                var cellCp = row.CreateCell(11);
                cellCp.SetCellValue(beneficiario.Ficha.CodigoPostal);
                var cellidLocalidad = row.CreateCell(12);
                cellidLocalidad.SetCellValue(beneficiario.Ficha.LocalidadFicha.IdLocalidad);
                var cellnLocalidad = row.CreateCell(13);
                cellnLocalidad.SetCellValue(beneficiario.Ficha.LocalidadFicha.NombreLocalidad);
                var cellIdDepartamento = row.CreateCell(14);
                cellIdDepartamento.SetCellValue(beneficiario.Ficha.LocalidadFicha.Departamento.IdDepartamento);
                var cellnDepartamento = row.CreateCell(15);
                cellnDepartamento.SetCellValue(beneficiario.Ficha.LocalidadFicha.Departamento.NombreDepartamento);
                var celldiscapacidad = row.CreateCell(16);
                celldiscapacidad.SetCellValue(beneficiario.Notificado ? "S" : "N");

                //Empresa
                var cellrazonSocial = row.CreateCell(17);
                cellrazonSocial.SetCellValue(beneficiario.Ficha.FichaReconversion.Empresa.NombreEmpresa);
                var cellCuit = row.CreateCell(18);
                cellCuit.SetCellValue(beneficiario.Ficha.FichaReconversion.Empresa.Cuit);
                var cellCalleemp = row.CreateCell(19);
                cellCalleemp.SetCellValue(beneficiario.Ficha.FichaReconversion.Empresa.Calle);
                var cellNumeroemp = row.CreateCell(20);
                cellNumeroemp.SetCellValue(beneficiario.Ficha.FichaReconversion.Empresa.Numero);
                var cellPisoemp = row.CreateCell(21);
                cellPisoemp.SetCellValue(beneficiario.Ficha.FichaReconversion.Empresa.Piso);
                var cellDptoemp = row.CreateCell(22);
                cellDptoemp.SetCellValue(beneficiario.Ficha.FichaReconversion.Empresa.Dpto);
                var cellCpemp = row.CreateCell(23);
                cellCpemp.SetCellValue(beneficiario.Ficha.FichaReconversion.Empresa.CodigoPostal);
                var cellIdlocemp = row.CreateCell(24);
                cellIdlocemp.SetCellValue(beneficiario.Ficha.FichaReconversion.Empresa.IdLocalidad == null
                                              ? String.Empty
                                              : beneficiario.Ficha.FichaReconversion.Empresa.LocalidadEmpresa.IdLocalidad.
                                                    ToString());
                var cellLocemp = row.CreateCell(25);
                cellLocemp.SetCellValue(beneficiario.Ficha.FichaReconversion.Empresa.LocalidadEmpresa.NombreLocalidad);
                var cellDepartamentoEmp = row.CreateCell(26);
                cellDepartamentoEmp.SetCellValue(beneficiario.Ficha.FichaReconversion.Empresa.IdLocalidad == null
                                                     ? String.Empty
                                                     : beneficiario.Ficha.FichaReconversion.Empresa.LocalidadEmpresa.
                                                           Departamento.NombreDepartamento);

                //Sede
                var cellCallesede = row.CreateCell(27);
                cellCallesede.SetCellValue(beneficiario.Ficha.FichaReconversion.Sede.Calle);
                var cellNumerosede = row.CreateCell(28);
                cellNumerosede.SetCellValue(beneficiario.Ficha.FichaReconversion.Sede.Numero);
                var cellPisosede = row.CreateCell(29);
                cellPisosede.SetCellValue(beneficiario.Ficha.FichaReconversion.Sede.Piso);
                var cellDptosede = row.CreateCell(30);
                cellDptosede.SetCellValue(beneficiario.Ficha.FichaReconversion.Sede.Dpto);
                var cellCpsede = row.CreateCell(31);
                cellCpsede.SetCellValue(beneficiario.Ficha.FichaReconversion.Sede.CodigoPostal);
                var cellIdlocsede = row.CreateCell(32);
                cellIdlocsede.SetCellValue(beneficiario.Ficha.FichaReconversion.Sede.IdLocalidad == null
                                               ? String.Empty
                                               : beneficiario.Ficha.FichaReconversion.Sede.IdLocalidad.ToString());
                var cellLocsede = row.CreateCell(33);
                cellLocsede.SetCellValue(beneficiario.Ficha.FichaReconversion.Sede.NombreLocalidad);


                IList<IHorarioFicha> listahorarioficha =
                    _horarioficharepositorio.GetHorarioFichaByEmpresa(beneficiario.Ficha.IdFicha,
                                                                      beneficiario.Ficha.FichaReconversion.Empresa.IdEmpresa);

                foreach (var horarioFicha in listahorarioficha)
                {
                    IList<IHorarioEmpresa> listahorarioEmpresa =
                        _horarioempresarepositorio.GetHorarioEmpresaForFicha(horarioFicha.IdEmpresaHorario);

                    foreach (var horarioempresa in listahorarioEmpresa)
                    {
                        string horario = horarioempresa.HoraDesde.ToString().PadLeft(2, '0') + ":" +
                                         horarioempresa.MinutosDesde.ToString().PadLeft(2, '0') + " - " +
                                         horarioempresa.HoraHasta.ToString().PadLeft(2, '0') + ":" +
                                         horarioempresa.MinutosHasta.ToString().PadLeft(2, '0');

                        switch (horarioFicha.DSemana)
                        {
                            case "01":
                                if (horarioFicha.Corte == 0)
                                {
                                    var cellLunes0 = row.CreateCell(34);
                                    cellLunes0.SetCellValue(horario);
                                }
                                if (horarioFicha.Corte == 1)
                                {
                                    var cellLunes1 = row.CreateCell(41);
                                    cellLunes1.SetCellValue(horario);
                                }
                                break;
                            case "02":
                                if (horarioFicha.Corte == 0)
                                {
                                    var cellMartes0 = row.CreateCell(35);
                                    cellMartes0.SetCellValue(horario);
                                }
                                if (horarioFicha.Corte == 1)
                                {
                                    var cellMartes0 = row.CreateCell(42);
                                    cellMartes0.SetCellValue(horario);
                                }
                                break;
                            case "03":
                                if (horarioFicha.Corte == 0)
                                {
                                    var cellMier0 = row.CreateCell(36);
                                    cellMier0.SetCellValue(horario);
                                }
                                if (horarioFicha.Corte == 1)
                                {
                                    var cellMier1 = row.CreateCell(43);
                                    cellMier1.SetCellValue(horario);
                                }
                                break;
                            case "04":
                                if (horarioFicha.Corte == 0)
                                {
                                    var cellJuev0 = row.CreateCell(37);
                                    cellJuev0.SetCellValue(horario);
                                }
                                if (horarioFicha.Corte == 1)
                                {
                                    var cellJuev1 = row.CreateCell(44);
                                    cellJuev1.SetCellValue(horario);
                                }
                                break;
                            case "05":
                                if (horarioFicha.Corte == 0)
                                {
                                    var cellVier0 = row.CreateCell(38);
                                    cellVier0.SetCellValue(horario);
                                }
                                if (horarioFicha.Corte == 1)
                                {
                                    var cellVier1 = row.CreateCell(45);
                                    cellVier1.SetCellValue(horario);
                                }
                                break;
                            case "06":
                                if (horarioFicha.Corte == 0)
                                {
                                    var cellSab0 = row.CreateCell(39);
                                    cellSab0.SetCellValue(horario);
                                }
                                if (horarioFicha.Corte == 1)
                                {
                                    var cellSab1 = row.CreateCell(46);
                                    cellSab1.SetCellValue(horario);
                                }
                                break;
                            case "07":
                                if (horarioFicha.Corte == 0)
                                {
                                    var cellDom0 = row.CreateCell(40);
                                    cellDom0.SetCellValue(horario);
                                }
                                if (horarioFicha.Corte == 1)
                                {
                                    var cellDom1 = row.CreateCell(47);
                                    cellDom1.SetCellValue(horario);
                                }
                                break;
                        }
                    }
                }
            }

            var ms = new MemoryStream();

            templateWorkbook.Write(ms);

            return ms.ToArray();
        }

        public byte[] ExportReporteArtHorariosEntrenamientoEfectores(IBeneficiariosVista vista)
        {
            string path = "";

            path = HttpContext.Current.Server.MapPath(@"\Archivos\Exportados\TemplateArtHorarios.xls");

            var fs = new FileStream(path, FileMode.Open, FileAccess.Read);

            var templateWorkbook = new HSSFWorkbook(fs, true);

            var sheet = templateWorkbook.GetSheet("Nomina");

            IList<IBeneficiario> listaparaExcel = _beneficiariorepositorio.GetBeneficiariosHorariosArt((int)Enums.Programas.EfectoresSociales, vista.FechaInicioBeneficio, vista.FechaBenefHasta);

            int i = 2;
            foreach (var beneficiario in listaparaExcel)
            {
                i++;

                //Ficha
                var row = sheet.CreateRow(i);
                var cellidficha = row.CreateCell(0);
                cellidficha.SetCellValue(beneficiario.Ficha.IdFicha);
                var cellApellido = row.CreateCell(1);
                cellApellido.SetCellValue(beneficiario.Ficha.Apellido);
                var cellNombre = row.CreateCell(2);
                cellNombre.SetCellValue(beneficiario.Ficha.Nombre);
                var cellCuil = row.CreateCell(3);
                cellCuil.SetCellValue(beneficiario.Ficha.Cuil);
                var cellDni = row.CreateCell(4);
                cellDni.SetCellValue(beneficiario.Ficha.NumeroDocumento);
                var cellFechaNa = row.CreateCell(5);
                cellFechaNa.SetCellValue(beneficiario.Ficha.FechaNacimiento.ToString());
                var cellCalle = row.CreateCell(6);
                cellCalle.SetCellValue(beneficiario.Ficha.Calle);
                var cellNro = row.CreateCell(7);
                cellNro.SetCellValue(beneficiario.Ficha.Numero);
                var cellPiso = row.CreateCell(8);
                cellPiso.SetCellValue(beneficiario.Ficha.Piso);
                var cellDpto = row.CreateCell(9);
                cellDpto.SetCellValue(beneficiario.Ficha.Dpto);
                var cellBarrio = row.CreateCell(10);
                cellBarrio.SetCellValue(beneficiario.Ficha.Barrio);
                var cellCp = row.CreateCell(11);
                cellCp.SetCellValue(beneficiario.Ficha.CodigoPostal);
                var cellidLocalidad = row.CreateCell(12);
                cellidLocalidad.SetCellValue(beneficiario.Ficha.LocalidadFicha.IdLocalidad);
                var cellnLocalidad = row.CreateCell(13);
                cellnLocalidad.SetCellValue(beneficiario.Ficha.LocalidadFicha.NombreLocalidad);
                var cellIdDepartamento = row.CreateCell(14);
                cellIdDepartamento.SetCellValue(beneficiario.Ficha.LocalidadFicha.Departamento.IdDepartamento);
                var cellnDepartamento = row.CreateCell(15);
                cellnDepartamento.SetCellValue(beneficiario.Ficha.LocalidadFicha.Departamento.NombreDepartamento);
                var celldiscapacidad = row.CreateCell(16);
                celldiscapacidad.SetCellValue(beneficiario.Notificado ? "S" : "N");

                //Empresa
                var cellrazonSocial = row.CreateCell(17);
                cellrazonSocial.SetCellValue(beneficiario.Ficha.FichaEfectores.Empresa.NombreEmpresa);
                var cellCuit = row.CreateCell(18);
                cellCuit.SetCellValue(beneficiario.Ficha.FichaEfectores.Empresa.Cuit);
                var cellCalleemp = row.CreateCell(19);
                cellCalleemp.SetCellValue(beneficiario.Ficha.FichaEfectores.Empresa.Calle);
                var cellNumeroemp = row.CreateCell(20);
                cellNumeroemp.SetCellValue(beneficiario.Ficha.FichaEfectores.Empresa.Numero);
                var cellPisoemp = row.CreateCell(21);
                cellPisoemp.SetCellValue(beneficiario.Ficha.FichaEfectores.Empresa.Piso);
                var cellDptoemp = row.CreateCell(22);
                cellDptoemp.SetCellValue(beneficiario.Ficha.FichaEfectores.Empresa.Dpto);
                var cellCpemp = row.CreateCell(23);
                cellCpemp.SetCellValue(beneficiario.Ficha.FichaEfectores.Empresa.CodigoPostal);
                var cellIdlocemp = row.CreateCell(24);
                cellIdlocemp.SetCellValue(beneficiario.Ficha.FichaEfectores.Empresa.IdLocalidad == null
                                              ? String.Empty
                                              : beneficiario.Ficha.FichaEfectores.Empresa.LocalidadEmpresa.IdLocalidad.
                                                    ToString());
                var cellLocemp = row.CreateCell(25);
                cellLocemp.SetCellValue(beneficiario.Ficha.FichaEfectores.Empresa.LocalidadEmpresa.NombreLocalidad);
                var cellDepartamentoEmp = row.CreateCell(26);
                cellDepartamentoEmp.SetCellValue(beneficiario.Ficha.FichaEfectores.Empresa.IdLocalidad == null
                                                     ? String.Empty
                                                     : beneficiario.Ficha.FichaEfectores.Empresa.LocalidadEmpresa.
                                                           Departamento.NombreDepartamento);

                //Sede
                var cellCallesede = row.CreateCell(27);
                cellCallesede.SetCellValue(beneficiario.Ficha.FichaEfectores.Sede.Calle);
                var cellNumerosede = row.CreateCell(28);
                cellNumerosede.SetCellValue(beneficiario.Ficha.FichaEfectores.Sede.Numero);
                var cellPisosede = row.CreateCell(29);
                cellPisosede.SetCellValue(beneficiario.Ficha.FichaEfectores.Sede.Piso);
                var cellDptosede = row.CreateCell(30);
                cellDptosede.SetCellValue(beneficiario.Ficha.FichaEfectores.Sede.Dpto);
                var cellCpsede = row.CreateCell(31);
                cellCpsede.SetCellValue(beneficiario.Ficha.FichaEfectores.Sede.CodigoPostal);
                var cellIdlocsede = row.CreateCell(32);
                cellIdlocsede.SetCellValue(beneficiario.Ficha.FichaEfectores.Sede.IdLocalidad == null
                                               ? String.Empty
                                               : beneficiario.Ficha.FichaEfectores.Sede.IdLocalidad.ToString());
                var cellLocsede = row.CreateCell(33);
                cellLocsede.SetCellValue(beneficiario.Ficha.FichaEfectores.Sede.NombreLocalidad);


                IList<IHorarioFicha> listahorarioficha =
                    _horarioficharepositorio.GetHorarioFichaByEmpresa(beneficiario.Ficha.IdFicha,
                                                                      beneficiario.Ficha.FichaEfectores.Empresa.IdEmpresa);

                foreach (var horarioFicha in listahorarioficha)
                {
                    IList<IHorarioEmpresa> listahorarioEmpresa =
                        _horarioempresarepositorio.GetHorarioEmpresaForFicha(horarioFicha.IdEmpresaHorario);

                    foreach (var horarioempresa in listahorarioEmpresa)
                    {
                        string horario = horarioempresa.HoraDesde.ToString().PadLeft(2, '0') + ":" +
                                         horarioempresa.MinutosDesde.ToString().PadLeft(2, '0') + " - " +
                                         horarioempresa.HoraHasta.ToString().PadLeft(2, '0') + ":" +
                                         horarioempresa.MinutosHasta.ToString().PadLeft(2, '0');

                        switch (horarioFicha.DSemana)
                        {
                            case "01":
                                if (horarioFicha.Corte == 0)
                                {
                                    var cellLunes0 = row.CreateCell(34);
                                    cellLunes0.SetCellValue(horario);
                                }
                                if (horarioFicha.Corte == 1)
                                {
                                    var cellLunes1 = row.CreateCell(41);
                                    cellLunes1.SetCellValue(horario);
                                }
                                break;
                            case "02":
                                if (horarioFicha.Corte == 0)
                                {
                                    var cellMartes0 = row.CreateCell(35);
                                    cellMartes0.SetCellValue(horario);
                                }
                                if (horarioFicha.Corte == 1)
                                {
                                    var cellMartes0 = row.CreateCell(42);
                                    cellMartes0.SetCellValue(horario);
                                }
                                break;
                            case "03":
                                if (horarioFicha.Corte == 0)
                                {
                                    var cellMier0 = row.CreateCell(36);
                                    cellMier0.SetCellValue(horario);
                                }
                                if (horarioFicha.Corte == 1)
                                {
                                    var cellMier1 = row.CreateCell(43);
                                    cellMier1.SetCellValue(horario);
                                }
                                break;
                            case "04":
                                if (horarioFicha.Corte == 0)
                                {
                                    var cellJuev0 = row.CreateCell(37);
                                    cellJuev0.SetCellValue(horario);
                                }
                                if (horarioFicha.Corte == 1)
                                {
                                    var cellJuev1 = row.CreateCell(44);
                                    cellJuev1.SetCellValue(horario);
                                }
                                break;
                            case "05":
                                if (horarioFicha.Corte == 0)
                                {
                                    var cellVier0 = row.CreateCell(38);
                                    cellVier0.SetCellValue(horario);
                                }
                                if (horarioFicha.Corte == 1)
                                {
                                    var cellVier1 = row.CreateCell(45);
                                    cellVier1.SetCellValue(horario);
                                }
                                break;
                            case "06":
                                if (horarioFicha.Corte == 0)
                                {
                                    var cellSab0 = row.CreateCell(39);
                                    cellSab0.SetCellValue(horario);
                                }
                                if (horarioFicha.Corte == 1)
                                {
                                    var cellSab1 = row.CreateCell(46);
                                    cellSab1.SetCellValue(horario);
                                }
                                break;
                            case "07":
                                if (horarioFicha.Corte == 0)
                                {
                                    var cellDom0 = row.CreateCell(40);
                                    cellDom0.SetCellValue(horario);
                                }
                                if (horarioFicha.Corte == 1)
                                {
                                    var cellDom1 = row.CreateCell(47);
                                    cellDom1.SetCellValue(horario);
                                }
                                break;
                        }
                    }
                }
            }

            var ms = new MemoryStream();

            templateWorkbook.Write(ms);

            return ms.ToArray();
        }

        public byte[] ExportReporteArtHorariosEntrenamientoVat(IBeneficiariosVista vista)
        {
            string path = "";

            path = HttpContext.Current.Server.MapPath(@"\Archivos\Exportados\TemplateArtHorarios.xls");

            var fs = new FileStream(path, FileMode.Open, FileAccess.Read);

            var templateWorkbook = new HSSFWorkbook(fs, true);

            var sheet = templateWorkbook.GetSheet("Nomina");

            IList<IBeneficiario> listaparaExcel = _beneficiariorepositorio.GetBeneficiariosHorariosArt((int)Enums.Programas.Vat, vista.FechaInicioBeneficio, vista.FechaBenefHasta);

            int i = 2;
            foreach (var beneficiario in listaparaExcel)
            {
                i++;

                //Ficha
                var row = sheet.CreateRow(i);
                var cellidficha = row.CreateCell(0);
                cellidficha.SetCellValue(beneficiario.Ficha.IdFicha);
                var cellApellido = row.CreateCell(1);
                cellApellido.SetCellValue(beneficiario.Ficha.Apellido);
                var cellNombre = row.CreateCell(2);
                cellNombre.SetCellValue(beneficiario.Ficha.Nombre);
                var cellCuil = row.CreateCell(3);
                cellCuil.SetCellValue(beneficiario.Ficha.Cuil);
                var cellDni = row.CreateCell(4);
                cellDni.SetCellValue(beneficiario.Ficha.NumeroDocumento);
                var cellFechaNa = row.CreateCell(5);
                cellFechaNa.SetCellValue(beneficiario.Ficha.FechaNacimiento.ToString());
                var cellCalle = row.CreateCell(6);
                cellCalle.SetCellValue(beneficiario.Ficha.Calle);
                var cellNro = row.CreateCell(7);
                cellNro.SetCellValue(beneficiario.Ficha.Numero);
                var cellPiso = row.CreateCell(8);
                cellPiso.SetCellValue(beneficiario.Ficha.Piso);
                var cellDpto = row.CreateCell(9);
                cellDpto.SetCellValue(beneficiario.Ficha.Dpto);
                var cellBarrio = row.CreateCell(10);
                cellBarrio.SetCellValue(beneficiario.Ficha.Barrio);
                var cellCp = row.CreateCell(11);
                cellCp.SetCellValue(beneficiario.Ficha.CodigoPostal);
                var cellidLocalidad = row.CreateCell(12);
                cellidLocalidad.SetCellValue(beneficiario.Ficha.LocalidadFicha.IdLocalidad);
                var cellnLocalidad = row.CreateCell(13);
                cellnLocalidad.SetCellValue(beneficiario.Ficha.LocalidadFicha.NombreLocalidad);
                var cellIdDepartamento = row.CreateCell(14);
                cellIdDepartamento.SetCellValue(beneficiario.Ficha.LocalidadFicha.Departamento.IdDepartamento);
                var cellnDepartamento = row.CreateCell(15);
                cellnDepartamento.SetCellValue(beneficiario.Ficha.LocalidadFicha.Departamento.NombreDepartamento);
                var celldiscapacidad = row.CreateCell(16);
                celldiscapacidad.SetCellValue(beneficiario.Notificado ? "S" : "N");

                //Empresa
                var cellrazonSocial = row.CreateCell(17);
                cellrazonSocial.SetCellValue(beneficiario.Ficha.FichaVat.Empresa.NombreEmpresa);
                var cellCuit = row.CreateCell(18);
                cellCuit.SetCellValue(beneficiario.Ficha.FichaVat.Empresa.Cuit);
                var cellCalleemp = row.CreateCell(19);
                cellCalleemp.SetCellValue(beneficiario.Ficha.FichaVat.Empresa.Calle);
                var cellNumeroemp = row.CreateCell(20);
                cellNumeroemp.SetCellValue(beneficiario.Ficha.FichaVat.Empresa.Numero);
                var cellPisoemp = row.CreateCell(21);
                cellPisoemp.SetCellValue(beneficiario.Ficha.FichaVat.Empresa.Piso);
                var cellDptoemp = row.CreateCell(22);
                cellDptoemp.SetCellValue(beneficiario.Ficha.FichaVat.Empresa.Dpto);
                var cellCpemp = row.CreateCell(23);
                cellCpemp.SetCellValue(beneficiario.Ficha.FichaVat.Empresa.CodigoPostal);
                var cellIdlocemp = row.CreateCell(24);
                cellIdlocemp.SetCellValue(beneficiario.Ficha.FichaVat.Empresa.IdLocalidad == null
                                              ? String.Empty
                                              : beneficiario.Ficha.FichaVat.Empresa.LocalidadEmpresa.IdLocalidad.
                                                    ToString());
                var cellLocemp = row.CreateCell(25);
                cellLocemp.SetCellValue(beneficiario.Ficha.FichaVat.Empresa.LocalidadEmpresa.NombreLocalidad);
                var cellDepartamentoEmp = row.CreateCell(26);
                cellDepartamentoEmp.SetCellValue(beneficiario.Ficha.FichaVat.Empresa.IdLocalidad == null
                                                     ? String.Empty
                                                     : beneficiario.Ficha.FichaVat.Empresa.LocalidadEmpresa.
                                                           Departamento.NombreDepartamento);

                //Sede
                var cellCallesede = row.CreateCell(27);
                cellCallesede.SetCellValue(beneficiario.Ficha.FichaVat.Sede.Calle);
                var cellNumerosede = row.CreateCell(28);
                cellNumerosede.SetCellValue(beneficiario.Ficha.FichaVat.Sede.Numero);
                var cellPisosede = row.CreateCell(29);
                cellPisosede.SetCellValue(beneficiario.Ficha.FichaVat.Sede.Piso);
                var cellDptosede = row.CreateCell(30);
                cellDptosede.SetCellValue(beneficiario.Ficha.FichaVat.Sede.Dpto);
                var cellCpsede = row.CreateCell(31);
                cellCpsede.SetCellValue(beneficiario.Ficha.FichaVat.Sede.CodigoPostal);
                var cellIdlocsede = row.CreateCell(32);
                cellIdlocsede.SetCellValue(beneficiario.Ficha.FichaVat.Sede.IdLocalidad == null
                                               ? String.Empty
                                               : beneficiario.Ficha.FichaVat.Sede.IdLocalidad.ToString());
                var cellLocsede = row.CreateCell(33);
                cellLocsede.SetCellValue(beneficiario.Ficha.FichaVat.Sede.NombreLocalidad);


                IList<IHorarioFicha> listahorarioficha =
                    _horarioficharepositorio.GetHorarioFichaByEmpresa(beneficiario.Ficha.IdFicha,
                                                                      beneficiario.Ficha.FichaVat.Empresa.IdEmpresa);

                foreach (var horarioFicha in listahorarioficha)
                {
                    IList<IHorarioEmpresa> listahorarioEmpresa =
                        _horarioempresarepositorio.GetHorarioEmpresaForFicha(horarioFicha.IdEmpresaHorario);

                    foreach (var horarioempresa in listahorarioEmpresa)
                    {
                        string horario = horarioempresa.HoraDesde.ToString().PadLeft(2, '0') + ":" +
                                         horarioempresa.MinutosDesde.ToString().PadLeft(2, '0') + " - " +
                                         horarioempresa.HoraHasta.ToString().PadLeft(2, '0') + ":" +
                                         horarioempresa.MinutosHasta.ToString().PadLeft(2, '0');

                        switch (horarioFicha.DSemana)
                        {
                            case "01":
                                if (horarioFicha.Corte == 0)
                                {
                                    var cellLunes0 = row.CreateCell(34);
                                    cellLunes0.SetCellValue(horario);
                                }
                                if (horarioFicha.Corte == 1)
                                {
                                    var cellLunes1 = row.CreateCell(41);
                                    cellLunes1.SetCellValue(horario);
                                }
                                break;
                            case "02":
                                if (horarioFicha.Corte == 0)
                                {
                                    var cellMartes0 = row.CreateCell(35);
                                    cellMartes0.SetCellValue(horario);
                                }
                                if (horarioFicha.Corte == 1)
                                {
                                    var cellMartes0 = row.CreateCell(42);
                                    cellMartes0.SetCellValue(horario);
                                }
                                break;
                            case "03":
                                if (horarioFicha.Corte == 0)
                                {
                                    var cellMier0 = row.CreateCell(36);
                                    cellMier0.SetCellValue(horario);
                                }
                                if (horarioFicha.Corte == 1)
                                {
                                    var cellMier1 = row.CreateCell(43);
                                    cellMier1.SetCellValue(horario);
                                }
                                break;
                            case "04":
                                if (horarioFicha.Corte == 0)
                                {
                                    var cellJuev0 = row.CreateCell(37);
                                    cellJuev0.SetCellValue(horario);
                                }
                                if (horarioFicha.Corte == 1)
                                {
                                    var cellJuev1 = row.CreateCell(44);
                                    cellJuev1.SetCellValue(horario);
                                }
                                break;
                            case "05":
                                if (horarioFicha.Corte == 0)
                                {
                                    var cellVier0 = row.CreateCell(38);
                                    cellVier0.SetCellValue(horario);
                                }
                                if (horarioFicha.Corte == 1)
                                {
                                    var cellVier1 = row.CreateCell(45);
                                    cellVier1.SetCellValue(horario);
                                }
                                break;
                            case "06":
                                if (horarioFicha.Corte == 0)
                                {
                                    var cellSab0 = row.CreateCell(39);
                                    cellSab0.SetCellValue(horario);
                                }
                                if (horarioFicha.Corte == 1)
                                {
                                    var cellSab1 = row.CreateCell(46);
                                    cellSab1.SetCellValue(horario);
                                }
                                break;
                            case "07":
                                if (horarioFicha.Corte == 0)
                                {
                                    var cellDom0 = row.CreateCell(40);
                                    cellDom0.SetCellValue(horario);
                                }
                                if (horarioFicha.Corte == 1)
                                {
                                    var cellDom1 = row.CreateCell(47);
                                    cellDom1.SetCellValue(horario);
                                }
                                break;
                        }
                    }
                }
            }

            var ms = new MemoryStream();

            templateWorkbook.Write(ms);

            return ms.ToArray();
        }

        public byte[] ExportReporteArtHorariosEntrenamientoPppp(IBeneficiariosVista vista)
        {
            string path = "";

            path = HttpContext.Current.Server.MapPath(@"\Archivos\Exportados\TemplateArtHorarios.xls");

            var fs = new FileStream(path, FileMode.Open, FileAccess.Read);

            var templateWorkbook = new HSSFWorkbook(fs, true);

            var sheet = templateWorkbook.GetSheet("Nomina");

            IList<IBeneficiario> listaparaExcel = _beneficiariorepositorio.GetBeneficiariosHorariosArt((int)Enums.Programas.PppProf, vista.FechaInicioBeneficio, vista.FechaBenefHasta);

            int i = 2;
            foreach (var beneficiario in listaparaExcel)
            {
                i++;

                //Ficha
                var row = sheet.CreateRow(i);
                var cellidficha = row.CreateCell(0);
                cellidficha.SetCellValue(beneficiario.Ficha.IdFicha);
                var cellApellido = row.CreateCell(1);
                cellApellido.SetCellValue(beneficiario.Ficha.Apellido);
                var cellNombre = row.CreateCell(2);
                cellNombre.SetCellValue(beneficiario.Ficha.Nombre);
                var cellCuil = row.CreateCell(3);
                cellCuil.SetCellValue(beneficiario.Ficha.Cuil);
                var cellDni = row.CreateCell(4);
                cellDni.SetCellValue(beneficiario.Ficha.NumeroDocumento);
                var cellFechaNa = row.CreateCell(5);
                cellFechaNa.SetCellValue(beneficiario.Ficha.FechaNacimiento.ToString());
                var cellCalle = row.CreateCell(6);
                cellCalle.SetCellValue(beneficiario.Ficha.Calle);
                var cellNro = row.CreateCell(7);
                cellNro.SetCellValue(beneficiario.Ficha.Numero);
                var cellPiso = row.CreateCell(8);
                cellPiso.SetCellValue(beneficiario.Ficha.Piso);
                var cellDpto = row.CreateCell(9);
                cellDpto.SetCellValue(beneficiario.Ficha.Dpto);
                var cellBarrio = row.CreateCell(10);
                cellBarrio.SetCellValue(beneficiario.Ficha.Barrio);
                var cellCp = row.CreateCell(11);
                cellCp.SetCellValue(beneficiario.Ficha.CodigoPostal);
                var cellidLocalidad = row.CreateCell(12);
                cellidLocalidad.SetCellValue(beneficiario.Ficha.LocalidadFicha.IdLocalidad);
                var cellnLocalidad = row.CreateCell(13);
                cellnLocalidad.SetCellValue(beneficiario.Ficha.LocalidadFicha.NombreLocalidad);
                var cellIdDepartamento = row.CreateCell(14);
                cellIdDepartamento.SetCellValue(beneficiario.Ficha.LocalidadFicha.Departamento.IdDepartamento);
                var cellnDepartamento = row.CreateCell(15);
                cellnDepartamento.SetCellValue(beneficiario.Ficha.LocalidadFicha.Departamento.NombreDepartamento);
                var celldiscapacidad = row.CreateCell(16);
                celldiscapacidad.SetCellValue(beneficiario.Notificado ? "S" : "N");

                //Empresa
                var cellrazonSocial = row.CreateCell(17);
                cellrazonSocial.SetCellValue(beneficiario.Ficha.FichaPppp.Empresa.NombreEmpresa);
                var cellCuit = row.CreateCell(18);
                cellCuit.SetCellValue(beneficiario.Ficha.FichaPppp.Empresa.Cuit);
                var cellCalleemp = row.CreateCell(19);
                cellCalleemp.SetCellValue(beneficiario.Ficha.FichaPppp.Empresa.Calle);
                var cellNumeroemp = row.CreateCell(20);
                cellNumeroemp.SetCellValue(beneficiario.Ficha.FichaPppp.Empresa.Numero);
                var cellPisoemp = row.CreateCell(21);
                cellPisoemp.SetCellValue(beneficiario.Ficha.FichaPppp.Empresa.Piso);
                var cellDptoemp = row.CreateCell(22);
                cellDptoemp.SetCellValue(beneficiario.Ficha.FichaPppp.Empresa.Dpto);
                var cellCpemp = row.CreateCell(23);
                cellCpemp.SetCellValue(beneficiario.Ficha.FichaPppp.Empresa.CodigoPostal);
                var cellIdlocemp = row.CreateCell(24);
                cellIdlocemp.SetCellValue(beneficiario.Ficha.FichaPppp.Empresa.IdLocalidad == null
                                              ? String.Empty
                                              : beneficiario.Ficha.FichaPppp.Empresa.LocalidadEmpresa.IdLocalidad.
                                                    ToString());
                var cellLocemp = row.CreateCell(25);
                cellLocemp.SetCellValue(beneficiario.Ficha.FichaPppp.Empresa.LocalidadEmpresa.NombreLocalidad);
                var cellDepartamentoEmp = row.CreateCell(26);
                cellDepartamentoEmp.SetCellValue(beneficiario.Ficha.FichaPppp.Empresa.IdLocalidad == null
                                                     ? String.Empty
                                                     : beneficiario.Ficha.FichaPppp.Empresa.LocalidadEmpresa.
                                                           Departamento.NombreDepartamento);

                //Sede
                var cellCallesede = row.CreateCell(27);
                cellCallesede.SetCellValue(beneficiario.Ficha.FichaPppp.Sede.Calle);
                var cellNumerosede = row.CreateCell(28);
                cellNumerosede.SetCellValue(beneficiario.Ficha.FichaPppp.Sede.Numero);
                var cellPisosede = row.CreateCell(29);
                cellPisosede.SetCellValue(beneficiario.Ficha.FichaPppp.Sede.Piso);
                var cellDptosede = row.CreateCell(30);
                cellDptosede.SetCellValue(beneficiario.Ficha.FichaPppp.Sede.Dpto);
                var cellCpsede = row.CreateCell(31);
                cellCpsede.SetCellValue(beneficiario.Ficha.FichaPppp.Sede.CodigoPostal);
                var cellIdlocsede = row.CreateCell(32);
                cellIdlocsede.SetCellValue(beneficiario.Ficha.FichaPppp.Sede.IdLocalidad == null
                                               ? String.Empty
                                               : beneficiario.Ficha.FichaPppp.Sede.IdLocalidad.ToString());
                var cellLocsede = row.CreateCell(33);
                cellLocsede.SetCellValue(beneficiario.Ficha.FichaPppp.Sede.NombreLocalidad);


                IList<IHorarioFicha> listahorarioficha =
                    _horarioficharepositorio.GetHorarioFichaByEmpresa(beneficiario.Ficha.IdFicha,
                                                                      beneficiario.Ficha.FichaPppp.Empresa.IdEmpresa);

                foreach (var horarioFicha in listahorarioficha)
                {
                    IList<IHorarioEmpresa> listahorarioEmpresa =
                        _horarioempresarepositorio.GetHorarioEmpresaForFicha(horarioFicha.IdEmpresaHorario);

                    foreach (var horarioempresa in listahorarioEmpresa)
                    {
                        string horario = horarioempresa.HoraDesde.ToString().PadLeft(2, '0') + ":" +
                                         horarioempresa.MinutosDesde.ToString().PadLeft(2, '0') + " - " +
                                         horarioempresa.HoraHasta.ToString().PadLeft(2, '0') + ":" +
                                         horarioempresa.MinutosHasta.ToString().PadLeft(2, '0');

                        switch (horarioFicha.DSemana)
                        {
                            case "01":
                                if (horarioFicha.Corte == 0)
                                {
                                    var cellLunes0 = row.CreateCell(34);
                                    cellLunes0.SetCellValue(horario);
                                }
                                if (horarioFicha.Corte == 1)
                                {
                                    var cellLunes1 = row.CreateCell(41);
                                    cellLunes1.SetCellValue(horario);
                                }
                                break;
                            case "02":
                                if (horarioFicha.Corte == 0)
                                {
                                    var cellMartes0 = row.CreateCell(35);
                                    cellMartes0.SetCellValue(horario);
                                }
                                if (horarioFicha.Corte == 1)
                                {
                                    var cellMartes0 = row.CreateCell(42);
                                    cellMartes0.SetCellValue(horario);
                                }
                                break;
                            case "03":
                                if (horarioFicha.Corte == 0)
                                {
                                    var cellMier0 = row.CreateCell(36);
                                    cellMier0.SetCellValue(horario);
                                }
                                if (horarioFicha.Corte == 1)
                                {
                                    var cellMier1 = row.CreateCell(43);
                                    cellMier1.SetCellValue(horario);
                                }
                                break;
                            case "04":
                                if (horarioFicha.Corte == 0)
                                {
                                    var cellJuev0 = row.CreateCell(37);
                                    cellJuev0.SetCellValue(horario);
                                }
                                if (horarioFicha.Corte == 1)
                                {
                                    var cellJuev1 = row.CreateCell(44);
                                    cellJuev1.SetCellValue(horario);
                                }
                                break;
                            case "05":
                                if (horarioFicha.Corte == 0)
                                {
                                    var cellVier0 = row.CreateCell(38);
                                    cellVier0.SetCellValue(horario);
                                }
                                if (horarioFicha.Corte == 1)
                                {
                                    var cellVier1 = row.CreateCell(45);
                                    cellVier1.SetCellValue(horario);
                                }
                                break;
                            case "06":
                                if (horarioFicha.Corte == 0)
                                {
                                    var cellSab0 = row.CreateCell(39);
                                    cellSab0.SetCellValue(horario);
                                }
                                if (horarioFicha.Corte == 1)
                                {
                                    var cellSab1 = row.CreateCell(46);
                                    cellSab1.SetCellValue(horario);
                                }
                                break;
                            case "07":
                                if (horarioFicha.Corte == 0)
                                {
                                    var cellDom0 = row.CreateCell(40);
                                    cellDom0.SetCellValue(horario);
                                }
                                if (horarioFicha.Corte == 1)
                                {
                                    var cellDom1 = row.CreateCell(47);
                                    cellDom1.SetCellValue(horario);
                                }
                                break;
                        }
                    }
                }
            }

            var ms = new MemoryStream();

            templateWorkbook.Write(ms);

            return ms.ToArray();
        }

        public byte[] ExportReporteArtHorariosEntrenamientoConfVos(IBeneficiariosVista vista)
        {
            string path = "";

            path = HttpContext.Current.Server.MapPath(@"\Archivos\Exportados\TemplateArtHorarios.xls");

            var fs = new FileStream(path, FileMode.Open, FileAccess.Read);

            var templateWorkbook = new HSSFWorkbook(fs, true);

            var sheet = templateWorkbook.GetSheet("Nomina");

            IList<IBeneficiario> listaparaExcel = _beneficiariorepositorio.GetBeneficiariosHorariosArt((int)Enums.Programas.ConfiamosEnVos, vista.FechaInicioBeneficio, vista.FechaBenefHasta);

            int i = 2;
            foreach (var beneficiario in listaparaExcel)
            {
                //int tieneHorasCargadas =_horarioficharepositorio.GetHorarioFichaByEmpresa(beneficiario.Ficha.IdFicha,beneficiario.Ficha.FichaPpp.Empresa.IdEmpresa).Count;
                //if (tieneHorasCargadas != 0)
                //{
                i++;

                //Ficha
                var row = sheet.CreateRow(i);
                var cellidficha = row.CreateCell(0);
                cellidficha.SetCellValue(beneficiario.Ficha.IdFicha);
                var cellApellido = row.CreateCell(1);
                cellApellido.SetCellValue(beneficiario.Ficha.Apellido);
                var cellNombre = row.CreateCell(2);
                cellNombre.SetCellValue(beneficiario.Ficha.Nombre);
                var cellCuil = row.CreateCell(3);
                cellCuil.SetCellValue(beneficiario.Ficha.Cuil);
                var cellDni = row.CreateCell(4);
                cellDni.SetCellValue(beneficiario.Ficha.NumeroDocumento);
                var cellFechaNa = row.CreateCell(5);
                cellFechaNa.SetCellValue(beneficiario.Ficha.FechaNacimiento.ToString());
                var cellCalle = row.CreateCell(6);
                cellCalle.SetCellValue(beneficiario.Ficha.Calle);
                var cellNro = row.CreateCell(7);
                cellNro.SetCellValue(beneficiario.Ficha.Numero);
                var cellPiso = row.CreateCell(8);
                cellPiso.SetCellValue(beneficiario.Ficha.Piso);
                var cellDpto = row.CreateCell(9);
                cellDpto.SetCellValue(beneficiario.Ficha.Dpto);
                var cellBarrio = row.CreateCell(10);
                cellBarrio.SetCellValue(beneficiario.Ficha.Barrio);
                var cellCp = row.CreateCell(11);
                cellCp.SetCellValue(beneficiario.Ficha.CodigoPostal);
                var cellidLocalidad = row.CreateCell(12);
                cellidLocalidad.SetCellValue(beneficiario.Ficha.LocalidadFicha.IdLocalidad);
                var cellnLocalidad = row.CreateCell(13);
                cellnLocalidad.SetCellValue(beneficiario.Ficha.LocalidadFicha.NombreLocalidad);
                var cellIdDepartamento = row.CreateCell(14);
                cellIdDepartamento.SetCellValue(beneficiario.Ficha.LocalidadFicha.Departamento.IdDepartamento);
                var cellnDepartamento = row.CreateCell(15);
                cellnDepartamento.SetCellValue(beneficiario.Ficha.LocalidadFicha.Departamento.NombreDepartamento);
                var celldiscapacidad = row.CreateCell(16);
                celldiscapacidad.SetCellValue(beneficiario.Notificado ? "S" : "N");

                //Empresa
                var cellrazonSocial = row.CreateCell(17);
                cellrazonSocial.SetCellValue(beneficiario.Ficha.FichaConfVos.Empresa.NombreEmpresa);
                var cellCuit = row.CreateCell(18);
                cellCuit.SetCellValue(beneficiario.Ficha.FichaConfVos.Empresa.Cuit);
                var cellCalleemp = row.CreateCell(19);
                cellCalleemp.SetCellValue(beneficiario.Ficha.FichaConfVos.Empresa.Calle);
                var cellNumeroemp = row.CreateCell(20);
                cellNumeroemp.SetCellValue(beneficiario.Ficha.FichaConfVos.Empresa.Numero);
                var cellPisoemp = row.CreateCell(21);
                cellPisoemp.SetCellValue(beneficiario.Ficha.FichaConfVos.Empresa.Piso);
                var cellDptoemp = row.CreateCell(22);
                cellDptoemp.SetCellValue(beneficiario.Ficha.FichaConfVos.Empresa.Dpto);
                var cellCpemp = row.CreateCell(23);
                cellCpemp.SetCellValue(beneficiario.Ficha.FichaConfVos.Empresa.CodigoPostal);
                var cellIdlocemp = row.CreateCell(24);
                cellIdlocemp.SetCellValue(beneficiario.Ficha.FichaConfVos.Empresa.IdLocalidad == null
                                              ? String.Empty
                                              : beneficiario.Ficha.FichaConfVos.Empresa.LocalidadEmpresa.IdLocalidad.
                                                    ToString());
                var cellLocemp = row.CreateCell(25);
                cellLocemp.SetCellValue(beneficiario.Ficha.FichaConfVos.Empresa.LocalidadEmpresa.NombreLocalidad);
                var cellDepartamentoEmp = row.CreateCell(26);
                cellDepartamentoEmp.SetCellValue(beneficiario.Ficha.FichaConfVos.Empresa.IdLocalidad == null
                                                     ? String.Empty
                                                     : beneficiario.Ficha.FichaConfVos.Empresa.LocalidadEmpresa.
                                                           Departamento.NombreDepartamento);

                //Sede
                var cellCallesede = row.CreateCell(27);
                cellCallesede.SetCellValue(beneficiario.Ficha.FichaConfVos.Sede.Calle);
                var cellNumerosede = row.CreateCell(28);
                cellNumerosede.SetCellValue(beneficiario.Ficha.FichaConfVos.Sede.Numero);
                var cellPisosede = row.CreateCell(29);
                cellPisosede.SetCellValue(beneficiario.Ficha.FichaConfVos.Sede.Piso);
                var cellDptosede = row.CreateCell(30);
                cellDptosede.SetCellValue(beneficiario.Ficha.FichaConfVos.Sede.Dpto);
                var cellCpsede = row.CreateCell(31);
                cellCpsede.SetCellValue(beneficiario.Ficha.FichaConfVos.Sede.CodigoPostal);
                var cellIdlocsede = row.CreateCell(32);
                cellIdlocsede.SetCellValue(beneficiario.Ficha.FichaConfVos.Sede.IdLocalidad == null
                                               ? String.Empty
                                               : beneficiario.Ficha.FichaConfVos.Sede.IdLocalidad.ToString());
                var cellLocsede = row.CreateCell(33);
                cellLocsede.SetCellValue(beneficiario.Ficha.FichaConfVos.Sede.NombreLocalidad);


                IList<IHorarioFicha> listahorarioficha =
                    _horarioficharepositorio.GetHorarioFichaByEmpresa(beneficiario.Ficha.IdFicha,
                                                                      beneficiario.Ficha.FichaConfVos.Empresa.IdEmpresa);

                foreach (var horarioFicha in listahorarioficha)
                {
                    IList<IHorarioEmpresa> listahorarioEmpresa =
                        _horarioempresarepositorio.GetHorarioEmpresaForFicha(horarioFicha.IdEmpresaHorario);

                    foreach (var horarioempresa in listahorarioEmpresa)
                    {
                        string horario = horarioempresa.HoraDesde.ToString().PadLeft(2, '0') + ":" +
                                         horarioempresa.MinutosDesde.ToString().PadLeft(2, '0') + " - " +
                                         horarioempresa.HoraHasta.ToString().PadLeft(2, '0') + ":" +
                                         horarioempresa.MinutosHasta.ToString().PadLeft(2, '0');

                        switch (horarioFicha.DSemana)
                        {
                            case "01":
                                if (horarioFicha.Corte == 0)
                                {
                                    var cellLunes0 = row.CreateCell(34);
                                    cellLunes0.SetCellValue(horario);
                                }
                                if (horarioFicha.Corte == 1)
                                {
                                    var cellLunes1 = row.CreateCell(41);
                                    cellLunes1.SetCellValue(horario);
                                }
                                break;
                            case "02":
                                if (horarioFicha.Corte == 0)
                                {
                                    var cellMartes0 = row.CreateCell(35);
                                    cellMartes0.SetCellValue(horario);
                                }
                                if (horarioFicha.Corte == 1)
                                {
                                    var cellMartes0 = row.CreateCell(42);
                                    cellMartes0.SetCellValue(horario);
                                }
                                break;
                            case "03":
                                if (horarioFicha.Corte == 0)
                                {
                                    var cellMier0 = row.CreateCell(36);
                                    cellMier0.SetCellValue(horario);
                                }
                                if (horarioFicha.Corte == 1)
                                {
                                    var cellMier1 = row.CreateCell(43);
                                    cellMier1.SetCellValue(horario);
                                }
                                break;
                            case "04":
                                if (horarioFicha.Corte == 0)
                                {
                                    var cellJuev0 = row.CreateCell(37);
                                    cellJuev0.SetCellValue(horario);
                                }
                                if (horarioFicha.Corte == 1)
                                {
                                    var cellJuev1 = row.CreateCell(44);
                                    cellJuev1.SetCellValue(horario);
                                }
                                break;
                            case "05":
                                if (horarioFicha.Corte == 0)
                                {
                                    var cellVier0 = row.CreateCell(38);
                                    cellVier0.SetCellValue(horario);
                                }
                                if (horarioFicha.Corte == 1)
                                {
                                    var cellVier1 = row.CreateCell(45);
                                    cellVier1.SetCellValue(horario);
                                }
                                break;
                            case "06":
                                if (horarioFicha.Corte == 0)
                                {
                                    var cellSab0 = row.CreateCell(39);
                                    cellSab0.SetCellValue(horario);
                                }
                                if (horarioFicha.Corte == 1)
                                {
                                    var cellSab1 = row.CreateCell(46);
                                    cellSab1.SetCellValue(horario);
                                }
                                break;
                            case "07":
                                if (horarioFicha.Corte == 0)
                                {
                                    var cellDom0 = row.CreateCell(40);
                                    cellDom0.SetCellValue(horario);
                                }
                                if (horarioFicha.Corte == 1)
                                {
                                    var cellDom1 = row.CreateCell(47);
                                    cellDom1.SetCellValue(horario);
                                }
                                break;
                        }
                    }
                }
            }

            var ms = new MemoryStream();

            templateWorkbook.Write(ms);

            return ms.ToArray();

        }

        public byte[] ReporteBeneficiarioArt()
        {
            var document = new Document(PageSize.A4, 50, 50, 25, 25);
            var memStream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(document, memStream);
            writer.CloseStream = false;

            document.Open();

            string path = HttpContext.Current.Server.MapPath(@"\Content\images\Logo Agencia.jpg");

            Image imagen = Image.GetInstance(path);

            var head = new PdfPTable(1) { TotalWidth = 60 };
            head.SetWidthPercentage(new float[] { 250 }, PageSize.A4);

            var imghead = new PdfPCell(imagen, true) { Border = 0, VerticalAlignment = Element.ALIGN_BOTTOM };

            var title =
                new PdfPCell(new Paragraph("Nómina Beneficiario " + DateTime.Now.Year,
                                           FontFactory.GetFont("Arial, Helvetica, sans-serif", 15, FontPdf.BOLD,
                                                               Color.BLACK)))
                    {
                        Border = 0,
                        VerticalAlignment = Element.ALIGN_BOTTOM,
                        HorizontalAlignment = Element.ALIGN_CENTER
                    };

            head.AddCell(imghead);
            head.AddCell(title);

            document.Add(head);

            document.Add(new Paragraph(" ", FontFactory.GetFont("Arial, Helvetica, sans-serif", 2)));

            var ls = new LineSeparator();
            document.Add(new Chunk(ls));

            document.Add(new Paragraph(" ", FontFactory.GetFont("Arial, Helvetica, sans-serif", 2)));

            IBeneficiariosVista beneficiarios = new BeneficiariosVista
                                                    {
                                                        ListaBeneficiarios =
                                                            _beneficiariorepositorio.GetBeneficiarios(null, null, null,
                                                                                                      null,
                                                                                                      (int)
                                                                                                      Enums.TipoFicha.
                                                                                                          Ppp, "T",
                                                                                                      "T", "T",
                                                                                                      "T", 0, "",0, "")
                                                    };

            PdfPTable unaTabla =
                GenerarTablaReporteArt(beneficiarios);

            document.Add(unaTabla);

            document.Close();
            var buf = new byte[memStream.Position];
            memStream.Position = 0;
            memStream.Read(buf, 0, buf.Length);


            return AddPageNumbers(buf);
        }

        public byte[] ReporteBeneficiarioNomina()
        {
            var document = new Document(PageSize.A4.Rotate(), 50, 50, 25, 25);
            var memStream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(document, memStream);
            writer.CloseStream = false;

            document.Open();

            string path = HttpContext.Current.Server.MapPath(@"\Content\images\Logo Agencia.jpg");

            Image imagen = Image.GetInstance(path);

            var head = new PdfPTable(1) { TotalWidth = 60 };
            head.SetWidthPercentage(new float[] { 250 }, PageSize.A4);

            var imghead = new PdfPCell(imagen, true) { Border = 0, VerticalAlignment = Element.ALIGN_BOTTOM };

            var title =
                new PdfPCell(new Paragraph("Nómina Beneficiario " + DateTime.Now.Year,
                                           FontFactory.GetFont("Arial, Helvetica, sans-serif", 15, FontPdf.BOLD,
                                                               Color.BLACK)))
                {
                    Border = 0,
                    VerticalAlignment = Element.ALIGN_BOTTOM,
                    HorizontalAlignment = Element.ALIGN_CENTER
                };

            head.AddCell(imghead);
            head.AddCell(title);

            document.Add(head);

            document.Add(new Paragraph(" ", FontFactory.GetFont("Arial, Helvetica, sans-serif", 2)));

            var ls = new LineSeparator();
            document.Add(new Chunk(ls));

            document.Add(new Paragraph(" ", FontFactory.GetFont("Arial, Helvetica, sans-serif", 2)));

            IBeneficiariosVista beneficiarios = new BeneficiariosVista
                                                    {
                                                        ListaBeneficiarios =
                                                            _beneficiariorepositorio.GetBeneficiarios(null, null, null,
                                                                                                      null,
                                                                                                      (int)
                                                                                                      Enums.TipoFicha.
                                                                                                          Ppp, "T",
                                                                                                      "T", "T",
                                                                                                      "T", 0, "",0, "")
                                                    };

            PdfPTable unaTabla =
                GenerarTablaReporteNomina(beneficiarios);

            document.Add(unaTabla);

            document.Close();
            var buf = new byte[memStream.Position];
            memStream.Position = 0;
            memStream.Read(buf, 0, buf.Length);


            return AddPageNumbersApaizados(buf);
        }

        public static PdfPTable GenerarTablaReporteArt(IBeneficiariosVista model)
        {

            var unaTabla = new PdfPTable(5);
            unaTabla.SetWidthPercentage(new float[] { 150, 150, 100, 150, 100 }, PageSize.A4);

            //Headers
            unaTabla.AddCell(new Paragraph("Apellido", FontFactory.GetFont("Arial, Helvetica, sans-serif", 10, FontPdf.BOLD, Color.WHITE)));
            unaTabla.AddCell(new Paragraph("Nombre", FontFactory.GetFont("Arial, Helvetica, sans-serif", 10, FontPdf.BOLD, Color.WHITE)));
            unaTabla.AddCell(new Paragraph("Dni", FontFactory.GetFont("Arial, Helvetica, sans-serif", 10, FontPdf.BOLD, Color.WHITE)));
            unaTabla.AddCell(new Paragraph("Razón Social Empresa", FontFactory.GetFont("Arial, Helvetica, sans-serif", 10, FontPdf.BOLD, Color.WHITE)));
            unaTabla.AddCell(new Paragraph("CUIT", FontFactory.GetFont("Arial, Helvetica, sans-serif", 10, FontPdf.BOLD, Color.WHITE)));

            unaTabla.HeaderRows = 1;
            //¿Le damos un poco de formato?
            foreach (PdfPCell celda in unaTabla.Rows[0].GetCells())
            {
                celda.BackgroundColor = new Color(106, 173, 218);
                celda.HorizontalAlignment = 1;
            }


            foreach (var beneficiario in model.ListaBeneficiarios)
            {
                var celdaApellido =
                    new PdfPCell(new Paragraph(beneficiario.Ficha.Apellido,
                                               FontFactory.GetFont("Arial, Helvetica, sans-serif", 10)));
                var celdaNombre =
                    new PdfPCell(new Paragraph(beneficiario.Ficha.Nombre,
                                               FontFactory.GetFont("Arial, Helvetica, sans-serif", 10)));
                var celdaNumeroDocumento =
                    new PdfPCell(new Paragraph(beneficiario.Ficha.NumeroDocumento,
                                               FontFactory.GetFont("Arial, Helvetica, sans-serif", 10)));

                var celdaRazonSocial =
                    new PdfPCell(new Paragraph(beneficiario.Ficha.FichaPpp.Empresa.NombreEmpresa,
                                               FontFactory.GetFont("Arial, Helvetica, sans-serif", 10)));

                var celdaCuit =
                    new PdfPCell(new Paragraph(beneficiario.Ficha.FichaPpp.Empresa.Cuit,
                                               FontFactory.GetFont("Arial, Helvetica, sans-serif", 10)));
                unaTabla.AddCell(celdaApellido);
                unaTabla.AddCell(celdaNombre);
                unaTabla.AddCell(celdaNumeroDocumento);
                unaTabla.AddCell(celdaRazonSocial);
                unaTabla.AddCell(celdaCuit);
            }

            return unaTabla;
        }

        public static PdfPTable GenerarTablaReporteNomina(IBeneficiariosVista model)
        {
            var unaTabla = new PdfPTable(9);
            unaTabla.SetWidthPercentage(new float[] { 60, 60, 45, 100, 55, 100, 30, 60, 60 }, PageSize.A4);

            //Headers
            unaTabla.AddCell(new Paragraph("Apellido", FontFactory.GetFont("Arial, Helvetica, sans-serif", 10, FontPdf.BOLD, Color.WHITE)));
            unaTabla.AddCell(new Paragraph("Nombre", FontFactory.GetFont("Arial, Helvetica, sans-serif", 10, FontPdf.BOLD, Color.WHITE)));
            unaTabla.AddCell(new Paragraph("Dni", FontFactory.GetFont("Arial, Helvetica, sans-serif", 10, FontPdf.BOLD, Color.WHITE)));
            unaTabla.AddCell(new Paragraph("Razón Social Empresa", FontFactory.GetFont("Arial, Helvetica, sans-serif", 10, FontPdf.BOLD, Color.WHITE)));
            unaTabla.AddCell(new Paragraph("CUIT", FontFactory.GetFont("Arial, Helvetica, sans-serif", 10, FontPdf.BOLD, Color.WHITE)));
            unaTabla.AddCell(new Paragraph("Calle", FontFactory.GetFont("Arial, Helvetica, sans-serif", 10, FontPdf.BOLD, Color.WHITE)));
            unaTabla.AddCell(new Paragraph("Nro.", FontFactory.GetFont("Arial, Helvetica, sans-serif", 10, FontPdf.BOLD, Color.WHITE)));
            unaTabla.AddCell(new Paragraph("Barrio", FontFactory.GetFont("Arial, Helvetica, sans-serif", 10, FontPdf.BOLD, Color.WHITE)));
            unaTabla.AddCell(new Paragraph("Localidad", FontFactory.GetFont("Arial, Helvetica, sans-serif", 10, FontPdf.BOLD, Color.WHITE)));

            unaTabla.HeaderRows = 1;

            foreach (PdfPCell celda in unaTabla.Rows[0].GetCells())
            {
                celda.BackgroundColor = new Color(106, 173, 218);
                celda.HorizontalAlignment = 1;
            }

            foreach (var beneficiario in model.ListaBeneficiarios)
            {
                var celdaApellido =
                    new PdfPCell(new Paragraph(beneficiario.Ficha.Apellido,
                                               FontFactory.GetFont("Arial, Helvetica, sans-serif", 8)));
                var celdaNombre =
                    new PdfPCell(new Paragraph(beneficiario.Ficha.Nombre,
                                               FontFactory.GetFont("Arial, Helvetica, sans-serif", 8)));
                var celdaNumeroDocumento =
                    new PdfPCell(new Paragraph(beneficiario.Ficha.NumeroDocumento,
                                               FontFactory.GetFont("Arial, Helvetica, sans-serif", 8)));

                var celdaRazonSocial =
                    new PdfPCell(new Paragraph(beneficiario.Ficha.FichaPpp.Empresa.NombreEmpresa,
                                               FontFactory.GetFont("Arial, Helvetica, sans-serif", 8)));

                var celdaCuit =
                    new PdfPCell(new Paragraph(beneficiario.Ficha.FichaPpp.Empresa.Cuit,
                                               FontFactory.GetFont("Arial, Helvetica, sans-serif", 8)));

                var celdaCalle =
                  new PdfPCell(new Paragraph(beneficiario.Ficha.FichaPpp.Empresa.Calle,
                                             FontFactory.GetFont("Arial, Helvetica, sans-serif", 8)));

                var celdaNro =
                  new PdfPCell(new Paragraph(beneficiario.Ficha.FichaPpp.Empresa.Numero,
                                             FontFactory.GetFont("Arial, Helvetica, sans-serif", 8)));

                var celdaBarrio =
                  new PdfPCell(new Paragraph(beneficiario.Ficha.FichaPpp.Empresa.NombreLocalidad,
                                             FontFactory.GetFont("Arial, Helvetica, sans-serif", 8)));

                var celdaLocalidad =
                  new PdfPCell(new Paragraph(beneficiario.Ficha.FichaPpp.Empresa.NombreLocalidad,
                                             FontFactory.GetFont("Arial, Helvetica, sans-serif", 8)));

                unaTabla.AddCell(celdaApellido);
                unaTabla.AddCell(celdaNombre);
                unaTabla.AddCell(celdaNumeroDocumento);
                unaTabla.AddCell(celdaRazonSocial);
                unaTabla.AddCell(celdaCuit);
                unaTabla.AddCell(celdaCalle);
                unaTabla.AddCell(celdaNro);
                unaTabla.AddCell(celdaBarrio);
                unaTabla.AddCell(celdaLocalidad);
            }

            return unaTabla;
        }

        public static byte[] AddPageNumbers(byte[] pdf)
        {
            var ms = new MemoryStream();
            ms.Write(pdf, 0, pdf.Length);
            var reader = new PdfReader(pdf);
            int n = reader.NumberOfPages;
            Rectangle psize = reader.GetPageSize(1);
            var document = new Document(psize, 50, 50, 50, 50);
            PdfWriter writer = PdfWriter.GetInstance(document, ms);
            document.Open();
            PdfContentByte cb = writer.DirectContent;
            int p = 0;
            for (int page = 1; page <= reader.NumberOfPages; page++)
            {
                document.NewPage();
                p++;
                PdfImportedPage importedPage = writer.GetImportedPage(reader, page);
                cb.AddTemplate(importedPage, 0, 0);
                BaseFont bf = BaseFont.CreateFont(BaseFont.COURIER_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                cb.BeginText();
                cb.SetFontAndSize(bf, 12);
                cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "Agencia de Promoción de Empleo y Formación Profesional: " + p + "/" + n, 350, 5, 0);
                cb.EndText();
            }
            document.Close();
            return ms.ToArray();
        }

        public static byte[] AddPageNumbersApaizados(byte[] pdf)
        {
            var ms = new MemoryStream();
            ms.Write(pdf, 0, pdf.Length);
            var reader = new PdfReader(pdf);
            int n = reader.NumberOfPages;
            Rectangle psize = reader.GetPageSize(1);
            var document = new Document(psize, 50, 50, 50, 50);
            PdfWriter writer = PdfWriter.GetInstance(document, ms);
            document.Open();
            PdfContentByte cb = writer.DirectContent;
            int p = 0;
            for (int page = 1; page <= reader.NumberOfPages; page++)
            {
                document.NewPage();
                p++;
                PdfImportedPage importedPage = writer.GetImportedPage(reader, page);
                cb.AddTemplate(importedPage, 0, 0);
                BaseFont bf = BaseFont.CreateFont(BaseFont.COURIER_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                cb.BeginText();
                cb.SetFontAndSize(bf, 12);
                //Pie de pagina:posicion x: 590, posicion y: 605, rotacion :90 (a 90grados de la pag vertical)
                cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "Agencia de Promoción de Empleo y Formación Profesional: " + p + "/" + n, 590, 605, 90);
                cb.EndText();
            }
            document.Close();
            return ms.ToArray();
        }

        private static void CargarMonedas(IBeneficiarioVista vista, IEnumerable<ITablaBcoCba> listaMonedas)
        {
            foreach (var lMoneda in listaMonedas)
            {
                vista.Monedas.Combo.Add(new ComboItem
                {
                    Id = Convert.ToInt32(lMoneda.IdTablaBcoCba),
                    Description = lMoneda.DescripcionBcoCba
                });
            }
        }

        public IList<IError> GetErrores()
        {
            return Errors;
        }

        // 26/02/2013 - LEANDRO DI CAMPLI - MEJORA DE RENDIMIENTO EXPORT EXCEL BENEFICIARIOS
        public byte[] ExportBeneficiarioBis(IBeneficiariosVista vista)
        {
            var path = String.Empty;

            switch (Convert.ToInt32(vista.Programas.Selected))
            {
                case (int)Enums.TipoFicha.Terciaria:
                    path = HttpContext.Current.Server.MapPath(@"\Archivos\Exportados\TemplateBeneficiarioTerciarios.xls");
                    break;
                case (int)Enums.TipoFicha.Universitaria:
                    path = HttpContext.Current.Server.MapPath(@"\Archivos\Exportados\TemplateBeneficiarioUniversitarios.xls");
                    break;
                case (int)Enums.TipoFicha.Ppp:
                    path = HttpContext.Current.Server.MapPath(@"\Archivos\Exportados\TemplateBeneficiariosPPP.xls");
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
                default:
                    return new byte[] { };
            }

            var fs = new FileStream(path, FileMode.Open, FileAccess.Read);

            var templateWorkbook = new HSSFWorkbook(fs, true);

            var sheet = templateWorkbook.GetSheet("Nomina");

            IList<IBeneficiario> listaparaExcel;
            //IList<IBeneficiario> lista;
            //lista = vista.ListaBeneficiarios;
            //int[] beneficiarios;


            //beneficiarios = vista.idBeneficiarios; //lista.Select(c => c.IdBeneficiario).ToArray();


            switch (vista.ConCuenta)
            {
                case "S":
                    listaparaExcel = _beneficiariorepositorio.GetBeneficiariosCompleto(vista.NombreBusqueda, vista.ApellidoBusqueda,
                                                                                  vista.CuilBusqueda,
                                                                                  vista.NumeroDocumentoBusqueda,
                                                                                  Convert.ToInt32(vista.Programas.Selected),
                                                                                  vista.ConApoderado, vista.ConModalidad,
                                                                                  vista.ConDiscapacidad, vista.ConAltaTemprana,
                                                                                  Convert.ToInt32(vista.EstadosBeneficiarios.Selected),
                                                                                  vista.ApellidoNombreApoderadoBusqueda, Convert.ToInt32(vista.etapas.Selected), "S");
                    break;
                case "N":
                    listaparaExcel = _beneficiariorepositorio.GetBeneficiariosCompleto(vista.NombreBusqueda, vista.ApellidoBusqueda,
                                                                                  vista.CuilBusqueda,
                                                                                  vista.NumeroDocumentoBusqueda,
                                                                                  Convert.ToInt32(vista.Programas.Selected),
                                                                                  vista.ConApoderado, vista.ConModalidad,
                                                                                  vista.ConDiscapacidad, vista.ConAltaTemprana,
                                                                                  Convert.ToInt32(vista.EstadosBeneficiarios.Selected),
                                                                                  vista.ApellidoNombreApoderadoBusqueda, Convert.ToInt32(vista.etapas.Selected), "N");
                    break;

                default:
                    //listaparaExcel = _beneficiariorepositorio.GetBeneficiariosCompleto(beneficiarios, Convert.ToInt32(vista.Programas.Selected));
                    listaparaExcel = _beneficiariorepositorio.GetBeneficiariosCompleto(vista.NombreBusqueda, vista.ApellidoBusqueda,
                                                                                  vista.CuilBusqueda,
                                                                                  vista.NumeroDocumentoBusqueda,
                                                                                  Convert.ToInt32(vista.Programas.Selected),
                                                                                  vista.ConApoderado, vista.ConModalidad,
                                                                                  vista.ConDiscapacidad, vista.ConAltaTemprana,
                                                                                  Convert.ToInt32(vista.EstadosBeneficiarios.Selected),
                                                                                  vista.ApellidoNombreApoderadoBusqueda, Convert.ToInt32(vista.etapas.Selected), "T");

                //listaparaExcel = _beneficiariorepositorio.GetBeneficiariosCompletoBis(beneficiarios, Convert.ToInt32(vista.Programas.Selected));
                    break;
            }

            var i = 1;

            switch (Convert.ToInt32(vista.Programas.Selected))
            {
                case (int)Enums.TipoFicha.Terciaria:
                    #region case (int)Enums.TipoFicha.Terciaria
                    foreach (var beneficiario in listaparaExcel)
                    {
                        i++;
                        //Ficha
                        var row = sheet.CreateRow(i);
                        var cellidficha = row.CreateCell(0); cellidficha.SetCellValue(beneficiario.Ficha.IdFicha);
                        var cellApellido = row.CreateCell(1); cellApellido.SetCellValue(beneficiario.Ficha.Apellido);
                        var cellNombre = row.CreateCell(2); cellNombre.SetCellValue(beneficiario.Ficha.Nombre);
                        var cellCuil = row.CreateCell(3); cellCuil.SetCellValue(beneficiario.Ficha.Cuil);
                        var cellDni = row.CreateCell(4); cellDni.SetCellValue(beneficiario.Ficha.NumeroDocumento);
                        var cellFechaNa = row.CreateCell(5); cellFechaNa.SetCellValue(beneficiario.Ficha.FechaNacimiento.ToString());
                        var cellCalle = row.CreateCell(6); cellCalle.SetCellValue(beneficiario.Ficha.Calle);
                        var cellNro = row.CreateCell(7); cellNro.SetCellValue(beneficiario.Ficha.Numero);
                        var cellPiso = row.CreateCell(8); cellPiso.SetCellValue(beneficiario.Ficha.Piso);
                        var cellDpto = row.CreateCell(9); cellDpto.SetCellValue(beneficiario.Ficha.Dpto);
                        var cellMono = row.CreateCell(10); cellMono.SetCellValue(beneficiario.Ficha.Monoblock);
                        var cellParcela = row.CreateCell(11); cellParcela.SetCellValue(beneficiario.Ficha.Parcela);
                        var cellManzana = row.CreateCell(12); cellManzana.SetCellValue(beneficiario.Ficha.Manzana);
                        var cellEntreCalle = row.CreateCell(13); cellEntreCalle.SetCellValue(beneficiario.Ficha.EntreCalles);
                        var cellBarrio = row.CreateCell(14); cellBarrio.SetCellValue(beneficiario.Ficha.Barrio);
                        var cellCp = row.CreateCell(15); cellCp.SetCellValue(beneficiario.Ficha.CodigoPostal);
                        var cellidLocalidad = row.CreateCell(16); cellidLocalidad.SetCellValue(beneficiario.Ficha.IdLocalidad == null ? String.Empty
                            : beneficiario.Ficha.LocalidadFicha.IdLocalidad.ToString());
                        var cellnLocalidad = row.CreateCell(17); cellnLocalidad.SetCellValue(beneficiario.Ficha.LocalidadFicha.NombreLocalidad);
                        var cellIdDepartamento = row.CreateCell(18); cellIdDepartamento.SetCellValue(beneficiario.Ficha.IdLocalidad == null ? String.Empty
                            : beneficiario.Ficha.LocalidadFicha.Departamento.IdDepartamento.ToString());
                        var cellnDepartamento = row.CreateCell(19); cellnDepartamento.SetCellValue(beneficiario.Ficha.LocalidadFicha.Departamento.NombreDepartamento);
                        var cellTelFijo = row.CreateCell(20); cellTelFijo.SetCellValue(beneficiario.Ficha.TelefonoFijo);
                        var cellTelCelular = row.CreateCell(21); cellTelCelular.SetCellValue(beneficiario.Ficha.TelefonoCelular);
                        var cellContacto = row.CreateCell(22); cellContacto.SetCellValue(beneficiario.Ficha.Contacto);
                        var cellEmail = row.CreateCell(23); cellEmail.SetCellValue(beneficiario.Ficha.Mail);
                        var cellSexo = row.CreateCell(24); cellSexo.SetCellValue(beneficiario.Ficha.Sexo);
                        var cellidestadoFicha = row.CreateCell(25); cellidestadoFicha.SetCellValue(beneficiario.Ficha.IdEstadoFicha == null ? String.Empty
                            : beneficiario.Ficha.IdEstadoFicha.ToString());
                        var cellnestadoFicha = row.CreateCell(26); cellnestadoFicha.SetCellValue(beneficiario.Ficha.NombreEstadoFicha);

                        var cellPromedioterc = row.CreateCell(27); cellPromedioterc.SetCellValue(beneficiario.Ficha.PromedioTerciaria == null ? String.Empty
                            : beneficiario.Ficha.PromedioTerciaria.ToString());
                        var cellOtracarreraterc = row.CreateCell(28); cellOtracarreraterc.SetCellValue(beneficiario.Ficha.OtraCarreraTerciaria);
                        var cellOtrainstiterc = row.CreateCell(29); cellOtrainstiterc.SetCellValue(beneficiario.Ficha.OtraInstitucionTerciaria);
                        var cellIdcarreraterc = row.CreateCell(30); cellIdcarreraterc.SetCellValue(beneficiario.Ficha.IdCarreraTerciaria == null ?
                            String.Empty : beneficiario.Ficha.CarreraFicha.IdCarrera.ToString());
                        var cellIdescuelaterc = row.CreateCell(31); cellIdescuelaterc.SetCellValue(beneficiario.Ficha.IdEscuelaTerciaria == null ?
                            String.Empty : beneficiario.Ficha.EscuelaFicha.Id_Escuela.ToString());
                        var cellEscuelaterc = row.CreateCell(32); cellEscuelaterc.SetCellValue(beneficiario.Ficha.EscuelaFicha.Nombre_Escuela);
                        var cellCueterc = row.CreateCell(33); cellCueterc.SetCellValue(beneficiario.Ficha.EscuelaFicha.Cue);
                        var cellAnexoterc = row.CreateCell(34); cellAnexoterc.SetCellValue(beneficiario.Ficha.EscuelaFicha.Anexo == null ? String.Empty
                            : beneficiario.Ficha.EscuelaFicha.Anexo.ToString());

                        var cellEscBarriterc = row.CreateCell(35); cellEscBarriterc.SetCellValue(beneficiario.Ficha.EscuelaFicha.Barrio);
                        var cellEscidlocalidadterc = row.CreateCell(36); cellEscidlocalidadterc.SetCellValue(beneficiario.Ficha.IdLocalidadEscTerc == null ?
                            String.Empty : beneficiario.Ficha.EscuelaFicha.LocalidadEscuela.IdLocalidad.ToString());
                        var cellEsclocalidadterc = row.CreateCell(37); cellEsclocalidadterc.SetCellValue(beneficiario.Ficha.EscuelaFicha.LocalidadEscuela.NombreLocalidad);
                        var cellOtrosectorterc = row.CreateCell(38); cellOtrosectorterc.SetCellValue(beneficiario.Ficha.OtroSectorTerciaria);
                        var cellCarreraterc = row.CreateCell(39); cellCarreraterc.SetCellValue(beneficiario.Ficha.CarreraFicha.NombreCarrera);
                        var cellIdnivelterc = row.CreateCell(40); cellIdnivelterc.SetCellValue(beneficiario.Ficha.CarreraFicha.IdNivel == null ?
                            String.Empty : beneficiario.Ficha.CarreraFicha.NivelCarrera.IdNivel.ToString());
                        var cellNivelterc = row.CreateCell(41); cellNivelterc.SetCellValue(beneficiario.Ficha.CarreraFicha.NivelCarrera.NombreNivel);
                        var cellIdinstitucionterc = row.CreateCell(42); cellIdinstitucionterc.SetCellValue(beneficiario.Ficha.CarreraFicha.IdInstitucion == null ?
                            String.Empty : beneficiario.Ficha.CarreraFicha.InstitucionCarrera.IdInstitucion.ToString());
                        var cellInstitucionterc = row.CreateCell(43); cellInstitucionterc.SetCellValue(beneficiario.Ficha.CarreraFicha.InstitucionCarrera.NombreInstitucion);
                        var cellIdsectorterc = row.CreateCell(44); cellIdsectorterc.SetCellValue(beneficiario.Ficha.CarreraFicha.IdSector == null ? String.Empty
                            : beneficiario.Ficha.CarreraFicha.SectorCarrera.IdSector.ToString());
                        var cellSectorterc = row.CreateCell(45); cellSectorterc.SetCellValue(beneficiario.Ficha.CarreraFicha.SectorCarrera.NombreSector);
                        var cellFechaModif = row.CreateCell(46); cellFechaModif.SetCellValue(beneficiario.Ficha.FechaSistema == null ? String.Empty
                            : beneficiario.Ficha.FechaSistema.Value.ToShortDateString());

                        //Beneficiario
                        var cellIdben = row.CreateCell(47); cellIdben.SetCellValue(beneficiario.IdBeneficiario);
                        var cellIdestadoben = row.CreateCell(48); cellIdestadoben.SetCellValue(beneficiario.IdEstado);
                        var cellEstadoben = row.CreateCell(49); cellEstadoben.SetCellValue(beneficiario.NombreEstado);
                        var cellNrocuentaben = row.CreateCell(50); cellNrocuentaben.SetCellValue(beneficiario.NumeroCuenta == null ? String.Empty
                            : beneficiario.NumeroCuenta.ToString());
                        var cellIdsucursalben = row.CreateCell(51); cellIdsucursalben.SetCellValue(beneficiario.IdSucursal == null ? String.Empty
                            : beneficiario.IdSucursal.ToString());
                        var cellCodsucursalben = row.CreateCell(52); cellCodsucursalben.SetCellValue(beneficiario.CodigoSucursal);
                        var cellSucursalben = row.CreateCell(53); cellSucursalben.SetCellValue(beneficiario.SucursalDescripcion);
                        var cellFecsolben = row.CreateCell(54); cellFecsolben.SetCellValue(beneficiario.FechaSolicitudCuenta == null ? String.Empty
                            : beneficiario.FechaSolicitudCuenta.Value.ToShortDateString());
                        var cellNotificado = row.CreateCell(55); cellNotificado.SetCellValue(beneficiario.Notificado ? "S" : "N");
                        var cellFecNotif = row.CreateCell(56); cellFecNotif.SetCellValue(beneficiario.FechaNotificacion == null ? String.Empty
                            : beneficiario.FechaNotificacion.Value.ToShortDateString());

                        var cellFechaIniBenf = row.CreateCell(57); cellFechaIniBenf.SetCellValue(beneficiario.FechaInicioBeneficio == null ? string.Empty : beneficiario.FechaInicioBeneficio.Value.ToShortDateString()); // 20/03/2013 - DI CAMPLI LEANDRO
                        var cellBajaBenef = row.CreateCell(58); cellBajaBenef.SetCellValue(beneficiario.FechaBajaBeneficio == null ? string.Empty : beneficiario.FechaBajaBeneficio.Value.ToShortDateString()); // 20/03/2013 - DI CAMPLI LEANDRO

                        var cellTieneapoben = row.CreateCell(59); cellTieneapoben.SetCellValue(beneficiario.TieneApoderado);

                        //Apoderado
                        var cellIdapoderado = row.CreateCell(60); cellIdapoderado.SetCellValue(beneficiario.Apoderado.IdApoderadoNullable == null ?
                            String.Empty : beneficiario.Apoderado.IdApoderadoNullable.ToString());
                        var cellApellidoapo = row.CreateCell(61); cellApellidoapo.SetCellValue(beneficiario.Apoderado.Apellido);
                        var cellNombreapo = row.CreateCell(62); cellNombreapo.SetCellValue(beneficiario.Apoderado.Nombre);
                        var cellDniapo = row.CreateCell(63); cellDniapo.SetCellValue(beneficiario.Apoderado.NumeroDocumento);
                        var cellCuilapo = row.CreateCell(64); cellCuilapo.SetCellValue(beneficiario.Apoderado.Cuil);
                        var cellSexoapo = row.CreateCell(65); cellSexoapo.SetCellValue(beneficiario.Apoderado.Sexo);
                        var cellNroctaapo = row.CreateCell(66); cellNroctaapo.SetCellValue(beneficiario.Apoderado.NumeroCuentaBco == null ? String.Empty
                            : beneficiario.Apoderado.NumeroCuentaBco.ToString());
                        var cellIdSuc = row.CreateCell(67); cellIdSuc.SetCellValue(beneficiario.Apoderado.IdSucursal == null ? String.Empty
                            : beneficiario.Apoderado.SucursalApoderado.IdSucursal.ToString());
                        var cellCodsucapo = row.CreateCell(68); cellCodsucapo.SetCellValue(beneficiario.Apoderado.SucursalApoderado.CodigoBanco);
                        var cellSucursalapo = row.CreateCell(69); cellSucursalapo.SetCellValue(beneficiario.Apoderado.SucursalApoderado.Detalle);
                        var cellFecsolcuentaapo = row.CreateCell(70); cellFecsolcuentaapo.SetCellValue(beneficiario.Apoderado.FechaSolicitudCuenta == null ?
                            String.Empty : beneficiario.Apoderado.FechaSolicitudCuenta.Value.ToShortDateString());
                        var cellFecnac = row.CreateCell(71); cellFecnac.SetCellValue(beneficiario.Apoderado.FechaNacimiento == null ? String.Empty :
                            beneficiario.Apoderado.FechaNacimiento.Value.ToShortDateString());
                        var cellCalleapo = row.CreateCell(72); cellCalleapo.SetCellValue(beneficiario.Apoderado.Calle);
                        var cellNroapo = row.CreateCell(73); cellNroapo.SetCellValue(beneficiario.Apoderado.Numero);
                        var cellPisoapo = row.CreateCell(74); cellPisoapo.SetCellValue(beneficiario.Apoderado.Piso);
                        var cellMonoblockapo = row.CreateCell(75); cellMonoblockapo.SetCellValue(beneficiario.Apoderado.Monoblock);
                        var cellDptoapo = row.CreateCell(76); cellDptoapo.SetCellValue(beneficiario.Apoderado.Dpto);
                        var cellParcelaapo = row.CreateCell(77); cellParcelaapo.SetCellValue(beneficiario.Apoderado.Parcela);
                        var cellManzanaapo = row.CreateCell(78); cellManzanaapo.SetCellValue(beneficiario.Apoderado.Manzana);
                        var cellEntrecalleapo = row.CreateCell(79); cellEntrecalleapo.SetCellValue(beneficiario.Apoderado.EntreCalles);
                        var cellBarrioapo = row.CreateCell(80); cellBarrioapo.SetCellValue(beneficiario.Apoderado.Barrio);
                        var cellCpapo = row.CreateCell(81); cellCpapo.SetCellValue(beneficiario.Apoderado.CodigoPostal);
                        var cellIdlocalidadapo = row.CreateCell(82); cellIdlocalidadapo.SetCellValue(beneficiario.Apoderado.IdLocalidad == null ?
                            String.Empty : beneficiario.Apoderado.LocalidadApoderado.IdLocalidad.ToString());
                        var cellLocalidadapo = row.CreateCell(83); cellLocalidadapo.SetCellValue(beneficiario.Apoderado.LocalidadApoderado.NombreLocalidad);
                        var cellIddeptoapo = row.CreateCell(84); cellIddeptoapo.SetCellValue(beneficiario.Apoderado.IdLocalidad == null ? String.Empty
                            : beneficiario.Apoderado.LocalidadApoderado.Departamento.IdDepartamento.ToString());
                        var cellDeptoapo = row.CreateCell(85); cellDeptoapo.SetCellValue(beneficiario.Apoderado.LocalidadApoderado.Departamento.NombreDepartamento);
                        var cellTelefonoapo = row.CreateCell(86); cellTelefonoapo.SetCellValue(beneficiario.Apoderado.TelefonoFijo);
                        var cellCelularapo = row.CreateCell(87); cellCelularapo.SetCellValue(beneficiario.Apoderado.TelefonoCelular);
                        var cellEmailapo = row.CreateCell(88);
                        cellEmailapo.SetCellValue(beneficiario.Apoderado.Mail);
                    }
                    #endregion
                    break;
                case (int)Enums.TipoFicha.Universitaria:
                    #region case (int)Enums.TipoFicha.Universitaria
                    foreach (var beneficiario in listaparaExcel)
                    {
                        i++;
                        //Ficha
                        var row = sheet.CreateRow(i);
                        var cellidficha = row.CreateCell(0); cellidficha.SetCellValue(beneficiario.Ficha.IdFicha);
                        var cellApellido = row.CreateCell(1); cellApellido.SetCellValue(beneficiario.Ficha.Apellido);
                        var cellNombre = row.CreateCell(2); cellNombre.SetCellValue(beneficiario.Ficha.Nombre);
                        var cellCuil = row.CreateCell(3); cellCuil.SetCellValue(beneficiario.Ficha.Cuil);
                        var cellDni = row.CreateCell(4); cellDni.SetCellValue(beneficiario.Ficha.NumeroDocumento);
                        var cellFechaNa = row.CreateCell(5); cellFechaNa.SetCellValue(beneficiario.Ficha.FechaNacimiento == null ? String.Empty
                            : beneficiario.Ficha.FechaNacimiento.Value.ToShortDateString());
                        var cellCalle = row.CreateCell(6); cellCalle.SetCellValue(beneficiario.Ficha.Calle);
                        var cellNro = row.CreateCell(7); cellNro.SetCellValue(beneficiario.Ficha.Numero);
                        var cellPiso = row.CreateCell(8); cellPiso.SetCellValue(beneficiario.Ficha.Piso);
                        var cellDpto = row.CreateCell(9); cellDpto.SetCellValue(beneficiario.Ficha.Dpto);
                        var cellMono = row.CreateCell(10); cellMono.SetCellValue(beneficiario.Ficha.Monoblock);
                        var cellParcela = row.CreateCell(11); cellParcela.SetCellValue(beneficiario.Ficha.Parcela);
                        var cellManzana = row.CreateCell(12); cellManzana.SetCellValue(beneficiario.Ficha.Manzana);
                        var cellEntreCalle = row.CreateCell(13); cellEntreCalle.SetCellValue(beneficiario.Ficha.EntreCalles);
                        var cellBarrio = row.CreateCell(14); cellBarrio.SetCellValue(beneficiario.Ficha.Barrio);
                        var cellCp = row.CreateCell(15); cellCp.SetCellValue(beneficiario.Ficha.CodigoPostal);
                        var cellidLocalidad = row.CreateCell(16); cellidLocalidad.SetCellValue(beneficiario.Ficha.IdLocalidad == null ?
                            String.Empty : beneficiario.Ficha.LocalidadFicha.IdLocalidad.ToString());
                        var cellnLocalidad = row.CreateCell(17); cellnLocalidad.SetCellValue(beneficiario.Ficha.LocalidadFicha.NombreLocalidad);
                        var cellIdDepartamento = row.CreateCell(18); cellIdDepartamento.SetCellValue(beneficiario.Ficha.IdLocalidad == null ?
                            String.Empty : beneficiario.Ficha.LocalidadFicha.Departamento.IdDepartamento.ToString());
                        var cellnDepartamento = row.CreateCell(19); cellnDepartamento.SetCellValue(beneficiario.Ficha.LocalidadFicha.Departamento.NombreDepartamento);
                        var cellTelFijo = row.CreateCell(20); cellTelFijo.SetCellValue(beneficiario.Ficha.TelefonoFijo);
                        var cellTelCelular = row.CreateCell(21); cellTelCelular.SetCellValue(beneficiario.Ficha.TelefonoCelular);
                        var cellContacto = row.CreateCell(22); cellContacto.SetCellValue(beneficiario.Ficha.Contacto);
                        var cellEmail = row.CreateCell(23); cellEmail.SetCellValue(beneficiario.Ficha.Mail);
                        var cellSexo = row.CreateCell(24); cellSexo.SetCellValue(beneficiario.Ficha.Sexo);
                        var cellidestadoFicha = row.CreateCell(25); cellidestadoFicha.SetCellValue(beneficiario.Ficha.IdEstadoFicha == null ? String.Empty :
                            beneficiario.Ficha.IdEstadoFicha.ToString());
                        var cellnestadoFicha = row.CreateCell(26); cellnestadoFicha.SetCellValue(beneficiario.Ficha.NombreEstadoFicha);

                        var cellPromedioUniv = row.CreateCell(27); cellPromedioUniv.SetCellValue(beneficiario.Ficha.PromedioUniversitaria == null ? String.Empty
                            : beneficiario.Ficha.PromedioUniversitaria.ToString());
                        var cellOtraCarreraUniv = row.CreateCell(28); cellOtraCarreraUniv.SetCellValue(beneficiario.Ficha.OtraCarreraUniversitaria);
                        var cellOtraInstiUniv = row.CreateCell(29); cellOtraInstiUniv.SetCellValue(beneficiario.Ficha.OtraInstitucionUniversitaria);
                        var cellIdCarreraUniv = row.CreateCell(30); cellIdCarreraUniv.SetCellValue(beneficiario.Ficha.IdCarreraUniversitaria == null ?
                            String.Empty : beneficiario.Ficha.CarreraFicha.IdCarrera.ToString());
                        var cellCarreraUniv = row.CreateCell(31); cellCarreraUniv.SetCellValue(beneficiario.Ficha.CarreraFicha.NombreCarrera);
                        var cellIdEscuelaUniv = row.CreateCell(32); cellIdEscuelaUniv.SetCellValue(beneficiario.Ficha.IdEscuelaUniversitaria == null ?
                            String.Empty : beneficiario.Ficha.EscuelaFicha.Id_Escuela.ToString());
                        var cellEscuelaUniv = row.CreateCell(33); cellEscuelaUniv.SetCellValue(beneficiario.Ficha.EscuelaFicha.Nombre_Escuela);
                        var cellCueUniv = row.CreateCell(34); cellCueUniv.SetCellValue(beneficiario.Ficha.EscuelaFicha.Cue);
                        var cellAnexoUniv = row.CreateCell(35); cellAnexoUniv.SetCellValue(beneficiario.Ficha.EscuelaFicha.Anexo == null ?
                            String.Empty : beneficiario.Ficha.EscuelaFicha.Anexo.ToString());

                        var cellEscBarrioUniv = row.CreateCell(36); cellEscBarrioUniv.SetCellValue(beneficiario.Ficha.EscuelaFicha.Barrio);
                        var cellEscIdLocalidadUniv = row.CreateCell(37); cellEscIdLocalidadUniv.SetCellValue(beneficiario.Ficha.EscuelaFicha.Id_Localidad == null ?
                            String.Empty : beneficiario.Ficha.EscuelaFicha.LocalidadEscuela.IdLocalidad.ToString());
                        var cellEscLocalidadUniv = row.CreateCell(38); cellEscLocalidadUniv.SetCellValue(beneficiario.Ficha.EscuelaFicha.LocalidadEscuela.NombreLocalidad);
                        var cellOtroSectorUniv = row.CreateCell(39); cellOtroSectorUniv.SetCellValue(beneficiario.Ficha.OtroSectorTerciaria);
                        var cellIdNivelUniv = row.CreateCell(40); cellIdNivelUniv.SetCellValue(beneficiario.Ficha.CarreraFicha.IdNivel == null ?
                            String.Empty : beneficiario.Ficha.CarreraFicha.NivelCarrera.IdNivel.ToString());
                        var cellNivelUniv = row.CreateCell(41); cellNivelUniv.SetCellValue(beneficiario.Ficha.CarreraFicha.NivelCarrera.NombreNivel);
                        var cellIdInstitucionUniv = row.CreateCell(42); cellIdInstitucionUniv.SetCellValue(beneficiario.Ficha.CarreraFicha.IdInstitucion == null ?
                            String.Empty : beneficiario.Ficha.CarreraFicha.InstitucionCarrera.IdInstitucion.ToString());
                        var cellInstitucionUniv = row.CreateCell(43); cellInstitucionUniv.SetCellValue(beneficiario.Ficha.CarreraFicha.InstitucionCarrera.NombreInstitucion);
                        var cellIdSectorUniv = row.CreateCell(44); cellIdSectorUniv.SetCellValue(beneficiario.Ficha.CarreraFicha.IdSector == null ?
                            String.Empty : beneficiario.Ficha.CarreraFicha.SectorCarrera.IdSector.ToString());
                        var cellSectorUniv = row.CreateCell(45); cellSectorUniv.SetCellValue(beneficiario.Ficha.CarreraFicha.SectorCarrera.NombreSector);
                        var cellFechaModif = row.CreateCell(46); cellFechaModif.SetCellValue(beneficiario.Ficha.FechaSistema == null ? String.Empty
                            : beneficiario.Ficha.FechaSistema.Value.ToShortDateString());

                        //Beneficiario
                        var cellIdben = row.CreateCell(47); cellIdben.SetCellValue(beneficiario.IdBeneficiario);
                        var cellIdestadoben = row.CreateCell(48); cellIdestadoben.SetCellValue(beneficiario.IdEstado);
                        var cellEstadoben = row.CreateCell(49); cellEstadoben.SetCellValue(beneficiario.NombreEstado);
                        var cellNrocuentaben = row.CreateCell(50); cellNrocuentaben.SetCellValue(beneficiario.NumeroCuenta == null ? String.Empty
                            : beneficiario.NumeroCuenta.ToString());
                        var cellIdsucursalben = row.CreateCell(51); cellIdsucursalben.SetCellValue(beneficiario.IdSucursal == null ? String.Empty
                            : beneficiario.IdSucursal.ToString());
                        var cellCodsucursalben = row.CreateCell(52); cellCodsucursalben.SetCellValue(beneficiario.CodigoSucursal);
                        var cellSucursalben = row.CreateCell(53); cellSucursalben.SetCellValue(beneficiario.SucursalDescripcion);
                        var cellFecsolben = row.CreateCell(54); cellFecsolben.SetCellValue(beneficiario.FechaSolicitudCuenta == null ?
                            String.Empty : beneficiario.FechaSolicitudCuenta.Value.ToShortDateString());
                        var cellNotificado = row.CreateCell(55); cellNotificado.SetCellValue(beneficiario.Notificado ? "S" : "N");
                        var cellFecNotif = row.CreateCell(56); cellFecNotif.SetCellValue(beneficiario.FechaNotificacion.ToString() ?? "");

                        var cellFechaIniBenf = row.CreateCell(57); cellFechaIniBenf.SetCellValue(beneficiario.FechaInicioBeneficio == null ? string.Empty : beneficiario.FechaInicioBeneficio.Value.ToShortDateString()); // 20/03/2013 - DI CAMPLI LEANDRO
                        var cellBajaBenef = row.CreateCell(58); cellBajaBenef.SetCellValue(beneficiario.FechaBajaBeneficio == null ? string.Empty : beneficiario.FechaBajaBeneficio.Value.ToShortDateString()); // 20/03/2013 - DI CAMPLI LEANDRO

                        var cellTieneapoben = row.CreateCell(59); cellTieneapoben.SetCellValue(beneficiario.TieneApoderado);

                        //Apoderado
                        var cellIdapoderado = row.CreateCell(60); cellIdapoderado.SetCellValue(beneficiario.Apoderado.IdApoderadoNullable == null ?
                            String.Empty : beneficiario.Apoderado.IdApoderadoNullable.ToString());
                        var cellApellidoapo = row.CreateCell(61); cellApellidoapo.SetCellValue(beneficiario.Apoderado.Apellido);
                        var cellNombreapo = row.CreateCell(62); cellNombreapo.SetCellValue(beneficiario.Apoderado.Nombre);
                        var cellDniapo = row.CreateCell(63); cellDniapo.SetCellValue(beneficiario.Apoderado.NumeroDocumento);
                        var cellCuilapo = row.CreateCell(64); cellCuilapo.SetCellValue(beneficiario.Apoderado.Cuil);
                        var cellSexoapo = row.CreateCell(65); cellSexoapo.SetCellValue(beneficiario.Apoderado.Sexo);
                        var cellNroctaapo = row.CreateCell(66); cellNroctaapo.SetCellValue(beneficiario.Apoderado.NumeroCuentaBco == null ? String.Empty
                            : beneficiario.Apoderado.NumeroCuentaBco.ToString());
                        var cellIdSuc = row.CreateCell(67); cellIdSuc.SetCellValue(beneficiario.Apoderado.IdSucursal == null ? String.Empty
                            : beneficiario.Apoderado.SucursalApoderado.IdSucursal.ToString());
                        var cellCodsucapo = row.CreateCell(68); cellCodsucapo.SetCellValue(beneficiario.Apoderado.SucursalApoderado.CodigoBanco);
                        var cellSucursalapo = row.CreateCell(69); cellSucursalapo.SetCellValue(beneficiario.Apoderado.SucursalApoderado.Detalle);
                        var cellFecsolcuentaapo = row.CreateCell(70); cellFecsolcuentaapo.SetCellValue(beneficiario.Apoderado.FechaSolicitudCuenta == null ?
                            String.Empty : beneficiario.Apoderado.FechaSolicitudCuenta.Value.ToShortDateString());
                        var cellFecnac = row.CreateCell(71); cellFecnac.SetCellValue(beneficiario.Apoderado.FechaNacimiento == null ? String.Empty :
                            beneficiario.Apoderado.FechaNacimiento.Value.ToShortDateString());
                        var cellCalleapo = row.CreateCell(72); cellCalleapo.SetCellValue(beneficiario.Apoderado.Calle);
                        var cellNroapo = row.CreateCell(73); cellNroapo.SetCellValue(beneficiario.Apoderado.Numero);
                        var cellPisoapo = row.CreateCell(74); cellPisoapo.SetCellValue(beneficiario.Apoderado.Piso);
                        var cellMonoblockapo = row.CreateCell(75); cellMonoblockapo.SetCellValue(beneficiario.Apoderado.Monoblock);
                        var cellDptoapo = row.CreateCell(76); cellDptoapo.SetCellValue(beneficiario.Apoderado.Dpto);
                        var cellParcelaapo = row.CreateCell(77); cellParcelaapo.SetCellValue(beneficiario.Apoderado.Parcela);
                        var cellManzanaapo = row.CreateCell(78); cellManzanaapo.SetCellValue(beneficiario.Apoderado.Manzana);
                        var cellEntrecalleapo = row.CreateCell(79); cellEntrecalleapo.SetCellValue(beneficiario.Apoderado.EntreCalles);
                        var cellBarrioapo = row.CreateCell(80); cellBarrioapo.SetCellValue(beneficiario.Apoderado.Barrio);
                        var cellCpapo = row.CreateCell(81); cellCpapo.SetCellValue(beneficiario.Apoderado.CodigoPostal);
                        var cellIdlocalidadapo = row.CreateCell(82); cellIdlocalidadapo.SetCellValue(beneficiario.Apoderado.IdLocalidad == null ?
                            String.Empty : beneficiario.Apoderado.LocalidadApoderado.IdLocalidad.ToString());
                        var cellLocalidadapo = row.CreateCell(83); cellLocalidadapo.SetCellValue(beneficiario.Apoderado.LocalidadApoderado.NombreLocalidad);
                        var cellIddeptoapo = row.CreateCell(84); cellIddeptoapo.SetCellValue(beneficiario.Apoderado.IdLocalidad == null ?
                            String.Empty : beneficiario.Apoderado.LocalidadApoderado.Departamento.IdDepartamento.ToString());
                        var cellDeptoapo = row.CreateCell(85); cellDeptoapo.SetCellValue(beneficiario.Apoderado.LocalidadApoderado.Departamento.NombreDepartamento);
                        var cellTelefonoapo = row.CreateCell(86); cellTelefonoapo.SetCellValue(beneficiario.Apoderado.TelefonoFijo);
                        var cellCelularapo = row.CreateCell(87); cellCelularapo.SetCellValue(beneficiario.Apoderado.TelefonoCelular);
                        var cellEmailapo = row.CreateCell(88); cellEmailapo.SetCellValue(beneficiario.Apoderado.Mail);
                    }
                    #endregion
                    break;
                case (int)Enums.TipoFicha.Ppp:
                    #region case (int)Enums.TipoFicha.Ppp
                    foreach (var beneficiario in listaparaExcel)
                    {
                        i++;
                        //Ficha
                        var row = sheet.CreateRow(i);
                        var cellidficha = row.CreateCell(0); cellidficha.SetCellValue(beneficiario.Ficha.IdFicha);
                        var cellApellido = row.CreateCell(1); cellApellido.SetCellValue(beneficiario.Ficha.Apellido);
                        var cellNombre = row.CreateCell(2); cellNombre.SetCellValue(beneficiario.Ficha.Nombre);
                        var cellCuil = row.CreateCell(3); cellCuil.SetCellValue(beneficiario.Ficha.Cuil);
                        var cellDni = row.CreateCell(4); cellDni.SetCellValue(beneficiario.Ficha.NumeroDocumento);
                        var cellFechaNa = row.CreateCell(5); cellFechaNa.SetCellValue(beneficiario.Ficha.FechaNacimiento == null ? String.Empty
                            : beneficiario.Ficha.FechaNacimiento.Value.ToShortDateString());
                        var cellCalle = row.CreateCell(6); cellCalle.SetCellValue(beneficiario.Ficha.Calle);
                        var cellNro = row.CreateCell(7); cellNro.SetCellValue(beneficiario.Ficha.Numero);
                        var cellPiso = row.CreateCell(8); cellPiso.SetCellValue(beneficiario.Ficha.Piso);
                        var cellDpto = row.CreateCell(9); cellDpto.SetCellValue(beneficiario.Ficha.Dpto);
                        var cellMono = row.CreateCell(10); cellMono.SetCellValue(beneficiario.Ficha.Monoblock);
                        var cellParcela = row.CreateCell(11); cellParcela.SetCellValue(beneficiario.Ficha.Parcela);
                        var cellManzana = row.CreateCell(12); cellManzana.SetCellValue(beneficiario.Ficha.Manzana);
                        var cellEntreCalle = row.CreateCell(13); cellEntreCalle.SetCellValue(beneficiario.Ficha.EntreCalles);
                        var cellBarrio = row.CreateCell(14); cellBarrio.SetCellValue(beneficiario.Ficha.Barrio);
                        var cellCp = row.CreateCell(15); cellCp.SetCellValue(beneficiario.Ficha.CodigoPostal);
                        var cellidLocalidad = row.CreateCell(16); cellidLocalidad.SetCellValue(beneficiario.Ficha.IdLocalidad == null ?
                            String.Empty : beneficiario.Ficha.LocalidadFicha.IdLocalidad.ToString());
                        var cellnLocalidad = row.CreateCell(17); cellnLocalidad.SetCellValue(beneficiario.Ficha.LocalidadFicha.NombreLocalidad);
                        var cellIdDepartamento = row.CreateCell(18); cellIdDepartamento.SetCellValue(beneficiario.Ficha.IdLocalidad == null ?
                            String.Empty : beneficiario.Ficha.LocalidadFicha.Departamento.IdDepartamento.ToString());
                        var cellnDepartamento = row.CreateCell(19); cellnDepartamento.SetCellValue(beneficiario.Ficha.LocalidadFicha.Departamento.NombreDepartamento);
                        var cellTelFijo = row.CreateCell(20); cellTelFijo.SetCellValue(beneficiario.Ficha.TelefonoFijo);
                        var cellTelCelular = row.CreateCell(21); cellTelCelular.SetCellValue(beneficiario.Ficha.TelefonoCelular);
                        var cellContacto = row.CreateCell(22); cellContacto.SetCellValue(beneficiario.Ficha.Contacto);
                        var cellEmail = row.CreateCell(23); cellEmail.SetCellValue(beneficiario.Ficha.Mail);

                        var cellTieneHijos = row.CreateCell(24); cellTieneHijos.SetCellValue(beneficiario.Ficha.TieneHijos);//TIENE_HIJOS	
                        var cellCantidadHijos = row.CreateCell(25); cellCantidadHijos.SetCellValue(beneficiario.Ficha.CantidadHijos == null ? "0" : beneficiario.Ficha.CantidadHijos.ToString());//CANTIDAD_HIJOS
                        var cellEsDiscapacitado = row.CreateCell(26); cellEsDiscapacitado.SetCellValue(beneficiario.Ficha.EsDiscapacitado == true ? "S" : "N");//ES_DIACAPACITADO
                        var cellTieneDeficienciaMotora = row.CreateCell(27); cellTieneDeficienciaMotora.SetCellValue(beneficiario.Ficha.TieneDeficienciaMotora == true ? "S" : "N");//TIENE_DEF_MOTORA
                        var cellTieneDeficienciaMental = row.CreateCell(28); cellTieneDeficienciaMental.SetCellValue(beneficiario.Ficha.TieneDeficienciaMental == true ? "S" : "N");//TIENE_DEF_MENTAL
                        var cellTieneDeficienciaSensorial = row.CreateCell(29); cellTieneDeficienciaSensorial.SetCellValue(beneficiario.Ficha.TieneDeficienciaSensorial == true ? "S" : "N");//TIENE_DEF_SENSORIAL
                        var cellTieneDeficienciaPsicologia = row.CreateCell(30); cellTieneDeficienciaPsicologia.SetCellValue(beneficiario.Ficha.TieneDeficienciaPsicologia == true ? "S" : "N");//TIENE_DEF_PSICOLOGICA
                        var cellDeficienciaOtra = row.CreateCell(31); cellDeficienciaOtra.SetCellValue(beneficiario.Ficha.DeficienciaOtra);//DEFICIENCIA_OTRA
                        var cellCertificadoDiscapacidad = row.CreateCell(32); cellCertificadoDiscapacidad.SetCellValue(beneficiario.Ficha.CertificadoDiscapacidad == true ? "S" : "N");//CERTIF_DISCAP



                        var cellSexo = row.CreateCell(33); cellSexo.SetCellValue(beneficiario.Ficha.Sexo);

                        var cellTipoFicha = row.CreateCell(34); cellTipoFicha.SetCellValue(beneficiario.Ficha.TipoFicha.ToString() ?? "0");//TIPO_FICHA
                        var cellEstadoCivilDesc = row.CreateCell(35); cellEstadoCivilDesc.SetCellValue(beneficiario.Ficha.EstadoCivilDesc);//ESTADO_CIVIL
                        var cellFechaSistema = row.CreateCell(36); cellFechaSistema.SetCellValue(beneficiario.Ficha.FechaSistema == null ? String.Empty : beneficiario.Ficha.FechaSistema.Value.ToString());//FECH_SIST
                        var cellOrigenCarga = row.CreateCell(37); cellOrigenCarga.SetCellValue(beneficiario.Ficha.OrigenCarga == "M" ? "Cargappp" : "Web");//ORIGEN_CARGA
                        var cellNroTramite = row.CreateCell(38); cellNroTramite.SetCellValue(beneficiario.Ficha.NroTramite ?? "");//NRO_TRAMITE
                        var cellidCaja = row.CreateCell(39); cellidCaja.SetCellValue(beneficiario.Ficha.idCaja == null ? "" : beneficiario.Ficha.idCaja.ToString());//ID_CAJA,
                        var cellIdTipoRechazo = row.CreateCell(40); cellIdTipoRechazo.SetCellValue(beneficiario.Ficha.IdTipoRechazo == null ? "" : beneficiario.Ficha.IdTipoRechazo.ToString());//ID_TIPO_RECHAZO,
                        var cellrechazo = row.CreateCell(41); cellrechazo.SetCellValue(beneficiario.Ficha.rechazo ?? "");//.N_TIPO_RECHAZO,

                        var cellidestadoFicha = row.CreateCell(42); cellidestadoFicha.SetCellValue(beneficiario.Ficha.IdEstadoFicha == null ? String.Empty :
                            beneficiario.Ficha.IdEstadoFicha.ToString());
                        var cellnestadoFicha = row.CreateCell(43); cellnestadoFicha.SetCellValue(beneficiario.Ficha.NombreEstadoFicha);
                        var cellModalidad = row.CreateCell(44); cellModalidad.SetCellValue(beneficiario.Ficha.FichaPpp.Modalidad == (int)Enums.Modalidad.Entrenamiento
                            ? "Entrenamiento" : "CTI");
                        var cellTarea = row.CreateCell(45); cellTarea.SetCellValue(beneficiario.Ficha.FichaPpp.Tareas);
                        var cellaltaTemprana = row.CreateCell(46); cellaltaTemprana.SetCellValue(beneficiario.Ficha.FichaPpp.AltaTemprana);
                        var cellatFechaInicio = row.CreateCell(47); cellatFechaInicio.SetCellValue(beneficiario.Ficha.FichaPpp.FechaInicioActividad == null ?
                            String.Empty : beneficiario.Ficha.FichaPpp.FechaInicioActividad.Value.ToShortDateString());
                        var cellatFechaCese = row.CreateCell(48); cellatFechaCese.SetCellValue(beneficiario.Ficha.FichaPpp.FechaFinActividad == null ?
                            String.Empty : beneficiario.Ficha.FichaPpp.FechaFinActividad.Value.ToShortDateString());
                        var cellidModContAfip = row.CreateCell(49); cellidModContAfip.SetCellValue(beneficiario.Ficha.FichaPpp.IdModalidadAfip == null ?
                            String.Empty : beneficiario.Ficha.FichaPpp.IdModalidadAfip.ToString());
                        var cellModContAfip = row.CreateCell(50); cellModContAfip.SetCellValue(beneficiario.Ficha.FichaPpp.ModalidadAfip);

                        var cellFechaModif = row.CreateCell(51); cellFechaModif.SetCellValue(beneficiario.Ficha.FechaSistema == null ? String.Empty
                            : beneficiario.Ficha.FechaSistema.Value.ToShortDateString());

                        //Empresa
                        var cellidEmp = row.CreateCell(52); cellidEmp.SetCellValue(beneficiario.Ficha.FichaPpp.Empresa.IdEmpresa);
                        var cellrazonSocial = row.CreateCell(53); cellrazonSocial.SetCellValue(beneficiario.Ficha.FichaPpp.Empresa.NombreEmpresa);
                        var cellCuit = row.CreateCell(54); cellCuit.SetCellValue(beneficiario.Ficha.FichaPpp.Empresa.Cuit);
                        var cellCodact = row.CreateCell(55); cellCodact.SetCellValue(beneficiario.Ficha.FichaPpp.Empresa.CodigoActividad);
                        var cellCantemp = row.CreateCell(56); cellCantemp.SetCellValue(beneficiario.Ficha.FichaPpp.Empresa.CantidadEmpleados == null ?
                            String.Empty : beneficiario.Ficha.FichaPpp.Empresa.CantidadEmpleados.ToString());
                        var cellCalleemp = row.CreateCell(57); cellCalleemp.SetCellValue(beneficiario.Ficha.FichaPpp.Empresa.Calle);
                        var cellNumeroemp = row.CreateCell(58); cellNumeroemp.SetCellValue(beneficiario.Ficha.FichaPpp.Empresa.Numero);
                        var cellPisoemp = row.CreateCell(59); cellPisoemp.SetCellValue(beneficiario.Ficha.FichaPpp.Empresa.Piso);
                        var cellDptoemp = row.CreateCell(60); cellDptoemp.SetCellValue(beneficiario.Ficha.FichaPpp.Empresa.Dpto);
                        var cellCpemp = row.CreateCell(61); cellCpemp.SetCellValue(beneficiario.Ficha.FichaPpp.Empresa.CodigoPostal);
                        var cellIdlocemp = row.CreateCell(62); cellIdlocemp.SetCellValue(beneficiario.Ficha.FichaPpp.Empresa.IdLocalidad == null ?
                            String.Empty : beneficiario.Ficha.FichaPpp.Empresa.LocalidadEmpresa.IdLocalidad.ToString());
                        var cellLocemp = row.CreateCell(63); cellLocemp.SetCellValue(beneficiario.Ficha.FichaPpp.Empresa.LocalidadEmpresa.NombreLocalidad);
                        var cellDepartamentoEmp = row.CreateCell(64); cellDepartamentoEmp.SetCellValue(beneficiario.Ficha.FichaPpp.Empresa.IdLocalidad == null ?
                            String.Empty : beneficiario.Ficha.FichaPpp.Empresa.LocalidadEmpresa.Departamento.NombreDepartamento);
                        var cellIdusuarioemp = row.CreateCell(65); cellIdusuarioemp.SetCellValue(beneficiario.Ficha.FichaPpp.Empresa.Usuario.IdUsuarioEmpresa == null ?
                            String.Empty : beneficiario.Ficha.FichaPpp.Empresa.Usuario.IdUsuarioEmpresa.ToString());
                        var cellNombreusuemp = row.CreateCell(66); cellNombreusuemp.SetCellValue(beneficiario.Ficha.FichaPpp.Empresa.Usuario.NombreUsuario);
                        var cellApellidousuemp = row.CreateCell(67); cellApellidousuemp.SetCellValue(beneficiario.Ficha.FichaPpp.Empresa.Usuario.ApellidoUsuario);
                        var cellEmailusuemp = row.CreateCell(68); cellEmailusuemp.SetCellValue(beneficiario.Ficha.FichaPpp.Empresa.Usuario.Mail);
                        var cellTeleusuemp = row.CreateCell(69); cellTeleusuemp.SetCellValue(beneficiario.Ficha.FichaPpp.Empresa.Usuario.Telefono);

                        //Beneficiario
                        var cellIdben = row.CreateCell(70); cellIdben.SetCellValue(beneficiario.IdBeneficiario);
                        var cellIdestadoben = row.CreateCell(71); cellIdestadoben.SetCellValue(beneficiario.IdEstado);
                        var cellEstadoben = row.CreateCell(72); cellEstadoben.SetCellValue(beneficiario.NombreEstado);
                        var cellNrocuentaben = row.CreateCell(73); cellNrocuentaben.SetCellValue(beneficiario.NumeroCuenta == null ? String.Empty
                            : beneficiario.NumeroCuenta.ToString());
                        var cellIdsucursalben = row.CreateCell(74); cellIdsucursalben.SetCellValue(beneficiario.IdSucursal == null ? String.Empty
                            : beneficiario.IdSucursal.ToString());
                        var cellCodsucursalben = row.CreateCell(75); cellCodsucursalben.SetCellValue(beneficiario.CodigoSucursal);
                        var cellSucursalben = row.CreateCell(76); cellSucursalben.SetCellValue(beneficiario.SucursalDescripcion);
                        var cellFecsolben = row.CreateCell(77); cellFecsolben.SetCellValue(beneficiario.FechaSolicitudCuenta == null ? String.Empty
                            : beneficiario.FechaSolicitudCuenta.Value.ToShortDateString());
                        var cellNotificado = row.CreateCell(78); cellNotificado.SetCellValue(beneficiario.Notificado ? "S" : "N");
                        var cellFecNotif = row.CreateCell(79); cellFecNotif.SetCellValue(beneficiario.FechaNotificacion == null ? String.Empty
                            : beneficiario.FechaNotificacion.Value.ToShortDateString());

                        var cellFechaIniBenf = row.CreateCell(80); cellFechaIniBenf.SetCellValue(beneficiario.FechaInicioBeneficio == null ? string.Empty : beneficiario.FechaInicioBeneficio.Value.ToShortDateString()); // 20/03/2013 - DI CAMPLI LEANDRO
                        var cellBajaBenef = row.CreateCell(81); cellBajaBenef.SetCellValue(beneficiario.FechaBajaBeneficio == null ? string.Empty : beneficiario.FechaBajaBeneficio.Value.ToShortDateString()); // 20/03/2013 - DI CAMPLI LEANDRO

                        var cellTieneapoben = row.CreateCell(82); cellTieneapoben.SetCellValue(beneficiario.TieneApoderado);

                        //Apoderado
                        var cellIdapoderado = row.CreateCell(83); cellIdapoderado.SetCellValue(beneficiario.Apoderado.IdApoderadoNullable == null ?
                            String.Empty : beneficiario.Apoderado.IdApoderadoNullable.ToString());
                        var cellApellidoapo = row.CreateCell(84); cellApellidoapo.SetCellValue(beneficiario.Apoderado.Apellido);
                        var cellNombreapo = row.CreateCell(85); cellNombreapo.SetCellValue(beneficiario.Apoderado.Nombre);
                        var cellDniapo = row.CreateCell(86); cellDniapo.SetCellValue(beneficiario.Apoderado.NumeroDocumento);
                        var cellCuilapo = row.CreateCell(87); cellCuilapo.SetCellValue(beneficiario.Apoderado.Cuil);
                        var cellSexoapo = row.CreateCell(88); cellSexoapo.SetCellValue(beneficiario.Apoderado.Sexo);
                        var cellNroctaapo = row.CreateCell(89); cellNroctaapo.SetCellValue(beneficiario.Apoderado.NumeroCuentaBco == null ? String.Empty
                            : beneficiario.Apoderado.NumeroCuentaBco.ToString());
                        var cellIdSuc = row.CreateCell(90); cellIdSuc.SetCellValue(beneficiario.Apoderado.IdSucursal == null ? String.Empty
                            : beneficiario.Apoderado.SucursalApoderado.IdSucursal.ToString());
                        var cellCodsucapo = row.CreateCell(91); cellCodsucapo.SetCellValue(beneficiario.Apoderado.SucursalApoderado.CodigoBanco);
                        var cellSucursalapo = row.CreateCell(92); cellSucursalapo.SetCellValue(beneficiario.Apoderado.SucursalApoderado.Detalle);
                        var cellFecsolcuentaapo = row.CreateCell(93); cellFecsolcuentaapo.SetCellValue(beneficiario.Apoderado.FechaSolicitudCuenta == null ?
                            String.Empty : beneficiario.Apoderado.FechaSolicitudCuenta.Value.ToShortDateString());
                        var cellFecnac = row.CreateCell(94); cellFecnac.SetCellValue(beneficiario.Apoderado.FechaNacimiento == null ? String.Empty :
                            beneficiario.Apoderado.FechaNacimiento.Value.ToShortDateString());
                        var cellCalleapo = row.CreateCell(95); cellCalleapo.SetCellValue(beneficiario.Apoderado.Calle);
                        var cellNroapo = row.CreateCell(96); cellNroapo.SetCellValue(beneficiario.Apoderado.Numero);
                        var cellPisoapo = row.CreateCell(97); cellPisoapo.SetCellValue(beneficiario.Apoderado.Piso);
                        var cellMonoblockapo = row.CreateCell(98); cellMonoblockapo.SetCellValue(beneficiario.Apoderado.Monoblock);
                        var cellDptoapo = row.CreateCell(99); cellDptoapo.SetCellValue(beneficiario.Apoderado.Dpto);
                        var cellParcelaapo = row.CreateCell(100); cellParcelaapo.SetCellValue(beneficiario.Apoderado.Parcela);
                        var cellManzanaapo = row.CreateCell(101); cellManzanaapo.SetCellValue(beneficiario.Apoderado.Manzana);
                        var cellEntrecalleapo = row.CreateCell(102); cellEntrecalleapo.SetCellValue(beneficiario.Apoderado.EntreCalles);
                        var cellBarrioapo = row.CreateCell(103); cellBarrioapo.SetCellValue(beneficiario.Apoderado.Barrio);
                        var cellCpapo = row.CreateCell(105); cellCpapo.SetCellValue(beneficiario.Apoderado.CodigoPostal);
                        var cellIdlocalidadapo = row.CreateCell(105); cellIdlocalidadapo.SetCellValue(beneficiario.Apoderado.IdLocalidad == null ? String.Empty
                            : beneficiario.Apoderado.LocalidadApoderado.IdLocalidad.ToString());
                        var cellLocalidadapo = row.CreateCell(106); cellLocalidadapo.SetCellValue(beneficiario.Apoderado.LocalidadApoderado.NombreLocalidad);
                        var cellIddeptoapo = row.CreateCell(107); cellIddeptoapo.SetCellValue(beneficiario.Apoderado.IdLocalidad == null ? String.Empty
                            : beneficiario.Apoderado.LocalidadApoderado.Departamento.IdDepartamento.ToString());
                        var cellDeptoapo = row.CreateCell(108); cellDeptoapo.SetCellValue(beneficiario.Apoderado.LocalidadApoderado.Departamento.NombreDepartamento);
                        var cellTelefonoapo = row.CreateCell(109); cellTelefonoapo.SetCellValue(beneficiario.Apoderado.TelefonoFijo);
                        var cellCelularapo = row.CreateCell(110); cellCelularapo.SetCellValue(beneficiario.Apoderado.TelefonoCelular);
                        var cellEmailapo = row.CreateCell(111);
                        cellEmailapo.SetCellValue(beneficiario.Apoderado.Mail);
                    }
                    #endregion
                    break;
                case (int)Enums.TipoFicha.PppProf:
                    #region case (int)Enums.TipoFicha.PppProf
                    foreach (var beneficiario in listaparaExcel)
                    {
                        i++;
                        //Ficha
                        var row = sheet.CreateRow(i);
                        var cellidficha = row.CreateCell(0); cellidficha.SetCellValue(beneficiario.Ficha.IdFicha);
                        var cellApellido = row.CreateCell(1); cellApellido.SetCellValue(beneficiario.Ficha.Apellido);
                        var cellNombre = row.CreateCell(2); cellNombre.SetCellValue(beneficiario.Ficha.Nombre);
                        var cellCuil = row.CreateCell(3); cellCuil.SetCellValue(beneficiario.Ficha.Cuil);
                        var cellDni = row.CreateCell(4); cellDni.SetCellValue(beneficiario.Ficha.NumeroDocumento);
                        var cellFechaNa = row.CreateCell(5); cellFechaNa.SetCellValue(beneficiario.Ficha.FechaNacimiento == null ? String.Empty
                            : beneficiario.Ficha.FechaNacimiento.Value.ToShortDateString());
                        var cellCalle = row.CreateCell(6); cellCalle.SetCellValue(beneficiario.Ficha.Calle);
                        var cellNro = row.CreateCell(7); cellNro.SetCellValue(beneficiario.Ficha.Numero);
                        var cellPiso = row.CreateCell(8); cellPiso.SetCellValue(beneficiario.Ficha.Piso);
                        var cellDpto = row.CreateCell(9); cellDpto.SetCellValue(beneficiario.Ficha.Dpto);
                        var cellMono = row.CreateCell(10); cellMono.SetCellValue(beneficiario.Ficha.Monoblock);
                        var cellParcela = row.CreateCell(11); cellParcela.SetCellValue(beneficiario.Ficha.Parcela);
                        var cellManzana = row.CreateCell(12); cellManzana.SetCellValue(beneficiario.Ficha.Manzana);
                        var cellEntreCalle = row.CreateCell(13); cellEntreCalle.SetCellValue(beneficiario.Ficha.EntreCalles);
                        var cellBarrio = row.CreateCell(14); cellBarrio.SetCellValue(beneficiario.Ficha.Barrio);
                        var cellCp = row.CreateCell(15); cellCp.SetCellValue(beneficiario.Ficha.CodigoPostal);
                        var cellidLocalidad = row.CreateCell(16); cellidLocalidad.SetCellValue(beneficiario.Ficha.IdLocalidad == null ? String.Empty
                            : beneficiario.Ficha.LocalidadFicha.IdLocalidad.ToString());
                        var cellnLocalidad = row.CreateCell(17); cellnLocalidad.SetCellValue(beneficiario.Ficha.LocalidadFicha.NombreLocalidad);
                        var cellIdDepartamento = row.CreateCell(18); cellIdDepartamento.SetCellValue(beneficiario.Ficha.IdLocalidad == null ? String.Empty
                            : beneficiario.Ficha.LocalidadFicha.Departamento.IdDepartamento.ToString());
                        var cellnDepartamento = row.CreateCell(19); cellnDepartamento.SetCellValue(beneficiario.Ficha.LocalidadFicha.Departamento.NombreDepartamento);
                        var cellTelFijo = row.CreateCell(20); cellTelFijo.SetCellValue(beneficiario.Ficha.TelefonoFijo);
                        var cellTelCelular = row.CreateCell(21); cellTelCelular.SetCellValue(beneficiario.Ficha.TelefonoCelular);
                        var cellContacto = row.CreateCell(22); cellContacto.SetCellValue(beneficiario.Ficha.Contacto);
                        var cellEmail = row.CreateCell(23); cellEmail.SetCellValue(beneficiario.Ficha.Mail);
                        var cellSexo = row.CreateCell(24); cellSexo.SetCellValue(beneficiario.Ficha.Sexo);
                        var cellidestadoFicha = row.CreateCell(25); cellidestadoFicha.SetCellValue(beneficiario.Ficha.IdEstadoFicha == null ? String.Empty
                            : beneficiario.Ficha.IdEstadoFicha.ToString());
                        var cellnestadoFicha = row.CreateCell(26); cellnestadoFicha.SetCellValue(beneficiario.Ficha.NombreEstadoFicha);
                        var cellModalidad = row.CreateCell(27); cellModalidad.SetCellValue(beneficiario.Ficha.FichaPppp.Modalidad == (int)Enums.Modalidad.Entrenamiento
                            ? "Entrenamiento" : "CTI");
                        var cellTarea = row.CreateCell(28); cellTarea.SetCellValue(beneficiario.Ficha.FichaPppp.Tareas);
                        var cellaltaTemprana = row.CreateCell(29); cellaltaTemprana.SetCellValue(beneficiario.Ficha.FichaPppp.AltaTemprana);
                        var cellatFechaInicio = row.CreateCell(30); cellatFechaInicio.SetCellValue(beneficiario.Ficha.FichaPppp.FechaInicioActividad == null ?
                            String.Empty : beneficiario.Ficha.FichaPppp.FechaInicioActividad.Value.ToShortDateString());
                        var cellatFechaCese = row.CreateCell(31); cellatFechaCese.SetCellValue(beneficiario.Ficha.FichaPppp.FechaFinActividad == null ?
                            String.Empty : beneficiario.Ficha.FichaPppp.FechaFinActividad.Value.ToShortDateString());
                        var cellidModContAfip = row.CreateCell(32); cellidModContAfip.SetCellValue(beneficiario.Ficha.FichaPppp.IdModalidadAFIP == null ?
                            String.Empty : beneficiario.Ficha.FichaPppp.IdModalidadAFIP.ToString());
                        var cellModContAfip = row.CreateCell(33); cellModContAfip.SetCellValue(beneficiario.Ficha.FichaPppp.ModalidadAFIP);
                        var cellFechaModif = row.CreateCell(34); cellFechaModif.SetCellValue(beneficiario.Ficha.FechaSistema == null ? String.Empty
                            : beneficiario.Ficha.FechaSistema.Value.ToShortDateString());

                        //Empresa
                        var cellidEmp = row.CreateCell(35); cellidEmp.SetCellValue(beneficiario.Ficha.FichaPppp.Empresa.IdEmpresa);
                        var cellrazonSocial = row.CreateCell(36); cellrazonSocial.SetCellValue(beneficiario.Ficha.FichaPppp.Empresa.NombreEmpresa);
                        var cellCuit = row.CreateCell(37); cellCuit.SetCellValue(beneficiario.Ficha.FichaPppp.Empresa.Cuit);
                        var cellCodact = row.CreateCell(38); cellCodact.SetCellValue(beneficiario.Ficha.FichaPppp.Empresa.CodigoActividad);
                        var cellCantemp = row.CreateCell(39); cellCantemp.SetCellValue(beneficiario.Ficha.FichaPppp.Empresa.CantidadEmpleados == null ?
                            String.Empty : beneficiario.Ficha.FichaPppp.Empresa.CantidadEmpleados.ToString());
                        var cellCalleemp = row.CreateCell(40); cellCalleemp.SetCellValue(beneficiario.Ficha.FichaPppp.Empresa.Calle);
                        var cellNumeroemp = row.CreateCell(41); cellNumeroemp.SetCellValue(beneficiario.Ficha.FichaPppp.Empresa.Numero);
                        var cellPisoemp = row.CreateCell(42); cellPisoemp.SetCellValue(beneficiario.Ficha.FichaPppp.Empresa.Piso);
                        var cellDptoemp = row.CreateCell(42); cellDptoemp.SetCellValue(beneficiario.Ficha.FichaPppp.Empresa.Dpto);
                        var cellCpemp = row.CreateCell(44); cellCpemp.SetCellValue(beneficiario.Ficha.FichaPppp.Empresa.CodigoPostal);
                        var cellIdlocemp = row.CreateCell(45); cellIdlocemp.SetCellValue(beneficiario.Ficha.FichaPppp.Empresa.IdLocalidad == null ? String.Empty
                            : beneficiario.Ficha.FichaPppp.Empresa.LocalidadEmpresa.IdLocalidad.ToString());
                        var cellLocemp = row.CreateCell(46); cellLocemp.SetCellValue(beneficiario.Ficha.FichaPppp.Empresa.LocalidadEmpresa.NombreLocalidad);
                        var cellDepartEmp = row.CreateCell(47); cellDepartEmp.SetCellValue(beneficiario.Ficha.FichaPppp.Empresa.IdLocalidad == null ? String.Empty
                            : beneficiario.Ficha.FichaPppp.Empresa.LocalidadEmpresa.Departamento.NombreDepartamento);
                        var cellIdusuarioemp = row.CreateCell(48); cellIdusuarioemp.SetCellValue(beneficiario.Ficha.FichaPppp.Empresa.IdUsuario == null ?
                            String.Empty : beneficiario.Ficha.FichaPppp.Empresa.Usuario.IdUsuarioEmpresa.ToString());
                        var cellNombreusuemp = row.CreateCell(49); cellNombreusuemp.SetCellValue(beneficiario.Ficha.FichaPppp.Empresa.Usuario.NombreUsuario);
                        var cellApellidousuemp = row.CreateCell(50); cellApellidousuemp.SetCellValue(beneficiario.Ficha.FichaPppp.Empresa.Usuario.ApellidoUsuario);
                        var cellEmailusuemp = row.CreateCell(51); cellEmailusuemp.SetCellValue(beneficiario.Ficha.FichaPppp.Empresa.Usuario.Mail);
                        var cellTeleusuemp = row.CreateCell(52); cellTeleusuemp.SetCellValue(beneficiario.Ficha.FichaPppp.Empresa.Usuario.Telefono);

                        //Beneficiario
                        var cellIdben = row.CreateCell(53); cellIdben.SetCellValue(beneficiario.IdBeneficiario);
                        var cellIdestadoben = row.CreateCell(54); cellIdestadoben.SetCellValue(beneficiario.IdEstado);
                        var cellEstadoben = row.CreateCell(55); cellEstadoben.SetCellValue(beneficiario.NombreEstado);
                        var cellNrocuentaben = row.CreateCell(56); cellNrocuentaben.SetCellValue(beneficiario.NumeroCuenta == null ? String.Empty
                            : beneficiario.NumeroCuenta.ToString());
                        var cellIdsucursalben = row.CreateCell(57); cellIdsucursalben.SetCellValue(beneficiario.IdSucursal == null ? String.Empty
                            : beneficiario.IdSucursal.ToString());
                        var cellCodsucursalben = row.CreateCell(58); cellCodsucursalben.SetCellValue(beneficiario.CodigoSucursal);
                        var cellSucursalben = row.CreateCell(59); cellSucursalben.SetCellValue(beneficiario.SucursalDescripcion);
                        var cellFecsolben = row.CreateCell(60); cellFecsolben.SetCellValue(beneficiario.FechaSolicitudCuenta == null ? String.Empty
                            : beneficiario.FechaSolicitudCuenta.Value.ToShortDateString());
                        var cellNotificado = row.CreateCell(61); cellNotificado.SetCellValue(beneficiario.Notificado ? "S" : "N");
                        var cellFecNotif = row.CreateCell(62); cellFecNotif.SetCellValue(beneficiario.FechaNotificacion == null ? String.Empty
                            : beneficiario.FechaNotificacion.Value.ToShortDateString());

                        var cellFechaIniBenf = row.CreateCell(63); cellFechaIniBenf.SetCellValue(beneficiario.FechaInicioBeneficio == null ? string.Empty : beneficiario.FechaInicioBeneficio.Value.ToShortDateString()); // 20/03/2013 - DI CAMPLI LEANDRO
                        var cellBajaBenef = row.CreateCell(64); cellBajaBenef.SetCellValue(beneficiario.FechaBajaBeneficio == null ? string.Empty : beneficiario.FechaBajaBeneficio.Value.ToShortDateString()); // 20/03/2013 - DI CAMPLI LEANDRO

                        var cellTieneapoben = row.CreateCell(65); cellTieneapoben.SetCellValue(beneficiario.TieneApoderado);

                        //Apoderado
                        var cellIdapoderado = row.CreateCell(66); cellIdapoderado.SetCellValue(beneficiario.Apoderado.IdApoderadoNullable == null ? String.Empty
                            : beneficiario.Apoderado.IdApoderadoNullable.ToString());
                        var cellApellidoapo = row.CreateCell(67); cellApellidoapo.SetCellValue(beneficiario.Apoderado.Apellido);
                        var cellNombreapo = row.CreateCell(68); cellNombreapo.SetCellValue(beneficiario.Apoderado.Nombre);
                        var cellDniapo = row.CreateCell(69); cellDniapo.SetCellValue(beneficiario.Apoderado.NumeroDocumento);
                        var cellCuilapo = row.CreateCell(70); cellCuilapo.SetCellValue(beneficiario.Apoderado.Cuil);
                        var cellSexoapo = row.CreateCell(71); cellSexoapo.SetCellValue(beneficiario.Apoderado.Sexo);
                        var cellNroctaapo = row.CreateCell(72); cellNroctaapo.SetCellValue(beneficiario.Apoderado.NumeroCuentaBco == null ? String.Empty
                            : beneficiario.Apoderado.NumeroCuentaBco.ToString());
                        var cellIdSuc = row.CreateCell(73); cellIdSuc.SetCellValue(beneficiario.Apoderado.IdSucursal == null ? String.Empty
                            : beneficiario.Apoderado.SucursalApoderado.IdSucursal.ToString());
                        var cellCodsucapo = row.CreateCell(74); cellCodsucapo.SetCellValue(beneficiario.Apoderado.SucursalApoderado.CodigoBanco);
                        var cellSucursalapo = row.CreateCell(75); cellSucursalapo.SetCellValue(beneficiario.Apoderado.SucursalApoderado.Detalle);
                        var cellFecsolcuentaapo = row.CreateCell(76); cellFecsolcuentaapo.SetCellValue(beneficiario.Apoderado.FechaSolicitudCuenta == null ?
                            String.Empty : beneficiario.Apoderado.FechaSolicitudCuenta.Value.ToShortDateString());
                        var cellFecnac = row.CreateCell(77); cellFecnac.SetCellValue(beneficiario.Apoderado.FechaNacimiento == null ? String.Empty :
                            beneficiario.Apoderado.FechaNacimiento.Value.ToShortDateString());
                        var cellCalleapo = row.CreateCell(78); cellCalleapo.SetCellValue(beneficiario.Apoderado.Calle);
                        var cellNroapo = row.CreateCell(79); cellNroapo.SetCellValue(beneficiario.Apoderado.Numero);
                        var cellPisoapo = row.CreateCell(80); cellPisoapo.SetCellValue(beneficiario.Apoderado.Piso);
                        var cellMonoblockapo = row.CreateCell(81); cellMonoblockapo.SetCellValue(beneficiario.Apoderado.Monoblock);
                        var cellDptoapo = row.CreateCell(82); cellDptoapo.SetCellValue(beneficiario.Apoderado.Dpto);
                        var cellParcelaapo = row.CreateCell(83); cellParcelaapo.SetCellValue(beneficiario.Apoderado.Parcela);
                        var cellManzanaapo = row.CreateCell(84); cellManzanaapo.SetCellValue(beneficiario.Apoderado.Manzana);
                        var cellEntrecalleapo = row.CreateCell(85); cellEntrecalleapo.SetCellValue(beneficiario.Apoderado.EntreCalles);
                        var cellBarrioapo = row.CreateCell(86); cellBarrioapo.SetCellValue(beneficiario.Apoderado.Barrio);
                        var cellCpapo = row.CreateCell(87); cellCpapo.SetCellValue(beneficiario.Apoderado.CodigoPostal);
                        var cellIdlocalidadapo = row.CreateCell(88); cellIdlocalidadapo.SetCellValue(beneficiario.Apoderado.IdLocalidad == null ? String.Empty
                            : beneficiario.Apoderado.LocalidadApoderado.IdLocalidad.ToString());
                        var cellLocalidadapo = row.CreateCell(89); cellLocalidadapo.SetCellValue(beneficiario.Apoderado.LocalidadApoderado.NombreLocalidad);
                        var cellIddeptoapo = row.CreateCell(90); cellIddeptoapo.SetCellValue(beneficiario.Apoderado.IdLocalidad == null ? String.Empty
                            : beneficiario.Apoderado.LocalidadApoderado.Departamento.IdDepartamento.ToString());
                        var cellDeptoapo = row.CreateCell(91); cellDeptoapo.SetCellValue(beneficiario.Apoderado.LocalidadApoderado.Departamento.NombreDepartamento);
                        var cellTelefonoapo = row.CreateCell(92); cellTelefonoapo.SetCellValue(beneficiario.Apoderado.TelefonoFijo);
                        var cellCelularapo = row.CreateCell(93); cellCelularapo.SetCellValue(beneficiario.Apoderado.TelefonoCelular);
                        var cellEmailapo = row.CreateCell(94);
                        cellEmailapo.SetCellValue(beneficiario.Apoderado.Mail);
                    }
                    #endregion
                    break;
                case (int)Enums.TipoFicha.Vat:
                    #region case (int)Enums.TipoFicha.Vat
                    foreach (var beneficiario in listaparaExcel)
                    {
                        i++;
                        //Ficha
                        var row = sheet.CreateRow(i);
                        var cellidficha = row.CreateCell(0); cellidficha.SetCellValue(beneficiario.Ficha.IdFicha);
                        var cellApellido = row.CreateCell(1); cellApellido.SetCellValue(beneficiario.Ficha.Apellido);
                        var cellNombre = row.CreateCell(2); cellNombre.SetCellValue(beneficiario.Ficha.Nombre);
                        var cellCuil = row.CreateCell(3); cellCuil.SetCellValue(beneficiario.Ficha.Cuil);
                        var cellDni = row.CreateCell(4); cellDni.SetCellValue(beneficiario.Ficha.NumeroDocumento);
                        var cellFechaNa = row.CreateCell(5); cellFechaNa.SetCellValue(beneficiario.Ficha.FechaNacimiento == null ? String.Empty
                            : beneficiario.Ficha.FechaNacimiento.Value.ToShortDateString());
                        var cellCalle = row.CreateCell(6); cellCalle.SetCellValue(beneficiario.Ficha.Calle);
                        var cellNro = row.CreateCell(7); cellNro.SetCellValue(beneficiario.Ficha.Numero);
                        var cellPiso = row.CreateCell(8); cellPiso.SetCellValue(beneficiario.Ficha.Piso);
                        var cellDpto = row.CreateCell(9); cellDpto.SetCellValue(beneficiario.Ficha.Dpto);
                        var cellMono = row.CreateCell(10); cellMono.SetCellValue(beneficiario.Ficha.Monoblock);
                        var cellParcela = row.CreateCell(11); cellParcela.SetCellValue(beneficiario.Ficha.Parcela);
                        var cellManzana = row.CreateCell(12); cellManzana.SetCellValue(beneficiario.Ficha.Manzana);
                        var cellEntreCalle = row.CreateCell(13); cellEntreCalle.SetCellValue(beneficiario.Ficha.EntreCalles);
                        var cellBarrio = row.CreateCell(14); cellBarrio.SetCellValue(beneficiario.Ficha.Barrio);
                        var cellCp = row.CreateCell(15); cellCp.SetCellValue(beneficiario.Ficha.CodigoPostal);
                        var cellidLocalidad = row.CreateCell(16); cellidLocalidad.SetCellValue(beneficiario.Ficha.IdLocalidad == null ? String.Empty
                            : beneficiario.Ficha.LocalidadFicha.IdLocalidad.ToString());
                        var cellnLocalidad = row.CreateCell(17); cellnLocalidad.SetCellValue(beneficiario.Ficha.LocalidadFicha.NombreLocalidad);
                        var cellIdDepartamento = row.CreateCell(18); cellIdDepartamento.SetCellValue(beneficiario.Ficha.IdLocalidad == null ? String.Empty
                            : beneficiario.Ficha.LocalidadFicha.Departamento.IdDepartamento.ToString());
                        var cellnDepartamento = row.CreateCell(19); cellnDepartamento.SetCellValue(beneficiario.Ficha.LocalidadFicha.Departamento.NombreDepartamento);
                        var cellTelFijo = row.CreateCell(20); cellTelFijo.SetCellValue(beneficiario.Ficha.TelefonoFijo);
                        var cellTelCelular = row.CreateCell(21); cellTelCelular.SetCellValue(beneficiario.Ficha.TelefonoCelular);
                        var cellContacto = row.CreateCell(22); cellContacto.SetCellValue(beneficiario.Ficha.Contacto);
                        var cellEmail = row.CreateCell(23); cellEmail.SetCellValue(beneficiario.Ficha.Mail);
                        var cellSexo = row.CreateCell(24); cellSexo.SetCellValue(beneficiario.Ficha.Sexo);
                        var cellidestadoFicha = row.CreateCell(25); cellidestadoFicha.SetCellValue(beneficiario.Ficha.IdEstadoFicha == null ? String.Empty
                            : beneficiario.Ficha.IdEstadoFicha.ToString());
                        var cellnestadoFicha = row.CreateCell(26); cellnestadoFicha.SetCellValue(beneficiario.Ficha.NombreEstadoFicha);
                        var cellModalidad = row.CreateCell(27); cellModalidad.SetCellValue(beneficiario.Ficha.FichaVat.Modalidad == (int)Enums.Modalidad.Entrenamiento
                            ? "Entrenamiento" : "CTI");
                        var cellTarea = row.CreateCell(28); cellTarea.SetCellValue(beneficiario.Ficha.FichaVat.Tareas);
                        var cellaltaTemprana = row.CreateCell(29); cellaltaTemprana.SetCellValue(beneficiario.Ficha.FichaVat.AltaTemprana);
                        var cellatFechaInicio = row.CreateCell(30); cellatFechaInicio.SetCellValue(beneficiario.Ficha.FichaVat.FechaInicioActividad == null ?
                            String.Empty : beneficiario.Ficha.FichaVat.FechaInicioActividad.Value.ToShortDateString());
                        var cellatFechaCese = row.CreateCell(31); cellatFechaCese.SetCellValue(beneficiario.Ficha.FichaVat.FechaFinActividad == null ?
                            String.Empty : beneficiario.Ficha.FichaVat.FechaFinActividad.Value.ToShortDateString());
                        var cellidModContAfip = row.CreateCell(32); cellidModContAfip.SetCellValue(beneficiario.Ficha.FichaVat.IdModalidadAFIP == null ? String.Empty
                            : beneficiario.Ficha.FichaVat.IdModalidadAFIP.ToString());
                        var cellModContAfip = row.CreateCell(33); cellModContAfip.SetCellValue(beneficiario.Ficha.FichaVat.ModalidadAFIP);
                        var cellFechaModif = row.CreateCell(34); cellFechaModif.SetCellValue(beneficiario.Ficha.FechaSistema == null ? String.Empty
                            : beneficiario.Ficha.FechaSistema.Value.ToShortDateString());

                        //Empresa
                        var cellidEmp = row.CreateCell(35); cellidEmp.SetCellValue(beneficiario.Ficha.FichaVat.Empresa.IdEmpresa);
                        var cellrazonSocial = row.CreateCell(36); cellrazonSocial.SetCellValue(beneficiario.Ficha.FichaVat.Empresa.NombreEmpresa);
                        var cellCuit = row.CreateCell(37); cellCuit.SetCellValue(beneficiario.Ficha.FichaVat.Empresa.Cuit);
                        var cellCodact = row.CreateCell(38); cellCodact.SetCellValue(beneficiario.Ficha.FichaVat.Empresa.CodigoActividad);
                        var cellCantemp = row.CreateCell(39); cellCantemp.SetCellValue(beneficiario.Ficha.FichaVat.Empresa.CantidadEmpleados == null ? String.Empty
                            : beneficiario.Ficha.FichaVat.Empresa.CantidadEmpleados.ToString());
                        var cellCalleemp = row.CreateCell(40); cellCalleemp.SetCellValue(beneficiario.Ficha.FichaVat.Empresa.Calle);
                        var cellNumeroemp = row.CreateCell(41); cellNumeroemp.SetCellValue(beneficiario.Ficha.FichaVat.Empresa.Numero);
                        var cellPisoemp = row.CreateCell(42); cellPisoemp.SetCellValue(beneficiario.Ficha.FichaVat.Empresa.Piso);
                        var cellDptoemp = row.CreateCell(43); cellDptoemp.SetCellValue(beneficiario.Ficha.FichaVat.Empresa.Dpto);
                        var cellCpemp = row.CreateCell(44); cellCpemp.SetCellValue(beneficiario.Ficha.FichaVat.Empresa.CodigoPostal);
                        var cellIdlocemp = row.CreateCell(45); cellIdlocemp.SetCellValue(beneficiario.Ficha.FichaVat.Empresa.IdLocalidad == null ? String.Empty
                            : beneficiario.Ficha.FichaVat.Empresa.LocalidadEmpresa.IdLocalidad.ToString());
                        var cellLocemp = row.CreateCell(46); cellLocemp.SetCellValue(beneficiario.Ficha.FichaVat.Empresa.LocalidadEmpresa.NombreLocalidad);
                        var cellIddepemp = row.CreateCell(47); cellIddepemp.SetCellValue(beneficiario.Ficha.FichaVat.Empresa.IdLocalidad == null ? String.Empty
                            : beneficiario.Ficha.FichaVat.Empresa.LocalidadEmpresa.Departamento.NombreDepartamento);
                        var cellIdusuarioemp = row.CreateCell(48); cellIdusuarioemp.SetCellValue(beneficiario.Ficha.FichaVat.Empresa.IdUsuario == null ?
                            String.Empty : beneficiario.Ficha.FichaVat.Empresa.Usuario.IdUsuarioEmpresa.ToString());
                        var cellNombreusuemp = row.CreateCell(49); cellNombreusuemp.SetCellValue(beneficiario.Ficha.FichaVat.Empresa.Usuario.NombreUsuario);
                        var cellApellidousuemp = row.CreateCell(50); cellApellidousuemp.SetCellValue(beneficiario.Ficha.FichaVat.Empresa.Usuario.ApellidoUsuario);
                        var cellEmailusuemp = row.CreateCell(51); cellEmailusuemp.SetCellValue(beneficiario.Ficha.FichaVat.Empresa.Usuario.Mail);
                        var cellTeleusuemp = row.CreateCell(52); cellTeleusuemp.SetCellValue(beneficiario.Ficha.FichaVat.Empresa.Usuario.Telefono);

                        //Beneficiario
                        var cellIdben = row.CreateCell(53); cellIdben.SetCellValue(beneficiario.IdBeneficiario);
                        var cellIdestadoben = row.CreateCell(54); cellIdestadoben.SetCellValue(beneficiario.IdEstado);
                        var cellEstadoben = row.CreateCell(55); cellEstadoben.SetCellValue(beneficiario.NombreEstado);
                        var cellNrocuentaben = row.CreateCell(56); cellNrocuentaben.SetCellValue(beneficiario.NumeroCuenta == null ? String.Empty
                            : beneficiario.NumeroCuenta.ToString());
                        var cellIdsucursalben = row.CreateCell(57); cellIdsucursalben.SetCellValue(beneficiario.IdSucursal == null ? String.Empty
                            : beneficiario.IdSucursal.ToString());
                        var cellCodsucursalben = row.CreateCell(58); cellCodsucursalben.SetCellValue(beneficiario.CodigoSucursal);
                        var cellSucursalben = row.CreateCell(59); cellSucursalben.SetCellValue(beneficiario.SucursalDescripcion);
                        var cellFecsolben = row.CreateCell(60); cellFecsolben.SetCellValue(beneficiario.FechaSolicitudCuenta == null ? String.Empty
                            : beneficiario.FechaSolicitudCuenta.Value.ToShortDateString());
                        var cellNotificado = row.CreateCell(61); cellNotificado.SetCellValue(beneficiario.Notificado ? "S" : "N");
                        var cellFecNotif = row.CreateCell(62); cellFecNotif.SetCellValue(beneficiario.FechaNotificacion == null ? String.Empty
                            : beneficiario.FechaNotificacion.Value.ToShortDateString());

                        var cellFechaIniBenf = row.CreateCell(63); cellFechaIniBenf.SetCellValue(beneficiario.FechaInicioBeneficio == null ? string.Empty : beneficiario.FechaInicioBeneficio.Value.ToShortDateString()); // 20/03/2013 - DI CAMPLI LEANDRO
                        var cellBajaBenef = row.CreateCell(64); cellBajaBenef.SetCellValue(beneficiario.FechaBajaBeneficio == null ? string.Empty : beneficiario.FechaBajaBeneficio.Value.ToShortDateString()); // 20/03/2013 - DI CAMPLI LEANDRO

                        var cellTieneapoben = row.CreateCell(65); cellTieneapoben.SetCellValue(beneficiario.TieneApoderado);

                        //Apoderado
                        var cellIdapoderado = row.CreateCell(66); cellIdapoderado.SetCellValue(beneficiario.Apoderado.IdApoderadoNullable == null ? String.Empty
                            : beneficiario.Apoderado.IdApoderadoNullable.ToString());
                        var cellApellidoapo = row.CreateCell(67); cellApellidoapo.SetCellValue(beneficiario.Apoderado.Apellido);
                        var cellNombreapo = row.CreateCell(68); cellNombreapo.SetCellValue(beneficiario.Apoderado.Nombre);
                        var cellDniapo = row.CreateCell(69); cellDniapo.SetCellValue(beneficiario.Apoderado.NumeroDocumento);
                        var cellCuilapo = row.CreateCell(70); cellCuilapo.SetCellValue(beneficiario.Apoderado.Cuil);
                        var cellSexoapo = row.CreateCell(71); cellSexoapo.SetCellValue(beneficiario.Apoderado.Sexo);
                        var cellNroctaapo = row.CreateCell(72); cellNroctaapo.SetCellValue(beneficiario.Apoderado.NumeroCuentaBco == null ? String.Empty
                            : beneficiario.Apoderado.NumeroCuentaBco.ToString());
                        var cellIdSuc = row.CreateCell(73); cellIdSuc.SetCellValue(beneficiario.Apoderado.IdSucursal == null ? String.Empty
                            : beneficiario.Apoderado.SucursalApoderado.IdSucursal.ToString());
                        var cellCodsucapo = row.CreateCell(74); cellCodsucapo.SetCellValue(beneficiario.Apoderado.SucursalApoderado.CodigoBanco);
                        var cellSucursalapo = row.CreateCell(75); cellSucursalapo.SetCellValue(beneficiario.Apoderado.SucursalApoderado.Detalle);
                        var cellFecsolcuentaapo = row.CreateCell(76); cellFecsolcuentaapo.SetCellValue(beneficiario.Apoderado.FechaSolicitudCuenta == null ?
                            String.Empty : beneficiario.Apoderado.FechaSolicitudCuenta.Value.ToShortDateString());
                        var cellFecnac = row.CreateCell(77); cellFecnac.SetCellValue(beneficiario.Apoderado.FechaNacimiento == null ? String.Empty :
                            beneficiario.Apoderado.FechaNacimiento.Value.ToShortDateString());
                        var cellCalleapo = row.CreateCell(78); cellCalleapo.SetCellValue(beneficiario.Apoderado.Calle);
                        var cellNroapo = row.CreateCell(79); cellNroapo.SetCellValue(beneficiario.Apoderado.Numero);
                        var cellPisoapo = row.CreateCell(80); cellPisoapo.SetCellValue(beneficiario.Apoderado.Piso);
                        var cellMonoblockapo = row.CreateCell(81); cellMonoblockapo.SetCellValue(beneficiario.Apoderado.Monoblock);
                        var cellDptoapo = row.CreateCell(82); cellDptoapo.SetCellValue(beneficiario.Apoderado.Dpto);
                        var cellParcelaapo = row.CreateCell(83); cellParcelaapo.SetCellValue(beneficiario.Apoderado.Parcela);
                        var cellManzanaapo = row.CreateCell(84); cellManzanaapo.SetCellValue(beneficiario.Apoderado.Manzana);
                        var cellEntrecalleapo = row.CreateCell(85); cellEntrecalleapo.SetCellValue(beneficiario.Apoderado.EntreCalles);
                        var cellBarrioapo = row.CreateCell(86); cellBarrioapo.SetCellValue(beneficiario.Apoderado.Barrio);
                        var cellCpapo = row.CreateCell(87); cellCpapo.SetCellValue(beneficiario.Apoderado.CodigoPostal);
                        var cellIdlocalidadapo = row.CreateCell(88); cellIdlocalidadapo.SetCellValue(beneficiario.Apoderado.IdLocalidad == null ?
                            String.Empty : beneficiario.Apoderado.LocalidadApoderado.IdLocalidad.ToString());
                        var cellLocalidadapo = row.CreateCell(89); cellLocalidadapo.SetCellValue(beneficiario.Apoderado.LocalidadApoderado.NombreLocalidad);
                        var cellIddeptoapo = row.CreateCell(90); cellIddeptoapo.SetCellValue(beneficiario.Apoderado.IdLocalidad == null ?
                            String.Empty : beneficiario.Apoderado.LocalidadApoderado.Departamento.IdDepartamento.ToString());
                        var cellDeptoapo = row.CreateCell(91); cellDeptoapo.SetCellValue(beneficiario.Apoderado.LocalidadApoderado.Departamento.NombreDepartamento);
                        var cellTelefonoapo = row.CreateCell(92); cellTelefonoapo.SetCellValue(beneficiario.Apoderado.TelefonoFijo);
                        var cellCelularapo = row.CreateCell(93); cellCelularapo.SetCellValue(beneficiario.Apoderado.TelefonoCelular);
                        var cellEmailapo = row.CreateCell(94);
                        cellEmailapo.SetCellValue(beneficiario.Apoderado.Mail);
                    }
                    #endregion
                    break;
                case (int)Enums.TipoFicha.ReconversionProductiva:
                    #region case (int)Enums.TipoFicha.Reconversion
                    foreach (var beneficiario in listaparaExcel)
                    {
                        i++;
                        //Ficha
                        var row = sheet.CreateRow(i);
                        var cellidficha = row.CreateCell(0); cellidficha.SetCellValue(beneficiario.Ficha.IdFicha);
                        var cellApellido = row.CreateCell(1); cellApellido.SetCellValue(beneficiario.Ficha.Apellido);
                        var cellNombre = row.CreateCell(2); cellNombre.SetCellValue(beneficiario.Ficha.Nombre);
                        var cellCuil = row.CreateCell(3); cellCuil.SetCellValue(beneficiario.Ficha.Cuil);
                        var cellDni = row.CreateCell(4); cellDni.SetCellValue(beneficiario.Ficha.NumeroDocumento);
                        var cellFechaNa = row.CreateCell(5); cellFechaNa.SetCellValue(beneficiario.Ficha.FechaNacimiento == null ? String.Empty
                            : beneficiario.Ficha.FechaNacimiento.Value.ToShortDateString());
                        var cellCalle = row.CreateCell(6); cellCalle.SetCellValue(beneficiario.Ficha.Calle);
                        var cellNro = row.CreateCell(7); cellNro.SetCellValue(beneficiario.Ficha.Numero);
                        var cellPiso = row.CreateCell(8); cellPiso.SetCellValue(beneficiario.Ficha.Piso);
                        var cellDpto = row.CreateCell(9); cellDpto.SetCellValue(beneficiario.Ficha.Dpto);
                        var cellMono = row.CreateCell(10); cellMono.SetCellValue(beneficiario.Ficha.Monoblock);
                        var cellParcela = row.CreateCell(11); cellParcela.SetCellValue(beneficiario.Ficha.Parcela);
                        var cellManzana = row.CreateCell(12); cellManzana.SetCellValue(beneficiario.Ficha.Manzana);
                        var cellEntreCalle = row.CreateCell(13); cellEntreCalle.SetCellValue(beneficiario.Ficha.EntreCalles);
                        var cellBarrio = row.CreateCell(14); cellBarrio.SetCellValue(beneficiario.Ficha.Barrio);
                        var cellCp = row.CreateCell(15); cellCp.SetCellValue(beneficiario.Ficha.CodigoPostal);
                        var cellidLocalidad = row.CreateCell(16); cellidLocalidad.SetCellValue(beneficiario.Ficha.IdLocalidad == null ?
                            String.Empty : beneficiario.Ficha.LocalidadFicha.IdLocalidad.ToString());
                        var cellnLocalidad = row.CreateCell(17); cellnLocalidad.SetCellValue(beneficiario.Ficha.LocalidadFicha.NombreLocalidad);
                        var cellIdDepartamento = row.CreateCell(18); cellIdDepartamento.SetCellValue(beneficiario.Ficha.IdLocalidad == null ?
                            String.Empty : beneficiario.Ficha.LocalidadFicha.Departamento.IdDepartamento.ToString());
                        var cellnDepartamento = row.CreateCell(19); cellnDepartamento.SetCellValue(beneficiario.Ficha.LocalidadFicha.Departamento.NombreDepartamento);
                        var cellTelFijo = row.CreateCell(20); cellTelFijo.SetCellValue(beneficiario.Ficha.TelefonoFijo);
                        var cellTelCelular = row.CreateCell(21); cellTelCelular.SetCellValue(beneficiario.Ficha.TelefonoCelular);
                        var cellContacto = row.CreateCell(22); cellContacto.SetCellValue(beneficiario.Ficha.Contacto);
                        var cellEmail = row.CreateCell(23); cellEmail.SetCellValue(beneficiario.Ficha.Mail);
                        var cellSexo = row.CreateCell(24); cellSexo.SetCellValue(beneficiario.Ficha.Sexo);
                        var cellidestadoFicha = row.CreateCell(25); cellidestadoFicha.SetCellValue(beneficiario.Ficha.IdEstadoFicha == null ? String.Empty :
                            beneficiario.Ficha.IdEstadoFicha.ToString());
                        var cellnestadoFicha = row.CreateCell(26); cellnestadoFicha.SetCellValue(beneficiario.Ficha.NombreEstadoFicha);
                        var cellModalidad = row.CreateCell(27); cellModalidad.SetCellValue(beneficiario.Ficha.FichaReconversion.Modalidad == (int)Enums.Modalidad.Entrenamiento
                            ? "Entrenamiento" : "CTI");
                        var cellaltaTemprana = row.CreateCell(28); cellaltaTemprana.SetCellValue(beneficiario.Ficha.FichaReconversion.AltaTemprana);
                        var cellatFechaInicio = row.CreateCell(29); cellatFechaInicio.SetCellValue(beneficiario.Ficha.FichaReconversion.FechaInicioActividad == null ?
                            String.Empty : beneficiario.Ficha.FichaReconversion.FechaInicioActividad.Value.ToShortDateString());
                        var cellatFechaCese = row.CreateCell(30); cellatFechaCese.SetCellValue(beneficiario.Ficha.FichaReconversion.FechaFinActividad == null ?
                            String.Empty : beneficiario.Ficha.FichaReconversion.FechaFinActividad.Value.ToShortDateString());
                        var cellidModContAfip = row.CreateCell(31); cellidModContAfip.SetCellValue(beneficiario.Ficha.FichaReconversion.IdModalidadAfip == null ?
                            String.Empty : beneficiario.Ficha.FichaReconversion.IdModalidadAfip.ToString());
                        var cellModContAfip = row.CreateCell(32); cellModContAfip.SetCellValue(beneficiario.Ficha.FichaReconversion.ModalidadAfip);

                        var cellFechaModif = row.CreateCell(33); cellFechaModif.SetCellValue(beneficiario.Ficha.FechaSistema == null ? String.Empty
                            : beneficiario.Ficha.FechaSistema.Value.ToShortDateString());

                        var cellnSubprograma = row.CreateCell(34); cellnSubprograma.SetCellValue(beneficiario.Ficha.FichaReconversion.Subprograma);

                        //Proyecto
                        var cellnProyecto = row.CreateCell(35); cellnProyecto.SetCellValue(beneficiario.Ficha.FichaReconversion.Proyecto.NombreProyecto);
                        var cellCantCapacitar = row.CreateCell(36); cellCantCapacitar.SetCellValue(beneficiario.Ficha.FichaReconversion.Proyecto.CantidadAcapacitar == null ? String.Empty : beneficiario.Ficha.FichaReconversion.Proyecto.CantidadAcapacitar.ToString());
                        var cellFecInicio = row.CreateCell(37); cellFecInicio.SetCellValue(beneficiario.Ficha.FichaReconversion.Proyecto.FechaInicioProyecto == null ? String.Empty : beneficiario.Ficha.FichaReconversion.Proyecto.FechaInicioProyecto.Value.ToShortDateString());
                        var cellFecFin = row.CreateCell(38); cellFecFin.SetCellValue(beneficiario.Ficha.FichaReconversion.Proyecto.FechaFinProyecto == null ? String.Empty : beneficiario.Ficha.FichaReconversion.Proyecto.FechaFinProyecto.Value.ToShortDateString());
                        var cellMesesDuracion = row.CreateCell(39); cellMesesDuracion.SetCellValue(beneficiario.Ficha.FichaReconversion.Proyecto.MesesDuracionProyecto == null ? String.Empty : beneficiario.Ficha.FichaReconversion.Proyecto.MesesDuracionProyecto.ToString());
                        var cellAportesEmpresa = row.CreateCell(40); cellAportesEmpresa.SetCellValue(beneficiario.Ficha.FichaReconversion.AportesDeLaEmpresa);
                        var cellApeTutor = row.CreateCell(41); cellApeTutor.SetCellValue(beneficiario.Ficha.FichaReconversion.ApellidoTutor);
                        var cellNomTutor = row.CreateCell(42); cellNomTutor.SetCellValue(beneficiario.Ficha.FichaReconversion.NombreTutor);
                        var cellDocTutor = row.CreateCell(43); cellDocTutor.SetCellValue(beneficiario.Ficha.FichaReconversion.NroDocumentoTutor);
                        var cellTelTutor = row.CreateCell(44); cellTelTutor.SetCellValue(beneficiario.Ficha.FichaReconversion.TelefonoTutor);
                        var cellEmailTutor = row.CreateCell(45); cellEmailTutor.SetCellValue(beneficiario.Ficha.FichaReconversion.EmailTutor);
                        var cellPuestoTutor = row.CreateCell(46); cellPuestoTutor.SetCellValue(beneficiario.Ficha.FichaReconversion.PuestoTutor);

                        //Empresa
                        var cellidEmp = row.CreateCell(47); cellidEmp.SetCellValue(beneficiario.Ficha.FichaReconversion.Empresa.IdEmpresa);
                        var cellrazonSocial = row.CreateCell(48); cellrazonSocial.SetCellValue(beneficiario.Ficha.FichaReconversion.Empresa.NombreEmpresa);
                        var cellCuit = row.CreateCell(49); cellCuit.SetCellValue(beneficiario.Ficha.FichaReconversion.Empresa.Cuit);
                        var cellCodact = row.CreateCell(50); cellCodact.SetCellValue(beneficiario.Ficha.FichaReconversion.Empresa.CodigoActividad);
                        var cellCantemp = row.CreateCell(51); cellCantemp.SetCellValue(beneficiario.Ficha.FichaReconversion.Empresa.CantidadEmpleados == null ?
                            String.Empty : beneficiario.Ficha.FichaReconversion.Empresa.CantidadEmpleados.ToString());
                        var cellCalleemp = row.CreateCell(52); cellCalleemp.SetCellValue(beneficiario.Ficha.FichaReconversion.Empresa.Calle);
                        var cellNumeroemp = row.CreateCell(53); cellNumeroemp.SetCellValue(beneficiario.Ficha.FichaReconversion.Empresa.Numero);
                        var cellPisoemp = row.CreateCell(54); cellPisoemp.SetCellValue(beneficiario.Ficha.FichaReconversion.Empresa.Piso);
                        var cellDptoemp = row.CreateCell(55); cellDptoemp.SetCellValue(beneficiario.Ficha.FichaReconversion.Empresa.Dpto);
                        var cellCpemp = row.CreateCell(56); cellCpemp.SetCellValue(beneficiario.Ficha.FichaReconversion.Empresa.CodigoPostal);
                        var cellIdlocemp = row.CreateCell(57); cellIdlocemp.SetCellValue(beneficiario.Ficha.FichaReconversion.Empresa.IdLocalidad == null ?
                            String.Empty : beneficiario.Ficha.FichaReconversion.Empresa.LocalidadEmpresa.IdLocalidad.ToString());
                        var cellLocemp = row.CreateCell(58); cellLocemp.SetCellValue(beneficiario.Ficha.FichaReconversion.Empresa.LocalidadEmpresa.NombreLocalidad);
                        var cellDepartamentoEmp = row.CreateCell(59); cellDepartamentoEmp.SetCellValue(beneficiario.Ficha.FichaReconversion.Empresa.IdLocalidad == null ?
                            String.Empty : beneficiario.Ficha.FichaReconversion.Empresa.LocalidadEmpresa.Departamento.NombreDepartamento);
                        var cellIdusuarioemp = row.CreateCell(60); cellIdusuarioemp.SetCellValue(beneficiario.Ficha.FichaReconversion.Empresa.Usuario.IdUsuarioEmpresa == null ?
                            String.Empty : beneficiario.Ficha.FichaReconversion.Empresa.Usuario.IdUsuarioEmpresa.ToString());
                        var cellNombreusuemp = row.CreateCell(61); cellNombreusuemp.SetCellValue(beneficiario.Ficha.FichaReconversion.Empresa.Usuario.NombreUsuario);
                        var cellApellidousuemp = row.CreateCell(62); cellApellidousuemp.SetCellValue(beneficiario.Ficha.FichaReconversion.Empresa.Usuario.ApellidoUsuario);
                        var cellEmailusuemp = row.CreateCell(63); cellEmailusuemp.SetCellValue(beneficiario.Ficha.FichaReconversion.Empresa.Usuario.Mail);
                        var cellTeleusuemp = row.CreateCell(64); cellTeleusuemp.SetCellValue(beneficiario.Ficha.FichaReconversion.Empresa.Usuario.Telefono);

                        //Sede
                        var cellIdSede = row.CreateCell(65); cellIdSede.SetCellValue(beneficiario.Ficha.FichaReconversion.IdSede == null ? String.Empty : beneficiario.Ficha.FichaReconversion.IdSede.ToString());
                        var cellnSede = row.CreateCell(66); cellnSede.SetCellValue(beneficiario.Ficha.FichaReconversion.Sede.NombreSede);
                        var cellCalleSede = row.CreateCell(67); cellCalleSede.SetCellValue(beneficiario.Ficha.FichaReconversion.Sede.Calle);
                        var cellNroSede = row.CreateCell(68); cellNroSede.SetCellValue(beneficiario.Ficha.FichaReconversion.Sede.Numero);
                        var cellPisoSede = row.CreateCell(69); cellPisoSede.SetCellValue(beneficiario.Ficha.FichaReconversion.Sede.Piso);
                        var cellDptoSede = row.CreateCell(70); cellDptoSede.SetCellValue(beneficiario.Ficha.FichaReconversion.Sede.Dpto);
                        var cellCpSede = row.CreateCell(71); cellCpSede.SetCellValue(beneficiario.Ficha.FichaReconversion.Sede.CodigoPostal);
                        var cellLocSede = row.CreateCell(72); cellLocSede.SetCellValue(beneficiario.Ficha.FichaReconversion.Sede.NombreLocalidad);
                        var cellApeContacto = row.CreateCell(73); cellApeContacto.SetCellValue(beneficiario.Ficha.FichaReconversion.Sede.ApellidoContacto);
                        var cellNomContacto = row.CreateCell(74); cellNomContacto.SetCellValue(beneficiario.Ficha.FichaReconversion.Sede.NombreContacto);
                        var cellTelcontacto = row.CreateCell(75); cellTelcontacto.SetCellValue(beneficiario.Ficha.FichaReconversion.Sede.Telefono);
                        var cellFaxContacto = row.CreateCell(76); cellFaxContacto.SetCellValue(beneficiario.Ficha.FichaReconversion.Sede.Fax);
                        var cellEmailContacto = row.CreateCell(77); cellEmailContacto.SetCellValue(beneficiario.Ficha.FichaReconversion.Sede.Email);

                        //Beneficiario
                        var cellIdben = row.CreateCell(78); cellIdben.SetCellValue(beneficiario.IdBeneficiario);
                        var cellIdestadoben = row.CreateCell(79); cellIdestadoben.SetCellValue(beneficiario.IdEstado);
                        var cellEstadoben = row.CreateCell(80); cellEstadoben.SetCellValue(beneficiario.NombreEstado);
                        var cellNrocuentaben = row.CreateCell(81); cellNrocuentaben.SetCellValue(beneficiario.NumeroCuenta == null ? String.Empty
                            : beneficiario.NumeroCuenta.ToString());
                        var cellIdsucursalben = row.CreateCell(82); cellIdsucursalben.SetCellValue(beneficiario.IdSucursal == null ? String.Empty
                            : beneficiario.IdSucursal.ToString());
                        var cellCodsucursalben = row.CreateCell(83); cellCodsucursalben.SetCellValue(beneficiario.CodigoSucursal);
                        var cellSucursalben = row.CreateCell(84); cellSucursalben.SetCellValue(beneficiario.SucursalDescripcion);
                        var cellFecsolben = row.CreateCell(85); cellFecsolben.SetCellValue(beneficiario.FechaSolicitudCuenta == null ? String.Empty
                            : beneficiario.FechaSolicitudCuenta.Value.ToShortDateString());
                        var cellNotificado = row.CreateCell(86); cellNotificado.SetCellValue(beneficiario.Notificado ? "S" : "N");
                        var cellFecNotif = row.CreateCell(87); cellFecNotif.SetCellValue(beneficiario.FechaNotificacion == null ? String.Empty
                            : beneficiario.FechaNotificacion.Value.ToShortDateString());

                        var cellFechaIniBenf = row.CreateCell(88); cellFechaIniBenf.SetCellValue(beneficiario.FechaInicioBeneficio == null ? string.Empty : beneficiario.FechaInicioBeneficio.Value.ToShortDateString()); // 20/03/2013 - DI CAMPLI LEANDRO
                        var cellBajaBenef = row.CreateCell(89); cellBajaBenef.SetCellValue(beneficiario.FechaBajaBeneficio == null ? string.Empty : beneficiario.FechaBajaBeneficio.Value.ToShortDateString()); // 20/03/2013 - DI CAMPLI LEANDRO

                        var cellTieneapoben = row.CreateCell(90); cellTieneapoben.SetCellValue(beneficiario.TieneApoderado);

                        //Apoderado
                        var cellIdapoderado = row.CreateCell(91); cellIdapoderado.SetCellValue(beneficiario.Apoderado.IdApoderadoNullable == null ?
                            String.Empty : beneficiario.Apoderado.IdApoderadoNullable.ToString());
                        var cellApellidoapo = row.CreateCell(92); cellApellidoapo.SetCellValue(beneficiario.Apoderado.Apellido);
                        var cellNombreapo = row.CreateCell(93); cellNombreapo.SetCellValue(beneficiario.Apoderado.Nombre);
                        var cellDniapo = row.CreateCell(94); cellDniapo.SetCellValue(beneficiario.Apoderado.NumeroDocumento);
                        var cellCuilapo = row.CreateCell(95); cellCuilapo.SetCellValue(beneficiario.Apoderado.Cuil);
                        var cellSexoapo = row.CreateCell(96); cellSexoapo.SetCellValue(beneficiario.Apoderado.Sexo);
                        var cellNroctaapo = row.CreateCell(97); cellNroctaapo.SetCellValue(beneficiario.Apoderado.NumeroCuentaBco == null ? String.Empty
                            : beneficiario.Apoderado.NumeroCuentaBco.ToString());
                        var cellIdSuc = row.CreateCell(98); cellIdSuc.SetCellValue(beneficiario.Apoderado.IdSucursal == null ? String.Empty
                            : beneficiario.Apoderado.SucursalApoderado.IdSucursal.ToString());
                        var cellCodsucapo = row.CreateCell(99); cellCodsucapo.SetCellValue(beneficiario.Apoderado.SucursalApoderado.CodigoBanco);
                        var cellSucursalapo = row.CreateCell(100); cellSucursalapo.SetCellValue(beneficiario.Apoderado.SucursalApoderado.Detalle);
                        var cellFecsolcuentaapo = row.CreateCell(101); cellFecsolcuentaapo.SetCellValue(beneficiario.Apoderado.FechaSolicitudCuenta == null ?
                            String.Empty : beneficiario.Apoderado.FechaSolicitudCuenta.Value.ToShortDateString());
                        var cellFecnac = row.CreateCell(102); cellFecnac.SetCellValue(beneficiario.Apoderado.FechaNacimiento == null ? String.Empty :
                            beneficiario.Apoderado.FechaNacimiento.Value.ToShortDateString());
                        var cellCalleapo = row.CreateCell(103); cellCalleapo.SetCellValue(beneficiario.Apoderado.Calle);
                        var cellNroapo = row.CreateCell(104); cellNroapo.SetCellValue(beneficiario.Apoderado.Numero);
                        var cellPisoapo = row.CreateCell(105); cellPisoapo.SetCellValue(beneficiario.Apoderado.Piso);
                        var cellMonoblockapo = row.CreateCell(106); cellMonoblockapo.SetCellValue(beneficiario.Apoderado.Monoblock);
                        var cellDptoapo = row.CreateCell(107); cellDptoapo.SetCellValue(beneficiario.Apoderado.Dpto);
                        var cellParcelaapo = row.CreateCell(108); cellParcelaapo.SetCellValue(beneficiario.Apoderado.Parcela);
                        var cellManzanaapo = row.CreateCell(109); cellManzanaapo.SetCellValue(beneficiario.Apoderado.Manzana);
                        var cellEntrecalleapo = row.CreateCell(110); cellEntrecalleapo.SetCellValue(beneficiario.Apoderado.EntreCalles);
                        var cellBarrioapo = row.CreateCell(111); cellBarrioapo.SetCellValue(beneficiario.Apoderado.Barrio);
                        var cellCpapo = row.CreateCell(112); cellCpapo.SetCellValue(beneficiario.Apoderado.CodigoPostal);
                        var cellIdlocalidadapo = row.CreateCell(113); cellIdlocalidadapo.SetCellValue(beneficiario.Apoderado.IdLocalidad == null ? String.Empty
                            : beneficiario.Apoderado.LocalidadApoderado.IdLocalidad.ToString());
                        var cellLocalidadapo = row.CreateCell(114); cellLocalidadapo.SetCellValue(beneficiario.Apoderado.LocalidadApoderado.NombreLocalidad);
                        var cellIddeptoapo = row.CreateCell(115); cellIddeptoapo.SetCellValue(beneficiario.Apoderado.IdLocalidad == null ? String.Empty
                            : beneficiario.Apoderado.LocalidadApoderado.Departamento.IdDepartamento.ToString());
                        var cellDeptoapo = row.CreateCell(116); cellDeptoapo.SetCellValue(beneficiario.Apoderado.LocalidadApoderado.Departamento.NombreDepartamento);
                        var cellTelefonoapo = row.CreateCell(117); cellTelefonoapo.SetCellValue(beneficiario.Apoderado.TelefonoFijo);
                        var cellCelularapo = row.CreateCell(118); cellCelularapo.SetCellValue(beneficiario.Apoderado.TelefonoCelular);
                        var cellEmailapo = row.CreateCell(119);
                        cellEmailapo.SetCellValue(beneficiario.Apoderado.Mail);
                    }
                    #endregion
                    break;
                case (int)Enums.TipoFicha.EfectoresSociales:
                    #region case (int)Enums.TipoFicha.EfectoresSociales
                    foreach (var beneficiario in listaparaExcel)
                    {
                        i++;
                        //Ficha
                        var row = sheet.CreateRow(i);
                        var cellidficha = row.CreateCell(0); cellidficha.SetCellValue(beneficiario.Ficha.IdFicha);
                        var cellApellido = row.CreateCell(1); cellApellido.SetCellValue(beneficiario.Ficha.Apellido);
                        var cellNombre = row.CreateCell(2); cellNombre.SetCellValue(beneficiario.Ficha.Nombre);
                        var cellCuil = row.CreateCell(3); cellCuil.SetCellValue(beneficiario.Ficha.Cuil);
                        var cellDni = row.CreateCell(4); cellDni.SetCellValue(beneficiario.Ficha.NumeroDocumento);
                        var cellFechaNa = row.CreateCell(5); cellFechaNa.SetCellValue(beneficiario.Ficha.FechaNacimiento == null ? String.Empty
                            : beneficiario.Ficha.FechaNacimiento.Value.ToShortDateString());
                        var cellCalle = row.CreateCell(6); cellCalle.SetCellValue(beneficiario.Ficha.Calle);
                        var cellNro = row.CreateCell(7); cellNro.SetCellValue(beneficiario.Ficha.Numero);
                        var cellPiso = row.CreateCell(8); cellPiso.SetCellValue(beneficiario.Ficha.Piso);
                        var cellDpto = row.CreateCell(9); cellDpto.SetCellValue(beneficiario.Ficha.Dpto);
                        var cellMono = row.CreateCell(10); cellMono.SetCellValue(beneficiario.Ficha.Monoblock);
                        var cellParcela = row.CreateCell(11); cellParcela.SetCellValue(beneficiario.Ficha.Parcela);
                        var cellManzana = row.CreateCell(12); cellManzana.SetCellValue(beneficiario.Ficha.Manzana);
                        var cellEntreCalle = row.CreateCell(13); cellEntreCalle.SetCellValue(beneficiario.Ficha.EntreCalles);
                        var cellBarrio = row.CreateCell(14); cellBarrio.SetCellValue(beneficiario.Ficha.Barrio);
                        var cellCp = row.CreateCell(15); cellCp.SetCellValue(beneficiario.Ficha.CodigoPostal);
                        var cellidLocalidad = row.CreateCell(16); cellidLocalidad.SetCellValue(beneficiario.Ficha.IdLocalidad == null ?
                            String.Empty : beneficiario.Ficha.LocalidadFicha.IdLocalidad.ToString());
                        var cellnLocalidad = row.CreateCell(17); cellnLocalidad.SetCellValue(beneficiario.Ficha.LocalidadFicha.NombreLocalidad);
                        var cellIdDepartamento = row.CreateCell(18); cellIdDepartamento.SetCellValue(beneficiario.Ficha.IdLocalidad == null ?
                            String.Empty : beneficiario.Ficha.LocalidadFicha.Departamento.IdDepartamento.ToString());
                        var cellnDepartamento = row.CreateCell(19); cellnDepartamento.SetCellValue(beneficiario.Ficha.LocalidadFicha.Departamento.NombreDepartamento);
                        var cellTelFijo = row.CreateCell(20); cellTelFijo.SetCellValue(beneficiario.Ficha.TelefonoFijo);
                        var cellTelCelular = row.CreateCell(21); cellTelCelular.SetCellValue(beneficiario.Ficha.TelefonoCelular);
                        var cellContacto = row.CreateCell(22); cellContacto.SetCellValue(beneficiario.Ficha.Contacto);
                        var cellEmail = row.CreateCell(23); cellEmail.SetCellValue(beneficiario.Ficha.Mail);
                        var cellSexo = row.CreateCell(24); cellSexo.SetCellValue(beneficiario.Ficha.Sexo);
                        var cellidestadoFicha = row.CreateCell(25); cellidestadoFicha.SetCellValue(beneficiario.Ficha.IdEstadoFicha == null ? String.Empty :
                            beneficiario.Ficha.IdEstadoFicha.ToString());
                        var cellnestadoFicha = row.CreateCell(26); cellnestadoFicha.SetCellValue(beneficiario.Ficha.NombreEstadoFicha);
                        var cellModalidad = row.CreateCell(27); cellModalidad.SetCellValue(beneficiario.Ficha.FichaEfectores.Modalidad == (int)Enums.Modalidad.Entrenamiento
                            ? "Entrenamiento" : "CTI");
                        var cellaltaTemprana = row.CreateCell(28); cellaltaTemprana.SetCellValue(beneficiario.Ficha.FichaEfectores.AltaTemprana);
                        var cellatFechaInicio = row.CreateCell(29); cellatFechaInicio.SetCellValue(beneficiario.Ficha.FichaEfectores.FechaInicioActividad == null ?
                            String.Empty : beneficiario.Ficha.FichaEfectores.FechaInicioActividad.Value.ToShortDateString());
                        var cellatFechaCese = row.CreateCell(30); cellatFechaCese.SetCellValue(beneficiario.Ficha.FichaEfectores.FechaFinActividad == null ?
                            String.Empty : beneficiario.Ficha.FichaEfectores.FechaFinActividad.Value.ToShortDateString());
                        var cellidModContAfip = row.CreateCell(31); cellidModContAfip.SetCellValue(beneficiario.Ficha.FichaEfectores.IdModalidadAfip == null ?
                            String.Empty : beneficiario.Ficha.FichaEfectores.IdModalidadAfip.ToString());
                        var cellModContAfip = row.CreateCell(32); cellModContAfip.SetCellValue(beneficiario.Ficha.FichaEfectores.ModalidadAfip);

                        var cellFechaModif = row.CreateCell(33); cellFechaModif.SetCellValue(beneficiario.Ficha.FechaSistema == null ? String.Empty
                            : beneficiario.Ficha.FechaSistema.Value.ToShortDateString());

                        var cellnSubprograma = row.CreateCell(34); cellnSubprograma.SetCellValue(beneficiario.Ficha.FichaEfectores.Subprograma);

                        //Proyecto
                        var cellnProyecto = row.CreateCell(35); cellnProyecto.SetCellValue(beneficiario.Ficha.FichaEfectores.Proyecto.NombreProyecto);
                        var cellCantCapacitar = row.CreateCell(36); cellCantCapacitar.SetCellValue(beneficiario.Ficha.FichaEfectores.Proyecto.CantidadAcapacitar == null ? String.Empty : beneficiario.Ficha.FichaEfectores.Proyecto.CantidadAcapacitar.ToString());
                        var cellFecInicio = row.CreateCell(37); cellFecInicio.SetCellValue(beneficiario.Ficha.FichaEfectores.Proyecto.FechaInicioProyecto == null ? String.Empty : beneficiario.Ficha.FichaEfectores.Proyecto.FechaInicioProyecto.Value.ToShortDateString());
                        var cellFecFin = row.CreateCell(38); cellFecFin.SetCellValue(beneficiario.Ficha.FichaEfectores.Proyecto.FechaFinProyecto == null ? String.Empty : beneficiario.Ficha.FichaEfectores.Proyecto.FechaFinProyecto.Value.ToShortDateString());
                        var cellMesesDuracion = row.CreateCell(39); cellMesesDuracion.SetCellValue(beneficiario.Ficha.FichaEfectores.Proyecto.MesesDuracionProyecto == null ? String.Empty : beneficiario.Ficha.FichaEfectores.Proyecto.MesesDuracionProyecto.ToString());
                        var cellAportesEmpresa = row.CreateCell(40); cellAportesEmpresa.SetCellValue(beneficiario.Ficha.FichaEfectores.AportesDeLaEmpresa);
                        var cellApeTutor = row.CreateCell(41); cellApeTutor.SetCellValue(beneficiario.Ficha.FichaEfectores.ApellidoTutor);
                        var cellNomTutor = row.CreateCell(42); cellNomTutor.SetCellValue(beneficiario.Ficha.FichaEfectores.NombreTutor);
                        var cellDocTutor = row.CreateCell(43); cellDocTutor.SetCellValue(beneficiario.Ficha.FichaEfectores.NroDocumentoTutor);
                        var cellTelTutor = row.CreateCell(44); cellTelTutor.SetCellValue(beneficiario.Ficha.FichaEfectores.TelefonoTutor);
                        var cellEmailTutor = row.CreateCell(45); cellEmailTutor.SetCellValue(beneficiario.Ficha.FichaEfectores.EmailTutor);
                        var cellPuestoTutor = row.CreateCell(46); cellPuestoTutor.SetCellValue(beneficiario.Ficha.FichaEfectores.PuestoTutor);

                        //Empresa
                        var cellidEmp = row.CreateCell(47); cellidEmp.SetCellValue(beneficiario.Ficha.FichaEfectores.Empresa.IdEmpresa);
                        var cellrazonSocial = row.CreateCell(48); cellrazonSocial.SetCellValue(beneficiario.Ficha.FichaEfectores.Empresa.NombreEmpresa);
                        var cellCuit = row.CreateCell(49); cellCuit.SetCellValue(beneficiario.Ficha.FichaEfectores.Empresa.Cuit);
                        var cellCodact = row.CreateCell(50); cellCodact.SetCellValue(beneficiario.Ficha.FichaEfectores.Empresa.CodigoActividad);
                        var cellCantemp = row.CreateCell(51); cellCantemp.SetCellValue(beneficiario.Ficha.FichaEfectores.Empresa.CantidadEmpleados == null ?
                            String.Empty : beneficiario.Ficha.FichaEfectores.Empresa.CantidadEmpleados.ToString());
                        var cellCalleemp = row.CreateCell(52); cellCalleemp.SetCellValue(beneficiario.Ficha.FichaEfectores.Empresa.Calle);
                        var cellNumeroemp = row.CreateCell(53); cellNumeroemp.SetCellValue(beneficiario.Ficha.FichaEfectores.Empresa.Numero);
                        var cellPisoemp = row.CreateCell(54); cellPisoemp.SetCellValue(beneficiario.Ficha.FichaEfectores.Empresa.Piso);
                        var cellDptoemp = row.CreateCell(55); cellDptoemp.SetCellValue(beneficiario.Ficha.FichaEfectores.Empresa.Dpto);
                        var cellCpemp = row.CreateCell(56); cellCpemp.SetCellValue(beneficiario.Ficha.FichaEfectores.Empresa.CodigoPostal);
                        var cellIdlocemp = row.CreateCell(57); cellIdlocemp.SetCellValue(beneficiario.Ficha.FichaEfectores.Empresa.IdLocalidad == null ?
                            String.Empty : beneficiario.Ficha.FichaEfectores.Empresa.LocalidadEmpresa.IdLocalidad.ToString());
                        var cellLocemp = row.CreateCell(58); cellLocemp.SetCellValue(beneficiario.Ficha.FichaEfectores.Empresa.LocalidadEmpresa.NombreLocalidad);
                        var cellDepartamentoEmp = row.CreateCell(59); cellDepartamentoEmp.SetCellValue(beneficiario.Ficha.FichaEfectores.Empresa.IdLocalidad == null ?
                            String.Empty : beneficiario.Ficha.FichaEfectores.Empresa.LocalidadEmpresa.Departamento.NombreDepartamento);
                        var cellIdusuarioemp = row.CreateCell(60); cellIdusuarioemp.SetCellValue(beneficiario.Ficha.FichaEfectores.Empresa.Usuario.IdUsuarioEmpresa == null ?
                            String.Empty : beneficiario.Ficha.FichaEfectores.Empresa.Usuario.IdUsuarioEmpresa.ToString());
                        var cellNombreusuemp = row.CreateCell(61); cellNombreusuemp.SetCellValue(beneficiario.Ficha.FichaEfectores.Empresa.Usuario.NombreUsuario);
                        var cellApellidousuemp = row.CreateCell(62); cellApellidousuemp.SetCellValue(beneficiario.Ficha.FichaEfectores.Empresa.Usuario.ApellidoUsuario);
                        var cellEmailusuemp = row.CreateCell(63); cellEmailusuemp.SetCellValue(beneficiario.Ficha.FichaEfectores.Empresa.Usuario.Mail);
                        var cellTeleusuemp = row.CreateCell(64); cellTeleusuemp.SetCellValue(beneficiario.Ficha.FichaEfectores.Empresa.Usuario.Telefono);

                        //Sede
                        var cellIdSede = row.CreateCell(65); cellIdSede.SetCellValue(beneficiario.Ficha.FichaEfectores.IdSede == null ? String.Empty : beneficiario.Ficha.FichaEfectores.IdSede.ToString());
                        var cellnSede = row.CreateCell(66); cellnSede.SetCellValue(beneficiario.Ficha.FichaEfectores.Sede.NombreSede);
                        var cellCalleSede = row.CreateCell(67); cellCalleSede.SetCellValue(beneficiario.Ficha.FichaEfectores.Sede.Calle);
                        var cellNroSede = row.CreateCell(68); cellNroSede.SetCellValue(beneficiario.Ficha.FichaEfectores.Sede.Numero);
                        var cellPisoSede = row.CreateCell(69); cellPisoSede.SetCellValue(beneficiario.Ficha.FichaEfectores.Sede.Piso);
                        var cellDptoSede = row.CreateCell(70); cellDptoSede.SetCellValue(beneficiario.Ficha.FichaEfectores.Sede.Dpto);
                        var cellCpSede = row.CreateCell(71); cellCpSede.SetCellValue(beneficiario.Ficha.FichaEfectores.Sede.CodigoPostal);
                        var cellLocSede = row.CreateCell(72); cellLocSede.SetCellValue(beneficiario.Ficha.FichaEfectores.Sede.NombreLocalidad);
                        var cellApeContacto = row.CreateCell(73); cellApeContacto.SetCellValue(beneficiario.Ficha.FichaEfectores.Sede.ApellidoContacto);
                        var cellNomContacto = row.CreateCell(74); cellNomContacto.SetCellValue(beneficiario.Ficha.FichaEfectores.Sede.NombreContacto);
                        var cellTelcontacto = row.CreateCell(75); cellTelcontacto.SetCellValue(beneficiario.Ficha.FichaEfectores.Sede.Telefono);
                        var cellFaxContacto = row.CreateCell(76); cellFaxContacto.SetCellValue(beneficiario.Ficha.FichaEfectores.Sede.Fax);
                        var cellEmailContacto = row.CreateCell(77); cellEmailContacto.SetCellValue(beneficiario.Ficha.FichaEfectores.Sede.Email);

                        //Beneficiario
                        var cellIdben = row.CreateCell(78); cellIdben.SetCellValue(beneficiario.IdBeneficiario);
                        var cellIdestadoben = row.CreateCell(79); cellIdestadoben.SetCellValue(beneficiario.IdEstado);
                        var cellEstadoben = row.CreateCell(80); cellEstadoben.SetCellValue(beneficiario.NombreEstado);
                        var cellNrocuentaben = row.CreateCell(81); cellNrocuentaben.SetCellValue(beneficiario.NumeroCuenta == null ? String.Empty
                            : beneficiario.NumeroCuenta.ToString());
                        var cellIdsucursalben = row.CreateCell(82); cellIdsucursalben.SetCellValue(beneficiario.IdSucursal == null ? String.Empty
                            : beneficiario.IdSucursal.ToString());
                        var cellCodsucursalben = row.CreateCell(83); cellCodsucursalben.SetCellValue(beneficiario.CodigoSucursal);
                        var cellSucursalben = row.CreateCell(84); cellSucursalben.SetCellValue(beneficiario.SucursalDescripcion);
                        var cellFecsolben = row.CreateCell(85); cellFecsolben.SetCellValue(beneficiario.FechaSolicitudCuenta == null ? String.Empty
                            : beneficiario.FechaSolicitudCuenta.Value.ToShortDateString());
                        var cellNotificado = row.CreateCell(86); cellNotificado.SetCellValue(beneficiario.Notificado ? "S" : "N");
                        var cellFecNotif = row.CreateCell(87); cellFecNotif.SetCellValue(beneficiario.FechaNotificacion == null ? String.Empty
                            : beneficiario.FechaNotificacion.Value.ToShortDateString());

                        var cellFechaIniBenf = row.CreateCell(88); cellFechaIniBenf.SetCellValue(beneficiario.FechaInicioBeneficio==null ? string.Empty : beneficiario.FechaInicioBeneficio.Value.ToShortDateString()); // 20/03/2013 - DI CAMPLI LEANDRO
                        var cellBajaBenef = row.CreateCell(89); cellBajaBenef.SetCellValue(beneficiario.FechaBajaBeneficio==null ? string.Empty : beneficiario.FechaBajaBeneficio.Value.ToShortDateString()); // 20/03/2013 - DI CAMPLI LEANDRO


                        var cellTieneapoben = row.CreateCell(90); cellTieneapoben.SetCellValue(beneficiario.TieneApoderado);

                        //Apoderado
                        var cellIdapoderado = row.CreateCell(91); cellIdapoderado.SetCellValue(beneficiario.Apoderado.IdApoderadoNullable == null ?
                            String.Empty : beneficiario.Apoderado.IdApoderadoNullable.ToString());
                        var cellApellidoapo = row.CreateCell(92); cellApellidoapo.SetCellValue(beneficiario.Apoderado.Apellido);
                        var cellNombreapo = row.CreateCell(93); cellNombreapo.SetCellValue(beneficiario.Apoderado.Nombre);
                        var cellDniapo = row.CreateCell(94); cellDniapo.SetCellValue(beneficiario.Apoderado.NumeroDocumento);
                        var cellCuilapo = row.CreateCell(95); cellCuilapo.SetCellValue(beneficiario.Apoderado.Cuil);
                        var cellSexoapo = row.CreateCell(96); cellSexoapo.SetCellValue(beneficiario.Apoderado.Sexo);
                        var cellNroctaapo = row.CreateCell(97); cellNroctaapo.SetCellValue(beneficiario.Apoderado.NumeroCuentaBco == null ? String.Empty
                            : beneficiario.Apoderado.NumeroCuentaBco.ToString());
                        var cellIdSuc = row.CreateCell(98); cellIdSuc.SetCellValue(beneficiario.Apoderado.IdSucursal == null ? String.Empty
                            : beneficiario.Apoderado.SucursalApoderado.IdSucursal.ToString());
                        var cellCodsucapo = row.CreateCell(99); cellCodsucapo.SetCellValue(beneficiario.Apoderado.SucursalApoderado.CodigoBanco);
                        var cellSucursalapo = row.CreateCell(100); cellSucursalapo.SetCellValue(beneficiario.Apoderado.SucursalApoderado.Detalle);
                        var cellFecsolcuentaapo = row.CreateCell(101); cellFecsolcuentaapo.SetCellValue(beneficiario.Apoderado.FechaSolicitudCuenta == null ?
                            String.Empty : beneficiario.Apoderado.FechaSolicitudCuenta.Value.ToShortDateString());
                        var cellFecnac = row.CreateCell(102); cellFecnac.SetCellValue(beneficiario.Apoderado.FechaNacimiento == null ? String.Empty :
                            beneficiario.Apoderado.FechaNacimiento.Value.ToShortDateString());
                        var cellCalleapo = row.CreateCell(103); cellCalleapo.SetCellValue(beneficiario.Apoderado.Calle);
                        var cellNroapo = row.CreateCell(104); cellNroapo.SetCellValue(beneficiario.Apoderado.Numero);
                        var cellPisoapo = row.CreateCell(105); cellPisoapo.SetCellValue(beneficiario.Apoderado.Piso);
                        var cellMonoblockapo = row.CreateCell(106); cellMonoblockapo.SetCellValue(beneficiario.Apoderado.Monoblock);
                        var cellDptoapo = row.CreateCell(107); cellDptoapo.SetCellValue(beneficiario.Apoderado.Dpto);
                        var cellParcelaapo = row.CreateCell(108); cellParcelaapo.SetCellValue(beneficiario.Apoderado.Parcela);
                        var cellManzanaapo = row.CreateCell(109); cellManzanaapo.SetCellValue(beneficiario.Apoderado.Manzana);
                        var cellEntrecalleapo = row.CreateCell(110); cellEntrecalleapo.SetCellValue(beneficiario.Apoderado.EntreCalles);
                        var cellBarrioapo = row.CreateCell(111); cellBarrioapo.SetCellValue(beneficiario.Apoderado.Barrio);
                        var cellCpapo = row.CreateCell(112); cellCpapo.SetCellValue(beneficiario.Apoderado.CodigoPostal);
                        var cellIdlocalidadapo = row.CreateCell(113); cellIdlocalidadapo.SetCellValue(beneficiario.Apoderado.IdLocalidad == null ? String.Empty
                            : beneficiario.Apoderado.LocalidadApoderado.IdLocalidad.ToString());
                        var cellLocalidadapo = row.CreateCell(114); cellLocalidadapo.SetCellValue(beneficiario.Apoderado.LocalidadApoderado.NombreLocalidad);
                        var cellIddeptoapo = row.CreateCell(115); cellIddeptoapo.SetCellValue(beneficiario.Apoderado.IdLocalidad == null ? String.Empty
                            : beneficiario.Apoderado.LocalidadApoderado.Departamento.IdDepartamento.ToString());
                        var cellDeptoapo = row.CreateCell(116); cellDeptoapo.SetCellValue(beneficiario.Apoderado.LocalidadApoderado.Departamento.NombreDepartamento);
                        var cellTelefonoapo = row.CreateCell(117); cellTelefonoapo.SetCellValue(beneficiario.Apoderado.TelefonoFijo);
                        var cellCelularapo = row.CreateCell(118); cellCelularapo.SetCellValue(beneficiario.Apoderado.TelefonoCelular);
                        var cellEmailapo = row.CreateCell(119);
                        cellEmailapo.SetCellValue(beneficiario.Apoderado.Mail);
                    }
                    #endregion
                    break;
            }

            var ms = new MemoryStream();

            templateWorkbook.Write(ms);

            return ms.ToArray();
        }


        // 26/02/2013 - LEANDRO DI CAMPLI - MEJORAR PERFORMANCE DE EXPORTAR EXCEL DE BENEFICIARIOS
        /// <summary>
        /// NO VIGENTE SOLO PARA PRUEBAS Y MEJORA DEL RENDIMIENTO DE LA CONSULTA
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="apellido"></param>
        /// <param name="cuil"></param>
        /// <param name="numeroDocumento"></param>
        /// <param name="idPrograma"></param>
        /// <param name="conApoderados"></param>
        /// <param name="modalidad"></param>
        /// <param name="discapacitado"></param>
        /// <param name="altatemprana"></param>
        /// <param name="idEstado"></param>
        /// <param name="apellidonombreapoderado"></param>
        /// <returns></returns>
        public IBeneficiariosVista GetBeneficiarioConCuentaBis(string nombre, string apellido, string cuil, string numeroDocumento, int idPrograma, string conApoderados, string modalidad, string discapacitado, string altatemprana, int idEstado, string apellidonombreapoderado, int idEtapa, string nrotramite)
        {
            IBeneficiariosVista vista = new BeneficiariosVista();

            bool accesoprograma = base.AccesoPrograma(Enums.Formulario.AbmBeneficiario);

            vista.ListaBeneficiarios = accesoprograma
                              ? _beneficiariorepositorio.GetBeneficiarioConCuenta(nombre, apellido, cuil,
                                                                                  numeroDocumento, idPrograma,
                                                                                  conApoderados, modalidad,
                                                                                  discapacitado, altatemprana, idEstado,
                                                                                  apellidonombreapoderado, idEtapa, nrotramite).OrderBy(
                                                                                                 c => c.Ficha.Apellido).
                    ThenBy(c => c.Ficha.Nombre).ToList()
                              : _beneficiariorepositorio.GetBeneficiarioConCuenta(nombre, apellido, cuil,
                                                                                  numeroDocumento, idPrograma,
                                                                                  conApoderados, modalidad,
                                                                                  discapacitado, altatemprana, idEstado,
                                                                                  apellidonombreapoderado, idEtapa, nrotramite).Where(
                                                                                      c =>
                                                                                      c.IdPrograma !=
                                                                                      (int)
                                                                                      Enums.Programas.EfectoresSociales).OrderBy(
                                                                                                 c => c.Ficha.Apellido).
                    ThenBy(c => c.Ficha.Nombre).ToList();
           


            vista.ConAltaTemprana = altatemprana;
            vista.ConApoderado = conApoderados;
            vista.ConDiscapacidad = discapacitado;
            vista.ConModalidad = modalidad;
            vista.ConCuenta = "S";

            foreach (var beneficiario in vista.ListaBeneficiarios.
                Where(beneficiario => beneficiario.Ficha.EstadoCivil == (int)Enums.EstadoCivil.CasadoConviviente))
            {
                beneficiario.Conyugue = _conyuguerepositorio.GetConyugeByBeneficiario(beneficiario.IdBeneficiario) ?? new Conyuge();
            }

            var lista = _programarepositorio.GetProgramas();
            CargarPrograma(vista, lista);

            CargarEstadosBeneficiarioforIndex(vista, _estadobeneficiariorepositorio.GetEstadosBeneficiario());

            vista.Programas.Selected = idPrograma.ToString();

            vista.EstadosBeneficiarios.Selected = idEstado.ToString();

            return vista;
        }

        // 26/02/2013 - LEANDRO DI CAMPLI - MEJORAR PERFORMANCE DE EXPORTAR EXCEL DE BENEFICIARIOS
        /// <summary>
        ///  NO VIGENTE SOLO PARA PRUEBAS Y MEJORA DEL RENDIMIENTO DE LA CONSULTA
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="apellido"></param>
        /// <param name="cuil"></param>
        /// <param name="numeroDocumento"></param>
        /// <param name="idPrograma"></param>
        /// <param name="conApoderados"></param>
        /// <param name="modalidad"></param>
        /// <param name="discapacitado"></param>
        /// <param name="altatemprana"></param>
        /// <param name="idEstado"></param>
        /// <param name="apellidonombreapoderado"></param>
        /// <returns></returns>
        public IBeneficiariosVista GetBeneficiarioSinCuentaBis(string nombre, string apellido, string cuil, string numeroDocumento, int idPrograma, string conApoderados, string modalidad, string discapacitado, string altatemprana, int idEstado, string apellidonombreapoderado, int idEtapa, string nrotramite)
        {
            IBeneficiariosVista vista = new BeneficiariosVista();

            var accesoprograma = base.AccesoPrograma(Enums.Formulario.AbmBeneficiario);

            vista.ListaBeneficiarios = accesoprograma == true
                              ? _beneficiariorepositorio.GetBeneficiarioSinCuenta(nombre, apellido, cuil,
                                                                                  numeroDocumento, idPrograma,
                                                                                  conApoderados, modalidad,
                                                                                  discapacitado, altatemprana, idEstado,
                                                                                  apellidonombreapoderado, idEtapa, nrotramite).
                    OrderBy(c => c.Ficha.Apellido).ThenBy(c => c.Ficha.Nombre).ToList()
                              : _beneficiariorepositorio.GetBeneficiarioSinCuenta(nombre, apellido, cuil,
                                                                                  numeroDocumento, idPrograma,
                                                                                  conApoderados, modalidad,
                                                                                  discapacitado, altatemprana, idEstado,
                                                                                  apellidonombreapoderado, idEtapa, nrotramite).Where(
                                                                                      c =>
                                                                                      c.IdPrograma !=
                                                                                      (int)
                                                                                      Enums.Programas.EfectoresSociales).
                    OrderBy(c => c.Ficha.Apellido).ThenBy(c => c.Ficha.Nombre).ToList();

            

            vista.ConCuenta = "N";
            vista.ConAltaTemprana = altatemprana;
            vista.ConApoderado = conApoderados;
            vista.ConDiscapacidad = discapacitado;
            vista.ConModalidad = modalidad;

            foreach (var beneficiario in vista.ListaBeneficiarios.Where(beneficiario => beneficiario.Ficha.EstadoCivil == (int)Enums.EstadoCivil.CasadoConviviente))
            {
                beneficiario.Conyugue = _conyuguerepositorio.GetConyugeByBeneficiario(beneficiario.IdBeneficiario) ?? new Conyuge();
            }

            var lista = _programarepositorio.GetProgramas();

            CargarPrograma(vista, lista);

            vista.Programas.Selected = idPrograma.ToString();

            CargarEstadosBeneficiarioforIndex(vista, _estadobeneficiariorepositorio.GetEstadosBeneficiario());

            return vista;
        }

        // 26/02/2013 - LEANDRO DI CAMPLI - MEJORAR PERFORMANCE DE EXPORTAR EXCEL DE BENEFICIARIOS
        /// <summary>
        ///  NO VIGENTE SOLO PARA PRUEBAS Y MEJORA DEL RENDIMIENTO DE LA CONSULTA
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="apellido"></param>
        /// <param name="cuil"></param>
        /// <param name="numeroDocumento"></param>
        /// <param name="idPrograma"></param>
        /// <param name="conApoderados"></param>
        /// <param name="modalidad"></param>
        /// <param name="discapacitado"></param>
        /// <param name="altatemprana"></param>
        /// <param name="idEstado"></param>
        /// <param name="apellidonombreapoderado"></param>
        /// <returns></returns>
        public IBeneficiariosVista GetBeneficiariosBis(string nombre, string apellido, string cuil, string numeroDocumento, int idPrograma, string conApoderados, string modalidad, string discapacitado, string altatemprana, int idEstado, string apellidonombreapoderado, int idEtapa)
        {
            IBeneficiariosVista vista = new BeneficiariosVista();

            var accesoprograma = base.AccesoPrograma(Enums.Formulario.AbmBeneficiario);

            vista.ListaBeneficiarios  = accesoprograma == true
                               ? _beneficiariorepositorio.GetBeneficiarios(nombre, apellido, cuil, numeroDocumento,
                                                                           idPrograma, conApoderados, modalidad,
                                                                           discapacitado, altatemprana, idEstado,
                                                                           apellidonombreapoderado, idEtapa, "")
                               : _beneficiariorepositorio.GetBeneficiarios(nombre, apellido, cuil, numeroDocumento,
                                                                           idPrograma, conApoderados, modalidad,
                                                                           discapacitado, altatemprana, idEstado,
                                                                           apellidonombreapoderado,
                                                                           (int)Enums.Programas.EfectoresSociales, idEtapa);

           
            vista.FileDownload = "../Archivos/" + vista.FileNombre;


           

            vista.ApellidoBusqueda = apellido;
            vista.CuilBusqueda = cuil;
            vista.NombreBusqueda = nombre;
            vista.NumeroDocumentoBusqueda = numeroDocumento;
            vista.ConCuenta = "T";

            vista.ConAltaTemprana = altatemprana ?? "T";
            vista.ConApoderado = conApoderados ?? "T";
            vista.ConDiscapacidad = discapacitado ?? "T";
            vista.ConModalidad = modalidad ?? "T";

            var lista = _programarepositorio.GetProgramas();

            CargarPrograma(vista, lista);

            CargarEstadosBeneficiarioforIndex(vista, _estadobeneficiariorepositorio.GetEstadosBeneficiario());

            return vista;
        }


        private void CargarProgramaEtapa(IBeneficiariosVista vista, IEnumerable<IPrograma> listaProgramas)
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



        public byte[] ExportPPP(IBeneficiariosVista vista)
        {
            var path = String.Empty;

            switch (Convert.ToInt32(vista.Programas.Selected))
            {

                case (int)Enums.TipoFicha.Ppp:
                    path = HttpContext.Current.Server.MapPath(@"\Archivos\Exportados\TemplateBeneficiariosPPP.xls");
                    break;
                default:
                    return new byte[] { };
            }

            var fs = new FileStream(path, FileMode.Open, FileAccess.Read);

            var templateWorkbook = new HSSFWorkbook(fs, true);

            var sheet = templateWorkbook.GetSheet("Nomina");


            List<Reportes_PPP> listaparaExcel = null;
            //listaparaExcel = new
                   listaparaExcel = _beneficiarioRepo.getBenefPPP(vista.NombreBusqueda,
                                                                              vista.ApellidoBusqueda,
                                                                              vista.CuilBusqueda,
                                                                              vista.NumeroDocumentoBusqueda,
                                                                              Convert.ToInt32(vista.Programas.Selected),
                                                                              vista.ConApoderado, vista.ConModalidad,
                                                                              vista.ConDiscapacidad,
                                                                              vista.ConAltaTemprana,
                                                                              Convert.ToInt32(vista.EstadosBeneficiarios.Selected),
                                                                              vista.ApellidoNombreApoderadoBusqueda, Convert.ToInt32(vista.etapas.Selected), vista.ConCuenta, Convert.ToInt32(vista.ComboTiposPpp.Selected), Convert.ToInt32(vista.ComboSubprogramas.Selected), vista.nrotramite_busqueda);



            var i = 1;

            var tiposppp = _tipopppRepositorio.GetTiposPpp();

            switch (Convert.ToInt32(vista.Programas.Selected))
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

        public byte[] ExportTerciario(IBeneficiariosVista vista)
        {
            var path = String.Empty;

            switch (Convert.ToInt32(vista.Programas.Selected))
            {
                case (int)Enums.TipoFicha.Terciaria:
                    path = HttpContext.Current.Server.MapPath(@"\Archivos\Exportados\TemplateBeneficiarioTerciarios.xls");
                    break;

                default:
                    return new byte[] { };
            }

            var fs = new FileStream(path, FileMode.Open, FileAccess.Read);

            var templateWorkbook = new HSSFWorkbook(fs, true);

            var sheet = templateWorkbook.GetSheet("Nomina");


            List<VT_REPORTES_TER> listaparaExcel = null;
            //listaparaExcel = new
            listaparaExcel = _beneficiarioRepo.getBenefTer(vista.NombreBusqueda,
                                                                       vista.ApellidoBusqueda,
                                                                       vista.CuilBusqueda,
                                                                       vista.NumeroDocumentoBusqueda,
                                                                       Convert.ToInt32(vista.Programas.Selected),
                                                                       vista.ConApoderado, vista.ConModalidad,
                                                                       vista.ConDiscapacidad,
                                                                       vista.ConAltaTemprana,
                                                                       Convert.ToInt32(vista.EstadosBeneficiarios.Selected),
                                                                       vista.ApellidoNombreApoderadoBusqueda, Convert.ToInt32(vista.etapas.Selected), vista.ConCuenta, Convert.ToInt32(vista.ComboSubprogramas.Selected));



            var i = 1;

            switch (Convert.ToInt32(vista.Programas.Selected))
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

        public byte[] ExportUniversitario(IBeneficiariosVista vista)
        {
            var path = String.Empty;

            switch (Convert.ToInt32(vista.Programas.Selected))
            {

                case (int)Enums.TipoFicha.Universitaria:
                    path = HttpContext.Current.Server.MapPath(@"\Archivos\Exportados\TemplateBeneficiarioUniversitarios.xls");
                    break;

                default:
                    return new byte[] { };
            }

            var fs = new FileStream(path, FileMode.Open, FileAccess.Read);

            var templateWorkbook = new HSSFWorkbook(fs, true);

            var sheet = templateWorkbook.GetSheet("Nomina");


            List<VT_REPORTES_UNI> listaparaExcel = null;
            //listaparaExcel = new
            listaparaExcel = _beneficiarioRepo.getBenefUni(vista.NombreBusqueda,
                                                                       vista.ApellidoBusqueda,
                                                                       vista.CuilBusqueda,
                                                                       vista.NumeroDocumentoBusqueda,
                                                                       Convert.ToInt32(vista.Programas.Selected),
                                                                       vista.ConApoderado, vista.ConModalidad,
                                                                       vista.ConDiscapacidad,
                                                                       vista.ConAltaTemprana,
                                                                       Convert.ToInt32(vista.EstadosBeneficiarios.Selected),
                                                                       vista.ApellidoNombreApoderadoBusqueda, Convert.ToInt32(vista.etapas.Selected), vista.ConCuenta, Convert.ToInt32(vista.ComboSubprogramas.Selected));



            var i = 1;

            switch (Convert.ToInt32(vista.Programas.Selected))
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

        public byte[] ExportPPPprof(IBeneficiariosVista vista)
        {
            var path = String.Empty;

            switch (Convert.ToInt32(vista.Programas.Selected))
            {

                case (int)Enums.TipoFicha.PppProf:
                    path = HttpContext.Current.Server.MapPath(@"\Archivos\Exportados\TemplateBeneficiariosPPPP.xls");
                    break;
                default:
                    return new byte[] { };
            }

            var fs = new FileStream(path, FileMode.Open, FileAccess.Read);

            var templateWorkbook = new HSSFWorkbook(fs, true);

            var sheet = templateWorkbook.GetSheet("Nomina");


            List<VT_REPORTES_PPPPROF> listaparaExcel = null;
            //listaparaExcel = new
            listaparaExcel = _beneficiarioRepo.getBenefPPPprof(vista.NombreBusqueda,
                                                                       vista.ApellidoBusqueda,
                                                                       vista.CuilBusqueda,
                                                                       vista.NumeroDocumentoBusqueda,
                                                                       Convert.ToInt32(vista.Programas.Selected),
                                                                       vista.ConApoderado, vista.ConModalidad,
                                                                       vista.ConDiscapacidad,
                                                                       vista.ConAltaTemprana,
                                                                       Convert.ToInt32(vista.EstadosBeneficiarios.Selected),
                                                                       vista.ApellidoNombreApoderadoBusqueda, Convert.ToInt32(vista.etapas.Selected), vista.ConCuenta);



            var i = 1;

            switch (Convert.ToInt32(vista.Programas.Selected))
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

        public byte[] ExportVat(IBeneficiariosVista vista)
        {
            var path = String.Empty;

            switch (Convert.ToInt32(vista.Programas.Selected))
            {

                case (int)Enums.TipoFicha.Vat:
                    path = HttpContext.Current.Server.MapPath(@"\Archivos\Exportados\TemplateBeneficiariosVAT.xls");
                    break;
                default:
                    return new byte[] { };
            }

            var fs = new FileStream(path, FileMode.Open, FileAccess.Read);

            var templateWorkbook = new HSSFWorkbook(fs, true);

            var sheet = templateWorkbook.GetSheet("Nomina");


            List<VT_REPORTES_VAT> listaparaExcel = null;
            //listaparaExcel = new
            listaparaExcel = _beneficiarioRepo.getBenefVat(vista.NombreBusqueda,
                                                                       vista.ApellidoBusqueda,
                                                                       vista.CuilBusqueda,
                                                                       vista.NumeroDocumentoBusqueda,
                                                                       Convert.ToInt32(vista.Programas.Selected),
                                                                       vista.ConApoderado, vista.ConModalidad,
                                                                       vista.ConDiscapacidad,
                                                                       vista.ConAltaTemprana,
                                                                       Convert.ToInt32(vista.EstadosBeneficiarios.Selected),
                                                                       vista.ApellidoNombreApoderadoBusqueda, Convert.ToInt32(vista.etapas.Selected), vista.ConCuenta);



            var i = 1;

            switch (Convert.ToInt32(vista.Programas.Selected))
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

        public byte[] ExportRec_Prod(IBeneficiariosVista vista)
        {
            var path = String.Empty;

            switch (Convert.ToInt32(vista.Programas.Selected))
            {
                case (int)Enums.TipoFicha.ReconversionProductiva:
                    path = HttpContext.Current.Server.MapPath(@"\Archivos\Exportados\TemplateBeneficiariosReconversion.xls");
                    break;

                default:
                    return new byte[] { };
            }

            var fs = new FileStream(path, FileMode.Open, FileAccess.Read);

            var templateWorkbook = new HSSFWorkbook(fs, true);

            var sheet = templateWorkbook.GetSheet("Nomina");


            List<VT_REPORTES_REC_PROD> listaparaExcel = null;
            //listaparaExcel = new



            listaparaExcel = _beneficiarioRepo.getBenefRec_Prod(vista.NombreBusqueda,
                                                                       vista.ApellidoBusqueda,
                                                                       vista.CuilBusqueda,
                                                                       vista.NumeroDocumentoBusqueda,
                                                                       Convert.ToInt32(vista.Programas.Selected),
                                                                       vista.ConApoderado, vista.ConModalidad,
                                                                       vista.ConDiscapacidad,
                                                                       vista.ConAltaTemprana,
                                                                       Convert.ToInt32(vista.EstadosBeneficiarios.Selected),
                                                                       vista.ApellidoNombreApoderadoBusqueda, Convert.ToInt32(vista.etapas.Selected), vista.ConCuenta);


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

            switch (Convert.ToInt32(vista.Programas.Selected))
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

        public byte[] ExportEfec_Soc(IBeneficiariosVista vista)
        {
            var path = String.Empty;

            switch (Convert.ToInt32(vista.Programas.Selected))
            {

                case (int)Enums.TipoFicha.EfectoresSociales:
                    path = HttpContext.Current.Server.MapPath(@"\Archivos\Exportados\TemplateBeneficiariosEfectoresSociales.xls");
                    break;
                default:
                    return new byte[] { };
            }

            var fs = new FileStream(path, FileMode.Open, FileAccess.Read);

            var templateWorkbook = new HSSFWorkbook(fs, true);

            var sheet = templateWorkbook.GetSheet("Nomina");


            List<VT_REPORTES_EFEC_SOC> listaparaExcel = null;
            //listaparaExcel = new
            listaparaExcel = _beneficiarioRepo.getBenefEfec_Soc(vista.NombreBusqueda,
                                                                       vista.ApellidoBusqueda,
                                                                       vista.CuilBusqueda,
                                                                       vista.NumeroDocumentoBusqueda,
                                                                       Convert.ToInt32(vista.Programas.Selected),
                                                                       vista.ConApoderado, vista.ConModalidad,
                                                                       vista.ConDiscapacidad,
                                                                       vista.ConAltaTemprana,
                                                                       Convert.ToInt32(vista.EstadosBeneficiarios.Selected),
                                                                       vista.ApellidoNombreApoderadoBusqueda, Convert.ToInt32(vista.etapas.Selected), vista.ConCuenta);

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

                        //verifico que subprogramas tiene disponible
                        var lsubprogramas = (from rs in _subprogramaRepositorio.GetSubprogramasRol()
                                             join u in usuarioRoles on rs.Id_Rol equals u.IdRol
                                             where rs.IdPrograma == (int)Enums.TipoFicha.EfectoresSociales
                                             group rs by new { rs.IdSubprograma, rs.NombreSubprograma } into grouping
                                             select new
                                                 Subprograma
                                             {
                                                 IdSubprograma = grouping.Key.IdSubprograma,
                                                 NombreSubprograma = grouping.Key.NombreSubprograma
                                             }).ToList();


                        listaparaExcel = (from f in listaparaExcel
                                          join sub in lsubprogramas on f.ID_SUBPROGRAMA equals sub.IdSubprograma
                                          where f.ID_SUBPROGRAMA != null
                                          select f).ToList();
                    
                }
                //**********************************************
            }

            var i = 1;

            switch (Convert.ToInt32(vista.Programas.Selected))
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


        public byte[] ExportConf_Vos(IBeneficiariosVista vista)
        {
            var path = String.Empty;

            switch (Convert.ToInt32(vista.Programas.Selected))
            {
                case (int)Enums.TipoFicha.ConfiamosEnVos:
                    path = HttpContext.Current.Server.MapPath(@"\Archivos\Exportados\TemplateBeneficiariosConfVos.xls");
                    break;

                default:
                    return new byte[] { };
            }

            var fs = new FileStream(path, FileMode.Open, FileAccess.Read);

            var templateWorkbook = new HSSFWorkbook(fs, true);

            var sheet = templateWorkbook.GetSheet("Nomina");


            List<VT_REPORTES_CONF_VOS> listaparaExcel = null;
            //listaparaExcel = new
            listaparaExcel = _beneficiarioRepo.getBenefConfVos(vista.NombreBusqueda,
                                                                       vista.ApellidoBusqueda,
                                                                       vista.CuilBusqueda,
                                                                       vista.NumeroDocumentoBusqueda,
                                                                       Convert.ToInt32(vista.Programas.Selected),
                                                                       vista.ConApoderado, vista.ConModalidad,
                                                                       vista.ConDiscapacidad,
                                                                       vista.ConAltaTemprana,
                                                                       Convert.ToInt32(vista.EstadosBeneficiarios.Selected),
                                                                       vista.ApellidoNombreApoderadoBusqueda, Convert.ToInt32(vista.etapas.Selected), vista.ConCuenta);


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

            switch (Convert.ToInt32(vista.Programas.Selected))
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
                    //#region case (int)Enums.TipoFicha.ConfiamosEnVos
                    //foreach (var beneficiario in listaparaExcel)
                    //{
                    //    i++;
                    //    //Ficha
                    //    var row = sheet.CreateRow(i);
                    //    var cellidficha = row.CreateCell(0); cellidficha.SetCellValue(beneficiario.ID_FICHA);
                    //    var cellApellido = row.CreateCell(1); cellApellido.SetCellValue(beneficiario.APELLIDO);
                    //    var cellNombre = row.CreateCell(2); cellNombre.SetCellValue(beneficiario.NOMBRE);
                    //    var cellCuil = row.CreateCell(3); cellCuil.SetCellValue(beneficiario.CUIL);
                    //    var cellDni = row.CreateCell(4); cellDni.SetCellValue(beneficiario.NUMERO_DOCUMENTO);
                    //    var cellFechaNa = row.CreateCell(5); cellFechaNa.SetCellValue(beneficiario.FER_NAC == null ? String.Empty
                    //        : beneficiario.FER_NAC.ToShortDateString());
                    //    var cellCalle = row.CreateCell(6); cellCalle.SetCellValue(beneficiario.CALLE);
                    //    var cellNro = row.CreateCell(7); cellNro.SetCellValue(beneficiario.NUMERO);
                    //    var cellPiso = row.CreateCell(8); cellPiso.SetCellValue(beneficiario.PISO);
                    //    var cellDpto = row.CreateCell(9); cellDpto.SetCellValue(beneficiario.DPTO);
                    //    var cellMono = row.CreateCell(10); cellMono.SetCellValue(beneficiario.MONOBLOCK);
                    //    var cellParcela = row.CreateCell(11); cellParcela.SetCellValue(beneficiario.PARCELA);
                    //    var cellManzana = row.CreateCell(12); cellManzana.SetCellValue(beneficiario.MANZANA);
                    //    var cellEntreCalle = row.CreateCell(13); cellEntreCalle.SetCellValue(beneficiario.ENTRECALLES);
                    //    var cellBarrio = row.CreateCell(14); cellBarrio.SetCellValue(beneficiario.BARRIO);
                    //    var cellCp = row.CreateCell(15); cellCp.SetCellValue(beneficiario.CODIGO_POSTAL);
                    //    var cellidLocalidad = row.CreateCell(16); cellidLocalidad.SetCellValue(beneficiario.ID_LOCALIDAD == null ?
                    //        String.Empty : beneficiario.ID_LOCALIDAD.ToString());
                    //    var cellnLocalidad = row.CreateCell(17); cellnLocalidad.SetCellValue(beneficiario.N_LOCALIDAD);
                    //    var cellIdDepartamento = row.CreateCell(18); cellIdDepartamento.SetCellValue(beneficiario.ID_LOCALIDAD == null ?
                    //        String.Empty : beneficiario.ID_DPTO.ToString());
                    //    var cellnDepartamento = row.CreateCell(19); cellnDepartamento.SetCellValue(beneficiario.N_DEPARTAMENTO);
                    //    var cellTelFijo = row.CreateCell(20); cellTelFijo.SetCellValue(beneficiario.TEL_FIJO);
                    //    var cellTelCelular = row.CreateCell(21); cellTelCelular.SetCellValue(beneficiario.TEL_CELULAR);
                    //    var cellContacto = row.CreateCell(22); cellContacto.SetCellValue(beneficiario.CONTACTO);
                    //    var cellEmail = row.CreateCell(23); cellEmail.SetCellValue(beneficiario.MAIL);

                    //    var cellTieneHijos = row.CreateCell(24); cellTieneHijos.SetCellValue(beneficiario.TIENE_HIJOS);//TIENE_HIJOS	
                    //    var cellCantidadHijos = row.CreateCell(25); cellCantidadHijos.SetCellValue(beneficiario.CANTIDAD_HIJOS == null ? "0" : beneficiario.CANTIDAD_HIJOS.ToString());//CANTIDAD_HIJOS
                    //    var cellEsDiscapacitado = row.CreateCell(26); cellEsDiscapacitado.SetCellValue(beneficiario.ES_DISCAPACITADO == null ? "N" : beneficiario.ES_DISCAPACITADO);//ES_DIACAPACITADO
                    //    var cellTieneDeficienciaMotora = row.CreateCell(27); cellTieneDeficienciaMotora.SetCellValue(beneficiario.TIENE_DEF_MOTORA == null ? "N" : beneficiario.TIENE_DEF_MOTORA);//TIENE_DEF_MOTORA
                    //    var cellTieneDeficienciaMental = row.CreateCell(28); cellTieneDeficienciaMental.SetCellValue(beneficiario.TIENE_DEF_MENTAL == null ? "N" : beneficiario.TIENE_DEF_MENTAL);//TIENE_DEF_MENTAL
                    //    var cellTieneDeficienciaSensorial = row.CreateCell(29); cellTieneDeficienciaSensorial.SetCellValue(beneficiario.TIENE_DEF_SENSORIAL == null ? "N" : beneficiario.TIENE_DEF_SENSORIAL);//TIENE_DEF_SENSORIAL
                    //    var cellTieneDeficienciaPsicologia = row.CreateCell(30); cellTieneDeficienciaPsicologia.SetCellValue(beneficiario.TIENE_DEF_PSICOLOGICA == null ? "N" : beneficiario.TIENE_DEF_PSICOLOGICA);//TIENE_DEF_PSICOLOGICA
                    //    var cellDeficienciaOtra = row.CreateCell(31); cellDeficienciaOtra.SetCellValue(beneficiario.DEFICIENCIA_OTRA == null ? "N" : beneficiario.TIENE_DEF_PSICOLOGICA);//DEFICIENCIA_OTRA
                    //    var cellCertificadoDiscapacidad = row.CreateCell(32); cellCertificadoDiscapacidad.SetCellValue(beneficiario.CERTIF_DISCAP == null ? "N" : beneficiario.CERTIF_DISCAP);//CERTIF_DISCAP

                    //    var cellSexo = row.CreateCell(33); cellSexo.SetCellValue(beneficiario.SEXO);
                    //    var cellidestadoFicha = row.CreateCell(34); cellidestadoFicha.SetCellValue(beneficiario.ID_EST_FIC == null ? String.Empty :
                    //        beneficiario.ID_EST_FIC.ToString());
                    //    var cellnestadoFicha = row.CreateCell(35); cellnestadoFicha.SetCellValue(beneficiario.N_ESTADO_FICHA);

                    //    var cellNivelEscolaridad = row.CreateCell(36); cellNivelEscolaridad.SetCellValue(beneficiario.NIVEL_ESCOLARIDAD);

                    //    var cellModalidad = row.CreateCell(37); cellModalidad.SetCellValue(beneficiario.MODALIDAD == (int)Enums.Modalidad.Entrenamiento
                    //        ? "Entrenamiento" : "CTI");
                    //    var cellaltaTemprana = row.CreateCell(38); cellaltaTemprana.SetCellValue(beneficiario.ALTA_TEMPRANA);
                    //    var cellatFechaInicio = row.CreateCell(39); cellatFechaInicio.SetCellValue(beneficiario.AT_FECHA_INICIO == null ?
                    //        String.Empty : beneficiario.AT_FECHA_INICIO.Value.ToShortDateString());
                    //    var cellatFechaCese = row.CreateCell(40); cellatFechaCese.SetCellValue(beneficiario.AT_FECHA_CESE == null ?
                    //        String.Empty : beneficiario.AT_FECHA_CESE.Value.ToShortDateString());
                    //    var cellidModContAfip = row.CreateCell(41); cellidModContAfip.SetCellValue(beneficiario.ID_MOD_CONT_AFIP == null ?
                    //        String.Empty : beneficiario.ID_MOD_CONT_AFIP.ToString());
                    //    var cellModContAfip = row.CreateCell(42); cellModContAfip.SetCellValue(beneficiario.MOD_CONT_AFIP);

                    //    var cellFechaModif = row.CreateCell(43); cellFechaModif.SetCellValue(beneficiario.FEC_MODIF == null ? String.Empty
                    //        : beneficiario.FEC_MODIF.Value.ToShortDateString());

                    //    var cellnSubprograma = row.CreateCell(44); cellnSubprograma.SetCellValue(beneficiario.SUBPROGRAMA);

                    //    //Proyecto
                    //    var cellnProyecto = row.CreateCell(45); cellnProyecto.SetCellValue(beneficiario.N_PROYECTO);
                    //    var cellCantCapacitar = row.CreateCell(46); cellCantCapacitar.SetCellValue(beneficiario.CANT_A_CAPACITAR == null ? String.Empty : beneficiario.CANT_A_CAPACITAR.ToString());
                    //    var cellFecInicio = row.CreateCell(47); cellFecInicio.SetCellValue(beneficiario.PRO_FEC_INICIO == null ? String.Empty : beneficiario.PRO_FEC_INICIO.Value.ToShortDateString());
                    //    var cellFecFin = row.CreateCell(48); cellFecFin.SetCellValue(beneficiario.PRO_FEC_FIN == null ? String.Empty : beneficiario.PRO_FEC_FIN.Value.ToShortDateString());
                    //    var cellMesesDuracion = row.CreateCell(49); cellMesesDuracion.SetCellValue(beneficiario.MESES_DURACION == null ? String.Empty : beneficiario.MESES_DURACION.ToString());
                    //    var cellAportesEmpresa = row.CreateCell(50); cellAportesEmpresa.SetCellValue(beneficiario.APORTES_DE_EMPRESA);
                    //    var cellApeTutor = row.CreateCell(51); cellApeTutor.SetCellValue(beneficiario.APELLIDO_TUTOR);
                    //    var cellNomTutor = row.CreateCell(52); cellNomTutor.SetCellValue(beneficiario.NOMBRE_TUTOR);
                    //    var cellDocTutor = row.CreateCell(53); cellDocTutor.SetCellValue(beneficiario.DNI_TUTOR);
                    //    var cellTelTutor = row.CreateCell(54); cellTelTutor.SetCellValue(beneficiario.TELEFONO_TUTOR);
                    //    var cellEmailTutor = row.CreateCell(55); cellEmailTutor.SetCellValue(beneficiario.EMAIL_TUTOR);
                    //    var cellPuestoTutor = row.CreateCell(56); cellPuestoTutor.SetCellValue(beneficiario.PUESTO_TUTOR);

                    //    //Empresa
                    //    var cellidEmp = row.CreateCell(57); cellidEmp.SetCellValue(beneficiario.ID_EMP == null ? "" : beneficiario.ID_EMP.ToString());
                    //    var cellrazonSocial = row.CreateCell(58); cellrazonSocial.SetCellValue(beneficiario.RAZON_SOCIAL);
                    //    var cellCuit = row.CreateCell(59); cellCuit.SetCellValue(beneficiario.EMP_CUIT);
                    //    var cellCodact = row.CreateCell(60); cellCodact.SetCellValue(beneficiario.COD_ACT);
                    //    var cellCantemp = row.CreateCell(61); cellCantemp.SetCellValue(beneficiario.CANT_EMP == null ?
                    //        String.Empty : beneficiario.CANT_EMP.ToString());
                    //    var cellCalleemp = row.CreateCell(62); cellCalleemp.SetCellValue(beneficiario.EMP_CALLE);
                    //    var cellNumeroemp = row.CreateCell(63); cellNumeroemp.SetCellValue(beneficiario.EMP_NUMERO);
                    //    var cellPisoemp = row.CreateCell(64); cellPisoemp.SetCellValue(beneficiario.EMP_PISO);
                    //    var cellDptoemp = row.CreateCell(65); cellDptoemp.SetCellValue(beneficiario.EMP_DPTO);
                    //    var cellCpemp = row.CreateCell(66); cellCpemp.SetCellValue(beneficiario.EMP_CP);
                    //    var cellIdlocemp = row.CreateCell(67); cellIdlocemp.SetCellValue(beneficiario.ID_LOC == null ?
                    //        String.Empty : beneficiario.ID_LOC.ToString());
                    //    var cellLocemp = row.CreateCell(68); cellLocemp.SetCellValue(beneficiario.EMP_N_LOCALIDAD);
                    //    var cellDepartamentoEmp = row.CreateCell(69); cellDepartamentoEmp.SetCellValue(beneficiario.ID_LOC == null ?
                    //        String.Empty : beneficiario.EMP_N_DEPARTAMENTO);
                    //    var cellIdusuarioemp = row.CreateCell(70); cellIdusuarioemp.SetCellValue(beneficiario.EU_ID_USUARIO == null ?
                    //        String.Empty : beneficiario.EU_ID_USUARIO.ToString());
                    //    var cellNombreusuemp = row.CreateCell(71); cellNombreusuemp.SetCellValue(beneficiario.EU_NOMBRE);
                    //    var cellApellidousuemp = row.CreateCell(72); cellApellidousuemp.SetCellValue(beneficiario.EMP_APELLIDO);
                    //    var cellEmailusuemp = row.CreateCell(73); cellEmailusuemp.SetCellValue(beneficiario.EU_MAIL);
                    //    var cellTeleusuemp = row.CreateCell(74); cellTeleusuemp.SetCellValue(beneficiario.EU_TELEFONO);

                    //    //Sede
                    //    var cellIdSede = row.CreateCell(75); cellIdSede.SetCellValue(beneficiario.ID_SEDE == null ? String.Empty : beneficiario.ID_SEDE.ToString());
                    //    var cellnSede = row.CreateCell(76); cellnSede.SetCellValue(beneficiario.N_SEDE);
                    //    var cellCalleSede = row.CreateCell(77); cellCalleSede.SetCellValue(beneficiario.SEDE_CALLE);
                    //    var cellNroSede = row.CreateCell(78); cellNroSede.SetCellValue(beneficiario.SEDE_NRO);
                    //    var cellPisoSede = row.CreateCell(79); cellPisoSede.SetCellValue(beneficiario.SEDE_PISO);
                    //    var cellDptoSede = row.CreateCell(80); cellDptoSede.SetCellValue(beneficiario.SEDE_DPTO);
                    //    var cellCpSede = row.CreateCell(81); cellCpSede.SetCellValue(beneficiario.SEDE_CP);
                    //    var cellLocSede = row.CreateCell(82); cellLocSede.SetCellValue(beneficiario.SEDE_LOCALIDAD);
                    //    var cellApeContacto = row.CreateCell(83); cellApeContacto.SetCellValue(beneficiario.SEDE_APE_CONTACTO);
                    //    var cellNomContacto = row.CreateCell(84); cellNomContacto.SetCellValue(beneficiario.SEDE_NOM_CONTACTO);
                    //    var cellTelcontacto = row.CreateCell(85); cellTelcontacto.SetCellValue(beneficiario.SEDE_TELEFONO);
                    //    var cellFaxContacto = row.CreateCell(86); cellFaxContacto.SetCellValue(beneficiario.SEDE_FAX);
                    //    var cellEmailContacto = row.CreateCell(87); cellEmailContacto.SetCellValue(beneficiario.SEDE_EMAIL);

                    //    //Beneficiario
                    //    var cellIdben = row.CreateCell(88); cellIdben.SetCellValue(beneficiario.ID_BENEFICIARIO == null ? "" : beneficiario.ID_BENEFICIARIO.ToString());
                    //    var cellIdestadoben = row.CreateCell(89); cellIdestadoben.SetCellValue(beneficiario.BEN_ID_ESTADO == null ? "" : beneficiario.BEN_ID_ESTADO.ToString());
                    //    var cellEstadoben = row.CreateCell(90); cellEstadoben.SetCellValue(beneficiario.BEN_N_ESTADO);
                    //    var cellNrocuentaben = row.CreateCell(91); cellNrocuentaben.SetCellValue(beneficiario.BEN_NRO_CTA == null ? String.Empty
                    //        : beneficiario.BEN_NRO_CTA.ToString());
                    //    var cellIdsucursalben = row.CreateCell(92); cellIdsucursalben.SetCellValue(beneficiario.BEN_ID_SUCURSAL == null ? String.Empty
                    //        : beneficiario.BEN_ID_SUCURSAL.ToString());
                    //    var cellCodsucursalben = row.CreateCell(93); cellCodsucursalben.SetCellValue(beneficiario.BEN_COD_SUC);
                    //    var cellSucursalben = row.CreateCell(94); cellSucursalben.SetCellValue(beneficiario.BEN_SUCURSAL);
                    //    var cellFecsolben = row.CreateCell(95); cellFecsolben.SetCellValue(beneficiario.BEN_FEC_SOL_CTA == null ? String.Empty
                    //        : beneficiario.BEN_FEC_SOL_CTA.Value.ToShortDateString());
                    //    var cellNotificado = row.CreateCell(96); cellNotificado.SetCellValue(beneficiario.NOTIFICADO);
                    //    var cellFecNotif = row.CreateCell(97); cellFecNotif.SetCellValue(beneficiario.FEC_NOTIF == null ? String.Empty
                    //        : beneficiario.FEC_NOTIF.Value.ToShortDateString());

                    //    var cellFechaIniBenf = row.CreateCell(98); cellFechaIniBenf.SetCellValue(beneficiario.BEN_FEC_INICIO == null ? string.Empty : beneficiario.BEN_FEC_INICIO.Value.ToShortDateString());
                    //    var cellBajaBenef = row.CreateCell(99); cellBajaBenef.SetCellValue(beneficiario.BEN_FEC_BAJA == null ? string.Empty : beneficiario.BEN_FEC_BAJA.Value.ToShortDateString());

                    //    var cellTieneapoben = row.CreateCell(100); cellTieneapoben.SetCellValue(beneficiario.TIENE_APODERADO);

                    //    //Apoderado
                    //    var cellIdapoderado = row.CreateCell(101); cellIdapoderado.SetCellValue(beneficiario.ID_APODERADO == null ?
                    //        String.Empty : beneficiario.ID_APODERADO.ToString());
                    //    var cellApellidoapo = row.CreateCell(102); cellApellidoapo.SetCellValue(beneficiario.APO_APELLIDO);
                    //    var cellNombreapo = row.CreateCell(103); cellNombreapo.SetCellValue(beneficiario.APO_NOMBRE);
                    //    var cellDniapo = row.CreateCell(104); cellDniapo.SetCellValue(beneficiario.APO_DNI);
                    //    var cellCuilapo = row.CreateCell(105); cellCuilapo.SetCellValue(beneficiario.APO_CUIL);
                    //    var cellSexoapo = row.CreateCell(106); cellSexoapo.SetCellValue(beneficiario.APO_SEXO);
                    //    var cellNroctaapo = row.CreateCell(107); cellNroctaapo.SetCellValue(beneficiario.APO_NRO_CTA == null ? String.Empty
                    //        : beneficiario.APO_NRO_CTA.ToString());
                    //    var cellIdSuc = row.CreateCell(108); cellIdSuc.SetCellValue(beneficiario.APO_ID_SUC == null ? String.Empty
                    //        : beneficiario.APO_ID_SUC.ToString());
                    //    var cellCodsucapo = row.CreateCell(109); cellCodsucapo.SetCellValue(beneficiario.APO_COD_SUC);
                    //    var cellSucursalapo = row.CreateCell(110); cellSucursalapo.SetCellValue(beneficiario.APO_SUCURSAL);
                    //    var cellFecsolcuentaapo = row.CreateCell(111); cellFecsolcuentaapo.SetCellValue(beneficiario.APO_FEC_SOL_CTA == null ?
                    //        String.Empty : beneficiario.APO_FEC_SOL_CTA.Value.ToShortDateString());
                    //    var cellFecnac = row.CreateCell(112); cellFecnac.SetCellValue(beneficiario.APO_FEC_NAC == null ? String.Empty :
                    //        beneficiario.APO_FEC_NAC.Value.ToShortDateString());
                    //    var cellCalleapo = row.CreateCell(113); cellCalleapo.SetCellValue(beneficiario.APO_CALLE);
                    //    var cellNroapo = row.CreateCell(114); cellNroapo.SetCellValue(beneficiario.APO_NRO);
                    //    var cellPisoapo = row.CreateCell(115); cellPisoapo.SetCellValue(beneficiario.APO_PISO);
                    //    var cellMonoblockapo = row.CreateCell(116); cellMonoblockapo.SetCellValue(beneficiario.APO_MONOBLOCK);
                    //    var cellDptoapo = row.CreateCell(117); cellDptoapo.SetCellValue(beneficiario.APO_DPTO);
                    //    var cellParcelaapo = row.CreateCell(118); cellParcelaapo.SetCellValue(beneficiario.APO_PARCELA);
                    //    var cellManzanaapo = row.CreateCell(119); cellManzanaapo.SetCellValue(beneficiario.APO_MANZANA);
                    //    var cellEntrecalleapo = row.CreateCell(120); cellEntrecalleapo.SetCellValue(beneficiario.APO_ENTRE_CALLES);
                    //    var cellBarrioapo = row.CreateCell(121); cellBarrioapo.SetCellValue(beneficiario.APO_BARRIO);
                    //    var cellCpapo = row.CreateCell(122); cellCpapo.SetCellValue(beneficiario.APO_CP);
                    //    var cellIdlocalidadapo = row.CreateCell(123); cellIdlocalidadapo.SetCellValue(beneficiario.APO_ID_LOC == null ? String.Empty
                    //        : beneficiario.APO_ID_LOC.ToString());
                    //    var cellLocalidadapo = row.CreateCell(124); cellLocalidadapo.SetCellValue(beneficiario.APO_LOCALIDAD);
                    //    var cellIddeptoapo = row.CreateCell(125); cellIddeptoapo.SetCellValue(beneficiario.APO_ID_LOC == null ? String.Empty
                    //        : beneficiario.APO_ID_DEPTO.ToString());
                    //    var cellDeptoapo = row.CreateCell(126); cellDeptoapo.SetCellValue(beneficiario.APO_DEPARTAMENTO);
                    //    var cellTelefonoapo = row.CreateCell(127); cellTelefonoapo.SetCellValue(beneficiario.APO_TELEFONO);
                    //    var cellCelularapo = row.CreateCell(128); cellCelularapo.SetCellValue(beneficiario.APO_CELULAR);
                    //    var cellEmailapo = row.CreateCell(129);
                    //    cellEmailapo.SetCellValue(beneficiario.APO_EMAIL);

                    //    var cellApoIdEstado = row.CreateCell(130);
                    //    cellApoIdEstado.SetCellValue(beneficiario.AP_ID_ESTADO == null ? String.Empty
                    //        : beneficiario.AP_ID_ESTADO.ToString());
                    //    var cellApoEstado = row.CreateCell(131);
                    //    cellApoEstado.SetCellValue(beneficiario.AP_N_ESTADO);

                    //    //ESCUELAS
                    //    var cellIdEscuela = row.CreateCell(132);
                    //    cellIdEscuela.SetCellValue(beneficiario.ESC_ID_ESCUELA == null ? "" : beneficiario.ESC_ID_ESCUELA.ToString());
                    //    var cellNEscuela = row.CreateCell(133);
                    //    cellNEscuela.SetCellValue(beneficiario.N_ESCUELA);
                    //    var cellEscCalle = row.CreateCell(134);
                    //    cellEscCalle.SetCellValue(beneficiario.ESC_CALLE);
                    //    var cellEscNumero = row.CreateCell(135);
                    //    cellEscNumero.SetCellValue(beneficiario.ESC_NUMERO);
                    //    var cellEscBarrio = row.CreateCell(136);
                    //    cellEscBarrio.SetCellValue(beneficiario.ESC_BARRIO);
                    //    var cellEscIdLocalidad = row.CreateCell(137);
                    //    cellEscIdLocalidad.SetCellValue(beneficiario.ESC_ID_LOCALIDAD == null ? "" : beneficiario.ESC_ID_LOCALIDAD.ToString());
                    //    var cellEscLocalidad = row.CreateCell(138);
                    //    cellEscLocalidad.SetCellValue(beneficiario.ESC_N_LOCALIDAD);

                    //}
                    //#endregion
                    
                    break;

            }

            var ms = new MemoryStream();

            templateWorkbook.Write(ms);

            return ms.ToArray();
        }

        public byte[] ExportBeneficiarioHorarios(IBeneficiariosVista vista)
        {
            string path = "";

            path = HttpContext.Current.Server.MapPath(@"\Archivos\Exportados\TemplateBeneficiarioHorarios.xls");

            var fs = new FileStream(path, FileMode.Open, FileAccess.Read);

            var templateWorkbook = new HSSFWorkbook(fs, true);

            var sheet = templateWorkbook.GetSheet("Nomina");

            IList<IBeneficiario> listaparaExcel = _beneficiariorepositorio.GetBeneficiariosHorarios((int)Enums.Programas.Ppp, Convert.ToInt32(vista.ComboTiposPpp.Selected), Convert.ToInt32(vista.EstadosBeneficiarios.Selected), vista.FechaInicioBeneficio, vista.FechaBenefHasta);

            int i = 2;
            foreach (var beneficiario in listaparaExcel)
            {
                //int tieneHorasCargadas =_horarioficharepositorio.GetHorarioFichaByEmpresa(beneficiario.Ficha.IdFicha,beneficiario.Ficha.FichaPpp.Empresa.IdEmpresa).Count;
                //if (tieneHorasCargadas != 0)
                //{
                i++;

                //Ficha
                var row = sheet.CreateRow(i);
                var cellidficha = row.CreateCell(0);
                cellidficha.SetCellValue(beneficiario.Ficha.IdFicha);
                var cellApellido = row.CreateCell(1);
                cellApellido.SetCellValue(beneficiario.Ficha.Apellido);
                var cellNombre = row.CreateCell(2);
                cellNombre.SetCellValue(beneficiario.Ficha.Nombre);
                var cellCuil = row.CreateCell(3);
                cellCuil.SetCellValue(beneficiario.Ficha.Cuil);
                var cellDni = row.CreateCell(4);
                cellDni.SetCellValue(beneficiario.Ficha.NumeroDocumento);
                var cellFechaNa = row.CreateCell(5);
                cellFechaNa.SetCellValue(beneficiario.Ficha.FechaNacimiento.ToString());
                var cellCalle = row.CreateCell(6);
                cellCalle.SetCellValue(beneficiario.Ficha.Calle);
                var cellNro = row.CreateCell(7);
                cellNro.SetCellValue(beneficiario.Ficha.Numero);
                var cellPiso = row.CreateCell(8);
                cellPiso.SetCellValue(beneficiario.Ficha.Piso);
                var cellDpto = row.CreateCell(9);
                cellDpto.SetCellValue(beneficiario.Ficha.Dpto);
                var cellBarrio = row.CreateCell(10);
                cellBarrio.SetCellValue(beneficiario.Ficha.Barrio);
                var cellCp = row.CreateCell(11);
                cellCp.SetCellValue(beneficiario.Ficha.CodigoPostal);
                var cellidLocalidad = row.CreateCell(12);
                cellidLocalidad.SetCellValue(beneficiario.Ficha.LocalidadFicha.IdLocalidad);
                var cellnLocalidad = row.CreateCell(13);
                cellnLocalidad.SetCellValue(beneficiario.Ficha.LocalidadFicha.NombreLocalidad);
                var cellIdDepartamento = row.CreateCell(14);
                cellIdDepartamento.SetCellValue(beneficiario.Ficha.LocalidadFicha.Departamento.IdDepartamento);
                var cellnDepartamento = row.CreateCell(15);
                cellnDepartamento.SetCellValue(beneficiario.Ficha.LocalidadFicha.Departamento.NombreDepartamento);
                var celldiscapacidad = row.CreateCell(16);
                celldiscapacidad.SetCellValue(beneficiario.Ficha.EsDiscapacitado ? "S" : "N");
                // 28/02/2017 nuevos campos
                var cellidTipoppp = row.CreateCell(17);
                cellidTipoppp.SetCellValue(beneficiario.Ficha.FichaPpp.TIPO_PPP.ToString());
                string tipoPpp = "";
                switch (beneficiario.Ficha.FichaPpp.TIPO_PPP)
                {
                    case 1:
                        tipoPpp = "PROGRAMA PRIMER PASO";
                        break;
                    case 2:
                        tipoPpp = "PROGRAMA PRIMER PASO APRENDIZ";
                        break;
                    case 3:
                        tipoPpp = "PROGRAMA XMÍ";
                        break;
                }
                var cellNTipoPPP = row.CreateCell(18); cellNTipoPPP.SetCellValue(tipoPpp);

                var cellidSubprograma= row.CreateCell(19);
                cellidSubprograma.SetCellValue(beneficiario.Ficha.FichaPpp.IdSubprograma == null ? "" : Convert.ToString(beneficiario.Ficha.FichaPpp.IdSubprograma));
                var cellsubprograma = row.CreateCell(20);
                cellsubprograma.SetCellValue(beneficiario.Ficha.FichaPpp.Subprograma);

                var cellEstadoBenef = row.CreateCell(21);
                cellEstadoBenef.SetCellValue(beneficiario.NombreEstado);
                var cellFecIni = row.CreateCell(22);
                cellFecIni.SetCellValue(beneficiario.FechaInicio == null ? String.Empty
                            : beneficiario.FechaInicio.Value.ToShortDateString());
                //Empresa
                var cellrazonSocial = row.CreateCell(23);
                cellrazonSocial.SetCellValue(beneficiario.Ficha.FichaPpp.Empresa.NombreEmpresa);
                var cellCuit = row.CreateCell(24);
                cellCuit.SetCellValue(beneficiario.Ficha.FichaPpp.Empresa.Cuit);
                var cellCalleemp = row.CreateCell(25);
                cellCalleemp.SetCellValue(beneficiario.Ficha.FichaPpp.Empresa.Calle);
                var cellNumeroemp = row.CreateCell(26);
                cellNumeroemp.SetCellValue(beneficiario.Ficha.FichaPpp.Empresa.Numero);
                var cellPisoemp = row.CreateCell(27);
                cellPisoemp.SetCellValue(beneficiario.Ficha.FichaPpp.Empresa.Piso);
                var cellDptoemp = row.CreateCell(28);
                cellDptoemp.SetCellValue(beneficiario.Ficha.FichaPpp.Empresa.Dpto);
                var cellCpemp = row.CreateCell(29);
                cellCpemp.SetCellValue(beneficiario.Ficha.FichaPpp.Empresa.CodigoPostal);
                var cellIdlocemp = row.CreateCell(30);
                cellIdlocemp.SetCellValue(beneficiario.Ficha.FichaPpp.Empresa.IdLocalidad == null
                                              ? String.Empty
                                              : beneficiario.Ficha.FichaPpp.Empresa.LocalidadEmpresa.IdLocalidad.
                                                    ToString());
                var cellLocemp = row.CreateCell(31);
                cellLocemp.SetCellValue(beneficiario.Ficha.FichaPpp.Empresa.LocalidadEmpresa.NombreLocalidad);
                var cellDepartamentoEmp = row.CreateCell(32);
                cellDepartamentoEmp.SetCellValue(beneficiario.Ficha.FichaPpp.Empresa.IdLocalidad == null
                                                     ? String.Empty
                                                     : beneficiario.Ficha.FichaPpp.Empresa.LocalidadEmpresa.
                                                           Departamento.NombreDepartamento);

                //Sede
                var cellCallesede = row.CreateCell(33);
                cellCallesede.SetCellValue(beneficiario.Ficha.FichaPpp.Sede.Calle);
                var cellNumerosede = row.CreateCell(34);
                cellNumerosede.SetCellValue(beneficiario.Ficha.FichaPpp.Sede.Numero);
                var cellPisosede = row.CreateCell(35);
                cellPisosede.SetCellValue(beneficiario.Ficha.FichaPpp.Sede.Piso);
                var cellDptosede = row.CreateCell(36);
                cellDptosede.SetCellValue(beneficiario.Ficha.FichaPpp.Sede.Dpto);
                var cellCpsede = row.CreateCell(37);
                cellCpsede.SetCellValue(beneficiario.Ficha.FichaPpp.Sede.CodigoPostal);
                var cellIdlocsede = row.CreateCell(38);
                cellIdlocsede.SetCellValue(beneficiario.Ficha.FichaPpp.Sede.IdLocalidad == null
                                               ? String.Empty
                                               : beneficiario.Ficha.FichaPpp.Sede.IdLocalidad.ToString());
                var cellLocsede = row.CreateCell(39);
                cellLocsede.SetCellValue(beneficiario.Ficha.FichaPpp.Sede.NombreLocalidad);


                IList<IHorarioFicha> listahorarioficha =
                    _horarioficharepositorio.GetHorarioFichaByEmpresa(beneficiario.Ficha.IdFicha,
                                                                      beneficiario.Ficha.FichaPpp.Empresa.IdEmpresa);

                foreach (var horarioFicha in listahorarioficha)
                {
                    IList<IHorarioEmpresa> listahorarioEmpresa =
                        _horarioempresarepositorio.GetHorarioEmpresaForFicha(horarioFicha.IdEmpresaHorario);

                    foreach (var horarioempresa in listahorarioEmpresa)
                    {
                        string horario = horarioempresa.HoraDesde.ToString().PadLeft(2, '0') + ":" +
                                         horarioempresa.MinutosDesde.ToString().PadLeft(2, '0') + " - " +
                                         horarioempresa.HoraHasta.ToString().PadLeft(2, '0') + ":" +
                                         horarioempresa.MinutosHasta.ToString().PadLeft(2, '0');

                        switch (horarioFicha.DSemana)
                        {
                            case "01":
                                if (horarioFicha.Corte == 0)
                                {
                                    var cellLunes0 = row.CreateCell(40);
                                    cellLunes0.SetCellValue(horario);
                                }
                                if (horarioFicha.Corte == 1)
                                {
                                    var cellLunes1 = row.CreateCell(47);
                                    cellLunes1.SetCellValue(horario);
                                }
                                break;
                            case "02":
                                if (horarioFicha.Corte == 0)
                                {
                                    var cellMartes0 = row.CreateCell(41);
                                    cellMartes0.SetCellValue(horario);
                                }
                                if (horarioFicha.Corte == 1)
                                {
                                    var cellMartes0 = row.CreateCell(48);
                                    cellMartes0.SetCellValue(horario);
                                }
                                break;
                            case "03":
                                if (horarioFicha.Corte == 0)
                                {
                                    var cellMier0 = row.CreateCell(42);
                                    cellMier0.SetCellValue(horario);
                                }
                                if (horarioFicha.Corte == 1)
                                {
                                    var cellMier1 = row.CreateCell(49);
                                    cellMier1.SetCellValue(horario);
                                }
                                break;
                            case "04":
                                if (horarioFicha.Corte == 0)
                                {
                                    var cellJuev0 = row.CreateCell(43);
                                    cellJuev0.SetCellValue(horario);
                                }
                                if (horarioFicha.Corte == 1)
                                {
                                    var cellJuev1 = row.CreateCell(50);
                                    cellJuev1.SetCellValue(horario);
                                }
                                break;
                            case "05":
                                if (horarioFicha.Corte == 0)
                                {
                                    var cellVier0 = row.CreateCell(44);
                                    cellVier0.SetCellValue(horario);
                                }
                                if (horarioFicha.Corte == 1)
                                {
                                    var cellVier1 = row.CreateCell(51);
                                    cellVier1.SetCellValue(horario);
                                }
                                break;
                            case "06":
                                if (horarioFicha.Corte == 0)
                                {
                                    var cellSab0 = row.CreateCell(45);
                                    cellSab0.SetCellValue(horario);
                                }
                                if (horarioFicha.Corte == 1)
                                {
                                    var cellSab1 = row.CreateCell(52);
                                    cellSab1.SetCellValue(horario);
                                }
                                break;
                            case "07":
                                if (horarioFicha.Corte == 0)
                                {
                                    var cellDom0 = row.CreateCell(46);
                                    cellDom0.SetCellValue(horario);
                                }
                                if (horarioFicha.Corte == 1)
                                {
                                    var cellDom1 = row.CreateCell(53);
                                    cellDom1.SetCellValue(horario);
                                }
                                break;
                        }
                    }
                }




            }

            var ms = new MemoryStream();

            templateWorkbook.Write(ms);

            return ms.ToArray();

        }


        public IBeneficiariosVista GetBeneficiarioHorarios()
        {
            IBeneficiariosVista vista = new BeneficiariosVista();

            //var lista = _programarepositorio.GetProgramas();

            vista.ListaBeneficiarios = new List<IBeneficiario>();

            CargarEstadosBeneficiarioforIndex(vista, _estadobeneficiariorepositorio.GetEstadosBeneficiario());
            
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

    }
}
