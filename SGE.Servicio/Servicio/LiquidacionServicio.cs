using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Linq;
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
using SGE.Servicio.VistaInterfaces.Shared;
using IError = SGE.Servicio.Comun.IError;
using Color = iTextSharp.text.BaseColor;
using FontPdf = iTextSharp.text.Font;
using Rectangle = iTextSharp.text.Rectangle;

namespace SGE.Servicio.Servicio
{
    public class LiquidacionServicio : BaseServicio, ILiquidacionServicio
    {
        private readonly ILiquidacionRepositorio _liquidacionRepositorio;
        private readonly IBeneficiarioRepositorio _beneficiarioRepositorio;
        private readonly IConceptoRepositorio _conceptoRepositorio;
        private readonly IProgramaRepositorio _programaRepositorio;
        private readonly IApoderadoRepositorio _apoderadoRepositorio;
        private readonly IEstadoLiquidacionRepositorio _estadoLiquidacionRepositorio;
        private readonly ISucursalRepositorio _sucursalRepositorio;
        private readonly IAutenticacionServicio _aut;
        private static ILiquidacionRepositorio _liqrepo;
        private static IConceptoRepositorio _conceprepo;
        private readonly IRolFormularioAccionRepositorio _rolformularioaccionrepositorio;

        private readonly ITipoPppRepositorio _tipopppRepositorio;

        public LiquidacionServicio()
        {
            _liqrepo = new LiquidacionRepositorio();
            _liquidacionRepositorio = new LiquidacionRepositorio();
            _beneficiarioRepositorio = new BeneficiarioRepositorio();
            _conceptoRepositorio = new ConceptoRepositorio();
            _aut = new AutenticacionServicio();
            _programaRepositorio = new ProgramaRepositorio();
            _apoderadoRepositorio = new ApoderadoRepositorio();
            _estadoLiquidacionRepositorio = new EstadoLiquidacionRepositorio();
            _sucursalRepositorio = new SucursalRepositorio();
            _conceprepo = new ConceptoRepositorio();
            _rolformularioaccionrepositorio = new RolFormularioAccionRepositorio();
            _tipopppRepositorio = new TipoPppRepositorio();
        }

        public ILiquidacionesVista GetLiquidacionesIndex()
        {
            ILiquidacionesVista vista = new LiquidacionesVista();

            var pager = new Pager(0,
                Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")), "FormIndexLiquidacion",
                _aut.GetUrl("IndexPager", "Liquidacion"));

            vista.Pager = pager;

            vista.Liquidaciones = new List<ILiquidacion>();
            vista.From = DateTime.Now.AddDays(-60);
            vista.To = DateTime.Now.AddDays(30);

            CargarEstadoLiquidacionIndex(vista, _estadoLiquidacionRepositorio.GetEstadosLiquidacion());

            CargarPrograma(vista, _programaRepositorio.GetProgramas());

            return vista;
        }

        public ILiquidacionesVista GetLiquidaciones()
        {

            ILiquidacionesVista vista = new LiquidacionesVista();

            var pager = new Pager(_liquidacionRepositorio.GetLiquidaciones().Count,
                Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")), "FormIndexLiquidacion",
                _aut.GetUrl("IndexPager", "Liquidacion"));

            vista.Pager = pager;

            vista.Liquidaciones = _liquidacionRepositorio.GetLiquidaciones(pager.Skip, pager.PageSize);

            CargarEstadoLiquidacionIndex(vista, _estadoLiquidacionRepositorio.GetEstadosLiquidacion());

            CargarPrograma(vista, _programaRepositorio.GetProgramas());

            return vista;
        }

        public ILiquidacionesVista GetLiquidaciones(IPager pager)
        {
            ILiquidacionesVista vista = new LiquidacionesVista
                                            {
                                                Pager = pager,
                                                Liquidaciones = _liquidacionRepositorio.GetLiquidaciones(pager.Skip,
                                                pager.PageSize)
                                            };

            return vista;
        }

        public ILiquidacionesVista GetLiquidaciones(DateTime from, DateTime to, int nroResolucion, int estado, int programa)
        {
            ILiquidacionesVista vista = new LiquidacionesVista();

            bool accesoprograma = base.AccesoPrograma(Enums.Formulario.Liquidaciones);

            int countliq = accesoprograma == true
                               ? _liquidacionRepositorio.GetLiquidaciones(from, to, nroResolucion, estado, programa).
                                     Count()
                               : _liquidacionRepositorio.GetLiquidaciones(from, to, nroResolucion, estado, programa).
                                     Where(c => c.IdPrograma != (int)Enums.Programas.EfectoresSociales).Count();

            var pager = new Pager(countliq,
                Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")), "FormIndexLiquidacion",
                _aut.GetUrl("IndexPager", "Liquidacion"));

            vista.Pager = pager;


            if (accesoprograma)
                vista.Liquidaciones = _liquidacionRepositorio.GetLiquidaciones(from, to, nroResolucion, estado, programa, pager.Skip,
                    pager.PageSize);
            else
                vista.Liquidaciones = _liquidacionRepositorio.GetLiquidaciones(from, to, nroResolucion, estado, programa,
                                                                               pager.Skip,
                                                                               pager.PageSize,
                                                                               (int) Enums.Programas.EfectoresSociales);

            vista.From = from;
            vista.To = to;

            CargarEstadoLiquidacionIndex(vista, _estadoLiquidacionRepositorio.GetEstadosLiquidacion());

            CargarPrograma(vista, _programaRepositorio.GetProgramas());

            return vista;
        }

        public ILiquidacionesVista GetLiquidaciones(IPager pagerQ, DateTime from, DateTime to, int nroResolucion, int estado, int programa)
        {
            ILiquidacionesVista vista = new LiquidacionesVista();


            bool accesoprograma = base.AccesoPrograma(Enums.Formulario.Liquidaciones);

            IList<ILiquidacion> lista;

            //int countliq = accesoprograma == true
            //                   ? _liquidacionRepositorio.GetLiquidaciones(from, to, nroResolucion, estado, programa).
            //                         Count
            //                   : _liquidacionRepositorio.GetLiquidaciones(from, to, nroResolucion, estado, programa).
            //                         Where(c => c.IdPrograma != (int)Enums.Programas.EfectoresSociales).Count();

            int countliq=0; 
                lista= accesoprograma == true
                               ? _liquidacionRepositorio.GetLiquidaciones(from, to, nroResolucion, estado, programa).ToList()
                               : _liquidacionRepositorio.GetLiquidaciones(from, to, nroResolucion, estado, programa).
                                     Where(c => c.IdPrograma != (int)Enums.Programas.EfectoresSociales).ToList();

                countliq = lista.Count();

            var pager = new Pager(countliq,
                Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")), "FormIndexLiquidacion",
                _aut.GetUrl("IndexPager", "Liquidacion"));


            if (pager.TotalCount != pagerQ.TotalCount)
            {
                pager.PageNumber = 1;
                pager.Skip = 0;
                vista.Pager = pager;
            }
            else
            {
                vista.Pager = pagerQ;
            }

            vista.NroResolución = nroResolucion;
            vista.From = from;
            vista.To = to;

            CargarEstadoLiquidacionIndex(vista, _estadoLiquidacionRepositorio.GetEstadosLiquidacion());

            vista.Estados.Selected = estado.ToString();

            CargarPrograma(vista, _programaRepositorio.GetProgramas());

            vista.Programas.Selected = programa.ToString();

            vista.Liquidaciones = lista.Skip(pagerQ.Skip).Take(pagerQ.PageSize).ToList();
            //if (accesoprograma)
            //    vista.Liquidaciones = _liquidacionRepositorio.GetLiquidaciones(from, to, nroResolucion, estado, programa, pagerQ.Skip,
            //        pagerQ.PageSize);
            //else
            //    vista.Liquidaciones = _liquidacionRepositorio.GetLiquidaciones(from, to, nroResolucion, estado, programa,
            //                                                                   pagerQ.Skip,
            //                                                                   pagerQ.PageSize,
            //                                                                   (int)Enums.Programas.EfectoresSociales);

            return vista;
        }

        public ILiquidacionVista GetLiquidacion(int idLiquidacion)
        {
            ILiquidacionVista vista = new LiquidacionVista();

            ILiquidacion localliquidacion = _liquidacionRepositorio.GetLiquidacion(idLiquidacion);

            foreach (var item in localliquidacion.Beneficiarios)
            {
                IBeneficiarioLiquidacion localbeneficiarioliquidacion = new BeneficiarioLiquidacion
                                                                            {
                                                                                IdBeneficiario = item.IdBeneficiario,
                                                                                Ficha = item.Ficha,
                                                                                ListaConceptos =
                                                                                (IList<IConcepto>)item.ConceptosBeneficiario,
                                                                                Conceptos =
                                                                                (IList<IConcepto>)item.ConceptosBeneficiario,
                                                                                IdEstadoBenConcLiq = item.IdEstadoBenConcLiq,
                                                                                Apoderado = new Apoderado { Apellido = item.Apoderado.Apellido ?? "", Nombre = item.Apoderado.Nombre ?? "", NumeroDocumento = item.Apoderado.NumeroDocumento ?? "0" }
                                                                            };

                vista.ListaBeneficiario.Add(localbeneficiarioliquidacion);
            }

            vista.FechaLiquidacion = localliquidacion.FechaLiquidacion;
            vista.Observacion = localliquidacion.Observacion;
            vista.NroResolucion = localliquidacion.NroResolucion;
            vista.IdLiquidacion = localliquidacion.IdLiquidacion;

            int año = localliquidacion.Beneficiarios.Take(1).Select(
                li => new Concepto { Año = li.ConceptosBeneficiario.Select(co => co.Año).SingleOrDefault() }).
                SingleOrDefault().Año;

            int mes = localliquidacion.Beneficiarios.Take(1).Select(
                li => new Concepto { Mes = li.ConceptosBeneficiario.Select(co => co.Mes).SingleOrDefault() }).
                SingleOrDefault().Mes;

            int localIdConcepto = localliquidacion.Beneficiarios.Take(1).Select(
                li => new Concepto { Id_Concepto = li.ConceptosBeneficiario.Select(co => co.Id_Concepto).SingleOrDefault() })
                .SingleOrDefault().Id_Concepto;

            IList<IConcepto> localListaConceptos =
                _conceptoRepositorio.GetConceptos(null, null, "").
                Where(c => c.Año <= año && (c.Mes <= mes)).OrderByDescending(c => c.Año).ThenByDescending(c => c.Mes).ToList();

            CargarConcepto(vista, localListaConceptos);

            IList<IPrograma> localListaProgramas = _programaRepositorio.GetProgramas();

            CargarPrograma(vista, localListaProgramas);

            vista.Conceptos.Enabled = false;
            vista.Programas.Enabled = false;
            vista.Conceptos.Selected = localIdConcepto.ToString();
            vista.SeleccionoConcepto = vista.Conceptos.Selected;

            return vista;
        }

        public ILiquidacionVista GetLiquidacion(int idLiquidacion, string accion)
        {
            ILiquidacionVista vista = new LiquidacionVista();

            ILiquidacion localliquidacion = _liquidacionRepositorio.GetLiquidacion(idLiquidacion);

            int idPrograma = 0;

            if (localliquidacion == null)
            {
                return vista;
            }

            if (localliquidacion.Beneficiarios.FirstOrDefault().IdPrograma == (int)Enums.Programas.EfectoresSociales)
            {
                foreach (var item in localliquidacion.Beneficiarios)
                {
                    IBeneficiarioLiquidacion obj = new BeneficiarioLiquidacion
                    {
                        IdBeneficiario = item.IdBeneficiario,
                        Ficha = item.Ficha,
                        Programa = item.Programa,
                        IdPrograma = item.IdPrograma,
                        IdEstadoBenConcLiq = item.IdEstadoBenConcLiq,
                        NroCuentadePago = item.NroCuentadePago,
                        Sucursal = item.Sucursal,
                        PagoaApoderado = item.PagoaApoderado,
                        EstadoBeneficiarioConcepto = item.EstadoBeneficiarioConcepto,
                        IdSucursal = item.IdSucursal,
                        Apoderado =
                            new Apoderado
                            {
                                Apellido = item.Apoderado.Apellido ?? "",
                                Nombre = item.Apoderado.Nombre ?? "",
                                NumeroDocumento =
                                    item.Apoderado.NumeroDocumento ?? "0"
                            }

                    };
                    idPrograma = item.IdPrograma;
                    obj.ListaConceptos = (IList<IConcepto>)item.ConceptosBeneficiario;
                    obj.Conceptos = (IList<IConcepto>)item.ConceptosBeneficiario;
                    obj.MontoPrograma = _conceptoRepositorio.GetConceptosByBeneficiarioEfectores(item.IdBeneficiario, idLiquidacion).MontoPagado;
                    //obj.MontoPrograma = _beneficiarioRepositorio.GetBeneficiario(item.IdBeneficiario).MontoPrograma;
                    obj.MontoLiquidado = item.MontoLiquidado;
                    vista.ListaBeneficiario.Add(obj);
                }
            }
            else
            {
                foreach (var item in localliquidacion.Beneficiarios)
                {
                    IBeneficiarioLiquidacion obj = new BeneficiarioLiquidacion
                                                       {
                                                           IdBeneficiario = item.IdBeneficiario,
                                                           Ficha = item.Ficha,
                                                           Programa = item.Programa,
                                                           IdPrograma = item.IdPrograma,
                                                           IdEstadoBenConcLiq = item.IdEstadoBenConcLiq,
                                                           NroCuentadePago = item.NroCuentadePago,
                                                           Sucursal = item.Sucursal,
                                                           PagoaApoderado = item.PagoaApoderado,
                                                           EstadoBeneficiarioConcepto = item.EstadoBeneficiarioConcepto,
                                                           IdSucursal = item.IdSucursal,
                                                           TipoPrograma = item.TipoPrograma,
                                                           idSubprograma = item.idSubprograma,
                                                           subprograma = item.subprograma,
                                                           IdEmpresa = item.IdEmpresa,
                                                           Apoderado =
                                                               new Apoderado
                                                                   {
                                                                       Apellido = item.Apoderado.Apellido ?? "",
                                                                       Nombre = item.Apoderado.Nombre ?? "",
                                                                       NumeroDocumento =
                                                                           item.Apoderado.NumeroDocumento ?? "0"
                                                                   }

                                                       };
                    idPrograma = item.IdPrograma;
                    obj.ListaConceptos = (IList<IConcepto>)item.ConceptosBeneficiario;
                    obj.Conceptos = (IList<IConcepto>)item.ConceptosBeneficiario;
                    obj.MontoPrograma = item.MontoPrograma;
                    obj.MontoLiquidado = item.MontoLiquidado;
                    vista.ListaBeneficiario.Add(obj);
                }
            }

            //foreach (var item in localliquidacion.Beneficiarios)
            //{
            //   IBeneficiarioLiquidacion obj = new BeneficiarioLiquidacion
            //                                       {
            //                                           IdBeneficiario = item.IdBeneficiario,
            //                                           Ficha = item.Ficha,
            //                                           Programa = item.Programa,
            //                                           IdPrograma = item.IdPrograma,
            //                                           IdEstadoBenConcLiq = item.IdEstadoBenConcLiq,
            //                                           NroCuentadePago = item.NroCuentadePago,
            //                                           Sucursal = item.Sucursal,
            //                                           PagoaApoderado = item.PagoaApoderado,
            //                                           EstadoBeneficiarioConcepto = item.EstadoBeneficiarioConcepto,
            //                                           IdSucursal = item.IdSucursal,
            //                                           Apoderado = new Apoderado { Apellido = item.Apoderado.Apellido ?? "", Nombre = item.Apoderado.Nombre ?? "", NumeroDocumento = item.Apoderado.NumeroDocumento ?? "0" }

            //                                       };
            //    idPrograma = item.IdPrograma;
            //    obj.ListaConceptos = (IList<IConcepto>)item.ConceptosBeneficiario;
            //    obj.Conceptos = (IList<IConcepto>)item.ConceptosBeneficiario;
            //    obj.MontoPrograma = item.MontoPrograma;
            //    vista.ListaBeneficiario.Add(obj);
            //}

            vista.FechaLiquidacion = localliquidacion.FechaLiquidacion;
            vista.Observacion = localliquidacion.Observacion;
            vista.NroResolucion = localliquidacion.NroResolucion;
            vista.Accion = accion;
            vista.IdLiquidacion = localliquidacion.IdLiquidacion;

            int año = localliquidacion.Beneficiarios.Take(1).Select(
                li => new Concepto { Año = li.ConceptosBeneficiario.Select(co => co.Año).SingleOrDefault() }).
                SingleOrDefault().Año;

            int mes = localliquidacion.Beneficiarios.Take(1).Select(
                li => new Concepto { Mes = li.ConceptosBeneficiario.Select(co => co.Mes).SingleOrDefault() }).
                SingleOrDefault().Mes;

            int idconcepto = localliquidacion.Beneficiarios.Take(1).Select(
                li =>
                new Concepto { Id_Concepto = li.ConceptosBeneficiario.Select(co => co.Id_Concepto).SingleOrDefault() })
                .
                SingleOrDefault().Id_Concepto;

            IList<IConcepto> listaconceptos =
                _conceptoRepositorio.GetConceptos(null, null, "").Where(
                    c => c.Año <= año && (c.Mes <= mes)).OrderByDescending(
                        c => c.Año).ThenByDescending(c => c.Mes).
                    ToList();

            CargarConcepto(vista, listaconceptos);

            IList<IPrograma> listaprogramas = _programaRepositorio.GetProgramas();

            CargarPrograma(vista, listaprogramas);

            IList<IEstadoLiquidacion> listaestadoliquidacion =
                _estadoLiquidacionRepositorio.GetEstadosLiquidacion().Where(
                    c =>
                    c.IdEstadoLiquidacion != localliquidacion.IdEstadoLiquidacion &&
                    c.IdEstadoLiquidacion != (int)Enums.EstadoLiquidacion.Liquidado &&
                    (localliquidacion.Generado
                         ? true
                         : c.IdEstadoLiquidacion != (int)Enums.EstadoLiquidacion.Liquidado)).ToList();

            CargarEstadoLiquidacion(vista, listaestadoliquidacion);

            vista.Conceptos.Enabled = false;
            vista.Programas.Enabled = false;
            vista.Conceptos.Selected = idconcepto.ToString();
            vista.SeleccionoConcepto = vista.Conceptos.Selected;
            vista.Programas.Selected = idPrograma.ToString();

            return vista;
        }

        public ILiquidacionVista GetLiquidacion()
        {
            ILiquidacionVista vista = new LiquidacionVista();

            IList<IConcepto> listaconceptos =
                _conceptoRepositorio.GetConceptos(null, null, "").Where(
                    c => (c.Año <= DateTime.Now.Year /*&& (c.Mes <= DateTime.Now.Month)*/) || c.Año == (DateTime.Now.Year - 2)/*c.Año <= DateTime.Now.Year && (c.Mes <= DateTime.Now.Month)*/).OrderByDescending(
                        c => c.Año).ThenByDescending(c => c.Mes).
                    ToList();// 14/02/2013 - Se añade tambien para Liquidaciones - Di Campli Leandro

            IList<IPrograma> listaprogramas = _programaRepositorio.GetProgramas();


            CargarConcepto(vista, listaconceptos);

            if (vista.Conceptos.Combo.Count > 1)
            {
                string seleccionado = vista.Conceptos.Combo[1].Id.ToString();
                vista.Conceptos.Selected = seleccionado;
            }


            CargarPrograma(vista, listaprogramas);

            IList<IConvenio> listaconvenios = _programaRepositorio.GetConvenios();
            CargarConvenio(vista, listaconvenios);
            return vista;
        }

        public ILiquidacionVista GetLiquidacionRepagos()
        {
            ILiquidacionVista vista = new LiquidacionVista();

            IList<IConcepto> listaconceptos =
                _conceptoRepositorio.GetConceptos(null, null, "").Where(
                    c => (c.Año <= DateTime.Now.Year /*&& (c.Mes <= DateTime.Now.Month)*/)  || c.Año == (DateTime.Now.Year - 2) ).OrderByDescending(
                        c => c.Año).ThenByDescending(c => c.Mes).
                    ToList(); //08/02/2013 - Se añade || c.Año == (DateTime.Now.Year - 1) para poder levantar los meses del año anterior para Repagos - Di Campli Leandro

            IList<IPrograma> listaprogramas = _programaRepositorio.GetProgramas();

            CargarConcepto(vista, listaconceptos);


            vista.Conceptos.Combo.OrderBy(c => c.Id).ToList();

            if (vista.Conceptos.Combo.Count > 1)
            {
                string seleccionado = vista.Conceptos.Combo[1].Id.ToString();
                vista.Conceptos.Selected = seleccionado;
            }

            vista.Accion = "Repago";

            CargarPrograma(vista, listaprogramas);
            IList<IConvenio> listaconvenios = _programaRepositorio.GetConvenios();
            CargarConvenio(vista, listaconvenios);


            return vista;
        }

        private static void CargarConcepto(ILiquidacionVista vista, IEnumerable<IConcepto> listaConceptos)
        {
            foreach (var listaconceptos in listaConceptos)
            {
                vista.Conceptos.Combo.Add(new ComboItem
                                              {
                                                  Id = listaconceptos.Id_Concepto,
                                                  Description =
                                                      listaconceptos.Año + " - " +
                                                      CultureInfo.CurrentCulture.DateTimeFormat.MonthNames[
                                                          listaconceptos.Mes - 1].ToUpper()
                                              });

                vista.SeleccionadoBeneficiario.ComboConceptos.Combo.Add(new ComboItem
                                                                            {
                                                                                Id = listaconceptos.Id_Concepto,
                                                                                Description =
                                                                                    listaconceptos.Año + " - " +
                                                                                    CultureInfo.CurrentCulture.DateTimeFormat.MonthNames[
                                                                                        listaconceptos.Mes - 1].ToUpper()
                                                                            });
            }
        }

        private void CargarPrograma(ILiquidacionVista vista, IEnumerable<IPrograma> listaProgramas)
        {
            var accesoprograma =
              _rolformularioaccionrepositorio.GetFormulariosDelUsuario(base.GetUsuarioLoguer().Id_Usuario).Where(
                  c =>
                  c.Id_Accion == (int)Enums.Acciones.VerEfectoresSociales &&
                  c.Id_Formulario == (int)Enums.Formulario.Liquidaciones).Count() > 0
                  ? true
                  : false;



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
                    if (accesoprograma)
                        vista.Programas.Combo.Add(new ComboItem
                        {
                            Id = item.IdPrograma,
                            Description = item.NombrePrograma

                        });
                }
            }
        }

        private void CargarPrograma(ILiquidacionesVista vista, IEnumerable<IPrograma> listaProgramas)
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
                    if (base.AccesoPrograma(Enums.Formulario.Liquidaciones))
                        vista.Programas.Combo.Add(new ComboItem
                        {
                            Id = item.IdPrograma,
                            Description = item.NombrePrograma

                        });
                }
            }
        }

        private static void CargarEstadoLiquidacion(ILiquidacionVista vista, IEnumerable<IEstadoLiquidacion> listaEstados)
        {
            foreach (var item in listaEstados)
            {
                vista.EstadosLiquidacion.Combo.Add(new ComboItem
                                                       {
                                                           Id = item.IdEstadoLiquidacion,
                                                           Description = item.NombreEstado

                                                       });
            }
        }

        private static void CargarEstadoLiquidacionIndex(ILiquidacionesVista vista, IEnumerable<IEstadoLiquidacion> listaEstados)
        {
            vista.Estados.Combo.Add(new ComboItem { Id = 0, Description = "Todos" });

            foreach (var item in listaEstados)
            {
                vista.Estados.Combo.Add(new ComboItem
                {
                    Id = item.IdEstadoLiquidacion,
                    Description = item.NombreEstado

                });
            }
        }

        public string GenerarFile(IList<IBeneficiarioLiquidacion> listaLiquidacionBeneficiario, DateTime fechaLiquidacion)
        {

            if (listaLiquidacionBeneficiario.Count <= 0)
            {
                return "";
            }

            int idPrograma = listaLiquidacionBeneficiario.Select(c => c.IdPrograma).Take(1).SingleOrDefault();

            int[] idBeneficiarios =
                listaLiquidacionBeneficiario.Where(
                    c => c.IdEstadoBenConcLiq == (short)Enums.EstadoBeneficiarioLiquidacion.EnviadoAlBanco).Select(
                        c => c.IdBeneficiario).ToArray();
            // 14/06/2013 - DI CAMPLI LEANDRO - SE AÑADE FILTRO DE ETAPAS - SE AGREGA 0 POR AHORA
            IList<IBeneficiario> lista =
                _beneficiarioRepositorio.GetBeneficiarioConCuenta("", "", "", "", idPrograma, "T", "T", "T", "T",
                                                                  (int)Enums.EstadoBeneficiario.Activo, "", 0, "").Where(
                                                                      c =>
                                                                      c.IdPrograma == idPrograma &&
                                                                      idBeneficiarios.Contains(c.IdBeneficiario)).ToList();


            if (listaLiquidacionBeneficiario.FirstOrDefault().IdPrograma == (int)Enums.Programas.EfectoresSociales)
            {
                foreach (var item in lista)
                {
                    IBeneficiario beneficiario = item;
                    IBeneficiario item1 = item;
                    IBeneficiarioLiquidacion liquidacion = listaLiquidacionBeneficiario.Where(c => c.IdBeneficiario == item1.IdBeneficiario).FirstOrDefault();
                    listaLiquidacionBeneficiario.Where(c => c.IdBeneficiario == beneficiario.IdBeneficiario).Select(c =>
                    {
                        c.NumeroConvenio = beneficiario.NumeroConvenio;
                        c.NumeroCuenta = beneficiario.NumeroCuenta;
                        c.CodigoMoneda = beneficiario.CodigoMoneda;
                        c.CodigoSucursal = beneficiario.CodigoSucursal;
                        c.TieneApoderado = beneficiario.TieneApoderado;
                        c.ImportePagoPlan = liquidacion.MontoPrograma;
                        c.MontoPrograma = liquidacion.MontoPrograma;
                        c.ListaConceptos = c.ListaConceptos;
                        c.Conceptos = c.Conceptos;
                        return c;
                    }).ToList();
                }
            }
            else
            {
                foreach (var item in lista)
                {
                    IBeneficiario beneficiario = item;
                    listaLiquidacionBeneficiario.Where(c => c.IdBeneficiario == beneficiario.IdBeneficiario).Select(c =>
                                                                                                                  {
                                                                                                                      c.NumeroConvenio =  beneficiario.NumeroConvenio;
                                                                                                                      c.NumeroCuenta = beneficiario.NumeroCuenta;
                                                                                                                      c.CodigoMoneda = beneficiario.CodigoMoneda;
                                                                                                                      c.CodigoSucursal = beneficiario.CodigoSucursal;
                                                                                                                      c.TieneApoderado = beneficiario.TieneApoderado;
                                                                                                                      c.ImportePagoPlan = beneficiario.ImportePagoPlan;
                                                                                                                      c.MontoPrograma = beneficiario.MontoPrograma;
                                                                                                                      c.ListaConceptos = c.ListaConceptos;
                                                                                                                      c.Conceptos = c.Conceptos;
                                                                                                                      return c;
                                                                                                                  }).ToList();
                }
            }

            string fileid = DateTime.Now.Date.Day + DateTime.Now.Date.Month.ToString() +
                            DateTime.Now.Date.Year + DateTime.Now.Hour +
                            DateTime.Now.Minute + DateTime.Now.Second;

            string file =
                HttpContext.Current.Server.MapPath("/Archivos/PSO" + ((listaLiquidacionBeneficiario[0].convenio == "" || listaLiquidacionBeneficiario[0].convenio ==null) ?
                listaLiquidacionBeneficiario[0].NumeroConvenio.PadLeft(5, '0') : listaLiquidacionBeneficiario[0].convenio.PadLeft(5, '0')) +
                                                   fileid +
                                                   ".hab");

            var fiinfo = new FileInfo(file);

            using (var sw = fiinfo.CreateText())
            {
                int mcuota = 0;

                foreach (var liquidacion in listaLiquidacionBeneficiario.Where(c => c.IdEstadoBenConcLiq == (short)Enums.EstadoBeneficiarioLiquidacion.EnviadoAlBanco).ToList())
                {
                    IApoderado apoderado = null;
                    if (liquidacion.TieneApoderado == "S")
                    {
                        apoderado = _apoderadoRepositorio.GetApoderadoByBeneficiarioActivo(liquidacion.IdBeneficiario);
                    }

                    mcuota++;
                    string tipoconvenio = "013".PadLeft(3, '0');
                    string sucursal = (apoderado == null
                                           ? liquidacion.CodigoSucursal.PadLeft(5, '0')
                                           : apoderado.CodigoSucursalBanco.PadLeft(5, '0'));

                    string moneda = (apoderado == null
                                         ? liquidacion.CodigoMoneda.PadLeft(2, '0')
                                         : apoderado.CodigoMoneda.PadLeft(2, '0'));



                    string sistema = "3".PadLeft(1, '0');

                    string nrocuenta = (apoderado == null
                                            ? liquidacion.NumeroCuenta.ToString().PadLeft(9, '0')
                                            : apoderado.NumeroCuentaBco.ToString().PadLeft(9, '0'));


                    string[] mimporte =
                        (liquidacion.ImportePagoPlan * liquidacion.Conceptos.Count()).ToString().Split(',');

                    string importeconcepto;

                    if (mimporte.Length > 1)
                    {
                        importeconcepto = mimporte[0] + mimporte[1];
                    }
                    else
                    {
                        importeconcepto = mimporte[0] + "00";
                    }

                    string importe = importeconcepto.Replace(",", string.Empty).Trim().PadLeft(18, '0');

                    string fecha =
                        fechaLiquidacion.ToString("yyyyMMdd").Replace("/", string.Empty).Trim().PadLeft(
                            8, '0');

                    string nroconvenio = //liquidacion.NumeroConvenio.PadLeft(5, '0');
                    
                     ((liquidacion.convenio == "" || liquidacion.convenio ==null) ?
                liquidacion.NumeroConvenio.PadLeft(5, '0') : liquidacion.convenio.PadLeft(5, '0'));
                     
                    string nrocomprobante = mcuota.ToString().PadLeft(6, '0');
                    string cbu = "0".PadLeft(22, '0');
                    string cuota = "".PadLeft(2, '0');
                    string usuario = (apoderado == null
                                          ? liquidacion.Ficha.NumeroDocumento.PadLeft(22, '0')
                                          : apoderado.NumeroDocumento.PadLeft(22, '0'));

                    string text = tipoconvenio + sucursal + moneda + sistema + nrocuenta + importe + fecha +
                                  nroconvenio + nrocomprobante + cbu + cuota + usuario;


                    sw.WriteLine(text);
                }
            }

            string nombreArchivo = ((listaLiquidacionBeneficiario[0].convenio == "" || listaLiquidacionBeneficiario[0].convenio == null) ?
                listaLiquidacionBeneficiario[0].NumeroConvenio.PadLeft(5, '0') : listaLiquidacionBeneficiario[0].convenio.PadLeft(5, '0')) + fileid + ".hab";

            string file2 =
                HttpContext.Current.Server.MapPath("/Archivos/PSO" + nombreArchivo);


            return file2;


        }

        public ILiquidacionesVista GetLiquidacionesPendientes(string apellido, string nombre, string cbu, string numeroDocumento)
        {
            ILiquidacionesVista vista = new LiquidacionesVista();

            var pager =
                new Pager(
                    _liquidacionRepositorio.GetLiquidacionesPendientes(apellido, nombre, cbu, numeroDocumento).Count,
                    Convert.ToInt32(ConfigurationManager.AppSettings.Get("PageCount")),
                    "FormIndexLiquidacion", _aut.GetUrl("IndexPager", "Liquidacion"));

            vista.Pager = pager;

            vista.Liquidaciones = _liquidacionRepositorio.GetLiquidacionesPendientes(apellido, nombre, cbu,
                                                                                     numeroDocumento, pager.Skip,
                                                                                     pager.PageSize);


            return vista;
        }

        public int AgregarLiquidacion(ILiquidacionVista liquidacion)
        {
            ILiquidacion liquidacionadd = new Liquidacion();

            IList<IBeneficiario> lista = liquidacion.ListaBeneficiario.Select(c => new Beneficiario
                                                                                       {
                                                                                           IdBeneficiario =
                                                                                               c.IdBeneficiario,
                                                                                           ListaConceptos =
                                                                                               c.Conceptos,
                                                                                           MontoPrograma =
                                                                                               c.MontoPrograma,
                                                                                           IdEstadoBenConcLiq =
                                                                                               c.IdEstadoBenConcLiq,
                                                                                           IdSucursal = (c.TieneApoderado == "S" ? (c.Apoderado == null ? null : c.Apoderado.IdSucursal) : c.IdSucursal),
                                                                                           NroCuentadePago =
                                                                                               (c.TieneApoderado == "S"
                                                                                                    ? (c.Apoderado ==
                                                                                                       null
                                                                                                           ? null
                                                                                                           : c.Apoderado
                                                                                                                 .
                                                                                                                 NumeroCuentaBco)
                                                                                                    : c.NumeroCuenta) ??
                                                                                               0,
                                                                                           PagoaApoderado =
                                                                                               c.TieneApoderado,
                                                                                           IdApoderadoPago = (c.TieneApoderado == "S" ? (c.Apoderado == null ? null : c.Apoderado.IdApoderadoNullable) : null),
                                                                                           IdEmpresa = c.IdEmpresa,
                                                                                       }).Cast
                <IBeneficiario>().ToList();


            liquidacionadd.ListaBeneficiarios = lista;
            liquidacionadd.IdEstadoLiquidacion = (int)Enums.EstadoLiquidacion.Pendiente;
            liquidacionadd.FechaLiquidacion = liquidacion.FechaLiquidacion;
            liquidacionadd.NroResolucion = liquidacion.NroResolucion ?? 0;

            return _liquidacionRepositorio.AddLiquidacion(liquidacionadd);
        }

        public bool UpdateLiquidacion(ILiquidacionVista liquidacion)
        {
            ILiquidacion liquidacionadd = new Liquidacion();
            // 14/06/2013 - DI CAMPLI LEANDRO - SE AÑADE FILTRO DE ETAPAS - SE AGREGA 0 POR AHORA
            IList<IBeneficiario> listabeneficiarioconcuenta = _beneficiarioRepositorio.GetBeneficiarioConCuenta(null, null, null, null, liquidacion.ListaBeneficiario.Take(1).Select(c => c.IdPrograma).FirstOrDefault(), null, null, null, null, (int)Enums.EstadoBeneficiario.Activo, null, 0, "");

            IList<IBeneficiario> listabeneficiariosincuenta = _beneficiarioRepositorio.
                GetBeneficiarioSinCuenta("", "", "", "",
                                         liquidacion.ListaBeneficiario.Take(1).Select(c => c.IdPrograma).FirstOrDefault(),
                                         "T",
                                         "T", "T", "T", (int)Enums.EstadoBeneficiario.Activo, "",0,"");




            foreach (var item in liquidacion.ListaBeneficiario)
            {
                IBeneficiario beneficiario =
                    listabeneficiarioconcuenta.Union(listabeneficiariosincuenta).Where(c => c.IdBeneficiario == item.IdBeneficiario).FirstOrDefault();
                
                if (beneficiario != null) // 08/02/2013 - verifica que traiga un beneficiario - Di Campli Leandro
                {
                    IBeneficiario benefi = new Beneficiario
                                               {
                                                   IdBeneficiario = item.IdBeneficiario,
                                                   ListaConceptos = item.Conceptos,
                                                   MontoPrograma = item.MontoPrograma,
                                                   IdEstadoBenConcLiq = item.IdEstadoBenConcLiq,
                                                   IdSucursal = (beneficiario.TieneApoderado == "S" ? (beneficiario.Apoderado == null ? 0 : beneficiario.Apoderado.IdSucursal) : beneficiario.IdSucursal),
                                                   NroCuentadePago =
                                                       (beneficiario.TieneApoderado == "S"
                                                            ? (beneficiario.Apoderado == null ? 0 : beneficiario.Apoderado.NumeroCuentaBco)
                                                            : beneficiario.NumeroCuenta) ?? 0,
                                                   PagoaApoderado = beneficiario.TieneApoderado,
                                                   IdApoderadoPago = (beneficiario.TieneApoderado == "S" ? (beneficiario.Apoderado == null ? null : beneficiario.Apoderado.IdApoderadoNullable) : null)
                                               };

                    liquidacionadd.ListaBeneficiarios.Add(benefi);
                }
            }
            liquidacionadd.IdEstadoLiquidacion = (int)Enums.EstadoLiquidacion.Pendiente;
            liquidacionadd.FechaLiquidacion = liquidacion.FechaLiquidacion;
            liquidacionadd.NroResolucion = liquidacion.NroResolucion ?? 0;
            liquidacionadd.IdLiquidacion = liquidacion.IdLiquidacion;
            liquidacionadd.Observacion = liquidacion.Observacion;

            return _liquidacionRepositorio.UpdateLiquidacion(liquidacionadd);
        }

        public bool ChangeEstadoLiquidacion(ILiquidacionVista liquidacion)
        {
            ILiquidacion liquidacionadd = new Liquidacion
                                              {
                                                  IdEstadoLiquidacion =
                                                      Convert.ToInt32(liquidacion.EstadosLiquidacion.Selected),
                                                  IdLiquidacion = liquidacion.IdLiquidacion
                                              };

            return _liquidacionRepositorio.ChangeEstado(liquidacionadd);
        }

        public ILiquidacionVista BuscarBeneficiarioEnNomina(ILiquidacionVista vista)
        {

            return vista;
        }

        public ILiquidacionVista QuitarBeneficiarioDeNomina(ILiquidacionVista vista, int idBeneficiario)
        {
            vista.ListaBeneficiario.Where(c => c.IdBeneficiario == idBeneficiario).Select(d =>
                                                                                              {
                                                                                                  d.Excluido = true;
                                                                                                  d.IdEstadoBenConcLiq =
                                                                                                      (short)
                                                                                                      Enums.
                                                                                                          EstadoBeneficiarioLiquidacion
                                                                                                          .Excluido;
                                                                                                  return d;
                                                                                              }).ToList();

            vista.ListaBeneficiario =
                vista.ListaBeneficiario.OrderBy(
                    c => c.IdEstadoBenConcLiq == (short)Enums.EstadoBeneficiarioLiquidacion.ExcluidoNoApto).ThenBy(
                        c => c.IdEstadoBenConcLiq == (short)Enums.EstadoBeneficiarioLiquidacion.Excluido).ThenBy(
                            c => c.Ficha.Apellido).ToList();


            return vista;
        }

        public ILiquidacionVista AgregarBeneficiarioDeNomina(ILiquidacionVista vista, int idBeneficiario)
        {
            vista.ListaBeneficiario.Where(c => c.IdBeneficiario == idBeneficiario).Select(d =>
            {
                d.Excluido = false;
                d.IdEstadoBenConcLiq =
                    (short)
                    Enums.
                        EstadoBeneficiarioLiquidacion
                        .EnviadoAlBanco;
                return d;
            }).ToList();

            vista.ListaBeneficiario =
                vista.ListaBeneficiario.OrderBy(
                    c => c.IdEstadoBenConcLiq == (short)Enums.EstadoBeneficiarioLiquidacion.ExcluidoNoApto).ThenBy(
                        c => c.IdEstadoBenConcLiq == (short)Enums.EstadoBeneficiarioLiquidacion.Excluido).ThenBy(
                            c => c.Ficha.Apellido).ToList();


            return vista;
        }


        //este se usa para la liq
        public ILiquidacionVista CargarConceptoBeneficiarios(ILiquidacionVista vista)
        {
            IConcepto objconcepto = _conceptoRepositorio.GetConcepto(Convert.ToInt32(vista.Conceptos.Selected));

            DateTime fechainiciopago =
                Convert.ToDateTime(("15" + "/" + objconcepto.Mes.ToString() + "/" + objconcepto.Año.ToString()));

            // 18/07/2014
            string convenio = "";
            if (Convert.ToInt32((vista.Convenios.Selected ?? "0")) > 0)
            {
                convenio = _programaRepositorio.GetConvenio(Convert.ToInt32(vista.Convenios.Selected)).NConvenio;
            }


            vista.ListaBeneficiario =
                (from c in
                     _beneficiarioRepositorio.GetBeneficiarioForLiquidacion(
                         Convert.ToInt32(vista.Programas.Selected), Convert.ToInt32(vista.Conceptos.Selected), fechainiciopago)
                 select
                     new BeneficiarioLiquidacion
                         {
                             IdBeneficiario = c.IdBeneficiario,
                             IdPrograma = c.IdPrograma,
                             Programa = c.Programa,
                             MontoPrograma = c.MontoPrograma,
                             Ficha = c.Ficha,
                             IdEstado = c.IdEstado,
                             FechaSistema = c.FechaSistema,
                             IdFicha = c.IdFicha,
                             IdUsuarioSistema = c.IdUsuarioSistema,
                             IdConvenio = c.IdConvenio,
                             NumeroConvenio = c.NumeroConvenio,
                             CodigoActivacionBancaria = c.CodigoActivacionBancaria,
                             CodigoNaturalezaJuridica = c.CodigoNaturalezaJuridica,
                             CondicionIva = c.CondicionIva,
                             Email = c.Email,
                             Nacionalidad = c.Nacionalidad,
                             Residente = c.Residente,
                             TipoPersona = c.TipoPersona,
                             CodigoMoneda = c.CodigoMoneda,
                             CodigoSucursal = c.CodigoSucursal,
                             NombreEstado = c.NombreEstado,
                             Cbu = c.Cbu,
                             ImportePagoPlan = c.ImportePagoPlan,
                             NumeroCuenta = c.NumeroCuenta,
                             TieneApoderado = c.TieneApoderado,
                             Excluido = false,
                             Apto = true,
                             IdSucursal = c.IdSucursal,
                             convenio = c.convenio, // 23/07/2014
                             IdEstadoBenConcLiq = (short)Enums.EstadoBeneficiarioLiquidacion.EnviadoAlBanco,
                             TipoPrograma = c.TipoPrograma,
                             idSubprograma = c.idSubprograma,
                             IdEmpresa = c.IdEmpresa,
                             Apoderado =
                                 new Apoderado
                                     {
                                         NumeroCuentaBco = c.Apoderado.NumeroCuentaBco,
                                         IdSucursal = c.Apoderado.IdSucursal,
                                         Apellido = c.Apoderado.Apellido ?? "",
                                         Nombre = c.Apoderado.Nombre ?? "",
                                         NumeroDocumento =
                                             (c.Apoderado.NumeroDocumento == " " ? "0" : c.Apoderado.NumeroDocumento) ??
                                             "0",
                                         IdApoderadoNullable = c.Apoderado.IdApoderadoNullable
                                     }

                         }).Union(from c in
                                      _beneficiarioRepositorio.GetBeneficiarioForLiquidacionExcluidos(
                                          Convert.ToInt32(vista.Programas.Selected),
                                          Convert.ToInt32(vista.Conceptos.Selected), fechainiciopago)
                                  select
                                      new BeneficiarioLiquidacion
                                          {
                                              IdBeneficiario = c.IdBeneficiario,
                                              IdPrograma = c.IdPrograma,
                                              Programa = c.Programa,
                                              MontoPrograma = c.MontoPrograma,
                                              Ficha = c.Ficha,
                                              IdEstado = c.IdEstado,
                                              FechaSistema = c.FechaSistema,
                                              IdFicha = c.IdFicha,
                                              IdUsuarioSistema = c.IdUsuarioSistema,
                                              IdConvenio = c.IdConvenio,
                                              NumeroConvenio = c.NumeroConvenio,
                                              CodigoActivacionBancaria =
                                                  c.CodigoActivacionBancaria,
                                              CodigoNaturalezaJuridica =
                                                  c.CodigoNaturalezaJuridica,
                                              CondicionIva = c.CondicionIva,
                                              Email = c.Email,
                                              Nacionalidad = c.Nacionalidad,
                                              Residente = c.Residente,
                                              TipoPersona = c.TipoPersona,
                                              CodigoMoneda = c.CodigoMoneda,
                                              CodigoSucursal = c.CodigoSucursal,
                                              NombreEstado = c.NombreEstado,
                                              Cbu = c.Cbu,
                                              ImportePagoPlan = c.ImportePagoPlan,
                                              NumeroCuenta = c.NumeroCuenta,
                                              TieneApoderado = c.TieneApoderado,
                                              Excluido = false,
                                              Apto = false,
                                              IdSucursal = c.IdSucursal,
                                              convenio = c.convenio, // 22/07/2014
                                              IdEstadoBenConcLiq =
                                                  (short)
                                                  Enums.EstadoBeneficiarioLiquidacion.ExcluidoNoApto,
                                              TipoPrograma = c.TipoPrograma,
                                              idSubprograma = c.idSubprograma,
                                              IdEmpresa = c.IdEmpresa,
                                              Apoderado =
                                                  new Apoderado
                                                      {
                                                          NumeroCuentaBco = c.Apoderado.NumeroCuentaBco,
                                                          IdSucursal = c.Apoderado.IdSucursal,
                                                          Apellido = c.Apoderado.Apellido ?? "",
                                                          Nombre = c.Apoderado.Nombre ?? "",
                                                          NumeroDocumento =
                                                              (c.Apoderado.NumeroDocumento == " "
                                                                   ? "0"
                                                                   : c.Apoderado.NumeroDocumento) ?? "0",
                                                          IdApoderadoNullable = c.Apoderado.IdApoderadoNullable
                                                      }


                                          }).Union(from c in
                                                       _beneficiarioRepositorio.
                                                       GetBeneficiarioSinCuenta("", "", "", "",
                                                                                Convert.ToInt32(
                                                                                    vista.Programas.
                                                                                        Selected), "T",
                                                                                "T", "T", "T",
                                                                                (int)Enums.EstadoBeneficiario.Activo,
                                                                                "",0,"")
                                                   select
                                                       new BeneficiarioLiquidacion
                                                           {
                                                               IdBeneficiario = c.IdBeneficiario,
                                                               IdPrograma = c.IdPrograma,
                                                               Programa = c.Programa,
                                                               MontoPrograma = c.MontoPrograma,
                                                               Ficha = c.Ficha,
                                                               IdEstado = c.IdEstado,
                                                               FechaSistema = c.FechaSistema,
                                                               IdFicha = c.IdFicha,
                                                               IdUsuarioSistema = c.IdUsuarioSistema,
                                                               IdConvenio = c.IdConvenio,
                                                               NumeroConvenio = c.NumeroConvenio,
                                                               CodigoActivacionBancaria =
                                                                   c.CodigoActivacionBancaria,
                                                               CodigoNaturalezaJuridica =
                                                                   c.CodigoNaturalezaJuridica,
                                                               CondicionIva = c.CondicionIva,
                                                               Email = c.Email,
                                                               Nacionalidad = c.Nacionalidad,
                                                               Residente = c.Residente,
                                                               TipoPersona = c.TipoPersona,
                                                               CodigoMoneda = c.CodigoMoneda,
                                                               CodigoSucursal = c.CodigoSucursal,
                                                               NombreEstado = c.NombreEstado,
                                                               Cbu = c.Cbu,
                                                               ImportePagoPlan = c.ImportePagoPlan,
                                                               NumeroCuenta = c.NumeroCuenta,
                                                               TieneApoderado = c.TieneApoderado,
                                                               Excluido = false,
                                                               Apto = false,
                                                               IdSucursal = c.IdSucursal,
                                                               convenio = c.convenio, // 23/07/2014
                                                               IdEstadoBenConcLiq =
                                                                   (short)
                                                                   Enums.EstadoBeneficiarioLiquidacion.ExcluidoNoApto,
                                                               TipoPrograma = c.TipoPrograma,
                                                               idSubprograma = c.idSubprograma,
                                                               IdEmpresa = c.IdEmpresa,
                                                               Apoderado =
                                                                   new Apoderado
                                                                       {
                                                                           NumeroCuentaBco = c.Apoderado.NumeroCuentaBco,
                                                                           IdSucursal = c.Apoderado.IdSucursal,
                                                                           Apellido = c.Apoderado.Apellido ?? "",
                                                                           Nombre = c.Apoderado.Nombre ?? "",
                                                                           NumeroDocumento =
                                                                               (c.Apoderado.NumeroDocumento == " "
                                                                                    ? "0"
                                                                                    : c.Apoderado.NumeroDocumento) ??
                                                                               "0",
                                                                           IdApoderadoNullable =
                                                                               c.Apoderado.IdApoderadoNullable
                                                                       }


                                                           }).Cast<IBeneficiarioLiquidacion>().OrderBy(
                                                               c =>
                                                               c.IdEstadoBenConcLiq ==
                                                               (short)
                                                               Enums.EstadoBeneficiarioLiquidacion.ExcluidoNoApto).
                    ThenBy(c =>
                           c.
                               IdEstadoBenConcLiq ==
                           (short)
                           Enums.
                               EstadoBeneficiarioLiquidacion
                               .
                               Excluido)
                    .ThenBy(
                        c => c.Ficha.Apellido).ThenBy(c => c.Ficha.Nombre).
                    ToList();



            vista.ListaBeneficiario.Select(c =>
            {
                c.Conceptos.Add(objconcepto);
                return c;
            }).ToList();

            if ((convenio ?? "0") != "0" && (convenio ?? "") != "")
            {
                vista.ListaBeneficiario = vista.ListaBeneficiario.Where(c => c.convenio == convenio).ToList();
            }
            vista.Conceptos.Enabled = false;
            vista.Programas.Enabled = false;
            vista.Convenios.Enabled = false;
            vista.Conceptos.Selected = objconcepto.Id_Concepto.ToString();
            vista.SeleccionoConcepto = vista.Conceptos.Selected;

            return vista;
        }

        //Para reporte anexoI trae benef en condiciones de cobrar y los retenidos
        public byte[] ReporteAnexo1Resolucion(ILiquidacionVista vista)
        {
            //tipo de hoja, margenes
            var document = new Document(PageSize.LEGAL.Rotate(), 40, 40, 25, 140);//60
            var memStream = new MemoryStream();

            PdfWriter writer = PdfWriter.GetInstance(document, memStream);

            writer.CloseStream = false;

            document.Open();

            //CREO EL ENCABEZADO
            var head = new PdfPTable(1) { TotalWidth = 60 };

            //ubicacion de la cabecera
            head.SetWidthPercentage(new float[] { 250 }, PageSize.LEGAL);


            ILiquidacion localliquidacion = _liquidacionRepositorio.GetLiquidacion(vista.IdLiquidacion);

            int año = (localliquidacion.Beneficiarios).Take(1).Select(
               li => new Concepto { Año = li.ConceptosBeneficiario.Select(co => co.Año).SingleOrDefault() }).
               SingleOrDefault().Año;

            int mes = (localliquidacion.Beneficiarios).Take(1).Select(
                li => new Concepto { Mes = li.ConceptosBeneficiario.Select(co => co.Mes).SingleOrDefault() }).
                SingleOrDefault().Mes;
            string periodo = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames[
                mes - 1].ToUpper() + " " + año;

            //string periodo = string.Format("{0:MMMM}", vista.FechaLiquidacion).ToUpper() + " " + vista.FechaLiquidacion.Year;

            var tablatitulo = new PdfPTable(3);
            //le doy el ancho a cada columna
            tablatitulo.SetWidthPercentage(new float[] { 200, 260, 100 }, PageSize.LEGAL);

            string titulo = "";
            ////obtengo los datos a listar
            ILiquidacionVista liquidacion;

            liquidacion = CargarBeneficiariosRepAnexoI(vista);

            int prog = liquidacion.ListaBeneficiariosAnexo.FirstOrDefault().IdPrograma;

            vista.Liquidacion.IdPrograma = prog;

            if (prog == (int)Enums.Programas.Ppp)
                titulo = "PROGRAMA PRIMER PASO";

            if (prog == (int)Enums.Programas.ReconversionProductiva)
                titulo = "PROGRAMA RECONVERSION PRODUCTIVA";

            if (prog == (int)Enums.Programas.Universitaria)
                titulo = "PROGRAMA BECA UNIVERSITARIA";

            if (prog == (int)Enums.Programas.Terciaria)
                titulo = "PROGRAMA BECA TERCIARIA";

            if (prog == (int)Enums.Programas.EfectoresSociales)
                titulo = "PROGRAMA EFECTORES SOCIALES";

            if (prog == (int)Enums.Programas.PppProf)
                titulo = "PROGRAMA PRIMER PASO PROFESIONAL";

            if (prog == (int)Enums.Programas.Vat)
                titulo = "PROGRAMA VOLVER AL TRABAJO";

            //creo una celda para cada columna
            var celda1 =
                new PdfPCell(new Paragraph("ANEXO I",
                                           FontFactory.GetFont("Arial, Helvetica, sans-serif", 14)));
            var celda2 =
                new PdfPCell(new Paragraph(titulo,
                                           FontFactory.GetFont("Arial, Helvetica, sans-serif", 14)));
            var celda3 =
                new PdfPCell(new Paragraph(periodo,
                                           FontFactory.GetFont("Arial, Helvetica, sans-serif", 14)));

            //agrego a la tabla las columnas (celdas)
            tablatitulo.AddCell(celda1);
            tablatitulo.AddCell(celda2);
            tablatitulo.AddCell(celda3);

            foreach (PdfPCell celda in tablatitulo.Rows[0].GetCells())
            {
                celda.Border = 0;
                celda.HorizontalAlignment = PdfContentByte.ALIGN_LEFT;
            }

            //agrego la cabecera al documento
            document.Add(tablatitulo);
            //creo un "sector"¿?del pdf, este es para la cabecera
            document.Add(new Paragraph(" ", FontFactory.GetFont("Arial, Helvetica, sans-serif", 2)));

            var ls = new LineSeparator();
            document.Add(new Chunk(ls)); //linea para separar

            document.Add(new Paragraph(" ", FontFactory.GetFont("Arial, Helvetica, sans-serif", 2))); //salto de linea

            //creo una tabla y le agrego los datos traidos anteriormente
            PdfPTable unaTabla = GenerarTablaReporteAnexo1(liquidacion);

            ////agrego la tabla al documento
            document.Add(unaTabla);

            document.Add(new Paragraph(" ", FontFactory.GetFont("Arial, Helvetica, sans-serif", 6)));

            PdfPTable tablaTotales = GenerarTablaTotalesReporteAnexo1(liquidacion, prog);

            document.Add(tablaTotales);

            //cierro el documento
            document.Close();

            //ESTO ES PARA TODOS LOS REPORTES.
            var buf = new byte[memStream.Position];
            memStream.Position = 0;
            memStream.Read(buf, 0, buf.Length);

            //MANEJO DE PAGINADO
            return AddPageNumbersApaizadosWithImage(buf);
        }

        public ILiquidacionVista CargarBeneficiariosRepAnexoI(ILiquidacionVista vista)
        {
            List<IBeneficiario> listabenef;
            listabenef = _beneficiarioRepositorio.GetBeneficiariosAnexoActivosRetenidos(vista.Liquidacion.IdPrograma).ToList();

            vista.ListaBeneficiariosAnexo = listabenef;

            return vista;
        }

        public ILiquidacionVista CargarConceptoBeneficiariosRepago(ILiquidacionVista vista)
        {
            IConcepto objconcepto = _conceptoRepositorio.GetConcepto(Convert.ToInt32(vista.Conceptos.Selected));

            // 04/08/2014
            int idconvenio = Convert.ToInt32((vista.Convenios.Selected ?? "0"));
             
            IList<IConvenio> listaconvenios = _programaRepositorio.GetConvenios();
            CargarConvenio(vista, listaconvenios);

            string convenio = "";
            if (idconvenio > 0)
            {
                vista.Convenios.Selected = Convert.ToString( idconvenio);
                convenio = _programaRepositorio.GetConvenio(Convert.ToInt32(vista.Convenios.Selected)).NConvenio;
            }


            IList<IConcepto> listaconceptos =
              _conceptoRepositorio.GetConceptos(null, null, "").Where(
                  c => c.Año <= DateTime.Now.Year && (c.Mes <= DateTime.Now.Month)).OrderByDescending(
                      c => c.Año).ThenByDescending(c => c.Mes).
                  ToList();

            IList<IPrograma> listaprogramas = _programaRepositorio.GetProgramas();

            CargarConcepto(vista, listaconceptos);

            CargarPrograma(vista, listaprogramas);

            //********************15/02/2013 - DI CAMPLI LEANDRO -******************************

            List<int> idBeneficiariosExport = new List<int>();
            
            string path = HttpContext.Current.Server.MapPath(@"\Archivos\Importados") + @"\" + vista.Archivo;

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
                            idBeneficiariosExport.Add(Convert.ToInt32(row["Id_Beneficiario"].ToString()));

                    }
                }
            }
            //********************15/02/2013 - DI CAMPLI LEANDRO -******************************


            //SE MODIFICA PARA QUE SOLO TENGA EN CUENTA LOS BENEFICIARIOS IMPORTADOS POR EL EXCEL
            // 14/06/2013 - DI CAMPLI LEANDRO - SE AÑADE FILTRO DE ETAPAS - SE AGREGA 0 POR AHORA
            vista.ListaBeneficiario =
                (from c in
                     _beneficiarioRepositorio.GetBeneficiarioConCuenta(null, null, null, null, Convert.ToInt32(vista.Programas.Selected), null, null, null, null, (int)Enums.EstadoBeneficiario.Activo, null, 0, "")
                 where idBeneficiariosExport.Contains(c.IdBeneficiario) //15/02/2013 - DI CAMPLI LEANDRO
                 
                 select
                     new BeneficiarioLiquidacion
                     {
                         IdBeneficiario = c.IdBeneficiario,
                         IdPrograma = c.IdPrograma,
                         Programa = c.Programa,
                         MontoPrograma = c.MontoPrograma,
                         Ficha = c.Ficha,
                         IdEstado = c.IdEstado,
                         FechaSistema = c.FechaSistema,
                         IdFicha = c.IdFicha,
                         IdUsuarioSistema = c.IdUsuarioSistema,
                         IdConvenio = c.IdConvenio,
                         NumeroConvenio = c.NumeroConvenio,
                         CodigoActivacionBancaria = c.CodigoActivacionBancaria,
                         CodigoNaturalezaJuridica = c.CodigoNaturalezaJuridica,
                         CondicionIva = c.CondicionIva,
                         Email = c.Email,
                         Nacionalidad = c.Nacionalidad,
                         Residente = c.Residente,
                         TipoPersona = c.TipoPersona,
                         CodigoMoneda = c.CodigoMoneda,
                         CodigoSucursal = c.CodigoSucursal,
                         NombreEstado = c.NombreEstado,
                         Cbu = c.Cbu,
                         ImportePagoPlan = c.ImportePagoPlan,
                         NumeroCuenta = c.NumeroCuenta,
                         TieneApoderado = c.TieneApoderado,
                         Excluido = false,
                         Apto = true,
                         IdSucursal = c.IdSucursal,
                         convenio = c.convenio, // 01/08/2014
                         IdEstadoBenConcLiq = (short)Enums.EstadoBeneficiarioLiquidacion.Excluido,
                         IdEmpresa = c.IdEmpresa, // 04/09/2018
                         Apoderado = new Apoderado { NumeroCuentaBco = c.Apoderado.NumeroCuentaBco, IdSucursal = c.Apoderado.IdSucursal, Apellido = c.Apoderado.Apellido ?? "", Nombre = c.Apoderado.Nombre ?? "", NumeroDocumento = (c.Apoderado.NumeroDocumento == " " ? "0" : c.Apoderado.NumeroDocumento) ?? "0", IdApoderadoNullable = c.Apoderado.IdApoderadoNullable }
                         
                     }).Union(from c in
                                  _beneficiarioRepositorio.
                                  GetBeneficiarioSinCuenta("", "", "", "",
                                                           Convert.ToInt32(
                                                               vista.Programas.
                                                                   Selected), "T",
                                                           "T", "T", "T", (int)Enums.EstadoBeneficiario.Activo, "",0, "")

                              where idBeneficiariosExport.Contains(c.IdBeneficiario) //15/02/2013 - DI CAMPLI LEANDRO
                              select
                                  new BeneficiarioLiquidacion
                                  {
                                      IdBeneficiario = c.IdBeneficiario,
                                      IdPrograma = c.IdPrograma,
                                      Programa = c.Programa,
                                      MontoPrograma = c.MontoPrograma,
                                      Ficha = c.Ficha,
                                      IdEstado = c.IdEstado,
                                      FechaSistema = c.FechaSistema,
                                      IdFicha = c.IdFicha,
                                      IdUsuarioSistema = c.IdUsuarioSistema,
                                      IdConvenio = c.IdConvenio,
                                      NumeroConvenio = c.NumeroConvenio,
                                      CodigoActivacionBancaria =
                                          c.CodigoActivacionBancaria,
                                      CodigoNaturalezaJuridica =
                                          c.CodigoNaturalezaJuridica,
                                      CondicionIva = c.CondicionIva,
                                      Email = c.Email,
                                      Nacionalidad = c.Nacionalidad,
                                      Residente = c.Residente,
                                      TipoPersona = c.TipoPersona,
                                      CodigoMoneda = c.CodigoMoneda,
                                      CodigoSucursal = c.CodigoSucursal,
                                      NombreEstado = c.NombreEstado,
                                      Cbu = c.Cbu,
                                      ImportePagoPlan = c.ImportePagoPlan,
                                      NumeroCuenta = c.NumeroCuenta,
                                      TieneApoderado = c.TieneApoderado,
                                      Excluido = false,
                                      Apto = false,
                                      IdSucursal = c.IdSucursal,
                                      convenio = c.convenio, // 01/08/2014
                                      IdEstadoBenConcLiq =
                                          (short)
                                          Enums.EstadoBeneficiarioLiquidacion.ExcluidoNoApto,
                                      IdEmpresa = c.IdEmpresa, // 04/09/2018
                                      Apoderado = new Apoderado { NumeroCuentaBco = c.Apoderado.NumeroCuentaBco, IdSucursal = c.Apoderado.IdSucursal, Apellido = c.Apoderado.Apellido ?? "", Nombre = c.Apoderado.Nombre ?? "", NumeroDocumento = (c.Apoderado.NumeroDocumento == " " ? "0" : c.Apoderado.NumeroDocumento) ?? "0", IdApoderadoNullable = c.Apoderado.IdApoderadoNullable }


                                  }).Cast<IBeneficiarioLiquidacion>().OrderBy(
                                              c =>
                                              c.IdEstadoBenConcLiq ==
                                              (short)Enums.EstadoBeneficiarioLiquidacion.ExcluidoNoApto).ThenBy(c =>
                                                                                                                 c.
                                                                                                                     IdEstadoBenConcLiq ==
                                                                                                                 (short)
                                                                                                                 Enums.
                                                                                                                     EstadoBeneficiarioLiquidacion
                                                                                                                     .
                                                                                                                     Excluido)
                    .ThenBy(
                        c => c.Ficha.Apellido).ThenBy(c => c.Ficha.Nombre).
                    ToList();



            vista.ListaBeneficiario.Select(c =>
            {
                c.Conceptos.Add(objconcepto);
                return c;
            }).ToList();


            //string path = HttpContext.Current.Server.MapPath(@"\Archivos\Importados") + @"\" + vista.Archivo;

            //var fileInfo = new FileInfo(path);

            //IExcelDataReader excelReader = null;

            //FileStream stream = File.Open(path, FileMode.Open, FileAccess.Read);

            //string file = fileInfo.Name;

            //if (file.Split('.')[1].Equals("xls"))
            //{
            //    excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
            //}
            //else if (file.Split('.')[1].Equals("xlsx"))
            //{
            //    excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            //}

            //if (excelReader != null) excelReader.IsFirstRowAsColumnNames = true;
            //var dsExcel = new DataSet();
            //if (excelReader != null)
            //{
            //    dsExcel = excelReader.AsDataSet();
            //    excelReader.Close();
            //}
            //if (dsExcel != null)
            //{
            //    if (dsExcel.Tables[0].Rows.Count > 0)
            //    {
            //        foreach (DataRow row in dsExcel.Tables[0].Rows)
            //        {
            //            vista.ListaBeneficiario.Where(c => c.IdBeneficiario == Convert.ToInt32(row["Id_Beneficiario"].ToString()) && c.IdEstadoBenConcLiq != (short)Enums.EstadoBeneficiarioLiquidacion.ExcluidoNoApto).Select(c =>
            //                                                                                                         {
            //                                                                                                             c.
            //                                                                                                                 IdEstadoBenConcLiq
            //                                                                                                                 =
            //                                                                                                                 (
            //                                                                                                                 short
            //                                                                                                                 )
            //                                                                                                                 Enums
            //                                                                                                                     .
            //                                                                                                                     EstadoBeneficiarioLiquidacion
            //                                                                                                                     .EnviadoAlBanco;
            //                                                                                                             return
            //                                                                                                                 c;
            //                                                                                                         }).ToList();
            //        }
            //    }
            //}


            vista.ListaBeneficiario.Where(c =>  c.IdEstadoBenConcLiq != 
                                                (short)Enums.EstadoBeneficiarioLiquidacion.ExcluidoNoApto)
                                                .Select(c =>
                                                            {   c.IdEstadoBenConcLiq =(short)Enums.EstadoBeneficiarioLiquidacion.EnviadoAlBanco;
                                                                   return c;
                                                            }
                                                        ).ToList();


            IList<IBeneficiarioLiquidacion> excluido = vista.ListaBeneficiario.Where(c => c.IdEstadoBenConcLiq != 1).ToList();

            IList<IBeneficiarioLiquidacion> incluidos = vista.ListaBeneficiario.Where(c => c.IdEstadoBenConcLiq == 1).ToList();


            vista.ListaBeneficiario = incluidos.Union(excluido).Cast<IBeneficiarioLiquidacion>().OrderBy(
                                              c =>
                                              c.IdEstadoBenConcLiq ==
                                              (short)Enums.EstadoBeneficiarioLiquidacion.ExcluidoNoApto).ThenBy(c =>
                                                                                                                 c.
                                                                                                                     IdEstadoBenConcLiq ==
                                                                                                                 (short)
                                                                                                                 Enums.
                                                                                                                     EstadoBeneficiarioLiquidacion
                                                                                                                     .
                                                                                                                     Excluido)
                    .ThenBy(
                        c => c.Ficha.Apellido).ThenBy(c => c.Ficha.Nombre).
                    ToList();


            if ((convenio ?? "0") != "0" && (convenio ?? "") != "")
            {
                vista.ListaBeneficiario = vista.ListaBeneficiario.Where(c => c.convenio == convenio).ToList();
            }

            vista.Conceptos.Enabled = false;
            vista.Programas.Enabled = false;
            vista.Convenios.Enabled = false;
            vista.Conceptos.Selected = objconcepto.Id_Concepto.ToString();
            vista.SeleccionoConcepto = vista.Conceptos.Selected;

            return vista;
        }

        public ILiquidacionVista VerConceptosBeneficiario(ILiquidacionVista vista, int idBeneficiario)
        {
            IComboBox combo = new ComboBox();

            IList<IConcepto> listaconceptos =
                _conceptoRepositorio.GetConceptos(null, null, "").Where(
                    c => c.Año <= DateTime.Now.Year && (c.Mes <= DateTime.Now.Month)).OrderByDescending(
                        c => c.Año).ThenByDescending(c => c.Mes).
                    ToList();

            foreach (var item in listaconceptos)
            {
                combo.Combo.Add(new ComboItem
                                    {
                                        Id = item.Id_Concepto,
                                        Description =
                                            item.Año + " - " +
                                            CultureInfo.CurrentCulture.DateTimeFormat.MonthNames[
                                                item.Mes - 1].ToUpper()
                                    });
            }

            vista.SeleccionadoBeneficiario =
                vista.ListaBeneficiario.Where(c => c.IdBeneficiario == idBeneficiario).SingleOrDefault();
            vista.SeleccionadoBeneficiario.ComboConceptos = combo;

            vista.SeleccionadoBeneficiario.ComboConceptos.Enabled = true;

            foreach (var item in _conceptoRepositorio.GetConceptosPagosBeneficiario(idBeneficiario))
            {
                IConcepto item1 = item;
                vista.SeleccionadoBeneficiario.ComboConceptos.Combo.Remove(
                    vista.SeleccionadoBeneficiario.ComboConceptos.Combo.Where(c => c.Id == item1.Id_Concepto).
                        SingleOrDefault());
            }

            vista.SeleccionadoBeneficiario.ComboConceptos.Combo.Remove(
                vista.SeleccionadoBeneficiario.ComboConceptos.Combo.Where(
                    c => c.Id == Convert.ToInt32(vista.Conceptos.Selected)).SingleOrDefault());

            foreach (var item in vista.SeleccionadoBeneficiario.Conceptos)
            {
                IConcepto item1 = item;
                vista.SeleccionadoBeneficiario.ComboConceptos.Combo.Remove(
                    vista.SeleccionadoBeneficiario.ComboConceptos.Combo.Where(c => c.Id == item1.Id_Concepto).
                        SingleOrDefault());
            }

            return vista;
        }

        public ILiquidacionVista VerMontoLiquidarBeneficiario(ILiquidacionVista vista, int idBeneficiario, int importeAliquidar)
        {
            vista.SeleccionadoBeneficiario = vista.ListaBeneficiario.Where(c => c.IdBeneficiario == idBeneficiario).SingleOrDefault();

            vista.ImportePlanEfe = importeAliquidar;

            return vista;
        }

        public ILiquidacionVista VolverFormBeneficiario(ILiquidacionVista vista)
        {
            vista.Conceptos.Enabled = false;

            return vista;
        }

        public ILiquidacionVista VolverFormBeneficiarioDesdeImporte(ILiquidacionVista vista)
        {
            vista.Conceptos.Enabled = false;
            vista.SeleccionadoBeneficiario.MontoPrograma = vista.SeleccionadoBeneficiario.MontoPrograma;

            return vista;
        }

        public ILiquidacionVista AgregarConceptoBeneficiario(ILiquidacionVista vista)
        {
            IConcepto concepto = _conceptoRepositorio.GetConcepto(Convert.ToInt32(vista.SeleccionadoBeneficiario.ComboConceptos.Selected));

            vista.SeleccionadoBeneficiario.Conceptos.Add(concepto);

            vista.SeleccionadoBeneficiario.ComboConceptos.Combo.Remove(
                vista.SeleccionadoBeneficiario.ComboConceptos.Combo.Where(
                    c => c.Id == Convert.ToInt32(vista.SeleccionadoBeneficiario.ComboConceptos.Selected)).
                    SingleOrDefault());

            return vista;
        }

        public ILiquidacionVista QuitarConceptoBeneficiario(ILiquidacionVista vista, int idBeneficiario, int idConcepto)
        {
            vista.SeleccionadoBeneficiario =
                vista.ListaBeneficiario.Where(c => c.IdBeneficiario == idBeneficiario).SingleOrDefault();

            IConcepto concepto = _conceptoRepositorio.GetConcepto(idConcepto);

            vista.SeleccionadoBeneficiario.ComboConceptos.Combo.Add(new ComboItem
                                                                        {
                                                                            Id = concepto.Id_Concepto,
                                                                            Description =
                                                                                concepto.Año + " - " +
                                                                                CultureInfo.CurrentCulture.DateTimeFormat.MonthNames[
                                                                                    (concepto.Mes) - 1].ToUpper()
                                                                        });

            vista.SeleccionadoBeneficiario.Conceptos.Remove(
                vista.SeleccionadoBeneficiario.Conceptos.Where(c => c.Id_Concepto == idConcepto).SingleOrDefault());
            vista.SeleccionadoBeneficiario.ComboConceptos.Enabled = true;

            return vista;
        }

        public string GenerarArchivo(ILiquidacionVista vista)
        {
            string mresultado = GenerarFile(vista.ListaBeneficiario, vista.FechaLiquidacion);

            if (mresultado != "")
            {
                ILiquidacion liquidacion = new Liquidacion
                                               {
                                                   IdLiquidacion = vista.IdLiquidacion,
                                                   TxtNombre =
                                                       "PSO" + ((vista.ListaBeneficiario[0].convenio == "" || vista.ListaBeneficiario[0].convenio ==null) ?
                                                       vista.ListaBeneficiario[0].NumeroConvenio.PadLeft(5, '0') : vista.ListaBeneficiario[0].convenio.PadLeft(5, '0'))
                                                        + ".hab"
                                               };

                _liquidacionRepositorio.FileGenerado(liquidacion);
            }

            return mresultado + "," + "PSO" + ((vista.ListaBeneficiario[0].convenio == "" || vista.ListaBeneficiario[0].convenio == null) ?
                                                       vista.ListaBeneficiario[0].NumeroConvenio.PadLeft(5, '0') : vista.ListaBeneficiario[0].convenio.PadLeft(5, '0')) + ".hab";
        }

        public IImportarLiquidacionVista SubirArchivo(HttpPostedFileBase archivo, int idliquidacion)
        {

            IImportarLiquidacionVista vista = new ImportarLiquidacionVista();

            if (archivo != null && archivo.ContentLength > 0)
            {

                var fileName = Path.GetFileName(archivo.FileName);
                var nombre = DateTime.Now.Year + DateTime.Now.Month.ToString() + DateTime.Now.Day +
                             DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + ".hab";

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

            vista.Liquidacion = _liquidacionRepositorio.GetLiquidacionSimple(idliquidacion);
            vista.IdLiquidacion = idliquidacion;

            return vista;
        }

        public IImportarLiquidacionVista ProcesarArchivo(IImportarLiquidacionVista model)
        {
            string path = HttpContext.Current.Server.MapPath(@"\Archivos\Importados") + @"\" + model.Archivo;

            model.Liquidacion = _liquidacionRepositorio.GetLiquidacionSimple(model.IdLiquidacion);

            ValidarArchivo(model);

            if (File.Exists(path) && model.Proceso)
            {
                using (var sr = new StreamReader(path))
                {
                    string nrodocumento = "";
                    int nrocuenta = 0;
                    int codigobanco = 0;
                    string resultado = "";

                    IList<IBeneficiario> beneficiariononliquidacion =
                        _liquidacionRepositorio.BeneficiarioOnliquidacion(model.IdLiquidacion);

                    while (sr.Peek() >= 0)
                    {
                        var linea = sr.ReadLine();

                        if (linea != null)
                        {
                            nrodocumento = linea.Substring(81, 22);
                            resultado = linea.Substring(103, 43);
                            nrocuenta = Convert.ToInt32(linea.Substring(11, 9));
                            codigobanco = Convert.ToInt32(linea.Substring(3, 5));
                        }

                        ISucursal sucursal = _sucursalRepositorio.GetSucursalByCodigoBanco(codigobanco.ToString());


                        IBeneficiario beneficariopago =
                            beneficiariononliquidacion.Where(
                                c => c.NumeroCuenta == nrocuenta && c.IdSucursal == sucursal.IdSucursal).FirstOrDefault();


                        if (beneficariopago != null)
                        {
                            Enums.EstadoBeneficiarioLiquidacion estado =
                               EstadoBeneficiarioLiquidacion(resultado.TrimEnd().TrimStart());

                            if (_liquidacionRepositorio.UpdateLiquidacionBeneficiarioConcepto(nrocuenta, model.IdLiquidacion, sucursal.IdSucursal, estado))
                            {
                                //IBeneficiario beneficiario = GetBeneficiario(beneficariopago);
                                beneficariopago.ResultadoLiquidacion = resultado.TrimEnd().TrimStart();
                                beneficariopago.NumeroCuenta = nrocuenta;
                                beneficariopago.Sucursal = sucursal.CodigoBanco + " - " + sucursal.Detalle;
                                model.ListaBeneficiarios.Add(beneficariopago);
                            }
                            else
                            {
                                //IBeneficiario beneficiario = GetBeneficiario(beneficariopago);
                                beneficariopago.ResultadoLiquidacion = "ERROR";
                                beneficariopago.NumeroCuenta = nrocuenta;
                                beneficariopago.Sucursal = sucursal.CodigoBanco + " - " + sucursal.Detalle;
                                model.ListaBeneficiarios.Add(beneficariopago);
                            }
                        }
                        else
                        {
                            IBeneficiario beneficiario = new Beneficiario
                                                             {
                                                                 ResultadoLiquidacion =
                                                                     "No se encuentra este Beneficiario en esta liquidación",
                                                                 NumeroCuenta = nrocuenta,
                                                                 Sucursal =
                                                                     sucursal.CodigoBanco + " - " + sucursal.Detalle,

                                                                 Ficha = new Ficha()
                                                             };
                            model.ListaBeneficiarios.Add(beneficiario);

                        }


                    }
                }
            }

            model.Liquidacion.IdEstadoLiquidacion = (int)Enums.EstadoLiquidacion.Liquidado;

            _liquidacionRepositorio.ChangeEstado(model.Liquidacion);

            return model;
        }

        public ILiquidacionVista SubirArchivoRepago(HttpPostedFileBase archivo, ILiquidacionVista model)
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
                        if (dsExcel.Tables[0].Columns.Contains("Id_Beneficiario"))
                        {
                            model.Importado = true;
                            model.Mensaje = "El archivo \"" + fileName + "\" fue subido al servidor correctamente. Confirme la acción de Procesar si lo cree conveniente.";
                            model.Archivo = nombre;
                        }
                        else
                        {
                            model.Importado = false;
                            model.Mensaje = "El archivo \"" + fileName + "\" fue subido al servidor correctamente. Pero el Archivo no tiene el Formato correcto.";
                            model.Archivo = null;
                            File.Delete(path);
                        }
                    }
                }
            }
            else
            {
                model.Importado = true;
                model.Mensaje = "El archivo no pudo ser subido al servidor. Vuelva a intentar.";
            }

            return model;
        }

        public IImportarLiquidacionVista GetLiquidacionImportacion(int idLiquidacion)
        {
            var vista = new ImportarLiquidacionVista
                                                  {
                                                      Liquidacion =
                                                          _liquidacionRepositorio.GetLiquidacionSimple(idLiquidacion),
                                                      IdLiquidacion = idLiquidacion
                                                  };

            return vista;

        }

        public byte[] ExportLiquidacion(ILiquidacionVista model)
        {
            string path = HttpContext.Current.Server.MapPath(@"\Archivos\Exportados\TemplateLiquidacion.xls");

            var fs =
                new FileStream(path, FileMode.Open, FileAccess.Read);

            var templateWorkbook = new HSSFWorkbook(fs, true);

            var sheet = templateWorkbook.GetSheet("Liquidacion");
            int idconcepto = Convert.ToInt32(model.Conceptos.Selected);
            string periodo = model.Conceptos.Combo.Where(c => c.Id == idconcepto).Select(c => c.Description).SingleOrDefault();
            string tipoPrograma = "";
            int i = 0;

            var tiposppp = _tipopppRepositorio.GetTiposPpp();

            foreach (var beneficiario in model.ListaBeneficiario)
            {
                i++;

                var row = sheet.CreateRow(i);
                var celIdFicha = row.CreateCell(0);
                celIdFicha.SetCellValue(beneficiario.Ficha.IdFicha.ToString());

                var celapellido = row.CreateCell(1);
                celapellido.SetCellValue(beneficiario.Ficha.Apellido);

                var celnombre = row.CreateCell(2);
                celnombre.SetCellValue(beneficiario.Ficha.Nombre);

                var celdni = row.CreateCell(3);
                celdni.SetCellValue(beneficiario.Ficha.NumeroDocumento);

                var celapellidoapo = row.CreateCell(4);
                celapellidoapo.SetCellValue(beneficiario.Apoderado.Apellido);

                var celnombreapo = row.CreateCell(5);
                celnombreapo.SetCellValue(beneficiario.Apoderado.Nombre);

                var celdniapo = row.CreateCell(6);
                celdniapo.SetCellValue(beneficiario.Apoderado.NumeroDocumento);

                var celcuenta = row.CreateCell(7);
                celcuenta.SetCellValue(beneficiario.NroCuentadePago);

                var celsucursale = row.CreateCell(8);
                celsucursale.SetCellValue( beneficiario.Sucursal);

                var celapoderado = row.CreateCell(9);
                celapoderado.SetCellValue(beneficiario.PagoaApoderado);

                var celfechapago = row.CreateCell(10);
                celfechapago.SetCellValue(model.FechaLiquidacion);

                var celestado = row.CreateCell(11);
                celestado.SetCellValue(beneficiario.EstadoBeneficiarioConcepto);

                var celImporte = row.CreateCell(12);
                //celImporte.SetCellValue(beneficiario.MontoPrograma == null ? "0" : beneficiario.MontoPrograma.ToString());
                celImporte.SetCellValue(beneficiario.MontoLiquidado == null ? "0" : beneficiario.MontoLiquidado.ToString());
                var celPeriodo = row.CreateCell(13);
                celPeriodo.SetCellValue(periodo == null ? "" : periodo);
                var celIdSubprograma = row.CreateCell(14);
                celIdSubprograma.SetCellValue(beneficiario.idSubprograma == null ? "" : beneficiario.idSubprograma.ToString());
                var celSubprograma = row.CreateCell(15);
                celSubprograma.SetCellValue(beneficiario.subprograma);
                var celTipoPrograma = row.CreateCell(16);

                foreach (var tipo in tiposppp)
                {
                    if (tipo.ID_TIPO_PPP == (beneficiario.TipoPrograma ?? 1))
                    {
                        tipoPrograma = tipo.N_TIPO_PPP;
                    }

                }

                //switch (beneficiario.TipoPrograma)
                //{ 
                //    case 1:
                //        tipoPrograma = "PPP";
                //        break;
                //    case 2:
                //        tipoPrograma = "PPP APRENDIZ";
                //        break;
                //    case 3:
                //        tipoPrograma = "XMÍ";
                //        break;
                //    case 4:
                //        tipoPrograma = "PILA";
                //        break;
                //    case 5:
                //        tipoPrograma = "PIP";
                //        break;
                //    case 6:
                //        tipoPrograma = "PROGRAMA CLIP";
                //        break;
                //    case 7:
                //        tipoPrograma = "PIL - COMERCIO EXTERIOR";
                //        break;
                //    case 8:
                //        tipoPrograma = "PIL - COMERCIO ELECTRONICO";
                //        break;
                //    case 9:
                //        tipoPrograma = "PIL - MAQUINARIA AGRICOLA";
                //        break;
                //    case 10:
                //        tipoPrograma = "PIL - TURISMO";
                //        break;
                //    default:
                //        tipoPrograma = "";
                //        break;
                
                //}
                celTipoPrograma.SetCellValue(tipoPrograma);
                var celIdEmpresa = row.CreateCell(17);
                celIdEmpresa.SetCellValue(beneficiario.IdEmpresa == null ? "" : beneficiario.IdEmpresa.ToString());
            }

            var ms = new MemoryStream();

            templateWorkbook.Write(ms);

            return ms.ToArray();
        }

        private static string TipoDniBco(int tipo)
        {
            string mReturn;

            switch (tipo)
            {
                case 0:
                    mReturn = "DNI";
                    break;
                case 1:
                    mReturn = "LE";
                    break;
                case 2:
                    mReturn = "LC";
                    break;
                case 3:
                    mReturn = "CI";
                    break;
                case 4:
                    mReturn = "Nunca tuvo";
                    break;
                case 5:
                    mReturn = "Cedula Extranjera";
                    break;
                default:
                    mReturn = "1";
                    break;
            }

            return mReturn;
        }

        private static PdfPTable GenerarTablaTotalesReporteAnexo1(ILiquidacionVista liquidacion, int programa)
        {
            var tablaTotales = new PdfPTable(4);
            tablaTotales.SetWidthPercentage(new float[] { 70, 80, 100, 60 }, PageSize.LEGAL);
            PdfPCell celdaUno;
            PdfPCell celdaDos;
            PdfPCell celdaTres;
            PdfPCell celdaCuatro;
            decimal totalPagadoEfectores = 0;

            if (programa == (int)Enums.Programas.EfectoresSociales)
            {
                foreach (var beneficiario in (liquidacion.ListaBeneficiariosAnexo))
                {
                    decimal montobenef = _conceprepo.GetConceptosByBeneficiarioEfectores(beneficiario.IdBeneficiario, liquidacion.IdLiquidacion).MontoPagado;

                    totalPagadoEfectores = totalPagadoEfectores + montobenef;
                }

                int totalBenef = liquidacion.ListaBeneficiariosAnexo.Count;


                celdaUno = new PdfPCell(new Paragraph("TOTAL ANEXO I",
                                               FontFactory.GetFont("Arial, Helvetica, sans-serif", 10, FontPdf.BOLD)));
                celdaDos = new PdfPCell(new Paragraph(totalBenef.ToString(),
                                                  FontFactory.GetFont("Arial, Helvetica, sans-serif", 10, FontPdf.BOLD)));
                celdaTres = new PdfPCell(new Paragraph("Importe Variable",
                                                   FontFactory.GetFont("Arial, Helvetica, sans-serif", 10, FontPdf.BOLD)));
                celdaCuatro = new PdfPCell(new Paragraph("$ " + String.Format("{0:n}", totalPagadoEfectores),
                                                   FontFactory.GetFont("Arial, Helvetica, sans-serif", 10, FontPdf.BOLD)));
            }
            else
            {
                int totalBenef = liquidacion.ListaBeneficiariosAnexo.Count;
                decimal importePlan = (liquidacion.ListaBeneficiariosAnexo).Take(1).Select(li => li.ImportePagoPlan).SingleOrDefault();
                decimal totalPagado = totalBenef * importePlan;

                celdaUno = new PdfPCell(new Paragraph("TOTAL ANEXO I",
                                               FontFactory.GetFont("Arial, Helvetica, sans-serif", 10, FontPdf.BOLD)));
                celdaDos = new PdfPCell(new Paragraph(totalBenef.ToString(),
                                                  FontFactory.GetFont("Arial, Helvetica, sans-serif", 10, FontPdf.BOLD)));
                celdaTres = new PdfPCell(new Paragraph("$ " + String.Format("{0:n}", importePlan),
                                                   FontFactory.GetFont("Arial, Helvetica, sans-serif", 10, FontPdf.BOLD)));
                celdaCuatro = new PdfPCell(new Paragraph("$ " + String.Format("{0:n}", totalPagado),
                                                   FontFactory.GetFont("Arial, Helvetica, sans-serif", 10, FontPdf.BOLD)));
            }

            tablaTotales.AddCell(celdaUno);
            tablaTotales.AddCell(celdaDos);
            tablaTotales.AddCell(celdaTres);
            tablaTotales.AddCell(celdaCuatro);

            //tablaTotales.DefaultCell.Border = Rectangle.LEFT_BORDER;
            tablaTotales.HorizontalAlignment = PdfContentByte.ALIGN_CENTER;

            foreach (PdfPCell celda in tablaTotales.Rows[0].GetCells())
            {
                //celda.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER;
                celda.HorizontalAlignment = PdfContentByte.ALIGN_RIGHT;
            }
            celdaUno.HorizontalAlignment = PdfContentByte.ALIGN_LEFT;

            return tablaTotales;
        }

        public static PdfPTable GenerarTablaReporteAnexo1(ILiquidacionVista model)
        {
            var unaTabla = new PdfPTable(14);

            //le doy el ancho a cada columna
            unaTabla.SetWidthPercentage(new float[] { 30, 50, 90, 30, 40, 30, 20, 30, 50, 50, 80, 40, 30, 30 }, PageSize.LEGAL);

            //Headers
            unaTabla.AddCell(new Paragraph("ORDEN", FontFactory.GetFont("Arial, Helvetica, sans-serif", 8, FontPdf.BOLD, Color.WHITE)));
            unaTabla.AddCell(new Paragraph("APELLIDO", FontFactory.GetFont("Arial, Helvetica, sans-serif", 8, FontPdf.BOLD, Color.WHITE)));
            unaTabla.AddCell(new Paragraph("NOMBRE", FontFactory.GetFont("Arial, Helvetica, sans-serif", 8, FontPdf.BOLD, Color.WHITE)));
            unaTabla.AddCell(new Paragraph("TIPO DOC", FontFactory.GetFont("Arial, Helvetica, sans-serif", 8, FontPdf.BOLD, Color.WHITE)));
            unaTabla.AddCell(new Paragraph("DOCUMENTO", FontFactory.GetFont("Arial, Helvetica, sans-serif", 8, FontPdf.BOLD, Color.WHITE)));
            unaTabla.AddCell(new Paragraph("PERIODO", FontFactory.GetFont("Arial, Helvetica, sans-serif", 8, FontPdf.BOLD, Color.WHITE)));
            unaTabla.AddCell(new Paragraph("SUC", FontFactory.GetFont("Arial, Helvetica, sans-serif", 8, FontPdf.BOLD, Color.WHITE)));
            unaTabla.AddCell(new Paragraph("CUENTA", FontFactory.GetFont("Arial, Helvetica, sans-serif", 8, FontPdf.BOLD, Color.WHITE)));
            unaTabla.AddCell(new Paragraph("DESC SUCURSAL", FontFactory.GetFont("Arial, Helvetica, sans-serif", 8, FontPdf.BOLD, Color.WHITE)));
            unaTabla.AddCell(new Paragraph("APELLIDO APO", FontFactory.GetFont("Arial, Helvetica, sans-serif", 8, FontPdf.BOLD, Color.WHITE)));
            unaTabla.AddCell(new Paragraph("NOMBRE APO", FontFactory.GetFont("Arial, Helvetica, sans-serif", 8, FontPdf.BOLD, Color.WHITE)));
            unaTabla.AddCell(new Paragraph("T_DOC APO", FontFactory.GetFont("Arial, Helvetica, sans-serif", 8, FontPdf.BOLD, Color.WHITE)));
            unaTabla.AddCell(new Paragraph("DOC APO", FontFactory.GetFont("Arial, Helvetica, sans-serif", 8, FontPdf.BOLD, Color.WHITE)));
            unaTabla.AddCell(new Paragraph("IMPORTE", FontFactory.GetFont("Arial, Helvetica, sans-serif", 8, FontPdf.BOLD, Color.WHITE)));

            //indica que tiene una sola fila como encabezado
            unaTabla.HeaderRows = 1;

            //formato A LA CABECERA
            foreach (PdfPCell celda in unaTabla.Rows[0].GetCells())
            {
                celda.BackgroundColor = new Color(106, 173, 218);
                celda.HorizontalAlignment = 1;
            }

            ILiquidacion objliquidacionperiodo = _liqrepo.GetLiquidacion(model.IdLiquidacion);

            int año = (objliquidacionperiodo.Beneficiarios).Take(1).Select(
               li => new Concepto { Año = li.ConceptosBeneficiario.Select(co => co.Año).SingleOrDefault() }).
               SingleOrDefault().Año;

            int mes = (objliquidacionperiodo.Beneficiarios).Take(1).Select(
                li => new Concepto { Mes = li.ConceptosBeneficiario.Select(co => co.Mes).SingleOrDefault() }).
                SingleOrDefault().Mes;

            string periodo = mes + "-" + año;

            int orden = 1;
            PdfPCell celdaImporte;

            //recorro la lista de datos 
            foreach (var beneficiario in (model.ListaBeneficiariosAnexo))
            {
                //creo una celda para cada columna
                var celdaOrden =
                    new PdfPCell(new Paragraph(orden.ToString(),
                                               FontFactory.GetFont("Arial, Helvetica, sans-serif", 8)));
                var celdaApellido =
                    new PdfPCell(new Paragraph(beneficiario.Ficha.Apellido == null ? " " : beneficiario.Ficha.Apellido.ToUpper(),
                                               FontFactory.GetFont("Arial, Helvetica, sans-serif", 8)));
                var celdaNombre =
                   new PdfPCell(new Paragraph(beneficiario.Ficha.Nombre == null ? " " : beneficiario.Ficha.Nombre.ToUpper(),
                                              FontFactory.GetFont("Arial, Helvetica, sans-serif", 8)));
                var celdaTipoDoc =
                   new PdfPCell(new Paragraph(TipoDniBco(beneficiario.Ficha.TipoDocumento.GetValueOrDefault()),
                                              FontFactory.GetFont("Arial, Helvetica, sans-serif", 8)));
                var celdaNumeroDocumento =
                    new PdfPCell(new Paragraph(beneficiario.Ficha.NumeroDocumento,
                                               FontFactory.GetFont("Arial, Helvetica, sans-serif", 8)));
                var celdaPeriodo =
                   new PdfPCell(new Paragraph(periodo.ToUpper() ?? "",
                                              FontFactory.GetFont("Arial, Helvetica, sans-serif", 8)));
                var celdaSucursal =
                    new PdfPCell(new Paragraph(beneficiario.CodigoSucursal ?? " ",
                                              FontFactory.GetFont("Arial, Helvetica, sans-serif", 8)));
                var celdaCuenta =
                   new PdfPCell(new Paragraph(beneficiario.NumeroCuenta.ToString(),
                                              FontFactory.GetFont("Arial, Helvetica, sans-serif", 8)));
                var celdaDesSuc =
                    new PdfPCell(new Paragraph(beneficiario.SucursalDescripcion == null ? " " : beneficiario.SucursalDescripcion.ToUpper() ?? "",
                                               FontFactory.GetFont("Arial, Helvetica, sans-serif", 8)));
                var celdaApoApellido =
                    new PdfPCell(new Paragraph(beneficiario.Apoderado.Apellido == null ? " " : beneficiario.Apoderado.Apellido.ToUpper(),
                                               FontFactory.GetFont("Arial, Helvetica, sans-serif", 8)));

                var celdaApoNombre =
                  new PdfPCell(new Paragraph(beneficiario.Apoderado.Nombre == null ? " " : beneficiario.Apoderado.Nombre.ToUpper(),
                                             FontFactory.GetFont("Arial, Helvetica, sans-serif", 8)));

                var celdaApoTipoDoc =
                    new PdfPCell(new Paragraph(beneficiario.Apoderado.NumeroDocumento == null ? " " : TipoDniBco(beneficiario.Apoderado.TipoDocumento.GetValueOrDefault()),
                                             FontFactory.GetFont("Arial, Helvetica, sans-serif", 8)));

                var celdaApoNroDoc =
                  new PdfPCell(new Paragraph(beneficiario.Apoderado.NumeroDocumento,
                                             FontFactory.GetFont("Arial, Helvetica, sans-serif", 8)));

                if (model.ListaBeneficiariosAnexo.FirstOrDefault().IdPrograma == (int)Enums.Programas.EfectoresSociales)
                {
                    decimal montoModificado = _conceprepo.GetConceptosByBeneficiarioEfectores(beneficiario.IdBeneficiario, model.IdLiquidacion).MontoPagado;
                    celdaImporte = new PdfPCell(new Paragraph("$ " + String.Format("{0:n}", montoModificado), FontFactory.GetFont("Arial, Helvetica, sans-serif", 8)));
                }
                else
                {
                    celdaImporte =
                    new PdfPCell(new Paragraph("$ " + String.Format("{0:n}", objliquidacionperiodo.Beneficiarios.FirstOrDefault().ImportePagoPlan),//beneficiario.ImportePagoPlan),
                                               FontFactory.GetFont("Arial, Helvetica, sans-serif", 8)));
                }


                celdaOrden.HorizontalAlignment = 2;
                celdaNumeroDocumento.HorizontalAlignment = 2;
                celdaPeriodo.HorizontalAlignment = 2;
                celdaSucursal.HorizontalAlignment = 2;
                celdaApoNroDoc.HorizontalAlignment = 2;
                celdaImporte.HorizontalAlignment = 2;

                //agrego a la tabla las columnas (celdas)
                unaTabla.AddCell(celdaOrden);
                unaTabla.AddCell(celdaApellido);
                unaTabla.AddCell(celdaNombre);
                unaTabla.AddCell(celdaTipoDoc);
                unaTabla.AddCell(celdaNumeroDocumento);
                unaTabla.AddCell(celdaPeriodo);
                unaTabla.AddCell(celdaSucursal);
                unaTabla.AddCell(celdaCuenta);
                unaTabla.AddCell(celdaDesSuc);
                unaTabla.AddCell(celdaApoApellido);
                unaTabla.AddCell(celdaApoNombre);
                unaTabla.AddCell(celdaApoTipoDoc);
                unaTabla.AddCell(celdaApoNroDoc);
                unaTabla.AddCell(celdaImporte);

                orden++;
            }

            return unaTabla;
        }

        //PARA ANEXO I
        public static byte[] AddPageNumbersApaizadosWithImage(byte[] pdf)
        {
            var ms = new MemoryStream();
            ms.Write(pdf, 0, pdf.Length);
            var reader = new PdfReader(pdf);
            int n = reader.NumberOfPages;
            Rectangle psize = reader.GetPageSize(1);
            var document = new Document(psize, 40, 40, 25, 70);//70
            PdfWriter writer = PdfWriter.GetInstance(document, ms);

            //ubicacion de la imagen
            string path = HttpContext.Current.Server.MapPath(@"\Content\images\sello.png");//Logo Agencia.jpg
            Image imagenSello = Image.GetInstance(path);

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

                //imagen-alto de la imagen- 0f-0f-ancho imagen- posicionx:lo separa del costado- posicion y: lo separa del piso
                //cb.AddImage(imagenSello,20f,0f,0f, 40f,540f,70f);

                if (page == 1)
                {
                    cb.AddImage(imagenSello, 80, 0f, 0f, 140, 495f, 70f);//545
                }
                else
                {
                    cb.AddImage(imagenSello, 80, 0f, 0f, 140, 500f, 70f);//550
                }
                //Pie de pagina:posicion x: 590, posicion y: 605, rotacion :90 (a 90grados de la pag vertical)505
                cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "Página " + p + " de " + n, 590, 525, 90);
                cb.EndText();
            }

            document.Close();
            return ms.ToArray();
        }

        private static IImportarLiquidacionVista ValidarArchivo(IImportarLiquidacionVista model)
        {
            string path = HttpContext.Current.Server.MapPath(@"\Archivos\Importados") + @"\" + model.Archivo;

            if (File.Exists(path))
            {
                using (var sr = new StreamReader(path))
                {
                    var arle = new StreamReader(path);
                    var textototal = sr.ReadToEnd();
                    var re = new Regex("\n", RegexOptions.Multiline);
                    MatchCollection lineas = re.Matches(textototal);
                    int cantlineas = lineas.Count;
                    string numeroconvenio = "";
                    string fechaliquidacion = "";

                    while (arle.Peek() >= 0)
                    {
                        var linea = arle.ReadLine();
                        if (linea != null)
                        {
                            fechaliquidacion = linea.Substring(44, 2) + "/" + linea.Substring(42, 2) + "/" + linea.Substring(38, 4);
                            numeroconvenio = linea.Substring(46, 5);
                        }
                        break;
                    }

                    if (cantlineas != model.Liquidacion.CantBenef)
                    {
                        model.Proceso = false;
                        model.Mensaje =
                            "El número de Liquidaciones en el archivo supera al de la Liquidación seleccionada.";

                        return model;
                    }

                    if (fechaliquidacion != model.Liquidacion.FechaLiquidacion.ToShortDateString())
                    {
                        model.Proceso = false;
                        model.Mensaje =
                            "La fecha de Liquidación del archivo no es la misma que la Liquidación seleccionada.";

                        return model;
                    }

                    if (numeroconvenio != model.Liquidacion.Convenio.PadLeft(5, '0'))
                    {
                        model.Proceso = false;
                        model.Mensaje =
                            "El convenio que figura en el archivo no es la misma que la Liquidación seleccionada.";

                        return model;
                    }
                }
            }
            model.Proceso = true;

            return model;
        }

        private static Enums.EstadoBeneficiarioLiquidacion EstadoBeneficiarioLiquidacion(string estado)
        {
            switch (estado)
            {
                case "IMPUTADO":
                    return Enums.EstadoBeneficiarioLiquidacion.ImputadoEnCuenta;
                case "CINCUENTA INEXISTENTE":
                    return Enums.EstadoBeneficiarioLiquidacion.CuentaInexistente;
                case "CBLCUENTA BLOQUEADA":
                    return Enums.EstadoBeneficiarioLiquidacion.CuentaBloqueada;
                case "CCECUENTA CERRADA":
                    return Enums.EstadoBeneficiarioLiquidacion.CuentaCerrada;
                case "CATCUENTA ACTUALIZANDOSE":
                    return Enums.EstadoBeneficiarioLiquidacion.CatcuentaActualizandose;
                case "CA0CUENTA DE ACREDITACION EN CERO":
                    return Enums.EstadoBeneficiarioLiquidacion.Ca0CuentaDeAcreditacionEnCero;
                case "CA1CUENTA DE ACREDITACION INEXISTENTE":
                    return Enums.EstadoBeneficiarioLiquidacion.Ca1CuentaDeAcreditacionInexistente;
                case "CA2CUENTA DE ACREDITACION CON ERROR":
                    return Enums.EstadoBeneficiarioLiquidacion.Ca2CuentaDeAcreditacionConError;
                case "CBDCUENTA CON BLOQUEO DE DÉBITOS":
                    return Enums.EstadoBeneficiarioLiquidacion.CbdcuentaConBloqueoDeDebitos;
                case "CBTCUENTA CON BLOQUEO TOTAL":
                    return Enums.EstadoBeneficiarioLiquidacion.CbtcuentaConBloqueoTotal;
                case "CCCCUENTA CERRADA POR RECHAZO DE CHEQUE":
                    return Enums.EstadoBeneficiarioLiquidacion.CcccuentaCerradaPorRechazoDeCheque;
                case "CCJCUENTA CON HIJAS":
                    return Enums.EstadoBeneficiarioLiquidacion.CcjcuentaConHijas;
                case "CCMCUENTA CON MOVIMIENTOS":
                    return Enums.EstadoBeneficiarioLiquidacion.CcmcuentaConMovimientos;
                case "CCSCUENTA CON SALDO":
                    return Enums.EstadoBeneficiarioLiquidacion.CcscuentaConSaldo;
                case "CECCTA.EXISTE REL. CLI.":
                    return Enums.EstadoBeneficiarioLiquidacion.CecctaExisteRelCli;
                case "CFICUENTA CON FONDO INSUFICIENTE":
                    return Enums.EstadoBeneficiarioLiquidacion.CficuentaConFondoInsuficiente;
                case "CIACUENTA INACTIVA":
                    return Enums.EstadoBeneficiarioLiquidacion.CiacuentaInactiva;
                case "CIHCUENTA INHABILITADA":
                    return Enums.EstadoBeneficiarioLiquidacion.CihcuentaInhabilitada;
                case "CILCUENTA LIBRADORA INEXISTENTE":
                    return Enums.EstadoBeneficiarioLiquidacion.CilcuentaLibradoraInexistente;
                case "CIZCUENTA INMOVILIZADA":
                    return Enums.EstadoBeneficiarioLiquidacion.CizcuentaInmovilizada;
                case "CI1CUENTA INCOMPLETA":
                    return Enums.EstadoBeneficiarioLiquidacion.Ci1CuentaIncompleta;
                case "CMECUENTA CON MOV. EN ACTUAL EJERCICIO":
                    return Enums.EstadoBeneficiarioLiquidacion.CmecuentaConMovEnActualEjercicio;
                case "CNECUENTA NO PUEDE REALIZAR ENTREGA":
                    return Enums.EstadoBeneficiarioLiquidacion.CnecuentaNoPuedeRealizarEntrega;
                case "CNOCUENTA NO OPERATIVA":
                    return Enums.EstadoBeneficiarioLiquidacion.CnocuentaNoOperativa;
                case "CSCCUENTA CON SUSPENSIÓN PAGO DE CHEQUE":
                    return Enums.EstadoBeneficiarioLiquidacion.CsccuentaConSuspensionPagoDeCheque;
                case "CSECUENTA SUPERIOR INEXISTENTE":
                    return Enums.EstadoBeneficiarioLiquidacion.CsecuentaSuperiorInexistente;
                case "CT0CUENTA EN CERO":
                    return Enums.EstadoBeneficiarioLiquidacion.Ct0CuentaEnCero;
                case "CT1NRO. CUENTA INVÁLIDO":
                    return Enums.EstadoBeneficiarioLiquidacion.Ct1NroCuentaInvalido;
                case "CUECUENTA EMBARGADA":
                    return Enums.EstadoBeneficiarioLiquidacion.CuecuentaEmbargada;
                case "CXPCUENTA NO ES DE PESOS":
                    return Enums.EstadoBeneficiarioLiquidacion.CxpcuentaNoEsDePesos;
                case "INTCUENTA SIN INTERESES":
                    return Enums.EstadoBeneficiarioLiquidacion.IntcuentaSinIntereses;
                case "MANMOVIMIENTO ANULADO":
                    return Enums.EstadoBeneficiarioLiquidacion.ManmovimientoAnulado;
                case "MCPCUENTA PRINCIPAL NO ES EN PESOS":
                    return Enums.EstadoBeneficiarioLiquidacion.McpcuentaPrincipalNoEsEnPesos;
                case "MDCMONEDA DISTINTA DE LA CUENTA":
                    return Enums.EstadoBeneficiarioLiquidacion.MdcmonedaDistintaDeLaCuenta;
                case "MDEMOVIMIENTO DEVUELTO":
                    return Enums.EstadoBeneficiarioLiquidacion.MdemovimientoDevuelto;
                case "MEIMOV.CON ESTADO INCORRECTO P/OPERACION":
                    return Enums.EstadoBeneficiarioLiquidacion.MeimovConEstadoIncorrectoPOperacion;
                case "MIVMONEDA INVALIDA":
                    return Enums.EstadoBeneficiarioLiquidacion.MivmonedaInvalida;
                case "MOIMONEDA INEXISTENTE":
                    return Enums.EstadoBeneficiarioLiquidacion.MoimonedaInexistente;
                case "NAINÚMERO DE ACUERDO INVÁLIDO":
                    return Enums.EstadoBeneficiarioLiquidacion.NainumeroDeAcuerdoInvalido;
                case "SAISUCURSAL DE ACREDITACION INVALIDA":
                    return Enums.EstadoBeneficiarioLiquidacion.SaisucursalDeAcreditacionInvalida;
                default:
                    return Enums.EstadoBeneficiarioLiquidacion.NoImputado;
            }
        }

        //Obtengo el Beneficiario ya se que recibe el DNI del mismo o se puede poner el DNI del apoderado.
        private IBeneficiario GetBeneficiario(IBeneficiario beneficiariopago)
        {

            if (beneficiariopago.PagoaApoderado == "N")
            {
                IBeneficiario beneficiario =
                    _beneficiarioRepositorio.GetBeneficiario(
                        beneficiariopago.IdBeneficiario);

                return beneficiario;

            }
            else
            {
                IApoderado apoderado =
                    _apoderadoRepositorio.GetApoderado(beneficiariopago.IdApoderadoPago ?? 0);


                IBeneficiario beneficiario = new Beneficiario
                                                 {
                                                     Ficha =
                                                         new Ficha
                                                             {
                                                                 Nombre = apoderado != null ? apoderado.Nombre : "",
                                                                 Apellido = apoderado != null ? apoderado.Apellido : "",
                                                                 NumeroDocumento =
                                                                     apoderado != null ? apoderado.NumeroDocumento : ""
                                                             }
                                                 };



                return beneficiario;

            }

            //if (beneficiario != null)
            //{
            //    return beneficiario;
            //}

            //IApoderado apoderado =
            //    _apoderadoRepositorio.GetApoderadosByNroCuentaAndSucursalActivo(nrocuenta, sucursal);

            //if (apoderado != null)
            //{
            //    return
            //        _beneficiarioRepositorio.GetBeneficiario(apoderado.IdBeneficiario ?? 0);

            //}

            //return new Beneficiario
            //           {
            //               Ficha = new Ficha { Numero_Documento = nrodocumento },
            //               ResultadoLiquidacion = "No se encontró el Nro. de Documento en la Liquidación."
            //           };
        }



        private void CargarConvenio(ILiquidacionVista vista, IEnumerable<IConvenio> listaConvenios)
        {

            vista.Convenios.Combo.Add(new ComboItem
            {
                Id = 0,
                Description = "TODOS"

            });

            foreach (var item in listaConvenios)
            {


                vista.Convenios.Combo.Add(new ComboItem
                    {
                        Id = item.IdConvenio,
                        Description = item.NConvenio

                    });

            }
        }



    }
}
